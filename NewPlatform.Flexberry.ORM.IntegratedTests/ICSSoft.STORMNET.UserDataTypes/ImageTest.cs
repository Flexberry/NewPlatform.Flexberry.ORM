namespace ICSSoft.STORMNET.Tests.TestClasses.UserDataTypes
{
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    /// <summary>
    /// Проверка класса Image.cs.
    /// </summary>
    public class ImageTest
    {
        /// <summary>
        /// Проверка метода Compare.
        /// </summary>
        [Fact]

        public void ImageCompareTest()
        {
            var image = new Image()
            { Data = "1234567", Format = "f", Height = 100, Width = 200, Name = "image", URL = "url-image" };
            var image1 = new Image()
            { Data = "7654321", Format = "t", Height = 200, Width = 100, Name = "egami", URL = "url-egami" };
            var image2 = new Image()
            { Data = "7654321", Format = "f", Height = 100, Width = 200, Name = "image", URL = "url-image" };
            var image3 = new Image()
            { Data = "1234567", Format = "f", Height = 100, Width = 200, Name = "image", URL = "url-imageNewUrl" };

            Assert.Equal(image.Compare(image1), 1);
            Assert.Equal(image.Compare(image2), 1);
            Assert.Equal(image.Compare(image3), 0);
            Assert.Equal(image.Compare(image), 0);
        }

        /// <summary>
        /// Проверка явного преобразования Image в string.
        /// </summary>
        [Fact]

        public void ImageExplicitImageToStringTest()
        {
            var testImage = new Image();
            Assert.Equal((string)testImage, "<image Width=\"0\" Height=\"0\" />");

            testImage.Name = "tName";
            Assert.Equal((string)testImage, "<image name=\"tName\" Width=\"0\" Height=\"0\" />");

            testImage.Format = "tFormat";
            Assert.Equal((string)testImage, "<image name=\"tName\" format=\"tFormat\" Width=\"0\" Height=\"0\" />");

            testImage.Width = 100;
            Assert.Equal((string)testImage, "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"0\" />");

            testImage.Height = 100;
            Assert.Equal((string)testImage, "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"100\" />");

            testImage.Data = "tData";
            Assert.Equal((string)testImage, "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"100\" rawData=\"tData\" />");

            testImage.URL = "tUrl";
            Assert.Equal((string)testImage, "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"100\" rawData=\"tData\" URL=\"tUrl\" />");
        }

        /// <summary>
        /// Проверка явного преобразования string в Image.
        /// </summary>
        [Fact]

        public void ImageExplicitStringToImageTest()
        {
            Assert.Null((Image)string.Empty);

            var testImage = new Image();
            var testXml = "<image Width=\"0\" Height=\"0\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));

            testImage.Name = "tName";
            testXml = "<image name=\"tName\" Width=\"0\" Height=\"0\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));

            testImage.Format = "tFormat";
            testXml = "<image name=\"tName\" format=\"tFormat\" Width=\"0\" Height=\"0\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));

            testImage.Width = 100;
            testXml = "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"0\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));

            testImage.Height = 100;
            testXml = "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"100\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));

            testImage.Data = "tData";
            testXml = "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"100\" rawData=\"tData\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));

            testImage.URL = "tUrl";
            testXml = "<image name=\"tName\" format=\"tFormat\" Width=\"100\" Height=\"100\" rawData=\"tData\" URL=\"tUrl\" />";
            Assert.True(ExplicitStringToImageTest(testXml, testImage));
        }

        /// <summary>
        /// Метод для проверки логики в тесте ImageExplicitStringToImageTest.
        /// </summary>
        /// <param name="testXml">
        /// XML для преобразования.
        /// </param>
        /// <param name="testImage">
        /// Сравниваемый контекст.
        /// </param>
        /// <returns>
        /// Если все правильно, возвращается true <see cref="bool"/>.
        /// </returns>
        private bool ExplicitStringToImageTest(string testXml, Image testImage)
        {
            Image xmlImage = (Image)testXml;
            return xmlImage.Name == testImage.Name && xmlImage.Format == testImage.Format
                   && xmlImage.Width == testImage.Width && xmlImage.Height == testImage.Height
                   && xmlImage.Data == testImage.Data && xmlImage.URL == testImage.URL
                   && xmlImage.ToString() == testImage.ToString();
        }
    }
}
