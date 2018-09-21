namespace ICSSoft.STORMNET.Business.Audit
{
    /// <summary>
    /// Константы для аудита.
    /// </summary>
    public class AuditConstants
    {
        /// <summary>
        /// Имя класса, который добавляется в DataObject.
        /// </summary>
        public const string AuditSettingsClassName = "AuditSettings";

        /// <summary>
        /// Имя по умолчанию для представления аудита.
        /// </summary>
        public const string DefaultAuditViewName = "AuditView";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string FieldValueDeletedConst = "-NULL-";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string FieldNamePrimaryKey = "LinkedPrimaryKey";

        /// <summary>
        /// Название настройки в конфиге, используемом классом, пишущим аудит,
        /// определяющей тип сервиса данных для записи данных аудита по умолчанию.
        /// </summary>
        public const string DefaultDsTypeConfigName = "DefaultDSType";

        /// <summary>
        /// Тип сервиса данных по умолчанию.
        /// </summary>
        public const string DefaultDataServiceType = "ICSSoft.STORMNET.Business.MSSQLDataService, ICSSoft.STORMNET.Business.MSSQLDataService";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string AuditEnabledFieldName = "AuditEnabled";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string AuditEnabledFieldSummary = "Включён ли аудит для класса";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string UseDefaultViewFieldName = "UseDefaultView";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string UseDefaultViewFieldSummary = "Использовать имя представления для аудита по умолчанию";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string SelectAuditFieldName = "SelectAudit";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string SelectAuditFieldSummary = "Включён ли аудит операции чтения";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string SelectAuditViewNameFieldName = "SelectAuditViewName";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string SelectAuditViewNameFieldSummary = "Имя представления для аудирования операции чтения";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string InsertAuditFieldName = "InsertAudit";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string InsertAuditFieldSummary = "Включён ли аудит операции создания";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string InsertAuditViewNameFieldName = "InsertAuditViewName";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string InsertAuditViewNameFieldSummary = "Имя представления для аудирования операции создания";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string UpdateAuditFieldName = "UpdateAudit";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string UpdateAuditFieldSummary = "Включён ли аудит операции изменения";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string UpdateAuditViewNameFieldName = "UpdateAuditViewName";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string UpdateAuditViewNameFieldSummary = "Имя представления для аудирования операции изменения";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string DeleteAuditFieldName = "DeleteAudit";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string DeleteAuditFieldSummary = "Включён ли аудит операции удаления";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string DeleteAuditViewNameFieldName = "DeleteAuditViewName";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string DeleteAuditViewNameFieldSummary = "Имя представления для аудирования операции удаления";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string FormUrlFieldName = "FormUrl";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string FormUrlFieldSummary = "Путь к форме просмотра результатов аудита";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string WriteModeFieldName = "WriteMode";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string WriteModeFieldSummary = "Режим записи данных аудита (синхронный или асинхронный)";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string PrunningLengthFieldName = "PrunningLength";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string PrunningLengthFieldSummary = "Максимальная длина сохраняемого значения поля (если 0, то строка обрезаться не будет)";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string ShowPrimaryKeyFieldName = "ShowPrimaryKey";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string ShowPrimaryKeyFieldSummary = "Показывать ли пользователям в изменениях первичные ключи";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string KeepOldValueFieldName = "KeepOldValue";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string KeepOldValueFieldSummary = "Сохранять ли старое значение";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string CompressFieldName = "Compress";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string CompressFieldSummary = "Сжимать ли сохраняемые значения";

        /// <summary>
        /// Имя соответствующего поля.
        /// </summary>
        public const string KeepAllValuesFieldName = "KeepAllValues";

        /// <summary>
        /// Комментарий к соответствующему полю.
        /// </summary>
        public const string KeepAllValuesFieldSummary = "Сохранять ли все значения атрибутов, а не только изменяемые";

        /// <summary>
        /// Имя поля, где хранится сервиса аудита, который будет писать аудит для конкретного класса.
        /// </summary>
        public const string AuditClassServiceFieldName = "AuditClassService";

        /// <summary>
        /// Имя поля, где хранится имя строки соединения с БД, куда необходимо писать аудит для конкретного класса.
        /// </summary>
        public const string AuditClassConnectionStringNameFieldName = "AuditClassConnectionStringName";
    }
}
