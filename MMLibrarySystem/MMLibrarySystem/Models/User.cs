using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMLibrarySystem.Bll;

namespace MMLibrarySystem.Models
{
    /// <summary>
    /// Represents a user of the library.
    /// </summary>
    public class User
    {
        private const string KeyCurrentUser = "CurrentUser";

        /// <summary>
        /// Gets the current logged on user, null if no user logged on.
        /// </summary>
        public static User Current
        {
            get
            {
                var session = HttpContext.Current.Session;
                var current = (User)session[KeyCurrentUser];
                if (current == null)
                {
                    current = FindUserByLoginName(CurrentLoginName);
                    session[KeyCurrentUser] = current;
                }

                return current;
            }
        }

        /// <summary>
        /// Gets name of the current logged on user.
        /// </summary>
        public static string CurrentLoginName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public long UserId { get; set; }

        public string LoginName { get; set; }

        public string FullName { get; set; }

        public int Role { get; set; }

        public string DisplayName
        {
            get { return string.IsNullOrEmpty(FullName) ? LoginName : FullName; }
        }

        public bool IsAdmin
        {
            get { return Role == (int)Roles.Admin; }
        }

        private static User FindUserByLoginName(string loginName)
        {
            User user;
            using (var db = new BookLibraryContext())
            {
                var users =
                    from u in db.Users
                    where u.LoginName == loginName
                    select u;
                user = users.FirstOrDefault();
            }

            return user ?? new User { LoginName = "Guest", Role = (int)Roles.Guest };
        }
    }
}