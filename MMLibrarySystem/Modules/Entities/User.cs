using System.Linq;
using System.Web;

namespace MMLibrarySystem.Models
{
    /// <summary>
    /// Represents a user of the library.
    /// </summary>
    public class User
    {
        public long UserId { get; set; }

        public string LoginName { get; set; }

        public string FullName { get; set; }

        public string EmailAdress { get; set; }

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