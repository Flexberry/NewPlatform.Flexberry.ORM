namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET.Business;

    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="DataServiceProvider" />.
    /// </summary>
    
    public class DataServiceProviderTest
    {
        /// <summary>
        /// Получение инстанции сервиса данных через провайдер.
        /// </summary>
        [Fact]
        public void DataServiceTest()
        {
            var ds = DataServiceProvider.DataService;
            Assert.NotNull(ds);
        }

        /// <summary>
        /// Проверка механизма шифрования-дешифрования строк с хэшем.
        /// </summary>
        [Fact]
        public void EncryptDecryptWithHashTest()
        {
            var source = "Peppa pig";

            var encryptedString = DataServiceProvider.Encrypt(source, true);
            var decryptedString = DataServiceProvider.Decrypt(encryptedString, true);

            Assert.Equal(source, decryptedString);
        }

        /// <summary>
        /// Проверка механизма шифрования-дешифрования строк с хэшем.
        /// </summary>
        [Fact(Skip = "Раскомментируйте этот тест после исправления проблемы с шифрованием без хеша.")]
        public void EncryptDecryptWithoutHashTest()
        {
            var source = "Winnie the pooh";

            var encryptedString = DataServiceProvider.Encrypt(source, false);
            var decryptedString = DataServiceProvider.Decrypt(encryptedString, false);

            Assert.Equal(source, decryptedString);
        }
    }
}