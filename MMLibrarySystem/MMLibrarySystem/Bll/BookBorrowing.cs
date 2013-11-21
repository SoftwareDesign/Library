using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MMLibrarySystem.Models;
using MMLibrarySystem.Utilities;

namespace MMLibrarySystem.Bll
{
    public class BookBorrowing
    {
        private static int BorrowLimit = GetBorrowLimit();

        private readonly BookLibraryContext _db;

        private readonly List<BorrowRecord> _recordCache;

        private readonly List<SubscribeRecord> _subscribeRecordCache;

        public BookBorrowing(BookLibraryContext db)
        {
            _db = db;
            _recordCache = _db.BorrowRecords.ToList();
            _subscribeRecordCache = _db.SubscribeRecords.ToList();
        }

        public BookBorrowState GetBookBorrowState(long bookId)
        {
            var record = _recordCache.FirstOrDefault(r => r.BookId == bookId);
            if (record != null)
            {
                var borrowState = new BookBorrowState(record);
                var subscribeRecord =
                    _subscribeRecordCache.FirstOrDefault(sr => sr.BookId == bookId && sr.UserId == User.Current.UserId);
                if (subscribeRecord != null)
                {
                    borrowState.CanSubscribe = false;
                }
                else
                {
                    borrowState.CanSubscribe = true;
                }

                return borrowState;
            }
            else
            {
                return new BookBorrowState(bookId);
            }
        }

        public bool IsBorrowed(long bookId)
        {
            return IsBookBorrowed(bookId);
        }

        public bool BorrowBook(User user, long bookId, out string message)
        {
            if (UserArrieveBorrowLimit(user.UserId))
            {
                message = string.Format("You can only borrowed less or equal to {0} books.", BorrowLimit);
                return false;
            }

            if (IsBookBorrowed(bookId))
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

        public bool SubscribeBook(User user, long bookid, out string message)
        {
            var record = _db.BorrowRecords.FirstOrDefault(r => r.BookId == bookid);
            if (record == null)
            {
                message = "The book has not been borrowed, please use borrow instead of subscribe.";
                return false;
            }

            if (record.UserId == user.UserId)
            {
                message = "You could not subscribe a book which borrowed by yourself.";
                return false;
            }

            //if (!record.IsCheckedOut)
            //{
            //    message = "Could not subscribe a not checked out book.";
            //    return false;
            //}

            var subscribeRecord = _db.SubscribeRecords.FirstOrDefault(r => r.BookId == bookid && r.UserId == User.Current.UserId);
            if (subscribeRecord != null)
            {
                message = "You already subscribed the book.";
                return false;
            }

            var subscribeInfo = new SubscribeRecord
                {
                    BookId = record.BookId,
                    SubscribeDate = DateTime.Now,
                    SubscribeRecordId = record.BorrowRecordId,
                    UserId = User.Current.UserId
                };

            _db.SubscribeRecords.Add(subscribeInfo);
            _db.SaveChanges();

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
                message = "Could not cancel a book borrowed by others user.";
                return false;
            }

            if (record.IsCheckedOut)
            {
                message = "Could not cancel a checked out book.";
                return false;
            }

            _db.BorrowRecords.Remove(record);
            _db.SaveChanges();

            message = string.Empty;
            return true;
        }

        public bool CheckOut(User user, long borrowId, out string message)
        {
            if (!user.IsAdmin)
            {
                message = "This operation needs admin's rights.";
                return false;
            }

            var record = _db.BorrowRecords.FirstOrDefault(i => i.BorrowRecordId == borrowId);
            if (record == null)
            {
                message = "The borrow record does not exist.";
                return false;
            }

            if (record.IsCheckedOut)
            {
                message = "The book had been checked out.";
                return false;
            }

            record.IsCheckedOut = true;
            _db.SaveChanges();

            message = string.Empty;
            return true;
        }

        public bool Return(User user, long borrowId, out string message)
        {
            if (!user.IsAdmin)
            {
                message = "This operation needs admin's rights.";
                return false;
            }

            var record = _db.BorrowRecords.FirstOrDefault(i => i.BorrowRecordId == borrowId);
            if (record == null)
            {
                message = "The borrow record does not exist.";
                return false;
            }

            if (!record.IsCheckedOut)
            {
                message = "The book has not been checked out.";
                return false;
            }

            _db.BorrowRecords.Remove(record);
            _db.SaveChanges();

            message = string.Empty;
            return true;
        }
        
        private bool IsBookBorrowed(long bookId)
        {
            return _recordCache.Any(r => r.BookId == bookId);
        }

        private Book GetBookById(long bookId)
        {
            var queryBooks = from b in _db.Books where b.BookId == bookId select b;
            return queryBooks.FirstOrDefault();
        }

        private bool UserArrieveBorrowLimit(long userId)
        {
            var borrowedCount = _recordCache.Count(r => r.UserId == userId);
            return borrowedCount >= BorrowLimit;
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
            string getBorrowLimitValue = GlobalConfigReader.ReadFromGlobalConfig("BorrowNumberLimit", "number");
            if (!string.IsNullOrEmpty(getBorrowLimitValue))
                number = Convert.ToInt32(getBorrowLimitValue);
            return number;
        }
    }
}