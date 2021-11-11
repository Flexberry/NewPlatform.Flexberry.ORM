﻿namespace ICSSoft.STORMNET.Business.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    /// <summary>
    /// Класс, содержащий бизнес-сервера для интерфейсов.
    /// </summary>
    public class InterfaceBusinessServer : BusinessServer
    {
        /// <summary>
        /// Список сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// </summary>
        private static IEnumerable<Assembly> assembliesForIReferencesNullDeleteSearch;

        /// <summary>
        /// Список сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// </summary>
        private static IEnumerable<Assembly> assembliesForIReferencesCascadeDeleteSearch;

        /// <summary>
        /// Кеш соответствия типа и списка типов, для которых он является мастером.
        /// </summary>
        private static Dictionary<Type, List<ReferencePropertyInfo>> cacheForReferencePropertyInfoLists;

        /// <summary>
        /// Список сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// При изменении данного свойства сбрасывается кеш соответствия типа и списка типов, для которых он является мастером.
        /// </summary>
        public static IEnumerable<Assembly> AssembliesForIReferencesNullDeleteSearch
        {
            get
            {
                return assembliesForIReferencesNullDeleteSearch;
            }

            set
            {
                if (assembliesForIReferencesNullDeleteSearch != value)
                {
                    cacheForReferencePropertyInfoLists = null;
                }

                assembliesForIReferencesNullDeleteSearch = value;
            }
        }

        /// <summary>
        /// Список сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// При изменении данного свойства сбрасывается кеш соответствия типа и списка типов, для которых он является мастером.
        /// </summary>
        public static IEnumerable<Assembly> AssembliesForIReferencesCascadeDeleteSearch
        {
            get
            {
                return assembliesForIReferencesCascadeDeleteSearch;
            }

            set
            {
                if (assembliesForIReferencesCascadeDeleteSearch != value)
                {
                    cacheForReferencePropertyInfoLists = null;
                }

                assembliesForIReferencesCascadeDeleteSearch = value;
            }
        }

        /// <summary>
        /// Задание списка сборок, где будет осуществляться поиск объектов, для которых переданный является мастером,
        /// при обработке интерфейсов <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete"/>
        /// и <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete"/> .
        /// Если настройка не будет задана, то поиск будет осуществляться только в сборке с удаляемым объектом.
        /// </summary>
        /// <param name="searchAssemblies">Список сборок.</param>
        public static void SetupAdditionalAssemblies(IEnumerable<Assembly> searchAssemblies)
        {
            AssembliesForIReferencesNullDeleteSearch = searchAssemblies;
            AssembliesForIReferencesCascadeDeleteSearch = searchAssemblies;
        }

        /// <summary>
        /// Определяем набор объектов, для которых переданный является мастером.
        /// </summary>
        /// <param name="masterObject">Объект, для которого мы будем искать набор объектов, чьим мастером он является.</param>
        /// <param name="searchAssemblies">Список сборок, где осуществляется поиск объектов, для которых переданный является мастером. Если <c>null</c>, то поиск осуществляется только в сборке класса <paramref name="masterObject"/>.</param>
        /// <returns>Набор объектов, для которых переданный является мастером.</returns>
        public static List<ReferencePropertyInfo> GetReferencedDataObjectsInfo(
            DataObject masterObject, IEnumerable<Assembly> searchAssemblies = null)
        {
            var realMasterObjectType = masterObject.GetType();
            var cache = GetCacheForReferencePropertyInfoLists();
            if (cache.ContainsKey(realMasterObjectType))
            {
                return cache[realMasterObjectType];
            }

            var referencePropertyInfos = new List<ReferencePropertyInfo>();
            var searchAssemblyArray = searchAssemblies == null
                                        ? new[] { realMasterObjectType.Assembly }
                                        : searchAssemblies;

            foreach (Assembly assembly in searchAssemblyArray)
            {
                referencePropertyInfos.AddRange(ReferencePropertyInfo.FormList(assembly, realMasterObjectType));
            }

            cache[realMasterObjectType] = referencePropertyInfos;
            return referencePropertyInfos;
        }

        /// <summary>
        /// Формируем представление, основываясь на информации о типе и необходимых в представлении свойств.
        /// </summary>
        /// <param name="referencePropertyInfo">Информации о типе и необходимых в представлении свойств.</param>
        /// <returns>Сформированное представление.</returns>
        public static View FormViewOnReferencePropertyInfo(ReferencePropertyInfo referencePropertyInfo)
        {
            if (referencePropertyInfo == null)
            {
                throw new ArgumentNullException("referencePropertyInfo");
            }

            var resultView = new View()
            {
                DefineClassType = referencePropertyInfo.TypeWithReference,
                Name = referencePropertyInfo.TypeWithReference.FullName + "View",
            };
            foreach (string referenceProperty in referencePropertyInfo.ReferenceProperties)
            {
                resultView.AddProperty(referenceProperty);
            }

            return resultView;
        }

        /// <summary>
        /// Формируем функцию ограничения, с помощью которой можно выявить все объекты, которые имеют мастеровую ссылку на интересующий объект.
        /// </summary>
        /// <param name="referenceProperties">Список свойств, где может содержаться ссылка на искомого мастера.</param>
        /// <param name="masterObject">Мастер, на который мы ищем ссылки.</param>
        /// <returns>Сформированная функция ограничения.</returns>
        public static Function FormLimitFunctionOnReferencePropertyInfo(List<string> referenceProperties, DataObject masterObject)
        {
            if (referenceProperties == null || !referenceProperties.Any())
            {
                throw new ArgumentNullException("referenceProperties");
            }

            if (masterObject == null)
            {
                throw new ArgumentNullException("masterObject");
            }

            var languageDef = SQLWhereLanguageDef.LanguageDef;
            object[] functionList = referenceProperties
                                .Select(referenceProperty => (object)languageDef.GetFunction(languageDef.funcEQ, new VariableDef(languageDef.StringType, referenceProperty), masterObject.__PrimaryKey))
                                .ToArray();

            return languageDef.GetFunction(languageDef.funcOR, functionList);
        }

        /// <summary>
        /// Вместо ссылки на удаляемого мастера проставляем <c>null</c> в соответствующие свойства объектов.
        /// </summary>
        /// <param name="masterDataObject">Удаляемый объект, ссылки на который необходимо почистить.</param>
        /// <param name="referenceObjectList">Список объектов, из которых нужно почистить ссылки на мастера, заменив их на <c>null</c>.</param>
        /// <param name="referencePropertyInfos">Набор информации о классах, для которых переданный объект может являться мастером, и соответствующие свойства, которыми они могут ссылаться на мастера.</param>
        public static void NullifyMasterReferences(DataObject masterDataObject, List<DataObject> referenceObjectList, List<ReferencePropertyInfo> referencePropertyInfos)
        {
            if (masterDataObject == null)
            {
                throw new ArgumentNullException("masterDataObject");
            }

            if (referenceObjectList == null)
            {
                throw new ArgumentNullException("referenceObjectList");
            }

            if (referencePropertyInfos == null)
            {
                throw new ArgumentNullException("referencePropertyInfos");
            }

            List<DataObject> referenceObjectListCopy = referenceObjectList.ToList();

            foreach (DataObject dataObject in referenceObjectListCopy)
            {
                // Зануляем ссылки.
                Type dataObjectType = dataObject.GetType();
                ReferencePropertyInfo referencePropertyInfo =
                    referencePropertyInfos.FirstOrDefault(x => x.TypeWithReference == dataObjectType);

                if (referencePropertyInfo == null)
                {
                    throw new ArgumentException("Список объектов, из которых нужно почистить ссылки на мастера, некорректен.");
                }

                DataObject currentDataObject = dataObject;
                List<string> filteredProperties = (from possibleProperty in referencePropertyInfo.ReferenceProperties
                                                   let propertyValue = Information.GetPropValueByName(currentDataObject, possibleProperty)
                                                   where propertyValue is DataObject
                                                         && propertyValue != null
                                                         && ((DataObject)propertyValue).__PrimaryKey.Equals(masterDataObject.__PrimaryKey)
                                                   select possibleProperty).ToList();

                foreach (string possibleProperty in filteredProperties)
                {
                    if (Information.GetPropertyNotNull(dataObjectType, possibleProperty))
                    {
                        throw new PropertyCouldnotBeNullException(possibleProperty, dataObject);
                    }

                    Information.SetPropValueByName(dataObject, possibleProperty, null);
                }
            }
        }

        /// <summary>
        /// Обработчик событий удаления объекта, реализующего интерфейс <see cref="IReferencesCascadeDelete"/>.
        /// </summary>
        /// <param name="UpdatedObject">Текущий удаляемый объект.</param>
        /// <returns>Массив объектов, которые тоже необходимо подвергнуть удалению.</returns>
        public DataObject[] OnUpdateIReferencesCascadeDelete(IReferencesCascadeDelete UpdatedObject)
        {
            if (!(UpdatedObject is DataObject))
            {
                throw new CantProcessingNonDataobjectTypeException();
            }

            var updatedDataObject = (DataObject)UpdatedObject;

            if (updatedDataObject.GetStatus() != ObjectStatus.Deleted)
            {
                return new DataObject[0];
            }

            List<ReferencePropertyInfo> referencePropertyInfos;
            var referenceObjectList = GetReferencedDataObjects(updatedDataObject, out referencePropertyInfos, AssembliesForIReferencesCascadeDeleteSearch);
            foreach (DataObject dataObject in referenceObjectList)
            {
                dataObject.SetStatus(ObjectStatus.Deleted);
            }

            return referenceObjectList.ToArray();
        }

        /// <summary>
        /// Обработчик событий удаления объекта, реализующего интерфейс <see cref="IReferencesNullDelete"/>.
        /// </summary>
        /// <param name="UpdatedObject">Текущий удаляемый объект.</param>
        /// <returns>Массив изменённых объектов (вместо ссылки на удаляемый объект проставлено <c>null</c>), которые нужно сохранить.</returns>
        public DataObject[] OnUpdateIReferencesNullDelete(IReferencesNullDelete UpdatedObject)
        {
            if (!(UpdatedObject is DataObject))
            {
                throw new CantProcessingNonDataobjectTypeException();
            }

            var updatedDataObject = (DataObject)UpdatedObject;

            if (updatedDataObject.GetStatus() != ObjectStatus.Deleted)
            {
                return new DataObject[0];
            }

            List<ReferencePropertyInfo> referencePropertyInfos;
            var referenceObjectList = GetReferencedDataObjects(
                updatedDataObject, out referencePropertyInfos, AssembliesForIReferencesNullDeleteSearch);
            NullifyMasterReferences(updatedDataObject, referenceObjectList, referencePropertyInfos);
            return referenceObjectList.ToArray();
        }

        /// <summary>
        /// Получаем список объектов, которые ссылаются на указанный объект как на мастера.
        /// </summary>
        /// <param name="masterObject">Объект, ссылающиеся на который объекты мы будем искать.</param>
        /// <param name="referencePropertyInfos">Набор информации о классах, для которых переданный объект может являться мастером, и соответствующие свойства, которыми они могут ссылаться на мастера.</param>
        /// <param name="searchAssemblies">Список сборок, где осуществляется поиск объектов, для которых переданный является мастером. Если <c>null</c>, то поиск осуществляется только в сборке класса <paramref name="masterObject"/>.</param>
        /// <returns>Список найденных объектов.</returns>
        public List<DataObject> GetReferencedDataObjects(
            DataObject masterObject,
            out List<ReferencePropertyInfo> referencePropertyInfos,
            IEnumerable<Assembly> searchAssemblies = null)
        {
            referencePropertyInfos = GetReferencedDataObjectsInfo(masterObject, searchAssemblies);

            if (!referencePropertyInfos.Any())
            {
                return new List<DataObject>();
            }

            var referencedObjectsList = new List<DataObject>();
            foreach (ReferencePropertyInfo referencePropertyInfo in referencePropertyInfos)
            {
                View view = FormViewOnReferencePropertyInfo(referencePropertyInfo);
                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(referencePropertyInfo.TypeWithReference, view);
                lcs.LimitFunction = FormLimitFunctionOnReferencePropertyInfo(
                                        referencePropertyInfo.ReferenceProperties,
                                        masterObject);

                referencedObjectsList.AddRange(DataService.LoadObjects(lcs).ToList());
            }

            return referencedObjectsList;
        }

        #region Вспомогательный класс.

        /// <summary>
        /// Вспомогательный класс для хранения информации о том, какие классы какие ссылки имеют на заданный мастеровой класс.
        /// </summary>
        public class ReferencePropertyInfo
        {
            /// <summary>
            /// Формируем структуру для хранения информации о том, какие классы какие ссылки имеют на заданный мастеровой класс.
            /// </summary>
            /// <param name="typeWithReference">Тип, в котором есть ссылки на мастеровой класс.</param>
            /// <param name="referenceProperties">Имена свойств, которыми тип ссылается на мастеровой класс.</param>
            private ReferencePropertyInfo(Type typeWithReference, List<string> referenceProperties)
            {
                if (typeWithReference == null)
                {
                    throw new ArgumentNullException("typeWithReference");
                }

                if (referenceProperties == null || referenceProperties.Count == 0)
                {
                    throw new ArgumentNullException("referenceProperties");
                }

                TypeWithReference = typeWithReference;
                ReferenceProperties = referenceProperties;
            }

            /// <summary>
            /// Тип, в котором есть ссылки на мастеровой класс.
            /// </summary>
            public Type TypeWithReference { get; private set; }

            /// <summary>
            /// Имена свойств, которыми тип ссылается на мастеровой класс.
            /// </summary>
            public List<string> ReferenceProperties { get; private set; }

            /// <summary>
            /// Формируем из предоставленной сборки соответствия между типом и списком свойства, которыми он ссылается на заданный мастеровой тип.
            /// </summary>
            /// <param name="assembly">Сборка, типы которой исследуются.</param>
            /// <param name="masterPropertyType">Тип, ссылки на который мы ищем.</param>
            /// <returns>Найденный список соответствий.</returns>
            public static List<ReferencePropertyInfo> FormList(Assembly assembly, Type masterPropertyType)
            {
                if (assembly == null)
                {
                    throw new ArgumentNullException(nameof(assembly));
                }

                if (masterPropertyType == null)
                {
                    throw new ArgumentNullException(nameof(masterPropertyType));
                }

                List<Type> dataObjectTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DataObject))).ToList();

                return (from currentCheckedType in dataObjectTypes
                        let masterReferenceProperties = Information.GetStorablePropertyNames(currentCheckedType)
                                                        .Where(x => Information.GetPropertyType(currentCheckedType, x).IsAssignableFrom(masterPropertyType) // Свойство имеет мастеровой тип или тип его предков.
                                                                    && Information.GetAgregatePropertyName(currentCheckedType) != x) // Это не детейл заданного типа.
                                                        .ToList()
                        where masterReferenceProperties.Any()
                        select new ReferencePropertyInfo(currentCheckedType, masterReferenceProperties)).ToList();
            }
        }

        #endregion Вспомогательный класс.

        /// <summary>
        /// Получение кеша соответствия типа и списка типов, для которых он является мастером.
        /// </summary>
        /// <returns>Кеш соответствия типа и списка типов, для которых он является мастером.</returns>
        private static Dictionary<Type, List<ReferencePropertyInfo>> GetCacheForReferencePropertyInfoLists()
        {
            if (cacheForReferencePropertyInfoLists == null)
            {
                cacheForReferencePropertyInfoLists = new Dictionary<Type, List<ReferencePropertyInfo>>();
            }

            return cacheForReferencePropertyInfoLists;
        }
    }
}
