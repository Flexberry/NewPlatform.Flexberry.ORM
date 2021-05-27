namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml;

    using ICSSoft.STORMNET.Business.Audit.Objects;

    using DataObject = ICSSoft.STORMNET.DataObject;

    /// <summary>
    /// Базовый класс для установки в очередь сообщений при асинхронной записи аудита.
    /// </summary>
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.RatificationAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.CommonAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.CheckedCustomAuditParameters))]
    public abstract class AuditParameters : DatabaseAuditParameters
    {
        [DataMember]
        public bool ThrowExceptions;

        [DataMember]
        public tExecutionVariant ExecutionResult;

        [DataMember]
        public DateTime CurrentTime;

        [DataMember]
        public tWriteMode WriteMode;

        [DataMember]
        public bool NeedSerialization = false;

        public AuditParameters(
            tExecutionVariant executionResult,
            DateTime currentTime,
            AppMode applicationMode,
            string auditConnectionStringName,
            bool needSerialization)
            : base(auditConnectionStringName, applicationMode)
        {
            ExecutionResult = executionResult;
            CurrentTime = currentTime;
            NeedSerialization = needSerialization;
        }

        public virtual Guid? GetAuditEntityGuid()
        {
            throw new Exception("Получение AuditEntityGuid доступно в некоторых классах-потомках.");
        }

        /// <summary>
        /// Конструирование объекта данных из xml.
        /// </summary>
        /// <param name="typeOfObject"> Имя типа объекта данных. </param>
        /// <param name="assemblyOfObject"> Имя сборки объекта данных. </param>
        /// <param name="xmlValue"> Сериализованное представление в виде xml. </param>
        /// <returns>  Сконструированный объект данных. </returns>
        protected DataObject GetDataObjectFromXml(string typeOfObject, string assemblyOfObject, string xmlValue)
        {
            if (string.IsNullOrEmpty(xmlValue) || string.IsNullOrEmpty(typeOfObject) || string.IsNullOrEmpty(assemblyOfObject))
            {
                return null;
            }

            Assembly asm = Assembly.LoadFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("ObjectAssemblies", assemblyOfObject)));
            Type objectType = asm.GetType(typeOfObject);
            var deserializedObject = (DataObject)Activator.CreateInstance(objectType);
            var xmlDocument = new XmlDocument { InnerXml = xmlValue };
            Tools.ToolXML.XMLDocument2DataObject(ref deserializedObject, xmlDocument);
            return deserializedObject;
        }
    }
}
