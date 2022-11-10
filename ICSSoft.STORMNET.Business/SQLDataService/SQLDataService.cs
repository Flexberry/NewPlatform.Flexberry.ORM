namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Security;

    using Unity;

    using SpecColl = System.Collections.Specialized;
    using STORMDO = ICSSoft.STORMNET;
    using STORMFunction = ICSSoft.STORMNET.FunctionalLanguage.Function;
    using StringCollection = System.Collections.Specialized.StringCollection;

    /// <summary>
    /// Делегат для события создания команды.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OnCreateCommandEventHandler(object sender, CreateCommandEventArgs e);

    /// <summary>
    /// Событие при генерации SQL Select запроса (перед).
    /// </summary>
    public delegate void OnGenerateSQLSelectEventHandler(object sender, GenerateSQLSelectQueryEventArgs e);

    /// <summary>
    /// Событие при генерации SQL Select запроса (после).
    /// </summary>
    public delegate void AfterGenerateSQLSelectQueryEventHandler(object sender, GenerateSQLSelectQueryEventArgs e);

    /// <summary>
    /// The before update objects event handler.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void BeforeUpdateObjectsEventHandler(object sender, DataObjectsEventArgs e);

    /// <summary>
    /// The after update objects event handler.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    public delegate void AfterUpdateObjectsEventHandler(object sender, DataObjectsEventArgs e);

    /// <summary>
    /// Data service for SQL storage.
    /// </summary>
    public abstract partial class SQLDataService : System.ComponentModel.Component, IDataService
    {
        /// <summary>
        /// The prv instance id.
        /// </summary>
        private Guid prvInstanceId = Guid.NewGuid();

        /// <summary>
        /// The prv storage type.
        /// </summary>
        private StorageTypeEnum prvStorageType = StorageTypeEnum.SimpleStorage;

        /// <summary>
        /// Тип хранилища.
        /// </summary>
        public StorageTypeEnum StorageType
        {
            get
            {
                return prvStorageType;
            }

            set
            {
                prvStorageType = value;
            }
        }

        /// <summary>
        /// An instance of the class responsible for converting values to a string for the SQL query.
        /// See <see cref="IConverterToQueryValueString"/>.
        /// </summary>
        public IConverterToQueryValueString ConverterToQueryValueString { get; set; }

        /// <summary>
        /// An instance of the class for custom process updated objects.
        /// See <see cref="INotifyUpdateObjects"/>.
        /// </summary>
        public INotifyUpdateObjects NotifierUpdateObjects { get; set; }

        /// <summary>
        /// Преобразовать значение в SQL строку.
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <param name="convertValue">делегат для преобразования констант.</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов.</param>
        /// <returns></returns>
        public virtual string FunctionToSql(
            SQLWhereLanguageDef sqlLangDef,
            Function function,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ключ инстанции сервиса.
        /// </summary>
        /// <returns>Instance key.</returns>
        public Guid GetInstanceId()
        {
            return prvInstanceId;
        }

        ////------ Э Т О       Н А Д О     П Е Р Е Г Р У З И Т Ь

        /// <summary>
        /// Вернуть объект <see cref="System.Data.IDbConnection"/>.
        /// </summary>
        /// <returns>Коннекция к БД.</returns>
        public abstract System.Data.IDbConnection GetConnection();

        /// <summary>
        /// A factory to create instances of the data source classes.
        /// </summary>
        public abstract DbProviderFactory ProviderFactory { get; }

        ////-----------------------------------------------------

        private string customizationString;
        private string _customizationStringName;

        /// <summary>
        /// Настроичная строка (строка соединения).
        /// </summary>
        public string CustomizationString
        {
            get { return customizationString; }
            set { customizationString = value; }
        }

        /// <summary>
        /// Имплементация интерфейса <see cref="IPasswordHasher"/> для хеширования пароля.
        /// </summary>
        private IConfigResolver _configResolver;

        /// <summary>
        /// Получение инстации класса для разрешения свойств классов на основе данных из файла конфигурации приложения.
        /// </summary>
        private IConfigResolver ConfigResolver
        {
            get
            {
                if (_configResolver != null)
                {
                    return _configResolver;
                }

                IUnityContainer container = UnityFactory.GetContainer();
                _configResolver = container.Resolve<IConfigResolver>();

                return _configResolver;
            }
        }

        /// <summary>
        /// Свойство для установки строки соединения по имени.
        /// </summary>
        public string CustomizationStringName
        {
            get
            {
                return _customizationStringName;
            }

            set
            {
                _customizationStringName = value;
                CustomizationString = ConfigResolver.ResolveConnectionString(_customizationStringName);
            }
        }

        /// <summary>
        /// Делегат для смены строки соединения.
        /// </summary>
        public static ChangeCustomizationStringDelegate ChangeCustomizationString = null;

        private bool _doNotChangeCustomizationString = false;

        /// <summary>
        /// Не менять строку соединения общим делегатом ChangeCustomizationString.
        /// </summary>
        public bool DoNotChangeCustomizationString
        {
            get { return _doNotChangeCustomizationString; }
            set { _doNotChangeCustomizationString = value; }
        }

        private System.Collections.SortedList prvTypesByKeys;

        /// <summary>
        /// Gets the types.
        /// </summary>
        protected System.Collections.SortedList Types
        {
            get
            {
                if (StorageType == StorageTypeEnum.HierarchicalStorage)
                {
                    if (prvTypesByKeys == null)
                    {
                        prvTypesByKeys = new SortedList();
                        object s = null;
                        object[][] res = ReadFirst("select TypeId,TypeName from __TYPES", ref s, 0);
                        for (int i = 0; i < res.Length; i++)
                        {
                            try
                            {
                                object typeKey = res[i][0];
                                string typeName = ((string)res[i][1]).Trim();
                                Type tp = Type.GetType(typeName);
                                prvTypesByKeys.Add(typeKey, tp);
                            }
                            catch { }
                        }
                    }

                    return prvTypesByKeys;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Сервис подсистемы полномочий, который применяется для проверки прав доступа. Рекомендуется устанавливать его через конструктор, в противном случае используется настройка в Unity.
        /// </summary>
        public ISecurityManager SecurityManager
        {
            get
            {
                if (_securityManager == null)
                {
                    IUnityContainer container = UnityFactory.GetContainer();
                    _securityManager = container.Resolve<ISecurityManager>();
                }

                return _securityManager;
            }

            protected set
            {
                _securityManager = value;
            }
        }

        private IAuditService _auditService;

        /// <inheritdoc/>
        public IAuditService AuditService
        {
            get
            {
                if (_auditService != null)
                {
                    return _auditService;
                }

                return ICSSoft.STORMNET.Business.Audit.AuditService.Current;
            }
        }

        /// <summary>
        /// Возвращает количество объектов удовлетворяющих запросу.
        /// </summary>
        /// <param name="customizationStruct">
        /// Что выбираем.
        /// </param>
        /// <returns>Количество объектов.</returns>
        public virtual int GetObjectsCount(LoadingCustomizationStruct customizationStruct)
        {
            RunChangeCustomizationString(customizationStruct.LoadingTypes);

            // Применим полномочия на строки
            ApplyReadPermissions(customizationStruct, SecurityManager);

            string query = string.Format(
                "Select count(*) from ({0}) QueryForGettingCount", GenerateSQLSelect(customizationStruct, true));
            object state = null;
            object[][] res = ReadFirst(query, ref state, customizationStruct.LoadingBufferSize);
            return (int)Convert.ChangeType(res[0][0], typeof(int));
        }

        /// <summary>
        /// Применение полномочий на чтение строк.
        /// </summary>
        /// <param name="customizationStruct">Настройка выборки, которая будет изменена.</param>
        /// <param name="securityManager">Менеджер полномочий.</param>
        private static void ApplyReadPermissions(LoadingCustomizationStruct customizationStruct, ISecurityManager securityManager)
        {
            object limitObject;
            bool canAccess;
            var operationResult = securityManager.GetLimitForAccess(
                customizationStruct.View.DefineClassType, tTypeAccess.Read, out limitObject, out canAccess);
            STORMFunction limit = limitObject as STORMFunction;
            if (operationResult == OperationResult.Успешно)
            {
                if (limit != null)
                {
                    // Применим его к lcs через И.
                    if (customizationStruct.LimitFunction == null)
                    {
                        customizationStruct.LimitFunction = limit;
                    }
                    else
                    {
                        FunctionalLanguage.SQLWhere.SQLWhereLanguageDef ldef =
                            FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                        customizationStruct.LimitFunction = ldef.GetFunction(
                            ldef.funcAND, customizationStruct.LimitFunction, limit);
                    }

                    // Убеждаемся, что все свойства из ограничения есть в представлении. Удалим добавленное из ограничения "STORMMainObjectKey".
                    var properties = new List<string>(customizationStruct.LimitFunction.GetLimitProperties().Where(x => x != SQLWhereLanguageDef.StormMainObjectKey));
                    customizationStruct.View.AddProperties(properties.ToArray());
                }
            }
            else
            {
                // TODO: тут надо подумать что будем делать. Наверное надо вызывать исключение и не давать ничего. Пока просто запишем в лог и не будем показывать ошибку.
                LogService.LogError(string.Format("SecurityManager.GetLimitForAccess: {0}", operationResult));
            }
        }

        /// <summary>
        /// Возвращает индекс первого объекта, встретившегося в массиве,
        /// при загрузке по указанному lcs. Объекты задаются через lf.
        /// </summary>
        /// <param name="lcs">Массив, в котором ищем.</param>
        /// <param name="limitFunction">Что собственно ищем.</param>
        /// <returns>Индекс первого элемента, если он был найден. Иначе -1.</returns>
        public int GetObjectIndex(LoadingCustomizationStruct lcs, FunctionalLanguage.Function limitFunction)
        {
            int? res = GetObjectIndexesWithPks(lcs, limitFunction).Select(s => s.Key).Cast<int?>().FirstOrDefault();
            return res.HasValue ? res.Value : -1;
        }

        /// <summary>
        /// Возвращает индексы объектов, встретившихся в массиве,
        /// при загрузке по указанному lcs. Объекты задаются через lf.
        /// </summary>
        /// <param name="lcs">Массив, в котором ищем.</param>
        /// <param name="limitFunction">Функция ограничения, определяющая искомые объекты.</param>
        /// <returns>Массив индексов найденных объектов. Не возвращает null.</returns>
        public int[] GetObjectIndexes(LoadingCustomizationStruct lcs, FunctionalLanguage.Function limitFunction)
        {
            return GetObjectIndexesWithPks(lcs, limitFunction).Select(s => s.Key).ToArray();
        }

        /// <summary>
        /// Возвращает индексы и ключи объектов, встретившихся в массиве,
        /// при загрузке по указанному lcs. Объекты задаются через lf.
        /// </summary>
        /// <param name="lcs">Массив, в котором ищем.</param>
        /// <param name="limitFunction">Функция ограничения, определяющая искомые объекты.</param>
        /// <param name="maxResults">
        /// Максимальное число возвращаемых результатов.
        /// Этот параметр не соответствует. <code>lcs.ReturnTop</code>, а устанавливает максимальное число
        /// искомых объектов, тогда как. <code>lcs.ReturnTop</code> ограничивает число объектов, в которых
        /// проводится поиск.
        /// Если значение не определено (<c>null</c>), то возвращаются все найденные результаты.
        /// </param>
        /// <returns>
        /// Массив индексов найденных объектов начиная с 1. Не возвращает null.
        /// </returns>
        public virtual IDictionary<int, string> GetObjectIndexesWithPks(LoadingCustomizationStruct lcs, FunctionalLanguage.Function limitFunction, int? maxResults = null)
        {
            var ret = new Dictionary<int, string>();
            if (lcs == null || limitFunction == null)
            {
                return ret;
            }

            if (maxResults.HasValue && maxResults < 0)
            {
                throw new ArgumentOutOfRangeException("maxResults", "Максимальное число возвращаемых результатов не может быть отрицательным.");
            }

            RunChangeCustomizationString(lcs.LoadingTypes);

            bool usedSorting = false;
            if (lcs.ColumnsSort == null || lcs.ColumnsSort.Length == 0)
            {
                lcs.ColumnsSort = new[] { new ColumnsSortDef("__PrimaryKey", SortOrder.Asc) };
            }
            else
            {
                usedSorting = true;
            }

            string innerQuery = GenerateSQLSelect(lcs, false);

            // надо добавить RowNumber
            // top int.MaxValue
            int orderByIndex = usedSorting ? innerQuery.ToLower().LastIndexOf("order by ") : -1;
            string orderByExpr = string.Empty, nl = Environment.NewLine;
            if (orderByIndex > -1)
            {
                orderByExpr = innerQuery.Substring(orderByIndex);
            }

            int fromInd = innerQuery.ToLower().IndexOf("from");

            if (!string.IsNullOrEmpty(orderByExpr))
            {
                innerQuery = innerQuery.Substring(0, innerQuery.Length - orderByExpr.Length);
                innerQuery = innerQuery.Insert(fromInd, $",{nl}row_number() over ({orderByExpr}) as \"RowNumber\"{nl}");
            }
            else
            {
                innerQuery = innerQuery.Insert(fromInd, $",{nl}row_number() over (ORDER BY {PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey)} ) as \"RowNumber\"{nl}");
            }

            string query = string.Format(
                "SELECT{3} \"RowNumber\", {5} FROM {1}({0}) QueryForGettingIndex {1} WHERE ({2}) {4}",
                innerQuery,
                nl,
                LimitFunction2SQLWhere(limitFunction),
                maxResults.HasValue ? (" TOP " + maxResults) : string.Empty,
                orderByExpr,
                PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey));

            object state = null;
            object[][] res = ReadFirst(query, ref state, lcs.LoadingBufferSize);
            if (res != null)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    object pk = res[i][1];

                    pk = Information.TranslateValueToPrimaryKeyType(lcs.LoadingTypes[0], pk);

                    ret[(int)Convert.ChangeType(res[i][0], typeof(int))] = pk.ToString();
                }

                return ret;
            }

            return ret;
        }

        /// <summary>
        /// Construct data service with default settings.
        /// </summary>
        public SQLDataService()
        {
            UseCommandTimeout = false;
            string commandTimeout = null;

            commandTimeout = System.Configuration.ConfigurationManager.AppSettings["SQLDataServiceCommandTimeout"];

            if (commandTimeout == null) // lanin: Для совместимости с 2003-м Штормом.
            {
                commandTimeout = System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeout"];
            }

            if (commandTimeout != null)
            {
                try
                {
                    CommandTimeout = int.Parse(commandTimeout);
                    UseCommandTimeout = true;
                }
                catch
                {
                    UseCommandTimeout = false;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDataService"/> class with specified security manager.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        public SQLDataService(ISecurityManager securityManager)
            : this()
        {
            _securityManager = securityManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDataService"/> class with specified converter.
        /// </summary>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        public SQLDataService(IConverterToQueryValueString converterToQueryValueString)
            : this()
        {
            ConverterToQueryValueString = converterToQueryValueString ?? throw new ArgumentNullException(nameof(converterToQueryValueString));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDataService"/> class with specified security manager and audit service.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service.</param>
        public SQLDataService(ISecurityManager securityManager, IAuditService auditService)
            : this()
        {
            _securityManager = securityManager;
            _auditService = auditService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLDataService"/> class with specified security manager, audit service and converter.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service instance.</param>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        /// <param name="notifierUpdateObjects">An instance of the class for custom process updated objects.</param>
        public SQLDataService(ISecurityManager securityManager, IAuditService auditService, IConverterToQueryValueString converterToQueryValueString, INotifyUpdateObjects notifierUpdateObjects)
            : this(securityManager, auditService)
        {
            ConverterToQueryValueString = converterToQueryValueString ?? throw new ArgumentNullException(nameof(converterToQueryValueString));
            NotifierUpdateObjects = notifierUpdateObjects;
        }

        private ICSSoft.STORMNET.TypeUsage fldTypeUsage;

        /// <summary>
        ///
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ICSSoft.STORMNET.TypeUsage TypeUsage
        {
            get
            {
                if (fldTypeUsage == null)
                {
                    fldTypeUsage = ICSSoft.STORMNET.TypeUsageProvider.TypeUsage;
                }

                return fldTypeUsage;
            }

            set
            {
                fldTypeUsage = value;
            }
        }

        /// <inheritdoc cref="IDataService.LoadObject(DataObject, bool, bool, DataObjectCache)"/>
        public virtual void LoadObject(
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            LoadObject(new STORMDO.View(dobject.GetType(), STORMDO.View.ReadType.OnlyThatObject), dobject, clearDataObject, checkExistingObject, dataObjectCache);
        }

        /// <inheritdoc cref="IDataService.LoadObject(string, DataObject, bool, bool, DataObjectCache)"/>
        public virtual void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            LoadObject(STORMDO.Information.GetView(dataObjectViewName, dobject.GetType()), dobject, clearDataObject, checkExistingObject, dataObjectCache);
        }

        /// <inheritdoc cref="IDataService.LoadObject(View, DataObject, DataObjectCache)"/>
        public virtual void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache)
        {
            LoadObject(dataObjectView, dobject, true, true, dataObjectCache);
        }

        private ChangeViewForTypeDelegate fchangeViewForTypeDelegate = null;

        protected void prv_AddMasterObjectsToCache(DataObject dataobject, System.Collections.ArrayList arrl, DataObjectCache dataObjectCache)
        {
            arrl.Add(dataobject);
            Type type = dataobject.GetType();
            string[] allPropertyNames = Information.GetAllPropertyNames(type);
            foreach (string sPropName in allPropertyNames)
            {
                if (Information.IsStoredProperty(type, sPropName))
                {
                    object val = Information.GetPropValueByName(dataobject, sPropName);

                    if (val != null && val is DataObject && !arrl.Contains(val))
                    {
                        DataObject master = (DataObject)val;

                        dataObjectCache.AddDataObject(master);

                        prv_AddMasterObjectsToCache(master, arrl, dataObjectCache);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузка объекта с указанной коннекцией в рамках указанной транзакции.
        /// </summary>
        /// <param name="dataObjectView">Представление, по которому будет зачитываться объект.</param>
        /// <param name="dobject">Объект, который будет дочитываться/зачитываться.</param>
        /// <param name="сlearDataObject">Следует ли при вычитке очистить поля существующего объекта данных.</param>
        /// <param name="сheckExistingObject">Проверить существовние встречающихся при вычитке объектов.</param>
        /// <param name="dataObjectCache">Кэш объектов.</param>
        /// <param name="connection">Коннекция, через которую будет происходить вычитка.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет проходить вычитка.</param>
        public virtual void LoadObjectByExtConn(
            View dataObjectView,
            DataObject dobject,
            bool сlearDataObject,
            bool сheckExistingObject,
            DataObjectCache dataObjectCache,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                Type dataObjectType = dobject.GetType();
                dataObjectCache.AddDataObject(dobject);

                if (сlearDataObject)
                {
                    dobject.Clear();
                }
                else
                {
                    prv_AddMasterObjectsToCache(dobject, new ArrayList(), dataObjectCache);
                }

                var prevPrimaryKey = dobject.__PrimaryKey;
                var lcs = GetLcsPrimaryKey(dobject, dataObjectView);

                ApplyLimitForAccess(lcs);

                // Cтроим запрос.
                StorageStructForView[] storageStruct;
                string query = GenerateSQLSelect(lcs, false, out storageStruct, false);

                // Получаем данные.
                object state = null;
                object[][] resValue = ReadFirstByExtConn(query, ref state, 0, connection, transaction);
                if (resValue == null)
                {
                    if (сheckExistingObject)
                    {
                        throw new CantFindDataObjectException(dataObjectType, dobject.__PrimaryKey);
                    }
                    else
                    {
                        return;
                    }
                }

                DataObject[] helpDataObjectArray = { dobject };

                Utils.ProcessingRowsetDataRef(
                    resValue, new[] { dataObjectType }, storageStruct, lcs, helpDataObjectArray, this, Types, сlearDataObject, dataObjectCache, SecurityManager, connection, transaction);

                if (dobject.Prototyped)
                {
                    dobject.SetStatus(ObjectStatus.Created);
                    dobject.SetLoadingState(LoadingState.NotLoaded);
                    dobject.__PrimaryKey = prevPrimaryKey;
                }
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dataObject">объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        /// <param name="dataObjectCache">свой кеш объектов.</param>
        public virtual void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dataObject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView), "Не указано представление для загрузки объекта. Обратитесь к разработчику.");
            }

            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject), "Не указан объект для загрузки. Обратитесь к разработчику.");
            }

            Type doType = dataObject.GetType();
            RunChangeCustomizationString(new Type[] { doType });

            LoadObjectByExtConn(dataObjectView, dataObject, clearDataObject, checkExistingObject, dataObjectCache, GetConnection(), null);
        }

        /// <summary>
        /// Метод для дочитки объекта данных. Загруженные ранее свойства не затираются, изменённые свойства не затираются. Подменяются поштучно свойства копии данных. TODO: дописать тесты, проверить и сделать публичным.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dataObject">бъект данных, который требуется загрузить.</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        /// <param name="dataObjectCache"></param>
        protected virtual void SecondLoadObject(
    View dataObjectView,
    DataObject dataObject, bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            PrvSecondLoadObject(dataObjectView, dataObject, checkExistingObject, dataObjectCache, null);
        }

        /// <summary>
        /// Метод для дочитки объекта данных. Загруженные ранее свойства не затираются, изменённые свойства не затираются. Подменяются поштучно свойства копии данных.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dataObject">бъект данных, который требуется загрузить.</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        /// <param name="dataObjectCache"></param>
        /// <param name="dataObjectFromDB"></param>
        protected virtual void PrvSecondLoadObject(
            View dataObjectView,
            DataObject dataObject, bool checkExistingObject, DataObjectCache dataObjectCache, DataObject dataObjectFromDB)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView), "Не указано представление для дозагрузки объекта. Обратитесь к разработчику.");
            }

            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject), "Не указан объект для дозагрузки. Обратитесь к разработчику.");
            }

            Type doType = dataObject.GetType();

            List<string> loadedProperties = new List<string>(dataObject.GetLoadedProperties());

            // TODO: надо оценить насколько полезно это здесь вызывать, может можно обойтись и без этого
            List<string> alteredPropertyNames = new List<string>(dataObject.GetAlteredPropertyNames(true));

            DataObject dobject = null;

            if (dataObject.GetStatus(false) != ObjectStatus.Created)
            {
                if (dataObjectFromDB == null)
                {
                    dobject = (DataObject)Activator.CreateInstance(doType);

                    dataObject.CopySysProps(dobject);

                    LoadObject(dataObjectView, dobject, false, checkExistingObject, dataObjectCache);
                }
                else
                {
                    dobject = dataObjectFromDB;
                }

                bool dataCopyWasEmpty = false;

                // если копия данных не была инициализирована ранее, то сделаем это сейчас и относительно базы данных
                if (dataObject.GetDataCopy() == null)
                {
                    dataObject.SetDataCopy(dobject.GetDataCopy());
                    dataCopyWasEmpty = true;
                }

                // Сольём изменения в оригинальный объект
                // обработаем собственные свойства
                foreach (PropertyInView propertyInView in dataObjectView.Properties)
                {
                    // собственные свойства
                    string name = propertyInView.Name;
                    if (name.IndexOf(".") == -1)
                    {
                        Type propType = Information.GetPropertyType(doType, name);
                        if (!loadedProperties.Contains(name))
                        {
                            if (!propType.IsSubclassOf(typeof(DataObject)))
                            {
                                if (dataCopyWasEmpty || !alteredPropertyNames.Contains(name))
                                {
                                    // обработаем случай с Altered-свойствами, которые не входят в loaded - меняем значение только если не Altered
                                    Information.SetPropValueByName(dataObject, name, Information.GetPropValueByName(dobject, name));
                                }

                                Information.SetPropValueByName(dataObject.GetDataCopy(), name, Information.GetPropValueByName(dobject.GetDataCopy(), name));
                                dataObject.AddLoadedProperties(name);
                            }
                        }
                    }
                }
            }

            // соберём мастеровые представления
            Dictionary<string, View> masterViews = new Dictionary<string, View>();
            foreach (PropertyInView propertyInView in dataObjectView.Properties)
            {
                string name = propertyInView.Name;
                int indexOfDot = name.IndexOf('.');
                if (indexOfDot > -1)
                {
                    string masterName = name.Substring(0, indexOfDot);
                    string propForMaster = name.Substring(indexOfDot + 1);
                    Type masterType = Information.GetPropertyType(doType, masterName);
                    View masterView;
                    if (masterViews.ContainsKey(masterName))
                    {
                        masterView = masterViews[masterName];
                    }
                    else
                    {
                        masterView = new View();
                        masterView.DefineClassType = masterType;
                        masterViews.Add(masterName, masterView);
                    }

                    masterView.AddProperty(propForMaster);
                }
                else // может это сам мастер, а не его свойства? тогда тоже надо добавить к обработке
                {
                    Type masterType = Information.GetPropertyType(doType, name);
                    if (masterType.IsSubclassOf(typeof(DataObject)) && !masterViews.ContainsKey(name))
                    {
                        View masterView = new View();
                        masterView.DefineClassType = masterType;
                        masterViews.Add(name, masterView);
                    }
                }
            }

            // TODO: кеш объектов данных - мастеровые объекты должны быть одинаковые

            // TODO: реализовать дочитку всех встречных мастеров. Пусть будет куча запросов - это остаётся на совести прикладных программистов.
            // TODO: подумать насчёт иерархии
            foreach (KeyValuePair<string, View> masterView in masterViews)
            {
                DataObject masterObject = (DataObject)Information.GetPropValueByName(dataObject, masterView.Key);

                DataObject masterObjectFromDB = null;

                if (dobject != null)
                {
                    masterObjectFromDB = (DataObject)Information.GetPropValueByName(dobject, masterView.Key);

                    // если объект был заменён на другой
                    if (masterObject != null && masterObject.__PrimaryKey != masterObjectFromDB.__PrimaryKey)
                    {
                        masterObjectFromDB = null;
                    }
                }

                if (masterObject != null)
                {
                    PrvSecondLoadObject(masterView.Value, masterObject, checkExistingObject, dataObjectCache, masterObjectFromDB);
                }
                else if (masterObjectFromDB != null)
                {
                    // присвоим значение из БД, поскольку в оригинальном объекте этот мастер не был заполнен
                    Information.SetPropValueByName(dataObject, masterView.Key, masterObjectFromDB);
                    Information.SetPropValueByName(dataObject.GetDataCopy(), masterView.Key, masterObjectFromDB.GetDataCopy());
                }

                if (!loadedProperties.Contains(masterView.Key) && dataObject.GetLoadingState() != LoadingState.NotLoaded)
                {
                    dataObject.AddLoadedProperties(new[] { masterView.Key });
                }
            }

            // TODO:Обработаем детейлы
            foreach (DetailInView detailInView in dataObjectView.Details)
            {
                string detName = detailInView.Name;
                if (loadedProperties.Contains(detName) || alteredPropertyNames.Contains(detName))
                {
                    DetailArray detailsFromDataObject = (DetailArray)Information.GetPropValueByName(dataObject, detName);
                    DetailArray detailsFromDB = (DetailArray)Information.GetPropValueByName(dobject, detName);
                    foreach (DataObject detObjFromDB in detailsFromDB)
                    {
                        DataObject detObj = detailsFromDataObject.GetByKey(detObjFromDB.__PrimaryKey);
                        if (detObj != null)
                        {
                            PrvSecondLoadObject(detailInView.View, detObj, checkExistingObject, dataObjectCache, detObjFromDB);
                        }
                        else
                        {
                            detailsFromDataObject.AddObject(detObjFromDB);
                        }
                    }
                }
                else
                {
                    DetailArray detailFromDB = (DetailArray)Information.GetPropValueByName(dobject, detName);
                    if (detailFromDB != null)
                    {
                        Information.SetPropValueByName(dataObject, detName, detailFromDB);
                    }

                    if (dataObject.GetLoadingState() != LoadingState.NotLoaded)
                    {
                        dataObject.AddLoadedProperties(new[] { detName });
                    }
                }
            }
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">имя представления объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache)
        {
            LoadObject(STORMDO.Information.GetView(dataObjectViewName, dobject.GetType()), dobject, true, true, dataObjectCache);
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache)
        {
            LoadObject(new STORMDO.View(dobject.GetType(), STORMDO.View.ReadType.OnlyThatObject), dobject, true, true, dataObjectCache);
        }

        /// <summary>
        /// Событие перед генерацией запроса
        /// </summary>
        public event OnGenerateSQLSelectEventHandler OnGenerateSQLSelect;

        /// <summary>
        /// После генерации, но до вычитки
        /// </summary>
        public event AfterGenerateSQLSelectQueryEventHandler AfterGenerateSQLSelectQuery;

        /// <summary>
        /// После генерации, но до вычитки, статический эвент (выполняется после обработки в AfterGenerateSQLSelectQuery)
        /// </summary>
        public static event AfterGenerateSQLSelectQueryEventHandler AfterGenerateSQLSelectQueryStatic;

        /// <summary>
        /// Перед выполнением обновления объектов в базе. После отработки бизнес-серверов.
        /// </summary>
        public event BeforeUpdateObjectsEventHandler BeforeUpdateObjects;

        /// <summary>
        /// После выполнения обновления объектов в базе.
        /// </summary>
        public event AfterUpdateObjectsEventHandler AfterUpdateObjects;

        public string[] GetPropertiesInExpression(string expression, string namespacewithpoint)
        {
            return Information.GetPropertiesInExpression(expression, namespacewithpoint);
        }

        private void AddPropsByLimits(View curView, View OldView, FunctionalLanguage.Function func)
        {
            Type thisType = this.GetType();
            if (func == null)
            {
                return;
            }

            if (func.FunctionDef.FreeQuery)
            {// добавляем все поля
                curView.Properties = OldView.Properties;
                return;
            }
            else
            {
                string[] vars = ((FunctionalLanguage.SQLWhere.SQLWhereLanguageDef)func.FunctionDef.Language).GetExistingVariableNames(func);
                if (vars != null && vars.Length > 0)
                {
                    foreach (string var in vars)
                    {
                        try
                        {
                            OldView.GetProperty(var);
                            curView.AddProperty(var);

                            string sPrefix = null;
                            string new_var = var;
                            Type tp = OldView.DefineClassType;

                            if (var.IndexOf(".") != -1)
                            {
                                int lastDotIndex = var.LastIndexOf(".");
                                sPrefix = var.Substring(0, lastDotIndex);
                                new_var = var.Substring(lastDotIndex + 1);

                                tp = Information.GetPropertyType(tp, sPrefix);
                            }

                            ICSSoft.STORMNET.Collections.TypeBaseCollection clct = Information.GetExpressionForProperty(tp, new_var);
                            if (clct != null)
                            {
                                for (int i = 0; i < clct.Count; i++)
                                {
                                    Type key = clct.Key(i);
                                    if (thisType == key || thisType.IsSubclassOf(key))
                                    {
                                        string[] exprVars = GetPropertiesInExpression((string)clct[i], string.Empty);
                                        foreach (string expVar in exprVars)
                                        {
                                            try
                                            {
                                                if (sPrefix != null)
                                                {
                                                    OldView.GetProperty(sPrefix + "." + expVar);
                                                    curView.AddProperty(sPrefix + "." + expVar);
                                                }
                                                else
                                                {
                                                    OldView.GetProperty(expVar);
                                                    curView.AddProperty(expVar);
                                                }
                                            }
                                            catch
                                            { }
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
            }

            foreach (object par in func.Parameters)
            {
                if (par is VariableDef varDef)
                {
                    string n = varDef.StringedView;
                    if (n != null && string.Equals(n, SQLWhereLanguageDef.StormMainObjectKey, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        OldView.GetProperty(n);
                        curView.AddProperty(n);

                        string sPrefix = null;
                        string new_var = n;
                        Type tp = OldView.DefineClassType;

                        if (n.IndexOf(".") != -1)
                        {
                            int lastDotIndex = n.LastIndexOf(".");
                            sPrefix = n.Substring(0, lastDotIndex);
                            new_var = n.Substring(lastDotIndex + 1);

                            tp = Information.GetPropertyType(tp, sPrefix);
                        }

                        ICSSoft.STORMNET.Collections.TypeBaseCollection clct = Information.GetExpressionForProperty(tp, new_var);
                        if (clct != null)
                        {
                            for (int i = 0; i < clct.Count; i++)
                            {
                                Type key = clct.Key(i);
                                if (thisType == key || thisType.IsSubclassOf(key))
                                {
                                    string[] exprVars = GetPropertiesInExpression((string)clct[i], string.Empty);
                                    foreach (string expVar in exprVars)
                                    {
                                        try
                                        {
                                            if (sPrefix != null)
                                            {
                                                OldView.GetProperty(sPrefix + "." + expVar);
                                                curView.AddProperty(sPrefix + "." + expVar);
                                            }
                                            else
                                            {
                                                OldView.GetProperty(expVar);
                                                curView.AddProperty(expVar);
                                            }
                                        }
                                        catch
                                        { }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                }
                else if (par is FunctionalLanguage.Function)
                {
                    FunctionalLanguage.Function fnc = par as FunctionalLanguage.Function;
                    AddPropsByLimits(curView, OldView, fnc);
                }
            }
        }

        private bool mustNewgenerate = false;

        /// <summary>
        /// получить запрос на вычитку данных.
        /// </summary>
        /// <param name="customizationStruct">настройка выборки.</param>
        /// <param name="StorageStruct">возвращается соответствующая структура выборки.</param>
        /// <returns>запрос.</returns>
        public virtual string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool ForReadValues, out STORMDO.Business.StorageStructForView[] StorageStruct, bool Optimized)
        {
            View prevV = customizationStruct.View;
            try
            {
                if (Optimized)
                {
                    View v = new View();
                    v.Name = "Optimized " + customizationStruct.View.Name;
                    v.DefineClassType = customizationStruct.View.DefineClassType;
                    AddPropsByLimits(v, customizationStruct.View, customizationStruct.LimitFunction);
                    customizationStruct.View = v;
                }

                StorageStruct = null;
                if (OnGenerateSQLSelect != null)
                {
                    GenerateSQLSelectQueryEventArgs e = new GenerateSQLSelectQueryEventArgs(customizationStruct, string.Empty, null);
                    OnGenerateSQLSelect(this, e);
                    if (e.GeneratedQuery != string.Empty && e.GeneratedQuery != null && e.CustomizationStruct != null)
                    {
                        StorageStruct = e.StorageStruct;
                        return e.GeneratedQuery;
                    }
                    else if (e.StorageStruct != null && e.StorageStruct.Length == customizationStruct.LoadingTypes.Length)
                    {
                        StorageStruct = e.StorageStruct;
                    }
                }

                // строим запрос
                System.Type[] dataObjectType = customizationStruct.LoadingTypes;
                if (StorageStruct == null)
                {
                    StorageStruct = new STORMDO.Business.StorageStructForView[dataObjectType.Length];
                }

                // Танцы с бубнами по поводу вычислимых свойств
                STORMDO.View dataObjectView = customizationStruct.View;
                if (dataObjectView.Name == string.Empty)
                {
                    Information.AppendPropertiesFromNotStored(dataObjectView, this.GetType());
                }

                STORMFunction LimitFunction = customizationStruct.LimitFunction;
                int[] CountKeys = new int[dataObjectType.Length];
                string[] Queries = new string[dataObjectType.Length];
                int MaxCountKeys = 0;
                for (int i = 0; i < dataObjectType.Length; i++)
                {
                    StorageStruct[i] = STORMDO.Information.GetStorageStructForView(dataObjectView, dataObjectType[i], StorageType, new ICSSoft.STORMNET.Information.GetPropertiesInExpressionDelegate(GetPropertiesInExpression), GetType());
                    if (StorageStruct[i] != null)
                    {
                        CountKeys[i] = Utils.CountMasterKeysInSelect(StorageStruct[i]);
                        if (CountKeys[i] > MaxCountKeys)
                        {
                            MaxCountKeys = CountKeys[i];
                        }
                    }
                }

                string[] asnameprop = new string[StorageStruct[0].props.Length];
                string[] altnameprop = new string[StorageStruct[0].props.Length];

                mustNewgenerate = (dataObjectType.Length == 1) && mustNewgenerate;

                if (mustNewgenerate)
                {
                    StorageStructForView.PropStorage[] propStorages = StorageStruct[0].props;
                    for (int i = 0; i < propStorages.Length; i++)
                    {
                        StorageStructForView.PropStorage propStorage = propStorages[i];
                        asnameprop[i] = propStorage.Name;
                        if (propStorage.storage[0][0] != null)
                        {
                            // не вычислимое св-во
                            propStorage.Name = propStorage.source.Name + "0." + propStorage.storage[0][0];
                        }
                    }
                }

                System.Text.StringBuilder QueryBilder = new System.Text.StringBuilder();
                bool notemptyQuery = false;
                string nl = Environment.NewLine;
                string UA = nl + " UNION ALL" + nl;
                bool MustDopSelect = dataObjectType.Length > 1;
                for (int i = 0; i < dataObjectType.Length; i++)
                {
                    if (StorageStruct[i] != null)
                    {
                        if (mustNewgenerate)
                        {
                            Queries[i] = GenerateSQLSelectByStorageStruct(StorageStruct[i], true, true, GetConvertToTypeExpression(typeof(decimal), i.ToString()) + " as " + PutIdentifierIntoBrackets("STORMNETDATAOBJECTTYPE"), MaxCountKeys - CountKeys[i], StorageType == StorageTypeEnum.HierarchicalStorage, mustNewgenerate, MustDopSelect);
                        }
                        else
                        {
                            Queries[i] = GenerateSQLSelectByStorageStruct(StorageStruct[i], true, true, GetConvertToTypeExpression(typeof(decimal), i.ToString()) + " as " + PutIdentifierIntoBrackets("STORMNETDATAOBJECTTYPE"), MaxCountKeys - CountKeys[i], StorageType == StorageTypeEnum.HierarchicalStorage);
                        }

                        if (notemptyQuery)
                        {
                            QueryBilder.Append(UA);
                        }

                        notemptyQuery = true;
                        QueryBilder.Append(Queries[i]);
                    }
                }

                string Query = QueryBilder.ToString(); // string.Join(Environment.NewLine+" UNION ALL"+Environment.NewLine,Queries);

                if (Query == string.Empty)
                {
                    return string.Empty;
                }

                // строим Wrap-Select
                string resQuery = "SELECT ";
                if (customizationStruct.Distinct /*&& ForReadValues*/)
                {
                    resQuery += "DISTINCT ";
                }

                if (customizationStruct.ReturnTop > 0)
                {
                    resQuery += "TOP " + customizationStruct.ReturnTop + " ";
                }

                #region задаем порядок колонок в запросе - результат в props : StringCollection

                System.Collections.Specialized.StringCollection props = new System.Collections.Specialized.StringCollection();
                ICSSoft.STORMNET.Collections.NameObjectCollection advprops = new ICSSoft.STORMNET.Collections.NameObjectCollection();
                for (int i = 0; i < dataObjectView.Properties.Length; i++)
                {
                    props.Add(PutIdentifierIntoBrackets(dataObjectView.Properties[i].Name));
                }

                if (customizationStruct.AdvansedColumns != null)
                {
                    for (int i = 0; i < customizationStruct.AdvansedColumns.Length; i++)
                    {
                        AdvansedColumn ac = customizationStruct.AdvansedColumns[i];
                        advprops.Add(PutIdentifierIntoBrackets(ac.Name), ac.Expression + " as " + PutIdentifierIntoBrackets(ac.Name));
                    }
                }

                if (customizationStruct.ColumnsOrder != null)
                {
                    int k = 0;
                    for (int i = 0; i < customizationStruct.ColumnsOrder.Length; i++)
                    {
                        string prop = PutIdentifierIntoBrackets(customizationStruct.ColumnsOrder[i]);
                        int index = props.IndexOf(prop);
                        if (index >= 0)
                        {
                            if (index != k)
                            {
                                props.RemoveAt(index);
                                props.Insert(k++, prop);
                            }
                            else
                            {
                                k++;
                            }
                        }
                        else
                        {
                            if (advprops.ContainsKey(prop))
                            {
                                props.Insert(k++, (string)advprops[prop]);
                                advprops.Remove(prop);
                            }
                        }
                    }
                }

                for (int i = 0; i < advprops.Count; i++)
                {
                    props.Add((string)advprops[i]);
                }

                if (!ForReadValues)
                {
                    props.Add(PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey));
                    for (int i = 0; i < MaxCountKeys; i++)
                    {
                        if (StorageType == StorageTypeEnum.HierarchicalStorage)
                        {
                            props.Add(PutIdentifierIntoBrackets("STORMJoinedMasterType" + i));
                        }

                        props.Add(PutIdentifierIntoBrackets("STORMJoinedMasterKey" + i));
                    }

                    props.Add(PutIdentifierIntoBrackets("STORMNETDATAOBJECTTYPE"));
                }
                #endregion

                string colsPart = null;
                if (mustNewgenerate)
                {
                    var match = Regex.Match(
                        Query,
                        @"([.]*(\""\w*\b\""))* as " + PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey));
                    colsPart = Query.Substring(Query.IndexOf(match.Value));

                    Query = "SELECT ";
                    if (customizationStruct.Distinct /*&& ForReadValues*/)
                    {
                        Query += "DISTINCT ";
                    }

                    if (customizationStruct.ReturnTop > 0)
                    {
                        Query += "TOP " + customizationStruct.ReturnTop + " ";
                    }
                }

                var propStatements = new List<string>();
                for (int i = 0; i < props.Count; i++)
                {
                    if (mustNewgenerate)
                    {
                        string selectF = string.Empty;
                        StorageStructForView.PropStorage[] propStorages = StorageStruct[0].props;
                        for (int j = 0; j < propStorages.Length; j++)
                        {
                            string prop = props[i];
                            if (PutIdentifierIntoBrackets(asnameprop[j]) == prop)
                            {
                                StorageStructForView.PropStorage propStorage = propStorages[j];
                                if (propStorage.MastersTypesCount > 0)
                                {
                                    string[] names = new string[propStorage.storage.Length];
                                    for (int jj = 0; jj < propStorage.storage.Length; jj++)
                                    {
                                        string[] namesM = new string[propStorage.MastersTypes[jj].Length];
                                        for (int k = 0; k < propStorage.MastersTypes[jj].Length; k++)
                                        {
                                            string curName = PutIdentifierIntoBrackets(propStorage.source.Name + jj) + "." +
                                                PutIdentifierIntoBrackets(propStorage.storage[jj][k]);
                                            namesM[k] = curName;
                                        }

                                        names[jj] = this.GetIfNullExpression(namesM);
                                    }

                                    altnameprop[j] = this.GetIfNullExpression(names);
                                    selectF = altnameprop[j] + " as " + prop;
                                }
                                else if (propStorage.storage[0][0] != null)
                                {
                                    // не вычислимое св-во
                                    altnameprop[j] = PutIdentifierIntoBrackets(propStorage.source.Name + "0") + "." +
                                        PutIdentifierIntoBrackets(propStorage.storage[0][0]);
                                    selectF = altnameprop[j] + " as " + prop;
                                    break;
                                }
                                else
                                {
                                    bool PointExist = false;
                                    altnameprop[j] = "NULL";
                                    if (propStorage.Expression != null)
                                    {
                                        altnameprop[j] = TranslateExpression(propStorage.Expression, string.Empty,
                                            PutIdentifierIntoBrackets(propStorage.source.Name + "0") + ".", out PointExist);
                                    }

                                    selectF = altnameprop[j] + " as " + prop;
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(selectF))
                        {
                            propStatements.Add(selectF);
                        }
                    }
                    else
                    {
                        propStatements.Add(props[i]);
                    }
                }

                if (mustNewgenerate)
                {
                    Query += string.Join("," + nl, propStatements);
                }
                else
                {
                    resQuery += string.Join("," + nl, propStatements);
                }

                // colsPart = resQuery;

                // добавим From-часть
                resQuery += nl + "FROM (" + nl + Query + nl + ") " + PutIdentifierIntoBrackets("STORMGENERATEDQUERY");

                if (customizationStruct.AdvansedColumns != null)
                {
                    for (int j = 0; j < customizationStruct.AdvansedColumns.Length; j++)
                    {
                        AdvansedColumn ac = customizationStruct.AdvansedColumns[j];
                        if (!string.IsNullOrEmpty(ac.StorageSourceModification))
                        {
                            resQuery += nl + "\t" + ac.StorageSourceModification;
                        }
                    }
                }

                if (mustNewgenerate)
                {
                    for (int i = 0; i < StorageStruct[0].props.Length; i++)
                    {
                        StorageStructForView.PropStorage propStorage = StorageStruct[0].props[i];
                        if (propStorage.storage[0][0] != null)
                        {
                            // не вычислимое св-во
                            Query = Regex.Replace(
                                Query,
                                propStorage.source.Name + "0." + propStorage.storage[0][0],
                                asnameprop[i]);
                        }
                    }

                    Query = Regex.Replace(
                        Query,
                        @"\""?STORMMAINObjectKey\""?",
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.Name + "0") + "." +
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.storage[0].PrimaryKeyStorageName),
                        RegexOptions.IgnoreCase);
                    Query += ((Query == "SELECT ") ? string.Empty : ",") + colsPart;
                }

                // добавим Where-часть
                if (LimitFunction != null)
                {
                    string sw = LimitFunction2SQLWhere(LimitFunction, StorageStruct, asnameprop, mustNewgenerate);
                    resQuery += nl + "WHERE " + sw;
                    if (mustNewgenerate)
                    {
                        Query += nl + "WHERE " + sw;
                    }
                }

                if (mustNewgenerate)
                {
                    resQuery = Query;
                }

                // добавим OrderBy-часть
                string orderByExpr = string.Empty;
                if (!Optimized)
                {
                    string[] AscDesc = { string.Empty, " asc", " desc" };
                    ColumnsSortDef[] sorts = customizationStruct.GetOwnerOnlySortDef();
                    if (sorts != null && sorts.Length > 0)
                    {
                        if (mustNewgenerate)
                        {
                            for (int i = 0; i < StorageStruct[0].props.Length; i++)
                            {
                                for (int j = 0; j < sorts.Length; j++)
                                {
                                    if (sorts[j].Name == asnameprop[i])
                                    {
                                        sorts[j].Name = altnameprop[i];
                                        break;
                                    }

                                    if (sorts[j].Name == "__PrimaryKey")
                                    {
                                        sorts[j].Name = PutIdentifierIntoBrackets(StorageStruct[0].sources.Name + "0") + "." +
                                           PutIdentifierIntoBrackets(StorageStruct[0].sources.storage[0].PrimaryKeyStorageName);
                                    }
                                }
                            }
                        }

                        var orderByParts = new List<string>();
                        for (int i = 0; i < sorts.Length; i++)
                        {
                            ColumnsSortDef sortDef = sorts[i];
                            if (mustNewgenerate)
                            {
                                string addStr = sortDef.Name + AscDesc[(int)sortDef.Sort];
                                orderByParts.Add(addStr);
                            }
                            else if (props.Contains(PutIdentifierIntoBrackets(sortDef.Name)))
                            {
                                sortDef.Name = PutIdentifierIntoBrackets(sortDef.Name);
                                string addStr = sortDef.Name + AscDesc[(int)sortDef.Sort];
                                orderByParts.Add(addStr);
                            }
                        }

                        if (orderByParts.Any())
                        {
                            orderByExpr = nl + "ORDER BY " + string.Join(", ", orderByParts);
                            resQuery += orderByExpr;
                        }
                    }

                    GenerateSQLRowNumber(customizationStruct, ref resQuery, orderByExpr);
                }

                if (mustNewgenerate)
                {
                    for (int i = 0; i < StorageStruct[0].props.Length; i++)
                    {
                        StorageStruct[0].props[i].Name = asnameprop[i];
                    }
                }

                if (AfterGenerateSQLSelectQuery != null)
                {
                    GenerateSQLSelectQueryEventArgs e = new GenerateSQLSelectQueryEventArgs(customizationStruct, resQuery, StorageStruct);
                    AfterGenerateSQLSelectQuery(this, e);
                    resQuery = e.GeneratedQuery;
                }

                if (AfterGenerateSQLSelectQueryStatic != null)
                {
                    GenerateSQLSelectQueryEventArgs e = new GenerateSQLSelectQueryEventArgs(customizationStruct, resQuery, StorageStruct);
                    AfterGenerateSQLSelectQueryStatic(this, e);
                    resQuery = e.GeneratedQuery;
                }

                return resQuery;
            }
            finally
            {
                customizationStruct.View = prevV;
            }
        }

        public virtual void GenerateSQLRowNumber(LoadingCustomizationStruct customizationStruct, ref string resQuery, string orderByExpr)
        {
            string nl = Environment.NewLine;

            // поддержка Row_number() Братчиков 07.03.2009
            // if (resQuery.ToLower().IndexOf("$row_number$") > -1)
            if (customizationStruct.RowNumber != null)
            {
                int fromInd = resQuery.IndexOf("FROM (");
                string селектСамогоВерхнегоУр = resQuery.Substring(0, fromInd);

                if (!string.IsNullOrEmpty(orderByExpr))
                {
                    resQuery = resQuery.Replace(orderByExpr, string.Empty);
                    resQuery = resQuery.Insert(fromInd, $",{nl}row_number() over ({orderByExpr}) as \"RowNumber\"{nl}");
                }
                else
                {
                    resQuery = resQuery.Insert(fromInd, $",{nl}row_number() over (ORDER BY \"{SQLWhereLanguageDef.StormMainObjectKey}\") as \"RowNumber\"{nl}");
                }

                resQuery = селектСамогоВерхнегоУр + nl + "FROM (" + nl + resQuery + ") rn" + nl + "where \"RowNumber\" between " + customizationStruct.RowNumber.StartRow + " and " + customizationStruct.RowNumber.EndRow + nl +
                    orderByExpr;
            }

            // поддержка Row_number() Братчиков 07.03.2009
        }

        /// <summary>
        /// получить запрос на вычитку данных.
        /// </summary>
        /// <param name="customizationStruct">настройка выборки.</param>
        /// <returns>запрос.</returns>
        public virtual string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool Optimized)
        {
            STORMDO.Business.StorageStructForView[] storageStructs;
            return GenerateSQLSelect(customizationStruct, false, out storageStructs, Optimized);
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns></returns>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache)
        {
            object state = null;
            ICSSoft.STORMNET.DataObject[] res = LoadObjects(customizationStruct, ref state, dataObjectCache);
            return res;
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="dataObjects">исходные объекты.</param>
        /// <param name="dataObjectView">представлене.</param>
        /// <param name="clearDataObject">очищать ли существующие.</param>
        public virtual void LoadObjects(ICSSoft.STORMNET.DataObject[] dataObjects,
            ICSSoft.STORMNET.View dataObjectView, bool clearDataObject, DataObjectCache dataObjectCache)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException(nameof(dataObjectView));
            }

            if (dataObjectCache == null)
            {
                dataObjectCache = new DataObjectCache();
            }

            if (dataObjects == null || dataObjects.Length == 0)
            {
                return;
            }

            dataObjectCache.StartCaching(false);
            try
            {
                RunChangeCustomizationString(dataObjects);

                SortedList allObjectKeys = new SortedList();
                SortedList readingKeys = new SortedList();
                LoadingCustomizationStruct customizationStruct = GetCustomizationStruct(dataObjects, dataObjectView, out allObjectKeys, out readingKeys);
                ApplyReadPermissions(customizationStruct, SecurityManager);

                StorageStructForView[] storageStruct;
                string selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // получаем данные
                object State = null;
                object[][] result = string.IsNullOrEmpty(selectString) ? new object[0][] : ReadFirst(
                    selectString,
                    ref State, 0);

                ConvertReadResult(result, dataObjects, customizationStruct, storageStruct, allObjectKeys, readingKeys, clearDataObject, dataObjectCache);
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View dataObjectView)
        {
            LoadingCustomizationStruct lc = new LoadingCustomizationStruct(GetInstanceId());
            lc.View = dataObjectView;
            lc.LoadingTypes = new[] { dataObjectView.DefineClassType };
            return LoadObjects(lc, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных по массиву представлений.
        /// </summary>
        /// <param name="dataObjectViews">массив представлений.</param>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View[] dataObjectViews)
        {
            System.Collections.ArrayList arr = new System.Collections.ArrayList();
            ICSSoft.STORMNET.DataObject[] res = null;
            for (int i = 0; i < dataObjectViews.Length; i++)
            {
                res = LoadObjects(dataObjectViews[i]);
                for (int j = 0; j < res.Length; j++)
                {
                    arr.Add(res[j]);
                }
            }

            res = new ICSSoft.STORMNET.DataObject[arr.Count];
            arr.CopyTo(res);
            return res;
        }

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">массив структур.</param>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct[] customizationStructs)
        {
            System.Collections.ArrayList arr = new System.Collections.ArrayList();
            ICSSoft.STORMNET.DataObject[] res = null;
            for (int i = 0; i < customizationStructs.Length; i++)
            {
                res = LoadObjects(customizationStructs[i], new DataObjectCache());
                for (int j = 0; j < res.Length; j++)
                {
                    arr.Add(res[j]);
                }
            }

            res = new ICSSoft.STORMNET.DataObject[arr.Count];
            arr.CopyTo(res);
            return res;
        }

        /// <summary>
        /// Загрузка объектов с использованием указанной коннекции и транзакции.
        /// </summary>
        /// <param name="customizationStruct">Структура, определяющая, что и как грузить.</param>
        /// <param name="state">Состояние вычитки (для последующей дочитки).</param>
        /// <param name="dataObjectCache">Кэш объектов для вычитки.</param>
        /// <param name="connection">Коннекция, через которую будут выполнена зачитска.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет выполнена вычитка.</param>
        /// <returns>Загруженные данные.</returns>
        public virtual DataObject[] LoadObjectsByExtConn(
            LoadingCustomizationStruct customizationStruct,
            ref object state, // TODO: разобраться, что это за параметр.
            DataObjectCache dataObjectCache,
            IDbConnection connection,
            IDbTransaction transaction = null)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                Type[] dataObjectType = customizationStruct.LoadingTypes;
                StorageStructForView[] storageStruct;

                string selectString = string.Empty;
                selectString = GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                // Получаем данные.
                object[][] resValue = ReadFirstByExtConn(
                                            selectString, ref state, customizationStruct.LoadingBufferSize, connection, transaction);
                state = new object[] { state, dataObjectType, storageStruct, customizationStruct, customizationString };
                DataObject[] res = null;
                if (resValue == null)
                {
                    res = new DataObject[0];
                }
                else
                {
                    res = Utils.ProcessingRowsetData(
                            resValue, dataObjectType, storageStruct, customizationStruct, this, Types, dataObjectCache, SecurityManager, connection, transaction);
                }

                return res;
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="state">Состояние вычитки (для последующей дочитки).</param>
        /// <returns></returns>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct,
            ref object state, DataObjectCache dataObjectCache)
        {
            RunChangeCustomizationString(customizationStruct.LoadingTypes);
            return LoadObjectsByExtConn(customizationStruct, ref state, dataObjectCache, GetConnection());
        }

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="changeViewForTypeDelegate">делегат.</param>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                this.fchangeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(dataObjectView);
        }

        /// <summary>
        /// Загрузка объектов данных по массиву представлений.
        /// </summary>
        /// <param name="dataObjectViews">массив представлений.</param>
        /// <param name="changeViewForTypeDelegate">делегат.</param>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View[] dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                this.fchangeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(dataObjectViews);
        }

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">массив структур.</param>
        /// <param name="changeViewForTypeDelegate">делегат.</param>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct[] customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                this.fchangeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(customizationStructs);
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="state">Состояние вычитки( для последующей дочитки).</param>
        /// <returns></returns>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(ref object state, DataObjectCache dataObjectCache)
        {
            if (state == null)
            {
                return new DataObject[0];
            }

            dataObjectCache.StartCaching(false);
            try
            {
                // получаем данные
                object[] stateArr = (object[])state;
                ICSSoft.STORMNET.DataObject[] res = null;
                if (stateArr[0] == null)
                {
                    res = new DataObject[0];
                }
                else
                {
                    object[][] resValue = ReadNext(ref stateArr[0], ((LoadingCustomizationStruct)stateArr[3]).LoadingBufferSize);
                    if (resValue == null)
                    {
                        res = new DataObject[0];
                    }
                    else
                    {
                        res = Utils.ProcessingRowsetData(resValue, (System.Type[])stateArr[1], (STORMNET.Business.StorageStructForView[])stateArr[2], (LoadingCustomizationStruct)stateArr[3], this, Types, dataObjectCache, SecurityManager);
                    }
                }

                dataObjectCache.StopCaching();
                return res;
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        public virtual object[][] ReadFirstByExtConn(string Query, ref object State, int LoadingBufferSize, IDbConnection Connection, IDbTransaction Transaction)
        {
            object taskid = BusinessTaskMonitor.BeginTask("Reading data" + Environment.NewLine + Query);
            try
            {
                using (IDbCommand myCommand = Connection.CreateCommand())
                {
                    myCommand.CommandText = Query;
                    myCommand.Transaction = Transaction;
                    CustomizeCommand(myCommand);

                    // Connection.Open();
                    IDataReader myReader = myCommand.ExecuteReader();
                    State = new object[] { Connection, myReader };
                    return ReadNextByExtConn(ref State, LoadingBufferSize);
                }
            }
            catch (Exception e)
            {
                throw new ExecutingQueryException(Query, string.Empty, e);
            }
            finally
            {
                BusinessTaskMonitor.EndTask(taskid);
            }
        }

        /// <summary>
        /// Вычитка первой партии данных.
        /// </summary>
        /// <param name="query">Запрос для вычитки.</param>
        /// <param name="state"></param>
        /// <param name="loadingBufferSize">Количество строк, которые нужно загрузить в рамках текущей вычитки (используется для повторной дочитки).</param>
        /// <returns></returns>
        public virtual object[][] ReadFirst(string query, ref object state, int loadingBufferSize)
        {
            object task = BusinessTaskMonitor.BeginTask("Reading data" + Environment.NewLine + query);

            IDbConnection connection = null;
            IDataReader reader = null;
            try
            {
                connection = GetConnection();
                connection.Open();

                IDbCommand command = connection.CreateCommand();
                command.CommandText = query;
                CustomizeCommand(command);

                reader = command.ExecuteReader();

                state = new object[] { connection, reader };
                return ReadNext(ref state, loadingBufferSize);
            }
            catch (Exception e)
            {
                reader?.Close();
                connection?.Close();

                throw new ExecutingQueryException(query, string.Empty, e);
            }
            finally
            {
                BusinessTaskMonitor.EndTask(task);
            }
        }

        public virtual object[][] ReadNextByExtConn(ref object State, int LoadingBufferSize)
        {
            if (State == null || !State.GetType().IsArray)
            {
                return null;
            }

            System.Data.IDataReader myReader = (System.Data.IDataReader)((object[])State)[1];
            if (myReader.Read())
            {
                System.Collections.ArrayList arl = new System.Collections.ArrayList();
                int i = 1;
                int FieldCount = myReader.FieldCount;

                // object[][] resar = new object[LoadingBufferSize][];

                while (i <= LoadingBufferSize || LoadingBufferSize == 0)
                {
                    if (i > 1)
                    {
                        if (!myReader.Read())
                        {
                            break;
                        }
                    }

                    object[] tmp = new object[FieldCount];
                    myReader.GetValues(tmp);
                    arl.Add(tmp);

                    // resar[i-1]= tmp;
                    i++;
                }

                object[][] result = null;

                // if (i<LoadingBufferSize)
                //              {
                //                  result = new object[i-1][];
                //                  for (int j=0;j<i-1;j++)
                //                      result[j]=resar[j];
                //
                //              }
                //              else
                //                  result = resar;
                result = (object[][])arl.ToArray(typeof(object[]));

                if (i < LoadingBufferSize || LoadingBufferSize == 0)
                {
                    myReader.Close();

                    // System.Data.IDbConnection myConnection = (System.Data.IDbConnection)((object[])State)[0];
                    // myConnection.Close();
                    State = null;
                }

                return result;
            }
            else
            {
                myReader.Close();

                // myConnection.Close();
                State = null;
                return null;
            }
        }

        /// <summary>
        /// Вычитка следующей порции данных.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="loadingBufferSize"></param>
        /// <returns></returns>
        public virtual object[][] ReadNext(ref object state, int loadingBufferSize)
        {
            if (state == null || !state.GetType().IsArray)
            {
                return null;
            }

            IDataReader reader = (IDataReader)((object[])state)[1];
            if (reader.Read())
            {
                var arl = new ArrayList();
                int i = 1;
                int fieldCount = reader.FieldCount;

                while (i <= loadingBufferSize || loadingBufferSize == 0)
                {
                    if (i > 1)
                    {
                        if (!reader.Read())
                        {
                            break;
                        }
                    }

                    object[] tmp = new object[fieldCount];
                    reader.GetValues(tmp);
                    arl.Add(tmp);
                    i++;
                }

                object[][] result = (object[][])arl.ToArray(typeof(object[]));

                if (i < loadingBufferSize || loadingBufferSize == 0)
                {
                    reader.Close();
                    IDbConnection connection = (IDbConnection)((object[])state)[0];
                    connection.Close();
                    state = null;
                }

                return result;
            }
            else
            {
                reader.Close();
                IDbConnection connection = (IDbConnection)((object[])state)[0];
                connection.Close();
                state = null;
                return null;
            }
        }

        /// <summary>
        /// Выполнить запрос.
        /// </summary>
        /// <param name="query">SQL запрос.</param>
        /// <returns>количество задетых строк.</returns>
        public virtual int ExecuteNonQuery(string query)
        {
            using (IDbConnection connection = GetConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = query;
                CustomizeCommand(command);
                return command.ExecuteNonQuery();
            }
        }

        private string GenString(string StringBlock, int count)
        {
            if (count == 0)
            {
                return string.Empty;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder(StringBlock.Length * count);
            for (int i = 0; i < count; i++)
            {
                sb.Append(StringBlock);
            }

            return sb.ToString();
        }

        public event OnCreateCommandEventHandler OnCreateCommand;

        /// <summary>
        /// получить LeftJoin выражение.
        /// </summary>
        /// <param name="subTable">имя таблицы.</param>
        /// <param name="subTableAlias">псевдоним таблицы.</param>
        /// <param name="parentAliasWithKey"></param>
        /// <param name="subTableKey"></param>
        /// <param name="subJoins"></param>
        /// <param name="baseOutline"></param>
        /// <returns></returns>
        public virtual void GetLeftJoinExpression(string subTable, string subTableAlias, string parentAliasWithKey, string subTableKey, string subJoins, string baseOutline,
            out string FromPart, out string WherePart)
        {
            string nl = Environment.NewLine + baseOutline;
            string subTableKeyField = PutIdentifierIntoBrackets(subTableAlias) + "." + PutIdentifierIntoBrackets(subTableKey);
            string joinCondition = !string.IsNullOrEmpty(parentAliasWithKey) && parentAliasWithKey.Contains(".")
                ? string.Concat(parentAliasWithKey, " = ", subTableKeyField)
                : string.Format("{0} = {1}", subTableKeyField, parentAliasWithKey);

            FromPart = string.Concat(nl, " LEFT JOIN ", subTable, " ", PutIdentifierIntoBrackets(subTableAlias),
                GetJoinTableModifierExpression(),
                subJoins,
                nl, " ON ", joinCondition);
            WherePart = string.Empty;
        }

        /// <summary>
        /// получить InnerJoin выражение.
        /// </summary>
        /// <param name="subTable"></param>
        /// <param name="subTableAlias"></param>
        /// <param name="parentAliasWithKey"></param>
        /// <param name="subTableKey"></param>
        /// <param name="subJoins"></param>
        /// <param name="baseOutline"></param>
        /// <param name="FromPart"></param>
        /// <param name="WherePart"></param>
        public virtual void GetInnerJoinExpression(string subTable, string subTableAlias, string parentAliasWithKey, string subTableKey, string subJoins, string baseOutline,
            out string FromPart, out string WherePart)
        {
            string nl = Environment.NewLine + baseOutline;
            string subTableKeyField = PutIdentifierIntoBrackets(subTableAlias) + "." + PutIdentifierIntoBrackets(subTableKey);
            string joinCondition = !string.IsNullOrEmpty(parentAliasWithKey) && parentAliasWithKey.Contains(".")
                ? string.Concat(parentAliasWithKey, " = ", subTableKeyField)
                : string.Format("{0} = {1}", subTableKeyField, parentAliasWithKey);

            FromPart = string.Concat(nl, " INNER JOIN ", subTable, " ", PutIdentifierIntoBrackets(subTableAlias),
                GetJoinTableModifierExpression(),
                subJoins,
                nl, " ON ", joinCondition);
            WherePart = string.Empty;
        }

        /// <summary>
        /// Получить выражения для обращения к таблице.
        /// </summary>
        /// <param name="tableName">Имя таблицы.</param>
        /// <param name="onJoin"><see langword="true" />, если имя таблицы требуется для соединения таблиц join.</param>
        /// <returns>Выражение для обращения к таблице.</returns>
        public virtual string GetTableStorageExpression(string tableName, bool onJoin)
        {
            return string.Concat(
                GetTableModifierPrefix(tableName, onJoin),
                PutIdentifierIntoBrackets(tableName),
                GetTableModifierSuffix(tableName, onJoin));
        }

        /// <summary>
        /// Получить префикс для обращения к таблице.
        /// </summary>
        /// <param name="tableName">Имя таблицы.</param>
        /// <param name="onJoin"><see langword="true" />, если имя таблицы требуется для соединения таблиц join.</param>
        /// <returns>Префикс-модификатор.</returns>
        public virtual string GetTableModifierPrefix(string tableName, bool onJoin)
        {
            return string.Empty;
        }

        /// <summary>
        /// Получить суффикс для обращения к таблице.
        /// </summary>
        /// <param name="tableName">Имя таблицы.</param>
        /// <param name="onJoin"><see langword="true" />, если имя таблицы требуется для соединения таблиц join.</param>
        /// <returns>Суффикс-модификатор.</returns>
        public virtual string GetTableModifierSuffix(string tableName, bool onJoin)
        {
            return string.Empty;
        }

        /// <summary>
        /// Вернуть модификатор для обращения к таблице (напр WITH (NOLOCK))
        /// Можно перегрузить этот метод в сервисе данных-наследнике
        /// для возврата соответствующего своего модификатора.
        /// Базовый <see cref="SQLDataService"/> возвращает пустую строку.
        /// </summary>
        /// <returns>"".</returns>
        public virtual string GetJoinTableModifierExpression()
        {
            return string.Empty;
        }

        /// <summary>
        /// Вернуть in выражение для where.
        /// </summary>
        /// <param name="identifiers">идентифткаторы.</param>
        /// <returns></returns>
        public virtual string GetINExpression(params string[] identifiers)
        {
            string result = identifiers[identifiers.Length - 1];
            for (int i = identifiers.Length - 2; i >= 0; i--)
            {
                result = string.Concat(" IN (", identifiers[i], ", ", result, ")");
            }

            return result;
        }

        /// <summary>
        /// Вернуть ifnull выражение.
        /// </summary>
        /// <param name="identifiers">идентифткаторы.</param>
        /// <returns></returns>
        public virtual string GetIfNullExpression(params string[] identifiers)
        {
            string result = identifiers[identifiers.Length - 1];
            for (int i = identifiers.Length - 2; i >= 0; i--)
            {
                result = string.Concat("IFNULL(", identifiers[i], ", ", result, ")");
            }

            return result;
        }

        /// <summary>
        /// Офромить идентификатор.
        /// </summary>
        /// <param name="identifier">идентификатор.</param>
        /// <returns>оформленный идентификатор(например в кавычках).</returns>
        public virtual string PutIdentifierIntoBrackets(string identifier)
        {
            return string.Concat("\"", identifier, "\"");
        }

        /// <summary>
        /// создать join соединения.
        /// </summary>
        /// <param name="source">источник с которого формируется соединение.</param>
        /// <param name="parentAlias">вышестоящий алиас.</param>
        /// <param name="index">индекс источника.</param>
        /// <param name="keysandtypes">ключи и типы.</param>
        /// <param name="baseOutline">смещение в запросе.</param>
        /// <param name="joinscount">количество соединений.</param>
        /// <returns></returns>
        public virtual void CreateJoins(STORMDO.Business.StorageStructForView.PropSource source,
            string parentAlias, int index,
            System.Collections.ArrayList keysandtypes,
            string baseOutline, out int joinscount,
            out string FromPart, out string WherePart)
        {
            string newOutLine = baseOutline + "\t";
            joinscount = 0;
            var fromParts = new List<string>();
            WherePart = string.Empty;
            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    StorageStructForView.ClassStorageDef classStorageDef = subSource.storage[j];
                    if (classStorageDef.parentStorageindex == index)
                    {
                        joinscount++;
                        string curAlias = subSource.Name + j;
                        keysandtypes.Add(
                            new string[]
                            {
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.PrimaryKeyStorageName),
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.TypeStorageName),
                                subSource.Name,
                            });
                        string Link = PutIdentifierIntoBrackets(parentAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.objectLinkStorageName); // +"_M"+(locindex++).ToString());
                        int subjoinscount;
                        string subjoin = string.Empty;
                        string temp;
                        CreateJoins(subSource, curAlias, j, keysandtypes, newOutLine, out subjoinscount, out subjoin, out temp);
                        string FromStr, WhereStr;

                        // Проверка прав на мастера. Значение атрибута отображается пользователю,
                        // если у данного пользователя есть права на операцию, описанную в специальном формате в DataServiceExpression,
                        // иначе подставляем ссылку на фиктивного мастера - специальное значение из того же DataServiceExpression.
                        if (SecurityManager.UseRightsOnAttribute)
                        {
                            string expression = Information.GetPropertyExpression(
                                source.storage[index].ownerType,
                                subSource.ObjectLink,
                                this.GetType());

                            string deniedAccessValue;
                            if (!string.IsNullOrEmpty(expression) && !SecurityManager.CheckAccessToAttribute(expression, out deniedAccessValue))
                            {
                                Link = deniedAccessValue;
                            }
                        }

                        string subTable = string.Concat(
                            GenString("(", subjoinscount),
                            " ",
                            GetTableStorageExpression(classStorageDef.Storage, true));
                        if (classStorageDef.nullableLink)
                        {
                            GetLeftJoinExpression(subTable, curAlias, Link, classStorageDef.PrimaryKeyStorageName, subjoin, baseOutline, out FromStr, out WhereStr);
                        }
                        else
                        {
                            GetInnerJoinExpression(subTable, curAlias, Link, classStorageDef.PrimaryKeyStorageName, subjoin, baseOutline, out FromStr, out WhereStr);
                        }

                        fromParts.Add(FromStr + ")");
                    }
                }
            }

            FromPart = string.Join(string.Empty, fromParts);
        }

        /// <summary>
        /// создать join соединения.
        /// </summary>
        /// <param name="source">источник с которого формируется соединение.</param>
        /// <param name="parentAlias">вышестоящий алиас.</param>
        /// <param name="index">индекс источника.</param>
        /// <param name="keysandtypes">ключи и типы.</param>
        /// <param name="baseOutline">смещение в запросе.</param>
        /// <param name="joinscount">количество соединений.</param>
        /// <returns></returns>
        public virtual void CreateJoins(STORMDO.Business.StorageStructForView.PropSource source,
            string parentAlias, int index,
            System.Collections.ArrayList keysandtypes,
            string baseOutline, out int joinscount,
            out string FromPart, out string WherePart, bool MustNewGenerate)
        {
            if (!MustNewGenerate)
            {
                CreateJoins(source, parentAlias, index, keysandtypes, baseOutline, out joinscount, out FromPart, out WherePart);
                return;
            }

            string newOutLine = baseOutline + "\t";
            joinscount = 0;
            var fromParts = new List<string>();
            WherePart = string.Empty;
            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    StorageStructForView.ClassStorageDef classStorageDef = subSource.storage[j];
                    if (classStorageDef.parentStorageindex == index)
                    {
                        joinscount++;
                        string curAlias = subSource.Name + j;
                        keysandtypes.Add(
                            new string[]
                            {
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.PrimaryKeyStorageName),
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.TypeStorageName),
                                subSource.Name,
                            });
                        string Link = PutIdentifierIntoBrackets(parentAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.objectLinkStorageName); // +"_M"+(locindex++).ToString());
                        string subjoin = string.Empty;
                        string temp;
                        int subjoinscount = 0;
                        string FromStr, WhereStr;

                        CreateJoins(subSource, curAlias, j, keysandtypes, newOutLine, out subjoinscount, out subjoin, out temp, MustNewGenerate);
                        string subTable = GetTableStorageExpression(classStorageDef.Storage, true);
                        if (classStorageDef.nullableLink)
                        {
                            GetLeftJoinExpression(subTable, curAlias, Link, classStorageDef.PrimaryKeyStorageName, string.Empty, baseOutline, out FromStr, out WhereStr);
                        }
                        else
                        {
                            GetInnerJoinExpression(subTable, curAlias, Link, classStorageDef.PrimaryKeyStorageName, string.Empty, baseOutline, out FromStr, out WhereStr);
                        }

                        fromParts.Add(FromStr);
                        if (!string.IsNullOrEmpty(subjoin))
                        {
                            fromParts.Add(subjoin);
                        }
                    }
                }
            }

            FromPart = string.Join(string.Empty, fromParts);
        }

        /// <summary>
        /// преобразовать выражение с учетом.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="namespacewithpoint"></param>
        /// <returns></returns>
        public virtual string TranslateExpression(string expression, string namespacewithpoint, string exteranlnamewithpoint, out bool PointExistInSourceIdentifier)
        {
            string[] expressarr = expression.Split('@');
            var resultParts = new List<string>();
            int nextIndex = 1;
            PointExistInSourceIdentifier = false;
            for (int i = 0; i < expressarr.Length; i++)
            {
                if (i != nextIndex)
                {
                    resultParts.Add(expressarr[i]);
                }
                else
                {
                    string nextIdentifier = expressarr[nextIndex];
                    if (nextIdentifier == string.Empty)
                    {
                        resultParts.Add("@");
                        nextIndex++;
                    }

                    // Обработка псевдонимов полей вида [@имяТега] для поддержки представления результата запроса в виде XML.
                    else if (Regex.IsMatch(nextIdentifier, @"^\w+\]"))
                    {
                        resultParts.Add("@" + expressarr[i]);
                        nextIndex++;
                    }
                    else
                    {
                        int dotIndex = nextIdentifier.IndexOf(".");
                        if (!PointExistInSourceIdentifier && dotIndex >= 0)
                        {
                            PointExistInSourceIdentifier = true;
                        }

                        if (namespacewithpoint != string.Empty)
                        {
                            resultParts.Add(exteranlnamewithpoint + PutIdentifierIntoBrackets(namespacewithpoint + nextIdentifier));
                        }

                        string st1 = exteranlnamewithpoint.Trim('.', '"');
                        if ((st1.IndexOf(".") == -1) && (dotIndex > 0))
                        {
                            st1 = st1.Substring(0, st1.Length - 1);
                            string st2 = string.Empty;
                            string st3 = string.Empty;
                            int lastDotIndex = nextIdentifier.LastIndexOf(".");
                            if (lastDotIndex != dotIndex)
                            {
                                st3 = nextIdentifier.Substring(lastDotIndex + 1);
                                st2 = nextIdentifier.Substring(0, lastDotIndex).Replace(".", string.Empty);
                            }

                            resultParts.Add(PutIdentifierIntoBrackets(st1 + st2 + "0") + "." + PutIdentifierIntoBrackets(st3));
                        }
                        else if (namespacewithpoint == string.Empty)
                        {
                            resultParts.Add(exteranlnamewithpoint + PutIdentifierIntoBrackets(nextIdentifier));
                        }

                        nextIndex += 2;
                    }
                }
            }

            return "(" + string.Join(string.Empty, resultParts) + ")";
        }

        public virtual string GetConvertToTypeExpression(Type valType, string value)
        {
            if (valType == typeof(Guid))
            {
                return "Convert(uniqueidentifier," + value + ")";
            }
            else if (valType == typeof(decimal))
            {
                return "Convert(decimal," + value + ")";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Получение SQL запроса в следующем формате
        /// SELECT
        ///     atr1,atr2, ...  atr3,
        ///     Key1,Key2, ...  key3
        /// FROM
        ///     fromjoins.
        /// </summary>
        /// <param name="storageStruct">структура хранилища.</param>
        /// <param name="AddingAdvansedField">довленные дополнительные свойства.</param>
        /// <param name="AddingKeysCount">добавленниые ключи.</param>
        /// <param name="addMasterFieldsCustomizer"></param>
        /// <param name="addNotMainKeys"></param>
        /// <param name="SelectTypesIds"></param>
        /// <returns></returns>
        public virtual string GenerateSQLSelectByStorageStruct(STORMDO.Business.StorageStructForView storageStruct, bool addNotMainKeys, bool addMasterFieldsCustomizer, string AddingAdvansedField, int AddingKeysCount, bool SelectTypesIds)
        {
            return GenerateSQLSelectByStorageStruct(storageStruct, addNotMainKeys, addMasterFieldsCustomizer, AddingAdvansedField, AddingKeysCount, SelectTypesIds, mustNewgenerate, true);
        }

        public virtual string GenerateSQLSelectByStorageStruct(STORMDO.Business.StorageStructForView storageStruct, bool addNotMainKeys, bool addMasterFieldsCustomizer, string AddingAdvansedField, int AddingKeysCount, bool SelectTypesIds, bool MustNewGenerate, bool MustDopSelect)
        {
            string nl = Environment.NewLine;
            string nlk = "," + nl;
            var selectPartFields = new List<string>();
            var superSelectPartFileds = new List<string>();

            var selectMasterFields = new List<string>();
            bool hasExpresions = false;

            var mainKeyByNamespace = new Dictionary<string, string>();

            for (int i = 0; i < storageStruct.props.Length; i++)
            {
                STORMDO.Business.StorageStructForView.PropStorage prop = storageStruct.props[i];
                if (prop.MultipleProp)
                {
                    continue;
                }

                string brackedIdent = PutIdentifierIntoBrackets(prop.Name);

                bool propStored = prop.Stored;

                // added by fat
                if (propStored && (string.IsNullOrEmpty(prop.Expression) || this.IsExpressionContainAttrubuteCheckOnly(prop.Expression)))
                {
                    if (prop.MastersTypesCount > 0)
                    {
                        // Проверка прав на атрибут. Значение атрибута отображается пользователю,
                        // если у данного пользователя есть права на операцию, описанную в специальном формате в DataServiceExpression,
                        // иначе выводим специальное значение из того же DataServiceExpression.
                        string deniedAccessValue = string.Empty;
                        bool isAccessDenied = SecurityManager.UseRightsOnAttribute
                                              && !SecurityManager.CheckAccessToAttribute(prop.Expression, out deniedAccessValue);

                        string[] names = new string[prop.storage.Length];
                        for (int j = 0; j < prop.storage.Length; j++)
                        {
                            string[] namesM = new string[prop.MastersTypes[j].Length];
                            for (int k = 0; k < prop.MastersTypes[j].Length; k++)
                            {
                                string curName = isAccessDenied ? deniedAccessValue : PutIdentifierIntoBrackets(prop.source.Name + j) + "." + PutIdentifierIntoBrackets(prop.storage[j][k]);
                                namesM[k] = curName;
                                selectMasterFields.Add(curName);
                            }

                            names[j] = this.GetIfNullExpression(namesM);
                        }

                        string field = isAccessDenied ? deniedAccessValue : this.GetIfNullExpression(names);

                        selectPartFields.Add(field + " as " + brackedIdent);
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds.Add(brackedIdent);
                        }
                    }
                    else
                    {
                        string[] names = new string[prop.storage.Length];
                        for (int j = 0; j < prop.storage.Length; j++)
                        {
                            names[j] = PutIdentifierIntoBrackets(prop.source.Name + j) + "." + PutIdentifierIntoBrackets(prop.storage[j][0]);
                        }

                        string field = this.GetIfNullExpression(names);
                        string deniedAccessValue = string.Empty;

                        // Проверка прав на атрибут. Значение атрибута отображается пользователю,
                        // если у данного пользователя есть права на операцию, описанную в специальном формате в DataServiceExpression,
                        // иначе выводим специальное значение из того же DataServiceExpression.
                        if (SecurityManager.UseRightsOnAttribute
                            && !SecurityManager.CheckAccessToAttribute(prop.Expression, out deniedAccessValue))
                        {
                            field = deniedAccessValue;
                        }

                        selectPartFields.Add(field + " as " + brackedIdent);
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds.Add(brackedIdent);
                        }
                    }
                }
                else
                {
                    string express = prop.Expression;
                    if (!string.IsNullOrEmpty(express))
                    {
                        string deniedAccessValue = string.Empty;
                        string translatedExpression;

                        // Проверка прав на атрибут. Значение атрибута отображается пользователю,
                        // если у данного пользователя есть права на операцию, описанную в специальном формате в DataServiceExpression,
                        // иначе выводим специальное значение из того же DataServiceExpression.
                        if (SecurityManager.UseRightsOnAttribute
                            && !SecurityManager.CheckAccessToAttribute(express, out deniedAccessValue))
                        {
                            translatedExpression = deniedAccessValue + " as " + brackedIdent;
                        }
                        else
                        {
                            bool pointExist;
                            string namespacewithpoint = prop.Name.Substring(
                                0,
                                prop.Name.Length - prop.simpleName.Length);
                            translatedExpression = TranslateExpression(
                                express,
                                namespacewithpoint,
                                PutIdentifierIntoBrackets(storageStruct.sources.storage[0].ownerType.FullName) + ".",
                                out pointExist) + " as " + brackedIdent;

                            if (!string.IsNullOrEmpty(namespacewithpoint)
                                && translatedExpression.IndexOf(SQLWhereLanguageDef.StormMainObjectKey, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                string mainObjectKeyReplace = $"{{{namespacewithpoint}{SQLWhereLanguageDef.StormMainObjectKey}}}";

                                if (!mainKeyByNamespace.ContainsKey(prop.source.Name))
                                {
                                    mainKeyByNamespace.Add(prop.source.Name, mainObjectKeyReplace);
                                }

                                var regex = new Regex($"\"?{SQLWhereLanguageDef.StormMainObjectKey}\"?", RegexOptions.IgnoreCase);
                                translatedExpression = regex.Replace(translatedExpression, mainObjectKeyReplace);
                            }
                        }

                        selectPartFields.Add("NULL as " + brackedIdent);
                        hasExpresions = true;
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds.Add(translatedExpression);
                        }
                    }
                    else
                    {
                        selectPartFields.Add("NULL as " + brackedIdent);
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds.Add(brackedIdent);
                        }
                    }
                }
            }

            string MainKeyBracked = PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey);
            string MainKey = PutIdentifierIntoBrackets(storageStruct.sources.Name + "0") + "." + PutIdentifierIntoBrackets(storageStruct.sources.storage[0].PrimaryKeyStorageName) + " as " + MainKeyBracked;

            string MainStor = storageStruct.sources.storage[0].Storage;
            string fromstring = string.Concat(
                GetTableStorageExpression(MainStor, false),
                " ",
                PutIdentifierIntoBrackets(storageStruct.sources.Name + "0"),
                " ",
                GetJoinTableModifierExpression());
            string wherestring = string.Empty;

            System.Collections.ArrayList keysandtypes = new System.Collections.ArrayList();
            if (storageStruct.sources.LinckedStorages.Length > 0)
            {
                int joinsCount;
                string frompar, wherepar;
                CreateJoins(storageStruct.sources, storageStruct.sources.Name + "0", 0, keysandtypes, "\t", out joinsCount, out frompar, out wherepar, MustNewGenerate);
                fromstring += frompar;
                if (joinsCount > 0)
                {
                    if (!MustNewGenerate)
                    {
                        fromstring = GenString("(", joinsCount) + fromstring;
                    }
                }

                if (!string.IsNullOrEmpty(wherepar))
                {
                    wherestring = wherepar;
                }
            }

            var selectKeyFields = new List<string> { MainKey, };
            var superSelectKeyFields = new List<string> { MainKeyBracked, };

            if (addNotMainKeys)
            {
                for (int i = 0; i < keysandtypes.Count; i++)
                {
                    string[] keyandtype = (string[])keysandtypes[i];

                    string masterKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterKey" + i);
                    selectKeyFields.Add(keyandtype[0] + " as " + masterKeyBracked);
                    superSelectKeyFields.Add(masterKeyBracked);

                    if (SelectTypesIds)
                    {
                        string typeKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterType" + i);
                        selectKeyFields.Add(keyandtype[1] + " as " + typeKeyBracked);
                        superSelectKeyFields.Add(typeKeyBracked);
                    }

                    string replace;
                    if (mainKeyByNamespace.TryGetValue(keyandtype[2], out replace))
                    {
                        superSelectPartFileds = superSelectPartFileds.Select(s => s.Replace(replace, masterKeyBracked)).ToList();
                    }
                }
            }

            int keyIndex = keysandtypes.Count;
            if (addMasterFieldsCustomizer)
            {
                for (int i = 0; i < selectMasterFields.Count; i++)
                {
                    string masterKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterKey" + keyIndex);
                    selectKeyFields.Add(selectMasterFields[i] + " as " + masterKeyBracked);
                    superSelectKeyFields.Add(masterKeyBracked);
                    keyIndex++;
                }
            }

            for (int i = 0; i < AddingKeysCount; i++)
            {
                string masterKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterKey" + keyIndex);
                selectKeyFields.Add(GetConvertToTypeExpression(typeof(Guid), "null") + " as " + masterKeyBracked);
                superSelectKeyFields.Add(masterKeyBracked);
                keyIndex++;
            }

            if (AddingAdvansedField != string.Empty)
            {
                selectKeyFields.Add(AddingAdvansedField);
                superSelectKeyFields.Add(AddingAdvansedField);
            }

            string MainSelect =
                "SELECT " + nl
                + (!selectPartFields.Any() ? string.Empty : string.Join(nlk, selectPartFields) + nlk)
                + string.Join(nlk, selectKeyFields) + nl
                + "FROM " + nl
                + fromstring
                + (string.IsNullOrEmpty(wherestring) ? string.Empty : $"{nl}WHERE {nl}{wherestring}");

            if (hasExpresions && MustDopSelect)
            {
                MainSelect =
                    "SELECT " + nl
                    + string.Join(nlk, superSelectPartFileds) + nlk
                    + string.Join(nlk, superSelectKeyFields) + nl
                    + "FROM " + nl +
                    "( " + MainSelect + ")" + PutIdentifierIntoBrackets(storageStruct.sources.storage[0].ownerType.FullName);
            }

            return MainSelect;
        }

        /// <summary>
        /// Конвертация константных значений в строки запроса.
        /// </summary>
        /// <param name="value">Значение, которое требуется преобразовать в соответствующую в БД строку.</param>
        /// <returns>Полученная строка.</returns>
        public virtual string ConvertSimpleValueToQueryValueString(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            Type valType = value.GetType();

            if (ConverterToQueryValueString?.IsSupported(valType) == true)
            {
                return ConverterToQueryValueString.ConvertToQueryValueString(value);
            }

            if (value is IConvertibleToQueryValueString convertibleValue)
            {
                return convertibleValue.ConvertToQueryValueString();
            }

            if (valType == typeof(string))
            {
                if ((string)value == string.Empty)
                {
                    return "NULL";
                }

                return "'" + value.ToString().Replace("'", "''") + "'";
            }

            if (value is char)
            {
                return Convert.ToInt32((char)value).ToString(CultureInfo.InvariantCulture);
            }

            if (valType == typeof(DateTime))
            {
                return "'" + ((DateTime)value).ToString(System.Globalization.DateTimeFormatInfo.InvariantInfo) + "." + ((DateTime)value).ToString("fff") + "'";
            }

            if (valType == typeof(TimeSpan))
            {
                return "'" + value.ToString() + "'";
            }

            if (valType == typeof(KeyGen.KeyGuid))
            {
                return string.Format("'{0}'", value);
            }

            if (valType.IsEnum)
            {
                string s = STORMDO.EnumCaption.GetCaptionFor(value);
                if (s == null || s == string.Empty)
                {
                    return "NULL";
                }
                else
                {
                    return "'" + s + "'";
                }
            }

            if (valType == typeof(bool))
            {
                if ((bool)value)
                {
                    return "1";
                }

                return "0";
            }

            if (valType == typeof(Guid))
            {
                return "'" + ((Guid)value).ToString("B") + "'";
            }

            if (valType == typeof(double))
            {
                return ((double)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }

            if (valType == typeof(decimal))
            {
                return ((decimal)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }

            if (valType == typeof(float))
            {
                return ((float)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }

            if (valType.IsSubclassOf(typeof(DataObject)))
            {
                return ConvertSimpleValueToQueryValueString(((DataObject)value).__PrimaryKey);
            }

            if (valType == typeof(byte[]))
            {
                var sb = new StringBuilder(BitConverter.ToString((byte[])value));
                sb.Insert(0, "0x").Replace("-", string.Empty);
                return sb.ToString();
            }

            return value.ToString();
        }

        /// <summary>
        /// конвертация значений в строки запроса.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual string ConvertValueToQueryValueString(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            else
            {
                if (Utils.IsInternalBaseType(value))
                {
                    return ConvertSimpleValueToQueryValueString(value);
                }
                else
                {
                    System.Type valType = value.GetType();

                    if (valType.IsEnum)
                    {
                        string s = STORMDO.EnumCaption.GetCaptionFor(value);
                        if (s == null || s == string.Empty)
                        {
                            return "NULL";
                        }

                        return "'" + s + "'";
                    }

                    System.Type storageType = Information.GetStorageType(value, this.GetType());
                    value = ICSSoft.STORMNET.Convertors.InOperatorsConverter.Convert(value, storageType);
                    return ConvertSimpleValueToQueryValueString(value);
                }
            }
        }

        /// <summary>
        /// Преобразование значение свойства в строку для запроса.
        /// </summary>
        /// <param name="dataobject"></param>
        /// <param name="propname"></param>
        /// <returns></returns>
        public virtual string ConvertValueToQueryValueString(DataObject dataobject, string propname)
        {
            object value = Information.GetPropValueByName(dataobject, propname);
            if (value == null)
            {
                return "NULL";
            }
            else
            {
                if (Utils.IsInternalBaseType(value))
                {
                    return ConvertSimpleValueToQueryValueString(value);
                }
                else
                {
                    System.Type valType = value.GetType();
                    if (valType.IsEnum)
                    {
                        string s = STORMDO.EnumCaption.GetCaptionFor(value);
                        if (s == null || s == string.Empty)
                        {
                            return "NULL";
                        }
                        else
                        {
                            return "'" + s + "'";
                        }
                    }
                    else
                    {
                        System.Type storageType = Information.GetPropertyStorageType(dataobject.GetType(), propname, this.GetType());
                        value = ICSSoft.STORMNET.Convertors.InOperatorsConverter.Convert(value, storageType);
                        return ConvertSimpleValueToQueryValueString(value);
                    }
                }
            }
        }

        private string ReplaceAliases(STORMDO.Business.StorageStructForView[] StorageStruct, string[] asnameprop, string sw)
        {
            for (int i = 0; i < StorageStruct[0].props.Length; i++)
            {
                string asname = asnameprop[i];
                string doprst = string.Empty;
                string rexst = "[^\"" + @"\w]" + asname + "[^\"" + @"\w]";

                // если кто-то написал в ограничениях без двойных кавычек, то исправляем
                while (System.Text.RegularExpressions.Regex.IsMatch(sw, rexst))
                {
                    string dsw = System.Text.RegularExpressions.Regex.Match(sw, rexst).Value;
                    sw = sw.Replace(dsw, dsw.Replace(asname, PutIdentifierIntoBrackets(asname)));
                }

                // заменим алиасы на нужные значения
                rexst = string.Empty;
                StorageStructForView.PropStorage prop = StorageStruct[0].props[i];
                string[] names = new string[prop.storage.Length];
                if (prop.MastersTypesCount > 0)
                {
                    for (int jj = 0; jj < names.Length; jj++)
                    {
                        string[] namesM = new string[prop.MastersTypes[jj].Length];
                        for (int k = 0; k < namesM.Length; k++)
                        {
                            string curName = PutIdentifierIntoBrackets(prop.source.Name + jj) + "." +
                                PutIdentifierIntoBrackets(prop.storage[jj][k]);
                            namesM[k] = curName;
                        }

                        names[jj] = this.GetIfNullExpression(namesM);
                    }

                    rexst = this.GetIfNullExpression(names);

                    // doprst = GetValueForLimitParam(LimitFunction, asname);
                }
                else if (prop.storage[0][0] != null)
                {
                    for (int jj = 0; jj < names.Length; jj++)
                    {
                        names[jj] = PutIdentifierIntoBrackets(prop.source.Name + jj) + "." +
                                PutIdentifierIntoBrackets(prop.storage[jj][0]);
                    }

                    rexst = this.GetIfNullExpression(names);
                }
                else if (prop.Expression != null)
                {
                    bool PointExist = false;
                    rexst = TranslateExpression(prop.Expression, string.Empty,
                        PutIdentifierIntoBrackets(prop.source.Name + "0") + ".", out PointExist);
                }
                else { }
                sw = System.Text.RegularExpressions.Regex.Replace(sw,
                    "(^|[ ])" + PutIdentifierIntoBrackets(asname) + doprst, " " + rexst);
                sw = System.Text.RegularExpressions.Regex.Replace(sw,
                    "(^|[(])" + PutIdentifierIntoBrackets(asname) + doprst, "(" + rexst);
                sw = System.Text.RegularExpressions.Regex.Replace(sw,
                    "(^|[^.])" + PutIdentifierIntoBrackets(asname) + doprst, rexst);
            }

            return sw;
        }

        private bool CheckExists(STORMFunction LimitFunction)
        {
            bool bexists = false;
            for (int i = 0; i < LimitFunction.Parameters.Count; i++)
            {
                if (LimitFunction.Parameters[i] is ICSSoft.STORMNET.FunctionalLanguage.Function)
                {
                    bexists = CheckExists((ICSSoft.STORMNET.FunctionalLanguage.Function)LimitFunction.Parameters[i]);
                }

                if (bexists)
                {
                    return true;
                }
            }

            if (LimitFunction.FunctionDef.StringedView.StartsWith("Exist"))
            {
                return true;
            }

            if (LimitFunction.Parameters[0] is string)
            {
                return ((string)LimitFunction.Parameters[0]).ToUpper().IndexOf("SELECT") != -1;
            }

            return bexists;
        }

        /// <summary>
        /// Преобразование функции.
        /// </summary>
        /// <param name="LimitFunction"></param>
        /// <returns></returns>
        public virtual string LimitFunction2SQLWhere(STORMFunction LimitFunction,
            STORMDO.Business.StorageStructForView[] StorageStruct, string[] asnameprop, bool MustNewGenerate)
        {
            string sw =
                ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(
                    LimitFunction,
                    new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString(ConvertValueToQueryValueString),
                    new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets(PutIdentifierIntoBrackets),
                    this);
            if (MustNewGenerate)
            {
                sw = ReplaceAliases(StorageStruct, asnameprop, sw);

                // не применять для существуют такие и им подобных..
                bool bExistsFounded = CheckExists(LimitFunction);
                if (!bExistsFounded)
                {
                    sw = System.Text.RegularExpressions.Regex.Replace(sw,
                        PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey),
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.Name + "0") + "." +
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.storage[0].PrimaryKeyStorageName),
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }

                string sPK = PutIdentifierIntoBrackets("STORMGENERATEDQUERY") + "." + PutIdentifierIntoBrackets(SQLWhereLanguageDef.StormMainObjectKey);
                int k = -1;
                while (sw.IndexOf(sPK, k + 1) > 0)
                {
                    k = sw.IndexOf(sPK, k + 1);
                    if (k > 0)
                    {
                        // здесь еще раз заменяем алиас связующего поля, во внутреннем запросе и PK внешней таблицы..
                        string st1 = sw.Substring(k + sPK.Length);
                        st1 = System.Text.RegularExpressions.Regex.Match(st1, @"\"".*\""",
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;
                        st1 = st1.Substring(0, st1.IndexOf((char)34, 1, st1.Length - 1) + 1);
                        string st2 = System.Text.RegularExpressions.Regex.Match(sw.Substring(0, k), @"(( as )|(SELECT ))?.*" + " as " + st1,
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;
                        st2 = System.Text.RegularExpressions.Regex.Replace(st2, " as " + st1, string.Empty);
                        string st3 = sw.Substring(k);
                        int kk = st3.IndexOf(st1) + st1.Length;
                        string st4 = st3.Substring(0, kk);
                        sw = sw.Substring(0, k) + st4.Replace(st1, st2) + st3.Substring(kk);
                    }
                }

                sw = System.Text.RegularExpressions.Regex.Replace(sw, sPK,
                    PutIdentifierIntoBrackets(StorageStruct[0].sources.Name + "0") + "." +
                    PutIdentifierIntoBrackets(StorageStruct[0].sources.storage[0].PrimaryKeyStorageName),
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            return sw;
        }

        /// <summary>
        /// Преобразование функции.
        /// </summary>
        /// <param name="LimitFunction"></param>
        /// <returns></returns>
        public virtual string LimitFunction2SQLWhere(STORMFunction LimitFunction)
        {
            return
                ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(LimitFunction,
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString(ConvertValueToQueryValueString),
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets(PutIdentifierIntoBrackets),
                this);
        }

        //-------LOAD separated string Objetcs ------------------------------------

        /// <summary>
        /// Загрузка без создания объектов.
        /// </summary>
        /// <param name="separator">разделитель в строках.</param>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>массив структур <see cref="ObjectStringDataView"/>.</returns>
        public virtual ObjectStringDataView[] LoadStringedObjectView(
            char separator,
            LoadingCustomizationStruct customizationStruct)
        {
            object state = null;
            ObjectStringDataView[] res = LoadStringedObjectView(separator, customizationStruct, ref state);
            return res;
        }

        /// <summary>
        /// Загрузка без создания объектов.
        /// </summary>
        /// <param name="separator">разделитель в строках.</param>
        /// <param name="customizationStruct"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public virtual ObjectStringDataView[] LoadStringedObjectView(
            char separator,
            LoadingCustomizationStruct customizationStruct,
            ref object State)
        {
            object ID = BusinessTaskMonitor.BeginTask("Load objects");
            try
            {
                System.Type[] dataObjectType = customizationStruct.LoadingTypes;
                RunChangeCustomizationString(dataObjectType);

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                StorageStructForView[] StorageStruct;

                string SelectString = string.Empty;

                // копия нужна для того, чтобы вернуть представление на место в случае
                // его изменения в генерейте (а оно будет при наличии вычислимых полей)
                View origView = customizationStruct.View.Clone();
                int origViewLength = origView.Properties.Length;

                SelectString = GenerateSQLSelect(customizationStruct, false, out StorageStruct, false);

                object id1 = BusinessTaskMonitor.BeginSubTask(SelectString, ID);

                // получаем данные
                object[][] resValue = ReadFirst(
                    SelectString,
                    ref State, customizationStruct.LoadingBufferSize);

                // Если представление было изменено, то ..
                if (!object.ReferenceEquals(customizationStruct.View, origView))
                {
                    int dif = customizationStruct.View.Properties.Length - origView.Properties.Length;

                    // .. меняем StorageStruct по оригинальному представлению
                    StorageStruct = new StorageStructForView[customizationStruct.LoadingTypes.Length];
                    for (int i = 0; i < customizationStruct.LoadingTypes.Length; i++)
                    {
                        Type ltype = customizationStruct.LoadingTypes[i];
                        StorageStruct[i] = ICSSoft.STORMNET.Information.GetStorageStructForView(origView, ltype, StorageType,
                        new ICSSoft.STORMNET.Information.GetPropertiesInExpressionDelegate(GetPropertiesInExpression), GetType());
                    }

                    // .. удаляем лишние данные из результата
                    if (resValue != null)
                    {
                        for (int i = 0; i < resValue.Length; i++)
                        {
                            object[] bak = resValue[i];
                            resValue[i] = new object[bak.Length - dif];
                            for (int j = 0, k = 0; j < resValue[i].Length; j++)
                            {
                                resValue[i][j] = bak[k];
                                k++;
                                if (k == origViewLength)
                                {
                                    k += dif;
                                }
                            }
                        }
                    }

                    // .. восстанавливаем исходное представление
                    customizationStruct.View = origView;
                }

                int PropCount = customizationStruct.View.Properties.Length + ((customizationStruct.AdvansedColumns != null) ? customizationStruct.AdvansedColumns.Length : 0);
                object[] procRead = null;
                ObjectStringDataView[] result = null;
                if (resValue == null)
                {
                    result = new ObjectStringDataView[0];
                }
                else
                {
                    result = Utils.ProcessingRowSet2StringedView(resValue, dataObjectType, PropCount, separator, customizationStruct, StorageStruct, this, Types, ref procRead, SecurityManager);
                }

                State = new object[] { State, dataObjectType, StorageStruct, separator, PropCount, customizationStruct, procRead };
                BusinessTaskMonitor.EndSubTask(id1);
                return result;
            }
            finally
            {
                BusinessTaskMonitor.EndTask(ID);
            }
        }

        /// <summary>
        /// Загрузка без создания объектов.
        /// </summary>
        /// <param name="customizationStruct"></param>
        /// <returns></returns>
        public virtual object[][] LoadRawValues(LoadingCustomizationStruct customizationStruct)
        {
            object state = null;
            object ID = BusinessTaskMonitor.BeginTask("Load raw values");
            try
            {
                RunChangeCustomizationString(customizationStruct.LoadingTypes);

                // Применим полномочия на строки
                ApplyReadPermissions(customizationStruct, SecurityManager);

                StorageStructForView[] storageStruct;

                // копия нужна для того, чтобы вернуть представление на место в случае
                // его изменения в генерейте (а оно будет при наличии вычислимых полей)
                View origView = customizationStruct.View.Clone();
                string selectString = this.GenerateSQLSelect(customizationStruct, false, out storageStruct, false);

                //// selectString = selectString.Substring(0, selectString.IndexOf("\"STORMMainObjectKey\",") - 3) + selectString.Substring(selectString.IndexOf("FROM ("), selectString.Length - selectString.IndexOf("FROM ("));

                if (customizationStruct.Distinct)
                {
                    /*
                     * В запросе вначале идут свойства из представления,
                     * потом свойства из DataServiceExpression (те, которые обернуты в @@),
                     * потом PrimaryKey и мастера.
                     * Т.к. Distinct, то нужно оставить только свойства из представления,
                     * а все осталоное до следущего подзапроса - убрать.
                    */

                    var iFr = selectString.IndexOf(',');
                    for (var i = 1; i < origView.Properties.Length; i++)
                    {
                        iFr = selectString.IndexOf(',', iFr + 1);
                    }

                    var iTo = selectString.IndexOf("FROM", iFr, StringComparison.Ordinal) - 1;
                    selectString = selectString.Remove(iFr, iTo - iFr + 1);
                    selectString = selectString.Insert(iFr, " ");

                    // представление будет восстановлено позже
                    if (customizationStruct.View.Properties.Length > 1)
                    {
                        customizationStruct.View.Properties =
                            new[]
                                {
                                    customizationStruct.View.Properties[0],
                                };
                    }
                }

                // получаем данные
                object[][] resValue = ReadFirst(selectString, ref state, customizationStruct.LoadingBufferSize)
                                      ?? new object[0][];

                // Если представление было изменено, то ..
                if (!ReferenceEquals(customizationStruct.View, origView))
                {
                    int dif = customizationStruct.View.Properties.Length - origView.Properties.Length;

                    // .. меняем StorageStruct по оригинальному представлению
                    storageStruct = new StorageStructForView[customizationStruct.LoadingTypes.Length];
                    for (int i = 0; i < customizationStruct.LoadingTypes.Length; i++)
                    {
                        Type ltype = customizationStruct.LoadingTypes[i];
                        storageStruct[i] = Information.GetStorageStructForView(origView, ltype, StorageType,
                        GetPropertiesInExpression, GetType());
                    }

                    // .. удаляем лишние данные из результата
                    for (int i = 0; i < resValue.Length; i++)
                    {
                        object[] bak = resValue[i];
                        resValue[i] = new object[bak.Length - dif];
                        for (int j = 0, k = 0; j < resValue[i].Length; j++)
                        {
                            resValue[i][j] = bak[k];
                            k++;
                            if (k == origView.Properties.Length)
                            {
                                k += dif;
                            }
                        }
                    }

                    // .. восстанавливаем исходное представление
                    customizationStruct.View = origView;
                }

                return resValue;
            }
            finally
            {
                BusinessTaskMonitor.EndTask(ID);
            }
        }

        public virtual ObjectStringDataView[] LoadValues(
            char separator,
            LoadingCustomizationStruct customizationStruct)
        {
            object ID = BusinessTaskMonitor.BeginTask("Load objects");
            try
            {
                RunChangeCustomizationString(customizationStruct.LoadingTypes);

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                STORMDO.Business.StorageStructForView[] StorageStruct;

                string SelectString = string.Empty;
                SelectString = GenerateSQLSelect(customizationStruct, false, out StorageStruct, false);

                object id1 = BusinessTaskMonitor.BeginSubTask(SelectString, ID);

                // получаем данные
                object State = null;
                object[][] resValue = ReadFirst(
                    SelectString,
                    ref State, customizationStruct.LoadingBufferSize);
                int PropCount = customizationStruct.View.Properties.Length + ((customizationStruct.AdvansedColumns != null) ? customizationStruct.AdvansedColumns.Length : 0);
                object[] procRead = null;
                ObjectStringDataView[] result = null;
                if (resValue == null)
                {
                    result = new ObjectStringDataView[0];
                }
                else
                {
                    result = Utils.ProcessingRowSet2StringedView(resValue, true, customizationStruct.LoadingTypes, PropCount, separator, customizationStruct, StorageStruct, this, Types, ref procRead, new DataObjectCache(), SecurityManager);
                }

                State = new object[] { State, customizationStruct.LoadingTypes, StorageStruct, separator, PropCount, customizationStruct, procRead };
                BusinessTaskMonitor.EndSubTask(id1);
                return result;
            }
            finally
            {
                BusinessTaskMonitor.EndTask(ID);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual ObjectStringDataView[] LoadStringedObjectView(ref object state)
        {
            object[] statearr = (object[])state;
            System.Type[] dataObjectType = (System.Type[])statearr[1];
            STORMDO.Business.StorageStructForView[] StorageStruct = (STORMDO.Business.StorageStructForView[])statearr[2];

            // получаем данные
            LoadingCustomizationStruct customizationStruct = (LoadingCustomizationStruct)statearr[5];
            object[][] resValue = ReadNext(ref statearr[0], customizationStruct.LoadingBufferSize);
            object[] procRread = (object[])statearr[6];

            int PropCount = (int)statearr[4];
            char separator = (char)statearr[3];
            if (resValue == null)
            {
                return new ObjectStringDataView[0];
            }
            else
            {
                return Utils.ProcessingRowSet2StringedView(resValue, dataObjectType, PropCount, separator, customizationStruct, StorageStruct, this, Types, ref procRread, SecurityManager);
            }

            // statearr[6] = procRread;
        }

        /// <summary>
        /// Корректное завершения операции порционного чтения LoadStringedObjectView.
        /// </summary>
        /// <param name="state">Параметр состояния загрузки (массив объектов).</param>
        public void CompleteLoadStringedObjectView(ref object state)
        {
            if (state == null || !state.GetType().IsArray)
            {
                return;
            }

            var stateArray = (object[])state;

            if (stateArray.Length == 0)
            {
                return;
            }

            if (stateArray[0] == null || !stateArray[0].GetType().IsArray)
            {
                return;
            }

            var parameterArray = (object[])stateArray[0];

            var reader = parameterArray[1] as IDataReader;

            var connection = parameterArray[0] as IDbConnection;

            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }

            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            state = null;
        }

        public virtual void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache)
        {
            UpdateObject(ref dobject, dataObjectCache, false);
        }

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        public virtual void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache, bool alwaysThrowException)
        {
            STORMDO.DataObject[] arr = new STORMDO.DataObject[] { dobject };
            UpdateObjects(ref arr, dataObjectCache, alwaysThrowException);
            if (arr != null && arr.Length > 0)
            {
                dobject = arr[0];
            }
            else
            {
                dobject = null;
            }
        }

        public virtual void UpdateObject(ICSSoft.STORMNET.DataObject dobject)
        {
            UpdateObject(dobject, false);
        }

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        public virtual void UpdateObject(ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache)
        {
            UpdateObject(ref dobject, dataObjectCache);
        }

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        public virtual void UpdateObject(ICSSoft.STORMNET.DataObject dobject, bool alwaysThrowException)
        {
            UpdateObject(ref dobject, new DataObjectCache(), alwaysThrowException);
        }

        /// <summary>
        /// Возвращает измененные данные со значениями.
        /// </summary>
        /// <param name="dobject">у кого проверяем.</param>
        /// <param name="CheckLoadedProps">проверять ли загруженность измененных свойств.</param>
        /// <param name="propsWithValues">пары свойство-значение.</param>
        /// <param name="detailObjects">вычисленные измененные объекты.</param>
        /// <param name="ReturnPropStorageNames">возвращать ли не сами свойства а их хранилища.</param>
        protected virtual void GetAlteredPropsWithValues(
            ICSSoft.STORMNET.DataObject dobject, bool CheckLoadedProps,
            out ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary propsWithValues,
            out ICSSoft.STORMNET.DataObject[] detailObjects,
            out ICSSoft.STORMNET.DataObject[] masterObjects,
            bool ReturnPropStorageNames)
        {
            propsWithValues = new ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary();
            System.Collections.ArrayList details = new System.Collections.ArrayList();
            System.Collections.ArrayList masters = new System.Collections.ArrayList();
            System.Type type = dobject.GetType();

            string[] props = (dobject.GetStatus(false) == ObjectStatus.Created) ? Information.GetPropertyNamesForInsert(type)
                : (Information.AutoAlteredClass(type) ? dobject.GetAlteredPropertyNames(false) : dobject.GetAlteredPropertyNames());

            props = Information.SortByLoadingOrder(type, props);
            if (CheckLoadedProps && dobject.GetLoadingState() != ICSSoft.STORMNET.LoadingState.Loaded)
            {
                SpecColl.StringCollection alteredprops = new System.Collections.Specialized.StringCollection();
                alteredprops.AddRange(props);
                string[] loadedprops = dobject.GetLoadedProperties();
                foreach (string lp in loadedprops)
                {
                    int index = alteredprops.IndexOf(lp);
                    if (index >= 0)
                    {
                        alteredprops.Remove(lp);
                    }
                }

                if (alteredprops.Count > 0)
                {
                    throw new CantUpdateNotLoadedPropertiesException(dobject, alteredprops);
                }
            }

            foreach (string prop in props)
            {
                object propval = STORMDO.Information.GetPropValueByName(dobject, prop);
                System.Type propType = STORMDO.Information.GetPropertyType(type, prop);
                string propstor = ReturnPropStorageNames
                    ? STORMDO.Information.GetPropertyStorageName(type, prop)
                    : prop;
                if (propType.IsSubclassOf(typeof(DataObject)))
                { // Это мастеровой объект
                    System.Type[] mastertypes = TypeUsage.GetUsageTypes(type, prop);
                    System.Type propValType = (propval == null) ? null : propval.GetType();
                    if (propval == null && Information.GetPropertyNotNull(type, prop))
                    {
                        throw new PropertyCouldnotBeNullException(prop, dobject);
                    }

                    bool findedMasterType = false;

                    if (ReturnPropStorageNames)
                    {
                        for (int i = 0; i < mastertypes.Length; i++)
                        {
                            string realpropname;
                            if (propstor == string.Empty)
                            {
                                realpropname = PutIdentifierIntoBrackets(STORMDO.Information.GetPropertyStorageName(type, prop, i));
                            }
                            else
                            {
                                realpropname = PutIdentifierIntoBrackets(propstor + "_m" + i);
                            }

                            if (!Information.CheckCompatiblePropertyStorageTypes(type, realpropname, propValType, mastertypes[i]))
                            {
                                propsWithValues.Add(realpropname, ConvertValueToQueryValueString(null));
                            }
                            else
                            {
                                DataObject dobjval = (DataObject)propval;
                                if (dobjval.GetStatus(false) == ObjectStatus.Created)
                                {
                                    KeyGen.KeyGenerator.GenerateUnique(dobjval, this);
                                }

                                ObjectStatus os = dobjval.GetStatus(false);
                                if (os == ObjectStatus.Created || os == ObjectStatus.Deleted)
                                {
                                    if (Array.IndexOf(Information.GetAutoStoreMastersDisabled(type), prop) == -1)
                                    {
                                        masters.Add(dobjval); // Автосоздание мастеров только, если не отключено
                                    }
                                }

                                propsWithValues.Add(realpropname, ConvertValueToQueryValueString(dobjval.__PrimaryKey));
                                findedMasterType = true;
                            }
                        }
                    }
                    else
                    {
                        string realpropname = propstor;
                        propsWithValues.Add(realpropname, ConvertValueToQueryValueString(((DataObject)propval).__PrimaryKey));
                        findedMasterType = true;
                    }

                    if (!findedMasterType && propval != null)
                    {
                        throw new NotFoundInTypeUsageException(type, prop, propValType);
                    }
                }
                else if (propType.IsSubclassOf(typeof(STORMDO.DetailArray)))
                { // Детейловые объекты
                    if (propval != null)
                    {
                        foreach (DataObject dob in (STORMDO.DetailArray)propval)
                        {
                            if (dob.GetStatus() != STORMDO.ObjectStatus.UnAltered)
                            {
                                details.Add(dob);
                            }
                        }
                    }
                }
                else
                { // Обычное свойство
                    propsWithValues.Add(
                        ReturnPropStorageNames ? PutIdentifierIntoBrackets(propstor)
                        : propstor,
                        ConvertValueToQueryValueString(propval));
                }
            }

            ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary tmpPropsWithValues = new ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary();
            foreach (var key in propsWithValues.GetAllKeys())
            {
                if (tmpPropsWithValues.ContainsKey(key))
                {
                    continue;
                }

                tmpPropsWithValues.Add(key, propsWithValues.Get(key));
            }

            propsWithValues = tmpPropsWithValues;

            detailObjects = (DataObject[])details.ToArray(typeof(DataObject));
            masterObjects = (DataObject[])masters.ToArray(typeof(DataObject));
        }

        private void AddOperationOnTable(SortedList tableOperations, string table, OperationType op)
        {
            if (tableOperations.ContainsKey(table))
            {
                OperationType ot = (OperationType)tableOperations[table];
                ot = ot | op;
                tableOperations[table] = ot;
            }
            else
            {
                tableOperations.Add(table, op);
            }
        }

        /// <summary>
        /// Удаляемые объекты особым образом добавляются в словарь.
        /// </summary>
        /// <param name="dobject">
        /// Удаляемый объект.
        /// </param>
        /// <param name="updaterFunction">
        /// Функция обновления.
        /// </param>
        /// <param name="deleteObjectsLimits">
        /// Соответствие между таблицей и ограничениями на первичные ключи удаляемых объектов в соответствующей таблице.
        /// </param>
        /// <param name="tableOperations">
        /// The table operations.
        /// </param>
        private void AddDeletedObjectToDeleteDictionary(
            DataObject dobject,
            Function updaterFunction,
            Dictionary<string, Function> deleteObjectsLimits,
            SortedList tableOperations)
        {
            var doType = dobject.GetType();
            System.Type[] dots = (StorageType == StorageTypeEnum.HierarchicalStorage)
                ? Information.GetCompatibleTypesForTypeConvertion(doType)
                : new Type[] { doType };

            for (int i = 0; i < dots.Length; i++)
            {
                string tableName = Information.GetClassStorageName(dots[i]);
                SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;
                if (!deleteObjectsLimits.ContainsKey(tableName))
                {
                    string prkeyStorName = Information.GetPrimaryKeyStorageName(dots[i]);
                    VariableDef var = new VariableDef(lang.GetObjectTypeForNetType(KeyGenerator.KeyType(doType)), prkeyStorName);
                    Function func = lang.GetFunction(lang.funcEQ, var, dobject.__PrimaryKey);

                    if (updaterFunction != null)
                    {
                        func = updaterFunction;
                    }

                    deleteObjectsLimits[tableName] = func;
                    AddOperationOnTable(tableOperations, tableName, OperationType.Delete);
                }
                else
                {
                    Function func = (Function)deleteObjectsLimits[tableName];
                    if (func.FunctionDef.StringedView == lang.funcEQ)
                    {
                        func = lang.GetFunction(lang.funcIN, func.Parameters[0], func.Parameters[1]);
                        deleteObjectsLimits[tableName] = func;
                    }

                    func.Parameters.Add(dobject.__PrimaryKey);
                }
            }
        }

        /// <summary>
        /// The operation type.
        /// </summary>
        [Flags]
        protected enum OperationType : short
        {
            None = 0,
            Update = 1,
            Delete = 2,
            Insert = 4,
        }

        /// <summary>
        /// Добавление в словаре обрабатываемых объектов.
        /// </summary>
        /// <param name="processedDictionary">Словарь обрабатываемых объектов.</param>
        /// <param name="dob">Объект данных.</param>
        private void AddToProcessingObjectsKeys(Dictionary<TypeKeyPair, bool> processedDictionary, DataObject dob)
        {
            TypeKeyPair typeKeyPair = new TypeKeyPair(dob.GetType(), dob.__PrimaryKey);
            processedDictionary.Add(typeKeyPair, true);
        }

        /// <summary>
        /// Проверка на наличие объекта в коллекции обрабатываемых объектов.
        /// </summary>
        /// <param name="processedDictionary">
        /// Словарь обрабатываемых объектов.
        /// </param>
        /// <param name="dob">
        /// Объект данных.
        /// </param>
        /// <returns>
        /// Если объект содержится в коллекции, то <c>true</c>.
        /// </returns>
        private bool ContainsKeyINProcessing(Dictionary<TypeKeyPair, bool> processedDictionary, DataObject dob)
        {
            TypeKeyPair typeKeyPair = new TypeKeyPair(dob.GetType(), dob.__PrimaryKey);
            return processedDictionary.ContainsKey(typeKeyPair);
        }

        /// <summary>
        /// Генерация запросов для изменения объектов.
        /// </summary>
        /// <param name="deleteQueries"> Запросы для удаление. </param>
        /// <param name="updateQueries"> Запросы для изменения. </param>
        /// <param name="updateFirstQueries"> Запросы для изменения, выполняемые до остальных запросов. </param>
        /// <param name="updateLastQueries"> Запросы для изменения, выполняемые после остальных запросов.</param>
        /// <param name="insertQueries"> Запросы для добавления. </param>
        /// <param name="tableOperations"> The Table Operations. </param>
        /// <param name="queryOrder"> The Query Order. </param>
        /// <param name="checkLoadedProps"> Проверять ли загруженность свойств. </param>
        /// <param name="processingObjects"> The processing Objects. </param>
        /// <param name="dataObjectCache"> The Data Object Cache.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapper" /> или <see cref="DbTransactionWrapperAsync" />.</param>
        /// <param name="dobjects"> Для чего генерим запросы. </param>
        public virtual void GenerateQueriesForUpdateObjects(
            Dictionary<string, List<string>> deleteQueries,
            Dictionary<string, List<string>> updateQueries,
            Dictionary<string, List<string>> updateFirstQueries,
            Dictionary<string, List<string>> updateLastQueries,
            Dictionary<string, List<string>> insertQueries,
            SortedList tableOperations,
            StringCollection queryOrder,
            bool checkLoadedProps,
            ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            object dbTransactionWrapper,
            params ICSSoft.STORMNET.DataObject[] dobjects)

        {
            GenerateQueriesForUpdateObjects(
                deleteQueries,
                updateQueries,
                updateFirstQueries,
                updateLastQueries,
                insertQueries,
                tableOperations,
                queryOrder,
                checkLoadedProps,
                processingObjects,
                dataObjectCache,
                null,
                dbTransactionWrapper,
                dobjects);
        }

        /// <summary>
        /// Поиск в списке объектов аналогичного указанному (должен совпасть тип и идентификатор).
        /// </summary>
        /// <param name="searchedDataObjectList">Список объектов, где производится поиск.</param>
        /// <param name="searchedDataObject">Объект, который ищется.</param>
        /// <returns>Найденный объект или <c>null</c>.</returns>
        private DataObject GetDataObjectFromSearchedList(List<DataObject> searchedDataObjectList, DataObject searchedDataObject)
        {
            Type searchedDataObjectType = searchedDataObject.GetType();
            return searchedDataObjectList
                .FirstOrDefault(x => x.__PrimaryKey.Equals(searchedDataObject.__PrimaryKey) && x.GetType() == searchedDataObjectType);
        }

        /// <summary>
        /// Сгенерировать объекты для учета аудита агрегаторов обновляемых объектов, если они обновляются отдельно от агрегатора.
        /// </summary>
        /// <param name="processingObjects">Объекты, которые необходимо обработать.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="auditObjects">Список объектов, для которых нужно создать записи аудита. Сюда записывается результат работы метода.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapper" />.</param>
        protected virtual void GenerateAuditForAggregators(
            ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            ref List<DataObject> auditObjects,
            DbTransactionWrapper dbTransactionWrapper = null)
        {
            if (!AuditService.IsAuditEnabled)
            {
                return;
            }

            var processingObjectsList = processingObjects.Cast<DataObject>().ToList();

            // Занесение агрегаторов обновляемых объектов в аудит, если они есть.
            foreach (var dataObject in processingObjectsList)
            {
                var dataObjectType = dataObject.GetType();

                var aggregatorPropertyName = Information.GetAgregatePropertyName(dataObjectType);
                if (string.IsNullOrEmpty(aggregatorPropertyName))
                {
                    continue;
                }

                Type aggregatorType = Information.GetPropertyType(dataObjectType, aggregatorPropertyName);

                string detailArrayPropertyName = Information.GetDetailArrayPropertyName(aggregatorType, dataObjectType);

                View aggregatorAuditView = AuditService.GetAuditViewByType(aggregatorType, tTypeOfAuditOperation.UPDATE);

                // Если данного детейла в представлении аудита агрегатора нет, значит и аудит агрегатора при
                // изменении этого детейла вести не нужно.
                if (aggregatorAuditView == null || !aggregatorAuditView.CheckPropname(detailArrayPropertyName, true))
                {
                    continue;
                }

                // Определение прежнего агрегатора (если объект только что создан, то его нет).
                DataObject oldAggregator = null;
                if (dataObject.GetStatus() != ObjectStatus.Created)
                {
                    oldAggregator =
                        (DataObject)Information.GetPropValueByName(dataObject.GetDataCopy(), aggregatorPropertyName);
                    if (oldAggregator == null)
                    {
                        // Загрузка агрегатора из БД. Производится во временный объект, так как в оригинальном объекте
                        // может храниться ссылка на новый агрегатор.
                        var tempObject = (DataObject)Activator.CreateInstance(dataObjectType);
                        tempObject.SetExistObjectPrimaryKey(dataObject.__PrimaryKey);
                        var tempView = new View { Name = "AggregatorLoadingView", DefineClassType = dataObjectType };
                        tempView.AddProperty(aggregatorPropertyName);

                        if (dbTransactionWrapper == null)
                        {
                            LoadObject(tempView, tempObject, dataObjectCache);
                        }
                        else
                        {
                            LoadObjectByExtConn(tempView, tempObject, true, false, dataObjectCache, dbTransactionWrapper.Connection, dbTransactionWrapper.Transaction);
                        }

                        oldAggregator =
                            (DataObject)Information.GetPropValueByName(tempObject, aggregatorPropertyName);
                    }
                    else
                    {
                        // Для корректной обработки аудитом объект не должен являться копией данных, поэтому, если он был взят
                        // из копии, то нужно создать новый объект.
                        var tempPrimaryKey = oldAggregator.__PrimaryKey;
                        oldAggregator = (DataObject)Activator.CreateInstance(aggregatorType);
                        oldAggregator.SetExistObjectPrimaryKey(tempPrimaryKey);
                        oldAggregator.SetStatus(ObjectStatus.Altered);
                    }
                }

                // Определение нового агрегатора (если объект удален, то он нам не нужен).
                DataObject newAggregator = null;
                if (dataObject.GetStatus() != ObjectStatus.Deleted)
                {
                    newAggregator =
                        (DataObject)Information.GetPropValueByName(dataObject, aggregatorPropertyName)
                        ?? oldAggregator;
                }

                // Агрегаторы уже могут быть в списке аудита, надо это проверить.
                DataObject existingObj;
                if (newAggregator != null)
                {
                    if ((existingObj = GetDataObjectFromSearchedList(auditObjects, newAggregator)) != null)
                    {
                        newAggregator = existingObj;
                    }
                    else
                    {
                        auditObjects.Add(newAggregator);
                    }
                }

                if (oldAggregator != null)
                {
                    if ((existingObj = GetDataObjectFromSearchedList(auditObjects, oldAggregator)) != null)
                    {
                        oldAggregator = existingObj;
                    }
                    else
                    {
                        auditObjects.Add(oldAggregator);
                    }
                }

                // Загрузка детейлов в агрегаторы, если они еще не были загружены.
                var aggregatorView = new View
                {
                    Name = "DetailsLoadingView",
                    DefineClassType = aggregatorType,
                };

                DetailInView detailInView = aggregatorAuditView.GetDetail(detailArrayPropertyName);
                aggregatorView.AddDetailInView(detailInView.Name, detailInView.View, true);
                var aggregatorsToLoad = new List<DataObject>();

                if (oldAggregator != null && !oldAggregator.CheckLoadedProperty(detailArrayPropertyName))
                {
                    aggregatorsToLoad.Add(oldAggregator);
                }

                if (newAggregator != null && newAggregator != oldAggregator
                    && !newAggregator.CheckLoadedProperty(detailArrayPropertyName))
                {
                    aggregatorsToLoad.Add(newAggregator);
                }

                foreach (DataObject aggregator in aggregatorsToLoad)
                {
                    DataObject tempAggregator = (DataObject)Activator.CreateInstance(aggregatorType);
                    tempAggregator.SetExistObjectPrimaryKey(aggregator.__PrimaryKey);

                    if (dbTransactionWrapper == null)
                    {
                        LoadObject(aggregatorView, tempAggregator, true, false, dataObjectCache);
                    }
                    else
                    {
                        LoadObjectByExtConn(aggregatorView, tempAggregator, true, false, dataObjectCache, dbTransactionWrapper.Connection, dbTransactionWrapper.Transaction);
                    }

                    DetailArray tempAggregatorDetailArray =
                        (DetailArray)Information.GetPropValueByName(tempAggregator, detailArrayPropertyName),
                        aggregatorDetailArray =
                            (DetailArray)Information.GetPropValueByName(aggregator, detailArrayPropertyName);
                    while (tempAggregatorDetailArray.Count > 0)
                    {
                        var detail = tempAggregatorDetailArray.ItemByIndex(0);
                        tempAggregatorDetailArray.RemoveByIndex(0);
                        aggregatorDetailArray.AddObject(detail);
                    }

                    aggregator.AddLoadedProperties(detailArrayPropertyName);
                }

                var oldAggregatorDetailArray = oldAggregator == null
                                                ? null
                                                : (DetailArray)Information.GetPropValueByName(oldAggregator, detailArrayPropertyName);
                var newAggregatorDetailArray = newAggregator == null
                                                ? null
                                                : (DetailArray)Information.GetPropValueByName(newAggregator, detailArrayPropertyName);

                DataObject deletedObj = oldAggregatorDetailArray != null ? oldAggregatorDetailArray.GetByKey(dataObject.__PrimaryKey) : null;
                switch (dataObject.GetStatus())
                {
                    case ObjectStatus.Altered:
                        if (oldAggregator != newAggregator && deletedObj != null)
                        {
                            deletedObj.SetStatus(ObjectStatus.Deleted);
                            oldAggregator.GetStatus(true);
                        }

                        if (dataObject.GetDetailArray() == null)
                        {
                            newAggregatorDetailArray.SetByKey(dataObject.__PrimaryKey, dataObject);
                        }

                        break;
                    case ObjectStatus.Deleted:
                        if (deletedObj != null)
                        {
                            deletedObj.SetStatus(ObjectStatus.Deleted);
                            oldAggregator.GetStatus(true);
                        }

                        break;
                    case ObjectStatus.Created:
                        if (dataObject.GetDetailArray() == null)
                        {
                            newAggregatorDetailArray.SetByKey(dataObject.__PrimaryKey, dataObject);
                        }

                        break;
                }

                if (newAggregator != null)
                {
                    newAggregator.GetStatus(true);
                }
            }
        }

        /// <summary>
        /// Добавление в коллекцию зависимостей.
        /// </summary>
        /// <param name="to">
        /// От чего зависит.
        /// </param>
        /// <param name="from">
        /// Что зависит.
        /// </param>
        /// <param name="dependencies">
        /// Коллекция зависимостей.
        /// </param>
        private void AddDependencies(Type to, Type @from, Dictionary<Type, List<Type>> dependencies)
        {
            if (dependencies.ContainsKey(to))
            {
                if (dependencies[to].IndexOf(@from) < 0)
                {
                    dependencies[to].Add(@from);
                }
            }
            else
            {
                List<Type> deps = new List<Type> { @from };
                dependencies.Add(to, deps);
            }
        }

        /// <summary>
        /// Формирование графа зависимостей.
        /// </summary>
        /// <param name="currentObject">Текущий обрабатываемый объект данных.</param>
        /// <param name="currentType">Текущий обрабатываемый тип.</param>
        /// <param name="dependencies">Дополняемые зависимости.</param>
        /// <param name="extraUpdateList">Дополнительные объекты на обновление.</param>
        private void GetDependencies(DataObject currentObject, Type currentType, Dictionary<Type, List<Type>> dependencies, List<DataObject> extraUpdateList)
        {
            string[] props = Information.GetAllPropertyNames(currentType);

            // Smirnov: GetStatus довольно тяжелая операция, при исполнении в цикле имеет значительное воздействие.
            // В общем GetStatus мб лишним только в случае отсутствия детейлов и мастеров в типе,
            // такими случаями можно пренебречь `for the greater good`.
            ObjectStatus? objectStatus = currentObject?.GetStatus();

            // Смотрим мастера и детейлы для выявления зависимостей.
            foreach (string prop in props)
            {
                Type propType = Information.GetPropertyType(currentType, prop);
                if (propType.IsSubclassOf(typeof(DetailArray)))
                {
                    // Обрабатываем детейлы.
                    Type[] types = Information.GetCompatibleTypesForDetailProperty(currentType, prop);
                    foreach (Type type in types)
                    {
                        // Если этот детейл еще не обходили.
                        if (!dependencies.ContainsKey(type))
                        {
                            AddDependencies(type, currentType, dependencies);

                            // Для детейлов доформируем зависимости.
                            GetDependencies(null, type, dependencies, extraUpdateList);
                        }
                    }

                    if (objectStatus == ObjectStatus.Deleted)
                    {
                        foreach (DataObject detail in (DetailArray)Information.GetPropValueByName(currentObject, prop))
                        {
                            if (detail.ContainsAlteredProps())
                            {
                                extraUpdateList.Add(detail);
                            }
                        }
                    }
                }
                else if (propType.IsSubclassOf(typeof(DataObject)))
                {
                    // Проверяем, есть ли детейловое свойство с таким-же типом.
                    string detailProp = Information.GetDetailArrayPropertyName(currentType, propType);
                    if (detailProp == null || (currentObject != null && Information.GetPropValueByName(currentObject, prop) != null))
                    {
                        // Обрабатываем мастера.
                        AddDependencies(currentType, propType, dependencies);

                        // Обрабатываем наследников мастера.
                        Type[] propertyTypes = TypeUsageProvider.TypeUsage.GetUsageTypes(currentType, prop);
                        foreach (Type type in propertyTypes)
                        {
                            AddDependencies(currentType, type, dependencies);
                        }
                    }
                    else if (objectStatus == ObjectStatus.Deleted && currentObject.ContainsAlteredProps())
                    {
                        extraUpdateList.Add(currentObject);
                    }
                }
            }
        }

        /// <summary>
        /// Получение из графа зависимостей линейного порядка.
        /// </summary>
        /// <param name="dependencies">
        /// Структура зависимостей.
        /// </param>
        /// <returns>
        /// Линейный порядок обработки.
        /// </returns>
        private List<Type> GetOrderFromDependencies(Dictionary<Type, List<Type>> dependencies)
        {
            List<Type> result = new List<Type>();
            var dependenciesTypes = dependencies.Keys.ToList();

            foreach (Type key in dependencies.Keys)
            {
                if (dependenciesTypes.Contains(key))
                {
                    dependenciesTypes.Remove(key);
                    foreach (var type in dependencies[key])
                    {
                        GetOrderFromDependenciesHelper(type, dependencies, ref dependenciesTypes, ref result);
                    }

                    if (!result.Contains(key))
                    {
                        result.Add(key);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Вспомогательный метод для получения из графа зависимостей линейного порядка.
        /// </summary>
        /// <param name="currentType">
        /// Текущий обрабатываемый тип.
        /// </param>
        /// <param name="dependencies">
        /// Структура зависимостей.
        /// </param>
        /// <param name="dependenciesTypes">
        /// Список не обработанных типов.
        /// </param>
        /// <param name="result">
        /// Формируемый результат.
        /// </param>
        private void GetOrderFromDependenciesHelper(Type currentType, Dictionary<Type, List<Type>> dependencies, ref List<Type> dependenciesTypes, ref List<Type> result)
        {
            if (dependenciesTypes.Contains(currentType))
            {
                dependenciesTypes.Remove(currentType);
                foreach (var type in dependencies[currentType])
                {
                    GetOrderFromDependenciesHelper(type, dependencies, ref dependenciesTypes, ref result);
                }

                if (!result.Contains(currentType))
                {
                    result.Add(currentType);
                }
            }
            else
            {
                if (!dependencies.ContainsKey(currentType) && !result.Contains(currentType))
                {
                    result.Add(currentType);
                }
            }
        }

        /// <summary>
        /// Поиск циклов в графе зависимостей.
        /// </summary>
        /// <param name="grafDependencies">Граф полученный в результате обхода графа зависимостей.</param>
        /// <param name="currentType">Текущий обрабатываемый тип.</param>
        /// <param name="dependencies">Графе зависимостей.</param>
        /// <param name="previousType">Предыдущий обрабатываемый тип.</param>
        /// <param name="dependenciesList">Коллекция графов зависимостей.</param>
        /// <param name="processingObjects">Текущие обрабатываемые объекты (то есть объекты, которые данный сервис данных планирует подтвердить в БД
        ///  в текущей транзакции).</param>
        /// <param name="createdList">Измененные данные со значениями, для которых строятся запросы, на создание.</param>
        /// <param name="alteredList">Измененные данные со значениями, для которых строятся запросы, на обновление.</param>
        /// <returns>False - если найден цикл и надо остановить обход.</returns>
        private bool FindCycles(
            ref List<Type> grafDependencies,
            Type currentType,
            Dictionary<Type, List<Type>> dependencies,
            Type previousType,
            List<Dictionary<Type, List<Type>>> dependenciesList,
            ArrayList processingObjects,
            Dictionary<DataObject, Collections.CaseSensivityStringDictionary> createdList,
            Dictionary<DataObject, Collections.CaseSensivityStringDictionary> alteredList)
        {
            if (dependencies.ContainsKey(currentType))
            {
                foreach (Type dependencie in dependencies[currentType])
                {
                    // Если зависимости нет в текущих обрабатываемых объектах со статусом на создание или удалени, то она удаляется из графа зависимостей.
                    var processingObjectsList = processingObjects.Cast<DataObject>().Where(t => t.GetType() == dependencie && t.GetStatus() == ObjectStatus.Created).ToList();
                    if (processingObjectsList.Count > 0)
                    {
                        // Проверяется создает ли цикл зависимость.
                        if (!grafDependencies.Contains(dependencie))
                        {
                            grafDependencies.Add(dependencie);
                            if (!FindCycles(ref grafDependencies, dependencie, dependencies, currentType, dependenciesList, processingObjects, createdList, alteredList))
                            {
                                return false;
                            }
                            else
                            {
                                grafDependencies.Remove(dependencie);
                            }
                        }
                        else
                        {
                            if (!FixCyclic(currentType, dependencie, dependencies[currentType], createdList, alteredList))
                            {
                                var indexDependencie = grafDependencies.IndexOf(dependencie);
                                var type = grafDependencies[indexDependencie + 1];
                                if (!FixCyclic(dependencie, type, dependencies[dependencie], createdList, alteredList))
                                {
                                    throw new Exception("Неразрешимый цикл");
                                }
                                else
                                {
                                    grafDependencies.RemoveRange(indexDependencie, grafDependencies.Count - indexDependencie);
                                    dependenciesList.Add(dependencies);
                                    return false;
                                }
                            }
                            else
                            {
                                dependenciesList.Add(dependencies);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (processingObjects.Cast<DataObject>().Where(t => t.GetType() == dependencie && t.GetStatus() == ObjectStatus.Deleted).ToList().Count() == 0)
                        {
                            dependencies[currentType].Remove(dependencie);
                            dependenciesList.Add(dependencies);
                            return false;
                        }
                    }
                }
            }
            else
            {
                // Если зависимость просто есть (в текущих обрабатываемых объектах её нет или статус на обновление), то она удаляется из графа зависимостей.
                if (processingObjects.Cast<DataObject>().Where(t => t.GetType() == currentType && (t.GetStatus() == ObjectStatus.Created || t.GetStatus() == ObjectStatus.Deleted)).ToList().Count > 0)
                {
                    grafDependencies.Remove(currentType);
                }
                else
                {
                    dependencies[previousType].Remove(currentType);
                    dependenciesList.Add(dependencies);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Разрешение циклической связи.
        /// </summary>
        /// <param name="currentType">Текущий обрабатываемый тип.</param>
        /// <param name="dependencie">Зависимость с циклом.</param>
        /// <param name="dependencies">Графе зависимостей.</param>
        /// <param name="createdList">Измененные данные со значениями, для которых строятся запросы, на создание.</param>
        /// <param name="alteredList">Измененные данные со значениями, для которых строятся запросы, на обновление.</param>
        /// <returns>True - если цикл можно исправить, False - если нет.</returns>
        private bool FixCyclic(
            Type currentType,
            Type dependencie,
            List<Type> dependencies,
            Dictionary<DataObject, Collections.CaseSensivityStringDictionary> createdList,
            Dictionary<DataObject, Collections.CaseSensivityStringDictionary> alteredList)
        {
            string[] props = Information.GetAllPropertyNames(currentType).Where(prop => currentType.GetProperty(prop)?.GetGetMethod()?.IsStatic != true).ToArray();

            // Поиск свойства, в нужном типе.
            var filterProps = props
                .Where(t => Information.GetPropertyType(currentType, t).FullName == dependencie.FullName)
                .ToList();

            if (filterProps.Count > 0)
            {
                foreach (string prop in filterProps)
                {
                    // Проверяется свойство на NotNull.
                    if (!Information.GetPropertyNotNull(currentType, prop))
                    {
                        dependencies.Remove(dependencie);
                        var createdObjects = createdList.Keys.Where(t => t.GetType() == currentType);

                        Type[] types = TypeUsage.GetUsageTypes(currentType, prop);
                        string[] propertyStorageNames = new string[types.Length];
                        string defaultStorageName = Information.GetPropertyStorageName(currentType, prop);
                        for (int i = 0; i < types.Length; i++)
                        {
                            string storageName = defaultStorageName == string.Empty ? Information.GetPropertyStorageName(currentType, prop, i) : $"{defaultStorageName}_m{i}";
                            propertyStorageNames[i] = PutIdentifierIntoBrackets(storageName);
                        }

                        // Изменяем значения в объектах, для устранения цикла.
                        foreach (var createdObject in createdObjects)
                        {
                            Collections.CaseSensivityStringDictionary propsCollection = createdList[createdObject];
                            foreach (var propertyStorageName in propertyStorageNames)
                            {
                                // Учитываем только непустые свойства в статусе Created.
                                object propValue = Information.GetPropValueByName(createdObject, prop);
                                if (propValue is DataObject propObj && propObj.GetStatus(false) == ObjectStatus.Created)
                                {
                                    // Добавляем свойство в запрос на изменение объекта.
                                    string propQueryValue = propsCollection.Get(propertyStorageName);
                                    if (alteredList.ContainsKey(createdObject))
                                    {
                                        alteredList[createdObject].Add(propertyStorageName, propQueryValue);
                                    }
                                    else
                                    {
                                        var alteredCollection = new Collections.CaseSensivityStringDictionary();
                                        alteredCollection.Add(propertyStorageName, propQueryValue);
                                        alteredList.Add(createdObject, alteredCollection);
                                    }

                                    // Удаляем из списка свойств на изменение в запросе на создание объекта.
                                    propsCollection.Remove(propertyStorageName);
                                }
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Генерация запросов для изменения объектов
        /// (дополнительно возвращается список объектов, для которых необходимо создание записей аудита).
        /// </summary>
        /// <param name="deleteQueries">Ключ - название таблицы; значение - список запросов на удаление в этой таблице (выходной параметр).</param>
        /// <param name="updateQueries">Сгенерированные запросы для изменения (выходной параметр).</param>
        /// <param name="updateFirstQueries"> Сгенерированные запросы для изменения (выходной параметр), выполняемые до остальных запросов. </param>
        /// <param name="updateLastQueries"> Запросы для изменения, выполняемые после остальных запросов.</param>
        /// <param name="insertQueries">Сгенерированные запросы для добавления (выходной параметр).</param>
        /// <param name="tableOperations">Операции, которые будут произведены над таблицами (выходной параметр).</param>
        /// <param name="queryOrder">Порядок исполнения генерируемых запросов, задаваемый именами таблиц (выходной параметр).</param>
        /// <param name="checkLoadedProps">Проверять ли загруженность свойств.</param>
        /// <param name="processingObjects">Текущие обрабатываемые объекты (то есть объекты, которые данный сервис данных планирует подтвердить в БД
        ///  в текущей транзакции). Выходной параметр.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="auditObjects">Список объектов, которые необходимо записать в аудит (выходной параметр). Заполняется в том случае, когда
        /// передан не null и текущий сервис аудита включен.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapper" /> или <see cref="DbTransactionWrapperAsync" />.</param>
        /// <param name="dobjects">Объекты, для которых генерируются запросы.</param>
        public virtual void GenerateQueriesForUpdateObjects(
            Dictionary<string, List<string>> deleteQueries,
            Dictionary<string, List<string>> updateQueries,
            Dictionary<string, List<string>> updateFirstQueries,
            Dictionary<string, List<string>> updateLastQueries,
            Dictionary<string, List<string>> insertQueries,
            SortedList tableOperations,
            StringCollection queryOrder,
            bool checkLoadedProps,
            ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            List<DataObject> auditObjects,
            object dbTransactionWrapper,
            params DataObject[] dobjects)
        {
            string nl = Environment.NewLine;
            string nlk = ",";
            var deleteObjectsLimits = new Dictionary<string, Function>();
            var deleteDetailsLimits = new Dictionary<string, List<string>>();
            var extraProcessingList = new List<DataObject>();
            var createdList = new Dictionary<DataObject, Collections.CaseSensivityStringDictionary>();
            var alteredList = new Dictionary<DataObject, Collections.CaseSensivityStringDictionary>();
            var alteredLastList = new Dictionary<DataObject, Collections.CaseSensivityStringDictionary>();
            var updateList = new Dictionary<DataObject, UpdaterObject>();

            List<DataObject> extraUpdateList = new List<DataObject>();
            Dictionary<Type, List<Type>> dependencies = new Dictionary<Type, List<Type>>();

            var processingObjectsKeys = new Dictionary<TypeKeyPair, bool>(new TypeKeyPairEqualityComparer());
            foreach (DataObject dobj in dobjects)
            {
                if (!ContainsKeyINProcessing(processingObjectsKeys, dobj))
                {
                    if (dobj.GetStatus(false) == ObjectStatus.Created)
                    {
                        KeyGenerator.GenerateUnique(dobj, this);
                    }

                    processingObjects.Add(dobj);
                    AddToProcessingObjectsKeys(processingObjectsKeys, dobj);
                }
            }

            for (int i = 0; i < processingObjects.Count; i++)
            {
                var processingObject = (DataObject)processingObjects[i];

                UpdaterObject updaterobject = null;
                if (typeof(UpdaterObject).IsInstanceOfType(processingObject))
                {
                    updaterobject = (UpdaterObject)processingObject;
                    processingObject = updaterobject.TemplateObject;
                }

                STORMDO.ObjectStatus curObjectStatus = (i < dobjects.Length) ? processingObject.GetStatus() : processingObject.GetStatus(false);
                Type typeOfProcessingObject = processingObject.GetType();
                BusinessServer[] bss = BusinessServerProvider.GetBusinessServer(typeOfProcessingObject, curObjectStatus, this);
                if (bss != null && bss.Length > 0)
                {
                    foreach (BusinessServer bs in bss)
                    {
                        ProcessBusinessServer(processingObject, typeOfProcessingObject, bs, processingObjects, processingObjectsKeys, ref curObjectStatus);
                    }
                }

                switch (curObjectStatus)
                {
                    case STORMDO.ObjectStatus.UnAltered:
                        break;
                    case STORMDO.ObjectStatus.Deleted:
                        {
                            AddDeletedObjectToDeleteDictionary(
                                processingObject,
                                updaterobject != null ? updaterobject.Function : null,
                                deleteObjectsLimits,
                                tableOperations);
                            STORMDO.DataObject[] subobjects;
                            STORMDO.DataObject[] smastersObj;
                            STORMDO.View[] views;
                            Utils.getDetailsObjects(this, processingObject, out subobjects, out smastersObj, out views);
                            foreach (STORMDO.DataObject subobject in subobjects)
                            {
                                subobject.SetStatus(STORMDO.ObjectStatus.Deleted);
                                if (!ContainsKeyINProcessing(processingObjectsKeys, subobject))
                                {
                                    processingObjects.Add(subobject);
                                    AddToProcessingObjectsKeys(processingObjectsKeys, subobject);
                                }
                            }

                            foreach (DataObject detobj in smastersObj)
                            {
                                if (!ContainsKeyINProcessing(processingObjectsKeys, detobj))
                                {
                                    if (detobj.GetStatus(false) == ObjectStatus.Created)
                                    {
                                        KeyGen.KeyGenerator.GenerateUnique(detobj, this);
                                    }

                                    processingObjects.Add(detobj);
                                    AddToProcessingObjectsKeys(processingObjectsKeys, detobj);
                                }
                            }

                            foreach (STORMDO.View subview in views)
                            {
                                var result = dbTransactionWrapper switch
                                {
                                    DbTransactionWrapper syncWrapper => AddDeletedViewToDeleteDictionary(subview, deleteDetailsLimits, processingObject.__PrimaryKey, tableOperations, dataObjectCache, syncWrapper),
                                    DbTransactionWrapperAsync asyncWrapper => AddDeletedViewToDeleteDictionaryAsync(subview, deleteDetailsLimits, processingObject.__PrimaryKey, tableOperations, dataObjectCache, asyncWrapper).GetAwaiter().GetResult(),
                                    _ => throw new ArgumentException("Параметр dbTransactionWrapper имеет неправильный тип (должен быть ICSSoft.STORMNET.Business.DbTransactionWrapper или ICSSoft.STORMNET.Business.DbTransactionWrapperAsync", nameof(dbTransactionWrapper))
                                };
                                IEnumerable<DataObject> extraProcessingObjects = result.Item1;
                                DataObject[] detailsObjects = result.Item2;

                                extraProcessingList.AddRange(extraProcessingObjects);

                                foreach (DataObject detailObject in detailsObjects)
                                {
                                    detailObject.SetStatus(STORMDO.ObjectStatus.Deleted);
                                    if (!ContainsKeyINProcessing(processingObjectsKeys, detailObject))
                                    {
                                        processingObjects.Add(detailObject);
                                        AddToProcessingObjectsKeys(processingObjectsKeys, detailObject);
                                    }
                                }
                            }

                            break;
                        }

                    case STORMDO.ObjectStatus.Created:
                        {
                            if (AuditService.IsTypeAuditable(typeOfProcessingObject))
                            {
                                AuditService.AddCreateAuditInformation(processingObject);
                            }

                            ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary propsWithValues;
                            DataObject[] detailsObjects;
                            DataObject[] mastersObjects;

                            if (StorageType == StorageTypeEnum.HierarchicalStorage)
                            {
                                GetAlteredPropsWithValues(
                                    processingObject,
                                    false,
                                    out propsWithValues,
                                    out detailsObjects,
                                    out mastersObjects,
                                    false);
                            }
                            else
                            {
                                GetAlteredPropsWithValues(
                                    processingObject,
                                    false,
                                    out propsWithValues,
                                    out detailsObjects,
                                    out mastersObjects,
                                    true);
                            }

                            foreach (DataObject detobj in detailsObjects)
                            {
                                if (!ContainsKeyINProcessing(processingObjectsKeys, detobj))
                                {
                                    if (detobj.GetStatus(false) == ObjectStatus.Created)
                                    {
                                        KeyGen.KeyGenerator.GenerateUnique(detobj, this);
                                    }

                                    processingObjects.Add(detobj);
                                    AddToProcessingObjectsKeys(processingObjectsKeys, detobj);
                                }
                            }

                            foreach (DataObject detobj in mastersObjects)
                            {
                                if (!ContainsKeyINProcessing(processingObjectsKeys, detobj))
                                {
                                    if (detobj.GetStatus(false) == ObjectStatus.Created)
                                    {
                                        KeyGen.KeyGenerator.GenerateUnique(detobj, this);
                                    }

                                    processingObjects.Add(detobj);
                                    AddToProcessingObjectsKeys(processingObjectsKeys, detobj);
                                }
                            }

                            createdList.Add(processingObject, propsWithValues);
                            updateList.Add(processingObject, updaterobject);

                            break;
                        }

                    case STORMDO.ObjectStatus.Altered:
                        {
                            if (AuditService.IsTypeAuditable(typeOfProcessingObject))
                            {
                                AuditService.AddEditAuditInformation(processingObject);
                            }

                            ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary propsWithValues;
                            DataObject[] detailsObjects;
                            DataObject[] mastersObjects;

                            if (StorageType == StorageTypeEnum.HierarchicalStorage)
                            {
                                GetAlteredPropsWithValues(
                                    processingObject,
                                    false,
                                    out propsWithValues,
                                    out detailsObjects,
                                    out mastersObjects,
                                    false);
                            }
                            else
                            {
                                GetAlteredPropsWithValues(processingObject, checkLoadedProps, out propsWithValues, out detailsObjects, out mastersObjects, true);
                            }

                            foreach (DataObject detobj in detailsObjects)
                            {
                                if (detobj.GetStatus(false) == ObjectStatus.Created)
                                {
                                    KeyGen.KeyGenerator.GenerateUnique(detobj, this);
                                }

                                if (!ContainsKeyINProcessing(processingObjectsKeys, detobj))
                                {
                                    processingObjects.Add(detobj);
                                    AddToProcessingObjectsKeys(processingObjectsKeys, detobj);
                                }
                            }

                            foreach (DataObject detobj in mastersObjects)
                            {
                                if (detobj.GetStatus(false) == ObjectStatus.Created)
                                {
                                    KeyGen.KeyGenerator.GenerateUnique(detobj, this);
                                }

                                if (!ContainsKeyINProcessing(processingObjectsKeys, detobj))
                                {
                                    processingObjects.Add(detobj);
                                    AddToProcessingObjectsKeys(processingObjectsKeys, detobj);
                                }
                            }

                            alteredList.Add(processingObject, propsWithValues);
                            updateList.Add(processingObject, updaterobject);

                            break;
                        }
                }
            }

            foreach (DataObject processingObject in processingObjects)
            {
                // Включем текущий объект в граф зависимостей.
                GetDependencies(processingObject, processingObject.GetType(), dependencies, extraUpdateList);
            }

            // Поиск и разрешение циклов в зависимостях.
            List<Type> grafDependencies = new List<Type>();
            List<Dictionary<Type, List<Type>>> dependenciesList = new List<Dictionary<Type, List<Type>>>();
            dependenciesList.Add(dependencies);
            for (int i = 0; i < dependenciesList.Count; i++)
            {
                var dependencieKeys = dependenciesList[i].Keys;
                foreach (Type dependencie in dependencieKeys)
                {
                    grafDependencies.Clear();
                    grafDependencies.Add(dependencie);
                    if (!FindCycles(ref grafDependencies, dependencie, dependenciesList[i], dependencie, dependenciesList, processingObjects, createdList, alteredLastList))
                    {
                        break;
                    }
                }
            }

            for (int i = 0; i < extraUpdateList.Count; i++)
            {
                DataObject processingObject = extraUpdateList[i];
                ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary propsWithValues;
                var alteredFirstList = new Dictionary<DataObject, Collections.CaseSensivityStringDictionary>();
                var updateFirstList = new Dictionary<DataObject, UpdaterObject>();
                DataObject[] detailsObjects;
                DataObject[] mastersObjects;

                UpdaterObject updaterobject = null;
                if (typeof(UpdaterObject).IsInstanceOfType(processingObject))
                {
                    updaterobject = (UpdaterObject)processingObject;
                    processingObject = updaterobject.TemplateObject;
                }

                if (StorageType == StorageTypeEnum.HierarchicalStorage)
                {
                    GetAlteredPropsWithValues(processingObject, false, out propsWithValues, out detailsObjects, out mastersObjects, false);
                }
                else
                {
                    GetAlteredPropsWithValues(processingObject, checkLoadedProps, out propsWithValues, out detailsObjects, out mastersObjects, true);
                }

                alteredFirstList.Add(processingObject, propsWithValues);
                updateFirstList.Add(processingObject, updaterobject);

                GenerateUpdateQueries(alteredFirstList, updateFirstList, tableOperations, updateFirstQueries);
            }

            List<Type> depList = GetOrderFromDependencies(dependencies);
            foreach (DataObject processingObject in processingObjects)
            {
                Type typeOfProcessingObject = processingObject.GetType();
                if (depList.IndexOf(typeOfProcessingObject) < 0)
                {
                    depList.Add(typeOfProcessingObject);
                }
            }

            queryOrder.Clear();
            foreach (Type t in depList)
            {
                string qry = Information.GetClassStorageName(t);
                if (!queryOrder.Contains(qry))
                {
                    queryOrder.Add(qry);
                }
            }

            if (createdList.Count > 0)
            {
                foreach (var processingObject in createdList.Keys)
                {
                    var propsWithValues = createdList[processingObject];

                    Type typeOfProcessingObject = processingObject.GetType();

                    if (propsWithValues.Count > 0)
                    {
                        if (StorageType == StorageTypeEnum.HierarchicalStorage)
                        {
                            string[] cols = propsWithValues.GetAllKeys();
                            var valuesByTables = new Dictionary<Type, List<string>>();
                            foreach (string col in cols)
                            {
                                Type defType = Information.GetPropertyDefineClassType(typeOfProcessingObject, col);
                                List<string> propsInTable;
                                if (valuesByTables.ContainsKey(defType))
                                {
                                    propsInTable = valuesByTables[defType];
                                }
                                else
                                {
                                    propsInTable = new List<string> { "__PrimaryKey", };
                                    valuesByTables.Add(defType, propsInTable);
                                }

                                propsInTable.Add(col);
                            }

                            foreach (var valueByTable in valuesByTables)
                            {
                                Type t = valueByTable.Key;
                                string tableName = Information.GetClassStorageName(t);
                                var propsInTable = valueByTable.Value;
                                string columns = string.Join(nlk, propsInTable.Select(PutIdentifierIntoBrackets));
                                string values = string.Join(nlk, propsInTable.Select(p => propsWithValues[p]));

                                string query = $"INSERT INTO {PutIdentifierIntoBrackets(tableName)}{nl} ( {nl}{columns}{nl} ) {nl} VALUES ({nl}{values}{nl})";
                                AddOperationOnTable(tableOperations, tableName, OperationType.Insert);

                                if (!insertQueries.ContainsKey(tableName))
                                {
                                    insertQueries[tableName] = new List<string>();
                                }

                                insertQueries[tableName].Add(query);
                            }
                        }
                        else
                        {
                            string mainTableName = Information.GetClassStorageName(typeOfProcessingObject);
                            string[] cols = propsWithValues.GetAllKeys();
                            string columns = string.Join(nlk, cols);
                            string values = string.Join(nlk, cols.Select(p => propsWithValues[p]));

                            string query = $"INSERT INTO {PutIdentifierIntoBrackets(mainTableName)}{nl} ( {nl}{columns}{nl} ) {nl} VALUES ({nl}{values}{nl})";
                            AddOperationOnTable(tableOperations, mainTableName, OperationType.Insert);

                            if (!insertQueries.ContainsKey(mainTableName))
                            {
                                insertQueries[mainTableName] = new List<string>();
                            }

                            insertQueries[mainTableName].Add(query);
                        }
                    }
                }
            }

            if (alteredList.Count > 0)
            {
                GenerateUpdateQueries(alteredList, updateList, tableOperations, updateQueries);
            }

            if (alteredLastList.Count > 0)
            {
                GenerateUpdateQueries(alteredLastList, updateList, tableOperations, updateLastQueries);
            }

            deleteQueries = new Dictionary<string, List<string>>();
            if (deleteObjectsLimits.Count > 0)
            {
                var tables = deleteObjectsLimits.Keys.Concat(deleteDetailsLimits.Keys).Distinct();

                foreach (string table in tables)
                {
                    deleteQueries[table] = new List<string>();
                    deleteQueries[table].Add($"DELETE FROM {PutIdentifierIntoBrackets(table)} WHERE {LimitFunction2SQLWhere(deleteObjectsLimits[table])}");
                    var deleteDetailsQueries = deleteDetailsLimits[table]
                            .Select(query => $"DELETE FROM {PutIdentifierIntoBrackets(table)} WHERE {query}");
                    deleteQueries[table].AddRange(deleteDetailsQueries);

                    deleteQueries[table] = deleteQueries[table].Distinct().ToList();
                }
            }

            if (AuditService.IsAuditEnabled && auditObjects != null)
            {
                List<DataObject> processingObjectsList =
                    (from DataObject mi in processingObjects select mi).ToList();
                auditObjects.AddRange(processingObjectsList);
                auditObjects.AddRange(extraProcessingList.Where(dataObject => !ContainsKeyINProcessing(processingObjectsKeys, dataObject)));
            }
        }

        private void ProcessBusinessServer(DataObject processingObject, Type typeOfProcessingObject, BusinessServer bs, ArrayList processingObjects, Dictionary<TypeKeyPair, bool> processingObjectsKeys, ref ObjectStatus curObjectStatus)
        {
            try
            {
                bs.ObjectsToUpdate = processingObjects;
                object prevPrimaryKey = processingObject.__PrimaryKey;
                DataObject[] subobjects = bs.OnUpdateDataobject(processingObject);
                curObjectStatus = processingObject.GetStatus(true);
                if (!processingObject.__PrimaryKey.Equals(prevPrimaryKey))
                {
                    TypeKeyPair typeKeyPair = new TypeKeyPair(typeOfProcessingObject, prevPrimaryKey);
                    processingObjectsKeys.Remove(typeKeyPair);
                    if (curObjectStatus == ObjectStatus.Created)
                    {
                        KeyGenerator.GenerateUnique(processingObject, this);
                    }

                    AddToProcessingObjectsKeys(processingObjectsKeys, processingObject);
                }

                foreach (DataObject subobject in subobjects)
                {
                    var subobjectStatus = subobject.GetStatus(true);
                    if (!ContainsKeyINProcessing(processingObjectsKeys, subobject))
                    {
                        if (subobjectStatus == ObjectStatus.Created)
                        {
                            KeyGenerator.GenerateUnique(subobject, this);
                        }

                        processingObjects.Add(subobject);
                        AddToProcessingObjectsKeys(processingObjectsKeys, subobject);
                    }
                }
            }
            finally
            {
                // Высвобождаем обрабатываемые объекты.
                bs.ObjectsToUpdate = null;
            }
        }

        /// <summary>
        /// Генерация запросов для изменения объектов.
        /// </summary>
        /// <param name="alteredList">Измененные данные со значениями, для которых строятся запросы.</param>
        /// <param name="updateList">Спецклассы, предназначенный для выполнения групповых операций.</param>
        /// <param name="tableOperations">Операции, которые будут произведены над таблицами (выходной параметр).</param>
        /// <param name="updateQueries">Сгенерированные запросы для изменения (выходной параметр).</param>
        private void GenerateUpdateQueries(
            Dictionary<DataObject, Collections.CaseSensivityStringDictionary> alteredList,
            Dictionary<DataObject, UpdaterObject> updateList,
            SortedList tableOperations,
            Dictionary<string, List<string>> updateQueries)
        {
            string nl = Environment.NewLine;
            string nlk = ",";
            foreach (var processingObject in alteredList.Keys)
            {
                var propsWithValues = alteredList[processingObject];
                var updaterobject = updateList[processingObject];

                Type typeOfProcessingObject = processingObject.GetType();
                string mainTableName = STORMDO.Information.GetClassStorageName(typeOfProcessingObject);

                if (propsWithValues.Count > 0)
                {
                    if (StorageType == StorageTypeEnum.HierarchicalStorage)
                    {
                        string[] cols = propsWithValues.GetAllKeys();
                        var valuesByTables = new ICSSoft.STORMNET.Collections.TypeBaseCollection();
                        foreach (string col in cols)
                        {
                            Type defType = Information.GetPropertyDefineClassType(typeOfProcessingObject, col);
                            StringCollection propsInTable = null;
                            if (valuesByTables.Contains(defType))
                            {
                                propsInTable = (StringCollection)valuesByTables[defType];
                            }
                            else
                            {
                                propsInTable = new StringCollection();
                                valuesByTables.Add(defType, propsInTable);
                            }

                            propsInTable.Add(col);
                        }

                        for (int k = 0; k < valuesByTables.Count; k++)
                        {
                            Type t = valuesByTables.Key(k);
                            var propsInTable = (StringCollection)valuesByTables[t];
                            string tableName = Information.GetClassStorageName(t);

                            string values = string.Join(nlk, propsInTable.Cast<string>().Select(p => PutIdentifierIntoBrackets(p) + " = " + propsWithValues[p]));
                            SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;
                            var var = new VariableDef(lang.GetObjectTypeForNetType(KeyGenerator.KeyType(t)), Information.GetPrimaryKeyStorageName(t));
                            Function func = lang.GetFunction(lang.funcEQ, var, processingObject.__PrimaryKey);
                            if (updaterobject != null)
                            {
                                func = updaterobject.Function;
                            }

                            string query = $"UPDATE {PutIdentifierIntoBrackets(tableName)} SET {nl}{values}{nl} WHERE {LimitFunction2SQLWhere(func)}";
                            AddOperationOnTable(tableOperations, tableName, OperationType.Update);

                            if (!updateQueries.ContainsKey(tableName))
                            {
                                updateQueries[tableName] = new List<string>();
                            }

                            updateQueries[tableName].Add(query);
                        }
                    }
                    else
                    {
                        string values = string.Join(nlk, propsWithValues.GetAllKeys().Select(p => p + " = " + propsWithValues[p]));
                        SQLWhereLanguageDef lang = SQLWhereLanguageDef.LanguageDef;
                        var var = new VariableDef(lang.GetObjectTypeForNetType(KeyGenerator.KeyType(typeOfProcessingObject)), Information.GetPrimaryKeyStorageName(typeOfProcessingObject));
                        Function func = lang.GetFunction(lang.funcEQ, var, processingObject.__PrimaryKey);
                        if (updaterobject != null)
                        {
                            func = updaterobject.Function;
                        }

                        string query = $"UPDATE {PutIdentifierIntoBrackets(mainTableName)} SET {nl}{values}{nl} WHERE {LimitFunction2SQLWhere(func)}";
                        AddOperationOnTable(tableOperations, mainTableName, OperationType.Update);

                        if (!updateQueries.ContainsKey(mainTableName))
                        {
                            updateQueries[mainTableName] = new List<string>();
                        }

                        updateQueries[mainTableName].Add(query);
                    }
                }
            }
        }

        private bool m_bUseCommandTimeout = false;
        private int m_iCommandTimeout = 0;

        /// <summary>
        /// Приватное поле для <see cref="SecurityManager"/>.
        /// </summary>
        private ISecurityManager _securityManager;

        /// <summary>
        /// IDbCommand.CommandTimeout кроме установки этого таймаута не забудьте установить флаг <see cref="UseCommandTimeout"/>.
        /// </summary>
        public int CommandTimeout
        {
            get { return m_iCommandTimeout; }
            set { m_iCommandTimeout = value; }
        }

        /// <summary>
        /// Использовать ли атрибут <see cref="CommandTimeout"/> (если задан через конфиг, то будет true) по-умолчанию false.
        /// </summary>
        public bool UseCommandTimeout
        {
            get { return m_bUseCommandTimeout; }
            set { m_bUseCommandTimeout = value; }
        }

        protected virtual void CustomizeCommand(System.Data.IDbCommand cmd)
        {
            if (UseCommandTimeout)
            {
                cmd.CommandTimeout = CommandTimeout;
            }

            if (OnCreateCommand != null)
            {
                OnCreateCommand(this, new ICSSoft.STORMNET.Business.CreateCommandEventArgs(cmd));
            }
        }

        protected OperationType Minus(OperationType ops, OperationType value)
        {
            return ops & (~value);
        }

        protected virtual IDbTransaction CreateTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Обновить объекты данных в указанном порядке.
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <param name="alwaysThrowException">
        /// Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию. По умолчанию true;.
        /// </param>
        public virtual void UpdateObjectsOrdered(ref DataObject[] objects, bool alwaysThrowException = true)
        {
            using (var dbTransactionWrapper = new DbTransactionWrapper(this))
            {
                var dataObjectCache = new DataObjectCache();
                try
                {
                    foreach (DataObject dataObject in objects)
                    {
                        DataObject[] dObjs = new[] { dataObject };
                        UpdateObjectsByExtConn(ref dObjs, dataObjectCache, alwaysThrowException, dbTransactionWrapper.Connection, dbTransactionWrapper.Transaction);
                    }

                    dbTransactionWrapper.CommitTransaction();
                }
                catch (Exception ex)
                {
                    dbTransactionWrapper.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Обновить хранилище по объектам (есть параметр, указывающий, всегда ли необходимо взводить ошибку
        /// и откатывать транзакцию при неудачном запросе в базу данных). Если
        /// он true, всегда взводится ошибка. Иначе, выполнение продолжается.
        /// Однако, при этом есть опасность преждевременного окончания транзакции, с переходом для остальных
        /// запросов режима транзакционности в autocommit. Проявлением проблемы являются ошибки навроде:
        /// The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION.
        /// </summary>
        /// <param name="objects">Объекты для обновления.</param>
        /// <param name="dataObjectCache">Кеш объектов.</param>
        /// <param name="alwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapper" />.</param>
        public virtual void UpdateObjectsByExtConn(ref DataObject[] objects, DataObjectCache dataObjectCache, bool alwaysThrowException, DbTransactionWrapper dbTransactionWrapper)
        {
            object id = BusinessTaskMonitor.BeginTask("Update objects");

            var deleteQueries = new Dictionary<string, List<string>>();
            var updateQueries = new Dictionary<string, List<string>>();
            var updateFirstQueries = new Dictionary<string, List<string>>();
            var updateLastQueries = new Dictionary<string, List<string>>();
            var insertQueries = new Dictionary<string, List<string>>();

            var tableOperations = new SortedList();
            var queryOrder = new StringCollection();

            var allQueriedObjects = new ArrayList();

            var auditOperationInfoList = new List<AuditAdditionalInfo>();
            var extraProcessingList = new List<DataObject>();

            GenerateQueriesForUpdateObjects(deleteQueries, updateQueries, updateFirstQueries, updateLastQueries, insertQueries, tableOperations, queryOrder, true, allQueriedObjects, dataObjectCache, extraProcessingList, dbTransactionWrapper, objects);

            GenerateAuditForAggregators(allQueriedObjects, dataObjectCache, ref extraProcessingList, dbTransactionWrapper);

            OnBeforeUpdateObjects(allQueriedObjects);

            // Сортируем объекты в порядке заданным графом связности.
            extraProcessingList.Sort((x, y) =>
            {
                int indexX = queryOrder.IndexOf(Information.GetClassStorageName(x.GetType()));
                int indexY = queryOrder.IndexOf(Information.GetClassStorageName(y.GetType()));
                return indexX.CompareTo(indexY);
            });

            AccessCheckBeforeUpdate(SecurityManager, allQueriedObjects);

            // Порядок выполнения запросов: delete, insert, update.
            if (deleteQueries.Count > 0 || updateQueries.Count > 0 || insertQueries.Count > 0)
            {
                Guid? operationUniqueId = null;

                if (NotifierUpdateObjects != null)
                {
                    operationUniqueId = Guid.NewGuid();
                    NotifierUpdateObjects.BeforeUpdateObjects(operationUniqueId.Value, this, dbTransactionWrapper.Transaction, objects);
                }

                if (AuditService.IsAuditEnabled)
                {
                    /* Аудит проводится именно здесь, поскольку на этот момент все бизнес-сервера на объектах уже выполнились,
                     * объекты находятся именно в том состоянии, в каком должны были пойти в базу + в будущем можно транзакцию передать на исполнение
                     */
                    AuditService.WriteCommonAuditOperationWithAutoFields(extraProcessingList, auditOperationInfoList, this, true, dbTransactionWrapper.Transaction); // TODO: подумать, как записывать аудит до OnBeforeUpdateObjects, но уже потенциально с транзакцией
                }

                string query = string.Empty;
                string prevQueries = string.Empty;
                object subTask = null;
                try
                {
                    Exception ex = null;
                    IDbCommand command = dbTransactionWrapper.CreateCommand();

                    // прошли вглубь обрабатывая only Update||Insert
                    do
                    {
                        string table = queryOrder[0];
                        if (!tableOperations.ContainsKey(table))
                        {
                            tableOperations.Add(table, OperationType.None);
                        }

                        var ops = (OperationType)tableOperations[table];

                        if ((ops & OperationType.Delete) != OperationType.Delete && updateLastQueries.Count == 0)
                        {
                            // Смотрим есть ли Инсерты
                            if ((ops & OperationType.Insert) == OperationType.Insert)
                            {
                                if ((ex = RunCommands(insertQueries[table], command, id, alwaysThrowException)) == null)
                                {
                                    ops = Minus(ops, OperationType.Insert);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // Смотрим есть ли Update
                            if ((ops & OperationType.Update) == OperationType.Update)
                            {
                                if ((ex = RunCommands(updateQueries[table], command, id, alwaysThrowException)) == null)
                                {
                                    ops = Minus(ops, OperationType.Update);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            queryOrder.RemoveAt(0);
                        } else
                        {
                            break;
                        }
                    }
                    while (queryOrder.Count > 0);

                    if (ex != null)
                    {
                        throw ex;
                    }

                    if (queryOrder.Count > 0)
                    {
                        // сзади чистые Update
                        int queryOrderIndex = queryOrder.Count - 1;
                        do
                        {
                            string table = queryOrder[queryOrderIndex];
                            if (tableOperations.ContainsKey(table))
                            {
                                var ops = (OperationType)tableOperations[table];

                                if (ops == OperationType.Update && updateLastQueries.Count == 0)
                                {
                                    if ((ex = RunCommands(updateQueries[table], command, id, alwaysThrowException)) == null)
                                    {
                                        ops = Minus(ops, OperationType.Update);
                                        tableOperations[table] = ops;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            queryOrderIndex--;
                        }
                        while (queryOrderIndex >= 0);
                    }

                    if (ex != null)
                    {
                        throw ex;
                    }

                    foreach (string table in queryOrder)
                    {
                        if ((ex = RunCommands(updateFirstQueries[table], command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }
                    }

                    // Удаляем в обратном порядке.
                    for (int i = queryOrder.Count - 1; i >= 0; i--)
                    {
                        string table = queryOrder[i];
                        if ((ex = RunCommands(deleteQueries[table], command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }
                    }

                    // А теперь опять с начала
                    foreach (string table in queryOrder)
                    {
                        if ((ex = RunCommands(insertQueries[table], command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }

                        if ((ex = RunCommands(updateQueries[table], command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }
                    }

                    foreach (string table in queryOrder)
                    {
                        if ((ex = RunCommands(updateLastQueries[table], command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }
                    }

                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    {
                        // Нужно зафиксировать операции аудита (то есть сообщить, что всё было корректно выполнено и запомнить время)
                        AuditService.RatifyAuditOperationWithAutoFields(
                            tExecutionVariant.Executed,
                            AuditAdditionalInfo.SetNewFieldValuesForList(dbTransactionWrapper.Transaction, this, auditOperationInfoList),
                            this,
                            true);
                    }

                    if (NotifierUpdateObjects != null)
                    {
                        NotifierUpdateObjects.AfterSuccessSqlUpdateObjects(operationUniqueId.Value, this, dbTransactionWrapper.Transaction, objects);
                    }
                }
                catch (Exception excpt)
                {
                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    {
                        // Нужно зафиксировать операции аудита (то есть сообщить, что всё было откачено).
                        AuditService.RatifyAuditOperationWithAutoFields(tExecutionVariant.Failed, auditOperationInfoList, this, false);
                    }

                    if (NotifierUpdateObjects != null)
                    {
                        NotifierUpdateObjects.AfterFailUpdateObjects(operationUniqueId.Value, this, objects);
                    }

                    BusinessTaskMonitor.EndSubTask(subTask);
                    throw new ExecutingQueryException(query, prevQueries, excpt);
                }

                var res = new ArrayList();
                foreach (DataObject changedObject in objects)
                {
                    changedObject.ClearPrototyping(true);

                    if (changedObject.GetStatus(false) != STORMDO.ObjectStatus.Deleted)
                    {
                        Utils.UpdateInternalDataInObjects(changedObject, true, dataObjectCache);
                        res.Add(changedObject);
                    }
                }

                foreach (DataObject dobj in allQueriedObjects)
                {
                    if (dobj.GetStatus(false) != STORMDO.ObjectStatus.Deleted
                        && dobj.GetStatus(false) != STORMDO.ObjectStatus.UnAltered)
                    {
                        Utils.UpdateInternalDataInObjects(dobj, true, dataObjectCache);
                    }
                }

                if (NotifierUpdateObjects != null)
                {
                    NotifierUpdateObjects.AfterSuccessUpdateObjects(operationUniqueId.Value, this, objects);
                }

                objects = new DataObject[res.Count];
                res.CopyTo(objects);

                BusinessTaskMonitor.EndTask(id);
            }

            if (AfterUpdateObjects != null)
            {
                AfterUpdateObjects(this, new DataObjectsEventArgs(objects));
            }
        }

        /// <summary>
        /// Обновить хранилище по объектам (есть параметр, указывающий, всегда ли необходимо взводить ошибку
        /// и откатывать транзакцию при неудачном запросе в базу данных). Если
        /// он true, всегда взводится ошибка. Иначе, выполнение продолжается.
        /// Однако, при этом есть опасность преждевременного окончания транзакции, с переходом для остальных
        /// запросов режима транзакционности в autocommit. Проявлением проблемы являются ошибки навроде:
        /// The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION.
        /// </summary>
        /// <param name="objects">Объекты для обновления.</param>
        /// <param name="dataObjectCache">Кеш объектов.</param>
        /// <param name="alwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        /// <param name="connection">Коннекция (не забудьте закрыть).</param>
        /// <param name="transaction">Транзакция (не забудьте завершить).</param>
        public virtual void UpdateObjectsByExtConn(
            ref DataObject[] objects, DataObjectCache dataObjectCache, bool alwaysThrowException, IDbConnection connection, IDbTransaction transaction)
        {
            DbTransactionWrapper dbTransactionWrapper = new DbTransactionWrapper(connection, transaction);
            UpdateObjectsByExtConn(ref objects, dataObjectCache, alwaysThrowException, dbTransactionWrapper);
        }

        private void OnBeforeUpdateObjects(ArrayList allQueriedObjects)
        {
            if (BeforeUpdateObjects == null)
            {
                return;
            }

            var changedObjects = new List<DataObject>(allQueriedObjects.Count);

            foreach (var obj in allQueriedObjects)
            {
                changedObjects.Add(obj as DataObject);
            }

            BeforeUpdateObjects(this, new DataObjectsEventArgs(changedObjects.ToArray()));
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(ICSSoft.STORMNET.DataObject dobject)
        {
            LoadObject(dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">имя представления объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject)
        {
            LoadObject(dataObjectViewName, dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject)
        {
            LoadObject(dataObjectView, dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        public virtual void LoadObject(
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject)
        {
            LoadObject(dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">наименование представления.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        public virtual void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject)
        {
            LoadObject(dataObjectViewName, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        public virtual void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject)
        {
            LoadObject(dataObjectView, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        //-----------------------------------------------------

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="dataobjects">исходные объекты.</param>
        /// <param name="dataObjectView">представлене.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        public virtual void LoadObjects(ICSSoft.STORMNET.DataObject[] dataobjects,
            ICSSoft.STORMNET.View dataObjectView, bool clearDataObject)
        {
            LoadObjects(dataobjects, dataObjectView, clearDataObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>результат запроса.</returns>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct)
        {
            return LoadObjects(customizationStruct, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="State">Состояние вычитки( для последующей дочитки ).</param>
        /// <returns></returns>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct,
            ref object State)
        {
            return LoadObjects(customizationStruct, ref State, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="State">Состояние вычитки( для последующей дочитки).</param>
        /// <returns></returns>
        public virtual ICSSoft.STORMNET.DataObject[] LoadObjects(ref object State)
        {
            return LoadObjects(ref State, new DataObjectCache());
        }

        //-------LOAD separated string Objetcs ------------------------------------
        public virtual void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject)
        {
            UpdateObject(ref dobject, false);
        }

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        public virtual void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, bool alwaysThrowException)
        {
            UpdateObject(ref dobject, new DataObjectCache(), alwaysThrowException);
        }

        /// <summary>
        /// Создание копии экземпляра сервиса данных.
        /// </summary>
        /// <returns>Копии экземпляра сервиса данных.</returns>
        public virtual object Clone()
        {
            var instance = (SQLDataService)Activator.CreateInstance(GetType());
            instance._doNotChangeCustomizationString = _doNotChangeCustomizationString;
            instance.customizationString = customizationString;
            instance.DoNotChangeCustomizationString = DoNotChangeCustomizationString;
            instance.fchangeViewForTypeDelegate = fchangeViewForTypeDelegate;
            instance.m_iCommandTimeout = m_iCommandTimeout;
            instance.fldTypeUsage = fldTypeUsage;
            instance.m_bUseCommandTimeout = m_bUseCommandTimeout;
            instance.mustNewgenerate = mustNewgenerate;
            instance.prvInstanceId = prvInstanceId;
            instance.prvStorageType = prvStorageType;
            instance.prvTypesByKeys = prvTypesByKeys;

            if (AfterGenerateSQLSelectQueryStatic != null)
            {
                instance.AfterGenerateSQLSelectQuery = (AfterGenerateSQLSelectQueryEventHandler)AfterGenerateSQLSelectQueryStatic.Clone();
            }

            if (AfterUpdateObjects != null)
            {
                instance.AfterUpdateObjects = (AfterUpdateObjectsEventHandler)AfterUpdateObjects.Clone();
            }

            if (BeforeUpdateObjects != null)
            {
                instance.BeforeUpdateObjects = (BeforeUpdateObjectsEventHandler)BeforeUpdateObjects.Clone();
            }

            if (OnCreateCommand != null)
            {
                instance.OnCreateCommand = (OnCreateCommandEventHandler)OnCreateCommand.Clone();
            }

            if (OnGenerateSQLSelect != null)
            {
                instance.OnGenerateSQLSelect = (OnGenerateSQLSelectEventHandler)OnGenerateSQLSelect.Clone();
            }

            return instance;
        }

        /// <summary>
        /// Выражение DataServiceExpression содержит только метаинформацию для контроля прав на атрибуты.
        /// </summary>
        /// <param name="expression">Выражение DataServiceExpression.</param>
        /// <returns>True/False.</returns>
        public bool IsExpressionContainAttrubuteCheckOnly(string expression)
        {
            return !string.IsNullOrEmpty(expression)
                   && Regex.IsMatch(
                       expression.Trim(),
                       string.Format(@"^{0}$", SecurityManager.AttributeCheckExpressionPattern));
        }
    }

    /// <summary>
    /// Исключительная ситуация, при выполнении запроса.
    /// </summary>
    [Serializable]
    public class ExecutingQueryException : Exception, System.Runtime.Serialization.ISerializable
    {
        /// <summary>
        /// Запрос при котором возникла ошибка.
        /// </summary>
        public string curQuery;

        /// <summary>
        /// Выполненные предыдущие запросы (в этой же транзакции).
        /// </summary>
        public string prevQueries;

        /// <summary>
        ///
        /// </summary>
        /// <param name="cq">Запрос при котором возникла ошибка.</param>
        /// <param name="pq"> Выполненные предыдущие запросы (в этой же транзакции).</param>
        /// <param name="inner"></param>
        public ExecutingQueryException(string cq, string pq, Exception inner)
            : base((string.IsNullOrEmpty(cq) && string.IsNullOrEmpty(pq)) ? "Executing query exception" : ((string.IsNullOrEmpty(cq) ? "Executing query exception" : ("Error on executing:" + Environment.NewLine + cq)) + (string.IsNullOrEmpty(pq) ? string.Empty : (Environment.NewLine + "Previous queries:" + Environment.NewLine + pq))), inner)
        {
            curQuery = cq;
            prevQueries = pq;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ExecutingQueryException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            curQuery = info.GetString("cur");
            prevQueries = info.GetString("prev");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("cur", this.curQuery);
            info.AddValue("prev", this.prevQueries);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
