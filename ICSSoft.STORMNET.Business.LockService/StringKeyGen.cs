namespace ICSSoft.STORMNET.Business
{
    using System;

    using ICSSoft.STORMNET.KeyGen;

    /// <summary>
    /// The string key gen.
    /// </summary>
    public class StringKeyGen : BaseKeyGenerator
    {
        #region Public Properties

        /// <summary>
        /// Gets KeyType.
        /// </summary>
        public override Type KeyType
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// Gets a value indicating whether Unique.
        /// </summary>
        public override bool Unique
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The generate.
        /// </summary>
        /// <param name="dataObjectType">
        /// The data object type.
        /// </param>
        /// <returns>
        /// The generate.
        /// </returns>
        public override object Generate(Type dataObjectType)
        {
            return string.Empty;
        }

        /// <summary>
        /// The generate.
        /// </summary>
        /// <param name="dataObjectType">
        /// The data object type.
        /// </param>
        /// <param name="sds">
        /// The sds.
        /// </param>
        /// <returns>
        /// The generate.
        /// </returns>
        public override object Generate(Type dataObjectType, object sds)
        {
            return string.Empty;
        }

        /// <summary>
        /// The generate uniqe.
        /// </summary>
        /// <param name="dataObjectType">
        /// The data object type.
        /// </param>
        /// <returns>
        /// The generate uniqe.
        /// </returns>
        public override object GenerateUniqe(Type dataObjectType)
        {
            return Generate(dataObjectType);
        }

        /// <summary>
        /// The generate uniqe.
        /// </summary>
        /// <param name="dataObjectType">
        /// The data object type.
        /// </param>
        /// <param name="sds">
        /// The sds.
        /// </param>
        /// <returns>
        /// The generate uniqe.
        /// </returns>
        public override object GenerateUniqe(Type dataObjectType, object sds)
        {
            return Generate(dataObjectType, sds);
        }

        #endregion
    }
}