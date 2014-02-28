using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BookLibrary.Utilities
{
    public class GlobalConfigReader
    {
        public static string ReadFromGlobalConfig(string nodeName, string attributeName)
        {
            string configValue = string.Empty;

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "GlobalConfig.xml";
            XElement xe = XElement.Load(filePath);
            IEnumerable<XElement> rootCatalog = from root in xe.Elements(nodeName) select root;
            var firstOrDefault = rootCatalog.FirstOrDefault();
            if (firstOrDefault != null)
                configValue = firstOrDefault.Attribute(attributeName).Value;
            return configValue;
        }

        public static string ReadFromLibraryServiceConfig(string nodeName, string attributeName)
        {
            string configValue = string.Empty;

            var rootpath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            var directory = new DirectoryInfo(rootpath);
            var rootPath = directory.Parent.FullName;
            var companyName = "SIG";
            var productName = "LibrarySystem";

            var filePath = Path.Combine(rootPath, companyName, productName, "LibraryServiceConfig.xml");

            XElement xe = XElement.Load(filePath);
            IEnumerable<XElement> rootCatalog = from root in xe.Elements(nodeName) select root;
            var firstOrDefault = rootCatalog.FirstOrDefault();
            if (firstOrDefault != null)
                configValue = firstOrDefault.Attribute(attributeName).Value;
            return configValue;
        }
    }
}