using System.Data;

namespace DatabaseFactory
{
    /// <summary>
    ///Represents a database object used to communicate with a relational database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets or sets the string used to open a database connection.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Create an IDbConnection to the data source.
        /// </summary>
        /// <returns>Returns an IDbConnection created using ConnectionString.</returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// Creates an IDbCommand that is executed while connected to the data source.
        /// </summary>
        /// <returns>Returns an IDbCommand.</returns>
        IDbCommand CreateCommand();

        /// <summary>
        /// Create an IDbConnection to the data source and open that connection.
        /// </summary>
        /// <returns>Returns an open IDbConnection created using ConnectionString.</returns>
        IDbConnection CreateOpenConnection();

        /// <summary>
        /// Creates an IDbCommand using the specified parameters and sets the command type to CommandType.Text.
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        IDbCommand CreateCommand(string commandText, IDbConnection connection);

        /// <summary>
        /// Creates an IDbCommand using the specified parameters and sets the command type to CommandTYpe.StoredProcedure.
        /// </summary>
        /// <param name="procName">The stored procedure to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);

        /// <summary>
        /// Creates an IDataParameter using the specified parameters.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="parameterValue">An Object that is the value of the IDataParameter.</param>
        /// <returns>Returns an IDataParameter.</returns>
        IDataParameter CreateParameter(string parameterName, object parameterValue);
    }
}
