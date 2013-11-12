using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Bll
{
    public class BookBorrowing
    {
        private static int BorrowLimit = GetBorrowLimit();

        private readonly BookLibraryContext _db;

        private readonly List<BorrowRecord> _recordCache;

        public BookBorrowing(BookLibraryContext db)
        {
            _db = db;
            _recordCache = _db.BorrowRecords.ToList();
        }

        public BookBorrowState GetBookBorrowState(long bookId)
        {
            var record = _recordCache.FirstOrDefault(r => r.BookId == bookId);
            return new BookBorrowState(record);
        }

        public bool IsBorrowed(long bookId)
        {
            return InternalIsBorrowed(bookId);
        }

        public bool BorrowBook(User user, long bookId, out string message)
        {
            if (UserArrieveBorrowLimit(user.UserId))
            {
                message = string.Format("You can only borrowed less or equal to {0} books.", BorrowLimit);
                return false;
            }

            if (InternalIsBorrowed(bookId))
            {
                message = "This book has been borrowed by others.";
                return false;
            }

            var book = GetBookById(bookId);
            if (book == null)
            {
                message = string.Format("The book with id [{0}] does not exist. Please contact the admin.", bookId);
                return false;
            }

            InternalBorrowBook(user, book);

            message = string.Empty;
            return true;
        }

        public bool CancelBorrow(User user, long bookid, out string message)
        {
            var record = _db.BorrowRecords.FirstOrDefault(r => r.BookId == bookid);
            if (record == null)
            {
                message = "Could not cancel a not borrowed book.";
                return false;
            }

            if (record.UserId != user.UserId)
            {
                message = "The book is not borrowed by current user.";
                return false;
            }

            _db.BorrowRecords.Remove(record);
            _db.SaveChanges();

            message = string.Empty;
            return true;
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

        private static int GetBorrowLimit()
        {
            int number = 5;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "GlobalConfig.xml";
            XElement xe = XElement.Load(filePath);
            IEnumerable<XElement> rootCatalog = from root in xe.Elements("BorrowLimit") select root;
            number = Convert.ToInt32(rootCatalog.FirstOrDefault().Attribute("number").Value);
            return number;
        }
    }
}