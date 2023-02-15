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
                var dataObjectForTest = new DataObjectForTest() { Name = "DOT" };
                DataObject[] dataObjects = new DataObject[] { bear, dataObjectForTest };
                ds.UpdateObjects(ref dataObjects);

                CheckSuccessUpdateObjects(ds, bear);

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

                CheckFailUpdateObjects(ds, failedBear);

                // Update success bear.
                bear.ПорядковыйНомер = 6;
                ds.UpdateObject(bear);

                CheckSuccessUpdateObjects(ds, bear);

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

                CheckFailUpdateObjects(ds, bear);

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

                CheckFailUpdateObjects(ds, bear);

                // Delete bear.
                flea.МедведьОбитания = null;
                ds.UpdateObject(flea);

                bear.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(bear);

                CheckSuccessUpdateObjects(ds, bear);
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
                ds.NotifierUpdateObjects = new NotifierUpdateObjects();

                // Act & Assert.
                // Create success homer.
                var homer = new Homer() { Name = "Bird" };
                var dataObjectForTest = new DataObjectForTest() { Name = "DOT" };
                DataObject[] dataObjects = new DataObject[] { dataObjectForTest, homer };
                ds.UpdateObjects(ref dataObjects);

                CheckSuccessUpdateObject(ds, homer);

                // Create failed homer.
                var failedHomer = new Homer();
                for (int i = 0; i < 300; i++)
                {
                    failedHomer.Name += "N";
                }

                try
                {
                    ds.UpdateObject(failedHomer);
                }
                catch (Exception)
                {
                }

                CheckFailUpdateObject(ds, failedHomer);

                // Update success homer.
                homer.Name = "UpdatedName";
                ds.UpdateObject(homer);

                CheckSuccessUpdateObject(ds, homer);

                // Update failed homer.
                for (int i = 0; i < 300; i++)
                {
                    homer.Name += "N";
                }

                try
                {
                    ds.UpdateObject(homer);
                }
                catch (Exception)
                {
                }

                CheckFailUpdateObject(ds, homer);

                // Delete failed homer.
                Parcel parcel = new Parcel() { DeliveredByHomer = homer };
                ds.UpdateObject(parcel);
                homer.SetStatus(ObjectStatus.Deleted);

                try
                {
                    ds.UpdateObject(homer);
                }
                catch (Exception)
                {
                }

                CheckFailUpdateObject(ds, homer);

                // Delete homer.
                parcel.DeliveredByHomer = null;
                ds.UpdateObject(parcel);

                homer.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(homer);

                CheckSuccessUpdateObject(ds, homer);
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
                ds.NotifierUpdateObjects = new NotifierUpdateObjects();

                // Act & Assert.
                // Create success mailman.
                var mailman = new Mailman() { Name = "Piter", Photo = new FileForTests() { Value = "1" } };
                ds.UpdateObject(mailman);

                CheckSuccessUpdateProperty(ds, mailman);

                // Create failed mailman.
                var failedMailman = new Mailman() { Name = "Boris", Photo = new FileForTests() { Value = "2" } };
                for (int i = 0; i < 300; i++)
                {
                    failedMailman.Name += "N";
                }

                try
                {
                    ds.UpdateObject(failedMailman);
                }
                catch (Exception)
                {
                }

                CheckFailUpdateProperty(ds, failedMailman);

                // Update success mailman.
                var coolPhoto = new FileForTests() { Value = "3" };
                mailman.Photo = coolPhoto;
                ds.UpdateObject(mailman);

                CheckSuccessUpdateProperty(ds, mailman);

                // Update failed mailman.
                for (int i = 0; i < 300; i++)
                {
                    mailman.Name += "N";
                }

                Assert.Equal(0, mailman.DynamicProperties.Count);

                mailman.Photo = new FileForTests() { Value = "4" };
                try
                {
                    ds.UpdateObject(mailman);
                }
                catch (Exception)
                {
                }

                CheckFailUpdateProperty(ds, mailman);

                // Remove Photo from AlteredPropertyNames.
                mailman.Photo = coolPhoto;

                // Delete failed mailman.
                Parcel parcel = new Parcel() { DeliveredByMailman = mailman };
                ds.UpdateObject(parcel);
                mailman.SetStatus(ObjectStatus.Deleted);

                try
                {
                    ds.UpdateObject(mailman);
                }
                catch (Exception)
                {
                }

                CheckFailUpdateProperty(ds, mailman);

                // Delete mailman.
                parcel.DeliveredByMailman = null;
                ds.UpdateObject(parcel);

                mailman.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(mailman);

                CheckSuccessUpdateProperty(ds, mailman);
            }
        }

        /// <summary>
        /// Test for <see cref="INotifyUpdatePropertyByType"/> intarface.
        /// </summary>
        [Fact(Skip = "Not implemented interface yet")]
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Check success footprint for <see cref="Медведь"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckSuccessUpdateObjects(IDataService dataService, Медведь dataObject)
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
        private void CheckFailUpdateObjects(IDataService dataService, Медведь dataObject)
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
        /// Check success footprint for <see cref="Homer"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckSuccessUpdateObject(IDataService dataService, Homer dataObject)
        {
            var beforeUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.BeforeUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.NotNull(beforeUpdateObjectsFootprint);
            Assert.Equal(dataObject, beforeUpdateObjectsFootprint.Item2.Last());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObject.BeforeUpdateObject));

            var afterSuccessSqlUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.AfterSuccessSqlUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.NotNull(afterSuccessSqlUpdateObjectsFootprint);
            Assert.Equal(dataObject, afterSuccessSqlUpdateObjectsFootprint.Item2.Last());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObject.AfterSuccessSqlUpdateObject));

            var afterSuccessUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.AfterSuccessUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.NotNull(afterSuccessUpdateObjectsFootprint);
            Assert.Equal(dataObject, afterSuccessUpdateObjectsFootprint.Item2.Last());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObject.AfterSuccessUpdateObject));

            var afterFailUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.AfterFailUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.Null(afterFailUpdateObjectsFootprint);

            Assert.Equal(beforeUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
            Assert.Equal(afterSuccessUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
        }

        /// <summary>
        /// Check failed footprint for <see cref="Homer"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckFailUpdateObject(IDataService dataService, Homer dataObject)
        {
            var failedBeforeUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.BeforeUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.NotNull(failedBeforeUpdateObjectsFootprint);
            Assert.Equal(dataObject, failedBeforeUpdateObjectsFootprint.Item2.Last());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObject.BeforeUpdateObject));

            var failedAfterSuccessSqlUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.AfterSuccessSqlUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.Null(failedAfterSuccessSqlUpdateObjectsFootprint);

            var failedAfterSuccessUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.AfterSuccessUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.Null(failedAfterSuccessUpdateObjectsFootprint);

            var failedAfterFailUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateObject.AfterFailUpdateObject)] as Tuple<ObjectStatus, IEnumerable<DataObject>>;
            Assert.NotNull(failedAfterFailUpdateObjectsFootprint);
            Assert.Equal(dataObject, failedAfterFailUpdateObjectsFootprint.Item2.Last());
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateObject.AfterFailUpdateObject));

            Assert.Equal(failedBeforeUpdateObjectsFootprint.Item1, failedAfterFailUpdateObjectsFootprint.Item1);
        }

        /// <summary>
        /// Check success footprint for <see cref="Mailman"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckSuccessUpdateProperty(IDataService dataService, Mailman dataObject)
        {
            var beforeUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.BeforeUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.NotNull(beforeUpdateObjectsFootprint);
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateProperty.BeforeUpdateProperty));

            var afterSuccessSqlUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.AfterSuccessSqlUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.NotNull(afterSuccessSqlUpdateObjectsFootprint);
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateProperty.AfterSuccessSqlUpdateProperty));

            var afterSuccessUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.AfterSuccessUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.NotNull(afterSuccessUpdateObjectsFootprint);
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateProperty.AfterSuccessUpdateProperty));

            var afterFailUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.AfterFailUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.Null(afterFailUpdateObjectsFootprint);

            Assert.Equal(beforeUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
            Assert.Equal(afterSuccessUpdateObjectsFootprint.Item1, afterSuccessSqlUpdateObjectsFootprint.Item1);
        }

        /// <summary>
        /// Check failed footprint for <see cref="Homer"/>.
        /// </summary>
        /// <param name="dataService">Data service for update operation.</param>
        /// <param name="dataObject">Data object for checking.</param>
        private void CheckFailUpdateProperty(IDataService dataService, Mailman dataObject)
        {
            var failedBeforeUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.BeforeUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.NotNull(failedBeforeUpdateObjectsFootprint);
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateProperty.BeforeUpdateProperty));

            var failedAfterSuccessSqlUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.AfterSuccessSqlUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.Null(failedAfterSuccessSqlUpdateObjectsFootprint);

            var failedAfterSuccessUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.AfterSuccessUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.Null(failedAfterSuccessUpdateObjectsFootprint);

            var failedAfterFailUpdateObjectsFootprint = dataObject.DynamicProperties[nameof(INotifyUpdateProperty.AfterFailUpdateProperty)] as Tuple<ObjectStatus, string, object, object>;
            Assert.NotNull(failedAfterFailUpdateObjectsFootprint);
            dataObject.DynamicProperties.Remove(nameof(INotifyUpdateProperty.AfterFailUpdateProperty));

            Assert.Equal(failedBeforeUpdateObjectsFootprint.Item1, failedAfterFailUpdateObjectsFootprint.Item1);
        }
    }
}
