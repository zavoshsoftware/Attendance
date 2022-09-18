using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Attendance.Models;
using Attendance.Models.Entities;
using ExcelDataReader;
using Attendance.Core.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

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
            ViewBag.Alphabet  = new SelectList(new List<string>() {
                "ب", "پ", "ت", "ث", "ج", "چ", "ح", "خ", "د", "ذ",
                "ر", "ز", "ژ", "س", "ش", "ص", "ض", "ط", "ظ",
                "ع", "غ", "ف", "ق", "ک", "گ", "ل", "م", "ن",
                "و", "ه", "ی"});


            
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
            ViewBag.Alphabet = new SelectList(new List<string>() {
                "ب", "پ", "ت", "ث", "ج", "چ", "ح", "خ", "د", "ذ",
                "ر", "ز", "ژ", "س", "ش", "ص", "ض", "ط", "ظ",
                "ع", "غ", "ف", "ق", "ک", "گ", "ل", "م", "ن",
                "و", "ه", "ی"});
            if (ModelState.IsValid)
            {
                if (!db.Cars.Any(c=>c.Number == car.Number.Trim()))
                {
                    car.IsDeleted = false;
                    car.CreationDate = DateTime.Now;
                    car.Id = Guid.NewGuid();
                    db.Cars.Add(car);
                    db.SaveChanges();
                TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "خودرویی با این پلاک در سیستم موجود است" };
                }
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
            ViewBag.Alphabet = new SelectList(new List<string>() {
                "ب", "پ", "ت", "ث", "ج", "چ", "ح", "خ", "د", "ذ",
                "ر", "ز", "ژ", "س", "ش", "ص", "ض", "ط", "ظ",
                "ع", "غ", "ف", "ق", "ک", "گ", "ل", "م", "ن",
                "و", "ه", "ی"});

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
            ViewBag.Alphabet = new SelectList(new List<string>() {
                "ب", "پ", "ت", "ث", "ج", "چ", "ح", "خ", "د", "ذ",
                "ر", "ز", "ژ", "س", "ش", "ص", "ض", "ط", "ظ",
                "ع", "غ", "ف", "ق", "ک", "گ", "ل", "م", "ن",
                "و", "ه", "ی"});
            if (ModelState.IsValid)
            {
                var pleck = db.Cars.AsNoTracking().FirstOrDefault(x => x.Id == car.Id)?.Number;
                if (!db.Cars.Any(c => c.Number == car.Number.Trim() && pleck != car.Number))
                {
                    car.IsDeleted = false;
                    car.LastModifiedDate = DateTime.Now;
                    db.Entry(car).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = "خودرویی با این پلاک در سیستم موجود است" };
                }
                
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

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult importexcel(HttpPostedFileBase upFile) 
        {
            try
            {
                if (upFile.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(upFile.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Files/Cars"), _FileName);
                    upFile.SaveAs(_path);
                    ViewBag.Message = "File Uploaded Successfully!!";

                    var list = new List<Car>();
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
                        var carTypeTitle = excelReader[0].ToString().ConvertDigit();
                        var carType = db.CarTypes.FirstOrDefault(c => c.Title.Trim() == carTypeTitle);
                        if (carType == null)
                        {
                            carType = db.CarTypes.FirstOrDefault();
                        }
                        
                        var car = new Car
                        {
                            Id = Guid.NewGuid(),
                            CarTypeId = carType.Id,
                            CarType=carType,
                            Title = excelReader[1].ToString().Trim().ConvertDigit(),
                            Number = excelReader[2].ToString().Trim().ConvertDigit(),  
                            CreationDate = DateTime.Now,
                            IsActive = true
                        };

                        var pleck = car.Number.ConvertDigit().Trim();
                        if (!db.Cars.Any(d => d.Number.Trim() == pleck))
                        {
                            list.Add(car);
                        }
                    }

                    excelReader.Close();
                    list.RemoveAt(0);
                    db.Cars.AddRange(list);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                return RedirectToAction("import");
            }
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
            var dt = db.Cars.Include(c=>c.CarType).Where(c=>!c.IsDeleted).ToList().Select(c => 
            new { 
                Car = c?.Title,
                Type = c?.CarType?.Title,
                Pleck = c?.Number,
                IsActive = c.IsActive?"فعال":"غیرفعال",
                Date = c?.CreationDate.ToShamsi('s') 
            }).ToList().ToDataTable();
            dt.TableName = "اکسل خودروها";
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",$"attachment; filename={dt.TableName}{DateTime.Now.ToShamsi('e')}.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            return RedirectToAction("Index");
        }


        public ActionResult Status(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            var status = new CarStatusHistory()
            {
                CarId = id,
                Car = car
            };
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status(CarStatusHistory status)
        {
            var car = db.Cars.Find(status.CarId);
            var current = new CarStatusHistory()
            {
                Id = Guid.NewGuid(),
                Car = car,
                CarId = status.CarId,
                Description = status.Description,
                PreviousStatus = car.IsActive,
                CurrentStatus = !car.IsActive,
                IsActive = true,
                CreationDate = DateTime.Now
            };
            car.IsActive = !car.IsActive;
            car.Description = status.Description;
            db.Entry(current).State = EntityState.Added;
            db.Entry(car).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }





    }
}
