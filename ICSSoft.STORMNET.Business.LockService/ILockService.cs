using ICSSoft.STORMNET.Business;

namespace NewPlatform.Flexberry.Services
{
    /// <summary>
    /// Base interface of service for locking objects.
    /// </summary>
    public interface ILockService
    {
        IDataService DataService { get; set; }
        /// <summary>
        /// Locks the object with specified key.
        /// </summary>
        /// <param name="dataObjectKey">The data object key.</param>
        /// <param name="userName">Name of the user who locks the object.</param>
        /// <returns>Information about lock.</returns>
        LockInfo LockObject(object dataObjectKey, string userName);

        /// <summary>
        /// Gets the lock of object with specified key.
        /// </summary>
        /// <param name="dataObjectKey">The data object key.</param>
        /// <returns>Information about lock.</returns>
        LockInfo GetLock(object dataObjectKey);

        /// <summary>
        /// Unlocks the object.
        /// </summary>
        /// <param name="dataObjectKey">The data object key.</param>
        /// <returns>Returns <c>true</c> if object is successfully unlocked or <c>false</c> if it's not exist.</returns>
        bool UnlockObject(object dataObjectKey);

        /// <summary>
        /// Unlocks all objects.
        /// </summary>
        void UnlockAllObjects();


        /// <summary>
        /// ”бить все блокировки пользовател€
        /// </summary>
        /// <param name="userName"></param>
        void ClearAllUserLocks(string userName);

        /// <summary>
        /// ”бить указанную блокировку дл€ указанного пользовател€
        /// </summary>
        /// <param name="lockName"></param>
        /// <param name="userName"></param>
        void ClearLock(string lockName, string userName);

        /// <summary>
        /// ”станавливает блокировку. ≈сли удачно - то возвращаетс€ пуста€ строка, если нет - UserName.
        /// </summary>
        /// <returns>»м€ пользовател€, которым зан€т ресурс</returns>
        string SetLock(string lockName, string userName);

    }
}