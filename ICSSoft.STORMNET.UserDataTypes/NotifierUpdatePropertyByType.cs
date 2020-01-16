namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Notifier default implementation for update properties of some technological types.
    /// </summary>
    public class NotifierUpdatePropertyByType : INotifyUpdatePropertyByType
    {
        /// <inheritdoc cref="INotifyUpdatePropertyByType"/>
        public virtual IDictionary<Type, IEnumerable<string>> GetSubscribedProperties()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="INotifyUpdatePropertyByType"/>
        public virtual void BeforeUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="INotifyUpdatePropertyByType"/>
        public virtual void AfterSuccessSqlUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="INotifyUpdatePropertyByType"/>
        public virtual void AfterSuccessUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="INotifyUpdatePropertyByType"/>
        public virtual void AfterFailUpdateProperty(DataObject dataObject, ObjectStatus status, string propertyName, object oldValue, object newValue)
        {
            throw new NotImplementedException();
        }
    }
}
