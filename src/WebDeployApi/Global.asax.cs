using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebDeployApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var appData = HostingEnvironment.MapPath($"~/App_Data");
            if (!System.IO.Directory.Exists(appData))
                System.IO.Directory.CreateDirectory(appData);
            var tempData = HostingEnvironment.MapPath($"~/App_Data/Temp");
            if (!System.IO.Directory.Exists(tempData))
                System.IO.Directory.CreateDirectory(tempData);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
