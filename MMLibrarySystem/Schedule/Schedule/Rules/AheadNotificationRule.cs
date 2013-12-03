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
    internal class AheadNotificationRule : IBorrowCheckRule
    {
        public ReadOnlyCollection<MailMessage> Verify(IEnumerable<BorrowRecord> borrowRecords)
        {
            int aheadNotificationDays = 7;
            string getAheadNotificationDaysValue = GlobalConfigReader.ReadFromGlobalConfig("AheadNotification", "days");
            if (!string.IsNullOrEmpty(getAheadNotificationDaysValue))
                aheadNotificationDays = Convert.ToInt32(getAheadNotificationDaysValue);
            int limitDays = 31;
            string getBorrowDayLimitValue = GlobalConfigReader.ReadFromGlobalConfig("BorrowDayLimit", "days");
            if (!string.IsNullOrEmpty(getBorrowDayLimitValue))
                limitDays = Convert.ToInt32(getBorrowDayLimitValue);
            var emailContextList = new List<MailMessage>();
            var limitDay = DateTime.Now.AddDays(aheadNotificationDays - limitDays).Date;
            var shouldReturnBookRecords =
                borrowRecords.Where(br => br.BorrowedDate.Date == limitDay);

            foreach (var shouldReturnBookRecord in shouldReturnBookRecords)
            {
                var body = string.Format(
                    "The book [{0}] you borrowed will out of date at {1}.{2} Please take keep an eye open with it.",
                    shouldReturnBookRecord.Book.BookType.Title,
                    shouldReturnBookRecord.BorrowedDate.AddDays(limitDays).ToShortDateString(),
                    Environment.NewLine);
                var message = Utility.BuildMail(
                    shouldReturnBookRecord.User.EmailAdress,
                    "A borrowed book will out of data",
                    body);
                emailContextList.Add(message);
            }

            return emailContextList.AsReadOnly();
        }
    }
}