namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using ICSSoft.STORMNET;
    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="DataObjectCache"/>.
    /// </summary>
    public class DataObjectCacheTest
    {
        /// <summary>
        /// A test for DataObjectCache Constructor.
        /// </summary>
        [Fact]
        public void DataObjectCacheConstructorTest()
        {
            DataObjectCache target = new DataObjectCache();
            Assert.NotNull(target);
        }

        private  DataObjectCache cache = new DataObjectCache();

        /// <summary>
        /// Проверка работы кэша объектов данных.
        /// </summary>
        [Fact]
        public void DataObjectCacheMainTest()
        {
            cache.StartCaching(false);
            object pkey = PrvCreateDataObject();
            DataObjectForTest sdo = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), pkey);
            cache.StopCaching();
            Assert.NotNull(sdo);
            Console.WriteLine(String.Format("Getted from cache dataobject name = {0}", sdo.Name));

        }

        /// <summary>
        /// Проверка включения/отключения родительского кеширования.
        /// </summary>
        [Fact]
        public void DataObjectCacheParentTest()
        {
            cache.StartCaching(false);
            object pkey = PrvCreateDataObject();
            cache.StartCaching(true);
            DataObjectForTest sdo = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), pkey);
            cache.StopCaching();
            cache.StopCaching();
            Assert.Null(sdo);
            Console.WriteLine(String.Format("Null when ClipParentCahce = true"));

            cache.StartCaching(false);
            object pkey1 = PrvCreateDataObject();
            cache.StartCaching(false);
            DataObjectForTest sdo1 = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), pkey1);
            cache.StopCaching();
            cache.StopCaching();
            Assert.NotNull(sdo1);
            Console.WriteLine(String.Format("Getted from cache dataobject name = {0}", sdo1.Name));

            //проверим что будет, если создадим объект в дочернем кэше - доступен ли он будет после его остановки?
            cache.StartCaching(false);
            cache.StartCaching(false);
            object pkey2 = PrvCreateDataObject();
            cache.StopCaching();
            DataObjectForTest sdo2 = (DataObjectForTest)cache.GetLivingDataObject(typeof(DataObjectForTest), pkey2);
            cache.StopCaching();
            Assert.NotNull(sdo2);
            Console.WriteLine(String.Format("Объект создали в дочернем кеше, а читаем в родительском"));

        }


        private object PrvCreateDataObject()
        {
            DataObjectForTest sdo = new DataObjectForTest();
            sdo.Name = "Объект данных";
            Console.WriteLine(String.Format("Created dataobject name = {0}", sdo.Name));
            cache.AddDataObject(sdo);
            return sdo.__PrimaryKey;
        }

        /// <summary>
        /// Проверка работы кэша объектов данных
        /// </summary>
        [Fact]
        public void DataObjectCacheCreatingTest()
        {
            cache.StartCaching(false);
            object pkey = PrvCreateDataObject();
            DataObjectForTest sdo = (DataObjectForTest)cache.CreateDataObject(typeof(DataObjectForTest), pkey);
            cache.StopCaching();
            Assert.NotNull(sdo);
            Console.WriteLine(String.Format("Getted from cache dataobject name = {0}", sdo.Name));

        }

        /// <summary>
        /// Проблема: загрузка детейла приводит к зачитке агрегатора, в том числе с обновлением соседних детейлов. Нужно читать так чтобы агрегатор взялся из кэша, а не из базы.
        /// </summary>
        [Fact]
        public void DataObjectLoadWithCacheTest()
        {

        }


        /// <summary>
        ///A test for AddDataObject
        ///</summary>
        [Fact(Skip = "A method that does not return a value cannot be verified.")]
        public void AddDataObjectTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            DataObject dobj = null; // TODO: Initialize to an appropriate value
            target.AddDataObject(dobj);
        }


        /// <summary>
        ///A test for ContextedLivingObjects
        ///</summary>
        [Fact]
        //[DeploymentItem("ICSSoft.STORMNET.DataObject.dll")]
        public void ContextedLivingObjectsTest()
        {
            //DataObjectCache_Accessor target = new DataObjectCache_Accessor(); // TODO: Initialize to an appropriate value
            //SortedList expected = null; // TODO: Initialize to an appropriate value
            //SortedList actual;
            //actual = target.ContextedLivingObjects();
            //Assert.Equal(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateDataObject
        ///</summary>
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
        ///A test for GetLivingDataObject
        ///</summary>
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
        ///A test for StartCaching
        ///</summary>
        [Fact(Skip = "A method that does not return a value cannot be verified.")]
        public void StartCachingTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            bool ClipParentCache = false; // TODO: Initialize to an appropriate value
            target.StartCaching(ClipParentCache);
        }

        /// <summary>
        ///A test for StopCaching
        ///</summary>
        [Fact(Skip = "A method that does not return a value cannot be verified.")]
        public void StopCachingTest()
        {
            DataObjectCache target = new DataObjectCache(); // TODO: Initialize to an appropriate value
            target.StopCaching();
        }

        /// <summary>
        ///A test for prvGetLivingDataObject
        ///</summary>
        [Fact]
        //[DeploymentItem("ICSSoft.STORMNET.DataObject.dll")]
        public void prvGetLivingDataObjectTest()
        {
            //DataObjectCache_Accessor target = new DataObjectCache_Accessor(); // TODO: Initialize to an appropriate value
            //Type typeofdataobject = null; // TODO: Initialize to an appropriate value
            //object key = null; // TODO: Initialize to an appropriate value
            //ICSSoft.STORMNET.DataObject expected = null; // TODO: Initialize to an appropriate value
            //ICSSoft.STORMNET.DataObject actual;
            //actual = target.prvGetLivingDataObject(typeofdataobject, key);
            //Assert.Equal(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for prvRemoveLivingDataObject
        ///</summary>
        [Fact]
        //[DeploymentItem("ICSSoft.STORMNET.DataObject.dll")]
        public void prvRemoveLivingDataObjectTest()
        {
            //DataObjectCache_Accessor target = new DataObjectCache_Accessor(); // TODO: Initialize to an appropriate value
            //Type typeofdataobject = null; // TODO: Initialize to an appropriate value
            //object key = null; // TODO: Initialize to an appropriate value
            //target.prvRemoveLivingDataObject(typeofdataobject, key);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Creator
        ///</summary>
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
