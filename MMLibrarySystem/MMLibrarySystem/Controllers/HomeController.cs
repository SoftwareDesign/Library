using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Default()
        {
            ActionResult result;
            var role = (Roles)Models.Users.Current.Role;
            switch (role)
            {
                case Roles.Customer:
                    result = RedirectToAction("Index", "BookList");
                    break;
                case Roles.Admin:
                    result = RedirectToAction("Index", "Admin");
                    break;
                case Roles.Guest:
                default:
                    result = RedirectToAction("Guest");
                    break;
            }

            return result;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Guest()
        {
            return View();
        }
    }
}
