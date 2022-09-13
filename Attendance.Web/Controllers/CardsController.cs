using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Attendance.Core.Data;
using Attendance.Core.Enums;
using Attendance.Models;
using Attendance.Models.Entities;

namespace Attendance.Web.Controllers
{
    public class CardsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Cards
        public ActionResult Index()
        {
            List<Card> cards;
            if (IsSuperAdmin())
                cards = db.Cards.Include(c => c.Driver).Where(c => c.IsDeleted == false)
                    .OrderByDescending(c => c.CreationDate).ToList();
            else
                cards = db.Cards.Include(c => c.Driver).Where(c => c.IsDeleted == false && c.IsHidden == false)
                    .OrderByDescending(c => c.CreationDate).ToList();

            return View(cards.ToList());
        }

        public bool IsSuperAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                if (role == SecurityRole.SuperAdmin)
                    return true;

                return false;
            }
            return false;

        }

        // GET: Cards/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // GET: Cards/Create
        public ActionResult Create()
        {
            ViewBag.DriverId = new SelectList(db.Drivers.Where(d => !d.IsDeleted), "Id", "FullName");
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Card card)
        {
            if (ModelState.IsValid)
            {
                card.IsDeleted = false;
                card.CreationDate = DateTime.Now;

                var driver = db.Drivers.Find(card.DriverId);
                if (driver != null)
                {
                    var day = (int)card.Day;
                    card.DisplayCode = $"{day}-{driver.NationalCode.Substring(3)}";
                }

                card.Id = Guid.NewGuid();
                if (!db.Cards.Any(c => c.Driver.NationalCode.Trim() == driver.NationalCode.Trim()))
                {
                    db.Cards.Add(card);
                    db.SaveChanges();
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                }
                else
                {
                    TempData["Toastr"]= new ToastrViewModel() { Class = "warning", Text = "یک کارت با مشخصات این راننده در سیستم وجود دارد. " };
                }
                return RedirectToAction("Index");
            }
             
            ViewBag.DriverId = new SelectList(db.Drivers.Where(d => !d.IsDeleted), "Id", "FullName", card.DriverId);
            TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
        };
            return View(card);
        }

        // GET: Cards/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverId = new SelectList(db.Drivers.Where(d => !d.IsDeleted), "Id", "FullName", card);
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Card card)
        {
            // Type day = typeof(Core.Enums.Enums.DayOfWeek);
            if (ModelState.IsValid)
            {
                card.IsDeleted = false;
                card.LastModifiedDate = DateTime.Now;

                var driver = db.Drivers.Find(card.DriverId);
                if (driver != null)
                {
                    var day = (int)card.Day;
                    card.DisplayCode = $"{day}-{driver.NationalCode.Substring(3)}";
                }

                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DriverId = new SelectList(db.Drivers.Where(d=>!d.IsDeleted), "Id", "FullName", card.DriverId);
            return View(card);
        }

        // GET: Cards/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Card card = db.Cards.Find(id);
            card.IsDeleted = true;
            card.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Authenticate()
        {
            return View();
        }


        public ActionResult AuthenticateForm(Guid id)
        {
            var cardId = id;
            ViewBag.plecks = db.Cars.Where(c=>!c.IsDeleted).Select(c => new Select2Model { id = c.Id.ToString(), text = c.Number }).ToList();
            //var login = db.CardLoginHistories.FirstOrDefault(c => c.Id == id);
            var card = db.Cards.Include(x => x.Driver).FirstOrDefault(x => x.Id == id);
            return PartialView(
                new AuthenticateFormViewModel()
                {
                    LoginId = Guid.NewGuid(),
                    Driver = card?.Driver ?? default,
                    //Car = card?.Driver?.
                    Car = card?.CardLoginHistories?.FirstOrDefault()?.Car,
                    Card = card,
                    cardId = card.Id,
                    DriverFirstName = card.Driver.FirstName,
                    DriverLastName = card.Driver.LastName,
                    DriverNatCode = card.Driver.NationalCode
                }
                );
        }

        [HttpPost]
        public JsonResult GetCarType(string q)
        {
            Guid carId = Guid.Parse(q);
            var car = db.Cars.Include(x => x.CarType).FirstOrDefault(x => x.Id == carId);
            var type = car?.CarType ?? default;
            return Json(new AuthenticateFormViewModel()
            {
                Type = type?.Title??"",
                Weight = type?.Weight??decimal.Zero,
                Err = car.IsActive && type.IsActive?false : true 
            });
        }

        [HttpPost]
        public ActionResult SubmitForm(AuthenticateFormViewModel model)
        {
            //var cardLoginHistory = db.CardLoginHistories.FirstOrDefault(c => c.Id == model.LoginId);
            var cardLoginHistory = new CardLoginHistory();

            cardLoginHistory.Id = model.LoginId;

            //Driver
            //Deiver exist?
            var driverId = Guid.Parse(model.DriverNatCode.Trim());
            var driver = db.Drivers.FirstOrDefault(d =>
            d.Id == driverId);
            if (driver == null)
            {
                //create exist
                driver = new Driver()
                {
                    Id = Guid.NewGuid(),
                    FirstName = model.DriverFirstName,
                    LastName = model.DriverLastName,
                    NationalCode = model.DriverNatCode,
                    CreationDate = DateTime.Now
                };
                db.Drivers.Add(driver);
                db.SaveChanges();
            }
            cardLoginHistory.Driver = driver;
            cardLoginHistory.DriverId = driver.Id;

            //Card is exist
            var card = db.Cards.FirstOrDefault(c => c.Id == model.cardId);
            cardLoginHistory.Card = card;
            cardLoginHistory.CardId = card.Id;

            //Car is exist? should be exist
            var carId = Guid.Parse(model.Pleck);
            var car = db.Cars.FirstOrDefault(c => c.Id == carId);
            cardLoginHistory.Car = car;
            cardLoginHistory.CarId = carId;

            cardLoginHistory.AssistanceLastName = model.AssistanceLastName;
            cardLoginHistory.AssistanceName = model.AssistanceName;
            cardLoginHistory.AssistanceNationalCode = model.AssistanceNationalCode;
            cardLoginHistory.IsActive = true;
            cardLoginHistory.CreationDate = DateTime.Now;
            cardLoginHistory.LoginDate = DateTime.Now;
            cardLoginHistory.IsActive = true;
            cardLoginHistory.IsSuccess = true;
            cardLoginHistory.Load = model.Load;
            db.CardLoginHistories.Add(cardLoginHistory);
            db.SaveChanges();
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            return Redirect("/cards/authenticate");
        }

        public JsonResult GetPleckList(string q)
        {
            if (q == null)
            {
                q = string.Empty;
            }
            var result = db.Cars.Where(c =>!c.IsDeleted && c.Number.Contains(q)).Select(c => new { Id = c.Id, Text = c.Number }).ToList();
            return Json(new { items = result 
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoginHistoryDetials(Guid id)
        {
            CardLoginHistory login = db.CardLoginHistories.Where(x=>!x.IsDeleted).Include(x => x.Car).Include(x => x.Driver).Include(x => x.Card)
                .FirstOrDefault(x => x.Id == id);
            ViewBag.PageTitle = $"تاریخچه ورود {login.Driver.FirstName} {login.Driver.LastName}";
            return View(login);
        }

        [HttpPost]
        public ActionResult LoginHistory(CardLoginHistory cardLoginHistory)
        {
            var loginHistory = db.CardLoginHistories.Find(cardLoginHistory.Id);
            loginHistory.Description = cardLoginHistory.Description;
            db.SaveChanges();
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            return RedirectToAction("LoginHistoryDetials",new { id = loginHistory.Id}); 
        }

        public JsonResult GetDriverList(string q)
        {
            if (q == null)
            {
                q = string.Empty;
            }
            var result = db.Drivers.Where(c =>!c.IsDeleted && c.NationalCode.Contains(q)).Select(c => new { Id = c.Id, Text = c.NationalCode }).ToList();
            return Json(new { items = result }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult GetDriver(string q)
        {
            Guid driverId;
            if (Guid.TryParse(q, out driverId))
            { 
                var driver = db.Drivers.FirstOrDefault(x => x.Id == driverId) ?? default;
                return Json(new
                {
                    DriverFirstName = driver.FirstName,
                    DriverLastName = driver.LastName
                });
            }
            return Json(new
            {
                DriverFirstName = "",
                DriverLastName = ""
            });
        }


        public ActionResult Export()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            DataTable dt = new DataTable();

            //Condition1: •	در یوزر مدیر معمولی زمانی که خروجی اکسل میگیریم فیلد
            //IsHidden
            //مشاهده نگردد و فقط در مدیر ارشد این قابلیت باشد. 
            #region Condition1
            if (role == SecurityRole.SuperAdmin)
            {
                dt = db.Cards.Include(c => c.Driver).Where(c => !c.IsDeleted).ToList().Select(c =>
new
{
    Code = c?.Code,
    DisplayCode = c?.DisplayCode,
    IsHidden = c.IsHidden ? "مخفی" : "قابل مشاهده",
    Day = c?.Day.GetDisplayName(),
    Driver = c?.Driver.FullName,
    NationalCode = c?.Driver?.NationalCode,
    IsActive = c.IsActive ? "فعال" : "غیرفعال",
    CreateDate = c?.CreationDate.ToShamsi('s'),
    UpdateDate = c?.CreationDate.ToShamsi('s'),
    Description = c?.Description
}).ToList().ToDataTable();
            }
            else
            {
                dt = db.Cards.Include(c => c.Driver).Where(c => !c.IsDeleted).ToList().Select(c =>
            new
            {
                Code = c?.Code,
                DisplayCode = c?.DisplayCode,
                Day = c?.Day.GetDisplayName(),
                Driver = c?.Driver.FullName,
                NationalCode = c?.Driver?.NationalCode,
                IsActive = c.IsActive ? "فعال" : "غیرفعال",
                CreateDate = c?.CreationDate.ToShamsi('s'),
                UpdateDate = c?.CreationDate.ToShamsi('s'),
                Description = c?.Description
            }).ToList().ToDataTable();
            }
            #endregion


            dt.TableName = "اکسل کارت ها"; 
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();
            


            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment; filename={dt.TableName}{DateTime.Now.ToShamsi('e')}.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            return RedirectToAction("Index");
        }

        public ActionResult PrintLoginHistoryDetails(Guid id)
        {
            CardLoginHistory login = db.CardLoginHistories.Where(x => !x.IsDeleted).Include(x => x.Car)
                .Include(x => x.Driver).Include(x => x.Card).FirstOrDefault(x => x.Id == id);
            ViewBag.PageTitle = $"تاریخچه ورود {login.Driver.FirstName} {login.Driver.LastName}";
            return View(login);
        }

        public ActionResult Status(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            CardStatusHistory status = new CardStatusHistory()
            {
                CardId = id,
                Card = card
            };
            return View(status);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status(CardStatusHistory status)
        { 
            Card card = db.Cards.Find(status.CardId); 
            var current = new CardStatusHistory()
            {
                Id=Guid.NewGuid(),
                Card=card,
                CardId=status.CardId,
                Description = status.Description,
                PreviousStatus=card.IsActive,
                CurrentStatus = !card.IsActive,
                IsActive=true,
                CreationDate=DateTime.Now
            };
            card.IsActive = !card.IsActive;
            db.Entry(current).State = EntityState.Added;
            db.Entry(card).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }




        public ActionResult Group(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Include(x=>x.CardGroupItems).FirstOrDefault(x=>x.Id==id);
            if (card == null)
            {
                return HttpNotFound();
            }
            var model = new CardGroupViewModel()
            {
                Card=card,
                CardGroups = db.Groups.Where(x=>x.GroupItems.Any())
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Group(List<GroupCardViewModel> model)
        {
            try
            {
                model.ForEach(x =>
                {
                    var cardId = Guid.Parse(x.CardId);
                    var groupItemId = Guid.Parse(x.GroupItemId);
                    if (!db.CardGroupItemCards.Any(c => c.CardId == cardId && c.CardGroupItemId == groupItemId))
                    {
                        var card = db.Cards.Find(cardId);
                        var groupItem = db.GroupItems.FirstOrDefault(gi=>gi.Id ==groupItemId);

                        db.CardGroupItemCards.RemoveRange(db.CardGroupItemCards.Where(cg => cg.GroupItem.GroupId == groupItem.GroupId && cg.CardId ==cardId));

                        db.CardGroupItemCards.Add(new CardGroupItemCard()
                        {
                            Card = card,
                            CardId = cardId,
                            GroupItem = groupItem,
                            CardGroupItemId = groupItemId,
                            Id = Guid.NewGuid(),
                            CreationDate = DateTime.Now,
                            IsActive = true
                        });
                        db.SaveChanges();
                    }
                });

                return Json(new { code = 0, message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex )
            {
                return Json(new { code = -1, message = "عملیات با خطا روبرو شد"+ ex.Message });
            }
        }

    }
}
