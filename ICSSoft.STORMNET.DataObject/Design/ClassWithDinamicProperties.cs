namespace ICSSoft.STORMNET.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Абстрактный класс позволяющий потомкам иметь динамический состав
    /// свойств отображаемый в PropertyEditor.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ClassWithDinamicProperties : ICustomTypeDescriptor
    {
        /// <summary>
        ///
        /// </summary>
        public ClassWithDinamicProperties()
        {
        }
        #region ICustomTypeDescriptor Members

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// коллекция динамических свойств.
        /// </summary>
        protected PropertyDescriptorCollection properties;

        /// <summary>
        /// Свойства соответствующие составу атрибутов.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract PropertyDescriptorCollection GetProperties(Attribute[] attributes);

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        #endregion

    }

    /// <summary>
    /// Описатель динамического свойства.
    /// </summary>
    public class DinamicPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// тип свойства.
        /// </summary>
        protected System.Type propertyType;

        /// <summary>
        /// значение свойства.
        /// </summary>
        protected object value;

        /// <summary>
        /// создаватель экземпляров класса DinamicPropertyDescriptor.
        /// </summary>
        /// <param name="name">имя свойства.</param>
        /// <param name="propType">тип свойства.</param>
        /// <param name="initValue">начальное значение.</param>
        /// <param name="attrs">атрибуты свойства.</param>
        public DinamicPropertyDescriptor(string name, System.Type propType, object initValue, params Attribute[] attrs)
            : base(name, attrs)
        {
            propertyType = propType;
            this.value = initValue;
        }

        /// <summary>
        /// Тип компонента.
        /// </summary>
        public override Type ComponentType
        {
            get { return typeof(string); }
        }

        /// <summary>
        /// Только на чтение ?.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Тип свойства.
        /// </summary>
        public override Type PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// можно ли вернуть начальные значения (False).
        /// </summary>
        /// <param name="component"></param>
        /// <returns>ложь.</returns>
        public override bool CanResetValue(object component)
        {
            return false;
        }

        /// <summary>
        /// Взять значение.
        /// </summary>
        /// <param name="component">компонент у которого берется значение.</param>
        /// <returns>значение.</returns>
        public override object GetValue(object component)
        {
            return value;
        }

        /// <summary>
        /// Вернуть начальное значение.
        /// </summary>
        /// <param name="component">объект у которого устанавливается свойство.</param>
        public override void ResetValue(object component)
        {
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="component">объект.</param>
        /// <param name="value">значение.</param>
        public override void SetValue(object component, object value)
        {
            this.value = value;
        }

        /// <summary>
        /// Можно ли сериализовать свойство.
        /// </summary>
        /// <param name="component">объект.</param>
        /// <returns>ложь.</returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
