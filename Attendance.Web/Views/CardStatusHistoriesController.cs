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

namespace Attendance.Web.Views
{
    public class CardStatusHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CardStatusHistories
        public ActionResult Index()
        {
            var cardStatusHistories = db.CardStatusHistories.Include(c => c.Card)
                .Where(c=>c.IsDeleted==false && !c.Card.IsHidden).OrderByDescending(c=>c.CreationDate);
            return View(cardStatusHistories.ToList());
        }

        // GET: CardStatusHistories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardStatusHistory cardStatusHistory = db.CardStatusHistories.Find(id);
            if (cardStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(cardStatusHistory);
        }

        // GET: CardStatusHistories/Create
        public ActionResult Create()
        {
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code");
            return View();
        }

        // POST: CardStatusHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PreviousStatus,CurrentStatus,CardId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardStatusHistory cardStatusHistory)
        {
            if (ModelState.IsValid)
            {
				cardStatusHistory.IsDeleted=false;
				cardStatusHistory.CreationDate= DateTime.Now; 
					
                cardStatusHistory.Id = Guid.NewGuid();
                db.CardStatusHistories.Add(cardStatusHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", cardStatusHistory.CardId);
            return View(cardStatusHistory);
        }

        // GET: CardStatusHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardStatusHistory cardStatusHistory = db.CardStatusHistories.Find(id);
            if (cardStatusHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", cardStatusHistory.CardId);
            return View(cardStatusHistory);
        }

        // POST: CardStatusHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PreviousStatus,CurrentStatus,CardId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardStatusHistory cardStatusHistory)
        {
            if (ModelState.IsValid)
            {
				cardStatusHistory.IsDeleted=false;
					cardStatusHistory.LastModifiedDate=DateTime.Now;
                db.Entry(cardStatusHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", cardStatusHistory.CardId);
            return View(cardStatusHistory);
        }

        // GET: CardStatusHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardStatusHistory cardStatusHistory = db.CardStatusHistories.Find(id);
            if (cardStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(cardStatusHistory);
        }

        // POST: CardStatusHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CardStatusHistory cardStatusHistory = db.CardStatusHistories.Find(id);
			cardStatusHistory.IsDeleted=true;
			cardStatusHistory.DeletionDate=DateTime.Now;
 
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
    }
}
