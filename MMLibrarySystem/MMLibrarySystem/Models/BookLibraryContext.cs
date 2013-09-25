using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MMLibrarySystem.Models
{
    public class BookLibraryContext : DbContext
    {       
        public DbSet<Book> Books { get; set; }

        public DbSet<BookInfo> BookInfos { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BorrowInfo> BorrowInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.Configuration.LazyLoadingEnabled = false;
            base.OnModelCreating(modelBuilder);            
        }
    }
}