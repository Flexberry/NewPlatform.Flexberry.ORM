namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Globalization;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Interfaces;
    using ICSSoft.STORMNET.Security;

    using Moq;

    using Xunit;

    /// <summary>
    /// Тесты PostgreSQL-сервиса: конвертация значений DateOnly, GetConvertToTypeExpression.
    /// </summary>
    public class PostgresDataServiceDateOnlyTest
    {
        private static PostgresDataService CreateDataServiceForTests()
        {
            Mock<ISecurityManager> mockSecurityManager = new Mock<ISecurityManager>();
            Mock<IAuditService> mockAuditService = new Mock<IAuditService>();
            Mock<IBusinessServerProvider> mockBusinessServerProvider = new Mock<IBusinessServerProvider>();
            var ds = new PostgresDataService(mockSecurityManager.Object, mockAuditService.Object, mockBusinessServerProvider.Object);
            ds.CustomizationString = "SERVER=localhost;User ID=postgres;Password=postgres;Port=5432;";
            return ds;
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void ConvertSimpleValueToQueryValueStringDateOnlyTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            var dateOnly = new DateOnly(2026, 5, 20);
            string val = ds.ConvertSimpleValueToQueryValueString(dateOnly);
            Assert.Equal("date '2026-05-20'", val);
        }

        [Fact]
        public void ConvertSimpleValueToQueryValueStringDateOnlyMinValueTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            string val = ds.ConvertSimpleValueToQueryValueString(DateOnly.MinValue);
            Assert.Equal("date '0001-01-01'", val);
        }

        [Fact]
        public void ConvertSimpleValueToQueryValueStringDateOnlyMaxValueTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            string val = ds.ConvertSimpleValueToQueryValueString(DateOnly.MaxValue);
            Assert.Equal("date '9999-12-31'", val);
        }

        [Fact]
        public void GetConvertToTypeExpressionDateOnlyTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            string result = ds.GetConvertToTypeExpression(typeof(DateOnly), "some_col");
            Assert.Equal("cast(some_col as date)", result);
        }

        [Fact]
        public void GetConvertToTypeExpressionDateTimeTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            string result = ds.GetConvertToTypeExpression(typeof(DateTime), "some_col");
            Assert.Equal("cast(some_col as timestamp)", result);
        }

        [Fact]
        public void ConvertSimpleValueToQueryValueStringTimeOnlyTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            var timeOnly = new TimeOnly(14, 30, 45);
            string val = ds.ConvertSimpleValueToQueryValueString(timeOnly);
            Assert.Equal("time '14:30:45.000000'", val);
        }

        [Fact]
        public void GetConvertToTypeExpressionTimeOnlyTest()
        {
            using PostgresDataService ds = CreateDataServiceForTests();
            string result = ds.GetConvertToTypeExpression(typeof(TimeOnly), "some_col");
            Assert.Equal("cast(some_col as time)", result);
        }
#endif
    }
}
