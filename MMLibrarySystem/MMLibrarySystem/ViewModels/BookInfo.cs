using System;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.ViewModels
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

        public void LoadInfo(Book book)
        {
            var type = book.BookType;
            BookId = book.BookId.ToString();
            BookNumber = book.BookNumber;
            NetPrice = book.NetPrice.ToString("F");
            PurchaseDate = book.PurchaseDate.ToShortDateString();
            RequestedBy = book.RequestedBy;
            PurchaseUrl = book.PurchaseUrl;
            Title = type.Title;
            Description = type.Description;
            UserAndTeam = type.UserAndTeam;
            Publisher = type.Publisher;
            Supplier = type.Supplier;
        }

        public void StoreInfo(Book book)
        {
            var type = book.BookType;
            book.BookNumber = BookNumber;
            book.NetPrice = float.Parse(NetPrice);
            book.PurchaseDate = DateTime.Parse(PurchaseDate);
            book.RequestedBy = RequestedBy;
            book.PurchaseUrl = PurchaseUrl;
            type.Title = Title;
            type.Description = Description;
            type.UserAndTeam = UserAndTeam;
            type.Publisher = Publisher;
            type.Supplier = Supplier;
        }
    }
}