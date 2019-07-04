namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.UserDataTypes;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;
    using Xunit.Abstractions;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using System.Linq;

    /// <summary>
    /// Тест для проверки ошибки на генерацию невалидного SQL.
    /// </summary>
    public class WrongQueryTest : BaseIntegratedTest
    {
        private ITestOutputHelper output;

        public WrongQueryTest(ITestOutputHelper output)
            : base("DetLoad")
        {
            this.output = output;
        }

        private Random _random = new Random();

       
        /// <summary>
        /// Метод для организации многопоточности.
        /// </summary>
        [Fact]
        public void ThreadsQueryTest()
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

                Thread threadCat = new Thread(CatQueue);
                Thread threadPaw = new Thread(BearQueue);
                threadCat.Start();
                threadPaw.Start();
            }
        }

        public void CatQueue()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                // Чтобы объекты в БД точно были, создадим их.
                var createdCat = new Кошка();
                ds.UpdateObject(createdCat);

                var createdPaw = new Лапа();
                ds.UpdateObject(createdPaw);

                // Теперь грузим их из БД.
                var кошка = new Кошка();
                кошка.SetExistObjectPrimaryKey(createdCat.__PrimaryKey);
                ds.LoadObject(кошка, false, false);

                var лапа = new Лапа();
                лапа.SetExistObjectPrimaryKey(createdPaw.__PrimaryKey);
                ds.LoadObject(лапа, false, false);

                Assert.NotNull(кошка);
                Assert.NotNull(лапа);

                кошка = new Кошка() { Кличка = nameof(ThreadsQueryTest), Порода = new Порода() { Название = "Беспородная" }, ДатаРождения = NullableDateTime.Now, Тип = ТипКошки.Дикая };
                лапа = new Лапа() { Цвет = "red", Номер = 1, ДатаРождения = NullableDateTime.Now, Кошка = кошка };
                кошка.Лапа.Add(new Лапа() { Номер = 1 });
                ds.UpdateObject(кошка);
                ds.UpdateObject(лапа);

                var statusNeedSigning = Сторона.Левая;
                var requests = (from x in ds.Query<Лапа>(Лапа.Views.ЛапаFull)
                                where x.Сторона == statusNeedSigning && x.Кошка.ДатаРождения == NullableDateTime.Now
                                select x).Skip(0).Take(100).ToList();
            }
        }

        public void BearQueue()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                // Чтобы объекты в БД точно были, создадим их.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                // Теперь грузим их из БД.
                var медведь = new Медведь();
                медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                ds.LoadObject(медведь, false, false);

                Assert.NotNull(медведь);

                медведь = new Медведь() { Пол = Пол.Женский, ДатаРождения = NullableDateTime.Now };
                ds.UpdateObject(медведь);

                var statusNeedSigning = Пол.Женский;
                var requests = (from x in ds.Query<Медведь>(Медведь.Views.МедведьE)
                                where x.Пол == statusNeedSigning && x.ДатаРождения == NullableDateTime.Now
                                select x).Skip(0).Take(100).ToList();
            }
        }

        /// <summary>
        /// Метод для многопоточного исполнения в <see cref="ThreadsQueryTest"/>.
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
                    lcs.LimitFunction = langdef.GetFunction(langdef.funcLike, new VariableDef(langdef.StringType, Information.ExtractPropertyName<Кошка>(к => к.Кличка)), nameof(ThreadsQueryTest));

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
