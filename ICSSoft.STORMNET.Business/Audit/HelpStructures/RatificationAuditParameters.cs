namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Вспомогательный класс для установки в очередь сообщений при асинхронной записи аудита о подтверждении записи аудита.
    /// </summary>
    [DataContract]
    public class RatificationAuditParameters : AuditParameters
    {
        /// <summary>
        /// Информация, которую необходимо дописать в аудит (с указанием идентификаторов записей аудита).
        /// </summary>
        [DataMember]
        public List<AuditAdditionalInfo> AuditOperationInfoList;

        /// <summary>
        /// Конструктор класса, который позволяет дописывать определённую информацию в аудит.
        /// </summary>
        /// <param name="executionVariant">Вариант завершения работы аудита.</param>
        /// <param name="currentTime">Текущее время.</param>
        /// <param name="auditOperationInfoList">Информация, которую необходимо дописать в аудит (с указанием идентификаторов записей аудита).</param>
        /// <param name="writeMode">Режим записи: синхронный или асинхронный.</param>
        /// <param name="applicationMode">Режим работы аудита: под win или web.</param>
        /// <param name="auditConnectionStringName">Имя строки соединения с БД, куда идут записи аудита.</param>
        /// <param name="needSerialization">Нужна ли сериализация (на случай передачи по wcf).</param>
        public RatificationAuditParameters(
            tExecutionVariant executionVariant,
            DateTime currentTime,
            List<AuditAdditionalInfo> auditOperationInfoList,
            tWriteMode writeMode,
            AppMode applicationMode,
            string auditConnectionStringName,
            bool needSerialization)
            : base(executionVariant, currentTime, applicationMode, auditConnectionStringName, needSerialization)
        {
            ExecutionResult = executionVariant;
            CurrentTime = currentTime;
            AuditOperationInfoList = auditOperationInfoList;
            WriteMode = writeMode;
        }

        /// <summary>
        /// Создаём копию структуры, за исключением <see cref="AuditOperationInfoList"/>, он не копируется.
        /// </summary>
        /// <returns>Сформированная копия.</returns>
        public RatificationAuditParameters CloneWithoutAuditOperationInfoList()
        {
            return new RatificationAuditParameters(
                        ExecutionResult,
                        CurrentTime,
                        null,
                        WriteMode,
                        ApplicationMode,
                        AuditConnectionStringName,
                        NeedSerialization)
            {
                ThrowExceptions = ThrowExceptions,
            };
        }
    }
}
