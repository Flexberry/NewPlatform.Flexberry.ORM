namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="ConfigHelper"/>.
    /// </summary>
    public class ConfigHelperTest
    {
        /// <summary>
        /// Тест для проверки получения строки соединения по имени <see cref="ConfigHelper.GetConnectionString(AppMode, string)"/>.
        /// </summary>
        [Fact]
        public void GetConnectionStringTest()
        {
            // Arrange.
            string connectionStringName = "TestConnStr";
            string expectedResult = @"SERVER=.\SQLEXPRESS;Trusted_connection=yes;DATABASE=Test;";

            // Act.
            string actualResult = ConfigHelper.GetConnectionString(AppMode.Win, connectionStringName);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
