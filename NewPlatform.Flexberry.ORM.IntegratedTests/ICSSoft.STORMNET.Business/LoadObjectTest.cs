namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Проверка логики по зачитке объектов.
    /// </summary>
    public class LoadObjectTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoadObjectTest()
            : base("LOTest")
        {
        }

        /// <summary>
        /// Проверка зачитки объекта по представлению, где нет свойств мастера,
        /// с установленным в конструкторе этим свойством мастера. Вычитка через lcs.
        /// TODO: нужно сделать чтобы обязательно выполнялся.
        /// </summary>
        [Fact(Skip = "Verify the correctness of this test method.")]
        public void TestMethod1()
        {
            // TODO: в чём состоит проблема: в классе Лес в конструкторе стоит "страна.SetExistObjectPrimaryKey("C254B561-B867-4888-837F-D8B68211E497")", но это не отрабатывает.
            foreach (IDataService dataService in DataServices)
            {
                string[] props = new[]
                { "Название", "Площадь", "Страна" /*,"Страна.__PrimaryKey"*/, /*"Страна.Название",*/ "Заповедник" };
                SQLDataService ds = (SQLDataService)dataService;

                Assert.NotNull(ds);
                View view = new View();
                view.DefineClassType = typeof(Лес);
                foreach (string prop in props)
                {
                    view.AddProperty(prop);
                }

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Лес), view);

                var dataObjects = ds.LoadObjects(lcs);
                Лес лес = (Лес)dataObjects[0];
                Assert.NotEqual("{C254B561-B867-4888-837F-D8B68211E497}",
                    лес.Страна.__PrimaryKey.ToString().ToUpper());
            }
        }

        /// <summary>
        /// Проверка зачитки объекта по представлению где нет свойств мастера
        /// с установленным в конструкторе этим свойством мастера. Вычитка одного объекта.
        /// </summary>
        [Fact]
        public void TestMethod2()
        {
            foreach (IDataService dataService in DataServices)
            {
                SQLDataService ds = (SQLDataService)dataService;

                string[] props = new[]
                {
                    Information.ExtractPropertyPath<Лес>(x => x.Название),
                    Information.ExtractPropertyPath<Лес>(x => x.Площадь),
                    Information.ExtractPropertyPath<Лес>(x => x.Страна),
                    Information.ExtractPropertyPath<Лес>(x => x.Заповедник),
                };

                var view = new View();
                view.DefineClassType = typeof(Лес);
                foreach (string prop in props)
                {
                    view.AddProperty(prop);
                }

                // Чтобы в базе был подходящий лес, делаем его сами.
                var country = new Страна() { Название = "РФ" };
                var forest = new Лес() { Название = "Черняевский", Страна = country };
                var updateArray = new DataObject[] { country, forest };
                ds.UpdateObjects(ref updateArray);

                var лес = new Лес();
                лес.SetExistObjectPrimaryKey(forest.__PrimaryKey);

                ds.LoadObject(view, лес, false, false);

                Assert.Equal(country.__PrimaryKey, лес.Страна.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки определения индексов объектов в выборке.
        /// На примере медведей проверяется корректность определения индекса одного
        /// объекта и нескольких объектов.
        /// </summary>
        [Fact]
        public void TestLoadingObjectIndex()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;
                var ldef = SQLWhereLanguageDef.LanguageDef;

                // Сначала создаём структуру данных, требуемую для теста.
                var forest = new Лес();
                var bear1 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 2 };
                var bear2 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 5 };
                var bear3 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 8 };
                var updateObjectsArray = new DataObject[] { forest, bear3, bear1, bear2 };
                ds.UpdateObjects(ref updateObjectsArray);

                // Формируем функцию ограничения, что нас интересуют только медведи из текущего леса.
                Function functionCurrentForest = ldef.GetFunction(
                    ldef.funcEQ,
                    new VariableDef(ldef.GuidType, Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания)),
                    forest.__PrimaryKey);

                var view = Медведь.Views.OrderNumberTest;
                string numberPropertyName = Information.ExtractPropertyPath<Медведь>(x => x.ПорядковыйНомер);

                // Проверка индекса первого элемента.
                var lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                lcs1.ColumnsSort = new[] { new ColumnsSortDef(numberPropertyName, SortOrder.Asc) };

                Function func1 = ldef.GetFunction(
                    ldef.funcAND,
                    functionCurrentForest,
                    ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey),
                        bear2.__PrimaryKey));

                int index1 = ds.GetObjectIndex(lcs1, func1);
                Assert.Equal(2, index1);

                // Проверка индекса первого элемента c учетом ограничения на номер записи.
                var lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                lcs2.ColumnsSort = new[] { new ColumnsSortDef(numberPropertyName, SortOrder.Asc) };
                lcs2.RowNumber = new RowNumberDef(2, 3);

                Function func2 = ldef.GetFunction(
                    ldef.funcAND,
                    functionCurrentForest,
                    ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey),
                        bear2.__PrimaryKey));

                int index2 = ds.GetObjectIndex(lcs2, func2);
                Assert.Equal(1, index2);

                // Проверка индекса нескольких элементов.
                var lcs3 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                lcs3.ColumnsSort = new[] { new ColumnsSortDef(numberPropertyName, SortOrder.Asc) };

                var variableDef3 = new VariableDef(ldef.NumericType, numberPropertyName);
                var func3 = ldef.GetFunction(ldef.funcG, variableDef3, 3);

                int[] indexes3 = ds.GetObjectIndexes(lcs3, func3);
                Assert.NotNull(indexes3);
                Assert.Equal(indexes3.Length, 2);
                Assert.Equal(indexes3[0], 2);
                Assert.Equal(indexes3[1], 3);

                // Проверка индексов с первичными ключами элементов.
                IDictionary<int, string> indexesWithPk3 = ds.GetObjectIndexesWithPks(lcs3, func3);
                Assert.NotNull(indexesWithPk3);
                Assert.Equal(indexesWithPk3.Count, 2);
                Assert.Equal(indexesWithPk3[2], bear2.__PrimaryKey.ToString());
                Assert.Equal(indexesWithPk3[3], bear3.__PrimaryKey.ToString());
            }
        }

        /// <summary>
        /// Test for paging with ordering with ColumnsSort.
        /// </summary>
        [Fact]
        public void TestLoadingObjectByPageWithOrder()
        {
            var view = Медведь.Views.OrderNumberTest;
            string sortPropertyName = Information.ExtractPropertyPath<Медведь>(x => x.ЦветГлаз);

            view.AddProperty(sortPropertyName);

            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
            lcs.ColumnsSort = new[] { new ColumnsSortDef(sortPropertyName, SortOrder.Asc) };

            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService)
                {
                    continue;
                }

                Assert.Equal(0, PageOrderingTest((SQLDataService)dataService, lcs));
            }
        }

        /// <summary>
        /// Test for paging with ordering without ColumnsSort.
        /// </summary>
        [Fact]
        public void TestLoadingObjectByPageWithoutOrder()
        {
            var view = Медведь.Views.OrderNumberTest;
            string sortPropertyName = Information.ExtractPropertyPath<Медведь>(x => x.ЦветГлаз);

            view.AddProperty(sortPropertyName);

            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);

            foreach (IDataService dataService in DataServices)
            {
                Assert.Equal(0, PageOrderingTest((SQLDataService)dataService, lcs));
            }
        }

        private int PageOrderingTest(SQLDataService ds, LoadingCustomizationStruct lcs)
        {
            var ldef = SQLWhereLanguageDef.LanguageDef;

            var forest = new Лес();

            int bearCount = 500;
            int bearPerPage = 10;

            var updateObjectsArray = new DataObject[bearCount];

            for (int i = 0; i < bearCount; i++)
            {
                var bear = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = i + 1, ЦветГлаз = (i % 20).ToString() };
                updateObjectsArray[i] = bear;
            }

            ds.UpdateObjects(ref updateObjectsArray);

            var result = new List<int>();

            for (int i = 1; i <= bearCount; i += bearPerPage)
            {
                lcs.RowNumber = new RowNumberDef(i, i + bearPerPage - 1);
                var pagedBears = ds.LoadObjects(lcs).Cast<Медведь>().ToArray();
                result.AddRange(pagedBears.Select(b => b.ПорядковыйНомер));
            }

            var query = result.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => new { Element = y.Key, Counter = y.Count() })
                .ToList();

            return query.Count;
        }

        /// <summary>
        /// Тестовый метод для проверки получения индексов объектов данных с первичными ключами
        /// при нулевых (<c>null</c>) аргументах. Не должно быть выброшено исключений и должен
        /// быть возвращен пустой список результатов.
        /// </summary>
        [Fact]
        public void TestLoadingObjectIndexesWithNullParams()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                IDictionary<int, string> indexes = ds.GetObjectIndexesWithPks(null, null, null);
                Assert.NotNull(indexes);
                Assert.Equal(0, indexes.Count);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки получения индексов объектов данных с отрицательным значением
        /// максимального числа возвращаемых результатов. Должно быть выброшено исключение <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [Fact]
        public void TestLoadingObjectIndexesWithNegativeMaxResultCount()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                foreach (IDataService dataService in DataServices)
                {
                    var ldef = SQLWhereLanguageDef.LanguageDef;
                    var ds = (SQLDataService)dataService;
                    var view = new View { DefineClassType = typeof(Медведь) };
                    var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                    var lf = ldef.GetFunction(ldef.funcG, 1, 0);

                    ds.GetObjectIndexesWithPks(lcs, lf, -1);
                }
            });
            Assert.IsType(typeof(ArgumentOutOfRangeException), exception);
        }

        /// <summary>
        /// Тестовый метод для проверки получения индексов объектов данных с установленным ограничением.
        /// на максимальное число возвращаемых результатов.
        /// </summary>
        [Fact]
        public void TestLoadingObjectIndexesWithMaxResultsCount()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;
                var ldef = SQLWhereLanguageDef.LanguageDef;

                // Сначала создаём структуру данных, требуемую для теста.
                var forest = new Лес();
                var bear1 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 2 };
                var bear2 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 5 };
                var bear3 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 8 };
                var updateObjectsArray = new DataObject[] { forest, bear3, bear1, bear2 };

                ds.UpdateObjects(ref updateObjectsArray);

                var view = Медведь.Views.OrderNumberTest;
                string numberPropertyName = Information.ExtractPropertyPath<Медведь>(x => x.ПорядковыйНомер);
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(numberPropertyName, SortOrder.Asc) };

                var variable = new VariableDef(ldef.NumericType, numberPropertyName);
                Function func = ldef.GetFunction(
                    ldef.funcAND,
                    ldef.GetFunction(
                        ldef.funcEQ,
                        new VariableDef(ldef.GuidType, Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания)),
                        forest.__PrimaryKey),
                    ldef.GetFunction(ldef.funcG, variable, 3));

                // Меньше чем всего результатов.
                IDictionary<int, string> indexes1 = ds.GetObjectIndexesWithPks(lcs, func, 1);

                // Столько же, сколько всего результатов.
                IDictionary<int, string> indexes2 = ds.GetObjectIndexesWithPks(lcs, func, 2);

                // Больше чем число результатов.
                IDictionary<int, string> indexes3 = ds.GetObjectIndexesWithPks(lcs, func, 3);

                Assert.NotNull(indexes1);
                Assert.Equal(1, indexes1.Count);
                Assert.Equal(bear2.__PrimaryKey.ToString(), indexes1[2]);

                Assert.NotNull(indexes2);
                Assert.Equal(2, indexes2.Count);
                Assert.Equal(bear2.__PrimaryKey.ToString(), indexes2[2]);
                Assert.Equal(bear3.__PrimaryKey.ToString(), indexes2[3]);

                Assert.NotNull(indexes3);
                Assert.Equal(2, indexes3.Count);
                Assert.Equal(bear2.__PrimaryKey.ToString(), indexes3[2]);
                Assert.Equal(bear3.__PrimaryKey.ToString(), indexes3[3]);
            }
        }

        /// <summary>
        /// Тестовый метод для вычислимого поля
        /// с использованием dataService.LoadStringedObjectView('\t', lcs).
        /// </summary>
        [Fact(Skip = "Раскоментить после решения задачи 4009.")]
        public void LoadingObjectLoadStringedObjectViewTest()
        {
            // TODO: Обработать после выполнения задачи 4009

            var dataService = new MSSQLDataService();
            dataService.CustomizationString = "SERVER=rtc-storm;Trusted_connection=yes;DATABASE=dochitka_test;";

            // Cоздание тестовой записи.
            var тестовыйМедведь = new Медведь() { ПорядковыйНомер = 15, Вес = 39 };
            dataService.UpdateObject(тестовыйМедведь);

            var view = new View { DefineClassType = typeof(Медведь) };
            view.AddProperties(
                new string[1]
                        {
                            "ВычислимоеПоле",
                        });

            // Загрузка объектов.
            IQueryable<Медведь> dataМедведи =
                dataService.Query<Медведь>(view).Where(w => w.__PrimaryKey == тестовыйМедведь.__PrimaryKey);

            var lcs = LinqToLcs.GetLcs(dataМедведи.Expression, view);
            lcs.View = view;
            lcs.LoadingTypes = new[] { typeof(Медведь) };
            lcs.ReturnType = LcsReturnType.Objects;

            // Загрузка данных без создания объктов.
            var медведи = dataService.LoadStringedObjectView('\t', lcs);

            Assert.True(1 == медведи.Length, "Запись должна быть одна.");
            foreach (var stringDataView in медведи)
            {
                int fieldSum = Int32.Parse(stringDataView.ObjectedData[0].ToString());
                Assert.True(54 == fieldSum, "ВычислимоеПоле");
            }

            Assert.True(1 == dataМедведи.Count(), "Запись должна быть одна.");
            foreach (var медведь in dataМедведи)
            {
                Assert.True(54 == медведь.ВычислимоеПоле, "ВычислимоеПоле");
            }

            // Удаление тестовой записи.
            тестовыйМедведь.SetStatus(ObjectStatus.Deleted);
            dataService.UpdateObject(тестовыйМедведь);
        }

        /// <summary>
        /// Тестовый метод для вычислимого поля
        /// с использованием dataService.LoadObjects(lcs).
        /// </summary>
        [Fact(Skip = "Раскоментить после решения задачи 4009.")]
        public void LoadingObjectLoadObjectsTest()
        {
            // TODO: Обработать после выполнения задачи 4009

            var dataService = new MSSQLDataService();
            dataService.CustomizationString = ConfigurationManager.ConnectionStrings["TestAppNormal"]
                .ConnectionString;

            // Создание тестовой записи.
            var тестовыйМедведь = new Медведь() { ПорядковыйНомер = 15, Вес = 39 };
            dataService.UpdateObject(тестовыйМедведь);

            var view = new View { DefineClassType = typeof(Медведь) };
            view.AddProperties(
                new string[1]
                        {
                            "ВычислимоеПоле",
                        });

            // Загрузка объектов.
            IQueryable<Медведь> dataМедведи =
                dataService.Query<Медведь>(view).Where(w => w.__PrimaryKey == тестовыйМедведь.__PrimaryKey);

            var lcs = LinqToLcs.GetLcs(dataМедведи.Expression, view);
            lcs.View = view;
            lcs.LoadingTypes = new[] { typeof(Медведь) };
            lcs.ReturnType = LcsReturnType.Objects;

            // Загрузка данных без создания объктов.
            var медведи = dataService.LoadObjects(lcs);

            Assert.True(1 == медведи.Length, "Запись должна быть одна.");
            foreach (var stringDataView in медведи)
            {
                int fieldSum = ((Медведь)stringDataView).ВычислимоеПоле;
                Assert.True(54 == fieldSum, "ВычислимоеПоле");
            }

            Assert.True(1 == dataМедведи.Count(), "Запись должна быть одна.");
            foreach (var медведь in dataМедведи)
            {
                Assert.True(1 == медведь.ВычислимоеПоле, "ВычислимоеПоле");
            }

            // Удаление тестовой записи.
            тестовыйМедведь.SetStatus(ObjectStatus.Deleted);
            dataService.UpdateObject(тестовыйМедведь);
        }

        /// <summary>
        /// Тестовый метод для вычислимого поля
        /// с использованием dataService.GetObjectsCount(lcs).
        /// </summary>
        [Fact(Skip = "Раскоментить после решения задачи 4009.")]
        public void LoadingObjectGetObjectsCount()
        {
            // TODO: Обработать после выполнения задачи 4009

            var dataService = new MSSQLDataService();
            dataService.CustomizationString = ConfigurationManager.ConnectionStrings["TestAppNormal"]
                .ConnectionString;

            // Создание тестовой записи.
            var тестовыйМедведь = new Медведь() { ПорядковыйНомер = 15, Вес = 39 };
            dataService.UpdateObject(тестовыйМедведь);

            var view = new View { DefineClassType = typeof(Медведь) };
            view.AddProperties(
                new string[1]
                        {
                            "ВычислимоеПоле",
                        });

            // Загрузка объектов.
            IQueryable<Медведь> dataМедведи =
                dataService.Query<Медведь>(view).Where(w => w.__PrimaryKey == тестовыйМедведь.__PrimaryKey);

            var lcs = LinqToLcs.GetLcs(dataМедведи.Expression, view);
            lcs.View = view;
            lcs.LoadingTypes = new[] { typeof(Медведь) };
            lcs.ReturnType = LcsReturnType.Objects;

            // Загрузка данных без создания объктов.
            var медведи = dataService.GetObjectsCount(lcs);

            Assert.True(1 == медведи, "Запись должна быть одна.");

            Assert.True(1 == dataМедведи.Count(), "Запись должна быть одна.");
            foreach (var медведь in dataМедведи)
            {
                Assert.True(54 == медведь.ВычислимоеПоле, "ВычислимоеПоле");
            }

            // Удаление тестовой записи.
            тестовыйМедведь.SetStatus(ObjectStatus.Deleted);
            dataService.UpdateObject(тестовыйМедведь);
        }

        /// <summary>
        /// Метод проверки параметра TOP при зачитке объектов данных методом <see cref="SQLDataService.LoadObjects(ICSSoft.STORMNET.Business.LoadingCustomizationStruct)"/>.
        /// </summary>
        [Fact]
        public void LoadObjectsWithTop()
        {
            foreach (IDataService dataService in DataServices)
            {
                SQLDataService ds = (SQLDataService)dataService;

                try
                {
                    // Arrange.
                    // Сначала создаём структуру данных, требуемую для теста.
                    int top = 2;
                    var forest = new Лес();
                    var bear1 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 2 };
                    var bear2 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 5 };
                    var bear3 = new Медведь() { ЛесОбитания = forest, ПорядковыйНомер = 8 };
                    var updateObjectsArray = new DataObject[] { forest, bear3, bear1, bear2 };

                    ds.UpdateObjects(ref updateObjectsArray);

                    var view = Медведь.Views.МедведьL;
                    var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                    lcs.ReturnTop = top;

                    // Выведем в консоль запрос, который генерируется данной операцией.
                    ds.AfterGenerateSQLSelectQuery -= ds_AfterGenerateSQLSelectQuery;
                    ds.AfterGenerateSQLSelectQuery += ds_AfterGenerateSQLSelectQuery;

                    // Act.
                    var dataObjects = ds.LoadObjects(lcs);

                    // Assert.
                    Assert.Equal(top, dataObjects.Length);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Тест запущен");
                    Debug.WriteLine(dataService.GetType().Name);
                    Debug.WriteLine(dataService.CustomizationString);
                    throw;
                }
                finally
                {
                    ds.AfterGenerateSQLSelectQuery -= ds_AfterGenerateSQLSelectQuery;
                }
            }
        }

        /// <summary>
        /// Метод для проверки логики зачитки детейлов. Они не должны перепутываться между агрегаторами.
        /// </summary>
        [Fact]
        public void DetailsLoadingTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // TODO: Fix OracleDataService error.
                if (dataService is OracleDataService)
                {
                    continue;
                }

                // Arrange
                SQLDataService ds = (SQLDataService)dataService;

                const string First = "Первый";
                const string Second = "Второй";
                const string Third = "Третий";
                const string Fourth = "Четвертый";
                const int catCount = 1000;

                for (int i = 0; i < catCount; i++)
                {
                    Кошка aggregator = new Кошка
                    {
                        ДатаРождения = (NullableDateTime)DateTime.Now,
                        Тип = ТипКошки.Дикая,
                        Порода = new Порода { Название = "Чеширская" + i },
                        Кличка = "Мурка" + i,
                    };
                    aggregator.Лапа.AddRange(
                        new Лапа { Цвет = First },
                        new Лапа { Цвет = Second },
                        new Лапа { Цвет = Third },
                        new Лапа { Цвет = Fourth });

                    ds.UpdateObject(aggregator);
                }

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.КошкаE);

                // Act
                DataObject[] dataObjects = ds.LoadObjects(lcs);

                // Assert
                Assert.Equal(catCount, dataObjects.Length);

                foreach (Кошка dataObject in dataObjects)
                {
                    Assert.Equal(4, dataObject.Лапа.Count);
                    Assert.True(dataObject.Лапа.Cast<Лапа>().Any(paw => paw.Цвет == First));
                    Assert.True(dataObject.Лапа.Cast<Лапа>().Any(paw => paw.Цвет == Second));
                    Assert.True(dataObject.Лапа.Cast<Лапа>().Any(paw => paw.Цвет == Third));
                    Assert.True(dataObject.Лапа.Cast<Лапа>().Any(paw => paw.Цвет == Fourth));
                }
            }
        }

        /// <summary>
        /// Метод для проверки логики зачитки строкового представления агрегатора при наличии детейлов.
        /// </summary>
        [Fact]
        public void DetailsStringLoadingTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange
                SQLDataService ds = (SQLDataService)dataService;

                const string First = "Первый";
                const string Second = "Второй";
                const string Third = "Третий";
                const string Fourth = "Четвертый";
                const int catCount = 1000;

                for (int i = 0; i < catCount; i++)
                {
                    Кошка aggregator = new Кошка
                    {
                        ДатаРождения = (NullableDateTime)DateTime.Now,
                        Тип = ТипКошки.Дикая,
                        Порода = new Порода { Название = "Чеширская" + i },
                        Кличка = "Мурка" + i,
                    };
                    aggregator.Лапа.AddRange(
                        new Лапа { Цвет = First },
                        new Лапа { Цвет = Second },
                        new Лапа { Цвет = Third },
                        new Лапа { Цвет = Fourth });

                    ds.UpdateObject(aggregator);
                }

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.КошкаE);

                // Act
                ObjectStringDataView[] dataObjectDefs = ds.LoadStringedObjectView('|', lcs);

                // Assert
                Assert.Equal(catCount, dataObjectDefs.Length);
            }
        }

        /// <summary>
        /// Проверка обработки NullableDateTime в linq-выражениях.
        /// </summary>
        [Fact(Skip = "Раскоментить после исправления ошибки 93668.")]
        public void NullableDateTimeTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                // Добавляются объекты данных в БД, чтобы проверить обработку
                // NullableDateTime в linq-выражениях.

                // Мастер.
                var testMasterObject = new FullTypesMaster1();
                testMasterObject.PoleInt = 1;

                // Первый объект для проверки работы linq-выражений,
                // если поле типа NullDateTime принимает значение null.
                var testObj1 = new FullTypesMainAgregator();
                testObj1.PoleNullDateTime = null;
                testObj1.FullTypesMaster1 = testMasterObject;

                // Второй объект для проверки работы linq-выражений,
                // если поле типа NullDateTime принимает значение текущего даты-времени.
                var testObj2 = new FullTypesMainAgregator();
                testObj2.PoleNullDateTime = DateTime.Now;
                testObj2.FullTypesMaster1 = testMasterObject;

                // Третий объект для проверки работы linq-выражений,
                // если поле типа NullDateTime принимает конкретное значение.
                var testObj3 = new FullTypesMainAgregator();
                testObj3.PoleNullableDateTime = (NullableDateTime)new DateTime(2015, 11, 14, 10, 37, 44);
                testObj3.FullTypesMaster1 = testMasterObject;

                // Список объектов данных, которые нужно сохранить.
                List<ICSSoft.STORMNET.DataObject> objectsToUpdate = new List<ICSSoft.STORMNET.DataObject>();

                objectsToUpdate.AddRange(new ICSSoft.STORMNET.DataObject[]
                {
                    testMasterObject, testObj1, testObj2, testObj3,
                });

                ICSSoft.STORMNET.DataObject[] objectsToUpdateArray = objectsToUpdate.ToArray();
                dataService.UpdateObjects(ref objectsToUpdateArray);

                // Контролдьная дата-время.
                var gaugeNullableDateTime = new NullableDateTime
                {
                    Value = DateTime.Today.AddDays(-7),
                };

                // Act.
                // Для testObj3 должно выполниться только первое условие под .Where.
                List<FullTypesMainAgregator> testList = dataService
                .Query<FullTypesMainAgregator>(FullTypesMainAgregator.Views.FullView.Name)
                .Where(w => w.PoleNullableDateTime <= (NullableDateTime)DateTime.Now
                && w.PoleNullableDateTime >= gaugeNullableDateTime)
                .ToList();
            }
        }

        /// <summary>
        /// Метод проверки вычитки объекта с нехранимым мастером без <see cref="DataServiceExpression"/> методом <see cref="SQLDataService.LoadObjects(LoadingCustomizationStruct)"/>.
        /// </summary>
        [Fact]
        public void LoadObjectWithNotStoredMasterTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                SQLDataService ds = (SQLDataService)dataService;

                try
                {
                    // Arrange.
                    // Сначала создаём структуру данных, требуемую для теста.
                    int top = 1;
                    var state = new Страна() { Название = "zzz" };
                    var forest = new Лес() { Название = "yyy", Страна = state };
                    var updateObjectsArray = new DataObject[] { state, forest };

                    ds.UpdateObjects(ref updateObjectsArray);

                    var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Страна), Страна.Views.СтранаL);
                    lcs.View.AddProperty(Information.ExtractPropertyPath<Страна>(s => s.Президент.__PrimaryKey));
                    lcs.ReturnTop = top;

                    View view = new View() { DefineClassType = typeof(Лес), Name = "yyy" };
                    var lcsForest = LoadingCustomizationStruct.GetSimpleStruct(typeof(Лес), view);
                    lcsForest.View.AddProperty(Information.ExtractPropertyPath<Лес>(f => f.Название));
                    lcsForest.View.AddProperty(Information.ExtractPropertyPath<Лес>(f => f.Страна.Президент.__PrimaryKey));
                    lcsForest.ReturnTop = top;

                    // Выведем в консоль запрос, который генерируется данной операцией.
                    ds.AfterGenerateSQLSelectQuery -= ds_AfterGenerateSQLSelectQuery;
                    ds.AfterGenerateSQLSelectQuery += ds_AfterGenerateSQLSelectQuery;

                    // Act.
                    var dataObjects = ds.LoadObjects(lcs);
                    var dataObjectsForest = ds.LoadObjects(lcsForest);

                    // Assert.
                    Assert.Equal(top, dataObjects.Length);
                    Assert.Equal(top, dataObjectsForest.Length);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Тест запущен");
                    Debug.WriteLine(dataService.GetType().Name);
                    Debug.WriteLine(dataService.CustomizationString);
                    throw;
                }
                finally
                {
                    ds.AfterGenerateSQLSelectQuery -= ds_AfterGenerateSQLSelectQuery;
                }
            }
        }

        /// <summary>
        /// Метод для проверки логики зачитки строкового представления с сортировкой.
        /// </summary>
        [Fact]
        public void OrderedLoadStringedObjectViewTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange
                SQLDataService ds = (SQLDataService)dataService;

                Random random = new Random();
                List<Кошка> objectsToUpdate = new List<Кошка>();

                int catCount = 1000;
                for (int i = 0; i < catCount; i++)
                {
                    Кошка cat = new Кошка
                    {
                        ДатаРождения = (NullableDateTime)DateTime.Now.AddDays(random.Next(29)).AddMonths(random.Next(13)).AddYears(random.Next(catCount)),
                        Тип = ТипКошки.Дикая,
                        Порода = new Порода { Название = "Чеширская" + i },
                        Кличка = "Мурка" + i,
                    };
                    objectsToUpdate.Add(cat);
                }

                DataObject[] dataObjects = objectsToUpdate.ToArray();
                ds.UpdateObjects(ref dataObjects);

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.КошкаE);
                lcs.ColumnsSort = new[] { new ColumnsSortDef(nameof(Кошка.ДатаРождения), SortOrder.Asc), new ColumnsSortDef(SQLWhereLanguageDef.StormMainObjectKey, SortOrder.Asc) };
                int returnTop = catCount / 2;
                lcs.ReturnTop = returnTop;
                lcs.RowNumber = new RowNumberDef(1, returnTop);

                // Act
                ObjectStringDataView[] dataObjectDefs = ds.LoadStringedObjectView('|', lcs);

                // Assert
                Assert.Equal(returnTop, dataObjectDefs.Length);

                bool ordered = true;

                DateTime lastDate = DateTime.Now;

                for (int i = 0; i < returnTop; i++)
                {
                    DateTime date = (DateTime)(dataObjectDefs[i].ObjectedData[1]);

                    if (date < lastDate)
                    {
                        ordered = false;
                        break;
                    }

                    lastDate = date;
                }

                Assert.True(ordered);
            }
        }

        /// <summary>
        /// Проверка зачитки объекта по ограничению с Nullable атрибутами.
        /// </summary>
        [Fact]
        public void NullableLimitLoadingTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                string[] props = new[]
                {
                    Information.ExtractPropertyPath<FullTypesMaster1>(x => x.PoleNullChar),
                    Information.ExtractPropertyPath<FullTypesMaster1>(x => x.PoleInt),
                };

                var view = new View();
                view.DefineClassType = typeof(FullTypesMaster1);
                foreach (string prop in props)
                {
                    view.AddProperty(prop);
                }

                DataObject[] objectsForUpdate = new DataObject[]
                {
                    new FullTypesMaster1() { PoleNullChar = 'W', PoleInt = 1 },
                    new FullTypesMaster1() { PoleNullChar = null, PoleInt = 2 },
                };
                dataService.UpdateObjects(ref objectsForUpdate);

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMaster1), view);

                SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
                lcs.LimitFunction = langDef.GetFunction(langDef.funcIsNull, new VariableDef(langDef.BoolType, nameof(FullTypesMaster1.PoleNullChar)));

                // Act.
                DataObject[] loadedObjects = dataService.LoadObjects(lcs);

                // Assert.
                Assert.Single(loadedObjects);
                Assert.Equal(2, (loadedObjects[0] as FullTypesMaster1).PoleInt);
            }
        }

        /// <summary>
        /// Обработчик события генерации SQL-запроса.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события генерации SQL-запроса.</param>
        private void ds_AfterGenerateSQLSelectQuery(object sender, GenerateSQLSelectQueryEventArgs e)
        {
            Debug.WriteLine(e.GeneratedQuery);
        }
    }
}
