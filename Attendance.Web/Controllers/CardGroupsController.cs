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
    public class CardGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CardGroups
        public ActionResult Index()
        {
            return View(db.Groups.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CardGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroup cardGroup = db.Groups.Find(id);
            if (cardGroup == null)
            {
                return HttpNotFound();
            }
            return View(cardGroup);
        }

        // GET: CardGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CardGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardGroup cardGroup)
        {
            if (ModelState.IsValid)
            {
				cardGroup.IsDeleted=false;
				cardGroup.CreationDate= DateTime.Now; 
					
                cardGroup.Id = Guid.NewGuid();
                db.Groups.Add(cardGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cardGroup);
        }

        // GET: CardGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroup cardGroup = db.Groups.Find(id);
            if (cardGroup == null)
            {
                return HttpNotFound();
            }
            return View(cardGroup);
        }

        // POST: CardGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CardGroup cardGroup)
        {
            if (ModelState.IsValid)
            {
				cardGroup.IsDeleted=false;
					cardGroup.LastModifiedDate=DateTime.Now;
                db.Entry(cardGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cardGroup);
        }

        // GET: CardGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardGroup cardGroup = db.Groups.Find(id);
            if (cardGroup == null)
            {
                return HttpNotFound();
            }
            return View(cardGroup);
        }

        // POST: CardGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CardGroup cardGroup = db.Groups.Find(id);
			cardGroup.IsDeleted=true;
			cardGroup.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CardGroupsItems()
        {
            return View();
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
