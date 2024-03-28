namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Тесты для класса <see cref="DataObjectCache"/>.
    /// </summary>
    public class DataObjectCacheTest
    {
        private readonly ITestOutputHelper output;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="output">Поток вывода теста.</param>
        public DataObjectCacheTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// A test for DataObjectCache Constructor.
        /// </summary>
        [Fact]
        public void DataObjectCacheConstructorTest()
        {
            DataObjectCache target = new DataObjectCache();
            Assert.NotNull(target);
        }

        /// <summary>
        /// Проверка работы кэша объектов данных.
        /// </summary>
        [Fact]
        public void DataObjectCacheMainTest()
        {
            DataObjectCache cache = new DataObjectCache();
            cache.StartCaching(false);
            DataObjectForTest dobj = PrvCreateDataObject(cache);
            DataObjectForTest sdo = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), dobj.__PrimaryKey);
            cache.StopCaching();
            Assert.NotNull(sdo);
            output.WriteLine($"Getted from cache dataobject name = {sdo.Name}");
        }

        /// <summary>
        /// Проверка включения/отключения родительского кеширования.
        /// </summary>
        [Fact]
        public void DataObjectCacheParentTest()
        {
            DataObjectCache cache = new DataObjectCache();
            cache.StartCaching(false);
            DataObjectForTest dobj = PrvCreateDataObject(cache);
            cache.StartCaching(true);
            DataObjectForTest sdo = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), dobj.__PrimaryKey);
            cache.StopCaching();
            cache.StopCaching();
            Assert.Null(sdo);
            output.WriteLine("Null when ClipParentCahce = true");

            cache.StartCaching(false);
            DataObjectForTest dobj1 = PrvCreateDataObject(cache);
            cache.StartCaching(false);
            DataObjectForTest sdo1 = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), dobj1.__PrimaryKey);
            cache.StopCaching();
            cache.StopCaching();
            Assert.NotNull(sdo1);
            output.WriteLine($"Getted from cache dataobject name = {sdo1.Name}");

            // проверим что будет, если создадим объект в дочернем кэше - доступен ли он будет после его остановки?
            cache.StartCaching(false);
            cache.StartCaching(false);
            DataObjectForTest dobj2 = PrvCreateDataObject(cache);
            cache.StopCaching();
            DataObjectForTest sdo2 = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), dobj2.__PrimaryKey);
            cache.StopCaching();
            Assert.NotNull(sdo2);
            output.WriteLine("Объект создали в дочернем кеше, а читаем в родительском");
        }

        private DataObjectForTest PrvCreateDataObject(DataObjectCache cache)
        {
            DataObjectForTest sdo = new DataObjectForTest();
            sdo.Name = "Объект данных";
            output.WriteLine($"Created dataobject name = {sdo.Name}");
            cache.AddDataObject(sdo);
            return sdo;
        }

        /// <summary>
        /// Проверка работы кэша объектов данных.
        /// </summary>
        [Fact]
        public void DataObjectCacheCreatingTest()
        {
            DataObjectCache cache = new DataObjectCache();
            cache.StartCaching(false);
            DataObjectForTest dobj = PrvCreateDataObject(cache);
            DataObjectForTest sdo = (DataObjectForTest)cache.CreateDataObject(typeof(DataObjectForTest), dobj.__PrimaryKey);
            cache.StopCaching();
            Assert.NotNull(sdo);
            output.WriteLine($"Getted from cache dataobject name = {sdo.Name}");
        }

        /// <summary>
        /// Проблема: загрузка детейла приводит к вычитке агрегатора, в том числе с обновлением соседних детейлов. Нужно читать так чтобы агрегатор взялся из кэша, а не из базы.
        /// </summary>
        [Fact]
        public void DataObjectLoadWithCacheTest()
        {
        }

        /// <summary>
        /// A test for AddDataObject.
        /// </summary>
        [Fact(Skip = "A method that does not return a value cannot be verified.")]
        public void AddDataObjectTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            DataObject dobj = null; // TODO: Initialize to an appropriate value
            target.AddDataObject(dobj);
        }

        /// <summary>
        /// A test for ContextedLivingObjects.
        /// </summary>
        [Fact]
        // [DeploymentItem("ICSSoft.STORMNET.DataObject.dll")]
        public void ContextedLivingObjectsTest()
        {
            // DataObjectCache_Accessor target = new DataObjectCache_Accessor(); // TODO: Initialize to an appropriate value
            // SortedList expected = null; // TODO: Initialize to an appropriate value
            // SortedList actual;
            // actual = target.ContextedLivingObjects();
            // Assert.Equal(expected, actual);
            // Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for CreateDataObject.
        /// </summary>
        [Fact(Skip = "Verify the correctness of this test method.")]
        public void CreateDataObjectTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            Type typeofdataobject = null; // TODO: Initialize to an appropriate value
            object Key = null; // TODO: Initialize to an appropriate value
            DataObject expected = null; // TODO: Initialize to an appropriate value
            DataObject actual;
            actual = target.CreateDataObject(typeofdataobject, Key);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// A test for GetLivingDataObject.
        /// </summary>
        [Fact(Skip = "Verify the correctness of this test method.")]
        public void GetLivingDataObjectTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            Type typeofdataobject = null; // TODO: Initialize to an appropriate value
            object key = null; // TODO: Initialize to an appropriate value
            DataObject expected = null; // TODO: Initialize to an appropriate value
            DataObject actual;
            actual = target.GetLivingDataObject(typeofdataobject, key);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// A test for StartCaching.
        /// </summary>
        [Fact(Skip = "A method that does not return a value cannot be verified.")]
        public void StartCachingTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            bool ClipParentCache = false; // TODO: Initialize to an appropriate value
            target.StartCaching(ClipParentCache);
        }

        /// <summary>
        /// A test for StopCaching.
        /// </summary>
        [Fact(Skip = "A method that does not return a value cannot be verified.")]
        public void StopCachingTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            target.StopCaching();
        }

        /// <summary>
        /// A test for prvGetLivingDataObject.
        /// </summary>
        [Fact]
        // [DeploymentItem("ICSSoft.STORMNET.DataObject.dll")]
        public void prvGetLivingDataObjectTest()
        {
            // DataObjectCache_Accessor target = new DataObjectCache_Accessor(); // TODO: Initialize to an appropriate value
            // Type typeofdataobject = null; // TODO: Initialize to an appropriate value
            // object key = null; // TODO: Initialize to an appropriate value
            // ICSSoft.STORMNET.DataObject expected = null; // TODO: Initialize to an appropriate value
            // ICSSoft.STORMNET.DataObject actual;
            // actual = target.prvGetLivingDataObject(typeofdataobject, key);
            // Assert.Equal(expected, actual);
            // Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for prvRemoveLivingDataObject.
        /// </summary>
        [Fact]
        // [DeploymentItem("ICSSoft.STORMNET.DataObject.dll")]
        public void prvRemoveLivingDataObjectTest()
        {
            // DataObjectCache_Accessor target = new DataObjectCache_Accessor(); // TODO: Initialize to an appropriate value
            // Type typeofdataobject = null; // TODO: Initialize to an appropriate value
            // object key = null; // TODO: Initialize to an appropriate value
            // target.prvRemoveLivingDataObject(typeofdataobject, key);
            // Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        /// A test for Creator.
        /// </summary>
        [Fact(Skip = "Verify the correctness of this test method.")]
        public void CreatorTest()
        {
            ObjectCreator expected = null; // TODO: Initialize to an appropriate value
            ObjectCreator actual;
            DataObjectCache.Creator = expected;
            actual = DataObjectCache.Creator;
            Assert.Equal(expected, actual);
        }
    }
}
