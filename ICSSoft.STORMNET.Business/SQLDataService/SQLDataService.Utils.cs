namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;

    using NewPlatform.Flexberry.ORM;

    /// <summary>
    /// Содержит внутренние вспомогательные методы для работы SQLDataService.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService, IAsyncDataService
    {
        /// <summary>
        /// Выполнить запросы <paramref name="queries"/> на указанной таблице <paramref name="table"/>.
        /// </summary>
        /// <returns>Возникла ошибка - возвращается <see cref="ExecutingQueryException"/>. Сработало без ошибок - возвращается <see langword="null" />.</returns>
        protected virtual async Task<Exception> RunCommandsAsync(StringCollection queries, StringCollection tables, string table, DbCommand command, object businessID, bool alwaysThrowException)
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }

            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            int i = 0;
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

        /// <summary>
        /// Конвертировать результат вычитки методов Read/ReadAsync в массив объектов данных.
        /// </summary>
        /// <param name="result">Результат вычитки, который будет сконвертирован.</param>
        protected virtual void ConvertReadResult(object[][] result, DataObject[] dataObjects, LoadingCustomizationStruct customizationStruct, StorageStructForView[] storageStructs, SortedList allObjectKeys, SortedList readingKeys, bool clearDataObject, DataObjectCache dataObjectCache)
        {
            if (result != null && result.Length != 0)
            {
                DataObject[] loadobjects = new ICSSoft.STORMNET.DataObject[result.Length];
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
            SortedList _allObjectsKeys = new SortedList();
            SortedList _readingKeys = new SortedList();

            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;
            FunctionalLanguage.VariableDef var = new FunctionalLanguage.VariableDef(
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
                    _allObjectsKeys.Add(dotype.FullName + readingKey.ToString(), i);
                    _readingKeys.Add(readingKey.ToString(), dobject.__PrimaryKey);
                }
            }

            allObjectsKeys = _allObjectsKeys;
            readingKeys = _readingKeys;

            LoadingCustomizationStruct customizationStruct = new LoadingCustomizationStruct(GetInstanceId());
            FunctionalLanguage.Function func = lang.GetFunction(lang.funcIN, keys.ToArray());
            customizationStruct.Init(null, func, types.ToArray(), dataObjectView, null);

            return customizationStruct;
        }

        /// <summary>
        /// У основного представления есть связь на представление на детейлы. Часть из них вообще не загружалась, данная функция обрабатывает как раз их.
        /// Данная функция либо возвращает объекты детейлов, если есть навешенные на них бизнес-сервера,
        /// иначе формирует запрос на удаление всех детейлов определённого типа у объекта.
        /// </summary>
        /// <param name="view">Представление, соответствующее детейлу.</param>
        /// <param name="deleteDictionary">The delete dictionary.</param>
        /// <param name="mainkey">Первичный ключ агрегатора детейлов.</param>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="tableOperations">The table operations.</param>
        /// <param name="dataObjectCache">The data object cache.</param>
        /// <param name="dbTransactionWrapperAsync">Экземпляр <see cref="DbTransactionWrapperAsync" />.</param>
        /// <returns>Кортеж. Первое значение - набор объектов, которые необходимо занести в аудит. Второе - детейлы, на которые навешены бизнес-сервера (соответственно, их массово удалить нельзя, необходимо каждый пропустить через бизнес-сервер).</returns>
        private async Task<Tuple<IEnumerable<DataObject>, DataObject[]>> AddDeletedViewToDeleteDictionaryAsync(
            View view,
            IDictionary<string, List<string>> deleteDictionary,
            object mainkey,
            StringCollection deleteTables,
            SortedList tableOperations,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            List<DataObject> extraProcessingObjects = new List<DataObject>();
            DataObject[] updateObjects = new DataObject[0];
            string prkeyStorName = view.Properties[1].Name;

            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;

            FunctionalLanguage.VariableDef var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.KeyType(view.DefineClassType)), prkeyStorName);
            FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, mainkey);

            LoadingCustomizationStruct cs = new LoadingCustomizationStruct(GetInstanceId());

            cs.Init(new ColumnsSortDef[0], func, new Type[] { view.DefineClassType }, view, new string[0]);
            BusinessServer[] bs = BusinessServerProvider.GetBusinessServer(view.DefineClassType, DataServiceObjectEvents.OnDeleteFromStorage, this);

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
                            AddOpertaionOnTable(deleteTables, tableOperations, tableName, OperationType.Delete);
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
                        AddOpertaionOnTable(deleteTables, tableOperations, tableName, OperationType.Delete);
                    }

                    var prevDicValue = deleteDictionary[tableName];
                    prevDicValue.Add(selectQuery);
                }
            }

            return new Tuple<IEnumerable<DataObject>, DataObject[]>(extraProcessingObjects, updateObjects);
        }
    }
}
