using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Models;
using MMLibrarySystem.ViewModels;
using MMLibrarySystem.ViewModels.Admin;
using MMLibrarySystem.ViewModels.BookList;


namespace MMLibrarySystem.Controllers
{

#if DEBUG

    /// <summary>
    /// The controller to help debug.
    /// </summary>
    public class DebugController : Controller
    {
        private const string TextContentType = "text/plain";

        private const string DebugUsage =
            @"
            Some operation provided to help debug the website:
            /Debug/SwitchRole to swith role of current user between customer and admin.
            /Debug/SwitchUser to swith crrent user between the user list in database.
            /Debug/AddCustomer to add a new customer and set as current user.";

        public ActionResult Index()
        {
            return Content(Server.HtmlEncode(DebugUsage), TextContentType, System.Text.Encoding.UTF8);
        }

        public ActionResult SwitchRole()
        {
            var user = Models.Users.Current;
            if (user.IsAdmin)
            {
                user.Role = (int)Roles.Customer;
            }
            else
            {
                user.Role = (int)Roles.Admin;
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult SwitchUser()
        {
            var user = Models.Users.Current;
            using (var db = new BookLibraryContext())
            {
                var users = db.Users;
                var another = users.FirstOrDefault(u => u.UserId != user.UserId);
                if (another == null)
                {
                    var errorAlert = string.Format("alert('{0}');", "Please create more than one user in the database first.");
                    return JavaScript(errorAlert);
                }

                Models.Users.Current = another;
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult AddCustomer()
        {
            using (var db = new BookLibraryContext())
            {
                var user = new Models.User
                {
                    LoginName = @"MM\TC",
                    FullName = "TestCustomer",
                    EmailAdress = "TestCustomer@b.c",
                    Role = (int)Roles.Customer
                };
                db.Users.Add(user);
                db.SaveChanges();

                Models.Users.Current = user;
            }

            return RedirectToAction("Index", "BookList");
        }
    }

#endif

}
