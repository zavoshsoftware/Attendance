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
        public ActionResult Index(Guid? id)
        {
            ViewBag.CardLoginHistoryId = id;
            if (id.HasValue)
            {
                var cardLoginHistory = db.CardLoginHistories.Find(id);
                if (cardLoginHistory != null)
                    ViewBag.Title = "تاریخچه ورود و خروج کارت شماره " + cardLoginHistory.Card.DisplayCode + " در تاریخ ورود" +
                                    cardLoginHistory.LoginDate.ToShamsi('s');
                var walkingLoginHistories = db.WalkingLoginHistories.
                   Where(w => w.IsDeleted == false && w.CardLoginHistoryId == id).OrderByDescending(w => w.CreationDate);
                return View(walkingLoginHistories.ToList());
            }
            else
            {
                ViewBag.Title = "تاریخچه ورود و خروج کارت  ";
                var walkingLoginHistories = db.WalkingLoginHistories.Include(x=>x.CardLoginHistory).
                   Where(w => w.IsDeleted == false).OrderByDescending(w => w.CreationDate);
                return View(walkingLoginHistories.ToList());
            }
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.CardLoginHistoryId = id;
            return View();
        }

        // POST: WalkingLoginHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkingLoginHistory walkingLoginHistory, Guid id)
        {
            if (ModelState.IsValid)
            {
                walkingLoginHistory.IsDeleted = false;
                walkingLoginHistory.CreationDate = DateTime.Now;
                walkingLoginHistory.CardLoginHistoryId = id;
                walkingLoginHistory.Id = Guid.NewGuid();

                db.WalkingLoginHistories.Add(walkingLoginHistory);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = walkingLoginHistory.CardLoginHistoryId });
            }

            ViewBag.CardLoginHistoryId = id;
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
            ViewBag.CardLoginHistoryId = walkingLoginHistory.CardLoginHistoryId;
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

                return RedirectToAction("Index", new { id = walkingLoginHistory.CardLoginHistoryId });
            }
            ViewBag.CardLoginHistoryId = walkingLoginHistory.CardLoginHistoryId;
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
            ViewBag.CardLoginHistoryId = walkingLoginHistory.CardLoginHistoryId;
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
            return RedirectToAction("Index", new { id = walkingLoginHistory.CardLoginHistoryId });
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
