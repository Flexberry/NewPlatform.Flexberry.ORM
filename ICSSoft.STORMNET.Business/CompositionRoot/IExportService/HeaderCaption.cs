namespace NewPlatform.Flexberry
{
    /// <summary>
    /// Заголовок столбца.
    /// </summary>
    public class HeaderCaption : IHeaderCaption
    {
        /// <summary>
        /// Название свойства.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Название столбца, если требуется чтоб заголовок отличался от названия свойства.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Имя мастера.
        /// </summary>
        public string MasterName { get; set; }

        /// <summary>
        /// Имя детейла.
        /// </summary>
        public string DetailName { get; set; }
    }
}
