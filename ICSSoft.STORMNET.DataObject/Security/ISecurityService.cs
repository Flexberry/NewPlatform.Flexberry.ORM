namespace ICSSoft.STORMNET.Security
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс для сервиса полномочий.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Получить информацию о пользователе (ключ агента и имя в базе полномочий).
        /// </summary>
        /// <param name="login">Логин, по которому ищем.</param>
        /// <param name="agentKey">Ключ агента.</param>
        /// <param name="name">Дружественное имя, которое присутсвует в системе полномочий.</param>
        /// <returns>Успешность выполнения операции.</returns>
        OperationResult GetProfileInfo(string login, out Guid? agentKey, out string name);

        /// <summary>
        /// Проверить наличие в системе логина (чувствительность к регистру зависит от настроек источника данных). Уникальность проверяется без контроля доменов, то есть гарантируется уникальность в рамках всей таблицы. Отключенные пользователи тоже учитываются, как занимающие логин.
        /// </summary>
        /// <param name="login">Логин, который проверяем.</param>
        /// <returns>OperationResult.ЛогинСвободен, OperationResult.ЛогинЗанят, OperationResult.ОшибкаВыполненияОперации, OperationResult.ОшибочныеАргументы.</returns>
        OperationResult CheckExistLogin(string login);

        /// <summary>
        /// Проверить наличие в системе логина (чувствительность к регистру зависит от настроек источника данных). Уникальность проверяется без контроля доменов, то есть гарантируется уникальность в рамках всей таблицы. Отключенные пользователи тоже учитываются, как занимающие логин.
        /// </summary>
        /// <param name="userKey">Ключ пользователя.</param>
        /// <param name="oldPassword">Старый пароль.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <returns>OperationResult.ЛогинСвободен, OperationResult.ЛогинЗанят, OperationResult.ОшибкаВыполненияОперации, OperationResult.ОшибочныеАргументы.</returns>
        OperationResult ChangePassword(Guid userKey, string oldPassword, string newPassword);

        /// <summary>
        /// Проверить полномочия на доступ к классу (проверяется как сам класс, так и все его роли и группы) ВАЖНО: Проверка на актуальность пользователя не производится, т.к. считается, что заблокированный пользователь отсекается на этапе логирования. Нужно будет сделать отключение сессии пользователя при его отключении в БД.
        /// </summary>
        /// <param name="userKey">Ключ пользователя.</param>
        /// <param name="subjectName">Имя объекта.</param>
        /// <param name="typeAccess">Запрашиваемый тип доступа.</param>
        /// <returns>Наличие данных полномочий.</returns>
        OperationResult CheckAccessClass(Guid userKey, string subjectName, string typeAccess);
        
        /// <summary>
        /// Проверить операцию.
        /// </summary>
        /// <param name="userKey">Ключ пользователя.</param>
        /// <param name="operationName">Имя операции.</param>
        /// <returns>Наличие данных полномочий.</returns>
        OperationResult CheckAccessOperation(Guid userKey, string operationName);

        /// <summary>
        /// Получить все субъекты, на которые есть права у данного пользователя.
        /// </summary>
        /// <param name="userKey">Ключ пользователя.</param>
        /// <param name="subjects">Список доступных субъектов.</param>
        /// <returns>Успешность выполнения операции.</returns>
        OperationResult GetAllPermitions(Guid userKey, out List<string> subjects);

        /// <summary>
        /// Обновить информацию о пользователе. Если такого логина не было, то будет создан новый пользователь.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="name">Имя пользователя в человеческом варианте.</param>
        /// <param name="pwd">Пароль в чистом виде.</param>
        /// <param name="enabled">Активна ли учётная запись.</param>
        /// <returns>Успешность выполнения операции.</returns>
        UpdateResult UpdateUser(string login, string name, string pwd, bool enabled);

        /// <summary>
        /// Обновить информацию о профиле пользователя. Можно обновить имя пользователя, а также управлять его активностью.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="name">Имя пользователя в человеческом варианте.</param>
        /// <param name="enabled">Активна ли учётная запись.</param>
        /// <returns>Успешность выполнения операции.</returns>
        UpdateResult UpdateProfileInfo(string login, string name, bool? enabled);
    }
}