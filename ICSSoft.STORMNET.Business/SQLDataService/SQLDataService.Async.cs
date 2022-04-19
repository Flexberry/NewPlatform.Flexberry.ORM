namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using NewPlatform.Flexberry.ORM;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : IAsyncDataService
    {
        public abstract System.Data.Common.DbConnection GetDbConnection();

        /// <inheritdoc/>
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

        #region LoadObjectAsync

        /// <summary>
        /// Загрузка одного объекта данных.
        /// DataObject обновляется по завершению асинхронной операции, не рекомендуется работать c объектом до завершения операции.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">очищать ли объект.</param>
        /// <param name="checkExistingObject">Выбрасывать ли ошибку, если объекта нет в хранилище.</param>
        /// <returns>Асинхронная операция (по её завершению DataObject обновляется).</returns>
        public virtual async Task LoadObjectAsync(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView), "Не указано представление для загрузки объекта. Обратитесь к разработчику.");
            }

            var doType = dobject.GetType();
            RunChangeCustomizationString(new Type[] { doType });

            // if (dobject.GetStatus(false)==ObjectStatus.Created && !dobject.Prototyped) return;
            dataObjectCache.StartCaching(false);
            try
            {
                dataObjectCache.AddDataObject(dobject);

                if (clearDataObject)
                {
                    dobject.Clear();
                }
                else
                {
                    prv_AddMasterObjectsToCache(dobject, new System.Collections.ArrayList(), dataObjectCache);
                }

                var prevPrimaryKey = dobject.__PrimaryKey;
                var lc = GetLcsPrimaryKey(dobject, dataObjectView);
                ApplyLimitForAccess(lc);

                // строим запрос
                StorageStructForView[] StorageStruct; // = STORMDO.Information.GetStorageStructForView(dataObjectView,doType);
                string query = GenerateSQLSelect(lc, false, out StorageStruct, false);

                // получаем данные
                object[][] resValue = await ReadAsync(query, 0).ConfigureAwait(false);
                if (resValue == null)
                {
                    if (checkExistingObject)
                    {
                        throw new CantFindDataObjectException(doType, dobject.__PrimaryKey);
                    }
                    else
                    {
                        return;
                    }
                }

                DataObject[] rrr = new DataObject[] { dobject };
                Utils.ProcessingRowsetDataRef(resValue, new Type[] { doType }, StorageStruct, lc, rrr, this, Types, clearDataObject, dataObjectCache, SecurityManager);
                if (dobject.Prototyped)
                {
                    dobject.SetStatus(ObjectStatus.Created);
                    dobject.SetLoadingState(LoadingState.NotLoaded);
                    dobject.__PrimaryKey = prevPrimaryKey;
                }
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(DataObject dobject, DataObjectCache cache)
        {
            return LoadObjectAsync(dobject, true, true, cache);
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(View view, DataObject dobject)
        {
            return LoadObjectAsync(view, dobject, new DataObjectCache());
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true)
        {
            return LoadObjectAsync(
                dobject,
                clearDataObject,
                checkExistingObject,
                new DataObjectCache());
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(View view, DataObject dobject, DataObjectCache cache)
        {
            return LoadObjectAsync(view, dobject, true, true, cache);
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            return LoadObjectAsync(
                new View(dobject.GetType(), View.ReadType.OnlyThatObject),
                dobject,
                clearDataObject,
                checkExistingObject,
                dataObjectCache);
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(View dataObjectView, DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true)
        {
            return LoadObjectAsync(dataObjectView, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        /// <inheritdoc/>
        public virtual Task LoadObjectAsync(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true)
        {
            return LoadObjectAsync(dataObjectView, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        #endregion

        #region LoadObjectsAsync

        /// <inheritdoc/>
        public virtual async Task LoadObjectsAsync(ICSSoft.STORMNET.DataObject[] dataObjects,
            ICSSoft.STORMNET.View dataObjectView, bool clearDataObject = true, DataObjectCache dataObjectCache = null)
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

        /// <inheritdoc/>
        public virtual async Task<ICSSoft.STORMNET.DataObject[]> LoadObjectsAsync(
            LoadingCustomizationStruct customizationStruct,
            DataObjectCache dataObjectCache)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                System.Type[] dataObjectType = customizationStruct.LoadingTypes;
                if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
                {
                    string cs = ChangeCustomizationString(dataObjectType);
                    customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
                }

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                StorageStructForView[] storageStruct;

                string selectString = string.Empty;
                selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // получаем данные
                object[][] resValue = await ReadAsync(selectString, customizationStruct.LoadingBufferSize)
                    .ConfigureAwait(false);
                ICSSoft.STORMNET.DataObject[] res = null;
                if (resValue == null)
                {
                    res = new DataObject[0];
                }
                else
                {
                    res = Utils.ProcessingRowsetData(resValue, dataObjectType, storageStruct, customizationStruct, this, Types, dataObjectCache, SecurityManager);
                }

                return res;
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <inheritdoc/>
        public virtual Task<ICSSoft.STORMNET.DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct)
        {
            return LoadObjectsAsync(customizationStruct, new DataObjectCache());
        }

        /// <inheritdoc/>
        public virtual async Task<ICSSoft.STORMNET.DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct[] customizationStructs)
        {
            System.Collections.ArrayList arr = new System.Collections.ArrayList();
            ICSSoft.STORMNET.DataObject[] res = null;
            for (int i = 0; i < customizationStructs.Length; i++)
            {
                res = await LoadObjectsAsync(customizationStructs[i]);
                for (int j = 0; j < res.Length; j++)
                {
                    arr.Add(res[j]);
                }
            }

            res = new ICSSoft.STORMNET.DataObject[arr.Count];
            arr.CopyTo(res);
            return res;
        }

        /// <inheritdoc/>
        public virtual Task<DataObject[]> LoadObjectsAsync(View dataObjectView)
        {
            LoadingCustomizationStruct lc = new LoadingCustomizationStruct(GetInstanceId());
            lc.View = dataObjectView;
            lc.LoadingTypes = new[] { dataObjectView.DefineClassType };
            return LoadObjectsAsync(lc, new DataObjectCache());
        }

        /// <inheritdoc/>
        public virtual async Task<DataObject[]> LoadObjectsAsync(View[] dataObjectViews)
        {
            System.Collections.ArrayList arr = new System.Collections.ArrayList();
            ICSSoft.STORMNET.DataObject[] res = null;
            for (int i = 0; i < dataObjectViews.Length; i++)
            {
                res = await LoadObjectsAsync(dataObjectViews[i])
                    .ConfigureAwait(false);
                for (int j = 0; j < res.Length; j++)
                {
                    arr.Add(res[j]);
                }
            }

            res = new ICSSoft.STORMNET.DataObject[arr.Count];
            arr.CopyTo(res);
            return res;
        }

        /// <summary>
        /// Асихнронная загрузка объекта с указанной коннекцией в рамках указанной транзакции.
        /// </summary>
        /// <param name="dataObjectView">Представление, по которому будет зачитываться объект.</param>
        /// <param name="dobject">Объект, который будет дочитываться/зачитываться.</param>
        /// <param name="сlearDataObject">Следует ли при зачитке очистить поля существующего объекта данных.</param>
        /// <param name="сheckExistingObject">Проверить существовние встречающихся при зачитке объектов.</param>
        /// <param name="dataObjectCache">Кэш объектов.</param>
        /// <param name="connection">Коннекция, через которую будет происходить зачитка.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет проходить зачитка.</param>
        public virtual async Task LoadObjectByExtConnAsync(
            View dataObjectView,
            DataObject dobject,
            bool сlearDataObject,
            bool сheckExistingObject,
            DataObjectCache dataObjectCache,
            DbConnection connection,
            DbTransaction transaction)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                Type dataObjectType = dobject.GetType();
                dataObjectCache.AddDataObject(dobject);

                if (сlearDataObject)
                {
                    dobject.Clear();
                }
                else
                {
                    prv_AddMasterObjectsToCache(dobject, new ArrayList(), dataObjectCache);
                }

                var prevPrimaryKey = dobject.__PrimaryKey;
                var lcs = GetLcsPrimaryKey(dobject, dataObjectView);

                ApplyLimitForAccess(lcs);

                // Cтроим запрос.
                StorageStructForView[] storageStruct;
                string query = GenerateSQLSelect(lcs, false, out storageStruct, false);

                // Получаем данные.
                object state = null;
                object[][] resValue = await ReadFirstByExtConnAsync(query, state, 0, connection, transaction);
                if (resValue == null)
                {
                    if (сheckExistingObject)
                    {
                        throw new CantFindDataObjectException(dataObjectType, dobject.__PrimaryKey);
                    }
                    else
                    {
                        return;
                    }
                }

                DataObject[] helpDataObjectArray = { dobject };

                Utils.ProcessingRowsetDataRef(
                    resValue, new[] { dataObjectType }, storageStruct, lcs, helpDataObjectArray, this, Types, сlearDataObject, dataObjectCache, SecurityManager, connection, transaction);

                if (dobject.Prototyped)
                {
                    dobject.SetStatus(ObjectStatus.Created);
                    dobject.SetLoadingState(LoadingState.NotLoaded);
                    dobject.__PrimaryKey = prevPrimaryKey;
                }
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        public virtual Task<object[][]> ReadFirstByExtConnAsync(string Query, object State, int LoadingBufferSize, DbConnection Connection, DbTransaction Transaction)
        {
            object taskid = BusinessTaskMonitor.BeginTask("Reading data" + Environment.NewLine + Query);
            try
            {
                using (DbCommand myCommand = Connection.CreateCommand())
                {
                    myCommand.CommandText = Query;
                    myCommand.Transaction = Transaction;
                    CustomizeCommand(myCommand);

                    DbDataReader myReader = myCommand.ExecuteReader();
                    State = new object[] { Connection, myReader };
                    return ReadNextByExtConnAsync(State, LoadingBufferSize);
                }
            }
            catch (Exception e)
            {
                throw new ExecutingQueryException(Query, string.Empty, e);
            }
            finally
            {
                BusinessTaskMonitor.EndTask(taskid);
            }
        }

        public virtual async Task<object[][]> ReadNextByExtConnAsync(object State, int LoadingBufferSize)
        {
            if (State == null || !State.GetType().IsArray)
            {
                return null;
            }

            DbDataReader myReader = (DbDataReader)((object[])State)[1];
            if (await myReader.ReadAsync())
            {
                ArrayList arl = new ArrayList();
                int i = 1;
                int FieldCount = myReader.FieldCount;

                while (i <= LoadingBufferSize || LoadingBufferSize == 0)
                {
                    if (i > 1)
                    {
                        if (!await myReader.ReadAsync())
                        {
                            break;
                        }
                    }

                    object[] tmp = new object[FieldCount];
                    myReader.GetValues(tmp);
                    arl.Add(tmp);
                    i++;
                }

                object[][] result = (object[][])arl.ToArray(typeof(object[]));

                if (i < LoadingBufferSize || LoadingBufferSize == 0)
                {
                    myReader.Close();
                }

                return result;
            }
            else
            {
                myReader.Close();
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Асинхронная вычитка данных.
        /// </summary>
        /// <param name="query">Запрос для вычитки.</param>
        /// <param name="loadingBufferSize"></param>
        /// <returns>Асинхронная операция (возвращает результат вычитки).</returns>
        public virtual async Task<object[][]> ReadAsync(string query, int loadingBufferSize)
        {
            object task = BusinessTaskMonitor.BeginTask("Reading data asynchronously" + Environment.NewLine + query);

            DbConnection connection = null;
            DbDataReader reader = null;
            try
            {
                connection = GetDbConnection();
                var openConnectionTask = connection.OpenAsync()
                    .ConfigureAwait(false);

                DbCommand command = connection.CreateCommand();
                command.CommandText = query;
                CustomizeCommand(command);

                await openConnectionTask;

                reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                var hasRows = await reader.ReadAsync()
                    .ConfigureAwait(false);

                if (hasRows)
                {
                    var arl = new ArrayList();
                    int i = 1;
                    int fieldCount = reader.FieldCount;

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

        /// <inheritdoc/>
        public virtual Task<DataObject> UpdateObjectAsync(DataObject dobject)
        {
            return UpdateObjectAsync(dobject, false);
        }

        /// <inheritdoc/>
        public virtual Task<DataObject> UpdateObjectAsync(DataObject dobject, bool alwaysThrowException)
        {
            return UpdateObjectAsync(dobject, new DataObjectCache(), alwaysThrowException);
        }

        /// <inheritdoc/>
        public virtual async Task<DataObject> UpdateObjectAsync(DataObject dobject, DataObjectCache dataObjectCache, bool alwaysThrowException)
        {
            DataObject[] arr = new DataObject[] { dobject };
            var result = await UpdateObjectsAsync(arr, dataObjectCache, alwaysThrowException).ConfigureAwait(false);
            if (result != null && result.Length > 0)
            {
                dobject = result[0];
            }
            else
            {
                dobject = null;
            }

            return dobject;
        }
    }
}
