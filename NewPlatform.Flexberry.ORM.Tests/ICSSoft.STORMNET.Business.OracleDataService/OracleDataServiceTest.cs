namespace ICSSoft.STORMNET.Business.OracleDataServiceTests
{
    using System.Data;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    using Moq;

    using Xunit;

    /// <summary>
    /// Класс для проверки работы <see cref="OracleDataService"/>.
    /// </summary>
    public class OracleDataServiceTest
    {
        /// <summary>
        /// Метод для создания OracleDataService для целей тестирования.
        /// </summary>
        /// <returns>Сконструированный OracleDataService.</returns>
        public static OracleDataService CreateOracleDataServiceForTests()
        {
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            using var ds = new OracleDataService(mockSecurityManager.Object, mockAuditService.Object);
            ds.CustomizationString = "Data Source=dbserver-oracle;User ID=ora_tester;Password=pwd;";
            return ds;
        }

        /// <summary>
        /// Проверка метода получения коннекции <see cref="OracleDataService.GetConnection"/>.
        /// </summary>
        [Fact]
        public void GetOraConnectionTest()
        {
            using OracleDataService ds = CreateOracleDataServiceForTests();
            IDbConnection cnn = ds.GetConnection();
            Assert.NotNull(cnn);
        }
    }
}
