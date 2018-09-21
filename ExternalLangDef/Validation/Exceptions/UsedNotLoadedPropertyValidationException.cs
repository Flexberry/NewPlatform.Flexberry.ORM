namespace NewPlatform.Flexberry.ORM.Validation.Exceptions
{
    using System;
    using NewPlatform.Flexberry.ORM.ExternalLanguageDefinition.Properties;

    /// <summary>
    /// Исключение, сообщающее, что в функции ограничения используется незагруженное свойство.
    /// </summary>
    [Serializable]
    public sealed class UsedNotLoadedPropertyValidationException : ValidationException
    {
        /// <summary>
        /// Создание исключения, сообщающего, что в функции ограничения используется незагруженное свойство.
        /// </summary>
        /// <param name="propertyName">Имя незагруженного свойства.</param>
        public UsedNotLoadedPropertyValidationException(string propertyName) : base(string.Format(ResourceData.UsedNotLoadedPropertyValidationExceptionMessage, propertyName))
        {
        }
    }
}
