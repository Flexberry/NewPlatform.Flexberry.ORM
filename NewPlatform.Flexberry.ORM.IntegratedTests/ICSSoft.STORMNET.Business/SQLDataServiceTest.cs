namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlTypes;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using ICSSoft.STORMNET.Exceptions;

    /// <summary>
    /// Тестовый класс для <see cref="SQLDataService"/>.
    /// </summary>

    public class SQLDataServiceTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SQLDataServiceTest()
            : base("SQLDS")
        {
        }

        /// <summary>
        /// Пробуем удалять данные каскадом: удалять мастер и забирать с ним все объекты, которые на него ссылаются.
        /// </summary>
        [Fact(Skip = "Раскомментируйте этот тест после закрытия ошибки \"Удаление объектов и их мастеров\" (номер 4010).")]
        public void CascadeDeleteTest()
        {
            // TODO: "Раскомментируйте этот тест после закрытия ошибки \"Удаление объектов и их мастеров\" (номер 4010)."

            foreach (IDataService dataService in DataServices)
            {
                var master = new SomeMasterClass { FieldA = "someFieldA_value" };

                var detail1 = new SomeDetailClass { FieldB = "SomeFieldB1", SomeMasterClass = master };

                var detail2 = new SomeDetailClass { FieldB = "SomeFieldB2", SomeMasterClass = master };

                var objsToUpdate = new DataObject[] { master, detail1, detail2 };

                dataService.UpdateObjects(ref objsToUpdate);

                // Попробуем удалить мастеровой объект
                master.SetStatus(ObjectStatus.Deleted);

                try
                {
                    dataService.UpdateObject(master);
                    Assert.True(false, "Assert.Fail");
                }
                catch (ExecutingQueryException)
                {
                    // Если провалился сюда - значит все хорошо
                }

                // Теперь попробуем удалить его и утащить за ним все данные в нем.
                // Примечание: логика написана в бизнес-сервере, который находится в файле SomeMasterClass.cs
                master.DynamicProperties.Add("DeleteAll", true);
                master.DynamicProperties.Add("dataService", dataService);
                try
                {
                    dataService.UpdateObject(master);
                }
                catch (ExecutingQueryException)
                {
                    // Почистим данные из базы.
                    master.SetStatus(ObjectStatus.Deleted);
                    detail1.SetStatus(ObjectStatus.Deleted);
                    detail2.SetStatus(ObjectStatus.Deleted);

                    var objectsToDelete = new DataObject[] {master, detail1, detail2};

                    dataService.UpdateObjects(ref objectsToDelete);

                    Assert.True(false, "Объекты не хотят удаляться при удалении мастера");
                }

                // После корректного срабатывания теста в базе не должно остаться ни одной записи.
                var detailCount =
                    ((SQLDataService) dataService).Query<SomeDetailClass>(SomeDetailClass.Views.ClassBE).Count();
                var masterCount =
                    ((SQLDataService) dataService).Query<SomeMasterClass>(SomeMasterClass.Views.ClassAE).Count();

                Assert.True(detailCount == 0);
                Assert.True(masterCount == 0);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки механизма порционного чтения через <see cref="SQLDataService.ReadFirst"/> и <see cref="SQLDataService.ReadNext"/>.
        /// Проверяет возможность чтения следующих порций данных после вызова <see cref="SQLDataService.ReadFirst"/> в том же соединении.
        /// </summary>
        [Fact]
        public void PortionReadingTest()
        {
            foreach (IDataService dataService in DataServices)
            {

                var ds = (SQLDataService) dataService;

                for (int i = 0; i < 21; i++)
                    ds.UpdateObject(new Кошка
                    {
                        ДатаРождения = (NullableDateTime) DateTime.Now,
                        Тип = ТипКошки.Дикая,
                        Порода = new Порода {Название = "Чеширская"},
                        Кличка = "Мурка" + i
                    });

                object state = null;
                Assert.NotNull(ds.ReadFirst("SELECT * FROM \"Кошка\"", ref state, 10));
                Assert.NotNull(ds.ReadNext(ref state, 10));
                Assert.NotNull(ds.ReadNext(ref state, 1));
                Assert.Null(ds.ReadNext(ref state, 1));
            }
        }

        /// <summary>
        /// Проверяем корректное означивание объекта, в котором есть особым образом сформированное вычислимое поле.
        /// Падало в классе <see cref="DynamicMethodCompiler"/>.
        /// </summary>
        [Fact]
        public void TestDynamicMethodCompiler()
        {
            foreach (IDataService dataService in DataServices)
            {
                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;
                // Arrange.
                var client = new Клиент();
                dataService.UpdateObject(client);
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Клиент), Клиент.Views.TestNotStoredGuid);
                lcs.ReturnTop = 1;

                // Act (выполнение не должно приводить к ошибкам). 
                DataObject[] result = dataService.LoadObjects(lcs);

                // Assert. 
                Assert.Equal(1, result.Length);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки метода <see cref="SQLDataService.GetObjectIndexesWithPks"/>.
        /// </summary>
        [Fact]
        public void GetObjectIndexesWithPksTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;
                var createdBear1 = new Медведь();
                createdBear1.ЦветГлаз = "Косолапый Мишка 1";
                ds.UpdateObject(createdBear1);

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof (Медведь), Медведь.Views.МедведьL);

                SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;

                Function findFunction = langDef.GetFunction(langDef.funcLike,
                    new VariableDef(langDef.StringType, Information.ExtractPropertyPath<Медведь>(m => m.ЦветГлаз)),
                    createdBear1.ЦветГлаз);

                // Act.
                IDictionary<int, string> result = ds.GetObjectIndexesWithPks(lcs, findFunction, 101);

                // Assert.
                Assert.Equal(1, result.Count);
                Assert.True(result[1].IndexOf("{") > -1);

                Console.WriteLine(result[1]);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки метода <see cref="SQLDataService.GetObjectIndexesWithPks"/> на выдачу детерменированного индекса.
        /// </summary>
        [Fact]
        public void GetObjectIndexesWithPksOrderingTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;
                SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;

                List<DataObject> dataObjects = new List<DataObject>();
                bool isMssql = dataService.GetType() == typeof(MSSQLDataService) || dataService.GetType().IsSubclassOf(typeof(MSSQLDataService));

                // MSSQL в отличие от других хранилищ имеет свой формат сортировки гуидов. Для его поддержки приходится пользоваться специальным типом SqlGuid.
                List<SqlGuid> keysMssqlList = new List<SqlGuid>();
                List<Guid> keysPostgresList = new List<Guid>();

                // Создадим 1000 медведей.
                int objectsCount = 1000;
                for (int i = 0; i < objectsCount; i++)
                {
                    // Для простоты анализа проблем с данными, если они возникнут, выдадим ненастоящие последовательные гуиды.
                    byte[] bytes = new byte[16];
                    BitConverter.GetBytes(i).CopyTo(bytes, 0);
                    var pk = new Guid(bytes);
                    if (isMssql)
                    {
                        keysMssqlList.Add(pk);
                    }
                    else
                    {
                        keysPostgresList.Add(pk);
                    }

                    var createdBear = new Медведь
                    {
                        __PrimaryKey = pk,
                        ЦветГлаз = "Косолапый Мишка " + i,
                        Вес = i
                    };
                    dataObjects.Add(createdBear);
                }

                DataObject[] dataObjectsForUpdate = dataObjects.ToArray();
                ds.UpdateObjects(ref dataObjectsForUpdate);

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), Медведь.Views.МедведьL);

                Function findFunction = langDef.GetFunction(langDef.funcL, new VariableDef(langDef.StringType, Information.ExtractPropertyPath<Медведь>(m => m.Вес)), objectsCount);
                if (isMssql)
                {
                    keysMssqlList.Sort();
                }
                else
                {
                    keysPostgresList.Sort();
                }

                // Act.
                IDictionary<int, string> result = ds.GetObjectIndexesWithPks(lcs, findFunction, null);

                // Assert.
                var values = result.Values.GetEnumerator();
                var keys = result.Keys.GetEnumerator();

                for (int i = 0; i < objectsCount; i++)
                {
                    Assert.Equal(objectsCount, result.Count);

                    values.MoveNext();
                    keys.MoveNext();
                    string key = isMssql ? ((Guid)keysMssqlList[i]).ToString("B") : keysPostgresList[i].ToString("B");
                    Assert.Equal(key, values.Current);
                    Assert.Equal(i + 1, keys.Current);
                }
            }
        }

        /// <summary>
        /// Проверка механизма удаления мастеров у детейлов в одной транзакции.
        /// </summary>
        [Fact]
        public void DetailsDeleteTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange
                SQLDataService ds = (SQLDataService) dataService;

                const string First = "Первый";
                const string Second = "Второй";
                const string Third = "Третий";
                const string Fourth = "Четвертый";

                ТипЛапы передняяЛапа = new ТипЛапы {Актуально = true, Название = "Передняя"};
                ТипЛапы задняяЛапа = new ТипЛапы {Актуально = true, Название = "Задняя"};


                Кошка aggregator = new Кошка
                {
                    ДатаРождения = (NullableDateTime) DateTime.Now,
                    Тип = ТипКошки.Дикая,
                    Порода = new Порода { Название = "Чеширская" },
                    Кличка = "Мурка"
                };
                aggregator.Лапа.AddRange(
                    new Лапа { Цвет = First, ТипЛапы = передняяЛапа },
                    new Лапа { Цвет = Second, ТипЛапы = передняяЛапа },
                    new Лапа { Цвет = Third, ТипЛапы = задняяЛапа },
                    new Лапа { Цвет = Fourth, ТипЛапы = задняяЛапа });

                ds.UpdateObject(aggregator);

                LoadingCustomizationStruct lcsCat = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.КошкаE);
                LoadingCustomizationStruct lcsPaws = LoadingCustomizationStruct.GetSimpleStruct(typeof(Лапа), Лапа.Views.ЛапаFull);
                LoadingCustomizationStruct lcsPawsType = LoadingCustomizationStruct.GetSimpleStruct(typeof(ТипЛапы), ТипЛапы.Views.ТипЛапыE);

                DataObject[] dataObjectsCats = ds.LoadObjects(lcsCat);
                DataObject[] dataObjectsPawsTypes = ds.LoadObjects(lcsPawsType);

                // Act
                int countCatBefore = ds.GetObjectsCount(lcsCat);
                int countPawsBefore = ds.GetObjectsCount(lcsPaws);
                int countPawsTypeBefore = ds.GetObjectsCount(lcsPawsType);

                List<DataObject> objectsForUpdateList = new List<DataObject>();

                foreach (Кошка кошка in dataObjectsCats)
                {
                    кошка.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(кошка);
                }

                foreach (ТипЛапы типЛапы in dataObjectsPawsTypes)
                {
                    типЛапы.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(типЛапы);
                }

                DataObject[] objectsForUpdate = objectsForUpdateList.ToArray();

                ds.UpdateObjects(ref objectsForUpdate);

                int countCatAfter = ds.GetObjectsCount(lcsCat);
                int countPawsAfter = ds.GetObjectsCount(lcsPaws);
                int countPawsTypeAfter = ds.GetObjectsCount(lcsPawsType);

                // Assert
                Assert.Equal(1, countCatBefore);
                Assert.Equal(4, countPawsBefore);
                Assert.Equal(2, countPawsTypeBefore);

                Assert.Equal(0, countCatAfter);
                Assert.Equal(0, countPawsAfter);
                Assert.Equal(0, countPawsTypeAfter);
            }
        }

        /// <summary>
        /// Тесты на формирование графа зависимостей методом <see cref="SQLDataService.GetDependencies"/>. 
        /// </summary>
        [Fact]
        public void GetDependenciesTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange
                SQLDataService ds = (SQLDataService) dataService;

                НаследникМ1 testDate = new НаследникМ1();
                testDate.Name = "test1";
                             
                TestClassA testDate2 = new TestClassA();
                testDate2.Мастер = testDate;
                testDate2.Name = "test2";

                var updateArray = new DataObject[]
                {
                     testDate2, testDate,
                };

                // Act.
                ds.UpdateObjects(ref updateArray);

                View view = new View();
                View view2 = new View();

                view.DefineClassType = typeof(НаследникМ1);
                view2.DefineClassType = typeof(TestClassA);

                view.AddProperty("Name");
                view2.AddProperty("Name");

                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof (НаследникМ1), view);
                var lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(TestClassA), view2);

                var resultDate = ds.LoadObjects(lcs).Cast<НаследникМ1>().Select(x => x.Name).ToList();      
                var resultDate2 = ds.LoadObjects(lcs2).Cast<TestClassA>().Select(x => x.Name).ToList();

                // Assert.
                Assert.Equal(testDate.Name, resultDate[0]);
                Assert.Equal(testDate2.Name, resultDate2[0]);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки удаления агрегатора без детейлов методом <see cref="SQLDataService.UpdateObjectsOrdered"/>.
        /// </summary>
        [Fact]
        public void DeleteAgregatorWithoutDetailsThroughUpdateObjectsOrderedMethodTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                SQLDataService ds = dataService as SQLDataService;

                // Создаем агрегатор без детейлов (медведя без берлог), но с бизнес-сервером привязанным к детейлам (к берлогам привязан бизнес-сервер).
                Медведь bear = new Медведь { ЦветГлаз = "Карие", Вес = 50 };
                DataObject[] dataObjectsForUpdate = new DataObject[]
                {
                    bear
                };

                // Сохраняем созданный агрегатор без детейлов.
                ds.UpdateObjects(ref dataObjectsForUpdate);

                // Помечаем сохраненный агрегатор на удаление.
                bear.SetStatus(ObjectStatus.Deleted);

                Exception updateException = null;
                try
                {
                    // Пытаемся удалить агрегатор через метод UpdateObjectsOrdered.
                    ds.UpdateObjectsOrdered(ref dataObjectsForUpdate);
                }
                catch(Exception ex)
                {
                    updateException = ex;
                }

                // Проверяем, что при удалении не возникло исключений.
                Assert.True(updateException==null, "При удалении через UpdateObjectsOrdered не возникло исключений");

                // Пытаемся загрузить удаленный объект из БД.
                Exception loadException = null;
                try
                {
                    Медведь loadedBear = new Медведь();
                    loadedBear.SetExistObjectPrimaryKey(bear.__PrimaryKey);
                    ds.LoadObject(loadedBear);
                }
                catch (Exception ex)
                {
                    loadException = ex;
                }

                // Проверяем, что удаленный объект действительно удалился.
                Assert.True(loadException is CantFindDataObjectException, "Объект удаленный через UpdateObjectsOrdered действительно больше не существует");
            }
        }

        /// <summary>
        /// Проверяется удаление детейлов 2 уровня.
        /// с помощью метода <see cref="SQLDataService.UpdateObject"/>.
        /// </summary>
        [Fact]
        public void DeleteCascadeDetailTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;

                Кошка кошка = new Кошка
                {
                    ДатаРождения = (NullableDateTime)DateTime.Now,
                    Тип = ТипКошки.Дикая,
                    Порода = new Порода { Название = "Чеширская"},
                    Кличка = "Мурка"
                };
                var лапа = new Лапа {Номер = 1};
                var перелом = new Перелом() { Тип = ТипПерелома.Закрытый, Дата = DateTime.Now};
                кошка.Лапа.Add(лапа);
                лапа.Перелом.Add(перелом);
                ds.UpdateObject(кошка);

                // Act & Assert.
                var aggregatorForUpdateActual =
                                    ds.Query<Кошка>(Кошка.Views.КошкаE)
                                        .First(x => x.__PrimaryKey == кошка.__PrimaryKey);
                aggregatorForUpdateActual.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(aggregatorForUpdateActual);

                var aggregatorForUpdateDeleted = ds.Query<Кошка>(Кошка.Views.КошкаE).FirstOrDefault(x => x.__PrimaryKey == кошка.__PrimaryKey);
                var detailDeleted = ds.Query<Перелом>(Перелом.Views.ПереломE).FirstOrDefault(x => x.__PrimaryKey == перелом.__PrimaryKey);

                Assert.Null(aggregatorForUpdateDeleted);
                Assert.Null(detailDeleted);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки порядка обновления и удаления циклически связанных объектов 
        /// с помощью метода <see cref="SQLDataService.UpdateObject"/>.
        /// </summary>
        [Fact]
        public void AggregatorWithLinkToDetailTest0()
        {
            foreach (IDataService ds in DataServices)
            {
                // Arrange.
                var aggregator = new AggregatorUpdateObjectTest { AggregatorName = "aggregatorName" };
                var detail = new DetailUpdateObjectTest { DetailName = "detailName" };
                var master = new MasterUpdateObjectTest { MasterName = "masterName", Detail = detail };
                aggregator.Details.Add(detail);
                aggregator.Masters.Add(master);
                ds.UpdateObject(aggregator);

                aggregator.Detail = detail;
                detail.Master = master;
                ds.UpdateObject(aggregator);

                // Act & Assert.
                var aggregatorActual = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .First(x => x.__PrimaryKey == aggregator.__PrimaryKey);

                Assert.NotNull(aggregatorActual);
                Assert.Equal(aggregator.__PrimaryKey, aggregatorActual.__PrimaryKey);
                Assert.Equal(aggregator.AggregatorName, aggregatorActual.AggregatorName);
                Assert.Equal(aggregator.Details.Count, aggregatorActual.Details.Count);
                Assert.Equal(aggregator.Masters.Count, aggregatorActual.Masters.Count);

                aggregatorActual.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(aggregatorActual);

                var aggregatorDeleted = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == aggregator.__PrimaryKey);
                var detailDeleted = ds.Query<DetailUpdateObjectTest>(DetailUpdateObjectTest.Views.DetailUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == detail.__PrimaryKey);
                var masterDeleted = ds.Query<MasterUpdateObjectTest>(MasterUpdateObjectTest.Views.MasterUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == detail.__PrimaryKey);

                Assert.Null(aggregatorDeleted);
                Assert.Null(detailDeleted);
                Assert.Null(masterDeleted);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки порядка обновления и удаления циклически связанных объектов 
        /// с помощью метода <see cref="SQLDataService.UpdateObject"/>.
        /// </summary>
        [Fact]
        public void AggregatorWithLinkToDetailTest1()
        {
            foreach (IDataService ds in DataServices)
            {
                // Arrange.
                var aggregator = new AggregatorUpdateObjectTest { AggregatorName = "aggregatorName" };
                var detail = new DetailUpdateObjectTest { DetailName = "detailName" };
                ds.UpdateObject(aggregator);

                // Act & Assert.
                aggregator = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .First(x => x.__PrimaryKey == aggregator.__PrimaryKey);

                aggregator.Details.Add(detail);
                aggregator.Detail = detail;
                ds.UpdateObject(aggregator);

                var master = new MasterUpdateObjectTest { MasterName = "masterName", Detail = detail };
                aggregator.Masters.Add(master);
                detail.Master = master;
                ds.UpdateObject(aggregator);

                var aggregatorActual = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .First(x => x.__PrimaryKey == aggregator.__PrimaryKey);

                Assert.NotNull(aggregatorActual);
                Assert.Equal(aggregator.__PrimaryKey, aggregatorActual.__PrimaryKey);
                Assert.Equal(aggregator.AggregatorName, aggregatorActual.AggregatorName);
                Assert.Equal(aggregator.Details.Count, aggregatorActual.Details.Count);
                Assert.Equal(aggregator.Masters.Count, aggregatorActual.Masters.Count);

                aggregatorActual.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(aggregatorActual);

                var aggregatorDeleted = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == aggregator.__PrimaryKey);
                var detailDeleted = ds.Query<DetailUpdateObjectTest>(DetailUpdateObjectTest.Views.DetailUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == detail.__PrimaryKey);
                var masterDeleted = ds.Query<MasterUpdateObjectTest>(MasterUpdateObjectTest.Views.MasterUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == detail.__PrimaryKey);

                Assert.Null(aggregatorDeleted);
                Assert.Null(detailDeleted);
                Assert.Null(masterDeleted);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки порядка обновления и удаления циклически связанных объектов 
        /// с помощью метода <see cref="SQLDataService.UpdateObject"/>.
        /// </summary>
        [Fact]
        public void AggregatorWithLinkToDetailTest2()
        {
            foreach (IDataService ds in DataServices)
            {
                // Arrange.
                var aggregator = new AggregatorUpdateObjectTest { AggregatorName = "aggregatorName" };
                ds.UpdateObject(aggregator);

                // Act & Assert.
                aggregator = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .First(x => x.__PrimaryKey == aggregator.__PrimaryKey);

                var detail0 = new DetailUpdateObjectTest { DetailName = "detailName0" };
                aggregator.Details.Add(detail0);
                aggregator.Detail = detail0;
                ds.UpdateObject(aggregator);

                var detail1 = new DetailUpdateObjectTest { DetailName = "detailName1" };
                aggregator.Details.Add(detail1);
                aggregator.Detail = detail1;
                var dojbs = new DataObject[] { detail1, detail0, aggregator };
                ds.UpdateObjects(ref dojbs);

                var aggregatorActual = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .First(x => x.__PrimaryKey == aggregator.__PrimaryKey);

                Assert.NotNull(aggregatorActual);
                Assert.Equal(aggregator.__PrimaryKey, aggregatorActual.__PrimaryKey);
                Assert.Equal(aggregator.AggregatorName, aggregatorActual.AggregatorName);
                Assert.Equal(aggregator.Details.Count, aggregatorActual.Details.Count);

                aggregatorActual.SetStatus(ObjectStatus.Deleted);
                ds.UpdateObject(aggregatorActual);

                var aggregatorDeleted = ds.Query<AggregatorUpdateObjectTest>(AggregatorUpdateObjectTest.Views.AggregatorUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == aggregator.__PrimaryKey);
                var detailDeleted = ds.Query<DetailUpdateObjectTest>(DetailUpdateObjectTest.Views.DetailUpdateObjectTestE)
                    .FirstOrDefault(x => x.__PrimaryKey == detail0.__PrimaryKey);

                Assert.Null(aggregatorDeleted);
                Assert.Null(detailDeleted);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки создания и удаления нескольких объектов связанных ассоциацией.
        /// </summary>
        [Fact]
        public void DeleteUpdateAssociationTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;

                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;
                var masterBreedType = new ТипПороды {Название = "тип породы1", ДатаРегистрации = DateTime.Now};
                var innerMasterBreed = new Порода { Название = "порода1", ТипПороды = masterBreedType};
                var innerMasterCat = new Кошка
                {
                    Кличка = "кошка",
                    ДатаРождения = (NullableDateTime)DateTime.Now,
                    Тип = ТипКошки.Дикая,
                    Порода = innerMasterBreed
                };
                var innerKitten = new Котенок { КличкаКотенка = "котеночек", Кошка = innerMasterCat };

                // Act
                ds.UpdateObject(innerKitten);

                LoadingCustomizationStruct lcsKitten = LoadingCustomizationStruct.GetSimpleStruct(typeof(Котенок), Котенок.Views.КотенокE);
                LoadingCustomizationStruct lcsCat = LoadingCustomizationStruct.GetSimpleStruct(typeof(Кошка), Кошка.Views.КошкаE);
                LoadingCustomizationStruct lcsBreed = LoadingCustomizationStruct.GetSimpleStruct(typeof(Порода), Порода.Views.ПородаE);
                LoadingCustomizationStruct lcsBreedType = LoadingCustomizationStruct.GetSimpleStruct(typeof(ТипПороды), ТипПороды.Views.ТипПородыE);

                DataObject[] dataObjectsKitten = ds.LoadObjects(lcsKitten);
                DataObject[] dataObjectsCats = ds.LoadObjects(lcsCat);
                DataObject[] dataObjectsBreed = ds.LoadObjects(lcsBreed);
                DataObject[] dataObjectsBreedTypes = ds.LoadObjects(lcsBreedType);

                int countKittenBefore = ds.GetObjectsCount(lcsKitten);
                int countCatBefore = ds.GetObjectsCount(lcsCat);
                int countBreedBefore = ds.GetObjectsCount(lcsBreed);
                int countBreedTypeBefore = ds.GetObjectsCount(lcsBreed);

                List<DataObject> objectsForUpdateList = new List<DataObject>();

                foreach (Котенок котенок in dataObjectsKitten)
                {
                    котенок.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(котенок);
                }

                foreach (Кошка кошка in dataObjectsCats)
                {
                    кошка.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(кошка);
                }

                foreach (Порода порода in dataObjectsBreed)
                {
                    порода.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(порода);
                }

                foreach (ТипПороды типПороды in dataObjectsBreedTypes)
                {
                    типПороды.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(типПороды);
                }

                DataObject[] objectsForUpdate = objectsForUpdateList.ToArray();

                ds.UpdateObjects(ref objectsForUpdate);

                int countKittenAfter = ds.GetObjectsCount(lcsKitten);
                int countCatAfter = ds.GetObjectsCount(lcsCat);
                int countBreedAfter = ds.GetObjectsCount(lcsBreed);
                int countBreedTypeAfter = ds.GetObjectsCount(lcsBreedType);

                // Assert
                Assert.Equal(1, countKittenBefore);
                Assert.Equal(1, countCatBefore);
                Assert.Equal(1, countBreedBefore);
                Assert.Equal(1, countBreedTypeBefore);

                Assert.Equal(0, countKittenAfter);
                Assert.Equal(0, countCatAfter);
                Assert.Equal(0, countBreedAfter);
                Assert.Equal(0, countBreedTypeAfter);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки создания, удаления и обновления агрегатора с иерархией и ассоциацией.
        /// </summary>
        [Fact]
        public void DeleteAggregatorWithHierarhiAndAssociationTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;

                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;

                var masterForest = new Лес { Название = "лес1" };
                var aggregatorBear = new Медведь { ПорядковыйНомер = 1, ЛесОбитания = masterForest };

                ds.UpdateObject(aggregatorBear);

                var aggregatorBearMother = new Медведь { ПорядковыйНомер = 2 };
                var aggregatorBearFather = new Медведь { ПорядковыйНомер = 2 };
                aggregatorBear.Мама = aggregatorBearMother;
                aggregatorBear.Папа = aggregatorBearFather;

                ds.UpdateObject(aggregatorBear);

                // Act & Assert.
                LoadingCustomizationStruct lcsBear = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), Медведь.Views.МедведьE);
                LoadingCustomizationStruct lcsForest = LoadingCustomizationStruct.GetSimpleStruct(typeof(Лес), Лес.Views.ЛесE);

                DataObject[] dataObjectsBear = ds.LoadObjects(lcsBear);
                DataObject[] dataObjectsForest = ds.LoadObjects(lcsForest);

                // Act
                int countBearBefore = ds.GetObjectsCount(lcsBear);
                int countForestBefore = ds.GetObjectsCount(lcsForest);

                List<DataObject> objectsForUpdateList = new List<DataObject>();

                foreach (Медведь медведь in dataObjectsBear)
                {
                    медведь.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(медведь);
                }

                foreach (Лес лес in dataObjectsForest)
                {
                    лес.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(лес);
                }

                DataObject[] objectsForUpdate = objectsForUpdateList.ToArray();

                ds.UpdateObjects(ref objectsForUpdate);

                int countBearAfter = ds.GetObjectsCount(lcsBear);
                int countForestAfter = ds.GetObjectsCount(lcsForest);

                // Assert
                Assert.Equal(3, countBearBefore);
                Assert.Equal(1, countForestBefore);

                Assert.Equal(0, countBearAfter);
                Assert.Equal(0, countForestAfter);
            }
        }

        /// <summary>
        /// Тестовый метод для проверки создания, обновления и удаления агрегатора с иерархией, детейлом и ассоциацией.
        /// </summary>
        [Fact]
        public void DeleteAggregatorWithDetailAndHierarhiTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;

                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;

                var masterForest = new Лес { Название = "лес1"};
                var detailDen = new Берлога {Наименование = "берлога1", ЛесРасположения = masterForest};
                var aggregatorBear = new Медведь { ПорядковыйНомер = 2, ЛесОбитания = masterForest };
                aggregatorBear.Берлога.Add(detailDen);

                ds.UpdateObject(aggregatorBear);

                var aggregatorBearMother = new Медведь {ПорядковыйНомер = 2};
                aggregatorBear.Мама = aggregatorBearMother;

                ds.UpdateObject(aggregatorBear);

                // Act & Assert.
                LoadingCustomizationStruct lcsBear = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), Медведь.Views.МедведьE);
                LoadingCustomizationStruct lcsForest = LoadingCustomizationStruct.GetSimpleStruct(typeof(Лес), Лес.Views.ЛесE);

                DataObject[] dataObjectsBear = ds.LoadObjects(lcsBear);
                DataObject[] dataObjectsForest = ds.LoadObjects(lcsForest);

                // Act
                int countBearBefore = ds.GetObjectsCount(lcsBear);
                int countForestBefore = ds.GetObjectsCount(lcsForest);

                List<DataObject> objectsForUpdateList = new List<DataObject>();

                foreach (Медведь медведь in dataObjectsBear)
                {
                    медведь.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(медведь);
                }

                foreach (Лес лес in dataObjectsForest)
                {
                    лес.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(лес);
                }

                DataObject[] objectsForUpdate = objectsForUpdateList.ToArray();

                ds.UpdateObjects(ref objectsForUpdate);

                int countBearAfter = ds.GetObjectsCount(lcsBear);
                int countForestAfter = ds.GetObjectsCount(lcsForest);

                // Assert
                Assert.Equal(2, countBearBefore);
                Assert.Equal(1, countForestBefore);

                Assert.Equal(0, countBearAfter);
                Assert.Equal(0, countForestAfter);
            }
        }

        /// <summary>
        /// Проверка создания и удаления агрегатора с несколькими детейлами разных типов.
        /// </summary>
        [Fact]
        public void DeleteAggregatorWithManyDetailsTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;

                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;

                var masterCl = new Пользователь { ФИО = "фио" };
                var aggregator = new Конкурс { Название = "название", Организатор = masterCl };

                var detailOne = new КритерийОценки { ПорядковыйНомер = 4 };
                var detailTwo = new ДокументацияККонкурсу { Файл = new WebFile() };
                aggregator.КритерииОценки.Add(detailOne);
                aggregator.Документы.Add(detailTwo);

                ds.UpdateObject(aggregator);

                // Act
                LoadingCustomizationStruct lcsAggregator = LoadingCustomizationStruct.GetSimpleStruct(typeof(Конкурс), Конкурс.Views.КонкурсE);
                LoadingCustomizationStruct lcsDetailOne = LoadingCustomizationStruct.GetSimpleStruct(typeof(КритерийОценки), КритерийОценки.Views.КритерийОценкиE);
                LoadingCustomizationStruct lcsDetailTwo = LoadingCustomizationStruct.GetSimpleStruct(typeof(ДокументацияККонкурсу), ДокументацияККонкурсу.Views.ДокументацияККонкурсуE);

                DataObject[] dataObjectsAggregator = ds.LoadObjects(lcsAggregator);
                DataObject[] dataObjectsDetailOne = ds.LoadObjects(lcsDetailOne);
                DataObject[] dataObjectsDetailTwo = ds.LoadObjects(lcsDetailTwo);

                int countAggregatorBefore = ds.GetObjectsCount(lcsAggregator);
                int countDetailOneBefore = ds.GetObjectsCount(lcsDetailOne);
                int countDetailTwoBefore = ds.GetObjectsCount(lcsDetailTwo);

                List<DataObject> objectsForUpdateList = new List<DataObject>();

                foreach (Конкурс конкурс in dataObjectsAggregator)
                {
                    конкурс.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(конкурс);
                }

                foreach (КритерийОценки критерий in dataObjectsDetailOne)
                {
                    критерий.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(критерий);
                }

                foreach (ДокументацияККонкурсу документ in dataObjectsDetailTwo)
                {
                    документ.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(документ);
                }

                DataObject[] objectsForUpdate = objectsForUpdateList.ToArray();

                ds.UpdateObjects(ref objectsForUpdate);

                int countAggregatorAfter = ds.GetObjectsCount(lcsAggregator);
                int countDetailOneAfter = ds.GetObjectsCount(lcsDetailOne);
                int countDetailTwoAfter = ds.GetObjectsCount(lcsDetailTwo);

                // Assert
                Assert.Equal(1, countAggregatorBefore);
                Assert.Equal(1, countDetailOneBefore);
                Assert.Equal(1, countDetailTwoBefore);

                Assert.Equal(0, countAggregatorAfter);
                Assert.Equal(0, countDetailOneAfter);
                Assert.Equal(0, countDetailTwoAfter);
            }
        }

        /// <summary>
        /// Проверка создания и удаления агрегатора с несколькими детейлами разных типов, связанных ассоциацией.
        /// </summary>
        [Fact]
        public void DeleteAggregatorWithManyDetailsAndAssociationTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = dataService as SQLDataService;

                //TODO: Fix OracleDataService error. 
                if (dataService is OracleDataService)
                    continue;

                var пользователь = new Пользователь { ФИО = "фио" };
                var критерийОценки = new КритерийОценки { ПорядковыйНомер = 4 };
                var конкурс =  new Конкурс { Название = "название", Организатор = пользователь };
                конкурс.КритерииОценки.Add(критерийОценки);
                var aggregatorIdea = new Идея{ Заголовок= "агрегатор1", Автор = пользователь, Конкурс = конкурс};
                aggregatorIdea.Файлы.Add(new ФайлИдеи { Владелец = пользователь});
                var masterAndDetail = new ЗначениеКритерия { Значение = "значение1", Критерий = критерийОценки};
                aggregatorIdea.ОценкиЭкспертов.Add( new ОценкаЭксперта {ЗначениеОценки = 2, ЗначениеКритерия = masterAndDetail, Эксперт = пользователь});
                aggregatorIdea.ЗначенияКритериев.Add(masterAndDetail);

                ds.UpdateObject(aggregatorIdea);

                // Act & Assert.
                LoadingCustomizationStruct lcsAggregator = LoadingCustomizationStruct.GetSimpleStruct(typeof(Идея), Идея.Views.ИдеяE);
                LoadingCustomizationStruct lcsDetail1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(ФайлИдеи), ФайлИдеи.Views.ФайлE);
                LoadingCustomizationStruct lcsDetail2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(ОценкаЭксперта), ОценкаЭксперта.Views.ОценкаЭкспертаE);
                LoadingCustomizationStruct lcsDetail3 = LoadingCustomizationStruct.GetSimpleStruct(typeof(ЗначениеКритерия), ЗначениеКритерия.Views.ЗначениеКритерияE);

                DataObject[] dataObjectsAggregator = ds.LoadObjects(lcsAggregator);
                DataObject[] dataObjectsDetail1 = ds.LoadObjects(lcsDetail1);
                DataObject[] dataObjectsDetail2 = ds.LoadObjects(lcsDetail2);
                DataObject[] dataObjectsDetail3 = ds.LoadObjects(lcsDetail3);

                // Act
                int countAggregatorBefore = ds.GetObjectsCount(lcsAggregator);
                int countDetail1Before = ds.GetObjectsCount(lcsDetail1);
                int countDetail2Before = ds.GetObjectsCount(lcsDetail2);
                int countDetail3Before = ds.GetObjectsCount(lcsDetail3);

                List<DataObject> objectsForUpdateList = new List<DataObject>();

                foreach (Идея идея in dataObjectsAggregator)
                {
                    идея.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(идея);
                }

                foreach (ФайлИдеи файл in dataObjectsDetail1)
                {
                    файл.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(файл);
                }

                foreach (ОценкаЭксперта оценка in dataObjectsDetail2)
                {
                    оценка.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(оценка);
                }

                foreach (ЗначениеКритерия критерий in dataObjectsDetail3)
                {
                    критерий.SetStatus(ObjectStatus.Deleted);
                    objectsForUpdateList.Add(критерий);
                }

                DataObject[] objectsForUpdate = objectsForUpdateList.ToArray();

                ds.UpdateObjects(ref objectsForUpdate);

                int countAggregatorAfter = ds.GetObjectsCount(lcsAggregator);
                int countDetail1After = ds.GetObjectsCount(lcsDetail1);
                int countDetail2After = ds.GetObjectsCount(lcsDetail2);
                int countDetail3After = ds.GetObjectsCount(lcsDetail3);

                // Assert
                Assert.Equal(1, countAggregatorBefore);
                Assert.Equal(1, countDetail1Before);
                Assert.Equal(1, countDetail2Before);
                Assert.Equal(1, countDetail3Before);

                Assert.Equal(0, countAggregatorAfter);
                Assert.Equal(0, countDetail1After);
                Assert.Equal(0, countDetail2After);
                Assert.Equal(0, countDetail3After);
            }
        }

        /// <summary>
        /// Тест для проверки установки строки соединения через свойство <see cref="SQLDataService.CustomizationStringName"/>.
        /// </summary>
        [Fact]
        public void CustomizationStringNameTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                string connectionStringName = "TestConnStr";
                string expectedResult = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
                SQLDataService ds = dataService as SQLDataService;
                if (ds == null)
                {
                    continue;
                }

                // Act.
                ds.CustomizationStringName = connectionStringName;
                string actualResult = dataService.CustomizationString;

                // Assert.
                Assert.Equal(expectedResult, actualResult);
            }
        }

        /// <summary>
        /// Тест для проверки наследуемых классов с одним хранилищем на уровне БД.
        /// </summary>
        [Fact]
        public void InheritedMasterClassTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var master = new InheritedMasterClass { StringMasterProperty = "prop", IntMasterProperty = 666 };
                master.DetailClass.Add(new DetailClass { Detailproperty = "detail" });

                // Act.
                dataService.UpdateObject(master);

                // Assert.
                Assert.Equal(1, master.DetailClass.Count);
            }
        }
    }
}