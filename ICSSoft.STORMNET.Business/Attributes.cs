namespace ICSSoft.STORMNET.Business
{
    using System;

    /// <summary>
    /// Типы событий на которые могу быть навешены обработчики.
    /// </summary>
    [Flags]
    public enum DataServiceObjectEvents
    {
        /// <summary>
        /// ???
        /// </summary>
        OnAnyEvent = 0,

        /// <summary>
        /// На добавление
        /// </summary>
        OnInsertToStorage = 1,

        /// <summary>
        /// На изменение
        /// </summary>
        OnUpdateInStorage = 2,

        /// <summary>
        /// На удаление
        /// </summary>
        OnDeleteFromStorage = 4,

        /// <summary>
        /// На все
        /// </summary>
        OnAllEvents = OnInsertToStorage | OnUpdateInStorage | OnDeleteFromStorage,
    }

    /// <summary>
    /// невозможно применить атрибут к этому типу.
    /// </summary>
    public class CantApplyBusinessServerAttributeWithNotBusinessServiceTypeException : Exception
    {
        /// <summary>
        /// проверяемый тип.
        /// </summary>
        public Type CheckingType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ct"></param>
        public CantApplyBusinessServerAttributeWithNotBusinessServiceTypeException(Type ct)
        {
            CheckingType = ct;
        }
    }

    /// <summary>
    /// Атрибут лоя установки бизнессервера обработки событий.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class BusinessServerAttribute : Attribute
    {
        /// <summary>
        /// Тип бизнессервера.
        /// </summary>
        public System.Type BusinessServerType;

        /// <summary>
        /// События
        /// По умолчаню OnAllEvents.
        /// </summary>
        public DataServiceObjectEvents ServerEvents = DataServiceObjectEvents.OnAllEvents;

        /// <summary>
        /// Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним. По-умолчанию: 0.
        /// </summary>
        public int Order = 0;

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        /// <param name="order">Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним.</param>
        public BusinessServerAttribute(System.Type businessServerType, int order)
        {
            Order = order;
            if (businessServerType.IsSubclassOf(typeof(BusinessServer)))
            {
                BusinessServerType = businessServerType;
            }
            else
            {
                throw new CantApplyBusinessServerAttributeWithNotBusinessServiceTypeException(businessServerType);
            }
        }

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        public BusinessServerAttribute(System.Type businessServerType)
        {
            if (businessServerType.IsSubclassOf(typeof(BusinessServer)))
            {
                BusinessServerType = businessServerType;
            }
            else
            {
                throw new CantApplyBusinessServerAttributeWithNotBusinessServiceTypeException(businessServerType);
            }
        }

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        public BusinessServerAttribute(string businessServerType)
            : this(Type.GetType(businessServerType, true, true))
        {
        }

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        /// <param name="serverEvents">События.</param>
        public BusinessServerAttribute(System.Type businessServerType, DataServiceObjectEvents serverEvents)
            : this(businessServerType)
        {
            ServerEvents = serverEvents;
        }

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        /// <param name="serverEvents">События.</param>
        /// <param name="order">Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним.</param>
        public BusinessServerAttribute(System.Type businessServerType, DataServiceObjectEvents serverEvents, int order)
            : this(businessServerType)
        {
            ServerEvents = serverEvents;
            Order = order;
        }

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        /// <param name="serverEvents">События.</param>
        public BusinessServerAttribute(string businessServerType, DataServiceObjectEvents serverEvents)
            : this(Type.GetType(businessServerType, true, true), serverEvents)
        {
        }

        /// <summary>
        /// Бизнессервер.
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера.</param>
        /// <param name="serverEvents">События.</param>
        /// <param name="order">Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним.</param>
        public BusinessServerAttribute(string businessServerType, DataServiceObjectEvents serverEvents, int order)
            : this(Type.GetType(businessServerType, true, true), serverEvents, order)
        {
        }
    }
}
