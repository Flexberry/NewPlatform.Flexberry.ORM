namespace ICSSoft.STORMNET.Business.Tests
{
    using ICSSoft.STORMNET.Business;
    using System;
    using System.Configuration;
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
                "select @Фамилия@+@Имя@+@Отчество@ from TableName where primaryKey=StormMainObjectKey", string.Empty,
                "exteranlnamewithpoint.", out pointExistInSourceIdentifier);

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
        /// Тест для проверки установки строки соединения через свойство <see cref="SQLDataService.CustomizationStringName"/>.
        /// </summary>
        [Fact]
        public void CustomizationStringNameTest()
        {
            // Arrange.
            SQLDataService ds = new MSSQLDataService();
            string connectionStringName = "TestConnStr";
            string expectedResult = @"SERVER=.\SQLEXPRESS;Trusted_connection=yes;DATABASE=Test;";
            ConfigurationManager.RefreshSection("connectionStrings");

            // Act.
            ds.CustomizationStringName = connectionStringName;
            string actualResult = ds.CustomizationString;

            // Assert.
            Assert.Equal(expectedResult, actualResult);

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
