namespace ICSSoft.STORMNET.Business.Interfaces
{
    using System;

    /// <summary>
    /// Определяет методы получения бизнес-серверов обрабатываемых объектов.
    /// </summary>
    public interface IBusinessServerProvider
    {
        /// <summary>
        /// Получить бизнес сервера.
        /// </summary>
        /// <param name="dataObjectType">для объекта типа.</param>
        /// <param name="objectStatus">Статус объекта.</param>
        /// <param name="ds">Текущий сервис данных.</param>
        /// <returns>БСы обрабатывающие тип данных.</returns>
        BusinessServer[] GetBusinessServer(Type dataObjectType, ObjectStatus objectStatus, IDataService ds);
    }
}
