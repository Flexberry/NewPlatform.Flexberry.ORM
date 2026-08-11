namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Сервис аудита, который не ведёт аудит.
    /// Может использоваться в качестве заглушки.
    /// </summary>
    public class EmptyAuditService : IAuditService
    {
        private EmptyAudit emptyAudit = new EmptyAudit();

        private AuditAppSetting auditAppSetting = new AuditAppSetting() { AuditEnabled = false, WriteSessions = false };

        /// <inheritdoc/>
        public bool IsAuditEnabled => false;

        /// <inheritdoc/>
        public bool IsAuditRemote => false;

        /// <inheritdoc/>
        public string AuditConnectionStringName => null;

        /// <inheritdoc/>
        public bool ShowPrimaryKey { get => false; set { } }

        /// <inheritdoc/>
        public bool PersistUtcDates { get => false; set { } }

        /// <inheritdoc/>
        public AuditAppSetting AppSetting { get => auditAppSetting; set { } }

        /// <inheritdoc/>
        public IAudit Audit { get => emptyAudit; set { } }

        /// <inheritdoc/>
        public void AddCreateAuditInformation(DataObject operationedObject)
        {
        }

        /// <inheritdoc/>
        public void AddEditAuditInformation(DataObject operationedObject)
        {
        }

        /// <inheritdoc/>
        public void DisableAudit()
        {
        }

        /// <inheritdoc/>
        public bool EnableAudit(bool throwExceptions)
        {
            return false;
        }

        /// <inheritdoc/>
        public View GetAuditViewByType(Type type, tTypeOfAuditOperation operationType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public View GetViewByAuditRecord(IAuditRecord auditRecord)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsTypeAuditable(Type curType)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, bool throwExceptions)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, IDataService dataService, bool throwExceptions)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool RatifyAuditOperationWithAutoFields(tExecutionVariant executionVariant, List<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions)
        {
            return true;
        }

        /// <inheritdoc/>
        public Guid? WriteCommonAuditOperation(DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
        {
            return Guid.NewGuid();
        }

        /// <inheritdoc/>
        public void WriteCommonAuditOperationWithAutoFields(IEnumerable<DataObject> operationedObjects, ICollection<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
        {
        }

        /// <inheritdoc/>
        public AuditAdditionalInfo WriteCommonAuditOperationWithAutoFields(DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, bool throwExceptions)
        {
            return Guid.NewGuid();
        }

        /// <inheritdoc/>
        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, IDataService dataService, bool throwExceptions)
        {
            return Guid.NewGuid();
        }

        /// <inheritdoc/>
        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
        {
            return Guid.NewGuid();
        }
    }
}
