using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMLibrarySystem.Models
{
    public class Book
    {
        public long Id {get;set;}

        public string BookNumber { get; set; }

        public Double NetPrice { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string RequestedBy { get; set; }

        public string PurchaseUrl { get; set; }

        public virtual BookInfo BookInfo { get; set; }
    }
}