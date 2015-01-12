using System.Collections.Generic;

namespace MiniOa.Books
{
    public interface IBookList
    {
        List<Book> GetBooks();

        Book GetBook(string bid);
    }
}