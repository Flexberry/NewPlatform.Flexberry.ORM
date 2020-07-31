namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Diagnostics;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using System.Configuration;

    /// <summary>
    /// Тест скорости сервиса данных.
    /// </summary>
    public class DataServiceSpeedTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataServiceSpeedTest()
            : base("DSSpeed")
        {
        }

        /// <summary>
        /// Цикл: создание, загрузка, обновление, удаление - количество циклов в секунду.
        /// </summary>
        [Fact]
        public void InsertSelectUpdateDeleteSpeedTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;

                var createdBear1 = new Медведь();
                createdBear1.ЦветГлаз = "Косолапый Мишка 1";
                ds.UpdateObject(createdBear1);

                Random random = new Random();

                Stopwatch stopwatch = new Stopwatch();

                int i = 0;

                stopwatch.Start();

                do
                {
                    // Чтобы медведь в БД точно был, создадим его.
                    var createdBear = new Медведь();
                    createdBear.ЦветГлаз = "Косолапый Мишка " + random.Next(101);
                    ds.UpdateObject(createdBear);

                    // Теперь грузим его из БД.
                    var медведь = new Медведь();
                    медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                    ds.LoadObject(медведь, false, false);

                    медведь.ЦветГлаз = "Топтыгин " + random.Next(101);

                    ds.UpdateObject(медведь);

                    медведь.SetStatus(ObjectStatus.Deleted);
                    ds.UpdateObject(медведь);
                    i++;
                } while (stopwatch.ElapsedMilliseconds < 1000);

                stopwatch.Stop();
                Console.WriteLine(i + " instances.");
            }
        }
    }
}
