using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Bll
{
    public class BookBorrowing : IDisposable
    {
        private const int BorrowLimit = 5;

        private BookLibraryContext _db;

        private BookBorrowing()
        {
            _db = new BookLibraryContext();
        }

        void IDisposable.Dispose()
        {
            if (_db == null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public static bool IsBorrowed(long bookid)
        {
            using (var bb = new BookBorrowing())
            {
                return bb.InternalIsBorrowed(bookid);
            }
        }

        public static bool BorrowBook(User user, long bookid, out string message)
        {
            using (var bb = new BookBorrowing())
            {
                if (bb.InternalIsBorrowed(bookid))
                {
                    message = "This book has been borrowed by others.";
                    return false;
                }

                if (bb.UserArrieveBorrowLimit(user.Id))
                {
                    message = string.Format("You can only borrowed less or equal to {0} books.", BorrowLimit);
                    return false;
                }

                bb.InternalBorrowBook(user, bookid);

                message = string.Empty;
                return true;
            }
        }

        private bool InternalIsBorrowed(long bookid)
        {
            var queryIsBorrowed = from b in _db.BorrowInfos where b.BookId == bookid select b;
            return queryIsBorrowed.Any();
        }

        private bool UserArrieveBorrowLimit(long userId)
        {
            var queryIsBorrowed = from b in _db.BorrowInfos where b.UserId == userId select b;
            return queryIsBorrowed.Count() >= BorrowLimit;
        }

        private void InternalBorrowBook(User user, long bookid)
        {
            var borrowInfo = new BorrowInfo(user, bookid);
            _db.BorrowInfos.Add(borrowInfo);
            _db.SaveChanges();
        }
    }
}