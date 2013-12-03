using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using BookLibrary.Entities;

namespace BookLibrary.Schedule
{
    internal interface IBorrowCheckRule
    {
        ReadOnlyCollection<MailMessage> Verify(IEnumerable<BorrowRecord> borrowRecords);
    }
}