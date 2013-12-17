using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookLibrary.Bl;
using BookLibrary.Entities;
using BookLibrary.Utilities;
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
            /Debug/ListUsers to list all users in the database.
            /Debug/SwitchUser?name=xxx to swith crrent user to the specified user.
            /Debug/AddCustomer to add a new customer and set as current user.";

        public ActionResult Index()
        {
            return TextContent(DebugUsage);
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

            var message = string.Format("Role changed to [{0}].", BuildUserInfo(Bl.Users.Current));
            return TextContent(message);
        }

        public ActionResult SwitchUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var errorAlert = string.Format("alert('{0}');", "Please input a valid login name.\n/Debug/ListUser to list all possible users.");
                return JavaScript(errorAlert);
            }

            List<User> users;
            using (var db = new BookLibraryContext())
            {
                users = db.Users.ToList();
            }

            var user = users.FirstOrDefault(u => UserMatch(u, name));
            if (user == null)
            {
                var errorAlert = Utility.BuildAlert("There is no user login name started with [{0}].", name);
                return JavaScript(errorAlert);
            }

            Bl.Users.Current = user;

            var message = string.Format("Swithed to user [{0}].", BuildUserInfo(Bl.Users.Current));
            return TextContent(message);
        }

        public ActionResult AddCustomer()
        {
            var user = new Entities.User
            {
                LoginName = @"MM\TC",
                FullName = "TestCustomer",
                EmailAdress = "TestCustomer@b.c",
                Role = (int)Roles.Customer
            };

            using (var db = new BookLibraryContext())
            {
                db.Users.Add(user);
                db.SaveChanges();

                Bl.Users.Current = user;
            }

            var message = string.Format("Created and swithed to new user [{0}].", BuildUserInfo(user));
            return TextContent(message);
        }

        public ActionResult ListUsers()
        {
            List<string> lines;
            using (var db = new BookLibraryContext())
            {
                lines = db.Users.Select(BuildUserLine).ToList();
            }

            var users = string.Join("\n", lines);

            return TextContent(users);
        }

        private static bool UserMatch(User user, string name)
        {
            return user.LoginName.Contains(name) || user.EmailAdress.Contains(name);
        }

        private static string BuildUserInfo(User user)
        {
            return string.Format("{0},{1},{2}", user.LoginName, user.FullName, (Roles)user.Role);
        }

        private static string BuildUserLine(User user)
        {
            return string.Format("{0}\t{1}\t{2}", user.LoginName, user.FullName, (Roles)user.Role);
        }

        private ActionResult TextContent(string text)
        {
            return Content(Server.HtmlEncode(text), TextContentType, System.Text.Encoding.UTF8);
        }
    }

#endif

}
