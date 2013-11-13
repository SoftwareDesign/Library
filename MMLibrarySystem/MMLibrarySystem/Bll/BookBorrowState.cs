using MMLibrarySystem.Models;

namespace MMLibrarySystem.Bll
{
    /// <summary>
    /// Represents the borrowing state of a book.
    /// </summary>
    public class BookBorrowState
    {
        private const string StateInLib = "In Library";

        private const string StateBorrowed = "Borrowed";

        private const string StateCheckedOut = "Checked Out";

        private const string StateBorrowAccepted = "Borrow Accepted";

        private const string OperationNone = "";

        private const string OperationBorrow = "Borrow";

        private const string OperationCancel = "Cancel";

        private const string OperationReturn = "Return";

        private const string OperationCheckOut = "CheckOut";

        private readonly BorrowRecord _borrowRecord;

        public BookBorrowState(BorrowRecord borrowRecord)
        {
            _borrowRecord = borrowRecord;
        }

        public string State
        {
            get { return _borrowRecord != null ? StateBorrowed : StateInLib; }
        }

        public string InternalState
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

        public string GetUserOperation(User user)
        {
            if (_borrowRecord == null)
            {
                return OperationBorrow;
            }

            var cancelable = !_borrowRecord.IsCheckedOut && _borrowRecord.UserId == user.UserId;
            return cancelable ? OperationCancel : OperationNone;
        }

        public string GetAdminOperation(User user)
        {
            if (_borrowRecord == null)
            {
                return OperationNone;
            }

            if (user.Role != (int)Roles.Admin)
            {
                return OperationNone;
            }

            return _borrowRecord.IsCheckedOut ? OperationReturn : OperationCheckOut;
        }
    }
}