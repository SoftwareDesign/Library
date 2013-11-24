using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Models;
using MMLibrarySystem.ViewModels;
using MMLibrarySystem.ViewModels.Admin;
using MMLibrarySystem.ViewModels.BookList;
using MMLibrarySystem.Utilities;

namespace MMLibrarySystem.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (!Models.User.Current.IsAdmin)
            {
                return RedirectToAction("Default", "Home");
            }

            var infos = GetAllBorrowInfo();
            return View(infos);
        }

        public ActionResult CheckOut(long borrowId)
        {
            var user = Models.User.Current;
            if (!user.IsAdmin)
            {
                return RedirectToAction("Default", "Home");
            }

            bool succeed;
            string message;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.CheckOut(user, borrowId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            var infos = GetAllBorrowInfo();
            return View("Index", infos);
        }

        public ActionResult Return(long borrowId)
        {
            var user = Models.User.Current;
            if (!user.IsAdmin)
            {
                return RedirectToAction("Default", "Home");
            }

            bool succeed;
            string message;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.Return(user, borrowId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            var infos = GetAllBorrowInfo();
            return View("Index", infos);
        }
        
        [HttpGet]
        public ActionResult EditBook(string operation, string bookId)
        {
            var user = Models.User.Current;
            if (!user.IsAdmin)
            {
                return RedirectToAction("Default", "Home");
            }

            EditBookInfo edit;
            if (operation == "Add")
            {
                edit = new EditBookInfo { PurchaseDate = DateTime.Now.ToShortDateString(), Operation = "Add" };
                return View(edit);
            }

            var id = long.Parse(bookId);
            Book book;
            using (var db = new BookLibraryContext())
            {
                book = db.Books.Include("BookType").FirstOrDefault(b => b.BookId == id);
            }

            if (book == null)
            {
                var alert = Utility.BuildAlert("Invalid book ID [{0}].", bookId);
                return JavaScript(alert);
            }

            edit = new EditBookInfo { Operation = "Edit" };
            edit.LoadInfo(book);
            return View(edit);
        }

        [HttpPost]
        public ActionResult EditBook(EditBookInfo editInfo)
        {
            var user = Models.User.Current;
            if (!user.IsAdmin)
            {
                return RedirectToAction("Default", "Home");
            }

            var bookNumber = editInfo.BookNumber;
            if (string.IsNullOrEmpty(bookNumber))
            {
                var alert = Utility.BuildAlert("Book number could not be empty.");
                return JavaScript(alert);
            }

            if (editInfo.Operation == "Add")
            {
                using (var db = new BookLibraryContext())
                {
                    var existBook = db.Books.Include("BookType").FirstOrDefault(b => b.BookNumber == bookNumber);
                    if (existBook != null)
                    {
                        var alert = Utility.BuildAlert("The book number [{0}] conflict with the book {1}.", bookNumber, existBook.BookType.Title);
                        return JavaScript(alert);
                    }

                    var book = CreateNewBook(editInfo);
                    db.Books.Add(book);
                    db.SaveChanges();
                }
            }
            else if (editInfo.Operation == "Edit")
            {
                using (var db = new BookLibraryContext())
                {
                    var bookId = long.Parse(editInfo.BookId);
                    var existBook = db.Books.Include("BookType").FirstOrDefault(b => b.BookId != bookId && b.BookNumber == bookNumber);
                    if (existBook != null)
                    {
                        var alert = Utility.BuildAlert("The book number [{0}] conflict with the book {1}.", bookNumber, existBook.BookType.Title);
                        return JavaScript(alert);
                    }

                    var book = db.Books.Include("BookType").FirstOrDefault(b => b.BookId == bookId);
                    editInfo.StoreInfo(book);
                    db.SaveChanges();
                }
            }

            var bookDetailUrl = string.Format("/BookList/BookDetail?bookNumber={0}", editInfo.BookNumber);
            return Redirect(bookDetailUrl);
        }

        private Book CreateNewBook(BookInfo info)
        {
            var type = new BookType
            {
                Title = info.Title,
                Description = info.Description,
                UserAndTeam = info.UserAndTeam,
                Publisher = info.Publisher   
            };

            var book = new Book
            {
                BookNumber = info.BookNumber,
                NetPrice = float.Parse(info.NetPrice),
                PurchaseDate = DateTime.Parse(info.PurchaseDate),
                RequestedBy = info.RequestedBy,
                PurchaseUrl = info.PurchaseUrl,
                Supplier = info.Supplier,
                BookType = type
            };

            return book;
        }

        private List<BorrowListItem> GetAllBorrowInfo()
        {
            using (var db = new BookLibraryContext())
            {
                var allInfos = db.BorrowRecords.Include("User").Include("Book").Include("Book.BookType");
                return allInfos.Select(CreateBorrowListItem).ToList();
            }
        }

        private BorrowListItem CreateBorrowListItem(BorrowRecord record)
        {
            var state = new AdminBookState(record);
            var item = new BorrowListItem
            {
                BorrowId = record.BorrowRecordId.ToString(),
                BookId = record.BookId.ToString(),
                BookNumber = record.Book.BookNumber,
                Title = record.Book.BookType.Title,
                UserId = record.UserId.ToString(),
                UserName = record.User.DisplayName,
                BorrowDate = record.BorrowedDate.ToShortDateString(),
                ReturnDate = record.BorrowedDate.AddDays(31).ToShortDateString(),
                State = state.State,
                Operation = state.Operation
            };

            return item;
        }
    }
}
