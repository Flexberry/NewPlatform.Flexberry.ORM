namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Collections.Generic;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    using Moq;

    public abstract class BaseSQLDataServiceTests
    {
        /// <summary>
        /// The data services for temp databases (for <see cref="DataServices"/>).
        /// </summary>
        private readonly List<SQLDataService> _dataServices = new List<SQLDataService>();

        /// <summary>
        /// Data services for temp databases.
        /// </summary>
        protected IEnumerable<SQLDataService> DataServices => _dataServices;

        protected BaseSQLDataServiceTests()
        {
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            _dataServices.Add(new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object));
            _dataServices.Add(new PostgresDataService(mockSecurityManager.Object, mockAuditService.Object));
            _dataServices.Add(new OracleDataService(mockSecurityManager.Object, mockAuditService.Object));
        }
    }
}
