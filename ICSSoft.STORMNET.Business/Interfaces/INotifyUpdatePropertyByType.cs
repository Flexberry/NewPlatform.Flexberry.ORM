namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Notify update objects.
    /// </summary>
    public interface INotifyUpdatePropertyByType
    {
        /// <summary>
        /// Get types and properties for notify calls.
        /// </summary>
        /// <returns>Types and property names for subscription.</returns>
        IDictionary<Type, IEnumerable<string>> GetSubscribedProperties();

        /// <summary>
        /// Before update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Data objest state.</param>
        /// <param name="propertyName">Changed property name.</param>
        /// <param name="oldValue">Changed property old value.</param>
        /// <param name="newValue">Changed property new value.</param>
        void BeforeUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue);

        /// <summary>
        /// After success execute sql update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Data objest state.</param>
        /// <param name="propertyName">Changed property name.</param>
        /// <param name="oldValue">Changed property old value.</param>
        /// <param name="newValue">Changed property new value.</param>
        void AfterSuccessSqlUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue);

        /// <summary>
        /// After update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Data objest state.</param>
        /// <param name="propertyName">Changed property name.</param>
        /// <param name="oldValue">Changed property old value.</param>
        /// <param name="newValue">Changed property new value.</param>
        void AfterSuccessUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue);

        /// <summary>
        /// After fail update objects.
        /// </summary>
        /// <param name="dataObject">Updated data object.</param>
        /// <param name="status">Data objest state.</param>
        /// <param name="propertyName">Changed property name.</param>
        /// <param name="oldValue">Changed property old value.</param>
        /// <param name="newValue">Changed property new value.</param>
        void AfterFailUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue);
    }
}
