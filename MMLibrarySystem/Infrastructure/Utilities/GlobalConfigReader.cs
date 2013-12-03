using System;
using System.Collections.Generic;
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
    }
}