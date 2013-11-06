using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMLibrarySystem.Models;

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
            var book = new Book();
            book.BookType = new BookType();
            return View(book);
        }

        [HttpPost]
        public ActionResult RegistNewBook(Book book)
        {
            using (var db = new BookLibraryContext())
            {
                db.Books.Add(book);
                db.SaveChanges();
            }
            return Redirect("/");
        }

        private List<BorrowRecord> GetAllBorrowInfo()
        {
            using (var db = new BookLibraryContext())
            {
                var allInfos = db.BorrowRecords.Include("User").Include("Book").Include("Book.BookType");
                return allInfos.ToList();
            }
        }
    }
}
