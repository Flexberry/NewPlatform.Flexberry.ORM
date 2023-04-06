namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ICSSoft.STORMNET.Business.Interfaces;

    /// <summary>
    /// Провайдер бизнессервисов.
    /// </summary>
    public class BusinessServerProvider : IBusinessServerProvider
    {
        private static readonly ConcurrentDictionary<string, Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>>> atrCache = new ConcurrentDictionary<string, Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>>>();

        /// <summary>
        /// Container for dependency injection.
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessServerProvider"/> class with container for dependency injections.
        /// This class is a factory so it allows using container straight forward.
        /// </summary>
        /// <param name="serviceProvider">A container for proper injection into <see cref="BusinessServer"/> entities.</param>
        public BusinessServerProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Получить бизнес-сервер.
        /// </summary>
        /// <param name="dataObjectType">для объекта типа.</param>
        /// <param name="objectStatus">Статус объекта.</param>
        /// <param name="ds">Текущий сервис данных.</param>
        /// <returns>БСы обрабатывающие тип данных.</returns>
        public virtual BusinessServer[] GetBusinessServer(Type dataObjectType, ObjectStatus objectStatus, IDataService ds)
        {
            switch (objectStatus)
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
        /// Создать бизнес-сервер.
        /// </summary>
        /// <param name="bsType">Тип бизнес-сервера.</param>
        /// <returns>Экземпляр БСа.</returns>
        protected virtual BusinessServer CreateBusinessServer(Type bsType)
        {
            object bsFromProvider = serviceProvider.GetService(bsType);
            return bsFromProvider == null ? (BusinessServer)Activator.CreateInstance(bsType) : (BusinessServer)bsFromProvider;
        }

        /// <summary>
        /// Получить бизнес-сервер.
        /// </summary>
        /// <param name="dataObjectType">для объекта типа.</param>
        /// <param name="dsevent">событие.</param>
        /// <param name="ds">Текущий сервис данных.</param>
        /// <returns>БСы обрабатывающие тип данных.</returns>
        protected virtual BusinessServer[] GetBusinessServer(Type dataObjectType, DataServiceObjectEvents dsevent, IDataService ds)
        {
            if (dataObjectType == null)
            {
                throw new ArgumentNullException(nameof(dataObjectType));
            }

            var pairs = GetBusinessServerAttributesWithInheritCached(dataObjectType, dsevent);

            List<BusinessServer> bss = new List<BusinessServer>();
            foreach (var pair in pairs)
            {
                foreach (var atr in pair.Value)
                {
                    BusinessServer bs = CreateBusinessServer(atr.BusinessServerType);
                    bs.Order = atr.Order;
                    bs.DataService = ds;
                    bs.SetType(pair.Key);
                    bss.Insert(0, bs);
                }
            }

            return bss.OrderBy(x => x.Order).ToArray();
        }

        /// <summary>
        /// Получить иерархию атрибутов <see cref="BusinessServerAttribute" /> с кэшированием.
        /// </summary>
        /// <param name="dataObjectType">для объекта типа.</param>
        /// <param name="dsevent">событие.</param>
        /// <returns>Словарь иерархии атрибутов.</returns>
        protected static Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>> GetBusinessServerAttributesWithInheritCached(Type dataObjectType, DataServiceObjectEvents dsevent)
        {
            if (dataObjectType == null)
            {
                throw new ArgumentNullException(nameof(dataObjectType));
            }

            string key = dataObjectType.FullName + "." + dsevent;
            return atrCache.GetOrAdd(key, k => GetBusinessServerAttributesWithInherit(dataObjectType, dsevent));
        }

        /// <summary>
        /// Сформировать иерархию атрибутов <see cref="BusinessServerAttribute" />.
        /// </summary>
        /// <param name="dataObjectType">для объекта типа.</param>
        /// <param name="dsevent">событие.</param>
        /// <returns>Словарь иерархии атрибутов.</returns>
        protected static Dictionary<Type, IReadOnlyCollection<BusinessServerAttribute>> GetBusinessServerAttributesWithInherit(Type dataObjectType, DataServiceObjectEvents dsevent)
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

        /// <summary>
        /// Получить коллекцию атрибутов <see cref="BusinessServerAttribute" />.
        /// </summary>
        /// <param name="type">для объекта типа.</param>
        /// <param name="dsevent">событие.</param>
        /// <returns>Коллекция атрибутов для типа.</returns>
        protected static IReadOnlyCollection<BusinessServerAttribute> GetBusinessServerAttributes(Type type, DataServiceObjectEvents dsevent)
        {
            return type.GetCustomAttributes<BusinessServerAttribute>(false).Where(atr => (dsevent & atr.ServerEvents) == dsevent).OrderBy(atr => atr.Order).ToList();
        }
    }
}