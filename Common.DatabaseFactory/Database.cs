using System;
using System.Data;

namespace Common.DatabaseFactory
{
    /// <summary>
    ///An abstract implementation of an <see cref="IDatabase"/>.
    /// </summary>
    public abstract class Database : IDatabase
    {
        /// <summary>
        /// Gets or sets the string used to open a database connection.
        /// </summary>
        public string ConnectionString { get; set; }

        #region Abstract Functions

        /// <summary>
        /// Create an IDbConnection to the data source.
        /// </summary>
        /// <returns>Returns an IDbConnection created using ConnectionString.</returns>
        public abstract IDbConnection CreateConnection();

        /// <summary>
        /// Creates an IDbCommand that is executed while connected to the data source.
        /// </summary>
        /// <returns>Returns an IDbCommand.</returns>
        public abstract IDbCommand CreateCommand();

        /// <summary>
        /// Create an IDbConnection to the data source and open that connection.
        /// </summary>
        /// <returns>Returns an open IDbConnection created using ConnectionString.</returns>
        public abstract IDbConnection CreateOpenConnection();

        /// <summary>
        /// Creates an IDbCommand using the specified parameters and sets the command type to CommandType.Text.
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);

        /// <summary>
        /// Creates an IDbCommand using the specified parameters and sets the command type to CommandTYpe.StoredProcedure.
        /// </summary>
        /// <param name="procName">The stored procedure to execute at the data source.</param>
        /// <param name="connection">The IDbConnection used by this instance of the IDbCommand.</param>
        /// <returns>Returns an IDbCommand.</returns>
        public abstract IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);

        /// <summary>
        /// Creates an IDataParameter using the specified parameters.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="parameterValue">An Object that is the value of the IDataParameter.</param>
        /// <returns>Returns an IDataParameter.</returns>
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);

        #endregion
    }
}