namespace ICSSoft.STORMNET.Exceptions
{
    using System;

    public interface IErrorMessage
    {
        string GetMessage();
    }

    /// <summary>
    /// Нет такого свойства (Исключительная ситуация).
    /// </summary>
    public class NoSuchPropertyException : Exception
    {
        /// <summary>
        /// Где нет.
        /// </summary>
        public Type Type;

        /// <summary>
        /// Чего нет.
        /// </summary>
        public string PropName;

        /// <summary>
        /// Нет такого свойства.
        /// </summary>
        /// <param name="Type">Где нет.</param>
        /// <param name="PropName">Чего нет.</param>
        public NoSuchPropertyException(Type Type, string PropName)
        {
            this.Type = Type;
            this.PropName = PropName;
        }

        /// <summary>
        ///
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("Property <{0}> not found in Type <{1}>", PropName, Type);
            }
        }
    }

    /// <summary>
    /// Нет такого представления.
    /// </summary>
    public class CantFindViewException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <param name="viewName">Имя представления.</param>
        public CantFindViewException(Type type, string viewName)
        {
            Type = type;
            ViewName = viewName;
        }

        /// <summary>
        /// Тип, в котором искали представление.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Имя представления.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("View <{0}> not found in Type <{1}>", ViewName, Type);
            }
        }
    }

    /// <summary>
    /// При разблокировке объекта используется неверный ключ.
    /// </summary>
    public class UnlockObjectDifferentKeyException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public UnlockObjectDifferentKeyException()
        {
        }
    }

    /// <summary>
    /// Объект заблокирован.
    /// </summary>
    public class DataObjectIsReadOnlyException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public DataObjectIsReadOnlyException()
        {
        }
    }

    /// <summary>
    /// Невозможно отсортировать объектв по этому свойству.
    /// </summary>
    public class NotSortableOrderColumnsType : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public NotSortableOrderColumnsType()
        {
        }
    }

    /// <summary>
    /// Этот DetailArray нельзя отсортировать.
    /// </summary>
    public class NotSortableDetailArrayException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public NotSortableDetailArrayException()
        {
        }
    }

    /// <summary>
    /// объект уже в DetailArray.
    /// </summary>
    public class ObjectAlreadyInDetailArrayException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public ObjectAlreadyInDetailArrayException()
        {
        }
    }

    /// <summary>
    /// DetailArray уже содержиться объект с такимже калючем.
    /// </summary>
    public class DetailArrayAlreadyContainsObjectWithThatKeyException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public DetailArrayAlreadyContainsObjectWithThatKeyException()
        {
        }
    }

    /// <summary>
    /// представление не подходит для класса.
    /// </summary>
    public class UncompatibleViewForClassException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public string viewName;

        /// <summary>
        ///
        /// </summary>
        public System.Type classType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewN"></param>
        /// <param name="classT"></param>
        public UncompatibleViewForClassException(string viewN, System.Type classT)
        {
            viewName = viewN;
            classType = classT;
        }
    }

    /// <summary>
    /// Один класс не является потомком другого класса.
    /// </summary>
    public class ClassIsNotSubclassOfOtherException : Exception
    {
        /// <summary>
        /// проверяемый.
        /// </summary>
        public System.Type checkedType;

        /// <summary>
        /// с чем сравниваем.
        /// </summary>
        public System.Type baseType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="checkedT"></param>
        /// <param name="baseT"></param>
        public ClassIsNotSubclassOfOtherException(Type checkedT, Type baseT)
            : base(string.Format("Class <{0}> isn't subclass of <{1}>", checkedT, baseT))
        {
            checkedType = checkedT;
            baseType = baseT;
        }
    }

    /// <summary>
    /// Не обнаружено свойство.
    /// </summary>
    public class CantFindPropertyException : Exception
    {
        /// <summary>
        /// имя свойства.
        /// </summary>
        public string propertyName;

        /// <summary>
        /// в каком типе.
        /// </summary>
        public System.Type classType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="prop">имя свойства.</param>
        /// <param name="type">в каком типе.</param>
        public CantFindPropertyException(string prop, Type type)
            : base("Cant find property <" + prop + "> in class " + type.FullName)
        {
            propertyName = prop;
            classType = type;
        }
    }

    /// <summary>
    /// Агрегатор должен быть приводим к DataObject.
    /// </summary>
    public class AgregatorPropertyMustBeDataObjectTypeException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public System.Type DefinitionType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="defType"></param>
        public AgregatorPropertyMustBeDataObjectTypeException(System.Type defType)
        {
            DefinitionType = defType;
        }
    }

    /// <summary>
    /// при создании DetailArray необходимо передать объект-владелец.
    /// </summary>
    public class OnCreationDetailArrayAgregatorObjectCantBeNullException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public OnCreationDetailArrayAgregatorObjectCantBeNullException()
        {
        }
    }

    /// <summary>
    /// нет возможности обработать не DataObject.
    /// </summary>
    public class CantProcessingNonDataobjectTypeException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public CantProcessingNonDataobjectTypeException()
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class DifferentDataObjectTypesException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public DifferentDataObjectTypesException()
        {
        }
    }

    /// <summary>
    /// Неверный тип первичного ключа.
    /// </summary>
    public class PrimaryKeyTypeException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public PrimaryKeyTypeException()
        {
        }
    }

    /// <summary>
    /// Исключение,возникающее в операциях над представлениями пи несовместимости представлений.
    /// </summary>
    public class IncompatibleTypesForViewOperationException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public string FirstViewType;

        /// <summary>
        ///
        /// </summary>
        public string SecondViewType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="FirstViewType"></param>
        /// <param name="SecondViewType"></param>
        public IncompatibleTypesForViewOperationException(string FirstViewType, string SecondViewType)
        {
            this.FirstViewType = FirstViewType;
            this.SecondViewType = SecondViewType;
        }
    }

    /// <summary>
    /// Исключение,возникающее программной несовмести типов (например при присваивании мастероваму свойству объекта типе, не включенного в UsingType).
    /// </summary>
    public class IncompatibleTypeException : Exception
    {
        private string sTypeName;
        private string sMustBeTypes;
        private string sPropName;
        private string sObjectType;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sObjectType">тип объекта в котором возникло исключение.</param>
        /// <param name="sPropName">свойство в котором возникло исключение.</param>
        /// <param name="sTypeName">имя типа на котором возникло исключение.</param>
        /// <param name="sMustBeTypes">должны бать следующие типы.</param>
        public IncompatibleTypeException(string sObjectType, string sPropName, string sTypeName, string sMustBeTypes)
        {
            this.sObjectType = sObjectType;
            this.sPropName = sPropName;
            this.sTypeName = sTypeName;
            this.sMustBeTypes = sMustBeTypes;
        }

        /// <summary>
        /// тип объекта в котором возникло исключение.
        /// </summary>
        public string TypeName
        {
            get { return sTypeName; }
        }

        /// <summary>
        /// олжны бать следующие типы.
        /// </summary>
        public string MustBeTypes
        {
            get { return sMustBeTypes; }
        }

        /// <summary>
        /// имя типа на котором возникло исключение.
        /// </summary>
        public string ObjectType
        {
            get { return sObjectType; }
        }

        /// <summary>
        /// свойство в котором возникло исключение.
        /// </summary>
        public string PropName
        {
            get { return sPropName; }
        }
    }

    /// <summary>
    ///  Тип проверяемого объекта не соответствует типу в TypeUsageAttribute.
    /// </summary>
    public class IncomatibleCheckingTypeException : IncompatibleTypeException
    {
        /// <summary>
        ///
        /// </summary>
        public IncomatibleCheckingTypeException(string sObjectType, string sPropName, string sTypeName, string sMustBeTypes)
            : base(sObjectType, sPropName, sTypeName, sMustBeTypes)
        {
        }
    }

    /// <summary>
    /// Тип не является перечислимым типом.
    /// </summary>
    public class NotEnumTypeException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="TypeName"></param>
        public NotEnumTypeException(string TypeName)
            : base(TypeName)
        {
        }
    }

    /// <summary>
    /// не смогли найти свойство указывающее на объект-владелец.
    /// </summary>
    public class NotFoundAggregatorProperty : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public NotFoundAggregatorProperty()
            : base()
        {
        }
    }

    /// <summary>
    /// Не нашли в типах.
    /// </summary>
    public class NotFoundInTypeUsageException : Exception
    {
        /// <summary>
        /// Тип для свойсва которого установлен TypeUsage.
        /// </summary>
        public Type ObjectType;

        /// <summary>
        /// Свойство для которого установлен TypeUsage.
        /// </summary>
        public string Property;

        /// <summary>
        /// Проверяемый тип.
        /// </summary>
        public Type CheckedType;

        /// <summary>
        /// Не нашли в типах.
        /// </summary>
        /// <param name="objectType"> Тип для свойсва которого установлен TypeUsage.</param>
        /// <param name="property">Свойство для которого установлен TypeUsage.</param>
        /// <param name="checkedType">Проверяемый тип.</param>
        public NotFoundInTypeUsageException(Type objectType, string property, Type checkedType)
        {
            ObjectType = objectType;
            Property = property;
            CheckedType = checkedType;
        }
    }
}
