
namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Common;
    using System.Data;
    using System.Collections;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

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
        public virtual async Task LoadObjectAsync(DataObject dobject, DataObjectCache cache)
        {
            await LoadObjectAsync(dobject, true, true, cache).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(string viewName, DataObject dataObject)
        {
            await LoadObjectAsync(viewName, dataObject, new DataObjectCache()).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(View view, DataObject dobject)
        {
            await LoadObjectAsync(view, dobject, new DataObjectCache());
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true)
        {
            await LoadObjectAsync(
                dobject,
                clearDataObject,
                checkExistingObject,
                new DataObjectCache()).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(string dataObjectViewName, DataObject dobject, DataObjectCache cache)
        {
            await LoadObjectAsync(Information.GetView(dataObjectViewName, dobject.GetType()), dobject, cache)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(View view, DataObject dobject, DataObjectCache cache)
        {
            await LoadObjectAsync(view, dobject, true, true, cache);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            await LoadObjectAsync(
                new View(dobject.GetType(), View.ReadType.OnlyThatObject),
                dobject,
                clearDataObject,
                checkExistingObject,
                dataObjectCache).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(string dataObjectViewName, DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true)
        {
            await LoadObjectAsync(
                Information.GetView(dataObjectViewName, dobject.GetType()),
                dobject,
                clearDataObject,
                checkExistingObject,
                new DataObjectCache()).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(View dataObjectView, DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true)
        {
            await LoadObjectAsync(dataObjectView, dobject, clearDataObject, checkExistingObject, new DataObjectCache())
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            await LoadObjectAsync(Information.GetView(dataObjectViewName, dobject.GetType()), dobject, clearDataObject, checkExistingObject, dataObjectCache);
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true)
        {
            await LoadObjectAsync(dataObjectView, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        #endregion

        /// <inheritdoc/>
        public virtual async Task LoadObjectsAsync(IEnumerable<DataObject> dataobjects, View dataObjectView, bool clearDataobject = true)
        {
            await Task.Run(() => LoadObjects(dataobjects.ToArray(), dataObjectView, clearDataobject));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(View dataObjectView)
        {
            return await Task.Run(() => LoadObjectsAsync(dataObjectView));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<View> dataObjectViews)
        {
            return await Task.Run(() => LoadObjects(dataObjectViews.ToArray()));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<LoadingCustomizationStruct> customizationStructs)
        {
            return await Task.Run(() => LoadObjects(customizationStructs.ToArray()));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            return await Task.Run(() => LoadObjects(dataObjectView, changeViewForTypeDelegate));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<View> dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            return await Task.Run(() => LoadObjects(dataObjectViews.ToArray(), changeViewForTypeDelegate));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(IEnumerable<LoadingCustomizationStruct> customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            return await Task.Run(() => LoadObjects(customizationStructs.ToArray(), changeViewForTypeDelegate));
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct)
        {
            return await Task.Run(() => LoadObjects(customizationStruct));
        }

        /// <inheritdoc/>
        public virtual async Task<DataObject> UpdateObjectAsync(DataObject dobject)
        {
            return await Task.Run(() =>
            {
                UpdateObject(ref dobject);
                return dobject;
            });
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<DataObject>> UpdateObjectsAsync(IEnumerable<DataObject> objects)
        {
            return await Task.Run(() =>
            {
                var objArray = objects.ToArray();
                UpdateObjects(ref objArray);
                return objArray;
            });
        }

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
    }
}
