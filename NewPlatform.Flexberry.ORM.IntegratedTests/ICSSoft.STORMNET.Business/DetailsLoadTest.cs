namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using System.Configuration;

    /// <summary>
    /// Проверка логики зачитки детейлов.
    /// </summary>
    
    public class DetailsLoadTest : BaseIntegratedTest
    {
        public DetailsLoadTest()
            : base("DetLoad")
        {
        }

        private Random _random = new Random();

        #region Проверка на адекватное количество загруженных свойств после догрузки

        /// <summary>
        /// Проверим сначала собственные свойства.
        /// </summary>
        [Fact]
        public void LoadTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService) dataService;

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
                var ds = (SQLDataService) dataService;

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
                var ds = (SQLDataService) dataService;

                var view = Медведь.Views.LoadTestView;

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof (Медведь), view);
                lcs.ColumnsSort = new[]
                {
                    new ColumnsSortDef(Information.ExtractPropertyPath<Медведь>(x => x.__PrimaryKey),
                        _random.Next(10) > 5 ? SortOrder.Asc : SortOrder.Desc)
                };

                lcs.InitDataCopy = true;

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                var dataObjects = ds.LoadObjects(lcs);

                Assert.True(dataObjects.Length > 0);

                var медведь = (Медведь) dataObjects[0];
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

            #endregion
        }
    }
}
