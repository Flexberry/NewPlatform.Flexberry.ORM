namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Data;

    /// <summary>
    /// Обёртка над <see cref="IDbConnection" /> и <see cref="IDbTransaction" />.
    /// </summary>
    public class DbTransactionWrapper : IDisposable
    {
        private IDbTransaction _transaction;

        /// <summary>
        /// Initializes instance of <see cref="DbTransactionWrapper" />.
        /// </summary>
        /// <param name="dataService">The instance of <see cref="SQLDataService" /> class.</param>
        public DbTransactionWrapper(SQLDataService dataService)
        {
            if (dataService == null)
            {
                throw new ArgumentNullException(nameof(dataService));
            }

            Connection = dataService.GetConnection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionWrapper" /> class by <see cref="IDbConnection" /> and <see cref="IDbTransaction" />.
        /// </summary>
        /// <param name="connection">An object representing an open connection to a data source.</param>
        /// <param name="transaction">An object representing the transaction.</param>
        public DbTransactionWrapper(IDbConnection connection, IDbTransaction transaction = null)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        /// <summary>
        /// An object representing an open connection to a data source.
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// An object representing the transaction.
        /// </summary>
        public IDbTransaction Transaction => GetTransaction();

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <param name="sql">The text command to execute.</param>
        /// <returns>A Command object associated with the connection.</returns>
        public virtual IDbCommand CreateCommand(string sql = null)
        {
            var cmd = Connection.CreateCommand();
            cmd.Transaction = Transaction;
            if (sql != null)
            {
                cmd.CommandText = sql;
            }

            return cmd;
        }

        /// <summary>
        /// Commits the database transaction.
        /// FYI: does nothing if the transaction has not begun.
        /// </summary>
        public virtual void CommitTransaction()
        {
            // Для защиты от `This NpgsqlTransaction has completed; it is no longer usable.`
            if (_transaction?.Connection != null)
            {
                _transaction?.Commit();
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// FYI: does nothing if the transaction has not begun.
        /// </summary>
        public virtual void RollbackTransaction()
        {
            // Для защиты от `This NpgsqlTransaction has completed; it is no longer usable.`
            if (_transaction?.Connection != null)
            {
                _transaction?.Rollback();
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            _transaction?.Dispose();
            Connection?.Close();
        }

        private IDbTransaction GetTransaction()
        {
            if (_transaction != null)
            {
                return _transaction;
            }

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                _transaction = Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception("An error has been occurred during initialization of the transaction.", ex);
            }

            return _transaction;
        }
    }
}