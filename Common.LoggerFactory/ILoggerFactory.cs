namespace Common.LoggerFactory
{
    /// <summary>
    /// Represents a logger factory used to create an instance of an <see cref="ILogger"/>.
    /// </summary>
    public interface ILoggerFactory
    {
        LoggerFactoryConfigurationSection ConfigurationSection { get; }
        ILogger CreateInstance(string loggerName);
    }
}