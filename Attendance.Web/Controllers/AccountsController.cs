using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Attendance.Models;
using Attendance.Models.Entities;
using ViewModels;

namespace Attendance.Web.Controllers
{
    public class AccountsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();


        private ActionResult RedirectToLocal(string returnUrl, string role)
        {

            if (role.ToLower().Contains("admin"))
                return RedirectToAction("index", "Drivers");

            return RedirectToAction("Authenticate", "Cards");

            return Redirect("/");
        }
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
            return Redirect("/");
        }




        [Route("login")]
        public ActionResult Login(string ReturnUrl = "")
        {

            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;

            LoginPageViewModel login = new LoginPageViewModel()
            {
                //MenuGalleryGroups = baseViewModelHelper.GetMenuGalleryGroups(),
                //MenuItem = baseViewModelHelper.GetMenuItems(),
            };
            return View(login);

        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginPageViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User oUser = db.Users.FirstOrDefault(a => a.CellNum == model.Username && a.Password == model.Password);

                if (oUser != null)
                {
                    var ident = new ClaimsIdentity(
                      new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
              new Claim(ClaimTypes.Name,oUser.Id.ToString()),

              // optionally you could add roles if any
               new Claim(ClaimTypes.Role, oUser.SecurityRole),

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = true }, ident);
                    return RedirectToLocal(returnUrl, oUser.SecurityRole); // auth succeed 

                }
                else
                {
                    // invalid username or password
                    TempData["WrongPass"] = "نام کاربری و یا کلمه عبور وارد شده صحیح نمی باشد.";

                }
            }


            return View(model);
        }


    }
}