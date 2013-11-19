﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MMLibrarySystem.Models;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Utilities;
using MMLibrarySystem.ViewModels;
using MMLibrarySystem.ViewModels.BookList;
using PagedList;

namespace MMLibrarySystem.Controllers
{
    public class BookListController : Controller
    {
        private const string StateInLib = "In Library";

        private BookBorrowing _bb;

        public ActionResult Index(string searchTerm = null, bool showInLibrary = false, int page = 1)
        {
            var pageSize = GetPageSize();

            IPagedList<BookListItem> bookList;
            using (var db = new BookLibraryContext())
            {
                _bb = new BookBorrowing(db);
                var tempBooks = db.Books.Include("BookType");

                if (showInLibrary)
                {
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        bookList =
                            tempBooks.OrderByDescending(b => b.BookNumber)
                                     .Select(CreateBookListItem)
                                     .Where(book => book.State == StateInLib)
                                     .ToPagedList(page, pageSize);
                    }
                    else
                    {
                        var searchResult =
                            tempBooks.Where(
                                b =>
                                b.BookType.Title.Contains(searchTerm) || b.BookType.Description.Contains(searchTerm));
                        bookList = searchResult.Select(CreateBookListItem)
                            .Where(book => book.State == StateInLib)
                            .ToPagedList(page, pageSize);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        bookList =
                            tempBooks.OrderByDescending(b => b.BookNumber)
                                     .Select(CreateBookListItem)
                                     .ToPagedList(page, pageSize);
                    }
                    else
                    {
                        var searchResult =
                            tempBooks.Where(
                                b =>
                                b.BookType.Title.Contains(searchTerm) || b.BookType.Description.Contains(searchTerm));
                        bookList = searchResult.Select(CreateBookListItem).ToPagedList(page, pageSize);
                    }
                }

                _bb = null;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_BookList", bookList);
            }

            return View(bookList);
        }

        public ActionResult BookDetail(string bookNumber)
        {
            Book book;
            using (var db = new BookLibraryContext())
            {
                book = db.Books.Include("BookType").FirstOrDefault(b => b.BookNumber == bookNumber);
            }

            if (book == null)
            {
                var errorAlert = string.Format(
                    "alert('Invalid book number [{0}]. Please contact the admin.');", bookNumber);
                return JavaScript(errorAlert);
            }

            var info = new BookDetailInfo();
            info.Operation = Models.User.Current.IsAdmin ? UserOperationFactory.CreateEditBookOperation(bookNumber) : null;
            info.LoadInfo(book);
            return View(info);
        }

        public ActionResult Borrow(long bookId)
        {
            string message;
            var user = Models.User.Current;
            if (user == null)
            {
                var errorAlert = string.Format(
                    "alert('Current user [{0}] has no rights to borrow books. Please contact the admin.');",
                    Models.User.CurrentLoginName);
                return JavaScript(errorAlert);
            }

            bool succeed;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.BorrowBook(user, bookId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult Subscribe(long bookId)
        {
            var user = Models.User.Current;

            bool succeed;
            string message;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.SubscribeBook(user, bookId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            return RedirectToAction("Index", "BookList");
        }

        public ActionResult Cancel(long bookId)
        {
            var user = Models.User.Current;

            bool succeed;
            string message;
            using (var db = new BookLibraryContext())
            {
                var bb = new BookBorrowing(db);
                succeed = bb.CancelBorrow(user, bookId, out message);
            }

            if (!succeed)
            {
                var errorAlert = string.Format("alert('{0}');", message);
                return JavaScript(errorAlert);
            }

            return RedirectToAction("Index", "BookList");
        }

        private int GetPageSize()
        {
            int size = 10;
            string getPageSizeValue = GlobalConfigReader.ReadFromGlobalConfig("PageInfo", "size");
            if (!string.IsNullOrEmpty(getPageSizeValue))
                size = Convert.ToInt32(getPageSizeValue);
            return size;
        }

        private BookListItem CreateBookListItem(Book book)
        {
            var state = _bb.GetBookBorrowState(book.BookId);
            var item = new BookListItem
            {
                BookId = book.BookId.ToString(),
                BookNumber = book.BookNumber,
                Title = UserOperationFactory.CreateBookDetailOperation(book.BookType.Title, book.BookNumber),
                Publisher = book.BookType.Publisher,
                PurchaseDate = book.PurchaseDate.ToShortDateString(),
                State = state.State,
                Operation = state.GetUserOperation(Models.User.Current)
            };

            return item;
        }
    }
}
