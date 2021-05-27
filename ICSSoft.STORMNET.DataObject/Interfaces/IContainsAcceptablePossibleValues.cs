namespace ICSSoft.STORMNET
{
    /// <summary>
    /// Интерфейс для пользовательских типов, которые содержат домен (список) допустимых значений.
    /// </summary>
    public interface IContainsAcceptablePossibleValues
    {
        /// <summary>
        /// Получить список допустимых значений note: [static].
        /// </summary>
        /// <remarks>Желательно, чтобы метод должен был статическим (либо иметь конструктор без параметров).</remarks>
        /// <returns>Список допустимых значений, поддерживаемый объектом пользовательского типа.</returns>
        object[] GetListOfPossibleValues();
    }
}
