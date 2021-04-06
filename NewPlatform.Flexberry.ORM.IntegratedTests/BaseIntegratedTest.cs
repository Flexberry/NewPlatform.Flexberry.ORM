﻿[assembly: Xunit.CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]

namespace NewPlatform.Flexberry.ORM.IntegratedTests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using ICSSoft.STORMNET.Business;
    using Npgsql;
    using Oracle.ManagedDataAccess.Client;
    using Xunit;

    public abstract class BaseIntegratedTest : IDisposable
    {
        private static string connectionStringOracle;

        private static string connectionStringPostgres;

        private static string connectionStringMssql;

        private const string poolingFalseConst = "Pooling=false;";

        /// <summary>
        /// The temporary database name prefix.
        /// </summary>
        private readonly string _tempDbNamePrefix;

        private string _databaseName;

        private string _tmpUserNameOracle;

        /// <summary>
        /// The data services for temp databases (for <see cref="DataServices"/>).
        /// </summary>
        private readonly List<IDataService> _dataServices = new List<IDataService>();

        /// <summary>
        /// Flag: Indicates whether "Dispose" has already been called.
        /// </summary>
        private bool _disposed;

        protected virtual string MssqlScript
        {
            get
            {
                return Resources.MssqlScript;
            }
        }

        protected virtual string PostgresScript
        {
            get
            {
                return Resources.PostgresScript;
            }
        }

        protected virtual string OracleScript
        {
            get
            {
                return Resources.OracleScript;
            }
        }

        /// <summary>
        /// Data services for temp databases.
        /// </summary>
        protected IEnumerable<IDataService> DataServices
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(null);
                }

                return _dataServices;
            }
        }

        static BaseIntegratedTest()
        {
            // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
            // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
            connectionStringPostgres = $"{poolingFalseConst}{ConfigurationManager.ConnectionStrings["ConnectionStringPostgres"]}";
            connectionStringMssql = $"{poolingFalseConst}{ConfigurationManager.ConnectionStrings["ConnectionStringMssql"]}";
            connectionStringOracle = $"{poolingFalseConst}{ConfigurationManager.ConnectionStrings["ConnectionStringOracle"]}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseIntegratedTest" /> class.
        /// </summary>
        /// <param name="tempDbNamePrefix">Prefix for temp database name.</param>
        protected BaseIntegratedTest(string tempDbNamePrefix)
        {
            if (!(tempDbNamePrefix != null))
            {
                throw new ArgumentNullException();
            }

            if (!(tempDbNamePrefix != string.Empty))
            {
                throw new ArgumentException();
            }

            if (!tempDbNamePrefix.All(char.IsLetterOrDigit))
            {
                throw new ArgumentException();
            }

            _tempDbNamePrefix = tempDbNamePrefix;
            _databaseName = _tempDbNamePrefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + Guid.NewGuid().ToString("N");
            bool watchdogEmptyTest = false;

            if (!string.IsNullOrWhiteSpace(PostgresScript) && connectionStringPostgres != poolingFalseConst)
            {
                if (!(tempDbNamePrefix.Length <= 12)) // Max length is 63 (-18 -32).
                {
                    throw new ArgumentException();
                }

                if (!char.IsLetter(tempDbNamePrefix[0])) // Database names must have an alphabetic first character.
                {
                    throw new ArgumentException();
                }

                watchdogEmptyTest = true;

                using (var conn = new NpgsqlConnection(connectionStringPostgres))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(string.Format("CREATE DATABASE \"{0}\" ENCODING = 'UTF8' CONNECTION LIMIT = -1;", _databaseName), conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                using (var conn = new NpgsqlConnection($"{connectionStringPostgres};Database={_databaseName}"))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(PostgresScript, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    _dataServices.Add(CreatePostgresDataService($"{connectionStringPostgres};Database={_databaseName}"));
                }
            }

            if (!string.IsNullOrWhiteSpace(MssqlScript) && connectionStringMssql != poolingFalseConst)
            {
                if (!(tempDbNamePrefix.Length <= 64)) // Max is 128.
                {
                    throw new ArgumentException();
                }

                watchdogEmptyTest = true;

                using (var connection = new SqlConnection(connectionStringMssql))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"CREATE DATABASE {_databaseName} COLLATE Cyrillic_General_CI_AS", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                using (var connection = new SqlConnection($"{connectionStringMssql};Database={_databaseName}"))
                {
                    connection.Open();
                    using (var command = new SqlCommand(MssqlScript, connection))
                    {
                        command.CommandTimeout = 180;
                        command.ExecuteNonQuery();
                    }

                    _dataServices.Add(CreateMssqlDataService($"{connectionStringMssql};Database={_databaseName}"));
                }
            }

            if (!string.IsNullOrWhiteSpace(OracleScript) && connectionStringOracle != poolingFalseConst)
            {
                if (!(tempDbNamePrefix.Length <= 8)) // Max length is 30 (-18 -4).
                {
                    throw new ArgumentException();
                }

                watchdogEmptyTest = true;

                using (var connection = new OracleConnection(connectionStringOracle))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        // "CREATE USER" privileges required.
                        var doWhile = true;
                        while (doWhile)
                        {
                            _tmpUserNameOracle = tempDbNamePrefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + new Random().Next(9999);
                            command.CommandText = $"CREATE USER {_tmpUserNameOracle} IDENTIFIED BY {_tmpUserNameOracle} DEFAULT TABLESPACE users  quota unlimited on users  TEMPORARY TABLESPACE temp";
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (OracleException ex)
                            {
                                Thread.Sleep(1000);
                                if (ex.Message.Contains("conflicts with another user or role name "))
                                {
                                    continue;
                                }

                                throw;
                            }

                            doWhile = false;
                        }

                        // "CREATE SESSION WITH ADMIN OPTION" privileges required.
                        command.CommandText = $"GRANT CREATE SESSION TO {_tmpUserNameOracle}";
                        command.ExecuteNonQuery();
                        command.CommandText = $"GRANT CREATE TABLE TO {_tmpUserNameOracle}";
                        command.ExecuteNonQuery();
                    }
                }

                using (var connection = new OracleConnection($"{ConnectionStringOracleDataSource};User Id={_tmpUserNameOracle};Password={_tmpUserNameOracle};"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        foreach (var cmdText in OracleScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            command.CommandText = cmdText.Trim();
                            if (!string.IsNullOrWhiteSpace(command.CommandText))
                            {
                                command.ExecuteNonQuery();
                            }
                        }

                        _dataServices.Add(CreateOracleDataService($"{ConnectionStringOracleDataSource};User Id={_tmpUserNameOracle};Password={_tmpUserNameOracle};"));
                    }
                }
            }

            Assert.True(watchdogEmptyTest);
        }

        /// <summary>
        /// Creates the <see cref="MSSQLDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="MSSQLDataService"/> instance.</returns>
        protected virtual MSSQLDataService CreateMssqlDataService(string connectionString)
        {
            return new MSSQLDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="PostgresDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="PostgresDataService"/> instance.</returns>
        protected virtual PostgresDataService CreatePostgresDataService(string connectionString)
        {
            return new PostgresDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="OracleDataService"/> instance for temp database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="OracleDataService"/> instance.</returns>
        protected virtual OracleDataService CreateOracleDataService(string connectionString)
        {
            return new OracleDataService { CustomizationString = connectionString };
        }

        /// <summary>
        /// Deletes the temporary databases and perform other cleaning.
        /// </summary>
        /// <param name="disposing">Flag: indicates whether method is calling from "Dispose()" or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    foreach (var ds in _dataServices)
                    {
                        if (ds is PostgresDataService)
                        {
                            using (var connection = new NpgsqlConnection(connectionStringPostgres))
                            {
                                connection.Open();
                                using (var command = connection.CreateCommand())
                                {
                                    command.CommandText = $"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = \'{_databaseName}\' AND pid <> pg_backend_pid();";
                                    command.ExecuteNonQuery();
                                    command.CommandText = $"DROP DATABASE \"{_databaseName}\";";
                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        if (ds is MSSQLDataService)
                        {
                            using (var connection = new SqlConnection(connectionStringMssql))
                            {
                                connection.Open();
                                using (var command = connection.CreateCommand())
                                {
                                    command.CommandText = $"DROP DATABASE {_databaseName}";
                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        if (ds is OracleDataService)
                        {
                            using (var connection = new OracleConnection(connectionStringOracle))
                            {
                                connection.Open();
                                using (var command = connection.CreateCommand())
                                {
                                    command.CommandText = $"DROP USER {_tmpUserNameOracle} CASCADE";
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            _disposed = true;
        }

        /// <summary>
        /// Deletes the temporary databases and perform other cleaning.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private static string ConnectionStringOracleDataSource
        {
            get
            {
                var dataSource = connectionStringOracle.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(t => t.Trim().ToLower().IndexOf("data") == 0 && t.ToLower().IndexOf("source") != -1);

                // ADO.NET doesn't close the connection with pooling. We have to disable it explicitly.
                // http://stackoverflow.com/questions/9033356/connection-still-idle-after-close
                return $"{poolingFalseConst}{dataSource};";
            }
        }
    }
}
