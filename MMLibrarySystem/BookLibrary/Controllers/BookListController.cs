using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BookLibrary.Entities;
using BookLibrary.Bl;
using BookLibrary.Utilities;
using BookLibrary.ViewModels;
using BookLibrary.ViewModels.BookList;
using PagedList;

namespace BookLibrary.Controllers
{
    public class BookListController : Controller
    {
        private const string StateInLib = "In Library";

        private BookBorrowing _bb;

        public ActionResult Index(string searchTerm = null, bool showInLibrary = false, int page = 1)
        {
            var pageSize = GetPageSize();

            IPagedList<BookListItem> bookList;
            using (var db = new BookLibraryContext())
            {
                _bb = new BookBorrowing(db);
                var tempBooks = db.Books.Include("BookType");

                var searchResult =
                            tempBooks.Where(DoesBookMeetSearchTermPredicate(searchTerm));
                bookList = searchResult.Select(CreateBookListItem)
                            .Where(DoesBookExistInLibraryPredicate(showInLibrary))
                            .ToPagedList(page, pageSize);

                _bb = null;
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
            using (var db = new BookLibraryContext())
            {
                book = db.Books.Include("BookType").FirstOrDefault(b => b.BookNumber == bookNumber);
            }

            if (book == null)
            {
                var errorAlert = string.Format(
                    "alert('Invalid book number [{0}]. Please contact the admin.');", bookNumber);
                return JavaScript(errorAlert);
            }

            var info = new BookDetailInfo();
            info.Operation = Bl.Users.Current.IsAdmin ? UserOperationFactory.CreateEditBookOperation(book.BookId) : null;
            info.LoadInfo(book);
            return View(info);
        }

        public ActionResult Borrow(long bookId)
        {
            string message;
            var user = Bl.Users.Current;
            if (user == null)
            {
                var errorAlert = string.Format(
                    "alert('Current user [{0}] has no rights to borrow books. Please contact the admin.');",
                    Bl.Users.CurrentLoginName);
                return JavaScript(errorAlert);
            }

            bool succeed;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.BorrowBook(user, bookId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult Subscribe(long bookId)
        {
            var user = Bl.Users.Current;

            bool succeed;
            string message;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.SubscribeBook(user, bookId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult Cancel(long bookId)
        {
            var user = Bl.Users.Current;

            bool succeed;
            string message;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.CancelBorrow(user, bookId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            return RedirectToAction("Index", "BookList");
        }

        private int GetPageSize()
        {
            int size = 10;
            string getPageSizeValue = GlobalConfigReader.ReadFromGlobalConfig("PageInfo", "size");
            if (!string.IsNullOrEmpty(getPageSizeValue))
                size = Convert.ToInt32(getPageSizeValue);
            return size;
        }

        private BookListItem CreateBookListItem(Book book)
        {
            var state = _bb.GetCustomerBookState(book.BookId);
            var item = new BookListItem
            {
                BookId = book.BookId.ToString(),
                BookNumber = book.BookNumber,
                Title = UserOperationFactory.CreateBookDetailOperation(book.BookType.Title, book.BookNumber),
                Publisher = book.BookType.Publisher,
                PurchaseDate = book.PurchaseDate.ToShortDateString(),
                State = state.State,
                Operation = state.Operation
            };

            return item;
        }

        private static Func<BookListItem, bool> DoesBookExistInLibraryPredicate(bool showInLibrary)
        {
            if (showInLibrary)
            {
                return book => book.State == StateInLib;
            }

            return book => !string.IsNullOrEmpty(book.State);
        }

        private static Expression<Func<Book, bool>> DoesBookMeetSearchTermPredicate(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm))
            {
                return b => !string.IsNullOrEmpty(b.BookNumber);
            }

            return b =>
                b.BookType.Title.Contains(searchTerm) || b.BookType.Description.Contains(searchTerm);
        }
    }
}
