namespace ICSSoft.STORMNET.Business.Audit
{
    using System;

    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Параметры для ведения аудита через API.
    /// </summary>
    public class CustomAuditParameters
    { // TODO: для удобства можно выделить общие конструкторы
        #region Определение операции

        private string сustomOperation;

        /// <summary>
        /// Тип операции
        /// (выбирается из списка, при инициализации означивает CustomOperation).
        /// </summary>
        public tTypeOfAuditOperation CommonAuditOperation
        {
            set
            {
                CustomAuditOperation = EnumCaption.GetCaptionFor(value);
            }
        }

        /// <summary>
        /// Тип операции (задаётся любой, может определяться при инициализации CommonAuditOperation).
        /// </summary>
        public string CustomAuditOperation
        {
            get
            {
                return !string.IsNullOrEmpty(сustomOperation)
                           ? сustomOperation
                           : "CustomOperation";
            }

            set
            {
                сustomOperation = string.IsNullOrEmpty(value) ? value : value.Trim();
            }
        }

        #endregion Определение операции

        #region Определение объекта

        /// <summary>
        /// Объект, аудит которого проводится
        /// (сам объект не сохраняется, вычленяется только его первичный ключ и AssemblyQualifiedName типа).
        /// </summary>
        public DataObject AuditDataObject
        {
            set
            {
                AuditObjectPrimaryKey = value.__PrimaryKey;
                AuditObjectType = value.GetType();
            }
        }

        /// <summary>
        /// Тип объекта, аудит которого проводится
        /// (сам тип не сохраняется, вычленяется только его AssemblyQualifiedName).
        /// </summary>
        public Type AuditObjectType
        {
            set
            {
                AuditObjectTypeOrDescription = value.AssemblyQualifiedName;
            }
        }

        private object auditObjectPrimaryKey;

        /// <summary>
        /// Первичный ключ объекта, аудит которого проводится
        /// (задание AuditDataObject автоматически означит это поле).
        /// </summary>
        public object AuditObjectPrimaryKey
        {
            get
            {
                return auditObjectPrimaryKey ?? Guid.Empty;
            }

            set
            {
                auditObjectPrimaryKey = value;
            }
        }

        private string auditObjectTypeOrDescription;

        /// <summary>
        /// Тип (AssemblyQualifiedName) или описание объекта, аудит которого проводится
        /// (задание AuditDataObject или AuditObjectType автоматически означит это поле).
        /// </summary>
        public string AuditObjectTypeOrDescription
        {
            get
            {
                return !string.IsNullOrEmpty(auditObjectTypeOrDescription)
                           ? auditObjectTypeOrDescription
                           : "CustomUserType";
            }

            set
            {
                auditObjectTypeOrDescription = string.IsNullOrEmpty(value) ? value : value.Trim();
            }
        }

        #endregion Определение объекта

        #region Определение параметров аудита

        /// <summary>
        /// Время выполнения операции.
        /// </summary>
        private DateTime? _operationTime = null;

        /// <summary>
        /// Время выполнения операции
        /// (если время не задано, то будет возвращаться DateTime.Now, а при асинхронной записи это может быть критично).
        /// </summary>
        public DateTime OperationTime
        {
            get
            {
                return _operationTime ?? DateTime.Now;
            }

            set
            {
                _operationTime = value;
            }
        }

        /// <summary>
        /// Вариант выполнения операции
        /// (см. варианты в типе ICSSoft.STORMNET.Business.Audit.Objects.tExecutionVariant).
        /// </summary>
        private tExecutionVariant _executionResult = tExecutionVariant.Unknown;

        /// <summary>
        /// Вариант выполнения операции
        /// (см. варианты в типе ICSSoft.STORMNET.Business.Audit.Objects.tExecutionVariant).
        /// </summary>
        public tExecutionVariant ExecutionResult
        {
            get
            {
                return _executionResult;
            }

            set
            {
                _executionResult = value;
            }
        }

        private CustomAuditFieldList customAuditFieldList;

        /// <summary>
        /// Список с изменениями объекта, которые необходимо записать в подсистему аудита.
        /// </summary>
        public CustomAuditFieldList CustomAuditFieldList
        {
            get
            {
                return customAuditFieldList ?? (customAuditFieldList = new CustomAuditFieldList());
            }

            set
            {
                customAuditFieldList = value;
            }
        }

        /// <summary>
        /// Режим записи данных аудита: синхронный или асинхронный.
        /// </summary>
        private tWriteMode _writeMode = tWriteMode.Synchronous;

        private bool _useDefaultWriteMode = true;

        /// <summary>
        ///
        /// </summary>
        public bool UseDefaultWriteMode
        {
            get
            {
                return _useDefaultWriteMode;
            }

            set
            {
                _useDefaultWriteMode = value;
            }
        }

        /// <summary>
        /// Режим записи данных аудита: синхронный или асинхронный
        /// (если UseDefaultWriteMode = true, то этот параметр не учитывается).
        /// </summary>
        public tWriteMode WriteMode
        {
            get
            {
                return _writeMode;
            }

            set
            {
                _writeMode = value;
            }
        }

        #endregion Определение параметров аудита
    }
}
