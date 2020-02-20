namespace NewPlatform.Flexberry.ORM.IntegratedTests
{
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    using DataObjectClass = ICSSoft.STORMNET.DataObject;

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
        public void CallAggregatorBSOnAddDetailTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var медведь = new Медведь();
                медведь.Берлога.Add(new Берлога());

                dataService.UpdateObject(медведь);

                var новаяБерлога = new Берлога();
                медведь.Берлога.Add(новаяБерлога);

                var dataObjects = new DataObjectClass[] { медведь, новаяБерлога };
                dataService.UpdateObjects(ref dataObjects);

                var берлоги = медведь.Берлога.GetAllObjects().Cast<Берлога>();

                Assert.Equal(1, берлоги.Count(б => б.Заброшена));
                Assert.Equal(1, берлоги.Count(б => !б.Заброшена));
            }
        }

        /// <summary>
        /// Test to check the call business server of aggregator when changing detail.
        /// </summary>
        [Fact]
        public void CallAggregatorBSOnChangeDetailTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                var медведь = new Медведь();
                медведь.Берлога.Add(new Берлога() { Заброшена = true });
                медведь.Берлога.Add(new Берлога() { Заброшена = true });

                dataService.UpdateObject(медведь);

                медведь.Берлога[0].Комфортность += 1;

                var dataObjects = new DataObjectClass[] { медведь, медведь.Берлога[0] };
                dataService.UpdateObjects(ref dataObjects);

                var берлоги = медведь.Берлога.GetAllObjects().Cast<Берлога>();
                var комфортнаяБерлога = берлоги.FirstOrDefault(б => б.Комфортность == 1);

                Assert.False(комфортнаяБерлога.Заброшена);
                Assert.Equal(1, берлоги.Count(б => б.Заброшена));
            }
        }
    }
}
