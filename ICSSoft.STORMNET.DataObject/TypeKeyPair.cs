namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// Вспомогательный класс для объединения в пары типа объекта и ключа.
    /// Используется в коллекции "живущих" сейчас объектов, собственно,
    /// для доступа к объекту.
    /// </summary> 
    public class TypeKeyPair : IComparable
    {
        /// <summary>
        /// Вспомогательный класс для объединения в пары типа объекта и ключа
        /// Используется в коллекции "живущих" сейчас объектов, собственно,
        /// для доступа к объекту.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="key">Ключ объекта.</param>
        public TypeKeyPair(Type type, object key)
        {
            Type = type;
            Key = key;
        }

        /// <summary>
        /// Тип объекта данных (.NET).
        /// </summary>
        public Type Type;

        /// <summary>
        /// Первичный ключ объекта данных.
        /// </summary>
        public object Key;

        /// <summary>
        /// Сравнение двух пар.
        /// </summary>
        /// <param name="obj">Объект для сравнения.</param>
        /// <returns>Результат сравнения.</returns>
        public int CompareTo(object obj)
        {
            TypeKeyPair typeKeyPair = (TypeKeyPair)obj;
            int result = Type.GetHashCode().CompareTo(typeKeyPair.Type.GetHashCode());
            result = result == 0 ? ((IComparable)Key).CompareTo(typeKeyPair.Key) : result;
            return result;
        }

        /// <summary>
        /// Serves as a hash function for a type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Key != null ? Key.GetHashCode() : 0);
            }
        }
    }
}