using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Attendance.Core.Code;
using Attendance.Core.Data;
using Attendance.Models;
using Attendance.Models.Entities;
using Attendance.Web.Services.Base;
using Attendance.Web.Services.Errors;
using ClosedXML.Excel;
using ExcelDataReader;

namespace Attendance.Web.Controllers
{
    public class DriversController : Controller
    {
        private UnitOfWork unitOfWork;
        private Repository<Card> _card;
        private Repository<Driver> _driver;
        private DatabaseContext db = new DatabaseContext();
        private IErrorService _errors;
        public DriversController()
        {
            unitOfWork = new UnitOfWork(); 
            _errors = new ErrorService();
            _card = unitOfWork.Repository<Card>();
            _driver= unitOfWork.Repository<Driver>();
        }
        // GET: Drivers
        public ActionResult Index(bool? penaltyCheck=false)
        {
            if (penaltyCheck.Value)
            {
                var drivers = db.Penalties.Include(x => x.Card).Select(x => x.Card.Driver).Distinct().ToList();
                return View(drivers);
            }
            else
            {
                return View(db.Drivers.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
            }
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


        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost] 
        public ActionResult CreatePartial(Driver driver, string BirthDateShamsi)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    driver.IsDeleted = false;
                    driver.CreationDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(BirthDateShamsi))
                    {
                        driver.BirthDate = BirthDateShamsi.ToMiladi();
                    }
                    driver.Id = Guid.NewGuid();
                    if (!db.Drivers.Any(d => d.NationalCode.Trim() == driver.NationalCode.Trim()))
                    {
                        db.Drivers.Add(driver);
                        db.SaveChanges();
                        TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                return Json(new { status = true , data = driver , message = TempData["Toastr"]});
             
                    }
                    else
                    {
                        TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = $"کاربری با کدملی {driver.NationalCode} وجود دارد" };
                    }
                }

                return Json(new { status = true , data = driver , message = string.Join(". ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, data = driver, message = ex.Message});
            }
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
        public ActionResult Create(Driver driver, string BirthDateShamsi)
        {
            if (ModelState.IsValid)
            {
                driver.IsDeleted = false;
                driver.CreationDate = DateTime.Now;
                if (!string.IsNullOrEmpty(BirthDateShamsi))
                {
                    driver.BirthDate = BirthDateShamsi.ToMiladi();
                }
                driver.Id = Guid.NewGuid();
                if (!db.Drivers.Any(d => d.NationalCode.Trim() == driver.NationalCode.Trim()))
                {
                    db.Drivers.Add(driver);
                    db.SaveChanges();
                    TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                    return RedirectToAction("Index");
                }
                TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = $"کاربری با کدملی {driver.NationalCode} وجود دارد" };
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
        public ActionResult Edit(Driver driver, string BirthDateShamsi)
        {
            if (ModelState.IsValid)
            {
                var nationalCode = db.Drivers.AsNoTracking().FirstOrDefault(f => f.Id == driver.Id).NationalCode;
                driver.IsDeleted = false;
                driver.LastModifiedDate = DateTime.Now;
                driver.BirthDate = BirthDateShamsi.ToMiladi();
                if (!db.Drivers.Any(d => nationalCode != driver.NationalCode && d.NationalCode.Trim() == driver.NationalCode.Trim()))
                {
                    db.Entry(driver).State = EntityState.Modified;
                    db.SaveChanges(); TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
                    return RedirectToAction("Index");
                }
                TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = $"کاربری با کدملی {driver.NationalCode} وجود دارد" };
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

            try
            {    
                _driver.Delete(id);
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException e)
            {
                string errors = _errors.HandleError(e, Debugger.Info());
                //TempData["Toastr"] = new ToastrViewModel() { Class = "warning", Text = $"خطایی پیش آمده{errors}" }; 
            }
            finally
            {
                driver.NationalCode = "0000000000";
                driver.IsDeleted = true;
                driver.DeletionDate = DateTime.Now;
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
            }
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
            int row = 0;
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
                        if (row != 0)
                        {
                            DateTime? birthday = null;
                            var a = excelReader[0].ToString();
                            var a1 = excelReader[1].ToString();
                            var a2 = excelReader[2].ToString();
                            var a3 = excelReader[3].ToString();
                            var test = excelReader[4];
                            if ((excelReader[4].ToString()!="0"))
                                 birthday = excelReader[4].ToString().ToMiladi();
                            if (a3.Length != 10)
                            {
                                row++;
                                throw new Exception("کد ملی باید حتما 10 کاراکتر باشد. ردیف: " + row );
                            }

                            var obj = new Driver
                            {
                                Id = Guid.NewGuid(),
                                FirstName = excelReader[0].ToString(),
                                LastName = excelReader[1].ToString(),
                                CellNumber = excelReader[2].ToString(),
                                NationalCode = excelReader[3].ToString(),
                                BirthDate = birthday ,
                                CreationDate = DateTime.Now,
                                IsActive = true
                            };
                            var nationalCode = obj.NationalCode.ConvertDigit().Trim();
                            if (!db.Drivers.Any(d => d.NationalCode.Trim() == nationalCode))
                            {
                                list.Add(obj);
                            }
                        }

                        row++;
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
                TempData["error"] =row+"----"+ ex.Message;
                ViewBag.Message = ex;
                return RedirectToAction("import");
            }
        }




        public ActionResult Export()
        {
            var dt = db.Drivers.Include(d => d.Cards).Where(c => !c.IsDeleted).ToList().Select(c =>
                  new
                  {
                      FirstName = c.FirstName,
                      LastName = c.LastName,
                      FatherName = c.Father,
                      Mobile = c.CellNumber,
                      NationalCode = c.NationalCode,
                      IsActive = c.IsActive ? "فعال" : "غیرفعال",
                      CreateDate = c.CreationDate.ToShamsi('s'),
                      BirthDate = c.BirthDate.ToShamsi('a'),
                      Description = c.Description
                  }).ToList().ToDataTable();
            dt.TableName = "اکسل رانندگان"; 
            TempData["Toastr"] = new ToastrViewModel() { Class = "success", Text = "عملیات با موفقیت انجام شد" };
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
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            DriverStatusHistory status = new DriverStatusHistory()
            {
                DriverId = id,
                Driver = driver
            };
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status(DriverStatusHistory status)
        {
            var driver = db.Drivers.Find(status.DriverId);
            var current = new DriverStatusHistory()
            {
                Id = Guid.NewGuid(),
                Driver = driver,
                DriverId = status.DriverId,
                Description = status.Description,
                PreviousStatus = driver.IsActive,
                CurrentStatus = !driver.IsActive,
                IsActive = true,
                CreationDate = DateTime.Now
            };
            driver.IsActive = !driver.IsActive;
            driver.Description = status.Description;
            db.Entry(current).State = EntityState.Added;
            db.Entry(driver).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }


    }
}
