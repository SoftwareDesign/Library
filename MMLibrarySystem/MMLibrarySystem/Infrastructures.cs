using MMLibrarySystem.Infrastructure.Email;
using MMLibrarySystem.Utilities;
using MMLibrarySystem.Infrastructure;

namespace MMLibrarySystem
{
    internal class Infrastructures
    {
        private static Infrastructures _instance;

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