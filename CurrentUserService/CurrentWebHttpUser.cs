namespace ICSSoft.Services
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Текущий пользователь в web-приложении.
    /// </summary>
    public class CurrentWebHttpUser : CurrentUserService.IUser
    {
        /// <summary>
        /// Логин пользователя, заданный вручную.
        /// </summary>
        private string _login;

        /// <summary>
        /// Домен пользователя, заданный вручную.
        /// </summary>
        private string _domain;

        /// <summary>
        /// Имя пользователя, заданное вручную.
        /// </summary>
        private string _friendlyName;

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login
        {
            get { return _login ?? GetLogin(); }
            set { _login = value; }
        }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string Domain
        {
            get { return _domain ?? GetDomain(); }
            set { _domain = value; }
        }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FriendlyName
        {
            get { return _friendlyName ?? Login; }
            set { _friendlyName = value; }
        }

        /// <summary>
        /// Получить логин пользователя в текущем веб окружении.
        /// </summary>
        /// <returns>Логин пользователя, либо <c>null</c>, если пользователь не авторизован.</returns>
        private static string GetLogin()
        {
            string s = GetIdentityName();
            if (s == null)
            {
                return null;
            }

            int index = s.IndexOf("\\", StringComparison.InvariantCulture);
            return (index > -1) ? s.Substring(index + 1, s.Length - index - 1) : s;
        }

        /// <summary>
        /// Получить домен пользователя в текущем веб окружении.
        /// </summary>
        /// <returns>
        /// Домен пользователя, либо <c>null</c>, если пользователь не авторизован.
        /// Если домен в учетных данных не указан, то метод возвратит пустую строку.
        /// </returns>
        private static string GetDomain()
        {
            string s = GetIdentityName();
            if (s == null)
            {
                return null;
            }

            int index = s.IndexOf("\\", StringComparison.InvariantCulture);
            return (index > -1) ? s.Substring(0, index) : string.Empty;
        }

        /// <summary>
        /// Получить имя текущего пользователя (домен + логин) из текущего HTTP контекста.
        /// </summary>
        /// <returns>
        /// Имя текущего пользователя, включающее домен и логин, разделенные обратным слэшем.
        /// <para/>Если домен не задан, то метод вернет только логин.
        /// <para/>Если пользователь не авторизован, метод вернет <c>null</c>.
        /// </returns>
        private static string GetIdentityName()
        {
            //var context = HttpContext.Current;
            //if (context == null)
            //{
            //    throw new WebException("Отсутствует текущий веб контекст.");
            //}

            //var user = context.User;
            //if (user == null)
            //{
            //    throw new HttpException("В текущем HTTP запросе отсутствует информация о пользователе.");
            //}

            //var identity = user.Identity;
            //if (!identity.IsAuthenticated)
            //{
                return null;
            //}

            //return identity.Name;
        }
    }
}