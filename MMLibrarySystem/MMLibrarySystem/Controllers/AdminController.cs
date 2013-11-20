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

namespace MMLibrarySystem.Controllers
{
    public class AdminController : MyControllerBase
    {
        public ActionResult Index()
        {
            var infos = GetAllBorrowInfo();
            return View(infos);
        }

        public ActionResult CheckOut(string borrowId)
        {
            var id = long.Parse(borrowId);
            using (var db = new BookLibraryContext())
            {
                var info = db.BorrowRecords.First(i => i.BorrowRecordId == id);
                info.IsCheckedOut = true;
                db.SaveChanges();
            }

            var infos = GetAllBorrowInfo();
            return View("Index", infos);
        }

        public ActionResult Return(string borrowId)
        {
            var id = long.Parse(borrowId);
            using (var db = new BookLibraryContext())
            {
                var info = db.BorrowRecords.First(i => i.BorrowRecordId == id);
                db.BorrowRecords.Remove(info);
                db.SaveChanges();
            }

            var infos = GetAllBorrowInfo();
            return View("Index", infos);
        }
        
        [HttpGet]
        public ActionResult RegistNewBook(string operation, long bookId)
        {
            EditBookInfo edit;
            if (operation == "Add")
            {
                edit = new EditBookInfo { PurchaseDate = DateTime.Now.ToShortDateString(), Operation = "Add" };
                return View(edit);
            }

            Book book;
            using (var db = new BookLibraryContext())
            {
                book = db.Books.Include("BookType").FirstOrDefault(b => b.BookId == bookId);
            }

            if (book == null)
            {
                return Alert("Invalid bookId [{0}].", bookId);
            }

            edit = new EditBookInfo { Operation = "Edit" };
            edit.LoadInfo(book);
            return View(edit);
        }

        [HttpPost]
        public ActionResult RegistNewBook(EditBookInfo editInfo)
        {
            if (editInfo.Operation == "Add")
            {
                using (var db = new BookLibraryContext())
                {
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
                Publisher = info.Publisher,
                Supplier = info.Supplier
            };

            var book = new Book
            {
                BookNumber = info.BookNumber,
                NetPrice = float.Parse(info.NetPrice),
                PurchaseDate = DateTime.Parse(info.PurchaseDate),
                RequestedBy = info.RequestedBy,
                PurchaseUrl = info.PurchaseUrl,
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
            var state = new BookBorrowState(record);
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
                State = state.InternalState,
                Operation = state.GetAdminOperation(Models.User.Current)
            };

            return item;
        }
    }
}
