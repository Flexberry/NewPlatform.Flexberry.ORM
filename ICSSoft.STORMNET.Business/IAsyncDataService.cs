namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;

    public interface IAsyncDataService : ICloneable
    {
        DataServiceOptions Options { get; set; }

        /// <summary>
        /// Менеджер полномочий.
        /// </summary>
        ISecurityManager SecurityManager { get; }

        /// <summary>
        /// Текущий сервис аудита.
        /// </summary>
        IAuditService AuditService { get; }

        /// <summary>
        /// Сервис кэша.
        /// </summary>
        ICacheService CacheService { get; }

        /// <summary>
        /// Возвращает количество объектов удовлетворяющих запросу.
        /// </summary>
        /// <param name="customizationStruct">Что выбираем.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит количество объектов.
        /// </returns>
        Task<int> GetObjectsCount(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Догрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Очищать ли объект.</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObject(DataObject dobject, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// Догрузка одного объекта данных по представлению.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="dataObjectView">Представление.</param>
        /// <param name="clearDataObject">Очищать ли объект.</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObject(DataObject dobject, View dataObjectView, bool clearDataObject = true, bool checkExistingObject = true);

        /// <summary>
        /// Догрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataobjects">Исходные объекты.</param>
        /// <param name="dataObjectView">Представлене.</param>
        /// <param name="clearDataobject">Очищать ли существующие.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjects(IEnumerable<DataObject> dataobjects, View dataObjectView, bool clearDataobject = true);

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">Представление.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(View dataObjectView);

        /// <summary>
        /// Загрузка объектов данных по нескольким представлениям.
        /// </summary>
        /// <param name="dataObjectViews">Массив представлений.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<View> dataObjectViews);

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">Массив структур.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<LoadingCustomizationStruct> customizationStructs);

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных по массиву представлений.
        /// </summary>
        /// <param name="dataObjectViews">массив представлений.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<View> dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">массив структур.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения.</param>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(IEnumerable<LoadingCustomizationStruct> customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>результат запроса.</returns>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="State">Состояние вычитки( для последующей дочитки ).</param>
        /// <returns></returns>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(LoadingCustomizationStruct customizationStruct, ref object State);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="State">Состояние вычитки( для последующей дочитки).</param>
        /// <returns></returns>
        /// <returns>
        /// Объект <see cref="Task"/>, представляющий асинхронную операцию.
        /// Результат содержит коллекцию объектов данных.
        /// </returns>
        Task<IEnumerable<DataObject>> LoadObjects(ref object State);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task UpdateObject(ref DataObject dobject);

        /// <summary>
        /// Обновление объектов данных.
        /// </summary>
        /// <param name="objects">объекты данных, которые требуется обновить.</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task UpdateObjects(ref IEnumerable<DataObject> objects);
    }
}
