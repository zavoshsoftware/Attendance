using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Attendance.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(new System.Web.Optimization.BundleCollection(){});
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register); 
            InitConfig.RegisterData(); 
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); 
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
