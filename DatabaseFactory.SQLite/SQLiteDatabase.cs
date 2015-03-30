using System;
using System.Data;
using System.Data.SQLite;

namespace DatabaseFactory.SQLite
{
    /// <summary>
    /// An implementation of the <see cref="Database"/> class used to communicate with Sql Server.
    /// </summary>
    public class SQLiteDatabase : Database
    {
        /// <summary>
        /// Create an SQLiteConnection to the data source.
        /// </summary>
        /// <returns>Returns an IDbConnection created using ConnectionString.</returns>
        public override IDbConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        /// <summary>
        /// Creates an SQLiteCommand that is executed while connected to the data source.
        /// </summary>
        /// <returns>Returns an IDbCommand.</returns>
        public override IDbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        /// <summary>
        /// Create an SQLiteConnection to the data source and open that connection.
        /// </summary>
        /// <returns>Returns an open IDbConnection created using ConnectionString.</returns>
        public override IDbConnection CreateOpenConnection()
        {
            var connection = (SQLiteConnection)CreateConnection();

            connection.Open();

            return connection;
        }

        /// <summary>
        /// Creates an SQLiteCommand using the specified parameters and sets the command type to CommandType.Text.
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            var command = (SQLiteCommand)CreateCommand();

            command.CommandText = commandText;
            command.Connection = connection as SQLiteConnection;
            command.CommandType = CommandType.Text;

            return command;
        }

        /// <summary>
        /// SQLite does not support stored procedures.
        /// </summary>
        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        { 
            throw new NotSupportedException("SQLite does not support stored procedures.");
        }

        /// <summary>
        /// Creates an SQLiteParameter using the specified parameters.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="parameterValue">An Object that is the value of the IDataParameter.</param>
        /// <returns>Returns an IDataParameter.</returns>
        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SQLiteParameter(parameterName, parameterValue);
        }
    }
}