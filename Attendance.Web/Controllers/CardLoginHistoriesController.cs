using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Attendance.Core.Data;
using Attendance.Core.Enums;
using Attendance.Models;
using Attendance.Models.Entities;
using ClosedXML.Excel;

namespace Attendance.Web.Controllers
{
    public class CardLoginHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CardLoginHistories
        public ActionResult Index(Guid? cardId, Guid? driverId, string fromdate, string todate)
        {
            var from = string.IsNullOrEmpty(fromdate) ? DateTime.Now.AddYears(-5).Date : fromdate.ToMiladi();
            var to = string.IsNullOrEmpty(todate) ? DateTime.Now.Date.AddDays(1) : todate.ToMiladi();
            ViewBag.From = fromdate;
            ViewBag.To = todate;
            ViewBag.CardId = cardId;
            ViewBag.DriverId = driverId;
            ViewBag.IsHidden = false;
            var cardLoginHistoryQuery = db.CardLoginHistories.AsQueryable().Include(c => c.Card).Include(c => c.Driver)
                .Where(c=>!c.IsDeleted && !c.Card.IsHidden && c.LoginDate>=from && c.LoginDate <= to)
                .OrderByDescending(c => c.CreationDate);
            
            if (cardId.HasValue)
            {
                var cardLoginHistories = cardLoginHistoryQuery.Where(c => c.CardId == cardId);
                return View(cardLoginHistories.ToList());
            }
            else if (driverId.HasValue)
            {
                var cardLoginHistories = cardLoginHistoryQuery.Where(c => c.DriverId == driverId) ;
                return View(cardLoginHistories.ToList());
            }
            else
            {
                var cardLoginHistories = cardLoginHistoryQuery;
                return View(cardLoginHistories.ToList());
            }
        }

        public ActionResult Hidden()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            ViewBag.IsHidden = true;
            if (role == SecurityRole.SuperAdmin)
            {
                var cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card).Where(c => c.IsDeleted == false && c.Card.IsHidden).OrderByDescending(c => c.CreationDate);
                return View("Index", cardLoginHistories.ToList());
            }
            TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "هشدار؛ شما به این بخش دسترسی ندارید" };
            return View("Index");
        }

        public ActionResult IndexNotExit(Guid? cardId,string date)
        {
            ViewBag.date = DateTime.Now.ToShamsi('a');
            if (cardId.HasValue)
            {
                ViewBag.card = db.Cards.Find(cardId.Value);
                var cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card)
            .Where(c => c.IsDeleted == false && c.ExitDate == null && c.CardId == cardId && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);
                return View(cardLoginHistories.ToList());
            }
            ViewBag.card = new Card();
            if (!string.IsNullOrEmpty(date))
            {
                ViewBag.date = date;
                var datetime = date.ToMiladi().Date;
                var cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card)
                          .Where(c => c.IsDeleted == false && c.ExitDate == null && !c.Card.IsHidden && c.LoginDate.Year == datetime.Year && c.LoginDate.Month == datetime.Month && c.LoginDate.Day == datetime.Day).OrderByDescending(c => c.CreationDate);
                return View(cardLoginHistories.ToList());
            }
            else
            {
                var cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card)
              .Where(c => c.IsDeleted == false && !c.Card.IsHidden && c.ExitDate == null).OrderByDescending(c => c.CreationDate);
                return View(cardLoginHistories.ToList());
            }
        }

        [HttpPost]
        public JsonResult GetCardList(string q="")
        {   
                var cards = db.Cards.Where(x => x.DisplayCode.Contains(q)).Select(c => new { Id = c.Id, Text = c.DisplayCode }).ToList();
                return Json(new { items = cards }, JsonRequestBehavior.AllowGet);
           
        }

        // GET: CardLoginHistories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardLoginHistory cardLoginHistory = db.CardLoginHistories.Find(id);
            if (cardLoginHistory == null)
            {
                return HttpNotFound();
            }
            return View(cardLoginHistory);
        }

        // GET: CardLoginHistories/Create
        public ActionResult Create()
        {
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code");
            return View();
        }

        // POST: CardLoginHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LoginDate,IsSuccess,CardId,ExitDate,DriverName,DriverHelperName,CarNumber,TotalLoad,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardLoginHistory cardLoginHistory)
        {
            if (ModelState.IsValid)
            {
                cardLoginHistory.IsDeleted = false;
                cardLoginHistory.CreationDate = DateTime.Now;

                cardLoginHistory.Id = Guid.NewGuid();
                db.CardLoginHistories.Add(cardLoginHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", cardLoginHistory.CardId);
            return View(cardLoginHistory);
        }

        // GET: CardLoginHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardLoginHistory cardLoginHistory = db.CardLoginHistories.Find(id);
            if (cardLoginHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", cardLoginHistory.CardId);
            return View(cardLoginHistory);
        }

        // POST: CardLoginHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LoginDate,IsSuccess,CardId,ExitDate,DriverName,DriverHelperName,CarNumber,TotalLoad,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardLoginHistory cardLoginHistory)
        {
            if (ModelState.IsValid)
            {
                cardLoginHistory.IsDeleted = false;
                cardLoginHistory.LastModifiedDate = DateTime.Now;
                db.Entry(cardLoginHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", cardLoginHistory.CardId);
            return View(cardLoginHistory);
        }

        // GET: CardLoginHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardLoginHistory cardLoginHistory = db.CardLoginHistories.Find(id);
            if (cardLoginHistory == null)
            {
                return HttpNotFound();
            }
            return View(cardLoginHistory);
        }

        // POST: CardLoginHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CardLoginHistory cardLoginHistory = db.CardLoginHistories.Find(id);
            cardLoginHistory.IsDeleted = true;
            cardLoginHistory.DeletionDate = DateTime.Now;

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

        public ActionResult Print(Guid? cardId, Guid? driverId)
        {
            ViewBag.CardId = cardId;
            ViewBag.DriverId = driverId;
            if (cardId.HasValue)
            {
                var cardLoginHistories = db.CardLoginHistories.Where(c => c.CardId == cardId).Include(c => c.Driver).Include(c => c.Card).Where(c => c.IsDeleted == false && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);
                TempData["CardLoginHistory"] = cardLoginHistories?.ToList() ?? Array.Empty<CardLoginHistory>().ToList();
                return View(cardLoginHistories.ToList());
            }
            else if (driverId.HasValue)
            {
                var cardLoginHistories = db.CardLoginHistories.Where(c => c.DriverId == driverId).Include(c => c.Driver).Include(c => c.Card).Where(c => c.IsDeleted == false && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);
                TempData["CardLoginHistory"] = cardLoginHistories?.ToList() ?? Array.Empty<CardLoginHistory>().ToList();
                return View(cardLoginHistories.ToList());
            }
            else
            {
                var cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card).Where(c => c.IsDeleted == false && !c.Card.IsHidden && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);
                TempData["CardLoginHistory"] = cardLoginHistories?.ToList() ?? Array.Empty<CardLoginHistory>().ToList();
                return View(cardLoginHistories.ToList());
            }
        }

        public ActionResult Export(Guid? cardId, Guid? driverId)
        {
            IOrderedQueryable<CardLoginHistory> cardLoginHistories;
            if (cardId.HasValue)
            {
                cardLoginHistories = db.CardLoginHistories.Where(c => c.CardId == cardId).Include(c => c.Driver)
                    .Include(c => c.Card).Where(c => c.IsDeleted == false && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);

            }
            else if (driverId.HasValue)
            {
                cardLoginHistories = db.CardLoginHistories.Where(c => c.DriverId == driverId).Include(c => c.Driver)
                    .Include(c => c.Card).Where(c => c.IsDeleted == false && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);

            }
            else
            {
                cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card)
                    .Where(c => c.IsDeleted == false && !c.Card.IsHidden).OrderByDescending(c => c.CreationDate);

            }
            if (cardLoginHistories.Any())
            {
                var dt = cardLoginHistories?.ToList()?.Select(c =>
                 new
                 {
                     OwnerName = c?.Card?.Driver?.FirstName,
                     OwnerLastName = c?.Card?.Driver?.LastName,
                     OwnerNationalCode = c?.Card?.Driver?.NationalCode,
                     OwnerBirthDate = c?.Card?.Driver?.BirthDate.ToShamsi(),
                     OwnerAge = c?.Card?.Driver?.BirthDate.GetAge(),
                     Enter = c.LoginDate.ToShamsi('s'),
                     Exit = c.ExitDate.ToShamsi('s'),
                     CardCode = c?.Card?.Code,
                     CardDisplay = c?.Card?.DisplayCode,
                     DriverName = c?.Driver?.FirstName,
                     DriverLastName = c?.Driver?.LastName,
                     DriverNationalCode = c?.Driver?.NationalCode,
                     DriverBirthDate = c?.Driver?.BirthDate.ToShamsi(),
                     DriverAge = c?.Driver?.BirthDate.GetAge(),
                     AssistName = c?.AssistanceName,
                     AssistLastName = c?.AssistanceLastName,
                     AssistNationalCode = c?.AssistanceNationalCode,
                     AssistBirthDate = db.Drivers.AsNoTracking().FirstOrDefault(x=>x.NationalCode == c.AssistanceNationalCode)?.BirthDate.ToShamsi(),
                     CarNumber = c?.Car?.Number,
                     Type = db.Cars.Include(x=>x.CarType).FirstOrDefault(x=>x.Id == c.CarId)?.CarType?.Title??"",
                     Weight = Convert.ToInt32(db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == c.Car.CarTypeId).Weight),
                     Load = Convert.ToInt32(c?.Load ?? 0),
                     MainWeight = ((Convert.ToInt32(c?.Load ?? 0)) -(Convert.ToInt32(db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == c.Car.CarTypeId).Weight))),
                     IsActive = c.IsActive ? "فعال" : "غیرفعال",
                     Date = c?.CreationDate.ToShamsi('s')
                 }).ToList().ToDataTable();
                dt.TableName = "اکسل تاریخچه ورود و خروج";
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
            }
            TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "موردی یافت نشد" };
            return RedirectToAction("Index", new { cardId = cardId, driverId = driverId });
        }
        public ActionResult ExportHidden(Guid? cardId, Guid? driverId)
        {
            IOrderedQueryable<CardLoginHistory> cardLoginHistories;
            if (cardId.HasValue)
            {
                cardLoginHistories = db.CardLoginHistories.Where(c => c.CardId == cardId).Include(c => c.Driver)
                    .Include(c => c.Card).Where(c => c.IsDeleted == false && c.Card.IsHidden).OrderByDescending(c => c.CreationDate);

            }
            else if (driverId.HasValue)
            {
                cardLoginHistories = db.CardLoginHistories.Where(c => c.DriverId == driverId).Include(c => c.Driver)
                    .Include(c => c.Card).Where(c => c.IsDeleted == false && c.Card.IsHidden).OrderByDescending(c => c.CreationDate);

            }
            else
            {
                cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card)
                    .Where(c => c.IsDeleted == false && c.Card.IsHidden).OrderByDescending(c => c.CreationDate);

            }
            if (cardLoginHistories.Any())
            {
                var dt = cardLoginHistories?.ToList()?.Distinct()?.Select(c =>
                 new
                 {
                     OwnerName = c?.Card?.Driver?.FirstName,
                     OwnerLastName = c?.Card?.Driver?.LastName,
                     OwnerNationalCode = c?.Card?.Driver?.NationalCode,
                     OwnerBirthDate = c?.Card?.Driver?.BirthDate.ToShamsi(),
                     OwnerAge = c?.Card?.Driver?.BirthDate.GetAge(),
                     Enter = c.LoginDate.ToShamsi('s'),
                     Exit = c.ExitDate.ToShamsi('s'),
                     CardCode = c?.Card?.Code,
                     CardDisplay = c?.Card?.DisplayCode,
                     DriverName = c?.Driver?.FirstName,
                     DriverLastName = c?.Driver?.LastName,
                     DriverNationalCode = c?.Driver?.NationalCode,
                     DriverBirthDate = c?.Driver?.BirthDate.ToShamsi(),
                     DriverAge = c?.Driver?.BirthDate.GetAge(),
                     AssistName = c?.AssistanceName,
                     AssistLastName = c?.AssistanceLastName,
                     AssistNationalCode = c?.AssistanceNationalCode,
                     AssistBirthDate = db.Drivers.AsNoTracking().FirstOrDefault(x=>x.NationalCode == c.AssistanceNationalCode)?.BirthDate.ToShamsi(),
                     CarNumber = c?.Car?.Number,
                     Type = db.Cars.Include(x=>x.CarType).FirstOrDefault(x=>x.Id == c.CarId)?.CarType?.Title??"",
                     Weight = Convert.ToInt32(db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == c.Car.CarTypeId).Weight),
                     Load = Convert.ToInt32(c?.Load ?? 0),
                     MainWeight = ((Convert.ToInt32(c?.Load ?? 0)) -(Convert.ToInt32(db.CarTypes.AsNoTracking().FirstOrDefault(x => x.Id == c.Car.CarTypeId).Weight))),
                     IsActive = c.IsActive ? "فعال" : "غیرفعال",
                     Date = c?.CreationDate.ToShamsi('s')
                 }).ToList().ToDataTable();
                dt.TableName = "اکسل تاریخچه ورود و خروج";
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
            }
            TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "موردی یافت نشد" };
            return RedirectToAction("Index", new { cardId = cardId, driverId = driverId });
        }
    }
}
