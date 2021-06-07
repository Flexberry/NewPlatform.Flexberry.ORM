namespace ICSSoft.STORMNET.Tests.TestClasses.UserDataTypes
{
    using System.IO;
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    /// <summary>
    /// Проверка класса WebFile.cs .
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

            var file = new WebFile()
            { Name = File1, Url = GetFileUrl(Dir1, File1) };
            var file1 = new WebFile()
            { Name = File1, Url = GetFileUrl(Dir1, File1) };
            var file2 = new WebFile()
            { Name = File2, Url = GetFileUrl(Dir1, File2) };
            var file3 = new WebFile()
            { Name = File1, Url = GetFileUrl(Dir2, File1) };
            var file4 = new WebFile()
            { Name = File2, Url = GetFileUrl(Dir2, File2) };

            Assert.Equal(0, file.Compare(file));
            Assert.Equal(0, file.Compare(file1));
            Assert.NotEqual(0, file.Compare(file2));
            Assert.NotEqual(0, file.Compare(file3));
            Assert.NotEqual(0, file.Compare(file4));
        }

        private string GetFileUrl(string dirName, string fileName)
        {
            return Path.Combine(dirName, fileName);
        }
    }
}
