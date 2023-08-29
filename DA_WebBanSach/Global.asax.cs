using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using DA_WebBanSach.Models;
using System.Globalization;
using DA_WebBanSach.Filters;

namespace DA_WebBanSach
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new MyDefaultBinder());
            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.DefaultNamespaces.Add("DA_WebBanSach.Controllers");
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            

            //Database.SetInitializer<UsersContext>(new InitSecurityDb());
            //UsersContext context = new UsersContext();
            //context.Database.Initialize(true);
            //if (!WebSecurity.Initialized)
            //    WebSecurity.InitializeDatabaseConnection("SachDBTest",
            //        "NguoiDungs", "UserId", "UserName", autoCreateTables: true);
        }
    }
}