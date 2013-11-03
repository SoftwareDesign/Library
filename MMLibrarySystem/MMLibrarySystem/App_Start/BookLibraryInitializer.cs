using System;
using System.Collections.Generic;
using System.Data.Entity;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Models;

namespace MMLibrarySystem
{
    public class BookLibraryInitializer : DropCreateDatabaseIfModelChanges<BookLibraryContext>
    {
        protected override void Seed(BookLibraryContext context)
        {
            CreateTestBookInfo(context);
            CreateTestUserInfo(context);
            context.SaveChanges();

            base.Seed(context);
        }

        private static void CreateTestBookInfo(BookLibraryContext context)
        {
            for (var i = 0; i < 10; i++)
            {
                CreateTestBookType(context, i);
            }
        }

        private static void CreateTestBookType(BookLibraryContext context, int typeId)
        {
            var bookType =
                new BookType
                    {
                        Title = "Test Book " + typeId,
                        Description = "A book type for test with id " + typeId,
                        Publisher = "Publisher " + typeId,
                        Supplier = "Supplier " + typeId,
                        UserAndTeam = "Developer"
                    };

            for (var i = 0; i < 3; i++)
            {
                CreateTestBook(context, bookType, typeId, i);
            }

            context.BookTypes.Add(bookType);
        }

        private static void CreateTestBook(BookLibraryContext context, BookType type, int typeId, int bookId)
        {
            var book =
                new Book
                    {
                        BookType = type,
                        BookNumber = string.Format("BNT{0:D4}{1:D2}", typeId, bookId),
                        PurchaseDate = DateTime.Now
                    };
            context.Books.Add(book);
        }

        private static void CreateTestUserInfo(BookLibraryContext context)
        {
            var dev = new User { LoginName = "HOMEXW\\WQ", Role = (int)Roles.Admin };
            context.Users.Add(dev);
        }
    }
}