using System.Collections.Generic;
using System.Net.Mail;
using System.Timers;
using MMLibrarySystem.Infrastructure;
using MMLibrarySystem.Models;

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
            var mails = ApplyRules();
            SendMails(mails);
        }

        private List<MailMessage> ApplyRules()
        {
            var mails = new List<MailMessage>();
            using (BookLibraryContext db = new BookLibraryContext())
            {
                var borrowRecords = db.BorrowRecords.Include("User").Include("Book").Include("Book.BookType");
                var ruleList = RulesCollection.GetRules();
                foreach (var rule in ruleList)
                {
                    var notifications = rule.ExcuteScheduleRule(borrowRecords);
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