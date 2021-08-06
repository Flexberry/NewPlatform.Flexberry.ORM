namespace NewPlatform.Flexberry.ORM.IntegratedTests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using System.Configuration;

    /// <summary>
    /// Интеграционный тест для <see cref="DataObjectCache"/>.
    /// </summary>
    public class DataObjectCacheTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataObjectCacheTest()
            : base("DOCTest")
        {
        }

        /// <summary>
        /// Проверка <see cref="DataObjectCache"/> на выполнение основной функции: инстанции мастеров после зачитки смотрят на один и тот же объект.
        /// </summary>
        [Fact]
        public void DataObjectCacheSingleMasterTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                // Создадим в БД структуру, которая будет служить для проверки.
                Plant2 plant = new Plant2 { Name = "plant" };
                Salad2 salad = new Salad2 { Ingridient1 = plant, Ingridient2 = plant };
                ICSSoft.STORMNET.DataObject[] objects = { plant, salad };
                dataService.UpdateObjects(ref objects);

                // Подготовим структуру для вычитки.
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Salad2),
                    Salad2.Views.Salad2E);
                lcs.ReturnTop = 1;

                // Act.
                ICSSoft.STORMNET.DataObject[] loadedObjects = dataService.LoadObjects(lcs);
                Salad2 actualSalad = (Salad2)loadedObjects[0];

                // Assert.
                Assert.True(actualSalad.Ingridient1.Equals(actualSalad.Ingridient2));
            }
        }
    }
}
