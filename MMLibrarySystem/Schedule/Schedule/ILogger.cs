namespace BookLibrary.Schedule
{
    /// <summary>
    /// A log service.
    /// </summary>
    public interface ILogger
    {
        void Log(string format, params object[] args);
    }
}
