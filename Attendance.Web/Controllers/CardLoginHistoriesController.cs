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
    public class CardLoginHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CardLoginHistories
        public ActionResult Index(Guid? cardId,Guid? driverId)
        {
            if (cardId.HasValue)
            {
                var cardLoginHistories = db.CardLoginHistories.Where(c=>c.CardId == cardId).Include(c => c.Driver).Include(c => c.Card).Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreationDate);
                return View(cardLoginHistories.ToList());
            }
            else if (driverId.HasValue)
            {
                var cardLoginHistories = db.CardLoginHistories.Where(c => c.DriverId == driverId).Include(c => c.Driver).Include(c => c.Card).Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreationDate);
                return View(cardLoginHistories.ToList());
            }
            else
            { 
            var cardLoginHistories = db.CardLoginHistories.Include(c=>c.Driver).Include(c => c.Card).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(cardLoginHistories.ToList());
            }
        }

        public ActionResult IndexNotExit()
        {
            var cardLoginHistories = db.CardLoginHistories.Include(c => c.Driver).Include(c => c.Card)
                .Where(c => c.IsDeleted == false&&c.ExitDate==null).OrderByDescending(c => c.CreationDate);
            return View(cardLoginHistories.ToList());
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
				cardLoginHistory.IsDeleted=false;
				cardLoginHistory.CreationDate= DateTime.Now; 
					
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
				cardLoginHistory.IsDeleted=false;
					cardLoginHistory.LastModifiedDate=DateTime.Now;
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
			cardLoginHistory.IsDeleted=true;
			cardLoginHistory.DeletionDate=DateTime.Now;
 
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
