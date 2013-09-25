using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMLibrarySystem.Models
{
    public class User
    {
        public long Id { get; set; }

        public string LogName { get; set; }

        public string FullName { get; set; }

        public int Roll { get; set; }
    }
}