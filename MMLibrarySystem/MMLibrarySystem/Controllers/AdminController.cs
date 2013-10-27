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
            var infos = new List<BorrowInfo>();
            using (var db = new BookLibraryContext())
            {
                var allInfos = db.BorrowInfos.Include("User").Include("Book");
                infos.AddRange(allInfos);
            }

            return View(infos);
        }

        public ActionResult CheckOut(string borrowId)
        {
            var id = long.Parse(borrowId);
            using (var db = new BookLibraryContext())
            {
                var info = db.BorrowInfos.First(i => i.Id == id);
                info.IsCheckedOut = true;
            }

            return Index();
        }

        public ActionResult Return(string borrowId)
        {
            var id = long.Parse(borrowId);
            using (var db = new BookLibraryContext())
            {
                var info = db.BorrowInfos.First(i => i.Id == id);
                db.BorrowInfos.Remove(info);
                db.SaveChanges();
            }

            return Index();
        }
    }
}
