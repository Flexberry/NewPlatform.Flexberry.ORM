namespace ICSSoft.STORMNET.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security;

    using NewPlatform.Flexberry.Security;

    /// <summary>
    /// Интерфейс для менеджера полномочий. Является основным API для доступа к подсистеме полномочий со стороны программистов.
    /// </summary>
    /// <remarks>
    /// Текущий пользователь для всех связанных действий определяется через <see cref="CurrentUserService"/>.
    /// </remarks>
    public interface ISecurityManager
    {
        /// <summary>
        /// Флаг включенных полномочий.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Флаг включенных полномочий над объектами (а не для всего типа сразу).
        /// </summary>
        bool UseRightsOnObjects { get; }

        /// <summary>
        /// Флаг включенных полномочий над атрибутами.
        /// </summary>
        bool UseRightsOnAttribute { get; }

        /// <summary>
        /// Регулярное выражение для извлечения информации о контроле прав на атрибуты из <see cref="DataServiceExpressionAttribute"/>.
        /// </summary>
        string AttributeCheckExpressionPattern { get; }

        /// <summary>
        /// Проверка полномочий на выполнение операции.
        /// </summary>
        /// <param name="operationId">Идентификатор операции.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// Возвращает <c>true</c> без проверок если полномочия выключены в <see cref="Enabled"/>.
        /// </returns>
        bool AccessCheck(int operationId);

        /// <summary>
        /// Проверка полномочий на выполнение операции.
        /// </summary>
        /// <param name="operationId">Идентификатор операции.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// Возвращает <c>true</c> без проверок если полномочия выключены в <see cref="Enabled"/>.
        /// </returns>
        bool AccessCheck(string operationId);

        /// <summary>
        /// Проверка полномочий на выполнение операции с типом.
        /// </summary>
        /// <param name="type">Тип объекта данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение в случае отсутсвия прав.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// Возвращает <c>true</c> без проверок если полномочия выключены в <see cref="Enabled"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="type"/>.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Исключение генерируется в том случае, если у пользователя отсутствует доступ и параметр <paramref name="throwException"/> установлен в <c>true</c>.
        /// </exception>
        bool AccessObjectCheck(Type type, tTypeAccess operation, bool throwException);

        /// <summary>
        /// Проверка полномочий на выполнение операции с объектом.
        /// </summary>
        /// <param name="obj">Объект данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение в случае отсутсвия прав.</param>
        /// <returns>
        /// Если у текущего пользователя есть доступ, то <c>true</c>.
        /// Возвращает <c>true</c> без проверок если полномочия выключены в <see cref="Enabled"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="obj"/>.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Исключение генерируется в том случае, если у пользователя отсутствует доступ и параметр <paramref name="throwException"/> установлен в <c>true</c>.
        /// </exception>
        bool AccessObjectCheck(object obj, tTypeAccess operation, bool throwException);

        /// <summary>
        /// Получить ограничение для текущего пользователя.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="limit">Ограничение, которое есть для текущего пользователя.</param>
        /// <param name="canAccess">Есть ли доступ к этому типу у пользователя.</param>
        /// <returns>Результат выполнения операции.</returns>
        OperationResult GetLimitForAccess(Type type, tTypeAccess operation, out object limit, out bool canAccess);

        /// <summary>
        /// Получить роли с заданными ограничениями, которые реализуют функцию разграничения по объектам.
        /// </summary>
        /// <param name="type">Класс, для которого получаем ограничения.</param>
        /// <param name="rolesWithAccesses">Роли с заданными ограничениями для этих ролей.</param>
        /// <returns>Результат выполнения операции.</returns>
        OperationResult GetLimitStrForRoles(Type type, out List<RoleWithAccesses> rolesWithAccesses);

        /// <summary>
        /// Задать ограничение для указанной роли.
        /// </summary>
        /// <param name="type">Тип объектов данных, для которых будет применяться данный фильтр.</param>
        /// <param name="operation">Тип доступа, для которого применяется этот фильтр.</param>
        /// <param name="roleName">Название роли.</param>
        /// <param name="filter">Сериализованный фильтр, который будет применяться для указанной роли.</param>
        /// <returns>Результат выполнения операции.</returns>
        OperationResult SetLimitStrForRole(Type type, tTypeAccess operation, string roleName, string filter);

        /// <summary>
        /// Проверить наличие в системе логина (чувствительность к регистру зависит от настроек источника данных).
        /// Уникальность проверяется без контроля доменов, то есть гарантируется уникальность в рамках всей таблицы.
        /// Отключенные пользователи тоже учитываются, как занимающие логин.
        /// </summary>
        /// <param name="login">Логин, который проверяем.</param>
        /// <returns>Если логин свободен, то <see cref="OperationResult.ЛогинСвободен"/>, если занят, то <see cref="OperationResult.ЛогинЗанят"/>.</returns>
        /// <exception cref="ArgumentException">Исключение генерируется при передаче <c>null</c> или <c>string.Empty</c> в качестве значения для <paramref name="login"/>.</exception>
        OperationResult CheckExistLogin(string login);

        /// <summary>
        /// Метод проверки прав на доступ текущего пользователя к операции, заданной в <see cref="DataServiceExpressionAttribute"/> атрибута.
        /// </summary>
        /// <param name="expression">Строка <see cref="DataServiceExpressionAttribute"/>.</param>
        /// <param name="deniedAccessValue">Значение, которое должен получить атрибут при отсутствии прав.</param>
        /// <returns>Если у текущего пользователя есть доступ, то <c>true</c>.</returns>
        bool CheckAccessToAttribute(string expression, out string deniedAccessValue);

        /// <summary>
        /// Создать операцию.
        /// </summary>
        /// <param name="name">Имя операции.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если операция с таким <paramref name="name"/> уже существует.</exception>
        void CreateOperation(string name);

        /// <summary>
        /// Создать класс.
        /// </summary>
        /// <param name="name">Имя класса.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если класс с таким <paramref name="name"/> уже существует.</exception>
        void CreateClass(string name);

        /// <summary>
        /// Создать класс.
        /// </summary>
        /// <param name="type">Тип класса.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если класс с таким <paramref name="type"/> уже существует.</exception>
        void CreateClass(Type type);

        /// <summary>
        /// Удалить операцию.
        /// </summary>
        /// <param name="name">Имя операции.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найдена операци с таким <paramref name="name"/>, или их найдено больше одной.</exception>
        void RemoveOperation(string name);

        /// <summary>
        /// Удалить класс.
        /// </summary>
        /// <param name="name">Имя класса.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найден класс с таким <paramref name="name"/>, или их найдено больше одного.</exception>
        void RemoveClass(string name);

        /// <summary>
        /// Удалить класс.
        /// </summary>
        /// <param name="type">Тип класса.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найден класс с таким <paramref name="type"/>, или их найдено больше одного.</exception>
        void RemoveClass(Type type);

        /// <summary>
        /// Добавить полномочия на операцию.
        /// </summary>
        /// <param name="operationName">Имя операции.</param>
        /// <param name="typeAccess">Тип доступа.</param>
        /// <param name="agent">Агент, если не указан, берется из CurrentUserService.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найдена операци с таким <paramref name="operationName"/>, или их найдено больше одной.</exception>
        void AddPermissionToOperation(string operationName, tTypeAccess typeAccess, IAgent agent = null);

        /// <summary>
        /// Добавить полномочия на класс.
        /// </summary>
        /// <param name="calssName">Имя класса.</param>
        /// <param name="typeAccess">Тип доступа.</param>
        /// <param name="agent">Агент, если не указан, берется из CurrentUserService.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найден класс с таким <paramref name="calssName"/>, или их найдено больше одного.</exception>
        void AddPermissionToClass(string calssName, tTypeAccess typeAccess, IAgent agent = null);

        /// <summary>
        /// Добавить полномочия на класс.
        /// </summary>
        /// <param name="calssType">Тип класса.</param>
        /// <param name="typeAccess">Тип доступа.</param>
        /// <param name="agent">Агент, если не указан, берется из CurrentUserService.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найден класс с таким <paramref name="calssType"/>, или их найдено больше одного.</exception>
        void AddPermissionToClass(Type calssType, tTypeAccess typeAccess, IAgent agent = null);

        /// <summary>
        /// Удалить полномочия на операцию.
        /// </summary>
        /// <param name="operationName">Имя операции.</param>
        /// <param name="typeAccess">Тип доступа.</param>
        /// <param name="agent">Агент, если не указан, берется из CurrentUserService.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найдена операци с таким <paramref name="operationName"/>, или их найдено больше одной.</exception>
        void RemovePermissionFromOperation(string operationName, tTypeAccess typeAccess, IAgent agent = null);

        /// <summary>
        /// Удалить полномочия на класс.
        /// </summary>
        /// <param name="calssName">Имя класса.</param>
        /// <param name="typeAccess">Тип доступа.</param>
        /// <param name="agent">Агент, если не указан, берется из CurrentUserService.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найден класс с таким <paramref name="calssName"/>, или их найдено больше одного.</exception>
        void RemovePermissionFromClass(string calssName, tTypeAccess typeAccess, IAgent agent = null);

        /// <summary>
        /// Удалить полномочия на класс.
        /// </summary>
        /// <param name="calssType">Тип класса.</param>
        /// <param name="typeAccess">Тип доступа.</param>
        /// <param name="agent">Агент, если не указан, берется из CurrentUserService.</param>
        /// <exception cref="SecurityException">Выбрасывается, если у текущего пользователя нет операции для управления полномочиями.</exception>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не найден класс с таким <paramref name="calssType"/>, или их найдено больше одного.</exception>
        void RemovePermissionFromClass(Type calssType, tTypeAccess typeAccess, IAgent agent = null);
    }
}
