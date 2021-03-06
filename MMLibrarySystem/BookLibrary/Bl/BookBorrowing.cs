﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BookLibrary.Entities;
using BookLibrary.Utilities;

namespace BookLibrary.Bl
{
    public class BookBorrowing
    {
        private static int BorrowLimit = GetBorrowLimit();

        private readonly BookLibraryContext _db;

        private readonly List<BorrowRecord> _borrowRecordCache;

        private readonly List<SubscribeRecord> _subscribeRecordCache;

        public BookBorrowing(BookLibraryContext db)
        {
            _db = db;
            _borrowRecordCache = _db.BorrowRecords.ToList();
            _subscribeRecordCache = _db.SubscribeRecords.OrderBy(r => r.SubscribeRecordId).ToList();
        }

        public CustomerBookState GetCustomerBookState(long bookId)
        {
            var record = _borrowRecordCache.FirstOrDefault(r => r.BookId == bookId);
            if (record != null)
            {
                var subscribeRecords = _subscribeRecordCache.Where(r => r.BookId == bookId);
                var borrowState = new CustomerBookState(_db, record, subscribeRecords);
                return borrowState;
            }
            else
            {
                return new CustomerBookState(bookId);
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
                message = string.Format("You can only borrow at most {0} books.", BorrowLimit);
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

            AddBorrowRecord(user.UserId, book.BookId);

            message = string.Empty;
            return true;
        }

        public bool SubscribeBook(User user, long bookId, out string message)
        {
            var record = _db.BorrowRecords.FirstOrDefault(r => r.BookId == bookId);
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

            var subscribeRecord = _db.SubscribeRecords.FirstOrDefault(r => r.BookId == bookId && r.UserId == Users.Current.UserId);
            if (subscribeRecord != null)
            {
                message = "You already subscribed the book.";
                return false;
            }

            var subscribeInfo = new SubscribeRecord
                {
                    BookId = bookId,
                    SubscribeDate = DateTime.Now,
                    SubscribeRecordId = record.BorrowRecordId,
                    UserId = Users.Current.UserId
                };

            _db.SubscribeRecords.Add(subscribeInfo);
            SubmitChanges();

            message = string.Empty;
            return true;
        }

        public bool CancelBorrow(User user, long bookId, out string message)
        {
            var record = _db.BorrowRecords.FirstOrDefault(r => r.BookId == bookId);
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

            RemoveBorrowRecord(record);

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
            SubmitChanges();

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

            RemoveBorrowRecord(record);

            message = string.Empty;
            return true;
        }

        private static int GetBorrowLimit()
        {
            int number = 5;
            string getBorrowLimitValue = GlobalConfigReader.ReadFromGlobalConfig("BorrowNumberLimit", "number");
            if (!string.IsNullOrEmpty(getBorrowLimitValue))
                number = Convert.ToInt32(getBorrowLimitValue);
            return number;
        }

        private void AddBorrowRecord(long userId, long bookId)
        {
            var borrowRecord = new BorrowRecord(userId, bookId);
            _db.BorrowRecords.Add(borrowRecord);
            SubmitChanges();

            OnBorrowRecordAdded(borrowRecord);
        }

        private void RemoveBorrowRecord(BorrowRecord record)
        {
            _db.BorrowRecords.Remove(record);
            SubmitChanges();

            OnBorrowRecordRemoved(record);
        }

        private void ApplyFirstSubscriberToBorrow(long bookId)
        {
            var firstSubscribe = _subscribeRecordCache.FirstOrDefault(r => r.BookId == bookId);
            if (firstSubscribe == null)
            {
                return;
            }

            _db.SubscribeRecords.Remove(firstSubscribe);
            var borrowRecord = new BorrowRecord(firstSubscribe.UserId, bookId);
            _db.BorrowRecords.Add(borrowRecord);
            SubmitChanges();

            OnBorrowRecordAdded(borrowRecord);
        }

        private void SubmitChanges()
        {
            _db.SaveChanges();
        }

        private void SendUserBorrowAcceptedNotify(BorrowRecord record)
        {
            var address = GetUeserEmailAddress(record.UserId);
            var book = record.BookId.ToString();
            if (record.Book != null)
            {
                book = record.Book.BookType.Title;
            }
            var body = string.Format(
                "The borrowing of book {0} has been accepted, please get the book from Admin ASAP.",
                book);
            var message = Utility.BuildMail(address,"Book Borrow Accepted", body);
            Infrastructures.Instance.Mail.Send(message);
        }

        private string GetUeserEmailAddress(long userId)
        {
            var user = _db.Users.First(u => u.UserId == userId);
            return user.EmailAdress;
        }
        
        private bool IsBookBorrowed(long bookId)
        {
            return _borrowRecordCache.Any(r => r.BookId == bookId);
        }

        private Book GetBookById(long bookId)
        {
            var queryBooks = from b in _db.Books where b.BookId == bookId select b;
            return queryBooks.FirstOrDefault();
        }

        private bool UserArrieveBorrowLimit(long userId)
        {
            var borrowedCount = _borrowRecordCache.Count(r => r.UserId == userId);
            return borrowedCount >= BorrowLimit;
        }

        private void OnBorrowRecordAdded(BorrowRecord borrowRecord)
        {
            SendUserBorrowAcceptedNotify(borrowRecord);
        }

        private void OnBorrowRecordRemoved(BorrowRecord record)
        {
            ApplyFirstSubscriberToBorrow(record.BookId);
        }
    }
}