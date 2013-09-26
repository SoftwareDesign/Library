using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Bll
{
    public class BookBorrowing
    {
        public static bool IsBorrowed(long bookid)
        {
            using (var db = new BookLibraryContext())
            {
                var queryIsBorrowed = from b in db.BorrowInfos where b.BookId == bookid select b;
                if (!queryIsBorrowed.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static void BorrowBook(BorrowInfo borrowInfo)
        {
            using (var db = new BookLibraryContext())
            {
                db.BorrowInfos.Add(borrowInfo);
                db.SaveChanges();
            }
        }
    }
}