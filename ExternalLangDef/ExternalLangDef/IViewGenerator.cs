namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;

    using ICSSoft.STORMNET;

    /// <summary>
    /// Интерфейс для генерации представления.
    /// Используется для работы в детейлах с динамическими представлениями.
    /// </summary>
    public interface IViewGenerator
    {
        /// <summary>
        /// Метод для получения представления, если традиционным методом представление получить не удалось.
        /// Используется при интергпретации в редакторе ограничений.
        /// </summary>
        /// <param name="viewName">Имя предпочитаемого представления.</param>
        /// <param name="dataObjectType">Имя типа, для которого нужно получить представление.</param>
        /// <returns> Сформированное представление. </returns>
        View GenerateView(string viewName, Type dataObjectType);
    }
}
