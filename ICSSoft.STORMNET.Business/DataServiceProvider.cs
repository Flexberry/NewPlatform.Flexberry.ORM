namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;

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
        /// Current <see cref="IDataService"/> instance.
        /// </summary>
        /// <exception cref="Exception">Throws if your configuration file has wrong parameters.</exception>
        public static IDataService DataService
        {
            get
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
                        string connectionStringName = ConfigurationManager.AppSettings["DefaultConnectionStringName"];
                        string connectionString = null;
                        if (!string.IsNullOrEmpty(connectionStringName))
                        {
                            ConnectionStringSettings connString = ConfigurationManager.ConnectionStrings[connectionStringName];
                            if (connString != null)
                            {
                                string enc = ConfigurationManager.AppSettings["Encrypted"];
                                if (!string.IsNullOrEmpty(enc) && enc.ToLower() == "true")
                                {
                                    connectionString = Decrypt(connString.ConnectionString, true);
                                }
                                else
                                {
                                    connectionString = connString.ConnectionString;
                                }
                            }
                            else
                            {
                                LogService.LogError(string.Format("Connection string '{0}' not found at configuration file.", connectionStringName));
                            }
                        }

                        dataService.CustomizationString = connectionString;
                    }

                    return dataService;
                }
                else
                {
                    throw new ConfigurationErrorsException("IDataService is not resolved. Check app configuration Unity section.");
                }
            }

            set
            {
                throw new ConfigurationErrorsException("Use Unity for configure IDataService type mapping");
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
                Padding = PaddingMode.PKCS7,
            };
            return tdes;
        }
    }
}
