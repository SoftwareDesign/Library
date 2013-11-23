﻿using System.Timers;
using MMLibrarySystem.Infrastructure;
using MMLibrarySystem.Models;
using MMLibrarySystem.Schedule.ScheduleRules;

namespace MMLibrarySystem.Schedule
{
    public class DailyPlan
    {
        private Timer _timer;

        private IMailService _email;

        public DailyPlan()
        {
            _email = Infrastructures.Instance.Mail;
            _timer = new Timer();
            _timer.Interval = 24*60*60*1000;
            _timer.Elapsed += BeginCheck;
            _timer.Start();
        }

        public void BeginCheck(object sender, ElapsedEventArgs e)
        {
            GetShouldReturnInfoes();
        }

        private void GetShouldReturnInfoes()
        {
            using (BookLibraryContext db = new BookLibraryContext())
            {
                var borrowRecords = db.BorrowRecords.Include("User").Include("Book").Include("Book.BookType");
                var ruleList = RulesCollection.GetRules();
                foreach (var rule in ruleList)
                {
                    var emailContextList = rule.ExcuteScheduleRule(borrowRecords);
                    foreach (var context in emailContextList)
                    {
                        _email.Send(context);
                    }
                }
            }
        }
    }
}