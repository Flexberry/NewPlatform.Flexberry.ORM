namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Возвращаемое зачение LCS, пока типы прописаны явно, чтобы не было путаницы. В дальнейшем могут быть изменены на int вместо Count, bool вместо Any и т.д.
    /// </summary>
    public enum LcsReturnType
    {
        /// <summary>
        /// Несколько объектов
        /// </summary>
        Objects,

        /// <summary>
        /// Один объект
        /// </summary>
        Object,

        /// <summary>
        /// Один объект обязательно
        /// </summary>
        ObjectRequired,

        /// <summary>
        /// Количество
        /// </summary>
        Count,

        /// <summary>
        /// Логический
        /// </summary>
        Any,

        /// <summary>
        /// Логический
        /// </summary>
        All
    }
}