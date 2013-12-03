using System.DirectoryServices;
using System.Linq;
using System.Web;
using BookLibrary.Entities;

namespace BookLibrary.Bl
{
    /// <summary>
    /// Helper class for user.
    /// </summary>
    public static class Users
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

#if DEBUG
            set
            {
                HttpContext.Current.Session[KeyCurrentUser] = value;
            }
#endif
        }

        /// <summary>
        /// Gets name of the current logged on user.
        /// </summary>
        public static string CurrentLoginName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public static User FindUserByLoginName(string loginName)
        {
            using (var db = new BookLibraryContext())
            {
                var users =
                    from u in db.Users
                    where u.LoginName == loginName
                    select u;
                var user = users.FirstOrDefault();
                if (user == null)
                {
                    using (
                        DirectoryEntry localMachine =
                            new DirectoryEntry("LDAP://OU=MM-SZ,OU=MM-Software,DC=corp,DC=mm-software,DC=com"))
                    {
                        var accountName = loginName.Substring(3);
                        DirectorySearcher deSearch = new DirectorySearcher();
                        deSearch.SearchRoot = localMachine;
                        deSearch.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + accountName + "))";
                        deSearch.SearchScope = SearchScope.Subtree;
                        SearchResult results = deSearch.FindOne();
                        if (results != null)
                        {
                            using (DirectoryEntry userInfo = new DirectoryEntry(results.Path))
                            {
                                var fullName = userInfo.Properties["name"].Value.ToString();
                                var emailAdress = userInfo.Properties["mail"].Value.ToString();
                                User newUser =new User();
                                newUser.LoginName = loginName;
                                newUser.FullName = fullName;
                                newUser.EmailAdress = emailAdress;
                                newUser.Role = (int)Roles.Customer;
                                db.Users.Add(newUser);
                                db.SaveChanges();
                                return newUser;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return user;
                }
            }
        }

        public static User CreateGuest(string loginName)
        {
            return new User { LoginName = loginName, FullName = "Guest " + loginName, Role = (int)Roles.Guest };
        }
    }
}