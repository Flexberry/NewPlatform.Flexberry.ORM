namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.Serialization;
    using System.Text;

    using ICSSoft.STORMNET.Exceptions;

    namespace Business
    {
        /// <summary>
        /// Тип структуры хранения.
        /// </summary>
        public enum StorageTypeEnum
        {
            /// <summary>
            /// Простое хранение (каждый класс в своем хранилище)
            /// </summary>
            SimpleStorage,

            /// <summary>
            /// Иерархическое хранение (Хранятся только свои атрибуты,
            /// а атрибуты предка хранятся в его хранилище)
            /// </summary>
            HierarchicalStorage,
        }

        #region Структура хранения

        /// <summary>
        /// структура для отображения представления в данные.
        /// </summary>
        [Serializable]
        public class StorageStructForView : ISerializable
        {
            /// <summary>
            /// описание хранилища для некоторого класса,в ветви наследованных объектов.
            /// </summary>
            [Serializable]
            public struct ClassStorageDef : ISerializable
            {
                /// <summary>
                /// название хранилища (ClassStorageName у класса).
                /// </summary>
                public string Storage;

                /// <summary>
                /// название хранилища для первичного ключа.
                /// </summary>
                public string PrimaryKeyStorageName;

                /// <summary>
                /// название хранилища для свойства в классе,ссылающемся на данный.
                /// </summary>
                public string objectLinkStorageName;

                /// <summary>
                /// индекс хранилища для класса ссылающегося на данный в объемлющем PropSource.
                /// </summary>
                public int parentStorageindex;

                /// <summary>
                /// тип класса с которым ассоциированно данное хранилище.
                /// </summary>
                public System.Type ownerType;

                public bool nullableLink;

                /// <summary>
                /// Имя хранилища для типа.
                /// </summary>
                public string TypeStorageName;

                /// <summary>
                /// Сздание при десериализации.
                /// </summary>
                /// <param name="info"></param>
                /// <param name="context"></param>
                public ClassStorageDef(SerializationInfo info, StreamingContext context)
                {
                    Storage = info.GetString("Storage");
                    PrimaryKeyStorageName = info.GetString("PrimaryKeyStorageName");
                    objectLinkStorageName = info.GetString("objectLinkStorageName");
                    parentStorageindex = info.GetInt32("parentStorageindex");
                    ownerType = (Type)info.GetValue("ownerType", typeof(Type));
                    TypeStorageName = info.GetString("TypeStorageName");
                    nullableLink = info.GetBoolean("nullableLink");
                }

                /// <summary>
                /// Данные для сериализации.
                /// </summary>
                /// <param name="info"></param>
                /// <param name="context"></param>
                public void GetObjectData(SerializationInfo info, StreamingContext context)
                {
                    info.AddValue("Storage", this.Storage);
                    info.AddValue("PrimaryKeyStorageName", this.PrimaryKeyStorageName);
                    info.AddValue("objectLinkStorageName", this.objectLinkStorageName);
                    info.AddValue("parentStorageindex", this.parentStorageindex);
                    info.AddValue("ownerType", this.ownerType);
                    info.AddValue("TypeStorageName", this.TypeStorageName);
                    info.AddValue("nullableLink", this.nullableLink);
                }
            }

            /// <summary>
            /// хранилище для свойства.
            /// </summary>
            [Serializable]
            public class PropStorage : ISerializable
            {
                /// <summary>
                ///
                /// </summary>
                public PropStorage()
                {
                }

                /// <summary>
                /// имя свойства в представлении.
                /// </summary>
                public string Name;

                /// <summary>
                /// в каком хранилище оно лежит.
                /// </summary>
                public PropSource source;

                /// <summary>
                /// для совойств DataObject-ного типа список типов мастеров для каждой  ветви выборки.
                /// </summary>
                public System.Type[][] MastersTypes = null;

                /// <summary>
                /// для совойств DataObject-ного,количество объектов в <see cref="PropStorage.MastersTypes"/>.
                /// </summary>
                public int MastersTypesCount = 0;

                /// <summary>
                /// имя хранилища свойства, для каждой  ветви выборки.
                /// </summary>
                public string[][] storage = new string[1][];

                /// <summary>
                /// имя свойств в объекте (без префикса доступа к объекту).
                /// </summary>
                public string simpleName;

                /// <summary>
                /// хранимый ли атрибут.
                /// </summary>
                public bool Stored = true;

                /// <summary>
                /// формула для хранения вычислимых атрибутов.
                /// </summary>
                public string Expression = null;

                /// <summary>
                /// тип свойства.
                /// </summary>
                public Type propertyType = null;

                /// <summary>
                /// Используется ли это свойство несколько раз, и это уже не первый.
                /// </summary>
                public bool MultipleProp = false;

                /// <summary>
                /// Добавочное свойство (для вычислимых свойств, использующих свойства не загружаемые в представлении).
                /// </summary>
                public bool AdditionalProp = false;

                /// <summary>
                ///
                /// </summary>
                /// <returns></returns>
                public override string ToString()
                {
                    return string.Format("\"{0}\".\"{1}\" as \"{2}\"", source.Name, storage[0][0], Name);
                }

                /// <summary>
                ///
                /// </summary>
                /// <param name="info"></param>
                /// <param name="context"></param>
                public PropStorage(SerializationInfo info, StreamingContext context)
                {
                    this.Name = info.GetString("Name");
                    this.source = (PropSource)info.GetValue("source", typeof(PropSource));
                    this.MastersTypes = (Type[][])info.GetValue("MastersTypes", typeof(Type[][]));
                    this.MastersTypesCount = info.GetInt32("MastersTypesCount");
                    this.storage = (string[][])info.GetValue("storage", typeof(string[][]));
                    this.simpleName = info.GetString("simpleName");
                    this.Stored = info.GetBoolean("Stored");
                    this.Expression = info.GetString("Expression");
                    this.propertyType = (Type)info.GetValue("propertyType", typeof(Type));
                    this.MultipleProp = info.GetBoolean("MultipleProp");
                }

                /// <summary>
                ///
                /// </summary>
                /// <param name="info"></param>
                /// <param name="context"></param>
                public void GetObjectData(SerializationInfo info, StreamingContext context)
                {
                    info.AddValue("Name", this.Name);
                    info.AddValue("source", this.source);
                    info.AddValue("MastersTypes", this.MastersTypes);
                    info.AddValue("MastersTypesCount", this.MastersTypesCount);
                    info.AddValue("storage", this.storage);
                    info.AddValue("simpleName", this.simpleName);
                    info.AddValue("Stored", this.Stored);
                    info.AddValue("Expression", this.Expression);
                    info.AddValue("propertyType", this.propertyType);
                    info.AddValue("MultipleProp", this.MultipleProp);
                }
            }

            /// <summary>
            /// описание хранилища для некоторого класса,в ветви мастеров.
            /// </summary>
            [Serializable]
            public class PropSource : ISerializable, IComparable
            {
                /// <summary>
                /// Связь по иерархии.
                /// </summary>
                public bool HierarchicalLink = false;

                /// <summary>
                /// имя источника данных с учетом влложенности.
                /// </summary>
                public string Name;

                /// <summary>
                /// имя мастера в объемдющем классе.
                /// </summary>
                public string ObjectLink;

                /// <summary>
                /// ветви наследования.
                /// </summary>
                public ClassStorageDef[] storage = new ClassStorageDef[1];

                /// <summary>
                /// ветви мастеров.
                /// </summary>
                public PropSource[] LinckedStorages = new PropSource[0];

                /// <summary>
                ///
                /// </summary>
                public PropSource()
                {
                }

                /// <summary>
                ///
                /// </summary>
                /// <param name="info"></param>
                /// <param name="context"></param>
                public PropSource(SerializationInfo info, StreamingContext context)
                {
                    this.HierarchicalLink = info.GetBoolean("HierarchicalLink");
                    this.Name = info.GetString("Name");
                    this.ObjectLink = info.GetString("ObjectLink");
                    this.storage = (ClassStorageDef[])info.GetValue("storage", typeof(ClassStorageDef[]));
                    this.LinckedStorages = (PropSource[])info.GetValue("LinckedStorages", typeof(PropSource[]));
                }

                /// <summary>
                ///
                /// </summary>
                /// <param name="info"></param>
                /// <param name="context"></param>
                public void GetObjectData(SerializationInfo info, StreamingContext context)
                {
                    info.AddValue("HierarchicalLink", this.HierarchicalLink);
                    info.AddValue("Name", this.Name);
                    info.AddValue("ObjectLink", this.ObjectLink);
                    info.AddValue("storage", this.storage);
                    info.AddValue("LinckedStorages", this.LinckedStorages);
                }

                int IComparable.CompareTo(object obj)
                {
                    if (obj is PropSource)
                    {
                        PropSource o = obj as PropSource;
                        return string.Compare(Name, o.Name);
                    }
                    else
                    {
                        return -1;
                    }
                }

                /// <summary>
                ///
                /// </summary>
                /// <returns></returns>
                public override string ToString()
                {
                    string res = string.Format("({0}) {1} as {2}", ObjectLink, storage[0].Storage, Name);
                    string[] subs = new string[LinckedStorages.Length];
                    string sss = Environment.NewLine[1] + "\t";
                    for (int i = 0; i < subs.Length; i++)
                    {
                        subs[i] = "\t" + string.Join(sss, LinckedStorages[i].ToString().Split(Environment.NewLine[1]));
                    }

                    return (subs.Length > 0) ? res + Environment.NewLine + string.Join(Environment.NewLine, subs) : res;
                }
            }

            /// <summary>
            /// свойства.
            /// </summary>
            public PropStorage[] props;

            /// <summary>
            /// источники данных.
            /// </summary>
            public PropSource sources = new PropSource();

            /// <summary>
            ///
            /// </summary>
            public StorageStructForView()
            {
                ID = Count++;
            }

            /// <summary>
            /// ключ.
            /// </summary>
            public long ID;

            /// <summary>
            /// Конструктор для десериализации.
            /// </summary>
            /// <param name="info"></param>
            /// <param name="context"></param>
            public StorageStructForView(SerializationInfo info, StreamingContext context)
            {
                this.props = (PropStorage[])info.GetValue("props", typeof(PropStorage[]));
                this.sources = (PropSource)info.GetValue("sources", typeof(PropSource));
                this.ID = info.GetInt32("ID");
            }

            /// <summary>
            /// Данные для сериализации.
            /// </summary>
            /// <param name="info"></param>
            /// <param name="context"></param>
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("props", this.props);
                info.AddValue("sources", this.sources);
                info.AddValue("ID", this.ID);
            }

            /// <summary>
            /// кличество раннее созданных.
            /// </summary>
            public static int Count;

            /// <summary>
            /// Как строка.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string[] prs = new string[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    prs[i] = props[i].ToString();
                }

                return "Properties:" + Environment.NewLine + string.Join(Environment.NewLine, prs) +
                    Environment.NewLine + "Sources:" + Environment.NewLine + sources.ToString();
            }
        }
        #endregion
    }
    #region class View

    /// <summary>
    /// описание представления для детейла в представлении шапки.
    /// </summary>
    [Serializable]
    public struct DetailInView : ISerializable
    {
        /// <summary>
        /// представление детейла.
        /// </summary>
        private View detailView;

        /// <summary>
        /// имя детейла.
        /// </summary>
        private string detailName;

        /// <summary>
        /// загружать ли детейл при загрузке шапки.
        /// </summary>
        private bool detailLoadOnLoadAgregator;
        private bool detailvisible;
        private string detailcaption;
        private string detailPath;
        private string[] aggregationFunctions;
        private bool detailUseAdaptiveTypeLoading;
        private ICSSoft.STORMNET.Collections.TypeBaseCollection detailAdaptiveTypeViews;

        /// <summary>
        /// использовать ли адаптивную настройку представлений при загрузке данных.
        /// </summary>
        public bool UseAdaptiveTypeLoading
        {
            get { return detailUseAdaptiveTypeLoading; }
            set { detailUseAdaptiveTypeLoading = value; }
        }

        /// <summary>
        /// настройка адаптации.
        /// </summary>
        public Collections.TypeBaseCollection AdaptiveTypeViews
        {
            get
            {
                if (detailAdaptiveTypeViews == null)
                {
                    detailAdaptiveTypeViews = new ICSSoft.STORMNET.Collections.TypeBaseCollection();
                }

                return detailAdaptiveTypeViews;
            }

            set
            {
                detailAdaptiveTypeViews = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="detailname">имя детейла.</param>
        /// <param name="detailview">представление детейла.</param>
        /// <param name="detailLoadOnLoadAgregator">загружать ли детейл при загрузке шапки.</param>
        /// <param name="detailPath">путь на форме.</param>
        /// <param name="caption">заголовок.</param>
        /// <param name="visible">видимость.</param>
        /// <param name="aggregationfunctions">агр.функции.</param>
        public DetailInView(string detailname, View detailview, bool detailLoadOnLoadAgregator, string detailPath, string caption, bool visible, string[] aggregationfunctions)
        {
            this.detailName = detailname;
            this.detailView = detailview;
            this.detailLoadOnLoadAgregator = detailLoadOnLoadAgregator;
            this.detailPath = detailPath;
            this.detailcaption = caption;
            this.detailvisible = visible;
            this.detailUseAdaptiveTypeLoading = false;
            this.detailAdaptiveTypeViews = null;
            this.aggregationFunctions = (aggregationfunctions == null) ? null : (string[])aggregationfunctions.Clone();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DetailInView(SerializationInfo info, StreamingContext context)
        {
            this.detailView = (View)info.GetValue("detailView", typeof(View));
            this.detailName = info.GetString("detailName");
            this.detailLoadOnLoadAgregator = info.GetBoolean("detailLoadOnLoadAgregator");
            this.detailvisible = info.GetBoolean("detailvisible");
            this.detailcaption = info.GetString("detailcaption");
            this.detailPath = info.GetString("detailPath");
            this.aggregationFunctions = (string[])info.GetValue("aggregationFunctions", typeof(string[]));
            this.detailUseAdaptiveTypeLoading = info.GetBoolean("detailUseAdaptiveTypeLoading");
            this.detailAdaptiveTypeViews = (ICSSoft.STORMNET.Collections.TypeBaseCollection)info.GetValue("detailAdaptiveTypeViews", typeof(ICSSoft.STORMNET.Collections.TypeBaseCollection));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("detailView", detailView);
            info.AddValue("detailName", detailName);
            info.AddValue("detailLoadOnLoadAgregator", detailLoadOnLoadAgregator);
            info.AddValue("detailvisible", detailvisible);
            info.AddValue("detailcaption", detailcaption);
            info.AddValue("detailPath", detailPath);
            info.AddValue("aggregationFunctions", aggregationFunctions);
            info.AddValue("detailUseAdaptiveTypeLoading", detailUseAdaptiveTypeLoading);
            info.AddValue("detailAdaptiveTypeViews", detailAdaptiveTypeViews);
        }

        /// <summary>
        /// представление.
        /// </summary>
        public View View
        {
            get { return detailView; }
            set { detailView = value; }
        }

        /// <summary>
        /// Имя детейлового свойства.
        /// </summary>
        public string Name
        {
            get { return detailName; }
        }

        /// <summary>
        /// Заголовок для детейла.
        /// </summary>
        public string Caption
        {
            get { return detailcaption; }
        }

        /// <summary>
        /// Путь на форме.
        /// </summary>
        public string FormPath
        {
            get { return detailPath; }
        }

        /// <summary>
        /// загружать ли вместе с владельцем.
        /// </summary>
        public bool LoadOnLoadAgregator
        {
            get { return detailLoadOnLoadAgregator; }
            set { detailLoadOnLoadAgregator = value; }
        }

        /// <summary>
        /// видимый-невидимый.
        /// </summary>
        public bool Visible
        {
            get { return detailvisible; }
        }

        /// <summary>
        /// используемые агрегиррующие функции.
        /// </summary>
        public string[] AggregationFunctions
        {
            get { return (aggregationFunctions == null) ? null : (string[])aggregationFunctions.Clone(); }
        }
    }

    /// <summary>
    /// настройка мастера (для визуальной части).
    /// </summary>
    [Serializable]
    public struct MasterInView : ISerializable
    {
        private LookupTypeEnum lookupType;
        private string masterName;
        private string lookupProperty;
        private string customizationString;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mastername">имя мастера.</param>
        /// <param name="lookuptype">настройка лукапа.</param>
        /// <param name="customizationstring"> тип лукапа.</param>
        /// <param name="lookupProperty">свойство отображаемое при lookupe.</param>
        public MasterInView(string mastername, LookupTypeEnum lookuptype, string customizationstring, string lookupProperty)
        {
            masterName = mastername;
            lookupType = lookuptype;
            customizationString = customizationstring;
            this.lookupProperty = lookupProperty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MasterInView(SerializationInfo info, StreamingContext context)
        {
            this.lookupType = (LookupTypeEnum)info.GetValue("lookupType", typeof(LookupTypeEnum));
            this.masterName = info.GetString("masterName");
            this.lookupProperty = info.GetString("lookupProperty");
            this.customizationString = info.GetString("customizationString");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("lookupType", this.lookupType);
            info.AddValue("masterName", this.masterName);
            info.AddValue("lookupProperty", this.lookupProperty);
            info.AddValue("customizationString", this.customizationString);
        }

        /// <summary>
        /// имя мастера.
        /// </summary>
        public string MasterName
        {
            get { return masterName; }
        }

        /// <summary>
        /// настройка лукапа.
        /// </summary>
        public string CustomizationString
        {
            get { return customizationString; }
        }

        /// <summary>
        /// тип лукапа.
        /// </summary>
        public LookupTypeEnum LookupType
        {
            get { return lookupType; }
        }

        /// <summary>
        /// свойство отображаемое при lookupe.
        /// </summary>
        public string LookupProperty
        {
            get { return lookupProperty; }
        }
    }

    /// <summary>
    /// Определение свойства в представлении.
    /// </summary>
    [Serializable]
    public struct PropertyInView : ISerializable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="name">имя свойства.</param>
        /// <param name="caption">заголовок.</param>
        /// <param name="visible">видимость.</param>
        /// <param name="formPath">путь на форме.</param>
        public PropertyInView(string name, string caption, bool visible, string formPath)
        {
            Name = (name == null) ? string.Empty : name;
            Caption = (caption == null) ? string.Empty : caption;
            Visible = visible;
            FormPath = (formPath == null) ? string.Empty : formPath;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PropertyInView(SerializationInfo info, StreamingContext context)
        {
            this.Name = info.GetString("Name");
            this.Caption = info.GetString("Caption");
            this.Visible = info.GetBoolean("Visible");
            this.FormPath = info.GetString("FormPath");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this.Name);
            info.AddValue("Caption", this.Caption);
            info.AddValue("Visible", this.Visible);
            info.AddValue("FormPath", this.FormPath);
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        /// <summary>
        /// Имя аттрибута.
        /// </summary>
        public string Name;

        /// <summary>
        /// Заголовок для данного атрибута в данном представлении.
        /// </summary>
        public string Caption;

        /// <summary>
        /// видимость атрибута.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Путь на форме.
        /// </summary>
        public string FormPath;
    }

    /// <summary>
    /// Определение представления.
    /// </summary>
    [Serializable]
    public sealed class View : ISerializable
    {
        private static object lockObject = new object();
        private System.Type defineClass;
        private string viewName;
        private PropertyInView[] properties;
        private DetailInView[] details;
        private MasterInView[] masters;
        private bool generatedByType = false;
        private View.ReadType readType;
        private Collections.NameObjectCollection masterTypeFilters;

        /// <summary>
        /// Создание копии представления.
        /// </summary>
        /// <returns></returns>
        public View Clone()
        {
            View res = (View)this.MemberwiseClone();
            res.properties = new PropertyInView[properties.Length];
            properties.CopyTo(res.properties, 0);
            res.masters = new MasterInView[masters.Length];
            masters.CopyTo(res.masters, 0);
            res.details = new DetailInView[details.Length];
            details.CopyTo(res.details, 0);

            return res;
        }

        /// <summary>
        /// ограничения по типам для вычитывания данных.
        /// </summary>
        public Collections.NameObjectCollection MasterTypeFilters
        {
            get
            {
                if (masterTypeFilters == null)
                {
                    masterTypeFilters = new ICSSoft.STORMNET.Collections.NameObjectCollection();
                }

                return masterTypeFilters;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool GeneratedByType
        {
            get { return generatedByType; }
        }

        /// <summary>
        ///
        /// </summary>
        public View.ReadType CreationReadType
        {
            get { return readType; }
        }

        /// <summary>
        /// создать "заготовку" для представления.
        /// </summary>
        public View()
        {
            viewName = string.Empty;
            properties = new PropertyInView[0];
            details = new DetailInView[0];
            masters = new MasterInView[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public View(SerializationInfo info, StreamingContext context)
        {
            this.viewName = info.GetString("viewName");
            this.defineClass = (System.Type)info.GetValue("defineClass", typeof(Type));
            this.generatedByType = info.GetBoolean("generatedByType");
            this.readType = (View.ReadType)info.GetValue("readType", typeof(View.ReadType));
            this.properties = (PropertyInView[])info.GetValue("properties", typeof(PropertyInView[]));
            this.details = (DetailInView[])info.GetValue("details", typeof(DetailInView[]));
            this.masters = (MasterInView[])info.GetValue("masters", typeof(MasterInView[]));
            this.masterTypeFilters = (Collections.NameObjectCollection)info.GetValue("masterTypeFilters", typeof(Collections.NameObjectCollection));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("viewName", this.viewName);
            info.AddValue("defineClass", this.defineClass);
            info.AddValue("generatedByType", this.generatedByType);
            info.AddValue("readType", this.readType);
            info.AddValue("properties", this.properties);
            info.AddValue("details", this.details);
            info.AddValue("masters", this.masters);
            info.AddValue("masterTypeFilters", this.masterTypeFilters);
        }

        /// <summary>
        /// создать представление по объекту (вычитанным свойствам объекта).
        /// </summary>
        /// <param name="dobject"></param>
        public View(DataObject dobject)
        {
            properties = new PropertyInView[0];
            details = new DetailInView[0];
            masters = new MasterInView[0];
            viewName = string.Empty;
            defineClass = dobject.GetType();
            string[] props = dobject.GetInitializedProperties();
            Type DetArrType = typeof(DetailArray);
            foreach (string prop in props)
            {
                Type propType = null;
                try
                {
                    propType = Information.GetPropertyType(defineClass, prop);
                }
                catch (CantFindPropertyException ex)
                {
                }

                if (propType != null && !propType.IsSubclassOf(DetArrType))
                {
                    AddProperty(prop, prop, false, string.Empty);
                }
            }

            details = new DetailInView[0];
        }

        /// <summary>
        /// как строить проедставление.
        /// </summary>
        public enum ReadType
        {
            /// <summary>
            /// только для заданного класса
            /// </summary>
            OnlyThatObject,

            /// <summary>
            /// для класса со всеми детейлами
            /// </summary>
            WithRelated,
        }

        /// <summary>
        /// создать представление по типу и критерию построения.
        /// </summary>
        /// <param name="DataObjectType">тип.</param>
        /// <param name="readType">как строить проедставление.</param>
        public View(System.Type DataObjectType, ReadType readType)
        {
            generatedByType = true;
            this.readType = readType;
            properties = new PropertyInView[0];
            details = new DetailInView[0];
            masters = new MasterInView[0];

            viewName = DataObjectType.FullName + "(" + readType.ToString() + ")";
            DefineClassType = DataObjectType;
            string[] allstorProps = Information.GetStorablePropertyNames(defineClass);

            System.Collections.Specialized.StringCollection simpleprops = new System.Collections.Specialized.StringCollection();
            System.Collections.Specialized.StringCollection detailprops = new System.Collections.Specialized.StringCollection();

            for (int i = 0; i < allstorProps.Length; i++)
            {
                if (Information.GetPropertyType(DataObjectType, allstorProps[i]).IsSubclassOf(typeof(DetailArray)))
                {
                    detailprops.Add(allstorProps[i]);
                }
                else
                {
                    if (!Information.GetPropertyDisableAutoViewing(DataObjectType, allstorProps[i]))
                    {
                        simpleprops.Add(allstorProps[i]);
                    }
                }
            }

            properties = new PropertyInView[simpleprops.Count];
            for (int i = 0; i < simpleprops.Count; i++)
            {
                properties[i].Caption = simpleprops[i];
                properties[i].Name = simpleprops[i];
            }

            if (readType == ReadType.WithRelated)
            {
                details = new DetailInView[detailprops.Count];
                for (int i = 0; i < detailprops.Count; i++)
                {
                    System.Type propType = Information.GetPropertyType(defineClass, detailprops[i]);
                    details[i] = new DetailInView(detailprops[i], new View(Information.GetItemType(defineClass, detailprops[i]), ReadType.WithRelated), true, string.Empty, string.Empty, true, null);
                }
            }
        }

        /// <summary>
        /// вернуть представление(ветку от текущего) для мастера.
        /// </summary>
        /// <param name="master">имя свойства-мастера.</param>
        /// <returns></returns>
        public View GetViewForMaster(string master)
        {
            View res = new View();
            res.defineClass = Information.GetPropertyType(this.defineClass, master);
            foreach (PropertyInView p in properties)
            {
                if (p.Name.StartsWith(master + "."))
                {
                    res.AddProperty(p.Name.Substring(master.Length + 1), p.Caption, p.Visible, p.FormPath);
                }
            }

            return res;
        }

        /// <summary>
        /// вернуть описание мастера(настроичные данные).
        /// </summary>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public MasterInView GetMaster(string masterName)
        {
            if (masters == null)
            {
                masters = new MasterInView[0];
            }

            for (int i = 0; i < masters.Length; i++)
            {
                if (masters[i].MasterName == masterName)
                {
                    return masters[i];
                }
            }

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.StartsWith(masterName + "."))
                {
                    return new MasterInView(masterName, LookupTypeEnum.Standard, string.Empty, string.Empty);
                }
            }

            throw new IndexOutOfRangeException(masterName);
        }

        /// <summary>
        /// удалить мастера.
        /// </summary>
        /// <param name="masterName"></param>
        public void RemoveMaster(string masterName)
        {
            if (masters == null)
            {
                masters = new MasterInView[0];
            }

            if (masters.Length == 0)
            {
                return;
            }

            bool masterfound = false;
            for (int i = 0; i < masters.Length; i++)
            {
                if (masters[i].MasterName == masterName)
                {
                    masterfound = true;
                }
                else if (masterfound)
                {
                    masters[i - 1] = masters[i];
                }
            }

            if (masterfound)
            {
                var newMasters = new MasterInView[masters.Length - 1];
                if (newMasters.Length > 0)
                {
                    for (int i = 0; i < masters.Length; i++)
                    {
                        newMasters[i] = masters[i];
                    }
                }

                masters = newMasters;
            }
        }

        /// <summary>
        /// получить описание детейла.
        /// </summary>
        /// <param name="detailName"></param>
        /// <returns></returns>
        public DetailInView GetDetail(string detailName)
        {
            if (details == null)
            {
                details = new DetailInView[0];
            }

            if (details.Length == 0)
            {
                throw new IndexOutOfRangeException(detailName);
            }

            for (int i = 0; i < details.Length; i++)
            {
                if (details[i].Name == detailName)
                {
                    return details[i];
                }
            }

            throw new IndexOutOfRangeException(detailName);
        }

        /// <summary>
        /// удалить детейл из представления.
        /// </summary>
        /// <param name="detailname"></param>
        public void RemoveDetail(string detailname)
        {
            if (details == null)
            {
                details = new DetailInView[0];
            }

            if (details.Length == 0)
            {
                return;
            }

            bool propfound = false;
            for (int i = 0; i < details.Length; i++)
            {
                if (details[i].Name == detailname)
                {
                    propfound = true;
                }
                else if (propfound)
                {
                    details[i - 1] = details[i];
                }
            }

            if (propfound)
            {
                DetailInView[] newprops = new DetailInView[details.Length - 1];
                for (int i = 0; i < newprops.Length; i++)
                {
                    newprops[i] = details[i];
                }

                details = newprops;
            }
        }

        /// <summary>
        /// получить свойство из проедставления.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public PropertyInView GetProperty(string propertyName)
        {
            if (properties == null)
            {
                properties = new PropertyInView[0];
            }

            if (properties.Length == 0)
            {
                throw new IndexOutOfRangeException(propertyName);
            }

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name == propertyName)
                {
                    return properties[i];
                }
            }

            throw new IndexOutOfRangeException(propertyName);
        }

        /// <summary>
        /// Проверка наличия свойства в представлении.
        /// </summary>
        /// <param name="propName">Имя свойства.</param>
        /// <returns>Метод возвращает true, если переданное в качестве параметра свойство присутствует в представлении, и false в противном случае.</returns>
        public bool CheckPropname(string propName)
        {
            return CheckPropname(propName, false);
        }

        /// <summary>
        /// Проверка наличия свойства в представлении.
        /// </summary>
        /// <param name="propName">Имя свойства.</param>
        /// <param name="checkDetails">Искать ли имя свойства в детейлах, имеющихся в данном представлении.</param>
        /// <returns>Метод возвращает true, если переданное в качестве параметра свойство присутствует в представлении, и false в противном случае.</returns>
        public bool CheckPropname(string propName, bool checkDetails)
        {
            return Properties.Any(x => x.Name == propName)
                || (checkDetails && Details.Any(x => x.Name == propName));
        }

        /// <summary>
        /// удалить свойство из представления.
        /// </summary>
        /// <param name="propName"></param>
        public void RemoveProperty(string propName)
        {
            if (properties == null)
            {
                properties = new PropertyInView[0];
            }

            if (properties.Length == 0)
            {
                return;
            }

            bool propfound = false;
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name == propName)
                {
                    propfound = true;
                }
                else if (propfound)
                {
                    properties[i - 1] = properties[i];
                }
            }

            if (propfound)
            {
                PropertyInView[] newprops = new PropertyInView[properties.Length - 1];
                for (int i = 0; i < newprops.Length; i++)
                {
                    newprops[i] = properties[i];
                }

                properties = newprops;
            }
        }

        /// <summary>
        /// Добавить detail в представление.
        /// </summary>
        /// <param name="detailname"></param>
        /// <param name="detailview"></param>
        /// <param name="loadOnLoadAgregator"></param>
        public void AddDetailInView(string detailname, View detailview, bool loadOnLoadAgregator)
        {
            AddDetailInView(detailname, detailview, loadOnLoadAgregator, string.Empty, false, string.Empty, null);
        }

        /// <summary>
        /// Добавить detail в представление.
        /// </summary>
        /// <param name="detailname"></param>
        /// <param name="detailview"></param>
        /// <param name="loadOnLoadAgregator"></param>
        /// <param name="path"></param>
        /// <param name="visible"></param>
        /// <param name="caption"></param>
        /// <param name="aggregateFunctions"></param>
        public void AddDetailInView(string detailname, View detailview, bool loadOnLoadAgregator, string path, bool visible, string caption, string[] aggregateFunctions)
        {
            if (details == null)
            {
                details = new DetailInView[0];
            }

            for (int i = 0; i < details.Length; i++)
            {
                if (details[i].Name == detailname)
                {
                    return;
                }
            }

            DetailInView[] divs = details;
            details = new DetailInView[divs.Length + 1];
            divs.CopyTo(details, 0);
            details[divs.Length] = new DetailInView(detailname, detailview, loadOnLoadAgregator, path, caption, visible, aggregateFunctions);
        }

        /// <summary>
        /// Добавить описание мастера в представление.
        /// </summary>
        /// <param name="masterName"></param>
        /// <param name="lookupType"></param>
        /// <param name="lookupcustomizationstring"></param>
        /// <param name="lookupProperty"></param>
        public void AddMasterInView(string masterName, LookupTypeEnum lookupType, string lookupcustomizationstring, string lookupProperty)
        {
            if (masters == null)
            {
                masters = new MasterInView[0];
            }

            for (int i = 0; i < masters.Length; i++)
            {
                if (masters[i].MasterName == masterName)
                {
                    return;
                }
            }

            MasterInView[] oldmasters = masters;
            masters = new MasterInView[oldmasters.Length + 1];
            oldmasters.CopyTo(masters, 0);

            masters[oldmasters.Length] = new MasterInView(masterName, lookupType, lookupcustomizationstring, lookupProperty);
            if (lookupProperty != string.Empty)
            {
                if (!CheckPropname(masterName + "." + lookupProperty))
                {
                    AddProperty(masterName + "." + lookupProperty);
                }
            }
        }

        /// <summary>
        /// Добавить описание мастера в представление.
        /// </summary>
        /// <param name="masterName"></param>
        public void AddMasterInView(string masterName)
        {
            AddMasterInView(masterName, LookupTypeEnum.Standard, string.Empty, string.Empty);
        }

        /// <summary>
        /// Добавить свойства.
        /// </summary>
        /// <param name="propertyNames">Имена свойств.</param>
        public void AddProperties(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                AddProperty(propertyName);
            }
        }

        /// <summary>
        /// Добавить свойство.
        /// </summary>
        /// <param name="propName"></param>
        public void AddProperty(string propName)
        {
            AddProperty(propName, string.Empty, false, string.Empty);
        }

        /// <summary>
        /// Добавить свойство в представление.
        /// </summary>
        /// <param name="propName">Название свойства.</param>
        /// <param name="propCaption">Заголовок свойства.</param>
        /// <param name="visible">Видимость свойства.</param>
        /// <param name="propPath">Путь свойства на форме.</param>
        public void AddProperty(string propName, string propCaption, bool visible, string propPath)
        {
            lock (lockObject)
            {
                // Проверим может оно уже есть.
                if (properties == null)
                {
                    properties = new PropertyInView[0];
                }

                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name == propName)
                    {
                        return;
                    }
                }

                // Увеличим количество свойств.
                PropertyInView[] piv = properties;
                properties = new PropertyInView[piv.Length + 1];
                piv.CopyTo(properties, 0);
                int propIndex = piv.Length;

                if (propCaption == string.Empty)
                {
                    if (!propName.EndsWith("*"))
                    {
                        propCaption = propName;
                    }
                }

                if (propName.EndsWith("*"))
                {
                    if (propCaption.EndsWith("*"))
                    {
                        propCaption = propCaption.Substring(0, propCaption.Length - 1);
                    }

                    // Смотрим кто же это.
                    string pref = string.Empty;
                    if (propName.LastIndexOf(".") >= 0)
                    {
                        pref = propName.Substring(0, propName.LastIndexOf("."));
                    }

                    Type type = DefineClassType;
                    string[] path = pref.Split('.');
                    if (pref != string.Empty)
                    {
                        for (int pathind = 0; pathind < path.Length; pathind++)
                        {
                            type = Information.GetPropertyType(type, path[pathind]);
                        }

                        pref = pref + ".";
                    }

                    string[] allprops = Information.GetAllPropertyNames(type);
                    for (int propsind = 0; propsind < allprops.Length; propsind++)
                    {
                        if (!Information.GetPropertyType(type, allprops[propsind]).IsSubclassOf(typeof(DetailArray)))
                        {
                            // Добавляем атрибут.
                            if (propsind != 0)
                            {
                                // Увеличим массив.
                                piv = properties;
                                properties = new PropertyInView[piv.Length + 1];
                                piv.CopyTo(properties, 0);
                            }

                            if (propCaption == string.Empty && pref == string.Empty)
                            {
                                properties[propIndex++] = new PropertyInView(pref + allprops[propsind], propCaption + allprops[propsind], visible, propPath);
                            }
                        }
                    }
                }
                else
                {
                    properties[propIndex++] = new PropertyInView(propName, propCaption, visible, propPath);
                }
            }
        }

        private string lightTrim(string s)
        {
            if (s == string.Empty)
            {
                return s;
            }
            else
            {
                return s.Substring(1, s.Length - 2);
            }
        }

        private void ParsePropDef(string propDef, out string propName, out string propCaption, out string FormPath)
        {
            string captionreg = "(\\s*[aA][sS]\\s*(?<caption>([\"][^\"]*[\"])|([\'][^\']*[\'])))?";
            string placementreg = "(\\s*[oO][nN]\\s*(?<placement>([\"][^\"]*[\"])|([\'][^\']*[\'])))?";
            string propnamereg = @"(?<propName>\S+)";
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(
                "(" + propnamereg + captionreg + placementreg + ")|(" + propnamereg + placementreg + captionreg + ")");
            System.Text.RegularExpressions.Match m = r.Match(propDef);
            propName = m.Groups["propName"].Success ? m.Groups["propName"].Value : null;
            propCaption = m.Groups["caption"].Success ? lightTrim(m.Groups["caption"].Value) : null;
            FormPath = m.Groups["placement"].Success ? lightTrim(m.Groups["placement"].Value) : null;
        }

        /// <summary>
        /// Создание представления по описанию в атрибуте.
        /// </summary>
        /// <param name="ViewDefAttribute">Атрибут, описывающий представление.</param>
        /// <param name="ViewDefClass">Класс, для которого создаётся представление.</param>
        public View(ViewAttribute ViewDefAttribute, System.Type ViewDefClass)
        {
            properties = new PropertyInView[0];
            details = new DetailInView[0];

            viewName = ViewDefAttribute.Name;
            ArrayList propsAL = new ArrayList();

            string[] hiddenAtrs = ViewDefAttribute.Hidden;
            for (int i = 0; i < ViewDefAttribute.Properties.Length; i++)
            {
                string propname, propCaption, propPath;
                ParsePropDef(ViewDefAttribute.Properties[i], out propname, out propCaption, out propPath);
                if (!propname.EndsWith("*") && propCaption == null)
                {
                    propCaption = propname;
                }

                if (propCaption == null)
                {
                    propCaption = string.Empty;
                }

                if (propPath == null)
                {
                    propPath = string.Empty;
                }

                bool visible = true;
                if (propname.EndsWith("*"))
                {
                    if (propCaption.EndsWith("*"))
                    {
                        propCaption = propCaption.Substring(0, propCaption.Length - 1);
                    }

                    // Cмотрим, кто же это.
                    string pref = string.Empty;
                    if (propname.LastIndexOf(".") >= 0)
                    {
                        pref = propname.Substring(0, propname.LastIndexOf("."));
                    }

                    System.Type cType = ViewDefClass;
                    string[] path = pref.Split('.');
                    if (pref != string.Empty)
                    {
                        for (int pathind = 0; pathind < path.Length; pathind++)
                        {
                            cType = Information.GetPropertyType(cType, path[pathind]);
                        }

                        pref = pref + ".";
                        if (propCaption == string.Empty)
                        {
                            propCaption = pref;
                        }
                    }

                    string[] allprops = Information.GetAllPropertyNames(cType);
                    for (int propsind = 0; propsind < allprops.Length; propsind++)
                    {
                        if (!Information.GetPropertyType(cType, allprops[propsind]).IsSubclassOf(typeof(DetailArray)))
                        {
                            if (!Information.GetPropertyDisableAutoViewing(ViewDefClass, pref + allprops[propsind]))
                            {
                                visible = Array.IndexOf(hiddenAtrs, propname) < 0 && Array.IndexOf(hiddenAtrs, pref + allprops[propsind]) < 0;
                                propsAL.Add(new PropertyInView(pref + allprops[propsind], propCaption + allprops[propsind], visible, propPath));
                            }
                        }
                    }
                }
                else
                {
                    if (Array.IndexOf(hiddenAtrs, propname) >= 0)
                    {
                        visible = false;
                    }

                    propsAL.Add(new PropertyInView(propname, propCaption, visible, propPath));
                }
            }

            properties = (PropertyInView[])propsAL.ToArray(typeof(PropertyInView));

            defineClass = ViewDefClass;

            object[] atrs = defineClass.GetCustomAttributes(typeof(AssociatedDetailViewAttribute), false);
            if (atrs != null)
            {
                int DetCount = 0;
                for (int i = 0; i < atrs.Length; i++)
                {
                    if (((AssociatedDetailViewAttribute)atrs[i]).ViewName == viewName)
                    {
                        DetCount++;
                    }
                }

                details = new DetailInView[DetCount];
                DetCount = 0;
                for (int i = 0; i < atrs.Length; i++)
                {
                    AssociatedDetailViewAttribute atr = (AssociatedDetailViewAttribute)atrs[i];
                    if (atr.ViewName == viewName)
                    {
                        System.Type[] dettypes = TypeUsageProvider.TypeUsage.GetUsageTypes(defineClass, atr.DetailName);
                        System.Type detType = Information.GetItemType(defineClass, atr.DetailName);
                        if (atr.UseAdaptiveViewsLoading)
                        {
                            View comv = Information.GetCompatibleView(atr.DetailViewName, dettypes);
                            if (comv == null)
                            {
                                comv = new View();
                                comv.DefineClassType = detType;
                            }

                            details[DetCount] = new DetailInView(atr.DetailName,
                                    comv,
                                    atr.LoadOnLoadAgregator, atr.FormPath, atr.Caption, atr.Visible, atr.AggregateOperations);
                            details[DetCount].UseAdaptiveTypeLoading = true;
                        }
                        else
                        {
                            if (Information.CheckViewForClasses(atr.DetailViewName, dettypes))
                            {
                                details[DetCount] = new DetailInView(atr.DetailName,
                                    Information.GetView(atr.DetailViewName, dettypes[0]),
                                    atr.LoadOnLoadAgregator, atr.FormPath, atr.Caption, atr.Visible, atr.AggregateOperations);
                                details[DetCount].UseAdaptiveTypeLoading = false;
                            }
                            else
                            {
                                throw new UncompatibleViewForClassException(atr.DetailViewName, detType);
                            }
                        }

                        DetCount++;
                    }
                }
            }

            atrs = defineClass.GetCustomAttributes(typeof(MasterViewDefineAttribute), false);
            if (atrs != null)
            {
                int mcount = 0;
                for (int i = 0; i < atrs.Length; i++)
                {
                    if (((MasterViewDefineAttribute)atrs[i]).ViewName == viewName)
                    {
                        mcount++;
                    }
                }

                masters = new MasterInView[mcount];
                mcount = 0;
                for (int i = 0; i < atrs.Length; i++)
                {
                    MasterViewDefineAttribute atr = (MasterViewDefineAttribute)atrs[i];
                    if (atr.ViewName == viewName)
                    {
                        masters[mcount++] = new MasterInView(atr.MasterName, atr.LookupType, atr.CustomizationString, atr.LookupProperty);
                        if (atr.LookupProperty != string.Empty)
                        {
                            if (!CheckPropname(atr.MasterName + "." + atr.LookupProperty))
                            {
                                AddProperty(atr.MasterName + "." + atr.LookupProperty);
                            }
                        }
                    }
                }
            }

            foreach (PropertyInView piv in properties)
            {
                if (Information.GetPropertyType(ViewDefClass, piv.Name).IsSubclassOf(typeof(DataObject)) &&
                    piv.Name.IndexOf(".") == -1)
                {
                    AddMasterInView(piv.Name);
                }
            }
        }

        /// <summary>
        /// Адаптировать представления для детейлов (в зависимости от типа).
        /// </summary>
        public void LoadingAdaptation()
        {
            LoadingAdaptation(this, defineClass);
        }

        /// <summary>
        /// Адаптировать представления для детейлов (в зависимости от типа).
        /// </summary>
        /// <param name="tp">тип по которому настраивать.</param>
        public void LoadingAdaptation(Type tp)
        {
            LoadingAdaptation(this, tp);
        }

        private void LoadingAdaptation(View v, Type t)
        {
            foreach (DetailInView div in v.details)
            {
                if (div.UseAdaptiveTypeLoading)
                {
                    Type[] typeUsages = TypeUsageProvider.TypeUsage.GetUsageTypes(t, div.Name);
                    Collections.TypeBaseCollection tc = new ICSSoft.STORMNET.Collections.TypeBaseCollection();

                    foreach (Type tp in typeUsages)
                    {
                        if (div.View.GeneratedByType)
                        {
                            View dv = new View(tp, div.View.readType);
                            dv.SetAllAdaptive();
                            tc.Add(tp, dv);
                            dv.LoadingAdaptation(tp);
                        }
                        else
                        {
                            View dv = Information.GetView(div.View.Name, tp);
                            if (dv == null)
                            {
                                dv = div.View;
                            }

                            tc.Add(tp, dv);
                            dv.LoadingAdaptation(tp);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Сделать все представление адаптируемым.
        /// </summary>
        public void SetAllAdaptive()
        {
            for (int i = 0; i < details.Length; i++)
            {
                details[i].UseAdaptiveTypeLoading = true;
                details[i].View.SetAllAdaptive();
            }
        }

        /// <summary>
        /// Наименование представления.
        /// </summary>
        public string Name
        {
            get { return viewName; }
            set { viewName = value; }
        }

        /// <summary>
        /// тип, для которого определено пердставление.
        /// </summary>
        public System.Type DefineClassType
        {
            get { return defineClass; }
            set { defineClass = value; }
        }

        /// <summary>
        /// Получить все детейлы.
        /// </summary>
        public DetailInView[] Details
        {
            get { return details ?? (details = new DetailInView[0]); }
            set { details = value; }
        }

        /// <summary>
        /// Получить все мастера.
        /// </summary>
        public MasterInView[] Masters
        {
            get { return masters; }
            set { masters = value; }
        }

        /// <summary>
        /// Список свойств входящих в представление.
        /// </summary>
        public PropertyInView[] Properties
        {
            get { return properties ?? (properties = new PropertyInView[0]); }
            set { properties = value; }
        }

        /// <summary>
        /// Строковое представление представления.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return viewName + "(" + defineClass.Name + ")";
        }

        /// <summary>
        /// в строку.
        /// </summary>
        /// <param name="fullView"></param>
        /// <returns></returns>
        public string ToString(bool fullView)
        {
            if (!fullView)
            {
                return ToString();
            }

            var res = new StringBuilder();
            res.Append(ToString());
            res.Append(":");
            foreach (PropertyInView pi in properties)
            {
                res.Append("(");
                res.Append(pi.Name);
                res.Append(",");
                res.Append(pi.Caption);
                res.Append(",");
                res.Append(pi.Visible);
                res.Append(")");
            }

            foreach (DetailInView div in details)
            {
                res.Append("{");
                res.Append(div.Name);
                res.Append(",");
                res.Append(div.View);
                res.Append(",");
                res.Append(div.LoadOnLoadAgregator);
                res.Append(")");
            }

            return res.ToString();
        }

        /// <summary>
        /// заготовка для 2-х представлений.
        /// </summary>
        /// <param name="firstView"></param>
        /// <param name="secondView"></param>
        /// <returns></returns>
        private static View getViewForViews(View firstView, View secondView)
        {
            Type firstType = firstView.defineClass;
            Type secondType = secondView.defineClass;
            View Result = new View();
            if (secondType == firstType)
            {
                Result.defineClass = firstType;
            }
            else if (secondType.IsSubclassOf(firstType))
            {
                Result.defineClass = secondType;
            }
            else if (firstType.IsSubclassOf(secondType))
            {
                Result.defineClass = firstType;
            }
            else
            {
                throw new IncompatibleTypesForViewOperationException(firstView.ToString(), secondView.ToString());
            }

            return Result;
        }

        /// <summary>
        /// OR - Объединение.
        /// </summary>
        /// <param name="firstView"></param>
        /// <param name="secondView"></param>
        /// <returns></returns>
        public static View operator |(View firstView, View secondView)
        {
            if (firstView == null)
            {
                return secondView;
            }

            if (secondView == null)
            {
                return firstView;
            }

            View Result = getViewForViews(firstView, secondView);
            Result.Name = firstView.ToString() + "|" + secondView.ToString();
            System.Collections.SortedList temp = new SortedList();

            // properties
            for (int i = 0; i < secondView.properties.Length; i++)
            {
                temp.Add(secondView.properties[i].Name, secondView.properties[i]);
            }

            for (int i = 0; i < firstView.properties.Length; i++)
            {
                if (!temp.ContainsKey(firstView.properties[i].Name))
                {
                    temp.Add(firstView.properties[i].Name, firstView.properties[i]);
                }
                else
                {
                    PropertyInView p = (PropertyInView)temp[firstView.properties[i].Name];
                    p.Visible = p.Visible || firstView.properties[i].Visible;
                    p.Caption = p.Caption + " / " + firstView.properties[i].Caption;
                    temp[firstView.properties[i].Name] = p;
                }
            }

            PropertyInView[] props = new PropertyInView[temp.Count];
            temp.Values.CopyTo(props, 0);
            Result.properties = props;

            // detail-properties
            temp.Clear();
            for (int i = 0; i < secondView.details.Length; i++)
            {
                temp.Add(secondView.details[i].Name, secondView.details[i]);
            }

            for (int i = 0; i < firstView.details.Length; i++)
            {
                if (!temp.ContainsKey(firstView.details[i].Name))
                {
                    temp.Add(firstView.details[i].Name, firstView.details[i]);
                }
                else
                {
                    DetailInView v = (DetailInView)temp[firstView.details[i].Name];
                    v.LoadOnLoadAgregator = v.LoadOnLoadAgregator || firstView.details[i].LoadOnLoadAgregator;
                    v.View = v.View | firstView.details[i].View;
                    temp[firstView.details[i].Name] = v;
                }
            }

            DetailInView[] dets = new DetailInView[temp.Count];
            temp.Values.CopyTo(dets, 0);
            Result.details = dets;
            return Result;
        }

        /// <summary>
        /// AND - Пересечение.
        /// </summary>
        /// <param name="firstView"></param>
        /// <param name="secondView"></param>
        /// <returns></returns>
        public static View operator &(View firstView, View secondView)
        {
            if (firstView == null || secondView == null)
            {
                return null;
            }

            View Result = getViewForViews(firstView, secondView);
            Result.Name = firstView.ToString() + "&" + secondView.ToString();
            System.Collections.SortedList temp = new SortedList();
            System.Collections.SortedList temp1 = new SortedList();

            // properties
            for (int i = 0; i < secondView.properties.Length; i++)
            {
                temp.Add(secondView.properties[i].Name, secondView.properties[i]);
            }

            for (int i = 0; i < firstView.properties.Length; i++)
            {
                if (temp.ContainsKey(firstView.properties[i].Name))
                {
                    temp1.Add(firstView.properties[i].Name, firstView.properties[i]);
                }
            }

            PropertyInView[] props = new PropertyInView[temp1.Count];
            temp1.Values.CopyTo(props, 0);
            Result.properties = props;

            // detail-properties
            temp.Clear();
            temp1.Clear();
            for (int i = 0; i < secondView.details.Length; i++)
            {
                temp.Add(secondView.details[i].Name, secondView.details[i]);
            }

            for (int i = 0; i < firstView.details.Length; i++)
            {
                if (temp.ContainsKey(firstView.details[i].Name))
                {
                    DetailInView div1 = (DetailInView)temp[firstView.details[i].Name];
                    DetailInView div2 = (DetailInView)firstView.details[i];
                    DetailInView resdiv = new DetailInView(
                        div1.Name, div1.View & div1.View, div1.LoadOnLoadAgregator && div1.LoadOnLoadAgregator, div1.FormPath,
                        div1.Caption + " & " + div2.Caption, div1.Visible && div2.Visible, div2.AggregationFunctions);
                    temp1.Add(firstView.details[i].Name, resdiv);
                }
            }

            DetailInView[] dets = new DetailInView[temp1.Count];
            temp1.Values.CopyTo(dets, 0);
            Result.details = dets;
            return Result;
        }

        /// <summary>
        /// - Разность.
        /// </summary>
        /// <param name="firstView"></param>
        /// <param name="secondView"></param>
        /// <returns></returns>
        public static View operator -(View firstView, View secondView)
        {
            View Result = getViewForViews(firstView, secondView);
            Result.Name = firstView.ToString() + "-" + secondView.ToString();
            System.Collections.SortedList temp = new SortedList();
            System.Collections.SortedList temp1 = new SortedList();

            // properties
            for (int i = 0; i < firstView.properties.Length; i++)
            {
                temp.Add(firstView.properties[i].Name, firstView.properties[i]);
            }

            for (int i = 0; i < secondView.properties.Length; i++)
            {
                if (temp.ContainsKey(secondView.properties[i].Name))
                {
                    temp.Remove(secondView.properties[i].Name);
                }
            }

            PropertyInView[] props = new PropertyInView[temp.Count];
            temp.Values.CopyTo(props, 0);
            Result.properties = props;

            // detail-properties
            temp.Clear();
            temp1.Clear();

            for (int i = 0; i < firstView.details.Length; i++)
            {
                temp.Add(firstView.details[i].Name, firstView.details[i]);
            }

            for (int i = 0; i < secondView.details.Length; i++)
            {
                if (temp.ContainsKey(secondView.details[i].Name))
                {
                    temp.Remove(secondView.details[i].Name);
                }
            }

            DetailInView[] dets = new DetailInView[temp.Count];
            temp.Values.CopyTo(dets, 0);
            Result.details = dets;
            return Result;
        }

        /// <summary>
        /// exclusive-OR.
        /// </summary>
        /// <param name="firstView"></param>
        /// <param name="secondView"></param>
        /// <returns></returns>
        public static View operator ^(View firstView, View secondView)
        {
            View res = (firstView | secondView) - (firstView & secondView);
            res.Name = firstView.Name + "^" + secondView.Name;
            return res;
        }

        public int[] GetOrderedIndexes(string[] orderCols, string[] advCols)
        {
            if (orderCols == null || orderCols.Length == 0)
            {
                int[] res = new int[properties.Length];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = i;
                }

                return res;
            }
            else
            {
                if (properties == null)
                {
                    return new int[0];
                }
                else
                {
                    string[] allProps = new string[properties.Length + advCols.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        allProps[i] = properties[i].Name;
                    }

                    int k = properties.Length;
                    for (int i = 0; i < advCols.Length; i++)
                    {
                        allProps[k++] = advCols[i];
                    }

                    int[] res = new int[allProps.Length];
                    int resindex = 0;
                    for (int i = 0; i < orderCols.Length; i++)
                    {
                        for (int j = 0; j < allProps.Length; j++)
                        {
                            if (allProps[j] == orderCols[i])
                            {
                                res[resindex++] = j;
                                allProps[j] = null;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < allProps.Length; i++)
                    {
                        if (allProps[i] != null)
                        {
                            res[resindex++] = i;
                        }
                    }

                    return res;
                }
            }
        }

        /// <summary>
        /// вернуть порядок упоминания свойств в представлении.
        /// </summary>
        /// <param name="orderCols"></param>
        /// <returns></returns>
        public int[] GetOrderedIndexes(params string[] orderCols)
        {
            return GetOrderedIndexes(orderCols, new string[0]);
        }

        /// <summary>
        /// Получить индекс свойства в представлении.
        /// Метод был добавлен для удобства работы с LoadStringedVeiw,
        /// но убедитесь, что это именно то представление, которое
        /// использовалось при загрузке и его никто не изменял.
        /// </summary>
        /// <param name="PropertyName">Имя свойства.</param>
        /// <returns>Индекс, начиная с 0. -1, если такого свойства нет.</returns>
        public int GetPropertyIndex(string PropertyName)
        {
            int retInd = -1;
            int i = 0;
            foreach (PropertyInView piv in Properties)
            {
                if (piv.Name == PropertyName)
                {
                    retInd = i;
                    break;
                }

                i++;
            }

            return retInd;
        }

        /// <summary>
        /// Проверить объект на вычитанность по представлению.
        /// </summary>
        /// <param name="dobject">Объект данных, который проверяется по загруженности по представлению.</param>
        /// <returns><c>True</c>, если объект вычитан по представлению.</returns>
        public bool TestObjectForViewing(DataObject dobject)
        {
            string[] propnames = dobject.GetInitializedProperties();
            Array.Sort(propnames);

            ArrayList arrMasters = new ArrayList();

            // Получили список проинициализированных свойств.
            foreach (PropertyInView piv in properties)
            {
                if (Information.IsStoredProperty(DefineClassType, piv.Name))
                {
                    if (piv.Name.IndexOf(".") != -1)
                    {
                        int i = piv.Name.IndexOf(".");
                        string sMaster = piv.Name.Substring(0, i);

                        if (!arrMasters.Contains(sMaster))
                        {
                            arrMasters.Add(sMaster);
                        }
                    }
                    else if (Array.BinarySearch(propnames, piv.Name) < 0)
                    {
                        return false;
                    }
                }
            }

            foreach (string sMaster in arrMasters)
            {
                View v = GetViewForMaster(sMaster);

                DataObject master = (DataObject)Information.GetPropValueByName(dobject, sMaster);

                if (master != null)
                {
                    if (!v.TestObjectForViewing(master))
                    {
                        return false;
                    }
                }
            }

            return details.All(div => !div.LoadOnLoadAgregator || Array.BinarySearch(propnames, div.Name) >= 0);
        }

        /// <summary>
        /// вернуть общий базовый класс для представлений.
        /// </summary>
        /// <param name="views"></param>
        /// <returns></returns>
        static Type GetCommonTypeForViews(params View[] views)
        {
            if (views.Length == 0)
            {
                return null;
            }
            else
            {
                System.Type tstType = views[0].defineClass;
                for (int i = 1; i < views.Length; i++)
                {
                    System.Type ctype = views[i].defineClass;
                    if (ctype != tstType)
                    {
                        if (ctype.IsSubclassOf(tstType))
                        {
                            tstType = ctype;
                        }
                        else if (!tstType.IsSubclassOf(ctype))
                        {
                            return null;
                        }
                    }
                }

                return tstType;
            }
        }

        /// <summary>
        /// Возвращает локализованную подпись свойства (в том числе и массива детейлов) из класса ресурсов Captions
        /// из пространства имен сборки с объектами.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="culture">Культура (если не задать, то используется текущая культура).</param>
        /// <returns>Подпись (если не найден ресурс, то возвращается обычная Caption).</returns>
        public string GetLocalizedPropertyCaption(string propertyName, CultureInfo culture = null)
        {
            if (DefineClassType == null)
            {
                throw new Exception("Свойство DefineClassType не проинициализировано");
            }

            var captionsResources = DefineClassType.Assembly.GetType(
                DefineClassType.Namespace + ".ObjectsResources.Captions");
            if (captionsResources == null)
            {
                return GetProperty(propertyName).Caption
                    ?? Information.GetPropertyCaption(DefineClassType, propertyName);
            }

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var resourceManager = captionsResources.GetProperty(
                "ResourceManager",
                bindingFlags).GetValue(null, null)
                as ResourceManager;
            if (resourceManager == null)
            {
                throw new Exception(string.Format(
                    "В классе {0} не найден ResourceManager",
                    captionsResources.FullName));
            }

            var resourceKey = string.Format(
                "{0}_{1}_{2}",
                DefineClassType.FullName.Replace('.', '_'),
                Name,
                propertyName);

            var result = resourceManager.GetString(resourceKey, culture);
            if (string.IsNullOrEmpty(result))
            {
                resourceKey = string.Format(
                "{0}_{1}_{2}",
                DefineClassType.FullName.Replace('.', '_'),
                "DefCaption",
                propertyName);
                result = resourceManager.GetString(resourceKey, culture);
            }

            return result
                ?? GetProperty(propertyName).Caption
                ?? Information.GetPropertyCaption(DefineClassType, propertyName);
        }
    }
    #endregion
}
