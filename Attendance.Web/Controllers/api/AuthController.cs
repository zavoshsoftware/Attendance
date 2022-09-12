using Attendance.Models;
using Attendance.Models.Entities;
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
            var card = db.Cards.Include(c=>c.Driver).FirstOrDefault(s => s.Code == id && s.IsActive);
            List<ToastrViewModel> toastrList = new List<ToastrViewModel>();
            
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            if (card!=null)
            {
                if (!card.Driver.IsActive)
                {
                    // چنانچه فردی را در قسمت رانندگان غیر فعال کردیم علت درج شود و چنانچه کارت در قسمت کنترل ورود و خروج ثبت شد سیستم اخطار دهد که این فرد به دلیلی که قید شده مجاز به تردد نمی باشد و فرد راننده باید تغییر کند یا از حالت غیر فعال خارج شود
                    var message = $"{card.Driver.FullName} به دلیل '{card.Driver.Description}' مجاز به تردد نمی باشد. با مدیر سیستم در ارتباط باشید.";
                    hubContext.Clients.All.Alarm(null, message);
                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = message } },
                        Ok = true
                    });
                }

                var logined = card.CardLoginHistories.FirstOrDefault(c => !c.ExitDate.HasValue);
                if (logined!=null) hubContext.Clients.All.Alarm(logined.Id, $"این کارت در تاریخ  {logined.LoginDate.ToShamsi('s')} ورودی داشته که تاریخ خروج برای آن ثبت نشده است.");


                if (card.Day.ToString() == today)
                { 
                    hubContext.Clients.All.addNewMessageToPage(card.Id,card.Driver.FirstName + " "+card.Driver.LastName,
                        null, $"با کد {card.DisplayCode} اجازه ورود دارد",card.Code);
                    return Ok(new CustomResponseViewModel()
                    {
                        Extra = "",
                        Messages = new List<MessageViewModel>() { new MessageViewModel() { Description = $"موفق" } },
                        Ok = true
                    });
                }
                 
            hubContext.Clients.All.addNewMessageToPage(null,null,null, $"کارت اجازه ورود ندارد");
                return Ok(new CustomResponseViewModel()
                {
                    Extra = "",
                    Messages = new List<MessageViewModel>() { new MessageViewModel(){ Description= $"کارت اجازه ورود ندارد" } },
                    Ok = true
                });
            } 
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
                var login = card.CardLoginHistories.OrderBy(x=>x.CreationDate).LastOrDefault(x=>!x.ExitDate.HasValue);
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
