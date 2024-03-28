namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// <see cref="IDataService"/> service locator.
    /// </summary>
    public class DataServiceProvider
    {
        private static IDataService dataService = null;

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
        [Obsolete("It is needed to use proper varians of dependency injection.")]
        public static IDataService DataService
        {
            get
            {
                return dataService ?? throw new NullReferenceException("DataServiceProvider.DataService is not set.");
            }

            set
            {
                if (dataService != null)
                {
                    throw new Exception("DataServiceProvider.DataService should not be reset.");
                }

                dataService = value;
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
