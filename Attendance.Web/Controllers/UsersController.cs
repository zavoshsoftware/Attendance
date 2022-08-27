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

namespace Attendance.Web.Controllers
{
    public class UsersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Users
        public ActionResult Index()
        {
            if (IsSuperAdmin())
                return View(db.Users.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
            return View(db.Users.Where(a => a.IsDeleted == false && a.SecurityRole != SecurityRole.SuperAdmin).OrderByDescending(a => a.CreationDate).ToList());

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
                        new SelectListItem {Text = SecurityRole.SuperAdmin, Value = SecurityRole.SuperAdmin}
                    };
                else
                    return new List<SelectListItem>()
                    {
                        new SelectListItem {Text = SecurityRole.Admin, Value = SecurityRole.Admin},
                        new SelectListItem {Text = SecurityRole.User, Value = SecurityRole.User}
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
                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;

                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
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
                user.IsDeleted = false;
                user.LastModifiedDate = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
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
            User user = db.Users.Find(id);
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
            User user = db.Users.Find(id);
            user.IsDeleted = true;
            user.DeletionDate = DateTime.Now;

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
            dt.TableName = "اکسل کاربران";
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment; filename={dt.TableName}{DateTime.Now.ToString("F")}.xls");
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
