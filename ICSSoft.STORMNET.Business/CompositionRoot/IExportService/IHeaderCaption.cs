namespace NewPlatform.Flexberry
{
    /// <summary>
    /// Интерфейс для заголовка столбца.
    /// </summary>
    public interface IHeaderCaption
    {
        /// <summary>
        /// Название свойства.
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Название столбца, если требуется чтоб заголовок отличался от названия свойства.
        /// </summary>
        string Caption { get; set; }

        /// <summary>
        /// Имя мастера.
        /// </summary>
        string MasterName { get; set; }

        /// <summary>
        /// Имя детейла.
        /// </summary>
        string DetailName { get; set; }
    }
}
