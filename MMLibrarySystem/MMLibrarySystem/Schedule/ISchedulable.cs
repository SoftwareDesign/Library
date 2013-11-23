using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Schedule
{
    public interface ISchedulable
    {
        List<MailMessage> ExcuteScheduleRule(DbQuery<BorrowRecord> borrowRecords);
    }
}