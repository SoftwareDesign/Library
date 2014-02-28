using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BookLibrary.Utilities
{
    public class LogWriter
    {
        public static void Write(string context)
        {
            StreamWriter streamWriter;
            var rootpath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            var directory = new DirectoryInfo(rootpath);
            var rootPath = directory.Parent.FullName;
            var companyName = "SIG";
            var productName = "LibrarySystem";

            var filePath = Path.Combine(rootPath, companyName, productName, "LogFile.log");

            streamWriter = File.Exists(filePath) ? new StreamWriter(filePath, true) : File.CreateText(filePath);
            streamWriter.WriteLine(DateTime.Now.ToString());
            streamWriter.WriteLine(context);
            streamWriter.Close();
        }
    }
}