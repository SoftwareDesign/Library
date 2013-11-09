using MMLibrarySystem.Bll;

namespace MMLibrarySystem.Models.BookListController
{
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