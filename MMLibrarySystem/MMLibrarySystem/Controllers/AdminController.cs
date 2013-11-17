using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Models;
using MMLibrarySystem.ViewModels.Admin;
using MMLibrarySystem.ViewModels.BookList;

namespace MMLibrarySystem.Controllers
{
    public class AdminController : Controller
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
        public ActionResult RegistNewBook()
        {
            var info = new BookInfo { PurchaseDate = DateTime.Now.ToShortDateString() };
            return View(info);
        }

        [HttpPost]
        public ActionResult RegistNewBook(BookInfo info)
        {
            using (var db = new BookLibraryContext())
            {
                var book = CreateNewBook(info);
                db.Books.Add(book);
                db.SaveChanges();
            }

            return Redirect("/");
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
