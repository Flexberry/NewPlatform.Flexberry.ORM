namespace ICSSoft.STORMNET.Business.Audit.Exceptions
{
    /// <summary>
    /// Исключение, сообщающее, что требуемые аудитом данные не удалось получить.
    /// </summary>
    public sealed class DataNotFoundAuditException : AuditException
    {
        private const string AuditDataNotFoundErrorMessageFormat = "Не найдены данные для работы подсистемы аудита: {0}";

        /// <summary>
        /// Конструктор для инициализации сообщения исключениия.
        /// </summary>
        /// <param name="message">Тест исключения.</param>
        public DataNotFoundAuditException(string message)
            : base(string.Format(AuditDataNotFoundErrorMessageFormat, message))
        {
        }
    }
}
