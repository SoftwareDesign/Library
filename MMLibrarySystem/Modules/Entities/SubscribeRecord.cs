using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookLibrary.Entities
{
    /// <summary>
    /// Represents a book subscribe record.
    /// </summary>
    public class SubscribeRecord
    {
        public SubscribeRecord()
        {
        }

        public SubscribeRecord(User user, Book book)
        {
            UserId = user.UserId;
            BookId = book.BookId;
            SubscribeDate = DateTime.Now;
        }

        public long SubscribeRecordId { get; set; }

        public long BookId { get; set; }

        public virtual Book Book { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime SubscribeDate { get; set; }
    }
}