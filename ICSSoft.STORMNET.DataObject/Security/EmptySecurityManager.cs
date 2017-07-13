namespace ICSSoft.STORMNET.Security
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Менеджер полномочий без проверки полномочий.
    /// </summary>
    public class EmptySecurityManager : ISecurityManager
    {
        /// <summary>
        /// Полномочия выключены, всегда возвращается <c>false</c>.
        /// </summary>
        public bool Enabled
        {
            get { return false; }
        }

        /// <summary>
        /// Режим проверки вплоть до объектов, а не для всего типа сразу, выключен - всегда возвращается <c>false</c>.
        /// </summary>
        public bool UseRightsOnObjects 
        {
            get { return false; }
        }

        /// <summary>
        /// Режим проверки атрибутов объектов выключен - всегда возвращается <c>false</c>.
        /// </summary>
        public bool UseRightsOnAttribute
        {
            get { return false; }
        }

        /// <summary>
        /// Регулярное выражение для извлечения информации о контроле прав на атрибуты из DataServiceExpression.
        /// </summary>
        public string AttributeCheckExpressionPattern
        {
            get { return @"/\*Operation:(?<Operation>.*);DeniedAccessValue:(?<DeniedAccessValue>.*)\*/"; }
        }

        /// <summary>
        /// Упрощённая проверка полномочий (на одну числовую операцию). Всегда возвращается <c>true</c>.
        /// </summary>
        /// <param name="operation">Идентификатор операции (такой, как указан для операции в AzMan или в имени операции).</param>
        /// <returns>Всегда возвращается <c>true</c>.</returns>
        public bool AccessCheck(int operation)
        {
            return true;
        }

        /// <summary>
        /// Упрощённая проверка полномочий (на одну строковую операцию). Всегда возвращается <c>true</c>.
        /// </summary>
        /// <param name="operation">Идентификатор операции (такой, как указан для операции в AzMan или в имени операции).</param>
        /// <returns>Всегда возвращается <c>true</c>.</returns>
        public bool AccessCheck(string operation)
        {
            return true;
        }

        /// <summary> 
        /// Проверка операций с объектом. Всегда возвращается <c>true</c>.
        /// </summary>
        /// <param name="type">Тип объекта данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение.</param>
        /// <returns>Всегда возвращается <c>true</c>.</returns>
        public bool AccessObjectCheck(Type type, tTypeAccess operation, bool throwException)
        {
            return true;
        }

        /// <summary> 
        /// Проверка операций с объектом. Всегда возвращается <c>true</c>.
        /// </summary>
        /// <param name="type">Объект данных.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="throwException">Генерировать ли исключение.</param>
        /// <returns>Всегда возвращается <c>true</c>.</returns>
        public bool AccessObjectCheck(object type, tTypeAccess operation, bool throwException)
        {
            return true;
        }

        /// <summary>
        /// Получить ограничение для текущего пользователя.
        /// </summary>
        /// <param name="subjectType">Тип объекта.</param>
        /// <param name="operation">Тип операции.</param>
        /// <param name="limit">Ограничение, которое есть для текущего пользователя. Всегда возвращается <c>null</c>.</param>
        /// <param name="canAccess">Есть ли доступ к этому типу у пользователя. Всегда возвращается <c>true</c>.</param>
        /// <returns>Всегда возвращается <see cref="OperationResult.Успешно"/>.</returns>
        public OperationResult GetLimitForAccess(Type subjectType, tTypeAccess operation, out object limit, out bool canAccess)
        {
            limit = null;
            canAccess = true;
            return OperationResult.Успешно;
        }

        /// <summary>
        /// Получить роли с заданными ограничениями, которые реализуют функцию разграничения по объектам. Всегда возвращается <c>null</c>.
        /// </summary>
        /// <param name="subjectType">Класс, для которого получаем ограничения.</param>
        /// <param name="rolesWithAccesses">Роли с заданными ограничениями для этих ролей.</param>
        /// <returns>Всегда возвращается <see cref="OperationResult.Успешно"/>.</returns>
        public OperationResult GetLimitStrForRoles(Type subjectType, out List<RoleWithAccesses> rolesWithAccesses)
        {
            rolesWithAccesses = null;
            return OperationResult.Успешно;
        }

        /// <summary>
        /// Задать ограничение для указанной роли.
        /// </summary>
        /// <param name="typeName">Тип объектов данных, для которых будет применяться данный фильтр.</param>
        /// <param name="operation">Тип доступа, для которого применяется этот фильтр.</param>
        /// <param name="roleName">Название роли.</param>
        /// <param name="filter">Сериализованный фильтр, который будет применяться для указанной роли.</param>
        /// <returns>Всегда возвращается <see cref="OperationResult.Успешно"/>.</returns>
        public OperationResult SetLimitStrForRole(Type typeName, tTypeAccess operation, string roleName, string filter)
        {
            return OperationResult.Успешно;
        }

        /// <summary>
        /// Проверить наличие в системе логина. Всегда возвращается <see cref="OperationResult.ЛогинСвободен"/>.
        /// </summary>
        /// <param name="login">Логин, который проверяем.</param>
        /// <returns>Всегда возвращается <see cref="OperationResult.ЛогинСвободен"/>.</returns>
        public OperationResult CheckExistLogin(string login)
        {
            return OperationResult.ЛогинСвободен;
        }

        /// <summary>
        /// Метод проверки прав на доступ текущего пользователя к операции, заданной в <see cref="DataServiceExpressionAttribute"/> атрибута.
        /// </summary>
        /// <param name="expression">Строка <see cref="DataServiceExpressionAttribute"/>.</param>
        /// <param name="deniedAccessValue">Значение, которое должен получить атрибут при отсутствии прав.</param>
        /// <returns><c>true</c> - права есть, <c>false</c> - прав нет.</returns>
        public bool CheckAccessToAttribute(string expression, out string deniedAccessValue)
        {
            deniedAccessValue = string.Empty;
            return true;
        }
    }
}