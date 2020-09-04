namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Типы событий на которые могу быть навешены обработчики
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
        OnAllEvents = OnInsertToStorage | OnUpdateInStorage | OnDeleteFromStorage
    }

    /// <summary>
    /// невозможно применить атрибут к этому типу
    /// </summary>
    public class CantApplyBusinessServerAttributeWithNotBusinessServiceTypeException: Exception
    {
        /// <summary>
        /// проверяемый тип
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
    /// Атрибут лоя установки бизнессервера обработки событий
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false)]
    public class BusinessServerAttribute: Attribute
    {
        /// <summary>
        /// Тип бизнессервера
        /// </summary>
        public System.Type BusinessServerType;

        /// <summary>
        /// События
        /// По умолчаню OnAllEvents
        /// </summary>
        public DataServiceObjectEvents ServerEvents = DataServiceObjectEvents.OnAllEvents;

        /// <summary>
        /// Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним. По-умолчанию: 0
        /// </summary>
        public int Order = 0;

        /// <summary>
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
        /// <param name="order">Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним</param>
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
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
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
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
        public BusinessServerAttribute(string businessServerType)
            : this(Type.GetType(businessServerType, true, true))
        {
        }

        /// <summary>
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
        /// <param name="serverEvents">События</param>
        public BusinessServerAttribute(System.Type businessServerType, DataServiceObjectEvents serverEvents)
            : this(businessServerType)
        {
            ServerEvents = serverEvents;
        }

        /// <summary>
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
        /// <param name="serverEvents">События</param>
        /// <param name="order">Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним</param>
        public BusinessServerAttribute(System.Type businessServerType, DataServiceObjectEvents serverEvents, int order)
            : this(businessServerType)
        {
            ServerEvents = serverEvents;
            Order = order;
        }

        /// <summary>
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
        /// <param name="serverEvents">События</param>
        public BusinessServerAttribute(string businessServerType, DataServiceObjectEvents serverEvents)
            : this(Type.GetType(businessServerType, true, true), serverEvents)
        {
        }

        /// <summary>
        /// Бизнессервер
        /// </summary>
        /// <param name="businessServerType">Тип бизнессервера</param>
        /// <param name="serverEvents">События</param>
        /// <param name="order">Упорядочение бизнес-серверов. 0 - выполнится раньше остальных, int.MaxValue - выполнится последним</param>
        public BusinessServerAttribute(string businessServerType, DataServiceObjectEvents serverEvents, int order)
            : this(Type.GetType(businessServerType, true, true), serverEvents, order)
        {
        }
    }

    /// <summary>
    /// Провайдер бизнессервисов
    /// </summary>
    public class BusinessServerProvider
    {
        private BusinessServerProvider()
        {
        }

        private static ConcurrentDictionary<string, Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>>> atrCache = new ConcurrentDictionary<string, Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>>>();

        /// <summary>
        /// Получить бизнессервер
        /// </summary>
        /// <param name="dataObjectType">для объекта типа</param>
        /// <param name="objectStatus">Статус объекта</param>
        /// <returns>бизнессервер</returns>
        static public BusinessServer[] GetBusinessServer(System.Type dataObjectType, ObjectStatus objectStatus, IDataService ds)
        {
            switch(objectStatus)
            {
                case ObjectStatus.Altered:
                    return GetBusinessServer(dataObjectType, DataServiceObjectEvents.OnUpdateInStorage, ds);
                case ObjectStatus.Created:
                    return GetBusinessServer(dataObjectType, DataServiceObjectEvents.OnInsertToStorage, ds);
                case ObjectStatus.Deleted:
                    return GetBusinessServer(dataObjectType, DataServiceObjectEvents.OnDeleteFromStorage, ds);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Получить бизнессерве
        /// </summary>
        /// <param name="dataObjectType">для объекта типа</param>
        /// <param name="dsevent">событие</param>
        /// <returns></returns>
        static public BusinessServer[] GetBusinessServer(System.Type dataObjectType, DataServiceObjectEvents dsevent, IDataService ds)
        {
            var pairs = GetBusinessServerAttributesWithInheritCached(dataObjectType, dsevent);

            ArrayList bss = new ArrayList();
            foreach (var pair in pairs)
            {
                foreach (var atr in pair.Value)
                {
                    BusinessServer bs = (BusinessServer)Activator.CreateInstance(atr.BusinessServerType);
                    bs.Order = atr.Order;
                    bs.DataService = ds;
                    bs.SetType(pair.Key);
                    bss.Insert(0, bs);
                }
            }

            return (BusinessServer[])bss.ToArray(typeof(BusinessServer));
        }

        private static Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>> GetBusinessServerAttributesWithInheritCached(Type dataObjectType, DataServiceObjectEvents dsevent)
        {
            string key = dataObjectType.FullName + "." + dsevent;
            return atrCache.GetOrAdd(key, k => GetBusinessServerAttributesWithInherit(dataObjectType, dsevent));
        }

        private static Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>> GetBusinessServerAttributesWithInherit(Type dataObjectType, DataServiceObjectEvents dsevent)
        {
            var atrs = new Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>>();
            while (dataObjectType != typeof(DataObject) && dataObjectType != typeof(object) && dataObjectType != null)
            {
                // TODO: разобраться с логикой выполнения и привести в соответствие со статьёй http://storm:3013/Otrabotka-polzovatelskih-operacii-v-processe-raboty-servisa-dannyh-integraciya-s-biznes-serverom.ashx.
                // получим сначала бизнес-сервера у самого класса (не может быть больше одного)
                atrs[dataObjectType] = GetBusinessServerAttributes(dataObjectType, dsevent);

                // добавим бизнес-сервера, которые достались от интерфейсов.
                // Smirnov: вытягиваются все интерфейсы, в тч и унаследованные.
                // Smirnov: сортируем по имени, чтобы исключить зависимость от платформы.
                Type[] interfaces = dataObjectType.GetInterfaces().OrderBy(i => i.FullName).ToArray();
                Type[] baseInterfaces = dataObjectType.BaseType?.GetInterfaces();

                foreach (Type interf in interfaces)
                {
                    if (baseInterfaces == null || !baseInterfaces.Contains(interf))
                    {
                        atrs[interf] = GetBusinessServerAttributes(interf, dsevent);
                    }
                }

                dataObjectType = dataObjectType.BaseType;
            }

            return atrs;
        }

        private static IReadOnlyCollection<BusinessServerAttribute> GetBusinessServerAttributes(Type type, DataServiceObjectEvents dsevent)
        {
            return type.GetCustomAttributes<BusinessServerAttribute>(false).Where(atr => (dsevent & atr.ServerEvents) == dsevent).OrderBy(atr => atr.Order).ToList();
        }
    }
}