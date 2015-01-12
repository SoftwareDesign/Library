using System.Collections.Generic;
using MiniOa.Books;

namespace MiniOa.BookService
{
    public class BookList : IBookList
    {
        public List<Book> GetBooks()
        {
            var bookList = new List<Book>
            {
                new Book("BID_001", "Book1", "Author1"),
                new Book("BID_002", "Book2", "Author2")
            };
            return bookList;
        }

        public Book GetBook(string bid)
        {
            return new Book(bid, "Book1", "Author1");
        }
    }
}