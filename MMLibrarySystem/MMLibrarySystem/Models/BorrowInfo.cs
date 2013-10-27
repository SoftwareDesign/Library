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
        }

        public BorrowInfo(User user, long bookId)
        {
            UserId = user.Id;
            BookId = bookId;
            BorrowedDate = DateTime.Now;
        }

        public long Id { get; set; }

        public long BookId { get; set; }

        public long UserId { get; set; }

        public DateTime BorrowedDate { get; set; }
    }
}