namespace NewPlatform.Flexberry.ORM.CurrentUserService
{
    /// <summary>
    /// Учетные данные пользователя.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Логин пользователя ("vpupkin").
        /// </summary>
        string Login { get; set; }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// Имя пользователя ("VASYA Pupkin").
        /// </summary>
        string FriendlyName { get; set; }
    }
}
