using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using MMLibrarySystem.Schedule.Interfaces;
using MMLibrarySystem.Models;
using MMLibrarySystem.Utilities;

namespace MMLibrarySystem.Schedule.ScheduleRules
{
    public class OutOfDateRule : ISchedulable
    {
        public List<EmailContext> ExcuteScheduleRule(DbQuery<BorrowRecord> borrowRecords)
        {
            int limitDays = 31;
            string getBorrowDayLimitValue = GlobalConfigReader.ReadFromGlobalConfig("BorrowDayLimit", "days");
            if (!string.IsNullOrEmpty(getBorrowDayLimitValue))
                limitDays = Convert.ToInt32(getBorrowDayLimitValue);
            var emailContextList = new List<EmailContext>();
            var shouldReturnBookRecords = borrowRecords.Where(br => EntityFunctions.DiffDays(br.BorrowedDate, DateTime.Now) > limitDays);
            foreach (var shouldReturnBookRecord in shouldReturnBookRecords)
            {
                var emailContext = new EmailContext();
                emailContext.Adress = shouldReturnBookRecord.User.EmailAdress;
                emailContext.Subject = "You have book is out of date";
                emailContext.Body =
                    string.Format("You had borrowed the book {0} at {1} is out of date.{2} Please return it sooon",
                                  shouldReturnBookRecord.Book.BookType.Title,
                                  shouldReturnBookRecord.BorrowedDate.ToShortDateString(),
                                  Environment.NewLine);
                emailContextList.Add(emailContext);
            }

            return emailContextList;
        }
    }
}