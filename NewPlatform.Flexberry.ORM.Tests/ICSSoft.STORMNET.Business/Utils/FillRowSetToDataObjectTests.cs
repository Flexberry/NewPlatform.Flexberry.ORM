namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

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

        /// <summary>
        /// Тест проверяет статус загрузки мастерового объекта второго уровня.
        /// </summary>
        [Fact]
        public void TestAlteredSecondMasterAfterLoading()
        {
            // Arrange.
            var sm = new EmptySecurityManager();
            var viewBear = new View { DefineClassType = typeof(Медведь) };
            viewBear.AddProperties(
                Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Название),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Страна),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Страна.Название));
            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), viewBear);

            var guidCountry = new Guid("37ea8412-2b3a-49d1-a129-8e7e5dd56473");
            var guidForest = new Guid("a1a31995-4322-47a8-8128-da251826b958");
            var guidBear = new Guid("89136856-459e-418d-a3be-547a60192517");
            var bear = PKHelper.CreateDataObject<Медведь>(guidBear);
            var values = new object[11]
                { 500, guidForest, "Черняевский", guidCountry, "РФ", guidBear, guidForest, guidCountry, guidForest, guidCountry, 0m };
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
            Лес loadedForest = bear.ЛесОбитания;
            Assert.Equal(ObjectStatus.UnAltered, loadedForest.GetStatus());
            Страна loadedCountry = loadedForest.Страна;
            Assert.Equal(ObjectStatus.UnAltered, loadedCountry.GetStatus());

            Страна copyLoadedCountry = ((Лес)bear.ЛесОбитания.GetDataCopy()).Страна;
            Assert.Equal(ObjectStatus.UnAltered, copyLoadedCountry.GetStatus());
            Assert.NotEqual(loadedCountry, copyLoadedCountry);
        }

        /// <summary>
        /// Тест проверяет, что при загрузке объекта по <see cref="View.ReadType.OnlyThatObject" />
        /// объекту присваивается <see cref="LoadingState.Loaded" />.
        /// </summary>
        [Fact]
        public void TestReadTypeOnlyThatObjectLoadedState()
        {
            // Arrange.
            var sm = new EmptySecurityManager();
            var view = new View(typeof(Лес), View.ReadType.OnlyThatObject);
            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Лес), view);

            var guidCountry = new Guid("37ea8412-2b3a-49d1-a129-8e7e5dd56473");
            var guidForest = new Guid("a1a31995-4322-47a8-8128-da251826b958");
            var forest = PKHelper.CreateDataObject<Лес>(guidForest);
            var values = new object[8]
                { "Черняевский", 500, false, DateTime.Today, guidCountry, guidForest, guidCountry, 0m };
            var storageStruct = Information.GetStorageStructForView(
                view,
                view.DefineClassType,
                StorageTypeEnum.SimpleStorage,
                null,
                typeof(SQLDataService));
            var dataObjectCache = new DataObjectCache();

            // Act.
            Utils.FillRowSetToDataObject(forest, values, storageStruct, lcs, null, lcs.AdvansedColumns, dataObjectCache, sm);

            // Assert.
            Assert.Equal(LoadingState.Loaded, forest.GetLoadingState());
        }
    }
}
