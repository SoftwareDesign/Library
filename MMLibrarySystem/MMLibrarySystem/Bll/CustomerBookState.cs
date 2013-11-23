using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MMLibrarySystem.Models;
using MMLibrarySystem.ViewModels;

namespace MMLibrarySystem.Bll
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
        }

        public CustomerBookState(BorrowRecord borrowRecord, IEnumerable<SubscribeRecord> subscribeRecords)
        {
            if (borrowRecord == null)
            {
                throw new ArgumentNullException("borrowRecord");
            }

            var currentUser = Models.User.Current;
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
        }

        public string State { get; private set; }

        public UserOperation Operation { get; private set; }

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