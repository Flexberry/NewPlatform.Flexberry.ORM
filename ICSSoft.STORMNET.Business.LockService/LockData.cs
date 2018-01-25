using System;
namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// The lock data.
    /// </summary>
    [ClassStorage("STORMNETLOCKDATA")]
    [PrimaryKeyStorage("LockKey")]
    [KeyGenerator(typeof(StringKeyGen))]
    [View("V", new[] { "*" })]
    [AutoAltered(true)]
    public class LockData : DataObject
    {
        /// <summary>
        /// The field user name.
        /// </summary>
        private string fieldUserName;

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


               
        public virtual DateTime TM { get; set; } = DateTime.Now;


        
    }
}