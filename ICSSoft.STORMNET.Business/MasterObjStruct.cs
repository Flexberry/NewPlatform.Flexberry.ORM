namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Структура мастеров в выборке
    /// </summary>
    [Serializable]
    public class MasterObjStruct : ISerializable
    {
        private object key;
        private System.Type objectType;
        private string propertyName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="key">ключ мастера</param>
        /// <param name="objectType">тип мастера</param>
        /// <param name="propName">имя роли(свойства)</param>
        public MasterObjStruct(object key, Type objectType, string propName)
        {
            this.key = key;
            this.objectType = objectType;
            this.propertyName = propName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MasterObjStruct(SerializationInfo info, StreamingContext context)
        {
            this.objectType = (System.Type)info.GetValue("objectType", typeof(Type));
            this.propertyName = info.GetString("propertyName");

            System.Type tp = (System.Type)info.GetValue("keytype", typeof(Type));

            string s = info.GetString("keystr");

            MethodInfo mi = tp.GetMethod("Parse", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

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
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("objectType", this.objectType);
            info.AddValue("propertyName", this.propertyName);

            info.AddValue("keytype", this.key.GetType());
            info.AddValue("keystr", this.key.ToString());
        }

        /// <summary>
        /// Ключ мастера
        /// </summary>
        public object Key { get { return key; } set { key = value; } }

        /// <summary>
        /// Тип мастера
        /// </summary>
        public Type ObjectType { get { return objectType; } set { objectType = value; } }

        /// <summary>
        /// Имя роли мастера(наименование свойства)
        /// </summary>
        public string PropertyName { get { return propertyName; } set { propertyName = value; } }
    }
}