namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;
    using NewPlatform.Flexberry.ORM;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : IAsyncDataService
    {
        /// <summary>
        /// Функция должна возвращать соединение <see cref="DbConnection"/>.
        /// </summary>
        /// <returns>Соединение <see cref="DbConnection"/>.</returns>
        public abstract DbConnection GetDbConnection();

        /// <inheritdoc cref="IAsyncDataService.GetObjectsCountAsync(LoadingCustomizationStruct)"/>
        public virtual async Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct)
        {
            RunChangeCustomizationString(customizationStruct.LoadingTypes);

            // Применим полномочия на строки
            ApplyReadPermissions(customizationStruct, SecurityManager);

            string query = string.Format(
                "Select count(*) from ({0}) QueryForGettingCount", GenerateSQLSelect(customizationStruct, true));
            object[][] res = await ReadAsync(query, customizationStruct.LoadingBufferSize)
                .ConfigureAwait(false);
            return res == null ? 0 : (int)Convert.ChangeType(res[0][0], typeof(int));
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectAsync(DataObject, View, bool, bool, DataObjectCache)"></inheritdoc>
        public virtual async Task LoadObjectAsync(DataObject dataObject, View dataObjectView = null, bool clearDataObject = true, bool checkExistingObject = true, DataObjectCache dataObjectCache = null)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject), "Не указан объект для загрузки. Обратитесь к разработчику.");
            }

            if (dataObjectView == null)
            {
                var doType = dataObject.GetType();
                dataObjectView = new View(doType, View.ReadType.OnlyThatObject);
            }

            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            using (var dbTransactionWrapperAsync = new DbTransactionWrapperAsync(this))
            {
                await LoadObjectByExtConnAsync(dataObject, dataObjectView, clearDataObject, checkExistingObject, dataObjectCache, dbTransactionWrapperAsync)
                    .ConfigureAwait(false);
            }
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectAsync(DataObject, View, bool, bool, DataObjectCache)"/>
        /// <summary>Асинхронная загрузка объекта с указанной коннекцией в рамках указанной транзакции.</summary>
        /// <param name="dbTransactionWrapperAsync">Содержит коннекцию и транзакцию, через которые будет происходить зачитка.</param>
        public virtual async Task LoadObjectByExtConnAsync(
            DataObject dataObject,
            View dataObjectView,
            bool clearDataObject,
            bool checkExistingObject,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                Type dataObjectType = dataObject.GetType();
                RunChangeCustomizationString(new Type[] { dataObjectType });

                dataObjectCache.AddDataObject(dataObject);

                if (clearDataObject)
                {
                    dataObject.Clear();
                }
                else
                {
                    prv_AddMasterObjectsToCache(dataObject, new ArrayList(), dataObjectCache);
                }

                var prevPrimaryKey = dataObject.__PrimaryKey;
                var lcs = GetLcsPrimaryKey(dataObject, dataObjectView);

                ApplyLimitForAccess(lcs);

                // Cтроим запрос.
                StorageStructForView[] storageStruct;
                string query = GenerateSQLSelect(lcs, false, out storageStruct, false);

                // Получаем данные.
                var transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
                object[][] resValue = await ReadByExtConnAsync(query, 0, dbTransactionWrapperAsync)
                    .ConfigureAwait(false);

                if (resValue == null)
                {
                    if (checkExistingObject)
                    {
                        throw new CantFindDataObjectException(dataObjectType, dataObject.__PrimaryKey);
                    }
                    else
                    {
                        return;
                    }
                }

                DataObject[] helpDataObjectArray = { dataObject };

                transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
                Utils.ProcessingRowsetDataRef(
                    resValue, new[] { dataObjectType }, storageStruct, lcs, helpDataObjectArray, this, Types, clearDataObject, dataObjectCache, SecurityManager, dbTransactionWrapperAsync.Connection, transaction);

                if (dataObject.Prototyped)
                {
                    dataObject.SetStatus(ObjectStatus.Created);
                    dataObject.SetLoadingState(LoadingState.NotLoaded);
                    dataObject.__PrimaryKey = prevPrimaryKey;
                }
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectsAsync(DataObject[], View, bool, DataObjectCache)"/>
        public virtual async Task LoadObjectsAsync(DataObject[] dataObjects, View dataObjectView = null, bool clearDataObject = true, DataObjectCache dataObjectCache = null)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView));
            }

            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            if (dataObjects == null || dataObjects.Length == 0)
            {
                return;
            }

            dataObjectCache.StartCaching(false);
            try
            {
                RunChangeCustomizationString(dataObjects);

                SortedList allObjectKeys = new SortedList();
                SortedList readingKeys = new SortedList();
                LoadingCustomizationStruct customizationStruct = GetCustomizationStruct(dataObjects, dataObjectView, out allObjectKeys, out readingKeys);
                ApplyReadPermissions(customizationStruct, SecurityManager);

                StorageStructForView[] storageStruct;
                string selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // получаем данные
                object[][] result = string.IsNullOrEmpty(selectString) ? new object[0][] : await ReadAsync(
                    selectString,
                    0).ConfigureAwait(false);

                ConvertReadResult(result, dataObjects, customizationStruct, storageStruct, allObjectKeys, readingKeys, clearDataObject, dataObjectCache);
            }
            finally
            {
                dataObjectCache.StopCaching();
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

        /// <inheritdoc cref="IAsyncDataService.LoadObjectsAsync(LoadingCustomizationStruct, DataObjectCache)"/>
        public virtual async Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache = null)
        {
            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            using (DbTransactionWrapperAsync wrapper = new DbTransactionWrapperAsync(this))
            {
                return await LoadObjectsByExtConnAsync(customizationStruct, dataObjectCache, wrapper)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Асинхронная загрузка объектов с использованием указанной коннекции и транзакции.
        /// </summary>
        /// <param name="customizationStruct">Структура, определяющая, что и как грузить.</param>
        /// <param name="dataObjectCache">Кэш объектов для зачитки.</param>
        /// <param name="dbTransactionWrapperAsync">Коннекция и транзакция, через которые будут выполнена зачитка.</param>
        /// <returns>Загруженные данные.</returns>
        public virtual async Task<DataObject[]> LoadObjectsByExtConnAsync(
            LoadingCustomizationStruct customizationStruct,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                RunChangeCustomizationString(customizationStruct.LoadingTypes);

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                Type[] dataObjectType = customizationStruct.LoadingTypes;
                StorageStructForView[] storageStruct;

                string selectString = string.Empty;
                selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // Получаем данные.
                object[][] resValue = await ReadByExtConnAsync(selectString, customizationStruct.LoadingBufferSize, dbTransactionWrapperAsync)
                    .ConfigureAwait(false);

                DataObject[] res = null;
                if (resValue == null)
                {
                    res = new DataObject[0];
                }
                else
                {
                    var transaction = await dbTransactionWrapperAsync.GetTransactionAsync()
                        .ConfigureAwait(false);
                    res = Utils.ProcessingRowsetData(
                            resValue, dataObjectType, storageStruct, customizationStruct, this, Types, dataObjectCache, SecurityManager, dbTransactionWrapperAsync.Connection, transaction);
                }

                return res;
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectsAsync(View, DataObjectCache)"/>
        public virtual Task<DataObject[]> LoadObjectsAsync(View dataObjectView, DataObjectCache dataObjectCache = null)
        {
            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            LoadingCustomizationStruct lc = new LoadingCustomizationStruct(GetInstanceId());
            lc.View = dataObjectView;
            lc.LoadingTypes = new[] { dataObjectView.DefineClassType };
            return LoadObjectsAsync(lc, dataObjectCache);
        }

        /// <summary>
        /// Асинхронная вычитка данных.
        /// </summary>
        /// <param name="query">Запрос для вычитки.</param>
        /// <param name="loadingBufferSize">Ограничение на количество строк, которые будут загружены.</param>
        /// <returns>Асинхронная операция (возвращает результат вычитки).</returns>
        public virtual async Task<object[][]> ReadAsync(string query, int loadingBufferSize)
        {
            using (DbTransactionWrapperAsync dbTransactionWrapperAsync = new DbTransactionWrapperAsync(this))
            {
                return await ReadByExtConnAsync(query, loadingBufferSize, dbTransactionWrapperAsync)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Асинхронная вычитка данных.
        /// </summary>
        /// <param name="query">Запрос для вычитки.</param>
        /// <param name="loadingBufferSize">Количество строк, которые нужно загрузить в рамках текущей вычитки (используется для повторной дочитки).</param>
        /// <param name="dbTransactionWrapperAsync">Содержит соединение и транзакцию, в рамках которых нужно выполнить запрос (если соединение закрыто - оно откроется).</param>
        /// <returns>Асинхронная операция (возвращает результат вычитки).</returns>
        public virtual async Task<object[][]> ReadByExtConnAsync(string query, int loadingBufferSize, DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            object task = BusinessTaskMonitor.BeginTask("Reading data asynchronously" + Environment.NewLine + query);

            DbDataReader reader = null;
            var connection = dbTransactionWrapperAsync?.Connection;
            try
            {
                bool connectionIsOpen = connection.State.HasFlag(ConnectionState.Open);
                if (!connectionIsOpen)
                {
                    await connection.OpenAsync()
                        .ConfigureAwait(false);
                }

                DbCommand command = await dbTransactionWrapperAsync.CreateCommandAsync(query)
                    .ConfigureAwait(false);
                CustomizeCommand(command);

                reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                var hasRows = await reader.ReadAsync()
                    .ConfigureAwait(false);

                if (hasRows)
                {
                    var arl = new ArrayList();
                    int i = 1;
                    int fieldCount = reader.FieldCount;

                    // Порционная вычитка:
                    while (i <= loadingBufferSize || loadingBufferSize == 0)
                    {
                        if (i > 1)
                        {
                            var hasMoreRows = await reader.ReadAsync()
                                .ConfigureAwait(false);

                            if (!hasMoreRows)
                            {
                                break;
                            }
                        }

                        object[] tmp = new object[fieldCount];
                        reader.GetValues(tmp);
                        arl.Add(tmp);
                        i++;
                    }

                    object[][] result = (object[][])arl.ToArray(typeof(object[]));
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new ExecutingQueryException(query, string.Empty, e);
            }
            finally
            {
                if (reader is not null)
                {
#if NETSTANDARD2_1
                    await reader.CloseAsync().ConfigureAwait(false);
#else
                    reader.Close();
#endif
                }

                if (connection is not null)
                {
#if NETSTANDARD2_1
                    await connection.CloseAsync().ConfigureAwait(false);
#else
                    connection.Close();
#endif
                }

                BusinessTaskMonitor.EndTask(task);
            }
        }

        /// <inheritdoc cref="IAsyncDataService.UpdateObjectAsync(DataObject, bool, DataObjectCache)"/>
        public virtual Task UpdateObjectAsync(DataObject dataObject, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null)
        {
            DataObject[] arr = new DataObject[] { dataObject };
            return UpdateObjectsAsync(arr, alwaysThrowException, dataObjectCache);
        }
    }
}
