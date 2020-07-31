namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Интерфейс для сервиса аудита (отвечает за API и настройки).
    /// </summary>
    public interface IAuditService
    {
        #region Основные свойства

        /// <summary>
        /// Включён ли аудит для приложения.
        /// </summary>
        bool IsAuditEnabled { get; }

        /// <summary>
        /// Выполняется ли аудит удалённо (то есть через вызов AuditWinService).
        /// </summary>
        bool IsAuditRemote { get; }

        /// <summary>
        /// Является ли класс аудируемым (проверяются настройки по аудиту приложения + настройки класса).
        /// </summary>
        /// <param name="curType"> Исследуемый тип. </param>
        /// <returns> True, если является и нужно вести аудит. </returns>
        bool IsTypeAuditable(Type curType);

        /// <summary>
        /// Получение имени строки соединения с БД аудита
        /// (используется для загрузки данных из БД).
        /// Если используется запись аудита через windows-сервис, то будет возвращено null.
        /// </summary>
        string AuditConnectionStringName { get; }

        /// <summary>
        /// Следует ли отображать записи с изменением первичного ключа на формах.
        /// </summary>
        bool ShowPrimaryKey { get; set; }

        #endregion Основные свойства

        #region Разное

        /// <summary>
        /// Включить ведение аудита в приложении
        /// (предварительно должны быть проинициализированы AppSetting и Audit).
        /// </summary>
        /// <param name="throwExceptions"> Следует ли вызывать исключения в ошибочной ситуации. </param>
        /// <returns> Удалось ли включить аудит. </returns>
        bool EnableAudit(bool throwExceptions);

        /// <summary>
        /// Отключить ведение аудита в приложении.
        /// </summary>
        void DisableAudit();

        /// <summary>
        /// Настройки аудита в приложении.
        /// </summary>
        AuditAppSetting AppSetting { get; set; }

        /// <summary>
        /// Элемент, реализующий логику аудита.
        /// </summary>
        IAudit Audit { get; set; }

        /// <summary>
        /// Режим, в котором работает приложение: win или web.
        /// </summary>
        AppMode ApplicationMode { get; set; }

        #endregion Разное

        #region Основные методы, используемый для аудита

        /// <summary>
        /// Сообщаем о совершении потенциально аудируемого действа.
        /// </summary>
        /// <param name="operationedObject"> Объект, над которым выполняется операция. </param>
        /// <param name="dataService"> Сервис данных, который выполянет операцию. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение (по умолчанию - true).</param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе AuditService иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - null.
        /// </param>
        /// <returns> Ответ о том, можно ли выполнять операцию (если null, то значит, что что-то пошло не так). </returns>
        Guid? WriteCommonAuditOperation(
            DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null);

        /// <summary>
        /// Подтверждение созданных ранее операций аудита
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="executionVariant">Какой статус будет присвоен операции.</param>
        /// <param name="auditOperationIdList">Список айдишников записей аудита.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns>True, если всё закончилось без ошибок.</returns>
        bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, bool throwExceptions);

        /// <summary>
        /// Подтверждение созданных ранее операций аудита
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="executionVariant"> Какой статус будет присвоен операции. </param>
        /// <param name="auditOperationIdList"> Список айдишников записей аудита. </param>
        /// <param name="dataServiceConnectionString"> Строка соединения сервиса данных, который выполняет запись в БД приложения.  </param>
        /// <param name="dataServiceType"> Тип сервиса данных, который выполняет запись в БД приложения.  </param>
        /// <param name="throwExceptions"> Следует ли пробрасывать дальше возникшее исключение. </param>
        /// <returns> True, если всё закончилось без ошибок. </returns>
        bool RatifyAuditOperation(
            tExecutionVariant executionVariant,
            List<Guid> auditOperationIdList,
            string dataServiceConnectionString,
            Type dataServiceType,
            bool throwExceptions);

        /// <summary>
        /// Подтверждение созданных ранее операций аудита (выполнение зависит от выбранного режима записи данных аудита)
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="executionVariant"> Какой статус будет присвоен операции. </param>
        /// <param name="auditOperationIdList"> Список айдишников записей аудита. </param>
        /// <param name="dataService"> Сервис данных, по параметрам которого (строка соединения и тип) осуществляется поиск в AuditService.Current.AppSetting.AuditDSSettings. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> True, если всё закончилось без ошибок. </returns>
        bool RatifyAuditOperation(
            tExecutionVariant executionVariant,
            List<Guid> auditOperationIdList,
            IDataService dataService,
            bool throwExceptions);

        /// <summary>
        /// Сделать запись в аудит
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="customAuditParameters"> Параметры аудита. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> Id записи аудита. </returns>
        Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, bool throwExceptions);

        /// <summary>
        /// Сделать запись в аудит
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="customAuditParameters"> Параметры аудита. </param>
        /// <param name="dataService"> Сервис данных, по параметрам которого (строка соединения и тип) осуществляется поиск в AuditService.Current.AppSetting.AuditDSSettings. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> Id записи аудита. </returns>
        Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, IDataService dataService, bool throwExceptions);

        /// <summary>
        /// Сделать запись в аудит
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="customAuditParameters"> Параметры аудита. </param>
        /// <param name="dataServiceConnectionString"> Строка соединения сервиса данных, который выполняет запись в БД приложения.  </param>
        /// <param name="dataServiceType"> Тип сервиса данных, который выполняет запись в БД приложения.  </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> Id записи аудита. </returns>
        Guid? WriteCustomAuditOperation(
            CustomAuditParameters customAuditParameters,
            string dataServiceConnectionString,
            Type dataServiceType,
            bool throwExceptions);

        #endregion Основные методы, используемый для аудита

        #region Добавление полей аудита

        /// <summary>
        /// Добавление информации о том, кто и когда создал объект, если он это поддерживает.
        /// </summary>
        /// <param name="operationedObject">
        /// Объект, куда добавляется информация.
        /// </param>
        void AddCreateAuditInformation(DataObject operationedObject);

        /// <summary>
        /// Добавление информации о том, кто и когда отредактировал объект, если он это поддерживает.
        /// </summary>
        /// <param name="operationedObject">
        /// Объект, куда добавляется информация.
        /// </param>
        void AddEditAuditInformation(DataObject operationedObject);

        #endregion

        #region Методы по обработке данных аудита

        /// <summary>
        /// Получение представления, по которому вероятнее всего вёлся аудит объекта,
        /// по операции над которым есть запись.
        /// Данное представление будет использоваться для получения кэпшенов полей.
        /// </summary>
        /// <param name="auditRecord">Запись из аудита, по которой необходимо определить представление.</param>
        /// <returns>Найденное представление (если что-то не удалось, то выдастся null; исключения не должно быть в любом случае).</returns>
        View GetViewByAuditRecord(IAuditRecord auditRecord);

        /// <summary>
        /// Получение представления для аудита у определенного типа.
        /// </summary>
        /// <param name="type">Тип, у которого нужно получить представление для аудита.</param>
        /// <param name="operationType">Тип аудируемой операции, для которой нужно получить представление.
        /// (Select, Insert, Update или Delete).</param>
        /// <returns>Представление для аудита.</returns>
        View GetAuditViewByType(Type type, tTypeOfAuditOperation operationType);

        #endregion Методы по обработке данных аудита

        #region Доработки для записи аудита полей, которые получают своё значение только после сохранения объекта в БД.

        /// <summary>
        /// Сообщаем о совершении потенциально аудируемого действа.
        /// </summary>
        /// <param name="operationedObjects"> Объекты, над которыми выполняется операция. </param>
        /// <param name="auditOperationInfoList"> Дополнительная информация, которую необходимо передать в аудит. </param>
        /// <param name="dataService"> Сервис данных, который выполянет операцию. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение (по умолчанию - true).</param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе AuditService иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - null.
        /// </param>
        void WriteCommonAuditOperationWithAutoFields(
            IEnumerable<DataObject> operationedObjects,
            ICollection<AuditAdditionalInfo> auditOperationInfoList,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null);

        /// <summary>
        /// Сообщаем о совершении потенциально аудируемого действа.
        /// </summary>
        /// <param name="operationedObject"> Объект, над которым выполняется операция. </param>
        /// <param name="dataService"> Сервис данных, который выполянет операцию. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение (по умолчанию - true).</param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе AuditService иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - null.
        /// </param>
        /// <returns> Ответ о том, можно ли выполнять операцию (если null, то значит, что что-то пошло не так). </returns>
        [Obsolete("Use WriteCommonAuditOperationWithAutoFields(IEnumerable{DataObject},ICollection{AuditAdditionalInfo},IDataService,bool,IDbTransaction).")]
        AuditAdditionalInfo WriteCommonAuditOperationWithAutoFields(
            DataObject operationedObject,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null);

        /// <summary>
        /// Подтверждение созданных ранее операций аудита (выполнение зависит от выбранного режима записи данных аудита)
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="executionVariant"> Какой статус будет присвоен операции. </param>
        /// <param name="auditOperationInfoList"> Дополнительная информация, которую необходимо передать в аудит. </param>
        /// <param name="dataService"> Сервис данных, по параметрам которого (строка соединения и тип) осуществляется поиск в AuditService.Current.AppSetting.AuditDSSettings. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> True, если всё закончилось без ошибок. </returns>
        bool RatifyAuditOperationWithAutoFields(
            tExecutionVariant executionVariant,
            List<AuditAdditionalInfo> auditOperationInfoList,
            IDataService dataService,
            bool throwExceptions);

        #endregion Доработки для записи аудита полей, которые получают своё значение только после сохранения объекта в БД.
    }
}
