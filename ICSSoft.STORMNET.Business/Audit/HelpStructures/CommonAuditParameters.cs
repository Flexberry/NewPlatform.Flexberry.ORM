namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Класс для записи общих данных аудита
    /// (которые скорее всего были сгенерированы сервисом данных).
    /// </summary>
    [DataContract]
    public class CommonAuditParameters : AuditParameters
    {
        [DataMember]
        public tTypeOfAuditOperation TypeOfAuditOperation;

        [DataMember]
        public bool NeedAnswer;

        #region Работа с объектом, который может понадобиться отправить по wcf.

        /// <summary>
        /// Объект, над которым провели аудируемую операцию (если это обновление, то здесь хранится новая версия объекта).
        /// </summary>
        private DataObject _operationedObject = null;

        /// <summary>
        /// Объект, над которым провели аудируемую операцию (если это обновление, то здесь хранится новая версия объекта).
        /// </summary>
        public DataObject OperatedObject
        {
            get
            {
                if (_operationedObject == null && NeedSerialization && !string.IsNullOrEmpty(_serializedOperatedObject))
                { // Чтобы вычитать адекватно объект при десериализации (если взаимодействие не через wcf, то не нужно)
                    _operationedObject = GetDataObjectFromXml(_typeOfOperatedObject, _assemblyOfOperatedObject, _serializedOperatedObject);
                }

                return _operationedObject;
            }

            set
            {
                if (NeedSerialization)
                { // Подготовка к предстоящей сериализации (если взаимодействие не через wcf, то не нужно)
                    if (value == null)
                    {
                        _serializedOperatedObject = string.Empty;
                        _typeOfOperatedObject = string.Empty;
                        _assemblyOfOperatedObject = string.Empty;
                    }
                    else
                    {
                        _serializedOperatedObject = Tools.ToolXML.DataObject2XMLDocument(ref value).InnerXml;
                        _typeOfOperatedObject = value.GetType().FullName;
                        _assemblyOfOperatedObject = Path.GetFileName(value.GetType().Assembly.Location);
                    }
                }

                _operationedObject = value;
            }
        }

        /// <summary>
        /// Сериализованное представление объекта, над которым выполнили аудируемую операцию.
        /// </summary>
        [DataMember]
        private string _serializedOperatedObject = null;

        /// <summary>
        /// Тип объекта, над которым выполнили аудируемую операцию.
        /// </summary>
        [DataMember]
        private string _typeOfOperatedObject = string.Empty;

        /// <summary>
        /// Сборка, в которой можно найти тип объекта, над которым выполнили аудируемую операцию.
        /// </summary>
        [DataMember]
        private string _assemblyOfOperatedObject = string.Empty;

        #endregion Работа с объектом, который может понадобиться отправить по wcf.

        #region Работа со старой версией объекта, которую может понадобиться отправить по wcf.

        /// <summary>
        /// Сериализованное представление старой версии объекта, над которым выполнили аудируемую операцию.
        /// </summary>
        [DataMember]
        private string _serializedOldOperatedObject = null;

        /// <summary>
        /// Старая версия объекта, над которым провели аудируемую операцию.
        /// </summary>
        private DataObject _oldVersionOperatedObject = null;

        /// <summary>
        /// Старая версия объекта, над которым провели аудируемую операцию.
        /// </summary>
        public DataObject OldVersionOperatedObject
        {
            get
            {
                if (_oldVersionOperatedObject == null && NeedSerialization && !string.IsNullOrEmpty(_serializedOldOperatedObject))
                { // Чтобы вычитать адекватно объект при десериализации (если взаимодействие не через wcf, то не нужно)
                    _oldVersionOperatedObject =
                        GetDataObjectFromXml(_typeOfOperatedObject, _assemblyOfOperatedObject, _serializedOldOperatedObject);
                }

                return _oldVersionOperatedObject;
            }

            set
            {
                if (NeedSerialization)
                { // Подготовка к предстоящей сериализации (если взаимодействие не через wcf, то не нужно)
                    _serializedOldOperatedObject = value == null
                        ? string.Empty
                        : Tools.ToolXML.DataObject2XMLDocument(ref value).InnerXml;
                }

                _oldVersionOperatedObject = value;
            }
        }

        #endregion Работа со старой версией объекта, которую может понадобиться отправить по wcf.

        [DataMember]
        public string AuditView;

        [DataMember]
        public string OperationSource;

        [DataMember]
        public string FullUserLogin;

        [DataMember]
        public string UserName;

        [DataMember]
        public Nullable<Guid> AuditEntityGuid = null;

        [DataMember]
        public string[] LoadedProperties;

        public CommonAuditParameters(
            bool needAnswer,
            tExecutionVariant executionResult,
            string operationSource,
            string fullUserLogin,
            string userName,
            DateTime currentTime,
            AppMode applicationMode,
            string auditConnectionStringName,
            bool needSerialization)
            : base(executionResult, currentTime, applicationMode, auditConnectionStringName, needSerialization)
        {
            NeedAnswer = needAnswer;
            OperationSource = operationSource;
            FullUserLogin = fullUserLogin;
            UserName = userName;
        }

        public override Guid? GetAuditEntityGuid()
        {
            return AuditEntityGuid;
        }
    }
}
