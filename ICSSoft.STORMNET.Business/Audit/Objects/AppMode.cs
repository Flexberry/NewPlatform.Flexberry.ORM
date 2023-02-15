namespace ICSSoft.STORMNET.Business.Audit
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Режим работы приложения.
    /// </summary>
    [DataContract]
    public enum AppMode
    {
        /// <summary>
        /// Неизвестно, как запущено приложение
        /// </summary>
        [EnumMemberAttribute]
        Unknown,

        /// <summary>
        /// То есть приложение запущено под win
        /// </summary>
        [EnumMemberAttribute]
        Win,

        /// <summary>
        /// То есть приложение запущено под web
        /// </summary>
        [EnumMemberAttribute]
        Web,
    }
}
