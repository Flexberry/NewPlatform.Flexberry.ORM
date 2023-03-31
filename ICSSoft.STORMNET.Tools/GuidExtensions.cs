namespace ICSSoft.STORMNET.Tools
{
    using System;

    /// <summary>
    /// Класс содержащий вспомогательные методы для работы с <see cref="Guid"/>.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Получить строковое представление идентификатора в формате пригодном для работы в файловой системе как части пути.
        /// </summary>
        /// <param name="guid">Идентификатор для преобразования в строку.</param>
        /// <returns>Строковое представление идентификатора в формате пригодном для работы в файловой системе как части пути.</returns>
        public static string ToFriendlyString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}
