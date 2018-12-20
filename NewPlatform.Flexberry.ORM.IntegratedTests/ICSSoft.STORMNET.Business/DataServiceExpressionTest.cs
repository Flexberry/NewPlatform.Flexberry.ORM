namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Проверка DataServiceExpression.
    /// </summary>

    public class DataServiceExpressionTest : BaseIntegratedTest
    {
        private ITestOutputHelper output;

        public DataServiceExpressionTest(ITestOutputHelper output)
            : base("DetLoad")
        {
            this.output = output;
        }

        [Fact]
        public void LoadComputedDetailWithComputedMaster()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Для проверик отдельных датасервисов нужно поменять.
                if (dataService is SQLDataService)
                {
                    var ds = (SQLDataService)dataService;

                    // Чтобы данные в БД точно был, создадим их.
                    var createdMaster = new ComputedMaster();
                    createdMaster.MasterField1 = "TestMaster1";
                    createdMaster.MasterField2 = "TestMaster2";

                    var masterComputedField = createdMaster.MasterComputedField1;

                    ds.UpdateObject(createdMaster);

                    var createdDetail = new ComputedDetail();
                    createdDetail.DetailField1 = "TestDetail1";
                    createdDetail.DetailField2 = "TestDetail2";
                    createdDetail.ComputedMaster = createdMaster;

                    var detailComputedField = createdDetail.DetailComputedField1;

                    ds.UpdateObject(createdDetail);

                    // Теперь грузим его их БД.
                    LoadingCustomizationStruct lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(ComputedMaster), ComputedMaster.Views.ComputedMasterL);
                    DataObject[] dataMaster = ds.LoadObjects(lcs1);

                    var loadCreatedMaster = dataMaster.Cast<ComputedMaster>().ToList().FirstOrDefault();

                    Assert.Equal(loadCreatedMaster.MasterComputedField1, masterComputedField);

                    LoadingCustomizationStruct lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(ComputedDetail), ComputedDetail.Views.ComputedDetail);
                    DataObject[] dataDetail = ds.LoadObjects(lcs2);

                    var loadCreatedDetail = dataDetail.Cast<ComputedDetail>().ToList().FirstOrDefault();

                    Assert.Equal(loadCreatedDetail.DetailComputedField1, detailComputedField);
                }
            }
        }
    }
}
