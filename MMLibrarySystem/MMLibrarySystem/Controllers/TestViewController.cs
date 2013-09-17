using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Controllers
{
    public class TestViewController : Controller
    {
        //
        // GET: /TestView/

        public ActionResult Index()
        {
            //using (var dbContext = new BookLibraryContext())
            //{
            //    var bookinfo = new BookInfo()
            //    {
            //        Title = "Test"
            //    };
            //    var book = new Book()
            //    {
            //        BookNumber = "22222",
            //        BookInfo = bookinfo
            //    };

            //    dbContext.Books.Add(book);
            //    dbContext.SaveChanges();
            //}
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
