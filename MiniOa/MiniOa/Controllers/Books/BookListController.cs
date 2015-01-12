using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MiniOa.Books;

namespace MiniOa.Controllers.Books
{
    public class BookListController : ApiController
    {
        private readonly IBookList _bookList;

        public BookListController(IBookList bookList)
        {
            _bookList = bookList;
        }

        // GET: api/BookList
        public IEnumerable<Book> Get()
        {
            return _bookList.GetBooks();
        }

        // GET: api/BookList/5
        public Book Get(string bid)
        {
            return _bookList.GetBook(bid);
        }
    }
}
