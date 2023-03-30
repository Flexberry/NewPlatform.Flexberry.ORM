namespace ICSSoft.Services
{
    using System;
    using System.DirectoryServices;

    /// <summary>
    /// Текущий пользователь в windows-приложении.
    /// </summary>
    public class CurrentWindowsUser : CurrentUserService.IUser
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        private string _login;

        /// <summary>
        /// Создать класс текущего пользователя и инициализировать данные из windows-окружения.
        /// </summary>
        public CurrentWindowsUser()
        {
            Login = Environment.UserName;
            Domain = Environment.UserDomainName;
        }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
                FriendlyName = GetUserFriendlyName(value) ?? value;
            }
        }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Получить полное имя пользователя по его логину в Windows.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Полное имя пользователя, либо <c>null</c>, если его не удалось получить.</returns>
        private static string GetUserFriendlyName(string login)
        {
            // полное имя пользователя
            const string UserCommonNameProperty = "cn";

            try
            {
                string filter = "(&(objectClass=user)(sAMAccountName= " + login + "))";
                var propertiesToLoad = new[] { UserCommonNameProperty };
                var ds = new DirectorySearcher(filter, propertiesToLoad) { CacheResults = true };

                var searchResult = ds.FindOne();
                if (searchResult == null)
                {
                    return null;
                }

                string friendlyName = searchResult.Properties[UserCommonNameProperty][0].ToString();
                return friendlyName;
            }
            catch (SystemException)
            {
                // Исправлено для совместимости с прежней версией получения имени текущего пользователя при хранении настроек.
                return Environment.UserName;
            }
        }
    }
}