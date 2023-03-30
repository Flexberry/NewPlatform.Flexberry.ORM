[assembly: Xunit.TestFramework("NewPlatform.Flexberry.ORM.Tests.XUnitTestRunnerInitializer", "NewPlatform.Flexberry.ORM.Tests")]

namespace NewPlatform.Flexberry.ORM.Tests
{
#if NETCOREAPP
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Text;
#endif

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.Windows.Forms;
    using Moq;
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
            IDataService ds = new MSSQLDataService(new Mock<ISecurityManager>().Object, new Mock<IAuditService>().Object);
            DataServiceProvider.DataService = ds;
            ExternalLangDef.LanguageDef = new ExternalLangDef(ds);
            DetailVariableDef.ViewGenerator = null;

#if NETCOREAPP
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string configFile = $"{Assembly.GetExecutingAssembly().Location}.config";
            string outputConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            File.Copy(configFile, outputConfigFile, true);
#endif
        }
    }
}
