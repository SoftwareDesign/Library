using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Schedule.Interfaces
{
    public interface ISchedulable
    {
        List<EmailContext> ExcuteScheduleRule(DbQuery<BorrowRecord> borrowRecords);
    }
}