namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;

    using Moq;

    using Xunit;

    /// <summary>
    /// Тесты метода <see cref="Utils.ProcessingRowsetDataRef" />.
    /// </summary>
    public class ProcessingRowsetDataRefTests
    {
        /// <summary>
        /// Тест проверяет статус загрузки мастерового объекта.
        /// </summary>
        [Fact]
        public void TestAlteredMasterAfterLoading()
        {
            // Arrange.
            var sm = new EmptySecurityManager();
            var dsMock = new Mock<SQLDataService>();
            var dObjs = new DataObject[0];
            dsMock.Setup(d => d.LoadObjects(It.IsAny<LoadingCustomizationStruct>(), It.IsAny<DataObjectCache>()))
                .Returns(dObjs);
            var viewDen = new View { DefineClassType = typeof(Берлога) };
            viewDen.AddProperties(
                Information.ExtractPropertyPath<Берлога>(b => b.Наименование),
                Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения),
                Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Название),
                Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Площадь));

            var viewBear = new View { DefineClassType = typeof(Медведь) };
            viewBear.AddProperties(
                Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания),
                Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Название));
            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), viewBear);
            var guidForest = new Guid("a1a31995-4322-47a8-8128-da251826b958");
            var guidBear = new Guid("89136856-459e-418d-a3be-547a60192517");
            var bear = PKHelper.CreateDataObject<Медведь>(guidBear);
            var value = new object[1][]
            {
                new object[] { 500, guidForest, "Черняевский", guidBear, guidForest, guidForest, 0m }
            };
            var storageStruct = new[]
            {
                Information.GetStorageStructForView(viewBear, viewBear.DefineClassType, StorageTypeEnum.SimpleStorage, null, typeof(SQLDataService))
            };
            var dataObjectCache = new DataObjectCache();
            var rrr = new DataObject[] { bear };

            // Act.
            Utils.ProcessingRowsetDataRef(value, lcs.LoadingTypes, storageStruct, lcs, rrr, dsMock.Object, null, true, dataObjectCache, sm);

            // Assert.
            Assert.Equal(ObjectStatus.UnAltered, bear.ЛесОбитания.GetStatus());
        }
    }
}
