using MMLibrarySystem.Models;

namespace MMLibrarySystem.ViewModels.BookList
{
    /// <summary>
    /// Contains all static information of a book.
    /// </summary>
    public class BookInfo
    {
        public string BookId { get; set; }

        public string BookNumber { get; set; }

        public string NetPrice { get; set; }

        public string PurchaseDate { get; set; }

        public string RequestedBy { get; set; }

        public string PurchaseUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string UserAndTeam { get; set; }

        public string Publisher { get; set; }

        public string Supplier { get; set; }

        public static BookInfo Create(Book book)
        {
            var type = book.BookType;
            return new BookInfo
            {
                BookId = book.BookId.ToString(),
                BookNumber = book.BookNumber,
                NetPrice = book.NetPrice.ToString("F"),
                PurchaseDate = book.PurchaseDate.ToShortDateString(),
                RequestedBy = book.RequestedBy,
                PurchaseUrl = book.PurchaseUrl,
                Title = type.Title,
                Description = type.Description,
                UserAndTeam = type.UserAndTeam,
                Publisher = type.Publisher,
                Supplier = type.Supplier,
            };
        }
    }
}