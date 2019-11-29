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

        /// <summary>
        /// Проверка механизма выдачи новых инстанций сервиса данных.
        /// </summary>
        [Fact]
        public void AlwaysNewDsTest()
        {
            DataServiceProvider.AlwaysNewDS = true;

            var ds1 = DataServiceProvider.DataService;
            var ds2 = DataServiceProvider.DataService;

            DataServiceProvider.AlwaysNewDS = false;

            var ds3 = DataServiceProvider.DataService;
            var ds4 = DataServiceProvider.DataService;

            Assert.NotSame(ds1, ds2);
            Assert.Same(ds3, ds4);
        }
    }
}