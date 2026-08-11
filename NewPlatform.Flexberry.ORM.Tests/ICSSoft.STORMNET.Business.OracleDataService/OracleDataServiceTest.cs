namespace ICSSoft.STORMNET.Business.OracleDataServiceTests
{
    using System;
    using System.Data;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Interfaces;
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
            Mock<ISecurityManager> mockSecurityManager = new Mock<ISecurityManager>();
            Mock<IAuditService> mockAuditService = new Mock<IAuditService>();
            Mock<IBusinessServerProvider> mockBusinessServerProvider = new Mock<IBusinessServerProvider>();
            using var ds = new OracleDataService(mockSecurityManager.Object, mockAuditService.Object, mockBusinessServerProvider.Object);
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

#if NET6_0_OR_GREATER
        [Fact]
        public void ConvertSimpleValueToQueryValueStringDateOnlyTest()
        {
            using OracleDataService ds = CreateOracleDataServiceForTests();
            var dateOnly = new DateOnly(2026, 5, 20);
            string val = ds.ConvertSimpleValueToQueryValueString(dateOnly);
            Assert.Equal("TO_DATE('2026-05-20', 'YYYY-MM-DD')", val);
        }

        [Fact]
        public void ConvertSimpleValueToQueryValueStringDateOnlyMinValueTest()
        {
            using OracleDataService ds = CreateOracleDataServiceForTests();
            string val = ds.ConvertSimpleValueToQueryValueString(DateOnly.MinValue);
            Assert.Equal("TO_DATE('0001-01-01', 'YYYY-MM-DD')", val);
        }

        [Fact]
        public void ConvertSimpleValueToQueryValueStringDateOnlyMaxValueTest()
        {
            using OracleDataService ds = CreateOracleDataServiceForTests();
            string val = ds.ConvertSimpleValueToQueryValueString(DateOnly.MaxValue);
            Assert.Equal("TO_DATE('9999-12-31', 'YYYY-MM-DD')", val);
        }
#endif
    }
}
