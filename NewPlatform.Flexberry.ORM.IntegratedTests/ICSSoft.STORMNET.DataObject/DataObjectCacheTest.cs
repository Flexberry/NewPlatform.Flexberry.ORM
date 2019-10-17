namespace NewPlatform.Flexberry.ORM.IntegratedTests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

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
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Salad2), Salad2.Views.Salad2E);
                lcs.ReturnTop = 1;

                // Act.
                ICSSoft.STORMNET.DataObject[] loadedObjects = dataService.LoadObjects(lcs);
                Salad2 actualSalad = (Salad2)loadedObjects[0];

                // Assert.
                Assert.True(actualSalad.Ingridient1.Equals(actualSalad.Ingridient2));
            }
        }

        /// <summary>
        /// Проверка <see cref="DataObjectCache"/> на выполнение основной функции: инстанции мастеров после зачитки смотрят на один и тот же объект.
        /// </summary>
        [Fact]
        public void DataObjectCacheSingleMasterTest2()
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
                View view1 = new View();
                view1.DefineClassType = typeof(Salad2);
                view1.AddProperties(Information.ExtractPropertyPath<Salad2>(p => p.SaladName), Information.ExtractPropertyPath<Salad2>(p => p.Ingridient1));

                LoadingCustomizationStruct lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Salad2), view1);
                lcs1.ReturnTop = 1;

                View view2 = new View();
                view2.DefineClassType = typeof(Salad2);
                view2.AddProperties(Information.ExtractPropertyPath<Salad2>(p => p.SaladName), Information.ExtractPropertyPath<Salad2>(p => p.Ingridient2));

                LoadingCustomizationStruct lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Salad2), view2);
                lcs2.ReturnTop = 1;

                DataObjectCache dataObjectCache = new DataObjectCache();
                dataObjectCache.StartCaching(true);

                // Act.
                ICSSoft.STORMNET.DataObject[] loadedObjects1 = dataService.LoadObjects(lcs1, dataObjectCache);
                Salad2 actualSalad1 = (Salad2)loadedObjects1[0];

                ICSSoft.STORMNET.DataObject[] loadedObjects2 = dataService.LoadObjects(lcs2, dataObjectCache);
                Salad2 actualSalad2 = (Salad2)loadedObjects2[0];

                // Assert.
                Assert.True(actualSalad1.Ingridient1.Equals(actualSalad2.Ingridient2));
            }
        }

        /// <summary>
        /// Проверка <see cref="DataObjectCache"/> на выполнение основной функции: инстанции мастеров после зачитки смотрят на один и тот же объект.
        /// </summary>
        [Fact]
        public void DataObjectCacheSingleMasterTest3()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                // Создадим в БД структуру, которая будет служить для проверки.
                Берлога берлога = new Берлога();
                object pk = берлога.__PrimaryKey;
                берлога.Наименование = "Первая";

                Медведь медведь = new Медведь();
                медведь.ПорядковыйНомер = 1;

                берлога.Медведь = медведь;

                ICSSoft.STORMNET.DataObject[] objects1 = { медведь, берлога };

                dataService.UpdateObjects(ref objects1);

                // Подготовим структуру для вычитки.
                View view1 = new View();
                view1.DefineClassType = typeof(Берлога);
                view1.AddProperties(Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ПорядковыйНомер), Information.ExtractPropertyPath<Берлога>(p => p.Наименование), Information.ExtractPropertyPath<Берлога>(p => p.Комфортность), Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ЦветГлаз));

                LoadingCustomizationStruct lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Берлога), view1);

                lcs1.LimitFunction = FunctionBuilder.BuildEquals(pk);
                lcs1.ReturnTop = 1;
                DataObjectCache dataObjectCache = new DataObjectCache();
                dataObjectCache.StartCaching(true);
                ICSSoft.STORMNET.DataObject[] loadedObjects1 = dataService.LoadObjects(lcs1, dataObjectCache);
                Берлога берлога1 = loadedObjects1[0] as Берлога;
                string newText = "изменённое наименование";

                // Act.
                берлога1.Наименование = newText;
                берлога1.Медведь.ПорядковыйНомер = 55;

                ICSSoft.STORMNET.DataObject[] objects2 = { берлога1.Медведь, берлога1 };

                dataService.UpdateObjects(ref objects2);

                // Assert.
                View view2 = new View();
                view2.DefineClassType = typeof(Берлога);
                view2.AddProperties(Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ПорядковыйНомер), Information.ExtractPropertyPath<Берлога>(p => p.Наименование),  Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ЦветГлаз));

                LoadingCustomizationStruct lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Берлога), view2);
                lcs2.LimitFunction = FunctionBuilder.BuildEquals(pk);

                lcs2.ReturnTop = 1;

                ICSSoft.STORMNET.DataObject[] loadedObjects2 = dataService.LoadObjects(lcs2);
                Берлога берлога2 = (Берлога)loadedObjects2[0];

                Assert.Equal(66, берлога2.Медведь.ПорядковыйНомер);
                Assert.Equal(newText, берлога2.Наименование);
                Assert.Equal("Зелёный", берлога2.Медведь.ЦветГлаз);
            }
        }
    }
}
