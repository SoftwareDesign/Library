using System.Collections.Generic;

namespace MiniOa.Books
{
    /// <summary>
    /// Service to provide static information of books in the library.
    /// </summary>
    public interface IBookList
    {
        List<Book> GetBooks();

        Book GetBook(string bid);
    }
}