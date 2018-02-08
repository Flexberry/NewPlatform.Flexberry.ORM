namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using Microsoft.Practices.Unity;
    using System;
    using System.Configuration;
    using System.Web;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="ConfigResolver"/>.
    /// </summary>
    public class ConfigResolverTest
    {
        /// <summary>
        /// Тест для проверки получения инстанции <see cref="ConfigResolver"/>.
        /// </summary>
        [Fact]
        public void GetConfigResolverTest()
        {
            // Arrange.
            IUnityContainer container = UnityFactory.GetContainer();

            // Act.
            IConfigResolver configResolver = container.Resolve<IConfigResolver>();
            configResolver.ResolveConnectionString("TestConnStr");

            // Assert.
            Assert.NotNull(configResolver);
        }

        /// <summary>
        /// Тест для проверки получения строки соединения по имени <see cref="ConfigResolver.ResolveConnectionString(string)"/>.
        /// </summary>
        [Fact]
        public void ResolveConnectionStringTest()
        {
            // Arrange.
            IUnityContainer container = UnityFactory.GetContainer();
            IConfigResolver configResolver = container.Resolve<IConfigResolver>();
            string connectionStringName = "TestConnStr";
            string expectedResult = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();


            // Act.
            string actualResult = ""; // configResolver.ResolveConnectionString(connectionStringName);
            var appMode = HttpRuntime.AppDomainAppId != null ? AppMode.Web : AppMode.Win;
            Console.WriteLine($"App mode is {appMode}");

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }
    }
}