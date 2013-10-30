namespace MMLibrarySystem.Bll
{
    /// <summary>
    /// Defines roles of the system.
    /// </summary>
    public enum Roles
    {
        /// <summary>
        /// Guests, readonly rights.
        /// </summary>
        Guest = 0,

        /// <summary>
        /// Employee, borrow books.
        /// </summary>
        Employee = 1,

        /// <summary>
        /// Admin, check out and check in books.
        /// </summary>
        Admin = 2
    }
}