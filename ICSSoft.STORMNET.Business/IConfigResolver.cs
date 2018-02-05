namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Интерфейс, определяющий абстракцию для способа разрешения свойств классов на основе данных из файла конфигурации приложения.
    /// </summary>
    public interface IConfigResolver
    {
        /// <summary>
        /// Получить строку соединения по ее имени в секции <c>connectionStrings</c>.
        /// </summary>
        /// <param name="connStringName">Имя строки соединения в секции <c>connectionStrings</c>.</param>
        /// <returns>Строка соединения.</returns>
        string ResolveConnectionString(string connStringName);
    }
}
