using System;

namespace LoggerFactory
{
    /// <summary>
    /// Log data that can be used by any <see cref="ILogger"/>
    /// </summary>
    public class LogItem
    {
        /// <summary>
        /// Message to log
        /// Defaults to <c>String.Empty</c>.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The <see cref="LogLevel"/> 
        /// Defaults to <see cref="LogLevel.Info"/>.
        /// </summary>
        public LogLevel LogLevel { get; set; }
        /// <summary>
        /// (Optional)Exception to log
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Initializes an new <see cref="LogItem"/> instance with default values.
        /// </summary>
        public LogItem()
        {
            Message = string.Empty;
            LogLevel = LogLevel.Info;
        }
    }
}
