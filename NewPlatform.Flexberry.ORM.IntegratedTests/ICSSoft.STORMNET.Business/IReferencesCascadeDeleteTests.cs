namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System.Linq;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />.
    /// </summary>
    public class IReferencesCascadeDeleteTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public IReferencesCascadeDeleteTests()
            : base("IRCD")
        {
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены.
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDelete()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var territoryToDelete = new Territory2() { XCoordinate = 33 };
                var territoryToNotDelete = new Territory2() { XCoordinate = 42 };
                var saladWithRef = new Place2()
                    { PlaceName = "OnlyOneDelete", TodayTerritory = territoryToDelete, TomorrowTeritory = territoryToNotDelete };
                var saladWithRef2 = new Place2()
                    { PlaceName = "WithNoDelete", TodayTerritory = territoryToNotDelete, TomorrowTeritory = territoryToNotDelete };
                var dishWithRef = new Human2()
                    { HumanName = "OnlyWithDelete", TodayHome = territoryToDelete };
                var dishWithRef2 = new Human2()
                    { HumanName = "WithNoDelete", TodayHome = territoryToNotDelete };
                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { territoryToDelete, territoryToNotDelete, saladWithRef, saladWithRef2, dishWithRef, dishWithRef2 };

                dataService.UpdateObjects(ref objsToUpdate);

                territoryToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { territoryToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var territories = ds.Query<Territory2>(Territory2.Views.Territory2E).ToList();
                Assert.DoesNotContain(territories, x => x.__PrimaryKey.Equals(territoryToDelete.__PrimaryKey));
                Assert.Contains(territories, x => x.__PrimaryKey.Equals(territoryToNotDelete.__PrimaryKey));
                var places = ds.Query<Place2>(Place2.Views.Place2E).ToList();
                Assert.DoesNotContain(places, x => x.__PrimaryKey.Equals(saladWithRef.__PrimaryKey));
                Assert.Contains(places, x => x.__PrimaryKey.Equals(saladWithRef2.__PrimaryKey));
                var people = ds.Query<Human2>(Human2.Views.Human2E).ToList();
                Assert.DoesNotContain(people, x => x.__PrimaryKey.Equals(dishWithRef.__PrimaryKey));
                Assert.Contains(people, x => x.__PrimaryKey.Equals(dishWithRef2.__PrimaryKey));
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, чей предок реализует интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены.
        /// </summary>
        [Fact]
        public void TestParentWithIReferencesCascadeDelete()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var cabbageToDelete = new Country2() { CountryName = "Delete" };
                var cabbageDetail = new Region() { RegionName = "Some part" };
                cabbageToDelete.Region = new DetailArrayOfRegion(cabbageToDelete);
                cabbageToDelete.Region.Add(cabbageDetail);
                var cabbageToNotDelete = new Country2() { CountryName = "NotDelete" };

                var saladWithRef = new Place2()
                    { PlaceName = "OnlyOneDelete", TodayTerritory = cabbageToDelete, TomorrowTeritory = cabbageToNotDelete };
                var saladWithRef2 = new Place2()
                    { PlaceName = "WithNoDelete", TodayTerritory = cabbageToNotDelete, TomorrowTeritory = cabbageToNotDelete };
                var dishWithRef = new Human2()
                    { HumanName = "OnlyWithDelete", TodayHome = cabbageToDelete };
                var dishWithRef2 = new Human2()
                    { HumanName = "WithNoDelete", TodayHome = cabbageToNotDelete };

                var cabbageSaladWithRef = new Apparatus2()
                    { ApparatusName = "OnlyOneDelete", Exporter = cabbageToDelete, Maker = cabbageToNotDelete };
                var cabbageSaladWithRef2 = new Apparatus2()
                    { ApparatusName = "WithNoDelete", Exporter = cabbageToNotDelete, Maker = cabbageToNotDelete };
                var soupWithRef = new Adress2()
                    { HomeNumber = 33, Country = cabbageToDelete };
                var soupWithRef2 = new Adress2()
                    { HomeNumber = 42, Country = cabbageToNotDelete };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    {
                        cabbageToDelete,
                        cabbageDetail,
                        cabbageToNotDelete,
                        saladWithRef,
                        saladWithRef2,
                        dishWithRef,
                        dishWithRef2,
                        cabbageSaladWithRef,
                        cabbageSaladWithRef2,
                        soupWithRef,
                        soupWithRef2,
                    };

                dataService.UpdateObjects(ref objsToUpdate);

                cabbageToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { cabbageToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var countries = ds.Query<Country2>(Country2.Views.Country2E).ToList();
                Assert.DoesNotContain(countries, x => x.__PrimaryKey.Equals(cabbageToDelete.__PrimaryKey));
                Assert.Contains(countries, x => x.__PrimaryKey.Equals(cabbageToNotDelete.__PrimaryKey));
                var regions = ds.Query<Region>(Region.Views.RegionE).ToList();
                Assert.Empty(regions);
                var places = ds.Query<Place2>(Place2.Views.Place2E).ToList();
                Assert.DoesNotContain(places, x => x.__PrimaryKey.Equals(saladWithRef.__PrimaryKey));
                Assert.Contains(places, x => x.__PrimaryKey.Equals(saladWithRef2.__PrimaryKey));
                var people = ds.Query<Human2>(Human2.Views.Human2E).ToList();
                Assert.DoesNotContain(people, x => x.__PrimaryKey.Equals(dishWithRef.__PrimaryKey));
                Assert.Contains(people, x => x.__PrimaryKey.Equals(dishWithRef2.__PrimaryKey));
                var apparatuses = ds.Query<Apparatus2>(Apparatus2.Views.Apparatus2E).ToList();
                Assert.DoesNotContain(apparatuses, x => x.__PrimaryKey.Equals(cabbageSaladWithRef.__PrimaryKey));
                Assert.Contains(apparatuses, x => x.__PrimaryKey.Equals(cabbageSaladWithRef2.__PrimaryKey));
                var adresses = ds.Query<Adress2>(Adress2.Views.Adress2E).ToList();
                Assert.DoesNotContain(adresses, x => x.__PrimaryKey.Equals(soupWithRef.__PrimaryKey));
                Assert.Contains(adresses, x => x.__PrimaryKey.Equals(soupWithRef2.__PrimaryKey));
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены с учётом существующей иерархии (у одного из удаляемых по каскаду объекта одна ссылки на удаляемый объект).
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDeleteWithHierarchy()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var objectToDelete = new HierarchyClassWithIRCD() { Name = "Delete" };
                var detailForDelete = new DetailForIRCD() { Name = "Delete" };
                objectToDelete.DetailForIRCD = new DetailArrayOfDetailForIRCD(objectToDelete);
                objectToDelete.DetailForIRCD.Add(detailForDelete);

                var oneLevel = new HierarchyClassWithIRCD() { Name = "OneLevel" };
                var detailOneLevel = new DetailForIRCD() { Name = "OneLevelDetail" };
                oneLevel.DetailForIRCD = new DetailArrayOfDetailForIRCD(oneLevel);
                oneLevel.DetailForIRCD.Add(detailOneLevel);
                oneLevel.Parent = objectToDelete;

                var freeObject = new HierarchyClassWithIRCD() { Name = "NotDelete" };

                var objectWithReference = new ClassToTestIRCD() { Name = "SomeName", CanBeNull = objectToDelete, CanNotBeNull = freeObject };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { objectToDelete, detailForDelete, oneLevel, detailOneLevel, freeObject, objectWithReference };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { objectToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var hierarchyClasses = ds.Query<HierarchyClassWithIRCD>(HierarchyClassWithIRCD.Views.HierarchyClassWithIRCDE).ToList();
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(objectToDelete.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(oneLevel.__PrimaryKey));
                Assert.Contains(hierarchyClasses, x => x.__PrimaryKey.Equals(freeObject.__PrimaryKey));

                var classTos = ds.Query<ClassToTestIRCD>(ClassToTestIRCD.Views.ClassToTestIRCDE).ToList();
                Assert.DoesNotContain(classTos, x => x.__PrimaryKey.Equals(objectWithReference.__PrimaryKey));
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены с учётом существующей иерархии (у одного из удаляемых по каскаду объекта сразу 2 ссылки на удаляемый объект).
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDeleteWithHierarchy2()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var objectToDelete = new HierarchyClassWithIRCD() { Name = "Delete" };
                var detailForDelete = new DetailForIRCD() { Name = "Delete" };
                objectToDelete.DetailForIRCD = new DetailArrayOfDetailForIRCD(objectToDelete);
                objectToDelete.DetailForIRCD.Add(detailForDelete);

                var oneLevel = new HierarchyClassWithIRCD() { Name = "OneLevel" };
                var detailOneLevel = new DetailForIRCD() { Name = "OneLevelDetail" };
                oneLevel.DetailForIRCD = new DetailArrayOfDetailForIRCD(oneLevel);
                oneLevel.DetailForIRCD.Add(detailOneLevel);
                oneLevel.Parent = objectToDelete;

                var objectWithReference = new ClassToTestIRCD() { Name = "SomeName", CanBeNull = objectToDelete, CanNotBeNull = objectToDelete };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { objectToDelete, detailForDelete, oneLevel, detailOneLevel, objectWithReference };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { objectToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var hierarchyClasses = ds.Query<HierarchyClassWithIRCD>(HierarchyClassWithIRCD.Views.HierarchyClassWithIRCDE).ToList();
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(objectToDelete.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(oneLevel.__PrimaryKey));

                var classTos = ds.Query<ClassToTestIRCD>(ClassToTestIRCD.Views.ClassToTestIRCDE).ToList();
                Assert.DoesNotContain(classTos, x => x.__PrimaryKey.Equals(objectWithReference.__PrimaryKey));
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены с учётом существующей иерархии (у одного из удаляемых по каскаду объекта одна ссылка на сам удаляемый и одна ссылка на удаляемый по каскаду).
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDeleteWithHierarchy3()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var objectToDelete = new HierarchyClassWithIRCD() { Name = "Delete" };
                var detailForDelete = new DetailForIRCD() { Name = "Delete" };
                objectToDelete.DetailForIRCD = new DetailArrayOfDetailForIRCD(objectToDelete);
                objectToDelete.DetailForIRCD.Add(detailForDelete);

                var oneLevel = new HierarchyClassWithIRCD() { Name = "OneLevel" };
                var detailOneLevel = new DetailForIRCD() { Name = "OneLevelDetail" };
                oneLevel.DetailForIRCD = new DetailArrayOfDetailForIRCD(oneLevel);
                oneLevel.DetailForIRCD.Add(detailOneLevel);
                oneLevel.Parent = objectToDelete;

                var objectWithReference = new ClassToTestIRCD() { Name = "SomeName", CanBeNull = objectToDelete, CanNotBeNull = oneLevel };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { objectToDelete, detailForDelete, oneLevel, detailOneLevel, objectWithReference };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { objectToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var hierarchyClasses = ds.Query<HierarchyClassWithIRCD>(HierarchyClassWithIRCD.Views.HierarchyClassWithIRCDE).ToList();
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(objectToDelete.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(oneLevel.__PrimaryKey));

                var classTos = ds.Query<ClassToTestIRCD>(ClassToTestIRCD.Views.ClassToTestIRCDE).ToList();
                Assert.DoesNotContain(classTos, x => x.__PrimaryKey.Equals(objectWithReference.__PrimaryKey));
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// будет проброшено исключение, если каскадом будет удалён объект, на которого остались обязательные ссылки.
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDeleteWithException()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var objectToDelete = new HierarchyClassWithIRCD() { Name = "Delete" };
                var oneLevel = new HierarchyClassWithIRCD() { Name = "OneLevel" };
                oneLevel.Parent = objectToDelete;
                var objectWithReference = new ClassToTestIRCD() { Name = "SomeName", CanNotBeNull = oneLevel };

                var objectForStructure1 = new HierarchyClassWithIRND() { Name = "SomeName" };
                var objectForStructure2 = new ClassToTestIRND() { Name = "SomeName", CanNotBeNull = objectForStructure1 };

                var masterWithProblems = new ClassWithMaster() { Name = "Problem", First = objectForStructure2, Second = objectWithReference };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { objectToDelete, oneLevel, objectWithReference, objectForStructure1, objectForStructure2, masterWithProblems };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { objectToDelete };

                // Act.
                var exception = Xunit.Record.Exception(() =>
                {
                    dataService.UpdateObjects(ref objsToUpdate);
                });

                // Assert.
                Assert.IsType<ExecutingQueryException>(exception);
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены с учётом существующей многоуровневой иерархии.
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDeleteWithDeepHierarchy()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var zeroLevel = new HierarchyClassWithIRCD() { Name = "Level0" };
                var objectToDelete = new HierarchyClassWithIRCD() { Name = "Delete", Parent = zeroLevel };
                var level1 = new HierarchyClassWithIRCD() { Name = "Level1", Parent = objectToDelete };
                var level2 = new HierarchyClassWithIRCD() { Name = "Level2", Parent = level1 };
                var level3 = new HierarchyClassWithIRCD() { Name = "Level3", Parent = level2 };
                var level4 = new HierarchyClassWithIRCD() { Name = "Level4", Parent = level3 };
                var level5 = new HierarchyClassWithIRCD() { Name = "Level5", Parent = level4 };
                var level6 = new HierarchyClassWithIRCD() { Name = "Level6", Parent = level5 };
                var level7 = new HierarchyClassWithIRCD() { Name = "Level7", Parent = level6 };
                var level8 = new HierarchyClassWithIRCD() { Name = "Level8", Parent = level7 };
                var level9 = new HierarchyClassWithIRCD() { Name = "Level9", Parent = level8 };
                var level10 = new HierarchyClassWithIRCD() { Name = "Level10", Parent = level1 };

                var objectWithReference1 = new ClassToTestIRCD() { Name = "SomeName", CanBeNull = level1, CanNotBeNull = level9 };
                var objectWithReference2 = new ClassToTestIRCD() { Name = "SomeName", CanBeNull = level2, CanNotBeNull = level10 };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    {
                        zeroLevel,
                        objectToDelete,
                        level1,
                        level2,
                        level3,
                        level4,
                        level5,
                        level6,
                        level7,
                        level8,
                        level9,
                        level10,
                        objectWithReference1,
                        objectWithReference2,
                    };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { objectToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var hierarchyClasses = ds.Query<HierarchyClassWithIRCD>(HierarchyClassWithIRCD.Views.HierarchyClassWithIRCDE).ToList();
                Assert.Contains(hierarchyClasses, x => x.__PrimaryKey.Equals(zeroLevel.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(objectToDelete.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level1.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level2.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level3.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level4.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level5.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level6.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level7.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level8.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level9.__PrimaryKey));
                Assert.DoesNotContain(hierarchyClasses, x => x.__PrimaryKey.Equals(level10.__PrimaryKey));

                var classTos = ds.Query<ClassToTestIRCD>(ClassToTestIRCD.Views.ClassToTestIRCDE).ToList();
                Assert.DoesNotContain(classTos, x => x.__PrimaryKey.Equals(objectWithReference1.__PrimaryKey));
                Assert.DoesNotContain(classTos, x => x.__PrimaryKey.Equals(objectWithReference2.__PrimaryKey));
            }
        }
    }
}
