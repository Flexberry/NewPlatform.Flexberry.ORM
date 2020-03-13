namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.UserDataTypes;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    public class LoadingStateTests : BaseIntegratedTest
    {
        public LoadingStateTests()
            : base("LState")
        {
        }

        /// <summary>
        /// Тест проверяет, что при загрузке объекта по <see cref="View.ReadType.OnlyThatObject" />
        /// объекту присваивается <see cref="LoadingState.Loaded" />.
        /// </summary>
        [Fact]
        public void TestReadTypeOnlyThatObject()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var guidCountry = new Guid("37ea8412-2b3a-49d1-a129-8e7e5dd56473");
                var guidForest = new Guid("a1a31995-4322-47a8-8128-da251826b958");
                var country = new Страна { __PrimaryKey = guidCountry, Название = "РФ" };
                var forest = new Лес { __PrimaryKey = guidForest, Название = "Черняевский", Площадь = 500, ДатаПоследнегоОсмотра = NullableDateTime.Today, Страна = country };
                dataService.UpdateObject(forest);

                // Полное представление для класса.
                var view = new View(typeof(Лес), View.ReadType.OnlyThatObject);

                // Act.
                var loadedForest = PKHelper.CreateDataObject<Лес>(forest);
                dataService.LoadObject(view, loadedForest);

                // Assert.
                Assert.Equal(LoadingState.Loaded, loadedForest.GetLoadingState());
                Assert.Equal(LoadingState.LightLoaded, loadedForest.Страна.GetLoadingState());
            }
        }

        /// <summary>
        /// Тест проверяет, что при загрузке объекта по <see cref="View.ReadType.OnlyThatObject" />
        /// объекту присваивается <see cref="LoadingState.Loaded" />.
        /// Класс имеет нехранимое свойство, которое не попадает в представление.
        /// </summary>
        [Fact]
        public void TestReadTypeOnlyThatObjectNotStoredProperty()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var guidMaster = new Guid("37ea8412-2b3a-49d1-a129-8e7e5dd56473");
                var master = new ComputedMaster { __PrimaryKey = guidMaster, MasterField1 = "РФ", MasterField2 = "Пермь" };
                dataService.UpdateObject(master);

                // Полное представление для класса.
                var view = new View(typeof(ComputedMaster), View.ReadType.OnlyThatObject);

                // Act.
                var loadedMaster = PKHelper.CreateDataObject<ComputedMaster>(master);
                dataService.LoadObject(view, loadedMaster);

                // Assert.
                Assert.Equal(LoadingState.Loaded, loadedMaster.GetLoadingState());
            }
        }
    }
}
