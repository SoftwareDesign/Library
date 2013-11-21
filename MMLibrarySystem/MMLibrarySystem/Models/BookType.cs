using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMLibrarySystem.Models
{
    /// <summary>
    /// Represent a book-type, which may have more than one instance in library.
    /// </summary>
    public class BookType
    {
        public BookType()
        {
            Books = new List<Book>();
        }

        public long BookTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string UserAndTeam { get; set; }

        public string Publisher { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
}