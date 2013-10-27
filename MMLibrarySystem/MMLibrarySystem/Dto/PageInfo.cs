using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MMLibrarySystem.Dto
{
    public class PageInfo
    {
        public PageInfo()
        {
            XmlDocument doc=new XmlDocument();
            doc.Load("");
        }
        private int _totalBookNumber = 0;
        public int PageSize { get; private set; }

        public int TotalBookNumber
        {
            get { return _totalBookNumber; }
            set
            { 
                _totalBookNumber = value;
                PageCount = _totalBookNumber/PageSize + 1;
            }
        }

        public int PageCount { get; private set; }
    }
}