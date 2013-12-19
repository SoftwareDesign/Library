using System.Collections.Generic;
using System.Net.Mail;
using System.Timers;
using System;
using BookLibrary.Infrastructure;
using BookLibrary.Entities;

namespace BookLibrary.Schedule
{
    internal class DailyPlan : IDisposable
    {
        private readonly ILogger _logger;

        private Timer _timer;

        private IMailService _email;

        public DailyPlan(ILogger logger)
        {
            _logger = logger;
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
                _timer.Close();
                _timer = null;
            }
        }

        public void Pause()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        public void Continue()
        {
            if (_timer != null)
            {
                _timer.Start();
            }
        }

        private void BeginCheck(object sender, ElapsedEventArgs e)
        {
            try
            {
                var mails = ApplyRules();
                SendMails(mails);
            }
            catch (Exception exception)
            {
                _logger.Log("Got error while apply rules:\r\n{0}", exception);
                return;
            }
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