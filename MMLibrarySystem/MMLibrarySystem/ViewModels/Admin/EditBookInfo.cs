namespace MMLibrarySystem.ViewModels.Admin
{
    /// <summary>
    /// Contains all information for the edit book view.
    /// </summary>
    public class EditBookInfo : BookInfo
    {
        public string PageTitle { get; set; }

        public string Operation { get; set; }

        public string Confirmmation { get; set; }
    }
}