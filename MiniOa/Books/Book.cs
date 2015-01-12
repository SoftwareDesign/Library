using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniOa.Books
{
    public class Book
    {
        public Book(string bookId, string name, string author)
        {
            Bid = bookId;
            Name = name;
            Author = author;
        }

        public string Bid { get; private set; }

        public string Name { get; private set; }

        public string Author { get; private set; }
    }
}
