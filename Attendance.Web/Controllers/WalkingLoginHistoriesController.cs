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
    public class WalkingLoginHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: WalkingLoginHistories
        public ActionResult Index(Guid? cardId)
        {
            ViewBag.cardId = cardId;
            if (cardId.HasValue)
            {
                var card = db.Cards.Find(cardId);
                if (card != null)
                    ViewBag.Title = "تاریخچه ورود و خروج کارت شماره " + card.DisplayCode;
                var walkingLoginHistories = db.WalkingLoginHistories.
                   Where(w => w.IsDeleted == false && w.CardId == cardId).OrderByDescending(w => w.CreationDate);
                return View(walkingLoginHistories.ToList());
            } 
            else
            {
                ViewBag.Title = "تاریخچه ورود و خروج کارت  ";
                var walkingLoginHistories = db.WalkingLoginHistories.Include(x=>x.Card).
                   Where(w => w.IsDeleted == false).OrderByDescending(w => w.CreationDate);
                return View(walkingLoginHistories.ToList());
            }
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.cardId = id;
            return View(new WalkingLoginHistory() { 
            CardId = id
            });
        }

        // POST: WalkingLoginHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkingLoginHistory walkingLoginHistory)
        {
            if (ModelState.IsValid)
            {
                var card= db.Cards.Find(walkingLoginHistory.CardId);
                walkingLoginHistory.IsDeleted = false;
                walkingLoginHistory.CreationDate = DateTime.Now;
                walkingLoginHistory.CardId = walkingLoginHistory.CardId;
                walkingLoginHistory.Id = Guid.NewGuid();
                walkingLoginHistory.Card = card;

                db.WalkingLoginHistories.Add(walkingLoginHistory);
                db.SaveChanges();
                return RedirectToAction("Index", new { cardId = walkingLoginHistory.CardId });
            }

            ViewBag.cardId = walkingLoginHistory.CardId;
            return View(walkingLoginHistory);
        }

        // GET: WalkingLoginHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WalkingLoginHistory walkingLoginHistory = db.WalkingLoginHistories.Find(id);
            if (walkingLoginHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.cardId = walkingLoginHistory.CardId;
            return View(walkingLoginHistory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WalkingLoginHistory walkingLoginHistory)
        {
            if (ModelState.IsValid)
            {
                walkingLoginHistory.IsDeleted = false;
                walkingLoginHistory.LastModifiedDate = DateTime.Now;
                db.Entry(walkingLoginHistory).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { cardId = walkingLoginHistory.CardId });
            }
            ViewBag.cardId = walkingLoginHistory.CardId;
            return View(walkingLoginHistory);
        }

        // GET: WalkingLoginHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WalkingLoginHistory walkingLoginHistory = db.WalkingLoginHistories.Find(id);
            if (walkingLoginHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.cardId = walkingLoginHistory.CardId;
            return View(walkingLoginHistory);
        }

        // POST: WalkingLoginHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            WalkingLoginHistory walkingLoginHistory = db.WalkingLoginHistories.Find(id);
            walkingLoginHistory.IsDeleted = true;
            walkingLoginHistory.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { cardId = walkingLoginHistory.CardId });
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
