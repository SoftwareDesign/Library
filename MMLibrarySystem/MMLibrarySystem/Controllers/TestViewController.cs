using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMLibrarySystem.Controllers
{
    public class TestViewController : Controller
    {
        //
        // GET: /TestView/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestView2()
        {
            return View();
        }
        public ActionResult TestView3()
        {
            return View();
        }
    }
}
