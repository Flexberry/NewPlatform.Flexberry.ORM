namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Обёртка над <see cref="DbConnection" /> и <see cref="DbTransaction" /> (асинхронный вариант).
    /// </summary>
    public class DbTransactionWrapperAsync : DbTransactionWrapper
    {
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
            : base(connection, transaction) { }

        /// <inheritdoc/>
        public new DbConnection Connection { get; }

        /// <inheritdoc cref="DbTransactionWrapper.Transaction"/>
        /// TODO: после прекращения поддержки net45, использовать async методы DbTransaction (доступны с .Net Core 3.1).
        public new DbTransaction Transaction => GetTransaction().GetAwaiter().GetResult();

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <param name="sql">The text command to execute.</param>
        /// <returns>A Command object associated with the connection.</returns>
        public new DbCommand CreateCommand(string sql = null)
        {
            var cmd = Connection.CreateCommand();
            cmd.Transaction = Transaction;
            if (sql != null)
            {
                cmd.CommandText = sql;
            }

            return cmd;
        }

        private async Task<DbTransaction> GetTransaction()
        {
            if (_transaction != null)
            {
                return _transaction as DbTransaction;
            }

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    await Connection.OpenAsync();
                }

                _transaction = Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception("An error has been occurred during initialization of the transaction.", ex);
            }

            return _transaction as DbTransaction;
        }
    }
}