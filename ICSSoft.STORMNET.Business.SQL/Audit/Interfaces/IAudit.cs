namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
#if DNX4
    using System.ServiceModel;
#endif
/// <summary>
/// Интерфейс для аудита (основная логика).
/// </summary>
#if DNX4
    [ServiceContract]
#endif
    public interface IAudit
    {
        /// <summary>
        /// Создание записи аудита об операции удаления, создания и изменения объекта.
        /// </summary>
        /// <param name="commonAuditParameters">Объект, содержащий данные для аудита.</param>
        /// <returns>Id сформированной записи аудита.</returns>
#if DNX4
        [OperationContract]
#endif
        Guid WriteCommonAuditOperation(CommonAuditParameters commonAuditParameters);

        /// <summary>
        /// Подтверждение созданных ранее операций аудита.
        /// </summary>
        /// <param name="ratificationAuditParameters">Параметры для утверждения внесённых ранее записей аудита.</param>
#if DNX4
        [OperationContract]
#endif
        void RatifyAuditOperation(RatificationAuditParameters ratificationAuditParameters);

        /// <summary>
        /// Создание записи аудита о собственной операции или действии над собственным объектом.
        /// </summary>
        /// <param name="checkedCustomAuditParameters">Параметры аудита.</param>
        /// <returns>Id сформированной записи аудита.</returns>
#if DNX4
        [OperationContract]
#endif
        Guid WriteCustomAuditOperation(CheckedCustomAuditParameters checkedCustomAuditParameters);
    }
}
