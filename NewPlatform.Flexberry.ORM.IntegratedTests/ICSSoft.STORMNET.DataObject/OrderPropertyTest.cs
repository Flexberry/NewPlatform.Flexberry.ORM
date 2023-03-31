namespace NewPlatform.Flexberry.ORM.IntegratedTests.DataObject
{
    using System;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;
    using System.Configuration;

    /// <summary>
    /// Класс, содержащий тестовые методы для проверки корректности работы ORM с атрибутами порядка у детейлов.
    /// </summary>
    public class OrderPropertyTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public OrderPropertyTest()
            : base("ORTest")
        {
        }

        /// <summary>
        /// Тест корректной работы метода <see cref="DetailArray.Renumerate"/>.
        /// </summary>
        [Fact]
        public void RenumerateDetailsAfterDeleteTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                const string First = "Первый";
                const string Second = "Второй";
                const string Third = "Третий";
                const string Fourth = "Четвертый";
                const string Fifth = "Пятый";
                IDataService ds = dataService;

                Кошка aggregator = new Кошка
                {
                    ДатаРождения = NullableDateTime.Now,
                    Тип = ТипКошки.Дикая,
                    Порода = new Порода(),
                };
                aggregator.Лапа.AddRange(
                    new Лапа { Цвет = First },
                    new Лапа { Цвет = Second },
                    new Лапа { Цвет = Third },
                    new Лапа { Цвет = Fourth });
                ds.UpdateObject(aggregator);

                // Для проверки того, что у свойства Номер есть атрибут Order.
                int[] initialNumbers = aggregator.Лапа.Cast<Лапа>().Select(x => x.Номер).ToArray();

                // Удаляются второй и третий элементы.
                aggregator.Лапа.ItemByIndex(1).SetStatus(ObjectStatus.Deleted);
                aggregator.Лапа.ItemByIndex(2).SetStatus(ObjectStatus.Deleted);
                aggregator.Лапа.Renumerate();
                ds.UpdateObject(aggregator);

                // Новый элемент добавляется в конец массива.
                aggregator.Лапа.Add(new Лапа { Цвет = Fifth });
                ds.UpdateObject(aggregator);

                Кошка loadedAggr = new Кошка();
                loadedAggr.SetExistObjectPrimaryKey(aggregator.__PrimaryKey);
                ds.LoadObject(Кошка.Views.КошкаE, loadedAggr);

                string expectedNumbers = string.Join(string.Empty, 1, 2, 3, 4);
                string actualNumbers = string.Join(string.Empty, initialNumbers);
                string expectedOrder = string.Join(string.Empty, First, Fourth, Fifth);
                string actualOrder = string.Join(String.Empty, loadedAggr.Лапа.Cast<Лапа>().Select(x => x.Цвет));

                Assert.Equal(expectedNumbers, actualNumbers);
                Assert.Equal(expectedOrder, actualOrder);
            }
        }
    }
}
