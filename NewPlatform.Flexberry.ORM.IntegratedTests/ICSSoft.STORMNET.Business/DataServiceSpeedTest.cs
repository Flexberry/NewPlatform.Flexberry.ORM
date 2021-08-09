namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Diagnostics;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Тест скорости сервиса данных.
    /// </summary>
    public class DataServiceSpeedTest : BaseIntegratedTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataServiceSpeedTest(ITestOutputHelper testOutputHelper)
            : base("DSSpeed")
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Цикл: создание, загрузка, обновление, удаление - количество циклов в секунду.
        /// </summary>
        [Fact]
        public void InsertSelectUpdateDeleteSpeedTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var createdBear1 = new Медведь();
                createdBear1.ЦветГлаз = "Косолапый Мишка 1";
                dataService.UpdateObject(createdBear1);

                Random random = new Random();

                Stopwatch stopwatch = new Stopwatch();

                int i = 0;

                stopwatch.Start();

                do
                {
                    // Чтобы медведь в БД точно был, создадим его.
                    var createdBear = new Медведь();
                    createdBear.ЦветГлаз = "Косолапый Мишка " + random.Next(101);
                    dataService.UpdateObject(createdBear);

                    // Теперь грузим его из БД.
                    var медведь = new Медведь();
                    медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                    dataService.LoadObject(медведь, false, false);

                    медведь.ЦветГлаз = "Топтыгин " + random.Next(101);

                    dataService.UpdateObject(медведь);

                    медведь.SetStatus(ObjectStatus.Deleted);
                    dataService.UpdateObject(медведь);
                    i++;
                }
                while (stopwatch.ElapsedMilliseconds < 10000);

                stopwatch.Stop();
                testOutputHelper.WriteLine($"{nameof(InsertSelectUpdateDeleteSpeedTest)}@{dataService.GetType().Name}: {i} iterations");
            }
        }
    }
}
