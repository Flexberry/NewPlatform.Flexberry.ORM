namespace NewPlatform.Flexberry.ORM.IntegratedTests.ExternalLDef
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Test for master details limits.
    /// </summary>
    public class MasterDetailsLimitTest : BaseIntegratedTest
    {
        public MasterDetailsLimitTest(ITestOutputHelper output)
            : base("MstDtLmt")
        {
            this.output = output;
        }

        /// <summary>
        /// Test for limitations by master details.
        /// </summary>
        [Fact]
        public void MasterDetailsLimitationTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                Медведь медведь1 = new Медведь() { ПорядковыйНомер = 1 };
                Медведь медведь2 = new Медведь() { ПорядковыйНомер = 2 };

                Берлога берлога1 = new Берлога() { Наименование = "Берлога 1" };
                Берлога берлога2 = new Берлога() { Наименование = "Берлога 2" };
                Берлога берлога3 = new Берлога() { Наименование = "Берлога 3" };
                Берлога берлога4 = new Берлога() { Наименование = "Берлога 4" };

                медведь1.Берлога.AddRange(берлога1, берлога2);
                медведь2.Берлога.AddRange(берлога3, берлога4);

                Блоха блоха1 = new Блоха() { Кличка = "Блоха 1", МедведьОбитания = медведь1 };
                Блоха блоха2 = new Блоха() { Кличка = "Блоха 2", МедведьОбитания = медведь2 };
                Блоха блоха3 = new Блоха() { Кличка = "Блоха 3" };
                Блоха блоха4 = new Блоха() { Кличка = "Блоха 4", МедведьОбитания = медведь1 };

                DataObject[] newDataObjects = new DataObject[] { медведь1, медведь2, берлога1, берлога2, берлога3, берлога4, блоха1, блоха2, блоха3, блоха4 };

                dataService.UpdateObjects(ref newDataObjects);

                View view = new View();
                view.DefineClassType = typeof(Блоха);
                view.AddProperty(Information.ExtractPropertyPath<Блоха>(б => б.Кличка));
                view.AddProperty(Information.ExtractPropertyPath<Блоха>(б => б.МедведьОбитания));
                view.AddProperty(Information.ExtractPropertyPath<Блоха>(б => б.МедведьОбитания.ПорядковыйНомер));

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Блоха), view);

                // Выберем всех блох, чей медведь живёт в берлоге 1.
                DetailVariableDef dvd = new DetailVariableDef();
                dvd.ConnectMasterPorp = Information.ExtractPropertyPath<Берлога>(б => б.Медведь);
                dvd.OwnerConnectProp = new string[] { Information.ExtractPropertyPath<Блоха>(б => б.МедведьОбитания) };
                View viewDetail = new View();
                viewDetail.DefineClassType = typeof(Берлога);
                viewDetail.AddProperty(Information.ExtractPropertyPath<Берлога>(б => б.Наименование));
                viewDetail.AddProperty(Information.ExtractPropertyPath<Берлога>(б => б.Медведь));

                dvd.View = viewDetail;
                ExternalLangDef ldef = ExternalLangDef.LanguageDef;
                dvd.Type = ldef.DetailsType;
                lcs.LimitFunction = ldef.GetFunction(ldef.funcExist, dvd, ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, Information.ExtractPropertyPath<Берлога>(б => б.Наименование)), "Берлога 1"));

                // Act.
                DataObject[] dataObjects = dataService.LoadObjects(lcs);

                // Assert.
                Assert.Equal(2, dataObjects.Length);

                foreach (Блоха блоха in dataObjects)
                {
                    if (блоха.Кличка == "Блоха 1" || блоха.Кличка == "Блоха 4")
                    {
                        continue;
                    }

                    throw new System.Exception("Limit not work properly.");
                }
            }
        }
    }
}
