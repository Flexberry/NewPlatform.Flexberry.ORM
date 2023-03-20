namespace NewPlatform.Flexberry.ORM.IntegratedTests.LINQProvider
{
    using System;
    using System.Linq;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.UserDataTypes;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Проверка цепочного вызова Where при LINQ-запросах к сервису данных.
    /// </summary>
    public class LinqToLcsChainWhereTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public LinqToLcsChainWhereTest()
            : base("LTest1")
        {
        }

        /// <summary>
        /// Проверка двух идущих подряд вызовов Where. В результитрующей выборке должны учитываться оба ограничения.
        /// </summary>
        [Fact]
        public void TestСhainWhere()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;

                ТипПороды типПороды = new ТипПороды() { Название = "Простая", ДатаРегистрации = DateTime.Now };

                Порода породаДикая = new Порода() { Название = "Дикая" };
                Порода породаПерсидская = new Порода() { Название = "Персидская", ТипПороды = типПороды };
                Порода породаБезПороды = new Порода() { Название = "БезПороды", Иерархия = породаДикая };

                ds.UpdateObject(породаДикая);
                ds.UpdateObject(породаПерсидская);
                ds.UpdateObject(породаБезПороды);

                var вася = new Кошка
                {
                    Кличка = "Вася",
                    ДатаРождения = (NullableDateTime)DateTime.Now,
                    Порода = породаПерсидская,
                    Тип = ТипКошки.Домашняя,
                    Агрессивная = false,
                };
                var кузя = new Кошка
                {
                    Кличка = "Кузя",
                    ДатаРождения = (NullableDateTime)DateTime.Now,
                    Порода = породаПерсидская,
                    Тип = ТипКошки.Дикая,
                    Агрессивная = false,
                };

                ds.UpdateObject(вася);
                ds.UpdateObject(кузя);

                // Act.
                var iqueryable =
                    ds.Query<Кошка>(Кошка.Views.КошкаE)
                        .Where(к => к.Агрессивная)
                        .Where(cat => cat.Порода.__PrimaryKey.Equals(породаПерсидская.__PrimaryKey));
                Кошка агрессивнаяКошка = iqueryable.FirstOrDefault();

                // Assert.
                // В тестовом наборе нет агрессивных кошек, следовательно FirstOrDefault должен вернуть null.
                Assert.Equal(null, агрессивнаяКошка);
            }
        }
    }
}
