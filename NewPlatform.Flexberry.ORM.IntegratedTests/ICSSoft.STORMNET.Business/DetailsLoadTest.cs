namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
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
                Assert.Equal(4, loadedProperties.Length);

                string s = "Подосиновая";
                медведь.Берлога[0].Наименование = s;
                ds.LoadObject(медведь, false, false);

                Assert.Equal(s, медведь.Берлога[0].Наименование);
                Assert.Equal(1, медведь.Берлога[0].GetAlteredPropertyNames(true).Length);
            }
        }

        /// <summary>
        /// Метод для проверки логики добавления свойства агрегатора в представление детейла. TODO: добиться повторения бага
        /// </summary>
        [Fact]
        public void AgregatorPropertyAddToViewTest()
        {
            Exception exception = null;
            foreach (IDataService ds in DataServices)
            {
                if (ds is OracleDataService)
                {
                    // TODO: Fix multithreading support for OracleDataService and Long names.
                    continue;
                }

                ExternalLangDef langdef = ExternalLangDef.LanguageDef;
                langdef.DataService = ds;

                (ds as SQLDataService).OnGenerateSQLSelect -= ThreadTesting_OnGenerateSQLSelect;
                (ds as SQLDataService).OnGenerateSQLSelect += ThreadTesting_OnGenerateSQLSelect;
                output.WriteLine($"start {ds.GetType().Name}");

                Порода порода = new Порода() { Название = "Беспородная" };
                Кошка кошка = new Кошка() { Кличка = nameof(AgregatorPropertyAddToViewTest), Порода = порода, ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая };
                кошка.Лапа.Add(new Лапа() { Номер = 1 });
                ds.UpdateObject(кошка);

                for (int i = 0; i < 2; i++)
                {
                    output.WriteLine($"start iteration {i}");
                    System.Threading.ThreadPool.SetMinThreads(150, 1);

                    int index = 0;

                    var loop = new int[150];
                    Parallel.ForEach(
                        loop,
                        item =>
                        {
                            if (exception == null)
                            {
                                try
                                {
                                    int taskIndex = index++;
                                    output.WriteLine($"start task {taskIndex}");
                                    // Arrange.
                                    Information.ClearCacheGetView();

                                    LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.k_КошкаE);
                                    lcs.ReturnTop = 1;
                                    lcs.LimitFunction = langdef.GetFunction(langdef.funcLike, new VariableDef(langdef.StringType, Information.ExtractPropertyName<Кошка>(к => к.Кличка)), nameof(AgregatorPropertyAddToViewTest));

                                    // Act & Assert.
                                    DataObject[] dObjs = ds.LoadObjects(lcs);
                                    output.WriteLine($"end task {taskIndex}");
                                }
                                catch (Exception ex)
                                {
                                    exception = ex;
                                }
                            }
                        });

                    System.Threading.ThreadPool.SetMinThreads(1, 1);

                    if (exception != null)
                    {
                        throw exception;
                    }

                    // Сбрасываем кеш для следующей попытки.
                    Information.ClearCacheGetView();

                    output.WriteLine($"end iteration {i}");
                }
                                
                кошка.SetStatus(ObjectStatus.Deleted);
                порода.SetStatus(ObjectStatus.Deleted);
                DataObject[] dataObjsForDel = new DataObject[] { кошка, порода };
                ds.UpdateObjects(ref dataObjsForDel);

            }
        }

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
