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
    public class ExitToolsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ExitTools
        public ActionResult Index()
        {
            return View(db.Tools.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ExitTools/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tools exitTools = db.Tools.Find(id);
            if (exitTools == null)
            {
                return HttpNotFound();
            }
            return View(exitTools);
        }

        // GET: ExitTools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExitTools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Order,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Tools exitTools)
        {
            if (ModelState.IsValid)
            {
				exitTools.IsDeleted=false;
				exitTools.CreationDate= DateTime.Now; 
					
                exitTools.Id = Guid.NewGuid();
                db.Tools.Add(exitTools);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exitTools);
        }

        // GET: ExitTools/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tools exitTools = db.Tools.Find(id);
            if (exitTools == null)
            {
                return HttpNotFound();
            }
            return View(exitTools);
        }

        // POST: ExitTools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Order,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Tools exitTools)
        {
            if (ModelState.IsValid)
            {
				exitTools.IsDeleted=false;
					exitTools.LastModifiedDate=DateTime.Now;
                db.Entry(exitTools).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exitTools);
        }

        // GET: ExitTools/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tools exitTools = db.Tools.Find(id);
            if (exitTools == null)
            {
                return HttpNotFound();
            }
            return View(exitTools);
        }

        // POST: ExitTools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Tools exitTools = db.Tools.Find(id);
			exitTools.IsDeleted=true;
			exitTools.DeletionDate=DateTime.Now;
 
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
