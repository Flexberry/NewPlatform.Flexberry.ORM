namespace ICSSoft.STORMNET.Business.Audit.Exceptions
{
    using System;

    /// <summary>
    /// Исключение, сообщающее, что запрашиваемое действие на настоящий момент не поддерживается аудитом
    /// </summary>
    public class ExecutionFailedAuditException : AuditException
    {
        private const string AuditExecutionFailedErrorMessage = "Не удалось записать данные в аудит";

        /// <summary>
        /// Конструктор для инициализации сообщения исключениия
        /// </summary>
        /// <param name="innerException"> Внутреннее исключение </param>
        public ExecutionFailedAuditException(Exception innerException) : base(AuditExecutionFailedErrorMessage, innerException)
        {
        }

        /// <summary>
        /// Конструктор для инициализации сообщения исключениия
        /// </summary>
        /// <param name="message"> Текст сообщения об ошибке </param>
        public ExecutionFailedAuditException(string message)
            : base(message)
        {
        }
    }
}