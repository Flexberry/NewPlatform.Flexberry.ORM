namespace ICSSoft.STORMNET.Tests.TestClasses.Business
{
    using ICSSoft.STORMNET.KeyGen;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using ICSSoft.STORMNET.Business;

    using System.Configuration;

    using NewPlatform.Flexberry.ORM.IntegratedTests;

    /// <summary>
    /// Класс для проверки методов обновления объектов данных в сложной структуре.
    /// </summary>
    public class PrimaryKeyStorageTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrimaryKeyStorageTest()
            : base("KeyStor")
        {
        }

        /// <summary>
        /// Проверка исправления ошибки, которая возникала, если в DataObject был указан атрибут [PrimaryKeyStorage("...")],
        /// но в тоже время требовалось переопределить свойство __PrimaryKey.
        /// </summary>
        [Fact]
        public void KeyStorageTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                ForKeyStorageTest класс = new ForKeyStorageTest();
                dataService.UpdateObject(класс);
                Assert.NotNull(класс.__PrimaryKey);
            }
        }
    }
}
