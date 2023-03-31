using System;
using System.Collections.Generic;

namespace ICSSoft.STORMNET.Security
{
    /// <summary>
    /// Класс для инкапсуляции имени роли с доступными этой роли правами доступа + ограничения.
    /// </summary>
    [Serializable]
    public class RoleWithAccesses
    {
        /// <summary>
        /// Имя роли.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Права доступа с описание ограничений.
        /// </summary>
        public Dictionary<tTypeAccess, string> Accesses { get; set; }

        /// <summary>
        /// Имя типа, для которого ищем ограничения.
        /// </summary>
        public string TypeName { get; set; }
    }
}
