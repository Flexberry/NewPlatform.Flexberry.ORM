namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using NewPlatform.Flexberry.ORM.IntegratedTests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Test class for <see cref="BusinessServer"/>.
    /// </summary>
    public class BusinessServerTest : BaseIntegratedTest
    {
        private ITestOutputHelper output;

        /// <summary>
        /// Creates new instance of test class.
        /// </summary>
        /// <param name="output">XUnit output.</param>
        public BusinessServerTest(ITestOutputHelper output)
            : base("BS")
        {
            this.output = output;
        }

        /// <summary>
        /// Проверка применения изменений мастера в <see cref="BusinessServer"/>, имеющего собственные изменения в этой же транзакции.
        /// </summary>
        [Fact]
        public void UpdateSameMasterInBusinessServerTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                // Создадим в БД структуру, которая будет служить для проверки.
                Берлога берлога = new Берлога();
                object pk = берлога.__PrimaryKey;
                берлога.Наименование = "Первая";

                Медведь медведь = new Медведь();
                медведь.ПорядковыйНомер = 1;

                берлога.Медведь = медведь;

                DataObject[] objects1 = { медведь, берлога };

                dataService.UpdateObjects(ref objects1);

                // Подготовим структуру для вычитки.
                View view1 = new View();
                view1.DefineClassType = typeof(Берлога);
                view1.AddProperties(Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ПорядковыйНомер), Information.ExtractPropertyPath<Берлога>(p => p.Наименование), Information.ExtractPropertyPath<Берлога>(p => p.Комфортность), Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ЦветГлаз));

                LoadingCustomizationStruct lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Берлога), view1);

                lcs1.LimitFunction = FunctionBuilder.BuildEquals(pk);
                lcs1.ReturnTop = 1;
                DataObjectCache dataObjectCache = new DataObjectCache();
                dataObjectCache.StartCaching(true);
                DataObject[] loadedObjects1 = dataService.LoadObjects(lcs1, dataObjectCache);
                Берлога берлога1 = loadedObjects1[0] as Берлога;
                string newText = "изменённое наименование";

                // Act.
                берлога1.Наименование = newText;
                берлога1.Медведь.ПорядковыйНомер = 55;

                DataObject[] objects2 = { берлога1.Медведь, берлога1 };

                dataService.UpdateObjects(ref objects2);

                // Assert.
                View view2 = new View();
                view2.DefineClassType = typeof(Берлога);
                view2.AddProperties(Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ПорядковыйНомер), Information.ExtractPropertyPath<Берлога>(p => p.Наименование), Information.ExtractPropertyPath<Берлога>(p => p.Медведь.ЦветГлаз));

                LoadingCustomizationStruct lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Берлога), view2);
                lcs2.LimitFunction = FunctionBuilder.BuildEquals(pk);

                lcs2.ReturnTop = 1;

                DataObject[] loadedObjects2 = dataService.LoadObjects(lcs2);
                Берлога берлога2 = (Берлога)loadedObjects2[0];

                Assert.Equal(66, берлога2.Медведь.ПорядковыйНомер);
                Assert.Equal(newText, берлога2.Наименование);
                Assert.Equal("Зелёный", берлога2.Медведь.ЦветГлаз);
            }
        }
    }
}
