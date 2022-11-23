namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    using NewPlatform.Flexberry.ORM;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService, IAsyncDataService
    {
        /// <inheritdoc cref="IAsyncDataService.UpdateObjectsAsync(DataObject[], bool, DataObjectCache)"/>
        public virtual async Task UpdateObjectsAsync(DataObject[] objects, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null)
        {
            if (objects == null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            if (!objects.Any())
            {
                return;
            }

            dataObjectCache ??= new DataObjectCache();

            RunChangeCustomizationString(objects);

            using (DbTransactionWrapperAsync dbTransactionWrapper = new DbTransactionWrapperAsync(this))
            {
                try
                {
                    await UpdateObjectsByExtConnAsync(objects, dataObjectCache, alwaysThrowException, dbTransactionWrapper)
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

        /// <summary>
        /// Сохранение объектов данных.
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у обрабатываемых объектов обновляются в процессе работы.</i></remarks>
        /// <param name="objects">Объекты данных, которые требуется обновить.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <param name="alwaysThrowException">true - выбрасывать исключение при первой же ошибке. false - при ошибке в одном из запросов, остальные запросы всё равно будут выполнены; выбрасывается только последнее исключение в самом конце.</param>
        /// <param name="dbTransactionWrapperAsync">Используемые объект подключения и транзакция.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        public virtual async Task UpdateObjectsByExtConnAsync(DataObject[] objects, DataObjectCache dataObjectCache, bool alwaysThrowException, DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            if (objects == null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            if (!objects.Any())
            {
                return;
            }

            if (dbTransactionWrapperAsync == null)
            {
                throw new ArgumentNullException(nameof(dbTransactionWrapperAsync), "Не указан DbTransactionWrapperAsync. Обратитесь к разработчику.");
            }

            dataObjectCache ??= new DataObjectCache();

            object id = BusinessTaskMonitor.BeginTask("Update objects");

            var deleteQueries = new Dictionary<string, List<string>>();
            var updateQueries = new Dictionary<string, List<string>>();
            var updateFirstQueries = new Dictionary<string, List<string>>();
            var updateLastQueries = new Dictionary<string, List<string>>();
            var insertQueries = new Dictionary<string, List<string>>();

            var tableOperations = new SortedList();
            var queryOrder = new StringCollection();

            var allQueriedObjects = new ArrayList();

            var auditOperationInfoList = new List<AuditAdditionalInfo>();
            var extraProcessingList = new List<DataObject>();

            GenerateQueriesForUpdateObjects(deleteQueries, updateQueries, updateFirstQueries, updateLastQueries, insertQueries, tableOperations, queryOrder, true, allQueriedObjects, dataObjectCache, extraProcessingList, dbTransactionWrapperAsync, objects);

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

            var queryRunner = new QueryRunner(deleteQueries, updateQueries, updateFirstQueries, updateLastQueries, insertQueries, tableOperations, this);

            // Порядок выполнения запросов: delete, insert, update.
            if (deleteQueries.Count > 0 || updateQueries.Count > 0 || insertQueries.Count > 0)
            {
                Guid? operationUniqueId = null;

                if (NotifierUpdateObjects != null)
                {
                    operationUniqueId = Guid.NewGuid();
                    var transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
                    NotifierUpdateObjects.BeforeUpdateObjects(operationUniqueId.Value, this, transaction, objects);
                }

                if (AuditService.IsAuditEnabled)
                {
                    /* Аудит проводится именно здесь, поскольку на этот момент все бизнес-сервера на объектах уже выполнились,
                     * объекты находятся именно в том состоянии, в каком должны были пойти в базу + в будущем можно транзакцию передать на исполнение
                     */
                    var transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
                    AuditService.WriteCommonAuditOperationWithAutoFields(extraProcessingList, auditOperationInfoList, this, true, transaction); // TODO: подумать, как записывать аудит до OnBeforeUpdateObjects, но уже потенциально с транзакцией
                }

                string query = string.Empty;
                string prevQueries = string.Empty;
                object subTask = null;
                try
                {
                    await queryRunner.RunQueriesAsync(queryOrder, dbTransactionWrapperAsync, alwaysThrowException, id);

                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    {
                        // Нужно зафиксировать операции аудита (то есть сообщить, что всё было корректно выполнено и запомнить время)
                        var transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
                        AuditService.RatifyAuditOperationWithAutoFields(
                            tExecutionVariant.Executed,
                            AuditAdditionalInfo.SetNewFieldValuesForList(transaction, this, auditOperationInfoList),
                            this,
                            true);
                    }

                    if (NotifierUpdateObjects != null)
                    {
                        var transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
                        NotifierUpdateObjects.AfterSuccessSqlUpdateObjects(operationUniqueId.Value, this, transaction, objects);
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
    }
}
