namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;
    using Xunit.Abstractions;

    public class SQLDataServiceGenerateSQLTests : BaseSQLDataServiceTests
    {
        /// <inheritdoc cref="BaseSQLDataServiceTests" />
        public SQLDataServiceGenerateSQLTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper) { }

        /// <summary>
        /// Тест проверяет метод <see cref="SQLDataService.GenerateSQLSelect(LoadingCustomizationStruct,bool)"/>,
        /// чтобы в sql-запрос не добавляется left join, когда от мастера требуется только его первичный ключ.
        /// </summary>
        [Fact]
        public void GenerateSQLSelectNoJoinsTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var view0 = new View { DefineClassType = typeof(Медведь) };
                view0.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания));
                var lcs0 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view0);

                var view1 = new View { DefineClassType = typeof(Медведь) };
                view1.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                var lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view1);

                var view2 = new View { DefineClassType = typeof(Медведь) };
                view2.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                var lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view2);

                var view3 = new View { DefineClassType = typeof(Медведь) };
                view3.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания));
                var lcs3 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view3);

                // Act.
                string query0 = ds.GenerateSQLSelect(lcs0, false);
                string query1 = ds.GenerateSQLSelect(lcs1, false);
                string query2 = ds.GenerateSQLSelect(lcs2, false);
                string query3 = ds.GenerateSQLSelect(lcs3, false);
                string from0 = query0.Substring(query0.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from1 = query1.Substring(query1.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from2 = query2.Substring(query2.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from3 = query3.Substring(query3.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));

                // Assert.
                Assert.DoesNotContain("join", from0, StringComparison.InvariantCultureIgnoreCase);
                Assert.Equal(from0, from1);
                Assert.Equal(from0, from2);
                Assert.Equal(from0, from3);
            }
        }

        /// <summary>
        /// Тест проверяет метод <see cref="SQLDataService.GenerateSQLSelectByStorageStruct(StorageStructForView,bool,bool,string,int,bool)"/>,
        /// чтобы в sql-запрос не добавляется left join, когда от мастера требуется только его первичный ключ.
        /// </summary>
        [Fact]
        public void GenerateSQLSelectByStorageStructNoJoinsTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var view0 = new View { DefineClassType = typeof(Медведь) };
                view0.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания));
                var storageStruct0 = Information.GetStorageStructForView(
                    view0,
                    view0.DefineClassType,
                    StorageTypeEnum.SimpleStorage,
                    ds.GetPropertiesInExpression,
                    ds.GetType());

                var view1 = new View { DefineClassType = typeof(Медведь) };
                view1.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                var storageStruct1 = Information.GetStorageStructForView(
                    view1,
                    view1.DefineClassType,
                    StorageTypeEnum.SimpleStorage,
                    ds.GetPropertiesInExpression,
                    ds.GetType());

                var view2 = new View { DefineClassType = typeof(Медведь) };
                view2.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                var storageStruct2 = Information.GetStorageStructForView(
                    view2,
                    view2.DefineClassType,
                    StorageTypeEnum.SimpleStorage,
                    ds.GetPropertiesInExpression,
                    ds.GetType());

                string advField = ds.GetConvertToTypeExpression(typeof(decimal), 0.ToString())
                                  + " as "
                                  + ds.PutIdentifierIntoBrackets("STORMNETDATAOBJECTTYPE");

                // Act.
                string query0 = ds.GenerateSQLSelectByStorageStruct(storageStruct0, true, true, advField, 0, false);
                string query1 = ds.GenerateSQLSelectByStorageStruct(storageStruct1, true, true, advField, 0, false);
                string query2 = ds.GenerateSQLSelectByStorageStruct(storageStruct2, true, true, advField, 0, false);
                string from0 = query0.Substring(query0.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from1 = query1.Substring(query1.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from2 = query2.Substring(query2.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));

                // Assert.
                Assert.DoesNotContain("join", from0, StringComparison.InvariantCultureIgnoreCase);
                Assert.Equal(from0, from1);
                Assert.Equal(from0, from2);
            }
        }

        /// <summary>
        /// Тест проверяет метод <see cref="SQLDataService.GenerateSQLSelect(LoadingCustomizationStruct,bool)"/>,
        /// чтобы в sql-запрос добавляется только один необходимый left join.
        /// </summary>
        [Fact]
        public void GenerateSQLSelectSingleJoinTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var view0 = new View { DefineClassType = typeof(Медведь) };
                view0.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна));
                var lcs0 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view0);

                var view1 = new View { DefineClassType = typeof(Медведь) };
                view1.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна));
                var lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view1);

                var view2 = new View { DefineClassType = typeof(Медведь) };
                view2.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна.__PrimaryKey));
                var lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view2);

                var view3 = new View { DefineClassType = typeof(Медведь) };
                view3.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна.__PrimaryKey),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна));
                var lcs3 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view3);

                // Act.
                string query0 = ds.GenerateSQLSelect(lcs0, false);
                string query1 = ds.GenerateSQLSelect(lcs1, false);
                string query2 = ds.GenerateSQLSelect(lcs2, false);
                string query3 = ds.GenerateSQLSelect(lcs3, false);
                string from0 = query0.Substring(query0.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from1 = query1.Substring(query1.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from2 = query2.Substring(query2.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));
                string from3 = query3.Substring(query3.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase));

                // Assert.
                Assert.Equal(1, new Regex("left join", RegexOptions.IgnoreCase).Matches(query0).Count);
                Assert.Equal(from0, from1);
                Assert.Equal(from0, from2);
                Assert.Equal(from0, from3);
            }
        }

        /// <summary>
        /// Проверка производительности генерации select-запроса.
        /// </summary>
        [Fact]
        public void GenerateSQLSelectAgregatorPerformanceTest()
        {
            foreach (SQLDataService dataService in DataServices)
            {
                // Arrange.
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), FullTypesMainAgregator.Views.FullView);
                lcs.LimitFunction = FunctionBuilder.BuildOr(
                    FunctionBuilder.BuildIsNull<FullTypesMainAgregator>(x => x.FullTypesMaster1),
                    FunctionBuilder.BuildAnd(
                        FunctionBuilder.BuildGreaterOrEqual<FullTypesMainAgregator>(x => x.PoleDateTime, DateTime.Today),
                        FunctionBuilder.BuildExists(
                            nameof(FullTypesDetail1.FullTypesMainAgregator),
                            FullTypesDetail1.Views.FullDetailView,
                            FunctionBuilder.BuildGreaterOrEqual<FullTypesDetail1>(x => x.PoleDateTime, DateTime.Today))),
                    FunctionBuilder.BuildAnd(
                        FunctionBuilder.BuildLess<FullTypesMainAgregator>(x => x.PoleDateTime, DateTime.Today),
                        FunctionBuilder.BuildExists(
                            nameof(FullTypesDetail2.FullTypesMainAgregator),
                            FullTypesDetail2.Views.FullTypesDetail2E,
                            FunctionBuilder.BuildLess<FullTypesDetail2>(x => x.PoleDateTime, DateTime.Today))));
                lcs.ReturnTop = 100;
                lcs.RowNumber = new RowNumberDef(200, 299);
                lcs.ColumnsSort = new[]
                {
                    new ColumnsSortDef(nameof(FullTypesMainAgregator.PoleDateTime), SortOrder.Desc),
                    new ColumnsSortDef(nameof(FullTypesMainAgregator.PoleDouble), SortOrder.Asc),
                };

                // Act.
                Stopwatch stopwatch = new Stopwatch();
                int i = 0;
                stopwatch.Start();

                do
                {
                    string query = dataService.GenerateSQLSelect(lcs, false);
                    i++;
                }
                while (stopwatch.ElapsedMilliseconds < 1000);

                stopwatch.Stop();
                TestOutputHelper.WriteLine($"{nameof(GenerateSQLSelectAgregatorPerformanceTest)}@{dataService.GetType().Name}: {i} iterations");
            }
        }
    }
}
