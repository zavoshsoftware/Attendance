using Attendance.Models;
using Attendance.Models.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http; 
namespace Attendance.Web.Controllers.api
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        public AuthController()
        {

        }
        private DatabaseContext db = new DatabaseContext();
        // GET /api/authors/1/books
        [Route("~/api/check/{id}/auth")]
        [HttpGet]
        public IHttpActionResult Authenticate(string id)
        {
            var today= System.DateTime.Now.ToString("dddd"); ;
            var card = db.Cards.Include("Driver").FirstOrDefault(s => s.Code == id && s.IsActive);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            if (card!=null)
            {
                var logined = card.CardLoginHistories.FirstOrDefault(c => !c.ExitDate.HasValue);
                if (logined!=null)
                {
                    hubContext.Clients.All.Alarm(logined.Id, $"این کارت در تاریخ  {logined.LoginDate.ToShamsi('s')} ورودی داشته که تاریخ خروج برای آن ثبت نشده است.");
                }
                //******************************* خط پایین موقت است
 
                if (db.Cards.Any(s =>((Attendance.Core.Enums.WeekDays)s.Day).ToString() == today))
                {

                    var loginDate = new Models.Entities.CardLoginHistory()
                    {
                        Card = card,
                        CardId = card.Id,
                        CarNumber = id,
                        CreationDate = DateTime.Now,
                        LoginDate = DateTime.Now,
                        DriverName = card.Driver.FullName,
                        IsSuccess = true
                    };
                    db.CardLoginHistories.Add(loginDate); db.SaveChanges();
                    hubContext.Clients.All.addNewMessageToPage(card.Id,card.Driver.FullName, loginDate.Id , $"با کد {card.Code} اجازه ورود دارد");
                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"موفق" } },
                        Ok = true
                    });
                }

                db.CardLoginHistories.Add(new Models.Entities.CardLoginHistory()
                {
                    Card = card,
                    CardId = card.Id,
                    CarNumber = id,
                    CreationDate = DateTime.Now,
                    LoginDate = DateTime.Now,
                    DriverName=card.Driver.FullName ,
                    Description = $"کارت اجازه ورود ندارد",
                    IsSuccess = false
                });
                db.SaveChanges();
            hubContext.Clients.All.addNewMessageToPage(null,null,null, $"کارت اجازه ورود ندارد");
                return Ok(new CustomResponseViewModel()
                {
                    Extra = "",
                    Messages = new List<MessageViewModel>() { new MessageViewModel(){ Description= $"کارت اجازه ورود ندارد" } },
                    Ok = true
                });
            }
            db.CardLoginHistories.Add(new Models.Entities.CardLoginHistory()
            {
                Card = null,
                CardId = null,
                CarNumber = id,
                CreationDate = DateTime.Now,
                LoginDate = DateTime.Now,
                IsSuccess = false

            }); db.SaveChanges();
            hubContext.Clients.All.addNewMessageToPage(null,null,null, $"کارت معتبر نیست");
            return Ok(new CustomResponseViewModel()
            {
                Extra = "",
                Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"کارت معتبر نیست" } },
                Ok = true
            });
        }
        // GET /api/authors/1/books
        [Route("~/api/Exit/{id}/auth")]
        [HttpGet]
        public IHttpActionResult Exit(string id)
        { 
            var card = db.Cards.Include("Driver").FirstOrDefault(s => s.Code == id && s.IsActive);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            if (card!=null)
            {
                var login = card.CardLoginHistories.LastOrDefault(x=>!x.ExitDate.HasValue);
                login.ExitDate = DateTime.Now;
                //db.SaveChanges();
            hubContext.Clients.All.Exit(login.Id, $"خروج با موفقیت ثبت شد");
                return Ok(new CustomResponseViewModel()
                {
                    Extra = "",
                    Messages = new List<MessageViewModel>() { new MessageViewModel(){ Description= $"خروج با موفقیت ثبت شد" } },
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
                        Extra= "",
                        Messages=null,
                        Ok=true
                    }
                    );
            } 
    }
}
