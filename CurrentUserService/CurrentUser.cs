namespace ICSSoft.Services
{
    using System.Web;

    /// <summary>
    /// Текущий пользователь приложения.
    /// </summary>
    public class CurrentUser : CurrentUserService.IUser
    {
        /// <summary>
        /// Текущий web-пользователь.
        /// </summary>
        private CurrentUserService.IUser _webHttpUser;

        /// <summary>
        /// Текущий windows-пользователь.
        /// </summary>
        private CurrentUserService.IUser _winUser;

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login
        {
            get { return GetUser().Login; }
            set { GetUser().Login = value; }
        }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string Domain
        {
            get { return GetUser().Domain; }
            set { GetUser().Domain = value; }
        }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FriendlyName
        {
            get { return GetUser().FriendlyName; }
            set { GetUser().FriendlyName = value; }
        }

        /// <summary>
        /// Получить пользователя в текущем окружении.
        /// </summary>
        /// <returns>
        /// Если текущее приложение исполняется в веб окружении,
        /// то вернет учетные данные о текущем web-пользователе, иначе о windows-пользователе.
        /// </returns>
        private CurrentUserService.IUser GetUser()
        {
            if (HttpContext.Current != null)
                return _webHttpUser ?? (_webHttpUser = new CurrentWebHttpUser());

            return _winUser ?? (_winUser = new CurrentWindowsUser());
        }
    }
}