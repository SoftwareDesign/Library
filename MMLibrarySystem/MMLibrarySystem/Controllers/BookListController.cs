using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Models;
using MMLibrarySystem.Bll;
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
                return PartialView("_BookList", books);
            }

            return View(books);
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
            long bookid = Convert.ToInt64(columid.Substring(3));
            if (BookBorrowing.IsBorrowed(bookid))
            {
                return JavaScript("alert('this book is borrowed by others');");
            }
            else
            {
                BorrowInfo borrowInfo = new BorrowInfo { BookId = bookid, BorrowedDate = DateTime.Now };
                BookBorrowing.BorrowBook(borrowInfo);
                return JavaScript("BorrowSuccessAction('" + bookid + "');");
            }
        }
    }
}
