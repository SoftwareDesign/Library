using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookLibrary.Bl;
using BookLibrary.Entities;
using BookLibrary.ViewModels;
using BookLibrary.ViewModels.Admin;
using BookLibrary.ViewModels.BookList;


namespace BookLibrary.Controllers
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
            var user = Bl.Users.Current;
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
            var user = Bl.Users.Current;
            using (var db = new BookLibraryContext())
            {
                var users = db.Users;
                var another = users.FirstOrDefault(u => u.UserId != user.UserId);
                if (another == null)
                {
                    var errorAlert = string.Format("alert('{0}');", "Please create more than one user in the database first.");
                    return JavaScript(errorAlert);
                }

                Bl.Users.Current = another;
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult AddCustomer()
        {
            using (var db = new BookLibraryContext())
            {
                var user = new Entities.User
                {
                    LoginName = @"MM\TC",
                    FullName = "TestCustomer",
                    EmailAdress = "TestCustomer@b.c",
                    Role = (int)Roles.Customer
                };
                db.Users.Add(user);
                db.SaveChanges();

                Bl.Users.Current = user;
            }

            return RedirectToAction("Index", "BookList");
        }
    }

#endif

}
