namespace ICSSoft.STORMNET.Business.Audit
{
    using System;

    /// <summary>
    /// Интерфейс, который имеется у типов, которые имеют дополнительные поля аудита.
    /// </summary>
    public interface IDataObjectWithAuditFields
    {
        /// <summary>
        /// Дата создания.
        /// </summary>
        DateTime? CreateTime { get; set; }

        /// <summary>
        /// Создатель.
        /// </summary>
        string Creator { get; set; }

        /// <summary>
        /// Дата последнего изменения.
        /// </summary>
        DateTime? EditTime { get; set; }

        /// <summary>
        /// Последний редактор.
        /// </summary>
        string Editor { get; set; }
    }
}
