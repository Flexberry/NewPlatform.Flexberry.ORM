namespace ICSSoft.STORMNET.Tools.Tests
{
    using System;
    using System.IO;

    using ICSharpCode.SharpZipLib.Core;
    using ICSharpCode.SharpZipLib.Zip;

    using Xunit;

    public class SharpZipLibTest
    {
        private const string FileNameOriginal = "SharpZipLibTest";

        private const string FileName086 = "SharpZipLib086Test";

        private const string FileName120 = "SharpZipLib120Test";

        private const int BYTESTOREAD = sizeof(long);

        [Fact]
        public void SharpZipLib086Test()
        {
            var archivePath086 = Path.Combine("Files", FileName086 + ".zip");

            var outFolder086 = Path.Combine("Files", FileName086);

            ExtractZipFile(archivePath086, outFolder086);

            string filePathOrigin = Path.Combine("Files", FileNameOriginal + ".txt");

            string filePath086 = Path.Combine(outFolder086, FileNameOriginal + ".txt");

            FileInfo fileInfoOriginal = new FileInfo(filePathOrigin);

            FileInfo fileInfo086 = new FileInfo(filePath086);

            Assert.True(FilesAreEqual(fileInfoOriginal, fileInfo086));

            fileInfo086.Delete();
        }

        [Fact]
        public void SharpZipLib120Test()
        {
            var archivePath120 = Path.Combine("Files", FileName120 + ".zip");

            var outFolder120 = Path.Combine("Files", FileName120);

            ExtractZipFile(archivePath120, outFolder120);

            string filePathOrigin = Path.Combine("Files", FileNameOriginal + ".txt");

            string filePath120 = Path.Combine(outFolder120, FileNameOriginal + ".txt");

            FileInfo fileInfoOriginal = new FileInfo(filePathOrigin);

            FileInfo fileInfo120 = new FileInfo(filePath120);

            Assert.True(FilesAreEqual(fileInfoOriginal, fileInfo120));

            fileInfo120.Delete();
        }

        private void ExtractZipFile(string archivePath, string outFolder)
        {
            using (Stream fs = File.OpenRead(archivePath))
            {
                using (ZipFile zf = new ZipFile(fs))
                {
                    foreach (ZipEntry zipEntry in zf)
                    {
                        if (!zipEntry.IsFile)
                        {
                            continue;
                        }

                        string entryFileName = zipEntry.Name;

                        var fullZipToPath = Path.Combine(outFolder, entryFileName);
                        var directoryName = Path.GetDirectoryName(fullZipToPath);

                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        var buffer = new byte[4096];

                        using (var zipStream = zf.GetInputStream(zipEntry))
                        {
                            using (Stream fsOutput = File.Create(fullZipToPath))
                            {
                                StreamUtils.Copy(zipStream, fsOutput, buffer);
                            }
                        }
                    }
                }
            }
        }

        private bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            int iterations = (int)Math.Ceiling((double)first.Length / BYTESTOREAD);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTESTOREAD];
                byte[] two = new byte[BYTESTOREAD];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTESTOREAD);
                    fs2.Read(two, 0, BYTESTOREAD);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
