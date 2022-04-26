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
        public abstract System.Data.Common.DbConnection GetDbConnection();

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
            return (int)Convert.ChangeType(res[0][0], typeof(int));
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectAsync(DataObject, View, bool, bool, DataObjectCache)"></inheritdoc>
        public virtual Task LoadObjectAsync(DataObject dataObject, View dataObjectView = null, bool clearDataObject = true, bool checkExistingObject = true, DataObjectCache dataObjectCache = null)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView), "Не указан объект для загрузки. Обратитесь к разработчику.");
            }

            var doType = dataObject.GetType();

            if (dataObjectView == null)
            {
                dataObjectView = new View(doType, View.ReadType.OnlyThatObject);
            }

            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            RunChangeCustomizationString(new Type[] { doType });

            return LoadObjectByExtConnAsync(dataObject, dataObjectView, clearDataObject, checkExistingObject, dataObjectCache, GetDbConnection());
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectAsync(DataObject, View, bool, bool, DataObjectCache)"/>
        /// <summary>Асинхронная загрузка объекта с указанной коннекцией в рамках указанной транзакции.</summary>
        /// <param name="connection">Коннекция, через которую будет происходить зачитка.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет проходить зачитка.</param>
        public virtual async Task LoadObjectByExtConnAsync(
            DataObject dataObject,
            View dataObjectView,
            bool clearDataObject,
            bool checkExistingObject,
            DataObjectCache dataObjectCache,
            DbConnection connection,
            DbTransaction transaction = null)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                Type dataObjectType = dataObject.GetType();
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
                object[][] resValue = await ReadAsyncByExtConn(query, 0, connection, transaction)
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

                Utils.ProcessingRowsetDataRef(
                    resValue, new[] { dataObjectType }, storageStruct, lcs, helpDataObjectArray, this, Types, clearDataObject, dataObjectCache, SecurityManager, connection, transaction);

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

            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                System.Collections.Generic.List<Type> tps = new System.Collections.Generic.List<Type>();
                foreach (DataObject d in dataObjects)
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

            dataObjectCache.StartCaching(false);
            try
            {
                System.Collections.ArrayList ALtypes = new System.Collections.ArrayList();
                System.Collections.ArrayList ALKeys = new System.Collections.ArrayList();
                System.Collections.SortedList ALobjectsKeys = new System.Collections.SortedList();
                System.Collections.SortedList readingKeys = new System.Collections.SortedList();
                for (int i = 0; i < dataObjects.Length; i++)
                {
                    DataObject dobject = dataObjects[i];
                    Type dotype = dobject.GetType();
                    bool addobj = false;
                    if (ALtypes.Contains(dotype))
                    {
                        addobj = true;
                    }
                    else
                    {
                        if ((dotype == dataObjectView.DefineClassType || dotype.IsSubclassOf(dataObjectView.DefineClassType)) && Information.IsStoredType(dotype))
                        {
                            ALtypes.Add(dotype);
                            addobj = true;
                        }
                    }

                    if (addobj)
                    {
                        object readingKey = dobject.Prototyped ? dobject.__PrototypeKey : dobject.__PrimaryKey;
                        ALKeys.Add(readingKey);
                        ALobjectsKeys.Add(dotype.FullName + readingKey.ToString(), i);
                        readingKeys.Add(readingKey.ToString(), dobject.__PrimaryKey);
                    }
                }

                LoadingCustomizationStruct customizationStruct = new LoadingCustomizationStruct(GetInstanceId());

                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                FunctionalLanguage.VariableDef var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                    lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.KeyType(dataObjectView.DefineClassType)), SQLWhereLanguageDef.StormMainObjectKey);
                object[] keys = new object[ALKeys.Count + 1];
                ALKeys.CopyTo(keys, 1);
                keys[0] = var;
                FunctionalLanguage.Function func = lang.GetFunction(lang.funcIN, keys);
                Type[] types = new Type[ALtypes.Count];
                ALtypes.CopyTo(types);

                customizationStruct.Init(null, func, types, dataObjectView, null);

                StorageStructForView[] StorageStruct;

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                string SelectString = string.Empty;
                SelectString = GenerateSQLSelect(customizationStruct, false, out StorageStruct, false);

                // получаем данные
                object[][] resValue = (SelectString == string.Empty) ? new object[0][] : await ReadAsync(
                    SelectString,
                    0).ConfigureAwait(false);
                if (resValue != null && resValue.Length != 0)
                {
                    DataObject[] loadobjects = new ICSSoft.STORMNET.DataObject[resValue.Length];
                    int ObjectTypeIndexPOs = resValue[0].Length - 1;
                    int keyIndex = StorageStruct[0].props.Length - 1;
                    while (StorageStruct[0].props[keyIndex].MultipleProp)
                    {
                        keyIndex--;
                    }

                    keyIndex++;

                    for (int i = 0; i < resValue.Length; i++)
                    {
                        Type tp = types[Convert.ToInt64(resValue[i][ObjectTypeIndexPOs].ToString())];
                        object ky = resValue[i][keyIndex];
                        ky = Information.TranslateValueToPrimaryKeyType(tp, ky);
                        int indexobj = ALobjectsKeys.IndexOfKey(tp.FullName + ky.ToString());
                        if (indexobj > -1)
                        {
                            loadobjects[i] = dataObjects[(int)ALobjectsKeys.GetByIndex(indexobj)];
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

                    Utils.ProcessingRowsetDataRef(resValue, types, StorageStruct, customizationStruct, loadobjects, this, Types, clearDataObject, dataObjectCache, SecurityManager);
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
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Асинхронная загрузка объектов с использованием указанной коннекции и транзакции.
        /// </summary>
        /// <param name="customizationStruct">Структура, определяющая, что и как грузить.</param>
        /// <param name="dataObjectCache">Кэш объектов для зачитки.</param>
        /// <param name="connection">Коннекция, через которую будут выполнена зачитска.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет выполнена зачитка.</param>
        /// <returns>Загруженные данные.</returns>
        public virtual async Task<DataObject[]> LoadObjectsByExtConnAsync(
            LoadingCustomizationStruct customizationStruct,
            DataObjectCache dataObjectCache,
            DbConnection connection,
            DbTransaction transaction = null)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                Type[] dataObjectType = customizationStruct.LoadingTypes;
                StorageStructForView[] storageStruct;

                string selectString = string.Empty;
                selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // Получаем данные.
                object[][] resValue = await ReadAsyncByExtConn(selectString, customizationStruct.LoadingBufferSize, connection, transaction)
                    .ConfigureAwait(false);

                DataObject[] res = null;
                if (resValue == null)
                {
                    res = new DataObject[0];
                }
                else
                {
                    res = Utils.ProcessingRowsetData(
                            resValue, dataObjectType, storageStruct, customizationStruct, this, Types, dataObjectCache, SecurityManager, connection, transaction);
                }

                return res;
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectsAsync(LoadingCustomizationStruct, DataObjectCache)"/>
        public virtual Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache = null)
        {
            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            RunChangeCustomizationString(customizationStruct.LoadingTypes);
            return LoadObjectsByExtConnAsync(customizationStruct, dataObjectCache, GetDbConnection());
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
        public virtual Task<object[][]> ReadAsync(string query, int loadingBufferSize)
        {
            return ReadAsyncByExtConn(query, loadingBufferSize, GetDbConnection());
        }

        /// <summary>
        /// Асинхронная вычитка данных.
        /// </summary>
        /// <param name="query">Запрос для вычитки.</param>
        /// <param name="loadingBufferSize">Количество строк, которые нужно загрузить в рамках текущей вычитки (используется для повторной дочитки).</param>
        /// <param name="connection">Соединение, в рамках которого нужно выполнить запрос (если соединение закрыто - оно откроется).</param>
        /// <param name="transaction">Транзакция, в рамках которой выполняется запрос.</param>
        /// <returns>Асинхронная операция (возвращает результат вычитки).</returns>
        public virtual async Task<object[][]> ReadAsyncByExtConn(string query, int loadingBufferSize, DbConnection connection, DbTransaction transaction = null)
        {
            object task = BusinessTaskMonitor.BeginTask("Reading data asynchronously" + Environment.NewLine + query);

            DbDataReader reader = null;
            try
            {
                bool connectionIsOpen = connection.State.HasFlag(ConnectionState.Open);
                if (!connectionIsOpen)
                {
                    await connection.OpenAsync()
                        .ConfigureAwait(false);
                }

                DbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Transaction = transaction;
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
                reader?.Close();
                connection?.Close();
                BusinessTaskMonitor.EndTask(task);
            }
        }

        /// <inheritdoc cref="IAsyncDataService.UpdateObjectAsync(DataObject, bool, DataObjectCache)"/>
        public virtual async Task UpdateObjectAsync(DataObject dataObject, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null)
        {
            DataObject[] arr = new DataObject[] { dataObject };
            await UpdateObjectsAsync(arr, alwaysThrowException, dataObjectCache).ConfigureAwait(false);
        }
    }
}
