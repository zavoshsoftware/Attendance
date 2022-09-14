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
    public class PenaltyReasonsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: PenaltyReasons
        public ActionResult Index()
        {
            return View(db.PenaltyReason.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: PenaltyReasons/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyReason penaltyReason = db.PenaltyReason.Find(id);
            if (penaltyReason == null)
            {
                return HttpNotFound();
            }
            return View(penaltyReason);
        }

        // GET: PenaltyReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PenaltyReasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PenaltyReason penaltyReason)
        {
            if (ModelState.IsValid)
            {
				penaltyReason.IsDeleted=false;
				penaltyReason.CreationDate= DateTime.Now; 
					
                penaltyReason.Id = Guid.NewGuid();
                db.PenaltyReason.Add(penaltyReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(penaltyReason);
        }

        // GET: PenaltyReasons/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyReason penaltyReason = db.PenaltyReason.Find(id);
            if (penaltyReason == null)
            {
                return HttpNotFound();
            }
            return View(penaltyReason);
        }

        // POST: PenaltyReasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PenaltyReason penaltyReason)
        {
            if (ModelState.IsValid)
            {
				penaltyReason.IsDeleted=false;
					penaltyReason.LastModifiedDate=DateTime.Now;
                db.Entry(penaltyReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(penaltyReason);
        }

        // GET: PenaltyReasons/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyReason penaltyReason = db.PenaltyReason.Find(id);
            if (penaltyReason == null)
            {
                return HttpNotFound();
            }
            return View(penaltyReason);
        }

        // POST: PenaltyReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PenaltyReason penaltyReason = db.PenaltyReason.Find(id);
			penaltyReason.IsDeleted=true;
			penaltyReason.DeletionDate=DateTime.Now;
 
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
