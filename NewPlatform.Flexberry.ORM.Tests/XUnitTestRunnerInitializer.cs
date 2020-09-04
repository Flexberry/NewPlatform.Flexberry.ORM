﻿[assembly: Xunit.TestFramework("NewPlatform.Flexberry.ORM.Tests.XUnitTestRunnerInitializer", "NewPlatform.Flexberry.ORM.Tests")]

namespace NewPlatform.Flexberry.ORM.Tests
{
#if NETCORE
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Text;
#endif
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// Инициализация тестового запуска.
    /// </summary>
    public class XUnitTestRunnerInitializer : XunitTestFramework
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitTestRunnerInitializer" /> class.
        /// </summary>
        /// <param name="messageSink">The message sink used to send diagnostic messages.</param>
        public XUnitTestRunnerInitializer(IMessageSink messageSink)
            : base(messageSink)
        {
#if NETCORE
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string configFile = $"{Assembly.GetExecutingAssembly().Location}.config";
            string outputConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            File.Copy(configFile, outputConfigFile, true);
#endif
        }
    }
}
