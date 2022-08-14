﻿using System;
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
            var cards = db.Cards.Include(c => c.Driver).Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreationDate);
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
                    var day= (int)card.Day;
                    card.DisplayCode = $"{day}-{driver.NationalCode.Substring(3)}";
                }

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
            ViewBag.plecks = db.Cars.Select(c => new Select2Model{ id = c.Id.ToString(), text = c.Number }).ToList();
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
            var type = db.Cars.Include(x => x.CarType).FirstOrDefault(x => x.Id == carId)?.CarType ?? default;
            return Json(new AuthenticateFormViewModel()
            {
                Type = type.Title,
                Weight = type.Weight
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
            var driver = db.Drivers.FirstOrDefault(d => d.NationalCode.Trim() == model.DriverNatCode.Trim());
            if (driver == null)
            {
                //create exist
                driver = new Driver()
                {
                    Id = Guid.NewGuid(),
                    FirstName = model.DriverFirstName,
                    LastName = model.DriverLastName,
                    NationalCode = model.DriverNatCode
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


            cardLoginHistory.AssistanceLastName = model.AssistanceLastName;
            cardLoginHistory.AssistanceName = model.AssistanceName;
            cardLoginHistory.AssistanceNationalCode = model.AssistanceNationalCode;
            cardLoginHistory.LoginDate = DateTime.Now;
            cardLoginHistory.IsActive = true;
            cardLoginHistory.CreationDate = DateTime.Now;
            cardLoginHistory.LoginDate = DateTime.Now;
            cardLoginHistory.IsActive = true;
            db.CardLoginHistories.Add(cardLoginHistory); 
            db.SaveChanges();
            return Redirect("/cards");
        }

        public JsonResult GetPleckList(string q)
        {
            if (q==null)
            {
                q = string.Empty;
            }
            var result = db.Cars.Where(c => c.Number.Contains(q)).Select(c => new { Id = c.Id, Text = c.Number }).ToList();
            return Json(new { items = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoginHistoryDetials(Guid id)
        {
            var login = db.CardLoginHistories.Include(x => x.Car).Include(x => x.Driver).Include(x => x.Card)
                .FirstOrDefault(x => x.Id == id);
            return View(login);
        }

    }
}
