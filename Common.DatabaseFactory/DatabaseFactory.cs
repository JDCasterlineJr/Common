using System;
using System.Configuration;

namespace Common.DatabaseFactory
{
    /// <summary>
    /// An implementation of <see cref="IDatabaseFactory"/> that uses reflection to create an instance of an <see cref="IDatabase"/>.
    /// </summary>
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly DatabaseFactoryConfigurationSection _configurationSection;

        public DatabaseFactory()
        {
            _configurationSection = (DatabaseFactoryConfigurationSection)ConfigurationManager.GetSection("DatabaseFactoryConfiguration");
        }
        /// <summary>
        /// The custom database factory section within the application configuration file.
        /// </summary>
        public DatabaseFactoryConfigurationSection ConfigurationSection
        {
            get { return _configurationSection; }
        }

        /// <summary>
        /// Creates <see cref="Database"/> used to interact with the data source.
        /// </summary>
        /// <returns>An instance of the Database class.</returns>
        public IDatabase CreateInstance()
        {
            // Verify a DatabaseFactoryConfiguration line exists in the web.config.
            if (ConfigurationSection == null || ConfigurationSection.Name.Length == 0)
            {
                throw new Exception("Database name not defined in DatabaseFactoryConfiguration section of web.config.");
            }

            try
            {
                // Find the class
                var database = Type.GetType(ConfigurationSection.Name);

                // Get it's constructor
                if (database != null)
                {
                    //Get the empty constructor
                    var constructor = database.GetConstructor(new Type[] { });

                    // Invoke it's constructor, which returns an instance.
                    if (constructor != null)
                    {
                        var createdObject = (Database)constructor.Invoke(null);

                        // Initialize the connection string property for the database.
                        createdObject.ConnectionString = ConfigurationSection.ConnectionString;

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