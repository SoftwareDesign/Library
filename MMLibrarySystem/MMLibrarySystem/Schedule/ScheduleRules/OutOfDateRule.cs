﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Net.Mail;
using MMLibrarySystem.Infrastructure;
using MMLibrarySystem.Models;
using MMLibrarySystem.Utilities;

namespace MMLibrarySystem.Schedule.ScheduleRules
{
    public class OutOfDateRule : ISchedulable
    {
        public List<MailMessage> ExcuteScheduleRule(DbQuery<BorrowRecord> borrowRecords)
        {
            int limitDays = 31;
            string getBorrowDayLimitValue = GlobalConfigReader.ReadFromGlobalConfig("BorrowDayLimit", "days");
            if (!string.IsNullOrEmpty(getBorrowDayLimitValue))
                limitDays = Convert.ToInt32(getBorrowDayLimitValue);
            var emailContextList = new List<MailMessage>();
            var shouldReturnBookRecords = borrowRecords.Where(br => EntityFunctions.DiffDays(br.BorrowedDate, DateTime.Now) > limitDays);
            foreach (var shouldReturnBookRecord in shouldReturnBookRecords)
            {
                var body = string.Format(
                    "You had borrowed the book {0} at {1} is out of date.{2} Please return it sooon",
                    shouldReturnBookRecord.Book.BookType.Title,
                    shouldReturnBookRecord.BorrowedDate.ToShortDateString(),
                    Environment.NewLine);
                var message = Utility.BuildMail(
                    shouldReturnBookRecord.User.EmailAdress,
                    "A borrowed book have to be returned",
                    body);
                emailContextList.Add(message);
            }

            return emailContextList;
        }
    }
}