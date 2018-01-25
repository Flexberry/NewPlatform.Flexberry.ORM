using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// проверить константу
    /// </summary>
    public delegate bool CheckConstDelegate(ObjectType sender, ref string value);

    /// <summary>
    /// Мета-описание типа (используется для описания типов операндов функций) (наследуется от ViewedObject)
    /// </summary>
    [NotStored]
    public class ObjectType : ViewedObject
    {
        //private CheckConstDelegate fieldCheckConst;
        private Type fieldNetCompatibilityType;
        private bool fieldEditableInTextBox = true;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="objImagedView"></param>
        /// <param name="netCompatibilityType"></param>
        public ObjectType(string objStringedView, string objCaption, Type netCompatibilityType)
            :
            base(objStringedView, objCaption) { fieldNetCompatibilityType = netCompatibilityType; }

        public ObjectType() { }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="objImagedView"></param>
        /// <param name="netCompatibilityType"></param>
        /// <param name="EditableInTextBox"></param>
        public ObjectType(string objStringedView, string objCaption, Type netCompatibilityType, bool EditableInTextBox)
            :
            base(objStringedView, objCaption) { fieldNetCompatibilityType = netCompatibilityType; fieldEditableInTextBox = EditableInTextBox; }
        /// <summary>
        /// .NET тип для этого типа
        /// </summary>
        public virtual System.Type NetCompatibilityType { get { return fieldNetCompatibilityType; } }


        [XmlElement("NetCompatibilityType")]
        public string AssemblyQualifiedTypeName
        {
            get { return NetCompatibilityType == null ? null : NetCompatibilityType.AssemblyQualifiedName; }
            set { fieldNetCompatibilityType = string.IsNullOrEmpty(value) ? null : Type.GetType(value); }
        }

        /// <summary>
        /// Можно ли его поредактировать в текстбоксе
        /// </summary>
        public bool EditableInTextBox { get { return fieldEditableInTextBox; } }

        [NonSerialized]
        private FunctionalLanguageDef fieldLanguage;
        /// <summary>
        /// Язык, в рамках которого определён этот тип
        /// </summary>
        [ICSSoft.STORMNET.Agregator]
        [XmlIgnore]
        public virtual FunctionalLanguageDef Language { get { return fieldLanguage; } set { fieldLanguage = value; } }
        /// <summary>
        /// Совместим с...
        /// </summary>
        /// <param name="type">тип</param>
        /// <returns>совместим ли</returns>
        public virtual bool CompatWith(ObjectType type)
        {
            if (this == type)
                return true;
            //CompatibilityTypeTest tst = new CompatibilityTypeTest();

            bool retBool = CompatibilityTypeTest.Check(NetCompatibilityType, type.NetCompatibilityType) != TypesCompatibilities.No;
            return retBool;
        }
        /// <summary>
        /// Получить только совпадающие типы
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool CompatWithEqual(ObjectType type)
        {
            if (this == type)
                return true;
            //CompatibilityTypeTest tst = new CompatibilityTypeTest();

            //bool retBool = CompatibilityTypeTest.Check(NetCompatibilityType, type.NetCompatibilityType) == TypesCompatibilities.Equal;
            bool retBool = NetCompatibilityType == type.NetCompatibilityType;

            if(!retBool && type.NetCompatibilityType.IsGenericType && type.NetCompatibilityType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                retBool = NetCompatibilityType == Nullable.GetUnderlyingType(type.NetCompatibilityType);
            }

            return retBool;
        }
        /// <summary>
        /// упрощение значения
        /// </summary>
        [XmlIgnoreAttribute]
        public SimplificationDelegate SimplificationValue;
        /// <summary>
        /// разупрощение значения
        /// </summary>
        [XmlIgnoreAttribute]
        public SimplificationDelegate UnSimplificationValue;

        /// <summary>
        /// Значение в простое значение
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object ValueToSimpleValue(object value)
        {
            if (SimplificationValue != null)
            {
                try
                {
                    return SimplificationValue(value);
                }
                catch
                {
                    return value;
                }
            }
            return value;
        }

        /// <summary>
        /// Простое значение в значение
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object SimpleValueToValue(object value)
        {
            if (UnSimplificationValue != null)
            {
                return UnSimplificationValue(value);
            }
            return value;
        }
    }
    /// <summary>
    /// упрощение
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate object SimplificationDelegate(object value);

    

}
