namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;

    using ICSSoft.STORMNET.Business;

    using Xunit.Abstractions;

    public abstract class BaseSQLDataServiceTests
    {
        /// <summary>
        /// The data services for temp databases (for <see cref="DataServices"/>).
        /// </summary>
        private readonly List<SQLDataService> _dataServices = new List<SQLDataService>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSQLDataServiceTests" /> class.
        /// </summary>
        /// <param name="testOutputHelper">Поток вывода теста.</param>
        protected BaseSQLDataServiceTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));

            _dataServices.Add(new MSSQLDataService());
            _dataServices.Add(new PostgresDataService());
            _dataServices.Add(new OracleDataService());
        }

        /// <summary>
        /// Data services for temp databases.
        /// </summary>
        protected IEnumerable<SQLDataService> DataServices => _dataServices;

        /// <summary>
        /// Поток вывода теста.
        /// </summary>
        protected ITestOutputHelper TestOutputHelper { get; }
    }
}
