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
        private IDictionary<string, Tuple<ObjectStatus, object, object>> stateStore;

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
                if (dataObject is INotifyUpdateObject notifyUpdateObject)
                {
                    notifyUpdateObject.BeforeUpdateObject(dataObject, dataObject.GetStatus(), dataObjects);
                }

                Type dataObjectType = dataObject.GetType();
                ObjectStatus status = dataObject.GetStatus();
                List<string> alteredPropertyNames = new List<string>(dataObject.GetAlteredPropertyNames());

                if (subscribedPropNotifiers.ContainsKey(dataObjectType) || status == ObjectStatus.Deleted)
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
                                string stateStoreKey = operationId.ToString() + dataObjectType.FullName + dataObject.__PrimaryKey + propertyName;
                                var stateStoreValue = new Tuple<ObjectStatus, object, object>(status, oldValue, newValue);

                                if (stateStore == null)
                                {
                                    stateStore = new Dictionary<string, Tuple<ObjectStatus, object, object>>();
                                }

                                stateStore.Add(stateStoreKey, stateStoreValue);

                                NotifierUpdatePropertyByType.BeforeUpdateProperty(dataObject, status, propertyName, oldValue, newValue);
                            }
                        }
                    }
                }

                foreach (string propertyName in alteredPropertyNames)
                {
                    Type propertyType = Information.GetPropertyType(dataObjectType, propertyName);

                    if (propertyType.IsSubclassOf(typeof(INotifyUpdateProperty)))
                    {
                        INotifyUpdateProperty oldValue = null;
                        INotifyUpdateProperty newValue = null;

                        string stateStoreKey = operationId.ToString() + dataObjectType.FullName + dataObject.__PrimaryKey + propertyName;

                        if (stateStore != null && stateStore.ContainsKey(stateStoreKey))
                        {
                            var valuesFromStateStore = stateStore[stateStoreKey];
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

                            if (stateStore == null)
                            {
                                stateStore = new Dictionary<string, Tuple<ObjectStatus, object, object>>();
                            }

                            stateStore.Add(stateStoreKey, stateStoreValue);
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
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="INotifyUpdateObjects"/>
        public virtual void AfterSuccessUpdateObjects(Guid operationId, IDataService dataService, IEnumerable<DataObject> dataObjects)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="INotifyUpdateObjects"/>
        public virtual void AfterFailUpdateObjects(Guid operationId, IDataService dataService, IEnumerable<DataObject> dataObjects)
        {
            throw new System.NotImplementedException();
        }
    }
}
