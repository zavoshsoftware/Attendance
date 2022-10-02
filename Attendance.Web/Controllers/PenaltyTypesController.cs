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
    public class PenaltyTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: PenaltyTypes
        public ActionResult Index()
        {
            return View(db.PenaltyTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: PenaltyTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyType penaltyType = db.PenaltyTypes.Find(id);
            if (penaltyType == null)
            {
                return HttpNotFound();
            }
            return View(penaltyType);
        }

        // GET: PenaltyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PenaltyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PenaltyType penaltyType)
        {
            if (ModelState.IsValid)
            {
				penaltyType.IsDeleted=false;
				penaltyType.CreationDate= DateTime.Now; 
					
                penaltyType.Id = Guid.NewGuid();
                db.PenaltyTypes.Add(penaltyType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(penaltyType);
        }

        // GET: PenaltyTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyType penaltyType = db.PenaltyTypes.Find(id);
            if (penaltyType == null)
            {
                return HttpNotFound();
            }
            return View(penaltyType);
        }

        // POST: PenaltyTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] PenaltyType penaltyType)
        {
            if (ModelState.IsValid)
            {
				penaltyType.IsDeleted=false;
					penaltyType.LastModifiedDate=DateTime.Now;
                db.Entry(penaltyType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(penaltyType);
        }

        // GET: PenaltyTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PenaltyType penaltyType = db.PenaltyTypes.Find(id);
            if (penaltyType == null)
            {
                return HttpNotFound();
            }
            return View(penaltyType);
        }

        // POST: PenaltyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PenaltyType penaltyType = db.PenaltyTypes.Find(id);
			penaltyType.IsDeleted=true;
			penaltyType.DeletionDate=DateTime.Now;
 
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
