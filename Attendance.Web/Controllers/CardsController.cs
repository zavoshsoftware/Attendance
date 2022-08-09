using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
            var cards = db.Cards.Include(c => c.Driver).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(cards.ToList());
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
            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FullName");
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,DriverId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description,Day")] Card card)
        { 
            if (ModelState.IsValid)
            { 
				card.IsDeleted=false;
				card.CreationDate= DateTime.Now;
                card.DisplayCode = $"{card.Day}-{card.Driver.NationalCode}";
                card.Id = Guid.NewGuid();
                db.Cards.Add(card);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FullName", card.DriverId);
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
            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FullName", card.DriverId);
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,DriverId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description,Day")] Card card)
        {
            Type day = typeof(Core.Enums.Enums.DayOfWeek);
            if (ModelState.IsValid)
            {
				card.IsDeleted=false;
					card.LastModifiedDate=DateTime.Now;
                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FullName", card.DriverId);
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
			card.IsDeleted=true;
			card.DeletionDate=DateTime.Now;
 
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
            ViewBag.plecks = db.Cars.Where(c => c.IsDeleted == false && c.IsActive).Select(x=>x.Number).ToList();
            var card = db.Cards.Include(x=>x.Driver).FirstOrDefault(x=>x.Id==id); 
            return PartialView(
                new AuthenticateFormViewModel()
                {
                    Driver = card?.Driver??default, 
                    //Car = card?.Driver?.
                    Car = card?.CardLoginHistories?.FirstOrDefault()?.Car,
                    Card = card,
                    cardId = card.Id,
                    DriverFullName=card.Driver.FullName,
                    DriverNatCode = card.Driver.NationalCode
                }
                );

        }  

        [HttpPost]
        public JsonResult GetCarType(string q)
        {
            var type = db.Cars.Include(x=>x.CarType).FirstOrDefault(x => x.Number.Trim() == q.Trim())?.CarType ?? default;
            return Json(new AuthenticateFormViewModel() { 
            Type=type.Title,
            Weight=type.Weight
            });
        }

        [HttpPost]
        public ActionResult SubmitForm(AuthenticateFormViewModel authenticateFormViewModel)
        {
            var card =  db.Cards.Include(s=>s.CardLoginHistories).Include(s=>s.Driver).FirstOrDefault(x => x.Id == authenticateFormViewModel.cardId);
            var cardLoginHist = card.CardLoginHistories.LastOrDefault(x => x.CardId == card.Id);
            card.Driver.FullName = authenticateFormViewModel.DriverFullName;
            card.Driver.NationalCode = authenticateFormViewModel.DriverNatCode;

            cardLoginHist.AssistanceLastName = authenticateFormViewModel.AssistanceLastName;
            cardLoginHist.AssistanceName = authenticateFormViewModel.AssistanceName;
            cardLoginHist.AssistanceNationalCode = authenticateFormViewModel.AssistanceNationalCode;

            //db.Cars.Where(x=>x.Id==cardLoginHist.CarId).FirstOrDefault().Number = authenticateFormViewModel.Pleck; 
            //var carType = db.Cars.Include(s => s.CarType).FirstOrDefault(x => x.Id == cardLoginHist.CarId).CarType;
            //carType.Title = authenticateFormViewModel.Type;
            //carType.Weight = authenticateFormViewModel.Weight;
            db.SaveChanges();
            return Redirect("/cards");
        }
    }
}
