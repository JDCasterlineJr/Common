using System;
using System.Configuration;

namespace DatabaseFactory
{
    /// <summary>
    /// Represents a custom database factory section within the application configuration file.
    /// </summary>
    public sealed class DatabaseFactoryConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the qualified class name of a concrete implementation of DatabaseFactory.Database to use.
        /// </summary>
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)base["Name"]; }
        }

        /// <summary>
        /// Gets the name of a connection string specified in the ConnectionStrings settings within the configuration file.
        /// </summary>
        [ConfigurationProperty("ConnectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["ConnectionStringName"]; }
        }

        /// <summary>
        /// Gets the string used to open a database connection.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }
                catch (Exception excep)
                {
                    throw new Exception("Connection string " + ConnectionStringName + " was not found in web.config. " +
                                        excep.Message);
                }
            }
        }
    }
}