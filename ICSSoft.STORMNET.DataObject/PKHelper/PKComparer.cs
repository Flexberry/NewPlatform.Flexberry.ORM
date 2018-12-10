namespace IIS.University.Tools
{
    using System.Collections.Generic;

    using ICSSoft.STORMNET;

    /// <summary>
    ///     Вспомогательный класс для сравнения объектов.
    ///     При использовании в Distinct оставит уникальные (по ключу) notnull-объекты
    ///     и один экземпляр null-объекта (если такой существует).
    /// </summary>
    public class PKComparer<T> : IEqualityComparer<T>
        where T : DataObject
    {
        public bool Equals(T x, T y)
        {
            return PKHelper.EQDataObject(x, y);
        }

        public int GetHashCode(T obj)
        {
            return PKHelper.GetKeyByObject(obj).ToString().ToLower().GetHashCode();
        }
    }
}
