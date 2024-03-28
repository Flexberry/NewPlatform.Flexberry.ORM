namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Data;
    using System.Globalization;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Interfaces;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.UserDataTypes;

    using Moq;

    using Xunit;

    /// <summary>
    /// Класс для проверки работы <see cref="MSSQLDataService"/>.
    /// </summary>
    public class MSSQLDataServiceTest
    {
        /// <summary>
        /// Метод для создания MSSQLDataService для целей тестирования.
        /// <remarks>Строка соединения не настоящая ("SERVER=server;Trusted_connection=yes;DATABASE=test;").</remarks>
        /// </summary>
        /// <returns>Сконструированный MSSQLDataService.</returns>
        public static MSSQLDataService CreateMSSQLDataServiceForTests()
        {
            Mock<ISecurityManager> mockSecurityManager = new Mock<ISecurityManager>();
            Mock<IAuditService> mockAuditService = new Mock<IAuditService>();
            Mock<IBusinessServerProvider> mockBusinessServerProvider = new Mock<IBusinessServerProvider>();
            using var ds = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object, mockBusinessServerProvider.Object);
            ds.CustomizationString = "SERVER=server;Trusted_connection=yes;DATABASE=test;";
            return ds;
        }

        /// <summary>
        /// Проверка метода получения коннекции <see cref="MSSQLDataService.GetConnection"/>.
        /// </summary>
        [Fact]
        public void GetConnectionTest()
        {
            using MSSQLDataService ds = CreateMSSQLDataServiceForTests();
            IDbConnection conn = ds.GetConnection();
            Assert.NotNull(conn);
        }

        /// <summary>
        /// Проверка метода, возвращающего ifnull выражение <see cref="MSSQLDataService.GetIfNullExpression"/>.
        /// </summary>
        [Fact]
        public void GetIfNullExpressionTest()
        {
            using MSSQLDataService ds = CreateMSSQLDataServiceForTests();

            string exp = ds.GetIfNullExpression("identifier1", "identifier2");
            Assert.Equal("ISNULL(identifier1, identifier2)", exp);

            exp = ds.GetIfNullExpression("identifier3", "identifier4", "identifier5");
            Assert.Equal("ISNULL(identifier3, ISNULL(identifier4, identifier5))", exp);

            exp = ds.GetIfNullExpression("identifier6");
            Assert.Equal("identifier6", exp);

            exp = ds.GetIfNullExpression(string.Empty);
            Assert.Equal(string.Empty, exp);
        }

        /// <summary>
        /// Проверка метода, возвращающего ifnull выражение <see cref="MSSQLDataService.GetIfNullExpression"/>. Передаём в качестве параметра <c>null</c>.
        /// </summary>
        [Fact]
        public void GetIfNullExpressionNullTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                using MSSQLDataService ds = CreateMSSQLDataServiceForTests();
                ds.GetIfNullExpression(null);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Проверка метода, возвращающего ifnull выражение <see cref="MSSQLDataService.GetIfNullExpression"/>. Передаём в качестве параметра пустой массив.
        /// </summary>
        [Fact]
        public void GetIfNullExpressionEmptyArrayTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                using MSSQLDataService ds = CreateMSSQLDataServiceForTests();
                ds.GetIfNullExpression(new string[] { });
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Проверка метода конвертации константных значений в строки запроса методом <see cref="MSSQLDataService.ConvertSimpleValueToQueryValueString"/>.
        /// </summary>
        [Fact]
        public void ConvertSimpleValueToQueryValueStringTest()
        {
            using MSSQLDataService ds = CreateMSSQLDataServiceForTests();

            string val = ds.ConvertSimpleValueToQueryValueString(null);
            Assert.Equal("NULL", val);

            val = ds.ConvertSimpleValueToQueryValueString(string.Empty);
            Assert.Equal("NULL", val);

            val = ds.ConvertSimpleValueToQueryValueString("null");
            Assert.Equal("N'null'", val);

            var dateTime = new DateTime(2014, 01, 30, 16, 14, 20, 679);
            const string resDateTimeString = "'20140130 16:14:20.679'";

            val = ds.ConvertSimpleValueToQueryValueString(dateTime);
            Assert.Equal(resDateTimeString, val);

            var sysNullableDateTime = new DateTime?(dateTime);
            val = ds.ConvertSimpleValueToQueryValueString(sysNullableDateTime);
            Assert.Equal(resDateTimeString, val);

            var caseNullableDateTime = new NullableDateTime { Value = dateTime };
            val = ds.ConvertSimpleValueToQueryValueString(caseNullableDateTime);
            Assert.Equal(resDateTimeString, val);

            int intVal = 5;
            val = ds.ConvertSimpleValueToQueryValueString(intVal);
            Assert.Equal(intVal.ToString(CultureInfo.InvariantCulture), val);
        }
    }
}
