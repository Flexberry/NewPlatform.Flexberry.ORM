namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Асинхронная обёртка над <see cref="DbConnection" /> и <see cref="DbTransaction" />.
    /// </summary>
    public class DbTransactionWrapperAsync : IDisposable
    {
        private DbTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionWrapper" /> class.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTransactionWrapper" /> class.
        /// </summary>
        /// <param name="connection">Объект соединения, который будет использоваться в обёртке.</param>
        /// <param name="transaction">Транзакция.</param>
        public DbTransactionWrapperAsync(DbConnection connection, DbTransaction transaction = null)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        /// <summary>
        /// Объект соединения, используемый обёрткой.
        /// </summary>
        public DbConnection Connection { get; }

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <param name="sql">The text command to execute.</param>
        /// <returns>A Command object associated with the connection.</returns>
        public async Task<DbCommand> CreateCommandAsync(string sql = null)
        {
            var cmd = Connection.CreateCommand();
            cmd.Transaction = await GetTransactionAsync()
                .ConfigureAwait(false);

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
        public virtual async Task CommitTransaction()
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
        public virtual async Task RollbackTransaction()
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

        /// <summary>
        /// Открыть соединение и получить транзакцию.
        /// </summary>
        /// <returns>Транзакция.</returns>
        /// <exception cref="Exception">Ошибка во время открытия соединения/создания транзакции.</exception>
        public async Task<DbTransaction> GetTransactionAsync()
        {
            if (_transaction != null)
            {
                return _transaction;
            }

            try
            {
                bool connectionIsOpen = Connection.State.HasFlag(ConnectionState.Open);
                if (!connectionIsOpen)
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
