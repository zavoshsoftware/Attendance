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
    public class CarStatusHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CarStatusHistories
        public ActionResult Index(Guid? id)
        {
            if (id.HasValue)
            {
                var carStatusHistories = db.CarStatusHistories.Include(c => c.Car).Where(c => c.IsDeleted == false && c.CarId == id).OrderByDescending(c => c.CreationDate);
                return View(carStatusHistories.ToList());
            }
            else
            {
            var carStatusHistories = db.CarStatusHistories.Include(c => c.Car).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(carStatusHistories.ToList()); 
             
            }
        }

        // GET: CarStatusHistories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarStatusHistory carStatusHistory = db.CarStatusHistories.Find(id);
            if (carStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(carStatusHistory);
        }

        // GET: CarStatusHistories/Create
        public ActionResult Create()
        {
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Title");
            return View();
        }

        // POST: CarStatusHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PreviousStatus,CurrentStatus,CarId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CarStatusHistory carStatusHistory)
        {
            if (ModelState.IsValid)
            {
				carStatusHistory.IsDeleted=false;
				carStatusHistory.CreationDate= DateTime.Now; 
					
                carStatusHistory.Id = Guid.NewGuid();
                db.CarStatusHistories.Add(carStatusHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarId = new SelectList(db.Cars, "Id", "Title", carStatusHistory.CarId);
            return View(carStatusHistory);
        }

        // GET: CarStatusHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarStatusHistory carStatusHistory = db.CarStatusHistories.Find(id);
            if (carStatusHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Title", carStatusHistory.CarId);
            return View(carStatusHistory);
        }

        // POST: CarStatusHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PreviousStatus,CurrentStatus,CarId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CarStatusHistory carStatusHistory)
        {
            if (ModelState.IsValid)
            {
				carStatusHistory.IsDeleted=false;
					carStatusHistory.LastModifiedDate=DateTime.Now;
                db.Entry(carStatusHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(db.Cars, "Id", "Title", carStatusHistory.CarId);
            return View(carStatusHistory);
        }

        // GET: CarStatusHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarStatusHistory carStatusHistory = db.CarStatusHistories.Find(id);
            if (carStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(carStatusHistory);
        }

        // POST: CarStatusHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CarStatusHistory carStatusHistory = db.CarStatusHistories.Find(id);
			carStatusHistory.IsDeleted=true;
			carStatusHistory.DeletionDate=DateTime.Now;
 
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
