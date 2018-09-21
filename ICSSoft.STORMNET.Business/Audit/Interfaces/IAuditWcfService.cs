namespace ICSSoft.STORMNET.Business.AuditWcfServiceLibrary
{
    using System;
    using System.ServiceModel;

    using ICSSoft.STORMNET.Business.Audit;

    /// <summary>
    /// Интерфейс для AuditWcfService
    /// (на настоящий момент представляет собой наследника IAudit)
    /// </summary>
    [ServiceContract]
    public interface IAuditWcfService : IAudit
    {
    }
}
