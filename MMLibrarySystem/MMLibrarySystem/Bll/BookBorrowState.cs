using MMLibrarySystem.Models;
using MMLibrarySystem.ViewModels;

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

        private readonly long _bookId;

        private readonly BorrowRecord _borrowRecord;

        public BookBorrowState(long bookId)
        {
            _bookId = bookId;
            _borrowRecord = null;
        }

        public BookBorrowState(BorrowRecord borrowRecord)
        {
            _bookId = borrowRecord.BookId;
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

        public UserOperation GetUserOperation(User currentUser)
        {
            if (_borrowRecord == null)
            {
                return UserOperationFactory.CreateBorrowOperation(_bookId);
            }

            var cancelable = !_borrowRecord.IsCheckedOut && _borrowRecord.UserId == currentUser.UserId;
            return cancelable ? UserOperationFactory.CreateCancelOperation(_bookId) : null;
        }

        public UserOperation GetAdminOperation(User currentUser)
        {
            if (_borrowRecord == null)
            {
                return null;
            }

            if (currentUser.Role != (int)Roles.Admin)
            {
                return null;
            }

            var recordId = _borrowRecord.BorrowRecordId;
            return _borrowRecord.IsCheckedOut ?
                UserOperationFactory.CreateReturnOperation(recordId) :
                UserOperationFactory.CreateCheckOutOperation(recordId);
        }
    }
}