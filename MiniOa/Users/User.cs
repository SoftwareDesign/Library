using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniOa.Users
{
    /// <summary>
    /// Represents an User in the OA system.
    /// </summary>
    public class User
    {
        public User(string uid, string fullName)
        {
            Uid = uid;
            FullName = fullName;
        }

        public string Uid { get; private set; }

        public string FullName { get; private set; }
    }
}
