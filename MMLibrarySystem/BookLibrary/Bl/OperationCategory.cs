namespace BookLibrary.Bl
{
    /// <summary>
    /// Defines operation category in the system.
    /// </summary>
    public enum OperationCategory
    {
        /// <summary>
        /// Default operaton, view the default page for the role.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Query the books.
        /// </summary>
        QueryBook,

        /// <summary>
        /// Borrow the books.
        /// </summary>
        BorrowBook,

        /// <summary>
        /// Check in and check out books.
        /// </summary>
        CheckInOutBook,

        /// <summary>
        /// Register a new book.
        /// </summary>
        RegisterBook,

        /// <summary>
        /// Manager role of users.
        /// </summary>
        ManagerUserRole
    }
}