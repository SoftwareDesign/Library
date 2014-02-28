using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using BookLibrary.Entities;
using BookLibrary.Utilities;
using BookLibrary.ViewModels;

namespace BookLibrary.Bl
{
    /// <summary>
    /// Represents the state and operations of a book in customer's view.
    /// </summary>
    public class CustomerBookState
    {
        private const string StateInLib = "In Library";

        private const string StateBorrowed = "Borrowed";

        private const string StateSubscribed = "Subscribed";

        public CustomerBookState(long bookId)
        {
            InitInLib(bookId);
            BorrowedBy = string.Empty;
            ReturnDate = string.Empty;
            SubscribedBy = string.Empty;
        }

        public CustomerBookState(BookLibraryContext db, BorrowRecord borrowRecord, IEnumerable<SubscribeRecord> subscribeRecords)
        {
            int limitDays = 31;
            string getBorrowDayLimitValue = GlobalConfigReader.ReadFromLibraryServiceConfig("BorrowDayLimit", "days");
            if (!string.IsNullOrEmpty(getBorrowDayLimitValue))
                limitDays = Convert.ToInt32(getBorrowDayLimitValue);

            if (borrowRecord == null)
            {
                throw new ArgumentNullException("borrowRecord");
            }

            var currentUser = Users.Current;
            if (borrowRecord.UserId == currentUser.UserId)
            {
                InitBorrowed(borrowRecord);
            }
            else if (subscribeRecords.Any(r => r.UserId == currentUser.UserId))
            {
                InitSubscribed();
            }
            else
            {
                InitCanSubscribe(borrowRecord.BookId);
            }

            BorrowedBy = GetUserName(db, borrowRecord.UserId);
            ReturnDate = borrowRecord.BorrowedDate.AddDays(limitDays).ToShortDateString();
            SubscribedBy = string.Join(", ", subscribeRecords.Select(s => GetUserName(db, s.UserId)));
        }

        public string State { get; private set; }

        public UserOperation Operation { get; private set; }

        public string BorrowedBy { get; private set; }

        public string ReturnDate { get; private set; }

        public string SubscribedBy { get; private set; }

        private static string GetUserName(BookLibraryContext db, long userId)
        {
            var user = db.Users.First(u => u.UserId == userId);
            return user.DisplayName;
        }

        private void InitInLib(long bookId)
        {
            State = StateInLib;
            Operation = UserOperationFactory.CreateBorrowOperation(bookId);
        }

        private void InitBorrowed(BorrowRecord borrowRecord)
        {
            State = StateBorrowed;
            Operation = borrowRecord.IsCheckedOut ? null : UserOperationFactory.CreateCancelOperation(borrowRecord.BookId);
        }

        private void InitSubscribed()
        {
            State = StateSubscribed;
            Operation = null;
        }

        private void InitCanSubscribe(long bookId)
        {
            State = StateBorrowed;
            Operation = UserOperationFactory.CreateSubscribeOperation(bookId);
        }
    }
}