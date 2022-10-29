using Attendance.Core.Security;
using Attendance.Models;
using Attendance.Models.Entities;
using Attendance.Web.Services.Base;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace Attendance.Web.Controllers.api
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private UnitOfWork unitOfWork;
        private Repository<User> _user;
        private Repository<Card> _card;
        private Repository<CardLoginHistory> _cardLoginHistory;
        public AuthController()
        {
            unitOfWork = new UnitOfWork();
            _user = unitOfWork.Repository<User>();
            _cardLoginHistory = unitOfWork.Repository<CardLoginHistory>();
            _card = unitOfWork.Repository<Card>();
        }
        private DatabaseContext db = new DatabaseContext();
        // GET /api/authors/1/books
        [Route("~/api/check/{id}/auth")]
        [HttpGet]
        public IHttpActionResult Authenticate(string id)
        {
            string operatorId = "";
            var today = System.DateTime.Now.ToString("dddd");
            if (Request.Headers.Contains("Key"))
            {
                operatorId = Request.Headers.GetValues("Key").First();
            }
            //یافتن کارت براساس کد و وجود راننده
            var card = _card.Get(x => x.Code == id && !x.Driver.IsDeleted&& !x.Driver.ShiftDelete, "Driver,CardLoginHistories").FirstOrDefault();

            List<ToastrViewModel> toastrList = new List<ToastrViewModel>();

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            hubContext.Clients.All.Generate(id,operatorId);
            if (card != null)
            {
                //Inquiry 
                hubContext.Clients.All.Inquiry(card.Id, card.Driver.FirstName + " " + card.Driver.LastName, null, $"با کد {card.DisplayCode} اجازه ورود دارد", card.Code, operatorId);


                if (!card.IsActive)
                {
                    var message = $"{card.Driver.FullName} با شماره کارت {card.DisplayCode} به دلیل '{card.Description}' مجاز به تردد نمی باشد. با مدیر سیستم در ارتباط باشید.";
                    hubContext.Clients.All.Alarm(null, message, operatorId);
                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = message } },
                        Ok = false
                    });
                }
                if (!card.Driver.IsActive)
                {
                    // چنانچه فردی را در قسمت رانندگان غیر فعال کردیم علت درج شود و چنانچه کارت در قسمت کنترل ورود و خروج ثبت شد سیستم اخطار دهد که این فرد به دلیلی که قید شده مجاز به تردد نمی باشد و فرد راننده باید تغییر کند یا از حالت غیر فعال خارج شود
                    var message = $"{card.Driver.FullName} به دلیل '{card.Driver.Description}' مجاز به تردد نمی باشد. با مدیر سیستم در ارتباط باشید.";
                    hubContext.Clients.All.Alarm(null, message, operatorId);
                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = message } },
                        Ok = false
                    });
                }

                var logined = _cardLoginHistory.Get(c => c.CardId == card.Id && !c.ExitDate.HasValue).FirstOrDefault();
                if (logined != null)
                {
                    var message = $"درخواست خروج";
                    
                    hubContext.Clients.All.Exit(logined.Id, message, operatorId);


                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = message } },
                        Ok = false
                    });
                }
                if (card.Day.ToString() == today)
                {
                    if (card.Driver.BirthDate.HasValue && (DateTime.Now - card.Driver.BirthDate.Value).TotalDays < (365 * 18))
                    {
                        var message = $"سن غیرمجاز، سن صاحب کارت زیر 18 سال است. سن صاحب کارت ({Convert.ToInt32((DateTime.Now - card.Driver.BirthDate.Value).TotalDays / 365)})";
                        hubContext.Clients.All.Alarm(null, message, operatorId);
                        return Ok(new CustomResponseViewModel()
                        {
                            Extra = "",
                            Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = message } },
                            Ok = false
                        });
                    }

                    if (card.CardLoginHistories.Any(x => x.LoginDate.Date == DateTime.Now.Date))
                    {
                        var message = $"این کارت در امروز یکبار تردد داشته است";

                        hubContext.Clients.All.Alarm(null, message, operatorId);
                        return Ok(new CustomResponseViewModel()
                        {
                            Extra = "",
                            Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = message } },
                            Ok = false
                        });
                    }


                    hubContext.Clients.All.addNewMessageToPage(card.Id, card.Driver.FirstName + " " + card.Driver.LastName,
                        null, $"با کد {card.DisplayCode} اجازه ورود دارد", card.Code, operatorId);
                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"موفق" } },
                        Ok = true
                    });
                }

                hubContext.Clients.All.addNewMessageToPage(null, null, null, $"کارت اجازه ورود ندارد",null, operatorId);
                return Ok(new CustomResponseViewModel()
                {
                    Extra = "",
                    Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"کارت اجازه ورود ندارد" } },
                    Ok = true
                });
            }
            hubContext.Clients.All.addNewMessageToPage(null, null, null, $"کارت معتبر نیست",null, operatorId);
            return Ok(new CustomResponseViewModel()
            {
                Extra = "",
                Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"کارت معتبر نیست" } },
                Ok = true
            });
        }

        [Route("~/api/operator/{username}/{password}/auth")]
        [HttpGet]
        public IHttpActionResult Operator(string username, string password)
        {
            //password = Encryptor.Decrypt(password.Replace("|", "/"), "zavosh110");
            var _op = _user.Get(x => x.CellNum == username && x.Password == password).FirstOrDefault();

            if (_op != null)
            {
                return Ok(new CustomResponseViewModel()
                {
                    Extra = _op.Id.ToString(),
                    Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = "" } },
                    Ok = true
                });
            }
            return Ok(new CustomResponseViewModel()
            {
                Extra = "",
                Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = "" } },
                Ok = false
            });
        }

        // GET /api/authors/1/books
        [Route("~/api/Exit/{id}/auth")]
        [HttpGet]
        public IHttpActionResult Exit(string id)
        {
            var card = db.Cards.Include("Driver").FirstOrDefault(s => s.Code == id && s.IsActive);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            if (card != null)
            {
                var login = card.CardLoginHistories.OrderBy(x => x.CreationDate).LastOrDefault(x => !x.ExitDate.HasValue);
                if (login != null)
                {
                    login.ExitDate = DateTime.Now;
                    db.SaveChanges();
                    hubContext.Clients.All.Exit(login.Id, $"خروج با موفقیت ثبت شد");
                }
                else
                {
                    hubContext.Clients.All.Exit(null, $"ورود این کاربر ثبت نشده است");
                }
                return Ok(new CustomResponseViewModel()
                {
                    Extra = "",
                    Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"خروج با موفقیت ثبت شد" } },
                    Ok = true
                });
            }
            hubContext.Clients.All.addNewMessageToPage(null, $"کارت معتبر نیست");
            return Ok(new CustomResponseViewModel()
            {
                Extra = "",
                Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"کارت معتبر نیست" } },
                Ok = true
            });
        }

        [Route("~/api/Generate/{id}/auth")]
        [HttpGet]
        public IHttpActionResult Genrate(string id)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            hubContext.Clients.All.Generate(id);
            return Ok(
                new CustomResponseViewModel()
                {
                    Extra = "",
                    Messages = null,
                    Ok = true
                }
                );
        }
    }
}
