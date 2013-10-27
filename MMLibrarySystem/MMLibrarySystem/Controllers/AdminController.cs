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
            using (var dbContext = new BookLibraryContext())
            {
                var allInfos = dbContext.BorrowInfos;
                infos.AddRange(allInfos);
            }

            return View(infos);
        }
    }
}
