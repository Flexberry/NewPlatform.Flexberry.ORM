namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using ICSSoft.STORMNET.Business;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="IReferencesNullDelete" />.
    /// </summary>
    public class IReferencesNullDeleteTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public IReferencesNullDeleteTests()
            : base("IRefNullDel")
        {
        }

        /// <summary>
        /// Выполняется проверка, что всем ссылающимся объектам будет поставлено <c>NULL</c> после удаления.
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
                    { SaladName = "OnlyWithDelete", Ingridient1 = plantToDelete, Ingridient2 = plantToNotDelete };
                var saladWithRef2 = new Salad2()
                    { SaladName = "WithOneDelete", Ingridient1 = plantToNotDelete, Ingridient2 = plantToNotDelete };
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
    }
}
