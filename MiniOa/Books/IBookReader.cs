namespace MiniOa.Books
{
    /// <summary>
    /// Service to provide operations for book readers.
    /// </summary>
    public interface IBookReader
    {
        void OrderBook(string bid);

        void CancelOrder(string bid);
        
        void SubscribeBook(string bid);

        void CancelSubscribe(string bid);
    }
}