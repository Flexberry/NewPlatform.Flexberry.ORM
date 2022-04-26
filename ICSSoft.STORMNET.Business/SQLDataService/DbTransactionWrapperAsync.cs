namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Обёртка над <see cref="DbConnection" /> и <see cref="DbTransaction" />.
    /// </summary>
    public class DbTransactionWrapperAsync : IDisposable
    {
        private DbTransaction _transaction;

        /// <summary>
        /// Initializes instance of <see cref="DbTransactionWrapper" />.
        /// </summary>
        /// <param name="dataService">The instance of <see cref="SQLDataService" /> class.</param>
        public DbTransactionWrapperAsync(SQLDataService dataService)
        {
            if (dataService == null)
            {
                throw new ArgumentNullException(nameof(dataService));
            }

            Connection = dataService.GetDbConnection();
        }

        /// <inheritdoc/>
        public DbTransactionWrapperAsync(DbConnection connection, DbTransaction transaction = null)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        /// <inheritdoc/>
        public DbConnection Connection { get; }

        /// <inheritdoc cref="DbTransactionWrapper.Transaction"/>
        public DbTransaction Transaction => GetTransaction().GetAwaiter().GetResult();

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <param name="sql">The text command to execute.</param>
        /// <returns>A Command object associated with the connection.</returns>
        public DbCommand CreateCommand(string sql = null)
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
        /// Для защиты от `This NpgsqlTransaction has completed; it is no longer usable.`.
        /// </summary>
#if NETSTANDARD2_1
        public virtual async void CommitTransaction()
        {
            if (_transaction?.Connection != null)
            {
                await _transaction?.CommitAsync()
                    .ConfigureAwait(false);
            }
        }
#else
        public virtual void CommitTransaction()
        {
            if (_transaction?.Connection != null)
            {
                _transaction?.Commit();
            }
        }
#endif

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// FYI: does nothing if the transaction has not begun.
        /// Для защиты от `This NpgsqlTransaction has completed; it is no longer usable.`.
        /// </summary>
#if NETSTANDARD2_1
        public virtual async void RollbackTransaction()
        {
            if (_transaction?.Connection != null)
            {
                await _transaction?.RollbackAsync()
                    .ConfigureAwait(false);
            }
        }
#else
        public virtual void RollbackTransaction()
        {
            if (_transaction?.Connection != null)
            {
                _transaction?.Rollback();
            }
        }
#endif

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _transaction?.Dispose();
            Connection?.Close();
        }

        private async Task<DbTransaction> GetTransaction()
        {
            if (_transaction != null)
            {
                return _transaction;
            }

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    await Connection.OpenAsync()
                        .ConfigureAwait(false);
                }

                _transaction =
                #if NETSTANDARD2_1
                    await Connection.BeginTransactionAsync()
                        .ConfigureAwait(false);
                #else
                    Connection.BeginTransaction();
                #endif
            }
            catch (Exception ex)
            {
                throw new Exception("An error has been occurred during initialization of the transaction.", ex);
            }

            return _transaction;
        }
    }
}