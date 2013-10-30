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

        public BorrowInfo(User user, Book book)
        {
            User = user;
            Book = book;
            BorrowedDate = DateTime.Now;
            IsCheckedOut = false;
        }

        public long Id { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }

        public bool IsCheckedOut { get; set; }

        public DateTime BorrowedDate { get; set; }
    }
}