namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;

    using Xunit;

    /// <summary>
    /// Тесты метода <see cref="Utils.FillRowSetToDataObject" />.
    /// </summary>
    public class FillRowSetToDataObjectTests
    {
        /// <summary>
        /// Тест проверяет статус загрузки мастерового объекта.
        /// </summary>
        [Fact]
        public void TestAlteredMasterAfterLoading()
        {
            // Arrange.
            var sm = new EmptySecurityManager();
            var viewBear = new View { DefineClassType = typeof(Медведь) };
            viewBear.AddProperties(
                Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Название));
            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), viewBear);

            var guidForest = new Guid("a1a31995-4322-47a8-8128-da251826b958");
            var guidBear = new Guid("89136856-459e-418d-a3be-547a60192517");
            var bear = PKHelper.CreateDataObject<Медведь>(guidBear);
            var values = new object[] { 500, guidForest, "Черняевский", guidBear, guidForest, guidForest, 0m };
            var storageStruct = Information.GetStorageStructForView(
                viewBear,
                viewBear.DefineClassType,
                StorageTypeEnum.SimpleStorage,
                null,
                typeof(SQLDataService));
            var dataObjectCache = new DataObjectCache();

            // Act.
            Utils.FillRowSetToDataObject(bear, values, storageStruct, lcs, null, lcs.AdvansedColumns, dataObjectCache, sm);

            // Assert.
            Assert.Equal(ObjectStatus.UnAltered, bear.ЛесОбитания.GetStatus());
        }
    }
}
