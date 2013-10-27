using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Models;
using MMLibrarySystem.Bll;
using PagedList;

namespace MMLibrarySystem.Controllers
{
    public class BookListController : Controller
    {      
        
        public ActionResult Index(string searchTerm = null,int page=1)
        {
            List<Book> books = new List<Book>();
            IPagedList<Book> bookList = new PagedList<Book>(books, 1, 10);
            using (var dbContext = new BookLibraryContext())
            {
                var tempBooks = dbContext.Books.Include("BookInfo");

                if (string.IsNullOrEmpty(searchTerm))
                {
                    bookList = tempBooks.OrderByDescending(r => r.BookNumber).ToPagedList(page, 10);
                }
                else
                {
                    bookList = (tempBooks.Where(b => b.BookInfo.Title.Contains(searchTerm) || b.BookInfo.Description.Contains(searchTerm)).ToPagedList(page, 10));
                }
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_BookList", bookList);
            }

            return View(bookList);
        }

        public ActionResult BookDetail(string bookNumber)
        {
            Book book;
            using (var dbContext = new BookLibraryContext())
            {
                book = dbContext.Books.Include("BookInfo").First(b => b.BookNumber == bookNumber);
            }

            return View(book);
        }

        public ActionResult BorrowBook(string columid)
        {
            var bookid = Convert.ToInt64(columid.Substring(3));

            string message;
            var user = Models.User.CurrentUser;
            if (user == null)
            {
                var errorAlert = string.Format(
                    "alert('Current user [{0}] has no rights to borrow books. Please contact the admin.');",
                    Models.User.CurrentLoginName);
                return JavaScript(errorAlert);
            }

            var succeed = BookBorrowing.BorrowBook(user, bookid, out message);
            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            var result = string.Format("BorrowSuccessAction('{0}');", bookid);
            return JavaScript(result);
        }
    }
}
