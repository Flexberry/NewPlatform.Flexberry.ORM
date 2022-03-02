
namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : IAsyncDataService
    {
        /// <inheritdoc/>
        public virtual async Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct)
        {
            return await Task.Run(() => this.GetObjectsCount(customizationStruct));
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
    }
}
