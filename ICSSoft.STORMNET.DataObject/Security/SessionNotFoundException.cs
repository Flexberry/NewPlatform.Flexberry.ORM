namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// Исключение, возникающее в случае, если нет ключа сессии даже после запроса авторизоваться
    /// </summary>
    public class SessionNotFoundException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public SessionNotFoundException()
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public SessionNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Исключение о том, что сессия пользователя не была найдена
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="innerException">Внутреннее исключение</param>
        public SessionNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}