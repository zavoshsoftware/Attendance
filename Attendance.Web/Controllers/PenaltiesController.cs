using System;
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
using RestSharp;

namespace Attendance.Web.Controllers
{
    public class PenaltiesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Penalties
        public ActionResult Index(Guid? driverId,Guid? cardId,bool solved = false)
        {  
            if (driverId.HasValue)
            { 
                var penalties = db.Penalties.Include(p => p.Card).Include(x=>x.Penalty_PenaltyTypes).Where(p => p.IsDeleted == false && p.Solved == solved && p.Card.DriverId == driverId).OrderByDescending(p => p.CreationDate);
                return View(penalties.ToList());
            }
            else if (cardId.HasValue)
            {
                var penalties = db.Penalties.Include(p => p.Card).Include(x => x.Penalty_PenaltyTypes).Where(p => p.IsDeleted == false && p.Solved == solved && p.CardId == cardId).OrderByDescending(p => p.CreationDate);
                return View(penalties.ToList());
            }
            else
            {
                var penalties = db.Penalties.Include(p => p.Card).Include(x => x.Penalty_PenaltyTypes).Where(p => p.IsDeleted == false && p.Solved == solved).OrderByDescending(p => p.CreationDate);
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
            ViewBag.PenaltyTypes = db.PenaltyTypes.Where(x=>!x.IsDeleted && x.IsActive).ToList();
            return View(new Penalty() { });
        }

        // POST: Penalties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Reason,PenaltyType,CardId,Solved,IsActive,CreationDate," +
            "LastModifiedDate,IsDeleted,DeletionDate,Description")] Penalty penalty,string penaltyTypeId)
        {
            var penaltyTypeIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(penaltyTypeId);

            if (ModelState.IsValid)
            { 
				penalty.IsDeleted=false;
				penalty.CreationDate= DateTime.Now; 
					
                penalty.Id = Guid.NewGuid();
                db.Penalties.Add(penalty);
                db.SaveChanges();
                foreach (var item in penaltyTypeIds)
                {
                    var penaltyType = db.PenaltyTypes.Where(x => !x.IsDeleted && x.IsActive).FirstOrDefault(x=>x.Id==item);
                    db.Penalty_PenaltyTypes.Add(new Penalty_PenaltyType()
                    {
                        CreationDate=DateTime.Now,
                        Id =Guid.NewGuid(),
                        IsActive = true,
                        PenaltyId = penalty.Id,
                        Penalty = penalty,
                        IsDeleted = false,
                        PenaltyType = penaltyType,
                        PenaltyTypeId = item
                    });
                }
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
            Penalty penalty = db.Penalties.Include(x=>x.Penalty_PenaltyTypes).Include(x=>x.Card)
                .FirstOrDefault(x=>x.Id==id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardId = new SelectList(db.Cards, "Id", "Code", penalty.CardId);
            ViewBag.ReasonId = new SelectList(db.PenaltyReason.Where(c => !c.IsDeleted), "Id", "Title",penalty.ReasonId);
            ViewBag.SelectedPenaltyTypes = penalty.Penalty_PenaltyTypes.Where(x => !x.IsDeleted && x.IsActive).Select(x=>x.PenaltyType).Distinct().ToList();
            ViewBag.PenaltyTypes = db.PenaltyTypes.Where(x => !x.IsDeleted && x.IsActive).ToList();
            return View(penalty);
        }

        // POST: Penalties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Penalty penalty, string penaltyTypeId)
        {
            var penaltyTypeIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(penaltyTypeId);

            if (ModelState.IsValid)
            {
				penalty.IsDeleted=false;
					penalty.LastModifiedDate=DateTime.Now;
                db.Entry(penalty).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var item in penaltyTypeIds)
                {
                    var lastPenalties = db.Penalty_PenaltyTypes.Where(x => x.PenaltyId == penalty.Id).ToList();
                    db.Penalty_PenaltyTypes.RemoveRange(lastPenalties);
                    var penaltyType = db.PenaltyTypes.Find(item);
                    db.Penalty_PenaltyTypes.Add(new Penalty_PenaltyType()
                    {
                        CreationDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        PenaltyId = penalty.Id,
                        Penalty = penalty,
                        IsDeleted = false,
                        PenaltyType = penaltyType,
                        PenaltyTypeId = item
                    });
                }
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
