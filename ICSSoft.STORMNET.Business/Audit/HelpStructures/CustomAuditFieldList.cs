namespace ICSSoft.STORMNET.Business.Audit
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Класс со списком данных аудита по полям объекта.
    /// </summary>
    public class CustomAuditFieldList
    {
        private List<CustomAuditField> customAuditFieldsList;

        private List<CustomAuditField> CustomAuditFieldsList
        {
            get
            {
                return customAuditFieldsList ?? (customAuditFieldsList = new List<CustomAuditField>());
            }
        }

        /// <summary>
        /// Добавление в список сведений об изменении поля в соответствии с аудитом.
        /// </summary>
        /// <param name="customAuditField">Сведения об изменении поля в соответствии с аудитом.</param>
        public void AddCustomAuditField(CustomAuditField customAuditField)
        {
            if (!(from auditField in CustomAuditFieldsList
                  where auditField.FieldName == customAuditField.FieldName &&
                  auditField.OldFieldValue == customAuditField.OldFieldValue &&
                  auditField.NewFieldValue == customAuditField.NewFieldValue
                  select auditField).Any())
            {
                CustomAuditFieldsList.Add(customAuditField);
            }
        }

        /// <summary>
        /// Добавление в список сведений об изменении полей в соответствии с аудитом.
        /// </summary>
        /// <param name="customAuditFields">
        /// Сведения об изменении полей в соответствии с аудитом.
        /// </param>
        public void AddCustomAuditFields(IEnumerable<CustomAuditField> customAuditFields)
        {
            foreach (var customAuditField in customAuditFields)
            {
                AddCustomAuditField(customAuditField);
            }
        }

        /// <summary>
        /// Очистка сведений об изменении полей.
        /// </summary>
        public void ClearCustomAuditFields()
        {
            CustomAuditFieldsList.Clear();
        }

        /// <summary>
        /// Получение копии списка со сведениями об изменении полей в соответствии с аудитом.
        /// </summary>
        /// <returns>
        /// Копия списка со сведениями об изменении полей в соответствии с аудитом.
        /// </returns>
        public List<CustomAuditField> GetCustomAuditFields()
        {
            return CustomAuditFieldsList.Select(x => x).ToList();
        }
    }
}
