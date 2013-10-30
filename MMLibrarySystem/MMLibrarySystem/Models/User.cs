using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMLibrarySystem.Bll;

namespace MMLibrarySystem.Models
{
    public class User
    {
        /// <summary>
        /// Gets the current logged on user, null if no user logged on.
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                using (var db = new BookLibraryContext())
                {
                    var loginName = CurrentLoginName;
                    var users =
                        from u in db.Users
                        where u.LoginName == loginName
                        select u;
                    return users.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Gets name of the current logged on user.
        /// </summary>
        public static string CurrentLoginName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public long Id { get; set; }

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
    }
}