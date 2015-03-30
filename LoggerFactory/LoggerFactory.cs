using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace LoggerFactory
{
    /// <summary>
    /// An implementation of <see cref="ILoggerFactory"/> that uses reflection to create an instance of an <see cref="ILogger"/>.
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        private readonly LoggerFactoryConfigurationSection _configurationSection;

        /// <summary>
        /// Initializes an new <see cref="LoggerFactory"/> instance.
        /// </summary>
        public LoggerFactory()
        {
            _configurationSection = (LoggerFactoryConfigurationSection)ConfigurationManager.GetSection("LoggerFactoryConfiguration");
        }

        /// <summary>
        /// The custom logger factory section within the application configuration file.
        /// </summary>
        public LoggerFactoryConfigurationSection ConfigurationSection
        {
            get { return _configurationSection; }
        }

        /// <summary>
        /// Creates an <see cref="Logger"/> used to log data.
        /// </summary>
        /// <returns>An <see cref="ILogger"/></returns>
        public ILogger CreateInstance([CallerMemberName]string loggerName="")
        {
            // Verify a LoggerFactoryConfiguration line exists in the web.config.
            if (ConfigurationSection == null || ConfigurationSection.Name.Length == 0)
            {
                throw new Exception("Logger name not defined in LoggerFactoryConfiguration section of web.config.");
            }

            try
            {
                // Find the class
                var logger = Type.GetType(ConfigurationSection.Name);

                // Get it's constructor
                if (logger != null)
                {
                    var constructor = logger.GetConstructor(new[] { typeof(string) });

                    // Invoke it's constructor, which returns an instance.
                    if (constructor != null)
                    {
                        var createdObject = (Logger)constructor.Invoke(new []{loggerName});

                        // Initialize the connection string property for the database.
                        //createdObject.ConnectionString = ConfigurationSection.ConnectionString;

                        // Pass back the instance as a Database
                        return createdObject;
                    }
                }
            }
            catch (Exception excep)
            {
                throw new Exception("Error instantiating database " + ConfigurationSection.Name + ". " + excep.Message);
            }
            throw new Exception("Error instantiating database " + ConfigurationSection.Name + ".");
        }
    }
}
