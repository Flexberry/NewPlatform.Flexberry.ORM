namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    using Moq;

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

            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            _dataServices.Add(new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object));
            _dataServices.Add(new PostgresDataService(mockSecurityManager.Object, mockAuditService.Object));
            _dataServices.Add(new OracleDataService(mockSecurityManager.Object, mockAuditService.Object));
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
