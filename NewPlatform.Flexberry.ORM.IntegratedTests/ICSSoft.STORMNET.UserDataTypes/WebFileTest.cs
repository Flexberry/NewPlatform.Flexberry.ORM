namespace ICSSoft.STORMNET.Tests.TestClasses.UserDataTypes
{
    using System.IO;
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    /// <summary>
    /// Проверка класса <see cref="WebFile" />.
    /// </summary>
    public class WebFileTest
    {
        /// <summary>
        /// Проверка метода Compare.
        /// </summary>
        [Fact]
        public void WebFileCompareTest()
        {
            const string Dir1 = "47192591-d342-4724-8962-426460fa51d9";
            const string Dir2 = "0260a48e-4196-4964-b02a-58b2c6837993";

            const string File1 = "file1";
            const string File2 = "file2";

            var file00 = new WebFile()
            { Name = File1, Url = GetFileUrl(Dir1, File1) };
            var file01 = new WebFile()
            { Name = File2, Url = GetFileUrl(Dir1, File2) };
            var file02 = new WebFile()
            { Name = File1, Url = GetFileUrl(Dir2, File1) };
            var file03 = new WebFile()
            { Name = File2, Url = GetFileUrl(Dir2, File2) };
            var file10 = new WebFile()
            { Name = File1 };
            var file11 = new WebFile()
            { Name = File2 };

            Assert.Equal(0, file00.Compare(file00));
            Assert.NotEqual(0, file00.Compare(file01));
            Assert.NotEqual(0, file00.Compare(file02));
            Assert.NotEqual(0, file00.Compare(file03));

            Assert.NotEqual(0, file00.Compare(file10));

            Assert.Equal(0, file10.Compare(file10));
            Assert.NotEqual(0, file10.Compare(file11));
        }

        private string GetFileUrl(string dirName, string fileName)
        {
            return Path.Combine(dirName, fileName);
        }
    }
}
