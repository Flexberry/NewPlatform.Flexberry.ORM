namespace ICSSoft.STORMNET.Business
{
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Security;
    using NewPlatform.Flexberry.ORM;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService, IAsyncDataService
    {
        /// <inheritdoc cref="IAsyncDataService.UpdateObjectsAsync(DataObject[], bool, DataObjectCache)"/>
        public virtual async Task UpdateObjectsAsync(DataObject[] objects, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null)
        {
            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            RunChangeCustomizationString(objects);

            using (DbTransactionWrapperAsync dbTransactionWrapper = new DbTransactionWrapperAsync(this))
            {
                try
                {
                    await UpdateObjectsByExtConnAsync(objects, dataObjectCache, alwaysThrowException, dbTransactionWrapper.Connection, dbTransactionWrapper.Transaction)
                        .ConfigureAwait(false);
#if NETSTANDARD2_1
                    await dbTransactionWrapper.CommitTransaction()
                        .ConfigureAwait(false);
#else
                    dbTransactionWrapper.CommitTransaction();
#endif
                }
                catch (Exception)
                {
#if NETSTANDARD2_1
                    await dbTransactionWrapper.RollbackTransaction()
                        .ConfigureAwait(false);
#else
                    dbTransactionWrapper.RollbackTransaction();
#endif
                    throw;
                }
            }
        }

        /// <inheritdoc cref="IAsyncDataService.UpdateObjectsAsync(DataObject[], bool, DataObjectCache)"/>
        /// <summary>Обновление объекта данных с использованием указанной коннекцией в рамках указанной транзакции.</summary>
        /// <param name="connection">Коннекция, через которую будет происходить зачитка.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет проходить зачитка.</param>
        public virtual async Task UpdateObjectsByExtConnAsync(DataObject[] objects, DataObjectCache dataObjectCache, bool alwaysThrowException, DbConnection connection, DbTransaction transaction = null)
        {
            object id = BusinessTaskMonitor.BeginTask("Update objects");

            var deleteQueries = new StringCollection();
            var updateQueries = new StringCollection();
            var updateFirstQueries = new StringCollection();
            var updateLastQueries = new StringCollection();
            var insertQueries = new StringCollection();

            var deleteTables = new StringCollection();
            var updateTables = new StringCollection();
            var insertTables = new StringCollection();
            var tableOperations = new SortedList();
            var queryOrder = new StringCollection();

            var allQueriedObjects = new ArrayList();

            var auditOperationInfoList = new List<AuditAdditionalInfo>();
            var extraProcessingList = new List<DataObject>();

            var dbTransactionWrapperAsync = new DbTransactionWrapperAsync(connection, transaction);
            GenerateQueriesForUpdateObjects(deleteQueries, deleteTables, updateQueries, updateFirstQueries, updateLastQueries, updateTables, insertQueries, insertTables, tableOperations, queryOrder, true, allQueriedObjects, dataObjectCache, extraProcessingList, connection, transaction, objects);

            extraProcessingList = await GenerateAuditForAggregatorsAsync(allQueriedObjects, dataObjectCache, dbTransactionWrapperAsync).ConfigureAwait(false);

            OnBeforeUpdateObjects(allQueriedObjects);

            // Сортируем объекты в порядке заданным графом связности.
            extraProcessingList.Sort((x, y) =>
            {
                int indexX = queryOrder.IndexOf(Information.GetClassStorageName(x.GetType()));
                int indexY = queryOrder.IndexOf(Information.GetClassStorageName(y.GetType()));
                return indexX.CompareTo(indexY);
            });

            AccessCheckBeforeUpdate(SecurityManager, allQueriedObjects);

            // Порядок выполнения запросов: delete, insert, update.
            if (deleteQueries.Count > 0 || updateQueries.Count > 0 || insertQueries.Count > 0)
            {
                Guid? operationUniqueId = null;

                if (NotifierUpdateObjects != null)
                {
                    operationUniqueId = Guid.NewGuid();
                    NotifierUpdateObjects.BeforeUpdateObjects(operationUniqueId.Value, this, dbTransactionWrapperAsync.Transaction, objects);
                }

                if (AuditService.IsAuditEnabled)
                {
                    /* Аудит проводится именно здесь, поскольку на этот момент все бизнес-сервера на объектах уже выполнились,
                     * объекты находятся именно в том состоянии, в каком должны были пойти в базу + в будущем можно транзакцию передать на исполнение
                     */
                    AuditService.WriteCommonAuditOperationWithAutoFields(extraProcessingList, auditOperationInfoList, this, true, dbTransactionWrapperAsync.Transaction); // TODO: подумать, как записывать аудит до OnBeforeUpdateObjects, но уже потенциально с транзакцией
                }

                string query = string.Empty;
                string prevQueries = string.Empty;
                object subTask = null;
                try
                {
                    Exception ex = null;
                    DbCommand command = dbTransactionWrapperAsync.CreateCommand();

                    // прошли вглубь обрабатывая only Update||Insert
                    bool go = true;
                    do
                    {
                        string table = queryOrder[0];
                        if (!tableOperations.ContainsKey(table))
                        {
                            tableOperations.Add(table, OperationType.None);
                        }

                        var ops = (OperationType)tableOperations[table];

                        if ((ops & OperationType.Delete) != OperationType.Delete && updateLastQueries.Count == 0)
                        {
                            // Смотрим есть ли Инсерты
                            if ((ops & OperationType.Insert) == OperationType.Insert)
                            {
                                if ((ex = await RunCommandsAsync(insertQueries, insertTables, table, command, id, alwaysThrowException).ConfigureAwait(false)) == null)
                                {
                                    ops = Minus(ops, OperationType.Insert);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    go = false;
                                }
                            }

                            // Смотрим есть ли Update
                            if (go && ((ops & OperationType.Update) == OperationType.Update))
                            {
                                if ((ex = await RunCommandsAsync(updateQueries, updateTables, table, command, id, alwaysThrowException).ConfigureAwait(false)) == null)
                                {
                                    ops = Minus(ops, OperationType.Update);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    go = false;
                                }
                            }

                            if (go)
                            {
                                queryOrder.RemoveAt(0);
                                go = queryOrder.Count > 0;
                            }
                        }
                        else
                        {
                            go = false;
                        }
                    }
                    while (go);

                    if (ex != null)
                    {
                        throw ex;
                    }

                    if (queryOrder.Count > 0)
                    {
                        // сзади чистые Update
                        go = true;
                        int queryOrderIndex = queryOrder.Count - 1;
                        do
                        {
                            string table = queryOrder[queryOrderIndex];
                            if (tableOperations.ContainsKey(table))
                            {
                                var ops = (OperationType)tableOperations[table];

                                if (ops == OperationType.Update && updateLastQueries.Count == 0)
                                {
                                    if ((ex = await RunCommandsAsync(updateQueries, updateTables, table, command, id, alwaysThrowException).ConfigureAwait(false)) == null)
                                    {
                                        ops = Minus(ops, OperationType.Update);
                                        tableOperations[table] = ops;
                                    }
                                    else
                                    {
                                        go = false;
                                    }

                                    if (go)
                                    {
                                        queryOrderIndex--;
                                        go = queryOrderIndex >= 0;
                                    }
                                }
                                else
                                {
                                    go = false;
                                }
                            }
                            else
                            {
                                queryOrderIndex--;
                            }
                        }
                        while (go);
                    }

                    if (ex != null)
                    {
                        throw ex;
                    }

                    foreach (string table in queryOrder)
                    {
                        await RunCommandsAsync(updateFirstQueries, updateTables, table, command, id, alwaysThrowException).ConfigureAwait(false);
                    }

                    // Удаляем в обратном порядке.
                    for (int i = queryOrder.Count - 1; i >= 0; i--)
                    {
                        string table = queryOrder[i];
                        await RunCommandsAsync(deleteQueries, deleteTables, table, command, id, alwaysThrowException).ConfigureAwait(false);
                    }

                    // А теперь опять с начала
                    foreach (string table in queryOrder)
                    {
                        await RunCommandsAsync(insertQueries, insertTables, table, command, id, alwaysThrowException).ConfigureAwait(false);
                        await RunCommandsAsync(updateQueries, updateTables, table, command, id, alwaysThrowException).ConfigureAwait(false);
                    }

                    foreach (string table in queryOrder)
                    {
                        await RunCommandsAsync(updateLastQueries, updateTables, table, command, id, alwaysThrowException).ConfigureAwait(false);
                    }

                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    {
                        // Нужно зафиксировать операции аудита (то есть сообщить, что всё было корректно выполнено и запомнить время)
                        AuditService.RatifyAuditOperationWithAutoFields(
                            tExecutionVariant.Executed,
                            AuditAdditionalInfo.SetNewFieldValuesForList(dbTransactionWrapperAsync.Transaction, this, auditOperationInfoList),
                            this,
                            true);
                    }

                    if (NotifierUpdateObjects != null)
                    {
                        NotifierUpdateObjects.AfterSuccessSqlUpdateObjects(operationUniqueId.Value, this, dbTransactionWrapperAsync.Transaction, objects);
                    }
                }
                catch (Exception excpt)
                {
                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    {
                        // Нужно зафиксировать операции аудита (то есть сообщить, что всё было откачено).
                        AuditService.RatifyAuditOperationWithAutoFields(tExecutionVariant.Failed, auditOperationInfoList, this, false);
                    }

                    if (NotifierUpdateObjects != null)
                    {
                        NotifierUpdateObjects.AfterFailUpdateObjects(operationUniqueId.Value, this, objects);
                    }

                    BusinessTaskMonitor.EndSubTask(subTask);
                    throw new ExecutingQueryException(query, prevQueries, excpt);
                }

                var res = new ArrayList();
                foreach (DataObject changedObject in objects)
                {
                    changedObject.ClearPrototyping(true);

                    if (changedObject.GetStatus(false) != ObjectStatus.Deleted)
                    {
                        Utils.UpdateInternalDataInObjects(changedObject, true, dataObjectCache);
                        res.Add(changedObject);
                    }
                }

                foreach (DataObject dobj in allQueriedObjects)
                {
                    if (dobj.GetStatus(false) != ObjectStatus.Deleted
                        && dobj.GetStatus(false) != ObjectStatus.UnAltered)
                    {
                        Utils.UpdateInternalDataInObjects(dobj, true, dataObjectCache);
                    }
                }

                if (NotifierUpdateObjects != null)
                {
                    NotifierUpdateObjects.AfterSuccessUpdateObjects(operationUniqueId.Value, this, objects);
                }

                objects = new DataObject[res.Count];
                res.CopyTo(objects);

                BusinessTaskMonitor.EndTask(id);
            }

            if (AfterUpdateObjects != null)
            {
                AfterUpdateObjects(this, new DataObjectsEventArgs(objects));
            }
        }

        protected virtual async Task<Exception> RunCommandsAsync(StringCollection queries, StringCollection tables,
            string table, DbCommand command,
            object businessID, bool alwaysThrowException)
        {
            int i = 0;
            bool res = true;
            Exception ex = null;
            while (i < queries.Count && ex == null)
            {
                if (tables[i] == table)
                {
                    string query = queries[i];
                    command.CommandText = query;
                    command.Parameters.Clear();
                    CustomizeCommand(command);
                    object subTask = BusinessTaskMonitor.BeginSubTask(query, businessID);
                    try
                    {
                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                        queries.RemoveAt(i);
                        tables.RemoveAt(i);
                    }
                    catch (Exception exc)
                    {
                        i++;
                        ex = new ExecutingQueryException(query, string.Empty, exc);
                    }

                    BusinessTaskMonitor.EndSubTask(subTask);
                }
                else
                {
                    i++;
                }
            }

            if (alwaysThrowException && ex != null)
            {
                return null;
                throw ex;
            }

            return ex;
        }

        protected virtual async Task<List<DataObject>> GenerateAuditForAggregatorsAsync(
            ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapper = null)
        {
            var auditObjects = new List<DataObject>();

            if (!AuditService.IsAuditEnabled)
            {
                return auditObjects;
            }

            var processingObjectsList = processingObjects.Cast<DataObject>().ToList();

            // Занесение агрегаторов обновляемых объектов в аудит, если они есть.
            foreach (var dataObject in processingObjectsList)
            {
                var dataObjectType = dataObject.GetType();

                var aggregatorPropertyName = Information.GetAgregatePropertyName(dataObjectType);
                if (string.IsNullOrEmpty(aggregatorPropertyName))
                {
                    continue;
                }

                Type aggregatorType = Information.GetPropertyType(dataObjectType, aggregatorPropertyName);

                string detailArrayPropertyName = Information.GetDetailArrayPropertyName(aggregatorType, dataObjectType);

                View aggregatorAuditView = AuditService.GetAuditViewByType(aggregatorType, tTypeOfAuditOperation.UPDATE);

                // Если данного детейла в представлении аудита агрегатора нет, значит и аудит агрегатора при
                // изменении этого детейла вести не нужно.
                if (aggregatorAuditView == null || !aggregatorAuditView.CheckPropname(detailArrayPropertyName, true))
                {
                    continue;
                }

                // Определение прежнего агрегатора (если объект только что создан, то его нет).
                DataObject oldAggregator = null;
                if (dataObject.GetStatus() != ObjectStatus.Created)
                {
                    oldAggregator =
                        (DataObject)Information.GetPropValueByName(dataObject.GetDataCopy(), aggregatorPropertyName);
                    if (oldAggregator == null)
                    {
                        // Загрузка агрегатора из БД. Производится во временный объект, так как в оригинальном объекте
                        // может храниться ссылка на новый агрегатор.
                        var tempObject = (DataObject)Activator.CreateInstance(dataObjectType);
                        tempObject.SetExistObjectPrimaryKey(dataObject.__PrimaryKey);
                        var tempView = new View { Name = "AggregatorLoadingView", DefineClassType = dataObjectType };
                        tempView.AddProperty(aggregatorPropertyName);

                        if (dbTransactionWrapper == null)
                        {
                            await LoadObjectAsync(tempObject, tempView, true, true, dataObjectCache).ConfigureAwait(false);
                        }
                        else
                        {
                            await LoadObjectByExtConnAsync(tempObject, tempView, true, false, dataObjectCache, dbTransactionWrapper.Connection, dbTransactionWrapper.Transaction)
                                .ConfigureAwait(false);
                        }

                        oldAggregator =
                            (DataObject)Information.GetPropValueByName(tempObject, aggregatorPropertyName);
                    }
                    else
                    {
                        // Для корректной обработки аудитом объект не должен являться копией данных, поэтому, если он был взят
                        // из копии, то нужно создать новый объект.
                        var tempPrimaryKey = oldAggregator.__PrimaryKey;
                        oldAggregator = (DataObject)Activator.CreateInstance(aggregatorType);
                        oldAggregator.SetExistObjectPrimaryKey(tempPrimaryKey);
                        oldAggregator.SetStatus(ObjectStatus.Altered);
                    }
                }

                // Определение нового агрегатора (если объект удален, то он нам не нужен).
                DataObject newAggregator = null;
                if (dataObject.GetStatus() != ObjectStatus.Deleted)
                {
                    newAggregator =
                        (DataObject)Information.GetPropValueByName(dataObject, aggregatorPropertyName)
                        ?? oldAggregator;
                }

                // Агрегаторы уже могут быть в списке аудита, надо это проверить.
                DataObject existingObj;
                if (newAggregator != null)
                {
                    if ((existingObj = GetDataObjectFromSearchedList(auditObjects, newAggregator)) != null)
                    {
                        newAggregator = existingObj;
                    }
                    else
                    {
                        auditObjects.Add(newAggregator);
                    }
                }

                if (oldAggregator != null)
                {
                    if ((existingObj = GetDataObjectFromSearchedList(auditObjects, oldAggregator)) != null)
                    {
                        oldAggregator = existingObj;
                    }
                    else
                    {
                        auditObjects.Add(oldAggregator);
                    }
                }

                // Загрузка детейлов в агрегаторы, если они еще не были загружены.
                var aggregatorView = new View
                {
                    Name = "DetailsLoadingView",
                    DefineClassType = aggregatorType,
                };

                DetailInView detailInView = aggregatorAuditView.GetDetail(detailArrayPropertyName);
                aggregatorView.AddDetailInView(detailInView.Name, detailInView.View, true);
                var aggregatorsToLoad = new List<DataObject>();

                if (oldAggregator != null && !oldAggregator.CheckLoadedProperty(detailArrayPropertyName))
                {
                    aggregatorsToLoad.Add(oldAggregator);
                }

                if (newAggregator != null && newAggregator != oldAggregator
                    && !newAggregator.CheckLoadedProperty(detailArrayPropertyName))
                {
                    aggregatorsToLoad.Add(newAggregator);
                }

                foreach (DataObject aggregator in aggregatorsToLoad)
                {
                    DataObject tempAggregator = (DataObject)Activator.CreateInstance(aggregatorType);
                    tempAggregator.SetExistObjectPrimaryKey(aggregator.__PrimaryKey);

                    if (dbTransactionWrapper == null)
                    {
                        await LoadObjectAsync(tempAggregator, aggregatorView, true, false, dataObjectCache).ConfigureAwait(false);
                    }
                    else
                    {
                        await LoadObjectByExtConnAsync(tempAggregator, aggregatorView, true, false, dataObjectCache, dbTransactionWrapper.Connection, dbTransactionWrapper.Transaction)
                            .ConfigureAwait(false);
                    }

                    DetailArray tempAggregatorDetailArray =
                        (DetailArray)Information.GetPropValueByName(tempAggregator, detailArrayPropertyName),
                        aggregatorDetailArray =
                            (DetailArray)Information.GetPropValueByName(aggregator, detailArrayPropertyName);
                    while (tempAggregatorDetailArray.Count > 0)
                    {
                        var detail = tempAggregatorDetailArray.ItemByIndex(0);
                        tempAggregatorDetailArray.RemoveByIndex(0);
                        aggregatorDetailArray.AddObject(detail);
                    }

                    aggregator.AddLoadedProperties(detailArrayPropertyName);
                }

                var oldAggregatorDetailArray = oldAggregator == null
                                                ? null
                                                : (DetailArray)Information.GetPropValueByName(oldAggregator, detailArrayPropertyName);
                var newAggregatorDetailArray = newAggregator == null
                                                ? null
                                                : (DetailArray)Information.GetPropValueByName(newAggregator, detailArrayPropertyName);

                DataObject deletedObj = oldAggregatorDetailArray != null ? oldAggregatorDetailArray.GetByKey(dataObject.__PrimaryKey) : null;
                switch (dataObject.GetStatus())
                {
                    case ObjectStatus.Altered:
                        if (oldAggregator != newAggregator && deletedObj != null)
                        {
                            deletedObj.SetStatus(ObjectStatus.Deleted);
                            oldAggregator.GetStatus(true);
                        }

                        if (dataObject.GetDetailArray() == null)
                        {
                            newAggregatorDetailArray.SetByKey(dataObject.__PrimaryKey, dataObject);
                        }

                        break;
                    case ObjectStatus.Deleted:
                        if (deletedObj != null)
                        {
                            deletedObj.SetStatus(ObjectStatus.Deleted);
                            oldAggregator.GetStatus(true);
                        }

                        break;
                    case ObjectStatus.Created:
                        if (dataObject.GetDetailArray() == null)
                        {
                            newAggregatorDetailArray.SetByKey(dataObject.__PrimaryKey, dataObject);
                        }

                        break;
                }

                if (newAggregator != null)
                {
                    newAggregator.GetStatus(true);
                }
            }

            return auditObjects;
        }
    }
}
