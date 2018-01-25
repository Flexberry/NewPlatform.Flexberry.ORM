namespace ICSSoft.STORMNET.Business.Audit
{
    using System;

    /// <summary>
    /// Класс-заглушка для сервиса аудита, через который никакой аудит не пишется.
    /// </summary>
    public class EmptyAudit : IAudit
    {
        /// <summary>
        /// Метод ничего не делает, возвращается случайный идентификатор.
        /// </summary>
        /// <param name="commonAuditParameters"> Объект, содержащий данные для аудита. </param>
        /// <returns> Случайный идентификатор. </returns>
        public Guid WriteCommonAuditOperation(CommonAuditParameters commonAuditParameters)
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Метод ничего не делает.
        /// </summary>
        /// <param name="ratificationAuditParameters"> Объект, содержащий данные для аудита. </param>
        public void RatifyAuditOperation(RatificationAuditParameters ratificationAuditParameters)
        {
        }

        /// <summary>
        /// Метод ничего не делает, возвращается случайный идентификатор.
        /// </summary>
        /// <param name="checkedCustomAuditParameters"> Объект, содержащий данные для аудита. </param>
        /// <returns> Случайный идентификатор. </returns>
        public Guid WriteCustomAuditOperation(CheckedCustomAuditParameters checkedCustomAuditParameters)
        {
            return Guid.NewGuid();
        }
    }
}
