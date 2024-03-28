namespace NewPlatform.Flexberry.ORM.IntegratedTests.ExternalLDef
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Класс для тестирования функций ограничения на даты и детейлы.
    /// </summary>
    public class RestrictionsOnTheDateAndDetailleTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public RestrictionsOnTheDateAndDetailleTests()
            : base("FuncTest")
        {
        }

        /// <summary>
        /// Проверка функции <see cref="ExternalLangDef.funcDayOfWeek"/> (вывести только те числа которые попали на понедельник).
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_funcDayOfWeek()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef languageDef = new ExternalLangDef(ds);

                // Контрольные значене.
                const int firstMonday = 1;
                const int secondMonday = 3;
                var controlValue = new List<int>() { firstMonday, secondMonday };

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                // Понедельник.
                var monday1 = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 11, 10, 37, 44),
                    PoleInt = firstMonday,
                    FullTypesMaster1 = testMasterObject,
                };

                // Пятница.
                var friday1 = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 22, 10, 37, 44),
                    PoleInt = 2,
                    FullTypesMaster1 = testMasterObject,
                };

                // Понедельник.
                var monday2 = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 12, 28, 10, 37, 44),
                    PoleInt = secondMonday,
                    FullTypesMaster1 = testMasterObject,
                };

                var updateObjectsArray = new DataObject[] { testMasterObject, monday1, friday1, monday2 };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                // Функция ограничения.
                // Применение функции ограничения.
                lcs.LimitFunction = languageDef.GetFunction(
                    languageDef.funcEQ,
                    languageDef.GetFunction(
                        languageDef.funcDayOfWeek,
                        new VariableDef(languageDef.DateTimeType,
                            Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleDateTime))),
                    1);
                // Act.
                // Получение, массива значений поля PoleInt.
                var poleIntValue = ds.LoadObjects(lcs).Cast<FullTypesMainAgregator>().Select(x => x.PoleInt).ToList();

                // Получение массивов Объединения и Пересечения обьектов(controlValue и PoleInt)
                var unionValue = poleIntValue.Union(controlValue).ToList();
                var intersectValue = poleIntValue.Intersect(controlValue).ToList();

                // Assert.
                Assert.Equal(controlValue.Count, unionValue.Count);
                Assert.Equal(controlValue.Count, intersectValue.Count);
            }
        }

        /// <summary>
        /// Проверка параметра <see cref="ExternalLangDef.paramMonthDIFF"/> (вывести только те числа которые были раньше контрольного как минимум на месяц).
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_paramMonthDIFF()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                DateTime controlDate = new DateTime(2016, 2, 2);
                const int firstDate = 1;
                const int secondDate = 3;
                var controlValue = new List<int>() { firstDate, secondDate };

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                // Дата, которая будет подходить под условия ограничения.
                var firstDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 12, 2),
                    PoleInt = firstDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var firstDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 2),
                    PoleInt = 2,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая будет подходить под условия ограничения.
                var secondDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 1),
                    PoleInt = secondDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var secondDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 2, 2),
                    PoleInt = 4,
                    FullTypesMaster1 = testMasterObject,
                };

                var updateObjectsArray = new DataObject[] { testMasterObject, firstDateTrue, firstDateFalse, secondDateTrue, secondDateFalse };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                lcs.LimitFunction = _ldef.GetFunction(
                    _ldef.funcL,
                    _ldef.GetFunction(
                        _ldef.funcDateAdd,
                        _ldef.GetFunction(_ldef.paramMonthDIFF),
                        1,
                        new VariableDef(_ldef.DateTimeType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleDateTime))),
                    _ldef.GetFunction(_ldef.funcOnlyDate, controlDate));

                // Act.
                // Получение, массива значений поля PoleInt.
                var poleIntValue = ds.LoadObjects(lcs).Cast<FullTypesMainAgregator>().Select(x => x.PoleInt).ToList();

                // Получение массивов Объединения и Пересечения обьектов(controlValue и PoleInt)
                var unionValue = poleIntValue.Union(controlValue).ToList();
                var intersectValue = poleIntValue.Intersect(controlValue).ToList();

                // Assert.
                Assert.Equal(controlValue.Count, unionValue.Count);
                Assert.Equal(controlValue.Count, intersectValue.Count);
            }
        }

        /// <summary>
        /// Проверка параметра <see cref="ExternalLangDef.paramWeekDIFF"/> (вывести только те числа которые были раньше контрольного как минимум на неделю).
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_paramWeekDIFF()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                DateTime controlDate = new DateTime(2016, 2, 2);
                const int firstDate = 1;
                const int secondDate = 3;
                var controlValue = new List<int>() { firstDate, secondDate };

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                // Дата, которая будет подходить под условия ограничения.
                var firstDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 25),
                    PoleInt = firstDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var firstDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 26),
                    PoleInt = 2,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая будет подходить под условия ограничения.
                var secondDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 12, 1),
                    PoleInt = secondDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var secondDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 2, 1),
                    PoleInt = 4,
                    FullTypesMaster1 = testMasterObject,
                };

                var updateObjectsArray = new DataObject[] { testMasterObject, firstDateTrue, firstDateFalse, secondDateTrue, secondDateFalse };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                lcs.LimitFunction = _ldef.GetFunction(
                    _ldef.funcL,
                    _ldef.GetFunction(
                        _ldef.funcDateAdd,
                        _ldef.GetFunction(_ldef.paramWeekDIFF),
                        1,
                        new VariableDef(_ldef.DateTimeType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleDateTime))),
                    _ldef.GetFunction(_ldef.funcOnlyDate, controlDate));

                // Act.
                // Получение, массива значений поля PoleInt.
                var poleIntValue = ds.LoadObjects(lcs).Cast<FullTypesMainAgregator>().Select(x => x.PoleInt).ToList();

                // Получение массивов Объединения и Пересечения обьектов(controlValue и PoleInt)
                var unionValue = poleIntValue.Union(controlValue).ToList();
                var intersectValue = poleIntValue.Intersect(controlValue).ToList();

                // Assert.
                Assert.Equal(controlValue.Count, unionValue.Count);
                Assert.Equal(controlValue.Count, intersectValue.Count);
            }
        }

        /// <summary>
        /// Проверка параметра <see cref="ExternalLangDef.paramQuarterDIFF"/> (вывести только те числа которые были раньше контрольного как минимум на четверть).
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_paramQuarterDIFF()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                DateTime controlDate = new DateTime(2016, 2, 2);
                const int firstDate = 1;
                const int secondDate = 3;
                var controlValue = new List<int>() { firstDate, secondDate };

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                // Дата, которая будет подходить под условия ограничения.
                var firstDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 11, 1),
                    PoleInt = firstDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var firstDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 12, 2),
                    PoleInt = 2,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая будет подходить под условия ограничения.
                var secondDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2014, 11, 2),
                    PoleInt = secondDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var secondDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 11, 2),
                    PoleInt = 4,
                    FullTypesMaster1 = testMasterObject,
                };

                var updateObjectsArray = new DataObject[] { testMasterObject, firstDateTrue, firstDateFalse, secondDateTrue, secondDateFalse };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                lcs.LimitFunction = _ldef.GetFunction(
                    _ldef.funcL,
                    _ldef.GetFunction(
                        _ldef.funcDateAdd,
                        _ldef.GetFunction(_ldef.paramQuarterDIFF),
                        1,
                        new VariableDef(_ldef.DateTimeType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleDateTime))),
                    _ldef.GetFunction(_ldef.funcOnlyDate, controlDate));

                // Act.
                // Получение, массива значений поля PoleInt.
                var poleIntValue = ds.LoadObjects(lcs).Cast<FullTypesMainAgregator>().Select(x => x.PoleInt).ToList();

                // Получение массивов Объединения и Пересечения обьектов(controlValue и PoleInt)
                var unionValue = poleIntValue.Union(controlValue).ToList();
                var intersectValue = poleIntValue.Intersect(controlValue).ToList();

                // Assert.
                Assert.Equal(controlValue.Count, unionValue.Count);
                Assert.Equal(controlValue.Count, intersectValue.Count);
            }
        }

        /// <summary>
        /// Проверка параметра <see cref="ExternalLangDef.paramDayDIFF"/> (вывести только те числа которые были раньше контрольного как минимум на 3 дня).
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_paramDayDIFF()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                DateTime controlDate = new DateTime(2016, 2, 2);
                const int firstDate = 1;
                const int secondDate = 3;
                var controlValue = new List<int>() { firstDate, secondDate };

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                // Дата, которая будет подходить под условия ограничения.
                var firstDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 11, 1),
                    PoleInt = firstDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var firstDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 30),
                    PoleInt = 2,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая будет подходить под условия ограничения.
                var secondDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 1, 29),
                    PoleInt = secondDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var secondDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 2, 2),
                    PoleInt = 4,
                    FullTypesMaster1 = testMasterObject,
                };

                var updateObjectsArray = new DataObject[] { testMasterObject, firstDateTrue, firstDateFalse, secondDateTrue, secondDateFalse };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                lcs.LimitFunction = _ldef.GetFunction(
                    _ldef.funcL,
                    _ldef.GetFunction(
                        _ldef.funcDateAdd,
                        _ldef.GetFunction(_ldef.paramDayDIFF),
                        3,
                        new VariableDef(_ldef.DateTimeType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleDateTime))),
                    _ldef.GetFunction(_ldef.funcOnlyDate, controlDate));

                // Act.
                // Получение, массива значений поля PoleInt.
                var poleIntValue = ds.LoadObjects(lcs).Cast<FullTypesMainAgregator>().Select(x => x.PoleInt).ToList();

                // Получение массивов Объединения и Пересечения обьектов(controlValue и PoleInt)
                var unionValue = poleIntValue.Union(controlValue).ToList();
                var intersectValue = poleIntValue.Intersect(controlValue).ToList();

                // Assert.
                Assert.Equal(controlValue.Count, unionValue.Count);
                Assert.Equal(controlValue.Count, intersectValue.Count);
            }
        }

        /// <summary>
        /// Проверка параметра <see cref="ExternalLangDef.paramYearDIFF"/> (вывести только те числа которые были раньше контрольного как минимум на 1 год).
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_paramYearDIFF()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                DateTime controlDate = new DateTime(2016, 2, 2);
                const int firstDate = 1;
                const int secondDate = 3;
                var controlValue = new List<int>() { firstDate, secondDate };

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                // Дата, которая будет подходить под условия ограничения.
                var firstDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 2, 1),
                    PoleInt = firstDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var firstDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2016, 2, 2),
                    PoleInt = 2,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая будет подходить под условия ограничения.
                var secondDateTrue = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2014, 2, 2),
                    PoleInt = secondDate,
                    FullTypesMaster1 = testMasterObject,
                };

                // Дата, которая не будет подходить под условия ограничения.
                var secondDateFalse = new FullTypesMainAgregator
                {
                    PoleDateTime = new DateTime(2015, 2, 2),
                    PoleInt = 4,
                    FullTypesMaster1 = testMasterObject,
                };

                var updateObjectsArray = new DataObject[] { testMasterObject, firstDateTrue, firstDateFalse, secondDateTrue, secondDateFalse };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                lcs.LimitFunction = _ldef.GetFunction(
                    _ldef.funcL,
                    _ldef.GetFunction(
                        _ldef.funcDateAdd,
                        _ldef.GetFunction(_ldef.paramYearDIFF),
                        1,
                        new VariableDef(_ldef.DateTimeType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleDateTime))),
                    _ldef.GetFunction(_ldef.funcOnlyDate, controlDate));

                // Act.
                // Получение, массива значений поля PoleInt.
                var poleIntValue = ds.LoadObjects(lcs).Cast<FullTypesMainAgregator>().Select(x => x.PoleInt).ToList();

                // Получение массивов Объединения и Пересечения обьектов(controlValue и PoleInt)
                var unionValue = poleIntValue.Union(controlValue).ToList();
                var intersectValue = poleIntValue.Intersect(controlValue).ToList();

                // Assert.
                Assert.Equal(controlValue.Count, unionValue.Count);
                Assert.Equal(controlValue.Count, intersectValue.Count);
            }
        }

        /// <summary>
        /// Проверить с помощью функции <see cref="ExternalLangDef.funcExistAllExact"/>, существование детейлов, которые подходят по определеному условию.
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_funcExistAllExact()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            //  IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                const int controlInt = 1;
                const bool controlBool = true;

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                var testFullTypesMainAgregator = new FullTypesMainAgregator
                {
                    FullTypesMaster1 = testMasterObject,
                };

                // Создание детейла, который подходит под условие ограничения.
                testFullTypesMainAgregator.FullTypesDetail1.Add(new FullTypesDetail1 { PoleInt = controlInt, PoleBool = controlBool });

                var updateObjectsArray = new DataObject[] { testMasterObject, testFullTypesMainAgregator };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullViewWithDetail1;
                var view2 = FullTypesDetail1.Views.FullDetailView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                string cmp = Information.ExtractPropertyPath<FullTypesDetail1>(x => x.FullTypesMainAgregator);
                string name = Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.FullTypesDetail1);

                // Описание детейла.
                var detail = new DetailVariableDef(_ldef.GetObjectType("Details"), name, view2, cmp, new[] { SQLWhereLanguageDef.StormMainObjectKey });
                // Проверка существования детейла такого, что:
                lcs.LimitFunction =
                    _ldef.GetFunction(_ldef.funcExistAllExact,
                    detail,
                    // Поле int в детейле равно контрольному значению (controlInt=1).
                    _ldef.GetFunction(_ldef.funcEQ,
                        new VariableDef(_ldef.NumericType,
                            Information.ExtractPropertyPath<FullTypesDetail1>(x => x.PoleInt)),
                        controlInt),
                    // И поле bool в детейле равно контрольному значению (controlBool=true).
                    _ldef.GetFunction(_ldef.funcEQ,
                        new VariableDef(_ldef.BoolType,
                            Information.ExtractPropertyPath<FullTypesDetail1>(x => x.PoleBool)),
                        controlBool));

                // Act.
                var dos = ds.LoadObjects(lcs);

                // Assert.
                Assert.Equal(dos.Length, 1);

                // Создание детейла, который не подходит под условие ограничения.
                testFullTypesMainAgregator.FullTypesDetail1.Add(new FullTypesDetail1 { PoleInt = 1, PoleBool = false });

                // Сохранение новых данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Act.
                var dos2 = ds.LoadObjects(lcs);

                // Assert.
                Assert.Equal(dos2.Length, 0);
            }
        }

        /// <summary>
        /// Проверить с помощью функции <see cref="ExternalLangDef.funcExistAll"/>, существование детейлов, которые подходят по определеному условию.
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_funcExistAll()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                const int controlInt = 1;
                const bool controlBool = true;

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                var testFullTypesMainAgregator = new FullTypesMainAgregator
                {
                    FullTypesMaster1 = testMasterObject,
                };

                // Создание детейла, который не подходит под условие ограничения.
                testFullTypesMainAgregator.FullTypesDetail1.Add(new FullTypesDetail1 { PoleInt = 2, PoleBool = false });

                var updateObjectsArray = new DataObject[] { testMasterObject, testFullTypesMainAgregator };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullViewWithDetail1;
                var view2 = FullTypesDetail1.Views.FullDetailView;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                string cmp = Information.ExtractPropertyPath<FullTypesDetail1>(x => x.FullTypesMainAgregator);
                string name = Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.FullTypesDetail1);

                var detail = new DetailVariableDef(_ldef.GetObjectType("Details"), name, view2, cmp, new[] { SQLWhereLanguageDef.StormMainObjectKey });
                // Проверка существования детейла такого, что:
                lcs.LimitFunction = _ldef.GetFunction(_ldef.funcExistAll,
                    detail,
                    // Поле int в детейле равно контрольному значению(controlInt = 1).
                    _ldef.GetFunction(_ldef.funcEQ,
                        new VariableDef(_ldef.NumericType,
                            Information.ExtractPropertyPath<FullTypesDetail1>(x => x.PoleInt)),
                        controlInt),
                    // Или поле bool в детейле равно контрольному значению (controlBool=true).
                    _ldef.GetFunction(_ldef.funcEQ,
                        new VariableDef(_ldef.BoolType,
                            Information.ExtractPropertyPath<FullTypesDetail1>(x => x.PoleBool)),
                        controlBool));

                // Act.
                var dos = ds.LoadObjects(lcs);

                // Assert.
                Assert.Equal(dos.Length, 0);

                // Создание детейла, который подходит под условие ограничения.
                testFullTypesMainAgregator.FullTypesDetail1.Add(new FullTypesDetail1 { PoleInt = controlInt, PoleBool = controlBool });

                // Сохранение новых данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Act.
                var dos2 = ds.LoadObjects(lcs);

                // Assert.
                Assert.Equal(dos2.Length, 1);
            }
        }

        /// <summary>
        /// Проверить с помощью функции <see cref="ExternalLangDef.funcExistDetails"/>, существование детейлов, которые подходят по определеному условию.
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_funcExistDetails()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            // IDataService dataService = DataServices[0];

            // TODO Вернуть данную строчку, как исправят баг 94309.
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ExternalLangDef _ldef = new ExternalLangDef(ds);

                // Контрольные значене.
                const int controlInt = 1;

                // Сначала создаём структуру данных, требуемую для теста.
                var testMasterObject = new FullTypesMaster1();

                var testFullTypesMainAgregator = new FullTypesMainAgregator
                {
                    FullTypesMaster1 = testMasterObject,
                };

                // Создание детейлов, которые не подходят под условие ограничения.
                testFullTypesMainAgregator.FullTypesDetail1.Add(new FullTypesDetail1 { PoleInt = controlInt });
                testFullTypesMainAgregator.FullTypesDetail2.Add(new FullTypesDetail2 { PoleInt = 2 });

                var updateObjectsArray = new DataObject[] { testMasterObject, testFullTypesMainAgregator };

                // Сохранение данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMainAgregator.Views.FullViewWithDetail1;
                var view2 = FullTypesDetail1.Views.FullDetailView;
                var view3 = FullTypesDetail2.Views.FullTypesDetail2E;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), view);

                string cmp = Information.ExtractPropertyPath<FullTypesDetail1>(x => x.FullTypesMainAgregator);
                string name1 = Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.FullTypesDetail1);
                string name2 = Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.FullTypesDetail2);

                var detail = new DetailVariableDef(_ldef.GetObjectType("Details"), name1, view2, cmp, new[] { SQLWhereLanguageDef.StormMainObjectKey });
                var detail2 = new DetailVariableDef(_ldef.GetObjectType("Details"), name2, view3, cmp, new[] { SQLWhereLanguageDef.StormMainObjectKey });
                // Проверка существования детейлов такого, что:
                lcs.LimitFunction = _ldef.GetFunction(_ldef.funcExistDetails,
                    detail,
                    detail2,
                    _ldef.GetFunction(
                        // Равны.
                        _ldef.funcEQ,
                        // Хотя бы одно значение в поле Int у первого детейла.
                        new VariableDef(_ldef.NumericType, Information.ExtractPropertyPath<FullTypesDetail1>(x => x.PoleInt)),
                        // И значение в поле Int у второго детейла.
                        new VariableDef(_ldef.NumericType, Information.ExtractPropertyPath<FullTypesDetail2>(x => x.PoleInt))));

                // Act.
                var dos = ds.LoadObjects(lcs);

                // Assert.
                Assert.Equal(dos.Length, 0);

                // Создание детейла, который подходит под условте ограничения.
                testFullTypesMainAgregator.FullTypesDetail2.Add(new FullTypesDetail2 { PoleInt = controlInt });

                // Сохранение новых данных.
                ds.UpdateObjects(ref updateObjectsArray);

                // Act.
                var dos2 = ds.LoadObjects(lcs);

                // Assert.
                Assert.Equal(dos2.Length, 1);
            }
        }

        /// <summary>
        /// Экземпляр ExternalLangDef для тестов.
        /// </summary>
        private readonly ExternalLangDef langDef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Метод для создания тестовых данных.
        /// </summary>
        /// <param name="ds">Используемый сервис данных.</param>
        private void updateTestObjects(IDataService ds)
        {
            var aggregator1 = new ХозДоговор()
            {
                НомерХозДоговора = 1,
            };

            var aggregator2 = new ХозДоговор()
            {
                НомерХозДоговора = 3,
            };

            var detail1 = new DetailArrayOfУчастникХозДоговора(aggregator1)
            {
                new УчастникХозДоговора() { НомерУчастникаХозДоговора = 3, Личность = new Личность() },
                new УчастникХозДоговора() { НомерУчастникаХозДоговора = 5, Личность = new Личность() },
            };

            var detail2 = new DetailArrayOfИФХозДоговора(aggregator1)
            {
                new ИФХозДоговора { НомерИФХозДоговора = 3, ИсточникФинансирования = new ИсточникФинансирования() },
                new ИФХозДоговора { НомерИФХозДоговора = 5, ИсточникФинансирования = new ИсточникФинансирования() },
            };

            aggregator1.УчастникХозДоговора = detail1;
            aggregator1.ИФХозДоговора = detail2;

            var updateObjectsArray = new DataObject[] { aggregator1, aggregator2 };

            // Сохранение данных.
            ds.UpdateObjects(ref updateObjectsArray);
        }

        /// <summary>
        /// Проверка работы вложенной функции <see cref="ExternalLangDef.funcExist"/>.
        /// Сейчас данный тест работает только с одним DataService. Как исправят баг 94309, выполните TODO.
        /// </summary>
        [Fact(Skip = "Раскоментировать, когда будет исправлено 94309.")]
        public void Test_InsertedFuncExist()
        {
            // TODO Удалить данную строчку, как исправят баг 94309.
            IDataService dataService = DataServices.First();

            // TODO Вернуть данную строчку, как исправят баг 94309.
            // foreach (IDataService dataService in DataServices)
            {
                SQLDataService ds = (SQLDataService)dataService;

                updateTestObjects(ds);

                var detVar = new DetailVariableDef
                {
                    Type = langDef.DetailsType,
                    View = ИФХозДоговора.Views.ИФХозДоговораE,
                    OwnerConnectProp = new[] { Information.ExtractPropertyName<УчастникХозДоговора>(x => x.ХозДоговор) },
                    ConnectMasterPorp = Information.ExtractPropertyName<ИФХозДоговора>(x => x.ХозДоговор),
                };

                var func = langDef.GetFunction(
                    langDef.funcExist,
                    detVar,
                    langDef.GetFunction(
                        langDef.funcEQ,
                        new VariableDef(
                            langDef.NumericType,
                            Information.ExtractPropertyName<ИсточникФинансирования>(x => x.НомерИсточникаФинансирования)),
                        3));

                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Личность), Личность.Views.ЛичностьL);

                var dvd = new DetailVariableDef
                {
                    Type = langDef.DetailsType,
                    View = УчастникХозДоговора.Views.УчастникХозДоговораE,
                    OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                    ConnectMasterPorp = Information.ExtractPropertyName<УчастникХозДоговора>(x => x.Личность),
                };

                lcs.LimitFunction = langDef.GetFunction(langDef.funcExist, dvd, func);

                ds.LoadObjects(lcs);
            }
        }
    }
}
