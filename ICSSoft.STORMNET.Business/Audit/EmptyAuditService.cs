using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSSoft.STORMNET.Business.Audit.HelpStructures;
using ICSSoft.STORMNET.Business.Audit.Objects;

namespace ICSSoft.STORMNET.Business.Audit
{
    public class EmptyAuditService : IAuditService
    {
        public bool IsAuditEnabled => false;

        public bool IsAuditRemote => false;

        public string AuditConnectionStringName => null;

        public bool ShowPrimaryKey { get => false; set => throw new NotImplementedException(); }
        public bool PersistUtcDates { get => false; set => throw new NotImplementedException(); }
        public AuditAppSetting AppSetting { get => null; set => throw new NotImplementedException(); }
        public IAudit Audit { get => null; set => throw new NotImplementedException(); }

        public void AddCreateAuditInformation(DataObject operationedObject)
        {
            throw new NotImplementedException();
        }

        public void AddEditAuditInformation(DataObject operationedObject)
        {
            throw new NotImplementedException();
        }

        public void DisableAudit()
        {
            throw new NotImplementedException();
        }

        public bool EnableAudit(bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public View GetAuditViewByType(Type type, tTypeOfAuditOperation operationType)
        {
            throw new NotImplementedException();
        }

        public View GetViewByAuditRecord(IAuditRecord auditRecord)
        {
            throw new NotImplementedException();
        }

        public bool IsTypeAuditable(Type curType)
        {
            return false;
        }

        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, IDataService dataService, bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public bool RatifyAuditOperationWithAutoFields(tExecutionVariant executionVariant, List<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public Guid? WriteCommonAuditOperation(DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public void WriteCommonAuditOperationWithAutoFields(IEnumerable<DataObject> operationedObjects, ICollection<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public AuditAdditionalInfo WriteCommonAuditOperationWithAutoFields(DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, IDataService dataService, bool throwExceptions)
        {
            throw new NotImplementedException();
        }

        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
        {
            throw new NotImplementedException();
        }
    }
}
