namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.KeyGen;

    /// <summary>
    /// Параметры аудита, используемые для записи в БД аудита
    /// сведений о произвольных операциях над произвольными объектами.
    /// </summary>
    [DataContract]
    public class CheckedCustomAuditParameters : AuditParameters
    {
        [DataMember]
        public string CustomOperation { get; set; }

        #region Блок для работы и передачи по wcf первичного ключа объекта.

        /// <summary>
        /// Первичный ключ объекта (может быть как типа GUID, так и int), о котором нужно внести запись в аудит.
        /// </summary>
        public object AuditObjectPrimaryKey
        {
            get
            {
                if (_auditObjectPrimaryKey == null && NeedSerialization && !string.IsNullOrEmpty(_auditObjectPrimaryKeyAsString))
                { // Чтобы вычитать адекватно объект при десериализации (если взаимодействие не через wcf, то не нужно)
                    Type identificatorType;
                    if (string.IsNullOrEmpty(_typeOfAuditObjectPrimaryKey) 
                        || (identificatorType = Type.GetType(_typeOfAuditObjectPrimaryKey)) == null)
                    {
                        throw new ArgumentNullException(
                            string.Format("Не удалось получить тип по имени \"{0}\"", _typeOfAuditObjectPrimaryKey));
                    }

                    if (identificatorType == typeof(KeyGuid))
                    {
                        _auditObjectPrimaryKey = new KeyGuid(_auditObjectPrimaryKeyAsString);
                    }
                    else
                    {
                        TypeConverter converterToObject = TypeDescriptor.GetConverter(identificatorType);
                        _auditObjectPrimaryKey = converterToObject.ConvertFromInvariantString(_auditObjectPrimaryKeyAsString);
                    }
                }

                return _auditObjectPrimaryKey;
            }

            set
            {
                if (NeedSerialization)
                { // Подготовка к предстоящей сериализации (если взаимодействие не через wcf, то не нужно)
                    if (value == null)
                    {
                        _auditObjectPrimaryKeyAsString = string.Empty;
                        _typeOfAuditObjectPrimaryKey = string.Empty;
                    }
                    else
                    {
                        _auditObjectPrimaryKeyAsString = value.ToString();
                        _typeOfAuditObjectPrimaryKey = value.GetType().AssemblyQualifiedName;
                    }
                }

                _auditObjectPrimaryKey = value;
            }
        }

        /// <summary>
        /// Первичный ключ объекта (может быть как типа GUID, так и int), о котором нужно внести запись в аудит.
        /// </summary>
        private object _auditObjectPrimaryKey;

        /// <summary>
        /// Тип идентификатора объекта.
        /// </summary>
        [DataMember]
        private string _typeOfAuditObjectPrimaryKey = string.Empty;

        /// <summary>
        /// Строковое представление идентификатора объекта.
        /// </summary>
        [DataMember]
        private string _auditObjectPrimaryKeyAsString = string.Empty;

        #endregion Блок для работы и передачи по wcf первичного ключа объекта.

        [DataMember]
        public string AuditObjectTypeOrDescription { get; set; }

        [DataMember]
        public CustomAuditField[] CustomAuditFieldList { get; set; }

        [DataMember]
        public string OperationSource { get; set; }

        [DataMember]
        public string FullUserLogin { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public Nullable<Guid> AuditEntityGuid = null;

        public CheckedCustomAuditParameters(
            tExecutionVariant executionResult,
            DateTime currentTime,
            AppMode applicationMode,
            string auditConnectionStringName,
            bool needSerialization)
            : base(executionResult, currentTime, applicationMode, auditConnectionStringName, needSerialization)
        {
        }

        public override Guid? GetAuditEntityGuid()
        {
            return AuditEntityGuid;
        }
    }
}
