using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Attendance.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate.js", "~/Scripts/jquery.validate.unobtrusive.js")); 
        // JQuery validator.
        //bundles.Add(new ScriptBundle("~/bundles/custom-validator").Include("~/Scripts/script-custom-validator.js"));
            BundleTable.EnableOptimizations = true;
        }
    }
}