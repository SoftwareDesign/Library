namespace BookLibrary.Entities
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
        /// Customer, borrow books.
        /// </summary>
        Customer = 1,

        /// <summary>
        /// Admin, regiser new book, check out and check in books.
        /// </summary>
        Admin = 2
    }
}