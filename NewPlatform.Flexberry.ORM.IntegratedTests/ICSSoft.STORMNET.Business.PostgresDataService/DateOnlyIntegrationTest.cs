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
    /// E2E-тесты для типа DateOnly: LINQ-запросы, LCS с DateOnly-функциями, диапазонные фильтры.
    /// </summary>
    public class DateOnlyIntegrationTest : BaseIntegratedTest
    {
        private static readonly DateTime AnyDate = new DateTime(2026, 1, 1);

        /// <summary>
        /// Конструктор. База данных будет создана с префиксом "DtOnlyInt".
        /// </summary>
        public DateOnlyIntegrationTest()
            : base("DtOnlyInt")
        {
        }

        protected override string MssqlScript => null;

        protected override string PostgresScript => Resources.PostgresDataServiceTestScript;

        protected override string OracleScript => null;

#if NET6_0_OR_GREATER
        /// <summary>
        /// LINQ-запрос по равенству DateOnly.
        /// </summary>
        [Fact]
        public void DateOnlyLinqEquals()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var date1 = new DateOnly(2026, 6, 15);
                var date2 = new DateOnly(2026, 12, 25);

                var obj1 = new Class_DateOnly { AttrDateOnly = date1, AttrDate = AnyDate, AttrString = "June15" };
                var obj2 = new Class_DateOnly { AttrDateOnly = date2, AttrDate = AnyDate, AttrString = "Dec25" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var result = ds.Query<Class_DateOnly>(view).Where(x => x.AttrDateOnly == date1).ToList();

                Assert.Single(result);
                Assert.Equal(date1, result[0].AttrDateOnly);
                Assert.Equal("June15", result[0].AttrString);
            }
        }

        /// <summary>
        /// LINQ-запрос с Year и DayOfWeek для DateOnly.
        /// </summary>
        [Fact]
        public void DateOnlyLinqYearAndDayOfWeek()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var date1 = new DateOnly(2026, 6, 15);
                var date2 = new DateOnly(2026, 12, 25);
                var date3 = new DateOnly(2025, 1, 1);

                var dayOfWeek1 = (int)date1.DayOfWeek;

                var obj1 = new Class_DateOnly { AttrDateOnly = date1, AttrDate = AnyDate, AttrString = "June15" };
                var obj2 = new Class_DateOnly { AttrDateOnly = date2, AttrDate = AnyDate, AttrString = "Dec25" };
                var obj3 = new Class_DateOnly { AttrDateOnly = date3, AttrDate = AnyDate, AttrString = "Jan1" };
                var toUpdate = new DataObject[] { obj1, obj2, obj3 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrDateOnly.Year == 2026 && (int)x.AttrDateOnly.DayOfWeek == dayOfWeek1)
                    .ToList();

                Assert.Single(result);
                Assert.Equal(date1, result[0].AttrDateOnly);
            }
        }

        /// <summary>
        /// LINQ-запрос с DateOnly.DayNumber.
        /// </summary>
        [Fact]
        public void DateOnlyLinqDayNumber()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var date1 = new DateOnly(2026, 6, 15);
                var dayNumber1 = date1.DayNumber;

                var obj1 = new Class_DateOnly { AttrDateOnly = date1, AttrDate = AnyDate, AttrString = "June15" };
                var obj2 = new Class_DateOnly { AttrDateOnly = new DateOnly(2026, 12, 25), AttrDate = AnyDate, AttrString = "Dec25" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrDateOnly.DayNumber == dayNumber1)
                    .ToList();

                Assert.Single(result);
                Assert.Equal("June15", result[0].AttrString);
            }
        }

        /// <summary>
        /// LINQ-запрос с DateOnly.DayOfYear.
        /// </summary>
        [Fact]
        public void DateOnlyLinqDayOfYear()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var date1 = new DateOnly(2026, 6, 15);
                var dayOfYear1 = date1.DayOfYear;

                var obj1 = new Class_DateOnly { AttrDateOnly = date1, AttrDate = AnyDate, AttrString = "June15" };
                var obj2 = new Class_DateOnly { AttrDateOnly = new DateOnly(2026, 12, 25), AttrDate = AnyDate, AttrString = "Dec25" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrDateOnly.DayOfYear == dayOfYear1)
                    .ToList();

                Assert.Single(result);
                Assert.Equal("June15", result[0].AttrString);
            }
        }

        /// <summary>
        /// LCS-запрос с диапазонными операциями (больше, меньше) для DateOnly.
        /// </summary>
        [Fact]
        public void DateOnlyLcsRangeQuery()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var langDef = ExternalLangDef.LanguageDef;

                var date1 = new DateOnly(2026, 6, 15);
                var date2 = new DateOnly(2026, 12, 25);
                var date3 = new DateOnly(2025, 1, 1);

                var obj1 = new Class_DateOnly { AttrDateOnly = date1, AttrDate = AnyDate, AttrString = "June15" };
                var obj2 = new Class_DateOnly { AttrDateOnly = date2, AttrDate = AnyDate, AttrString = "Dec25" };
                var obj3 = new Class_DateOnly { AttrDateOnly = date3, AttrDate = AnyDate, AttrString = "Jan1" };
                var toUpdate = new DataObject[] { obj1, obj2, obj3 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var varDef = new VariableDef(langDef.DateTimeType, "AttrDateOnly");

                var lcsGt = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcsGt.LimitFunction = langDef.GetFunction(langDef.funcG, varDef, date1);
                var gtResult = ds.LoadObjects(lcsGt).Cast<Class_DateOnly>().ToList();
                Assert.Single(gtResult);
                Assert.Equal("Dec25", gtResult[0].AttrString);

                var lcsLt = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcsLt.LimitFunction = langDef.GetFunction(langDef.funcL, varDef, date1);
                var ltResult = ds.LoadObjects(lcsLt).Cast<Class_DateOnly>().ToList();
                Assert.Single(ltResult);
                Assert.Equal("Jan1", ltResult[0].AttrString);
            }
        }

        /// <summary>
        /// LCS-запрос с функцией DayNumber для DateOnly.
        /// </summary>
        [Fact]
        public void DateOnlyLcsDayNumber()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var langDef = ExternalLangDef.LanguageDef;

                var date1 = new DateOnly(2026, 6, 15);
                var dayNumber1 = date1.DayNumber;

                var obj1 = new Class_DateOnly { AttrDateOnly = date1, AttrDate = AnyDate, AttrString = "June15" };
                var obj2 = new Class_DateOnly { AttrDateOnly = new DateOnly(2026, 12, 25), AttrDate = AnyDate, AttrString = "Dec25" };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var varDef = new VariableDef(langDef.DateTimeType, "AttrDateOnly");
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcs.LimitFunction = langDef.GetFunction(
                    langDef.funcEQ,
                    langDef.GetFunction(langDef.funcDayNumber, varDef),
                    dayNumber1);
                var result = ds.LoadObjects(lcs).Cast<Class_DateOnly>().ToList();

                Assert.Single(result);
                Assert.Equal("June15", result[0].AttrString);
            }
        }

        /// <summary>
        /// E2E: загрузка объекта с nullable DateOnly=null, сохранение и повторная загрузка.
        /// </summary>
        [Fact]
        public void DateOnlyNullableSaveLoadNull()
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
                    AttrDateOnly = new DateOnly(2026, 1, 1),
                    AttrDateOnlyNullable = null,
                    AttrDate = AnyDate,
                    AttrString = "NullNullable",
                };
                var toUpdate = new DataObject[] { obj };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyViewWithNullable();

                var loaded = new Class_DateOnly { __PrimaryKey = obj.__PrimaryKey };
                ds.LoadObject(view, loaded);

                Assert.Equal(new DateOnly(2026, 1, 1), loaded.AttrDateOnly);
                Assert.Null(loaded.AttrDateOnlyNullable);
            }
        }

        /// <summary>
        /// E2E: сохранение и загрузка nullable DateOnly со значением.
        /// </summary>
        [Fact]
        public void DateOnlyNullableSaveLoadValue()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var dateVal = new DateOnly(2026, 7, 20);
                var obj = new Class_DateOnly
                {
                    AttrDateOnly = new DateOnly(2026, 1, 1),
                    AttrDateOnlyNullable = dateVal,
                    AttrDate = AnyDate,
                    AttrString = "WithNullable",
                };
                var toUpdate = new DataObject[] { obj };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyViewWithNullable();

                var loaded = new Class_DateOnly { __PrimaryKey = obj.__PrimaryKey };
                ds.LoadObject(view, loaded);

                Assert.Equal(dateVal, loaded.AttrDateOnlyNullable);
            }
        }

        /// <summary>
        /// E2E: LINQ-запрос по nullable DateOnly.
        /// </summary>
        [Fact]
        public void DateOnlyNullableLinqEquals()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var dateVal = new DateOnly(2026, 7, 20);
                var obj1 = new Class_DateOnly
                {
                    AttrDateOnly = new DateOnly(2026, 1, 1),
                    AttrDateOnlyNullable = dateVal,
                    AttrDate = AnyDate,
                    AttrString = "HasValue",
                };
                var obj2 = new Class_DateOnly
                {
                    AttrDateOnly = new DateOnly(2026, 2, 1),
                    AttrDateOnlyNullable = null,
                    AttrDate = AnyDate,
                    AttrString = "NullValue",
                };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyViewWithNullable();

                var result = ds.Query<Class_DateOnly>(view)
                    .Where(x => x.AttrDateOnlyNullable == dateVal)
                    .ToList();

                Assert.Single(result);
                Assert.Equal("HasValue", result[0].AttrString);
            }
        }

        /// <summary>
        /// E2E: LCS-фильтрация по nullable DateOnly (funcEQ).
        /// </summary>
        [Fact]
        public void DateOnlyNullableLcsEquals()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var langDef = ExternalLangDef.LanguageDef;

                var dateVal = new DateOnly(2026, 7, 20);
                var obj1 = new Class_DateOnly
                {
                    AttrDateOnly = new DateOnly(2026, 1, 1),
                    AttrDateOnlyNullable = dateVal,
                    AttrDate = AnyDate,
                    AttrString = "HasValue",
                };
                var obj2 = new Class_DateOnly
                {
                    AttrDateOnly = new DateOnly(2026, 2, 1),
                    AttrDateOnlyNullable = null,
                    AttrDate = AnyDate,
                    AttrString = "NullValue",
                };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyViewWithNullable();
                var varDef = new VariableDef(langDef.DateTimeType, "AttrDateOnlyNullable");

                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Class_DateOnly), view);
                lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, varDef, dateVal);
                var result = ds.LoadObjects(lcs).Cast<Class_DateOnly>().ToList();

                Assert.Single(result);
                Assert.Equal("HasValue", result[0].AttrString);
            }
        }

        /// <summary>
        /// E2E: DateOnly.MinValue и DateOnly.MaxValue — round-trip через БД.
        /// </summary>
        [Fact]
        public void DateOnlyMinMaxRoundTrip()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService == null)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                var obj1 = new Class_DateOnly
                {
                    AttrDateOnly = DateOnly.MinValue,
                    AttrDate = AnyDate,
                    AttrString = "Min",
                };
                var obj2 = new Class_DateOnly
                {
                    AttrDateOnly = DateOnly.MaxValue,
                    AttrDate = AnyDate,
                    AttrString = "Max",
                };
                var toUpdate = new DataObject[] { obj1, obj2 };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyView();

                var loaded1 = new Class_DateOnly { __PrimaryKey = obj1.__PrimaryKey };
                ds.LoadObject(view, loaded1);
                Assert.Equal(DateOnly.MinValue, loaded1.AttrDateOnly);

                var loaded2 = new Class_DateOnly { __PrimaryKey = obj2.__PrimaryKey };
                ds.LoadObject(view, loaded2);
                Assert.Equal(DateOnly.MaxValue, loaded2.AttrDateOnly);
            }
        }

        /// <summary>
        /// E2E: обновление nullable DateOnly с null на значение и обратно.
        /// </summary>
        [Fact]
        public void DateOnlyNullableUpdateNullToValueAndBack()
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
                    AttrDateOnly = new DateOnly(2026, 1, 1),
                    AttrDateOnlyNullable = null,
                    AttrDate = AnyDate,
                    AttrString = "UpdateTest",
                };
                var toUpdate = new DataObject[] { obj };
                ds.UpdateObjects(ref toUpdate);

                var view = CreateDateOnlyViewWithNullable();

                var dateVal = new DateOnly(2026, 8, 15);
                obj.AttrDateOnlyNullable = dateVal;
                ds.UpdateObject(obj);

                var loaded = new Class_DateOnly { __PrimaryKey = obj.__PrimaryKey };
                ds.LoadObject(view, loaded);
                Assert.Equal(dateVal, loaded.AttrDateOnlyNullable);

                loaded.AttrDateOnlyNullable = null;
                ds.UpdateObject(loaded);

                var loaded2 = new Class_DateOnly { __PrimaryKey = obj.__PrimaryKey };
                ds.LoadObject(view, loaded2);
                Assert.Null(loaded2.AttrDateOnlyNullable);
            }
        }

        private static View CreateDateOnlyView()
        {
            var view = new View();
            view.DefineClassType = typeof(Class_DateOnly);
            view.Properties = new[]
            {
                new PropertyInView("AttrDateOnly", "AttrDateOnly", true, string.Empty),
                new PropertyInView("AttrString", "AttrString", true, string.Empty),
            };

            return view;
        }

        private static View CreateDateOnlyViewWithNullable()
        {
            var view = new View();
            view.DefineClassType = typeof(Class_DateOnly);
            view.Properties = new[]
            {
                new PropertyInView("AttrDateOnly", "AttrDateOnly", true, string.Empty),
                new PropertyInView("AttrDateOnlyNullable", "AttrDateOnlyNullable", true, string.Empty),
                new PropertyInView("AttrString", "AttrString", true, string.Empty),
            };

            return view;
        }
#endif
    }
}
