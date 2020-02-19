namespace NewPlatform.Flexberry.ORM.IntegratedTests
{
    using System.Linq;
    using ICSSoft.STORMNET.Business;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    public class BusinessServersTest : BaseIntegratedTest
    {
        /// <summary>
        /// Default constructor for <see cref="BusinessServersTest"/>.
        /// </summary>
        public BusinessServersTest()
            : base("BSTest")
        {
        }

        /// <summary>
        /// Test to check the call business server of aggregator when adding detail.
        /// </summary>
        [Fact]
        public void CallAgregatorBSOnAddDetailTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var медведь = new Медведь();
                var берлога = new Берлога();
                медведь.Берлога.Add(берлога);

                dataService.UpdateObject(медведь);

                var новаяБерлога = new Берлога();
                медведь.Берлога.Add(новаяБерлога);

                ICSSoft.STORMNET.DataObject[] dataObjects = new ICSSoft.STORMNET.DataObject[] { медведь, новаяБерлога };
                dataService.UpdateObjects(ref dataObjects);

                var берлоги = медведь.Берлога.GetAllObjects().Cast<Берлога>();

                Assert.Equal(1, берлоги.Count(б => б.Заброшена));
                Assert.Equal(1, берлоги.Count(б => !б.Заброшена));
            }
        }
    }
}
