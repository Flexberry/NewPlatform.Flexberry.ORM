namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// В виде этой структуры приходят строковое предстваление объектов.
    /// </summary>
    [Serializable]
    public struct ObjectStringDataView : ISerializable
    {
        /// <summary>
        /// Приватное поле для свойства <see cref="Key"/>.
        /// </summary>
        private object key;

        /// <summary>
        /// Приватное поле для свойства <see cref="ObjectType"/>.
        /// </summary>
        private Type objectType;

        /// <summary>
        /// Приватное поле для свойства <see cref="ObjectedData"/>.
        /// </summary>
        private object[] objectedData;

        /// <summary>
        /// Приватное поле для свойства <see cref="Separator"/>.
        /// </summary>
        private char separator;

        /// <summary>
        /// Приватное поле для свойства <see cref="Masters"/>.
        /// </summary>
        private MasterObjStruct[] masters;

        /// <summary>
        /// Мастера (связанные обьекты).
        /// </summary>
        public MasterObjStruct[] Masters
        {
            get => masters;
            set => masters = value?.Clone() as MasterObjStruct[];
        }

        /// <summary>
        /// Ключ объекта.
        /// </summary>
        public object Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// Разделитель в строке.
        /// </summary>
        public char Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        /// <summary>
        /// Тип объекта.
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return objectType;
            }

            set
            {
                if (value.IsSubclassOf(typeof(DataObject)))
                    objectType = value;
                else
                    throw new Exceptions.CantProcessingNonDataobjectTypeException();
            }
        }

        /// <summary>
        /// Преобразование простых типизированных данных в строку.
        /// </summary>
        /// <param name="value">Данные для преобразования.</param>
        /// <returns>Преобразованные в строку данные.</returns>
        public static string ConvertSimpleValueString(object value)
        {
            if (value == null) 
                return null;

            Type valType = value.GetType();
            if (valType == typeof(Decimal))
            {
                return ((decimal)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }

            if (valType == typeof(Guid))
                return ((Guid)value).ToString("B");

            return value.ToString();
        }

        /// <summary>
        /// Строковое представление. Геттер данного свойства выполняет логику построения строки из сырых данных, поэтому не вызывайте его лишний раз.
        /// </summary>
        public string Data
        {
            get
            {
                if (objectedData == null || objectedData.Length == 0)
                    return null;
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder(ConvertSimpleValueString(objectedData[0]));
                for (int i = 1; i < objectedData.Length; i++)
                {
                    sb.Append(separator);
                    sb.Append(ConvertSimpleValueString(objectedData[i]));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Массив с данными.
        /// </summary>
        public object[] ObjectedData
        {
            get => objectedData;
            set => objectedData = value; //TODO: ivashkevich: почему тут просто присвоение, а для Masters через clone? Хотя в конструкторе оба через Clone
        }

        /// <summary>
        /// Создать структуру строкового представления объекта данных.
        /// </summary>
        /// <param name="key">Ключ объекта.</param>
        /// <param name="objectType">Тип объекта.</param>
        /// <param name="separator">Разделитель в строке.</param>
        /// <param name="masters">Массив структур мастеров.</param>
        /// <param name="objectedData">Массив данных.</param>
        public ObjectStringDataView(object key, Type objectType, char separator, MasterObjStruct[] masters, object[] objectedData)
        {
            this.key = key;
            this.objectType = objectType;
            this.separator = separator;
            this.masters = masters?.Clone() as MasterObjStruct[];
            this.objectedData = objectedData?.Clone() as object[];
        }

        /// <summary>
        /// Конструктор десериализации.
        /// </summary>
        /// <param name="info">Информация для десериализации.</param>
        /// <param name="context">Контекст десериализации.</param>
        public ObjectStringDataView(SerializationInfo info, StreamingContext context)
        {
            this.objectType = (Type)info.GetValue("objectType", typeof(Type));
            this.objectedData = (object[])info.GetValue("data", typeof(object[]));
            this.separator = info.GetChar("separator");
            this.masters = info.GetValue("masters", typeof(MasterObjStruct[])) as MasterObjStruct[];

            Type tp = (Type)info.GetValue("keytype", typeof(Type));

            string s = info.GetString("keystr");

            MethodInfo mi = tp.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);

            if (mi != null)
            {
                this.key = mi.Invoke(null, new object[] { s });
            }
            else
            {
                this.key = Activator.CreateInstance(tp, new object[] { s });
            }
        }

        /// <summary>
        /// Метод сериализации.
        /// </summary>
        /// <param name="info">Информация для сериализации.</param>
        /// <param name="context">Контест сериализации.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("objectType", this.objectType);
            info.AddValue("data", this.objectedData);
            info.AddValue("separator", this.separator);
            info.AddValue("masters", this.masters);

            info.AddValue("keytype", this.key.GetType());
            info.AddValue("keystr", this.key.ToString());
        }

        //TODO: ivashkevich: переопределить Equals и GetHashCode
    }
}