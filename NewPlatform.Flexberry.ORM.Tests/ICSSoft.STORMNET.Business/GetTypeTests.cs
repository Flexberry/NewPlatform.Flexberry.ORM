namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using Xunit;

    /// <summary>
    /// Класс для тестирования получения типов по его наименованию.
    /// </summary>
    public class GetTypeTests
    {
        /// <summary>
        /// Тест получения типа по его наименованию.
        /// </summary>
        /// <param name="typeName">наименование типа.</param>
        [Theory]
        [InlineData("ICSSoft.STORMNET.Business.IDataService, ICSSoft.STORMNET.Business, Version=1.0.0.1, Culture=neutral, PublicKeyToken=c17bb360f7843f45")]
        public void GetTypeTest(string typeName)
        {
            // Act.
            Type result = Type.GetType(typeName);

            // Assert.
            Assert.NotNull(result);
        }
    }
}
