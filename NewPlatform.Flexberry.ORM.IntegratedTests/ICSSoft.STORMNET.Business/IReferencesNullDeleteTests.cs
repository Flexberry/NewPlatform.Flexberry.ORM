namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Exceptions;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />.
    /// </summary>
    public class IReferencesNullDeleteTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public IReferencesNullDeleteTests()
            : base("IRND")
        {
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />,
        /// всем ссылающимся объектам будет проставлено <c>NULL</c>.
        /// </summary>
        [Fact]
        public void TestIReferencesNullDelete()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var plantToDelete = new Plant2() { Name = "Delete" };
                var plantToNotDelete = new Plant2() { Name = "NotDelete" };
                var saladWithRef = new Salad2()
                { SaladName = "OnlyOneDelete", Ingridient1 = plantToDelete, Ingridient2 = plantToNotDelete };
                var saladWithRef2 = new Salad2()
                { SaladName = "WithNoDelete", Ingridient1 = plantToNotDelete, Ingridient2 = plantToNotDelete };
                var dishWithRef = new Dish2()
                { DishName = "OnlyWithDelete", MainIngridient = plantToDelete };
                var dishWithRef2 = new Dish2()
                { DishName = "WithNoDelete", MainIngridient = plantToNotDelete };
                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { plantToDelete, plantToNotDelete, saladWithRef, saladWithRef2, dishWithRef, dishWithRef2 };

                dataService.UpdateObjects(ref objsToUpdate);

                plantToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { plantToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var updatedSaladWithRef = new Salad2();
                updatedSaladWithRef.SetExistObjectPrimaryKey(saladWithRef.__PrimaryKey);
                ds.LoadObject(Salad2.Views.Salad2L, updatedSaladWithRef);
                Assert.Null(updatedSaladWithRef.Ingridient1);
                Assert.Equal(plantToNotDelete.__PrimaryKey, updatedSaladWithRef.Ingridient2.__PrimaryKey);

                var updatedSaladWithRef2 = new Salad2();
                updatedSaladWithRef2.SetExistObjectPrimaryKey(saladWithRef2.__PrimaryKey);
                ds.LoadObject(Salad2.Views.Salad2L, updatedSaladWithRef2);
                Assert.Equal(plantToNotDelete.__PrimaryKey, updatedSaladWithRef2.Ingridient1.__PrimaryKey);
                Assert.Equal(plantToNotDelete.__PrimaryKey, updatedSaladWithRef2.Ingridient2.__PrimaryKey);

                var updatedDishWithRef = new Dish2();
                updatedDishWithRef.SetExistObjectPrimaryKey(dishWithRef.__PrimaryKey);
                ds.LoadObject(Dish2.Views.Dish2E, updatedDishWithRef);
                Assert.Null(updatedDishWithRef.MainIngridient);

                var updatedDishWithRef2 = new Dish2();
                updatedDishWithRef2.SetExistObjectPrimaryKey(dishWithRef2.__PrimaryKey);
                ds.LoadObject(Dish2.Views.Dish2E, updatedDishWithRef2);
                Assert.Equal(plantToNotDelete.__PrimaryKey, updatedDishWithRef2.MainIngridient.__PrimaryKey);
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />,
        /// при попытке задать значение <c>NULL</c> ссылкам на удаляемый объект, которые не могут иметь значение <c>NULL</c>, бросается исключение <see cref="PropertyCouldnotBeNullException" />.
        /// </summary>
        [Fact]
        public void TestIReferencesNullDeleteWithException()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var plantToDelete = new Plant2() { Name = "Delete" };
                var saladWithRef = new Salad2()
                { SaladName = "ProblemDelete", Ingridient1 = plantToDelete, Ingridient2 = plantToDelete };
                var objsToUpdate = new ICSSoft.STORMNET.DataObject[] { plantToDelete, saladWithRef };

                dataService.UpdateObjects(ref objsToUpdate);

                plantToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { plantToDelete };

                // Act.
                var exception = Xunit.Record.Exception(() =>
                {
                    dataService.UpdateObjects(ref objsToUpdate);
                });

                // Assert.
                Assert.IsType<PropertyCouldnotBeNullException>(exception);
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, чей предок реализует интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />,
        /// всем ссылающимся объектам будет проставлено <c>NULL</c>.
        /// </summary>
        [Fact]
        public void TestParentWithIReferencesNullDelete()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var cabbageToDelete = new Cabbage2() { Name = "Delete" };
                var cabbageDetail = new CabbagePart2() { PartName = "Some part" };
                cabbageToDelete.CabbageParts = new DetailArrayOfCabbagePart2(cabbageToDelete);
                cabbageToDelete.CabbageParts.Add(cabbageDetail);
                var cabbageToNotDelete = new Cabbage2() { Name = "NotDelete" };
                var saladWithRef = new Salad2()
                { SaladName = "OnlyOneDelete", Ingridient1 = cabbageToDelete, Ingridient2 = cabbageToNotDelete };
                var saladWithRef2 = new Salad2()
                { SaladName = "WithNoDelete", Ingridient1 = cabbageToNotDelete, Ingridient2 = cabbageToNotDelete };
                var dishWithRef = new Dish2()
                { DishName = "OnlyWithDelete", MainIngridient = cabbageToDelete };
                var dishWithRef2 = new Dish2()
                { DishName = "WithNoDelete", MainIngridient = cabbageToNotDelete };
                var cabbageSaladWithRef = new CabbageSalad()
                { CabbageSaladName = "OnlyOneDelete", Cabbage1 = cabbageToDelete, Cabbage2 = cabbageToNotDelete };
                var cabbageSaladWithRef2 = new CabbageSalad()
                { CabbageSaladName = "WithNoDelete", Cabbage1 = cabbageToNotDelete, Cabbage2 = cabbageToNotDelete };
                var soupWithRef = new Soup2()
                { SoupName = "WithNoDelete", CabbageType = cabbageToNotDelete };

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
                    };

                dataService.UpdateObjects(ref objsToUpdate);

                cabbageToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { cabbageToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var updatedSaladWithRef = new Salad2();
                updatedSaladWithRef.SetExistObjectPrimaryKey(saladWithRef.__PrimaryKey);
                ds.LoadObject(Salad2.Views.Salad2L, updatedSaladWithRef);
                Assert.Null(updatedSaladWithRef.Ingridient1);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedSaladWithRef.Ingridient2.__PrimaryKey);

                var updatedSaladWithRef2 = new Salad2();
                updatedSaladWithRef2.SetExistObjectPrimaryKey(saladWithRef2.__PrimaryKey);
                ds.LoadObject(Salad2.Views.Salad2L, updatedSaladWithRef2);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedSaladWithRef2.Ingridient1.__PrimaryKey);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedSaladWithRef2.Ingridient2.__PrimaryKey);

                var updatedDishWithRef = new Dish2();
                updatedDishWithRef.SetExistObjectPrimaryKey(dishWithRef.__PrimaryKey);
                ds.LoadObject(Dish2.Views.Dish2E, updatedDishWithRef);
                Assert.Null(updatedDishWithRef.MainIngridient);

                var updatedDishWithRef2 = new Dish2();
                updatedDishWithRef2.SetExistObjectPrimaryKey(dishWithRef2.__PrimaryKey);
                ds.LoadObject(Dish2.Views.Dish2E, updatedDishWithRef2);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedDishWithRef2.MainIngridient.__PrimaryKey);

                var updatedCabbageSaladWithRef = new CabbageSalad();
                updatedCabbageSaladWithRef.SetExistObjectPrimaryKey(cabbageSaladWithRef.__PrimaryKey);
                ds.LoadObject(CabbageSalad.Views.CabbageSaladE, updatedCabbageSaladWithRef);
                Assert.Null(updatedCabbageSaladWithRef.Cabbage1);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedCabbageSaladWithRef.Cabbage2.__PrimaryKey);

                var updatedCabbageSaladWithRef2 = new CabbageSalad();
                updatedCabbageSaladWithRef2.SetExistObjectPrimaryKey(cabbageSaladWithRef2.__PrimaryKey);
                ds.LoadObject(CabbageSalad.Views.CabbageSaladE, updatedCabbageSaladWithRef2);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedCabbageSaladWithRef2.Cabbage2.__PrimaryKey);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedCabbageSaladWithRef2.Cabbage2.__PrimaryKey);

                var updatedSoupWithRef = new Soup2();
                updatedSoupWithRef.SetExistObjectPrimaryKey(soupWithRef.__PrimaryKey);
                ds.LoadObject(Soup2.Views.Soup2E, updatedSoupWithRef);
                Assert.Equal(cabbageToNotDelete.__PrimaryKey, updatedSoupWithRef.CabbageType.__PrimaryKey);
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, чей предок реализует интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />,
        /// при попытке задать значение <c>NULL</c> ссылкам на удаляемый объект, которые не могут иметь значение <c>NULL</c>, бросается исключение <see cref="PropertyCouldnotBeNullException" />.
        /// </summary>
        [Fact]
        public void TestParentWithIReferencesNullDeleteWithException()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var cabbageToDelete = new Cabbage2() { Name = "Delete" };
                var soupWithRef = new Soup2()
                { SoupName = "ProblemDelete", CabbageType = cabbageToDelete };
                var objsToUpdate = new ICSSoft.STORMNET.DataObject[] { cabbageToDelete, soupWithRef };

                dataService.UpdateObjects(ref objsToUpdate);

                cabbageToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { cabbageToDelete };

                // Act.
                var exception = Xunit.Record.Exception(() =>
                {
                    dataService.UpdateObjects(ref objsToUpdate);
                });

                // Assert.
                Assert.IsType<PropertyCouldnotBeNullException>(exception);
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />,
        /// всем ссылающимся объектам будет проставлено <c>NULL</c> с учётом существующей иерархии.
        /// </summary>
        [Fact]
        public void TestIReferencesNullDeleteWithHierarchy()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var objectToDelete = new HierarchyClassWithIRND() { Name = "Delete" };
                var detailForDelete = new DetailForIRND() { Name = "Delete" };
                objectToDelete.DetailForIRND = new DetailArrayOfDetailForIRND(objectToDelete);
                objectToDelete.DetailForIRND.Add(detailForDelete);

                var oneLevel = new HierarchyClassWithIRND() { Name = "OneLevel" };
                var detailOneLevel = new DetailForIRND() { Name = "OneLevelDetail" };
                oneLevel.DetailForIRND = new DetailArrayOfDetailForIRND(oneLevel);
                oneLevel.DetailForIRND.Add(detailOneLevel);
                oneLevel.Parent = objectToDelete;

                var objectWithReference = new ClassToTestIRND() { Name = "SomeName", CanBeNull = objectToDelete, CanNotBeNull = oneLevel };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { objectToDelete, detailForDelete, oneLevel, detailOneLevel, objectWithReference };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { objectToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var oneLevelUpdated = new HierarchyClassWithIRND();
                oneLevelUpdated.SetExistObjectPrimaryKey(oneLevel.__PrimaryKey);
                ds.LoadObject(HierarchyClassWithIRND.Views.HierarchyClassWithIRNDE, oneLevelUpdated);
                Assert.Null(oneLevelUpdated.Parent);
                Assert.Equal(1, oneLevelUpdated.DetailForIRND.Count);

                var objectWithReferenceUpdated = new ClassToTestIRND();
                objectWithReferenceUpdated.SetExistObjectPrimaryKey(objectWithReference.__PrimaryKey);
                ds.LoadObject(ClassToTestIRND.Views.ClassToTestIRNDE, objectWithReferenceUpdated);
                Assert.Null(objectWithReferenceUpdated.CanBeNull);
                Assert.Equal(oneLevel.__PrimaryKey, objectWithReferenceUpdated.CanNotBeNull.__PrimaryKey);
            }
        }

        /// <summary>
        /// При доработке сервиса данных для обработки интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" /> был изменён порядок формирования запросов.
        /// Данный тест на изменённый порядок запросов.
        /// </summary>
        [Fact(Skip = "Раскомментируйте этот тест после закрытия ошибки #204 (https://github.com/Flexberry/NewPlatform.Flexberry.ORM/issues/204).")]
        public void TestProperQueryOrder()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var objectToDelete = new HierarchyClassWithIRND() { Name = "Delete" };
                var detailForDelete = new DetailForIRND() { Name = "Delete" };
                objectToDelete.DetailForIRND = new DetailArrayOfDetailForIRND(objectToDelete);
                objectToDelete.DetailForIRND.Add(detailForDelete);

                var oneLevel = new HierarchyClassWithIRND() { Name = "OneLevel", Parent = objectToDelete };
                var detailOneLevel = new DetailForIRND() { Name = "OneLevelDetail" };
                oneLevel.DetailForIRND.Add(detailOneLevel);

                var objectWithReference = new ClassToTestIRND() { Name = "SomeName", CanBeNull = objectToDelete, CanNotBeNull = oneLevel };
                var freeObject = new HierarchyClassWithIRND() { Name = "Free object" };
                var objectWithoutReference = new ClassToTestIRND() { Name = "SomeName", CanNotBeNull = oneLevel };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { objectToDelete, detailForDelete, oneLevel, detailOneLevel, objectWithReference, freeObject, objectWithoutReference };

                dataService.UpdateObjects(ref objsToUpdate);

                objectToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                oneLevel.Parent = null;
                detailOneLevel.Name = "AnotherName";
                freeObject.Name = "Freedom";
                objectWithoutReference.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);

                /*
                 * Проблема в том, что если на одной таблице сразу есть операции помимо Delete, то сначала выполняется Delete, а потом Update и Create.
                 * Если удаляется запись, а потом обновляется имеющая на удаляемую ссылку, то это приводит к ошибке
                 * (для повтора проблемы на тип, который обновляется, так же должно быть Delete).
                 */
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { detailOneLevel, freeObject, oneLevel, objectWithoutReference, objectToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var oneLevelUpdated = new HierarchyClassWithIRND();
                oneLevelUpdated.SetExistObjectPrimaryKey(oneLevel.__PrimaryKey);
                ds.LoadObject(HierarchyClassWithIRND.Views.HierarchyClassWithIRNDE, oneLevelUpdated);
                Assert.Null(oneLevelUpdated.Parent);
                Assert.Equal(1, oneLevelUpdated.DetailForIRND.Count);

                var objectWithReferenceUpdated = new ClassToTestIRND();
                objectWithReferenceUpdated.SetExistObjectPrimaryKey(objectWithReference.__PrimaryKey);
                ds.LoadObject(ClassToTestIRND.Views.ClassToTestIRNDE, objectWithReferenceUpdated);
                Assert.Null(objectWithReferenceUpdated.CanBeNull);
                Assert.Equal(oneLevel.__PrimaryKey, objectWithReferenceUpdated.CanNotBeNull.__PrimaryKey);
            }
        }
    }
}
