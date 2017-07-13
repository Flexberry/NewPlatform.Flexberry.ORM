namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// The lock data.
    /// </summary>
    [ClassStorage("STORMNETLOCKDATA")]
    [PrimaryKeyStorage("LockKey")]
    [KeyGenerator(typeof(StringKeyGen))]
    [View("V", new[] { "*" })]
    public class LockData : DataObject
    {
        #region Constants and Fields

        /// <summary>
        /// The field user name.
        /// </summary>
        private string fieldUserName;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets LockKey.
        /// </summary>
        [NotStored]
        public virtual string LockKey
        {
            get
            {
                return (string)__PrimaryKey;
            }

            set
            {
                __PrimaryKey = value;
            }
        }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        public virtual string UserName
        {
            get
            {
                return fieldUserName;
            }

            set
            {
                fieldUserName = value;
            }
        }

        /// <summary>
        /// Gets or sets __PrimaryKey.
        /// </summary>
        public override object __PrimaryKey
        {
            get
            {
                return base.__PrimaryKey;
            }

            set
            {
                if (value != string.Empty)
                {
                    base.__PrimaryKey = value;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The combined key.
        /// </summary>
        /// <returns>
        /// The combined key.
        /// </returns>
        public string CombinedKey()
        {
            return LockKey + " by " + UserName;
        }

        #endregion
    }
}