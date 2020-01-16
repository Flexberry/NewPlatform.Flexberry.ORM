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
                // Create bear.
                var bear = new Медведь() { ПорядковыйНомер = 5 };
                ds.UpdateObject(bear);

                var beforeUpdateObjectsFootprint = bear.DynamicProperties[nameof(INotifyUpdateObjects.BeforeUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.NotNull(beforeUpdateObjectsFootprint);
                Assert.Equal(ds, beforeUpdateObjectsFootprint.Item2);
                Assert.Equal(bear, beforeUpdateObjectsFootprint.Item4.First());
                bear.DynamicProperties.Remove(nameof(INotifyUpdateObjects.BeforeUpdateObjects));

                var afterSuccessSqlUpdateObjectsFootprint = bear.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessSqlUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.NotNull(afterSuccessSqlUpdateObjectsFootprint);
                Assert.Equal(ds, afterSuccessSqlUpdateObjectsFootprint.Item2);
                Assert.Equal(bear, afterSuccessSqlUpdateObjectsFootprint.Item4.First());
                bear.DynamicProperties.Remove(nameof(INotifyUpdateObjects.AfterSuccessSqlUpdateObjects));

                var afterSuccessUpdateObjectsFootprint = bear.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.NotNull(afterSuccessUpdateObjectsFootprint);
                Assert.Equal(ds, afterSuccessUpdateObjectsFootprint.Item2);
                Assert.Equal(bear, afterSuccessUpdateObjectsFootprint.Item4.First());
                bear.DynamicProperties.Remove(nameof(INotifyUpdateObjects.AfterSuccessUpdateObjects));

                var afterFailUpdateObjectsFootprint = bear.DynamicProperties[nameof(INotifyUpdateObjects.AfterFailUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.Null(afterFailUpdateObjectsFootprint);

                Assert.Equal(beforeUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
                Assert.Equal(afterSuccessUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);

                var failedBear = new Медведь();
                for (int i = 0; i < 300; i++)
                {
                    failedBear.ЦветГлаз += "Ц";
                }

                ds.UpdateObject(failedBear);

                var failedBeforeUpdateObjectsFootprint = failedBear.DynamicProperties[nameof(INotifyUpdateObjects.BeforeUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.NotNull(failedBeforeUpdateObjectsFootprint);
                Assert.Equal(ds, failedBeforeUpdateObjectsFootprint.Item2);
                Assert.Equal(bear, failedBeforeUpdateObjectsFootprint.Item4.First());
                failedBear.DynamicProperties.Remove(nameof(INotifyUpdateObjects.BeforeUpdateObjects));

                var failedAfterSuccessSqlUpdateObjectsFootprint = failedBear.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessSqlUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.Null(failedAfterSuccessSqlUpdateObjectsFootprint);

                var failedAfterSuccessUpdateObjectsFootprint = failedBear.DynamicProperties[nameof(INotifyUpdateObjects.AfterSuccessUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.Null(failedAfterSuccessUpdateObjectsFootprint);

                var failedAfterFailUpdateObjectsFootprint = failedBear.DynamicProperties[nameof(INotifyUpdateObjects.AfterFailUpdateObjects)] as Tuple<Guid, IDataService, System.Data.IDbTransaction, IEnumerable<DataObject>>;
                Assert.NotNull(failedAfterFailUpdateObjectsFootprint);
                Assert.Equal(ds, failedAfterFailUpdateObjectsFootprint.Item2);
                Assert.Equal(bear, failedAfterFailUpdateObjectsFootprint.Item4.First());
                failedBear.DynamicProperties.Remove(nameof(INotifyUpdateObjects.AfterFailUpdateObjects));

                Assert.Equal(failedBeforeUpdateObjectsFootprint.Item1, failedAfterFailUpdateObjectsFootprint.Item1);

                // Update bear.
                bear.ПорядковыйНомер = 6;
                ds.UpdateObject(bear);

                // Delete bear.
                bear.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(bear);
            }
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
