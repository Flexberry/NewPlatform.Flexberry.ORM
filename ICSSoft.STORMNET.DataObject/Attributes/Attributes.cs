namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    // Определение (Definition level)

    #region Определяем данные

    #region ..Взаимосвязь объектов

    /// <summary>
    /// Указывает объект, являющийся частью вышестоящего объекта.
    /// логика работы не реализована.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SubobjectAttribute : Attribute
    {
        /// <summary>
        /// Возможно вам понадобится этот конструетор для создания экземпляра класса SubobjectAttribute.
        /// </summary>
        public SubobjectAttribute()
        {
        }
    }

    /// <summary>
    /// Указывает Агрегирующий объект для детейлового объекта.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AgregatorAttribute : Attribute
    {
        /// <summary>
        /// Возможно вам понадобится этот конструетор для создания экземпляра класса AgregatorAttribute.
        /// </summary>
        public AgregatorAttribute()
        {
        }
    }
    #endregion
    #region ..Значения свойств

    /// <summary>
    /// Указывает, что данный атрибут не может принимать значения Null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : Attribute
    {
        private bool bnotnull;

        /// <summary>
        /// Используется для указания недопустимости пустого значения.
        /// </summary>
        public NotNullAttribute()
        {
            bnotnull = true;
        }

        /// <summary>
        /// Используется для отмены NotNull в наследнике для свойства, в предке указанного как NotNull.
        /// Для отмены пишите false;.
        /// </summary>
        public NotNullAttribute(bool value)
        {
            this.bnotnull = value;
        }

        /// <summary>
        /// Значение атрибута. Если false, то пустое значение допустимо.
        /// </summary>
        public bool NotNull
        {
            get
            {
                return bnotnull;
            }
        }
    }

    /// <summary>
    /// Указывает, что данный атрибут не может принимать значения длиннее явно определённого
    /// (Проверка будет осуществляться при присваивании объекту).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StrLenAttribute : Attribute
    {
        private int len;

        /// <summary>
        /// Используется для указания отсутствия проверки на длину.
        /// </summary>
        public StrLenAttribute()
        {
            len = -1;
        }

        /// <summary>
        /// Используется для указания допустимой длины строки
        /// Для отмены пишите -1;.
        /// </summary>
        public StrLenAttribute(int value)
        {
            this.len = value;
        }

        /// <summary>
        /// Значение атрибута. Если -1, то проверки не будет.
        /// </summary>
        public int StrLen
        {
            get
            {
                return len;
            }
        }
    }

    /// <summary>
    /// Указывает порядок атрибутов, в соответствии с которым упорядочены объекты в свойстве-массиве объектов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderAttribute : Attribute
    {
        /// <summary>
        /// Возможно вам понадобится этот конструетор для создания экземпляра класса OrderAttribute.
        /// </summary>
        public OrderAttribute()
        {
        }
    }
    #endregion

    /// <summary>
    /// Укеазывает допустимые по присваиванию объекты объектов данных
    /// для мастеровых свойств и детайловых классов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class TypeUsageAttribute : Attribute
    {
        private Type[] usetypes;

        /// <summary>
        /// Конструктор с именами типов в конкретной сборке.
        /// </summary>
        /// <param name="assemblyName">Имя сборки.</param>
        /// <param name="typeNames">Имена типов.</param>
        public TypeUsageAttribute(string assemblyName, params string[] typeNames)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName);

            if (assembly == null)
            {
                throw new ArgumentException("В текущем домене приложения не найдена сборка " + assemblyName);
            }

            var types = new List<Type>(typeNames.Length);
            types.AddRange(typeNames.Select(typeName => assembly.GetType(typeName)));

            usetypes = new Type[types.Count];
            types.CopyTo(usetypes, 0);
        }

        /// <summary>
        /// Возможно вам понадобится этот конструетор для создания экземпляра класса TypeUsageAttribute.
        /// </summary>
        /// <param name="types"> перечислим типы, допустимые по использованию.</param>
        public TypeUsageAttribute(params string[] types)
        {
            Type[] ts = new Type[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                ts[i] = Type.GetType(types[i], false, true);

                if (ts[i] == null)
                {
                    foreach (System.Reflection.Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Type t = ass.GetType(types[i]);
                        if (t != null)
                        {
                            ts[i] = t;
                            break;
                        }
                    }

                    if (ts[i] == null)
                    {
                        throw new Exception("TypeUsage Exception: Невозможно найти тип " + ts[i]);
                    }
                }
            }

            usetypes = ts;
        }

        /// <summary>
        /// Возможно вам понадобится этот конструетор для создания экземпляра класса TypeUsageAttribute.
        /// </summary>
        /// <param name="types"> перечислим типы, допустимые по использованию.</param>
        public TypeUsageAttribute(params Type[] types)
        {
            usetypes = new Type[types.Length];
            types.CopyTo(usetypes, 0);
        }

        /// <summary>
        /// список типов, используемых в свойстве.
        /// </summary>
        public Type[] UseTypes
        {
            get
            {
                Type[] result = new Type[usetypes.Length];
                usetypes.CopyTo(result, 0);
                return result;
            }
        }
    }

    /// <summary>
    /// <see cref="TypeUsageAttribute"/>, только указывается не непосредственно у свойства,а у класса.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PropertyTypeUsageAttribute : TypeUsageAttribute
    {
        private string property;

        /// <summary>
        /// Свойство,для которого назначаются типы.
        /// </summary>
        public string Property
        {
            get
            {
                return property;
            }
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса PropertyTypeUsageAttribute.
        /// </summary>
        /// <param name="propertyName">Свойство,для которого назначаются типы.</param>
        /// <param name="types">список типов, используемых в свойстве.</param>
        public PropertyTypeUsageAttribute(string propertyName, params Type[] types)
            : base(types)
        {
            property = propertyName;
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса PropertyTypeUsageAttribute.
        /// </summary>
        /// <param name="propertyName">Свойство,для которого назначаются типы.</param>
        /// <param name="types">список типов, используемых в свойстве.</param>
        public PropertyTypeUsageAttribute(string propertyName, params string[] types)
            : base(types)
        {
            property = propertyName;
        }
    }

    #endregion

    // Хранение (Storage level)
    #region Где данные хранятся
    #region .. Вся сборка

    /// <summary>
    /// Место сохранения объектов данных в данной сборке.
    /// Указывается имя сервиса данных, использующихся для хранения
    /// и параметры к этому сервису данных.
    /// Например, для реляционного сервиса данных -- это DSN.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyStorageAttribute : Attribute
    {
        private string name;

        private Type dataServiceType;

        /// <summary>
        /// наименование хранилища.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// сервис данных для работы с указанным хранилищем.
        /// </summary>
        public Type DataServiceType
        {
            get { return dataServiceType; }
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса AssemblyStorageAttribute.
        /// </summary>
        /// <param name="StorageName">наименование хранилища.</param>
        /// <param name="StorageDataServiceName">сервис данных для работы с указанным хранилищем.</param>
        public AssemblyStorageAttribute(string StorageName, Type StorageDataServiceName)
        {
            name = StorageName;
            dataServiceType = StorageDataServiceName;
        }

        public AssemblyStorageAttribute(string StorageName, string StorageDataServiceName)
            : this(StorageName, Type.GetType(StorageDataServiceName, true, true))
        {
        }
    }
    #endregion
    #region ... Объекты класса

    /// <summary>
    /// Некоторое логическое имя, под которым хранятся экземпляры объектов данных,
    /// например, для реляционного хранения это -- имя таблицы.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassStorageAttribute : Attribute
    {
        private string name;

        /// <summary>
        /// Наименование хранилища для класса.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса ClassStorageAttribute.
        /// </summary>
        /// <param name="StorageName">Наименование хранилища для класса.</param>
        public ClassStorageAttribute(string StorageName)
        {
            name = StorageName;
        }
    }

    /// <summary>
    /// Прописывается тип(класс) генератора первичных ключей.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyGeneratorAttribute : System.Attribute
    {
        private Type typeofgenerator;

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса KeyGeneratorAttribute.
        /// </summary>
        /// <param name="typeofgenerator">тип генератора ключей.</param>
        public KeyGeneratorAttribute(string typeofgenerator)
            : this(Type.GetType(typeofgenerator, true, true))
        {
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса KeyGeneratorAttribute.
        /// </summary>
        /// <param name="typeofgenerator">тип генератора ключей.</param>
        public KeyGeneratorAttribute(Type typeofgenerator)
        {
            this.typeofgenerator = typeofgenerator;
        }

        /// <summary>
        /// тип генератора ключей.
        /// </summary>
        public Type TypeOfGenerator
        {
            get { return typeofgenerator; }
        }
    }
    #endregion
    #region .... Свойства объектов

    /// <summary>
    /// Некоторое логическое имя, под которым хранятся свойства (атрибуты и ассоциации),
    /// например, для реляционного хранения это -- имя поля в таблице.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyStorageAttribute : Attribute
    {
        private string name = string.Empty;
        private string[] names = null;

        /// <summary>
        /// наименование хранилища атрибута.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// наименование хранилища для множественного связывания.
        /// </summary>
        public string[] Names
        {
            get { return names; }
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса PropertyStorageAttribute.
        /// </summary>
        /// <param name="StorageName">наименование хранилища атрибута.</param>
        public PropertyStorageAttribute(string StorageName)
        {
            name = StorageName;
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса PropertyStorageAttribute.
        /// </summary>
        /// <param name="StorageNames">наименование хранилищ атрибута для классов, указанных в TypeUsage.</param>
        public PropertyStorageAttribute(string[] StorageNames)
        {
            names = StorageNames;
        }
    }
    #endregion
    #region .... Первичный ключ

    /// <summary>
    /// Некоторое логическое имя, под которым хранится первичный ключ,
    /// например, для реляционного хранения это -- имя поля в таблице.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyStorageAttribute : Attribute
    {
        private string name;

        /// <summary>
        /// наименование хранилища первичного ключа.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса PrimaryKeyStorageAttribute.
        /// </summary>
        /// <param name="StorageName">наименование хранилища первичного ключа.</param>
        public PrimaryKeyStorageAttribute(string StorageName)
        {
            name = StorageName;
        }
    }
    #endregion
    #region .... ключ типа

    /// <summary>
    /// Некоторое логическое имя, под которым хранится тип,
    /// например, для реляционного хранения это -- имя таблицы.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TypeStorageAttribute : Attribute
    {
        private string name;

        /// <summary>
        /// наименование хранилища для типа.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса PrimaryKeyStorageAttribute.
        /// </summary>
        /// <param name="StorageName">наименование хранилища первичного ключа.</param>
        public TypeStorageAttribute(string StorageName)
        {
            name = StorageName;
        }
    }
    #endregion
    #endregion
    #region Все хранится кроме ...(или хитро хранится)))

    /// <summary>
    /// Отключить автоматическое сохранение мастерового объекта данных при сохранении основного объекта.
    /// </summary>
    public class AutoStoreMasterDisabled : System.Attribute
    {
        private bool fAutoCreateMasterDisabled = true;

        /// <summary>
        /// Отключить автоматическое сохранение мастерового объекта данных при сохранении основного объекта.
        /// </summary>
        /// <param name="fAutoCreateMasterDisabled">если false - обратно включить автосохранение.</param>
        public AutoStoreMasterDisabled(bool fAutoCreateMasterDisabled)
        {
            this.fAutoCreateMasterDisabled = fAutoCreateMasterDisabled;
        }

        /// <summary>
        /// Отключить автоматическое сохранение мастерового объекта данных при сохранении основного объекта.
        /// </summary>
        public AutoStoreMasterDisabled()
            : this(true)
        {
        }

        public bool AutoCreateMasterDisabled
        {
            get
            {
                return fAutoCreateMasterDisabled;
            }

            set
            {
                fAutoCreateMasterDisabled = value;
            }
        }
    }

    /// <summary>
    /// Указывает нехранимый(вычислимый) атрибут (то есть он не сохраняется).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NotStoredAttribute : System.Attribute
    {
        private bool notstored;

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса NotStoredAttribute.
        /// </summary>
        /// <param name="notStored">нехранимое-true,хранимое - false.</param>
        public NotStoredAttribute(bool notStored)
        {
            this.notstored = notStored;
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса NotStoredAttribute.
        /// </summary>
        public NotStoredAttribute()
        {
            this.notstored = true;
        }

        /// <summary>
        /// нехранимое/хранимое.
        /// </summary>
        public bool Value
        {
            get { return notstored; }
        }
    }

    /// <summary>
    /// Указывает формулу вычисления атрибута - для стрингованных данных (для конкретного типа сервиса данных).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DataServiceExpressionAttribute : System.Attribute
    {
        private Type dataServiceType;
        private string storageExpression;

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса DataServiceExpressionAttribute.
        /// </summary>
        /// <param name="DataServiceType">тип датасервиса.</param>
        /// <param name="StorageExpression">запрос-формула.</param>
        public DataServiceExpressionAttribute(Type DataServiceType, string StorageExpression)
        {
            this.dataServiceType = DataServiceType;
            this.storageExpression = StorageExpression;
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса DataServiceExpressionAttribute.
        /// </summary>
        /// <param name="DataServiceType">тип датасервиса.</param>
        /// <param name="StorageExpression">запрос-формула.</param>
        public DataServiceExpressionAttribute(string DataServiceType, string StorageExpression)
            : this(Type.GetType(DataServiceType, false, true), StorageExpression)
        {
        }

        /// <summary>
        /// запрос-формула.
        /// </summary>
        public string Expression
        {
            get { return storageExpression; }
        }

        /// <summary>
        /// тип датасервиса.
        /// </summary>
        public Type TypeofDataService
        {
            get { return dataServiceType; }
        }
    }

    /// <summary>
    /// Определение порядка загрузки атрибутов, для тех случаев,
    /// например, часть объектов детейлов является мастерами других детейлов,
    /// либо, например, для правильного счёта вычислимых атрибутов.
    /// Важно помнить, что принципиальный порядок загрузки остается следующим
    /// - вначале создаются мастера
    /// - затем заполняются свои-мастеровые атрибуты (В заданном порядке)
    /// - заполняются детейлы(в порядке заданном во View).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LoadingOrderAttribute : Attribute
    {
        private string[] order;

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса LoadingOrderAttribute.
        /// </summary>
        /// <param name="order"></param>
        public LoadingOrderAttribute(string[] order)
        {
            this.order = order;
        }

        /// <summary>
        /// порядок загрузки атрибутов.
        /// </summary>
        public string[] Order
        {
            get { return order; }
            set { order = value; }
        }
    }
    #endregion
    #region Как хранятся

    /// <summary>
    /// Как хранить в конкретном хранилище указанный тип или свойство.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = true)]
    public class StoreInstancesInTypeAttribute : System.Attribute
    {
        /// <summary>
        /// Тип сервиса данных.
        /// </summary>
        public System.Type DataServiceType;

        /// <summary>
        /// Тип хранения.
        /// </summary>
        public System.Type StorageType;

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса StoreInstancesInTypeAttribute
        /// Если тип сервиса не найден, исключение выброшено не будет.
        /// </summary>
        /// <param name="dataservice">тип сервиса данных.</param>
        /// <param name="type">как хранится.</param>
        public StoreInstancesInTypeAttribute(string dataservice, string stype)
            : this(Type.GetType(dataservice, false, true), Type.GetType(stype, false, true))
        {
        }

        /// <summary>
        /// Возможно вам понадобится этот конструктор для создания экземпляра класса StoreInstancesInTypeAttribute.
        /// </summary>
        /// <param name="dataservice">тип сервиса данных.</param>
        /// <param name="type">как хранится.</param>
        public StoreInstancesInTypeAttribute(System.Type dataservice, System.Type type)
        {
            DataServiceType = dataservice;
            StorageType = type;
        }
    }

    /// <summary>
    /// происходит обрубание строк(применять функцию Trim()) при работе со строковыми данными через
    /// <see cref="Information.GetPropValueByName"/>  и <see cref="Information.GetPropValueByName"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class TrimmedStringStorageAttribute : System.Attribute
    {
        private bool trimmedStrings;

        /// <summary>
        /// обрубать строки,или нет.
        /// </summary>
        public bool TrimmedStrings
        {
            get { return trimmedStrings; }
        }

        /// <summary>
        /// обрубать строки,или нет.
        /// </summary>
        /// <param name="trimmedStrings">обрубать строки,или нет.</param>
        public TrimmedStringStorageAttribute(bool trimmedStrings)
        {
            this.trimmedStrings = trimmedStrings;
        }

        /// <summary>
        /// обрубать строки.
        /// </summary>
        public TrimmedStringStorageAttribute()
        {
            this.trimmedStrings = true;
        }
    }
    #endregion

    // Представление (Presentation level)
    #region Определение представления

    /// <summary>
    /// Указывает представление для класса объекта данных
    /// формат указания: [Имя мастера].[Имя мастера мастера].[Имя мастера мастера мастера ...].[Имя атрибута] as [пользовательское имя атрибута]
    /// Пользовательское имя атрибута может включать пробелы
    /// В исходный код этот атрибут полностью генерируется CASE.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ViewAttribute : System.Attribute
    {
        private string[] properties;
        private string[] hiddenProperties;
        private string viewName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ViewName">имя представления.</param>
        /// <param name="properties">свойства в представлении.</param>
        public ViewAttribute(string ViewName, string[] properties)
        {
            if (ViewName == string.Empty)
            {
                throw new Exception("View name in ViewAttribute coultd not be empty");
            }

            this.viewName = ViewName;
            this.properties = (properties == null) ? null : (string[])properties.Clone();
            this.hiddenProperties = new string[0];
        }

        /// <summary>
        /// имя представления.
        /// </summary>
        public string Name
        {
            get { return viewName; }
        }

        /// <summary>
        /// состав атрибутов.
        /// </summary>
        public string[] Properties
        {
            get { return (properties == null) ? null : (string[])properties.Clone(); }
        }

        /// <summary>
        /// невидимые атрибуты.
        /// </summary>
        public string[] Hidden
        {
            get
            {
                return (hiddenProperties == null) ? null : (string[])hiddenProperties.Clone();
            }

            set
            {
                hiddenProperties = (value == null) ? null : (string[])value.Clone();
            }
        }
    }

    /// <summary>
    /// Указывает для представления класса шапки представление детейла.
    /// В сущности, представление шапки включает в себя представления детейлов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AssociatedDetailViewAttribute : System.Attribute
    {
        private string viewname;
        private string detailViewName;
        private string detailName;
        private bool detailLoadOnLoadAgregator;
        private string detailPath;
        private string detailcaption;
        private bool detailvisible;
        private string[] aggregateOperations;
        private bool detailUseAdaptiveViewsLoading = false;

        /// <summary>
        /// использовать ли адаптивную вычитку.
        /// </summary>
        public bool UseAdaptiveViewsLoading
        {
            get { return detailUseAdaptiveViewsLoading; }
            set { detailUseAdaptiveViewsLoading = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewname">представление шапки.</param>
        /// <param name="detailname">имя детейла.</param>
        /// <param name="detailviewname">представление детейла.</param>
        /// <param name="detailLoadOnLoadAgregator">читать ли детейл при чтении агрегирующего объекта.</param>
        /// <param name="detailPath">путь на форме.</param>
        /// <param name="detailCaption">заголовок.</param>
        /// <param name="detailVisible">видимость.</param>
        /// <param name="AggregateOperations">агр.функции.</param>
        public AssociatedDetailViewAttribute(string viewname, string detailname, string detailviewname, bool detailLoadOnLoadAgregator, string detailPath, string detailCaption, bool detailVisible, string[] AggregateOperations)
        {
            this.detailName = detailname;
            this.detailViewName = detailviewname;
            this.viewname = viewname;
            this.detailLoadOnLoadAgregator = detailLoadOnLoadAgregator;
            this.detailPath = detailPath;
            this.detailcaption = detailCaption;
            this.detailvisible = detailVisible;
            this.aggregateOperations = (AggregateOperations == null) ? null : (string[])AggregateOperations.Clone();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewname">представление.</param>
        /// <param name="detailname">имя детейла.</param>
        /// <param name="detailviewname">представление детейла.</param>
        /// <param name="detailLoadOnLoadAgregator">прочитывать ли при загрузке.</param>
        public AssociatedDetailViewAttribute(string viewname, string detailname, string detailviewname, bool detailLoadOnLoadAgregator)
            : this(viewname, detailname, detailviewname, detailLoadOnLoadAgregator, string.Empty, string.Empty, false, new string[0])
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewname">представление.</param>
        /// <param name="detailname">имя детейла.</param>
        /// <param name="detailviewname">представление детейла.</param>
        public AssociatedDetailViewAttribute(string viewname, string detailname, string detailviewname)
            : this(viewname, detailname, detailviewname, true)
        {
        }

        /// <summary>
        /// имя детейла.
        /// </summary>
        public string DetailName
        {
            get { return detailName; }
        }

        /// <summary>
        /// представление детейла.
        /// </summary>
        public string DetailViewName
        {
            get { return detailViewName; }
        }

        /// <summary>
        /// представление шапки.
        /// </summary>
        public string ViewName
        {
            get { return viewname; }
        }

        /// <summary>
        /// читать ли детейл при чтении агрегирующего объекта.
        /// </summary>
        public bool LoadOnLoadAgregator
        {
            get { return detailLoadOnLoadAgregator; }
            set { detailLoadOnLoadAgregator = value; }
        }

        /// <summary>
        /// где располагается на форме.
        /// </summary>
        public string FormPath
        {
            get { return detailPath; }
            set { detailPath = value; }
        }

        /// <summary>
        /// Заголовок на форме.
        /// </summary>
        public string Caption
        {
            get { return detailcaption; }
            set { detailcaption = value; }
        }

        /// <summary>
        /// видимость контрола на форме.
        /// </summary>
        public bool Visible
        {
            get { return detailvisible; }
            set { detailvisible = value; }
        }

        /// <summary>
        /// аггрегирующие операции доступные в этом представлении.
        /// </summary>
        public string[] AggregateOperations
        {
            get { return (aggregateOperations == null) ? null : (string[])aggregateOperations.Clone(); }
            set { aggregateOperations = value; }
        }
    }
    #endregion
    #region Контролы для представления

    /// <summary>
    /// Указывает пользовательское имя для значения перечислимого типа, для класса данных, для свойства класса данных.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field)]
    public class CaptionAttribute : Attribute
    {
        private string value;

        /// <summary>
        /// заголовок.
        /// </summary>
        public string Value
        {
            get { return value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="caption">заголовок.</param>
        public CaptionAttribute(string caption)
        {
            value = caption;
        }
    }

    /// <summary>
    /// Если true, то значения перечислимого типа
    /// должны быть отображены в виде ComboBox,
    /// иначе группа RadioButton.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class OnlyShowSelectedValueAttribute : Attribute
    {
        private bool value;

        /// <summary>
        /// показывать только выделенное значение?.
        /// </summary>
        public bool Value
        {
            get { return value; }
        }

        /// <summary>
        /// показывать только выделенное значение.
        /// </summary>
        /// <param name="showSelected">показывать только выделенное значение?.</param>
        public OnlyShowSelectedValueAttribute(bool showSelected)
        {
            value = showSelected;
        }
    }

    /// <summary>
    /// Атрибут, указывающий значение перечисления, рассматриваемое как незаполненное.
    /// </summary>
    public class EmptyEnumValueAttribute : Attribute
    {
    }
    #endregion

    /// <summary>
    /// Помещать ли свойство в автоматически генерируемые прадставления.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisableAutoViewedAttribute : System.Attribute
    {
        /// <summary>
        /// Значение (true/false).
        /// </summary>
        public bool value = true;

        /// <summary>
        ///
        /// </summary>
        public DisableAutoViewedAttribute()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        public DisableAutoViewedAttribute(bool val)
        {
            value = val;
        }
    }

    /// <summary>
    /// Помещать ли свойство в Insert-ы.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisableInsertPropertyAttribute : System.Attribute
    {
        /// <summary>
        /// Значение (true/false).
        /// </summary>
        public bool Value = true;

        /// <summary>
        ///
        /// </summary>
        public DisableInsertPropertyAttribute()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        public DisableInsertPropertyAttribute(bool val)
        {
            Value = val;
        }
    }

    // Поведение объекта

    /// <summary>
    /// Автоматическое вычисление статуса Altered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoAlteredAttribute : Attribute
    {
        /// <summary>
        /// автоматическое вычисление статуса Altered.
        /// </summary>
        public bool value = true;

        /// <summary>
        ///
        /// </summary>
        public AutoAlteredAttribute()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="avalue">Если True, статус Altered вычисляется автоматически.</param>
        public AutoAlteredAttribute(bool avalue)
        {
            value = avalue;
        }
    }

    /// <summary>
    /// Картинка для класса.
    /// Должна лежать в этой же сборке как embedded ресурс.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassImageFileAttribute : System.Attribute
    {
        /// <summary>
        /// Имя картинки.
        /// </summary>
        public string fileName = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="fName">Имя картинки.</param>
        public ClassImageFileAttribute(string fName)
        {
            fileName = fName;
        }
    }

    /// <summary>
    /// Свойство, предоставляющее картинку для экземпляров этого класса.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassImagePropertyAttribute : System.Attribute
    {
        /// <summary>
        /// название свойства.
        /// </summary>
        public string property = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="fproperty">название свойства.</param>
        public ClassImagePropertyAttribute(string fproperty)
        {
            property = fproperty;
        }
    }

    /// <summary>
    /// Свойство, предоставляющее заголовок для экземпляров этого класса.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InstanceCaptionPropertyAttribute : System.Attribute
    {
        /// <summary>
        /// название свойства.
        /// </summary>
        public string fieldCaptionProperty = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="CaptionProperty">название свойства.</param>
        public InstanceCaptionPropertyAttribute(string CaptionProperty)
        {
            fieldCaptionProperty = CaptionProperty;
        }
    }

    /// <summary>
    /// Агрегирующая функция в DetailArray.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AggregationFunctionAttribute : System.Attribute
    {
        /// <summary>
        /// как выводить результат ( типа "сумма = {0} или больше").
        /// </summary>
        public string CaptionFormat;

        /// <summary>
        /// под каким свойством выводить в табличных контролах.
        /// </summary>
        public string PropertyName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="captionFormat">как выводить результат ( типа "сумма = {0} или больше").</param>
        /// <param name="propertyName">под каким свойством выводить в табличных контролах.</param>
        public AggregationFunctionAttribute(string captionFormat, string propertyName)
        {
            CaptionFormat = captionFormat;
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// Типы выбора связанного объекта.
    /// </summary>
    public enum LookupTypeEnum
    {
        /// <summary>
        /// Выпадающий список - комбобокс
        /// </summary>
        Combo,

        /// <summary>
        /// По умолчанию, в отдельном списке
        /// </summary>
        Standard,

        /// <summary>
        /// Коротенький список по представлению
        /// </summary>
        Quick,

        /// <summary>
        /// Другой, произвольный
        /// </summary>
        Custom,
    }

    /// <summary>
    /// Дополнительная настройка мастера в представлении.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MasterViewDefineAttribute : System.Attribute
    {
        private LookupTypeEnum lookupType;
        private string viewName;
        private string masterName;
        private string customizationString;
        private string lookupProperty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewname">имя представления.</param>
        /// <param name="mastername">имя свойства - мастера.</param>
        /// <param name="lookuptype">тип лукапа.</param>
        /// <param name="customizationstring">настройка лукапа.</param>
        /// <param name="lookupProperty">отображаемое свойство.</param>
        public MasterViewDefineAttribute(string viewname, string mastername, LookupTypeEnum lookuptype, string customizationstring, string lookupProperty)
        {
            viewName = viewname;
            masterName = mastername;
            lookupType = lookuptype;
            customizationString = customizationstring;
            this.lookupProperty = lookupProperty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewname"></param>
        /// <param name="mastername"></param>
        /// <param name="lookuptype"></param>
        public MasterViewDefineAttribute(string viewname, string mastername, LookupTypeEnum lookuptype)
            : this(viewname, mastername, lookuptype, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// имя представления.
        /// </summary>
        public string ViewName
        {
            get { return viewName; }
        }

        /// <summary>
        /// имя свойства - мастера.
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
        /// отображаемое свойство.
        /// </summary>
        public string LookupProperty
        {
            get { return lookupProperty; }
        }
    }

    /// <summary>
    /// Тип для кэтчера.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EventArgCatcherTypeAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public string value = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        public EventArgCatcherTypeAttribute(string type)
        {
            value = type;
        }
    }
}
