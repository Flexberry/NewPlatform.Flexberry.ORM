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
        //private IDictionary<string, Tuple<ObjectStatus, object, object>> stateStore1 = new Dictionary<string, Tuple<ObjectStatus, object, object>>(); // TODO: remove it.

        private IDictionary<Guid, IDictionary<Type, IDictionary<object, Tuple<ObjectStatus, object, object>>>> stateStore = new Dictionary<Guid, IDictionary<Type, IDictionary<object, Tuple<ObjectStatus, object, object>>>>();

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
                    string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey);
                    var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, null, null);
                    //stateStore1.Add(stateStoreKey, stateStoreValue);
                    AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, stateStoreValue);

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
                                string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);
                                //stateStore1.Add(stateStoreKey, stateStoreValue);
                                AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, stateStoreValue);

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

                        string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);
                        var valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);

                        if (/*stateStore1.ContainsKey(stateStoreKey)*/ valuesFromStateStore != null) // TODO: GetFromStateStore
                        {
                            //var valuesFromStateStore = stateStore1[stateStoreKey];
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
                            //stateStore1.Add(stateStoreKey, stateStoreValue);
                            AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, stateStoreValue);
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.BeforeUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                // TODO: обработать если объект отправляют на удаление, а свойство не было загружено. Событие всё равно должно вызваться. Надо прокешировать рефлекшен по всем свойствам типа данных, чтобы каждый раз не дёргать его.
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
                                string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);

                                //stateStore1.Add(stateStoreKey, stateStoreValue);
                                AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, stateStoreValue);

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

                        string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);

                        Tuple<ObjectStatus, object, object> valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);

                        if (valuesFromStateStore != null /*stateStore1.ContainsKey(stateStoreKey)*/) // TODO: GetFromStateStore
                        {
                            //var valuesFromStateStore = stateStore1[stateStoreKey];
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

                            //stateStore1.Add(stateStoreKey, stateStoreValue);
                            AddToStateStore(operationId, dataObjectType, dataObject.__PrimaryKey, stateStoreValue);
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.AfterSuccessSqlUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                // TODO: обработать если объект отправляют на удаление, а свойство не было загружено. Событие всё равно должно вызваться. Надо прокешировать рефлекшен по всем свойствам типа данных, чтобы каждый раз не дёргать его.
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
                    string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey);
                    //Tuple<ObjectStatus, object, object> stateStoreValue = stateStore1[stateStoreKey];
                    //stateStore1.Remove(stateStoreKey);
                    Tuple<ObjectStatus, object, object> stateStoreValue = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);
                    RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);

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
                                string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);

                                //stateStore1.Add(stateStoreKey, stateStoreValue);
                                RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);


                                NotifierUpdatePropertyByType.AfterSuccessUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                // TODO: тут уже нет информации об изменённых полях, надо брать то, что есть в хранилище.
                foreach (string propertyName in alteredPropertyNames)
                {
                    Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                    if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                    {
                        INotifyUpdateProperty oldValue = null;
                        INotifyUpdateProperty newValue = null;

                        string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);

                        Tuple<ObjectStatus, object, object> valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);
                        if (/*stateStore1 != null && stateStore1.ContainsKey(stateStoreKey)*/ valuesFromStateStore != null)
                        {
                            //var valuesFromStateStore = stateStore1[stateStoreKey];
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

                            //stateStore1.Add(stateStoreKey, stateStoreValue);
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.AfterSuccessUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                // TODO: обработать если объект отправляют на удаление, а свойство не было загружено. Событие всё равно должно вызваться. Надо прокешировать рефлекшен по всем свойствам типа данных, чтобы каждый раз не дёргать его.
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
                    string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey);
                    //Tuple<ObjectStatus, object, object> stateStoreValue = stateStore1[stateStoreKey];
                    //stateStore1.Remove(stateStoreKey);
                    Tuple<ObjectStatus, object, object> stateStoreValue = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);
                    RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);

                    notifyUpdateObject.AfterFailUpdateObject(dataObject, stateStoreValue.Item1, dataObjects);
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
                                string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);

                                //stateStore1.Add(stateStoreKey, stateStoreValue);
                                RemoveFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);

                                NotifierUpdatePropertyByType.AfterFailUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                // TODO
                foreach (string propertyName in alteredPropertyNames)
                {
                    Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                    if (typeof(INotifyUpdateProperty).IsAssignableFrom(propertyType))
                    {
                        INotifyUpdateProperty oldValue = null;
                        INotifyUpdateProperty newValue = null;

                        string stateStoreKey = string.Join("!", operationId.ToString(), dataObjectType.FullName, dataObject.__PrimaryKey, propertyName);

                        Tuple<ObjectStatus, object, object> valuesFromStateStore = GetFromStateStore(operationId, dataObjectType, dataObject.__PrimaryKey);
                        if (/*stateStore1.ContainsKey(stateStoreKey)*/ valuesFromStateStore != null) // TODO: 
                        {
                            // var valuesFromStateStore = stateStore1[stateStoreKey];
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

                            //stateStore1.Add(stateStoreKey, stateStoreValue); // TODO
                        }

                        INotifyUpdateProperty notifyUpdateProperty = newValue ?? oldValue;

                        if (notifyUpdateProperty != null)
                        {
                            notifyUpdateProperty.AfterFailUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                        }
                    }
                }

                // TODO: обработать если объект отправляют на удаление, а свойство не было загружено. Событие всё равно должно вызваться. Надо прокешировать рефлекшен по всем свойствам типа данных, чтобы каждый раз не дёргать его.
            }
        }

        /// <summary>
        /// Add data to state store.
        /// </summary>
        /// <param name="operationId">Operation id.</param>
        /// <param name="dataObjectType">Data object type.</param>
        /// <param name="dataObjectPrimaryKey">Data object primaryKey.</param>
        /// <param name="stateStoreValue">Value for state store.</param>
        private void AddToStateStore(Guid operationId, Type dataObjectType, object dataObjectPrimaryKey, Tuple<ObjectStatus, object, object> stateStoreValue)
        {
            if (!stateStore.ContainsKey(operationId))
            {
                stateStore.Add(operationId, new Dictionary<Type, IDictionary<object, Tuple<ObjectStatus, object, object>>>());
            }

            IDictionary<Type, IDictionary<object, Tuple<ObjectStatus, object, object>>> typeDictionary = stateStore[operationId];

            if (!typeDictionary.ContainsKey(dataObjectType))
            {
                typeDictionary.Add(dataObjectType, new Dictionary<object, Tuple<ObjectStatus, object, object>>());
            }

            IDictionary<object, Tuple<ObjectStatus, object, object>> primaryKeyDictionary = typeDictionary[dataObjectType];

            if (!primaryKeyDictionary.ContainsKey(dataObjectPrimaryKey))
            {
                primaryKeyDictionary.Add(dataObjectPrimaryKey, stateStoreValue);
            }
        }

        /// <summary>
        /// Get data from state store.
        /// </summary>
        /// <param name="operationId">Operation id.</param>
        /// <param name="dataObjectType">Data object type.</param>
        /// <param name="dataObjectPrimaryKey">Data object primaryKey.</param>
        /// <returns>Value from state store or <c>null</c> if it is absent.</returns>
        private Tuple<ObjectStatus, object, object> GetFromStateStore(Guid operationId, Type dataObjectType, object dataObjectPrimaryKey)
        {
            if (stateStore.ContainsKey(operationId))
            {
                IDictionary<Type, IDictionary<object, Tuple<ObjectStatus, object, object>>> typeDictionary = stateStore[operationId];

                if (typeDictionary.ContainsKey(dataObjectType))
                {
                    IDictionary<object, Tuple<ObjectStatus, object, object>> primaryKeyDictionary = typeDictionary[dataObjectType];

                    if (primaryKeyDictionary.ContainsKey(dataObjectPrimaryKey))
                    {
                        return primaryKeyDictionary[dataObjectPrimaryKey];
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
        /// <returns>Operation result.</returns>
        private bool RemoveFromStateStore(Guid operationId, Type dataObjectType, object dataObjectPrimaryKey)
        {
            if (stateStore.ContainsKey(operationId))
            {
                IDictionary<Type, IDictionary<object, Tuple<ObjectStatus, object, object>>> typeDictionary = stateStore[operationId];

                if (typeDictionary.ContainsKey(dataObjectType))
                {
                    IDictionary<object, Tuple<ObjectStatus, object, object>> primaryKeyDictionary = typeDictionary[dataObjectType];

                    if (primaryKeyDictionary.ContainsKey(dataObjectPrimaryKey))
                    {
                        return primaryKeyDictionary.Remove(dataObjectPrimaryKey);
                    }
                }
            }

            return false;
        }
    }
}
