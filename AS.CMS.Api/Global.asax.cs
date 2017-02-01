using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;

namespace AS.CMS.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterGlobalFilters(GlobalFilters.Filters);

            if (ConfigurationManager.AppSettings["SSLEnabled"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["SSLEnabled"]))
            {
                GlobalFilters.Filters.Add(new RequireHttpsAttribute());
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}