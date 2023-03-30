namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.Exceptions;

    using DataObject = ICSSoft.STORMNET.DataObject;
    using View = ICSSoft.STORMNET.View;

    /// <summary>
    /// Статическая обёртка для класса, наследующего от <see cref="IAuditService"/>.
    /// </summary>
    public class AuditService : IAuditService
    {
        #region Статические элементы

        /// <summary>
        /// Текущий класс для работы с сервисом аудита.
        /// </summary>
        private static IAuditService _currentAuditService;

        /// <summary>
        /// Текущий класс для работы с сервисом аудита.
        /// </summary>
        public static IAuditService Current => _currentAuditService ?? (_currentAuditService = new AuditService());

        /// <summary>
        /// Инициализация текущего сервиса аудита.
        /// </summary>
        /// <param name="appSetting">Настройки аудита приложения.</param>
        /// <param name="audit">Элемент, реализующий логику аудита.</param>
        /// <param name="service">Сервис аудита, который будет установлен как текущий.</param>
        public static void InitAuditService(AuditAppSetting appSetting, IAudit audit, IAuditService service)
        {
            _currentAuditService = service;
            Current.AppSetting = appSetting;
            Current.Audit = audit;
            Current.ApplicationMode = AppMode.Win;
            Current.ShowPrimaryKey = false;
        }

        #endregion Статические элементы

        #region Поля и свойства

        /// <summary>
        /// Current audit settings loader for types.
        /// </summary>
        private readonly TypeAuditSettingsLoader _typeAuditSettingsLoader = new TypeAuditClassSettingsLoader();

        /// <summary>
        /// Элемент, реализующий логику аудита.
        /// </summary>
        private IAudit _audit;

        /// <summary>
        /// Контроллер для организации асинхронной записи аудита.
        /// </summary>
        private readonly AsyncAuditController _asyncAuditController = new AsyncAuditController();

        /// <summary>
        /// Режим, в котором работает приложение: win или web.
        /// </summary>
        public AppMode ApplicationMode { get; set; } = AppMode.Unknown;

        /// <summary>
        /// Включён ли аудит для приложения.
        /// </summary>
        public bool IsAuditEnabled => AppSetting != null && AppSetting.AuditEnabled;

        /// <summary>
        /// Выполняется ли аудит удалённо (то есть через вызов AuditWinService).
        /// </summary>
        public bool IsAuditRemote => !string.IsNullOrEmpty(AppSetting.AuditWinServiceUrl);

        /// <summary>
        /// Получение имени строки соединения с БД аудита.
        /// (используется для загрузки данных из БД).
        /// Если используется запись аудита через windows-сервис, то будет возвращено <c>null</c>.
        /// </summary>
        public string AuditConnectionStringName
        {
            get
            {
                if (IsAuditRemote)
                {
                    // Строка соединения определяется в win-сервисе, так просто не получить.
                    throw new NotImplementedException("получение строки соединения при нелокальном выполнении аудита");
                }

                var connectionStringName =
                    AppSetting.IsDatabaseLocal
                                ? GetConnectionStringName(null)
                                : AppSetting.AuditConnectionStringName;

                // Проверяем, есть ли в конфиг-файле соответствующая строка соединения.
                if (!CheckHelper.IsNullOrWhiteSpace(ConfigHelper.GetConnectionString(ApplicationMode, connectionStringName)))
                {
                    // Если не нашли, то возвращаем ту, что используется по умолчанию.
                    connectionStringName = ConfigHelper.GetAppSetting(ApplicationMode, "DefaultConnectionStringName");
                }

                return connectionStringName;
            }
        }

        /// <summary>
        /// Следует ли отображать записи с изменением первичного ключа на формах.
        /// </summary>
        public bool ShowPrimaryKey { get; set; }

        /// <summary>
        /// Включить ведение аудита в приложении
        /// (предварительно должны быть проинициализированы <see cref="AppSetting"/> и <see cref="Audit"/>).
        /// </summary>
        /// <param name="throwExceptions">Следует ли вызывать исключения в ошибочной ситуации.</param>
        /// <returns> Удалось ли включить аудит. </returns>
        public bool EnableAudit(bool throwExceptions)
        {
            var isAuditEnabled = false;
            if (AppSetting != null && Audit != null)
            {
                AppSetting.AuditEnabled = true;
                isAuditEnabled = true;
            }
            else
            {
                if (throwExceptions)
                {
                    throw new DataNotFoundAuditException("необходимо инициализировать AppSetting и Audit");
                }
            }

            return isAuditEnabled;
        }

        /// <summary>
        /// Отключить ведение аудита в приложении.
        /// </summary>
        public void DisableAudit()
        {
            if (AppSetting != null)
            {
                AppSetting.AuditEnabled = false;
            }
        }

        /// <summary>
        /// Настройки аудита в приложении.
        /// </summary>
        public AuditAppSetting AppSetting { get; set; }

        /// <summary>
        /// Flag indicates that storage contains UTC audit dates. If <c>true</c> then UTC dates else local timezone dates. Default is <c>false</c>.
        /// </summary>
        public bool PersistUtcDates { get; set; }

        /// <summary>
        /// Flag indicates that this service uses <see cref="LogService.LogInfo(object)" /> and <see cref="LogService.LogInfoFormat" /> to log audit operation information.
        /// Default is <see langword="true" />.
        /// </summary>
        public bool DetailedLogEnabled { get; set; } = true;

        /// <summary>
        /// Элемент, реализующий логику аудита.
        /// </summary>
        public IAudit Audit
        {
            get
            {
                return _audit;
            }

            set
            {
                if (value == null)
                {
                    throw new Exception("Переданный экземпляр IAudit не может быть null");
                }

                _audit = value;
                _asyncAuditController.Audit = value;
            }
        }

        #endregion Поля и свойства

        #region Доработки для записи аудита полей, которые получают своё значение только после сохранения объекта в БД.

        /// <inheritdoc cref="IAuditService" />
        public virtual void WriteCommonAuditOperationWithAutoFields(
            IEnumerable<DataObject> operationedObjects,
            ICollection<AuditAdditionalInfo> auditOperationInfoList,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null)
        {
            WriteCommonAuditOperationWithAutoFieldsPrivate(operationedObjects, auditOperationInfoList, dataService, throwExceptions, transaction);
        }

        /// <inheritdoc cref="IAuditService" />
        public virtual AuditAdditionalInfo WriteCommonAuditOperationWithAutoFields(
            DataObject operationedObject,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null)
        {
            return WriteCommonAuditOperationPrivate(operationedObject, dataService, throwExceptions, transaction);
        }

        /// <inheritdoc cref="IAuditService" />
        public bool RatifyAuditOperationWithAutoFields(tExecutionVariant executionVariant, List<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions)
        {
            return RatifyAuditOperation(
                executionVariant, auditOperationInfoList, dataService.CustomizationString, dataService.GetType(), throwExceptions, true);
        }

        #endregion Доработки для записи аудита полей, которые получают своё значение только после сохранения объекта в БД.

        #region Методы по обработке данных аудита

        /// <summary>
        /// Получение представления, по которому вероятнее всего вёлся аудит объекта,
        /// по операции над которым есть запись.
        /// Данное представление будет использоваться для получения кэпшенов полей.
        /// </summary>
        /// <param name="auditRecord">Запись из аудита, по которой необходимо определить представление.</param>
        /// <returns>Найденное представление (если что-то не удалось, то выдастся <c>null</c>; исключения не должно быть в любом случае).</returns>
        public View GetViewByAuditRecord(IAuditRecord auditRecord)
        {
            // Исключений при любом раскладе не должно быть.
            if (auditRecord == null)
            {
                return null;
            }

            try
            {
                var operation = (tTypeOfAuditOperation)EnumCaption.GetValueFor(auditRecord.OperationType, typeof(tTypeOfAuditOperation));
                var type = Type.GetType(auditRecord.ObjectTypeQualifiedName);

                return _typeAuditSettingsLoader.GetAuditView(type, operation);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Получение представления для аудита у определенного типа.
        /// </summary>
        /// <param name="type">Тип, у которого нужно получить представление для аудита.</param>
        /// <param name="operationType">Тип аудируемой операции, для которой нужно получить представление (Select, Insert, Update или Delete).</param>
        /// <returns>Представление для аудита.</returns>
        public View GetAuditViewByType(Type type, tTypeOfAuditOperation operationType)
        {
            return _typeAuditSettingsLoader.HasSettings(type)
                ? _typeAuditSettingsLoader.GetAuditView(type, operationType)
                : null;
        }

        #endregion Методы по обработке данных аудита

        #region Методы для записи данных аудита

        /// <summary>
        /// Сделать запись в аудит
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="customAuditParameters">Параметры аудита.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> Идентификатор записи аудита. </returns>
        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, bool throwExceptions)
        {
            return WriteCustomAuditOperation(customAuditParameters, null, null, throwExceptions);
        }

        /// <summary>
        /// Сделать запись в аудит
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="customAuditParameters">Параметры аудита.</param>
        /// <param name="dataService">Сервис данных, по параметрам которого (строка соединения и тип) осуществляется поиск в AuditService.Current.AppSetting.AuditDSSettings.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> Идентификатор записи аудита. </returns>
        public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, IDataService dataService, bool throwExceptions)
        {
            return WriteCustomAuditOperation(
                customAuditParameters, dataService.CustomizationString, dataService.GetType(), throwExceptions);
        }

        /// <summary>
        /// Сделать запись в аудит
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="customAuditParameters">Параметры аудита. </param>
        /// <param name="dataServiceConnectionString">Строка соединения сервиса данных, который выполняет запись в БД приложения.</param>
        /// <param name="dataServiceType">Тип сервиса данных, который выполняет запись в БД приложения.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> Идентификатор записи аудита.  </returns>
        public Guid? WriteCustomAuditOperation(
            CustomAuditParameters customAuditParameters, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
        {
            try
            {
                if (AppSetting == null || !AppSetting.AuditEnabled)
                {
                    throw new DisabledAuditException();
                }

                var checkedCustomAuditParameters =
                        new CheckedCustomAuditParameters(
                            customAuditParameters.ExecutionResult,
                            customAuditParameters.OperationTime,
                            ApplicationMode,
                            AppSetting.IsDatabaseLocal
                                ? GetConnectionStringName(dataServiceConnectionString, dataServiceType)
                                : AppSetting.AuditConnectionStringName,
                            IsAuditRemote)
                        {
                            AuditObjectPrimaryKey = customAuditParameters.AuditObjectPrimaryKey,
                            AuditObjectTypeOrDescription = customAuditParameters.AuditObjectTypeOrDescription,
                            CustomAuditFieldList = customAuditParameters.CustomAuditFieldList.GetCustomAuditFields().ToArray(),
                            CustomOperation = customAuditParameters.CustomAuditOperation,
                            WriteMode = customAuditParameters.UseDefaultWriteMode ?
                                        AppSetting.DefaultWriteMode :
                                        customAuditParameters.WriteMode,
                            FullUserLogin = GetCurrentUserInfo(ApplicationMode, false),
                            UserName = GetCurrentUserInfo(ApplicationMode, true),
                            OperationSource = GetSourceInfo(ApplicationMode),
                            ThrowExceptions = throwExceptions,
                        };

                return CheckAndSendToAudit(checkedCustomAuditParameters);
            }
            catch (Exception ex)
            {
                ErrorProcesser.ProcessAuditError(ex, "AuditService, RatifyAuditOperation", throwExceptions);
                return null;
            }
        }

        /// <summary>
        /// Является ли класс аудируемым (проверяются настройки по аудиту приложения + настройки класса).
        /// </summary>
        /// <param name="curType">Исследуемый тип.</param>
        /// <returns> <c>True</c>, если является и нужно вести аудит. </returns>
        public bool IsTypeAuditable(Type curType)
        {
            return AppSetting != null
                && AppSetting.AuditEnabled
                && _typeAuditSettingsLoader.HasSettings(curType)
                && _typeAuditSettingsLoader.IsAuditEnabled(curType);
        }

        /// <summary>
        /// Подтверждение созданных ранее операций аудита
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="executionVariant">Какой статус будет присвоен операции.</param>
        /// <param name="auditOperationIdList">Список идентификаторов записей аудита.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns><c>True</c>, если всё закончилось без ошибок.</returns>
        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, bool throwExceptions)
        {
            return RatifyAuditOperation(executionVariant, auditOperationIdList, null, null, throwExceptions);
        }

        /// <summary>
        /// Подтверждение созданных ранее операций аудита
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="executionVariant">Какой статус будет присвоен операции.</param>
        /// <param name="auditOperationIdList">Список идентификаторов записей аудита.</param>
        /// <param name="dataServiceConnectionString">Строка соединения сервиса данных, который выполняет запись в БД приложения. </param>
        /// <param name="dataServiceType">Тип сервиса данных, который выполняет запись в БД приложения. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> <c>True</c>, если всё закончилось без ошибок. </returns>
        public bool RatifyAuditOperation(
            tExecutionVariant executionVariant, List<Guid> auditOperationIdList, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
        {
            return RatifyAuditOperation(
                executionVariant,
                AuditAdditionalInfo.GenerateRecords(auditOperationIdList),
                dataServiceConnectionString,
                dataServiceType,
                throwExceptions);
        }

        /// <summary>
        /// Подтверждение созданных ранее операций аудита (выполнение зависит от выбранного режима записи данных аудита)
        /// (если аудит идёт в одну БД с приложением,
        /// то будет в сервис аудита передаваться имя строки соединения,
        /// найденное в AuditService.Current.AppSetting.AuditDSSettings по параметрам переданного сервиса данных).
        /// </summary>
        /// <param name="executionVariant">Какой статус будет присвоен операции.</param>
        /// <param name="auditOperationIdList">Список идентификаторов записей аудита.</param>
        /// <param name="dataService">Сервис данных, по параметрам которого (строка соединения и тип) осуществляется поиск в AuditService.Current.AppSetting.AuditDSSettings.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <returns> <c>True</c>, если всё закончилось без ошибок. </returns>
        public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, IDataService dataService, bool throwExceptions)
        {
            return RatifyAuditOperation(
                executionVariant, auditOperationIdList, dataService.CustomizationString, dataService.GetType(), throwExceptions);
        }

        /// <summary>
        /// Сообщаем о совершении потенциально аудируемого действа.
        /// </summary>
        /// <param name="operationedObject">Объект, над которым выполняется операция.</param>
        /// <param name="dataService">Сервис данных, который выполняет операцию.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение (по умолчанию - <c>true</c>).</param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе <see cref="AuditService"/> иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - <c>null</c>.
        /// </param>
        /// <returns> Ответ о том, можно ли выполнять операцию (если <c>null</c>, то значит, что что-то пошло не так). </returns>
        public Guid? WriteCommonAuditOperation(
            DataObject operationedObject,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null)
        {
            AuditAdditionalInfo result = WriteCommonAuditOperationPrivate(operationedObject, dataService, throwExceptions, transaction);
            return result?.AuditRecordPrimaryKey;
        }

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
        private void WriteCommonAuditOperationWithAutoFieldsPrivate(
            IEnumerable<DataObject> operationedObjects,
            ICollection<AuditAdditionalInfo> auditOperationInfoList,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null)
        {
            if (operationedObjects != null)
            {
                foreach (var operationedObject in operationedObjects)
                {
                    AuditAdditionalInfo auditAdditionalInfo = WriteCommonAuditOperationWithAutoFields(operationedObject, dataService, throwExceptions, transaction); // Если что, то исключение будет проброшено
                    if (auditAdditionalInfo != null && auditAdditionalInfo.AuditRecordPrimaryKey != Guid.Empty)
                    {
                        auditOperationInfoList.Add(auditAdditionalInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Сообщаем о совершении потенциально аудируемого действа.
        /// </summary>
        /// <param name="operationedObject">Объект, над которым выполняется операция.</param>
        /// <param name="dataService">Сервис данных, который выполняет операцию.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение (по умолчанию - <c>true</c>).</param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе <see cref="AuditService"/> иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - <c>null</c>.
        /// </param>
        /// <returns>Ответ о том, можно ли выполнять операцию (если <c>null</c>, то значит, что что-то пошло не так).</returns>
        private AuditAdditionalInfo WriteCommonAuditOperationPrivate(
            DataObject operationedObject,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null)
        {
            try
            {
                if (AppSetting == null || !AppSetting.AuditEnabled)
                {
                    throw new DisabledAuditException();
                }

                AuditAdditionalInfo auditAdditionalInfo = null;
                Type dataObjectType = operationedObject.GetType();
                var commonAuditParameters = GenerateCommonAuditParameters(operationedObject, dataService, throwExceptions, transaction);
                if (commonAuditParameters != null)
                {
                    var operatedObject = commonAuditParameters.OldVersionOperatedObject ?? commonAuditParameters.OperatedObject; // для UPDATE нужно взять старую версию.
                    if (operatedObject != null)
                    {
                        // При удалении OperatedObject может быть null, если не удалось найти объект данных в БД.
                        var auditOperationId = CheckAndSendToAudit(commonAuditParameters, dataObjectType);
                        auditAdditionalInfo = AuditAdditionalInfo.CreateRecord(
                            auditOperationId,
                            operatedObject,
                            ConvertTypeOfAudit(commonAuditParameters.TypeOfAuditOperation),
                            commonAuditParameters.AuditView);
                    }
                }

                return auditAdditionalInfo;
            }
            catch (Exception ex)
            {
                ErrorProcesser.ProcessAuditError(ex, "AuditService, WriteCommonAuditOperation", throwExceptions);
                return null;
            }
        }

        /// <summary>
        /// Сгенерировать общие параметры операции аудита.
        /// </summary>
        /// <param name="operationedObject">Объект, над которым выполняется операция.</param>
        /// <param name="dataService">Сервис данных, который выполняет операцию.</param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение (по умолчанию - <c>true</c>).</param>
        /// <param name="transaction">
        /// Транзакция, через которую необходимо проводить выполнение зачиток из БД приложения аудиту
        /// (при работе <see cref="AuditService"/> иногда необходимо дочитать объект или получить сохранённую копию,
        /// а выполнение данного действия без транзакции может привести к взаимоблокировке).
        /// По умолчанию - <c>null</c>.
        /// </param>
        /// <returns>Параметры операции аудита.</returns>
        protected virtual CommonAuditParameters GenerateCommonAuditParameters(
            DataObject operationedObject,
            IDataService dataService,
            bool throwExceptions = true,
            IDbTransaction transaction = null)
        {
            try
            {
                if (AppSetting == null || !AppSetting.AuditEnabled)
                {
                    throw new DisabledAuditException();
                }

                CommonAuditParameters commonAuditParameters = null;
                Type dataObjectType = operationedObject.GetType();
                var objectStatus = operationedObject.GetStatus(false);
                if (objectStatus != ObjectStatus.UnAltered
                    && _typeAuditSettingsLoader.HasSettings(dataObjectType)
                    && _typeAuditSettingsLoader.IsAuditEnabled(dataObjectType))
                {
                    var typeOfAudit = ConvertObjectStatus(objectStatus);
                    if (_typeAuditSettingsLoader.IsAuditEnabled(dataObjectType, typeOfAudit))
                    {
                        // Настройки вообще есть и аудит для приложения и для класса включён.
                        if (DetailedLogEnabled)
                        {
                            LogService.LogInfoFormat(
                                "AuditService, WriteCommonAuditOperation: На аудит получен объект {2}:{0} со статусом {1}",
                                operationedObject,
                                objectStatus,
                                operationedObject.__PrimaryKey);
                        }

                        // Пробуем вычитать имя строки соединения непосредственно из настроек класса. Если не получается, берём по старой схеме.
                        string classConnectionStringName = _typeAuditSettingsLoader.GetAuditConnectionString(dataObjectType);
                        classConnectionStringName = CheckHelper.IsNullOrWhiteSpace(classConnectionStringName)
                            ? classConnectionStringName
                            : (AppSetting.IsDatabaseLocal ? GetConnectionStringName(dataService) : AppSetting.AuditConnectionStringName);

                        commonAuditParameters = GenerateBaseCommonAuditParameters(operationedObject, classConnectionStringName, throwExceptions);
                        commonAuditParameters.AuditView = _typeAuditSettingsLoader.GetAuditViewName(dataObjectType, typeOfAudit);
                        var curView = _typeAuditSettingsLoader.GetAuditView(dataObjectType, typeOfAudit);

                        switch (objectStatus)
                        {
                            case ObjectStatus.Deleted:
                                var deletedObject = LoadDatabaseCopy(operationedObject, curView, dataService, transaction);
                                if (deletedObject != null)
                                {
                                    commonAuditParameters.TypeOfAuditOperation = tTypeOfAuditOperation.DELETE;
                                    commonAuditParameters.OperatedObject = deletedObject;
                                }

                                break;

                            case ObjectStatus.Created:
                                var createdObject = CopyNotSavedDataObject(operationedObject);

                                commonAuditParameters.TypeOfAuditOperation = tTypeOfAuditOperation.INSERT;
                                commonAuditParameters.OperatedObject = createdObject;

                                break;

                            case ObjectStatus.Altered:
                                // Текущая версия объекта.
                                var newObject = CopyNotSavedDataObject(operationedObject);

                                // Хранимое в БД значение.
                                var oldObject = LoadDatabaseCopy(operationedObject, curView, dataService, transaction);
                                if (oldObject != null)
                                {
                                    // Догружаем объект по представлению аудита, чтобы корректно записать все поля, особенно актуально для нехранимых полей.
                                    List<string> loadedProperties = CopyAlteredNotSavedDataObject(oldObject, newObject, curView, dataService, transaction);

                                    // Пишем аудит изменения.
                                    commonAuditParameters.TypeOfAuditOperation = tTypeOfAuditOperation.UPDATE;
                                    commonAuditParameters.OperatedObject = newObject;
                                    commonAuditParameters.OldVersionOperatedObject = oldObject;
                                    commonAuditParameters.LoadedProperties = loadedProperties.ToArray();
                                }
                                else
                                {
                                    commonAuditParameters.TypeOfAuditOperation = tTypeOfAuditOperation.INSERT;
                                    commonAuditParameters.OperatedObject = newObject;
                                }

                                break;
                        }
                    }
                }

                return commonAuditParameters;
            }
            catch (Exception ex)
            {
                ErrorProcesser.ProcessAuditError(ex, "AuditService, GenerateCommonAuditParameters", throwExceptions);
                return null;
            }
        }

        /// <summary>
        /// Подтверждение созданных ранее операций аудита
        /// (если аудит идёт в одну БД с приложением, то будет использован сервис данных по умолчанию).
        /// </summary>
        /// <param name="executionVariant">Какой статус будет присвоен операции.</param>
        /// <param name="auditOperationInfoList">Информация о том, что и куда в аудит нужно добавить.</param>
        /// <param name="dataServiceConnectionString">Строка соединения сервиса данных, который выполняет запись в БД приложения. </param>
        /// <param name="dataServiceType">Тип сервиса данных, который выполняет запись в БД приложения. </param>
        /// <param name="throwExceptions">Следует ли пробрасывать дальше возникшее исключение.</param>
        /// <param name="checkClassAuditSettings">Следует ли проверять настройки аудита самого класса.</param>
        /// <returns> <c>True</c>, если всё закончилось без ошибок. </returns>
        protected virtual bool RatifyAuditOperation(
            tExecutionVariant executionVariant,
            List<AuditAdditionalInfo> auditOperationInfoList,
            string dataServiceConnectionString,
            Type dataServiceType,
            bool throwExceptions,
            bool checkClassAuditSettings = false)
        {
            try
            {
                if (AppSetting == null || !AppSetting.AuditEnabled)
                {
                    throw new DisabledAuditException();
                }

                if (auditOperationInfoList != null && auditOperationInfoList.Count > 0)
                {
                    // Настройки вообще есть и аудит для приложения включён.
                    var auditRatifyParameters = new RatificationAuditParameters(
                        executionVariant,
                        PersistUtcDates ? DateTime.UtcNow : DateTime.Now,
                        auditOperationInfoList,
                        AppSetting.DefaultWriteMode,
                        ApplicationMode,
                        AppSetting.IsDatabaseLocal
                                        ? GetConnectionStringName(dataServiceConnectionString, dataServiceType)
                                        : AppSetting.AuditConnectionStringName,
                        IsAuditRemote)
                    { ThrowExceptions = throwExceptions };

                    CheckAndSendToAudit(auditRatifyParameters, checkClassAuditSettings);
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorProcesser.ProcessAuditError(ex, "AuditService, RatifyAuditOperation", throwExceptions);
                return false;
            }
        }

        #endregion Методы для записи данных аудита

        #region Разрешение варианта отправки данных аудиту

        /// <summary>
        /// Проверка режима записи данных аудита и в зависимости от этого отправка нужным способом данных аудита.
        /// </summary>
        /// <param name="commonAuditParameters">Параметры аудита.</param>
        /// <param name="auditClassSettings">Настройки класса, объект которого отправляют на аудит.</param>
        /// <returns> Идентификатор записи об аудите. </returns>
        private Guid? CheckAndSendToAudit(CommonAuditParameters commonAuditParameters, Type dataObjectType)
        {
            var currentWriteMode = _typeAuditSettingsLoader.GetAuditWriteMode(dataObjectType);

            commonAuditParameters.WriteMode = currentWriteMode;

            if (DetailedLogEnabled)
            {
                LogService.LogInfoFormat(
                    "AuditService, CheckAndSendToAudit: {0}:{1} отправляется {2}",
                    commonAuditParameters.OperatedObject.__PrimaryKey,
                    commonAuditParameters.OperatedObject,
                    currentWriteMode == tWriteMode.Synchronous ? "синхронно" : "асинхронно");
            }

            Guid? auditOperationId = null;
            if (currentWriteMode == tWriteMode.Asynchronous)
            {
                // TODO: временно данный вариант отключён из-за проблемы с HttpContext.Current
                throw new NotImplementedException("вызов асинхронного аудита");

                // Режим записи данных асинхронный, нужно самостоятельно определить Guid записи.
                auditOperationId = Guid.NewGuid();
                commonAuditParameters.AuditEntityGuid = auditOperationId;
            }

            if (IsAuditRemote)
            {
                throw new NotImplementedException("RemoteAuditController");
            }
            else
            {
                switch (currentWriteMode)
                {
                    case tWriteMode.Synchronous:
                        // Если в классе определён собственный вариант сервиса, через который необходимо писать аудит, то пишем через него.
                        var auditClassService = _typeAuditSettingsLoader.GetAuditService(dataObjectType);
                        auditClassService = auditClassService ?? Audit;
                        auditOperationId = auditClassService.WriteCommonAuditOperation(commonAuditParameters);
                        break;

                    case tWriteMode.Asynchronous:
                        _asyncAuditController.WriteAuditOperationAsync(commonAuditParameters);
                        break;
                }
            }

            return auditOperationId;
        }

        /// <summary>
        /// Проверка режима записи данных аудита и в зависимости от этого отправка нужным способом данных аудита.
        /// </summary>
        /// <param name="checkedCustomAuditParameters">Параметры аудита.</param>
        /// <returns> Идентификатор записи об аудите. </returns>
        private Guid? CheckAndSendToAudit(CheckedCustomAuditParameters checkedCustomAuditParameters)
        {
            var currentWriteMode = checkedCustomAuditParameters.WriteMode;

            if (DetailedLogEnabled)
            {
                LogService.LogInfoFormat(
                    "AuditService, CheckAndSendToAudit: {0}:{1} отправляется {2}",
                    checkedCustomAuditParameters.AuditObjectPrimaryKey,
                    checkedCustomAuditParameters.AuditObjectTypeOrDescription,
                    currentWriteMode == tWriteMode.Synchronous ? "синхронно" : "асинхронно");
            }

            Guid? auditOperationId = null;
            if (currentWriteMode == tWriteMode.Asynchronous)
            {
                // TODO: временно данный вариант отключён из-за проблемы с HttpContext.Current
                throw new NotImplementedException("вызов асинхронного аудита");

                // Режим записи данных асинхронный, нужно самостоятельно определить Guid записи.
                auditOperationId = Guid.NewGuid();
                checkedCustomAuditParameters.AuditEntityGuid = auditOperationId;
            }

            if (IsAuditRemote)
            {
                throw new NotImplementedException("RemoteAuditController");
            }
            else
            {
                switch (currentWriteMode)
                {
                    case tWriteMode.Synchronous:
                        auditOperationId = Audit.WriteCustomAuditOperation(checkedCustomAuditParameters);
                        break;
                    case tWriteMode.Asynchronous:
                        _asyncAuditController.WriteAuditOperationAsync(checkedCustomAuditParameters);
                        break;
                }
            }

            return auditOperationId;
        }

        /// <summary>
        /// Проверка режима записи данных аудита и в зависимости от этого отправка нужным способом данных аудита.
        /// </summary>
        /// <param name="ratificationAuditParameters">Параметры аудита.</param>
        /// <param name="checkClassAuditSettings">Следует ли проверять настройки аудита в классах.</param>
        protected virtual void CheckAndSendToAudit(RatificationAuditParameters ratificationAuditParameters, bool checkClassAuditSettings)
        {
            if (DetailedLogEnabled)
            {
                LogService.LogInfoFormat(
                    "AuditService, CheckAndSendToAudit: {0} отправляется {1} со статусом {2}",
                    string.Join(", ", ratificationAuditParameters.AuditOperationInfoList.Select(x => x.ToString()).ToArray()),
                    AppSetting.DefaultWriteMode == tWriteMode.Synchronous ? "синхронно" : "асинхронно",
                    ratificationAuditParameters.ExecutionResult);
            }

            if (AppSetting.DefaultWriteMode == tWriteMode.Asynchronous)
            {
                // TODO: временно данный вариант отключён из-за проблемы с HttpContext.Current
                throw new NotImplementedException("вызов асинхронного аудита");
            }

            if (IsAuditRemote)
            {
                throw new NotImplementedException("RemoteAuditController");
            }
            else
            {
                switch (ratificationAuditParameters.WriteMode)
                {
                    case tWriteMode.Synchronous:
                        ProperRatificationWithClassSettings(ratificationAuditParameters, checkClassAuditSettings);
                        break;
                    case tWriteMode.Asynchronous:
                        _asyncAuditController.WriteAuditOperationAsync(ratificationAuditParameters);
                        break;
                }
            }
        }

        /// <summary>
        /// Вспомогательный класс для метода <see cref="ProperRatificationWithClassSettings"/>.
        /// </summary>
        private class ProperRatificationInfo
        {
            /// <summary>
            /// Вариант аудита, через который идёт работа.
            /// </summary>
            public IAudit Audit { get; set; }

            /// <summary>
            /// Имя строки соединения с БД аудита.
            /// </summary>
            public string ConnectionStringName { get; set; }

            /// <summary>
            /// Список изменений, которые необходимо послать в аудит.
            /// </summary>
            public List<AuditAdditionalInfo> AuditAdditionalInfoList { get; set; }
        }

        /// <summary>
        /// Определяем в зависимости от настроек класса, через что и куда будут писаться данные для аудита.
        /// </summary>
        /// <param name="ratificationAuditParameters">Параметры аудита.</param>
        /// <param name="checkClassAuditSettings">Следует ли проверять настройки аудита в классах.</param>
        internal void ProperRatificationWithClassSettings(RatificationAuditParameters ratificationAuditParameters, bool checkClassAuditSettings)
        {
            if (!checkClassAuditSettings)
            {
                Audit.RatifyAuditOperation(ratificationAuditParameters);
                return;
            }

            // Сначала нужно сформировать корректный список, через какие сервисы аудита какой тип должен проходить.
            List<AuditAdditionalInfo> auditAdditionalInfos = ratificationAuditParameters.AuditOperationInfoList.ToList();
            List<AuditAdditionalInfo> standartAuditAdditionalInfos = new List<AuditAdditionalInfo>();
            List<ProperRatificationInfo> properRatificationInfos = new List<ProperRatificationInfo>();

            while (auditAdditionalInfos.Any())
            {
                // Выбираем объекты одного типа, поскольку для них настройки одинаковы.
                AuditAdditionalInfo auditAdditionalInfo = auditAdditionalInfos[0];
                List<AuditAdditionalInfo> sameTypeObjects = auditAdditionalInfos
                    .Where(x => x.AssemblyQualifiedObjectType == auditAdditionalInfo.AssemblyQualifiedObjectType)
                    .ToList();

                foreach (AuditAdditionalInfo sameTypeObject in sameTypeObjects)
                {
                    auditAdditionalInfos.Remove(sameTypeObject);
                }

                Type dataObjectType = Type.GetType(auditAdditionalInfo.AssemblyQualifiedObjectType, true);
                if (!_typeAuditSettingsLoader.HasSettings(dataObjectType))
                {
                    // В классе не указаны настройки аудита. Обрабатываем стандартным образом.
                    standartAuditAdditionalInfos.AddRange(sameTypeObjects);
                    continue;
                }

                // Если в классе определён собственный вариант сервиса, через который необходимо писать аудит, то пишем через него.
                var auditClassService = _typeAuditSettingsLoader.GetAuditService(dataObjectType);
                auditClassService = auditClassService ?? Audit;

                // Пробуем вычитать имя строки соединения непосредственно из настроек класса. Если не получается, берём по старой схеме.
                var classConnectionStringName = _typeAuditSettingsLoader.GetAuditConnectionString(dataObjectType);
                classConnectionStringName = CheckHelper.IsNullOrWhiteSpace(classConnectionStringName)
                                            ? classConnectionStringName
                                            : ratificationAuditParameters.AuditConnectionStringName;

                if (auditClassService == Audit && classConnectionStringName == ratificationAuditParameters.AuditConnectionStringName)
                {
                    // В классе указаны стандартные настройки аудита, как и для всего приложения. Обрабатываем стандартным образом.
                    standartAuditAdditionalInfos.AddRange(sameTypeObjects);
                    continue;
                }

                ProperRatificationInfo properRatificationInfoExisted
                        = properRatificationInfos.FirstOrDefault(x => x.Audit == auditClassService && x.ConnectionStringName == classConnectionStringName);
                if (properRatificationInfoExisted != null)
                {
                    // Есть другие объекты, которые нужно обрабатывать с такими же настройками.
                    properRatificationInfoExisted.AuditAdditionalInfoList.AddRange(sameTypeObjects);
                }
                else
                {
                    properRatificationInfos.Add(
                        new ProperRatificationInfo { Audit = auditClassService, ConnectionStringName = classConnectionStringName, AuditAdditionalInfoList = sameTypeObjects });
                }
            }

            properRatificationInfos.Insert(
                0,
                new ProperRatificationInfo
                {
                    Audit = Audit,
                    ConnectionStringName = ratificationAuditParameters.AuditConnectionStringName,
                    AuditAdditionalInfoList = standartAuditAdditionalInfos,
                });

            foreach (ProperRatificationInfo properRatificationInfo in properRatificationInfos)
            {
                RatificationAuditParameters ratificationAuditParametersClone = ratificationAuditParameters.CloneWithoutAuditOperationInfoList();
                ratificationAuditParametersClone.AuditOperationInfoList = properRatificationInfo.AuditAdditionalInfoList;
                ratificationAuditParametersClone.AuditConnectionStringName = properRatificationInfo.ConnectionStringName;
                properRatificationInfo.Audit.RatifyAuditOperation(ratificationAuditParametersClone);
            }
        }

        #endregion Разрешение варианта отправки данных аудиту

        #region Вспомогательные методы

        /// <summary>
        /// Генерация базовой части <see cref="CommonAuditParameters"/>
        /// (остальное генерится по-своему в зависимости от типа операции).
        /// </summary>
        /// <param name="connectionStringName">Имя строки соединения, которое должно использоваться сервисом аудита.</param>
        /// <param name="throwExceptions">Используемый при конструировании параметр, следует ли пробрасывать исключение.</param>
        /// <returns> Сконструированные параметры аудита. </returns>
        private CommonAuditParameters GenerateBaseCommonAuditParameters(DataObject operatedObject, string connectionStringName, bool throwExceptions)
        {
            // В сам объект будет записываться имя того, кто совершил операцию, а не логин.
            string fullUserLogin = GetCurrentUserInfo(ApplicationMode, false);
            string userName = GetCurrentUserInfo(ApplicationMode, true);
            string currentSourceInfo = GetSourceInfo(ApplicationMode);
            DateTime operationTime = GetAuditOperationTime(operatedObject);

            return new CommonAuditParameters(
                true,
                tExecutionVariant.Unexecuted,
                currentSourceInfo,
                fullUserLogin,
                userName,
                operationTime,
                ApplicationMode,
                connectionStringName,
                IsAuditRemote)
            {
                ThrowExceptions = throwExceptions,
            };
        }

        /// <summary>
        /// Gets the time when auditable operation occurred with the specified object.
        /// </summary>
        /// <param name="operatedObject">The operated object.</param>
        /// <returns>Returns time when auditable operation occurred (<see cref="DateTime.Now"/> by default).</returns>
        protected virtual DateTime GetAuditOperationTime(DataObject operatedObject)
        {
            return PersistUtcDates ? DateTime.UtcNow : DateTime.Now;
        }

        /// <summary>
        /// Получение имени строки соединения с БД приложения на основании переданного сервиса данных
        /// (поиск осуществляется среди <see cref="DetailArrayOfAuditDSSetting"/> по строке соединения и типу сервиса данных;
        /// если переданный сервис данных null, то берётся первое валидное имя строки соединения в <see cref="DetailArrayOfAuditDSSetting"/>).
        /// </summary>
        /// <param name="dataService">Сервис данных.</param>
        /// <returns> Имя строки соединения с БД приложения. </returns>
        private string GetConnectionStringName(IDataService dataService)
        {
            return dataService == null ?
                GetConnectionStringName(null, null) :
                GetConnectionStringName(dataService.CustomizationString, dataService.GetType());
        }

        /// <summary>
        /// Получение имени строки соединения с БД приложения на основании переданного сервиса данных
        /// (поиск осуществляется среди <see cref="DetailArrayOfAuditDSSetting"/> по строке соединения и типу сервиса данных;
        /// если переданный сервис данных null, то берётся первое валидное имя строки соединения в <see cref="DetailArrayOfAuditDSSetting"/>).
        /// </summary>
        /// <param name="dataServiceConnectionString">Строка соединения сервиса данных, который выполняет запись в БД приложения. </param>
        /// <param name="dataServiceType">Тип сервиса данных, который выполняет запись в БД приложения.</param>
        /// <returns> Имя строки соединения с БД приложения. </returns>
        protected virtual string GetConnectionStringName(string dataServiceConnectionString, Type dataServiceType)
        {
            if (AppSetting == null)
            {
                return string.Empty;
            }

            var resultConnectionStringName = string.Empty;
            var detailArrayOfAuditDsSetting = AppSetting.AuditDSSettings;
            if (detailArrayOfAuditDsSetting == null || detailArrayOfAuditDsSetting.Count <= 0)
            {
                return string.Empty;
            }

            if (dataServiceType == null || !CheckHelper.IsNullOrWhiteSpace(dataServiceConnectionString))
            {
                resultConnectionStringName =
                    (from AuditDSSetting auditDsSetting in detailArrayOfAuditDsSetting
                     where CheckHelper.IsNullOrWhiteSpace(auditDsSetting.ConnStringName)
                     select auditDsSetting.ConnStringName).FirstOrDefault();
            }
            else
            {
                var auditDsSettingList = (from AuditDSSetting auditDsSetting in detailArrayOfAuditDsSetting
                                          where
                                              string.Equals(auditDsSetting.ConnString, dataServiceConnectionString, StringComparison.CurrentCultureIgnoreCase)
                                              && auditDsSetting.DataServiceType == dataServiceType
                                              && CheckHelper.IsNullOrWhiteSpace(auditDsSetting.ConnStringName)
                                          select auditDsSetting.ConnStringName).ToList();
                if (auditDsSettingList.Any())
                {
                    resultConnectionStringName = auditDsSettingList[0];
                }
            }

            return CheckHelper.IsNullOrWhiteSpace(resultConnectionStringName)
                ? resultConnectionStringName.Trim()
                : string.Empty;
        }

        /// <summary>
        /// Добавление информации о том, кто и когда создал объект, если он это поддерживает.
        /// </summary>
        /// <param name="operationedObject">Объект, куда добавляется информация.</param>
        public virtual void AddCreateAuditInformation(DataObject operationedObject)
        {
            var dataObjectWithAuditFields = operationedObject as IDataObjectWithAuditFields;
            if (dataObjectWithAuditFields != null)
            {
                // Добавляем поля, кто же создал объект. //TODO: определить эту запись в правильное место.
                dataObjectWithAuditFields.CreateTime = GetAuditOperationTime(operationedObject);
                dataObjectWithAuditFields.Creator = GetCurrentUserInfo(ApplicationMode, true);
            }
        }

        /// <summary>
        /// Добавление информации о том, кто и когда отредактировал объект, если он это поддерживает.
        /// </summary>
        /// <param name="operationedObject">Объект, куда добавляется информация.</param>
        public virtual void AddEditAuditInformation(DataObject operationedObject)
        {
            var dataObjectWithAuditFields = operationedObject as IDataObjectWithAuditFields;
            if (dataObjectWithAuditFields != null)
            {
                // Добавляем поля, кто же изменил объект. //TODO: определить эту запись в правильное место.
                operationedObject.AddLoadedProperties(nameof(IDataObjectWithAuditFields.EditTime), nameof(IDataObjectWithAuditFields.Editor));
                dataObjectWithAuditFields.EditTime = GetAuditOperationTime(operationedObject);
                dataObjectWithAuditFields.Editor = GetCurrentUserInfo(ApplicationMode, true);
            }
        }

        #region Копирование свойств из старого объекта в новый.

        /// <summary>
        /// Скопировать недостающие свойства из старого объекта, которые не загружены в новом объекте.
        /// </summary>
        /// <param name="oldObject">Старый вариант объекта (как он лежит в БД).</param>
        /// <param name="newObject">Текущая версия объекта (как она сейчас будет записана в БД).</param>
        /// <param name="auditView">Представление, по которому должна быть загружена текущая версия.</param>
        /// <param name="dataService">Сервис данных, через который возможно придётся вычитывать мастера.</param>
        /// <param name="transaction">Транзакция, через которую необходимо проводить дочитывание объекта, чтобы не привести к взаимоблокировке.</param>
        /// <param name="prefix">Префикс, который нужно дописать к возвращаемым свойствам.</param>
        /// <returns>Дозагруженные (вручную проставленные) свойства объекта.</returns>
        internal static List<string> CopyAlteredNotSavedDataObject(
            DataObject oldObject, DataObject newObject, View auditView, IDataService dataService, IDbTransaction transaction, string prefix = null)
        {
            if (newObject == null || auditView == null)
            {
                throw new ArgumentNullException();
            }

            // Догружаем объект по представлению аудита, чтобы корректно записать все поля, особенно актуально для нехранимых полей.
            List<string> loadedProperties = newObject.GetLoadedProperties().ToList();
            List<string> alteredProperties = newObject.GetAlteredPropertyNames().ToList();
            List<string> alreadyLoadedProperties = loadedProperties.ToList();
            List<string> notStoredProperties = Information.GetNotStorablePropertyNames(newObject.GetType()).ToList();

            List<PropertyInView> auditProperties = auditView.Properties.ToList();

            while (auditProperties.Any())
            {
                PropertyInView currentProperty = auditProperties[0];
                var propertyName = currentProperty.Name;
                if (propertyName.Contains("."))
                {
                    // Из свойства мастера делаем просто мастера.
                    propertyName = propertyName.Split('.')[0];
                }

                Type propertyType = Information.GetPropertyType(newObject.GetType(), propertyName);
                oldObject = propertyType.IsSubclassOf(typeof(DataObject))
                    ? CopyMaster(auditProperties, propertyName, alreadyLoadedProperties, loadedProperties, alteredProperties, notStoredProperties, oldObject, newObject, auditView, dataService, transaction)
                    : CopyOwnProperty(auditProperties, currentProperty, alreadyLoadedProperties, notStoredProperties, oldObject, newObject, auditView, dataService, transaction);
            }

            alreadyLoadedProperties = alreadyLoadedProperties.Union(loadedProperties).Distinct().ToList();
            return string.IsNullOrEmpty(prefix)
                ? alreadyLoadedProperties
                : alreadyLoadedProperties.Select(alreadyLoadedProperty => prefix + alreadyLoadedProperty).ToList();
        }

        /// <summary>
        /// Копирование мастера и его свойств из исходного варианта аудируемого объекта в текущий.
        /// </summary>
        /// <param name="auditProperties">Свойства, которые требуется скопировать.</param>
        /// <param name="propertyName">Текущее обрабатываемое свойство (имя мастера).</param>
        /// <param name="alreadyLoadedProperties">Свойства, которые либо были изначально загружены, либо уже скопированы.</param>
        /// <param name="loadedProperties">Свойства, которые либо были изначально загружены.</param>
        /// <param name="alteredProperties">Свойства, которые были изменены в текущей версии аудируемого объекта.</param>
        /// <param name="notStoredProperties">Нехранимые свойства аудируемого объекта.</param>
        /// <param name="oldObject">Исходный вариант аудируемого объекта.</param>
        /// <param name="newObject">Текущий вариант аудируемого объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит объекта.</param>
        /// <param name="dataService">Сервис данных, через который по необходимости может быть произведено обращение к БД.</param>
        /// <param name="transaction">Транзакция, в рамках которой может потребоваться обратиться к БД.</param>
        private static DataObject CopyMaster(
            List<PropertyInView> auditProperties,
            string propertyName,
            List<string> alreadyLoadedProperties,
            List<string> loadedProperties,
            List<string> alteredProperties,
            List<string> notStoredProperties,
            DataObject oldObject,
            DataObject newObject,
            View auditView,
            IDataService dataService,
            IDbTransaction transaction)
        {
            // Это мастер.
            if (!notStoredProperties.Contains(propertyName))
            {
                oldObject = CopyStoredMaster(auditProperties, propertyName, alreadyLoadedProperties, loadedProperties, alteredProperties, oldObject, newObject, auditView, dataService, transaction);
            }
            else
            {
                CopyNotStoredMaster(auditProperties, alreadyLoadedProperties, propertyName);
            }

            return oldObject;
        }

        /// <summary>
        /// Копирование хранимого мастера и его свойств из исходного варианта аудируемого объекта в текущий.
        /// </summary>
        /// <param name="auditProperties">Свойства, которые требуется скопировать.</param>
        /// <param name="propertyName">Текущее обрабатываемое свойство (имя мастера).</param>
        /// <param name="alreadyLoadedProperties">Свойства, которые либо были изначально загружены, либо уже скопированы.</param>
        /// <param name="loadedProperties">Свойства, которые либо были изначально загружены.</param>
        /// <param name="alteredProperties">Свойства, которые были изменены в текущей версии аудируемого объекта.</param>
        /// <param name="oldObject">Исходный вариант аудируемого объекта.</param>
        /// <param name="newObject">Текущий вариант аудируемого объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит объекта.</param>
        /// <param name="dataService">Сервис данных, через который по необходимости может быть произведено обращение к БД.</param>
        /// <param name="transaction">Транзакция, в рамках которой может потребоваться обратиться к БД.</param>
        private static DataObject CopyStoredMaster(
            List<PropertyInView> auditProperties,
            string propertyName,
            List<string> alreadyLoadedProperties,
            List<string> loadedProperties,
            List<string> alteredProperties,
            DataObject oldObject,
            DataObject newObject,
            View auditView,
            IDataService dataService,
            IDbTransaction transaction)
        {
            // Свойства хранимые, спокойно копируем.
            oldObject = !loadedProperties.Contains(propertyName)
                ? CopyStoredNotLoadedMaster(auditProperties, propertyName, alreadyLoadedProperties, oldObject, newObject, auditView, dataService, transaction)
                : CopyStoredLoadedMaster(auditProperties, propertyName, alreadyLoadedProperties, alteredProperties, oldObject, newObject, auditView, dataService, transaction);

            return oldObject;
        }

        /// <summary>
        /// Копирование хранимого мастера, который был загружен, и его свойств из исходного варианта аудируемого объекта в текущий.
        /// </summary>
        /// <param name="auditProperties">Свойства, которые требуется скопировать.</param>
        /// <param name="propertyName">Текущее обрабатываемое свойство (имя мастера).</param>
        /// <param name="alreadyLoadedProperties">Свойства, которые либо были изначально загружены, либо уже скопированы.</param>
        /// <param name="alteredProperties">Свойства, которые были изменены в текущей версии аудируемого объекта.</param>
        /// <param name="oldObject">Исходный вариант аудируемого объекта.</param>
        /// <param name="newObject">Текущий вариант аудируемого объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит объекта.</param>
        /// <param name="dataService">Сервис данных, через который по необходимости может быть произведено обращение к БД.</param>
        /// <param name="transaction">Транзакция, в рамках которой может потребоваться обратиться к БД.</param>
        private static DataObject CopyStoredLoadedMaster(
            List<PropertyInView> auditProperties,
            string propertyName,
            List<string> alreadyLoadedProperties,
            List<string> alteredProperties,
            DataObject oldObject,
            DataObject newObject,
            View auditView,
            IDataService dataService,
            IDbTransaction transaction)
        {
            var newValue = (DataObject)Information.GetPropValueByName(newObject, propertyName);
            List<PropertyInView> removeList = auditProperties.Where(x => x.Name == propertyName || x.Name.StartsWith(propertyName + ".")).ToList();
            if (newValue != null)
            {
                // В новом объекте мастер загружен. Теперь мастер либо изменился, либо его первичный ключ остался тот же.
                DataObject oldValueMaster = null;

                // Если мастер не изменён, то свойства можно копировать из подгруженной копии.
                if (!alteredProperties.Contains(propertyName))
                {
                    // Возможно мастер ещё не подгружен.
                    if (oldObject == null)
                    {
                        oldObject = LoadDatabaseCopy(newObject, auditView, dataService, transaction);
                    }

                    oldValueMaster =
                        (DataObject)Information.GetPropValueByName(oldObject, propertyName);
                }

                alreadyLoadedProperties.AddRange(
                    CopyAlteredNotSavedDataObject(oldValueMaster, newValue, auditView.GetViewForMaster(propertyName), dataService, transaction, propertyName + '.'));
                if (!alreadyLoadedProperties.Contains(propertyName))
                {
                    alreadyLoadedProperties.Add(propertyName);
                }

                foreach (PropertyInView removeProperty in removeList)
                {
                    auditProperties.Remove(removeProperty);
                }
            }
            else
            {
                // Новый мастер загружен и он имеет значение null.
                foreach (PropertyInView removeProperty in removeList)
                {
                    auditProperties.Remove(removeProperty);
                    if (!alreadyLoadedProperties.Contains(removeProperty.Name))
                    {
                        alreadyLoadedProperties.Add(removeProperty.Name);
                    }
                }
            }

            return oldObject;
        }

        /// <summary>
        /// Копирование хранимого мастера, который не был загружен, и его свойств из исходного варианта аудируемого объекта в текущий.
        /// </summary>
        /// <param name="auditProperties">Свойства, которые требуется скопировать.</param>
        /// <param name="propertyName">Текущее обрабатываемое свойство (имя мастера).</param>
        /// <param name="alreadyLoadedProperties">Свойства, которые либо были изначально загружены, либо уже скопированы.</param>
        /// <param name="oldObject">Исходный вариант аудируемого объекта.</param>
        /// <param name="newObject">Текущий вариант аудируемого объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит объекта.</param>
        /// <param name="dataService">Сервис данных, через который по необходимости может быть произведено обращение к БД.</param>
        /// <param name="transaction">Транзакция, в рамках которой может потребоваться обратиться к БД.</param>
        private static DataObject CopyStoredNotLoadedMaster(
            List<PropertyInView> auditProperties,
            string propertyName,
            List<string> alreadyLoadedProperties,
            DataObject oldObject,
            DataObject newObject,
            View auditView,
            IDataService dataService,
            IDbTransaction transaction)
        {
            // В новом объекте мастер не загружен. Можно его просто скопировать целиком из старого объекта.
            // Возможно мастер ещё не подгружен.
            if (oldObject == null)
            {
                oldObject = LoadDatabaseCopy(newObject, auditView, dataService, transaction);
            }

            object masterValue = Information.GetPropValueByName(oldObject, propertyName);

            // Целиком скопируем мастера.
            if (masterValue != null)
            {
                Information.SetPropValueByName(newObject, propertyName, masterValue);
            }

            // И все зависимые можно удалить.
            List<PropertyInView> removeList = auditProperties.Where(x => x.Name == propertyName || x.Name.StartsWith(propertyName + ".")).ToList();
            foreach (PropertyInView removeProperty in removeList)
            {
                auditProperties.Remove(removeProperty);
                if (!alreadyLoadedProperties.Contains(removeProperty.Name))
                {
                    alreadyLoadedProperties.Add(removeProperty.Name);
                }
            }

            return oldObject;
        }

        /// <summary>
        /// Копирование нехранимого мастера и его свойств из исходного варианта аудируемого объекта в текущий.
        /// Фактически мастер не копируется.
        /// </summary>
        /// <param name="auditProperties">Свойства, которые требуется скопировать.</param>
        /// <param name="propertyName">Текущее обрабатываемое свойство (имя мастера).</param>
        /// <param name="alreadyLoadedProperties">Свойства, которые либо были изначально загружены, либо уже скопированы.</param>
        private static void CopyNotStoredMaster(
            List<PropertyInView> auditProperties,
            List<string> alreadyLoadedProperties,
            string propertyName)
        {
            // Нехранимые свойства копировать не будем.
            List<PropertyInView> removeList = auditProperties.Where(x => x.Name == propertyName || x.Name.StartsWith(propertyName + ".")).ToList();
            foreach (PropertyInView removeProperty in removeList)
            {
                auditProperties.Remove(removeProperty);
                if (!alreadyLoadedProperties.Contains(removeProperty.Name))
                {
                    alreadyLoadedProperties.Add(removeProperty.Name);
                }
            }
        }

        /// <summary>
        /// Копирование собственных свойств объекта.
        /// </summary>
        /// <param name="auditProperties">Свойства, которые требуется скопировать.</param>
        /// <param name="currentProperty">Текущее обрабатываемое свойство из представления.</param>
        /// <param name="alreadyLoadedProperties">Свойства, которые либо были изначально загружены, либо уже скопированы.</param>
        /// <param name="notStoredProperties">Нехранимые свойства аудируемого объекта.</param>
        /// <param name="oldObject">Исходный вариант аудируемого объекта.</param>
        /// <param name="newObject">Текущий вариант аудируемого объекта.</param>
        /// <param name="auditView">Представление, по которому ведётся аудит объекта.</param>
        /// <param name="dataService">Сервис данных, через который по необходимости может быть произведено обращение к БД.</param>
        /// <param name="transaction">Транзакция, в рамках которой может потребоваться обратиться к БД.</param>
        private static DataObject CopyOwnProperty(
            List<PropertyInView> auditProperties,
            PropertyInView currentProperty,
            List<string> alreadyLoadedProperties,
            List<string> notStoredProperties,
            DataObject oldObject,
            DataObject newObject,
            View auditView,
            IDataService dataService,
            IDbTransaction transaction)
        {
            string propertyName = currentProperty.Name;

            // Это собственное свойство.
            auditProperties.Remove(currentProperty);

            if (alreadyLoadedProperties.Contains(propertyName))
            {
                return oldObject;
            }

            // Нехранимые свойства копировать не будем.
            if (!notStoredProperties.Contains(propertyName))
            {
                // Возможно мастер ещё не подгружен.
                if (oldObject == null)
                {
                    oldObject = LoadDatabaseCopy(newObject, auditView, dataService, transaction);
                }

                object propertyValue = Information.GetPropValueByName(oldObject, propertyName);
                Information.SetPropValueByName(newObject, propertyName, propertyValue);
            }

            alreadyLoadedProperties.Add(propertyName);

            return oldObject;
        }

        #endregion Копирование свойств из старого объекта в новый.

        /// <summary>
        /// Скопировать объект, который ещё не был сохранён.
        /// </summary>
        /// <param name="operationedObject">Объект для копирования.</param>
        /// <returns> Копия объекта. </returns>
        private static DataObject CopyNotSavedDataObject(DataObject operationedObject)
        {
            // Копируем создаваемый объект (а то вдруг он понадобится ещё).
            var createdObject = (DataObject)Activator.CreateInstance(operationedObject.GetType());
            operationedObject.CopyTo(createdObject, true, true, false);
            return createdObject;
        }

        /// <summary>
        /// Вычитать из базы соответствующий объекту объект.
        /// TODO: в большинстве ситуаций достаточно проверить, что объект уже загружен необходимыми свойствами (рекурсия по детейлам).
        /// </summary>
        /// <param name="operationedObject">Объект, чью копию будем вычитывать из БД.</param>
        /// <param name="curView">Представление, по которому будем вычитывать из БД.</param>
        /// <param name="dataService">Сервис данных.</param>
        /// <param name="transaction">Транзакция, через которую необходимо проводить дочитывание объекта, чтобы не привести к взаимоблокировке.</param>
        /// <returns> Вычитанный из БД объект (<c>null</c>, если что-то пошло не так). </returns>
        private static DataObject LoadDatabaseCopy(DataObject operationedObject, View curView, IDataService dataService, IDbTransaction transaction)
        {
            DataObject operationedDbObject = (DataObject)Activator.CreateInstance(operationedObject.GetType());
            operationedDbObject.SetExistObjectPrimaryKey(operationedObject.__PrimaryKey);

            try
            {
                // TODO: потом добавить проверку по правам, убрать аудит вычитки.
                if (transaction == null)
                {
                    // Не передано никаких транзакций, поэтому можно выполнить вычитку традиционным образом.
                    dataService.LoadObject(curView, operationedDbObject, true, true);
                }
                else
                {
                    // Передана транзакция; чтение нужно проводить через неё.
                    if (transaction.Connection == null)
                    {
                        throw new NullReferenceException("У переданной транзакции поле Connection не определено.");
                    }

                    ((SQLDataService)dataService).LoadObjectByExtConn(curView, operationedDbObject, true, true, new DataObjectCache(), transaction.Connection, transaction);
                }

                return operationedDbObject;
            }
            catch (Exception ex)
            {
                // CantFindDataObjectException значит, что в БД этот объект не сохранялся
                if (!(ex is CantFindDataObjectException))
                {
                    throw;
                }

                return null;
            }
        }

        /// <summary>
        /// Получение информации о текущем пользователе.
        /// </summary>
        /// <param name="curMode">Текущий режим работы приложения.</param>
        /// <param name="needNameNotLogin">Данный метод должен постараться вернуть дружественное имя пользователя (логин выдаётся в крайнем случае).</param>
        /// <returns>Имя пользователя.</returns>
        /// <exception cref="Exception">Если задан неподдерживаемый режим, то произойдёт исключение.</exception>
        private static string GetCurrentUserInfo(AppMode curMode, bool needNameNotLogin)
        {
            // Сначала пробуем определить имя пользователя через CurrentUserService.
            try
            {
                // Данный метод должен отработать как в win, так и в web.
                var currentUser = CurrentUserService.CurrentUser;
                if (currentUser != null)
                {
                    if (needNameNotLogin)
                    {
                        var friendlyName = currentUser.FriendlyName;
                        if (CheckHelper.IsNullOrWhiteSpace(friendlyName))
                        {
                            return friendlyName;
                        }
                    }
                    else
                    {
                        if (CheckHelper.IsNullOrWhiteSpace(currentUser.Domain))
                        {
                            return string.Join("\\", new[] { currentUser.Domain, currentUser.Login });
                        }

                        return currentUser.Login;
                    }

                    // Дружественное имя не определено. Используется логин.
                    var loginName = currentUser.Login;
                    if (CheckHelper.IsNullOrWhiteSpace(loginName))
                    {
                        return loginName;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("AuditService, GetCurrentUserInfo: Произошла ошибка при работе с CurrentUserService", ex);
            }

            // TODO: подумать, что стоит делать.
            throw new DataNotFoundAuditException("не удалось определить текущего пользователя");
        }

        /// <summary>
        /// Получение информации о том, откуда выполняется аудируемая операция.
        /// </summary>
        /// <param name="curMode">Текущий режим работы приложения.</param>
        /// <returns> Информация о том, откуда выполняется аудируемая операция. </returns>
        /// <exception cref="Exception">Если задан неподдерживаемый режим, то произойдёт исключение. </exception>
        private static string GetSourceInfo(AppMode curMode)
        {
            switch (curMode)
            {
                case AppMode.Web:
                case AppMode.Win:
                    return $"Имя компьютера: {Environment.MachineName}; Домен: {Environment.UserDomainName}; Пользователь: {Environment.UserName}";
            }

            throw new NotImplementedException("работа аудита в неподдерживаемом режиме");
        }

        private static ObjectStatus ConvertTypeOfAudit(tTypeOfAuditOperation typeOfAudit)
        {
            switch (typeOfAudit)
            {
                case tTypeOfAuditOperation.INSERT:
                    return ObjectStatus.Created;
                case tTypeOfAuditOperation.UPDATE:
                    return ObjectStatus.Altered;
                case tTypeOfAuditOperation.DELETE:
                    return ObjectStatus.Deleted;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeOfAudit), typeOfAudit, null);
            }
        }

        private static tTypeOfAuditOperation ConvertObjectStatus(ObjectStatus status)
        {
            switch (status)
            {
                case ObjectStatus.Created:
                    return tTypeOfAuditOperation.INSERT;
                case ObjectStatus.Deleted:
                    return tTypeOfAuditOperation.DELETE;
                case ObjectStatus.Altered:
                    return tTypeOfAuditOperation.UPDATE;
                case ObjectStatus.UnAltered:
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        #endregion Вспомогательные методы
    }
}
