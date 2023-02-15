namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.Business.Audit.HelpStructures;

    /// <summary>
    /// Класс с данными аудита по полю объекта.
    /// </summary>
    [DataContract]
    public class CustomAuditField
    {
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="fieldName">Имя поля.</param>
        /// <param name="oldValue">Старое значение поля.</param>
        /// <param name="newValue">Новое значение поля.</param>
        public CustomAuditField(
            string fieldName,
            string oldValue,
            string newValue)
        {
            FieldName = fieldName;
            OldFieldValue = oldValue;
            NewFieldValue = newValue;
        }

        private void CheckFieldName(string fieldName)
        {
            if (!CheckHelper.IsNullOrWhiteSpace(fieldName))
            {
                throw new Exception("FieldName в CustomAuditField не может быть пусто");
            }
        }

        private string _fieldName;

        /// <summary>
        /// Имя поля, по которому записаны данные аудита.
        /// </summary>
        [DataMember]
        public string FieldName
        {
            get
            {
                CheckFieldName(_fieldName);
                return _fieldName;
            }

            set
            {
                CheckFieldName(value);
                _fieldName = value;
            }
        }

        /// <summary>
        /// Старое значение поля, по которому записаны данные аудита.
        /// </summary>
        [DataMember]
        public string OldFieldValue { get; set; }

        /// <summary>
        /// Новое значение поля, по которому записаны данные аудита.
        /// </summary>
        [DataMember]
        public string NewFieldValue { get; set; }
    }
}
