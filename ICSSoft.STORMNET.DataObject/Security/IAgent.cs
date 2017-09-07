namespace NewPlatform.Flexberry.Security
{
    using System;

    /// <summary>
    /// Интерфейс для представления информации о пользователе, которая требуется для менеджера пользователей.
    /// </summary>
    public interface IAgent
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
        /// Имя агента (пользователя или роли).
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Является ли текущий агент пользователем.
        /// </summary>
        bool IsUser { get; set; }

        /// <summary>
        /// Является ли текущий агент группой.
        /// </summary>
        bool IsGroup { get; set; }

        /// <summary>
        /// Является ли текущий агент ролью.
        /// </summary>
        bool IsRole { get; set; }

        /// <summary>
        /// Адрес электронной почты пользователя.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Является ли текущий агент активным (включенным).
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Ключ агента.
        /// </summary>
        Guid? AgentKey { get; set; }
    }
}
