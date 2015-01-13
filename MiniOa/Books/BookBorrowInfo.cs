using System;
using MiniOa.Users;

namespace MiniOa.Books
{
    public class BookBorrowInfo
    {
        public Book Book { get; set; }

        public User Reader { get; set; }

        public DateTime BorrowTime { get; set; }
    }
}