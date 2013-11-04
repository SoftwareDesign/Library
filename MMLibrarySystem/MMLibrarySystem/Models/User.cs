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
                    var loginName = CurrentLoginName;
                    current = FindUserByLoginName(loginName) ?? CreateGuest(loginName);
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

        public static User FindUserByLoginName(string loginName)
        {
            using (var db = new BookLibraryContext())
            {
                var users =
                    from u in db.Users
                    where u.LoginName == loginName
                    select u;
                return users.FirstOrDefault();
            }
        }

        public static User CreateGuest(string loginName)
        {
            return new User { LoginName = loginName, FullName = "Guest " + loginName, Role = (int)Roles.Guest };
        }
    }
}