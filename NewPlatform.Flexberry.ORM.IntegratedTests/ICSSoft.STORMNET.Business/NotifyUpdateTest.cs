namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Test class for NotifyUpdate functionality.
    /// </summary>
    public class NotifyUpdateTest : BaseIntegratedTest
    {
        private ITestOutputHelper output;

        public NotifyUpdateTest(ITestOutputHelper output)
            : base("NtfyUpd")
        {
            this.output = output;
        }

        /// <summary>
        /// Test for <see cref="INotifyUpdateObjects"/> intarface.
        /// </summary>
        [Fact]
        public void INotifyUpdateObjectsTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                ds.NotifierUpdateObjects = new NotifyUpdateObjectsGeneratedMock();

                // Act & Assert.
                // Create success bear.
                var bear = new Медведь() { ПорядковыйНомер = 5 };
                ds.UpdateObject(bear);

                CheckSuccessUpdate(ds, bear);

                // Create failed bear.
                var failedBear = new Медведь();
                for (int i = 0; i < 300; i++)
                {
                    failedBear.ЦветГлаз += "Ц";
                }

                try
                {
                    ds.UpdateObject(failedBear);
                }
                catch (Exception)
                {
                }

                CheckFailUpdate(ds, failedBear);

                // Update success bear.
                bear.ПорядковыйНомер = 6;
                ds.UpdateObject(bear);

                CheckSuccessUpdate(ds, bear);

                // Update failed bear.
                for (int i = 0; i < 300; i++)
                {
                    bear.ЦветГлаз += "Ц";
                }

                try
                {
                    ds.UpdateObject(bear);
                }
                catch (Exception)
                {
                }

                CheckFailUpdate(ds, bear);

                // Delete failed bear.
                Блоха flea = new Блоха() { МедведьОбитания = bear };
                ds.UpdateObject(flea);
                bear.SetStatus(ObjectStatus.Deleted);

                try
                {
                    ds.UpdateObject(bear);
                }
                catch (Exception)
                {
                }

                CheckFailUpdate(ds, bear);

                // Delete bear.
                flea.МедведьОбитания = null;
                ds.UpdateObject(flea);

                bear.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(bear);

                CheckSuccessUpdate(ds, bear);
            }
        }

        /// <summary>
        /// Check success footprint for <see cref="Медведь"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckSuccessUpdate(IDataService dataService, Медведь dataObject)
        {
            var beforeUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.BeforeUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
            Assert.NotNull(beforeUpdateObjectsFootprint);
            Assert.Equal(dataService, beforeUpdateObjectsFootprint.Item2);
            Assert.Equal(dataObject, beforeUpdateObjectsFootprint.Item4.First());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObjects.BeforeUpdateObjects));

            var afterSuccessSqlUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessSqlUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
            Assert.NotNull(afterSuccessSqlUpdateObjectsFootprint);
            Assert.Equal(dataService, afterSuccessSqlUpdateObjectsFootprint.Item2);
            Assert.Equal(dataObject, afterSuccessSqlUpdateObjectsFootprint.Item4.First());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObjects.AfterSuccessSqlUpdateObjects));

            var afterSuccessUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessUpdateObjects)] as Tuple<Guid, IDataService, IEnumerable<DataObject>>;
            Assert.NotNull(afterSuccessUpdateObjectsFootprint);
            Assert.Equal(dataService, afterSuccessUpdateObjectsFootprint.Item2);
            Assert.Equal(dataObject, afterSuccessUpdateObjectsFootprint.Item3.First());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObjects.AfterSuccessUpdateObjects));

            var afterFailUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.AfterFailUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
            Assert.Null(afterFailUpdateObjectsFootprint);

            Assert.Equal(beforeUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
            Assert.Equal(afterSuccessUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
        }

        /// <summary>
        /// Check failed footprint for <see cref="Медведь"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckFailUpdate(IDataService dataService, Медведь dataObject)
        {
            var failedBeforeUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.BeforeUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
            Assert.NotNull(failedBeforeUpdateObjectsFootprint);
            Assert.Equal(dataService, failedBeforeUpdateObjectsFootprint.Item2);
            Assert.Equal(dataObject, failedBeforeUpdateObjectsFootprint.Item4.First());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObjects.BeforeUpdateObjects));

            var failedAfterSuccessSqlUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessSqlUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
            Assert.Null(failedAfterSuccessSqlUpdateObjectsFootprint);

            var failedAfterSuccessUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessUpdateObjects)] as Tuple<Guid, IDataService, IEnumerable<DataObject>>;
            Assert.Null(failedAfterSuccessUpdateObjectsFootprint);

            var failedAfterFailUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObjects.AfterFailUpdateObjects)] as Tuple<Guid, IDataService, IEnumerable<DataObject>>;
            Assert.NotNull(failedAfterFailUpdateObjectsFootprint);
            Assert.Equal(dataService, failedAfterFailUpdateObjectsFootprint.Item2);
            Assert.Equal(dataObject, failedAfterFailUpdateObjectsFootprint.Item3.First());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObjects.AfterFailUpdateObjects));

            Assert.Equal(failedBeforeUpdateObjectsFootprint.Item1, failedAfterFailUpdateObjectsFootprint.Item1);
        }

        /// <summary>
        /// Test for <see cref="INotifyUpdateObject"/> intarface.
        /// </summary>
        [Fact]
        public void INotifyUpdateObjectTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                var notifierUpdatePropertyByType = new NotifierUpdatePropertyByType();
                ds.NotifierUpdateObjects = new NotifierUpdateObjects();

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                // Act.
                // Теперь грузим его из БД.
                var медведь = new Медведь();
                медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                ds.LoadObject(медведь, false, false);

                // Assert.
                Assert.NotNull(медведь.Берлога);
                Assert.Equal(0, медведь.Берлога.Count);
            }
        }

        /// <summary>
        /// Test for <see cref="INotifyUpdateProperty"/> intarface.
        /// </summary>
        [Fact]
        public void INotifyUpdatePropertyTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                var notifierUpdatePropertyByType = new NotifierUpdatePropertyByType();
                ds.NotifierUpdateObjects = new NotifierUpdateObjects();

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                // Act.
                // Теперь грузим его из БД.
                var медведь = new Медведь();
                медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                ds.LoadObject(медведь, false, false);

                // Assert.
                Assert.NotNull(медведь.Берлога);
                Assert.Equal(0, медведь.Берлога.Count);
            }
        }

        /// <summary>
        /// Test for <see cref="INotifyUpdatePropertyByType"/> intarface.
        /// </summary>
        [Fact]
        public void INotifyUpdatePropertyByTypeTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var ds = (SQLDataService)dataService;
                var notifierUpdatePropertyByType = new NotifierUpdatePropertyByType();
                ds.NotifierUpdateObjects = new NotifierUpdateObjects();

                // Чтобы медведь в БД точно был, создадим его.
                var createdBear = new Медведь();
                ds.UpdateObject(createdBear);

                // Act.
                // Теперь грузим его из БД.
                var медведь = new Медведь();
                медведь.SetExistObjectPrimaryKey(createdBear.__PrimaryKey);
                ds.LoadObject(медведь, false, false);

                // Assert.
                Assert.NotNull(медведь.Берлога);
                Assert.Equal(0, медведь.Берлога.Count);
            }
        }


    }
}
