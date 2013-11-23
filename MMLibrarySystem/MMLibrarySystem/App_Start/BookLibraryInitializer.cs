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

        private Random _random = new Random();

        protected override void Seed(BookLibraryContext context)
        {
            CreateTestBookInfo(context);
            CreateTestUserInfo(context);
            context.SaveChanges();

            base.Seed(context);
        }

        private void CreateTestBookInfo(BookLibraryContext context)
        {
            for (var i = 1; i < 13; i++)
            {
                CreateTestBookType(context, i);
            }
        }

        private void CreateTestBookType(BookLibraryContext context, int typeId)
        {
            var title = GetTitle(typeId);
            var bookType =
                new BookType
                    {
                        Title = title,
                        Description = "A test book " + title,
                        Publisher = GetPublisher(typeId),
                    };
            context.BookTypes.Add(bookType);

            var bookCount = _random.Next(2, 4);
            for (var i = 1; i < bookCount; i++)
            {
                CreateTestBook(context, bookType, typeId, i);
            }
        }

        private string GetTitle(int typeId)
        {
            var index = typeId % Books.Length;
            return string.Concat(Books[index], " Book ", typeId);
        }

        private string GetPublisher(int typeId)
        {
            var index = typeId % Publishers.Length;
            return string.Concat(Publishers[index]);
        }

        private string GetSupplier(int typeId)
        {
            var index = typeId % Suppliers.Length;
            return string.Concat(Suppliers[index]);
        }

        private void CreateTestBook(BookLibraryContext context, BookType type, int typeId, int bookId)
        {
            var book =
                new Book
                    {
                        BookType = type,
                        BookNumber = string.Format("BNT{0:D4}{1:D2}", typeId, bookId),
                        PurchaseDate = GetPurchaseDate(),
                        Supplier = GetSupplier(typeId)
                    };
            context.Books.Add(book);
        }

        private DateTime GetPurchaseDate()
        {
            var pastDays = _random.Next(-1000, -1);
            return DateTime.Now.AddDays(pastDays);
        }

        private void CreateTestUserInfo(BookLibraryContext context)
        {
            var developers = new []
            {
                new User {LoginName = "HOMEXW\\WQ", Role = (int)Roles.Admin, EmailAdress = "pxu@home.net", FullName = "Pingsheng@Home" },
                new User {LoginName = "MM\\PXu", Role = (int)Roles.Admin, EmailAdress = "pxu@mm-software.com", FullName = "Pingsheng@MM" },
                new User {LoginName = "MM\\JYe", Role = (int)Roles.Admin, EmailAdress = "jye@mm-software.com", FullName = "Jining@MM" },
                new User {LoginName = "MM\\Lma", Role = (int)Roles.Admin, EmailAdress = "lma@mm-software.com", FullName = "Liang@MM" }
            };

            foreach (var user in developers)
            {
                context.Users.Add(user);
            }
        }
    }
}