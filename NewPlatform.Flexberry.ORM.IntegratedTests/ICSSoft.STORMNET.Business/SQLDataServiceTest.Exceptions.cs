namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты обработки исключений хранилища.
    /// </summary>
    public partial class SQLDataServiceTest
    {
        /// <summary>
        /// Тест проверяет, что <see cref="SQLDataService.RunCommands" />.
        /// </summary>
        [Fact]
        public void RunCommandReturnsValidException()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var bear = new Медведь { ПорядковыйНомер = 1, };
                var lair0 = new Берлога
                {
                    Наименование = "LUXURY GOLD",
                    Заброшена = true,
                    ЛесРасположения = new Лес
                    {
                        Название = "Черняевский",
                    },
                };
                var lair1 = new Берлога
                {
                    Наименование = "Дача с уточкой",
                    ЛесРасположения = PKHelper.CreateDataObject<Лес>(Guid.NewGuid()), // объекта в БД нет, должен свалиться FK Constraint.
                };
                var lair2 = new Берлога
                {
                    Наименование = "Just in case",
                    Заброшена = true,
                    ЛесРасположения = new Лес
                    {
                        Название = "Шишкин",
                    },
                };
                bear.Берлога.Add(lair0);
                bear.Берлога.Add(lair1);
                bear.Берлога.Add(lair2);

                // Assert.
                Assert.Throws<ExecutingQueryException>(() => dataService.UpdateObject(bear));
            }
        }
    }
}