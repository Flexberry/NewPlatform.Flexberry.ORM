﻿namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

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
            SQLDataService ds = new MSSQLDataService();
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
            SQLDataService ds = new MSSQLDataService();
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
            var ds = new MSSQLDataService();
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
            var ds = new MSSQLDataService();
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
            var ds = new MSSQLDataService();
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
            var mock = new Mock<IConvertibleToQueryValueString>();
            mock.Setup(m => m.ConvertToQueryValueString()).Returns(string.Empty);

            var dataService = new MSSQLDataService();

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

            var mock = new Mock<IConverterToQueryValueString>();
            mock.Setup(m => m.IsSupported(typeof(object))).Returns(true);
            mock.Setup(m => m.IsSupported(typeof(int))).Returns(false);
            mock.Setup(m => m.ConvertToQueryValueString(supportedValue)).Returns(string.Empty);

            var dataService = new MSSQLDataService(mock.Object);

            // Act.
            dataService.ConvertSimpleValueToQueryValueString(supportedValue);
            dataService.ConvertSimpleValueToQueryValueString(0);

            // Assert.
            mock.Verify(m => m.IsSupported(typeof(object)), Times.Once);
            mock.Verify(m => m.IsSupported(typeof(int)), Times.Once);
            mock.Verify(m => m.ConvertToQueryValueString(supportedValue), Times.Once);
        }
    }
}
