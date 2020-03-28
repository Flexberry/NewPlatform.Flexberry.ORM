namespace ICSSoft.STORMNET
{
    using System.Collections.Generic;

    /// <summary>
    /// Notify update objects.
    /// </summary>
    public interface INotifyUpdateObject
    {
        /// <summary>
        /// Before update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Updated data object status.</param>
        /// <param name="dataObjects">All data objects for update.</param>
        void BeforeUpdateObject(DataObject dataObject, ObjectStatus status, IEnumerable<DataObject> dataObjects);

        /// <summary>
        /// After success execute sql update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Updated data object status.</param>
        /// <param name="dataObjects">All data objects for update.</param>
        void AfterSuccessSqlUpdateObject(DataObject dataObject, ObjectStatus status, IEnumerable<DataObject> dataObjects);

        /// <summary>
        /// After update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Updated data object status.</param>
        /// <param name="dataObjects">All data objects for update.</param>
        void AfterSuccessUpdateObject(DataObject dataObject, ObjectStatus status, IEnumerable<DataObject> dataObjects);

        /// <summary>
        /// After fail update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Updated data object status.</param>
        /// <param name="dataObjects">All data objects for update.</param>
        void AfterFailUpdateObject(DataObject dataObject, ObjectStatus status, IEnumerable<DataObject> dataObjects);
    }
}
