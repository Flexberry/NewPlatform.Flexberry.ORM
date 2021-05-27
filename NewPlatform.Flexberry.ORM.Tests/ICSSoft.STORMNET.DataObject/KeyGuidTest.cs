namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET.KeyGen;
    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="KeyGuid"/>.
    /// </summary>
    public class KeyGuidTest
    {
        /// <summary>
        /// Тест приверки отсутствия фигурных скобок при преобразовании GUID в строку.
        /// </summary>
        [Fact]
        public void KeyGuidToStringConverteTest()
        {
            // Arrange.
            var keyGuid = KeyGuid.NewGuid();
            string leftParenthesis = "{";
            string rightParenthesis = "}";

            // Act.
            string stringKeyGuid = keyGuid.Guid.ToString("D");

            // Assert.
            // Проверка наличия фигурных скобок в GUID.
            Assert.DoesNotContain(stringKeyGuid, leftParenthesis);
            Assert.DoesNotContain(stringKeyGuid, rightParenthesis);
        }
    }
}
