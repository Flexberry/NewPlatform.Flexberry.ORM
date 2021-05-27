using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ICSSoft.STORMNET.Tools
{
    /// <summary>
    /// Класс предназначен для работы с System.DirectoryServices.
    /// К нему относится все, что касается системных групп, пользователей и доменов.
    /// </summary>
    public static class DirectoryServicesTools
    {
        /// <summary>
        /// Получить имя пользователя по логину на локальном комьютере.
        /// </summary>
        /// <param name="userLogin">Логин для поиска пользователя.</param>
        /// <returns>Имя найденного пользователя. Будет пустым, если не найдется.</returns>
        public static string GetUserFullNameFromLocalMachine(string userLogin)
        {
            var root = new DirectoryEntry(string.Format("WinNT://{0},computer", Environment.MachineName));
            try
            {
                root = root.Children.Find(userLogin, "user");
                return root.Properties["FullName"].Value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Получить имя пользователя по логину в указанной Active Directory.
        /// </summary>
        /// <param name="activeDirectoryName">Active Directory в которой будем искать.</param>
        /// <param name="userLogin">Логин для поиска пользователя.</param>
        /// <returns>Имя найденного пользователя. Будет пустым, если не найдется.</returns>
        public static string GetUserFullNameFromActiveDirectory(string activeDirectoryName, string userLogin)
        {
            try
            {
                var objContext = new DirectoryContext(DirectoryContextType.Domain, activeDirectoryName);
                Domain objDomain = Domain.GetDomain(objContext);
                DirectoryEntry directoryEntry = objDomain.GetDirectoryEntry();

                var directorySearcher = new DirectorySearcher(directoryEntry)
                {
                    Filter = string.Format("(SAMAccountName={0})", userLogin),
                };
                var resultCollection = directorySearcher.FindAll();
                if (resultCollection.Count > 0)
                {
                    return resultCollection[0].Properties["Name"][0].ToString();
                }

                return string.Empty;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                // Если не нашли такую ActiveDirectory,
                // то возможно мы в другой сети или она не верно задана.
                return string.Empty;
            }
        }
    }
}
