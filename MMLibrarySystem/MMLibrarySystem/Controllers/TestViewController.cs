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
            List<Book> books;
            using (var dbContext = new BookLibraryContext())
            {
                books = dbContext.Books.Include("BookInfo").ToList();
            }

            if (Request.IsAjaxRequest())
            {
                PartialView("_BookList", books);
            }

            return View(books);
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
