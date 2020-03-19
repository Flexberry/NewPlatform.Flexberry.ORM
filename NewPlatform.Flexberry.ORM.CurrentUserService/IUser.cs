namespace ICSSoft.Services
{
    // todo: нужно вынести интерфейс IUser из нутров класса CurrentUserService.

    /// <summary>
    /// Сервис для получения текущего пользователя.
    /// </summary>
    public static partial class CurrentUserService
    {
        /// <summary>
        /// Учетные данные пользователя.
        /// </summary>
        public interface IUser
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
}
