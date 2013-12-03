using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BookLibrary.Entities
{
    public class BookLibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<BookType> BookTypes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        public DbSet<SubscribeRecord> SubscribeRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.Configuration.LazyLoadingEnabled = false;
            base.OnModelCreating(modelBuilder);
        }
    }
}