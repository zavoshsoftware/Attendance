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
    public class CarsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Cars
        public ActionResult Index()
        {
            var cars = db.Cars.Include(c => c.CarType).Where(c=>c.IsDeleted==false).OrderByDescending(c=>c.CreationDate);
            return View(cars.ToList());
        }

        // GET: Cars/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CarTypeId,Title,Number,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Car car)
        {
            if (ModelState.IsValid)
            {
				car.IsDeleted=false;
				car.CreationDate= DateTime.Now;  
                car.Id = Guid.NewGuid();
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title", car.CarTypeId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title", car.CarTypeId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CarTypeId,Title,Number,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Car car)
        {
            if (ModelState.IsValid)
            {
				car.IsDeleted=false;
					car.LastModifiedDate=DateTime.Now;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "Id", "Title", car.CarTypeId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Car car = db.Cars.Find(id);
			car.IsDeleted=true;
			car.DeletionDate=DateTime.Now;
 
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
