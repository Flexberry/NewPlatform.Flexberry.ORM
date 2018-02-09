namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using System.Configuration;
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
            ConfigurationManager.RefreshSection("connectionStrings");
            var conn = ConfigurationManager.ConnectionStrings[connectionStringName];
            Assert.NotNull(conn);

            ConfigurationManager.RefreshSection("connectionStrings");
            var conn2 = ConfigurationManager.ConnectionStrings[connectionStringName];
            Assert.NotNull(conn2);

            string expectedResult = conn.ConnectionString;
            ConfigurationManager.RefreshSection("connectionStrings");

            // Act.
            string actualResult = ConfigHelper.GetConnectionString(AppMode.Win, connectionStringName);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
