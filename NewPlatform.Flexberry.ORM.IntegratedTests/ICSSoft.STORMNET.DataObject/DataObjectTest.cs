namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using System;
    using System.Diagnostics;
    using ICSSoft.STORMNET.Exceptions;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тестовый класс для DataObject.
    /// </summary>
    public class DataObjectTest
    {
        /// <summary>
        /// Тест метода <see cref="GetAlteredPropertyNames()"/>, который возвращает список свойств (атрибутов, мастеров, детейлов),
        /// чьи значения изменились по сравнению с внутренней копией.
        /// </summary>
        [Fact]

        public void IsAlteredPropertyTest()
        {
            var target = CreateDataObject(); // TODO: Initialize to an appropriate value
            var propName = "Name"; // TODO: Initialize to an appropriate value
            var expected = false; // TODO: Initialize to an appropriate value
            var actual = target.IsAlteredProperty(propName);
            Assert.Equal(expected, actual);

            actual = target.IsAlteredProperty("Height");
            Assert.True(actual);
            Debug.WriteLine(string.Join(Environment.NewLine, target.GetAlteredPropertyNames()));
        }

        /// <summary>
        /// Тест копирования объектов без применения кэширования.
        /// </summary>
        [Fact]

        public void CopyToObjectWithoutCacheTest1()
        {
            // Входной параметр: новый объект класса DataObjectForTest().
            var source = (DataObjectForTest)CreateDataObject();
            STORMNET.DataObject target = null;

            // Ожидаемый результат: копия объекта класса DataObjectForTest().
            source.CopyToObjectWithoutCache(ref target, true, true);

            Assert.Equal(source.Name, ((DataObjectForTest)target).Name);

            Assert.Equal(source.Height, ((DataObjectForTest)target).Height);

            Debug.WriteLine(source.Name);
        }

        /// <summary>
        /// Тест метода  <see cref="CopyTo()"/>, Создающего копию этого объекта данных.
        /// </summary>
        [Fact]

        public void CopyToObjectDataCopyTest()
        {
            // Входные параметры: два объекта класса DataObjectForTest.
            var source = (DataObjectForTest)CreateDataObject();
            STORMNET.DataObject target = new DataObjectForTest();

            // Ожидаемый результат: В объект target копируется объект source.
            source.CopyTo(target, true, true, true);
            target.InitDataCopy();
            Assert.Equal(((DataObjectForTest)source.GetDataCopy()).Name, ((DataObjectForTest)target.GetDataCopy()).Name);

            ((DataObjectForTest)source.GetDataCopy()).Name = "Коля";
            Assert.NotEqual(((DataObjectForTest)source.GetDataCopy()).Name, ((DataObjectForTest)target.GetDataCopy()).Name);
            Console.WriteLine(((DataObjectForTest)source.GetDataCopy()).Name + " " + ((DataObjectForTest)target.GetDataCopy()).Name);
        }

        /// <summary>
        /// Проверка переключения статусов из статуса Created.
        /// </summary>
        [Fact]

        public void SetStatusFromCreatedTest()
        {
            // Входной параметр: новый объкт класса DataObjectForTest.
            var source = new DataObjectForTest();
            source.SetStatus(ObjectStatus.Altered);

            // Ожидаемый результат: изменение статуса объекта на Created.
            Assert.Equal(source.GetStatus(), ObjectStatus.Created);
            source.SetStatus(ObjectStatus.UnAltered);
            Assert.Equal(source.GetStatus(), ObjectStatus.Created);
            source.SetStatus(ObjectStatus.Created);
            Assert.Equal(source.GetStatus(), ObjectStatus.Created);
            source.SetStatus(ObjectStatus.Deleted);

            // Ожидаемый результат: изменение статуса объекта на Deleted.
            Assert.Equal(source.GetStatus(), ObjectStatus.Deleted);
        }

        /// <summary>
        /// Проверяем делегат на PresentationValue.
        /// </summary>
        [Fact]

        public void GetPresentationValueDelegateTest()
        {
            const string Expected = "MySuperPuperPresentationValue";

            // Установим делегат для предоставления презентационного значения для объекта
            STORMNET.DataObject.GetPresentationValueDelegate = dataObject => Expected;

            // Вытащим презентационное значение для любого объекта
            var actual = new DataObjectForTest().GetPresentationValue();

            Assert.Equal(Expected, actual);

            // Вернем на исходную делегат для предоставления презентационного значения для объекта
            STORMNET.DataObject.GetPresentationValueDelegate = null;
        }

        /// <summary>
        /// Тест метода <see cref="CopyToObjectWithoutCache()"/>, Копирующего объекты без применения кэширования.
        /// </summary>
        [Fact]

        public void CopyToObjectWithoutCacheTest2()
        {
            // Входной параметр: новый объект классса clb.
            var b = new clb();
            b.ref1 = new cla { info = "1" };
            b.ref2 = new cla { info = "2" };
            b.ref2.__PrimaryKey = b.ref1.__PrimaryKey;
            b.ref1.parent = b;
            b.ref2.parent = b;
            STORMNET.DataObject dst = null;
            b.CopyToObjectWithoutCache(ref dst, true, true);

            // Ожидаемый результат: эквивалентность исходного ископированного объекьтов.
            Assert.Equal(((clb)dst).ref1.info, "1");
            Assert.Equal(((clb)dst).ref2.info, "2");
        }

        /// <summary>
        /// Тест метода <see cref="CopyToObjectWithoutCache()"/>, Копирующего объекты без применения кэширования.
        /// </summary>
        [Fact]

        public void CopyToObjectWithoutCacheTest3()
        {
            // Входные параметры: новые объекты классов cla и clb.
            var b = new cla();
            var a = new clb();
            b.info = "tada";
            a.ref2 = b;
            a.ref1 = b;

            STORMNET.DataObject copy = null;
            a.CopyToObjectWithoutCache(ref copy, true, true);

            // Ожидаемый результат: эквивалентность свойства info у исходных ископированных объекьтов.
            Assert.Equal("tada", ((clb)copy).ref1.info);
            Assert.Equal("tada", ((clb)copy).ref2.info);
        }

        /// <summary>
        /// Тест метода <see cref="CopyToObjectWithoutCache()"/>, Копирующего объекты без применения кэширования.
        /// </summary>
        [Fact]

        public void CopyToObjectWithoutCacheTest4()
        {
            // Входные параметры: новые объекты классов cla и clb.
            var b = new cla();
            var b1 = new cla();
            var a = new clb();
            b.info = "tada";
            b1.info = "tada";
            a.ref2 = b;
            a.ref1 = b1;

            STORMNET.DataObject copy = null;
            a.CopyToObjectWithoutCache(ref copy, true, true);

            // Ожидаемый результат: эквивалентность свойства info у исходных ископированных объекьтов.
            Assert.Equal("tada", ((clb)copy).ref1.info);
            Assert.Equal("tada", ((clb)copy).ref2.info);
        }

        /// <summary>
        /// Проверка переключения статусов из статуса Created.
        /// </summary>
        [Fact]

        public void DynamicPropertiesTest()
        {
            // Входные параметры: новый объект типа DataObjectForTest, новый гуид.
            var dObj = new DataObjectForTest();
            var g = new Guid("{C2EE01FC-1664-4005-AADB-968809E32E33}");
            const string Key = "myProp";
            dObj.DynamicProperties[Key] = g;
            object dp = dObj.DynamicProperties[Key];
            Assert.Equal(g, dp);
            dObj.InitDataCopy();
            object dp1 = dObj.DynamicProperties[Key];
            Assert.Equal(g, dp1);
        }

        /// <summary>
        /// Тест функции <see cref="GetPresentationValue()"/>, получающей презентационное значение для объекта.
        /// </summary>
        [Fact]

        public void GetPresentationValueTest()
        {
            // Входной параметр: новый объект типа DataObjectForTest.
            var dObj = new DataObjectForTest();

            // Ожидаемый результат: строка "DataObjectForTest(Gender=True, Height=0)"
            Assert.Equal(null, dObj.GetPresentationValue());

            // Входной параметр: новый объект типа DataObjectForTest со значением свойств: Name="Поросенок Пётр", Gender=false.
            var dObj1 = new DataObjectForTest { Name = "Поросенок Пётр", Gender = false };

            // Ожидаемый результат: "Поросенок Пётр"
            Assert.Equal("Поросенок Пётр", dObj1.GetPresentationValue());
        }

        /// <summary>
        /// Тест метода <see cref="LockObject()"/>, блокирующего объект.
        /// </summary>
        [Fact]

        public void LockObjectTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Входной параметр: новый объект класса DataObjec4Test.
                var dObj = new DataObjectForTest();

                // Ожидаемый результат: объект переведён в заблокирогванное состояние.
                dObj.LockObject(1);
                Assert.True(dObj.IsReadOnly);

                // Вызываем LockObject второй раз, но уже с другим ключом для того, чтобы выпало исключение.
                dObj.LockObject(2);
            });
            Assert.IsType(typeof(DataObjectIsReadOnlyException), exception);
        }

        /// <summary>
        /// Тест проверки что объект залочен.
        /// </summary>
        [Fact]

        public void IsReadOnlyTest()
        {
            // Входной параметр: новый объект класса DataObjec4Test.
            var dObj = new DataObjectForTest();
            dObj.LockObject(null);

            // Ожидаемый результат: объект не залочен.
            Assert.False(dObj.IsReadOnly);
            dObj.LockObject(666);

            // Ожидаемый результат: объект залочен.
            Assert.True(dObj.IsReadOnly);
        }

        /// <summary>
        /// Тест метода <see cref="UnLockObject()"/>, разблокирующего объект.
        /// </summary>
        [Fact]

        public void UnLockObjectTest()
        {
            // Входной параметр: новый объект класса DataObjec4Test.
            var dObj = new DataObjectForTest();
            dObj.LockObject(666);
            Assert.True(dObj.IsReadOnly);

            // Ожидаемый результат: объект сначала блокируется, потом блокировка снимается.
            dObj.UnLockObject(666);
            Assert.False(dObj.IsReadOnly);
        }

        /// <summary>
        /// Тест выпадения UnlockObjectDifferentKeyException в методе UnlockObject()
        /// при несовпадении ключей, использовавшихся при установке и снятии блокировки.
        /// </summary>
        [Fact]

        public void UnlockObjectDifferentKeyExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var dObj = new DataObjectForTest();
                dObj.LockObject(666);
                dObj.UnLockObject(6);
            });
            Assert.IsType(typeof(UnlockObjectDifferentKeyException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="DataObject.GetInitializedProperties()"/>, получающий проинициализированные свойства, собственные и мастеровые (загруженные+означенные).
        /// </summary>
        [Fact]

        public void GetInitializedPropertiesTest()
        {
            var dObj = new DataObjectForTest { Name = "Вася" };
            string[] expected =
            {
                Information.ExtractPropertyPath<DataObjectForTest>(x => x.Name),
                Information.ExtractPropertyPath<DataObjectForTest>(x => x.Height),
                Information.ExtractPropertyPath<DataObjectForTest>(x => x.BirthDate),
                Information.ExtractPropertyPath<DataObjectForTest>(x => x.Gender),
                Information.ExtractPropertyPath<DataObjectForTest>(x => x.__PrimaryKey),
            };

            string[] actual = dObj.GetInitializedProperties();
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));

            var obj = new ClassWithCaptions();
            string[] expected1 =
            {
                Information.ExtractPropertyPath<ClassWithCaptions>(x => x.InformationTestClass4),
                Information.ExtractPropertyPath<ClassWithCaptions>(x => x.__PrimaryKey),
            };

            string[] actual1 = obj.GetInitializedProperties();
            Assert.True(EquivalenceMethods.EqualStringArrays(expected1, actual1));
        }

        /// <summary>
        /// Тест метода <see cref="GetInitializedProperties(bool)"/>, получающий проинициализированные свойства (загруженные+означенные).
        /// </summary>
        [Fact]

        public void GetInitializedPropertiesWithMasterTest()
        {
            // Входной параметр: объект класса DataObjectForTest.
            var dObj = new DataObjectForTest();
            var actual = dObj.GetInitializedProperties(true);

            // Ожидаемый результат: массив строк {"Name", "Height", "BirthDate", "Gender", "_PrimaryKey"}.
            var expected = new string[] { "Name", "Height", "BirthDate", "Gender", "__PrimaryKey" };
            Assert.True(EquivalenceMethods.EqualStringArrays(actual, expected));
            var actual1 = dObj.GetInitializedProperties(false);
            Assert.True(EquivalenceMethods.EqualStringArrays(actual1, expected));
        }

        /// <summary>
        /// Тест метода <see cref="CopySysProps()"/>, копирующего только системные свойства ("primaryKey", "prototypeKey", "readKey", "CheckDetail", "state", "DisabledInitDataCopy").
        /// </summary>
        [Fact]

        public void CopySysPropsTest()
        {
            // Входной параметр: новый объект класса DataObjectForTest().
            var dObj = new DataObjectForTest { Name = "Вася", __PrimaryKey = "C2EE01FC-1664-4005-AADB-968809E32E33" };
            var result = new DataObjectForTest();
            dObj.CopySysProps(result);
            result.GetInitializedProperties();

            // Ожидаемый результат: значения поля __PrimaryKey у исходного и скопированого объектов совпадают, а значения поля Name - нет.
            Assert.Equal(dObj.__PrimaryKey, result.__PrimaryKey);
            Assert.NotEqual(dObj.Name, result.Name);
        }

        /// <summary>
        /// Тест выпадения ArgumentException в методе CopySysProps(),
        ///  если не указан объект данных для копирования.
        /// </summary>
        [Fact]

        public void CopySysPropsArgumentExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var dObj = new DataObjectForTest();
                dObj.CopySysProps(null);
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="ClearPrototyping()"/>, сбрасывающего прототипизацию объекта.
        /// </summary>
        [Fact]

        public void ClearPrototypingTest()
        {
            // Входные параметры: два объекта класса DataObjectForTest.
            var dObj = new DataObjectForTest();
            var result = new DataObjectForTest();
            dObj.Prototyping();
            dObj.CopySysProps(result);
            dObj.ClearPrototyping();

            // Ожидаемый результат: значение свойства __PrototypeKey у исходного объекта со снятой прототипизацией и копии исходного объекта не эквивалентны.
            Assert.NotEqual(dObj.__PrototypeKey, result.__PrototypeKey);
        }

        /// <summary>
        /// Тест метода <see cref="DisableInitDataCopy()"/>, отключающего инициализацию копии данных объекта при вычитке.
        /// </summary>
        [Fact]

        public void DisableInitDataCopyTest()
        {
            // Входной параметр: новый объект класса DataObjectForTest().
            var dObj = new DataObjectForTest();
            var result = new DataObjectForTest();
            dObj.CopyTo(result, true, true, true);
            dObj.DisableInitDataCopy();

            // Ожидаемый результат: несмотря на отключение инициализации копии данных у исходного объкекта, данные у исходного объекта и его копии эквивалентны.
            Assert.Equal(dObj.GetDataCopy(), result.GetDataCopy());
        }

        /// <summary>
        /// Тест метода <see cref="EnableInitDataCopy()"/>, включающего инициализацию копии данных объекта при вычитке.
        /// </summary>
        [Fact]

        public void EnableInitDataCopyTest()
        {
            // Входной параметр: новый объект класса DataObjectForTest().
            var dObj = new DataObjectForTest();
            dObj.DisableInitDataCopy();
            var result = new DataObjectForTest();
            dObj.CopyTo(result, true, true, true);
            dObj.EnableInitDataCopy();

            // Ожидаемый результат: несмотря на отключение, а заьеи включение инициализации копии данных у исходного объкекта, данные у исходного объекта и его копии эквивалентны.
            Assert.Equal(dObj.GetDataCopy(), result.GetDataCopy());
        }

        /// <summary>
        /// Тест метода <see cref="FullClearDataCopy()"/>, очищающего внутренней копии данных в собственном объекте, а также рекурсивно копии мастеровых и детейловых объектов.
        /// </summary>
        [Fact]

        public void FullClearDataCopyTest()
        {
            // Входные параметры: объекты данных классов MasterClass и DetailClass.
            var obj = new MasterClass();
            var detailobj = new DetailClass();
            obj.StringMasterProperty = "StringMasterProperty";
            obj.DetailClass.Add(detailobj);
            obj.IntMasterProperty = 666;
            detailobj.Detailproperty = "DetailProperty";
            obj.FullClearDataCopy();

            // Ожидаемый результат: после очистки, всех данные равны null.
            Assert.Null(obj.GetDataCopy());
            Assert.Null(detailobj.GetDataCopy());
        }

        /// <summary>
        /// Тест метода <see cref="SetDataCopy()"/>, Устанавливающго внутреннюю копию объекта данных.
        /// </summary>
        [Fact]

        public void SetDataCopyTest()
        {
            // Входной параметр: новый объект класса DataObjectForTest.
            var obj = new DataObjectForTest();
            var obj1 = new DataObjectForTest();
            obj.Name = "Семён";
            obj.SetDataCopy(obj1);

            // Ожидаемый результат: подтверждение наличия внутренней копии у исходгного объекта данных.
            Assert.NotNull(obj.GetDataCopy());
        }

        /// <summary>
        /// Тест метода <see cref="CheckNotNullProperties(Dictionary detailSkip)"/>, ищущего незаполненные поля.
        /// </summary>
        [Fact]

        public void CheckNotNullPropertiesTest()
        {
            // Входной параметр: новый объект класса MasterClass.
            var obj = new MasterClass();

            // Ожидаемый результат: пустой массив строк.
            var expected = new string[0];
            string[] actual = obj.CheckNotNullProperties();
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));
            obj.SetStatus(ObjectStatus.Deleted);
            actual = obj.CheckNotNullProperties();
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="CheckNotNullProperties(View view, bool returnCaptions, Dictionary detailSkip)"/>,
        /// ищущего незаполненные поля и возвращающего заголовки по представлению.
        /// </summary>
        [Fact]

        public void CheckNotNullPropertiesTest1()
        {
            // Входной параметр: новый объект класса MasterClass.
            var obj = new MasterClass();

            // Ожидаемый результат: пустой массив строк.
            var expected = new string[0];
            string[] actual = obj.CheckNotNullProperties(
                Information.GetView("MasterClassE", typeof(MasterClass)), false, null);
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));
            obj.SetStatus(ObjectStatus.Deleted);
            actual = obj.CheckNotNullProperties();
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="CheckNotNullProperties(View view, bool returnCaptions)"/>,
        /// ищущего незаполненные поля и возвращающего заголовки по представлению.
        /// </summary>
        [Fact]

        public void CheckNotNullPropertiesTest2()
        {
            // Входной параметр: новый объект класса MasterClass.
            var obj = new MasterClass();
            string[] actual = obj.CheckNotNullProperties(Information.GetView("MasterClassE", typeof(MasterClass)), false);

            // Ожидаемый результат: пустой массив строк.
            Assert.True(EquivalenceMethods.EqualStringArrays(new string[0], actual));
        }

        /// <summary>
        /// Тест метода <see cref="Insert()"/>, вставляющего объект на определённое место.
        /// </summary>
        [Fact]

        public void InsertTest()
        {
            // Строки, содержащие значения свойств Detailproperty детейловых объектов из массива детейловых объектов до и после вставки.
            var expected = string.Empty;
            var actual = string.Empty;

            // Входной параметр: новый объект класса MasterClass.
            var obj = new MasterClass();

            // Массив детейловых объектов.
            var dArr = new DetailArrayOfDetailClass(obj);

            // Детейловые объекты.
            var дObj1 = new DetailClass() { Detailproperty = "первый" };
            var дObj2 = new DetailClass() { Detailproperty = "второй" };
            var objectForInsert = new DetailClass { Detailproperty = "влез без очереди" };
            dArr.Insert(0, дObj1);
            dArr.Insert(1, дObj2);
            for (var i = 0; i < 2; i++)
            {
                expected += dArr[i].Detailproperty;
            }

            dArr.Insert(1, objectForInsert);
            for (var i = 0; i < 2; i++)
            {
                actual += dArr[i].Detailproperty;
            }

            // Ожидаемый результат: строки со значениями Детейлproperty различны.
            Assert.NotEqual(expected, actual);
        }

        /// <summary>
        /// Тест метода <see cref="Move()"/>, перемещающего объект внутри массива.
        /// </summary>
        [Fact]

        public void MoveTest()
        {
            // Строки, содержащие значения свойств Detailproperty детейловых объектов из массива детейловых объектов до и после перемещения.
            var expected = string.Empty;
            var actual = string.Empty;

            // Входной параметр: новый объект класса MasterClass.
            var obj = new MasterClass();

            // Массив детейловых объектов.
            var dArr = new DetailArrayOfDetailClass(obj);

            // Детейловые объекты.
            var дObj1 = new DetailClass() { Detailproperty = "первый" };
            var дObj2 = new DetailClass() { Detailproperty = "второй" };
            dArr.Insert(0, дObj1);
            dArr.Insert(1, дObj2);
            for (var i = 0; i < 2; i++)
            {
                expected += dArr[i].Detailproperty;
            }

            dArr.Move(0, 1);
            for (var i = 0; i < 2; i++)
            {
                actual += dArr[i].Detailproperty;
            }

            // Ожидаемый результат: строки со значениями Детейлproperty различны.
            Assert.NotEqual(expected, actual);
        }

        /// <summary>
        /// Тест выпадения ArgumentOutOfRangeException в методе Insert, при указании некорректного индекса.
        /// </summary>
        [Fact]

        public void InsertArgumentOutOfRangeExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var obj = new MasterClass();

                // Массив детейловых объектов.
                var dArr = new DetailArrayOfDetailClass(obj);

                // Детейловый объект.
                var дObj1 = new DetailClass() { Detailproperty = "первый" };
                dArr.Insert(-1, дObj1);
            });
            Assert.IsType(typeof(ArgumentOutOfRangeException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения в методе Insert при добавлении объекта равного null.
        /// </summary>
        [Fact]

        public void InsertNullReferenceExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var master = new MasterClass();
                var arra = new DetailArrayOfDetailClass(master);
                arra.Insert(0, null);
            });
            Assert.IsType(typeof(NullReferenceException), exception);
        }

        /// <summary>
        /// Тест создания нового экземпляра DetailArray.
        /// </summary>
        [Fact]

        public void DetailArrayTest()
        {
            var myArray = new DetailArray();
            Assert.NotNull(myArray);
        }

        /// <summary>
        /// Тест выпадения исключения при попытке создания объекта данных с первичным ключом равным null.
        /// </summary>
        [Fact]

        public void PrimaryKeyTypeExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                new DetailClass() { __PrimaryKey = null };
            });
            Assert.IsType(typeof(PrimaryKeyTypeException), exception);
        }

        /// <summary>
        /// Тест выпадения DetailArrayAlreadyContainsObjectWithThatKeyException при попытке добавить в DetailArray два одинаковых объекта.
        /// </summary>
        [Fact]

        public void AddObjectDetailArrayAlreadyContainsObjectWithThatKeyExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var arrayOfDetailClass = new DetailArrayOfDetailClass(new MasterClass());
                var detail = new DetailClass();
                arrayOfDetailClass.Add(detail);
                arrayOfDetailClass.Add(detail);
            });
            Assert.IsType(typeof(DetailArrayAlreadyContainsObjectWithThatKeyException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="SetStatus()"/>, устанавливающего статус объекта данных.
        /// </summary>
        [Fact]

        public void SetStatusTest()
        {
            // Входной параметр: новый объект класса DataObjectForTest.
            var obj = new DataObjectForTest();
            obj.SetLoadingState(LoadingState.NotLoaded);
            obj.SetStatus(ObjectStatus.Deleted);
            obj.SetStatus(ObjectStatus.Altered);
            obj.SetStatus(ObjectStatus.Deleted);
            obj.SetStatus(ObjectStatus.UnAltered);

            // Ожидаемый результат: статус объекта = Created.
            Assert.True(obj.GetStatus() == ObjectStatus.Created);
        }

        /// <summary>
        /// Тест метода <see cref="SetLoadingState()"/>, устанавливающего состояние загрузки.
        /// </summary>
        [Fact]

        public void SetLoadingStateTest()
        {
            // Входной параметр: новый объект класса DataObjectForTest.
            var obj = new DataObjectForTest();
            obj.SetLoadingState(LoadingState.Loaded);
            obj.SetLoadingState(LoadingState.NotLoaded);

            // Ожидаемый результат: сосотяние загрузки объекта = NotLoaded.
            Assert.True(obj.GetLoadingState() == LoadingState.NotLoaded);
        }

        /// <summary>
        /// Тест выпадения NullReferenceException в том случае, если список добавляемых свойств равен null.
        /// </summary>
        [Fact]

        public void AddLoadedPropertiesNullReferenceExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Входной параметр: новый объект класса DataObjectForTest.
                var obj = new DataObjectForTest();
                obj.AddLoadedProperties((string[])null);
            });
            Assert.IsType(typeof(NullReferenceException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="Prototyping()"/>, Прототипизирующего объект.
        /// </summary>
        [Fact]

        public void ProtoypingTest()
        {
            // Входной параметр: новый объект класса MasterClass.
            var obj = new MasterClass();
            obj.Prototyping(true);
            Assert.True(obj.Prototyped);
        }

        /// <summary>
        /// Тест метода <see cref="InitDataCopy()"/>, инициализирующего копию данных.
        /// </summary>
        [Fact]

        public void InitDataCopyTest()
        {
            try
            {
                var obj = new DataObjectForTest();
                obj.DisableInitDataCopy();
                obj.InitDataCopy();
                obj.GetDataCopy();
            }
            catch
            {
                Assert.True(false, "Assert.Fail");
            }

            // Ожидаемый результат: отсутствие исключений, возникающих при использовании метода InitDataCopy().
        }

        /// <summary>
        /// Тест метода <see cref="ToString()"/>, переводящего DataObject в строковое представление.
        /// </summary>
        [Fact]

        public void ToStringTest()
        {
            // Входной параметр: объект класса DataObjectForTest со свойствами Name = Кеша, Gender = false, Height = 170.
            var obj = new DataObjectForTest { Name = "Кеша", Gender = false, Height = 170 };
            string actual = obj.ToString();

            // Ожидаемый результат: строка "DataObjectForTest(Name=Кеша, Gender=False, Height=170)"
            Assert.Equal("DataObjectForTest(Gender=False, Height=170, Name=Кеша)", actual);

            // Добавляем объекту динамичемкие свойства.
            obj.DynamicProperties.Add("property1", new DataObjectForTest());
            obj.DynamicProperties.Add("IntMasterProperty", "some string");

            // Ожидаемый результат: строка "DataObjectForTest(Name=Кеша, Gender=False, Height=170,property1={ef52b97-dd64-4cf2-a5a8-2aed7a074420}, property3=somestring)"
            string actual1 = obj.ToString();

            // Однакоко строки не эквиваленты засчёт того, что свойство property1 формируется динамически.
            Assert.NotEqual("DataObjectForTest(Name=Кеша, Gender=False, Height=170,property1={ef52b97-dd64-4cf2-a5a8-2aed7a074420}, property3=somestring)", actual1);

            // Берём метод ToString с параметрами Name, Height.
            string actual2 = obj.ToString(new[] { "Name", "Height" });

            // Ожидаемый результат: строка "DataObjectForTest(Name=Кеша, Height=170)"
            Assert.Equal("DataObjectForTest(Height=170, Name=Кеша)", actual2);
        }

        /// <summary>
        /// Создаётся объект DataObjectForTest, вправляется ему новый гуид, Name присваивается в Вася, выполняется InitDataCopy и присваивается Height = 200 (Статус UnAltered).
        /// </summary>
        /// <returns>
        /// DataObjectForTest с новым гуидом, именем "Вася", ростом = 200.
        /// </returns>
        internal virtual STORMNET.DataObject CreateDataObject()
        {
            // TODO: Instantiate an appropriate concrete class.
            var target = new DataObjectForTest();
            target.SetExistObjectPrimaryKey(Guid.NewGuid());

            target.Name = "Вася";

            target.InitDataCopy();

            target.Height = 200;

            return target;
        }
    }
}
