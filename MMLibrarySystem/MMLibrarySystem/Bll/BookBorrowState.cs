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
                return "Borrow";
            }

            var cancelable = !_borrowRecord.IsCheckedOut && _borrowRecord.UserId == user.UserId;
            return cancelable ? "Cancel" : string.Empty;
        }
    }
}