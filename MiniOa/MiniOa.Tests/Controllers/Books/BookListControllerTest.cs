using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniOa.Books;
using MiniOa.Controllers.Books;
using Moq;

namespace MiniOa.Tests.Controllers.Books
{
    [TestClass]
    public class BookListControllerTest
    {
        private Mock<IBookList> _listMock;
        private BookListController _controller;
        private List<Book> _books;
        private Book _singleBook;

        [TestInitialize]
        public void TestCaseInitialize()
        {
            _books = new List<Book>();
            _singleBook = null;
            _listMock = new Mock<IBookList>();
            _listMock.Setup(l => l.GetBooks()).Returns(() => _books);
            _listMock.Setup(l => l.GetBook(It.IsAny<string>())).Returns(() => _singleBook);

            _controller = new BookListController(_listMock.Object);
        }

        [TestCleanup]
        public void TestCaseCleanup()
        {
            _controller = null;
            _listMock = null;
            _books = null;
        }

        [TestMethod]
        public void GetList_DefaultArgs_LimitedResult()
        {
            _books = new List<Book>
            {
                new Book("BID_001", "Book1", "Author1"),
                new Book("BID_002", "Book2", "Author2")
            };

            var books = _controller.Get();
            Assert.AreEqual(2, books.Count());
        }

        [TestMethod]
        public void GetBook_Exist_SameBid()
        {
            _singleBook = new Book("BID_321", "Book1", "Author1");

            var book = _controller.Get("BID_321");
            Assert.AreEqual("BID_321", book.Bid);
        }
    }
}
