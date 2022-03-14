
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

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : IAsyncDataService
    {
        public abstract System.Data.Common.DbConnection GetDbConnection();

        /// <inheritdoc/>
        public virtual async Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct)
        {
            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                string cs = ChangeCustomizationString(customizationStruct.LoadingTypes);
                customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
            }

            // Применим полномочия на строки
            ApplyReadPermissions(customizationStruct, SecurityManager);

            string query = string.Format(
                "Select count(*) from ({0}) QueryForGettingCount", GenerateSQLSelect(customizationStruct, true));
            object[][] res = await ReadAsync(query, customizationStruct.LoadingBufferSize);
            return (int)Convert.ChangeType(res[0][0], typeof(int));
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true)
        {
            await Task.Run(() => this.LoadObject(dobject, clearDataObject, checkExistingObject));
        }

        /// <inheritdoc/>
        public virtual async Task LoadObjectAsync(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true)
        {
            await Task.Run(() => LoadObject(dataObjectView, dobject, clearDataObject, checkExistingObject));
        }

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
        /// <param name="query"></param>
        /// <param name="loadingBufferSize"></param>
        /// <returns></returns>
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

                    if (i < loadingBufferSize || loadingBufferSize == 0)
                    {
                        reader.Close();
                        connection.Close();
                    }

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
