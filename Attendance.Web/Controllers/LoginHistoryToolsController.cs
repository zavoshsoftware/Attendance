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
    public class LoginHistoryToolsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: LoginHistoryTools
        public ActionResult Index()
        {
            var loginHistoryTools = db.LoginHistoryTools.Include(l => l.CardLoginHistory).Where(l=>l.IsDeleted==false).OrderByDescending(l=>l.CreationDate).Include(l => l.Tools).Where(l=>l.IsDeleted==false).OrderByDescending(l=>l.CreationDate).Include(l => l.Units).Where(l=>l.IsDeleted==false).OrderByDescending(l=>l.CreationDate);
            return View(loginHistoryTools.ToList());
        }

        // GET: LoginHistoryTools/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginHistoryTool loginHistoryTool = db.LoginHistoryTools.Find(id);
            if (loginHistoryTool == null)
            {
                return HttpNotFound();
            }
            return View(loginHistoryTool);
        }

        // GET: LoginHistoryTools/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.CardLoginHistoryId = new SelectList(db.CardLoginHistories.Where(x=>x.Id==id).ToList().Select(x=>new { Id = x.Id , Title = $"کارت:{x.Card.DisplayCode} - {x.LoginDate.ToShamsi('s')}"}), "Id", "Title",id);
            ViewBag.ToolId = new SelectList(db.Tools, "Id", "Title");
            ViewBag.UnitId = new SelectList(db.Units, "Id", "Title");
            ViewBag.LastLogins = db.LoginHistoryTools.Where(x => x.CardLoginHistoryId == id).Select(x=>new LoginToolViewModel (){ Tool = x.Tools.Title,Unit=x.Units.Title,Amount=x.Amount??0}).ToList();
            return PartialView();
        }

        // POST: LoginHistoryTools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CardLoginHistoryId,ToolId,UnitId,Amount,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] LoginHistoryTool loginHistoryTool)
        {
            if (ModelState.IsValid)
            {
				loginHistoryTool.IsDeleted=false;
				loginHistoryTool.CreationDate= DateTime.Now; 
					
                loginHistoryTool.Id = Guid.NewGuid();
                db.LoginHistoryTools.Add(loginHistoryTool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardLoginHistoryId = new SelectList(db.CardLoginHistories.ToList().Select(x => new { Id = x.Id, Title = $"کارت:{x.Card.DisplayCode} - {x.LoginDate.ToShamsi('s')}" }), "Id", "Title");
            ViewBag.ToolId = new SelectList(db.Tools, "Id", "Title", loginHistoryTool.ToolId);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "Title", loginHistoryTool.UnitId);
            return View(loginHistoryTool);
        }

        // GET: LoginHistoryTools/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginHistoryTool loginHistoryTool = db.LoginHistoryTools.Find(id);
            if (loginHistoryTool == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardLoginHistoryId = new SelectList(db.CardLoginHistories.ToList().Select(x => new { Id = x.Id, Title = $"کارت:{x.Card.DisplayCode} - {x.LoginDate.ToShamsi('s')}" }), "Id", "Title", loginHistoryTool.CardLoginHistoryId);
            ViewBag.ToolId = new SelectList(db.Tools, "Id", "Title", loginHistoryTool.ToolId);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "Title", loginHistoryTool.UnitId);
            return View(loginHistoryTool);
        }

        // POST: LoginHistoryTools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CardLoginHistoryId,ToolId,UnitId,Amount,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] LoginHistoryTool loginHistoryTool)
        {
            if (ModelState.IsValid)
            {
				loginHistoryTool.IsDeleted=false;
					loginHistoryTool.LastModifiedDate=DateTime.Now;
                db.Entry(loginHistoryTool).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardLoginHistoryId = new SelectList(db.CardLoginHistories, "Id", "DriverName", loginHistoryTool.CardLoginHistoryId);
            ViewBag.ToolId = new SelectList(db.Tools, "Id", "Title", loginHistoryTool.ToolId);
            ViewBag.UnitId = new SelectList(db.Units, "Id", "Title", loginHistoryTool.UnitId);
            return View(loginHistoryTool);
        }

        // GET: LoginHistoryTools/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginHistoryTool loginHistoryTool = db.LoginHistoryTools.Find(id);
            if (loginHistoryTool == null)
            {
                return HttpNotFound();
            }
            return View(loginHistoryTool);
        }

        // POST: LoginHistoryTools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            LoginHistoryTool loginHistoryTool = db.LoginHistoryTools.Find(id);
			loginHistoryTool.IsDeleted=true;
			loginHistoryTool.DeletionDate=DateTime.Now;
 
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
