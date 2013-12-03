using BookLibrary.Infrastructure.Email;
using BookLibrary.Utilities;
using BookLibrary.Infrastructure;

namespace BookLibrary
{
    public class Infrastructures
    {
        private static Infrastructures _instance = new Infrastructures();

        private Infrastructures()
        {
            if (DomainHelper.InDomainEnvironment())
            {
                Initialize();
            }
            else
            {
                InitializeMock();
            }
        }

        public static Infrastructures Instance
        {
            get
            {
                return _instance;
            }
        }

        public IMailService Mail { get; private set; }

        private void Initialize()
        {
            Mail = new SmtpMailService();
        }

        private void InitializeMock()
        {
            Mail = new MailServiceMock();
        }
    }
}