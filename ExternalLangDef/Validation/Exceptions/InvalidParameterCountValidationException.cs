namespace NewPlatform.Flexberry.ORM.Validation.Exceptions
{
    using System;
    using NewPlatform.Flexberry.ORM.ExternalLanguageDefinition.Properties;

    /// <summary>
    /// Исключение, сообщающее, что в функции ограничения некорректное количество параметров.
    /// </summary>
    [Serializable]
    public sealed class InvalidParameterCountValidationException : ValidationException
    {
        /// <summary>
        /// Создание исключения, сообщающего, что в функции ограничения некорректное количество параметров.
        /// </summary>
        /// <param name="functionName">Имя функции, для которой задано некорректное количество параметров.</param>
        public InvalidParameterCountValidationException(string functionName) : base(string.Format(ResourceData.InvalidParameterCountValidationExceptionMessage, functionName))
        {
        }
    }
}
