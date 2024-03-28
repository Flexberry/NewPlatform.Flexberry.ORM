namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Notifier default implementation for update objects process.
    /// </summary>
    public class NotifierUpdateObjects : INotifyUpdateObjects
    {
        private INotifyUpdatePropertyByType notifierUpdatePropertyByType;

        private IDictionary<Type, IEnumerable<string>> subscribedPropNotifiers;

        private IDictionary<Guid, IDictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>>> stateStore = new Dictionary<Guid, IDictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>>>();

        /// <summary>
        /// An instance of the class for custom process updated objects properties.
        /// See <see cref="INotifyUpdateObjects"/>.
        /// </summary>
        public INotifyUpdatePropertyByType NotifierUpdatePropertyByType
        {
            get => notifierUpdatePropertyByType;
            set
            {
                notifierUpdatePropertyByType = value;
                if (notifierUpdatePropertyByType != null)
                {
                    subscribedPropNotifiers = notifierUpdatePropertyByType.GetSubscribedProperties();
                }
            }
        }

        /// <summary>
        /// Initializes instance of <see cref="NotifierUpdateObjects" />.
        /// </summary>
        public NotifierUpdateObjects()
        {
        }

        /// <summary>
        /// Initializes instance of <see cref="NotifierUpdateObjects" />.
        /// </summary>
        /// <param name="notifierUpdatePropertyByType">An instance of the class for custom process updated objects properties.</param>
        public NotifierUpdateObjects(INotifyUpdatePropertyByType notifierUpdatePropertyByType)
        {
            NotifierUpdatePropertyByType = notifierUpdatePropertyByType;
        }

        /// <inheritdoc cref="INotifyUpdateObjects"/>
        public virtual void BeforeUpdateObjects(Guid operationId, IDataService dataService, IDbTransaction transaction, IEnumerable<DataObject> dataObjects)
        {
            if (dataObjects == null)
            {
                return;
            }

            foreach (DataObject dataObject in dataObjects)
            {
                Type dataObjectType = dataObject.GetType();
                ObjectStatus status = dataObject.GetStatus(false);

                if (dataObject is INotifyUpdateObject notifyUpdateObject)
                {
                    var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, null, null);
                    AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, string.Empty, stateStoreValue);

                    notifyUpdateObject.BeforeUpdateObject(dataObject, status, dataObjects);
                }

                List<string> alteredPropertyNames = new List<string>(dataObject.GetAlteredPropertyNames());

                if (subscribedPropNotifiers != null && (subscribedPropNotifiers.ContainsKey(dataObjectType) || status == ObjectStatus.Deleted))
                {
                    IEnumerable<string> subscribedPropertyNames = subscribedPropNotifiers[dataObjectType];
                    if (subscribedPropertyNames != null)
                    {
                        foreach (string propertyName in subscribedPropertyNames)
                        {
                            object oldValue = null;
                            object newValue = null;
                            bool notify = false;
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (alteredPropertyNames.Contains(propertyName))
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName);
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }

                            if (notify)
                            {
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);
                                AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName, stateStoreValue);

                                NotifierUpdatePropertyByType.BeforeUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                foreach (string propertyName in alteredPropertyNames)
                {
                    Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                    if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                    {
                        INotifyUpdateProperty oldValue = null;
                        INotifyUpdateProperty newValue = null;

                        var valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                        if (valuesFromStateStore != null)
                        {
                            oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                            newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                        }
                        else
                        {
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                            else
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName) as INotifyUpdateProperty;
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }

                            var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);
                            AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName, stateStoreValue);
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.BeforeUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                if (status == ObjectStatus.Deleted)
                {
                    string[] allPropertyNames = Information.GetAllPropertyNames(dataObjectType);

                    foreach (string propertyName in allPropertyNames)
                    {
                        if (propertyName == nameof(DataObject.IsReadOnly) || propertyName == nameof(DataObject.DynamicProperties) || propertyName == nameof(DataObject.__PrototypeKey) || propertyName == nameof(DataObject.Prototyped) || propertyName == nameof(DataObject.__PrimaryKey))
                        {
                            continue;
                        }

                        Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                        if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                        {
                            INotifyUpdateProperty oldValue = null;
                            INotifyUpdateProperty newValue = null;

                            var valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                            if (valuesFromStateStore != null)
                            {
                                oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                                newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                            }
                            else
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;

                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);
                                AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName, stateStoreValue);
                            }

                            INotifyUpdateProperty notifyUpdateProperty = oldValue;

                            if (notifyUpdateProperty != null)
                            {
                                notifyUpdateProperty.BeforeUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc cref="INotifyUpdateObjects"/>
        public virtual void AfterSuccessSqlUpdateObjects(Guid operationId, IDataService dataService, IDbTransaction transaction, IEnumerable<DataObject> dataObjects)
        {
            if (dataObjects == null)
            {
                return;
            }

            foreach (DataObject dataObject in dataObjects)
            {
                Type dataObjectType = dataObject.GetType();
                ObjectStatus status = dataObject.GetStatus(false);

                if (dataObject is INotifyUpdateObject notifyUpdateObject)
                {
                    notifyUpdateObject.AfterSuccessSqlUpdateObject(dataObject, status, dataObjects);
                }

                List<string> alteredPropertyNames = new List<string>(dataObject.GetAlteredPropertyNames());

                if (subscribedPropNotifiers != null && (subscribedPropNotifiers.ContainsKey(dataObjectType) || status == ObjectStatus.Deleted))
                {
                    IEnumerable<string> subscribedPropertyNames = subscribedPropNotifiers[dataObjectType];
                    if (subscribedPropertyNames != null)
                    {
                        foreach (string propertyName in subscribedPropertyNames)
                        {
                            object oldValue = null;
                            object newValue = null;
                            bool notify = false;
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (alteredPropertyNames.Contains(propertyName))
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName);
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }

                            if (notify)
                            {
                                NotifierUpdatePropertyByType.AfterSuccessSqlUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                foreach (string propertyName in alteredPropertyNames)
                {
                    Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                    if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                    {
                        INotifyUpdateProperty oldValue = null;
                        INotifyUpdateProperty newValue = null;

                        Tuple<ObjectStatus, object, object> valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                        if (valuesFromStateStore != null)
                        {
                            oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                            newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                        }
                        else
                        {
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                            else
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName) as INotifyUpdateProperty;
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }

                            var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);

                            AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName, stateStoreValue);
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.AfterSuccessSqlUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                if (status == ObjectStatus.Deleted)
                {
                    string[] allPropertyNames = Information.GetAllPropertyNames(dataObjectType);

                    foreach (string propertyName in allPropertyNames)
                    {
                        if (propertyName == nameof(DataObject.IsReadOnly) || propertyName == nameof(DataObject.DynamicProperties) || propertyName == nameof(DataObject.__PrototypeKey) || propertyName == nameof(DataObject.Prototyped) || propertyName == nameof(DataObject.__PrimaryKey))
                        {
                            continue;
                        }

                        Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                        if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                        {
                            INotifyUpdateProperty oldValue = null;
                            INotifyUpdateProperty newValue = null;

                            var valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                            if (valuesFromStateStore != null)
                            {
                                oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                                newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                            }
                            else
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;

                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);
                                AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName, stateStoreValue);
                            }

                            INotifyUpdateProperty notifyUpdateProperty = oldValue;

                            if (notifyUpdateProperty != null)
                            {
                                notifyUpdateProperty.AfterSuccessSqlUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc cref="INotifyUpdateObjects"/>
        public virtual void AfterSuccessUpdateObjects(Guid operationId, IDataService dataService, IEnumerable<DataObject> dataObjects)
        {
            if (dataObjects == null)
            {
                return;
            }

            foreach (DataObject dataObject in dataObjects)
            {
                Type dataObjectType = dataObject.GetType();
                ObjectStatus status = dataObject.GetStatus(false);

                if (dataObject is INotifyUpdateObject notifyUpdateObject)
                {
                    Tuple<ObjectStatus, object, object> stateStoreValue = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, string.Empty);
                    RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, string.Empty);

                    notifyUpdateObject.AfterSuccessUpdateObject(dataObject, stateStoreValue.Item1, dataObjects);
                }

                List<string> alteredPropertyNames = new List<string>(dataObject.GetAlteredPropertyNames());

                if (subscribedPropNotifiers != null && (subscribedPropNotifiers.ContainsKey(dataObjectType) || status == ObjectStatus.Deleted))
                {
                    IEnumerable<string> subscribedPropertyNames = subscribedPropNotifiers[dataObjectType];
                    if (subscribedPropertyNames != null)
                    {
                        foreach (string propertyName in subscribedPropertyNames)
                        {
                            object oldValue = null;
                            object newValue = null;
                            bool notify = false;
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (alteredPropertyNames.Contains(propertyName))
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName);
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }

                            if (notify)
                            {
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);
                                RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                                NotifierUpdatePropertyByType.AfterSuccessUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                IDictionary<string, Tuple<ObjectStatus, object, object>> dataFromStore = null;

                if (stateStore.ContainsKey(operationId))
                {
                    IDictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>> typeDictionary = stateStore[operationId];

                    if (typeDictionary.ContainsKey(dataObjectType))
                    {
                        IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>> primaryKeyDictionary = typeDictionary[dataObjectType];

                        if (primaryKeyDictionary.ContainsKey(dataObject.__PrimaryKey))
                        {
                            dataFromStore = primaryKeyDictionary[dataObject.__PrimaryKey];
                        }
                    }
                }

                if (dataFromStore != null)
                {
                    string[] keys = new string[dataFromStore.Count];
                    dataFromStore.Keys.CopyTo(keys, 0);
                    foreach (string propertyName in keys)
                    {
                        if (!string.IsNullOrWhiteSpace(propertyName))
                        {
                            Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                            if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                            {
                                Tuple<ObjectStatus, object, object> valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);
                                if (valuesFromStateStore != null)
                                {
                                    ObjectStatus objectStatusFromStateStore = valuesFromStateStore.Item1;
                                    INotifyUpdateProperty oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                                    INotifyUpdateProperty newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;

                                    INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                                    if (notifyUpdateProperty != null)
                                    {
                                        notifyUpdateProperty.AfterSuccessUpdateProperty(dataObject, objectStatusFromStateStore, propertyName, oldValue, newValue);
                                    }

                                    RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);
                                }
                            }
                        }
                    }
                }

                if (status == ObjectStatus.Deleted)
                {
                    string[] allPropertyNames = Information.GetAllPropertyNames(dataObjectType);

                    foreach (string propertyName in allPropertyNames)
                    {
                        if (propertyName == nameof(DataObject.IsReadOnly) || propertyName == nameof(DataObject.DynamicProperties) || propertyName == nameof(DataObject.__PrototypeKey) || propertyName == nameof(DataObject.Prototyped) || propertyName == nameof(DataObject.__PrimaryKey))
                        {
                            continue;
                        }

                        Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                        if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                        {
                            INotifyUpdateProperty oldValue = null;
                            INotifyUpdateProperty newValue = null;

                            var valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                            if (valuesFromStateStore != null)
                            {
                                oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                                newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                                RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);
                            }
                            else
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }

                            INotifyUpdateProperty notifyUpdateProperty = oldValue;

                            if (notifyUpdateProperty != null)
                            {
                                notifyUpdateProperty.AfterSuccessUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc cref="INotifyUpdateObjects"/>
        public virtual void AfterFailUpdateObjects(Guid operationId, IDataService dataService, IEnumerable<DataObject> dataObjects)
        {
            if (dataObjects == null)
            {
                return;
            }

            foreach (DataObject dataObject in dataObjects)
            {
                Type dataObjectType = dataObject.GetType();
                ObjectStatus status = dataObject.GetStatus(false);

                if (dataObject is INotifyUpdateObject notifyUpdateObject)
                {
                    Tuple<ObjectStatus, object, object> stateStoreValue = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, string.Empty);
                    RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, string.Empty);

                    if (stateStoreValue != null)
                    {
                        notifyUpdateObject.AfterFailUpdateObject(dataObject, stateStoreValue.Item1, dataObjects);
                    }
                }

                List<string> alteredPropertyNames = new List<string>(dataObject.GetAlteredPropertyNames());

                if (subscribedPropNotifiers != null && (subscribedPropNotifiers.ContainsKey(dataObjectType) || status == ObjectStatus.Deleted))
                {
                    IEnumerable<string> subscribedPropertyNames = subscribedPropNotifiers[dataObjectType];
                    if (subscribedPropertyNames != null)
                    {
                        foreach (string propertyName in subscribedPropertyNames)
                        {
                            object oldValue = null;
                            object newValue = null;
                            bool notify = false;
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }
                            else if (alteredPropertyNames.Contains(propertyName))
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName);
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName);
                                notify = true;
                            }

                            if (notify)
                            {
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);

                                RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                                NotifierUpdatePropertyByType.AfterFailUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                foreach (string propertyName in alteredPropertyNames)
                {
                    Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                    if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                    {
                        INotifyUpdateProperty oldValue = null;
                        INotifyUpdateProperty newValue = null;

                        Tuple<ObjectStatus, object, object> valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);
                        if (valuesFromStateStore != null)
                        {
                            oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                            newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                        }
                        else
                        {
                            if (status == ObjectStatus.Deleted)
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                            else if (status == ObjectStatus.Created)
                            {
                                newValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                            else
                            {
                                DataObject dataCopy = dataObject.GetDataCopy();
                                if (dataCopy != null)
                                {
                                    oldValue = Information.GetPropValueByName(dataCopy, propertyName) as INotifyUpdateProperty;
                                }

                                newValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;
                            }
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.AfterFailUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                if (status == ObjectStatus.Deleted)
                {
                    string[] allPropertyNames = Information.GetAllPropertyNames(dataObjectType);

                    foreach (string propertyName in allPropertyNames)
                    {
                        if (propertyName == nameof(DataObject.IsReadOnly) || propertyName == nameof(DataObject.DynamicProperties) || propertyName == nameof(DataObject.__PrototypeKey) || propertyName == nameof(DataObject.Prototyped) || propertyName == nameof(DataObject.__PrimaryKey))
                        {
                            continue;
                        }

                        Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                        if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                        {
                            INotifyUpdateProperty oldValue = null;
                            INotifyUpdateProperty newValue = null;

                            var valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);

                            if (valuesFromStateStore != null)
                            {
                                oldValue = valuesFromStateStore.Item2 as INotifyUpdateProperty;
                                newValue = valuesFromStateStore.Item3 as INotifyUpdateProperty;
                            }
                            else
                            {
                                oldValue = Information.GetPropValueByName(dataObject, propertyName) as INotifyUpdateProperty;

                                RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, propertyName);
                            }

                            INotifyUpdateProperty notifyUpdateProperty = oldValue;

                            if (notifyUpdateProperty != null)
                            {
                                notifyUpdateProperty.AfterFailUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add data to state store.
        /// </summary>
        /// <param name="operationId">Operation id.</param>
        /// <param name="dataObjectType">Data object type.</param>
        /// <param name="dataObjectPrimaryKey">Data object primaryKey.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="stateStoreValue">Value for state store.</param>
        private void AddToStateStore(Guid operationId, Type dataObjectType, object dataObjectPrimaryKey, string propertyName, Tuple<ObjectStatus, object, object> stateStoreValue)
        {
            if (!stateStore.ContainsKey(operationId))
            {
                stateStore.Add(operationId, new Dictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>>());
            }

            IDictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>> typeDictionary = stateStore[operationId];

            if (!typeDictionary.ContainsKey(dataObjectType))
            {
                typeDictionary.Add(dataObjectType, new Dictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>());
            }

            IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>> primaryKeyDictionary = typeDictionary[dataObjectType];

            if (!primaryKeyDictionary.ContainsKey(dataObjectPrimaryKey))
            {
                primaryKeyDictionary.Add(dataObjectPrimaryKey, new Dictionary<string, Tuple<ObjectStatus, object, object>>());
            }

            IDictionary<string, Tuple<ObjectStatus, object, object>> propertyNameDictionary = primaryKeyDictionary[dataObjectPrimaryKey];

            if (!propertyNameDictionary.ContainsKey(propertyName))
            {
                propertyNameDictionary.Add(propertyName, stateStoreValue);
            }
        }

        /// <summary>
        /// Get data from state store.
        /// </summary>
        /// <param name="operationId">Operation id.</param>
        /// <param name="dataObjectType">Data object type.</param>
        /// <param name="dataObjectPrimaryKey">Data object primaryKey.</param>
        /// <returns>Value from state store or <c>null</c> if it is absent.</returns>
        /// <param name="propertyName">Property name.</param>
        private Tuple<ObjectStatus, object, object> GetFromStateStore(Guid operationId, Type dataObjectType, object dataObjectPrimaryKey, string propertyName)
        {
            if (stateStore.ContainsKey(operationId))
            {
                IDictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>> typeDictionary = stateStore[operationId];

                if (typeDictionary.ContainsKey(dataObjectType))
                {
                    IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>> primaryKeyDictionary = typeDictionary[dataObjectType];

                    if (primaryKeyDictionary.ContainsKey(dataObjectPrimaryKey))
                    {
                        IDictionary<string, Tuple<ObjectStatus, object, object>> propertyNameDictionary = primaryKeyDictionary[dataObjectPrimaryKey];

                        if (propertyNameDictionary.ContainsKey(propertyName))
                        {
                            return propertyNameDictionary[propertyName];
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Remove data from state store.
        /// </summary>
        /// <param name="operationId">Operation id.</param>
        /// <param name="dataObjectType">Data object type.</param>
        /// <param name="dataObjectPrimaryKey">Data object primaryKey.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Operation result.</returns>
        private bool RemoveFromStateStore(Guid operationId, Type dataObjectType, object dataObjectPrimaryKey, string propertyName)
        {
            if (stateStore.ContainsKey(operationId))
            {
                IDictionary<Type, IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>>> typeDictionary = stateStore[operationId];

                if (typeDictionary.ContainsKey(dataObjectType))
                {
                    IDictionary<object, IDictionary<string, Tuple<ObjectStatus, object, object>>> primaryKeyDictionary = typeDictionary[dataObjectType];

                    if (primaryKeyDictionary.ContainsKey(dataObjectPrimaryKey))
                    {
                        IDictionary<string, Tuple<ObjectStatus, object, object>> propertyNameDictionary = primaryKeyDictionary[dataObjectPrimaryKey];

                        if (propertyNameDictionary.ContainsKey(propertyName))
                        {
                            return propertyNameDictionary.Remove(propertyName);
                        }
                    }
                }
            }

            return false;
        }
    }
}
