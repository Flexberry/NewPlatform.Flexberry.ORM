namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using Xunit;

    /// <summary>
    /// Тесты <see cref="BusinessServerProvider" />
    /// </summary>
    public class BusinessServerProviderTests
    {
        /// <summary>
        /// Многопоточный тест метода <see cref="BusinessServerProvider.GetBusinessServer(Type,DataServiceObjectEvents,IDataService)"/>
        /// </summary>
        [Fact]
        public void GetBusinessServerMultiThreadTest()
        {
            // Arrange.
            const int length = 100;
            int[] range = Enumerable.Range(0, length).ToArray();

            // Act.
            range.AsParallel().ForAll(Action);
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
