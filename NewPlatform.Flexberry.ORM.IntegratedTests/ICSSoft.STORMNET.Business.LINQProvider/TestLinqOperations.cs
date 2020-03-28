namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.UserDataTypes;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.IntegratedTests;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Класс для тестирования операций при работе с LINQProvider'ом в нескольких потоках.
    /// </summary>
    public class TestLinqOperations : BaseIntegratedTest
    {
        private ITestOutputHelper output;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TestLinqOperations(ITestOutputHelper output)
            : base("LTest3")
        {
            this.output = output;
        }

        /// <summary>
        /// Работа LINQProvider'а с Take() в нескольких потоках.
        /// </summary>
        [Fact]
        public void TestWorkLinqWithTake()
        {
            foreach (IDataService dataService in DataServices)
            {
                CreateTestData(dataService);
            }

            MultiThreadingTestTool multiThreadingTestTool = new MultiThreadingTestTool(MultiThreadWorkLinqWithTake);
            multiThreadingTestTool.StartThreads(150, DataServices);

            var exception = multiThreadingTestTool.GetExceptions();

            if (exception != null)
            {
                foreach (var item in exception.InnerExceptions)
                {
                    output.WriteLine(item.Value.ToString());
                }

                throw exception.InnerException;
            }
        }

        /// <summary>
        /// Метод для многопоточного исполнения в <see cref="TestWorkLinqWithTake"/>.
        /// </summary>
        /// <param name="sender">Словарь со значениями параметров для исполнения метода.</param>
        private void MultiThreadWorkLinqWithTake(object sender)
        {
            Random rand = new Random();
            var parametersDictionary = sender as Dictionary<string, object>;
            List<ICSSoft.STORMNET.Business.IDataService> dsList = parametersDictionary[MultiThreadingTestTool.ParamNameSender] as List<ICSSoft.STORMNET.Business.IDataService>;

            Dictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as Dictionary<string, Exception>;

            for (int i = 0; i < 5; i++)
            {
                if (!(bool)parametersDictionary[MultiThreadingTestTool.ParamNameWorking])
                {
                    return;
                }

                try
                {
                    IDataService ds = dsList[rand.Next(dsList.Count - 1)];

                    // Arrange.
                    var requestsQuery = from x in ds.Query<Кошка>(Кошка.Views.КошкаE, new[] { Лапа.Views.ЛапаShort })
                                        where !x.Агрессивная &&
                                          x.Лапа.Cast<Лапа>().Any(s => s.Размер == 2)
                                        select x;
                    var requests = requestsQuery.Take(1).ToList();

                    // Act & Assert.
                    Assert.Equal(requests.Count, 1);
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
        /// Работа Lcs c ReturnTop в нескольких потоках (аналогичен тесту <see cref="TestWorkLinqWithTake"/>).
        /// </summary>
        [Fact]
        public void TestWorkLcsWithReturnTop()
        {
            foreach (IDataService dataService in DataServices)
            {
                CreateTestData(dataService);
            }

            MultiThreadingTestTool multiThreadingTestTool = new MultiThreadingTestTool(MultiThreadWorkLcsWithReturnTop);
            multiThreadingTestTool.StartThreads(150, DataServices);

            var exception = multiThreadingTestTool.GetExceptions();

            if (exception != null)
            {
                foreach (var item in exception.InnerExceptions)
                {
                    output.WriteLine(item.Value.ToString());
                }

                throw exception.InnerException;
            }
        }

        /// <summary>
        /// Метод для многопоточного исполнения в <see cref="TestWorkLcsWithReturnTop"/>.
        /// </summary>
        /// <param name="sender">Словарь со значениями параметров для исполнения метода.</param>
        private void MultiThreadWorkLcsWithReturnTop(object sender)
        {
            Random rand = new Random();
            var parametersDictionary = sender as Dictionary<string, object>;
            List<ICSSoft.STORMNET.Business.IDataService> dsList = parametersDictionary[MultiThreadingTestTool.ParamNameSender] as List<ICSSoft.STORMNET.Business.IDataService>;

            Dictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as Dictionary<string, Exception>;

            for (int i = 0; i < 5; i++)
            {
                if (!(bool)parametersDictionary[MultiThreadingTestTool.ParamNameWorking])
                {
                    return;
                }

                try
                {
                    IDataService ds = dsList[rand.Next(dsList.Count - 1)];

                    ExternalLangDef ldef = ExternalLangDef.LanguageDef;
                    ldef.DataService = ds;

                    // Arrange.
                    ICSSoft.STORMNET.View pawView = new ICSSoft.STORMNET.View();
                    pawView.DefineClassType = typeof(Лапа);

                    pawView.Properties = new PropertyInView[] 
                    {
                        new PropertyInView("Размер", "Размер", true, string.Empty),
                    };

                    ICSSoft.STORMNET.View catView = new ICSSoft.STORMNET.View();
                    catView.DefineClassType = typeof(Кошка);

                    catView.Properties = new PropertyInView[] 
                    {
                        new PropertyInView("Агрессивная", "Агрессивная", true, string.Empty),
                    };

                    catView.AddDetailInView("Лапа", pawView, true);

                    var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), catView);

                    var detail = new DetailVariableDef
                    {
                        Type = ldef.DetailsType,
                        View = pawView,
                        OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                        ConnectMasterPorp = "Кошка"
                    };

                    var limitFunction1 = ldef.GetFunction(
                            ldef.funcExist,
                            detail,
                            ldef.GetFunction(
                                ldef.funcEQ,
                                new VariableDef(ldef.NumericType, Information.ExtractPropertyPath<Лапа>(x => x.Размер)),
                                2));

                    lcs.LimitFunction =
                        ldef.GetFunction(
                            ldef.funcAND,
                            ldef.GetFunction(
                                ldef.funcEQ,
                                new VariableDef(ldef.BoolType, Information.ExtractPropertyPath<Кошка>(x => x.Агрессивная)),
                                false),
                            limitFunction1);

                    lcs.ReturnTop = 1;

                    var resultValue = ds.LoadObjects(lcs).ToList();

                    // Act & Assert.
                    Assert.Equal(resultValue.Count, 1);
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
        /// Создание тестовых данных.
        /// </summary>
        /// <param name="ds">Сервис данных.</param>
        private void CreateTestData(IDataService ds)
        {
            var testCat1 = new Кошка { Кличка = "Кат1", Агрессивная = false, ПородаСтрокой = "Дворовая", ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая, Порода = new Порода() };
            var testCat1Paw1 = new Лапа { Цвет = "Черный", Размер = 4, Номер = 1, ТипЛапы = new ТипЛапы() };
            testCat1.Лапа.Add(testCat1Paw1);
            testCat1Paw1.Перелом.Add(new Перелом { Тип = ТипПерелома.Закрытый, Дата = DateTime.Now });
            testCat1.Лапа.Add(new Лапа { Цвет = "Черный", Размер = 4, Номер = 2, ТипЛапы = new ТипЛапы() });
            testCat1.Лапа.Add(new Лапа { Цвет = "Черный", Размер = 4, Номер = 3, ТипЛапы = new ТипЛапы() });
            var testCat1Paw4 = new Лапа { Цвет = "Черный", Размер = 4, Номер = 4, ТипЛапы = new ТипЛапы() };
            testCat1.Лапа.Add(testCat1Paw4);
            testCat1Paw4.Перелом.Add(new Перелом { Тип = ТипПерелома.Закрытый, Дата = DateTime.Now });

            var testCat2 = new Кошка { Кличка = "Кат2", Агрессивная = false, ПородаСтрокой = "Сфинкс", ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая, Порода = new Порода() };
            testCat2.Лапа.Add(new Лапа { Цвет = "Розовый", Размер = 2, Номер = 1, ТипЛапы = new ТипЛапы() });
            testCat2.Лапа.Add(new Лапа { Цвет = "Розовый", Размер = 2, Номер = 2, ТипЛапы = new ТипЛапы() });
            testCat2.Лапа.Add(new Лапа { Цвет = "Розовый", Размер = 2, Номер = 3, ТипЛапы = new ТипЛапы() });
            testCat2.Лапа.Add(new Лапа { Цвет = "Розовый", Размер = 2, Номер = 4, ТипЛапы = new ТипЛапы() });

            var testCat3 = new Кошка { Кличка = "Кат3", Агрессивная = false, ПородаСтрокой = "Веслоухая", ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая, Порода = new Порода() };
            testCat3.Лапа.Add(new Лапа { Цвет = "Синий", Размер = 2, Номер = 1, ТипЛапы = new ТипЛапы() });
            testCat3.Лапа.Add(new Лапа { Цвет = "Синий", Размер = 2, Номер = 2, ТипЛапы = new ТипЛапы() });
            var testCat3Paw3 = new Лапа { Цвет = "Синий", Размер = 2, Номер = 3, ТипЛапы = new ТипЛапы() };
            testCat3.Лапа.Add(testCat3Paw3);
            testCat3Paw3.Перелом.Add(new Перелом { Тип = ТипПерелома.Закрытый, Дата = DateTime.Now });
            testCat3Paw3.Перелом.Add(new Перелом { Тип = ТипПерелома.Открытый, Дата = DateTime.Now });
            testCat3.Лапа.Add(new Лапа { Цвет = "Синий", Размер = 2, Номер = 4, ТипЛапы = new ТипЛапы() });

            var testCat4 = new Кошка { Кличка = "Кат4", Агрессивная = false, ПородаСтрокой = "Мэйнкун", ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая, Порода = new Порода() };
            var testCat4Paw1 = new Лапа { Цвет = "Белый", Размер = 6, Номер = 1, ТипЛапы = new ТипЛапы() };
            testCat4.Лапа.Add(testCat4Paw1);
            testCat4.Лапа.Add(new Лапа { Цвет = "Белый", Размер = 6, Номер = 2, ТипЛапы = new ТипЛапы() });
            testCat4.Лапа.Add(new Лапа { Цвет = "Белый", Размер = 6, Номер = 3, ТипЛапы = new ТипЛапы() });
            testCat4.Лапа.Add(new Лапа { Цвет = "Белый", Размер = 6, Номер = 4, ТипЛапы = new ТипЛапы() });
            testCat4Paw1.Перелом.Add(new Перелом { Тип = ТипПерелома.Закрытый, Дата = DateTime.Now });
            testCat4Paw1.Перелом.Add(new Перелом { Тип = ТипПерелома.Закрытый, Дата = DateTime.Now });

            var testCat5 = new Кошка { Кличка = "Кат5", Агрессивная = true, ПородаСтрокой = "Сиамская", ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая, Порода = new Порода() };
            testCat5.Лапа.Add(new Лапа { Цвет = "Черный", Размер = 2, Номер = 1, ТипЛапы = new ТипЛапы() });
            testCat5.Лапа.Add(new Лапа { Цвет = "Белый", Размер = 2, Номер = 2, ТипЛапы = new ТипЛапы() });
            testCat5.Лапа.Add(new Лапа { Цвет = "Черный", Размер = 2, Номер = 3, ТипЛапы = new ТипЛапы() });
            testCat5.Лапа.Add(new Лапа { Цвет = "Белый", Размер = 2, Номер = 4, ТипЛапы = new ТипЛапы() });

            // Сохранение данных.
            var updateObjectsArray = new DataObject[] { testCat1, testCat2, testCat3, testCat4, testCat5 };
            ds.UpdateObjects(ref updateObjectsArray);
        }
    }
}
