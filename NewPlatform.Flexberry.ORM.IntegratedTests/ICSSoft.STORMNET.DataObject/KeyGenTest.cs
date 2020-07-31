namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using ICSSoft.STORMNET.KeyGen;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тестовый класс для KeyGen.
    /// </summary>
    public class KeyGenTest
    {
        /// <summary>
        /// Проверка генерации ключа.
        /// </summary>
        [Fact]

        public void KeyGenGenerateUniqueTest()
        {
            ICSSoft.STORMNET.DataObject dataObject = new DataObjectForTest();

            // Установка флага уникального ключа в false.
            dataObject.PrimaryKeyIsUnique = false;
            var result = KeyGenerator.GenerateUnique(dataObject, null);

            // Проверка смены флага уникального ключа на true.
            Assert.Equal(true, dataObject.PrimaryKeyIsUnique);
        }
    }
}
