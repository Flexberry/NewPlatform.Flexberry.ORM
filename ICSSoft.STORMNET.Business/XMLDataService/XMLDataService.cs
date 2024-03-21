namespace ICSSoft.STORMNET.Business
{
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Security;

    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using STORMFunction = ICSSoft.STORMNET.FunctionalLanguage.Function;

    /// <summary>
    /// Сервис данных для XML.
    /// </summary>
    public partial class XMLFileDataService : System.ComponentModel.Component, IDataService
    {
        private int _fieldLoadingBufferSize;
        private string _customizationString;
        private DataSet _dataSet = new DataSet();
        private DataSet _altdataSet = new DataSet();
        private TypeUsage _ldTypeUsage;
        private Guid _instanceId = Guid.NewGuid();
        private ChangeViewForTypeDelegate _changeViewForTypeDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="XMLFileDataService"/> class with specified security manager and audit service.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service.</param>
        public XMLFileDataService(ISecurityManager securityManager, IAuditService auditService)
        {
            SecurityManager = securityManager;
            AuditService = auditService;
        }

        /// <inheritdoc cref="IDataService" />
        public ISecurityManager SecurityManager { get; }

        /// <inheritdoc cref="IDataService" />
        public IAuditService AuditService { get; }

        /// <summary>
        /// Путь до файлов с базой и схемой.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Имя файла базы и схемы (без расширения).
        /// </summary>
        public string DataBaseName { get; set; }

        public int LoadingBufferSize
        {
            get
            {
                return _fieldLoadingBufferSize;
            }

            set
            {
                _fieldLoadingBufferSize = value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Режимы работы XMLFileDataService.
        /// </summary>
        private enum WorkMode
        {
            /// <summary>
            /// Работа с файлом в файловой системе.
            /// </summary>
            FileSystem,

            /// <summary>
            /// Работа с памятью.
            /// </summary>
            InMemory,
        }

        /// <summary>
        /// Режим работы со схемой.
        /// </summary>
        private WorkMode _schemaWorkMode = WorkMode.FileSystem;

        /// <summary>
        /// Режим работы с файлом данных.
        /// </summary>
        private WorkMode _dataWorkMode = WorkMode.FileSystem;

        /// <summary>
        /// Поток со схемой.
        /// </summary>
        private Stream _schemaStream;

        /// <summary>
        /// Поток со схемой.
        /// </summary>
        public Stream SchemaStream
        {
            get
            {
                if (_schemaStream != null && _schemaStream.CanSeek)
                {
                    _schemaStream.Seek(0, SeekOrigin.Begin);
                }

                return _schemaStream;
            }

            set
            {
                if (_schemaWorkMode == WorkMode.FileSystem)
                {
                    throw new Exception("XMLFileDataService функционирует в режиме работы с файловой системой. Нельзя подменять поток схемы вручную.");
                }

                ClearDataSet();
                _schemaStream = value;
                if (_dataStream != null || _dataWorkMode == WorkMode.FileSystem)
                {
                    LoadDataSet();
                }
            }
        }

        /// <summary>
        /// Поток с данными.
        /// </summary>
        private Stream _dataStream;

        /// <summary>
        /// Поток с данными.
        /// </summary>
        public Stream DataStream
        {
            get
            {
                if (_dataStream != null && _dataStream.CanSeek)
                {
                    _dataStream.Seek(0, SeekOrigin.Begin);
                }

                return _dataStream;
            }

            set
            {
                if (_dataWorkMode == WorkMode.FileSystem)
                {
                    throw new Exception("XMLFileDataService функционирует в режиме работы с файловой системой. Нельзя подменять поток данных вручную.");
                }

                ClearDataSet();
                _dataStream = value;
                LoadDataSet();
            }
        }

        #region IDataService Members

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

        public string CustomizationString
        {
            get
            {
                return _customizationString;
            }

            set
            {
                if (_customizationString != value)
                {
                    ClearDataSet();
                    _customizationString = value;

                    if (value.Contains("SchemaMode=InMemory"))
                    {
                        _schemaWorkMode = WorkMode.InMemory;
                    }

                    if (value.Contains("DataMode=InMemory"))
                    {
                        _dataWorkMode = WorkMode.InMemory;
                    }

                    string[] parts = value.Split(';');
                    string filePath = parts.FirstOrDefault(x => !x.Contains("="));

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        int lastSlash = filePath.LastIndexOf(@"\");
                        if (lastSlash >= 0)
                        {
                            Folder = filePath.Substring(0, lastSlash);
                            DataBaseName = filePath.Substring(lastSlash + 1);
                        }
                    }

                    LoadDataSet();
                }
            }
        }

        public TypeUsage TypeUsage
        {
            get
            {
                return _ldTypeUsage ?? (_ldTypeUsage = TypeUsageProvider.TypeUsage);
            }

            set
            {
                _ldTypeUsage = value;
            }
        }

        public Guid GetInstanceId()
        {
            return _instanceId;
        }

        /// <summary>
        /// возвращает количество объектов удовлетворяющих запросу.
        /// </summary>
        /// <param name="customizationStruct">что выбираем.</param>
        /// <returns></returns>
        public int GetObjectsCount(LoadingCustomizationStruct customizationStruct)
        {
            return -1;
        }

        public virtual ObjectStringDataView[] LoadValues(char separator,
                                                        LoadingCustomizationStruct customizationStruct)
        {
            return new ObjectStringDataView[0];
        }

        public void LoadObject(DataObject dataObject, DataObjectCache dataObjectCache)
        {
            LoadObject(new View(dataObject.GetType(), View.ReadType.OnlyThatObject), dataObject, true, true,
                       dataObjectCache);
        }

        public void LoadObject(View dataObjectView, DataObject dobject, bool clearDataObject,
                              bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            dataObjectCache.StartCaching(false);
            try
            {
                dataObjectCache.AddDataObject(dobject);

                if (clearDataObject)
                {
                    dobject.Clear();
                }

                Type doType = dobject.GetType();
                var lc = new LoadingCustomizationStruct(GetInstanceId());
                var lang = FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                var var = new FunctionalLanguage.VariableDef(
                    lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.KeyType(doType)), "STORMMainObjectKey");
                object prevPrimaryKey = null;

                if (dobject.Prototyped)
                {
                    prevPrimaryKey = dobject.__PrimaryKey;
                }

                FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, dobject.__PrimaryKey);
                lc.Init(new ColumnsSortDef[0], func, new[] { doType }, dataObjectView, new string[0]);
                bool changedView = false;
                LoadingCustomizationStruct cst = null;

                if (_changeViewForTypeDelegate != null)
                {
                    View v = _changeViewForTypeDelegate(doType);
                    if (v != null)
                    {
                        cst = LoadingCustomizationStruct.GetSimpleStruct(doType, v);
                        cst.AdvansedColumns = lc.AdvansedColumns;
                        cst.ColumnsOrder = lc.ColumnsOrder;
                        cst.ColumnsSort = lc.ColumnsSort;
                        cst.Distinct = lc.Distinct;
                        cst.InitDataCopy = lc.InitDataCopy;
                        cst.LimitFunction = lc.LimitFunction;
                        cst.LoadingBufferSize = lc.LoadingBufferSize;
                        changedView = true;
                    }
                }

                if (changedView)
                {
                    doType = lc.LoadingTypes[0];
                    lc = cst;
                }

                // строим запрос
                StorageStructForView[] storageStruct;

                // получаем данные
                object[][] resValue = ReadData(lc, out storageStruct);

                if (resValue != null && resValue.Length == 0)
                {
                    resValue = null;
                }

                if (resValue == null)
                {
                    if (checkExistingObject)
                    {
                        throw new CantFindDataObjectException(doType, dobject.__PrimaryKey);
                    }
                    else
                    {
                        return;
                    }
                }

                var rrr = new[] { dobject };
                Utils.ProcessingRowsetDataRef(resValue, new[] { doType }, storageStruct, lc, rrr, this, null,
                                              clearDataObject, dataObjectCache, SecurityManager);

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

        public virtual void LoadObject(View dataObjectView, DataObject dobject, bool clearDataObject,
                                       bool checkExistingObject, DataObjectCache dataObjectCache,
                                       ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                _changeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            LoadObject(dataObjectView, dobject, true, true, dataObjectCache);
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(DataObject dobject)
        {
            LoadObject(dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">имя представления объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(string dataObjectViewName, DataObject dobject)
        {
            LoadObject(dataObjectViewName, dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        public virtual void LoadObject(View dataObjectView, DataObject dobject)
        {
            LoadObject(dataObjectView, dobject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Вызывать исключение если объекта нет в хранилище.</param>
        public virtual void LoadObject(DataObject dobject, bool clearDataObject, bool checkExistingObject)
        {
            LoadObject(dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">наименование представления.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Вызывать исключение если объекта нет в хранилище.</param>
        public virtual void LoadObject(string dataObjectViewName, DataObject dobject,
                                       bool clearDataObject, bool checkExistingObject)
        {
            LoadObject(dataObjectViewName, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Вызывать исключение если объекта нет в хранилище.</param>
        public virtual void LoadObject(View dataObjectView, DataObject dobject,
                                       bool clearDataObject, bool checkExistingObject)
        {
            LoadObject(dataObjectView, dobject, clearDataObject, checkExistingObject, new DataObjectCache());
        }

        public void LoadObject(string dataObjectViewName, DataObject dobject, DataObjectCache dataObjectCache)
        {
            LoadObject(Information.GetView(dataObjectViewName, dobject.GetType()), dobject, true, false, dataObjectCache);
        }

        public void LoadObject(View dataObjectView, DataObject dobject, DataObjectCache dataObjectCache)
        {
            LoadObject(dataObjectView, dobject, true, true, dataObjectCache);
        }

        public void LoadObject(DataObject dobject, bool clearDataObject,
                               bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            LoadObject(new View(dobject.GetType(), View.ReadType.OnlyThatObject), dobject, clearDataObject,
                       checkExistingObject, dataObjectCache);
        }

        public void LoadObject(string dataObjectViewName, DataObject dobject, bool clearDataObject,
                               bool checkExistingObject, DataObjectCache dataObjectCache)
        {
            LoadObject(Information.GetView(dataObjectViewName, dobject.GetType()), dobject, clearDataObject,
                       checkExistingObject, dataObjectCache);
        }

        public void LoadObjects(DataObject[] dataobjects, View dataObjectView,
                                bool clearDataObject, DataObjectCache dataObjectCache)
        {
            dataObjectCache.StartCaching(false);

            try
            {
                var aLtypes = new ArrayList();
                var alKeys = new ArrayList();
                var aLobjectsKeys = new SortedList();

                for (int i = 0; i < dataobjects.Length; i++)
                {
                    DataObject dobject = dataobjects[i];
                    Type dotype = dobject.GetType();
                    bool addobj = false;

                    if (aLtypes.Contains(dotype))
                    {
                        addobj = true;
                    }
                    else
                    {
                        if (dotype == dataObjectView.DefineClassType || dotype.IsSubclassOf(dataObjectView.DefineClassType))
                        {
                            aLtypes.Add(dotype);
                            addobj = true;
                        }
                    }

                    if (addobj)
                    {
                        aLobjectsKeys.Add(dotype.FullName + dobject.__PrimaryKey, i);
                        alKeys.Add(dobject.__PrimaryKey);
                    }
                }

                var customizationStruct = new LoadingCustomizationStruct(_instanceId);
                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                var var = new FunctionalLanguage.VariableDef(lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.KeyType(dataObjectView.DefineClassType)),
                                                             "STORMMainObjectKey");
                var keys = new object[alKeys.Count + 1];
                alKeys.CopyTo(keys, 1);
                keys[0] = var;
                FunctionalLanguage.Function func = lang.GetFunction(lang.funcIN, keys);
                var types = new Type[aLtypes.Count];
                aLtypes.CopyTo(types);
                customizationStruct.Init(null, func, types, dataObjectView, null);
                StorageStructForView[] storageStruct;
                object[][] resValue = ReadData(customizationStruct, out storageStruct);

                if (resValue != null)
                {
                    var loadobjects = new DataObject[resValue.Length];
                    int objectTypeIndexPOs = resValue[0].Length - 1;
                    int keyIndex = storageStruct[0].props.Length;

                    for (int i = 0; i < resValue.Length; i++)
                    {
                        int indexobj = aLobjectsKeys.IndexOfKey(types[(int)resValue[i][objectTypeIndexPOs]].FullName + resValue[i][keyIndex]);

                        if (indexobj > -1)
                        {
                            loadobjects[i] = dataobjects[(int)aLobjectsKeys.GetByIndex(indexobj)];

                            if (clearDataObject)
                            {
                                loadobjects[i].Clear();
                            }

                            dataObjectCache.AddDataObject(loadobjects[i]);
                        }
                        else
                        {
                            loadobjects[i] = null;
                        }
                    }

                    Utils.ProcessingRowsetDataRef(resValue, types, storageStruct, customizationStruct, loadobjects,
                                                  this, null, clearDataObject, dataObjectCache, SecurityManager);
                }
            }
            finally
            {
                dataObjectCache.StopCaching();
            }
        }

        public DataObject[] LoadObjects(LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache)
        {
            object state = null;
            int prevBufferSize = _fieldLoadingBufferSize;
            _fieldLoadingBufferSize = 0;
            DataObject[] res = LoadObjects(customizationStruct, ref state, dataObjectCache);
            _fieldLoadingBufferSize = prevBufferSize;
            return res;
        }

        public DataObject[] LoadObjects(LoadingCustomizationStruct customizationStruct,
                                        ref object state, DataObjectCache dataObjectCache)
        {
            DataObject[] res;
            dataObjectCache.StartCaching(false);

            try
            {
                Type[] dataObjectType = customizationStruct.LoadingTypes;
                StorageStructForView[] storageStruct;
                LoadingCustomizationStruct cst = null;
                bool changedView = false;

                if (_changeViewForTypeDelegate != null)
                {
                    foreach (Type type in dataObjectType)
                    {
                        View v = _changeViewForTypeDelegate(type);
                        if (v != null)
                        {
                            cst = LoadingCustomizationStruct.GetSimpleStruct(type, v);
                            cst.AdvansedColumns = customizationStruct.AdvansedColumns;
                            cst.ColumnsOrder = customizationStruct.ColumnsOrder;
                            cst.ColumnsSort = customizationStruct.ColumnsSort;
                            cst.Distinct = customizationStruct.Distinct;
                            cst.InitDataCopy = customizationStruct.InitDataCopy;
                            cst.LimitFunction = customizationStruct.LimitFunction;
                            cst.LoadingBufferSize = customizationStruct.LoadingBufferSize;
                            changedView = true;
                            break;
                        }
                    }
                }

                if (changedView)
                {
                    dataObjectType = cst.LoadingTypes;
                    customizationStruct = cst;
                }

                // получаем данные
                object[][] resValue = ReadData(customizationStruct, out storageStruct);
                state = null;

                res = resValue == null
                          ? new DataObject[0]
                          : Utils.ProcessingRowsetData(resValue, dataObjectType, storageStruct, customizationStruct,
                                                       this, null, dataObjectCache, SecurityManager);
            }
            finally
            {
                dataObjectCache.StopCaching();
            }

            return res;
        }

        public DataObject[] LoadObjects(ref object state, DataObjectCache dataObjectCache)
        {
            return null;

            // все никакого иного кода быть не может
        }

        public virtual DataObject[] LoadObjects(View dataObjectView)
        {
            LoadingCustomizationStruct lc = LoadingCustomizationStruct.GetSimpleStruct(dataObjectView.DefineClassType,
                                                                                       dataObjectView);
            return LoadObjects(lc, new DataObjectCache());
        }

        public virtual DataObject[] LoadObjects(View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                _changeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(dataObjectView);
        }

        public virtual void LoadObjects(DataObject[] dataobjects, View dataObjectView, bool clearDataObject)
        {
            LoadObjects(dataobjects, dataObjectView, clearDataObject, new DataObjectCache());
        }

        public virtual DataObject[] LoadObjects(View[] dataObjectViews)
        {
            var arr = new ArrayList();
            DataObject[] res;

            foreach (View view in dataObjectViews)
            {
                res = LoadObjects(view);

                foreach (DataObject dataObject in res)
                {
                    arr.Add(dataObject);
                }
            }

            res = new DataObject[arr.Count];
            arr.CopyTo(res);
            return res;
        }

        public virtual DataObject[] LoadObjects(LoadingCustomizationStruct[] customizationStructs)
        {
            var arr = new ArrayList();
            DataObject[] res;

            foreach (LoadingCustomizationStruct customizationStruct in customizationStructs)
            {
                res = LoadObjects(customizationStruct, new DataObjectCache());

                foreach (DataObject dataObject in res)
                {
                    arr.Add(dataObject);
                }
            }

            res = new DataObject[arr.Count];
            arr.CopyTo(res);
            return res;
        }

        public virtual DataObject[] LoadObjects(View[] dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                _changeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(dataObjectViews);
        }

        public virtual DataObject[] LoadObjects(LoadingCustomizationStruct[] customizationStructs,
                                                ChangeViewForTypeDelegate changeViewForTypeDelegate)
        {
            if (changeViewForTypeDelegate != null)
            {
                _changeViewForTypeDelegate = changeViewForTypeDelegate;
            }

            return LoadObjects(customizationStructs);
        }

        public virtual DataObject[] LoadObjects(LoadingCustomizationStruct customizationStruct)
        {
            return LoadObjects(customizationStruct, new DataObjectCache());
        }

        public virtual DataObject[] LoadObjects(LoadingCustomizationStruct customizationStruct, ref object state)
        {
            return LoadObjects(customizationStruct, ref state, new DataObjectCache());
        }

        public virtual DataObject[] LoadObjects(ref object state)
        {
            return LoadObjects(ref state, new DataObjectCache());
        }

        public ObjectStringDataView[] LoadStringedObjectView(char separator,
                                                     LoadingCustomizationStruct customizationStruct)
        {
            object state = null;
            int prevBufferSize = _fieldLoadingBufferSize;
            _fieldLoadingBufferSize = 0;
            ObjectStringDataView[] res = LoadStringedObjectView(separator, customizationStruct, ref state);
            _fieldLoadingBufferSize = prevBufferSize;
            return res;
        }

        public ObjectStringDataView[] LoadStringedObjectView(char separator, LoadingCustomizationStruct customizationStruct,
                                                             ref object state)
        {
            Type[] dataObjectType = customizationStruct.LoadingTypes;
            StorageStructForView[] storageStruct;
            object[][] resValue = ReadData(customizationStruct, out storageStruct);
            int propCount = customizationStruct.View.Properties.Length;
            state = new[] { state, dataObjectType, storageStruct, separator, propCount, customizationStruct };
            object[] stst = null;

            if (resValue == null)
            {
                return new ObjectStringDataView[0];
            }

            return Utils.ProcessingRowSet2StringedView(resValue, dataObjectType, propCount, separator,
                                                       customizationStruct, storageStruct, this, null, ref stst, SecurityManager);
        }

        public ObjectStringDataView[] LoadStringedObjectView(ref object state)
        {
            return null;

            // все никакого иного кода быть не может
        }

        /// <summary>
        /// Сохранить объект данных в XML-файл.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="dataObjectCache"></param>
        /// <param name="alwaysThrowException"></param>
        public void UpdateObject(ref DataObject dataObject, DataObjectCache dataObjectCache, bool alwaysThrowException)
        {
            UpdateObject(ref dataObject, dataObjectCache);
        }

        /// <summary>
        /// Сохранить объект данных в XML-файл.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="dataObjectCache"></param>
        public void UpdateObject(ref DataObject dataObject, DataObjectCache dataObjectCache)
        {
            var arr = new[] { dataObject };
            UpdateObjects(ref arr, dataObjectCache);
            if (arr != null && arr.Length > 0)
            {
                dataObject = arr[0];
            }
            else
            {
                dataObject = null;
            }
        }

        /// <summary>
        /// Сохранить объект данных в XML-файл.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="dataObjectCache"></param>
        public void UpdateObject(DataObject dataObject, DataObjectCache dataObjectCache)
        {
            UpdateObject(ref dataObject, dataObjectCache, false);
        }

        /// <summary>
        /// Сохранить объект данных в XML-файл.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="alwaysThrowException"></param>
        public void UpdateObject(DataObject dataObject, bool alwaysThrowException)
        {
            UpdateObject(dataObject);
        }

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        /// <param name="alwaysThrowException"></param>
        public virtual void UpdateObject(ref DataObject dobject, bool alwaysThrowException)
        {
            UpdateObject(ref dobject);
        }

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        public virtual void UpdateObject(ref DataObject dobject)
        {
            UpdateObject(ref dobject, new DataObjectCache());
        }

        public virtual void UpdateObjects(ref DataObject[] objects)
        {
            UpdateObjects(ref objects, new DataObjectCache());
        }

        /// <summary>
        /// Сохранить объект данных в XML-файл.
        /// </summary>
        /// <param name="dataObject"></param>
        public void UpdateObject(DataObject dataObject)
        {
            UpdateObject(ref dataObject, new DataObjectCache());
        }

        public virtual void UpdateObjects(ref DataObject[] objects, bool alwaysThrowException)
        {
            UpdateObjects(
                ref objects, new DataObjectCache(), alwaysThrowException);
        }

        public virtual void UpdateObjects(ref DataObject[] objects, DataObjectCache dataObjectCache,
                                          bool alwaysThrowException)
        {
            UpdateObjects(ref objects, dataObjectCache);
        }

        /// <summary>
        /// Корректное преобразование строкового значения к указанному типу.
        /// </summary>
        /// <param name="sValue">Строковое значение для приведения.</param>
        /// <param name="castType">Тип к которому преобразуем.</param>
        /// <returns>Преобразованное значение.</returns>
        public static object ChangeType(string sValue, Type castType)
        {
            MethodInfo methodInfo = castType.GetMethod("Parse", new[] { typeof(string) });

            if (methodInfo != null && methodInfo.IsStatic)
            {
                return methodInfo.Invoke(Activator.CreateInstance(castType), new object[] { sValue });
            }

            return Convert.ChangeType(sValue, castType);
        }

        public void UpdateObjects(ref DataObject[] objects, DataObjectCache dataObjectCache)
        {
            LoadDataSet();
            var deletedObjects = new ArrayList();
            var updatedObjects = new ArrayList();
            var insertedObjects = new ArrayList();
            CollectUpdateData(deletedObjects, updatedObjects, insertedObjects, true, dataObjectCache, objects);

            bool enforceConstraints = _dataSet.EnforceConstraints;

            // отключаем проверку ссылочной целостности на время обновления объектов данных
            if (_dataSet.EnforceConstraints)
            {
                _dataSet.EnforceConstraints = false;
            }

            if (deletedObjects.Count > 0 || updatedObjects.Count > 0 || insertedObjects.Count > 0)
            {
                // порядок выполнения запросов
                // delete,insert,update
                for (int i = deletedObjects.Count - 1; i >= 0; i--)
                {
                    var dos = (DeleteObjectStruct)deletedObjects[i];
                    DataTable dt = _dataSet.Tables[dos.Table];

                    if (dt.PrimaryKey == null || dt.PrimaryKey.Length == 0)
                    {
                        dt.PrimaryKey = new[] { dt.Columns[dos.PrimaryKeyName] };
                    }

                    DataRow dr = dt.Rows.Find(dos.Key);
                    dr.Delete();
                }

                foreach (InsertObjectStruct ios in insertedObjects)
                {
                    DataTable dt = _dataSet.Tables[ios.Table];
                    DataRow dr = dt.NewRow();

                    for (int j = 0; j < ios.Values.Count; j++)
                    {
                        var colName = (string)ios.Values.GetKey(j);
                        object value = ios.Values.GetByIndex(j);

                        if (dt.Columns.IndexOf(colName) != -1)
                        {
                            if ((value != null) && (value != DBNull.Value))
                            {
                                dr[colName] = ChangeType(value.ToString(),
                                                         _altdataSet.Tables[ios.Table].Columns[colName].DataType);
                            }
                            else
                            {
                                dr[colName] = DBNull.Value;
                            }
                        }
                        else if ((value != null) && (value != DBNull.Value))
                        {
                            dt.Columns.Add(colName, _altdataSet.Tables[ios.Table].Columns[colName].DataType);
                            dr[colName] = ChangeType(value.ToString(),
                                                     _altdataSet.Tables[ios.Table].Columns[colName].DataType);
                        }
                    }

                    dt.Rows.Add(dr);
                }

                foreach (UpdateObjectStruct uos in updatedObjects)
                {
                    DataTable dt = _dataSet.Tables[uos.Table];

                    if (dt.PrimaryKey == null || dt.PrimaryKey.Length == 0)
                    {
                        dt.PrimaryKey = new[] { dt.Columns[uos.PrimaryKeyName] };
                    }

                    DataRow dr = dt.Rows.Find(uos.Key);
                    bool bnewr = false;

                    if (dr == null)
                    {
                        dr = dt.NewRow();
                        bnewr = true;
                    }

                    for (int j = 0; j < uos.Values.Count; j++)
                    {
                        var colName = (string)uos.Values.GetKey(j);
                        object value = uos.Values.GetByIndex(j);

                        if (dt.Columns.IndexOf(colName) != -1)
                        {
                            if ((value != null) && (value != DBNull.Value))
                            {
                                dr[colName] = ChangeType(value.ToString(),
                                                                 _altdataSet.Tables[uos.Table].Columns[colName].DataType);
                            }
                            else
                            {
                                dr[colName] = DBNull.Value;
                            }
                        }
                        else if ((value != null) && (value != DBNull.Value))
                        {
                            dt.Columns.Add(colName, _altdataSet.Tables[uos.Table].Columns[colName].DataType);
                            dr[colName] = ChangeType(value.ToString(),
                                                             _altdataSet.Tables[uos.Table].Columns[colName].DataType);
                        }
                    }

                    if (bnewr)
                    {
                        dt.Rows.Add(dr);
                    }
                }

                Exception enforceConstraintsError = null;

                try
                {
                    if (enforceConstraints)
                    {
                        _dataSet.EnforceConstraints = true;
                    }
                }
                catch (Exception exception)
                {
                    enforceConstraintsError = exception;
                }

                if (!_dataSet.HasErrors && enforceConstraintsError == null)
                {
                    _dataSet.AcceptChanges();
                    SaveDataSet();
                    var res = new ArrayList();

                    foreach (DataObject dataObject in objects)
                    {
                        if (dataObject.GetStatus(false) != ObjectStatus.Deleted)
                        {
                            Utils.UpdateInternalDataInObjects(dataObject, true, dataObjectCache);
                            res.Add(dataObject);
                        }
                    }

                    objects = new DataObject[res.Count];
                    res.CopyTo(objects);
                }
                else
                {
                    Exception exception;

                    if (enforceConstraintsError != null)
                    {
                        exception = enforceConstraintsError;
                    }
                    else
                    {
                        string err = string.Empty;

                        foreach (DataTable dt in _dataSet.Tables)
                        {
                            err += dt.TableName + Environment.NewLine;
                        }

                        exception = new Exception("DataSet Error in" + err);
                    }

                    _dataSet.RejectChanges();
                    throw exception;
                }
            }
        }

        #endregion IDataService Members

        public void LoadDataSet()
        {
            if (_dataSet.Tables.Count == 0)
            {
                if (_schemaWorkMode == WorkMode.FileSystem)
                {
                    _altdataSet.ReadXmlSchema(Folder + @"\" + DataBaseName + ".XSD");
                }
                else
                {
                    _altdataSet.ReadXmlSchema(SchemaStream);
                }

                LoadDataSet(_dataSet);
            }
        }

        private void LoadDataSet(DataSet dataSet)
        {
            if (_schemaWorkMode == WorkMode.FileSystem)
            {
                dataSet.ReadXmlSchema(Folder + @"\" + DataBaseName + ".XSD");
            }
            else
            {
                dataSet.ReadXmlSchema(SchemaStream);
            }

            if (_dataWorkMode == WorkMode.FileSystem)
            {
                dataSet.ReadXml(Folder + @"\" + DataBaseName + ".XML");
            }
            else
            {
                dataSet.ReadXml(DataStream);
            }
        }

        public void ClearDataSet()
        {
            _dataSet.Clear();
        }

        public void SaveDataSet()
        {
            if (_dataWorkMode == WorkMode.FileSystem)
            {
                _dataSet.WriteXml(Folder + @"\" + DataBaseName + ".XML");
            }
            else
            {
                _dataSet.WriteXml(DataStream);
            }
        }

        public void AddDataStructForTable(Type dataObjectType)
        {
            CreateTableForClass(dataObjectType, _dataSet);
            if (_schemaWorkMode == WorkMode.FileSystem)
            {
                _dataSet.WriteXmlSchema(Folder + @"\" + DataBaseName + ".XSD");
            }
            else
            {
                _dataSet.WriteXmlSchema(SchemaStream);
            }
        }

        private DataTable CreateTableForClass(Type dataobjectType, DataSet ds)
        {
            string tableName = Information.GetClassStorageName(dataobjectType);
            if (!ds.Tables.Contains(tableName))
            {
                var dt = new System.Data.DataTable(tableName);
                ds.Tables.Add(dt);
                string[] storprops = Information.GetStorablePropertyNames(dataobjectType);
                Type dotype = typeof(DataObject);
                Type datype = typeof(DetailArray);

                // вначале PrimaryKey;
                Type primkeyType = Information.GetStorageTypeForType(Information.GetPropertyType(dataobjectType, "__PrimaryKey"),
                                                                     typeof(XMLFileDataService));
                dt.Columns.Add(new System.Data.DataColumn(Information.GetPrimaryKeyStorageName(dataobjectType),
                                                          primkeyType, string.Empty, System.Data.MappingType.Attribute));
                dt.PrimaryKey = new[] { dt.Columns[0] };

                foreach (string prop in storprops)
                {
                    Type propType = Information.GetPropertyType(dataobjectType, prop);
                    Type storType = Information.GetStorageTypeForType(propType, typeof(XMLFileDataService));
                    string propstor = Information.GetPropertyStorageName(dataobjectType, prop);

                    if (propType.IsSubclassOf(dotype))
                    {
                        Type[] mastertypes = TypeUsage.GetUsageTypes(dataobjectType, prop);
                        for (int mi = 0; mi < mastertypes.Length; mi++)
                        {
                            System.Data.DataTable masterTable = CreateTableForClass(mastertypes[mi], ds);
                            string propstorname;

                            if (propstor == string.Empty)
                            {
                                propstorname = Information.GetPropertyStorageName(dataobjectType, prop, mi);
                            }
                            else
                            {
                                propstorname = propstor + "_M" + mi;
                            }

                            Type masterkeyType =
                                Information.GetStorageTypeForType(Information.GetPropertyType(mastertypes[mi], "__PrimaryKey"),
                                                                  typeof(XMLFileDataService));
                            var dc = new System.Data.DataColumn(propstorname, masterkeyType, string.Empty,
                                                                System.Data.MappingType.Attribute);
                            dt.Columns.Add(dc);
                            string masterprimkeyname = Information.GetPrimaryKeyStorageName(mastertypes[mi]);
                            ds.Relations.Add(tableName + "_" + prop + "__" + masterTable.TableName,
                                             masterTable.Columns[masterprimkeyname], dc, true);
                        }
                    }
                    else if (propType.IsSubclassOf(datype))
                    {
                        Type[] dettypes = TypeUsage.GetUsageTypes(dataobjectType, prop);
                        foreach (Type tp in dettypes)
                        {
                            CreateTableForClass(tp, ds);
                        }
                    }
                    else if (prop != "__PrimaryKey")
                    {
                        dt.Columns.Add(
                            new System.Data.DataColumn(propstor, storType, string.Empty, System.Data.MappingType.Attribute));
                    }
                }
            }

            return ds.Tables[tableName];
        }

        private DataObject[] ObjectsByDeleteView(View view, object mainkey, DataObjectCache dataObjectCache)
        {
            string tableName = Information.GetClassStorageName(view.DefineClassType);
            string prkeyStorName = view.Properties[1].Name;

            // string prevDicValue = "";
            FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lang = FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
            var var = new FunctionalLanguage.VariableDef(
                lang.GetObjectTypeForNetType(KeyGen.KeyGenerator.KeyType(view.DefineClassType)), prkeyStorName);
            FunctionalLanguage.Function func = lang.GetFunction(lang.funcEQ, var, mainkey);
            var cs = new LoadingCustomizationStruct(GetInstanceId());
            cs.Init(new ColumnsSortDef[0], func, new[] { view.DefineClassType }, view, new string[0]);
            return LoadObjects(cs, dataObjectCache);
        }

        protected virtual void GetAlteredPropsWithValues(DataObject dobject, bool checkLoadedProps,
                                                         out SortedList propsWithValues, out DataObject[] detailObjects)
        {
            propsWithValues = new SortedList();
            var details = new ArrayList();
            Type type = dobject.GetType();
            string[] props = (dobject.GetStatus(false) == ObjectStatus.Created)
                                 ? Information.GetPropertyNamesForInsert(type)
                                 : (Information.AutoAlteredClass(type)
                                        ? dobject.GetAlteredPropertyNames(false)
                                        : dobject.GetAlteredPropertyNames());
            props = Information.SortByLoadingOrder(type, props);

            if (checkLoadedProps && dobject.GetLoadingState() != LoadingState.Loaded)
            {
                var alteredprops = new StringCollection();
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
            }

            foreach (string prop in props)
            {
                object propval = Information.GetPropValueByName(dobject, prop);
                Type propType = Information.GetPropertyType(type, prop);
                string propstor = Information.GetPropertyStorageName(type, prop);

                if (propType.IsSubclassOf(typeof(DataObject)))
                {
                    Type[] mastertypes = TypeUsage.GetUsageTypes(type, prop);
                    Type propValType = null;
                    if (propval != null)
                    {
                        propValType = propval.GetType();
                    }

                    for (int i = 0; i < mastertypes.Length; i++)
                    {
                        string realpropname;

                        if (propstor == string.Empty)
                        {
                            realpropname = Information.GetPropertyStorageName(type, prop, i);
                        }
                        else
                        {
                            realpropname = propstor + "_M" + i;
                        }

                        propsWithValues.Add(realpropname, propValType == mastertypes[i]
                                                          ? ((DataObject)propval).__PrimaryKey
                                                          : null);
                    }
                }
                else if (propType.IsSubclassOf(typeof(DetailArray)))
                {
                    if (propval != null)
                    {
                        foreach (DataObject dob in (DetailArray)propval)
                        {
                            if (dob.GetStatus(false) != ObjectStatus.UnAltered)
                            {
                                details.Add(dob);
                            }
                        }
                    }
                }
                else if (propType.IsEnum)
                {
                    propval = EnumCaption.GetCaptionFor(propval);
                    propsWithValues.Add(propstor, propval);
                }
                else
                {
                    propsWithValues.Add(propstor, propval);
                }
            }

            detailObjects = new DataObject[details.Count];
            details.CopyTo(detailObjects);
        }

        public virtual void CollectUpdateData(ArrayList deletedObjects, ArrayList updatedObjects,
                                              ArrayList insertedObjects, bool checkLoadedProps,
                                              DataObjectCache dataObjectCache, params DataObject[] dobjects)
        {
            var processingObjects = new ArrayList();
            processingObjects.AddRange(dobjects);

            for (int i = 0; i < processingObjects.Count; i++)
            {
                var dobject = (DataObject)processingObjects[i];
                ObjectStatus curObjectStatus = (i < dobjects.Length) ? dobject.GetStatus() : dobject.GetStatus(false);
                Type doType = dobject.GetType();

                switch (curObjectStatus)
                {
                    case ObjectStatus.UnAltered:
                        break;
                    case ObjectStatus.Deleted:
                        {
                            string table = Information.GetClassStorageName(doType);
                            deletedObjects.Add(new DeleteObjectStruct(table, dobject.__PrimaryKey,
                                                                      Information.GetPrimaryKeyStorageName(doType)));
                            DataObject[] subobjects;
                            DataObject[] smastersobjs;
                            View[] views;
                            Utils.getDetailsObjects(this, dobject, out subobjects, out smastersobjs, out views);

                            foreach (DataObject subobject in subobjects)
                            {
                                subobject.SetStatus(ObjectStatus.Deleted);

                                if (!processingObjects.Contains(subobject))
                                {
                                    processingObjects.Add(subobject);
                                }
                            }

                            foreach (View subview in views)
                            {
                                DataObject[] detobjects = ObjectsByDeleteView(subview, dobject.__PrimaryKey, dataObjectCache);

                                foreach (DataObject subobject in detobjects)
                                {
                                    subobject.SetStatus(ObjectStatus.Deleted);

                                    if (!processingObjects.Contains(subobject))
                                    {
                                        processingObjects.Add(subobject);
                                    }
                                }
                            }

                            break;
                        }

                    case ObjectStatus.Created:
                        {
                            SortedList propsWithValues;
                            DataObject[] detailsObjects;
                            GetAlteredPropsWithValues(dobject, false, out propsWithValues, out detailsObjects);

                            foreach (DataObject detobj in detailsObjects)
                            {
                                if (!processingObjects.Contains(detobj))
                                {
                                    processingObjects.Add(detobj);
                                }
                            }

                            InsertObjectStruct ios;
                            ios.Table = Information.GetClassStorageName(doType);
                            var cols = new string[propsWithValues.Count];
                            propsWithValues.Keys.CopyTo(cols, 0);
                            ios.Values = propsWithValues;
                            insertedObjects.Add(ios);
                            break;
                        }

                    case ObjectStatus.Altered:
                        {
                            SortedList propsWithValues;
                            DataObject[] detailsObjects;
                            GetAlteredPropsWithValues(dobject, checkLoadedProps, out propsWithValues, out detailsObjects);

                            foreach (DataObject detobj in detailsObjects)
                            {
                                if (!processingObjects.Contains(detobj))
                                {
                                    processingObjects.Add(detobj);
                                }
                            }

                            if (propsWithValues.Count > 0)
                            {
                                UpdateObjectStruct uos;
                                uos.Table = Information.GetClassStorageName(doType);
                                uos.Key = dobject.__PrimaryKey;
                                uos.Values = propsWithValues;
                                uos.PrimaryKeyName = Information.GetPrimaryKeyStorageName(doType);
                                updatedObjects.Add(uos);
                            }

                            break;
                        }
                }
            }
        }

        public virtual string ConvertSimpleValueToQueryValueString(object value)
        {
            Type valType = value.GetType();

            if (valType == typeof(string))
            {
                return "'" + value + "'";
            }

            if (valType == typeof(DateTime))
            {
                return "'" + ((DateTime)value).ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.UniversalSortableDateTimePattern) + "'";
            }

            if (valType.IsEnum)
            {
                return "'" + EnumCaption.GetCaptionFor(value) + "'";
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

            if (valType == typeof(KeyGen.KeyGuid))
            {
                return "'" + value + "'";
            }

            if (valType == typeof(decimal))
            {
                return ((decimal)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }

            return value.ToString();
        }

        public virtual string ConvertValueToQueryValueString(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            if (Utils.IsInternalBaseType(value))
            {
                return ConvertSimpleValueToQueryValueString(value);
            }

            Type valType = value.GetType();

            if (valType.IsEnum)
            {
                return "'" + EnumCaption.GetCaptionFor(value) + "'";
            }

            Type storageType = Information.GetStorageType(value, GetType());
            value = Convertors.InOperatorsConverter.Convert(value, storageType);

            return ConvertSimpleValueToQueryValueString(value);
        }

        private string LimitFunction2FilterExpression(STORMFunction limitFunction)
        {
            return FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(limitFunction,
                                                                               ConvertValueToQueryValueString,
                                                                               PutIdentifierIntoBrackets,
                                                                               this);
        }

        public virtual string PutIdentifierIntoBrackets(string identifier)
        {
            return identifier;
        }

        private struct DeleteObjectStruct
        {
            public string Table;
            public object Key;
            public string PrimaryKeyName;

            public DeleteObjectStruct(string table, object key, string primaryKey)
            {
                Table = table;
                Key = key;
                PrimaryKeyName = primaryKey;
            }
        }

        private struct InsertObjectStruct
        {
            public string Table;
            public SortedList Values;

            public InsertObjectStruct(string table, SortedList values)
            {
                Table = table;
                Values = values;
            }
        }

        private struct UpdateObjectStruct
        {
            public string Table;
            public object Key;
            public SortedList Values;
            public string PrimaryKeyName;

            public UpdateObjectStruct(string table, object key, SortedList values, string primaryKey)
            {
                Table = table;
                Key = key;
                Values = values;
                PrimaryKeyName = primaryKey;
            }
        }

        /// <summary>
        /// Создание копии экземпляра сервиса данных.
        /// </summary>
        /// <returns>Копии экземпляра сервиса данных.</returns>
        public object Clone()
        {
            return new XMLFileDataService(SecurityManager, AuditService)
            {
                _altdataSet = _altdataSet,
                _changeViewForTypeDelegate = _changeViewForTypeDelegate,
                _customizationString = _customizationString,
                _dataSet = _dataSet,
                _dataStream = _dataStream,
                _dataWorkMode = _dataWorkMode,
                _fieldLoadingBufferSize = _fieldLoadingBufferSize,
                _instanceId = _instanceId,
                _ldTypeUsage = _ldTypeUsage,
                _schemaStream = _schemaStream,
                _schemaWorkMode = _schemaWorkMode,
                DataBaseName = DataBaseName,
                Folder = Folder,
            };
        }

        /// <summary>
        /// Корректное завершения операции порционного чтения LoadStringedObjectView.
        /// </summary>
        /// <param name="state">Параметр состояния загрузки (массив объектов).</param>
        public void CompleteLoadStringedObjectView(ref object state)
        {
            throw new NotImplementedException();
        }
    }
}
