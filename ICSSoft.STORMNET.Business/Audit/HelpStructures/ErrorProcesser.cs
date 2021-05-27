namespace ICSSoft.STORMNET.Business.Audit.HelpStructures
{
    using System;

    using ICSSoft.STORMNET.Business.Audit.Exceptions;

    /// <summary>
    /// Класс для обработки возникших исключений.
    /// </summary>
    public static class ErrorProcesser
    {
        /// <summary>
        /// Сообщание для передачи из wcf-сервиса.
        /// </summary>
        private const string WinServiceErrorMessageStart = "При работе сервиса записи аудита возникло исключение. ";

        /// <summary>
        /// Обработка исключения c клиентской стороны.
        /// </summary>
        /// <param name="exception"> Исключение, которое будет записано в лог + проброшено, если задана настройка. </param>
        /// <param name="errorHeader"> Заголовок для ошибки. </param>
        /// <param name="throwExceptions"> Следует ли пробрасывать исключение далее. </param>
        public static void ProcessAuditError(Exception exception, string errorHeader, bool throwExceptions)
        {
            LogService.LogError(errorHeader, exception);
            if (throwExceptions)
            {
                throw exception is AuditException
                    ? exception
                    : new AuditException(exception);
            }
        }

        /// <summary>
        /// Обработка ошибок, произошедших при работе wcf-сервиса (напрямую данная технология передавать не позволяет).
        /// </summary>
        /// <param name="exception">Исключение, информация о котором будет записана в лог + проброшена далее так, как требует wcf.</param>
        public static void ProcessWinServiceError(Exception exception)
        {
            LogService.LogError(WinServiceErrorMessageStart, exception);
            throw new Exception(string.Format( // На стороне клиента будет вызвано FaultException
                                    "{2}\"{1}\" (подробная информация записана в лог).{0}",
                                    Environment.NewLine,
                                    exception.Message,
                                    WinServiceErrorMessageStart));
        }
    }
}
