namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business;
    using Unity;
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
            Assert.NotNull(configResolver);
            string connectionStringName = "TestConnStr";
            string expectedResult = @"SERVER=.\SQLEXPRESS;Trusted_connection=yes;DATABASE=Test;";

            // Act.
            string actualResult = configResolver.ResolveConnectionString(connectionStringName);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }
    }
}