namespace NewPlatform.Flexberry.ORM.Validation.Exceptions
{
    using ExternalLangDef.Properties;

    using System;

    /// <summary>
    /// Исключение, сообщающее, что в функции ограничения используется параметр неверного типа.
    /// </summary>
    [Serializable]
    public sealed class InvalidParameterTypeValidationException : ValidationException
    {
        /// <summary>
        /// Создание исключения, сообщающего, что в функции ограничения используется параметр неверного типа.
        /// </summary>
        /// <param name="functionName">Имя функции, где присутствует параметр некорректного типа.</param>
        /// <param name="expectedTypes">Тип, который должен был быть у параметра.</param>
        public InvalidParameterTypeValidationException(string functionName, string expectedTypes)
            : base(string.Format(ResourceData.InvalidParameterTypeValidationExceptionMessage, functionName, expectedTypes))
        {
        }
    }
}
