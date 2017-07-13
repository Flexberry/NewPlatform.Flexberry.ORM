namespace ICSSoft.STORMNET.Business.Audit.IntegratedTests
{
    using System.Configuration;

    using ICSSoft.STORMNET.Security;

    using NewPlatform.Flexberry.ORM.IntegratedTests;

    public abstract class BaseAuditServiceTest : BaseIntegratedTest
    {
        protected BaseAuditServiceTest()
            : base("OrmAudit")
        {
        }

        /// <summary>
        /// Creates the <see cref="MSSQLDataService" /> instance for temp database with audit service for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="MSSQLDataService" /> instance.</returns>
        protected override MSSQLDataService CreateMssqlDataService(string connectionString)
        {
            return new MSSQLDataService(new EmptySecurityManager(), GetAuditServiceForTest()) { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="PostgresDataService" /> instance for temp database with audit service for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="PostgresDataService" /> instance.</returns>
        protected override PostgresDataService CreatePostgresDataService(string connectionString)
        {
            return new PostgresDataService(new EmptySecurityManager(), GetAuditServiceForTest()) { CustomizationString = connectionString };
        }

        /// <summary>
        /// Creates the <see cref="OracleDataService" /> instance for temp database with audit service for testing.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="OracleDataService" /> instance.</returns>
        protected override OracleDataService CreateOracleDataService(string connectionString)
        {
            return new OracleDataService(new EmptySecurityManager(), GetAuditServiceForTest()) { CustomizationString = connectionString };
        }

        /// <summary>
        /// Gets the audit service for the test.
        /// </summary>
        /// <returns>Returns instance of the <see cref="AuditService"/> class that will be used for the test.</returns>
        protected abstract AuditService GetAuditServiceForTest();
    }
}