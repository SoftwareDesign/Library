
namespace MMLibrarySystem.ViewModels.BookList
{
    /// <summary>
    /// Contains all the data of a book which will be shown in the book list view.
    /// </summary>
    public class BookListItem
    {
        public string BookId { get; set; }

        public string BookNumber { get; set; }

        public string Title { get; set; }

        public string Publisher { get; set; }

        public string PurchaseDate { get; set; }

        public string State { get; set; }

        public string Operation { get; set; }
    }
}