namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Security;
    using NewPlatform.Flexberry.ORM;

    /// <summary>
    /// Содержит внутренние вспомогательные методы для работы SQLDataService.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService, IAsyncDataService
    {
        /// <summary>
        /// Выполнить запросы <paramref name="queries"/>./>.
        /// </summary>
        /// <param name="queries">Список запросов, которые нужно выполнить.</param>
        /// <param name="command">Команда, с помощью которой выполнится запрос.</param>
        /// <param name="businessID">ID операции (см. <see cref="BusinessTaskMonitor.BeginTask(string)"/>).</param>
        /// <param name="alwaysThrowException">true - выбрасывать исключение при первой же ошибке. false - при ошибке в одном из запросов, остальные запросы всё равно будут выполнены; выбрасывается только последнее исключение в самом конце.</param>
        /// <returns>Возникла ошибка - возвращается <see cref="ExecutingQueryException"/>. Сработало без ошибок - возвращается <see langword="null" />.</returns>
        internal virtual Exception RunCommands(IList<string> queries, System.Data.IDbCommand command, object businessID, bool alwaysThrowException)
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Exception ex = null;

            foreach (string query in queries)
            {
                command.CommandText = query;
                command.Parameters.Clear();
                CustomizeCommand(command);
                object subTask = BusinessTaskMonitor.BeginSubTask(query, businessID);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    ex = new ExecutingQueryException(query, string.Empty, exc);
                    break;
                }

                BusinessTaskMonitor.EndSubTask(subTask);
            }

            if (alwaysThrowException && ex != null)
            {
                throw ex;
            }

            return ex;
        }

        /// <summary>
        /// Выполнить запросы <paramref name="queries"/>./> (асинхронно).
        /// </summary>
        /// <param name="queries">Список запросов, которые нужно выполнить.</param>
        /// <param name="command">Команда, с помощью которой выполнится запрос.</param>
        /// <param name="businessID">ID операции (см. <see cref="BusinessTaskMonitor.BeginTask(string)"/>).</param>
        /// <param name="alwaysThrowException">true - выбрасывать исключение при первой же ошибке. false - при ошибке в одном из запросов, остальные запросы всё равно будут выполнены; выбрасывается только последнее исключение в самом конце.</param>
        /// <returns>Возникла ошибка - возвращается <see cref="ExecutingQueryException"/>. Сработало без ошибок - возвращается <see langword="null" />.</returns>
        internal virtual async Task<Exception> RunCommandsAsync(IList<string> queries, DbCommand command, object businessID, bool alwaysThrowException)
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Exception ex = null;

            foreach (string query in queries)
            {
                command.CommandText = query;
                command.Parameters.Clear();
                CustomizeCommand(command);
                object subTask = BusinessTaskMonitor.BeginSubTask(query, businessID);
                try
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                    ex = new ExecutingQueryException(query, string.Empty, exc);
                    break;
                }

                BusinessTaskMonitor.EndSubTask(subTask);
            }

            if (alwaysThrowException && ex != null)
            {
                throw ex;
            }

            return ex;
        }

        /// <summary>
        /// Сгенерировать объекты для учета аудита агрегаторов обновляемых объектов, если они обновляются отдельно от агрегатора.
        /// </summary>
        /// <param name="processingObjects">Объекты, которые необходимо обработать.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapperAsync" />.</param>
        /// <returns><see cref="Task"/> - результат операции содержит объекты данных для подсистемы аудита.</returns>
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
                            await LoadObjectByExtConnAsync(tempObject, tempView, true, false, dataObjectCache, dbTransactionWrapper)
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
                        await LoadObjectByExtConnAsync(tempAggregator, aggregatorView, true, false, dataObjectCache, dbTransactionWrapper)
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

                DataObject deletedObj = oldAggregatorDetailArray?.GetByKey(dataObject.__PrimaryKey);
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

        /// <summary>
        /// Конвертировать результат вычитки методов Read/ReadAsync в массив объектов данных.
        /// </summary>
        /// <param name="result">Результат вычитки, который будет сконвертирован.</param>
        /// <param name="dataObjects">исходные объекты.</param>
        /// <param name="customizationStruct">настройка выборки.</param>
        /// <param name="storageStructs">Коллекция структур для отображения представления в данные.</param>
        /// <param name="allObjectKeys">Вспомогательная структура для дальнейшей вычитки.</param>
        /// <param name="readingKeys">Вспомогательная структура для дальнеишей вычитки.</param>
        /// <param name="clearDataObject">очищать ли существующие.</param>
        /// <param name="dataObjectCache">Кеш.</param>
        protected virtual void ConvertReadResult(
            object[][] result,
            DataObject[] dataObjects,
            LoadingCustomizationStruct customizationStruct,
            StorageStructForView[] storageStructs,
            SortedList allObjectKeys,
            SortedList readingKeys,
            bool clearDataObject,
            DataObjectCache dataObjectCache)
        {
            if (result == null || result.Length == 0)
            {
                return;
            }

            if (dataObjects == null || dataObjects.Length == 0)
            {
                return;
            }

            if (customizationStruct == null)
            {
                throw new ArgumentNullException(nameof(customizationStruct));
            }

            if (storageStructs == null)
            {
                throw new ArgumentNullException(nameof(storageStructs));
            }

            if (allObjectKeys == null)
            {
                throw new ArgumentNullException(nameof(allObjectKeys));
            }

            if (readingKeys == null)
            {
                throw new ArgumentNullException(nameof(readingKeys));
            }

            dataObjectCache ??= new DataObjectCache();

            DataObject[] loadobjects = new DataObject[result.Length];
            int objectTypeIndexPOs = result[0].Length - 1;
            int keyIndex = storageStructs[0].props.Length - 1;
            while (storageStructs[0].props[keyIndex].MultipleProp)
            {
                keyIndex--;
            }

            keyIndex++;

            for (int i = 0; i < result.Length; i++)
            {
                Type tp = customizationStruct.LoadingTypes[Convert.ToInt64(result[i][objectTypeIndexPOs].ToString())];
                object ky = result[i][keyIndex];
                ky = Information.TranslateValueToPrimaryKeyType(tp, ky);
                int indexobj = allObjectKeys.IndexOfKey(tp.FullName + ky.ToString());
                if (indexobj > -1)
                {
                    loadobjects[i] = dataObjects[(int)allObjectKeys.GetByIndex(indexobj)];
                    if (clearDataObject)
                    {
                        loadobjects[i].Clear();
                    }

                    dataObjectCache.AddDataObject(loadobjects[i]);
                }
                else
                {
                    loadobjects[i] = null;
                }
            }

            Utils.ProcessingRowsetDataRef(result, customizationStruct.LoadingTypes, storageStructs, customizationStruct, loadobjects, this, Types, clearDataObject, dataObjectCache, SecurityManager);
            foreach (DataObject dobj in loadobjects)
            {
                if (dobj != null && dobj.Prototyped)
                {
                    dobj.__PrimaryKey = readingKeys[dobj.__PrimaryKey.ToString()];
                    dobj.SetStatus(ObjectStatus.Created);
                    dobj.SetLoadingState(LoadingState.NotLoaded);
                }
            }
        }

        /// <summary>
        /// Сгенерировать <see cref="LoadingCustomizationStruct"/> - результат представляет собой ограничение "Один из переданных объектов данных".
        /// Используется в дальнейшем для генерации SQL.
        /// </summary>
        /// <param name="dataObjects">Объекты данных, по которым будет генерироваться <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="dataObjectView">Представление, по которому будет генерироваться <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="allObjectsKeys">Вспомогательная структура для дальнейшей вычитки.</param>
        /// <param name="readingKeys">Вспомогательная структура для дальнеишей вычитки.</param>
        /// <returns>Итоговое ограничение.</returns>
        protected virtual LoadingCustomizationStruct GetCustomizationStruct(DataObject[] dataObjects, View dataObjectView, out SortedList allObjectsKeys, out SortedList readingKeys)
        {
            if (dataObjects == null)
            {
                throw new ArgumentNullException(nameof(dataObjects));
            }

            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView), "Не указано представление для вычитки объектов.");
            }

            List<Type> types = new List<Type>();
            List<object> keys = new List<object>();
            allObjectsKeys = new SortedList();
            readingKeys = new SortedList();

            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;
            VariableDef var = new VariableDef(
                lang.GetObjectTypeForNetType(KeyGenerator.KeyType(dataObjectView.DefineClassType)), SQLWhereLanguageDef.StormMainObjectKey);
            keys.Add(var);

            for (int i = 0; i < dataObjects.Length; i++)
            {
                DataObject dobject = dataObjects[i];
                Type dotype = dobject.GetType();
                bool addobj = false;
                if (types.Contains(dotype))
                {
                    addobj = true;
                }
                else
                {
                    if ((dotype == dataObjectView.DefineClassType || dotype.IsSubclassOf(dataObjectView.DefineClassType)) && Information.IsStoredType(dotype))
                    {
                        types.Add(dotype);
                        addobj = true;
                    }
                }

                if (addobj)
                {
                    object readingKey = dobject.Prototyped ? dobject.__PrototypeKey : dobject.__PrimaryKey;
                    keys.Add(readingKey);
                    allObjectsKeys.Add(dotype.FullName + readingKey.ToString(), i);
                    readingKeys.Add(readingKey.ToString(), dobject.__PrimaryKey);
                }
            }

            LoadingCustomizationStruct customizationStruct = new LoadingCustomizationStruct(GetInstanceId());
            Function func = lang.GetFunction(lang.funcIN, keys.ToArray());
            customizationStruct.Init(null, func, types.ToArray(), dataObjectView, null);

            return customizationStruct;
        }

        /// <summary>
        /// Проверка прав пользователя перед изменением объектов (выбрасывает исключение если доступ закрыт).
        /// </summary>
        /// <param name="securityManager">Менеджер полномочий, выполняющий проверку.</param>
        /// <param name="dataObjects">Изменённые объекты (проверяются права на изменение этих объектов).</param>
        public static void AccessCheckBeforeUpdate(ISecurityManager securityManager, ArrayList dataObjects)
        {
            if (securityManager == null)
            {
                throw new ArgumentNullException(nameof(securityManager));
            }

            foreach (DataObject dtob in dataObjects)
            {
                Type dobjType = dtob.GetType();
                if (!securityManager.AccessObjectCheck(dobjType, tTypeAccess.Full, false))
                {
                    switch (dtob.GetStatus(false))
                    {
                        case ObjectStatus.Created:
                            securityManager.AccessObjectCheck(dobjType, tTypeAccess.Insert, true);
                            break;
                        case ObjectStatus.Altered:
                            securityManager.AccessObjectCheck(dobjType, tTypeAccess.Update, true);
                            break;
                        case ObjectStatus.Deleted:
                            securityManager.AccessObjectCheck(dobjType, tTypeAccess.Delete, true);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Данная функция либо возвращает объекты детейлов, если есть навешенные на них бизнес-сервера,
        /// иначе формирует запрос на удаление всех детейлов определённого типа у объекта.
        /// </summary>
        /// <param name="view">Представление, соответствующее детейлу.</param>
        /// <param name="deleteDictionary">Запросы на удаление.</param>
        /// <param name="mainkey">Первичный ключ агрегатора детейлов.</param>
        /// <param name="tableOperations">Список операций над таблицами.</param>
        /// <param name="dataObjectCache">Кеш.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapperAsync" />.</param>
        /// <returns>Кортеж. Первое значение - набор объектов, которые необходимо занести в аудит. Второе - детейлы, на которые навешены бизнес-сервера (соответственно, их массово удалить нельзя, необходимо каждый пропустить через бизнес-сервер).</returns>
        private Tuple<IEnumerable<DataObject>, DataObject[]> AddDeletedViewToDeleteDictionary(
            View view,
            IDictionary<string, List<string>> deleteDictionary,
            object mainkey,
            SortedList tableOperations,
            DataObjectCache dataObjectCache,
            DbTransactionWrapper dbTransactionWrapper)
        {
            List<DataObject> extraProcessingObjects = new List<DataObject>();
            DataObject[] updateObjects = new DataObject[0];
            string prkeyStorName = view.Properties[1].Name;

            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;

            VariableDef var = new VariableDef(
                lang.GetObjectTypeForNetType(KeyGenerator.KeyType(view.DefineClassType)), prkeyStorName);
            Function func = lang.GetFunction(lang.funcEQ, var, mainkey);

            LoadingCustomizationStruct cs = new LoadingCustomizationStruct(GetInstanceId());

            cs.Init(new ColumnsSortDef[0], func, new Type[] { view.DefineClassType }, view, new string[0]);
            object state = null;
            BusinessServer[] bs = BusinessServerProvider.GetBusinessServer(view.DefineClassType, ObjectStatus.Deleted, this);
            if (bs != null && bs.Length > 0)
            {
                // Если на детейловые объекты навешены бизнес-сервера, то тогда детейлы будут подгружены
                updateObjects = LoadObjectsByExtConn(cs, ref state, dataObjectCache, dbTransactionWrapper);
            }
            else
            {
                if (AuditService.IsTypeAuditable(view.DefineClassType))
                {
                    /* Аудиту необходимо зафиксировать удаление детейлов.
                    * Здесь в аудит идут уже актуальные детейлы, поскольку на них нет бизнес-серверов,
                    * а бизнес-сервера основного объекта уже выполнились.
                    */
                    DataObject[] detailObjects = LoadObjectsByExtConn(cs, ref state, dataObjectCache, dbTransactionWrapper);
                    if (detailObjects != null)
                    {
                        foreach (var detailObject in detailObjects)
                        {
                            // Мы будем сии детейлы удалять, поэтому им необходимо проставить соответствующий статус
                            detailObject.SetStatus(ObjectStatus.Deleted);
                        }

                        extraProcessingObjects.AddRange(detailObjects);
                    }
                }

                string sq = GenerateSQLSelect(cs, false);

                if (StorageType == StorageTypeEnum.HierarchicalStorage)
                {
                    Type[] types = Information.GetCompatibleTypesForTypeConvertion(view.DefineClassType);
                    for (int i = 0; i < types.Length; i++)
                    {
                        string tableName = Information.GetClassStorageName(types[i]);
                        AddToDeleteDictionary(deleteDictionary, tableName, view, sq, tableOperations);
                    }
                }
                else
                {
                    string tableName = Information.GetClassStorageName(view.DefineClassType);
                    AddToDeleteDictionary(deleteDictionary, tableName, view, sq, tableOperations);
                }
            }

            return new Tuple<IEnumerable<DataObject>, DataObject[]>(extraProcessingObjects, updateObjects);
        }

        private void AddToDeleteDictionary(IDictionary<string, List<string>> deleteDictionary, string tableName, View view, string innerSelectQuery, SortedList tableOperations)
        {
            StringBuilder sb = new StringBuilder();
            var pkIdentifier = Information.GetPrimaryKeyStorageName(view.DefineClassType);
            sb.Append(PutIdentifierIntoBrackets(pkIdentifier));
            sb.Append(" IN ( SELECT ");
            sb.Append(PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey));
            sb.AppendFormat(" FROM ({0} ) a )", innerSelectQuery);
            string selectQuery = sb.ToString();

            if (!deleteDictionary.ContainsKey(tableName))
            {
                deleteDictionary.Add(tableName, new List<string>());
                AddOperationOnTable(tableOperations, tableName, OperationType.Delete);
            }

            var prevDicValue = deleteDictionary[tableName];
            prevDicValue.Add(selectQuery);
        }

        /// <summary>
        /// У основного представления есть связь на представление на детейлы. Часть из них вообще не загружалась, данная функция обрабатывает как раз их.
        /// Данная функция либо возвращает объекты детейлов, если есть навешенные на них бизнес-сервера,
        /// иначе формирует запрос на удаление всех детейлов определённого типа у объекта.
        /// </summary>
        /// <param name="view">Представление, соответствующее детейлу.</param>
        /// <param name="deleteDictionary">Запросы на удаление.</param>
        /// <param name="mainkey">Первичный ключ агрегатора детейлов.</param>
        /// <param name="tableOperations">Список операций над таблицами.</param>
        /// <param name="dataObjectCache">Кеш.</param>
        /// <param name="dbTransactionWrapperAsync">Экземпляр <see cref="DbTransactionWrapperAsync" />.</param>
        /// <returns>Кортеж. Первое значение - набор объектов, которые необходимо занести в аудит. Второе - детейлы, на которые навешены бизнес-сервера (соответственно, их массово удалить нельзя, необходимо каждый пропустить через бизнес-сервер).</returns>
        private async Task<Tuple<IEnumerable<DataObject>, DataObject[]>> AddDeletedViewToDeleteDictionaryAsync(
            View view,
            IDictionary<string, List<string>> deleteDictionary,
            object mainkey,
            SortedList tableOperations,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            List<DataObject> extraProcessingObjects = new List<DataObject>();
            DataObject[] updateObjects = new DataObject[0];
            string prkeyStorName = view.Properties[1].Name;

            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;

            VariableDef var = new VariableDef(
                lang.GetObjectTypeForNetType(KeyGenerator.KeyType(view.DefineClassType)), prkeyStorName);
            Function func = lang.GetFunction(lang.funcEQ, var, mainkey);

            LoadingCustomizationStruct cs = new LoadingCustomizationStruct(GetInstanceId());

            cs.Init(new ColumnsSortDef[0], func, new Type[] { view.DefineClassType }, view, new string[0]);
            BusinessServer[] bs = BusinessServerProvider.GetBusinessServer(view.DefineClassType, ObjectStatus.Deleted, this);

            if (bs != null && bs.Length > 0)
            {
                // Если на детейловые объекты навешены бизнес-сервера, то тогда детейлы будут подгружены
                updateObjects = await LoadObjectsByExtConnAsync(cs, dataObjectCache, dbTransactionWrapperAsync)
                        .ConfigureAwait(false);
            }
            else
            {
                if (AuditService.IsTypeAuditable(view.DefineClassType))
                {
                    /* Аудиту необходимо зафиксировать удаление детейлов.
                    * Здесь в аудит идут уже актуальные детейлы, поскольку на них нет бизнес-серверов,
                    * а бизнес-сервера основного объекта уже выполнились.
                    */
                    DataObject[] detailObjects = await LoadObjectsByExtConnAsync(cs, dataObjectCache, dbTransactionWrapperAsync)
                        .ConfigureAwait(false);

                    if (detailObjects != null)
                    {
                        foreach (var detailObject in detailObjects)
                        {
                            // Мы будем сии детейлы удалять, поэтому им необходимо проставить соответствующий статус
                            detailObject.SetStatus(ObjectStatus.Deleted);
                        }

                        extraProcessingObjects.AddRange(detailObjects);
                    }
                }

                string sq = GenerateSQLSelect(cs, false);

                if (StorageType == StorageTypeEnum.HierarchicalStorage)
                {
                    Type[] types = Information.GetCompatibleTypesForTypeConvertion(view.DefineClassType);
                    for (int i = 0; i < types.Length; i++)
                    {
                        string tableName = Information.GetClassStorageName(types[i]);
                        string selectQuery = PutIdentifierIntoBrackets(Information.GetPrimaryKeyStorageName(view.DefineClassType)) + " IN ( SELECT " + PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey) + " FROM (" + sq + " ) a )";
                        if (!deleteDictionary.ContainsKey(tableName))
                        {
                            deleteDictionary.Add(tableName, new List<string>());
                            AddOperationOnTable(tableOperations, tableName, OperationType.Delete);
                        }

                        var prevDicValue = deleteDictionary[tableName];
                        prevDicValue.Add(selectQuery);
                    }
                }
                else
                {
                    string tableName = Information.GetClassStorageName(view.DefineClassType);
                    string selectQuery = PutIdentifierIntoBrackets(Information.GetPrimaryKeyStorageName(view.DefineClassType)) + " IN ( SELECT " + PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey) + " FROM (" + sq + " ) a )";
                    if (!deleteDictionary.ContainsKey(tableName))
                    {
                        deleteDictionary.Add(tableName, new List<string>());
                        AddOperationOnTable(tableOperations, tableName, OperationType.Delete);
                    }

                    var prevDicValue = deleteDictionary[tableName];
                    prevDicValue.Add(selectQuery);
                }
            }

            return new Tuple<IEnumerable<DataObject>, DataObject[]>(extraProcessingObjects, updateObjects);
        }

        /// <summary>
        /// Добавить ограничение доступа (у текущего пользователя) к lcs.
        /// </summary>
        /// <param name="lc">Lcs, к которому добавится ограничение.</param>
        private void ApplyLimitForAccess(LoadingCustomizationStruct lc)
        {
            object limitObject;
            bool canAccess;
            var operationResult = SecurityManager.GetLimitForAccess(lc.View.DefineClassType, tTypeAccess.Read, out limitObject, out canAccess);
            Function limit = limitObject as Function;
            if (operationResult == OperationResult.Успешно)
            {
                if (limit != null)
                {
                    SQLWhereLanguageDef ldef = SQLWhereLanguageDef.LanguageDef;
                    lc.LimitFunction = ldef.GetFunction(ldef.funcAND, lc.LimitFunction, limit);
                }
            }
            else
            {
                // TODO: тут надо подумать что будем делать. Наверное надо вызывать исключение и не давать ничего. Пока просто запишем в лог и не будем показывать ошибку.
                LogService.LogError(string.Format("SecurityManager.GetLimitForAccess: {0}", operationResult));
            }
        }

        /// <summary>
        /// Получить ограничение по первичному ключу для объекта данных.
        /// </summary>
        /// <param name="dataObject">Объект данных.</param>
        /// <param name="dataObjectView">Представление для ограничения.</param>
        /// <returns>Ограничение по первичному ключу.</returns>
        private LoadingCustomizationStruct GetLcsPrimaryKey(DataObject dataObject, View dataObjectView)
        {
            var doType = dataObject.GetType();

            LoadingCustomizationStruct lc = new LoadingCustomizationStruct(GetInstanceId());
            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;
            VariableDef var = new VariableDef(
                lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.KeyType(doType)), SQLWhereLanguageDef.StormMainObjectKey);
            object readingkey = dataObject.__PrimaryKey;
            if (dataObject.Prototyped)
            {
                readingkey = dataObject.__PrototypeKey;
            }

            Function func = lang.GetFunction(lang.funcEQ, var, readingkey);

            lc.Init(new ColumnsSortDef[0], func, new Type[] { doType }, dataObjectView, new string[0]);

            return lc;
        }

        /// <summary>
        /// Изменить строку соединения, согласно делегату <see cref="ChangeCustomizationString"/>.
        /// </summary>
        /// <param name="dataObjects">Загружаемые объекты - по списку их типов будет изменена строка соединения.</param>
        private void RunChangeCustomizationString(DataObject[] dataObjects)
        {
            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                var types = dataObjects.Select(x => x.GetType()).Distinct().ToArray();
                string cs = ChangeCustomizationString(types);
                CustomizationString = string.IsNullOrEmpty(cs) ? CustomizationString : cs;
            }
        }

        /// <summary>
        /// Изменить строку соединения, согласно делегату <see cref="ChangeCustomizationString"/>.
        /// </summary>
        /// <param name="types">Типы загружаемых объектов - по ним будет изменена строка соединения.</param>
        private void RunChangeCustomizationString(Type[] types)
        {
            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                string cs = ChangeCustomizationString(types);
                CustomizationString = string.IsNullOrEmpty(cs) ? CustomizationString : cs;
            }
        }
    }
}
