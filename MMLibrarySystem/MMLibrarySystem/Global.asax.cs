using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;

using MMLibrarySystem.Models;
using MMLibrarySystem.Schedule;

namespace MMLibrarySystem
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            Database.SetInitializer(new BookLibraryInitializer());
#else
            Database.SetInitializer(new CreateDatabaseIfNotExists<BookLibraryContext>());
#endif

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            DailyPlan dailyPlan = new DailyPlan();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                var loginName = Models.User.CurrentLoginName;
                var current = Models.User.FindUserByLoginName(loginName);
                if (current == null)
                {
                    using (var db = new BookLibraryContext())
                    {
                        var debgger = new User { LoginName = loginName, Role = (int)Roles.Admin, EmailAdress = "test@email.com" };
                        db.Users.Add(debgger);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}