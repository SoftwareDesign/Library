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
            /Debug/SwitchRole to swith role of current user between employee and admin.
            /Debug/SwitchUser to swith crrent user between the user list in database.
            /Debug/AddEmployee to add a new employee user as current user.";

        public ActionResult Index()
        {
            return Content(Server.HtmlEncode(DebugUsage), TextContentType, System.Text.Encoding.UTF8);
        }

        public ActionResult SwitchRole()
        {
            var user = Models.User.Current;
            if (user.IsAdmin)
            {
                user.Role = (int)Roles.Employee;
            }
            else
            {
                user.Role = (int)Roles.Admin;
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult SwitchUser()
        {
            var user = Models.User.Current;
            using (var db = new BookLibraryContext())
            {
                var users = db.Users;
                var another = users.FirstOrDefault(u => u.UserId != user.UserId);
                if (another == null)
                {
                    var errorAlert = string.Format("alert('{0}');", "Please create more than one user in the database first.");
                    return JavaScript(errorAlert);
                }

                Models.User.Current = another;
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult AddEmployee()
        {
            using (var db = new BookLibraryContext())
            {
                var user = new Models.User
                {
                    LoginName = @"MM\TE",
                    FullName = "TestEmployee",
                    EmailAdress = "a@b.c",
                    Role = (int)Roles.Employee
                };
                db.Users.Add(user);
                db.SaveChanges();

                Models.User.Current = user;
            }

            return RedirectToAction("Index", "BookList");
        }
    }

#endif

}
