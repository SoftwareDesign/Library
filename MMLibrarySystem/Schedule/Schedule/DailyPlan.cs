using System.Collections.Generic;
using System.Net.Mail;
using System.Timers;
using System;
using MMLibrarySystem.Infrastructure;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Schedule
{
    public class DailyPlan : IDisposable
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

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        public void BeginCheck(object sender, ElapsedEventArgs e)
        {
            var mails = ApplyRules();
            SendMails(mails);
        }

        private List<MailMessage> ApplyRules()
        {
            var mails = new List<MailMessage>();
            using (var db = new BookLibraryContext())
            {
                var borrowRecords = db.BorrowRecords.Include("User").Include("Book").Include("Book.BookType");
                var ruleList = RulesCollection.GetRules();
                foreach (var rule in ruleList)
                {
                    var notifications = rule.Verify(borrowRecords);
                    mails.AddRange(notifications);
                }
            }

            return mails;
        }

        private void SendMails(IEnumerable<MailMessage> mails)
        {
            foreach (var mail in mails)
            {
                _email.Send(mail);
            }
        }
    }
}