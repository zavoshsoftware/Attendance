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
using Attendance.Core.Enums;
using Attendance.Models;
using Attendance.Models.Entities;
using Attendance.Web.Services.Base;
using ClosedXML.Excel;

namespace Attendance.Web.Controllers
{
    public class UsersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork unitOfWork;
        private Repository<User> _userRepository;
        public UsersController()
        {
            unitOfWork = new UnitOfWork();
            _userRepository = unitOfWork.Repository<User>();
        }
        // GET: Users
        public ActionResult Index()
        { 
            if (IsSuperAdmin())
                return View(_userRepository.GetSorted(u=>u.CreationDate));
            return View(_userRepository.GetSorted( a=> a.SecurityRole != SecurityRole.SuperAdmin,u => u.CreationDate));

        }
        public bool IsSuperAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                if (role == SecurityRole.SuperAdmin)
                    return true;

                return false;
            }
            return false;

        }

        public List<SelectListItem> UserSecurityRoleEnums()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
                string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

                if (role == SecurityRole.SuperAdmin)
                    return new List<SelectListItem>()
                    {
                        new SelectListItem {Text = SecurityRole.Admin, Value = SecurityRole.Admin},
                        new SelectListItem {Text = SecurityRole.User, Value = SecurityRole.User},
                        new SelectListItem {Text = SecurityRole.SuperAdmin, Value = SecurityRole.SuperAdmin},
                        new SelectListItem {Text = SecurityRole.Monitoring, Value = SecurityRole.Monitoring},
                    };
                else
                    return new List<SelectListItem>()
                    {
                        new SelectListItem {Text = SecurityRole.Admin, Value = SecurityRole.Admin},
                        new SelectListItem {Text = SecurityRole.User, Value = SecurityRole.User},
                        new SelectListItem {Text = SecurityRole.Monitoring, Value = SecurityRole.Monitoring},
                    };
            }
            else
                return new List<SelectListItem>();
        }

        public ActionResult Create()
        { 
            ViewBag.SecurityRoleList = UserSecurityRoleEnums(); 
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.BirthDate = DateTime.Now;
                _userRepository.Insert(user);
                return RedirectToAction("Index");
            }
            ViewBag.SecurityRoleList = UserSecurityRoleEnums();

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecurityRoleList = UserSecurityRoleEnums();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            { 
                _userRepository.Update(user);
                return RedirectToAction("Index");
            }
            ViewBag.SecurityRoleList = UserSecurityRoleEnums();

            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userRepository.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        { 
            _userRepository.Delete(id);
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
            var dt = _userRepository.Get(c => c.SecurityRole != SecurityRole.SuperAdmin).ToList().Select(c =>
                new {
                    Title = c.FullName,
                    PhoneNumber = c.CellNum,
                    Email = c.Email,
                    Role = c.SecurityRole,  
                    IsActive = c.IsActive ? "فعال" : "غیرفعال",
                    CreateDate = c.CreationDate.ToShamsi('s'),
                    UpdateDate = c.CreationDate.ToShamsi('s'),
                    Description = c.Description
                }).ToList().ToDataTable();
            dt.TableName = "اکسل کاربران"; 
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.ColumnWidth = 400;
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{dt.TableName}{DateTime.Now.ToShamsi('e')}.xlsx");
                }
            }
            return RedirectToAction("Index");
        }



    }
}
