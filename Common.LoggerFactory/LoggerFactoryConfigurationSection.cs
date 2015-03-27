using System.Configuration;

namespace Common.LoggerFactory
{
    /// <summary>
    /// Represents a custom database factory section within the application configuration file.
    /// </summary>
    public sealed class LoggerFactoryConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the qualified class name of an implementation of <see cref="ILoggerFactory"/> to use.
        /// </summary>
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)base["Name"]; }
        }
    }
}
