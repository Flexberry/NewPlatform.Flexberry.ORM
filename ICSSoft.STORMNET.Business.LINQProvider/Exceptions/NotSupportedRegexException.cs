namespace ICSSoft.STORMNET.Business.LINQProvider.Exceptions
{
    using System;

    /// <summary>
    /// Исключение, сообщающее, что на настоящий момент данный шаблон regex не может быть преобразован в sql-like.
    /// </summary>
    public class NotSupportedRegexException : NotSupportedException
    {
        /// <summary>
        /// Формирование исключения.
        /// </summary>
        /// <param name="message"> Сообщение исключения (в начале сообщения будет добавлен текст "Представленное регулярное выражение не поддерживается. "). </param>
        public NotSupportedRegexException(string message)
            : base("Представленное регулярное выражение не поддерживается. " + message)
        {
        }
    }
}
