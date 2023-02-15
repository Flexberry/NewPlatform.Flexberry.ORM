namespace ICSSoft.STORMNET.Business.Audit.Exceptions
{
    /// <summary>
    /// Исключение, сообщающее, что аудит выключен, хотя пытается произвестись запись.
    /// </summary>
    public sealed class DisabledAuditException : AuditException
    {
        private const string AuditDisableErrorMessage = "Аудит в приложении отключён";

        /// <summary>
        /// Конструктор для инициализации сообщения исключениия.
        /// </summary>
        public DisabledAuditException()
            : base(AuditDisableErrorMessage)
        {
        }
    }
}
