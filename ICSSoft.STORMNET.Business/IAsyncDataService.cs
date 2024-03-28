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
        /// Получение количества объектов удовлетворяющих запросу.
        /// </summary>
        /// <param name="customizationStruct">Объект <see cref="LoadingCustomizationStruct"/> для спецификации запроса.</param>
        /// <returns>
        /// <see cref="Task"/> - результат операции содержит количество объектов.
        /// </returns>
        Task<int> GetObjectsCountAsync(LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Загрузка одного объекта данных <i>(атрибуты для <paramref name="dataObject"/> загружаются в процессе работы)</i>.
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у загружаемого объекта обновляются в процессе работы.</i></remarks>
        /// <param name="dataObject">Объект данных, который требуется загрузить.</param>
        /// <param name="dataObjectView">Представление, по которому загружается объект. Если <see langword="null"/>, будут загружены все атрибуты объекта, без детейлов (см. <see cref="View.ReadType.OnlyThatObject"/>).</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Вызывать исключение если объекта нет в хранилище.</param>
        /// <param name="dataObjectCache">Кэш объектов (если <see langword="null"/>, будет использован временный кеш).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjectAsync(DataObject dataObject, View dataObjectView = null, bool clearDataObject = true, bool checkExistingObject = true, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Загрузка нескольких объектов данных.
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у обрабатываемых объектов обновляются в процессе работы.</i></remarks>
        /// <param name="dataObjects">Объекты данных, которые требуется загрузить.</param>
        /// <param name="dataObjectView">Представление, по которому загружаются объекты.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="dataObjectCache">Кэш объектов (если <see langword="null"/>, будет использован временный кеш).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task LoadObjectsAsync(DataObject[] dataObjects, View dataObjectView, bool clearDataObject = true, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Загрузка нескольких объектов данных (с помощью представления).
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у обрабатываемых объектов обновляются в процессе работы.</i></remarks>
        /// <param name="dataObjectView">Представление, по которому загружаются объекты.</param>
        /// <param name="dataObjectCache">Кэш объектов (если <see langword="null"/>, будет использован временный кеш).</param>
        /// <returns><see cref="Task"/> - результат операции содержит загруженные объекты данных.</returns>
        Task<DataObject[]> LoadObjectsAsync(View dataObjectView, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Загрузка нескольких объектов данных (с помощью LCS).
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у обрабатываемых объектов обновляются в процессе работы.</i></remarks>
        /// <param name="customizationStruct">Структура (LCS) для загрузки объектов.</param>
        /// <param name="dataObjectCache">Кэш объектов (если <see langword="null"/>, будет использован временный кеш).</param>
        /// <returns><see cref="Task"/> - результат операции содержит загруженные объекты данных.</returns>
        Task<DataObject[]> LoadObjectsAsync(LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Сохранение объекта данных.
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у обрабатываемых объектов обновляются в процессе работы.</i></remarks>
        /// <param name="dataObject">Объект данных, который требуется обновить.</param>
        /// <param name="alwaysThrowException">Останавливать метод при возникновении ошибки (<see langword="false"/> - часть объектов обновится несмотря на ошибки).</param>
        /// <param name="dataObjectCache">Кэш объектов (если <see langword="null"/>, будет использован временный кеш).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task UpdateObjectAsync(DataObject dataObject, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null);

        /// <summary>
        /// Сохранение объектов данных.
        /// </summary>
        /// <remarks><i>Атрибуты loadingState и status у обрабатываемых объектов обновляются в процессе работы.</i></remarks>
        /// <param name="objects">Объекты данных, которые требуется обновить.</param>
        /// <param name="alwaysThrowException">Останавливать метод при возникновении ошибки (<see langword="false"/> - часть объектов обновится несмотря на ошибки).</param>
        /// <param name="dataObjectCache">Кэш объектов (если <see langword="null"/>, будет использован временный кеш).</param>
        /// <returns>Объект <see cref="Task"/>, представляющий асинхронную операцию.</returns>
        Task UpdateObjectsAsync(DataObject[] objects, bool alwaysThrowException = false, DataObjectCache dataObjectCache = null);
    }
}
