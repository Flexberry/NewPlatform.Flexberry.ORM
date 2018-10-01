namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.KeyGen;

    #region class DataObject
    /// <summary>
    /// Статус объекта данных
    /// </summary>
    public enum ObjectStatus
    {
        /// <summary>
        /// Создан
        /// </summary>
        Created,

        /// <summary>
        /// Удалён
        /// </summary>
        Deleted,

        /// <summary>
        /// Не изменён
        /// </summary>
        UnAltered,

        /// <summary>
        /// Изменён
        /// </summary>
        Altered
    }

    /// <summary>
    /// Состояние загрузки объекта данных
    /// </summary>
    public enum LoadingState
    {
        /// <summary>
        /// Не загружен
        /// </summary>
        NotLoaded,

        /// <summary>
        /// Загружен частично, 
        /// подробности выясняются методами <see cref="DataObject.GetLoadedProperties"/> 
        /// и <see cref="DataObject.CheckLoadedProperty"/>
        /// </summary>
        LightLoaded,

        /// <summary>
        /// Полностью загружен
        /// </summary>
        Loaded
    }

    /// <summary>
    /// Базовый абстрактный класс, от которого наследуются все объекты данных STORM.NET
    /// </summary>	
    [Serializable]
    [PrimaryKeyStorage("primaryKey")]
    [TypeStorage("typeId")]
    [KeyGenerator(typeof(GUIDGenerator))]
    [TrimmedStringStorage]
    public abstract class DataObject
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        private object primaryKey;

        /// <summary>
        /// Состояние объекта (иногда лучше перевычислить, а не брать это значение)
        /// </summary>
        private ObjectStatus state = ObjectStatus.UnAltered;
        
        /// <summary>
        /// Состояние загруженности объекта. По-умолчанию LoadingState.NotLoaded
        /// </summary>
        private LoadingState loading = LoadingState.NotLoaded;

        /// <summary>
        /// Массив имён загруженных свойств объекта
        /// </summary>
        private string[] LoadedProperties;

        /// <summary>
        /// Копия данных
        /// </summary>
        private DataObject dataCopy;

        /// <summary>
        /// Ссылка на DetailArray, в котором находится объект
        /// </summary>
        private DetailArray array;

        /// <summary>
        /// Нужно ли проверять детейларрей в его сеттере
        /// </summary>
        private bool CheckDetail = true;

        /// <summary>
        /// Ключ блокировки объекта. Используется для ReadOnly на формах. Не связан с LockService
        /// </summary>
        private object readKey = null;

        /// <summary>
        /// Массив изменённых свойств
        /// </summary>
        private string[] fieldAlteredpropertyNames;

        /// <summary>
        /// Ключ прототипизации
        /// </summary>
        private object prototypeKey;

        /// <summary>
        /// Dynamic Properties
        /// </summary>
        private Collections.NameObjectCollection fieldDynamicProperties = null;

        /// <summary>
        /// Первичный ключ является уникальным
        /// </summary>
        public bool PrimaryKeyIsUnique;

        /// <summary>
        /// кэш для делегатов присвоения по полям
        /// </summary>
        private static Dictionary<FieldInfo, SetHandler> cacheSetHandler = new Dictionary<FieldInfo, SetHandler>();

        /// <summary>
        /// кэш для делегатов получения значения поля
        /// </summary>
        private static Dictionary<FieldInfo, GetHandler> cacheGetHandler = new Dictionary<FieldInfo, GetHandler>();

        /// <summary>
        /// Установить первичный ключ в объект данных.
        /// Выполняется операция Clear() для объекта, присваивается первичный ключ,
        /// SetLoadingState(LoadingState.LightLoaded);
        /// SetLoadedProperties("__PrimaryKey");
        /// </summary>
        /// <param name="primaryKey">Первичный ключ</param>
        public void SetExistObjectPrimaryKey(object primaryKey)
        {
            Clear();
            __PrimaryKey = primaryKey;
            SetLoadingState(LoadingState.LightLoaded);
            SetLoadedProperties("__PrimaryKey");
            InitDataCopy();
        }

        /// <summary>
        /// является ли объект копией
        /// </summary>
        protected bool IsDataCopy;

        /// <summary>
        /// Проверка что объект залочен
        /// </summary>
        [NotStored]
        [DisableAutoViewed]
        public bool IsReadOnly { get { return readKey != null; } }


        /// <summary>
        /// Динамические свойства объекта
        /// </summary>
        [NotStored]
        [DisableAutoViewed]
        public Collections.NameObjectCollection DynamicProperties
        {
            get
            {
                return fieldDynamicProperties
                       ?? (fieldDynamicProperties = new ICSSoft.STORMNET.Collections.NameObjectCollection());
            }

            set
            {
                fieldDynamicProperties = value;
            }
        }



        /// <summary>
        /// Ключ прототипа
        /// </summary>
        [NotStored]
        [DisableAutoViewed]
        public object __PrototypeKey
        {
            get
            {
                return prototypeKey;
            }
        }

        /// <summary>
        /// Прототипизированный ли объект
        /// </summary>
        [NotStored]
        [DisableAutoViewed]
        public bool Prototyped
        {
            get
            {
                return prototypeKey != null;
            }
        }

        /// <summary>
        /// Делегат для получения презентационного значения объекта. Если не прописан или возвращает null, то будет использована стандартная логика получения этого значения
        /// </summary>
        public static GetPresentationValueDelegate GetPresentationValueDelegate;

        /// <summary>
        /// Функция для получения презентационного значения для объекта по умолчанию. 
        /// Презентационное значение используется в случаях, когда необходимо каким-либо образом 
        /// с максимальной степенью адекватности отобразить объект, а средства настройки этого отображения недоступны.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultPresentationValue()
        {
            #region Поиск подходящего начального названия для ярлыка

            try
            {
                var properties = Information.GetAllPropertyNames(GetType());

                var attributes = new List<string>() { "Название", "Наименование", "Name" };
                attributes.AddRange(properties);

                foreach (var attribute in attributes)
                {
                    if (Information.CheckPropertyExist(GetType(), attribute)
                        && Information.GetPropertyType(GetType(), attribute) == typeof(string))
                        return  Information.GetPropValueByName(this, attribute) as string;
                }
            }
            catch (Exception ex)
            {
                if (LogService.Log.IsWarnEnabled)
                    LogService.Log.Warn("Ошибка при поиске имени для ярлыка.", ex);
            }
            #endregion

            return ToString();
        }


        /// <summary>
        /// Функция для получения презентационного значения для объекта. Используется, как минимум, в ярлыках на рабочем столе.
        /// </summary>
        /// <returns></returns>
        public virtual string GetPresentationValue()
        {
            if (GetPresentationValueDelegate != null)
            {
                string retS = GetPresentationValueDelegate(this);
                if (retS != null)
                {
                    return retS;
                }
            }

            return GetDefaultPresentationValue();
        }

        /// <summary>
        /// Заблокировать объект
        /// </summary>
        /// <param name="key">ключ блокировки объекта</param>
        public void LockObject(object key)
        {
            CheckReadOnly();
            readKey = key;
        }

        /// <summary>
        /// Процедура проверки объекта на заблокированность
        /// </summary>
        protected void CheckReadOnly()
        {
            if (IsReadOnly)
                throw new DataObjectIsReadOnlyException();
        }

        /// <summary>
        /// Разблокировать объект
        /// </summary>
        /// <param name="key">ключ блокировки объекта</param>
        public void UnLockObject(object key)
        {
            if (IsReadOnly)
            {
                if (key.Equals(readKey))
                    readKey = null;
                else
                    throw new UnlockObjectDifferentKeyException();
            }
        }

        /// <summary>
        /// Ссылка на DetailArray, в котором находится объект
        /// </summary>
        [NotStored]
        [DisableAutoViewed]
        internal DetailArray DetailArray
        {
            get { return array; }
            set
            {
                if (CheckDetail && (array != null) && (value != null) && (value != array))
                    throw new ObjectAlreadyInDetailArrayException();
                array = value;
            }
        }

        public DetailArray GetDetailArray()
        {
            return array;
        }

        /// <summary>
        /// Базовый конструктор по-умолчанию
        /// </summary>
        public DataObject()
        {
            KeyGenerator.Generate(this, null);
            this.SetStatus(ObjectStatus.Created);
            this.SetLoadingState(LoadingState.NotLoaded);
            LoadedProperties = new string[0];// Information.GetStorablePropertyNames(this.GetType());
            //Array.Sort(LoadedProperties);
        }

        /// <summary>
        /// Финализатор, обеспечивающий уничтожение объекта данных из кеша
        /// </summary>
        ~DataObject()
        {
            /*
            if (this.__PrimaryKey!=null)
                    DataObjectCache.RemoveLivingDataObject(this.GetType(), this.__PrimaryKey);
                    */
        }

        /// <summary>
        /// Получить проинициализированные свойства, собственные и мастеровые (загруженные+означенные)
        /// </summary>
        /// <returns>строковый массив имён свойств</returns>
        public string[] GetInitializedProperties()
        {
            return GetInitializedProperties(true);
        }

        /// <summary>
        /// Выполняется метод получения проинициализированных свойств public string[] GetInitializedProperties(bool WithMasters)
        /// </summary>
        protected bool bInGetInitializedProperties = false;

        /// <summary>
        /// Получить проинициализированные свойства (загруженные+означенные).
        /// </summary>
        /// <param name="withMasters">Если True, мастеровые учитываются.</param>
        /// <returns>Строковый массив имён свойств.</returns>
        public string[] GetInitializedProperties(bool withMasters)
        {
            if (!bInGetInitializedProperties)
            {
                try
                {
                    bInGetInitializedProperties = true;
                    List<string> lp = GetLoadedPropertiesList();
                    string[] ap = GetAlteredPropertyNames(true);
                    System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
                    sc.AddRange(lp.ToArray());
                    foreach (string s in ap)
                    {
                        if (!lp.Contains(s))
                            sc.Add(s);
                    }

                    if (withMasters)
                    {
                        Type thisType = GetType();
                        Type doType = typeof(DataObject);
                        string[] sca = new string[sc.Count];
                        sc.CopyTo(sca, 0);
                        foreach (string s in sca)
                        {
                            Type propType = Information.GetPropertyType(thisType, s);
                            if (propType.IsSubclassOf(doType))
                            {
                                DataObject mpValue = (DataObject)Information.GetPropValueByName(this, s);
                                if (mpValue != null)
                                {
                                    string[] mp = mpValue.GetInitializedProperties();
                                    foreach (string ss in mp)
                                        sc.Add(s + "." + ss);
                                }
                            }
                        }
                    }

                    string[] propnames = new string[sc.Count]; 
                    sc.CopyTo(propnames, 0);
                    return propnames;
                }
                finally
                {
                    bInGetInitializedProperties = false;
                }
            }

            return new string[] { };
        }

        /// <summary>
        /// Установить первичный ключ
        /// </summary>
        /// <param name="value"></param>
        private void SetKey1(object value, DataObjectCache DataObjectCache)
        {
            if (value != null && !value.Equals(primaryKey))
            {
                object oldkey = primaryKey;
                if (array != null)
                {
                    if (array.keys.ContainsKey(value))
                        throw new DetailArrayAlreadyContainsObjectWithThatKeyException();
                    else
                    {
                        object val = array.keys[oldkey];
                        array.keys.Remove(oldkey);
                        array.keys.Add(value, val);
                    }
                }
                primaryKey = value;
                if (oldkey != null)
                    DataObjectCache.ChangeKeyForLivingDataObject(this, oldkey);
            }
        }

        /// <summary>
        /// Установить первичный ключ
        /// </summary>
        /// <param name="value"></param>
        private void SetKey(object value)
        {
            if (value != null && !value.Equals(primaryKey))
            {
                object oldkey = primaryKey;
                if (array != null)
                {
                    if (array.keys.ContainsKey(value))
                        throw new DetailArrayAlreadyContainsObjectWithThatKeyException();
                    object val = array.keys[oldkey];
                    array.keys.Remove(oldkey);
                    array.keys.Add(value, val);
                }

                primaryKey = value;
            }
        }

        /// <summary>
        /// Установка/получение первичного ключа
        /// </summary>
        [DisableAutoViewed]
        public virtual object __PrimaryKey
        {
            get
            {
                return primaryKey;
            }
            set
            {
                Type keyType = KeyGenerator.KeyType(this);
                if (value != null)
                {
                    Type valueType = value.GetType();

                    if (valueType == keyType)
                        SetKey(value);
                    else
                    {
                        if (Convertors.InOperatorsConverter.CanConvert(valueType, keyType))
                            SetKey(Convertors.InOperatorsConverter.Convert(value, keyType));
                        else
                            throw new PrimaryKeyTypeException();
                    }
                }
                else
                {
                    throw new PrimaryKeyTypeException();
                }
            }
        }

        /// <summary>
        /// Получение статуса
        /// </summary>
        public ObjectStatus GetStatus()
        {
            if (IsDataCopy)
                return state;

            if (state == ObjectStatus.Deleted)//Братчиков: 2010-05-26 Для ускорения, поскольку для удалённых объектов нет смысла пересчитывать изменённые свойства
            {
                return state;
            }
            if (Information.AutoAlteredClass(GetType()))
            {
                if ((state == ObjectStatus.Altered || state == ObjectStatus.UnAltered))
                {
                    fieldAlteredpropertyNames = null;//потому как эта штука всё помнит, а нам сейчас это не нужно, мы бегали не по всем свойствам
                    state = ContainsAlteredProps() ? ObjectStatus.Altered : ObjectStatus.UnAltered;
                }
                else
                    GetAlteredPropertyNames();
            }
            return state;
        }

        /// <summary>
        /// Получение статуса (можно отменить автоматическое вычисление статуса)
        /// </summary>
        /// <param name="recountIfAutoaltered">перевычислять если класс с автоматическим вычислением статуса </param>
        /// <returns></returns>
        public ObjectStatus GetStatus(bool recountIfAutoaltered)
        {
            if (IsDataCopy)
                return state;
            if (recountIfAutoaltered)
                return GetStatus();
            return state;
        }

        /// <summary>
        /// Получение состояния загрузки
        /// </summary>
        public LoadingState GetLoadingState() { return loading; }

        /// <summary>
        /// Установка статуса
        /// </summary>
        public virtual void SetStatus(ObjectStatus newState)
        {
            switch (newState)
            {
                case ObjectStatus.Altered:
                    {
                        switch (state)
                        {
                            case ObjectStatus.Deleted:
                                if (loading == LoadingState.NotLoaded)
                                    state = ObjectStatus.Created;
                                else
                                    state = ObjectStatus.Altered;
                                break;
                            case ObjectStatus.UnAltered:
                                state = ObjectStatus.Altered;
                                break;
                        }
                        break;
                    }
                case ObjectStatus.Created:
                    state = ObjectStatus.Created;
                    loading = LoadingState.NotLoaded;
                    break;
                case ObjectStatus.Deleted:
                    state = ObjectStatus.Deleted;
                    break;
                case ObjectStatus.UnAltered:
                    {
                        switch (state)
                        {
                            case ObjectStatus.Altered:
                                state = ObjectStatus.UnAltered;
                                break;
                            case ObjectStatus.Deleted:
                                if (loading == LoadingState.NotLoaded)
                                    state = ObjectStatus.Created;
                                else
                                    state = ObjectStatus.UnAltered;
                                break;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Установка состояния загрузки.
        /// </summary>
        public void SetLoadingState(LoadingState newState)
        {
            if (newState == LoadingState.NotLoaded)
            {
                if (state == ObjectStatus.Altered || state == ObjectStatus.UnAltered)
                    state = ObjectStatus.Created;
            }
            else
            {
                if (state == ObjectStatus.Created)
                {
                    state = ObjectStatus.UnAltered;
                }
            }

            loading = newState;
        }

        /// <summary>
        /// Получение списка свойств, значения в которые установлены 
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        public string[] GetLoadedProperties()
        {
            if (LoadedProperties == null) 
                LoadedProperties = new string[0];
            return (string[])LoadedProperties.Clone();
        }

        /// <summary>
        /// Получение списка свойств, значения в которые установлены 
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        /// <returns></returns>
        public List<string> GetLoadedPropertiesList()
        {
            if (LoadedProperties == null)
                return new List<string>();
            return new List<string>(LoadedProperties);
        }


        /// <summary>
        /// Установить список свойств, значения в которые установлены 
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        public void SetLoadedProperties(params string[] loadedProperties)
        {
            LoadedProperties = (loadedProperties == null) ? new string[0] : (string[])loadedProperties.Clone();
        }

        /// <summary>
        /// Добавить список свойств, значения в которые установлены 
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        /// <param name="addingLoadedProperties">Массив добавляемых свойств.</param>
        public void AddLoadedProperties(params string[] addingLoadedProperties)
        {
            if (LoadedProperties == null) LoadedProperties = new string[0];
            System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
            sc.AddRange(LoadedProperties);
            foreach (string s in addingLoadedProperties)
                if (!sc.Contains(s))
                    sc.Add(s);
            LoadedProperties = new String[sc.Count];
            sc.CopyTo(LoadedProperties, 0);
            sc.Clear();
        }

        /// <summary>
        /// Добавить список свойств, значения в которые установлены 
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        /// <param name="propertyNamesList">Массив добавляемых свойств.</param>
        public void AddLoadedProperties(List<string> propertyNamesList)
        {
            if (propertyNamesList == null || propertyNamesList.Count == 0)
            {
                return;
            }

            if (LoadedProperties != null)
            {
                foreach (string property in LoadedProperties)
                {
                    if (!propertyNamesList.Contains(property))
                    {
                        propertyNamesList.Add(property);
                    }
                }
            }

            LoadedProperties = propertyNamesList.ToArray();
        }

        /// <summary>
        /// Проверить, установлено ли значение в указанное свойство
        /// (требуется в случае, когда состояние загрузки -- LightLoaded).
        /// </summary>
        public bool CheckLoadedProperty(string propertyName)
        {
            List<string> lp = GetLoadedPropertiesList();
            return lp.Contains(propertyName);
        }


        private void prvCopyTo_initDataCopy(DataObject toObject, DataObjectCache DataObjectCache)
        {
            System.Type thisType = this.GetType();
            toObject.Clear();
            FieldInfo[] fis = GetPrivateFields();
            System.Type dotype = typeof(DataObject);
            System.Type datype = typeof(DetailArray);

            string s = Information.GetAgregatePropertyName(thisType);
            object agregator = (s == null || s == string.Empty) ? null : Information.GetPropValueByName(this, s);

            //object p = null;
            foreach (FieldInfo fi in fis)
            {
                string name = fi.Name;
                if (name == "ClippingCacheOnCopy" ||
                    name == "IsDataCopy" ||
                    name == "bInGetInitializedProperties" ||
                    name == "PrimaryKeyIsUnique" ||
                    name == "fieldAlteredpropertyNames" ||
                    name == "inInitDataCopy" ||
                    name == "DisabledInitDataCopy") continue;

                GetHandler getHandler;
                SetHandler setHandler;

                if (!cacheSetHandler.TryGetValue(fi, out setHandler))
                {
                    lock (cacheSetHandler)
                    {
                        if (!cacheSetHandler.ContainsKey(fi))
                        {
                            setHandler = DynamicMethodCompiler.CreateSetHandler(thisType, fi);
                            cacheSetHandler.Add(fi, setHandler);
                        }
                    }
                }

                if (!cacheGetHandler.TryGetValue(fi, out getHandler))
                {
                    lock (cacheGetHandler)
                    {
                        if (!cacheGetHandler.ContainsKey(fi))
                        {
                            getHandler = DynamicMethodCompiler.CreateGetHandler(thisType, fi);
                            cacheGetHandler.Add(fi, getHandler);
                        }
                    }
                }

                //fieldval = fi.GetValue(this);
                object fieldval;

                try
                {
                    if (getHandler==null)
                        fieldval = fi.GetValue(this);
                    else
                        fieldval = getHandler(this);
                }
                catch
                {
                    fieldval = fi.GetValue(this);
                }

                if (fieldval != null)
                {
                    System.Type tp = fi.FieldType;
                    if (tp.IsSubclassOf(dotype))
                    {
                        if (fieldval == agregator)
                        {
                            System.Type motype = fieldval.GetType();
                            System.Object primKey = ((DataObject)fieldval).__PrimaryKey;
                            DataObject newobj = DataObjectCache.GetLivingDataObject(motype, primKey);
                            if (newobj == null)
                            {
                                newobj = DataObjectCache.CreateDataObject(motype, primKey);
                                ((DataObject)fieldval).prvCopyTo_initDataCopy(newobj, DataObjectCache);
                            }
                            newobj.IsDataCopy = true;
                            //fi.SetValue(toObject, newobj);
                            try
                            {
                                setHandler(toObject, newobj);
                            }
                            catch
                            {
                                fi.SetValue(toObject, newobj);
                            }
                        }
                        else
                        {
                            DataObject old = (DataObject)fieldval;
                            bool prevClipping = old.ClippingCacheOnCopy;
                            old.ClippingCacheOnCopy = false;
                            if (old.dataCopy == null)
                                old.InitDataCopy(DataObjectCache);
                            else if (
                                DataObjectCache.GetLivingDataObject(old.dataCopy.GetType(),
                                                                    old.dataCopy.__PrimaryKey) != old)
                                DataObjectCache.AddDataObject(old.dataCopy);
                            old.ClippingCacheOnCopy = prevClipping;
                            DataObject newobj = old.dataCopy;

                            //fi.SetValue(toObject, newobj);
                            try
                            {
                                setHandler(toObject, newobj);
                            }
                            catch
                            {
                                fi.SetValue(toObject, newobj);
                            }
                        }
                    }
                    else if (tp.IsSubclassOf(datype))
                    {
                        DetailArray newarr =
                            (DetailArray)
                            fieldval.GetType().GetConstructor(new Type[] { thisType }).Invoke(new object[] { this });
                        DetailArray curArr = (DetailArray)fieldval;
                        for (int i = 0; i < curArr.Count; i++)
                        {
                            DataObject arobject = curArr.ItemByIndex(i);
                            bool prevClipping = arobject.ClippingCacheOnCopy;
                            arobject.ClippingCacheOnCopy = false;
                            if (arobject.dataCopy != null)
                            {
                                DataObject livingDataObject = DataObjectCache.GetLivingDataObject(arobject.dataCopy.GetType(), arobject.dataCopy.__PrimaryKey);
                                if (livingDataObject != arobject)
                                {
                                    // Братчиков 2011-08-16: т.к. мастер при создании инициализирует уже свою копию, раньше этот метод дополнительно не выполнялся. Это приводило к тому, что у детейлов с иерархией попадались такие у которых копия данных была неполная после LoadObjects
                                    if (arobject.DynamicProperties.ContainsKey("MasterInitDataCopy"))
                                    {
                                        arobject.InitDataCopy(DataObjectCache);
                                        arobject.DynamicProperties.Remove("MasterInitDataCopy");
                                    }

                                    //arobject.InitDataCopy(DataObjectCache);
                                    //Братчиков
                                    DataObjectCache.AddDataObject(arobject);
                                }
                            }
                            else
                                arobject.InitDataCopy(DataObjectCache);

                            arobject.ClippingCacheOnCopy = prevClipping;
                            DataObject newobj = arobject.dataCopy;
                            if (newobj != null)
                            {
                                newobj.CheckDetail = false;
                                newarr.AddObject(newobj);
                                newobj.CheckDetail = true;
                            }
                        }
                        //fi.SetValue(toObject, newarr);
                        try
                        {
                            if (setHandler == null)
                                fi.SetValue(toObject, newarr);
                            else
                                setHandler(toObject, newarr);
                        }
                        catch
                        {
                            fi.SetValue(toObject, newarr);
                        }

                    }
                    else
                    {
                        //fi.SetValue(toObject, fieldval);
                        try
                        {   
                            if (setHandler==null)
                                fi.SetValue(toObject, fieldval);
                            else
                                setHandler(toObject, fieldval);
                        }
                        catch
                        {
                            fi.SetValue(toObject, fieldval);
                        }
                        //bool b = false;
                        //if (b)
                        //{
                        //    System.Reflection.FieldInfo fi1 = toObject.GetType().GetField(fi.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        //    fi1.SetValue(toObject, fieldval);
                        //}
                    }
                }
            }
        }



        private void PrvCopyToCopyObject(DataObject toObject, bool createDataObjectsCopy, bool primaryKeyCopy, DataObjectCache dataObjectCache)
        {
            Type thisType = this.GetType();
            toObject.Clear();
            FieldInfo[] fis = GetPrivateFields();
            Type dotype = typeof(DataObject);
            Type datype = typeof(DetailArray);



            foreach (FieldInfo fi in fis)
            {
                string name = fi.Name;

                if ((name == "primaryKey" && !primaryKeyCopy) ||
                    name == "ClippingCacheOnCopy" ||
                    name == "bInGetInitializedProperties" ||
                    name == "fieldAlteredpropertyNames" ||
                    name == "inInitDataCopy" ||
                    name == "PrimaryKeyIsUnique" ||
                    name == "IsDataCopy") continue;

                //object fieldval = fi.GetValue(this);
                GetHandler getHandler;
                SetHandler setHandler;

                if (!cacheSetHandler.TryGetValue(fi, out setHandler))
                {
                    lock (cacheSetHandler)
                    {
                        if (!cacheSetHandler.ContainsKey(fi))
                        {
                            setHandler = DynamicMethodCompiler.CreateSetHandler(thisType, fi);
                            cacheSetHandler.Add(fi, setHandler);
                        }
                    }
                }

                if (!cacheGetHandler.TryGetValue(fi, out getHandler))
                {
                    lock (cacheGetHandler)
                    {
                        if (!cacheGetHandler.ContainsKey(fi))
                        {
                            getHandler = DynamicMethodCompiler.CreateGetHandler(thisType, fi);
                            cacheGetHandler.Add(fi, getHandler);
                        }
                    }
                }

                //fieldval = fi.GetValue(this);
                object fieldval;
                try
                {
                    fieldval = getHandler(this);
                }
                catch
                {
                    fieldval = fi.GetValue(this);
                }

                if (fieldval != null)
                {
                    if (createDataObjectsCopy)
                    {
                        Type tp = fi.FieldType;
                        if (tp.IsSubclassOf(dotype))
                        {
                            Type motype = fieldval.GetType();
                            Object primKey = ((DataObject)fieldval).__PrimaryKey;
                            DataObject newobj = dataObjectCache.GetLivingDataObject(motype, primKey);
                            if (newobj == null)
                            {
                                newobj = dataObjectCache.CreateDataObject(motype, primKey);
                                ((DataObject)fieldval).PrvCopyToCopyObject(newobj, true, primaryKeyCopy,
                                                                            dataObjectCache);
                            }
                            //fi.SetValue(toObject, newobj);
                            try
                            {
                                setHandler(toObject, newobj);
                            }
                            catch
                            {
                                fi.SetValue(toObject, newobj);
                            }
                        }
                        else if (tp.IsSubclassOf(datype))
                        {
                            DetailArray newarr =
                                (DetailArray)
                                fieldval.GetType().GetConstructor(new[] { thisType }).Invoke(new object[] { toObject });
                            DetailArray curArr = (DetailArray)fieldval;
                            for (int i = 0; i < curArr.Count; i++)
                            {
                                DataObject arobject = curArr.ItemByIndex(i);
                                Type detotype = arobject.GetType();
                                Object primKey;
                                if (!primaryKeyCopy)
                                {
                                    primKey = KeyGenerator.Generate(detotype, null);
                                }
                                else
                                {
                                    primKey = arobject.__PrimaryKey;
                                }

                                DataObject newobj = dataObjectCache.GetLivingDataObject(detotype, primKey);
                                if (newobj == null)
                                {
                                    newobj = dataObjectCache.CreateDataObject(detotype, primKey);
                                    arobject.PrvCopyToCopyObject(newobj, true, primaryKeyCopy, dataObjectCache);
                                    if (!primaryKeyCopy)
                                    {
                                        newobj.SetStatus(ObjectStatus.Created);
                                        newobj.clearDataCopy();
                                    }
                                }
                                newobj.CheckDetail = false;
                                newarr.AddObject(newobj);
                                newobj.CheckDetail = true;
                            }
                            //fi.SetValue(toObject, newarr);
                            try
                            {
                                setHandler(toObject, newarr);
                            }
                            catch
                            {
                                fi.SetValue(toObject, newarr);
                            }
                        }
                        else
                        {
                            //fi.SetValue(toObject, fieldval);
                            try
                            {
                                setHandler(toObject, fieldval);
                            }
                            catch
                            {
                                fi.SetValue(toObject, fieldval);
                            }
                        }
                    }
                    else
                    {
                        //fi.SetValue(toObject, fieldval);
                        try
                        {
                            setHandler(toObject, fieldval);
                        }
                        catch
                        {
                            fi.SetValue(toObject, fieldval);
                        }
                    }
                }
            }
        }


        ///<summary>
        /// Копирование объектов без применения кэширования
        ///</summary>
        ///<param name="toObject">Объект, в который копируем (если будет null, то создадим по типу исходного)</param>
        ///<param name="createDataObjectsCopy">Запускать ли механизм копирования для мастеров и детейлов или ограничиться только своими свойствами (публичными и приватными)</param>
        ///<param name="primaryKeyCopy">Копировать ли первичный ключ</param>
        public void CopyToObjectWithoutCache(ref DataObject toObject, bool createDataObjectsCopy, bool primaryKeyCopy)
        {
            PrvCopyToObjectWithoutCache(ref toObject, createDataObjectsCopy, primaryKeyCopy, null);
        }

        ///<summary>
        /// Копирование объектов без применения кэширования
        ///</summary>
        ///<param name="toObject">Объект, в который копируем (если будет null, то создадим по типу исходного)</param>
        ///<param name="createDataObjectsCopy">Запускать ли механизм копирования для мастеров и детейлов или ограничиться только своими свойствами (публичными и приватными)</param>
        ///<param name="primaryKeyCopy">Копировать ли первичный ключ</param>
        ///<param name="usedDobjs">Список объектов, которые уже скопировали - борьба с зацикливанием</param>
        private void PrvCopyToObjectWithoutCache(ref DataObject toObject, bool createDataObjectsCopy, bool primaryKeyCopy, Hashtable usedDobjs)
        {
            if (usedDobjs == null)
            {
                usedDobjs = new Hashtable();
            }
            //проверим, не было ли этого объекта уже
            Type thisObjType = GetType();
            if (usedDobjs.ContainsKey(this))
            {
                toObject = (DataObject)usedDobjs[this];
                return;
            }

            string temp = string.Empty;

            if (toObject == null)
            {
                toObject = (DataObject)DataObjectCache.Creator.CreateObject(thisObjType);
            }
            else
            {
                toObject.Clear();
            }
            usedDobjs.Add(this, toObject);

            FieldInfo[] fis = GetPrivateFields();
            Type dotype = typeof(DataObject);
            Type datype = typeof(DetailArray);

            foreach (FieldInfo fi in fis)
            {
                string name = fi.Name;

                if ((name == "primaryKey" && !primaryKeyCopy) ||
                    name == "ClippingCacheOnCopy" ||
                    name == "bInGetInitializedProperties" ||
                    name == "PrimaryKeyIsUnique" ||
                    name == "fieldAlteredpropertyNames" ||
                    name == "inInitDataCopy" ||
                    name == "IsDataCopy") continue;

                //object fieldval = fi.GetValue(this);
                GetHandler getHandler;
                SetHandler setHandler;

                if (!cacheSetHandler.TryGetValue(fi, out setHandler))
                {
                    lock (cacheSetHandler)
                    {
                        if (!cacheSetHandler.ContainsKey(fi))
                        {
                            setHandler = DynamicMethodCompiler.CreateSetHandler(thisObjType, fi);
                            cacheSetHandler.Add(fi, setHandler);
                        }
                    }
                }

                if (!cacheGetHandler.TryGetValue(fi, out getHandler))
                {
                    lock (cacheGetHandler)
                    {
                        if (!cacheGetHandler.ContainsKey(fi))
                        {
                            getHandler = DynamicMethodCompiler.CreateGetHandler(thisObjType, fi);
                            cacheGetHandler.Add(fi, getHandler);
                        }
                    }
                }

                //fieldval = fi.GetValue(this);
                object fieldval;
                try
                {
                    fieldval = getHandler(this);
                }
                catch
                {
                    fieldval = fi.GetValue(this);
                }

                if (fieldval != null)
                {
                    if (createDataObjectsCopy)
                    {
                        Type tp = fi.FieldType;
                        if (tp.IsSubclassOf(dotype))
                        {
                            Type motype = fieldval.GetType();
                            Object primKey = ((DataObject)fieldval).__PrimaryKey;

                            primKey = Information.TranslateValueToPrimaryKeyType(motype, primKey);
                            if (primKey.ToString() == "{fbd82b95-cba3-4f84-9fc2-cbb08ce9da9a}")
                            {
                                //этот бред взят из DataObjectCache.CreateDataObject потому что есть подозрение на логику в ToString()
                                temp = "oiphohoih";
                            }
                            DataObject newobj = (DataObject)DataObjectCache.Creator.CreateObject(motype);
                            newobj.__PrimaryKey = primKey;


                            //newobj  = dataObjectCache.CreateDataObject(motype, primKey);

                            ((DataObject)fieldval).PrvCopyToObjectWithoutCache(ref newobj, true, primaryKeyCopy,
                                                                                usedDobjs);
                            //fi.SetValue(toObject, newobj);
                            try
                            {
                                setHandler(toObject, newobj);
                            }
                            catch
                            {
                                fi.SetValue(toObject, newobj);
                            }
                        }
                        else if (tp.IsSubclassOf(datype))
                        {
                            DetailArray newarr =
                                (DetailArray)
                                fieldval.GetType().GetConstructor(new Type[] { this.GetType() }).Invoke(new object[] { toObject });
                            DetailArray curArr = (DetailArray)fieldval;
                            for (int i = 0; i < curArr.Count; i++)
                            {
                                DataObject arobject = curArr.ItemByIndex(i);
                                Type detotype = arobject.GetType();
                                Object primKey = arobject.__PrimaryKey;
                                if (!primaryKeyCopy)
                                    primKey = KeyGenerator.Generate(detotype, null);

                                primKey = Information.TranslateValueToPrimaryKeyType(detotype, primKey);
                                if (primKey.ToString() == "{fbd82b95-cba3-4f84-9fc2-cbb08ce9da9a}")
                                {
                                    temp = "oiphohoih";
                                }
                                DataObject newobj = (DataObject)DataObjectCache.Creator.CreateObject(detotype);
                                newobj.__PrimaryKey = primKey;


                                //DataObject newobj = dataObjectCache.CreateDataObject(detotype, primKey);
                                arobject.PrvCopyToObjectWithoutCache(ref newobj, true, primaryKeyCopy, usedDobjs);
                                if (!primaryKeyCopy)
                                {
                                    newobj.SetStatus(ObjectStatus.Created);
                                    newobj.clearDataCopy();
                                }
                                newobj.CheckDetail = false;
                                newarr.AddObject(newobj);
                                newobj.CheckDetail = true;
                            }
                            //fi.SetValue(toObject, newarr);
                            try
                            {
                                setHandler(toObject, newarr);
                            }
                            catch
                            {
                                fi.SetValue(toObject, newarr);
                            }
                        }
                        else
                        {
                            //fi.SetValue(toObject, fieldval);
                            try
                            {
                                setHandler(toObject, fieldval);
                            }
                            catch
                            {
                                fi.SetValue(toObject, fieldval);
                            }
                        }
                    }
                    else
                    {
                        //fi.SetValue(toObject, fieldval);
                        try
                        {
                            setHandler(toObject, fieldval);
                        }
                        catch
                        {
                            fi.SetValue(toObject, fieldval);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Создать копию этого объекта данных (не забудьте вызвать InitDataCopy или ClearDataCopy если планируете обновлять объект в БД)
        /// </summary>
        /// <param name="toObject">куда копировать</param>
        /// <param name="CreateDataObjectsCopy">создавать ли копии связанных объектов 
        /// или ограничиться копированием ссылки</param>
        /// <param name="PrimaryKeyCopy">Копировать ли первичные ключи</param>
        /// <param name="UseParentCaching">Использовать ли вышеустановленное кеширование</param>
        public virtual void CopyTo(DataObject toObject, bool CreateDataObjectsCopy, bool PrimaryKeyCopy, bool UseParentCaching)
        {
            CopyTo(toObject, CreateDataObjectsCopy, PrimaryKeyCopy, UseParentCaching, new DataObjectCache());
        }
        /// <summary>
        /// Создать копию этого объекта данных (не забудьте вызвать InitDataCopy или ClearDataCopy если планируете обновлять объект в БД)
        /// </summary>
        /// <param name="toObject">куда копировать</param>
        /// <param name="CreateDataObjectsCopy">создавать ли копии связанных объектов 
        /// или ограничиться копированием ссылки</param>
        /// <param name="PrimaryKeyCopy">Копировать ли первичные ключи</param>
        /// <param name="UseParentCaching">Использовать ли вышеустановленное кеширование</param>
        public virtual void CopyTo(DataObject toObject, bool CreateDataObjectsCopy, bool PrimaryKeyCopy, bool UseParentCaching, DataObjectCache DataObjectCache)
        {
            //DataObjectCache DataObjectCache = new DataObjectCache();
            DataObjectCache.StartCaching(!UseParentCaching);
            try
            {
                PrvCopyToCopyObject(toObject, CreateDataObjectsCopy, PrimaryKeyCopy, DataObjectCache);
            }
            finally
            {
                DataObjectCache.StopCaching();
            }
        }

        /// <summary>
        /// Скопировать только системные свойства ("primaryKey", "prototypeKey", "readKey", "CheckDetail", "state", "DisabledInitDataCopy")
        /// </summary>
        /// <param name="toObject"></param>
        public virtual void CopySysProps(DataObject toObject)
        {
            if (toObject == null)
            {
                throw new ArgumentException("Не указан объект данных для копирования, обратитесь к разработчику", "toObject");
            }
            Type thisType = GetType();
            FieldInfo[] fis = GetPrivateFields();

            List<string> propsForCopy = new List<string>(new[] { "primaryKey", "prototypeKey", "readKey", "CheckDetail", "state", "DisabledInitDataCopy" });

            foreach (FieldInfo fi in fis)
            {
                string name = fi.Name;

                if (!propsForCopy.Contains(name)) continue;

                GetHandler getHandler;
                SetHandler setHandler;

                if (!cacheSetHandler.TryGetValue(fi, out setHandler))
                {
                    lock (cacheSetHandler)
                    {
                        if (!cacheSetHandler.ContainsKey(fi))
                        {
                            setHandler = DynamicMethodCompiler.CreateSetHandler(thisType, fi);
                            cacheSetHandler.Add(fi, setHandler);
                        }
                    }
                }

                if (!cacheGetHandler.TryGetValue(fi, out getHandler))
                {
                    lock (cacheGetHandler)
                    {
                        if (!cacheGetHandler.ContainsKey(fi))
                        {
                            getHandler = DynamicMethodCompiler.CreateGetHandler(thisType, fi);
                            cacheGetHandler.Add(fi, getHandler);
                        }
                    }
                }

                object fieldval;
                try
                {
                    fieldval = getHandler(this);
                }
                catch
                {
                    fieldval = fi.GetValue(this);
                }

                if (fieldval != null)
                {
                    try
                    {
                        setHandler(toObject, fieldval);
                    }
                    catch
                    {
                        fi.SetValue(toObject, fieldval);
                    }
                }
            }
        }



        /// <summary>
        /// Сбросить прототипизацию объекта (очистить все что относится к прототипу)
        /// </summary>
        public virtual void ClearPrototyping()
        {
            ClearPrototyping(true);
        }
        /// <summary>
        /// Сбросить прототипизацию объекта (очистить все что относится к прототипу)
        /// </summary>
        /// <param name="withDetails">с детейлами или без</param>
        public virtual void ClearPrototyping(bool withDetails)
        {
            prototypeKey = null;
            if (withDetails)
            {
                string[] properties = Information.GetAllPropertyNames(GetType());
                foreach (string property in properties)
                {
                    Type proptype = Information.GetPropertyType(GetType(), property);
                    if (proptype.IsSubclassOf(typeof(DetailArray)))
                    {
                        foreach (DataObject detobj in (DetailArray)Information.GetPropValueByName(this, property))
                            detobj.ClearPrototyping(withDetails);
                    }
                }
            }
        }

        /// <summary>
        /// Прототипизировать
        /// </summary>
        public virtual void Prototyping()
        {
            Prototyping(true);
        }

        /// <summary>
        /// Прототипизировать
        /// </summary>
        /// <param name="withDetails">с детейлами или без</param>
        public virtual void Prototyping(bool withDetails)
        {
            prototypeKey = __PrimaryKey;
            KeyGenerator.Generate(this, null);
            SetStatus(ObjectStatus.Created);
            SetLoadingState(LoadingState.NotLoaded);
            LoadedProperties = new string[0];
            InitDataCopy();
            if (withDetails)
            {
                Type type = GetType();
                string[] properties = Information.GetAllPropertyNames(type);
                foreach (string property in properties)
                {
                    Type proptype = Information.GetPropertyType(type, property);
                    if (proptype.IsSubclassOf(typeof(DetailArray)))
                    {
                        foreach (DataObject detobj in (DetailArray)Information.GetPropValueByName(this, property))
                            detobj.Prototyping(true);//потому что if (withDetails)
                    }
                }
            }
        }


        /// <summary>
        /// обрезать кэш при копировании
        /// </summary>
        internal bool ClippingCacheOnCopy = true;

        /// <summary>
        /// Не инициализировать копию данных объекта при зачитке. По-умолчанию инициализируется.
        /// </summary>
        private bool DisabledInitDataCopy = false;

        /// <summary>
        /// Не инициализировать копию данных объекта при зачитке. По-умолчанию инициализируется.
        /// </summary>
        public void DisableInitDataCopy()
        {
            DisabledInitDataCopy = true;
        }

        /// <summary>
        /// Включить инициализацию копии данных объекта при зачитке. По-умолчанию инициализируется.
        /// </summary>
        public void EnableInitDataCopy()
        {
            DisabledInitDataCopy = false;
        }


        /// <summary>
        /// Инициализация внутренней копии данных объекта данных
        /// </summary>
        private bool inInitDataCopy = false;




        /// <summary>
        /// Проинициализировать копию данных
        /// </summary>
        public void InitDataCopy(DataObjectCache DataObjectCache=null, bool exceptAlteredProperies=false)
        {
            if (DisabledInitDataCopy) return;

            if (inInitDataCopy) return;
            inInitDataCopy = true;
            if (DataObjectCache == null) DataObjectCache = new DataObjectCache();

            Dictionary<string, object> AlteredValues = new Dictionary<string, object>();
            //сохраним предыдущие значения (установив временно их копии)
            if (exceptAlteredProperies)
            {
                foreach (string s in this.GetAlteredPropertyNames(false))
                {
                    if (!(Information.GetPropValueByName(this, s) is DetailArray))
                    {
                        AlteredValues.Add(s, Information.GetPropValueByName(this, s));
                        Information.SetPropValueByName(this, s, Information.GetPropValueByName(this.dataCopy, s));
                    }
                }
            }
            if (GetStatus(false)!=ObjectStatus.Deleted)
                SetStatus(ObjectStatus.UnAltered);
            if (GetStatus(false) != ObjectStatus.Created)
            {
                DataObjectCache.StartCaching(ClippingCacheOnCopy);
                try
                {
                    dataCopy = DataObjectCache.CreateDataObject(this.GetType(), __PrimaryKey);
                    
                    prvCopyTo_initDataCopy(dataCopy, DataObjectCache);
                }
                finally
                {
                    DataObjectCache.StopCaching();
                }
                if (dataCopy != null)
                {
                    dataCopy.IsDataCopy = true;
                }
            }
            else
            {
                if (Prototyped)
                {
                    string[] props = Information.GetAllPropertyNames(GetType());
                    foreach (string prop in props)
                    {
                        Type proptype = Information.GetPropertyType(GetType(), prop);
                        if (proptype.IsSubclassOf(typeof(DetailArray)))
                        {
                            DetailArray dar = (DetailArray)Information.GetPropValueByName(this, prop);
                            if (dar != null)
                                foreach (DataObject d in dar)
                                    d.InitDataCopy(DataObjectCache,exceptAlteredProperies);
                        }
                    }
                }
                dataCopy = null;
            }

            if (exceptAlteredProperies)
            {
                foreach (KeyValuePair<string, object> kvp in AlteredValues)
                {
                    if (!(kvp.Value is DetailArray))
                        Information.SetPropValueByName(this, kvp.Key, kvp.Value);
                }
            }
            inInitDataCopy = false;
        }

        /// <summary>
        /// Очистить внутреннюю копию данных
        /// </summary>
        public void clearDataCopy()
        {
            dataCopy = null;
        }

        /// <summary>
        /// Очистка внутренней копии данных в собственном объекте, а также рекурсивно копии мастеровых и детейловых объектов
        /// </summary>
        public void FullClearDataCopy()
        {
            if (this.inInitDataCopy) return;

            this.inInitDataCopy = true;

            try
            {

                FieldInfo[] fis = GetPrivateFields();
                System.Type dotype = typeof(DataObject);
                System.Type datype = typeof(DetailArray);

                foreach (FieldInfo fi in fis)
                    if ((fi.Name != "ClippingCacheOnCopy") && (fi.Name != "IsDataCopy"))
                    {
                        this.clearDataCopy();
                        object fieldval = fi.GetValue(this);
                        if (fieldval != null)
                        {
                            System.Type tp = fi.FieldType;
                            if (tp.IsSubclassOf(dotype))
                            {
                                ((DataObject)fieldval).FullClearDataCopy();
                            }
                            if (tp.IsSubclassOf(datype))
                            {
                                DetailArray da = ((DetailArray)fieldval);
                                for (int i = 0; i < da.Count; i++)
                                {
                                    da.ItemByIndex(i).FullClearDataCopy();
                                }
                            }
                        }
                    }
            }
            finally
            {
                this.inInitDataCopy = false;
            }
        }

        /// <summary>
        /// Получить внутреннюю копию объекта данных
        /// </summary>
        public DataObject GetDataCopy() { return dataCopy; }

        /// <summary>
        /// Установить внутреннюю копию объекта данных
        /// </summary>
        /// <param name="value">Устанавливаемый объект как копия существующего </param>
        public void SetDataCopy(DataObject value) { dataCopy = value; }

        /// <summary>
        /// Возвращает список свойств (атрибутов, мастеров, детейлов),
        /// чьи значения изменились по сравнению с внутренней копией
        /// </summary>
        public string[] GetAlteredPropertyNames(bool Recount)
        {
            if (Recount)
                return GetAlteredPropertyNames();
            else if (fieldAlteredpropertyNames == null)
                return GetAlteredPropertyNames();
            else
                return (fieldAlteredpropertyNames == null) ? null : (string[])fieldAlteredpropertyNames.Clone();
        }
        /// <summary>
        /// Возвращает список свойств (атрибутов, мастеров, детейлов),
        /// чьи значения изменились по сравнению с внутренней копией
        /// </summary>
        /// <returns></returns>
        public string[] GetAlteredPropertyNames()
        {
            fieldAlteredpropertyNames = Information.GetAlteredPropertyNames(this, dataCopy, true);
            return (fieldAlteredpropertyNames == null) ? null : (string[])fieldAlteredpropertyNames.Clone();
        }

        ///<summary>
        /// Проверить, есть ли это свойство в списке изменённых. Выполняется полная проверка каждый раз, поэтому метод не очень производительный. 
        ///</summary>
        ///<param name="propName"></param>
        /// <remarks>Если этого свойства нет в объекте, то не упадёт, а просто скажет что оно не менялось, имейте в виду</remarks>
        ///<returns></returns>
        public bool IsAlteredProperty(string propName)
        {
            var lst = new List<string>();
            lst.AddRange(GetAlteredPropertyNames());
            return lst.Contains(propName);
        }
        /// <summary>
        /// Было ли изменение значений свойств по сравнению с внутренней копией
        /// </summary>
        /// <returns></returns>
        public bool ContainsAlteredProps()
        {
            return Information.ContainsAlteredProps(this, dataCopy, true);
        }

        //private static Collections.TypeBaseCollection fieldsCollection = new ICSSoft.STORMNET.Collections.TypeBaseCollection();
        /// <summary>
        /// Кэш массивов приватных полей
        /// </summary>
        private static Dictionary<Type, FieldInfo[]> fieldsCollection = new Dictionary<Type, FieldInfo[]>();

        /// <summary>
        /// константа для блокирования межпотокового доступа
        /// </summary>
        private static string m_ObjNull = "CONST";

        /// <summary>
        /// Возвращает массив приватных полей
        /// </summary>
        /// <returns>массив приватных полей</returns>
        private FieldInfo[] GetPrivateFields()
        {
            Type thisType = GetType();
            if (!fieldsCollection.ContainsKey(thisType))
            {
                lock (m_ObjNull)
                {
                    if (!fieldsCollection.ContainsKey(thisType))
                    {
                        ArrayList resarr = new ArrayList();
                        Type ct = thisType;
                        while (true)
                        {
                            FieldInfo[] fis = ct.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                            resarr.AddRange(fis);
                            if (ct == typeof(DataObject))
                                break;
                            ct = ct.BaseType;
                        }
                        FieldInfo[] res = new FieldInfo[resarr.Count];
                        resarr.CopyTo(res);
                        fieldsCollection.Add(thisType, res);
                    }
                }
            }
            return (FieldInfo[])fieldsCollection[thisType].Clone();
        }


        /// <summary>
        /// Очистка объекта данных.
        /// Остается означеным только первичный ключ и вычислимые свойства (если такая возможность предусмотрена)
        /// Объект получает статусы ObjectStatus.UnAltered,LoadingState.NotLoaded
        /// </summary>
        public virtual void Clear()
        {
            FieldInfo[] fis = GetPrivateFields();
            foreach (FieldInfo fi in fis)
            {
                string name = fi.Name;
                if (name == "primaryKey" ||
                    name == "PrimaryKeyIsUnique" ||
                    name == "bInGetInitializedProperties" ||
                    name == "PrimaryKeyIsUnique" ||
                    name == "inInitDataCopy" ||
                    name == "ClippingCacheOnCopy" ||
                    name == "state" ||
                    name == "loading")
                    continue;
                //fi.SetValue(this, null);
                if (fi.FieldType.IsValueType)
                {
                    fi.SetValue(this, null);
                }
                else
                {
                    try
                    {
                        SetHandler fiSetValue;
                        if (!cacheSetHandler.TryGetValue(fi, out fiSetValue))
                        {
                            lock (cacheSetHandler)
                            {
                                if (!cacheSetHandler.ContainsKey(fi))
                                {
                                    fiSetValue = DynamicMethodCompiler.CreateSetHandler(GetType(), fi);
                                    cacheSetHandler.Add(fi, fiSetValue);
                                }
                            }
                        }
                        if (fiSetValue!=null)
                            fiSetValue(this, null);
                        else
                            fi.SetValue(this, null);
                    }
                    catch
                    {
                        fi.SetValue(this, null);
                    }
                }
            }
            ClippingCacheOnCopy = true;
            SetStatus(ObjectStatus.Created);
            SetLoadingState(LoadingState.NotLoaded);
        }

        /// <summary>
        /// Преобразуем объект данных в его строковое представление.
        /// При этом включаются все свойства объекта, в том числе динамические; нединамические свойства сортируются по алфавиту.
        /// </summary>
        /// <returns>Сформированное строковое представление объекта данных.</returns>
        public override string ToString()
        {
            System.Type curType = this.GetType();
            string[] propNames = Information.GetAllPropertyNames(curType);
            return ToStringAllData(propNames, true, true);
        }

        /// <summary>
        /// Преобразуем объект данных в его строковое представление.
        /// При этом не включаются динамические свойства, свойства сортируются по алфавиту.
        /// </summary>
        /// <param name="propNames">
        /// Свойства, значения которых будут включены в строковое представление.
        /// Если будет передано <c>null</c>, то в результате не будет отображаться ни одно свойство.
        /// </param>
        /// <returns>Сформированное строковое представление объекта данных.</returns>
        public string ToString(string[] propNames)
        {
            return ToStringAllData(propNames, false, true);
        }

        /// <summary>
        /// Преобразование к строке только по видимым нединамическим свойствам (используется в аудите).
        /// При этом не включаются динамические свойства, свойства не сортируются по алфавиту (пишутся по представлению уже так, как пользователь задал).
        /// </summary>
        /// <param name="stringView">
        /// Представление, по которому нужно создавать строковое представление (берётся видимость полей и заголовки классов).
        /// Если передано <c>null</c>, то будут взяты все загруженные свойства и записаны в алфавитном порядке.
        /// </param>
        /// <returns> Представление объекта данных в виде строки. </returns>
        public string ToStringForAudit(View stringView)
        {
            string propertiesString;
            if (stringView == null)
            {
                propertiesString = ToStringAllData(GetLoadedProperties(), false, true);
            }
            else
            {
                // Пишем только видимые поля.
                IEnumerable<PropertyInView> tostringPropertyList = stringView.Properties.Where(x => x.Visible);
                string[] resultData = tostringPropertyList
                    .Select(propertyInView => ToStringPropertyWithCaptionForAudit(propertyInView.Name, propertyInView.Caption))
                    .Where(propertyString => !string.IsNullOrEmpty(propertyString))
                    .ToArray();
                propertiesString = string.Join(", ", resultData);
            }

            return ToStringFormResult(propertiesString);
        }

        /// <summary>
        /// Преобразование в строковое представление собственных, нединамических свойств объекта для аудита.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="propertyCaption">Заголовок для свойства.</param>
        /// <returns>Сформированное строковое представление для значения свойства объекта.</returns>
        internal string ToStringPropertyWithCaptionForAudit(string propertyName, string propertyCaption)
        {
            object val = Information.GetPropValueByName(this, propertyName) ?? "null";

            return string.Format(
                "{0}={1}",
                string.IsNullOrEmpty(propertyCaption) ? propertyName : propertyCaption,
                val is DataObject ? ((DataObject)val).__PrimaryKey.ToString() : val.ToString());
        }
        
        /// <summary>
        /// Преобразование в строковое представление собственных, нединамических свойств объекта.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="propertyCaption">Заголовок для свойства.</param>
        /// <returns>Сформированное строковое представление для значения свойства объекта.</returns>
        internal string ToStringPropertyWithCaption(string propertyName, string propertyCaption)
        {
            object val;
            Type curType = this.GetType();
            if (Information.GetPropertyDisableAutoViewing(curType, propertyName)
                    || !Information.IsStoredProperty(curType, propertyName)
                    || (val = Information.GetPropValueByName(this, propertyName)) == null)
            {
                return null;
            }

            return string.Format(
                "{0}={1}",
                string.IsNullOrEmpty(propertyCaption) ? propertyName : propertyCaption,
                val is DataObject ? ((DataObject)val).__PrimaryKey.ToString() : val.ToString());
        }

        /// <summary>
        /// Формируем из строки, сгенерированной для строкового представления значений свойств объекта, итоговое строковое представление объекта. 
        /// </summary>
        /// <param name="propertiesString">Строка, сгенерированная для строкового представления значений свойств объекта.</param>
        /// <returns>Строковое представление объекта данных.</returns>
        internal string ToStringFormResult(string propertiesString)
        {
            return string.Format("{0}({1})", GetType().Name, propertiesString);
        }

        /// <summary>
        /// Перевод объекта данных в строковое представление.
        /// </summary>
        /// <param name="propNamesArray"> Имена свойств, которые должны попасть в строковое представление. </param>
        /// <param name="includeDynamicProperties"> Включать ли динамические свойства. </param>
        /// <param name="needOrder"> Должны ли быть свойства в строковом представлении отсортированы по алфавиту. </param>
        /// <returns> Строковое представление объекта данных. </returns>
        private string ToStringAllData(string[] propNamesArray, bool includeDynamicProperties, bool needOrder)
        {
            List<string> propNames = (propNamesArray ?? new string[] { }).ToList();
            if (needOrder)
            {
                propNames.Sort();
            }

            string[] resultData = propNames
                .Select(property => ToStringPropertyWithCaption(property, null))
                .Where(propertyString => !string.IsNullOrEmpty(propertyString))
                .ToArray();
            var stringBuilder = new StringBuilder(string.Join(", ", resultData));

            if (includeDynamicProperties)
            {
                for (int i = 0; i < DynamicProperties.Count; i++)
                {
                    object val = DynamicProperties[i];
                    if (val != null)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            stringBuilder.Append(", ");
                        }

                        stringBuilder.Append(
                            string.Format(
                                "{0}={1}",
                                DynamicProperties.GetKey(i),
                                val is DataObject
                                    ? ((DataObject)val).__PrimaryKey.ToString()
                                    : val.ToString()));
                    }
                }
            }

            return ToStringFormResult(stringBuilder.ToString());
        }

        /// <summary>
        /// Найти незаполненные поля.
        /// </summary>
        /// <param name="detailSkip">Не обращать внимания на удаленные детейлы. Если детейла нет в этом словаре или значение для него <c>false</c>, то пропущен не будет. Может быть <c>null</c>.</param>
        /// <returns>Заголовки свойств незаполненных полей.</returns>
        public virtual string[] CheckNotNullProperties(Dictionary<Type, bool> detailSkip)
        {
            if (state == ObjectStatus.Deleted) return new string[0];
            Type thisType = GetType();
            ArrayList sc = new ArrayList();
            foreach (string property in Information.GetAllPropertyNames(thisType))
            {
                object val = Information.GetPropValueByName(this, property);
                if (Information.GetPropertyNotNull(thisType, property))
                {
                    if (Information.IsEmptyPropertyValue(val))
                        sc.Add(property);
                }

                DetailArray dar = val as DetailArray;
                if (dar != null)
                {
                    for (int i = 0, j = 0; i < dar.Count; i++)
                    {
                        Type detailType = dar.ItemByIndex(i).GetType();
                        bool skipDeletedDetails = false;
                        if (detailSkip != null && detailSkip.ContainsKey(detailType))
                            skipDeletedDetails = detailSkip[detailType];

                        if (!skipDeletedDetails || dar.ItemByIndex(i).GetStatus() != ObjectStatus.Deleted)
                        {
                            j++;

                            string[] props = dar.ItemByIndex(i).CheckNotNullProperties();
                            if (props.Length > 0)
                            {
                                foreach (string p in props)
                                    sc.Add($"{property}[{j}].{p}");
                            }
                        }
                    }
                }
            }
            return (string[])sc.ToArray(typeof(string));
        }

        private bool IsEnumValueEmpty(object val)
        {
            try
            {
                if (val == null)
                    return true;

                var propertyType = val.GetType();
                var fields = propertyType.GetFields();
                var usedEmptyEnumValueAttribute = false;
                foreach (var field in fields)
                {
                    if (field.IsSpecialName)
                        continue;

                    var atrs = field.GetCustomAttributes(typeof(EmptyEnumValueAttribute), true);

                    //Если текущее значение совпадает с помеченным значением, то покажем таракана.
                    if (atrs != null && atrs.Length > 0)
                    {
                        usedEmptyEnumValueAttribute = true;
                        if ( /*Enum.Parse(propertyType, field.Name) == propertyValue*/
                            Enum.GetName(propertyType, val) == field.Name)
                            return true;
                    }
                }

                var caption = EnumCaption.GetCaptionFor(val);
                if (!usedEmptyEnumValueAttribute && string.IsNullOrEmpty(caption))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                if (LogService.Log.IsWarnEnabled)
                    LogService.Log.Warn("Ошибка в методе IsEnumValueEmpty.", ex);
                return false;
            }
        }

        /// <summary>
        /// Поискать незаполенные поля.
        /// </summary>
        /// <returns>Заголовки свойств незаполненных полей.</returns>
        public virtual string[] CheckNotNullProperties()
        {
            return CheckNotNullProperties(null);
        }

        /// <summary>
        /// Найти незаполненные поля и вернуть заголовки свойств по представлению.
        /// </summary>
        /// <param name="view">Представление объекта.</param>
        /// <param name="returnCaptions">Если <c>true</c>, то вернутся заголовки свойств, иначе имена.</param>
        /// <param name="detailSkip">Не обращать внимания на удаленные детейлы. Если детейла нет в этом словаре или значение для него <c>false</c>, то пропущен не будет. Может быть <c>null</c>.</param>
        /// <returns>Заголовки свойств незаполненных полей.</returns>
        public virtual string[] CheckNotNullProperties(View view, bool returnCaptions, Dictionary<Type, bool> detailSkip)
        {
            if (state == ObjectStatus.Deleted) return new string[0];
            Type thisType = GetType();
            ArrayList sc = new ArrayList();
            foreach (PropertyInView property in view.Properties)
            {
                string propertyCaption = property.Caption;
                if (propertyCaption == null || propertyCaption.Trim() == string.Empty)
                    propertyCaption = property.Name;
                if (property.Name.IndexOf('.') == -1)
                {
                    object val = Information.GetPropValueByName(this, property.Name);
                    if (Information.GetPropertyNotNull(thisType, property.Name))
                    {
                        if (Information.IsEmptyPropertyValue(val))
                            sc.Add(returnCaptions ? propertyCaption : property.Name);
                    }
                }
            }

            foreach (DetailInView div in view.Details)
            {
                DetailArray dar = Information.GetPropValueByName(this, div.Name) as DetailArray;
                string propertyCaption = div.Caption;
                if (propertyCaption == null || propertyCaption.Trim() == string.Empty)
                    propertyCaption = div.Name;

                if (dar != null)
                {
                    for (int i = 0, j = 0; i < dar.Count; i++)
                    {
                        Type detailType = dar.ItemByIndex(i).GetType();
                        bool skipDeletedDetails = false;
                        if (detailSkip != null && detailSkip.ContainsKey(detailType))
                            skipDeletedDetails = detailSkip[detailType];

                        if (!skipDeletedDetails || dar.ItemByIndex(i).GetStatus() != ObjectStatus.Deleted)
                        {
                            j++;

                            string[] props = dar.ItemByIndex(i).CheckNotNullProperties(div.View, returnCaptions);
                            if (props.Length > 0)
                            {
                                foreach (string p in props)
                                    sc.Add($"{(returnCaptions ? propertyCaption : div.Name)}[{j}].{p}");
                            }
                        }
                    }
                }
            }

            return (string[])sc.ToArray(typeof(string));
        }

        /// <summary>
        /// Поискать незаполенные поля и возвращать заголовки свойств по представлению.
        /// </summary>
        /// <param name="view">Представление объекта.</param>
        /// <param name="returnCaptions">Возвращать имена свойств или заголовки.</param>
        /// <returns>Заголовки свойств незаполненных полей.</returns>
        public virtual string[] CheckNotNullProperties(View view, bool returnCaptions)
        {
            return CheckNotNullProperties(view, returnCaptions, null);
        }
    }

    /// <summary>
    /// Делегат для получения презентационного значения
    /// </summary>
    /// <param name="dataObject"></param>
    /// <returns></returns>
    public delegate string GetPresentationValueDelegate(DataObject dataObject);

    #endregion

    #region class DetailArray
    
    /// <summary>
    /// Контейнер (массив) детейловых объектов
    /// </summary>
    [Serializable]
    abstract public class DetailArray : IEnumerable
    {
        /// <summary>
        /// Енумератор
        /// </summary>
        private class DetailEnumumerator : IEnumerator
        {
            private DetailArray arr;
            private int curIndex;
            public DetailEnumumerator(DetailArray array)
            {
                arr = array;
                curIndex = -1;
            }
            void IEnumerator.Reset()
            { curIndex = -1; }
            bool IEnumerator.MoveNext()
            {
                if (curIndex++ >= arr.Count - 1)
                    return false;
                else
                    return true;
            }
            object IEnumerator.Current { get { return arr.ItemByIndex(curIndex); } }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DetailEnumumerator(this);
        }

        private Type objectType;
        private PropertyInfo masterProp;
        private DataObject masterObject;
        private PropertyInfo keyProp;
        private ArrayList objects;
        internal SortedList keys;
        private long fixedSize;

        /// <summary>
        /// Добавление объектов в коллекцию
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<ItemsAddedEventArgs> ItemsAdded;

        /// <summary>
        /// Очистить массив
        /// </summary>
        public void Clear()
        {
            while (objects.Count > 0)
                RemoveByIndex(0);
        }

        /// <summary>
        /// Вставить объект
        /// </summary>
        /// <param name="Index">позиция</param>
        /// <param name="obj">что вставляем</param>
        public void Insert(int Index, DataObject obj)
        {
            if (Index >= 0 && Index <= Count)
            {
                for (int i = 0; i < keys.Count; i++)
                    if (((int)keys.GetByIndex(i)) >= Index)
                        keys.SetByIndex(i, ((int)keys.GetByIndex(i)) + 1);
                objects.Insert(Index, obj);
                keys.Add(obj.__PrimaryKey, Index);
                prv_SetAggregator(obj);
                if (OrderedObjects())
                    Renumerate();
                obj.DetailArray = this;
            }
            else
                throw new ArgumentOutOfRangeException("Index");
        }

        /// <summary>
        /// Переместить объект внутри массива - remove,insert
        /// </summary>
        /// <param name="oldIndex">старая позиция</param>
        /// <param name="newIndex">новая позиция</param>
        public void Move(int oldIndex, int newIndex)
        {
            if (oldIndex >= 0 && oldIndex < Count && newIndex >= 0 && newIndex < Count && oldIndex != newIndex)
            {
                DataObject dobj = this.ItemByIndex(oldIndex);
                RemoveByIndex(oldIndex);
                Insert(newIndex, dobj);
            }
        }

        private bool orderedObjects = false;
        private bool countOrdered = false;
        private bool OrderedObjects()
        {
            if (countOrdered)
                return orderedObjects;
            countOrdered = true;
            orderedObjects = (Information.GetOrderPropertyName(objectType) != string.Empty);
            return orderedObjects;
        }

        /// <summary>
        /// Перевычисление автонумеруемых объектов
        /// </summary>
        public void Renumerate()
        {
            string orderProp = Information.GetOrderPropertyName(objectType);
            if (orderProp == string.Empty)
                throw new NotSortableDetailArrayException();
            else
            {
                try
                {
                    int index = 1;
                    for (int i = 0; i < Count; i++)
                    {
                        DataObject item = ItemByIndex(i);
                        if (item.GetStatus(false) != ObjectStatus.Deleted)
                        {
                            if ((Information.GetPropertyType(item.GetType(), orderProp).Equals(typeof(int)) && ((int)Information.GetPropValueByName(item, orderProp)) != index) || (Information.GetPropertyType(item.GetType(), orderProp).Name == "NullableInt" && !Information.GetPropValueByName(item, orderProp).Equals(index)) || ((int)Information.GetPropValueByName(item, orderProp)) != index)
                            {
                                Information.SetPropValueByName(item, orderProp, index);
                                if (item.GetStatus() == ObjectStatus.UnAltered)
                                    item.SetStatus(ObjectStatus.Altered);
                            }
                            index++;
                        }
                    }
                }
                catch
                {
                    throw new NotSortableOrderColumnsType();
                }
            }
        }


        /// <summary>
        /// Переупорядочить объекты данных в соответствии с автонумерацией
        /// </summary>
        public void Ordering()
        {
            string orderProp = Information.GetOrderPropertyName(objectType);
            if (orderProp == string.Empty)
                throw new NotSortableDetailArrayException();
            else
            {
                try
                {
                    SortedList sl = new SortedList();
                    ArrayList deleted = new ArrayList();
                    for (int i = 0; i < Count; i++)
                    {
                        DataObject item = ItemByIndex(i);
                        if (item.GetStatus(false) != ObjectStatus.Deleted)
                        {
                            //Братчиков 15.04.2008 Исправление бага: 
                            //пользователи сами лезут к orderProp и из-за этого ключи могут повториться
                            object key = Information.GetPropValueByName(item, orderProp);
                            if (key is int)
                            {
                                while (sl.ContainsKey(key))
                                {
                                    key = (int)key + 1;
                                }
                            }
                            sl.Add(key, item);
                        }
                        else
                            deleted.Add(item);

                    }
                    Clear();
                    for (int i = 0; i < sl.Count; i++)
                        AddObject((DataObject)sl.GetByIndex(i));
                    for (int i = 0; i < deleted.Count; i++)
                        AddObject((DataObject)deleted[i]);
                }
                catch
                {
                    throw new NotSortableOrderColumnsType();
                }
            }
        }


        /// <summary>
        /// Ссылка на шапку (задается при создании массива)
        /// </summary>
        public DataObject AgregatorObject { get { return masterObject; } }

        /// <summary>
        /// Возвращает тип элементов DetailArray
        /// </summary>
        public System.Type ItemType { get { return objectType; } }
        /// <summary>
        /// Размер зафиксированный для данного массива объектов
        /// </summary>
        public long FixedSize { get { return fixedSize; } set { fixedSize = value; } }

        /// <summary>
        /// Создать по типу хранимых объектов и мастеровому объекту данных
        /// </summary>
        public DetailArray(Type objecttype, DataObject masterObj)
        {
            Init(objecttype, objecttype.GetProperty("__PrimaryKey"), masterObj, -1);
        }
        /// <summary>
        /// Создать по типу хранимых объектов, мастеровому объекту данных, фиксированного размера
        /// </summary>
        public DetailArray(Type objecttype, DataObject masterObj, long size)
        {
            Init(objecttype, objecttype.GetProperty("__PrimaryKey"), masterObj, size);
        }
        /// <summary>
        /// Создать по типу хранимых объектов, информации о свойстве первичного ключа мастера и объекте данных мастера
        /// </summary>
        public DetailArray(Type objecttype, PropertyInfo key, DataObject masterObj)
        {
            Init(objecttype, key, masterObj, -1);
        }
        /// <summary>
        /// Создать по типу хранимых объектов, информации о свойстве первичного ключа мастера и объекте данных мастера, фиксированного размера
        /// </summary>
        public DetailArray(Type objecttype, PropertyInfo key, DataObject masterObj, long size)
        {
            Init(objecttype, key, masterObj, size);
        }

        private void Init(Type objecttype, PropertyInfo key, DataObject masterObj, long fixedsize)
        {
            //if ( masterObj == null ) throw new OnCreationDetailArrayAgregatorObjectCantBeNullException();
            string agregatorName = Information.GetAgregatePropertyName(objecttype);
            if (agregatorName == string.Empty)
                throw new NotFoundAggregatorProperty();
            masterProp = objecttype.GetProperty(Information.GetAgregatePropertyName(objecttype));
            objects = new ArrayList();
            keys = new SortedList();
            FixedSize = fixedsize;
            Type DataObjectType = typeof(DataObject);
            if (!objecttype.IsSubclassOf(DataObjectType))
            {
                throw new CantProcessingNonDataobjectTypeException();
            }
            else
            {
                if (!masterProp.PropertyType.IsSubclassOf(DataObjectType) && masterProp.PropertyType != DataObjectType)
                    throw new AgregatorPropertyMustBeDataObjectTypeException(objectType);
                else
                {
                    objectType = objecttype;
                    masterObject = masterObj;
                    keyProp = key;
                }
            }

        }

        /// <summary>
        /// Получить все объекты в виде одномерного массива
        /// </summary>
        /// <returns></returns>
        public DataObject[] GetAllObjects()
        {
            DataObject[] res = new DataObject[Count];
            objects.CopyTo(res);
            return res;
        }

        /// <summary>
        /// Получить объект данных по индексу
        /// </summary>
        public DataObject ItemByIndex(int index) { return (DataObject)objects[index]; }
        /*
        public DataObject this[object key]
        {
            get 
            {return (DataObject)objects[(int)keys[key]];} 
            set
            {
                if (keys.ContainsKey(key))
                    objects[(int)keys[key]]=value;
                else
                {
                    objects.Add(value);
                    keys.Add(key,objects.Count-1);
                }
            }
        }*/

        /// <summary>
        /// Получить объект данных по первичному ключу.
        /// У конкретного прикладного DetailArray можно получить объект по ключу через операцию []
        /// </summary>
        public DataObject GetByKey(object key)
        {
            //return null;
            // if (keys.Contains(null))
            if (keys.Contains(key))
                return (DataObject)objects[(int)keys[key]];
            else
                return null;
        }

        /// <summary>
        /// Установить объект данных по первичному ключу.
        /// У конкретного прикладного DetailArray можно установить объект по ключу через операцию []
        /// </summary>
        public void SetByKey(object key, DataObject value)
        {
            value.DetailArray = this;
            if (keys.ContainsKey(key))
                objects[(int)keys[key]] = value;
            else
            {
                objects.Add(value);
                keys.Add(key, objects.Count - 1);
            }
            prv_SetAggregator(value);
            if (OrderedObjects()) Renumerate();
        }


        /// <summary>
        /// Добавить объекты данных
        /// Если у объекта данных первичный ключ будет равен null, то будет сгенерирован новый ключ.
        /// </summary>
        /// <param name="dataobjects">Массив объектов данных</param>
        public virtual void AddRange(params DataObject[] dataobjects)
        {
            for (int i = 0; i < dataobjects.Length; i++)
            {
                DataObject dataobject = dataobjects[i];
                if (dataobject == null)
                {
                    continue;
                }

                if (dataobject.__PrimaryKey == null)
                {
                    KeyGenerator.Generate(dataobject, null);
                }
                if (keys.ContainsKey(dataobject.__PrimaryKey))
                    throw new DetailArrayAlreadyContainsObjectWithThatKeyException();
                dataobject.DetailArray = this;
                objects.Add(dataobject);
                keys.Add(dataobject.__PrimaryKey, objects.Count - 1);
                prv_SetAggregator(dataobject);
            }

            if (dataobjects.Length > 0 && OrderedObjects())
                Renumerate();

            OnItemsAdded(new ItemsAddedEventArgs{DataObjects = dataobjects});

        }

        /// <summary>
        /// Добавить объект данных
        /// </summary>
        public void AddObject(DataObject dataobject)
        {
            if (dataobject == null) throw new ArgumentNullException(nameof(dataobject));
            
            if (dataobject.__PrimaryKey == null)
            {
                KeyGenerator.Generate(dataobject, null);
            }
            if (keys.ContainsKey(dataobject.__PrimaryKey))
                throw new DetailArrayAlreadyContainsObjectWithThatKeyException();
            SetByKey(dataobject.__PrimaryKey, dataobject);
        }

        //чтобы работала XML сериализация
        public void Add(Object dataobject)
        {
            AddObject((DataObject)dataobject);
        }

        /// <summary>
        /// Удалить объект данных
        /// </summary>		
        public void Remove(DataObject dataobject)
        {

            if (dataobject.GetDetailArray() == this)
            {
                if (this.IndexOf(dataobject) < 0)
                {
                    dataobject.DetailArray = null;
                    return;
                }
            }

            int prevIndex = (int)keys[dataobject.__PrimaryKey];
            keys.Remove(dataobject.__PrimaryKey);
            objects.Remove(dataobject);
            dataobject.DetailArray = null;
            for (int i = 0; i < keys.Count; i++)
            {
                int val = (int)keys.GetByIndex(i);
                if (val > prevIndex)
                {
                    val--;
                    keys.SetByIndex(i, val);
                }
            }
            if (OrderedObjects()) Renumerate();
        }


        /// <summary>
        /// Удалить объект данных по индексу
        /// </summary>
        public void RemoveByIndex(int index)
        {
            DataObject dobj = (DataObject)objects[index];
            RemoveByKey(dobj.__PrimaryKey);
        }

        /// <summary>
        /// Удалить объект данных по первичному ключу
        /// </summary>
        public void RemoveByKey(object key)
        {
            int objindex = (int)keys[key];
            keys.Remove(key);
            DataObject dobj = (DataObject)objects[objindex];
            dobj.DetailArray = null;
            objects.RemoveAt(objindex);
            for (int i = 0; i < keys.Count; i++)
            {
                int val = (int)keys.GetByIndex(i);
                if (val > objindex)
                {
                    val--;
                    keys.SetByIndex(i, val);
                }
            }
            if (OrderedObjects()) Renumerate();
        }

        /// <summary>
        /// Количество объектов
        /// </summary>
        public int Count { get { return objects.Count; } }

        /// <summary>
        /// Установить объект агрегатор
        /// </summary>
        /// <param name="dataobject"> объект-шапка</param>
        protected void prv_SetAggregator(DataObject dataobject)
        {
            Information.SetPropValueByName(dataobject, Information.GetAgregatePropertyName(dataobject.GetType()), this.masterObject);
        }


        public int IndexOf(DataObject dobj)
        {
            return objects.IndexOf(dobj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnItemsAdded(ItemsAddedEventArgs e)
        {
            EventHandler<ItemsAddedEventArgs> handler = ItemsAdded;
            if (handler != null) handler(this, e);
        }



    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class ItemsAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Добавленные в массив объекты данных
        /// </summary>
        public DataObject[] DataObjects { get; set;}

    }

}
