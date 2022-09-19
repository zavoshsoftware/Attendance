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
    public class DriverStatusHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: DriverStatusHistories
        public ActionResult Index()
        {
            var driverStatusHistories = db.DriverStatusHistories.Include(d => d.Driver).Where(d=>d.IsDeleted==false).OrderByDescending(d=>d.CreationDate);
            return View(driverStatusHistories.ToList());
        }

        // GET: DriverStatusHistories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverStatusHistory driverStatusHistory = db.DriverStatusHistories.Find(id);
            if (driverStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(driverStatusHistory);
        }

        // GET: DriverStatusHistories/Create
        public ActionResult Create()
        {
            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FirstName");
            return View();
        }

        // POST: DriverStatusHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PreviousStatus,CurrentStatus,DriverId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] DriverStatusHistory driverStatusHistory)
        {
            if (ModelState.IsValid)
            {
				driverStatusHistory.IsDeleted=false;
				driverStatusHistory.CreationDate= DateTime.Now; 
					
                driverStatusHistory.Id = Guid.NewGuid();
                db.DriverStatusHistories.Add(driverStatusHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FirstName", driverStatusHistory.DriverId);
            return View(driverStatusHistory);
        }

        // GET: DriverStatusHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverStatusHistory driverStatusHistory = db.DriverStatusHistories.Find(id);
            if (driverStatusHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FirstName", driverStatusHistory.DriverId);
            return View(driverStatusHistory);
        }

        // POST: DriverStatusHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PreviousStatus,CurrentStatus,DriverId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] DriverStatusHistory driverStatusHistory)
        {
            if (ModelState.IsValid)
            {
				driverStatusHistory.IsDeleted=false;
					driverStatusHistory.LastModifiedDate=DateTime.Now;
                db.Entry(driverStatusHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DriverId = new SelectList(db.Drivers, "Id", "FirstName", driverStatusHistory.DriverId);
            return View(driverStatusHistory);
        }

        // GET: DriverStatusHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverStatusHistory driverStatusHistory = db.DriverStatusHistories.Find(id);
            if (driverStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(driverStatusHistory);
        }

        // POST: DriverStatusHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DriverStatusHistory driverStatusHistory = db.DriverStatusHistories.Find(id);
			driverStatusHistory.IsDeleted=true;
			driverStatusHistory.DeletionDate=DateTime.Now;
 
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
