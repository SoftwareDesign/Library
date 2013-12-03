using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Schedule
{
    public interface IBorrowCheckRule
    {
        ReadOnlyCollection<MailMessage> Verify(IEnumerable<BorrowRecord> borrowRecords);
    }
}