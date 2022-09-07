﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Attendance.Core.Data;
using Attendance.Models;
using Attendance.Models.Entities;
using ExcelDataReader;

namespace Attendance.Web.Controllers
{
    public class DriversController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Drivers
        public ActionResult Index()
        {
            return View(db.Drivers.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

        // GET: Drivers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Driver driver)
        {
            if (ModelState.IsValid)
            {
                driver.IsDeleted = false;
                driver.CreationDate = DateTime.Now;

                driver.Id = Guid.NewGuid();
                db.Drivers.Add(driver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        // GET: Drivers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Driver driver)
        {
            if (ModelState.IsValid)
            {
                driver.IsDeleted = false;
                driver.LastModifiedDate = DateTime.Now;
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Driver driver = db.Drivers.Find(id);
            driver.IsDeleted = true;
            driver.DeletionDate = DateTime.Now;

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

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase upFile)
        {
            try
            {
                if (upFile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(upFile.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Files/Drivers"), _FileName);
                    upFile.SaveAs(_path);
                    ViewBag.Message = "File Uploaded Successfully!!";

                    var list = new List<Driver>();
                    FileStream stream = System.IO.File.Open(_path, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = null;
                    if (upFile.FileName.Split('.')[1] == "xls")
                    {
                        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    while (excelReader.Read())
                    {
                        var obj = new Driver
                        {
                            Id = Guid.NewGuid(),
                            FirstName = excelReader[0].ToString(),
                            LastName = excelReader[1].ToString(),
                            CellNumber = excelReader[2].ToString(),
                            NationalCode = excelReader[3].ToString(),
                            CreationDate = DateTime.Now,
                            IsActive = true
                        };

                        if (!db.Drivers.Any(d=>d.NationalCode.Trim() == obj.NationalCode.ConvertDigit().Trim()))
                        {
                            list.Add(obj);
                        }
                    }

                    excelReader.Close();
                    list.RemoveAt(0);
                    db.Drivers.AddRange(list);
                    db.SaveChanges();
                }
                TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Toastr"] = new ToastrViewModel() { Class = "danger", Text = ex.Message };
                ViewBag.Message = ex;
                return RedirectToAction("import");
            }
        }




        public ActionResult Export()
        {
            var dt = db.Drivers.Include(d=>d.Cards).Where(c => !c.IsDeleted).ToList().Select(c =>
                new {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    FatherName = c.Father, 
                    Mobile = c.CellNumber,
                    NationalCode = c.NationalCode,
                    IsActive = c.IsActive ? "فعال" : "غیرفعال",
                    CreateDate = c.CreationDate.ToShamsi('s'),
                    UpdateDate = c.CreationDate.ToShamsi('s'),
                    Description = c.Description
                }).ToList().ToDataTable();
            dt.TableName = "اکسل رانندگان";
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();
             
            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename={dt.TableName}{DateTime.Now.ToShamsi('e')}.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);

            grid.RenderControl(hw);

            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            Response.Write(sw.ToString());
            Response.End();

          
            return RedirectToAction("Index");
        }


    }
}
