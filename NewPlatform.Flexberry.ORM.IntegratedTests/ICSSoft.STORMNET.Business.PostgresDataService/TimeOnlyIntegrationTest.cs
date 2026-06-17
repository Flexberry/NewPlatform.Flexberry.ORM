namespace NewPlatform.Flexberry.ORM.IntegratedTests.Postgres
{
    using System;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using IIS.TestClassesForPostgres;
    using Xunit;

    /// <summary>
    /// E2E-тесты для типа TimeOnly: LINQ-запросы, LCS с TimeOnly-функциями.
    /// </summary>
    public class TimeOnlyIntegrationTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор. База данных будет создана с префиксом "TmOnlyInt".
        /// </summary>
        public TimeOnlyIntegrationTest()
            : base("TmOnlyInt")
        {
        }

        protected override string MssqlScript => null;

        protected override string PostgresScript => Resources.PostgresDataServiceTestScript;

        protected override string OracleScript => null;

#if NET6_0_OR_GREATER
        [Fact]
        public void TimeOnlyLinqEquals()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var t1 = new TimeOnly(10, 0, 0);
                var t2 = new TimeOnly(14, 30, 45);

                var obj1 = new Class_DateOnly { AttrTimeOnly = t1, AttrDate = DateTime.Now, AttrString = "Morning" };
                var obj2 = new Class_DateOnly { AttrTimeOnly = t2, AttrDate = DateTime.Now, AttrString = "Afternoon" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyView();

                var result = ds.Query<Class_DateOnly>(view).Where(x => x.AttrTimeOnly == t1).ToList();
                Assert.Single(result);
                Assert.Equal("Morning", result[0].AttrString);
            }
        }

        [Fact]
        public void TimeOnlyLinqHourAndMinute()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var obj1 = new Class_DateOnly { AttrTimeOnly = new TimeOnly(14, 30, 0), AttrDate = DateTime.Now, AttrString = "T1" };
                var obj2 = new Class_DateOnly { AttrTimeOnly = new TimeOnly(8, 15, 0), AttrDate = DateTime.Now, AttrString = "T2" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyView();

                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrTimeOnly.Hour == 14 && x.AttrTimeOnly.Minute == 30)
                    .ToList();

                Assert.Single(result);
                Assert.Equal("T1", result[0].AttrString);
            }
        }

        [Fact]
        public void TimeOnlyLinqSecond()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var obj1 = new Class_DateOnly { AttrTimeOnly = new TimeOnly(14, 30, 45), AttrDate = DateTime.Now, AttrString = "T1" };
                var obj2 = new Class_DateOnly { AttrTimeOnly = new TimeOnly(14, 30, 0), AttrDate = DateTime.Now, AttrString = "T2" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyView();

                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrTimeOnly.Second == 45)
                    .ToList();

                Assert.Single(result);
                Assert.Equal("T1", result[0].AttrString);
            }
        }

        [Fact]
        public void TimeOnlyLcsRangeQuery()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var langDef = ExternalLangDef.LanguageDef;

                var t1 = new TimeOnly(8, 0, 0);
                var t2 = new TimeOnly(14, 0, 0);
                var t3 = new TimeOnly(20, 0, 0);

                var obj1 = new Class_DateOnly { AttrTimeOnly = t1, AttrDate = DateTime.Now, AttrString = "T1" };
                var obj2 = new Class_DateOnly { AttrTimeOnly = t2, AttrDate = DateTime.Now, AttrString = "T2" };
                var obj3 = new Class_DateOnly { AttrTimeOnly = t3, AttrDate = DateTime.Now, AttrString = "T3" };
                var toUpdate = new DataObject[] { obj1, obj2, obj3 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyView();
                var varDef = new VariableDef(langDef.DateTimeType, "AttrTimeOnly");

                var lcsGt = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcsGt.LimitFunction = langDef.GetFunction(langDef.funcG, varDef, t2);
                var gtResult = ds.LoadObjects(lcsGt).Cast<Class_DateOnly>().ToList();
                Assert.Single(gtResult);
                Assert.Equal("T3", gtResult[0].AttrString);

                var lcsLt = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcsLt.LimitFunction = langDef.GetFunction(langDef.funcL, varDef, t2);
                var ltResult = ds.LoadObjects(lcsLt).Cast<Class_DateOnly>().ToList();
                Assert.Single(ltResult);
                Assert.Equal("T1", ltResult[0].AttrString);
            }
        }

        [Fact]
        public void TimeOnlyLcsHour()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var langDef = ExternalLangDef.LanguageDef;

                var obj1 = new Class_DateOnly { AttrTimeOnly = new TimeOnly(14, 30, 0), AttrDate = DateTime.Now, AttrString = "T1" };
                var obj2 = new Class_DateOnly { AttrTimeOnly = new TimeOnly(8, 15, 0), AttrDate = DateTime.Now, AttrString = "T2" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyView();
                var varDef = new VariableDef(langDef.DateTimeType, "AttrTimeOnly");
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, langDef.GetFunction(langDef.funcHHPart, varDef), 14);
                var result = ds.LoadObjects(lcs).Cast<Class_DateOnly>().ToList();

                Assert.Single(result);
                Assert.Equal("T1", result[0].AttrString);
            }
        }

        [Fact]
        public void TimeOnlyNullableSaveLoadNull()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var obj = new Class_DateOnly
                {
                    AttrTimeOnly = new TimeOnly(12, 0, 0),
                    AttrTimeOnlyNullable = null,
                    AttrDate = DateTime.Now,
                    AttrString = "NullNullable",
                };
                var toUpdate = new DataObject[] { obj };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyViewWithNullable();
                var loaded = new Class_DateOnly { __PrimaryKey = obj.__PrimaryKey };
                ds.LoadObject(view, loaded);

                Assert.Null(loaded.AttrTimeOnlyNullable);
            }
        }

        [Fact]
        public void TimeOnlyNullableSaveLoadValue()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var timeVal = new TimeOnly(15, 45, 30);
                var obj = new Class_DateOnly
                {
                    AttrTimeOnly = new TimeOnly(12, 0, 0),
                    AttrTimeOnlyNullable = timeVal,
                    AttrDate = DateTime.Now,
                    AttrString = "WithNullable",
                };
                var toUpdate = new DataObject[] { obj };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyViewWithNullable();
                var loaded = new Class_DateOnly { __PrimaryKey = obj.__PrimaryKey };
                ds.LoadObject(view, loaded);

                Assert.Equal(timeVal, loaded.AttrTimeOnlyNullable);
            }
        }

        [Fact]
        public void TimeOnlyNullableLinqEquals()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var timeVal = new TimeOnly(15, 45, 30);
                var obj1 = new Class_DateOnly
                {
                    AttrTimeOnly = new TimeOnly(12, 0, 0),
                    AttrTimeOnlyNullable = timeVal,
                    AttrDate = DateTime.Now,
                    AttrString = "HasValue",
                };
                var obj2 = new Class_DateOnly
                {
                    AttrTimeOnly = new TimeOnly(12, 0, 0),
                    AttrTimeOnlyNullable = null,
                    AttrDate = DateTime.Now,
                    AttrString = "NullValue",
                };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyViewWithNullable();
                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrTimeOnlyNullable == timeVal)
                    .ToList();

                Assert.Single(result);
                Assert.Equal("HasValue", result[0].AttrString);
            }
        }

        [Fact]
        public void TimeOnlyEdgeValuesRoundTrip()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var minTime = new TimeOnly(0, 0, 0);
                var maxTime = new TimeOnly(23, 59, 59);

                var obj1 = new Class_DateOnly { AttrTimeOnly = minTime, AttrDate = DateTime.Now, AttrString = "Min" };
                var obj2 = new Class_DateOnly { AttrTimeOnly = maxTime, AttrDate = DateTime.Now, AttrString = "Max" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateTimeOnlyView();

                var loaded1 = new Class_DateOnly { __PrimaryKey = obj1.__PrimaryKey };
                ds.LoadObject(view, loaded1);
                Assert.Equal(minTime, loaded1.AttrTimeOnly);

                var loaded2 = new Class_DateOnly { __PrimaryKey = obj2.__PrimaryKey };
                ds.LoadObject(view, loaded2);
                Assert.Equal(maxTime, loaded2.AttrTimeOnly);
            }
        }

        private static View CreateTimeOnlyView()
        {
            var view = new View();
            view.DefineClassType = typeof(Class_DateOnly);
            view.Properties = new[]
            {
                new PropertyInView("AttrTimeOnly", "AttrTimeOnly", true, string.Empty),
                new PropertyInView("AttrString", "AttrString", true, string.Empty),
            };
            return view;
        }

        private static View CreateTimeOnlyViewWithNullable()
        {
            var view = new View();
            view.DefineClassType = typeof(Class_DateOnly);
            view.Properties = new[]
            {
                new PropertyInView("AttrTimeOnly", "AttrTimeOnly", true, string.Empty),
                new PropertyInView("AttrTimeOnlyNullable", "AttrTimeOnlyNullable", true, string.Empty),
                new PropertyInView("AttrString", "AttrString", true, string.Empty),
            };
            return view;
        }
#endif
    }
}
