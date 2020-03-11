namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    public class MasterLoadingTests : BaseIntegratedTest
    {
        public MasterLoadingTests()
            : base("MastLoad")
        {
        }

        /// <summary>
        /// Тест проверяет равенство загруженного мастера у агрегатора и детейла.
        /// </summary>
        [Fact]
        public void TestMasterCaching()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Медведь bear = CreateTestBear(dataService);

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
                viewBear.AddDetailInView(nameof(Медведь.Берлога), viewDen, true);

                // Act.
                var loadedBear = PKHelper.CreateDataObject<Медведь>(bear);
                dataService.LoadObject(viewBear, loadedBear);

                // Assert.
                var loadedDens = loadedBear.Берлога.Cast<Берлога>().ToList();
                Assert.Equal(1, loadedDens.Count);

                Лес loadedBearForest = loadedBear.ЛесОбитания;
                Лес loadedDenForest = loadedDens.First().ЛесРасположения;

                Assert.Equal(loadedBearForest, loadedDenForest);
            }
        }

        /// <summary>
        /// Тест проверяет состояние загруженного объекта при одинаковом наборе мастеровых свойств.
        /// </summary>
        [Fact]
        public void TestAlteredAfterLoading()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Медведь bear = CreateTestBear(dataService);

                // Для Берлога и Медведь есть общий мастер Лес.
                // Проверим корректность загрузки такого мастера.
                var viewDen = new View { DefineClassType = typeof(Берлога) };
                viewDen.AddProperties(
                    Information.ExtractPropertyPath<Берлога>(b => b.Наименование),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Название));

                var viewBear = new View { DefineClassType = typeof(Медведь) };
                viewBear.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Название));
                viewBear.AddDetailInView(nameof(Медведь.Берлога), viewDen, true);

                // Act.
                var loadedBear = PKHelper.CreateDataObject<Медведь>(bear);
                dataService.LoadObject(viewBear, loadedBear);

                // Assert.
                Лес loadedForest = loadedBear.ЛесОбитания;
                var forestLoadedProps = loadedForest.GetLoadedProperties();
                var forestAlteredProps = loadedForest.GetAlteredPropertyNames();
                var forestStatus = loadedForest.GetStatus();

                Assert.Equal(2, forestLoadedProps.Length);
                Assert.Equal(0, forestAlteredProps.Length);
                Assert.Equal(ObjectStatus.UnAltered, forestStatus);
                Assert.NotEqual(loadedBear.ЛесОбитания, ((Медведь)loadedBear.GetDataCopy()).ЛесОбитания);
            }
        }

        /// <summary>
        /// Тест проверяет состояние загруженного объекта при разном наборе мастеровых свойств.
        /// </summary>
        [Fact]
        public void TestAlteredAfterLoadingExtraProp()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Медведь bear = CreateTestBear(dataService);

                // Для Берлога и Медведь есть общий мастер Лес.
                // Поэтому добавим в представление Берлога больше свойств класса Лес: Площадь.
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
                viewBear.AddDetailInView(nameof(Медведь.Берлога), viewDen, true);

                // Act.
                var loadedBear = PKHelper.CreateDataObject<Медведь>(bear);
                dataService.LoadObject(viewBear, loadedBear);

                // Assert.
                Лес loadedBearForest = loadedBear.ЛесОбитания;
                var forestLoadedProps = loadedBearForest.GetLoadedProperties();
                var forestAlteredProps = loadedBearForest.GetAlteredPropertyNames();
                var forestStatus = loadedBearForest.GetStatus();

                Assert.Equal(3, forestLoadedProps.Length);
                Assert.Equal(0, forestAlteredProps.Length);
                Assert.Equal(ObjectStatus.UnAltered, forestStatus);
            }
        }

        /// <summary>
        /// Тест проверяет состояние загруженного объекта при разном наборе мастеровых свойств.
        /// </summary>
        [Fact]
        public void TestAlteredAfterLoadingExtraMasterProp()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Медведь bear = CreateTestBear(dataService);

                // Для Берлога и Медведь есть общий мастер Лес.
                // Поэтому добавим в представление Берлога больше свойств класса Лес: Площадь.
                var viewDen = new View { DefineClassType = typeof(Берлога) };
                viewDen.AddProperties(
                    Information.ExtractPropertyPath<Берлога>(b => b.Наименование),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Название),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Страна),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Страна.Название),
                    Information.ExtractPropertyPath<Берлога>(b => b.ЛесРасположения.Площадь));

                var viewBear = new View { DefineClassType = typeof(Медведь) };
                viewBear.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Название),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Страна),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.Страна.Название));
                viewBear.AddDetailInView(nameof(Медведь.Берлога), viewDen, true);

                // Act.
                var loadedBear = PKHelper.CreateDataObject<Медведь>(bear);
                dataService.LoadObject(viewBear, loadedBear);

                // Assert.
                Лес loadedBearForest = loadedBear.ЛесОбитания;
                var forestLoadedProps = loadedBearForest.GetLoadedProperties();
                var forestAlteredProps = loadedBearForest.GetAlteredPropertyNames();
                var forestStatus = loadedBearForest.GetStatus();

                Assert.Equal(4, forestLoadedProps.Length);
                Assert.Equal(0, forestAlteredProps.Length);
                Assert.Equal(ObjectStatus.UnAltered, forestStatus);
                Assert.NotEqual(loadedBearForest.Страна, ((Лес)loadedBearForest.GetDataCopy()).Страна);
            }
        }

        /// <summary>
        /// Тест проверяет, что функция ограничения на первичный ключ мастера остается валидной.
        /// </summary>
        [Fact]
        public void TestLimitFunctionOnMasterPrimaryKey()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Медведь bear = CreateTestBear(dataService);

                var viewBear = new View { DefineClassType = typeof(Медведь) };
                viewBear.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(b => b.ЛесОбитания.__PrimaryKey));

                var langDef = ExternalLangDef.LanguageDef;
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), viewBear);
                var masterVarDef = new VariableDef(
                    langDef.DataObjectType,
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания));
                var masterKeyVarDef = new VariableDef(
                    langDef.GuidType,
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                lcs.LimitFunction = langDef.GetFunction(
                    langDef.funcAND,
                    langDef.GetFunction(langDef.funcEQ, masterKeyVarDef, bear.ЛесОбитания.__PrimaryKey),
                    langDef.GetFunction(langDef.funcEQ, masterVarDef, bear.ЛесОбитания));

                // Act.
                var loadedBears = dataService.LoadObjects(lcs).Cast<Медведь>().ToList();
                var loadedBear = loadedBears.First();

                // Assert.
                Assert.Equal(1, loadedBears.Count);
                Assert.True(PKHelper.EQDataObject(bear.ЛесОбитания, loadedBear.ЛесОбитания));
            }
        }

        private static Медведь CreateTestBear(IDataService dataService)
        {
            var guidCountry = new Guid("37ea8412-2b3a-49d1-a129-8e7e5dd56473");
            var guidForest = new Guid("a1a31995-4322-47a8-8128-da251826b958");
            var guidBear = new Guid("89136856-459e-418d-a3be-547a60192517");
            var guidDen = new Guid("66aa9864-4325-4cf8-8838-b30254dbcf61");
            var country = new Страна { __PrimaryKey = guidCountry, Название = "РФ" };
            var forest = new Лес { __PrimaryKey = guidForest, Название = "Черняевский", Площадь = 600, Страна = country };
            var bear = new Медведь { __PrimaryKey = guidBear, ЛесОбитания = forest, Вес = 500 };
            var den = new Берлога { __PrimaryKey = guidDen, Наименование = "Спрятанная", Комфортность = 3, ЛесРасположения = forest };
            bear.Берлога.Add(den);

            dataService.UpdateObject(bear);

            return bear;
        }
    }
}
