namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System.Linq;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Тестовый класс для <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />.
    /// </summary>
    public class IReferencesCascadeDeleteTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public IReferencesCascadeDeleteTests()
            : base("IRCD")
        {
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, реализуюзего интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены.
        /// </summary>
        [Fact]
        public void TestIReferencesCascadeDelete()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var territoryToDelete = new Territory2() { XCoordinate = 33 };
                var territoryToNotDelete = new Territory2() { XCoordinate = 42 };
                var saladWithRef = new Place2()
                    { PlaceName = "OnlyOneDelete", TodayTerritory = territoryToDelete, TomorrowTeritory = territoryToNotDelete };
                var saladWithRef2 = new Place2()
                    { PlaceName = "WithNoDelete", TodayTerritory = territoryToNotDelete, TomorrowTeritory = territoryToNotDelete };
                var dishWithRef = new Human2()
                    { HumanName = "OnlyWithDelete", TodayHome = territoryToDelete };
                var dishWithRef2 = new Human2()
                    { HumanName = "WithNoDelete", TodayHome = territoryToNotDelete };
                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    { territoryToDelete, territoryToNotDelete, saladWithRef, saladWithRef2, dishWithRef, dishWithRef2 };

                dataService.UpdateObjects(ref objsToUpdate);

                territoryToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { territoryToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var territories = ds.Query<Territory2>(Territory2.Views.Territory2E).ToList();
                Assert.DoesNotContain(territories, x => x.__PrimaryKey.Equals(territoryToDelete.__PrimaryKey));
                Assert.Contains(territories, x => x.__PrimaryKey.Equals(territoryToNotDelete.__PrimaryKey));
                var places = ds.Query<Place2>(Place2.Views.Place2E).ToList();
                Assert.DoesNotContain(places, x => x.__PrimaryKey.Equals(saladWithRef.__PrimaryKey));
                Assert.Contains(places, x => x.__PrimaryKey.Equals(saladWithRef2.__PrimaryKey));
                var people = ds.Query<Human2>(Human2.Views.Human2E).ToList();
                Assert.DoesNotContain(people, x => x.__PrimaryKey.Equals(dishWithRef.__PrimaryKey));
                Assert.Contains(people, x => x.__PrimaryKey.Equals(dishWithRef2.__PrimaryKey));
            }
        }

        /// <summary>
        /// Выполняется проверка, что при удалении объекта, чей предок реализует интерфейс <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete" />,
        /// все ссылающиеся объекты будут удалены.
        /// </summary>
        [Fact]
        public void TestParentWithIReferencesCascadeDelete()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                SQLDataService ds = (SQLDataService)dataService;
                var cabbageToDelete = new Country2() { CountryName = "Delete" };
                var cabbageDetail = new Region() { RegionName = "Some part" };
                cabbageToDelete.Region = new DetailArrayOfRegion(cabbageToDelete);
                cabbageToDelete.Region.Add(cabbageDetail);
                var cabbageToNotDelete = new Country2() { CountryName = "NotDelete" };

                var saladWithRef = new Place2()
                    { PlaceName = "OnlyOneDelete", TodayTerritory = cabbageToDelete, TomorrowTeritory = cabbageToNotDelete };
                var saladWithRef2 = new Place2()
                    { PlaceName = "WithNoDelete", TodayTerritory = cabbageToNotDelete, TomorrowTeritory = cabbageToNotDelete };
                var dishWithRef = new Human2()
                    { HumanName = "OnlyWithDelete", TodayHome = cabbageToDelete };
                var dishWithRef2 = new Human2()
                    { HumanName = "WithNoDelete", TodayHome = cabbageToNotDelete };

                var cabbageSaladWithRef = new Apparatus2()
                    { ApparatusName = "OnlyOneDelete", Exporter = cabbageToDelete, Maker = cabbageToNotDelete };
                var cabbageSaladWithRef2 = new Apparatus2()
                    { ApparatusName = "WithNoDelete", Exporter = cabbageToNotDelete, Maker = cabbageToNotDelete };
                var soupWithRef = new Adress2()
                    { HomeNumber = 33, Country = cabbageToDelete };
                var soupWithRef2 = new Adress2()
                    { HomeNumber = 42, Country = cabbageToNotDelete };

                var objsToUpdate = new ICSSoft.STORMNET.DataObject[]
                    {
                        cabbageToDelete,
                        cabbageDetail,
                        cabbageToNotDelete,
                        saladWithRef,
                        saladWithRef2,
                        dishWithRef,
                        dishWithRef2,
                        cabbageSaladWithRef,
                        cabbageSaladWithRef2,
                        soupWithRef,
                        soupWithRef2,
                    };

                dataService.UpdateObjects(ref objsToUpdate);

                cabbageToDelete.SetStatus(ICSSoft.STORMNET.ObjectStatus.Deleted);
                objsToUpdate = new ICSSoft.STORMNET.DataObject[] { cabbageToDelete };

                // Act.
                dataService.UpdateObjects(ref objsToUpdate);

                // Assert.
                var countries = ds.Query<Country2>(Country2.Views.Country2E).ToList();
                Assert.DoesNotContain(countries, x => x.__PrimaryKey.Equals(cabbageToDelete.__PrimaryKey));
                Assert.Contains(countries, x => x.__PrimaryKey.Equals(cabbageToNotDelete.__PrimaryKey));
                var regions = ds.Query<Region>(Region.Views.RegionE).ToList();
                Assert.Empty(regions);
                var places = ds.Query<Place2>(Place2.Views.Place2E).ToList();
                Assert.DoesNotContain(places, x => x.__PrimaryKey.Equals(saladWithRef.__PrimaryKey));
                Assert.Contains(places, x => x.__PrimaryKey.Equals(saladWithRef2.__PrimaryKey));
                var people = ds.Query<Human2>(Human2.Views.Human2E).ToList();
                Assert.DoesNotContain(people, x => x.__PrimaryKey.Equals(dishWithRef.__PrimaryKey));
                Assert.Contains(people, x => x.__PrimaryKey.Equals(dishWithRef2.__PrimaryKey));
                var apparatuses = ds.Query<Apparatus2>(Apparatus2.Views.Apparatus2E).ToList();
                Assert.DoesNotContain(apparatuses, x => x.__PrimaryKey.Equals(cabbageSaladWithRef.__PrimaryKey));
                Assert.Contains(apparatuses, x => x.__PrimaryKey.Equals(cabbageSaladWithRef2.__PrimaryKey));
                var adresses = ds.Query<Adress2>(Adress2.Views.Adress2E).ToList();
                Assert.DoesNotContain(adresses, x => x.__PrimaryKey.Equals(soupWithRef.__PrimaryKey));
                Assert.Contains(adresses, x => x.__PrimaryKey.Equals(soupWithRef2.__PrimaryKey));
            }
        }
    }
}
