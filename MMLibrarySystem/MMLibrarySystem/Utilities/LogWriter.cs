using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MMLibrarySystem.Utilities
{
    public class LogWriter
    {
        public static void Write(string context)
        {
            StreamWriter streamWriter;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "LogFile.log";
            streamWriter = File.Exists(filePath) ? new StreamWriter(filePath, true) : File.CreateText(filePath);
            streamWriter.WriteLine(DateTime.Now.ToString());
            streamWriter.WriteLine(context);
            streamWriter.Close();
        }
    }
}