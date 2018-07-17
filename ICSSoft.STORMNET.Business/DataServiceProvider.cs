namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using ICSSoft.Services;

    using Unity;

    /// <summary>
    /// <see cref="IDataService"/> service locator.
    /// </summary>
    public class DataServiceProvider
    {
        /// <summary>
        /// Deny construct instance directly.
        /// </summary>
        private DataServiceProvider()
        {
        }

        /// <summary>
        /// Static cache field for <see cref="IDataService"/> instance.
        /// </summary>
        private static IDataService _dataService;

        /// <summary>
        /// Private field for <see cref="AlwaysNewDS"/> property.
        /// </summary>
        private static bool _alwaysNewDS = IsWebApp;

        /// <summary>
        /// If <c>true</c> the <see cref="DataServiceProvider.DataService"/> returns new data service instance. Default <c>true</c> for web applications (<see cref="HttpContext.Current"/> is not null), <c>false</c> for other applications.
        /// </summary>
        public static bool AlwaysNewDS
        {
            get { return _alwaysNewDS; }
            set { _alwaysNewDS = value; }
        }

        /// <summary>
        /// Name of configuration settings for DataService type.
        /// </summary>
        public const string ConfigurationDataServiceType = "DataServiceType";

        /// <summary>
        /// Name of configuration settings for customization strings. Ussually is connection string.
        /// </summary>
        public const string ConfigurationCustomizationStrings = "CustomizationStrings";

        /// <summary>
        /// Current <see cref="IDataService"/> instance.
        /// </summary>
        /// <exception cref="Exception">Throws if your configuration file has wrong parameters.</exception>
        public static IDataService DataService
        {
            get
            {
                if (_alwaysNewDS || _dataService == null)
                {
                    try
                    {
                        // Может IDataService определено через Unity.
                        IUnityContainer container = UnityFactory.GetContainer();
                        if (container.IsRegistered<IDataService>())
                        {
                            // Получаем тип сервиса данных.
                            IDataService dataService = container.Resolve<IDataService>();

                            if (dataService.CustomizationString == null)
                            {
                                // Пробуем получить строку соединения в web-стиле.
                                string connectionStringName =
                                    WebConfigurationManager.AppSettings["DefaultConnectionStringName"];
                                string connectionString = null;
                                if (!string.IsNullOrEmpty(connectionStringName))
                                {
                                    ConnectionStringSettings connString =
                                        WebConfigurationManager.ConnectionStrings[connectionStringName];
                                    if (connString != null)
                                    {
                                        connectionString = connString.ConnectionString;
                                    }
                                    else
                                    {
                                        LogService.LogError(string.Format("Connection string '{0}' not found at configuration file.", connectionStringName));
                                    }
                                }

                                // Получаем строку соединения в win-стиле.
                                if (string.IsNullOrEmpty(connectionString))
                                {
                                    connectionString = GetConnectionString();
                                }

                                dataService.CustomizationString = connectionString;
                            }

                            _dataService = dataService;
                            return dataService;
                        }
                    }
                    catch (Exception exception)
                    {
                        LogService.LogError("IDataService is not resolved.", exception);
                    }
                }

                // Web applications served by ICSSoft.STORMNET.Web.Tools.BridgeToDS class for support some extended features like multidatabase, etc.
                if (_alwaysNewDS && IsWebApp)
                {
                    // Resolve ICSSoft.STORMNET.Web.Tools.BridgeToDS by reflection. It is allow resolve dependency to ICSSoft.STORMNET.Web.Tools in runtime.
                    string typeName = "ICSSoft.STORMNET.Web.Tools.BridgeToDS,ICSSoft.STORMNET.Web.Tools";
                    Type bridgeType = Type.GetType(typeName);
                    if (bridgeType != null)
                    {
                        System.Reflection.MethodInfo methodinfo = bridgeType.GetMethod("GetDataService", new Type[] { });
                        try
                        {
                            IDataService ds = (IDataService)methodinfo.Invoke(null, null);
                            return ds;
                        }
                        catch (Exception ex)
                        {
                            string message = "Data service get method failed.";
                            LogService.LogError(message, ex);
                            throw new Exception(message, ex);
                        }
                    }

                    string mess = "Type not found: " + typeName;
                    LogService.LogError(mess);
                    throw new Exception(mess);
                }

                if (_alwaysNewDS || _dataService == null)
                {
                    string dataServiceTypeString = ConfigurationManager.AppSettings[ConfigurationDataServiceType];
                    if (!string.IsNullOrEmpty(dataServiceTypeString))
                    {
                        Type type = Type.GetType(dataServiceTypeString);
                        if (type == null)
                        {
                            throw new Exception(string.Format("DataService type name is invalid. Check your configuration file, settings name: {0}", ConfigurationDataServiceType));
                        }

                        _dataService = (IDataService)Activator.CreateInstance(type);
                        _dataService.CustomizationString = GetConnectionString();
                    }
                    else
                    {
                        throw new Exception(string.Format("Can't find keys {0} and {1} in configuration file!", ConfigurationDataServiceType, ConfigurationCustomizationStrings));
                    }
                }

                return _dataService;
            }

            set
            {
                _dataService = value;
            }
        }

        /// <summary>
        /// Получаем строку соединения в win-стиле.
        /// </summary>
        /// <returns>Полученная строка соединения.</returns>
        internal static string GetConnectionString()
        {
            string dataServiceCustomizationString = ConfigurationManager.AppSettings[ConfigurationCustomizationStrings];
            string enc = ConfigurationManager.AppSettings["Encrypted"];
            if (!string.IsNullOrEmpty(enc) && enc.ToLower() == "true")
            {
                dataServiceCustomizationString = Decrypt(dataServiceCustomizationString, true);
            }

            return dataServiceCustomizationString;
        }

        /// <summary>
        /// Private field for <see cref="IsWebApp"/> property.
        /// </summary>
        private static bool? _isWebApp;

        /// <summary>
        /// Determinate is a web application. Will be check <see cref="HttpContext.Current"/> is not null for web applications.
        /// </summary>
        public static bool IsWebApp
        {
            get
            {
                if (_isWebApp == null)
                {
                    _isWebApp = HttpContext.Current != null;
                }

                return _isWebApp.Value;
            }
        }

        /// <summary>
        /// Encryption method for customization string.
        /// </summary>
        /// <param name="toEncrypt">String for encrypt.</param>
        /// <param name="useHashing">Use hashing for encryption.</param>
        /// <returns>Encrypted string.</returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] resultArray;
            using (TripleDESCryptoServiceProvider tdes = GetProvider(useHashing))
            {
                ICryptoTransform cryptoTransform = tdes.CreateEncryptor();
                resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
            }

            // Encrypted string.
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypt method to get original string.
        /// </summary>
        /// <param name="cipherString">String for decription.</param>
        /// <param name="useHashing">Use hashing for decription.</param>
        /// <returns>Original string.</returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] resultArray;
            using (TripleDESCryptoServiceProvider tdes = GetProvider(useHashing))
            {
                ICryptoTransform cryptoTransform = tdes.CreateDecryptor();
                resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
            }

            // Decrypted string.
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Get hash bytes for key.
        /// </summary>
        /// <param name="useHashing">Use key hashing.</param>
        /// <returns>Hask bytes.</returns>
        private static byte[] GetHash(bool useHashing)
        {
            string key = "SecurityKey";
            byte[] keyArray;
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            return keyArray;
        }

        /// <summary>
        /// Get triple DES crypto service provider.
        /// </summary>
        /// <param name="useHashing">Use key hashing.</param>
        /// <returns>Triple DES crypto service provider.</returns>
        private static TripleDESCryptoServiceProvider GetProvider(bool useHashing)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = GetHash(useHashing),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            return tdes;
        }
    }
}
