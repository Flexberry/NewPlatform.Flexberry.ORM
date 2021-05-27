namespace ICSSoft.STORMNET.Tests.TestClasses.UserDataTypes
{
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    /// <summary>
    /// Проверка класса GeoData.cs.
    /// </summary>
    public class GeoDataTest
    {
        /// <summary>
        /// Проверка явного преобразования GeoData в string.
        /// </summary>
        [Fact]

        public void GeoDataExplicitGeoDataToStringTest()
        {
            var testGeoData = new GeoData();
            Assert.Equal((string)testGeoData, "<geoData />");

            testGeoData.Name = "tName";
            Assert.Equal((string)testGeoData, "<geoData name=\"tName\" />");

            testGeoData.AxesSystem = "tAxesSystem";
            Assert.Equal((string)testGeoData, "<geoData name=\"tName\" axes=\"tAxesSystem\" />");

            testGeoData.Poligons = "tPoligons";
            Assert.Equal((string)testGeoData, "<geoData name=\"tName\" axes=\"tAxesSystem\" wkt=\"tPoligons\" />");
        }

        /// <summary>
        /// Проверка явного преобразования string в GeoData.
        /// </summary>
        [Fact]

        public void GeoDataExplicitStringToGeoDataTest()
        {
            Assert.Null((GeoData)string.Empty);

            var testGeoData = new GeoData();
            var testXml = "<geoData />";
            Assert.True(ExplicitStringToGeoDataTest(testXml, testGeoData));

            testGeoData.Name = "tName";
            testXml = "<geoData name=\"tName\" />";
            Assert.True(ExplicitStringToGeoDataTest(testXml, testGeoData));

            testGeoData.AxesSystem = "tAxesSystem";
            testXml = "<geoData name=\"tName\" axes=\"tAxesSystem\" />";
            Assert.True(ExplicitStringToGeoDataTest(testXml, testGeoData));

            testGeoData.Poligons = "tPoligons";
            testXml = "<geoData name=\"tName\" axes=\"tAxesSystem\" wkt=\"tPoligons\" />";
            Assert.True(ExplicitStringToGeoDataTest(testXml, testGeoData));
        }

        /// <summary>
        /// Метод для проверки логики в тесте GeoDataExplicitStringToGeoDataTest.
        /// </summary>
        /// <param name="testXml">
        /// XML для преобразования.
        /// </param>
        /// <param name="testGeoData">
        /// Сравниваемый контекст.
        /// </param>
        /// <returns>
        /// Если все правильно, возвращается true <see cref="bool"/>.
        /// </returns>
        private bool ExplicitStringToGeoDataTest(string testXml, GeoData testGeoData)
        {
            var xmlGeoData = (GeoData)testXml;
            return xmlGeoData.Name == testGeoData.Name && xmlGeoData.AxesSystem == testGeoData.AxesSystem
                   && xmlGeoData.Poligons == testGeoData.Poligons && xmlGeoData.ToString() == testGeoData.ToString();
        }
    }
}
