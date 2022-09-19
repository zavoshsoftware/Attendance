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
    public class CarTypeStatusHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CarTypeStatusHistories
        public ActionResult Index()
        {
            var carTypeStatusHistories = db.CarTypeStatusHistories.Include(c => c.CarType).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(carTypeStatusHistories.ToList());
        }

        // GET: CarTypeStatusHistories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarTypeStatusHistory carTypeStatusHistory = db.CarTypeStatusHistories.Find(id);
            if (carTypeStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(carTypeStatusHistory);
        }

        // GET: CarTypeStatusHistories/Create
        public ActionResult Create()
        {
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title");
            return View();
        }

        // POST: CarTypeStatusHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PreviousStatus,CurrentStatus,CarTypeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CarTypeStatusHistory carTypeStatusHistory)
        {
            if (ModelState.IsValid)
            {
				carTypeStatusHistory.IsDeleted=false;
				carTypeStatusHistory.CreationDate= DateTime.Now; 
					
                carTypeStatusHistory.Id = Guid.NewGuid();
                db.CarTypeStatusHistories.Add(carTypeStatusHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title", carTypeStatusHistory.CarTypeId);
            return View(carTypeStatusHistory);
        }

        // GET: CarTypeStatusHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarTypeStatusHistory carTypeStatusHistory = db.CarTypeStatusHistories.Find(id);
            if (carTypeStatusHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title", carTypeStatusHistory.CarTypeId);
            return View(carTypeStatusHistory);
        }

        // POST: CarTypeStatusHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PreviousStatus,CurrentStatus,CarTypeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CarTypeStatusHistory carTypeStatusHistory)
        {
            if (ModelState.IsValid)
            {
				carTypeStatusHistory.IsDeleted=false;
					carTypeStatusHistory.LastModifiedDate=DateTime.Now;
                db.Entry(carTypeStatusHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title", carTypeStatusHistory.CarTypeId);
            return View(carTypeStatusHistory);
        }

        // GET: CarTypeStatusHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarTypeStatusHistory carTypeStatusHistory = db.CarTypeStatusHistories.Find(id);
            if (carTypeStatusHistory == null)
            {
                return HttpNotFound();
            }
            return View(carTypeStatusHistory);
        }

        // POST: CarTypeStatusHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CarTypeStatusHistory carTypeStatusHistory = db.CarTypeStatusHistories.Find(id);
			carTypeStatusHistory.IsDeleted=true;
			carTypeStatusHistory.DeletionDate=DateTime.Now;
 
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
