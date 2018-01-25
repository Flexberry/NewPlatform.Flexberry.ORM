namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.Security;

    using SortedList = System.Collections.SortedList;
    using STORMDO = ICSSoft.STORMNET;
    using StringCollection = System.Collections.Specialized.StringCollection;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService
    {
        /// <summary>
        /// Обновить хранилище по объектам (есть параметр, указывающий, всегда ли необходимо взводить ошибку
        /// и откатывать транзакцию при неудачном запросе в базу данных). Если
        /// он true, всегда взводится ошибка. Иначе, выполнение продолжается.
        /// Однако, при этом есть опасность преждевременного окончания транзакции, с переходом для остальных
        /// запросов режима транзакционности в autocommit. Проявлением проблемы являются ошибки навроде:
        /// The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION
        /// </summary>
        /// <param name="objects">Объекты для обновления.</param>
        /// <param name="DataObjectCache">Кэш объектов данных.</param>
        /// <param name="AlwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        public virtual void UpdateObjects(ref DataObject[] objects, DataObjectCache DataObjectCache, bool AlwaysThrowException)
        {
            DataObjectCache.StartCaching(false);
            try
            {
                foreach (DataObject dobj in objects)
                    DataObjectCache.AddDataObject(dobj);

                object id = BusinessTaskMonitor.BeginTask("Update objects");
                if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
                {
                    var tps = new List<Type>();
                    foreach (DataObject d in objects)
                    {
                        Type t = d.GetType();
                        if (!tps.Contains(t))
                        {
                            tps.Add(t);
                        }
                    }

                    string cs = ChangeCustomizationString(tps.ToArray());
                    customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
                }

                // Перенесли этот метод повыше, потому что строка соединения может быть сменена в бизнес-сервере делегатом смены строки соединения (если что-нибудь почитают).
                IDbConnection conection = GetConnection();

                var DeleteQueries = new StringCollection();
                var UpdateQueries = new StringCollection();
                var InsertQueries = new StringCollection();

                var DeleteTables = new StringCollection();
                var UpdateTables = new StringCollection();
                var InsertTables = new StringCollection();
                var TableOperations = new SortedList();
                var QueryOrder = new StringCollection();

                var AllQueriedObjects = new ArrayList();

                var auditOperationInfoList = new List<AuditAdditionalInfo>();
                var extraProcessingList = new List<DataObject>();
                GenerateQueriesForUpdateObjects(DeleteQueries, DeleteTables, UpdateQueries, UpdateTables, InsertQueries, InsertTables, TableOperations, QueryOrder, true, AllQueriedObjects, DataObjectCache, extraProcessingList, objects);

                GenerateAuditForAggregators(AllQueriedObjects, DataObjectCache, ref extraProcessingList);

                Object BeforeEventTag = OnBeforeUpdateObjects(AllQueriedObjects);

                Exception ex = null;

                /*access checks*/

                foreach (DataObject dtob in AllQueriedObjects)
                {
                    Type dobjType = dtob.GetType();
                    if (!SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Full, false))
                    {
                        switch (dtob.GetStatus(false))
                        {
                            case ObjectStatus.Created:
                                SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Insert, true);
                                break;
                            case ObjectStatus.Altered:
                                SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Update, true);
                                break;
                            case ObjectStatus.Deleted:
                                SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Delete, true);
                                break;
                        }
                    }
                }

                /*access checks*/

                if (DeleteQueries.Count > 0 || UpdateQueries.Count > 0 || InsertQueries.Count > 0)
                { // Порядок выполнения запросов: delete,insert,update
                    if (AuditService.IsAuditEnabled)
                    {
                        /* Аудит проводится именно здесь, поскольку на этот момент все бизнес-сервера на объектах уже выполнились,
                         * объекты находятся именно в том состоянии, в каком должны были пойти в базу + в будущем можно транзакцию передать на исполнение
                         */
                        AuditOperation(extraProcessingList, auditOperationInfoList); // TODO: подумать, как записывать аудит до OnBeforeUpdateObjects, но уже потенциально с транзакцией
                    }

                    conection.Open();
                    IDbTransaction trans = null;

                    string query = string.Empty;
                    string prevQueries = string.Empty;
                    object subTask = null;
                    try
                    {
                        trans = CreateTransaction(conection);
                        IDbCommand command = conection.CreateCommand();
                        command.Transaction = trans;

                        #region прошли вглубь обрабатывая only Update||Insert
                        bool go = true;
                        do
                        {
                            string table = QueryOrder[0];
                            if (!TableOperations.ContainsKey(table))
                                TableOperations.Add(table, OperationType.None);
                            var ops = (OperationType)TableOperations[table];

                            if ((ops & OperationType.Delete) != OperationType.Delete)
                            {
                                // Смотрим есть ли Инсерты
                                if ((ops & OperationType.Insert) == OperationType.Insert)
                                {
                                    if (
                                        (ex =
                                         RunCommands(InsertQueries, InsertTables, table, command, id, AlwaysThrowException))
                                        == null)
                                    {
                                        ops = Minus(ops, OperationType.Insert);
                                        TableOperations[table] = ops;
                                    }
                                    else
                                    {
                                        go = false;
                                    }
                                }

                                // Смотрим есть ли Update
                                if (go && ((ops & OperationType.Update) == OperationType.Update))
                                {
                                    if ((ex = RunCommands(UpdateQueries, UpdateTables, table, command, id, AlwaysThrowException)) == null)
                                    {
                                        ops = Minus(ops, OperationType.Update);
                                        TableOperations[table] = ops;
                                    }
                                    else
                                    {
                                        go = false;
                                    }
                                }

                                if (go)
                                {
                                    QueryOrder.RemoveAt(0);
                                    go = QueryOrder.Count > 0;
                                }
                            }
                            else
                                go = false;

                        }
                        while (go);

                        #endregion
                        if (QueryOrder.Count > 0)
                        {
                            #region сзади чистые Update

                            go = true;
                            int queryOrderIndex = QueryOrder.Count - 1;
                            do
                            {
                                string table = QueryOrder[queryOrderIndex];
                                if (TableOperations.ContainsKey(table))
                                {
                                    var ops = (OperationType)TableOperations[table];

                                    if (ops == OperationType.Update)
                                    {
                                        if (
                                            (ex = RunCommands(UpdateQueries, UpdateTables, table, command, id, AlwaysThrowException)) == null)
                                        {
                                            ops = Minus(ops, OperationType.Update);
                                            TableOperations[table] = ops;
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
                                        go = false;
                                }
                                else
                                    queryOrderIndex--;
                            }
                            while (go);

                            #endregion
                        }

                        // Удаляем в обратном порядке.
                        for (int i = QueryOrder.Count - 1; i >= 0; i--)
                        {
                            string table = QueryOrder[i];
                            if ((ex = RunCommands(DeleteQueries, DeleteTables, table, command, id, AlwaysThrowException)) != null)
                                throw ex;
                        }

                        // А теперь опять с начала
                        foreach (string table in QueryOrder)
                        {
                            if ((ex = RunCommands(InsertQueries, InsertTables, table, command, id, AlwaysThrowException)) != null)
                                throw ex;
                            if ((ex = RunCommands(UpdateQueries, UpdateTables, table, command, id, AlwaysThrowException)) != null)
                                throw ex;
                        }

                        if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                        { // Нужно зафиксировать операции аудита (то есть сообщить, что всё было корректно выполнено и запомнить время)
                            AuditService.RatifyAuditOperationWithAutoFields(
                                tExecutionVariant.Executed,
                                AuditAdditionalInfo.SetNewFieldValuesForList(trans, this, auditOperationInfoList),
                                this,
                                true);
                        }

                        if (trans != null)
                            trans.Commit();
                    }
                    catch (Exception excpt)
                    {
                        if (trans != null)
                            trans.Rollback();
                        if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                        { // Нужно зафиксировать операции аудита (то есть сообщить, что всё было откачено)
                            AuditService.RatifyAuditOperationWithAutoFields(tExecutionVariant.Failed, auditOperationInfoList, this, false);
                        }

                        conection.Close();
                        BusinessTaskMonitor.EndSubTask(subTask);
                        throw new ExecutingQueryException(query, prevQueries, excpt);
                    }

                    conection.Close();

                    var res = new ArrayList();
                    foreach (DataObject changedObject in objects)
                    {
                        changedObject.ClearPrototyping(true);

                        if (changedObject.GetStatus(false) != STORMDO.ObjectStatus.Deleted)
                        {
                            Utils.UpdateInternalDataInObjects(changedObject, true, DataObjectCache);
                            res.Add(changedObject);
                        }
                    }

                    foreach (DataObject dobj in AllQueriedObjects)
                    {
                        if (dobj.GetStatus(false) != STORMDO.ObjectStatus.Deleted
                            && dobj.GetStatus(false) != STORMDO.ObjectStatus.UnAltered)
                            Utils.UpdateInternalDataInObjects(dobj, true, DataObjectCache);
                    }

                    objects = new DataObject[res.Count];
                    res.CopyTo(objects);
                    BusinessTaskMonitor.EndTask(id);
                }

                if (AfterUpdateObjects != null)
                    AfterUpdateObjects(this, new DataObjectsEventArgs(objects, BeforeEventTag));
            }
            finally
            {
                DataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Обновить хранилище по объектам. При ошибках делается попытка возобновления транзакции с другого запроса, 
        /// т.к. предполагается, что запросы должны быть выполнены в другом порядке.
        /// </summary>
        /// <param name="objects">Объекты данных для обновления.</param>
        /// <param name="DataObjectCache">Кэш объектов данных.</param>
        public virtual void UpdateObjects(ref DataObject[] objects, DataObjectCache DataObjectCache)
        {
            UpdateObjects(ref objects, DataObjectCache, false);
        }

        /// <summary>
        /// Обновить хранилище по объектам.
        /// </summary>
        /// <param name="objects">Объекты данных для обновления.</param>
        public virtual void UpdateObjects(ref DataObject[] objects)
        {
            UpdateObjects(ref objects, new DataObjectCache());
        }

        /// <summary>
        /// Обновить хранилище по объектам.
        /// </summary>
        /// <param name="objects">Объекты данных для обновления.</param>
        /// <param name="AlwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        public virtual void UpdateObjects(ref DataObject[] objects, bool AlwaysThrowException)
        {
            UpdateObjects(ref objects, new DataObjectCache(), AlwaysThrowException);
        }
    }
}
