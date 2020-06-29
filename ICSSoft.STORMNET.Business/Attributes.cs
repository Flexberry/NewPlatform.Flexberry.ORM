using System;
using System.Collections.Generic;
using System.Reflection;
using ICSSoft.STORMNET;
using System.Collections;

namespace ICSSoft.STORMNET.Business
{
    using System.Linq;

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

        /// <summary>
        /// Кеш бизнессерверов
        /// </summary>
        static private System.Collections.SortedList cache = new System.Collections.SortedList();

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
            // 2011-08-04 Братчиков: кешируем с учётом разных строк соединения. Это нужно для того чтобы не переписывать чужому бизнес-серверу датасервис
            string key = dataObjectType.FullName + "." + dsevent + "." + (ds != null ? (ds.CustomizationString ?? "salt") : "tlas").GetHashCode();
            lock(cache)
            {
                if (cache.ContainsKey(key))
                {
                    BusinessServer[] ret_bs = (BusinessServer[])cache[key];

                    foreach(BusinessServer bsi in ret_bs)
                    {
                        bsi.DataService = ds;
                    }

                    return ret_bs;
                }

                ArrayList bss = new ArrayList();
                bool needSort = false;
                while (dataObjectType != typeof(DataObject) && dataObjectType != typeof(object))
                { // TODO: разобраться с логикой выполнения и привести в соответствие со статьёй http://storm:3013/Otrabotka-polzovatelskih-operacii-v-processe-raboty-servisa-dannyh-integraciya-s-biznes-serverom.ashx.
                    // получим сначала бизнес-сервера у самого класса (не может быть больше одного)
                    ArrayList atrs = new ArrayList(dataObjectType.GetCustomAttributes(typeof(BusinessServerAttribute), false));

                    // добавим бизнес-сервера, которые достались от интерфейсов
                    // Smirnov: сортируем по имени, чтобы исключить зависимость от платформы.
                    Type[] interfaces = dataObjectType.GetInterfaces().OrderBy(i => i.FullName).ToArray();
                    List<Type> baseInterfaces = new List<Type>();
                    if (dataObjectType.BaseType != null)
                    {
                        baseInterfaces.AddRange(dataObjectType.BaseType.GetInterfaces());
                    }

                    foreach (Type interf in interfaces)
                    {
                        if (!baseInterfaces.Contains(interf))
                        {
                            atrs.AddRange(interf.GetCustomAttributes(typeof (BusinessServerAttribute), false));
                        }
                    }

                    // создадим инстанции бизнес-серверов и добавим в итоговый массив
                    foreach (BusinessServerAttribute atr in atrs)
                    {
                        if ((dsevent & atr.ServerEvents) == dsevent)
                        {
                            BusinessServer bs = (BusinessServer)Activator.CreateInstance(atr.BusinessServerType);
                            bs.DataService = ds;
                            bs.SetType(dataObjectType);
                            bss.Insert(0, bs);
                            if (atr.Order != 0)
                            {
                                bs.Order = atr.Order;
                                needSort = true;
                            }
                        }
                    }

                    dataObjectType = dataObjectType.BaseType;
                }

                // пересортируем бизнессерверы
                if (needSort)
                {
                    // Получим отсортированный список, в котором будет упорядоченная коллекция с допустимыми одинаковыми ключами
                    // bss.Sort(new BusinesServerComparer());
                    ArrayList sortedArList = new ArrayList();
                    SortedList sl = new SortedList();
                    foreach (BusinessServer bs in bss)
                    {
                        if (!sl.ContainsKey(bs.Order))
                        {
                            sl.Add(bs.Order, new ArrayList());
                        }

                        ((ArrayList)sl[bs.Order]).Add(bs);
                    }

                    foreach (DictionaryEntry entry in sl)
                    {
                        ArrayList arl = (ArrayList)entry.Value;
                        sortedArList.AddRange(arl);
                    }

                    bss = sortedArList;
                }

                BusinessServer[] res = (BusinessServer[])bss.ToArray(typeof(BusinessServer));
                cache.Add(key, res);
                return res;
            }
        }
    }
}