namespace NewPlatform.Flexberry.ORM.Validation.Exceptions
{
    using System;

    /// <summary>
    /// Исключения, генерируемые при работе <see cref="DataObjectValidator"/>.
    /// </summary>
    [Serializable]
    public abstract class ValidationException : Exception
    {
        /// <summary>
        /// Создание исключения.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        protected ValidationException(string message)
            : base(message)
        {
        }
    }
}
