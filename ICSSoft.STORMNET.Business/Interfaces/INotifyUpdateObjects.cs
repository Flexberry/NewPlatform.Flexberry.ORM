namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Notify update objects.
    /// </summary>
    public interface INotifyUpdateObjects
    {
        /// <summary>
        /// Before update objects.
        /// </summary>
        /// <param name="operationId">Unique operation Id.</param>
        /// <param name="dataService">Data service.</param>
        /// <param name="transaction">DB transaction.</param>
        /// <param name="dataObjects">Data objects for update.</param>
        void BeforeUpdateObjects(Guid operationId, IDataService dataService, IDbTransaction transaction, IEnumerable<DataObject> dataObjects);

        /// <summary>
        /// After success execute sql update objects.
        /// </summary>
        /// <param name="operationId">Unique operation Id.</param>
        /// <param name="dataService">Data service.</param>
        /// <param name="transaction">DB transaction.</param>
        /// <param name="dataObjects">Data objects for update.</param>
        void AfterSuccessSqlUpdateObjects(Guid operationId, IDataService dataService, IDbTransaction transaction, IEnumerable<DataObject> dataObjects);

        /// <summary>
        /// After update objects.
        /// </summary>
        /// <param name="operationId">Unique operation Id.</param>
        /// <param name="dataService">Data service.</param>
        /// <param name="dataObjects">Data objects for update.</param>
        void AfterSuccessUpdateObjects(Guid operationId, IDataService dataService, IEnumerable<DataObject> dataObjects);

        /// <summary>
        /// After fail update objects.
        /// </summary>
        /// <param name="operationId">Unique operation Id.</param>
        /// <param name="dataService">Data service.</param>
        /// <param name="dataObjects">Data objects for update.</param>
        void AfterFailUpdateObjects(Guid operationId, IDataService dataService, IEnumerable<DataObject> dataObjects);
    }
}
