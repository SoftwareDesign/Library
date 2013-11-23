using System;
using System.Collections.Generic;
using MMLibrarySystem.Models;
using MMLibrarySystem.ViewModels;

namespace MMLibrarySystem.Bll
{
    /// <summary>
    /// Represents the state and operations of a book in admin's view.
    /// </summary>
    public class AdminBookState
    {
        private const string StateInLib = "In Library";

        private const string StateCheckedOut = "Checked Out";

        private const string StateBorrowAccepted = "Borrow Accepted";

        private readonly BorrowRecord _borrowRecord;

        public AdminBookState(BorrowRecord borrowRecord)
        {
            if (borrowRecord == null)
            {
                throw new ArgumentNullException("borrowRecord");
            }

            _borrowRecord = borrowRecord;
        }

        public string State
        {
            get
            {
                if (_borrowRecord == null)
                {
                    return StateInLib;
                }

                return _borrowRecord.IsCheckedOut ? StateCheckedOut : StateBorrowAccepted;
            }
        }

        public UserOperation Operation
        {
            get
            {
                var recordId = _borrowRecord.BorrowRecordId;
                return _borrowRecord.IsCheckedOut ?
                    UserOperationFactory.CreateReturnOperation(recordId) :
                    UserOperationFactory.CreateCheckOutOperation(recordId);
            }
        }
    }
}