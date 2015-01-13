using System.Collections.Generic;

namespace MiniOa.Books
{
    /// <summary>
    /// Service to provide book management operations.
    /// </summary>
    public interface IBookAdmin
    {
        /// <summary>
        /// Register a new book into the library.
        /// </summary>
        /// <param name="bookInfo">The full book information, without BID.</param>
        /// <returns>The BID.</returns>
        string RegistBook(BookDetail bookInfo);

        /// <summary>
        /// Update the information of an exiting book.
        /// </summary>
        /// <param name="bookInfo">The updated book information.</param>
        void UpdateBook(BookDetail bookInfo);

        // TODO: design question, use a generic BookOperationInfo, or dedicated BookOrderInfo (nearly same members as BookBorrowInfo)?
        List<Book> GetOrderedBooks();

        List<BookBorrowInfo> GetBorrowedBooks();

        void LendBook(string bid);

        void ReturnBook(string bid);
    }
}