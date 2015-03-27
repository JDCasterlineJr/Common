namespace Common.LoggerFactory
{
    /// <summary>
    ///Represents a logger used to log messages.
    /// </summary>
    public interface ILogger
    {
        void Log(LogItem item);
    }
}