namespace ICSSoft.STORMNET.Business.Audit
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Базовый класс для установки в очередь сообщений при асинхронной записи аудита.
    /// </summary>
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.DatabaseAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.AuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.RatificationAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.CommonAuditParameters))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ICSSoft.STORMNET.Business.Audit.CheckedCustomAuditParameters))]
    public abstract class AuditParametersBase
    {
    }
}
