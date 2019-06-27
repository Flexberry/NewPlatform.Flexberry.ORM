namespace NewPlatform.Flexberry.Services
{
    using System;

    using ICSSoft.STORMNET;

    /// <summary>
    /// Data object class for storing lock data in the database.
    /// </summary>
    [ClassStorage("STORMNETLOCKDATA")]
    [PrimaryKeyStorage("LockKey")]
    [View("V", new[] { "*" })]
    [View("WebLockDataL", new[] { "UserName as \"Пользователь\"", "LockDate as \"Дата блокировки\"", "LockKey as \"Ключ объекта\"" })]
    [AutoAltered]
    public class Lock : ICSSoft.STORMNET.Business.LockData
    {
        /// <summary>
        /// Date and time of adding lock.
        /// </summary>
        public DateTime LockDate { get; set; }

        /// <summary>
        /// Class views container
        /// </summary>
        public class Views
        {
            /// <summary>
            /// "V" view
            /// </summary>
            public static View V => Information.GetView("V", typeof(Lock));

            /// <summary>
            /// "WebLockDataL" view
            /// </summary>
            public static View WebLockDataL => Information.GetView("WebLockDataL", typeof(Lock));
        }
    }
}
