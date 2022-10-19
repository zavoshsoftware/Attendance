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
using Attendance.Web.ViewModels;
using ClosedXML.Excel;

namespace Attendance.Web.Controllers
{
    public class CardsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Cards
        public ActionResult Index(bool isDeleted=false)
        {
            ViewBag.IsDeleted = isDeleted;
            List<Card> cards;
            var result = db.Cards.Include(c => c.Driver).Where(c => c.IsDeleted == isDeleted && !c.Driver.IsDeleted).OrderByDescending(c => c.CreationDate);
            if (IsSuperAdmin())
                cards = result.ToList();
            else
                cards = result.Where(c => c.IsHidden == false)
                    .ToList();

            return View(cards.ToList().Distinct());
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
                card.IsActive = true;
                if (!db.Cards.Any(c => c.Driver.Id == driver.Id && !c.IsDeleted))
                {
                    db.Cards.Add(card);
                    db.SaveChanges();
                    db.CardStatusHistories.Add(new CardStatusHistory() { 
                    Card=card,
                    CardId = card.Id,
                    Id=Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    IsActive=true,
                    IsDeleted =false,
                    PreviousStatus = false,
                    CurrentStatus = true,
                    Description="کارت با موفقیت ایجاد شد",
                    Operator = User.Identity.Name
                    });
                    db.SaveChanges();
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                }
                else
                {
                    TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "یک کارت با مشخصات این راننده در سیستم وجود دارد. " };
                }
                return RedirectToAction("Index");
            }

            ViewBag.DriverId = new SelectList(db.Drivers.Where(d => !d.IsDeleted), "Id", "FullName", card.DriverId);
            TempData["Toastr"] = new ToastrViewModel()
            {
                Class = "warning",
                Text = string.Join("; ", ModelState.Values
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
                var lastDriverId = db.Cards.AsNoTracking().FirstOrDefault(c => c.Id == card.Id).DriverId;


                var driver = db.Drivers.Find(card.DriverId);
                if (driver != null)
                {
                    var day = (int)card.Day;
                    card.DisplayCode = $"{day}-{driver.NationalCode.Substring(3)}";
                }

                if ((lastDriverId != card.DriverId && !db.Cards.Any(c => c.Driver.Id == driver.Id && !c.IsDeleted)) || lastDriverId == card.DriverId)
                {
                    if (lastDriverId != card.DriverId)
                    {
                        db.CardOwnerHistories.Add(new CardOwnerHistory()
                        {
                            Id = Guid.NewGuid(),
                            CardId = card.Id,
                            CreationDate = DateTime.Now,
                            PreviousDriver = lastDriverId,
                            CurrentDriver = card.DriverId,
                            IsActive = true
                        });
                        db.SaveChanges();
                    }

                    db.Entry(card).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                }
                else
                {
                    TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "یک کارت با مشخصات این راننده در سیستم وجود دارد. " };
                }

                return RedirectToAction("Index");
            }
            ViewBag.DriverId = new SelectList(db.Drivers.Where(d => !d.IsDeleted), "Id", "FullName", card.DriverId);
            return View(card);
        }

        public ActionResult DriversHistory(Guid cardId)
        {
            var driverHistory = db.CardOwnerHistories.Where(x => x.CardId == cardId).OrderByDescending(x=>x.CreationDate).ToList();
            return View(driverHistory);
        }

        public string GetDriverFullName(Guid driverId)
        {
            var driver = db.Drivers.Find(driverId);
            return driver.FullName;
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
            ViewBag.plecks = db.Cars.Where(c => !c.IsDeleted).Select(c => new Select2Model { id = c.Id.ToString(), text = c.Number }).ToList();
            //var login = db.CardLoginHistories.FirstOrDefault(c => c.Id == id);
            var card = db.Cards.Include(x => x.Driver).Include(x=>x.CardLoginHistories).FirstOrDefault(x => x.Id == id);
            CardLoginHistory login = card.CardLoginHistories.OrderByDescending(x=>x.CreationDate).FirstOrDefault();
            try
            {
                if (login != null)
                {
                    return PartialView(
                    new AuthenticateFormViewModel()
                    {
                        LoginId = Guid.NewGuid(),
                        Driver = card?.Driver ?? default,
                        carId = login?.CarId ?? null,
                        Car = login?.Car,
                        Card = card,
                        cardId = card?.Id ?? null,
                        DriverFirstName = card.Driver.FirstName,
                        DriverLastName = card.Driver.LastName,
                        DriverNatCode = card.Driver.NationalCode,
                        AssistanceId = db.Drivers.AsNoTracking().FirstOrDefault(x => x.NationalCode == login.AssistanceNationalCode)?.Id ?? null,
                        AssistanceName = login?.AssistanceName ?? "",
                        AssistanceLastName = login?.AssistanceLastName ?? "",
                        AssistanceNationalCode = login?.AssistanceNationalCode ?? "",
                        Type = db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == login.Car.CarTypeId)?.Title ?? "",
                        Load = login.Load,
                        Pleck = login?.Car?.Number ?? "",
                        Weight = db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == login.Car.CarTypeId)?.Weight ?? decimal.Zero,
                    }
                    );
                }
                else
                {
                    return PartialView(
                    new AuthenticateFormViewModel()
                    {
                        LoginId = Guid.NewGuid(),
                        Driver = card?.Driver ?? default,
                        Card = card,
                        cardId = card.Id,
                        DriverFirstName = card.Driver.FirstName,
                        DriverLastName = card.Driver.LastName,
                        DriverNatCode = card.Driver.NationalCode,
                    }
                    );
                }
            }
            catch (Exception)
            {
                return PartialView(
                    new AuthenticateFormViewModel()
                    {
                        LoginId = Guid.NewGuid(),
                        Driver = card?.Driver ?? default,
                        Card = card,
                        cardId = card.Id,
                        DriverFirstName = card.Driver.FirstName,
                        DriverLastName = card.Driver.LastName,
                        DriverNatCode = card.Driver.NationalCode,
                    }
                    );
            }
        }

        [HttpPost]
        public JsonResult GetCarType(string q)
        {
            Guid carId = Guid.Parse(q);
            var car = db.Cars.Include(x => x.CarType).FirstOrDefault(x => x.Id == carId);
            var type = car?.CarType ?? default;
            return Json(new AuthenticateFormViewModel()
            {
                Type = type?.Title ?? "",
                Weight = type?.Weight ?? decimal.Zero,
                Err = car.IsActive && type.IsActive ? false : true,
                ErrMessage = car.IsActive ? type.Description : car.Description
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
            var car = db.Cars.Include(s => s.CarType).FirstOrDefault(c => c.Id == carId);
            cardLoginHistory.Car = car;
            cardLoginHistory.CarId = carId;

            var assistId = Guid.Parse(model.AssistanceNationalCode.Trim());
            var assist = db.Drivers.FirstOrDefault(d =>
            d.Id == driverId);
            if (assist != null)
            {

                cardLoginHistory.AssistanceLastName = model.AssistanceLastName;
                cardLoginHistory.AssistanceName = model.AssistanceName;
                cardLoginHistory.AssistanceNationalCode = assist.NationalCode;
            }
            cardLoginHistory.IsActive = true;
            cardLoginHistory.CreationDate = DateTime.Now;
            cardLoginHistory.LoginDate = DateTime.Now;
            cardLoginHistory.IsActive = true;
            cardLoginHistory.IsSuccess = true;
            cardLoginHistory.Load = model.Load;
            cardLoginHistory.TotalLoad = model.Load + car.CarType.Weight;
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
            var result = db.Cars.Where(c => !c.IsDeleted && c.Number.Contains(q)).Select(c => new { Id = c.Id, Text = c.Number }).ToList();
            return Json(new
            {
                items = result
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoginHistoryDetials(Guid id,bool isExit=false)
        {
            ViewBag.IsExit = isExit;
            CardLoginHistory login = db.CardLoginHistories.Where(x => !x.IsDeleted).Include(x => x.Car).Include(x => x.Driver).Include(x => x.Card)
                .FirstOrDefault(x => x.Id == id);
            ViewBag.Weight = (int)db.CarTypes.AsNoTracking()?.FirstOrDefault(x => x.Id == login.Car.CarTypeId)?.Weight;
            ViewBag.PageTitle = $"تاریخچه ورود {login.Driver.FirstName} {login.Driver.LastName}";
         
            return View(login);
        }

        [HttpPost]
        public ActionResult LoginHistory(CardLoginHistory cardLoginHistory,LoginHistoryTool loginHistoryTool)
        {
            var loginHistory = db.CardLoginHistories.Find(cardLoginHistory.Id);
            loginHistory.Description = cardLoginHistory.Description;
            loginHistory.Devices = cardLoginHistory.Devices;
            db.LoginHistoryTools.Add(new LoginHistoryTool() {
            Amount = loginHistoryTool.Amount,
            ToolId = loginHistoryTool.ToolId,
            UnitId = loginHistoryTool.UnitId,
            CardLoginHistoryId = loginHistoryTool.CardLoginHistoryId,
            CreationDate = DateTime.Now,
            IsActive = true,
            Id = Guid.NewGuid(),
            IsDeleted=false 
            });
            db.SaveChanges();
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            return RedirectToAction("LoginHistoryDetials", new { id = loginHistory.Id });
        }

        public JsonResult GetDriverList(string q, int type = 0)
        {
            if (q == null)
            {
                q = string.Empty;
            }
            Guid driverId;
            if (Guid.TryParse(q, out driverId))
            {
                var result = db.Drivers.Where(c => !c.IsDeleted && c.Id.Equals(driverId)/* && c.DriverType == (DriverType)type*/).Select(c => new { Id = c.Id, Text = c.NationalCode }).ToList();
                return Json(new { items = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = db.Drivers.Where(c => !c.IsDeleted && c.NationalCode.Contains(q) /*&& c.DriverType == (DriverType)type*/).Select(c => new { Id = c.Id, Text = c.NationalCode }).ToList();
                return Json(new { items = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCardList(string q)
        {
            if (q == null)
            {
                q = string.Empty;
            }
            Guid cardId;
            if (Guid.TryParse(q, out cardId))
            {
                var result = db.Cards.Where(c => !c.IsDeleted && c.Id.Equals(cardId)/* && c.DriverType == (DriverType)type*/).Select(c => new { Id = c.Id, Text = c.DisplayCode }).ToList();
                return Json(new { items = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = db.Cards.Where(c => !c.IsDeleted && c.DisplayCode.Contains(q) /*&& c.DriverType == (DriverType)type*/)
                    .Select(c => new { Id = c.Id, Text = c.DisplayCode /*, LoginHistoryId = c.CardLoginHistories.LastOrDefault().Id*/ }).ToList();
                return Json(new { items = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDriver(string q, int type = 0)
        {
            Guid driverId;
            if (Guid.TryParse(q, out driverId))
            {
                var driver = db.Drivers/*.Where(x=>x.DriverType == (DriverType)type)*/.FirstOrDefault(x => x.Id == driverId) ?? default;
                return Json(new
                {
                    DriverFirstName = driver.FirstName,
                    DriverLastName = driver.LastName,
                    Id = driverId
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
                dt = db.Cards.Include(c => c.Driver).Where(c => !c.IsDeleted && !c.IsHidden).ToList().Select(c =>
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
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Worksheets.FirstOrDefault().ColumnWidth = 20;
                using (MemoryStream stream = new MemoryStream())
                {
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{dt.TableName}{DateTime.Now.ToShamsi('e')}.xlsx");
                }
            }
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
                Id = Guid.NewGuid(),
                Card = card,
                CardId = status.CardId,
                Description = status.Description,
                PreviousStatus = card.IsActive,
                CurrentStatus = !card.IsActive,
                IsActive = true,
                CreationDate = DateTime.Now,
                Operator = User.Identity.Name
            };
            card.IsActive = !card.IsActive;
            card.Description = status.Description;
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
            Card card = db.Cards.Include(x => x.CardGroupItems).FirstOrDefault(x => x.Id == id);
            if (card == null)
            {
                return HttpNotFound();
            }
            var model = new CardGroupViewModel()
            {
                Card = card,
                CardGroups = db.Groups.Where(x => x.GroupItems.Any()&&!x.IsDeleted)
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
                        var groupItem = db.GroupItems.FirstOrDefault(gi => gi.Id == groupItemId);

                        db.CardGroupItemCards.RemoveRange(db.CardGroupItemCards.Where(cg => cg.GroupItem.GroupId == groupItem.GroupId && cg.CardId == cardId));

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
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "عملیات با خطا روبرو شد" + ex.Message });
            }
        }


        public JsonResult GetLastCardLoginHistory(Guid id)
        {
            var lastCardLoginHistory = db.Cards.Include(x => x.CardLoginHistories).Include(x => x.Driver).Include(x => x.Penalties).FirstOrDefault(x => x.Id == id)
                .CardLoginHistories.OrderByDescending(x=>x.CreationDate)?.FirstOrDefault()??null;
            if(lastCardLoginHistory!=null)
            {
                return Json(new
                {
                    Id = lastCardLoginHistory?.Id,
                    CreationDate = lastCardLoginHistory?.CreationDate,
                    AssistanceId = db.Drivers.AsNoTracking()?.FirstOrDefault(x=>x.NationalCode == lastCardLoginHistory.AssistanceNationalCode)?.Id??null,
                    AssistanceName = lastCardLoginHistory?.AssistanceName,
                    AssistanceLastName = lastCardLoginHistory?.AssistanceLastName,
                    AssistanceNationalCode = lastCardLoginHistory?.AssistanceNationalCode,
                    DriverFirstName = lastCardLoginHistory?.Driver?.FirstName,
                    DriverFirstLastName = lastCardLoginHistory?.Driver?.LastName,
                    LoginDate = lastCardLoginHistory?.LoginDate,
                    CarNumber = lastCardLoginHistory?.CarNumber,
                    Load = lastCardLoginHistory?.Load,
                    Car = lastCardLoginHistory?.Car?.Title,
                    CarId = lastCardLoginHistory?.Car?.Id,
                    CarLoad = lastCardLoginHistory?.Car?.CarType?.Weight ?? 0
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Inquiry()
        {
            return View();
        }

        public ActionResult InquiryForm(Guid id)
        {
            var cardId = id;
            ViewBag.plecks = db.Cars.Where(c => !c.IsDeleted).Select(c => new Select2Model { id = c.Id.ToString(), text = c.Number }).ToList();
            //var login = db.CardLoginHistories.FirstOrDefault(c => c.Id == id);
            var card = db.Cards.Include(x => x.Driver).Include(x => x.CardLoginHistories).FirstOrDefault(x => x.Id == id);
            CardLoginHistory login = card.CardLoginHistories.OrderByDescending(x => x.CreationDate).FirstOrDefault();
            if (login != null)
            {
                return PartialView(
                new AuthenticateFormViewModel()
                {
                    LoginId = Guid.NewGuid(),
                    Driver = card?.Driver ?? default,
                    carId = login?.CarId ?? null,
                    Car = login?.Car,
                    Card = card,
                    cardId = card.Id,
                    DriverFirstName = card.Driver.FirstName,
                    DriverLastName = card.Driver.LastName,
                    DriverNatCode = card.Driver.NationalCode,
                    AssistanceId = db.Drivers.AsNoTracking().FirstOrDefault(x => x.NationalCode == login.AssistanceNationalCode).Id,
                    AssistanceName = login?.AssistanceName ?? "",
                    AssistanceLastName = login?.AssistanceLastName ?? "",
                    AssistanceNationalCode = login?.AssistanceNationalCode ?? "",
                    Type = db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == login.Car.CarTypeId)?.Title ?? "",
                    Load = login.Load,
                    Pleck = login?.Car?.Number ?? "",
                    Weight = db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == login.Car.CarTypeId)?.Weight ?? decimal.Zero,
                }
                );
            }
            else
            {
                return PartialView(
                new AuthenticateFormViewModel()
                {
                    LoginId = Guid.NewGuid(),
                    Driver = card?.Driver ?? default,
                    Card = card,
                    cardId = card.Id,
                    DriverFirstName = card.Driver.FirstName,
                    DriverLastName = card.Driver.LastName,
                    DriverNatCode = card.Driver.NationalCode,
                }
                );
            }
        }

        public ActionResult UseCard(Guid id)
        {
            var card = db.Cards.Find(id);
            if (card!=null)
            {
                card.IsDeleted = false;
                db.SaveChanges(); 
                TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                return RedirectToAction("Index");
            }
            TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "کارت یافت نشد" };
            return RedirectToAction("Index");
        }

    }
}
