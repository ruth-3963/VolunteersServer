using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BL;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BL.ScheduleFunction.RunPrepareDaily(DateTime.Now);
//           BL.ScheduleFunction.RunPrepareDaily(new DateTime(DateTime.Today.Year,
//DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0));
        }
    }
}
