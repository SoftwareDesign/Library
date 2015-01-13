using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniOa.Books
{
    /// <summary>
    /// Contains basic information of a book.
    /// </summary>
    public class Book
    {
        public Book()
        {
        }

        public Book(string bookId, string name, string author)
        {
            Bid = bookId;
            Name = name;
            Author = author;
        }

        public string Bid { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }
    }
}
