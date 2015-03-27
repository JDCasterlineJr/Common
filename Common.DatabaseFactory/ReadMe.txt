#Settings to go in the app.config
<configSections>
<section name="DatabaseFactoryConfiguration" type="Common.DatabaseFactory.DatabaseFactoryConfigurationSection, DatabaseFactory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
</configSections>

<connectionStrings>
<clear/>
<add name="DefaultConnectionString" providerName="System.Data.SqlClient" connectionString="Server=LOCALHOST;Database=Test; Trusted_Connection=True;" />
</connectionStrings>

//Namespace qualified name  
<DatabaseFactoryConfiguration Name="Common.DatabaseFactory.Databases.SqlServerDatabase" ConnectionStringName="DefaultConnectionString" />
//Assembly qualified name
<DatabaseFactoryConfiguration Name="Common.DatabaseFactory.Databases.SqlServerDatabase, Common.DatabaseFactory" ConnectionStringName="DefaultConnectionString" />
#Settings to go in the app.config

DatabaseFactoryConfiguration.Name = qualified name of the concrete implementation DatabaseFactory.Database to use

Example for query

//Create a database instance.
var _database = new DatabaseFactory().CreateDatabase();
//Create and open a connection to the database.
using (var connection = _database.CreateOpenConnection())
{
	//Create a command to interact with the database.
	using (var command = _database.CreateCommand("SELECT statement", connection))
	{
		//Execute the command process the data.
		using (var reader = command.ExecuteReader())
		{
			//process data here
		}
	}
}

Example for non query using transactions

//Create a database instance.
var _database = new DatabaseFactory().CreateDatabase();
//Create and open a connection to the database.
using (var connection = _database.CreateOpenConnection())
{
	using(var transaction = connection.BeginTransaction())
	{
		try
		{
			//Create a command to interact with the database.
			using (var command = _database.CreateCommand("INSERT/UPDATE/DELETE statement", connection, transaction)
				command.ExecuteNonQuery();

			transaction.Commit();
		}
		catch(Exception ex)
		{
			//log error
			try
			{
				transaction.Rollback();
			}
			catch(Exception ex)
			{
				//log error
			}
		}
	}
}