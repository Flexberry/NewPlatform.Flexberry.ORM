namespace NewPlatform.Flexberry.ORM
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    public interface IAsyncDataService : ICloneable
    {
        /// <summary>
        /// Возвращает количество объектов удовлетворяющих запросу.
        /// </summary>
        /// <param name="customizationStruct">Что выбираем.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит количество объектов.
        /// </returns>
        Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Догрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Очищать ли объект.</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище (вызывать исключение если объекта нет).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjectAsync(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// Догрузка одного объекта данных по представлению.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="dataObjectView">Представление.</param>
        /// <param name="clearDataObject">Очищать загруженные объекты (см. <see cref="ICSSoft.STORMNET.DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище (вызывать исключение если объекта нет).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjectAsync(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// Загрузка объектов данных по нескольким представлениям.
        /// </summary>
        /// <param name="dataObjectViews">Массив представлений.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<DataObject[]> LoadObjectsAsync(View[] dataObjectViews);

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">Массив структур.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct[] customizationStructs);

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">Представление.</param>
        /// <param name="changeViewForTypeDelegate">Делегат, возвращающий представление в зависимости от типа объекта (используется когда получаемый список DataObject'ов может состоять из нескольких типов - напр. при наследовании DataObject'ов).</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        //Task<DataObject[]> LoadObjectsAsync(View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных по массиву представлений.
        /// </summary>
        /// <param name="dataObjectViews">Массив представлений.</param>
        /// <param name="changeViewForTypeDelegate">Делегат, возвращающий представление в зависимости от типа объекта (используется когда получаемый список DataObject'ов может состоять из нескольких типов - напр. при наследовании DataObject'ов).</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        //Task<DataObject[]> LoadObjectsAsync(View[] dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">Массив структур.</param>
        /// <param name="changeViewForTypeDelegate">Делегат, возвращающий представление в зависимости от типа объекта (используется когда получаемый список DataObject'ов может состоять из нескольких типов - напр. при наследовании DataObject'ов).</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        //Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct[] customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">Настроечная структура для выборки <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется обновить.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task<DataObject> UpdateObjectAsync(DataObject dobject);

        /// <summary>
        /// Обновление объектов данных.
        /// </summary>
        /// <param name="objects">Объекты данных, которые требуется обновить.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task<DataObject[]> UpdateObjectsAsync(DataObject[] objects);
    }
}
