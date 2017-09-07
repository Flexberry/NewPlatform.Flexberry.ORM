namespace ICSSoft.STORMNET.Business.AuditWcfServiceLibrary
{
    using System;

#if DNX4
    using System.ServiceModel;
#endif
    using ICSSoft.STORMNET.Business.Audit;

    /// <summary>
    /// Интерфейс для AuditWcfService
    /// (на настоящий момент представляет собой наследника IAudit)
    /// </summary>
#if DNX4
    [ServiceContract]
#endif
    public interface IAuditWcfService : IAudit
    { 
    }
}
