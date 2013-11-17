using MMLibrarySystem.ViewModels.BookList;

namespace MMLibrarySystem.ViewModels.Admin
{
    /// <summary>
    /// Contains all information for an edit book operation.
    /// </summary>
    public class EditBookInfo : BookInfo
    {
        public string Operation { get; set; }
    }
}