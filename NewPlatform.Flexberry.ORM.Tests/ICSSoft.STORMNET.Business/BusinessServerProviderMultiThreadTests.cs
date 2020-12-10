namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Тесты <see cref="BusinessServerProvider" />
    /// </summary>
    public class BusinessServerProviderMultiThreadTests
    {
        private readonly ITestOutputHelper Output;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BusinessServerProviderMultiThreadTests(ITestOutputHelper output)
        {
            Output = output;
        }

        /// <summary>
        /// Многопоточный тест метода <see cref="BusinessServerProvider.GetBusinessServer(Type,DataServiceObjectEvents,IDataService)"/> для <see cref="ParallelEnumerable.AsParallel"/>
        /// </summary>
        [Fact]
        public void GetBusinessServerMultiThreadAsParallelTest()
        {
            // Arrange.
            const int length = 100;
            IEnumerable<int> range = Enumerable.Range(0, length);

            // Act.
            range.AsParallel().ForAll(Action);
        }

        /// <summary>
        /// Многопоточный тест метода <see cref="BusinessServerProvider.GetBusinessServer(Type,DataServiceObjectEvents,IDataService)"/> с использованием <see cref="MultiThreadingTestTool"/>
        /// </summary>
        [Fact]
        public void GetBusinessServerMultiThreadToolTest()
        {
            // Arrange.
            const int length = 100;
            var multiThreadingTestTool = new MultiThreadingTestTool(MultiThreadMethod);
            multiThreadingTestTool.StartThreads(length);

            // Assert.
            var exception = multiThreadingTestTool.GetExceptions();
            if (exception != null)
            {
                foreach (var item in exception.InnerExceptions)
                {
                    Output.WriteLine($"Thread {item.Key}: {item.Value}");
                }

                // Пусть так.
                Assert.Empty(exception.InnerExceptions);
            }
        }

        private void MultiThreadMethod(object sender)
        {
            var parametersDictionary = sender as Dictionary<string, object>;
            ConcurrentDictionary<string, Exception> exceptions = parametersDictionary[MultiThreadingTestTool.ParamNameExceptions] as ConcurrentDictionary<string, Exception>;

            // Arrange.
            var processingObject = new ObjectsToUpdateMultiThreadClass();
            var processingObjects = new ArrayList { processingObject };

            // Act.
            var bss = BusinessServerProvider.GetBusinessServer(typeof(ObjectsToUpdateMultiThreadClass), DataServiceObjectEvents.OnInsertToStorage, null);
            foreach (BusinessServer bs in bss)
            {
                try
                {
                    bs.ObjectsToUpdate = processingObjects;
                    DataObject[] subobjects = bs.OnUpdateDataobject(processingObject);

                    // Assert.
                    Assert.Empty(subobjects);
                }
                catch (Exception exception)
                {
                    exceptions.TryAdd(Thread.CurrentThread.Name, exception);
                    parametersDictionary[MultiThreadingTestTool.ParamNameWorking] = false;
                    return;
                }
            }
        }

        private void Action(int i)
        {
            // Arrange.
            var processingObject = new ObjectsToUpdateMultiThreadClass();
            var processingObjects = new ArrayList { processingObject };

            // Act.
            var bss = BusinessServerProvider.GetBusinessServer(typeof(ObjectsToUpdateMultiThreadClass), DataServiceObjectEvents.OnInsertToStorage, null);
            foreach (BusinessServer bs in bss)
            {
                bs.ObjectsToUpdate = processingObjects;
                DataObject[] subobjects = bs.OnUpdateDataobject(processingObject);

                // Assert.
                Assert.Empty(subobjects);
            }
        }

        public class ObjectsToUpdateMultiThreadBS : BusinessServer
        {
            public virtual DataObject[] OnUpdateObjectsToUpdateMultiThreadClass(ObjectsToUpdateMultiThreadClass UpdatedObject)
            {
                if (!ObjectsToUpdate.Contains(UpdatedObject))
                {
                    throw new Exception("Обновляемый объект не найден в перечне обновляемых объектов бизнес-сервера.");
                }

                return new DataObject[0];
            }
        }

        [BusinessServer(typeof(ObjectsToUpdateMultiThreadBS), DataServiceObjectEvents.OnAllEvents)]
        public class ObjectsToUpdateMultiThreadClass : DataObject
        {
        }
    }
}
