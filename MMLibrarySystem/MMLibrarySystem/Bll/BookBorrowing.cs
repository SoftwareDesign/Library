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

        public static bool IsBorrowed(long bookId)
        {
            using (var bb = new BookBorrowing())
            {
                return bb.InternalIsBorrowed(bookId);
            }
        }

        public static bool BorrowBook(User user, long bookId, out string message)
        {
            using (var bb = new BookBorrowing())
            {
                if (bb.UserArrieveBorrowLimit(user.UserId))
                {
                    message = string.Format("You can only borrowed less or equal to {0} books.", BorrowLimit);
                    return false;
                }

                if (bb.InternalIsBorrowed(bookId))
                {
                    message = "This book has been borrowed by others.";
                    return false;
                }

                var book = bb.GetBookById(bookId);
                if (book == null)
                {
                    message = string.Format("The book with id [{0}] does not exist. Please contact the admin.", bookId);
                    return false;
                }

                bb.InternalBorrowBook(user, book);

                message = string.Empty;
                return true;
            }
        }

        void IDisposable.Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        private bool InternalIsBorrowed(long bookId)
        {
            var queryIsBorrowed = from b in _db.BorrowRecords where b.Book.BookId == bookId select b;
            return queryIsBorrowed.Any();
        }

        private Book GetBookById(long bookId)
        {
            var queryBooks = from b in _db.Books where b.BookId == bookId select b;
            return queryBooks.FirstOrDefault();
        }

        private bool UserArrieveBorrowLimit(long userId)
        {
            var queryIsBorrowed = from b in _db.BorrowRecords where b.User.UserId == userId select b;
            return queryIsBorrowed.Count() >= BorrowLimit;
        }

        private void InternalBorrowBook(User user, Book book)
        {
            var borrowInfo = new BorrowRecord(user, book);
            _db.BorrowRecords.Add(borrowInfo);
            _db.SaveChanges();
        }
    }
}