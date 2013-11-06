using System;
using System.Collections.Generic;
using System.Data.Entity;
using MMLibrarySystem.Bll;
using MMLibrarySystem.Models;

namespace MMLibrarySystem
{
    public class BookLibraryInitializer : DropCreateDatabaseIfModelChanges<BookLibraryContext>
    {
        private static readonly string[] Books = { "C++", "C#", "Java", "Office" };

        private static readonly string[] Publishers = { "Electronics Industry", "Machine Press", "Posts & Telecom" };

        private static readonly string[] Suppliers = { "jd.com", "z.cn", };

        protected override void Seed(BookLibraryContext context)
        {
            CreateTestBookInfo(context);
            CreateTestUserInfo(context);
            context.SaveChanges();

            base.Seed(context);
        }

        private static void CreateTestBookInfo(BookLibraryContext context)
        {
            for (var i = 1; i < 13; i++)
            {
                CreateTestBookType(context, i);
            }
        }

        private static void CreateTestBookType(BookLibraryContext context, int typeId)
        {
            var title = GetTitle(typeId);
            var bookType =
                new BookType
                    {
                        Title = title,
                        Description = "A test book " + title,
                        Publisher = GetPublisher(typeId),
                        Supplier = GetSupplier(typeId)
                    };
            context.BookTypes.Add(bookType);

            var random = new Random();
            var bookCount = random.Next(1, 5);
            for (var i = 1; i < bookCount; i++)
            {
                CreateTestBook(context, bookType, typeId, i);
            }
        }

        private static string GetTitle(int typeId)
        {
            var index = typeId % Books.Length;
            return string.Concat(Books[index], " Book ", typeId);
        }

        private static string GetPublisher(int typeId)
        {
            var index = typeId % Publishers.Length;
            return string.Concat(Publishers[index]);
        }

        private static string GetSupplier(int typeId)
        {
            var index = typeId % Suppliers.Length;
            return string.Concat(Suppliers[index]);
        }

        private static void CreateTestBook(BookLibraryContext context, BookType type, int typeId, int bookId)
        {
            var book =
                new Book
                    {
                        BookType = type,
                        BookNumber = string.Format("BNT{0:D4}{1:D2}", typeId, bookId),
                        PurchaseDate = GetPurchaseData()
                    };
            context.Books.Add(book);
        }

        private static DateTime GetPurchaseData()
        {
            var random = new Random();
            var pastDays = random.Next(-1000, -1);
            return DateTime.Now.AddDays(pastDays);
        }

        private static void CreateTestUserInfo(BookLibraryContext context)
        {
            var dev = new User { LoginName = "HOMEXW\\WQ", Role = (int)Roles.Admin };
            context.Users.Add(dev);
        }
    }
}