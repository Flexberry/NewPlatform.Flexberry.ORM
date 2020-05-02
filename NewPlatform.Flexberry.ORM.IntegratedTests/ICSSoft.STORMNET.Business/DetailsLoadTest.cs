namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.UserDataTypes;
    using ICSSoft.STORMNET.Windows.Forms;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Проверка логики зачитки детейлов.
    /// </summary>
    public class DetailsLoadTest : BaseIntegratedTest
    {
        private ITestOutputHelper output;

        public DetailsLoadTest(ITestOutputHelper output)
            : base("DetLoad")
        {
            this.output = output;
        }

        private Random _random = new Random();

        /// <summary>
        /// Проверим сначала собственные свойства.
        /// </summary>
        [Fact]
        public void LoadTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                // Теперь грузим его из БД.
                var медведь = new Медведь();
                медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                ds.LoadObject(медведь, false, false);

                Assert.NotNull(медведь.Берлога);
                Assert.Equal(0, медведь.Берлога.Count);
            }
        }

        /// <summary>
        /// Проверим сначала собственные свойства.
        /// </summary>
        [Fact]
        public void LoadTest2()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                // Теперь грузим его из БД.
                Медведь медведь = new Медведь();
                медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                медведь.Берлога.Add(new Берлога
                {
                    Наименование = "Новая"
                });

                ds.LoadObject(медведь, false, false);

                Assert.NotNull(медведь.Берлога);
                Assert.Equal(1, медведь.Берлога.Count);
            }
        }

        /// <summary>
        /// Проверка загрузки объектов из базы данных.
        /// </summary>
        [Fact]
        public void LoadTest3()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                var view = Медведь.Views.LoadTestView;

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                lcs.ColumnsSort = new[]
                {
                    new ColumnsSortDef(Information.ExtractPropertyPath<Медведь>(x => x.__PrimaryKey), _random.Next(10) > 5 ? SortOrder.Asc : SortOrder.Desc)
                };

                lcs.InitDataCopy = true;

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                var dataObjects = ds.LoadObjects(lcs);

                Assert.True(dataObjects.Length > 0);

                var медведь = (Медведь)dataObjects[0];
                if (медведь.Берлога.Count == 0)
                {
                    медведь.Берлога.Add(new Берлога
                    {
                        Наименование = "Некий лес"
                    });
                    ds.UpdateObject(медведь);
                }

                var alteredPropertyNames = медведь.GetAlteredPropertyNames();
                var loadedProperties = медведь.GetLoadedProperties();

                Assert.NotNull(alteredPropertyNames);
                Assert.Equal(0, alteredPropertyNames.Length);
                Assert.NotNull(loadedProperties);
                Assert.Equal(5, loadedProperties.Length);

                string s = "Подосиновая";
                медведь.Берлога[0].Наименование = s;
                ds.LoadObject(медведь, false, false);

                Assert.Equal(s, медведь.Берлога[0].Наименование);
                Assert.Equal(1, медведь.Берлога[0].GetAlteredPropertyNames(true).Length);
            }
        }

        /// <summary>
        /// Метод для проверки логики добавления свойства агрегатора в представление детейла.
        /// </summary>
        [Fact]
        public void AgregatorPropertyAddToViewTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                if (ds is OracleDataService)
                {
                    // TODO: Fix multithreading support for OracleDataService and Long names.
                    continue;
                }

                ds.OnGenerateSQLSelect -= ThreadTesting_OnGenerateSQLSelect;
                ds.OnGenerateSQLSelect += ThreadTesting_OnGenerateSQLSelect;
                output.WriteLine($"start {ds.GetType().Name}");

                Порода порода = new Порода() { Название = "Беспородная" };
                Кошка кошка = new Кошка() { Кличка = nameof(AgregatorPropertyAddToViewTest), Порода = порода, ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая };
                кошка.Лапа.Add(new Лапа() { Номер = 1 });
                ds.UpdateObject(кошка);

                MultiThreadingTestTool multiThreadingTestTool = new MultiThreadingTestTool(MultiThreadMethod);
                multiThreadingTestTool.StartThreads(150, ds);

                var exception = multiThreadingTestTool.GetExceptions();

                if (exception != null)
                {
                    foreach (var item in exception.InnerExceptions)
                    {
                        output.WriteLine(item.Value.ToString());
                    }

                    throw exception.InnerException;
                }

                ds.OnGenerateSQLSelect -= ThreadTesting_OnGenerateSQLSelect;

                кошка.SetStatus(ObjectStatus.Deleted);
                порода.SetStatus(ObjectStatus.Deleted);
                DataObject[] dataObjsForDel = new DataObject[] { кошка, порода };
                ds.UpdateObjects(ref dataObjsForDel);
            }
        }

        [Fact]
        public void CachedCascadeDetailLoadTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var breed = new Порода { Название = "Чеширская" };
                var cat = new Кошка { ДатаРождения = NullableDateTime.UtcNow, Тип = ТипКошки.Домашняя, Порода = breed, Кличка = "Мурзик" };
                var leg1 = new Лапа { Номер = 1 };
                var leg2 = new Лапа { Номер = 2 };
                var fracture1 = new Перелом { Тип = ТипПерелома.Закрытый, Дата = DateTime.UtcNow };
                leg1.Перелом.Add(fracture1);
                cat.Лапа.AddRange(leg1, leg2);
                dataService.UpdateObject(cat);

                // OData читает мастеровые объекты вместе с первичным ключом, но можно подгрузить любое другое свойство.
                var legView = new View { DefineClassType = typeof(Лапа) };
                legView.AddProperty(nameof(Лапа.__PrimaryKey));

                // В представлении должны быть детейлы, а в представлении детейлов - детейлы следующего уровня.
                var catView = new View(typeof(Кошка), View.ReadType.WithRelated);

                // Act.
                var cache = new DataObjectCache();
                cache.StartCaching(false);
                var loadedLeg = PKHelper.CreateDataObject<Лапа>(leg1);
                dataService.LoadObject(legView, loadedLeg, false, true, cache);
                var loadedCat = PKHelper.CreateDataObject<Кошка>(cat);
                dataService.LoadObject(catView, loadedCat, false, true, cache);

                // Assert.
                Assert.Equal(ObjectStatus.UnAltered, loadedLeg.GetStatus());
                Assert.Equal(ObjectStatus.UnAltered, loadedCat.GetStatus());
            }
        }

        /// <summary>
        /// Метод для многопоточного исполнения в <see cref="AgregatorPropertyAddToViewTest"/>.
        /// </summary>
        /// <param name="sender">Словарь со значениями параметров для исполнения метода.</param>
        public void MultiThreadMethod(object sender)
        {
            var parametersDictionary = sender as Dictionary<string, object>;
            IDataService ds = parametersDictionary[MultiThreadingTestTool.ParamNameSender] as SQLDataService;
            Dictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as Dictionary<string, Exception>;

            ExternalLangDef langdef = ExternalLangDef.LanguageDef;
            langdef.DataService = ds;

            // Use a custom number of iterations: less time and false positives or more time and a guaranteed result.
            for (int i = 0; i < 5; i++)
            {
                if (!(bool)parametersDictionary[MultiThreadingTestTool.ParamNameWorking])
                {
                    return;
                }

                try
                {
                    // Arrange.
                    LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.k_КошкаE);
                    lcs.ReturnTop = 1;
                    lcs.LimitFunction = langdef.GetFunction(langdef.funcLike, new VariableDef(langdef.StringType, Information.ExtractPropertyName<Кошка>(к => к.Кличка)), nameof(AgregatorPropertyAddToViewTest));

                    // Act & Assert.
                    DataObject[] dObjs = ds.LoadObjects(lcs);
                    Information.ClearCacheGetView();
                }
                catch (Exception exception)
                {
                    exceptions.Add(Thread.CurrentThread.Name, exception);
                    parametersDictionary[MultiThreadingTestTool.ParamNameWorking] = false;
                    return;
                }
            }
        }

        /// <summary>
        /// Обработчик события генерации SQL-запроса с отловом неправильно сформированного представления.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ThreadTesting_OnGenerateSQLSelect(object sender, GenerateSQLSelectQueryEventArgs e)
        {
            View view = e.CustomizationStruct.View;
            for (int i = 0; i < view.Properties.Length; i++)
            {
                PropertyInView property = view.Properties[i];
                for (int j = i + 1; j < view.Properties.Length; j++)
                {
                    PropertyInView nextProperty = view.Properties[j];
                    if (property.Name == nextProperty.Name)
                    {
                        throw new Exception($"Свойство {property.Name} встречается несколько раз (OnGenerateSQLSelect).");
                    }
                }
            }
        }
    }
}
