namespace NewPlatform.Flexberry.ORM
{
    using System;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Интерфейс асинхронного датасервиса.
    /// </summary>
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
        /// Загрузка одного объекта данных <i>(атрибуты <paramref name="dataObject"/> загружается в процессе работы)</i>.
        /// </summary>
        /// <param name="dataObject">Объект данных, который требуется загрузить.</param>
        /// <param name="dataObjectView">Представление, по которому загружается объект. Если null, будут загружены все атрибуты объекта, без детейлов (см. <see cref="View.ReadType.OnlyThatObject"/>).</param>
        /// <param name="clearDataObject">Очистить объект перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Вызывать исключение если объекта нет в хранилище.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjectAsync(DataObject dataObject, View dataObjectView = null, bool clearDataObject = true, bool checkExistingObject = true, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Загрузка нескольких объектов данных <i>(атрибуты <paramref name="dataObjects"/> загружаются в процессе работы)</i>.
        /// </summary>
        /// <param name="dataObjects">Объекты данных, которые требуется загрузить.</param>
        /// <param name="dataObjectView">Представление, по которому загружаются объекты.</param>
        /// <param name="clearDataObject">Очистить объект перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjectsAsync(DataObject[] dataObjects, View dataObjectView, bool clearDataObject = true, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Загрузка нескольких объектов данных (с помощью представления).
        /// </summary>
        /// <param name="dataObjectView">Представление, по которому загружаются объекты.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию. Результат содержит коллекцию объектов данных.</returns>
        Task<DataObject[]> LoadObjectsAsync(View dataObjectView, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Загрузка нескольких объектов данных (с помощью LCS).
        /// </summary>
        /// <param name="customizationStruct">Структура (LCS) для загрузки объектов.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <returns>Асинхронная операция <see cref="Task"/>. Результат операции содержит загруженные объекты данных.</returns>
        Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Сохранение объекта данных <i>(атрибуты состояния <paramref name="dataObject"/> (state, loading) изменяются в процессе работы)</i>.
        /// </summary>
        /// <param name="dataObject">Объект данных, который требуется обновить.</param>
        /// <param name="alwaysThrowException">.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task UpdateObjectAsync(DataObject dataObject, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Сохранение объектов данных <i>(атрибуты состояния объектов <paramref name="objects"/> (state и loading) изменяются в процессе работы</i>.
        /// </summary>
        /// <param name="objects">Объекты данных, которые требуется обновить.</param>
        /// <param name="alwaysThrowException">.</param>
        /// <param name="dataObjectCache">Кэш объектов (если null, будет использован временный кеш, созданный внутри метода).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task<DataObject[]> UpdateObjectsAsync(DataObject[] objects, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null);
    }
}
