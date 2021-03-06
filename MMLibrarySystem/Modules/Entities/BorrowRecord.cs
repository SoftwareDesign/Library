﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookLibrary.Entities
{
    /// <summary>
    /// Represents a not finished borrow record.
    /// </summary>
    public class BorrowRecord
    {
        public BorrowRecord()
        {
        }

        public BorrowRecord(long userId, long bookId)
        {
            UserId = userId;
            BookId = bookId;
            BorrowedDate = DateTime.Now;
            IsCheckedOut = false;
        }

        public long BorrowRecordId { get; set; }

        public long BookId { get; set; }

        public virtual Book Book { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsCheckedOut { get; set; }

        public DateTime BorrowedDate { get; set; }
    }
}