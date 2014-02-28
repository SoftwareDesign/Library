using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using BookLibrary.Infrastructure;
using BookLibrary.Entities;
using BookLibrary.Utilities;

namespace BookLibrary.Schedule.ScheduleRules
{
    internal class OutOfDateRule : IBorrowCheckRule
    {
        public ReadOnlyCollection<MailMessage> Verify(IEnumerable<BorrowRecord> borrowRecords)
        {
            int limitDays = 31;
            string getBorrowDayLimitValue = GlobalConfigReader.ReadFromLibraryServiceConfig("BorrowDayLimit", "days");
            if (!string.IsNullOrEmpty(getBorrowDayLimitValue))
                limitDays = Convert.ToInt32(getBorrowDayLimitValue);
            var emailContextList = new List<MailMessage>();
            var limitDay = DateTime.Now.AddDays(-limitDays);
            var shouldReturnBookRecords = borrowRecords.Where(br => br.BorrowedDate < limitDay);
            foreach (var shouldReturnBookRecord in shouldReturnBookRecords)
            {
                var body = string.Format(
                    "The book [{0}] you borrowed at {1} is out of date.{2} Please return it ASAP.",
                    shouldReturnBookRecord.Book.BookType.Title,
                    shouldReturnBookRecord.BorrowedDate.ToShortDateString(),
                    Environment.NewLine);
                var message = Utility.BuildMail(
                    shouldReturnBookRecord.User.EmailAdress,
                    "A borrowed book have to be returned",
                    body);
                emailContextList.Add(message);
            }

            return emailContextList.AsReadOnly();
        }
    }
}