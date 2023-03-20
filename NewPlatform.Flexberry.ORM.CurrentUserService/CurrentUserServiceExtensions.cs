﻿namespace ICSSoft.Services
{
    using System;

    /// <summary>
    /// Class with extension methods for <see cref="CurrentUserService"/>.
    /// </summary>
    public static class CurrentUserServiceExtensions
    {
        /// <summary>
        /// Returns user login in "DOMAIN\login" format (<see href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa380525(v=vs.85).aspx"/>).
        /// "DOMAIN" - NetBIOS domain name; "login" - user account name.
        /// </summary>
        /// <param name="user">User for building login.</param>
        /// <returns>User login in "DOMAIN\login" format.</returns>
        public static string GetDownLevelLogonName(this CurrentUserService.IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return string.Concat(user.Domain, "\\", user.Login);
        }
    }
}
