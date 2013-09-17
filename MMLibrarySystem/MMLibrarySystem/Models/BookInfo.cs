using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMLibrarySystem.Models
{
    public class BookInfo
    {
        public BookInfo()
        {
            Books = new List<Book>();
        }

        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string UserAndTeam { get; set; }

        public string Publisher { get; set; }

        public string Supplier { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
}