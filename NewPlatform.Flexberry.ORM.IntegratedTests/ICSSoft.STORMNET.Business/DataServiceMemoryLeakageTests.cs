namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты утечки памяти.
    /// </summary>
    public class DataServiceMemoryLeakageTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataServiceMemoryLeakageTests()
            : base("DSMem") { }

        /// <summary>
        /// Тест создания тяжелых объектов.
        /// </summary>
        [Fact(Skip = "Manual testing")]
        public void CreateTest()
        {
            // Arrange.
            const int iterations = 10;
            const int length = 10000;
            int[] range = Enumerable.Range(0, length).ToArray();

            foreach (var dataService in DataServices)
            {
                CleanupMemory();

                // Act.
                IList<WeakReference> wrList = ProcessDataServiceCreate(dataService, iterations, range);

                CleanupMemory();

                // Assert.
                Assert.All(
                    wrList,
                    wr =>
                    {
                        if (wr.IsAlive)
                            throw new Exception("GC не смог освободить объект.");
                    });
            }
        }

        /// <summary>
        /// Тест обновления тяжелых объектов.
        /// </summary>
        [Fact(Skip = "Manual testing")]
        public void UpdateTest()
        {
            // Arrange.
            const int iterations = 10;
            const int length = 10000;
            int[] range = Enumerable.Range(0, length).ToArray();

            foreach (var dataService in DataServices)
            {
                CleanupMemory();

                // Act.
                IList<WeakReference> wrList = ProcessDataServiceUpdate(dataService, iterations, range);

                CleanupMemory();

                // Assert.
                Assert.All(
                    wrList,
                    wr =>
                    {
                        if (wr.IsAlive)
                            throw new Exception("GC не смог освободить объект.");
                    });
            }
        }

        /// <summary>
        /// Тест чтения тяжелых объектов.
        /// </summary>
        [Fact(Skip = "Manual testing")]
        public void ReadTest()
        {
            // Arrange.
            const int iterations = 10;
            const int length = 10000;
            int[] range = Enumerable.Range(0, length).ToArray();

            foreach (var dataService in DataServices)
            {
                CleanupMemory();

                // Act.
                IList<WeakReference> wrList = ProcessDataServiceRead(dataService, iterations, range);

                CleanupMemory();

                // Assert.
                Assert.All(
                    wrList,
                    wr =>
                    {
                        if (wr.IsAlive)
                            throw new Exception("GC не смог освободить объект.");
                    });
            }
        }

        private static void CleanupMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private IList<WeakReference> ProcessDataServiceCreate(IDataService dataService, int iterations, int[] range)
        {
            // Act.
            List<WeakReference> wrList = new List<WeakReference>();
            for (int i = 0; i < iterations; i++)
            {
                var bear = IterateCreate(dataService, range);
                wrList.Add(new WeakReference(bear));
            }

            return wrList;
        }

        private IList<WeakReference> ProcessDataServiceUpdate(IDataService dataService, int iterations, int[] range)
        {
            // Arrange.
            Медведь bear = new Медведь { ПорядковыйНомер = 1, ЦветГлаз = "Красивый" };
            bear.Берлога.AddRange(range.Select(i => new Берлога { Наименование = $"Элитная берлога #{i} ({Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()})", Комфортность = i % 5 }).ToArray());
            dataService.UpdateObject(bear);

            // Act.
            List<WeakReference> wrList = new List<WeakReference>();
            for (int i = 0; i < iterations; i++)
            {
                var loadedBear = IterateUpdate(dataService, bear);
                wrList.Add(new WeakReference(loadedBear));
            }

            return wrList;
        }

        private IList<WeakReference> ProcessDataServiceRead(IDataService dataService, int iterations, int[] range)
        {
            // Arrange.
            Медведь bear = new Медведь { ПорядковыйНомер = 1, ЦветГлаз = "Красивый" };
            bear.Берлога.AddRange(range.Select(i => new Берлога { Наименование = $"Элитная берлога #{i} ({Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()})", Комфортность = i % 5 }).ToArray());
            dataService.UpdateObject(bear);

            // Act.
            List<WeakReference> wrList = new List<WeakReference>();
            for (int i = 0; i < iterations; i++)
            {
                var loadedBear = IterateRead(dataService, bear);
                wrList.Add(new WeakReference(loadedBear));
            }

            return wrList;
        }

        private Медведь IterateCreate(IDataService dataService, int[] range)
        {
            Медведь bear = new Медведь { ПорядковыйНомер = 1, ЦветГлаз = "Красивый" };
            bear.Берлога.AddRange(range.Select(i => new Берлога { Наименование = $"Элитная берлога #{i} ({Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()})", Комфортность = i % 5 }).ToArray());
            dataService.UpdateObject(bear);

            return bear;
        }

        private Медведь IterateUpdate(IDataService dataService, Медведь bear)
        {
            bear.ПорядковыйНомер = bear.ПорядковыйНомер++;
            foreach (var берлога in bear.Берлога.Cast<Берлога>())
            {
                берлога.Комфортность = 5 - берлога.Комфортность;
            }

            dataService.UpdateObject(bear);

            return bear;
        }

        private Медведь IterateRead(IDataService dataService, Медведь bear)
        {
            Медведь loadedBear = PKHelper.CreateDataObject<Медведь>(bear);
            dataService.LoadObject(Медведь.Views.МедведьE, loadedBear);

            return loadedBear;
        }
    }
}
