using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using MMLibrarySystem.Models;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Models.BookListController;
using PagedList;

namespace MMLibrarySystem.Controllers
{
    public class BookListController : Controller
    {      
        
        public ActionResult Index(string searchTerm = null,int page=1)
        {
            var pageSize = GetPageSize();

            IPagedList<BookListItem> bookList;
            using (var db = new BookLibraryContext())
            {
                var tempBooks = db.Books.Include("BookType");

                if (string.IsNullOrEmpty(searchTerm))
                {
                    bookList = tempBooks.OrderByDescending(r => r.BookNumber).Select(CreateBookListItem).ToPagedList(page, pageSize);
                }
                else
                {
                    var searchResult =
                        tempBooks.Where(
                            b => b.BookType.Title.Contains(searchTerm) || b.BookType.Description.Contains(searchTerm));
                    bookList = searchResult.Select(CreateBookListItem).ToPagedList(page, pageSize);
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
                book = dbContext.Books.Include("BookType").First(b => b.BookNumber == bookNumber);
            }

            return View(book);
        }

        public ActionResult Borrow(string columid)
        {
            var bookid = Convert.ToInt64(columid.Substring(3));

            string message;
            var user = Models.User.Current;
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

        private int GetPageSize()
        {
            int size = 1;
            string filePath = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()) + "GlobalConfig.xml";
            XElement xe = XElement.Load(filePath);
            IEnumerable<XElement> rootCatalog = from root in xe.Elements("PageInfo") select root;
            size = Convert.ToInt32(rootCatalog.FirstOrDefault().Attribute("size").Value);
            return size;
        }

        private BookListItem CreateBookListItem(Book book)
        {
            var borrowed = BookBorrowing.IsBorrowed(book.BookId);
            var item =
                new BookListItem
                    {
                        BookId = book.BookId.ToString(),
                        BookNumber = book.BookNumber,
                        Title = book.BookType.Title,
                        Publisher = book.BookType.Publisher,
                        PurchaseDate = book.PurchaseDate.ToShortDateString(),

                        State = borrowed ? "Borrowed" : "In Library",
                        Operation = borrowed ? string.Empty : "Borrow"
                    };
            return item;
        }
    }
}
