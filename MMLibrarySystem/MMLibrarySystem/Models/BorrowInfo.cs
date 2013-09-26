using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMLibrarySystem.Models
{
    public class BorrowInfo
    {
        public BorrowInfo()
        {
            using (var db = new BookLibraryContext())
            {
                var queryTheUserId = from u in db.Users
                                     where u.LogName == HttpContext.Current.User.Identity.Name
                                     select u;
                UserId = queryTheUserId.First().Id;
            }
        }

        public long Id { get; set; }

        public long BookId { get; set; }

        public long UserId { get; set; }

        public DateTime BorrowedDate { get; set; }
    }
}