namespace ICSSoft.STORMNET
{
    /// <summary>
    /// Интерфейс конвертирования имени ошибки доступа. Применяется в <see cref="UnauthorizedAccessException.ErrorNameConverter"/>.
    /// </summary>
    public interface IAccessErrorNameConverter
    {
        /// <summary>
        /// Конвертирование имени ошибки доступа.
        /// </summary>
        /// <param name="name">Имя ошибки доступа.</param>
        /// <returns>Результат конвертирования имени.</returns>
        string Convert(string name);
    }
}