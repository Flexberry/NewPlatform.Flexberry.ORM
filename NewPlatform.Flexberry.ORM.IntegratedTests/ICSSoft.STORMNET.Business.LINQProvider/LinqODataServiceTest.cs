namespace NewPlatform.Flexberry.ORM.IntegratedTests.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.UserDataTypes;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Проверка цепочного вызова Where при LINQ-запросах к сервису данных.
    /// </summary>
    public class LinqODataServiceTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public LinqODataServiceTest()
            : base("LinqODat")
        {
        }

        [Fact]
        public void TestODataMasterFieldOrderby()
        {
            foreach (IDataService dataService in DataServices)
            {
                // ICSSoft.STORMNET.Windows.Forms.ExternalLangDef.LanguageDef.DataService = dataService;
                if (dataService is MSSQLDataService)
                {
                    continue;
                }

                Лес лес1 = new Лес { Название = "Бор" };
                Лес лес2 = new Лес { Название = "Березовая роща" };
                Медведь медв = new Медведь { Вес = 48, Пол = Пол.Мужской, ЛесОбитания = лес1 };
                Медведь медв2 = new Медведь { Вес = 148, Пол = Пол.Мужской, ЛесОбитания = лес1 };
                Медведь медв3 = new Медведь { Вес = 148, Пол = Пол.Мужской, ЛесОбитания = лес2 };
                медв.ЛесОбитания = лес1;
                var берлога1 = new Берлога { Наименование = "Для хорошего настроения", ЛесРасположения = лес1 };
                var берлога2 = new Берлога { Наименование = "Для плохого настроения", ЛесРасположения = лес2 };
                var берлога3 = new Берлога { Наименование = "Для хорошего настроения", ЛесРасположения = лес1 };
                медв.Берлога.Add(берлога1);
                медв.Берлога.Add(берлога2);
                медв2.Берлога.Add(берлога3);
                var objs = new ICSSoft.STORMNET.DataObject[] { медв, медв2, медв3, берлога2, берлога1, берлога3, лес1, лес2 };
                var ds = (SQLDataService)dataService;
                ds.UpdateObjects(ref objs);
                var expr0 = ds.Query<Медведь>(Медведь.Views.МедведьE).Where(
                    x => x.ЛесОбитания.Название == "Бор");

                var expr1 = ds.Query<Медведь>(Медведь.Views.МедведьE).OrderBy(x => x.ЛесОбитания.Название);
                var lcs1 = LinqToLcs.GetLcs(expr1.Expression, Медведь.Views.МедведьE);
                lcs1.View = Медведь.Views.МедведьE;
                lcs1.LoadingTypes = new Type[] { typeof(Медведь) };
                var l1 = dataService.LoadObjects(lcs1);
                var expr2 = ds.Query<Медведь>(Медведь.Views.МедведьE).OrderByDescending(x => x.ЛесОбитания.Название);
                var lcs2 = LinqToLcs.GetLcs(expr2.Expression, Медведь.Views.МедведьE);
                lcs2.View = Медведь.Views.МедведьE;
                lcs2.LoadingTypes = new Type[] { typeof(Медведь) };
                var l2 = dataService.LoadObjects(lcs2);

                var lcs3 = new LoadingCustomizationStruct(null);
                lcs3.View = Медведь.Views.МедведьE;
                lcs3.LoadingTypes = new Type[] { typeof(Медведь) };
                lcs3.ColumnsSort = new[] { new ColumnsSortDef(Information.ExtractPropertyPath<Медведь>(c => c.ЛесОбитания.Название), ICSSoft.STORMNET.Business.SortOrder.Asc) };
                var l3 = dataService.LoadObjects(lcs3);

                var lcs4 = new LoadingCustomizationStruct(null);
                lcs4.View = Медведь.Views.МедведьE;
                lcs4.LoadingTypes = new Type[] { typeof(Медведь) };
                lcs4.ColumnsSort = new[] { new ColumnsSortDef(Information.ExtractPropertyPath<Медведь>(c => c.ЛесОбитания.Название), ICSSoft.STORMNET.Business.SortOrder.Desc) };
                var l4 = dataService.LoadObjects(lcs4);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Fact]
        public void TestODataAny()
        {
            foreach (IDataService dataService in DataServices)
            {
                ICSSoft.STORMNET.Windows.Forms.ExternalLangDef.LanguageDef.DataService = dataService;
                // if (dataService is MSSQLDataService)
                //    continue;
                Медведь медв = new Медведь { Вес = 48, Пол = Пол.Мужской };
                Медведь медв2 = new Медведь { Вес = 148, Пол = Пол.Мужской };
                Лес лес1 = new Лес { Название = "Бор" };
                Лес лес2 = new Лес { Название = "Березовая роща" };
                медв.ЛесОбитания = лес1;
                var берлога1 = new Берлога { Наименование = "Для хорошего настроения", ЛесРасположения = лес1 };
                var берлога2 = new Берлога { Наименование = "Для плохого настроения", ЛесРасположения = лес2 };
                var берлога3 = new Берлога { Наименование = "Для хорошего настроения", ЛесРасположения = лес1 };
                медв.Берлога.Add(берлога1);
                медв.Берлога.Add(берлога2);
                медв2.Берлога.Add(берлога3);
                var objs = new ICSSoft.STORMNET.DataObject[] { медв, медв2, берлога2, берлога1, берлога3, лес1, лес2 };
                var ds = (SQLDataService)dataService;
                ds.UpdateObjects(ref objs);

                var v = new View(new ViewAttribute("AllProps", new string[] { "*" }), typeof(Кошка));
                var ob = ds.LoadObjects(v);

                var l = ds.Query<Медведь>(Медведь.Views.МедведьE).Where(
                  x => x.Берлога.Cast<Берлога>().Any(o => o.Наименование == "Для хорошего настроения")).ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Fact]
        public void TestODataIsOf()
        {
            foreach (IDataService dataService in DataServices)
            {
                ICSSoft.STORMNET.Windows.Forms.ExternalLangDef.LanguageDef.DataService = dataService;
                var ds = (SQLDataService)dataService;
                Plant2 cls1 = new Plant2() { Name = "n1" };
                Cabbage2 cls2 = new Cabbage2() { Name = "n2", Type = "type" };

                var updateObjectsArray = new ICSSoft.STORMNET.DataObject[] { cls1, cls2 };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = Plant2.Views.Plant2E;

                IQueryable<Plant2> limit;
                // limit = ds.Query<Plant2>(view).Where(it => it.__PrimaryKey != null);

                LoadingCustomizationStruct lcs;
                /*
                lcs = LinqToLcs.GetLcs(limit.Expression, view);
                lcs.LoadingTypes = new Type[] { typeof(Plant2), typeof(Cabbage2) };
                lcs.LoadingTypes = new Type[] { typeof(Cabbage2) };
                lcs.View = view;
                var list = ds.LoadObjects(lcs);
                */
                // limit = ds.Query<Plant2>(view).Where(it => typeof(Cabbage2).IsInstanceOfType(it));
                // lcs = LinqToLcs.GetLcs(limit.Expression, view);
                // var list2 = ds.LoadObjects(lcs);
            }
        }

        /// <summary>
        /// Проверка использования поля Value при поиске NullableDateTime, NullableDecimal, NullableInt .
        /// </summary>
        [Fact]
        public void TestQueryNullableValue()
        {
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;

                NullableDateTime date = new NullableDateTime();
                date.Value = DateTime.Now;
                NullableDecimal dec = new NullableDecimal();
                dec.Value = new decimal(77.111);
                NullableDecimal decGreater = new NullableDecimal();
                decGreater.Value = dec.Value + new decimal(0.001);
                NullableInt i = new NullableInt();
                i.Value = 77;
                FullTypesMaster1 fullTypes = new FullTypesMaster1() { PoleNullableDateTime = date, PoleNullableDecimal = dec, PoleNullableInt = i };

                // Сохранение данных.
                var updateObjectsArray = new ICSSoft.STORMNET.DataObject[] { fullTypes };
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMaster1.Views.FullMasterView;
                List<FullTypesMaster1> list;

                // Проверка поиска NullableDateTime.
                list = ds.Query<FullTypesMaster1>(view).Where(d => d.PoleNullableDateTime.Value <= date.Value).ToList();
                Assert.Equal(1, list.Count);

                // Проверка поиска NullableDecimal.
                list = ds.Query<FullTypesMaster1>(view).Where(d => d.PoleNullableDecimal.Value < decGreater.Value).ToList();
                Assert.Equal(1, list.Count);

                // Проверка поиска NullableInt.
                list = ds.Query<FullTypesMaster1>(view).Where(d => d.PoleNullableInt.Value == i.Value).ToList();
                Assert.Equal(1, list.Count);
            }
        }

        /// <summary>
        /// Проверка результата после применения Take и Skip.
        /// </summary>
        [Fact]
        public void TestSkipTake()
        {
            foreach (IDataService dataService in DataServices)
            {
                // TODO: Fix OracleDataService error.
                if (dataService is OracleDataService)
                {
                    continue;
                }

                // Arrange.
                var ds = (SQLDataService)dataService;

                Порода породаДикая = new Порода() { Название = "Дикая" };
                Порода породаПерсидская = new Порода() { Название = "Персидская" };
                Порода породаБезПороды = new Порода() { Название = "БезПороды" };
                Порода породаСибирская = new Порода() { Название = "Сибирская" };
                Порода породаСиамская = new Порода() { Название = "Сиамская" };
                Порода породаРусская = new Порода() { Название = "Русская" };

                var obj = new ICSSoft.STORMNET.DataObject[] { породаДикая, породаПерсидская, породаБезПороды, породаСибирская, породаСиамская, породаРусская };

                ds.UpdateObjects(ref obj);

                var expected = ds.Query<Порода>(Порода.Views.ПородаE).Skip(0).ToArray();

                var actual1 = ds.Query<Порода>(Порода.Views.ПородаE).Skip(2).Take(3).Skip(2).ToArray();
                Assert.Equal(expected[4].__PrimaryKey, actual1[0].__PrimaryKey);

                var actual2 = ds.Query<Порода>(Порода.Views.ПородаE).Skip(2).Take(3).ToArray();
                Assert.Equal(expected[2].__PrimaryKey, actual2[0].__PrimaryKey);
                Assert.Equal(expected[3].__PrimaryKey, actual2[1].__PrimaryKey);
                Assert.Equal(expected[4].__PrimaryKey, actual2[2].__PrimaryKey);

                var actual3 = ds.Query<Порода>(Порода.Views.ПородаE).Take(3).Skip(1).ToArray();
                Assert.Equal(expected[1].__PrimaryKey, actual3[0].__PrimaryKey);
                Assert.Equal(expected[2].__PrimaryKey, actual3[1].__PrimaryKey);
            }
        }

        /// <summary>
        /// Проверка корректной работы запросов с FirstOrDefault.
        /// </summary>
        [Fact(Skip = "Необходимо поправить обработку запросов с FirstOrDefault, задача 144169")]
        public void TestEnum()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Кошка cat1 = new Кошка { Тип = ТипКошки.Домашняя };
                Кошка cat2 = new Кошка { Тип = ТипКошки.Дикая };
                Кошка cat3 = new Кошка { Тип = ТипКошки.НетЗначения };
                var objs = new DataObject[] { cat1, cat2, cat3 };
                var ds = (SQLDataService)dataService;
                ds.UpdateObjects(ref objs);

                var expected = ds.Query<Кошка>(Кошка.Views.КошкаL).FirstOrDefault(w => w.Тип == ТипКошки.Домашняя);

                // Assert.
                Assert.NotNull(expected);
            }
        }
    }
}
