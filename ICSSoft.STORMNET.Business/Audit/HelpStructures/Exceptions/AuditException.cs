namespace ICSSoft.STORMNET.Business.Audit.Exceptions
{
    using System;

    /// <summary>
    /// Общий базовый класс для исключений подсистемы аудита.
    /// </summary>
    public class AuditException : Exception
    {
        /// <summary>
        /// Формирование сообщения об ошибке "При работе подсистемы аудита произошло исключение" с соответствующим внутренним исключением.
        /// </summary>
        /// <param name="innerException"> Внутреннее исключение. </param>
        public AuditException(Exception innerException)
            : base("При работе подсистемы аудита произошло исключение", innerException)
        {
        }

        /// <summary>
        /// Формирование сообщения об ошибке с указанным текстом.
        /// </summary>
        /// <param name="message"> Текст сообщения об ошибке. </param>
        public AuditException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///  Формирование сообщения об ошибке с указанным текстом с соответствующим внутренним исключением.
        /// </summary>
        /// <param name="message"> Текст сообщения об ошибке. </param>
        /// <param name="innerException"> Внутреннее исключение. </param>
        public AuditException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
