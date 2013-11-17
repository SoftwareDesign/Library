namespace MMLibrarySystem.ViewModels.BookList
{
    /// <summary>
    /// Contains all information for the book detail view.
    /// </summary>
    public class BookDetailInfo : BookInfo
    {
        public UserOperation Operation { get; set; }
    }
}