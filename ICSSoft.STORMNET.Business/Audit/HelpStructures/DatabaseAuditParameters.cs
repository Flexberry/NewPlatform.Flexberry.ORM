namespace ICSSoft.STORMNET.Business.Audit
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Базовый класс, содержащий настройки БД, куда будет писаться аудит.
    /// </summary>
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.AuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.RatificationAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.CommonAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.CheckedCustomAuditParameters))]
    public class DatabaseAuditParameters : AuditParametersBase
    {
        /// <summary>
        /// Имя строки подключения к БД аудита.
        /// </summary>
        [DataMember]
        public string AuditConnectionStringName = string.Empty;

        public DatabaseAuditParameters(
            string auditConnectionStringName)
        {
            AuditConnectionStringName = auditConnectionStringName;
        }
    }
}
