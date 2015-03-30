using System.Data;
using System.Data.SqlClient;

namespace DatabaseFactory.Databases
{
    /// <summary>
    /// An implementation of the <see cref="Database"/> class used to communicate with Sql Server.
    /// </summary>
    public class SqlServerDatabase : Database
    {
        /// <summary>
        /// Create an SqlConnection to the data source.
        /// </summary>
        /// <returns>Returns an IDbConnection created using ConnectionString.</returns>
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Creates an SqlCommand that is executed while connected to the data source.
        /// </summary>
        /// <returns>Returns an IDbCommand.</returns>
        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// Create an SqlConnection to the data source and open that connection.
        /// </summary>
        /// <returns>Returns an open IDbConnection created using ConnectionString.</returns>
        public override IDbConnection CreateOpenConnection()
        {
            var connection = (SqlConnection)CreateConnection();

            connection.Open();

            return connection;
        }

        /// <summary>
        /// Creates an SqlCommand using the specified parameters and sets the command type to CommandType.Text.
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            var command = (SqlCommand)CreateCommand();

            command.CommandText = commandText;
            command.Connection = connection as SqlConnection;
            command.CommandType = CommandType.Text;

            return command;
        }

        /// <summary>
        /// Creates an SqlCommand using the specified parameters and sets the command type to CommandTYpe.StoredProcedure.
        /// </summary>
        /// <param name="procName">The stored procedure to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            var command = (SqlCommand)CreateCommand();

            command.CommandText = procName;
            command.Connection = connection as SqlConnection;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        /// <summary>
        /// Creates an SqlParameter using the specified parameters.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="parameterValue">An Object that is the value of the IDataParameter.</param>
        /// <returns>Returns an IDataParameter.</returns>
        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter(parameterName, parameterValue);
        }
    }
}
