using Attendance.Models;
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
            var today= System.DateTime.Now.ToString("dddd");
            var card = db.Cards.Include("CardDays").Include("Driver").FirstOrDefault(s => s.Code == id && s.IsActive);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<AtnHub>();
            if (card!=null)
            {
                if (db.Cards.Any(s=> s.CardDays.Any(x => x.WeekDays.ToString() == today)))
                {
                    db.CardLoginHistories.Add(new Models.Entities.CardLoginHistory()
                    {
                        Card = card,
                        CardId = card.Id,
                        CarNumber = id,
                        CreationDate = DateTime.Now,
                        LoginDate = DateTime.Now,
                        DriverName = card.Driver.FullName
                    }); db.SaveChanges();
                    hubContext.Clients.All.addNewMessageToPage(card.Driver.FullName,$"با کد {card.Code} اجازه ورود دارد");
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
                    Description = $"کارت اجازه ورود ندارد"
                });
                db.SaveChanges();
            hubContext.Clients.All.addNewMessageToPage(null, $"کارت اجازه ورود ندارد");
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
                LoginDate = DateTime.Now
            }); db.SaveChanges();
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
