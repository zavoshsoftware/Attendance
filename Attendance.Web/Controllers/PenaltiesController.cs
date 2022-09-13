﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Attendance.Core.Data;
using Attendance.Core.Entity;
using Attendance.Models;
using Attendance.Models.Entities;
using Helpers;

namespace Attendance.Web.Controllers
{
    public class PenaltiesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Penalties
        public ActionResult Index(Guid? driverId,Guid? cardId)
        {
            if (driverId.HasValue)
            { 
                var penalties = db.Penalties.Include(p => p.Card).Where(p => p.IsDeleted == false && !p.Solved && p.Card.DriverId == driverId).OrderByDescending(p => p.CreationDate);
                return View(penalties.ToList());
            }
            else if (cardId.HasValue)
            {
                var penalties = db.Penalties.Include(p => p.Card).Where(p => p.IsDeleted == false && !p.Solved && p.CardId == cardId).OrderByDescending(p => p.CreationDate);
                return View(penalties.ToList());
            }
            else
            {
                var penalties = db.Penalties.Include(p => p.Card).Where(p => p.IsDeleted == false && !p.Solved).OrderByDescending(p => p.CreationDate);
                return View(penalties.ToList());
            }
        }

        // GET: Penalties/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = db.Penalties.Find(id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // GET: Penalties/Create
        public ActionResult Create(Guid? cardId)
        {
            if (cardId.HasValue)
            {
                var card = db.Cards.Find(cardId);
                ViewBag.CardId = new SelectList(db.Cards.Where(c => !c.IsDeleted), "Id", "Code",card); 
                ViewBag.ReasonId = new SelectList(db.PenaltyReason.Where(c => !c.IsDeleted), "Id", "Title");
            }
            else
            {

            ViewBag.CardId = new SelectList(db.Cards.Where(c=>!c.IsDeleted), "Id", "Code");
            ViewBag.ReasonId = new SelectList(db.PenaltyReason.Where(c=>!c.IsDeleted), "Id", "Title");
            }
            return View(new Penalty() { });
        }

        // POST: Penalties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Reason,PenaltyType,CardId,Solved,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Penalty penalty)
        {
            if (ModelState.IsValid)
            {
				penalty.IsDeleted=false;
				penalty.CreationDate= DateTime.Now; 
					
                penalty.Id = Guid.NewGuid();
                db.Penalties.Add(penalty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardId = new SelectList(db.Cards.Where(c=>!c.IsDeleted && !c.IsHidden), "Id", "DisplayCode", penalty.CardId);
            ViewBag.ReasonId = new SelectList(db.PenaltyReason.Where(c => !c.IsDeleted), "Id", "Title");
            return View(penalty);
        }

        // GET: Penalties/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = db.Penalties.Find(id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", penalty.CardId);
            ViewBag.ReasonId = new SelectList(db.PenaltyReason.Where(c => !c.IsDeleted), "Id", "Title",penalty.ReasonId);
            return View(penalty);
        }

        // POST: Penalties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Reason,PenaltyType,CardId,Solved,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Penalty penalty)
        {
            if (ModelState.IsValid)
            {
				penalty.IsDeleted=false;
					penalty.LastModifiedDate=DateTime.Now;
                db.Entry(penalty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", penalty.CardId);
            ViewBag.ReasonId = new SelectList(db.PenaltyReason.Where(c => !c.IsDeleted), "Id", "Title", penalty.ReasonId);
            return View(penalty);
        }

        // GET: Penalties/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = db.Penalties.Find(id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // POST: Penalties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Penalty penalty = db.Penalties.Find(id);
			penalty.IsDeleted=true;
			penalty.DeletionDate=DateTime.Now;
 
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


        public ActionResult Cancel(Guid id)
        {
            var penalty = db.Penalties.Include(p => p.Card).FirstOrDefault(p => p.Id == id);
            penalty.Solved = true;
            db.SaveChanges();
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            return RedirectToAction("Index");
        }


        public ActionResult Export()
        {
            var dt = db.Penalties.Include(p=>p.Card).Where(c => !c.IsDeleted).ToList().Select(c =>
                new {
                    CardCode = c.Card.Code,
                    CardDisplayCode = c.Card.DisplayCode,
                    document = ((PenaltyType)c.PenaltyType).GetDisplayName(),
                    Reason = c.Reason, 
                    Solved = c.Solved?"رفع توقیق شده":"رفع توقیف نشده",
                    IsActive = c.IsActive ? "فعال" : "غیرفعال",
                    CreateDate = c.CreationDate.ToShamsi('s'),
                    UpdateDate = c.CreationDate.ToShamsi('s'),
                    Description = c.Description
                }).ToList().ToDataTable();
            dt.TableName = "اکسل توقیفات";
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment; filename={dt.TableName}{DateTime.Now.ToShamsi('e')}.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }

    }
}
