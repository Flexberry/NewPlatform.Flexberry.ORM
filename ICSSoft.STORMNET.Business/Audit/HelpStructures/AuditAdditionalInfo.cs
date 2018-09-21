namespace ICSSoft.STORMNET.Business.Audit.HelpStructures
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.Exceptions;

    /// <summary>
    /// Структура для сохранения сведений об автогенерируемых полях (тех, что имеют атрибут DisableInsertPropertyAttribute).
    /// Она будет доотправляться в аудит после того, как объект будет сохранён.
    /// </summary>
    [DataContract]
    public class AuditAdditionalInfo
    {
        /// <summary>
        /// Класс для хранения старого и нового значения поля.
        /// </summary>
        [DataContract]
        public class FieldValues
        {
            /// <summary>
            /// Старое значение поля.
            /// </summary>
            [DataMember]
            public string OldValue;

            /// <summary>
            /// Новое значение поля.
            /// </summary>
            [DataMember]
            public string NewValue;

            /// <summary>
            /// Конструктор, где определяется только старое значение поля.
            /// </summary>
            /// <param name="oldValue">Старое значение поля.</param>
            public FieldValues(string oldValue)
            {
                OldValue = oldValue;
            }

            /// <summary>
            /// Конструктор, где определяются сразу старое и новое значения поля.
            /// </summary>
            /// <param name="oldValue">Старое значение поля.</param>
            /// <param name="newValue">Новое значение поля.</param>
            public FieldValues(string oldValue, string newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }

            /// <summary>
            /// Создать копию текущего объекта.
            /// </summary>
            /// <returns>Копия объекта.</returns>
            public FieldValues Clone()
            {
                return new FieldValues(OldValue, NewValue);
            }
        }

        #region Статические методы.

        /// <summary>
        /// Обновление значений сохранённых полей, которые нужно дообновить в объекте.
        /// </summary>
        /// <param name="transaction">Транзакция, в рамках которой можно выполнить дочитку.</param>
        /// <param name="sqlDataService">Сервис данных, с помощью которого можно выполнить дочитку.</param>
        /// <param name="auditAdditionalInfoList">Текущий список для обновления.</param>
        /// <returns>Обновлённый список.</returns>
        public static List<AuditAdditionalInfo> SetNewFieldValuesForList(
            IDbTransaction transaction, SQLDataService sqlDataService, List<AuditAdditionalInfo> auditAdditionalInfoList)
        {
            foreach (var auditAdditionalInfo in auditAdditionalInfoList)
            {
                auditAdditionalInfo.SetNewFieldValues(transaction, sqlDataService);
            }

            return auditAdditionalInfoList;
        }

        /// <summary>
        /// Создание записи типа AuditAdditionalInfo о дополнительных полях, которые стоило бы передавть в аудит.
        /// Если auditRecordPrimaryKey = null или auditRecordPrimaryKey.Value == Guid.Empty, то запись сформирована не будет, поскольку неизвестно будет, куда именно дописывать данные аудита.
        /// </summary>
        /// <param name="auditRecordPrimaryKey">Первичный ключ записи аудита, в который нужно дописать данные.</param>
        /// <param name="operatedObject">Объект, поля которого нужно дописать в аудит.</param>
        /// <param name="operatedObjectStatus">Статус объекта (только при изменении нужно писать старое и новое значения поля).</param>
        /// <param name="viewName">Имя представления, по которому проводится аудит.</param>
        /// <returns>Сформированная запись.</returns>
        public static AuditAdditionalInfo CreateRecord(
            Guid? auditRecordPrimaryKey, DataObject operatedObject, ObjectStatus operatedObjectStatus, string viewName)
        {
            if (auditRecordPrimaryKey == null || auditRecordPrimaryKey.Value == Guid.Empty)
            {
                return null;
            }

            return new AuditAdditionalInfo(auditRecordPrimaryKey.Value, operatedObject, operatedObjectStatus, viewName);
        }

        /// <summary>
        /// На основе списка идентификаторов записей аудита получаем список с информацией, которую необходимо передать в аудит.
        /// </summary>
        /// <param name="auditRecordPrimaryKeys">Список идентификаторов записей аудита.</param>
        /// <returns>Список с дополнительной информацией для аудита.</returns>
        public static List<AuditAdditionalInfo> GenerateRecords(List<Guid> auditRecordPrimaryKeys)
        {
            return auditRecordPrimaryKeys
                .Select(auditRecordPrimaryKey => new AuditAdditionalInfo(auditRecordPrimaryKey))
                .ToList();
        }

        /// <summary>
        /// Проверка, имеет ли свойство атрибут DisableInsertPropertyAttribute.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objectType">Тип объекта, в котором будет проверяться свойство.</param>
        /// <returns>Ответ о том, содержит ли свойство атрибут.</returns>
        public static bool HasPropertyDisableInsertPropertyAttribute(
            string propertyName, Type objectType)
        {
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new CantFindPropertyException(propertyName, objectType);
            }

            return propertyInfo.GetCustomAttributes(typeof(DisableInsertPropertyAttribute), true).Any();
        }

        #endregion Статические методы.

        #region Конструкторы.

        /// <summary>
        /// Конструктор объекта типа AuditAdditionalInfo, содержащий информацию о дополнительных полях, которые стоило бы передать в аудит.
        /// В данном конструкторе никаких полей не передаётся (сделано для удобства перехода с первой версии подтверждения аудита).
        /// </summary>
        /// <param name="auditRecordPriKey">Первичный ключ записи аудита, в который нужно дописать данные.</param>
        /// <returns>Сформированная запись.</returns>
        private AuditAdditionalInfo(Guid auditRecordPriKey)
        {
            _auditRecordPrimaryKey = auditRecordPriKey;
        }

        /// <summary>
        /// Конструктор объекта типа AuditAdditionalInfo, содержащий информацию о дополнительных полях, которые стоило бы передавть в аудит.
        /// </summary>
        /// <param name="auditRecordPriKey">Первичный ключ записи аудита, в который нужно дописать данные.</param>
        /// <param name="operatedObject">Объект, поля которого нужно дописать в аудит.</param>
        /// <param name="operatedObjectStatus">Статус объекта (только при изменении нужно писать старое и новое значения поля).</param>
        /// <param name="viewName">Имя представления, по которому проводится аудит.</param>
        /// <returns>Сформированная запись.</returns>
        private AuditAdditionalInfo(
            Guid auditRecordPriKey, DataObject operatedObject, ObjectStatus operatedObjectStatus, string viewName)
        {
            if (operatedObject == null)
            {
                throw new ArgumentNullException("operatedObject");
            }

            if (string.IsNullOrEmpty(viewName) || (viewName = viewName.Trim()) == string.Empty)
            {
                throw new ArgumentNullException("viewName");
            }

            Type operatedObjectType = operatedObject.GetType();

            var view = Information.GetView(viewName, operatedObjectType);
            if (view == null)
            {
                throw new CantFindViewException(operatedObjectType, viewName);
            }

            _auditRecordPrimaryKey = auditRecordPriKey;
            _objectPrimaryKey = operatedObject.__PrimaryKey;

            // Данная настройка нужна как для проверки атрибута DisableInsertPropertyAttribute, так и для разруливания настроек аудита для класса.
            _assemblyQualifiedObjectType = operatedObjectType.AssemblyQualifiedName;

            // В общем случае означивание полей идёт при первом сохранении объекта.
            if (operatedObjectStatus != ObjectStatus.Created)
            {
                return;
            }

            // Рассмотрим только собственные свойства, поскольку остальные нас не касаются в аудите.
            foreach (var currentProperty in
                from currentProperty in view.Properties.Where(x => !x.Name.Contains(".") && x.Visible) // Нас интересуют только видимые поля в представлении.
                where HasPropertyDisableInsertPropertyAttribute(currentProperty.Name, operatedObjectType) // Есть ли у свойства атрибут DisableInsertPropertyAttribute.
                let property = operatedObjectType.GetProperty(currentProperty.Name)
                where !property.PropertyType.IsSubclassOf(typeof(DataObject)) // Это не мастеровое свойство.
                select currentProperty)
            {
                _keptFieldsValues.Add(currentProperty.Name, new FieldValues(AuditConstants.FieldValueDeletedConst));
            }
        }

        #endregion Конструкторы.

        /// <summary>
        /// Идентификатор самого объекта.
        /// </summary>
        public object ObjectPrimaryKey
        {
            get
            {
                return _objectPrimaryKey;
            }
        }

        /// <summary>
        /// Идентификатор записи аудита.
        /// </summary>
        public Guid AuditRecordPrimaryKey
        {
            get
            {
                return _auditRecordPrimaryKey;
            }
        }

        /// <summary>
        /// Полное имя типа объекта.
        /// Используется, чтобы перепроверить, что указанные поля объекта действительно имеют атрибут DisableInsertPropertyAttribute.
        /// </summary>
        public string AssemblyQualifiedObjectType
        {
            get
            {
                return _assemblyQualifiedObjectType;
            }
        }

        /// <summary>
        /// Конструирование объекта для зачитывания исключительно свойств, которые имеют атрибут DisableInsertPropertyAttribute.
        /// </summary>
        /// <returns>Сформированное представление.</returns>
        public View ConstructViewForKeptFields()
        {
            var objectType = Type.GetType(_assemblyQualifiedObjectType, true);
            var view = new View { DefineClassType = objectType, Name = "TempAuditView" };
            foreach (var fieldValue in _keptFieldsValues)
            {
                view.AddProperty(fieldValue.Key);
            }

            return view;
        }

        /// <summary>
        /// Из БД вычитывается объект и определяются, какие значения после сохранения в БД приняли поля с атрибутом DisableInsertPropertyAttribute.
        /// Зачитка объекта идёт в той же транзакции, что и были обновлены объекты.
        /// </summary>
        /// <param name="transaction">Транзакция, в рамках которой нужно производить зачитку.</param>
        /// <param name="sqlDataService">Сервис данных, с помощью которого нужно проводить зачитку.</param>
        public void SetNewFieldValues(IDbTransaction transaction, SQLDataService sqlDataService)
        {
            if (_auditRecordPrimaryKey == null)
            {
                throw new NullReferenceException("Идентификатор записи аудита не определён.");
            }

            if (!_keptFieldsValues.Any())
            {
                return;
            }

            if (_objectPrimaryKey == null)
            {
                throw new NullReferenceException("Идентификатор записи объекта не определён.");
            }

            var objectType = Type.GetType(_assemblyQualifiedObjectType, true);
            var view = ConstructViewForKeptFields();
            var objectInstanse = (DataObject)Activator.CreateInstance(objectType);
            objectInstanse.SetExistObjectPrimaryKey(_objectPrimaryKey);
            objectInstanse.InitDataCopy();

            sqlDataService.LoadObjectByExtConn(
                view, objectInstanse, true, true, new DataObjectCache(), transaction.Connection, transaction);

            foreach (var keptFieldsValue in _keptFieldsValues)
            {
                var propertyValue = objectType.GetProperty(keptFieldsValue.Key).GetValue(objectInstanse, null);
                _keptFieldsValues[keptFieldsValue.Key].NewValue =
                    propertyValue == null
                    ? null
                    : propertyValue.ToString();
            }
        }

        /// <summary>
        /// Список с сохранёнными данными полей, имеющих атрибут DisableInsertPropertyAttribute.
        /// (Возвращается копия сохранённого списка).
        /// </summary>
        public Dictionary<string, FieldValues> KeptFieldsValues
        {
            get
            {
                return _keptFieldsValues
                    .ToDictionary(fieldValue => fieldValue.Key, fieldValue => fieldValue.Value.Clone());
            }
        }

        /// <summary>
        /// Список с сохранёнными данными полей, имеющих атрибут DisableInsertPropertyAttribute.
        /// </summary>
        [DataMember]
        private readonly Dictionary<string, FieldValues> _keptFieldsValues = new Dictionary<string, FieldValues>();

        /// <summary>
        /// Идентификатор записи аудита.
        /// </summary>
        [DataMember]
        private Guid _auditRecordPrimaryKey;

        /// <summary>
        /// Полное имя типа объекта.
        /// Используется, чтобы перепроверить, что указанные поля объекта действительно имеют атрибут DisableInsertPropertyAttribute.
        /// </summary>
        [DataMember]
        private string _assemblyQualifiedObjectType;

        /// <summary>
        /// Идентификатор самого объекта.
        /// </summary>
        private readonly object _objectPrimaryKey;
    }
}
