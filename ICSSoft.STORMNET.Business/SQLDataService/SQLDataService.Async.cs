namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Exceptions;

    using NewPlatform.Flexberry.ORM;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : IAsyncDataService
    {
        /// <summary>
        /// Метод должен возвращать соединение <see cref="DbConnection"/>.
        /// </summary>
        /// <returns>Соединение <see cref="DbConnection"/>.</returns>
        public abstract DbConnection GetDbConnection();

        /// <inheritdoc cref="IAsyncDataService.GetObjectsCountAsync(LoadingCustomizationStruct)"/>
        public virtual async Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct)
        {
            RunChangeCustomizationString(customizationStruct.LoadingTypes);

            // Применим полномочия на строки
            ApplyReadPermissions(customizationStruct, SecurityManager);

            string selectQuery = GenerateSQLSelect(customizationStruct, true);
            string countQuery = $"Select count(*) from ({selectQuery}) QueryForGettingCount";
            object[][] res = await ReadAsync(countQuery, customizationStruct.LoadingBufferSize)
                .ConfigureAwait(false);
            return res == null ? 0 : (int)Convert.ChangeType(res[0][0], typeof(int));
        }

        /// <inheritdoc cref="IAsyncDataService.LoadObjectAsync(DataObject, View, bool, bool, DataObjectCache)" />
        public virtual async Task LoadObjectAsync(DataObject dataObject, View dataObjectView = null, bool clearDataObject = true, bool checkExistingObject = true, DataObjectCache dataObjectCache = null)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject), "Не указан объект для загрузки.");
            }

            if (dataObjectView == null)
            {
                var doType = dataObject.GetType();
                dataObjectView = new View(doType, View.ReadType.OnlyThatObject);
            }

            dataObjectCache ??= new DataObjectCache();

            RunChangeCustomizationString(new DataObject[] { dataObject });
            using (var dbTransactionWrapperAsync = new DbTransactionWrapperAsync(this))
            {
                await LoadObjectByExtConnAsync(dataObject, dataObjectView, clearDataObject, checkExistingObject, dataObjectCache, dbTransactionWrapperAsync)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Асинхронная загрузка объекта с указанным объектом подключения в рамках указанной транзакции.
        /// </summary>
        /// <param name="dataObject">Объект данных, который требуется загрузить.</param>
        /// <param name="dataObjectView">Представление, по которому загружается объект. Если null, будут загружены все атрибуты объекта, без детейлов (см. <see cref="View.ReadType.OnlyThatObject"/>).</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Вызывать исключение если объекта нет в хранилище.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <param name="dbTransactionWrapperAsync">Содержит объект подключения и транзакцию, через которые будет происходить вычитка.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        public virtual async Task LoadObjectByExtConnAsync(
            DataObject dataObject,
            View dataObjectView,
            bool clearDataObject,
            bool checkExistingObject,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject), "Не указан объект для загрузки.");
            }

            if (dbTransactionWrapperAsync == null)
            {
                throw new ArgumentNullException(nameof(dbTransactionWrapperAsync), "Не указан DbTransactionWrapperAsync.");
            }

            if (dataObjectView == null)
            {
                var doType = dataObject.GetType();
                dataObjectView = new View(doType, View.ReadType.OnlyThatObject);
            }

            dataObjectCache ??= new DataObjectCache();

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

                var transaction = await dbTransactionWrapperAsync.GetTransactionAsync().ConfigureAwait(false);
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

            if (dataObjects == null || dataObjects.Length == 0)
            {
                return;
            }

            dataObjectCache ??= new DataObjectCache();

            dataObjectCache.StartCaching(false);
            try
            {
                RunChangeCustomizationString(dataObjects);

                SortedList allObjectKeys;
                SortedList readingKeys;
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

        /// <inheritdoc cref="IAsyncDataService.LoadObjectsAsync(LoadingCustomizationStruct, DataObjectCache)"/>
        public virtual async Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache = null)
        {
            dataObjectCache ??= new DataObjectCache();

            RunChangeCustomizationString(customizationStruct.LoadingTypes);
            using (DbTransactionWrapperAsync wrapper = new DbTransactionWrapperAsync(this))
            {
                return await LoadObjectsByExtConnAsync(customizationStruct, dataObjectCache, wrapper)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Асинхронная загрузка объектов с использованием указанного объекта подключения и транзакции.
        /// </summary>
        /// <param name="customizationStruct">Структура, определяющая, что и как грузить.</param>
        /// <param name="dataObjectCache">Кэш объектов для вычитки.</param>
        /// <param name="dbTransactionWrapperAsync">Объект подключения и транзакция, через которые будут выполнена вычитка.</param>
        /// <returns>Загруженные данные.</returns>
        public virtual async Task<DataObject[]> LoadObjectsByExtConnAsync(
            LoadingCustomizationStruct customizationStruct,
            DataObjectCache dataObjectCache,
            DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            if (customizationStruct == null)
            {
                throw new ArgumentNullException(nameof(customizationStruct));
            }

            if (dbTransactionWrapperAsync == null)
            {
                throw new ArgumentNullException(nameof(dbTransactionWrapperAsync));
            }

            dataObjectCache ??= new DataObjectCache();

            dataObjectCache.StartCaching(false);
            try
            {
                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                Type[] dataObjectType = customizationStruct.LoadingTypes;
                StorageStructForView[] storageStruct;

                string selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // Получаем данные.
                object[][] resValue = await ReadByExtConnAsync(selectString, customizationStruct.LoadingBufferSize, dbTransactionWrapperAsync)
                    .ConfigureAwait(false);

                DataObject[] res;
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
            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView));
            }

            dataObjectCache ??= new DataObjectCache();

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
        protected virtual async Task<object[][]> ReadAsync(string query, int loadingBufferSize)
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
        protected virtual async Task<object[][]> ReadByExtConnAsync(string query, int loadingBufferSize, DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            if (dbTransactionWrapperAsync == null)
            {
                throw new ArgumentNullException(nameof(dbTransactionWrapperAsync));
            }

            object task = BusinessTaskMonitor.BeginTask("Reading data asynchronously" + Environment.NewLine + query);

            try
            {
                DbCommand command = await dbTransactionWrapperAsync.CreateCommandAsync(query)
                    .ConfigureAwait(false);
                CustomizeCommand(command);

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var hasRows = await reader.ReadAsync()
                        .ConfigureAwait(false);

                    if (hasRows)
                    {
                        var arl = new List<object[]>();
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

                        return arl.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw new ExecutingQueryException(query, string.Empty, e);
            }
            finally
            {
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
