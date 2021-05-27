namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;

    /// <summary>
    /// Служебный класс для представления параметров в ограничениях LINQ.
    /// </summary>
    public class ParamSet
    {
        /// <summary>
        /// Получить значение параметра по имени.
        /// </summary>
        /// <typeparam name="T">Тип параметра (и его значения).</typeparam>
        /// <param name="paramName">Имя параметра.</param>
        /// <returns>Значение параметра.</returns>
        /// <remarks>На самом деле метод пока никогда не вызывается в рантайме
        /// и служит только для создания соответствующих выражений.</remarks>
        public T Get<T>(string paramName)
        {
            throw new NotImplementedException();
        }
    }
}
