using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using WebSurveying2017.App_Start;
using WebSurveying2017.Mapper;

namespace WebSurveying2017
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

           
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IoCConfigurator.ConfigureUnityContainer();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfiguration.Configure();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling = ReferenceLoopHandling.Ignore;

            

        }
    }
}
