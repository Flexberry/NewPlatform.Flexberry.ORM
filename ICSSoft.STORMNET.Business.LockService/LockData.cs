using System;


namespace ICSSoft.STORMNET.Business
{

    /// <summary>
    /// Class that represents information about lock.
    /// </summary>
    public struct LockInfo
    {
        /// <summary>
        /// Is lock acquired now?
        /// </summary>
        public bool Acquired { get; }

        /// <summary>
        /// User who acquired lock.
        /// </summary>
        public string UseName { get; }

        /// <summary>
        /// The key of the acquired lock.
        /// </summary>
        public object Key { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockData" /> struct.
        /// </summary>
        /// <param name="lockKey">The key of the acquired lock.</param>
        /// <param name="userName">User who acquired lock.</param>
        /// <param name="acquired">Is lock acquired now?</param>
        public LockInfo(object lockKey, string userName, bool acquired)
        {
            Acquired = acquired;
            UseName = userName;
            Key = lockKey;
        }
    }


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

        public virtual DateTime TM { get; set; } = DateTime.Now;
    }
}