namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
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
        /// Тип хранилища
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
        /// Преобразовать значение в SQL строку
        /// </summary>
        /// <param name="function">Функция</param>
        /// <param name="convertValue">делегат для преобразования констант</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов</param>
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
        /// Вернуть объект <see cref="System.Data.IDbConnection"/>
        /// </summary>
        /// <returns>Коннекция к БД</returns>
        public abstract System.Data.IDbConnection GetConnection();

        ////-----------------------------------------------------

        private string customizationString;
        private string _customizationStringName;

        /// <summary>
        /// Настроичная строка (строка соединения)
        /// </summary>
        public string CustomizationString
        {
            get { return customizationString; } set { customizationString = value; }
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
        /// Свойство для установки строки соединения по имени
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
        /// Делегат для смены строки соединения
        /// </summary>
        public static ChangeCustomizationStringDelegate ChangeCustomizationString = null;

        private bool _doNotChangeCustomizationString = false;

        /// <summary>
        /// Не менять строку соединения общим делегатом ChangeCustomizationString
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
        /// Возвращает количество объектов удовлетворяющих запросу
        /// </summary>
        /// <param name="customizationStruct">
        /// Что выбираем
        /// </param>
        /// <returns>
        /// </returns>
        public virtual int GetObjectsCount(LoadingCustomizationStruct customizationStruct)
        {
            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                string cs = ChangeCustomizationString(customizationStruct.LoadingTypes);
                customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
            }

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
        /// Этот параметр не соответствует <code>lcs.ReturnTop</code>, а устанавливает максимальное число
        /// искомых объектов, тогда как <code>lcs.ReturnTop</code> ограничивает число объектов, в которых
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

            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                string cs = ChangeCustomizationString(lcs.LoadingTypes);
                customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
            }

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
                innerQuery = innerQuery.Insert(fromInd, "," + nl + "row_number() over (" + orderByExpr + ") as \"RowNumber\"" + nl);
            }
            else
            {
                innerQuery = innerQuery.Insert(fromInd, "," + nl + "row_number() over (ORDER BY " + PutIdentifierIntoBrackets("STORMMainObjectKey") + " ) as \"RowNumber\"" + nl);
            }

            string query = string.Format(
                "SELECT{3} \"RowNumber\", {5} FROM {1}({0}) QueryForGettingIndex {1} WHERE ({2}) {4}",
                innerQuery,
                nl,
                LimitFunction2SQLWhere(limitFunction),
                maxResults.HasValue ? (" TOP " + maxResults) : string.Empty,
                orderByExpr,
                PutIdentifierIntoBrackets("STORMMainObjectKey"));

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

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        /// <param name="ClearDataObject">очищать ли объект</param>
        /// <param name="CheckExistingObject">проверять ли существование объекта в хранилище</param>
        virtual public void LoadObject(
            ICSSoft.STORMNET.DataObject dobject, bool ClearDataObject, bool CheckExistingObject, DataObjectCache DataObjectCache)
        {
            LoadObject(new STORMDO.View(dobject.GetType(), STORMDO.View.ReadType.OnlyThatObject), dobject, ClearDataObject, CheckExistingObject, DataObjectCache);
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectViewName">наименование представления</param>
        /// <param name="dobject">бъект данных, который требуется загрузить</param>
        /// <param name="ClearDataObject">очищать ли объект</param>
        /// <param name="CheckExistingObject">проверять ли существование объекта в хранилище</param>
        virtual public void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool ClearDataObject, bool CheckExistingObject, DataObjectCache DataObjectCache)
        {
            LoadObject(STORMDO.Information.GetView(dataObjectViewName, dobject.GetType()), dobject, ClearDataObject, CheckExistingObject, DataObjectCache);
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectView">представление объекта</param>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        virtual public void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, DataObjectCache DataObjectCache)
        {
            LoadObject(dataObjectView, dobject, true, true, DataObjectCache);
        }

        private ChangeViewForTypeDelegate fchangeViewForTypeDelegate = null;

        private void AppendLog(string s)
        {
            string ts = System.Configuration.ConfigurationSettings.AppSettings["LogFile"];
            if (ts != null && ts != string.Empty)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ts, true);
                sw.WriteLine(s);
                sw.Close();
            }
        }

        protected void prv_AddMasterObjectsToCache(DataObject dataobject, System.Collections.ArrayList arrl, DataObjectCache DataObjectCache)
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

                        DataObjectCache.AddDataObject(master);

                        prv_AddMasterObjectsToCache(master, arrl, DataObjectCache);
                    }
                }
            }
        }

        /// <summary>
        /// Загрузка объекта с указанной коннекцией в рамках указанной транзакции.
        /// </summary>
        /// <param name="dataObjectView">Представление, по которому будет зачитываться объект.</param>
        /// <param name="dobject">Объект, который будет дочитываться/зачитываться.</param>
        /// <param name="сlearDataObject">Следует ли при зачитке очистить поля существующего объекта данных.</param>
        /// <param name="сheckExistingObject">Проверить существовние встречающихся при зачитке объектов.</param>
        /// <param name="dataObjectCache">Кэш объектов.</param>
        /// <param name="connection">Коннекция, через которую будет происходить зачитка.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет проходить зачитка.</param>
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
                dataObjectCache.AddDataObject(dobject);

                if (сlearDataObject)
                {
                    dobject.Clear();
                }
                else
                {
                    prv_AddMasterObjectsToCache(dobject, new ArrayList(), dataObjectCache);
                }

                Type dataObjectType = dobject.GetType();

                var lcs = new LoadingCustomizationStruct(GetInstanceId());

                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang =
                    FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                var variable = new FunctionalLanguage.VariableDef(
                    lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(dataObjectType).KeyType),
                    FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.StormMainObjectKey);
                object readingkey = dobject.__PrimaryKey;
                object prevPrimaryKey = null;
                if (dobject.Prototyped)
                {
                    readingkey = dobject.__PrototypeKey;
                    prevPrimaryKey = dobject.__PrimaryKey;
                }

                FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, variable, readingkey);

                lcs.Init(new ColumnsSortDef[0], func, new Type[] { dataObjectType }, dataObjectView, new string[0]);

                // Cтроим запрос.
                StorageStructForView[] storageStruct;

                // Применим полномочия на строки. НАЧАЛО.
                object limitObject;
                bool canAccess;
                var operationResult = SecurityManager.GetLimitForAccess(lcs.View.DefineClassType, tTypeAccess.Read, out limitObject, out canAccess);
                STORMFunction limit = limitObject as STORMFunction;
                if (operationResult == OperationResult.Успешно)
                {
                    if (limit != null)
                    {
                        lcs.LimitFunction = lang.GetFunction(lang.funcAND, lcs.LimitFunction, limit);
                    }
                }
                else
                {
                    // TODO: тут надо подумать что будем делать. Наверное надо вызывать исключение и не давать ничего. Пока просто запишем в лог и не будем показывать ошибку.
                    LogService.LogError(string.Format("SecurityManager.GetLimitForAccess: {0}", operationResult));
                }

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
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectView">представление</param>
        /// <param name="dobject">бъект данных, который требуется загрузить</param>
        /// <param name="ClearDataObject">очищать ли объект</param>
        /// <param name="CheckExistingObject">проверять ли существование объекта в хранилище</param>
        virtual public void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, bool ClearDataObject, bool CheckExistingObject, DataObjectCache DataObjectCache)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException("dataObjectView", "Не указано представление для загрузки объекта. Обратитесь к разработчику.");
            }

            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                string cs = ChangeCustomizationString(new Type[] { dobject.GetType() });
                customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
            }

            // if (dobject.GetStatus(false)==ObjectStatus.Created && !dobject.Prototyped) return;
            DataObjectCache.StartCaching(false);
            try
            {
                DataObjectCache.AddDataObject(dobject);

                if (ClearDataObject)
                {
                    dobject.Clear();
                }
                else
                {
                    prv_AddMasterObjectsToCache(dobject, new System.Collections.ArrayList(), DataObjectCache);
                }

                System.Type doType = dobject.GetType();

                LoadingCustomizationStruct lc = new LoadingCustomizationStruct(GetInstanceId());

                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                FunctionalLanguage.VariableDef var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                    lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(doType).KeyType), "STORMMainObjectKey");
                object readingkey = dobject.__PrimaryKey;
                object prevPrimaryKey = null;
                if (dobject.Prototyped)
                {
                    readingkey = dobject.__PrototypeKey;
                    prevPrimaryKey = dobject.__PrimaryKey;
                }

                FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, readingkey);

                // ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef fd = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                lc.Init(new ColumnsSortDef[0], func, new Type[] { doType }, dataObjectView, new string[0]);

                // Применим полномочия на строки: НАЧАЛО.
                object limitObject;
                bool canAccess;
                var operationResult = SecurityManager.GetLimitForAccess(lc.View.DefineClassType, tTypeAccess.Read, out limitObject, out canAccess);
                STORMFunction limit = limitObject as STORMFunction;
                if (operationResult == OperationResult.Успешно)
                {
                    if (limit != null)
                    {
                        ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef ldef =
                            ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                        lc.LimitFunction = ldef.GetFunction(ldef.funcAND, lc.LimitFunction, limit);
                    }
                }
                else
                {
                    // TODO: тут надо подумать что будем делать. Наверное надо вызывать исключение и не давать ничего. Пока просто запишем в лог и не будем показывать ошибку.
                    LogService.LogError(string.Format("SecurityManager.GetLimitForAccess: {0}", operationResult));
                }

                // Применим полномочия на строки: КОНЕЦ.

                // строим запрос
                STORMDO.Business.StorageStructForView[] StorageStruct; // = STORMDO.Information.GetStorageStructForView(dataObjectView,doType);
                string Query = GenerateSQLSelect(lc, false, out StorageStruct, false);

                // получаем данные
                object state = null;
                object[][] resValue = ReadFirst(Query, ref state, 0);
                if (resValue == null)
                {
                    if (CheckExistingObject)
                    {
                        throw new CantFindDataObjectException(doType, dobject.__PrimaryKey);
                    }
                    else
                    {
                        return;
                    }
                }

                ICSSoft.STORMNET.DataObject[] rrr = new ICSSoft.STORMNET.DataObject[] { dobject };
                Utils.ProcessingRowsetDataRef(resValue, new Type[] { doType }, StorageStruct, lc, rrr, this, Types, ClearDataObject, DataObjectCache, SecurityManager);
                if (dobject.Prototyped)
                {
                    dobject.SetStatus(ObjectStatus.Created);
                    dobject.SetLoadingState(LoadingState.NotLoaded);
                    dobject.__PrimaryKey = prevPrimaryKey;
                }
            }
            finally
            {
                DataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Метод для дочитки объекта данных. Загруженные ранее свойства не затираются, изменённые свойства не затираются. Подменяются поштучно свойства копии данных. TODO: дописать тесты, проверить и сделать публичным
        /// </summary>
        /// <param name="dataObjectView">представление</param>
        /// <param name="dataObject">бъект данных, который требуется загрузить</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище</param>
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
        /// <param name="dataObjectView">представление</param>
        /// <param name="dataObject">бъект данных, который требуется загрузить</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище</param>
        /// <param name="dataObjectCache"></param>
        /// <param name="dataObjectFromDB"></param>
        protected virtual void PrvSecondLoadObject(
            View dataObjectView,
            DataObject dataObject, bool checkExistingObject, DataObjectCache dataObjectCache, DataObject dataObjectFromDB)
        {
            if (dataObjectView == null)
            {
                throw new ArgumentNullException("dataObjectView", "Не указано представление для дозагрузки объекта. Обратитесь к разработчику.");
            }

            if (dataObject == null)
            {
                throw new ArgumentNullException("dataObject", "Не указан объект для дозагрузки. Обратитесь к разработчику.");
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
                if (name.IndexOf(".") > -1)
                {
                    int indexOfDot = name.IndexOf('.');
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
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectViewName">имя представления объекта</param>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        virtual public void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, DataObjectCache DataObjectCache)
        {
            LoadObject(STORMDO.Information.GetView(dataObjectViewName, dobject.GetType()), dobject, true, true, DataObjectCache);
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        virtual public void LoadObject(ICSSoft.STORMNET.DataObject dobject, DataObjectCache DataObjectCache)
        {
            LoadObject(new STORMDO.View(dobject.GetType(), STORMDO.View.ReadType.OnlyThatObject), dobject, true, true, DataObjectCache);
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
                                sPrefix = var.Substring(0, var.LastIndexOf("."));
                                new_var = var.Substring(var.LastIndexOf(".") + 1);

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
                if (par is ICSSoft.STORMNET.FunctionalLanguage.VariableDef)
                {
                    string n = (par as ICSSoft.STORMNET.FunctionalLanguage.VariableDef).StringedView;
                    if (n != null && n.ToLower() == "stormmainobjectkey")
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
                            sPrefix = n.Substring(0, n.LastIndexOf("."));
                            new_var = n.Substring(n.LastIndexOf(".") + 1);

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
        /// получить запрос на вычитку данных
        /// </summary>
        /// <param name="customizationStruct">настройка выборки</param>
        /// <param name="StorageStruct">возвращается соответствующая структура выборки</param>
        /// <returns>запрос</returns>
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
                    for (int i = 0; i < dataObjectView.Properties.Length; i++)
                    {
                        if (!Information.IsStoredProperty(dataObjectView.DefineClassType, dataObjectView.Properties[i].Name))
                        {
                            string[] arr1 = Information.AllViews(dataObjectView.DefineClassType);
                            for (int ii = 0; ii < arr1.Length; ii++)
                            {
                                View v1 = Information.GetView(arr1[ii], dataObjectView.DefineClassType);
                                if (v1.CheckPropname(dataObjectView.Properties[i].Name))
                                {
                                    for (int j = 0; j < v1.Properties.Length; j++)
                                    {
                                        dataObjectView.AddProperty(v1.Properties[j].Name, v1.Properties[j].Name, dataObjectView.Properties[i].Name.Equals(v1.Properties[j].Name), string.Empty);
                                    }

                                    break;
                                }
                            }
                        }
                    }
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
                    for (int i = 0; i < StorageStruct[0].props.Length; i++)
                    {
                        asnameprop[i] = StorageStruct[0].props[i].Name;
                        if (StorageStruct[0].props[i].storage[0][0] != null) // не вычислимое св-во
                        {
                            StorageStruct[0].props[i].Name = StorageStruct[0].props[i].source.Name + "0." +
                                StorageStruct[0].props[i].storage[0][0];
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
                    resQuery += "TOP " + customizationStruct.ReturnTop.ToString() + " ";
                }

                string resStart = resQuery;

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
                    props.Add(PutIdentifierIntoBrackets("STORMMainObjectKey"));
                    for (int i = 0; i < MaxCountKeys; i++)
                    {
                        if (StorageType == StorageTypeEnum.HierarchicalStorage)
                        {
                            props.Add(PutIdentifierIntoBrackets("STORMJoinedMasterType" + i.ToString()));
                        }

                        props.Add(PutIdentifierIntoBrackets("STORMJoinedMasterKey" + i.ToString()));
                    }

                    props.Add(PutIdentifierIntoBrackets("STORMNETDATAOBJECTTYPE"));
                }
                #endregion

                string colsPart = Query.Substring(Query.IndexOf(System.Text.RegularExpressions.Regex.Match(Query,
                    @"([.]*(\""\w*\b\""))* as " + PutIdentifierIntoBrackets("STORMMainObjectKey")).Value));
                if (mustNewgenerate)
                {
                    Query = "SELECT ";
                    if (customizationStruct.Distinct /*&& ForReadValues*/)
                    {
                        Query += "DISTINCT ";
                    }

                    if (customizationStruct.ReturnTop > 0)
                    {
                        Query += "TOP " + customizationStruct.ReturnTop.ToString() + " ";
                    }
                }

                for (int i = 0; i < props.Count; i++)
                {
                    if (mustNewgenerate)
                    {
                        string selectF = string.Empty;
                        for (int j = 0; j < StorageStruct[0].props.Length; j++)
                        {
                            if (PutIdentifierIntoBrackets(asnameprop[j]) == props[i])
                            {
                                if (StorageStruct[0].props[j].MastersTypesCount > 0)
                                {
                                    string[] names = new string[StorageStruct[0].props[j].storage.Length];
                                    for (int jj = 0; jj < StorageStruct[0].props[j].storage.Length; jj++)
                                    {
                                        string[] namesM = new string[StorageStruct[0].props[j].MastersTypes[jj].Length];
                                        for (int k = 0; k < StorageStruct[0].props[j].MastersTypes[jj].Length; k++)
                                        {
                                            string curName = PutIdentifierIntoBrackets(StorageStruct[0].props[j].source.Name + jj.ToString()) + "." +
                                                PutIdentifierIntoBrackets(StorageStruct[0].props[j].storage[jj][k]);
                                            namesM[k] = curName;
                                        }

                                        names[jj] = this.GetIfNullExpression(namesM);
                                    }

                                    altnameprop[j] = this.GetIfNullExpression(names);
                                    selectF = altnameprop[j] + " as " + props[i];
                                }
                                else if (StorageStruct[0].props[j].storage[0][0] != null) // не вычислимое св-во
                                {
                                    altnameprop[j] = PutIdentifierIntoBrackets(StorageStruct[0].props[j].source.Name + "0") + "." +
                                        PutIdentifierIntoBrackets(StorageStruct[0].props[j].storage[0][0]);
                                    selectF = altnameprop[j] + " as " + props[i];
                                    break;
                                }
                                else
                                {
                                    bool PointExist = false;
                                    altnameprop[j] = "NULL";
                                    if (StorageStruct[0].props[j].Expression != null)
                                    {
                                        altnameprop[j] = TranslateExpression(StorageStruct[0].props[j].Expression, string.Empty,
                                            PutIdentifierIntoBrackets(StorageStruct[0].props[j].source.Name + "0") + ".", out PointExist);
                                    }

                                    selectF = altnameprop[j] + " as " + props[i];
                                    break;
                                }
                            }
                        }

                        if (selectF != string.Empty)
                        {
                            Query += ((i > 0) ? "," : string.Empty) + nl + selectF;
                        }
                    }
                    else
                    {
                        resQuery += ((i > 0) ? "," : string.Empty) + nl + props[i];
                    }
                }

                // colsPart = resQuery;

                // добавим From-часть
                resQuery += nl + "FROM (" + nl + Query + nl + ") " + PutIdentifierIntoBrackets("STORMGENERATEDQUERY");

                if (customizationStruct.AdvansedColumns != null)
                {
                    for (int j = 0; j < customizationStruct.AdvansedColumns.Length; j++)
                    {
                        AdvansedColumn ac = customizationStruct.AdvansedColumns[j];
                        if (ac.StorageSourceModification != null && ac.StorageSourceModification != string.Empty)
                        {
                            resQuery += nl + "\t" + ac.StorageSourceModification;
                        }
                    }
                }

                if (mustNewgenerate)
                {
                    for (int i = 0; i < StorageStruct[0].props.Length; i++)
                    {
                        if (StorageStruct[0].props[i].storage[0][0] != null) // не вычислимое св-во
                        {
                            Query = System.Text.RegularExpressions.Regex.Replace(Query,
                                StorageStruct[0].props[i].source.Name + "0." + StorageStruct[0].props[i].storage[0][0],
                                asnameprop[i]);
                        }
                    }

                    Query = System.Text.RegularExpressions.Regex.Replace(Query,
                        @"\""?STORMMAINObjectKey\""?",
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.Name + "0") + "." +
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.storage[0].PrimaryKeyStorageName),
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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

                        bool notfirst = false;
                        for (int i = 0; i < sorts.Length; i++)
                        {
                            if (mustNewgenerate)
                            {
                                if (notfirst)
                                {
                                    resQuery += ", ";
                                    orderByExpr += ", ";
                                }
                                else
                                {
                                    resQuery += nl + "ORDER BY ";
                                    orderByExpr += nl + "ORDER BY ";
                                }

                                string addStr = sorts[i].Name + AscDesc[(int)sorts[i].Sort];
                                resQuery += addStr;
                                orderByExpr += addStr;
                                notfirst = true;
                            }
                            else if (props.Contains(PutIdentifierIntoBrackets(sorts[i].Name)))
                            {
                                if (notfirst)
                                {
                                    resQuery += ", ";
                                    orderByExpr += ", ";
                                }
                                else
                                {
                                    resQuery += nl + "ORDER BY ";
                                    orderByExpr += nl + "ORDER BY ";
                                }

                                sorts[i].Name = PutIdentifierIntoBrackets(sorts[i].Name);
                                string addStr = sorts[i].Name + AscDesc[(int)sorts[i].Sort];
                                resQuery += addStr;
                                orderByExpr += addStr;
                                notfirst = true;
                            }
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
                    resQuery = resQuery.Insert(fromInd, "," + nl + "row_number() over (" + orderByExpr + ") as \"RowNumber\"" + nl);
                }
                else
                {
                    resQuery = resQuery.Insert(fromInd, "," + nl + "row_number() over (ORDER BY \"STORMMainObjectKey\") as \"RowNumber\"" + nl);
                }

                resQuery = селектСамогоВерхнегоУр + nl + "FROM (" + nl + resQuery + ") rn" + nl + "where \"RowNumber\" between " + customizationStruct.RowNumber.StartRow.ToString() + " and " + customizationStruct.RowNumber.EndRow.ToString() + nl +
                    orderByExpr;
            }

            // поддержка Row_number() Братчиков 07.03.2009
        }

        /// <summary>
        /// получить запрос на вычитку данных
        /// </summary>
        /// <param name="customizationStruct">настройка выборки</param>
        /// <returns>запрос</returns>
        public virtual string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool Optimized)
        {
            STORMDO.Business.StorageStructForView[] storageStructs;
            return GenerateSQLSelect(customizationStruct, false, out storageStructs, Optimized);
        }

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/></param>
        /// <returns></returns>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct, DataObjectCache DataObjectCache)
        {
            object state = null;
            ICSSoft.STORMNET.DataObject[] res = LoadObjects(customizationStruct, ref state, DataObjectCache);
            return res;
        }

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="dataobjects">исходные объекты</param>
        /// <param name="dataObjectView">представлене</param>
        /// <param name="ClearDataobject">очищать ли существующие</param>
        virtual public void LoadObjects(ICSSoft.STORMNET.DataObject[] dataobjects,
            ICSSoft.STORMNET.View dataObjectView, bool ClearDataobject, DataObjectCache DataObjectCache)
        {
            if (dataobjects == null || dataobjects.Length == 0)
            {
                return;
            }

            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                System.Collections.Generic.List<Type> tps = new System.Collections.Generic.List<Type>();
                foreach (DataObject d in dataobjects)
                {
                    Type t = d.GetType();
                    if (!tps.Contains(t))
                    {
                        tps.Add(t);
                    }
                }

                string cs = ChangeCustomizationString(tps.ToArray());
                customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
            }

            DataObjectCache.StartCaching(false);
            try
            {
                System.Collections.ArrayList ALtypes = new System.Collections.ArrayList();
                System.Collections.ArrayList ALKeys = new System.Collections.ArrayList();
                System.Collections.SortedList ALobjectsKeys = new System.Collections.SortedList();
                System.Collections.SortedList readingKeys = new System.Collections.SortedList();
                for (int i = 0; i < dataobjects.Length; i++)
                {
                    DataObject dobject = dataobjects[i];
                    Type dotype = dobject.GetType();
                    bool addobj = false;
                    if (ALtypes.Contains(dotype))
                    {
                        addobj = true;
                    }
                    else
                    {
                        if ((dotype == dataObjectView.DefineClassType || dotype.IsSubclassOf(dataObjectView.DefineClassType)) && Information.IsStoredType(dotype))
                        {
                            ALtypes.Add(dotype);
                            addobj = true;
                        }
                    }

                    if (addobj)
                    {
                        object readingKey = dobject.Prototyped ? dobject.__PrototypeKey : dobject.__PrimaryKey;
                        ALKeys.Add(readingKey);
                        ALobjectsKeys.Add(dotype.FullName + readingKey.ToString(), i);
                        readingKeys.Add(readingKey.ToString(), dobject.__PrimaryKey);
                    }
                }

                LoadingCustomizationStruct customizationStruct = new LoadingCustomizationStruct(GetInstanceId());

                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                FunctionalLanguage.VariableDef var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                    lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(dataObjectView.DefineClassType).KeyType), "STORMMainObjectKey");
                object[] keys = new object[ALKeys.Count + 1];
                ALKeys.CopyTo(keys, 1);
                keys[0] = var;
                FunctionalLanguage.Function func = lang.GetFunction(lang.funcIN, keys);
                Type[] types = new Type[ALtypes.Count];
                ALtypes.CopyTo(types);

                customizationStruct.Init(null, func, types, dataObjectView, null);

                STORMDO.Business.StorageStructForView[] StorageStruct;

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                string SelectString = string.Empty;
                SelectString = GenerateSQLSelect(customizationStruct, false, out StorageStruct, false);

                // получаем данные
                object State = null;

                object[][] resValue = (SelectString == string.Empty) ? new object[0][] : ReadFirst(
                    SelectString,
                    ref State, 0);
                if (resValue != null && resValue.Length != 0)
                {
                    DataObject[] loadobjects = new ICSSoft.STORMNET.DataObject[resValue.Length];
                    int ObjectTypeIndexPOs = resValue[0].Length - 1;
                    int keyIndex = StorageStruct[0].props.Length - 1;
                    while (StorageStruct[0].props[keyIndex].MultipleProp)
                    {
                        keyIndex--;
                    }

                    keyIndex++;

                    for (int i = 0; i < resValue.Length; i++)
                    {
                        Type tp = types[Convert.ToInt64(resValue[i][ObjectTypeIndexPOs].ToString())];
                        object ky = resValue[i][keyIndex];
                        ky = Information.TranslateValueToPrimaryKeyType(tp, ky);
                        int indexobj = ALobjectsKeys.IndexOfKey(tp.FullName + ky.ToString());
                        if (indexobj > -1)
                        {
                            loadobjects[i] = dataobjects[(int)ALobjectsKeys.GetByIndex(indexobj)];
                            if (ClearDataobject)
                            {
                                loadobjects[i].Clear();
                            }

                            DataObjectCache.AddDataObject(loadobjects[i]);
                        }
                        else
                        {
                            loadobjects[i] = null;
                        }
                    }

                    Utils.ProcessingRowsetDataRef(resValue, types, StorageStruct, customizationStruct, loadobjects, this, Types, ClearDataobject, DataObjectCache, SecurityManager);
                    foreach (DataObject dobj in loadobjects)
                    {
                        if (dobj != null && dobj.Prototyped)
                        {
                            dobj.__PrimaryKey = readingKeys[dobj.__PrimaryKey.ToString()];
                            dobj.SetStatus(ObjectStatus.Created);
                            dobj.SetLoadingState(LoadingState.NotLoaded);
                        }
                    }
                }
            }
            finally
            {
                DataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Загрузка объектов данных по представлению
        /// </summary>
        /// <param name="dataObjectView">представление</param>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View dataObjectView)
        {
            LoadingCustomizationStruct lc = new LoadingCustomizationStruct(GetInstanceId());
            lc.View = dataObjectView;
            lc.LoadingTypes = new[] { dataObjectView.DefineClassType };
            return LoadObjects(lc, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных по массиву представлений
        /// </summary>
        /// <param name="dataObjectViews">массив представлений</param>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
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
        /// Загрузка объектов данных по массиву структур
        /// </summary>
        /// <param name="customizationStructs">массив структур</param>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
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
        /// <param name="state"> </param>
        /// <param name="dataObjectCache">Кэш объектов для зачитки.</param>
        /// <param name="connection">Коннекция, через которую будут выполнена зачитска.</param>
        /// <param name="transaction">Транзакция, в рамках которой будет выполнена зачитка.</param>
        /// <returns>Загруженные данные.</returns>
        public virtual DataObject[] LoadObjectsByExtConn(
            LoadingCustomizationStruct customizationStruct,
            ref object state, // TODO: разобраться, что это за параметр.
            DataObjectCache dataObjectCache,
            IDbConnection connection,
            IDbTransaction transaction)
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
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/></param>
        /// <param name="State">Состояние вычитки( для последующей дочитки )</param>
        /// <returns></returns>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct,
            ref object State, DataObjectCache DataObjectCache)
        {
            DataObjectCache.StartCaching(false);
            try
            {
                System.Type[] dataObjectType = customizationStruct.LoadingTypes;
                if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
                {
                    string cs = ChangeCustomizationString(dataObjectType);
                    customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
                }

                // Применим полномочия на строки.
                ApplyReadPermissions(customizationStruct, SecurityManager);

                STORMDO.Business.StorageStructForView[] StorageStruct;

                string SelectString = string.Empty;
                SelectString = GenerateSQLSelect(customizationStruct, false, out StorageStruct, false);

                // получаем данные
                object[][] resValue = ReadFirst(
                    SelectString,
                    ref State, customizationStruct.LoadingBufferSize);
                State = new object[] { State, dataObjectType, StorageStruct, customizationStruct, customizationString };
                ICSSoft.STORMNET.DataObject[] res = null;
                if (resValue == null)
                {
                    res = new DataObject[0];
                }
                else
                {
                    res = Utils.ProcessingRowsetData(resValue, dataObjectType, StorageStruct, customizationStruct, this, Types, DataObjectCache, SecurityManager);
                }

                return res;
            }
            finally
            {
                DataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Загрузка объектов данных по представлению
        /// </summary>
        /// <param name="dataObjectView">представление</param>
        /// <param name="changeViewForTypeDelegate">делегат</param>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                this.fchangeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(dataObjectView);
        }

        /// <summary>
        /// Загрузка объектов данных по массиву представлений
        /// </summary>
        /// <param name="dataObjectViews">массив представлений</param>
        /// <param name="changeViewForTypeDelegate">делегат</param>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View[] dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                this.fchangeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(dataObjectViews);
        }

        /// <summary>
        /// Загрузка объектов данных по массиву структур
        /// </summary>
        /// <param name="customizationStructs">массив структур</param>
        /// <param name="changeViewForTypeDelegate">делегат</param>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct[] customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                this.fchangeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(customizationStructs);
        }

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="State">Состояние вычитки( для последующей дочитки)</param>
        /// <returns></returns>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(ref object State, DataObjectCache DataObjectCache)
        {
            if (State == null)
            {
                return new DataObject[0];
            }

            DataObjectCache.StartCaching(false);
            try
            {
                // получаем данные
                object[] stateArr = (object[])State;
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
                        res = Utils.ProcessingRowsetData(resValue, (System.Type[])stateArr[1], (STORMNET.Business.StorageStructForView[])stateArr[2], (LoadingCustomizationStruct)stateArr[3], this, Types, DataObjectCache, SecurityManager);
                    }
                }

                DataObjectCache.StopCaching();
                return res;
            }
            finally
            {
                DataObjectCache.StopCaching();
            }
        }

        public virtual object[][] ReadFirstByExtConn(string Query, ref object State, int LoadingBufferSize, System.Data.IDbConnection Connection, System.Data.IDbTransaction Transaction)
        {
            object taskid = BusinessTaskMonitor.BeginTask("Reading data" + Environment.NewLine + Query);
            try
            {
                System.Data.IDbCommand myCommand = Connection.CreateCommand();
                myCommand.CommandText = Query;
                myCommand.Transaction = Transaction;
                CustomizeCommand(myCommand);

                // Connection.Open();
                System.Data.IDataReader myReader = myCommand.ExecuteReader();
                object[] state = new object[] { Connection, myReader };
                State = state;
                return ReadNextByExtConn(ref State, LoadingBufferSize);
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
        /// <param name="query"></param>
        /// <param name="state"></param>
        /// <param name="loadingBufferSize"></param>
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
                if (reader != null)
                {
                    reader.Close();
                }

                if (connection != null)
                {
                    connection.Close();
                }

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
                System.Data.IDbConnection myConnection = (System.Data.IDbConnection)((object[])State)[0];
                myReader.Close();

                // myConnection.Close();
                State = null;
                return null;
            }
        }

        /// <summary>
        /// Вычитка следующей порции данных
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
        /// Выполнить запрос
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <returns>количество задетых строк</returns>
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
        /// получить LeftJoin выражение
        /// </summary>
        /// <param name="subTable">имя таблицы</param>
        /// <param name="subTableAlias">псевдоним таблицы</param>
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
            string joinCondition = !string.IsNullOrEmpty(parentAliasWithKey) && parentAliasWithKey.Contains(".") ?
                string.Concat(parentAliasWithKey, " = ", subTableKeyField)
                : string.Format("{0} = {1}", subTableKeyField, parentAliasWithKey);

            FromPart = string.Concat(nl, " LEFT JOIN ", subTable, " ", PutIdentifierIntoBrackets(subTableAlias),
                GetJoinTableModifierExpression(),
                subJoins,
                nl, " ON ", joinCondition);
            WherePart = string.Empty;
        }

        /// <summary>
        /// получить InnerJoin выражение
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
            string joinCondition = !string.IsNullOrEmpty(parentAliasWithKey) && parentAliasWithKey.Contains(".") ?
                string.Concat(parentAliasWithKey, " = ", subTableKeyField)
                : string.Format("{0} = {1}", subTableKeyField, parentAliasWithKey);

            FromPart = string.Concat(nl, " INNER JOIN ", subTable, " ", PutIdentifierIntoBrackets(subTableAlias),
                GetJoinTableModifierExpression(),
                subJoins,
                nl, " ON ", parentAliasWithKey, " = ", joinCondition);
            WherePart = string.Empty;
        }

        /// <summary>
        /// Вернуть модификатор для обращения к таблице (напр WITH (NOLOCK))
        /// Можно перегрузить этот метод в сервисе данных-наследнике
        /// для возврата соответствующего своего модификатора.
        /// Базовый <see cref="SQLDataService"/> возвращает пустую строку.
        /// </summary>
        /// <returns>""</returns>
        public virtual string GetJoinTableModifierExpression()
        {
            return string.Empty;
        }

        /// <summary>
        /// Вернуть in выражение для where
        /// </summary>
        /// <param name="identifiers">идентифткаторы</param>
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
        /// Вернуть ifnull выражение
        /// </summary>
        /// <param name="identifiers">идентифткаторы</param>
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
        /// Офромить идентификатор
        /// </summary>
        /// <param name="identifier">идентификатор</param>
        /// <returns>оформленный идентификатор(например в кавычках)</returns>
        public virtual string PutIdentifierIntoBrackets(string identifier)
        {
            return string.Concat("\"", identifier, "\"");
        }

        /// <summary>
        /// создать join соединения
        /// </summary>
        /// <param name="source">источник с которого формируется соединение</param>
        /// <param name="parentAlias">вышестоящий алиас</param>
        /// <param name="index">индекс источника</param>
        /// <param name="keysandtypes">ключи и типы</param>
        /// <param name="baseOutline">смещение в запросе</param>
        /// <param name="joinscount">количество соединений</param>
        /// <returns></returns>
        public virtual void CreateJoins(STORMDO.Business.StorageStructForView.PropSource source,
            string parentAlias, int index,
            System.Collections.ArrayList keysandtypes,
            string baseOutline, out int joinscount,
            out string FromPart, out string WherePart)
        {
            string nl = Environment.NewLine + baseOutline;
            string newOutLine = baseOutline + "\t";
            joinscount = 0;
            FromPart = string.Empty;
            WherePart = string.Empty;
            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    if (subSource.storage[j].parentStorageindex == index)
                    {
                        joinscount++;
                        string curAlias = subSource.Name + j.ToString();
                        keysandtypes.Add(
                            new string[]
                            {
                                        PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(subSource.storage[j].PrimaryKeyStorageName),
                                        PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(subSource.storage[j].TypeStorageName),
                                            subSource.Name
                                    });
                        string Link = PutIdentifierIntoBrackets(parentAlias) + "." + PutIdentifierIntoBrackets(subSource.storage[j].objectLinkStorageName); // +"_M"+(locindex++).ToString());
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

                        if (subSource.storage[j].nullableLink)
                        {
                            GetLeftJoinExpression(GenString("(", subjoinscount) + " " + PutIdentifierIntoBrackets(subSource.storage[j].Storage), curAlias, Link, subSource.storage[j].PrimaryKeyStorageName, subjoin, baseOutline, out FromStr, out WhereStr);
                        }
                        else
                        {
                            GetInnerJoinExpression(GenString("(", subjoinscount) + " " + PutIdentifierIntoBrackets(subSource.storage[j].Storage), curAlias, Link, subSource.storage[j].PrimaryKeyStorageName, subjoin, baseOutline, out FromStr, out WhereStr);
                        }

                        FromPart += FromStr + ")";
                    }
                }
            }
        }

        /// <summary>
        /// создать join соединения
        /// </summary>
        /// <param name="source">источник с которого формируется соединение</param>
        /// <param name="parentAlias">вышестоящий алиас</param>
        /// <param name="index">индекс источника</param>
        /// <param name="keysandtypes">ключи и типы</param>
        /// <param name="baseOutline">смещение в запросе</param>
        /// <param name="joinscount">количество соединений</param>
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

            string nl = Environment.NewLine + baseOutline;
            string newOutLine = baseOutline + "\t";
            joinscount = 0;
            FromPart = string.Empty;
            WherePart = string.Empty;
            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    if (subSource.storage[j].parentStorageindex == index)
                    {
                        joinscount++;
                        string curAlias = subSource.Name + j.ToString();
                        keysandtypes.Add(
                            new string[]
                            {
                                            PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(subSource.storage[j].PrimaryKeyStorageName),
                                            PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(subSource.storage[j].TypeStorageName),
                                            subSource.Name
                                        });
                        string Link = PutIdentifierIntoBrackets(parentAlias) + "." + PutIdentifierIntoBrackets(subSource.storage[j].objectLinkStorageName); // +"_M"+(locindex++).ToString());
                        string subjoin = string.Empty;
                        string temp;
                        int subjoinscount = 0;
                        string FromStr, WhereStr;

                        CreateJoins(subSource, curAlias, j, keysandtypes, newOutLine, out subjoinscount, out subjoin, out temp, MustNewGenerate);
                        if (subSource.storage[j].nullableLink)
                        {
                            GetLeftJoinExpression(PutIdentifierIntoBrackets(subSource.storage[j].Storage), curAlias, Link, subSource.storage[j].PrimaryKeyStorageName, string.Empty, baseOutline, out FromStr, out WhereStr);
                        }
                        else
                        {
                            GetInnerJoinExpression(PutIdentifierIntoBrackets(subSource.storage[j].Storage), curAlias, Link, subSource.storage[j].PrimaryKeyStorageName, string.Empty, baseOutline, out FromStr, out WhereStr);
                        }

                        FromPart += FromStr;
                        if (subjoin != string.Empty)
                        {
                            FromPart += subjoin;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// преобразовать выражение с учетом
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="namespacewithpoint"></param>
        /// <returns></returns>
        virtual public string TranslateExpression(string expression, string namespacewithpoint, string exteranlnamewithpoint, out bool PointExistInSourceIdentifier)
        {
            string[] expressarr = expression.Split('@');
            string result = string.Empty;
            int nextIndex = 1;
            PointExistInSourceIdentifier = false;
            for (int i = 0; i < expressarr.Length; i++)
            {
                if (i != nextIndex)
                {
                    result += expressarr[i];
                }
                else
                {
                    if (expressarr[nextIndex] == string.Empty)
                    {
                        result += "@";
                        nextIndex++;
                    }

                    // Обработка псевдонимов полей вида [@имяТега] для поддержки представления результата запроса в виде XML.
                    else if (Regex.IsMatch(expressarr[nextIndex], @"^\w+\]"))
                    {
                        result += "@" + expressarr[i];
                        nextIndex++;
                    }
                    else
                    {
                        if (!PointExistInSourceIdentifier && expressarr[nextIndex].IndexOf(".") >= 0)
                        {
                            PointExistInSourceIdentifier = true;
                        }

                        if (namespacewithpoint != string.Empty)
                        {
                            result += exteranlnamewithpoint + PutIdentifierIntoBrackets(namespacewithpoint + expressarr[nextIndex]);
                        }

                        string st1 = exteranlnamewithpoint.Trim('.', '"');
                        if ((st1.IndexOf(".") == -1) && (expressarr[nextIndex].IndexOf(".") > 0))
                        {
                            st1 = st1.Substring(0, st1.Length - 1);
                            string st2 = string.Empty;
                            string st3 = string.Empty;
                            if (expressarr[nextIndex].LastIndexOf(".") != expressarr[nextIndex].IndexOf("."))
                            {
                                st3 = expressarr[nextIndex].Substring(expressarr[nextIndex].LastIndexOf(".") + 1);
                                st2 = expressarr[nextIndex].Substring(0, expressarr[nextIndex].LastIndexOf(".")).Replace(".", string.Empty);
                            }

                            result += PutIdentifierIntoBrackets(st1 + st2 + "0") + "." + PutIdentifierIntoBrackets(st3);
                        }
                        else if (namespacewithpoint == string.Empty)
                        {
                            result += exteranlnamewithpoint + PutIdentifierIntoBrackets(expressarr[nextIndex]);
                        }

                        nextIndex += 2;
                    }
                }
            }

            return "(" + result + ")";
        }

        virtual public string GetConvertToTypeExpression(Type valType, string value)
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
        ///     fromjoins
        /// </summary>
        /// <param name="storageStruct">структура хранилища</param>
        /// <param name="AddingAdvansedField">довленные дополнительные свойства</param>
        /// <param name="AddingKeysCount">добавленниые ключи</param>
        /// <param name="addMasterFieldsCustomizer"></param>
        /// <param name="addNotMainKeys"></param>
        /// <param name="SelectTypesIds"></param>
        /// <returns></returns>
        virtual public string GenerateSQLSelectByStorageStruct(STORMDO.Business.StorageStructForView storageStruct, bool addNotMainKeys, bool addMasterFieldsCustomizer, string AddingAdvansedField, int AddingKeysCount, bool SelectTypesIds)
        {
            return GenerateSQLSelectByStorageStruct(storageStruct, addNotMainKeys, addMasterFieldsCustomizer, AddingAdvansedField, AddingKeysCount, SelectTypesIds, mustNewgenerate, true);
        }

        virtual public string GenerateSQLSelectByStorageStruct(STORMDO.Business.StorageStructForView storageStruct, bool addNotMainKeys, bool addMasterFieldsCustomizer, string AddingAdvansedField, int AddingKeysCount, bool SelectTypesIds, bool MustNewGenerate, bool MustDopSelect)
        {
            string nl = Environment.NewLine;
            string nlk = "," + nl;
            string selectPartFields = string.Empty;
            string superSelectPartFileds = string.Empty;

            bool firstSelectPartFields = true;

            System.Collections.Specialized.StringCollection SelectMasterFields = new System.Collections.Specialized.StringCollection();
            bool HasExpresions = false;

            var mainKeyByNamespace = new Dictionary<string, string>();

            for (int i = 0; i < storageStruct.props.Length; i++)
            {
                STORMDO.Business.StorageStructForView.PropStorage prop = storageStruct.props[i];
                if (prop.MultipleProp)
                {
                    continue;
                }

                string brackedIdent = PutIdentifierIntoBrackets(prop.Name);
                if (!firstSelectPartFields)
                {
                    selectPartFields += nlk;
                    if (!prop.AdditionalProp)
                    {
                        superSelectPartFileds += nlk;
                    }
                }

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
                                string curName = isAccessDenied ? deniedAccessValue : PutIdentifierIntoBrackets(prop.source.Name + j.ToString()) + "." + PutIdentifierIntoBrackets(prop.storage[j][k]);
                                namesM[k] = curName;
                                SelectMasterFields.Add(curName);
                            }

                            names[j] = this.GetIfNullExpression(namesM);
                        }

                        string field = isAccessDenied ? deniedAccessValue : this.GetIfNullExpression(names);

                        selectPartFields += field + " as " + brackedIdent;
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds += brackedIdent;
                        }
                    }
                    else
                    {
                        string[] names = new string[prop.storage.Length];
                        for (int j = 0; j < prop.storage.Length; j++)
                        {
                            names[j] = PutIdentifierIntoBrackets(prop.source.Name + j.ToString()) + "." + PutIdentifierIntoBrackets(prop.storage[j][0]);
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

                        selectPartFields += field + " as " + brackedIdent;
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds += brackedIdent;
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
                                && translatedExpression.ToLower().Contains("stormmainobjectkey"))
                            {
                                string mainObjectKeyReplace = "{" + namespacewithpoint + "STORMMainObjectKey}";

                                if (!mainKeyByNamespace.ContainsKey(prop.source.Name))
                                {
                                    mainKeyByNamespace.Add(prop.source.Name, mainObjectKeyReplace);
                                }

                                var regex = new Regex("\"?STORMMainObjectKey\"?", RegexOptions.IgnoreCase);
                                translatedExpression = regex.Replace(translatedExpression, mainObjectKeyReplace);
                            }
                        }

                        selectPartFields += "NULL as " + brackedIdent;
                        HasExpresions = true;
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds += translatedExpression;
                        }
                    }
                    else
                    {
                        selectPartFields += "NULL as " + brackedIdent;
                        if (!prop.AdditionalProp)
                        {
                            superSelectPartFileds += brackedIdent;
                        }
                    }
                }

                firstSelectPartFields = false;
            }

            string MainKeyBracked = PutIdentifierIntoBrackets("STORMMainObjectKey");
            string MainKey = PutIdentifierIntoBrackets(storageStruct.sources.Name + "0") + "." + PutIdentifierIntoBrackets(storageStruct.sources.storage[0].PrimaryKeyStorageName) + " as " + MainKeyBracked;

            string selectKeyFields = string.Empty;
            string superSelectKeyFields = string.Empty;

            string MainStor = storageStruct.sources.storage[0].Storage;
            string fromstring =
                PutIdentifierIntoBrackets(MainStor) + " " + PutIdentifierIntoBrackets(storageStruct.sources.Name + "0") + " " + GetJoinTableModifierExpression();
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
                    if (wherestring != string.Empty)
                    {
                        wherestring += " and ";
                    }

                    wherestring += wherepar;
                }
            }

            selectKeyFields = MainKey;
            superSelectKeyFields = MainKeyBracked;

            if (addNotMainKeys)
            {
                for (int i = 0; i < keysandtypes.Count; i++)
                {
                    string[] keyandtype = (string[])keysandtypes[i];

                    string MasterKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterKey" + i.ToString());
                    selectKeyFields += nlk + keyandtype[0] + " as " + MasterKeyBracked;
                    superSelectKeyFields += nlk + MasterKeyBracked;

                    if (SelectTypesIds)
                    {
                        string TypeKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterType" + i.ToString());
                        selectKeyFields += nlk + keyandtype[1] + " as " + TypeKeyBracked;
                        superSelectKeyFields += nlk + TypeKeyBracked;
                    }

                    string replace;
                    if (mainKeyByNamespace.TryGetValue(keyandtype[2], out replace))
                    {
                        superSelectPartFileds =
                            superSelectPartFileds.Replace(replace, MasterKeyBracked);
                    }
                }
            }

            int keyIndex = keysandtypes.Count;
            if (addMasterFieldsCustomizer && (SelectMasterFields.Count > 0))
            {
                for (int i = 0; i < SelectMasterFields.Count; i++)
                {
                    string MasterKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterKey" + keyIndex.ToString());
                    selectKeyFields += nlk + SelectMasterFields[i] + " as " + MasterKeyBracked;
                    superSelectKeyFields += nlk + MasterKeyBracked;
                    keyIndex++;
                }
            }

            for (int i = 0; i < AddingKeysCount; i++)
            {
                string MasterKeyBracked = PutIdentifierIntoBrackets("STORMJoinedMasterKey" + keyIndex.ToString());

                if (selectKeyFields != string.Empty)
                {
                    selectKeyFields += ", ";
                }

                selectKeyFields += GetConvertToTypeExpression(typeof(Guid), "null") + " as " + MasterKeyBracked;
                if (superSelectKeyFields != string.Empty)
                {
                    superSelectKeyFields += ", ";
                }

                superSelectKeyFields += MasterKeyBracked;

                keyIndex++;
            }

            if (AddingAdvansedField != string.Empty)
            {
                if (selectKeyFields != string.Empty)
                {
                    selectKeyFields += ", ";
                }

                selectKeyFields += AddingAdvansedField;
                if (superSelectKeyFields != string.Empty)
                {
                    superSelectKeyFields += ", ";
                }

                superSelectKeyFields += AddingAdvansedField;
            }

            string MainSelect =
                "SELECT " + nl
                + ((selectPartFields == string.Empty) ? string.Empty : selectPartFields + "," + nl)
                + selectKeyFields + nl
                + "FROM " + nl
                + fromstring;
            if (wherestring != string.Empty)
            {
                MainSelect += nl + "WHERE " + nl + wherestring;
            }

            if (HasExpresions && MustDopSelect)
            {
                MainSelect =
                    "SELECT " + nl
                    + superSelectPartFileds + nlk
                    + superSelectKeyFields + nl
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

            System.Type valType = value.GetType();
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
        /// конвертация значений в строки запроса
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
        /// Преобразование значение свойства в строку для запроса
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
                string[] names = new string[StorageStruct[0].props[i].storage.Length];
                if (StorageStruct[0].props[i].MastersTypesCount > 0)
                {
                    for (int jj = 0; jj < names.Length; jj++)
                    {
                        string[] namesM = new string[StorageStruct[0].props[i].MastersTypes[jj].Length];
                        for (int k = 0; k < namesM.Length; k++)
                        {
                            string curName = PutIdentifierIntoBrackets(StorageStruct[0].props[i].source.Name + jj.ToString()) + "." +
                                PutIdentifierIntoBrackets(StorageStruct[0].props[i].storage[jj][k]);
                            namesM[k] = curName;
                        }

                        names[jj] = this.GetIfNullExpression(namesM);
                    }

                    rexst = this.GetIfNullExpression(names);

                    // doprst = GetValueForLimitParam(LimitFunction, asname);
                }
                else if (StorageStruct[0].props[i].storage[0][0] != null)
                {
                    for (int jj = 0; jj < names.Length; jj++)
                    {
                        names[jj] = PutIdentifierIntoBrackets(StorageStruct[0].props[i].source.Name + jj.ToString()) + "." +
                                PutIdentifierIntoBrackets(StorageStruct[0].props[i].storage[jj][0]);
                    }

                    rexst = this.GetIfNullExpression(names);
                }
                else if (StorageStruct[0].props[i].Expression != null)
                {
                    bool PointExist = false;
                    rexst = TranslateExpression(StorageStruct[0].props[i].Expression, string.Empty,
                        PutIdentifierIntoBrackets(StorageStruct[0].props[i].source.Name + "0") + ".", out PointExist);
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
        /// Преобразование функции
        /// </summary>
        /// <param name="LimitFunction"></param>
        /// <returns></returns>
        public virtual string LimitFunction2SQLWhere(STORMFunction LimitFunction,
            STORMDO.Business.StorageStructForView[] StorageStruct, string[] asnameprop, bool MustNewGenerate)
        {
            string sw =
                ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(LimitFunction,
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString(ConvertValueToQueryValueString),
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets(PutIdentifierIntoBrackets));
            if (MustNewGenerate)
            {
                sw = ReplaceAliases(StorageStruct, asnameprop, sw);

                // не применять для существуют такие и им подобных..
                bool bExistsFounded = CheckExists(LimitFunction);
                ;
                if (!bExistsFounded)
                {
                    sw = System.Text.RegularExpressions.Regex.Replace(sw,
                        PutIdentifierIntoBrackets("STORMMainObjectKey"),
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.Name + "0") + "." +
                        PutIdentifierIntoBrackets(StorageStruct[0].sources.storage[0].PrimaryKeyStorageName),
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }

                string sPK = PutIdentifierIntoBrackets("STORMGENERATEDQUERY") + "." + PutIdentifierIntoBrackets("STORMMainObjectKey");
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
        /// Преобразование функции
        /// </summary>
        /// <param name="LimitFunction"></param>
        /// <returns></returns>
        public virtual string LimitFunction2SQLWhere(STORMFunction LimitFunction)
        {
            return
                ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(LimitFunction,
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString(ConvertValueToQueryValueString),
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets(PutIdentifierIntoBrackets));
        }

        //-------LOAD separated string Objetcs ------------------------------------

        /// <summary>
        /// Загрузка без создания объектов
        /// </summary>
        /// <param name="separator">разделитель в строках</param>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/></param>
        /// <returns>массив структур <see cref="ObjectStringDataView"/></returns>
        virtual public ObjectStringDataView[] LoadStringedObjectView(
            char separator,
            LoadingCustomizationStruct customizationStruct)
        {
            object state = null;
            ObjectStringDataView[] res = LoadStringedObjectView(separator, customizationStruct, ref state);
            return res;
        }

        /// <summary>
        /// Загрузка без создания объектов
        /// </summary>
        /// <param name="separator">разделитель в строках</param>
        /// <param name="customizationStruct"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        virtual public ObjectStringDataView[] LoadStringedObjectView(
            char separator,
            LoadingCustomizationStruct customizationStruct,
            ref object State)
        {
            object ID = BusinessTaskMonitor.BeginTask("Load objects");
            try
            {
                System.Type[] dataObjectType = customizationStruct.LoadingTypes;
                if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
                {
                    string cs = ChangeCustomizationString(dataObjectType);
                    customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
                }

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
                ;
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
        /// Загрузка без создания объектов
        /// </summary>
        /// <param name="customizationStruct"></param>
        /// <returns></returns>
        virtual public object[][] LoadRawValues(LoadingCustomizationStruct customizationStruct)
        {
            object state = null;
            object ID = BusinessTaskMonitor.BeginTask("Load raw values");
            try
            {
                Type[] dataObjectType = customizationStruct.LoadingTypes;
                if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
                {
                    string cs = ChangeCustomizationString(dataObjectType);
                    customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
                }

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
                                    customizationStruct.View.Properties[0]
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

        virtual public ObjectStringDataView[] LoadValues(
            char separator,
            LoadingCustomizationStruct customizationStruct)
        {
            object ID = BusinessTaskMonitor.BeginTask("Load objects");
            try
            {
                System.Type[] dataObjectType = customizationStruct.LoadingTypes;
                if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
                {
                    string cs = ChangeCustomizationString(dataObjectType);
                    customizationString = string.IsNullOrEmpty(cs) ? customizationString : cs;
                }

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
                ;
                ObjectStringDataView[] result = null;
                if (resValue == null)
                {
                    result = new ObjectStringDataView[0];
                }
                else
                {
                    result = Utils.ProcessingRowSet2StringedView(resValue, true, dataObjectType, PropCount, separator, customizationStruct, StorageStruct, this, Types, ref procRead, new DataObjectCache(), SecurityManager);
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
        ///
        /// </summary>
        virtual public ObjectStringDataView[] LoadStringedObjectView(ref object state)
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

        virtual public void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, DataObjectCache DataObjectCache)
        {
            UpdateObject(ref dobject, DataObjectCache, false);
        }

        /// <summary>
        /// Обновление объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить</param>
        virtual public void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, DataObjectCache DataObjectCache, bool AlwaysThrowException)
        {
            STORMDO.DataObject[] arr = new STORMDO.DataObject[] { dobject };
            UpdateObjects(ref arr, DataObjectCache, AlwaysThrowException);
            if (arr != null && arr.Length > 0)
            {
                dobject = arr[0];
            }
            else
            {
                dobject = null;
            }
        }

        virtual public void UpdateObject(ICSSoft.STORMNET.DataObject dobject)
        {
            UpdateObject(dobject, false);
        }

        /// <summary>
        /// Обновление объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить</param>
        virtual public void UpdateObject(ICSSoft.STORMNET.DataObject dobject, DataObjectCache DataObjectCache)
        {
            UpdateObject(ref dobject, DataObjectCache);
        }

        /// <summary>
        /// Обновление объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить</param>
        virtual public void UpdateObject(ICSSoft.STORMNET.DataObject dobject, bool AlwaysThrowException)
        {
            UpdateObject(ref dobject, new DataObjectCache(), AlwaysThrowException);
        }

        /// <summary>
        /// Возвращает измененные данные со значениями
        /// </summary>
        /// <param name="dobject">у кого проверяем</param>
        /// <param name="CheckLoadedProps">проверять ли загруженность измененных свойств</param>
        /// <param name="propsWithValues">пары свойство-значение</param>
        /// <param name="detailObjects">вычисленные измененные объекты</param>
        /// <param name="ReturnPropStorageNames">возвращать ли не сами свойства а их хранилища</param>
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
                                realpropname = PutIdentifierIntoBrackets(propstor + "_m" + i.ToString());
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

        private void AddOpertaionOnTable(StringCollection OpTables, SortedList TableOperations, string Table, OperationType op)
        {
            OpTables.Add(Table);
            if (TableOperations.ContainsKey(Table))
            {
                OperationType ot = (OperationType)TableOperations[Table];
                ot = ot | op;
                TableOperations[Table] = ot;
            }
            else
            {
                TableOperations.Add(Table, op);
            }
        }

        /// <summary>
        /// Удаляемые объекты особым образом добавляются в словарь
        /// </summary>
        /// <param name="dobject">
        /// Удаляемый объект
        /// </param>
        /// <param name="UpdaterFunction">
        /// Функция обновления
        /// </param>
        /// <param name="DeleteList">
        /// Соответствие между таблицей и первичными ключами удаляемых объектов
        /// </param>
        /// <param name="DeleteTables">
        /// The delete tables.
        /// </param>
        /// <param name="TableOperations">
        /// The table operations.
        /// </param>
        private void AddDeletedObjectToDeleteDictionary(
            STORMDO.DataObject dobject,
            FunctionalLanguage.Function UpdaterFunction,
            System.Collections.SortedList DeleteList,
            StringCollection DeleteTables,
            SortedList TableOperations)
        {
            System.Type[] dots = (StorageType == StorageTypeEnum.HierarchicalStorage)
                ? Information.GetCompatibleTypesForTypeConvertion(dobject.GetType())
                : new Type[] { dobject.GetType() };
            for (int i = 0; i < dots.Length; i++)
            {
                string tableName = Information.GetClassStorageName(dots[i]);
                string prkeyStorName = Information.GetPrimaryKeyStorageName(dots[i]);
                if (!DeleteList.ContainsKey(tableName))
                {
                    FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;

                    FunctionalLanguage.VariableDef var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                        lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(dobject.GetType()).KeyType), prkeyStorName);
                    FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, dobject.__PrimaryKey);

                    if (UpdaterFunction != null)
                    {
                        func = UpdaterFunction;
                    }

                    DeleteList.Add(tableName, func);
                    AddOpertaionOnTable(DeleteTables, TableOperations, tableName, OperationType.Delete);
                }
                else
                {
                    FunctionalLanguage.Function func = (FunctionalLanguage.Function)DeleteList[tableName];
                    FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                    if (func.FunctionDef.StringedView == lang.funcEQ)
                    {
                        func = lang.GetFunction(lang.funcIN, func.Parameters[0], func.Parameters[1]);
                        DeleteList[tableName] = func;
                    }

                    func.Parameters.Add(dobject.__PrimaryKey);
                }
            }
        }

        /// <summary>
        /// У основного представления есть связь на представление на детейлы. Часть из них вообще не загружалась, данная функция обрабатывает как раз их.
        /// Данная функция либо возвращает объекты детейлов, если есть навешенные на них бизнес-сервера,
        /// иначе формирует запрос на удаление всех детейлов определённого типа у объекта
        /// </summary>
        /// <param name="view">
        /// Представление, соответствующее детейлу
        /// </param>
        /// <param name="DeleteDictionary">
        /// The delete dictionary.
        /// </param>
        /// <param name="mainkey">
        /// Первичный ключ агрегатора детейлов
        /// </param>
        /// <param name="DeleteOrder">
        /// The delete order.
        /// </param>
        /// <param name="updateobjects">
        /// Детейлы, на которые навешены бизнес-сервера
        /// (соответственно, их массово удалить нельзя, необходимо каждый пропустить через бизнес-сервер)
        /// </param>
        /// <param name="DeleteTables">
        /// The delete tables.
        /// </param>
        /// <param name="TableOperations">
        /// The table operations.
        /// </param>
        /// <param name="DataObjectCache">
        /// The data object cache.
        /// </param>
        /// <param name="processingObjectsKeys">
        /// Ключи обрабатываемых объектов
        /// (список содержит первичные ключи объектов, которые уже попали в список на обновление)
        /// </param>
        /// <returns>
        /// Набор объектов, которые необходимо занести в аудит
        /// </returns>
        private IEnumerable<DataObject> AddDeletedViewToDeleteDictionary(
            STORMDO.View view,
            ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary DeleteDictionary,
            object mainkey,
            out DataObject[] updateobjects,
            StringCollection DeleteTables,
            SortedList TableOperations,
            DataObjectCache DataObjectCache)
        {
            List<DataObject> extraProcessingObjects = new List<DataObject>();
            updateobjects = new DataObject[0];
            string tableName = Information.GetClassStorageName(view.DefineClassType);
            string prkeyStorName = view.Properties[1].Name;
            string prevDicValue = string.Empty;

            FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;

            FunctionalLanguage.VariableDef var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(view.DefineClassType).KeyType), prkeyStorName);
            FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, mainkey);

            LoadingCustomizationStruct cs = new LoadingCustomizationStruct(GetInstanceId());

            cs.Init(new ColumnsSortDef[0], func, new Type[] { view.DefineClassType }, view, new string[0]);
            string sq = GenerateSQLSelect(cs, false);

            if (sq != string.Empty)
            {
                BusinessServer[] bs = BusinessServerProvider.GetBusinessServer(view.DefineClassType, DataServiceObjectEvents.OnDeleteFromStorage, this);
                if (bs != null && bs.Length > 0)
                { // Если на детейловые объекты навешены бизнес-сервера, то тогда детейлы будут подгружены
                    updateobjects = LoadObjects(cs, DataObjectCache);
                }
                else
                {
                    if (AuditService.IsTypeAuditable(view.DefineClassType))
                    { /* Аудиту необходимо зафиксировать удаление детейлов.
                       * Здесь в аудит идут уже актуальные детейлы, поскольку на них нет бизнес-серверов,
                       * а бизнес-сервера основного объекта уже выполнились.
                       */
                        DataObject[] detailObjects = LoadObjects(cs);
                        if (detailObjects != null)
                        {
                            foreach (var detailObject in detailObjects)
                            {// Мы будем сии детейлы удалять, поэтому им необходимо проставить соответствующий статус
                                detailObject.SetStatus(ObjectStatus.Deleted);
                            }

                            extraProcessingObjects.AddRange(detailObjects.ToList());
                        }
                    }

                    if (StorageType == StorageTypeEnum.HierarchicalStorage)
                    {
                        Type[] types = Information.GetCompatibleTypesForTypeConvertion(view.DefineClassType);
                        for (int i = 0; i < types.Length; i++)
                        {
                            tableName = Information.GetClassStorageName(types[i]);
                            prevDicValue = string.Empty;
                            if (!DeleteDictionary.ContainsKey(tableName))
                            {
                                DeleteDictionary.Add(tableName, string.Empty);
                                AddOpertaionOnTable(DeleteTables, TableOperations, tableName, OperationType.Delete);
                            }
                            else
                            {
                                prevDicValue = DeleteDictionary[tableName] + " OR ";
                            }

                            string selectQuery = " IN ( SELECT " + PutIdentifierIntoBrackets("STORMMainObjectKey") + " FROM (" + sq + " ) a )";
                            DeleteDictionary[tableName] = prevDicValue + PutIdentifierIntoBrackets(Information.GetPrimaryKeyStorageName(view.DefineClassType)) + selectQuery;
                        }
                    }
                    else
                    {
                        string selectQuery = PutIdentifierIntoBrackets(Information.GetPrimaryKeyStorageName(view.DefineClassType)) + " IN ( SELECT " + PutIdentifierIntoBrackets("STORMMainObjectKey") + " FROM (" + sq + " ) a )";
                        if (!DeleteDictionary.ContainsKey(tableName))
                        {
                            DeleteDictionary.Add(tableName, string.Empty);
                            AddOpertaionOnTable(DeleteTables, TableOperations, tableName, OperationType.Delete);
                            DeleteDictionary[tableName] = prevDicValue + selectQuery;
                        }
                        else
                        {
                            prevDicValue = DeleteDictionary[tableName];
                            if (prevDicValue.IndexOf(selectQuery) < 0)
                            {
                                int index0 = prevDicValue.LastIndexOf((char)0);
                                if (prevDicValue.Length - index0 > 5000)
                                {
                                    prevDicValue = DeleteDictionary[tableName] + ((char)0).ToString();
                                }
                                else
                                {
                                    prevDicValue = DeleteDictionary[tableName] + " OR ";
                                }

                                DeleteDictionary[tableName] = prevDicValue + selectQuery;
                            }
                        }
                    }
                }
            }

            return extraProcessingObjects;
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
            Insert = 4
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
        /// <param name="deleteQueries"> Запросы для удаление </param>
        /// <param name="deleteTables"> The Delete Tables. </param>
        /// <param name="updateQueries"> Запросы для изменения </param>
        /// <param name="updateFirstQueries"> Запросы для изменения, выполняемые до остальных запросов </param>
        /// <param name="updateTables"> The Update Tables. </param>
        /// <param name="insertQueries"> Запросы для добавления </param>
        /// <param name="insertTables"> The Insert Tables. </param>
        /// <param name="tableOperations"> The Table Operations. </param>
        /// <param name="queryOrder"> The Query Order. </param>
        /// <param name="checkLoadedProps"> Проверять ли загруженность свойств </param>
        /// <param name="processingObjects"> The processing Objects. </param>
        /// <param name="dataObjectCache"> The Data Object Cache.</param>
        /// <param name="dobjects"> Для чего генерим запросы </param>
        public virtual void GenerateQueriesForUpdateObjects(
            StringCollection deleteQueries,
            StringCollection deleteTables,
            StringCollection updateQueries,
            StringCollection updateFirstQueries,
            StringCollection updateTables,
            StringCollection insertQueries,
            StringCollection insertTables,
            SortedList tableOperations,
            StringCollection queryOrder,
            bool checkLoadedProps,
            System.Collections.ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            params ICSSoft.STORMNET.DataObject[] dobjects)
        {
            GenerateQueriesForUpdateObjects(
                deleteQueries,
                deleteTables,
                updateQueries,
                updateFirstQueries,
                updateTables,
                insertQueries,
                insertTables,
                tableOperations,
                queryOrder,
                checkLoadedProps,
                processingObjects,
                dataObjectCache,
                null,
                dobjects);
        }

        /// <summary>
        /// Провести аудит операции для одного объекта.
        /// </summary>
        /// <param name="dobject"> Объект, аудит которого нужно провести. </param>
        /// <param name="auditOperationInfoList"> Список id записей аудита. </param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе AuditService иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// </param>
        private void AuditOperation(DataObject dobject, ICollection<AuditAdditionalInfo> auditOperationInfoList, IDbTransaction transaction)
        {
            if (dobject != null && AuditService.IsAuditEnabled)
            {
                AuditAdditionalInfo auditAdditionalInfo =
                            AuditService.WriteCommonAuditOperationWithAutoFields(dobject, this, true, transaction); // Если что, то исключение будет проброшено
                if (auditAdditionalInfo != null && auditAdditionalInfo.AuditRecordPrimaryKey != Guid.Empty)
                {
                    auditOperationInfoList.Add(auditAdditionalInfo);
                }
            }
        }

        /// <summary>
        /// Провести аудит операции для нескольких объектов.
        /// </summary>
        /// <param name="dobjects"> Объект, аудит которого нужно провести. </param>
        /// <param name="auditOperationInfoList"> Список id записей аудита. </param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе AuditService иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - null.
        /// </param>
        private void AuditOperation(IEnumerable<DataObject> dobjects, ICollection<AuditAdditionalInfo> auditOperationInfoList, IDbTransaction transaction = null)
        {
            if (dobjects != null)
            {
                foreach (var dobject in dobjects)
                {
                    AuditOperation(dobject, auditOperationInfoList, transaction);
                }
            }
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
        /// <param name="transaction">Транзакция, в которой необходимо производить чтение (необязательный параметр).</param>
        protected virtual void GenerateAuditForAggregators(
            ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            ref List<DataObject> auditObjects,
            IDbTransaction transaction = null)
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

                        if (transaction == null)
                        {
                            LoadObject(tempView, tempObject, dataObjectCache);
                        }
                        else
                        {
                            LoadObjectByExtConn(tempView, tempObject, true, false, dataObjectCache, transaction.Connection, transaction);
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
                    DefineClassType = aggregatorType
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

                    if (transaction == null)
                    {
                        LoadObject(aggregatorView, tempAggregator, true, false, dataObjectCache);
                    }
                    else
                    {
                        LoadObjectByExtConn(aggregatorView, tempAggregator, true, false, dataObjectCache, transaction.Connection, transaction);
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

                    if (currentObject != null && currentObject.GetStatus() == ObjectStatus.Deleted)
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
                    else if (currentObject != null && currentObject.GetStatus() == ObjectStatus.Deleted && currentObject.ContainsAlteredProps())
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
        /// Генерация запросов для изменения объектов
        /// (дополнительно возвращается список объектов, для которых необходимо создание записей аудита).
        /// </summary>
        /// <param name="deleteQueries">Запросы для удаление (выходной параметр).</param>
        /// <param name="deleteTables">Таблицы, из которых будет проведено удаление данных (выходной параметр).</param>
        /// <param name="updateQueries">Сгенерированные запросы для изменения (выходной параметр).</param>
        /// <param name="updateFirstQueries"> Сгенерированные запросы для изменения (выходной параметр), выполняемые до остальных запросов </param>
        /// <param name="updateTables">Таблицы, в которых будет проведено изменение данных (выходной параметр).</param>
        /// <param name="insertQueries">Сгенерированные запросы для добавления (выходной параметр).</param>
        /// <param name="insertTables">Таблицы, в которые будет проведена вставка данных (выходной параметр).</param>
        /// <param name="tableOperations">Операции, которые будут произведены над таблицами (выходной параметр).</param>
        /// <param name="queryOrder">Порядок исполнения генерируемых запросов, задаваемый именами таблиц (выходной параметр).</param>
        /// <param name="checkLoadedProps">Проверять ли загруженность свойств.</param>
        /// <param name="processingObjects">Текущие обрабатываемые объекты (то есть объекты, которые данный сервис данных планирует подтвердить в БД
        ///  в текущей транзакции). Выходной параметр.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="auditObjects">Список объектов, которые необходимо записать в аудит (выходной параметр). Заполняется в том случае, когда
        /// передан не null и текущий сервис аудита включен.</param>
        /// <param name="dobjects">Объекты, для которых генерируются запросы.</param>
        public virtual void GenerateQueriesForUpdateObjects(
            StringCollection deleteQueries,
            StringCollection deleteTables,
            StringCollection updateQueries,
            StringCollection updateFirstQueries,
            StringCollection updateTables,
            StringCollection insertQueries,
            StringCollection insertTables,
            SortedList tableOperations,
            StringCollection queryOrder,
            bool checkLoadedProps,
            ArrayList processingObjects,
            DataObjectCache dataObjectCache,
            List<DataObject> auditObjects,
            params DataObject[] dobjects)
        {
            string nl = Environment.NewLine;
            string nlk = ",";
            var deleteList = new System.Collections.SortedList();
            var deleteDictionary = new ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary();
            var extraProcessingList = new List<DataObject>();
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
                        bs.ObjectsToUpdate = processingObjects;
                        object prevPrimaryKey = processingObject.__PrimaryKey;
                        STORMDO.DataObject[] subobjects = bs.OnUpdateDataobject(processingObject);
                        curObjectStatus = processingObject.GetStatus(true);
                        if (!processingObject.__PrimaryKey.Equals(prevPrimaryKey))
                        {
                            TypeKeyPair typeKeyPair = new TypeKeyPair(typeOfProcessingObject, prevPrimaryKey);
                            processingObjectsKeys.Remove(typeKeyPair);
                            if (processingObject.GetStatus(false) == ObjectStatus.Created)
                            {
                                KeyGen.KeyGenerator.GenerateUnique(processingObject, this);
                            }

                            AddToProcessingObjectsKeys(processingObjectsKeys, processingObject);
                        }

                        foreach (STORMDO.DataObject subobject in subobjects)
                        {
                            subobject.GetStatus(true);
                            if (!ContainsKeyINProcessing(processingObjectsKeys, subobject))
                            {
                                if (subobject.GetStatus(false) == ObjectStatus.Created)
                                {
                                    KeyGen.KeyGenerator.GenerateUnique(subobject, this);
                                }

                                processingObjects.Add(subobject);
                                AddToProcessingObjectsKeys(processingObjectsKeys, subobject);
                            }
                        }
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
                                deleteList,
                                deleteTables,
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

                            string thisTable = Information.GetClassStorageName(processingObject.GetType());
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
                                DataObject[] detailsObjects;
                                IEnumerable<DataObject> extraProcessingObjects =
                                    AddDeletedViewToDeleteDictionary(subview, deleteDictionary, processingObject.__PrimaryKey, out detailsObjects, deleteTables, tableOperations, dataObjectCache);
                                extraProcessingList.AddRange(extraProcessingObjects);

                                foreach (DataObject detobj in detailsObjects)
                                {
                                    detobj.SetStatus(STORMDO.ObjectStatus.Deleted);
                                    if (!ContainsKeyINProcessing(processingObjectsKeys, detobj))
                                    {
                                        processingObjects.Add(detobj);
                                        AddToProcessingObjectsKeys(processingObjectsKeys, detobj);
                                    }
                                }
                            }

                            break;
                        }

                    case STORMDO.ObjectStatus.Created:
                        {
                            if (AuditService.IsTypeAuditable(processingObject.GetType()))
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
                                        propsInTable.Add("__PrimaryKey");
                                        valuesByTables.Add(defType, propsInTable);
                                    }

                                    propsInTable.Add(col);
                                }

                                for (int k = 0; k < valuesByTables.Count; k++)
                                {
                                    Type t = valuesByTables.Key(k);
                                    string tableName =
                                        Information.GetClassStorageName(t);
                                    string query = "INSERT INTO " + PutIdentifierIntoBrackets(tableName) + nl;
                                    var propsInTable = (StringCollection)valuesByTables[t];
                                    string columns = propsInTable[0];
                                    string values = propsWithValues[propsInTable[0]];
                                    for (int j = 1; j < propsInTable.Count; j++)
                                    {
                                        columns += nlk + PutIdentifierIntoBrackets(propsInTable[j]);
                                        values += nlk + propsWithValues[propsInTable[j]];
                                    }

                                    query += " ( " + nl + columns + nl + " ) " + nl + " VALUES (" + nl + values + nl + ")";
                                    AddOpertaionOnTable(insertTables, tableOperations, tableName, OperationType.Insert);
                                    insertQueries.Add(query);
                                }
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

                                string mainTableName = STORMDO.Information.GetClassStorageName(typeOfProcessingObject);
                                foreach (DataObject detobj in mastersObjects)
                                {
                                    string tableName = Information.GetClassStorageName(detobj.GetType());
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

                                string[] cols = propsWithValues.GetAllKeys();
                                string query = "INSERT INTO " + PutIdentifierIntoBrackets(mainTableName) + nl;
                                string columns = cols[0];
                                string values = propsWithValues[cols[0]];
                                for (int j = 1; j < propsWithValues.Count; j++)
                                {
                                    columns += nlk + cols[j];
                                    values += nlk + propsWithValues[cols[j]];
                                }

                                query += " ( " + nl + columns + nl + " ) " + nl + " VALUES (" + nl + values + nl + ")";
                                AddOpertaionOnTable(insertTables, tableOperations, mainTableName, OperationType.Insert);
                                insertQueries.Add(query);
                            }

                            break;
                        }

                    case STORMDO.ObjectStatus.Altered:
                        {
                            if (AuditService.IsTypeAuditable(processingObject.GetType()))
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

                                if (propsWithValues.Count > 0)
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
                                        string query = "UPDATE " + PutIdentifierIntoBrackets(tableName) + " SET " + nl;

                                        string values = propsInTable[0] + " = " + propsWithValues[propsInTable[0]];
                                        for (int j = 1; j < propsInTable.Count; j++)
                                        {
                                            values += nlk + PutIdentifierIntoBrackets(propsInTable[j]) + " = " + propsWithValues[propsInTable[j]];
                                        }

                                        query += values + nl + " WHERE ";
                                        FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                                        var var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                                            lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(t).KeyType), Information.GetPrimaryKeyStorageName(t));
                                        FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, processingObject.__PrimaryKey);
                                        if (updaterobject != null)
                                        {
                                            func = updaterobject.Function;
                                        }

                                        query += LimitFunction2SQLWhere(func);
                                        AddOpertaionOnTable(updateTables, tableOperations, tableName, OperationType.Update);
                                        if (!updateQueries.Contains(query))
                                        {
                                            updateQueries.Add(query);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                GetAlteredPropsWithValues(processingObject, checkLoadedProps, out propsWithValues, out detailsObjects, out mastersObjects, true);
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

                                string mainTableName = STORMDO.Information.GetClassStorageName(typeOfProcessingObject);
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

                                if (propsWithValues.Count > 0)
                                {
                                    string query = "UPDATE " + PutIdentifierIntoBrackets(mainTableName) + " SET " + nl;
                                    string[] cols = propsWithValues.GetAllKeys();
                                    string values = cols[0] + " = " + propsWithValues[cols[0]];
                                    for (int j = 1; j < propsWithValues.Count; j++)
                                    {
                                        values += nlk + cols[j] + " = " + propsWithValues[cols[j]];
                                    }

                                    query += values + nl + " WHERE ";
                                    FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                                    var var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                                        lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(processingObject.GetType()).KeyType), Information.GetPrimaryKeyStorageName(typeOfProcessingObject));
                                    FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, processingObject.__PrimaryKey);
                                    if (updaterobject != null)
                                    {
                                        func = updaterobject.Function;
                                    }

                                    query += LimitFunction2SQLWhere(func);
                                    AddOpertaionOnTable(updateTables, tableOperations, mainTableName, OperationType.Update);
                                    if (!updateQueries.Contains(query))
                                    {
                                        updateQueries.Add(query);
                                    }
                                }
                            }

                            break;
                        }
                }
            }

            foreach (DataObject processingObject in processingObjects)
            {
                // Включем текущий объект в граф зависимостей.
                GetDependencies(processingObject, processingObject.GetType(), dependencies, extraUpdateList);
            }

            for (int i = 0; i < extraUpdateList.Count; i++)
            {
                DataObject processingObject = extraUpdateList[i];
                ICSSoft.STORMNET.Collections.CaseSensivityStringDictionary propsWithValues;
                Type typeOfProcessingObject = processingObject.GetType();
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

                    if (propsWithValues.Count > 0)
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
                            string query = "UPDATE " + PutIdentifierIntoBrackets(tableName) + " SET " + nl;

                            string values = propsInTable[0] + " = " + propsWithValues[propsInTable[0]];
                            for (int j = 1; j < propsInTable.Count; j++)
                            {
                                values += nlk + PutIdentifierIntoBrackets(propsInTable[j]) + " = " + propsWithValues[propsInTable[j]];
                            }

                            query += values + nl + " WHERE ";
                            FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                            var var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                                lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(t).KeyType), Information.GetPrimaryKeyStorageName(t));
                            FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, processingObject.__PrimaryKey);
                            if (updaterobject != null)
                            {
                                func = updaterobject.Function;
                            }

                            query += LimitFunction2SQLWhere(func);
                            AddOpertaionOnTable(updateTables, tableOperations, tableName, OperationType.Update);
                            if (!updateQueries.Contains(query))
                            {
                                updateFirstQueries.Add(query);
                            }
                        }
                    }
                }
                else
                {
                    GetAlteredPropsWithValues(processingObject, checkLoadedProps, out propsWithValues, out detailsObjects, out mastersObjects, true);

                    string mainTableName = STORMDO.Information.GetClassStorageName(typeOfProcessingObject);

                    if (propsWithValues.Count > 0)
                    {
                        string query = "UPDATE " + PutIdentifierIntoBrackets(mainTableName) + " SET " + nl;
                        string[] cols = propsWithValues.GetAllKeys();
                        string values = cols[0] + " = " + propsWithValues[cols[0]];
                        for (int j = 1; j < propsWithValues.Count; j++)
                        {
                            values += nlk + cols[j] + " = " + propsWithValues[cols[j]];
                        }

                        query += values + nl + " WHERE ";
                        FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                        var var = new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(
                            lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.Generator(processingObject.GetType()).KeyType), Information.GetPrimaryKeyStorageName(typeOfProcessingObject));
                        FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, processingObject.__PrimaryKey);
                        if (updaterobject != null)
                        {
                            func = updaterobject.Function;
                        }

                        query += LimitFunction2SQLWhere(func);
                        AddOpertaionOnTable(updateTables, tableOperations, mainTableName, OperationType.Update);
                        if (!updateQueries.Contains(query))
                        {
                            updateFirstQueries.Add(query);
                        }
                    }
                }
            }

            List<Type> depList = GetOrderFromDependencies(dependencies);
            foreach (DataObject processingObject in processingObjects)
            {
                if (depList.IndexOf(processingObject.GetType()) < 0)
                {
                    depList.Add(processingObject.GetType());
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

            deleteTables.Clear();
            if (deleteList.Count > 0)
            {
                for (int j = 0; j < queryOrder.Count; j++)
                {
                    if (deleteDictionary[queryOrder[j]] != string.Empty)
                    {
                        if (deleteList.ContainsKey(queryOrder[j]))
                        {
                            FunctionalLanguage.Function func = (STORMFunction)deleteList[queryOrder[j]];
                            string Query = "DELETE FROM " + PutIdentifierIntoBrackets(queryOrder[j]) + " WHERE " +
                                           LimitFunction2SQLWhere(func);
                            if (!deleteQueries.Contains(Query))
                            {
                                deleteTables.Add(queryOrder[j]);
                                deleteQueries.Add(Query);
                            }
                        }

                        if (deleteDictionary.ContainsKey(queryOrder[j]))
                        {
                            string[] sq = deleteDictionary[queryOrder[j]].Split((char)0);
                            foreach (string s in sq)
                            {
                                string query = "DELETE FROM " + PutIdentifierIntoBrackets(queryOrder[j]) + " WHERE " + s;
                                if (!deleteQueries.Contains(query))
                                {
                                    deleteTables.Add(queryOrder[j]);
                                    deleteQueries.Add(query);
                                }
                            }
                        }
                    }
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

        private bool m_bUseCommandTimeout = false;
        private int m_iCommandTimeout = 0;

        /// <summary>
        /// Приватное поле для <see cref="SecurityManager"/>.
        /// </summary>
        private ISecurityManager _securityManager;

        /// <summary>
        /// IDbCommand.CommandTimeout кроме установки этого таймаута не забудьте установить флаг <see cref="UseCommandTimeout"/>
        /// </summary>
        public int CommandTimeout
        {
            get { return m_iCommandTimeout; }
            set { m_iCommandTimeout = value; }
        }

        /// <summary>
        /// Использовать ли атрибут <see cref="CommandTimeout"/> (если задан через конфиг, то будет true) по-умолчанию false
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

        protected virtual Exception RunCommands(StringCollection queries, StringCollection tables,
            string table, System.Data.IDbCommand command,
            object businessID, bool AlwaysThrowException)
        {
            int i = 0;
            bool res = true;
            Exception ex = null;
            while (i < queries.Count)
            {
                if (tables[i] == table)
                {
                    string query = queries[i];
                    command.CommandText = query;
                    command.Parameters.Clear();
                    CustomizeCommand(command);
                    object subTask = BusinessTaskMonitor.BeginSubTask(query, businessID);
                    try
                    {
                        command.ExecuteNonQuery();
                        queries.RemoveAt(i);
                        tables.RemoveAt(i);
                    }
                    catch (Exception exc)
                    {
                        i++;
                        res = false;
                        ex = new ExecutingQueryException(query, string.Empty, exc);
                        if (AlwaysThrowException)
                        {
                            BusinessTaskMonitor.EndSubTask(subTask);
                            throw ex;
                        }
                    }

                    BusinessTaskMonitor.EndSubTask(subTask);
                }
                else
                {
                    i++;
                }
            }

            if (!res)
            {
                if (AlwaysThrowException)
                {
                    throw ex;
                }

                return ex;
            }
            else
            {
                return null;
            }
        }

        protected OperationType Minus(OperationType ops, OperationType value)
        {
            return ops & (~value);
        }

        protected virtual System.Data.IDbTransaction CreateTransaction(System.Data.IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Обновить объекты данных в указанном порядке
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <param name="alwaysThrowException">
        /// Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию. По умолчанию true;
        /// </param>
        virtual public void UpdateObjectsOrdered(ref DataObject[] objects, bool alwaysThrowException = true)
        {
            IDbConnection connection = GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            var dataObjectCache = new DataObjectCache();
            try
            {
                foreach (DataObject dataObject in objects)
                {
                    DataObject[] dObjs = new[] { dataObject };
                    UpdateObjectsByExtConn(ref dObjs, dataObjectCache, alwaysThrowException, connection, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Обновить хранилище по объектам (есть параметр, указывающий, всегда ли необходимо взводить ошибку
        /// и откатывать транзакцию при неудачном запросе в базу данных). Если
        /// он true, всегда взводится ошибка. Иначе, выполнение продолжается.
        /// Однако, при этом есть опасность преждевременного окончания транзакции, с переходом для остальных
        /// запросов режима транзакционности в autocommit. Проявлением проблемы являются ошибки навроде:
        /// The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION
        /// TODO: Объединить код с UpdateObjects.
        /// </summary>
        /// <param name="objects">Объекты для обновления.</param>
        /// <param name="dataObjectCache">Кеш объектов.</param>
        /// <param name="alwaysThrowException">Если произошла ошибка в базе данных, не пытаться выполнять других запросов, сразу взводить ошибку и откатывать транзакцию.</param>
        /// <param name="connection">Коннекция (не забудьте закрыть).</param>
        /// <param name="transaction">Транзакция (не забудьте завершить).</param>
        public virtual void UpdateObjectsByExtConn(
            ref DataObject[] objects, DataObjectCache dataObjectCache, bool alwaysThrowException, IDbConnection connection, IDbTransaction transaction)
        {
            object id = BusinessTaskMonitor.BeginTask("Update objects");
            var deleteQueries = new StringCollection();
            var updateQueries = new StringCollection();
            var updateFirstQueries = new StringCollection();
            var insertQueries = new StringCollection();

            var deleteTables = new StringCollection();
            var updateTables = new StringCollection();
            var insertTables = new StringCollection();
            var tableOperations = new SortedList();
            var queryOrder = new StringCollection();

            var allQueriedObjects = new System.Collections.ArrayList();

            var auditOperationInfoList = new List<AuditAdditionalInfo>();
            var extraProcessingList = new List<DataObject>();
            GenerateQueriesForUpdateObjects(deleteQueries, deleteTables, updateQueries, updateFirstQueries, updateTables, insertQueries, insertTables, tableOperations, queryOrder, true, allQueriedObjects, dataObjectCache, extraProcessingList, objects);

            GenerateAuditForAggregators(allQueriedObjects, dataObjectCache, ref extraProcessingList, transaction);

            OnBeforeUpdateObjects(allQueriedObjects);

            Exception ex = null;

            /*access checks*/

            foreach (DataObject dtob in allQueriedObjects)
            {
                Type dobjType = dtob.GetType();
                if (!SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Full, false))
                {
                    switch (dtob.GetStatus(false))
                    {
                        case ObjectStatus.Created:
                            SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Insert, true);
                            break;
                        case ObjectStatus.Altered:
                            SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Update, true);
                            break;
                        case ObjectStatus.Deleted:
                            SecurityManager.AccessObjectCheck(dobjType, tTypeAccess.Delete, true);
                            break;
                    }
                }
            }

            /*access checks*/

            if (deleteQueries.Count > 0 || updateQueries.Count > 0 || insertQueries.Count > 0)
            {
                // порядок выполнения запросов: delete,insert,update
                if (AuditService.IsAuditEnabled)
                { // На этот момент транзакция уже открыта, поэтому нужно писать в транзакциях, иначе возникнут проблемы взаимоблокировок.
                    /* Аудит проводится именно здесь, поскольку на этот момент все бизнес-сервера на объектах уже выполнились,
                     * объекты находятся именно в том состоянии, в каком должны были пойти в базу.
                     */
                    AuditOperation(extraProcessingList, auditOperationInfoList, transaction);
                }

                string query = string.Empty;
                string prevQueries = string.Empty;
                object subTask = null;
                try
                {
                    IDbCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    // прошли вглубь обрабатывая only Update||Insert
                    bool go = true;
                    do
                    {
                        string table = queryOrder[0];
                        if (!tableOperations.ContainsKey(table))
                        {
                            tableOperations.Add(table, OperationType.None);
                        }

                        var ops = (OperationType)tableOperations[table];

                        if ((ops & OperationType.Delete) != OperationType.Delete)
                        {
                            // смотрим есть ли Инсерты
                            if ((ops & OperationType.Insert) == OperationType.Insert)
                            {
                                if (
                                    (ex =
                                     RunCommands(insertQueries, insertTables, table, command, id, alwaysThrowException))
                                    == null)
                                {
                                    ops = Minus(ops, OperationType.Insert);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    go = false;
                                }
                            }

                            // смотрим есть ли Update
                            if (go && ((ops & OperationType.Update) == OperationType.Update))
                            {
                                if (
                                    (ex =
                                     RunCommands(updateQueries, updateTables, table, command, id, alwaysThrowException))
                                    == null)
                                {
                                    ops = Minus(ops, OperationType.Update);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    go = false;
                                }
                            }

                            if (go)
                            {
                                queryOrder.RemoveAt(0);
                                go = queryOrder.Count > 0;
                            }
                        }
                        else
                        {
                            go = false;
                        }
                    }
                    while (go);

                    if (queryOrder.Count > 0)
                    {
                        // сзади чистые Update
                        go = true;
                        do
                        {
                            string table = queryOrder[queryOrder.Count - 1];
                            if (!tableOperations.ContainsKey(table))
                            {
                                tableOperations.Add(table, OperationType.None);
                            }

                            var ops = (OperationType)tableOperations[table];
                            if (ops == OperationType.Update)
                            {
                                if (
                                    (ex =
                                     RunCommands(updateQueries, updateTables, table, command, id, alwaysThrowException))
                                    == null)
                                {
                                    ops = Minus(ops, OperationType.Update);
                                    tableOperations[table] = ops;
                                }
                                else
                                {
                                    go = false;
                                }

                                if (go)
                                {
                                    queryOrder.RemoveAt(queryOrder.Count - 1);
                                    go = queryOrder.Count > 0;
                                }
                            }
                            else
                            {
                                go = false;
                            }
                        }
                        while (go);
                    }

                    foreach (string table in queryOrder)
                    {
                        if ((ex = RunCommands(updateFirstQueries, updateTables, table, command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }
                    }

                    for (int i = deleteQueries.Count - 1; i >= 0; i--)
                    {
                        query = deleteQueries[i];
                        command.CommandText = query;
                        CustomizeCommand(command);
                        subTask = BusinessTaskMonitor.BeginSubTask(query, id);
                        command.ExecuteNonQuery();
                        BusinessTaskMonitor.EndSubTask(subTask);
                        prevQueries += query + "\n \n";
                    }

                    // а теперь опять с начала
                    for (int i = 0; i < queryOrder.Count; i++)
                    {
                        string table = queryOrder[i];
                        if ((ex = RunCommands(insertQueries, insertTables, table, command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }

                        if ((ex = RunCommands(updateQueries, updateTables, table, command, id, alwaysThrowException)) != null)
                        {
                            throw ex;
                        }
                    }

                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    { // Нужно зафиксировать операции аудита (то есть сообщить, что всё было корректно выполнено и запомнить время)
                        AuditService.RatifyAuditOperationWithAutoFields(
                            tExecutionVariant.Executed,
                            AuditAdditionalInfo.SetNewFieldValuesForList(transaction, this, auditOperationInfoList),
                            this,
                            true);
                    }
                }
                catch (Exception excpt)
                {
                    if (AuditService.IsAuditEnabled && auditOperationInfoList.Count > 0)
                    { // Нужно зафиксировать операции аудита (то есть сообщить, что всё было откачено)
                        AuditService.RatifyAuditOperationWithAutoFields(tExecutionVariant.Failed, auditOperationInfoList, this, false);
                    }

                    BusinessTaskMonitor.EndSubTask(subTask);
                    throw new ExecutingQueryException(query, prevQueries, excpt);
                }

                var res = new ArrayList();
                for (int i = 0; i < objects.Length; i++)
                {
                    objects[i].ClearPrototyping(true);

                    if (objects[i].GetStatus(false) != STORMDO.ObjectStatus.Deleted)
                    {
                        Utils.UpdateInternalDataInObjects(objects[i], true, dataObjectCache);
                        res.Add(objects[i]);
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

                objects = new ICSSoft.STORMNET.DataObject[res.Count];
                res.CopyTo(objects);
                BusinessTaskMonitor.EndTask(id);
            }

            if (AfterUpdateObjects != null)
            {
                AfterUpdateObjects(this, new DataObjectsEventArgs(objects));
            }
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
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        virtual public void LoadObject(ICSSoft.STORMNET.DataObject dobject)
        {
            LoadObject(dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectViewName">имя представления объекта</param>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        virtual public void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject)
        {
            LoadObject(dataObjectViewName, dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectView">представление объекта</param>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        virtual public void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject)
        {
            LoadObject(dataObjectView, dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить</param>
        /// <param name="ClearDataObject">очищать ли объект</param>
        /// <param name="CheckExistingObject">проверять ли существование объекта в хранилище</param>
        virtual public void LoadObject(
            ICSSoft.STORMNET.DataObject dobject, bool ClearDataObject, bool CheckExistingObject)
        {
            LoadObject(dobject, ClearDataObject, CheckExistingObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectViewName">наименование представления</param>
        /// <param name="dobject">бъект данных, который требуется загрузить</param>
        /// <param name="ClearDataObject">очищать ли объект</param>
        /// <param name="CheckExistingObject">проверять ли существование объекта в хранилище</param>
        virtual public void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool ClearDataObject, bool CheckExistingObject)
        {
            LoadObject(dataObjectViewName, dobject, ClearDataObject, CheckExistingObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных
        /// </summary>
        /// <param name="dataObjectView">представление</param>
        /// <param name="dobject">бъект данных, который требуется загрузить</param>
        /// <param name="ClearDataObject">очищать ли объект</param>
        /// <param name="CheckExistingObject">проверять ли существование объекта в хранилище</param>
        virtual public void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, bool ClearDataObject, bool CheckExistingObject)
        {
            LoadObject(dataObjectView, dobject, ClearDataObject, CheckExistingObject, new DataObjectCache());
        }

        //-----------------------------------------------------

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="dataobjects">исходные объекты</param>
        /// <param name="dataObjectView">представлене</param>
        /// <param name="ClearDataobject">очищать ли существующие</param>
        virtual public void LoadObjects(ICSSoft.STORMNET.DataObject[] dataobjects,
            ICSSoft.STORMNET.View dataObjectView, bool ClearDataobject)
        {
            LoadObjects(dataobjects, dataObjectView, ClearDataobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/></param>
        /// <returns>результат запроса</returns>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct)
        {
            return LoadObjects(customizationStruct, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/></param>
        /// <param name="State">Состояние вычитки( для последующей дочитки )</param>
        /// <returns></returns>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct,
            ref object State)
        {
            return LoadObjects(customizationStruct, ref State, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="State">Состояние вычитки( для последующей дочитки)</param>
        /// <returns></returns>
        virtual public ICSSoft.STORMNET.DataObject[] LoadObjects(ref object State)
        {
            return LoadObjects(ref State, new DataObjectCache());
        }

        //-------LOAD separated string Objetcs ------------------------------------
        virtual public void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject)
        {
            UpdateObject(ref dobject, false);
        }

        /// <summary>
        /// Обновление объекта данных
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить</param>
        virtual public void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, bool AlwaysThrowException)
        {
            UpdateObject(ref dobject, new DataObjectCache(), AlwaysThrowException);
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
        /// <param name="expression">Выражение DataServiceExpression</param>
        /// <returns>True/False</returns>
        public bool IsExpressionContainAttrubuteCheckOnly(string expression)
        {
            return !string.IsNullOrEmpty(expression)
                   && Regex.IsMatch(
                       expression.Trim(),
                       string.Format(@"^{0}$", SecurityManager.AttributeCheckExpressionPattern));
        }
    }

    /// <summary>
    /// Исключительная ситуация, при выполнении запроса
    /// </summary>
    [Serializable]
    public class ExecutingQueryException : Exception, System.Runtime.Serialization.ISerializable
    {
        /// <summary>
        /// Запрос при котором возникла ошибка
        /// </summary>
        public string curQuery;

        /// <summary>
        /// Выполненные предыдущие запросы (в этой же транзакции)
        /// </summary>
        public string prevQueries;

        /// <summary>
        ///
        /// </summary>
        /// <param name="cq">Запрос при котором возникла ошибка</param>
        /// <param name="pq"> Выполненные предыдущие запросы (в этой же транзакции)</param>
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
