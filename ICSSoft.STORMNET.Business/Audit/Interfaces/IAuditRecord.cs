namespace ICSSoft.STORMNET.Business.Audit
{
    /// <summary>
    /// Интерфейс для определения записи, которая идёт в аудит.
    /// Этот интерфейс позволяет не завязываться на конкретной структуре, как хранится аудит,
    /// но получать необходимые данные.
    /// </summary>
    public interface IAuditRecord
    {
        /// <summary>
        /// Имя типа, который имел записанный объект.
        /// </summary>
        string ObjectTypeQualifiedName { get; }

        /// <summary>
        /// Тип операции, которая была выполнена над объектом.
        /// </summary>
        string OperationType { get; }
    }
}
