namespace Common.LoggerFactory
{
    /// <summary>
    ///An abstract implementation of an <see cref="ILogger"/>.
    /// </summary>
    public abstract class Logger : ILogger
    {
        /// <summary>
        /// Writes the specified data to the log for the given <see cref="LogItem"/>.
        /// </summary>
        /// <param name="item"><see cref="LogItem"/> to log.</param>
        public abstract void Log(LogItem item);
    }
}