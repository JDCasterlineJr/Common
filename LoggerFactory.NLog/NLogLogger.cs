using NLog;

namespace LoggerFactory.NLogLogger
{
    /// <summary>
    /// An implementation of the <see cref="Logger"/> class that uses NLog for logging.
    /// </summary>
    public class NLogLogger : Logger
    {
        private NLog.Logger _logger;

        /// <summary>
        /// Initializes a new <see cref="NLogLogger"/> using the specified logger name. 
        /// </summary>
        /// <param name="name">Logger name.</param>
        public NLogLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// Writes the specified data to the log for the given <see cref="LogItem"/>.
        /// </summary>
        /// <param name="item"><see cref="LogItem"/> to log.</param>
        public override void Log(LogItem item)
        {
            switch (item.LogLevel)
            {
                case LogLevel.Trace:
                    _logger.Trace(item.Message, item.Exception);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(item.Message, item.Exception);
                    break;
                case LogLevel.Info:
                    _logger.Info(item.Message, item.Exception);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(item.Message, item.Exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(item.Message, item.Exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(item.Message, item.Exception);
                    break;
                default:
                    _logger.Info(item.Message, item.Exception);
                    break;
            }
        }
    }
}
