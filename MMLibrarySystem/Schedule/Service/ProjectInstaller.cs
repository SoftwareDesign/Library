using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace BookLibrary.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            CopyTheConfigFile();
        }

        private void CopyTheConfigFile()
        {
            var fileName = "LibraryServiceConfig.xml";
            
            var path = GetDeployPath();
            var file = Path.Combine(path, fileName);

            if (File.Exists(file))
            {
                return;
            }

            var fileFullName = "BookLibrary.Resources." + fileName;
            var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileFullName);

            var fileStream = File.Create(file);
            xmlStream.CopyTo(fileStream);
            fileStream.Close();
        }

        private string GetDeployPath()
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            var directory = new DirectoryInfo(root);
            var rootPath = directory.Parent.FullName;
            var companyName = "SIG";
            var productName = "LibrarySystem";

            var filePath = Path.Combine(rootPath, companyName, productName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            return filePath;
        }
    }
}
