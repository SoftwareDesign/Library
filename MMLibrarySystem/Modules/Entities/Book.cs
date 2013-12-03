using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookLibrary.Entities
{
    /// <summary>
    /// Represents a book instance.
    /// </summary>
    public class Book
    {
        public long BookId { get; set; }

        public string BookNumber { get; set; }

        public Double NetPrice { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string RequestedBy { get; set; }

        public string PurchaseUrl { get; set; }

        public string Supplier { get; set; }

        public long BookTypeId { get; set; }

        public virtual BookType BookType { get; set; }
    }
}