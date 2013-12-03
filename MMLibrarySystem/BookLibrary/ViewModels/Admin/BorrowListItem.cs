
namespace BookLibrary.ViewModels.Admin
{
    /// <summary>
    /// Contains all the data of a borrow record which will be shown in the borrow list view.
    /// </summary>
    public class BorrowListItem
    {
        public string BorrowId { get; set; }

        public string BookId { get; set; }

        public string BookNumber { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string BorrowDate { get; set; }

        public string ReturnDate { get; set; }

        public string State { get; set; }

        public UserOperation Operation { get; set; }
    }
}