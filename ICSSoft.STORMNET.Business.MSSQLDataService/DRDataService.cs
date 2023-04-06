namespace ICSSoft.STORMNET.Business
{
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Interfaces;
    using ICSSoft.STORMNET.Security;

    /// <summary>
    /// Сервис данных для грязного чтения (MSSQLServer).
    /// </summary>
    public class DRDataService : ICSSoft.STORMNET.Business.MSSQLDataService
    {
        /// <summary>
        /// Создание сервиса данных для Microsoft SQL Server с указанием настроек проверки полномочий.
        /// </summary>
        /// <param name="securityManager">Менеджер полномочий.</param>
        /// <param name="auditService">Сервис аудита.</param>
        /// <param name="businessServerProvider">The provider for <see cref="BusinessServer"/> creation.</param>
        public DRDataService(ISecurityManager securityManager, IAuditService auditService, IBusinessServerProvider businessServerProvider)
            : base(securityManager, auditService, businessServerProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DRDataService"/> class with specified security manager, audit service and converter.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service instance.</param>
        /// <param name="businessServerProvider">The provider for <see cref="BusinessServer"/> creation.</param>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        /// <param name="notifierUpdateObjects">An instance of the class for custom process updated objects.</param>
        public DRDataService(ISecurityManager securityManager, IAuditService auditService, IBusinessServerProvider businessServerProvider, IConverterToQueryValueString converterToQueryValueString, INotifyUpdateObjects notifierUpdateObjects = null)
            : base(securityManager, auditService, businessServerProvider, converterToQueryValueString, notifierUpdateObjects)
        {
        }

        /// <inheritdoc />
        public override string GetJoinTableModifierExpression()
        {
            return " WITH (NOLOCK) ";
        }
    }
}
