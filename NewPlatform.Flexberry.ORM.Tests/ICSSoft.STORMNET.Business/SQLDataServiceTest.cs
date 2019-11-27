namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

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
    }
}
