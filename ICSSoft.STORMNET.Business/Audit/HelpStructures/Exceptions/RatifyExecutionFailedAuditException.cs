namespace ICSSoft.STORMNET.Business.Audit.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Исключение, сообщающее об ошибке, произошедшей при подтверждении записей аудита (когда сразу нескольким записям определяется другой статус).
    /// </summary>
    public sealed class RatifyExecutionFailedAuditException : ExecutionFailedAuditException
    {
        /// <summary>
        /// Список первичных ключей записей, в ходе изменения которых происходили ошибки.
        /// </summary>
        public List<Guid> FailedAuditIdList;

        /// <summary>
        /// Инициализация исключения с сообщением "Не удалось изменить статус записей аудита" и указанием записей, в ходе изменения которых происходили ошибки.
        /// </summary>
        /// <param name="failedAuditIdList"> Список первичных ключей записей, в ходе изменения которых происходили ошибки. </param>
        public RatifyExecutionFailedAuditException(List<Guid> failedAuditIdList)
            : base("Не удалось изменить статус записей аудита " + string.Join(", ", failedAuditIdList.Select(guid => guid.ToString()).ToArray()))
        {
            FailedAuditIdList = failedAuditIdList;
        }
    }
}
