namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    using Moq;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="SQLDataService"/>.
    /// </summary>
    public class SQLDataServiceTest
    {
        /// <summary>
        /// Тесты преобразования DataServiceExpression методом <see cref="SQLDataService.TranslateExpression"/>.
        /// </summary>
        [Fact]
        public void TranslateExpressionTest()
        {
            // Arrange.
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            using var ds = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object);
            bool pointExistInSourceIdentifier;

            string expectedResult =
                "(select exteranlnamewithpoint.\"Фамилия\"+exteranlnamewithpoint.\"Имя\"+exteranlnamewithpoint.\"Отчество\" from TableName where primaryKey=StormMainObjectKey)";

            // Act.
            string result = ds.TranslateExpression(
                "select @Фамилия@+@Имя@+@Отчество@ from TableName where primaryKey=StormMainObjectKey",
                string.Empty,
                "exteranlnamewithpoint.",
                out pointExistInSourceIdentifier);

            // Assert.
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Тесты преобразования DataServiceExpression методом <see cref="SQLDataService.TranslateExpression"/> с передачей строки содержащей конструкцию "For XML PATH".
        /// </summary>
        [Fact]
        public void TranslateExpressionForXmlPathTest()
        {
            // Arrange.
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            using var ds = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object);
            bool pointExistInSourceIdentifier;
            string expectedResult = "(select '' as [@caption], [KP].[ФИО] as [@value] For XML PATH ('element'), TYPE)";

            // Act.
            string result = ds.TranslateExpression(expectedResult, string.Empty, "exteranlnamewithpoint.", out pointExistInSourceIdentifier);

            // Assert.
            Assert.Equal(string.Format("({0})", expectedResult), result);
        }

        /// <summary>
        /// Test <see cref="DataServiceExpressionAttribute"/> for <see cref="SQLDataService"/>.
        /// </summary>
        [Fact]
        public void AddPripertiesFromDataServiceExpressionToDynamicView1()
        {
            // Arrange.
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            using var ds = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object);
            ds.AfterGenerateSQLSelectQuery += (sender, e) =>
            {
                // Assert.
                Assert.True(e.CustomizationStruct.View.CheckPropname(Information.ExtractPropertyPath<Кошка>(c => c.Кличка)));
                throw new System.InvalidOperationException("Stop.");
            };
            var dynamicView = new View() { DefineClassType = typeof(Кошка) };
            dynamicView.AddProperty(Information.ExtractPropertyName<Кошка>(c => c.КошкаСтрокой));

            // Act.
            Assert.Throws<System.InvalidOperationException>(() => ds.LoadObjects(LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), dynamicView)));
        }

        /// <summary>
        /// Test <see cref="DataServiceExpressionAttribute"/> with master property.
        /// </summary>
        [Fact]
        public void AddPripertiesFromDataServiceExpressionToDynamicView2()
        {
            // Arrange.
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            using var ds = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object);
            ds.AfterGenerateSQLSelectQuery += (sender, e) =>
            {
                // Assert.
                Assert.True(e.CustomizationStruct.View.CheckPropname(Information.ExtractPropertyPath<Медведь>(b => b.ПорядковыйНомер)));
                Assert.True(e.CustomizationStruct.View.CheckPropname(Information.ExtractPropertyPath<Медведь>(b => b.Мама.ЦветГлаз)));
                throw new System.InvalidOperationException("Stop.");
            };
            var dynamicView = new View() { DefineClassType = typeof(Медведь) };
            dynamicView.AddProperty(Information.ExtractPropertyName<Медведь>(b => b.МедведьСтрокой));

            // Act.
            Assert.Throws<System.InvalidOperationException>(() => ds.LoadObjects(LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), dynamicView)));
        }

        /// <summary>
        /// Test <see cref="DataServiceExpressionAttribute"/> without properties.
        /// </summary>
        [Fact]
        public void AddPripertiesFromDataServiceExpressionToDynamicView3()
        {
            // Arrange.
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            using var ds = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object);
            ds.AfterGenerateSQLSelectQuery += (sender, e) =>
            {
                // Assert.
                Assert.Equal(1, e.CustomizationStruct.View.Properties.Length);
                throw new System.InvalidOperationException("Stop.");
            };
            var dynamicView = new View() { DefineClassType = typeof(Клиент) };
            dynamicView.AddProperty(Information.ExtractPropertyName<Клиент>(c => c.NotStoredGuid));

            // Act.
            Assert.Throws<System.InvalidOperationException>(() => ds.LoadObjects(LoadingCustomizationStruct.GetSimpleStruct(typeof(Клиент), dynamicView)));
        }

        /// <summary>
        /// Test <see cref="Information.AppendPropertiesFromNotStored" /> for master property.
        /// </summary>
        [Fact]
        public void TestAppendPropertiesFromNotStoredForMaster()
        {
            // Arrange.
            var view = new View { DefineClassType = typeof(Котенок) };
            view.AddProperties(
                Information.ExtractPropertyPath<Котенок>(x => x.Кошка),
                Information.ExtractPropertyPath<Котенок>(x => x.Кошка.КошкаСтрокой));
            string missingProp = Information.ExtractPropertyPath<Котенок>(x => x.Кошка.Кличка);

            // Act.
            Information.AppendPropertiesFromNotStored(view, typeof(SQLDataService));

            // Assert.
            Assert.Equal(3, view.Properties.Length);
            Assert.Contains(missingProp, view.Properties.Select(x => x.Name));
        }

        /// <summary>
        /// Test <see cref="Information.AppendPropertiesFromNotStored" /> for own property.
        /// </summary>
        [Fact]
        public void TestAppendPropertiesFromNotStored()
        {
            // Arrange.
            var view = new View { DefineClassType = typeof(Кошка) };
            view.AddProperties(
                Information.ExtractPropertyPath<Кошка>(x => x.КошкаСтрокой));
            string missingProp = Information.ExtractPropertyPath<Кошка>(x => x.Кличка);

            // Act.
            Information.AppendPropertiesFromNotStored(view, typeof(SQLDataService));

            // Assert.
            Assert.Equal(2, view.Properties.Length);
            Assert.Contains(missingProp, view.Properties.Select(x => x.Name));
        }

        /// <summary>
        /// Test for using <see cref="IConvertibleToQueryValueString"/> in <see cref="SQLDataService.ConvertSimpleValueToQueryValueString(object)"/>.
        /// </summary>
        [Fact]
        public void TestConvertibleToQueryValueString()
        {
            // Arrange.
            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            var mock = new Mock<IConvertibleToQueryValueString>();
            mock.Setup(m => m.ConvertToQueryValueString()).Returns(string.Empty);

            using var dataService = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object);

            // Act.
            dataService.ConvertSimpleValueToQueryValueString(mock.Object);

            // Assert.
            mock.Verify(m => m.ConvertToQueryValueString(), Times.Once);
        }

        /// <summary>
        /// Test for using <see cref="IConverterToQueryValueString"/> in <see cref="SQLDataService.ConvertSimpleValueToQueryValueString(object)"/>.
        /// </summary>
        [Fact]
        public void TestConverterToQueryValueString()
        {
            // Arrange.
            object supportedValue = new object();

            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            var mockConverter = new Mock<IConverterToQueryValueString>();
            mockConverter.Setup(m => m.IsSupported(typeof(object))).Returns(true);
            mockConverter.Setup(m => m.IsSupported(typeof(int))).Returns(false);
            mockConverter.Setup(m => m.ConvertToQueryValueString(supportedValue)).Returns(string.Empty);

            using var dataService = new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object, mockConverter.Object);

            // Act.
            dataService.ConvertSimpleValueToQueryValueString(supportedValue);
            dataService.ConvertSimpleValueToQueryValueString(0);

            // Assert.
            mockConverter.Verify(m => m.IsSupported(typeof(object)), Times.Once);
            mockConverter.Verify(m => m.IsSupported(typeof(int)), Times.Once);
            mockConverter.Verify(m => m.ConvertToQueryValueString(supportedValue), Times.Once);
        }

        /// <summary>
        /// Тест для проверки установки строки соединения через свойство <see cref="SQLDataService.CustomizationStringName"/>.
        /// </summary>
        [Fact]
        public void CustomizationStringNameTest()
        {
            var configResolver = new ConfigResolver();

            var mockSecurityManager = new Mock<ISecurityManager>();
            var mockAuditService = new Mock<IAuditService>();
            var dataServices = new List<SQLDataService>
            {
                new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object) { ConfigResolver = configResolver, },
                new PostgresDataService(mockSecurityManager.Object, mockAuditService.Object) { ConfigResolver = configResolver, },
                new OracleDataService(mockSecurityManager.Object, mockAuditService.Object) { ConfigResolver = configResolver, },
            };

            foreach (var dataService in dataServices)
            {
                // Arrange.
                const string connectionStringName = "TestConnStr";
                string expectedResult = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();

                // Act.
                dataService.CustomizationStringName = connectionStringName;
                string actualResult = dataService.CustomizationString;

                // Assert.
                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}
