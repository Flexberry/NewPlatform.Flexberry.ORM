namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public class DataObjectMemoryLeakageTests
    {
        [Fact]
        public void CreateTest()
        {
            // Arrange.
            const int iterations = 10;
            const int length = 10000;
            int[] range = Enumerable.Range(0, length).ToArray();

            CleanupMemory();

            // Act.
            IList<WeakReference> wrList = Process(iterations, range);

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

        private static void CleanupMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private IList<WeakReference> Process(int iterations, int[] range)
        {
            // Act.
            List<WeakReference> wrList = new List<WeakReference>();
            for (int i = 0; i < iterations; i++)
            {
                var bear = Iterate(range);
                wrList.Add(new WeakReference(bear));
            }

            return wrList;
        }

        private Медведь Iterate(int[] range)
        {
            Медведь bear = new Медведь { ПорядковыйНомер = 1, ЦветГлаз = "Красивый" };
            bear.Берлога.AddRange(range.Select(i => new Берлога { Наименование = $"Элитная берлога #{i} ({Guid.NewGuid()})", Комфортность = i % 5 }).ToArray());

            return bear;
        }
    }
}
