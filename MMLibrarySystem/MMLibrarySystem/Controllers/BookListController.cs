using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Controllers
{
    public class BookListController : Controller
    {      
        public ActionResult Index(string searchTerm = null)
        {
            List<Book> books = new List<Book>();
            using (var dbContext = new BookLibraryContext())
            {
                var tempBooks = dbContext.Books.Include("BookInfo");

                if (string.IsNullOrEmpty(searchTerm))
                {
                    books.AddRange(tempBooks.ToList());
                }
                else
                {
                    books.AddRange(tempBooks.Where(b => b.BookInfo.Title.Contains(searchTerm) || b.BookInfo.Description.Contains(searchTerm)));
                }
            }

            if (Request.IsAjaxRequest())
            {
                PartialView("_BookList", books);
            }

            return View(books);
        }
    }
}
