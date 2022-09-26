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
using Attendance.Models;
using Attendance.Models.Entities;
using ClosedXML.Excel;

namespace Attendance.Web.Controllers
{
    public class CarTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CarTypes
        public ActionResult Index()
        {
            return View(db.CarTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CarTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // GET: CarTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Weight,Code,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CarType carType)
        {
            if (ModelState.IsValid)
            {
				carType.IsDeleted=false;
				carType.CreationDate= DateTime.Now; 
					
                carType.Id = Guid.NewGuid();
                db.CarTypes.Add(carType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carType);
        }

        // GET: CarTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // POST: CarTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Weight,Code,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] CarType carType)
        {
            if (ModelState.IsValid)
            {
				carType.IsDeleted=false;
					carType.LastModifiedDate=DateTime.Now;
                db.Entry(carType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carType);
        }

        // GET: CarTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // POST: CarTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CarType carType = db.CarTypes.Find(id);
			carType.IsDeleted=true;
			carType.DeletionDate=DateTime.Now;
 
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


        public ActionResult Export()
        {
            var dt = db.CarTypes.Include(c => c.Cars).Where(c => !c.IsDeleted).ToList().Select(c =>
                new {
                    Title = c.Title,
                    Brand = c.Brand,
                    Weight = c.Weight, 
                    IsActive = c.IsActive ? "فعال" : "غیرفعال",
                    CreateDate = c.CreationDate.ToShamsi('s'),
                    UpdateDate = c.CreationDate.ToShamsi('s'),
                    Description = c.Description
                }).ToList().ToDataTable();
            dt.TableName = "اکسل نوع خودرو";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Worksheets.FirstOrDefault().ColumnWidth = 20;
                using (MemoryStream stream = new MemoryStream())
                {
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{dt.TableName}{DateTime.Now.ToShamsi('e')}.xlsx");
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult Status(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            var status = new CarTypeStatusHistory()
            {
                CarTypeId = id,
                CarType = carType
            };
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status(CarTypeStatusHistory status)
        {
            var carType = db.CarTypes.Find(status.CarTypeId);
            var current = new CarTypeStatusHistory()
            {
                Id = Guid.NewGuid(),
                CarType = carType,
                CarTypeId = status.CarTypeId,
                Description = status.Description,
                PreviousStatus = carType.IsActive,
                CurrentStatus = !carType.IsActive,
                IsActive = true,
                CreationDate = DateTime.Now
            };
            carType.IsActive = !carType.IsActive;
            carType.Description = status.Description;
            db.Entry(current).State = EntityState.Added;
            db.Entry(carType).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }



    }
}
