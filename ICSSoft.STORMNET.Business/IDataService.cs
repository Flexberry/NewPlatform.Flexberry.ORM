namespace ICSSoft.STORMNET.Business
{
    using System;
    using FunctionalLanguage;
    using FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Business.Audit;
    using Security;

    /// <summary>
    /// Интерфейс сервисов данных.
    /// Все реализации наследуются от него.
    /// </summary>
    public interface IDataService : ICloneable
    {
        /// <summary>
        /// Возвращает идентификатор инстанции сервиса данных.
        /// </summary>
        /// <returns>Идентификатор инстанции сервиса данных.</returns>
        Guid GetInstanceId();

        /// <summary>
        /// Строка настройки.
        /// </summary>
        string CustomizationString { get; set; }

        /// <summary>
        /// Структура определения <see cref="ICSSoft.STORMNET.TypeUsage"/>.
        /// </summary>
        ICSSoft.STORMNET.TypeUsage TypeUsage { get; set; }

        /// <summary>
        /// Менеджер полномочий.
        /// </summary>
        ISecurityManager SecurityManager { get; }

        /// <summary>
        /// Текущий сервис аудита.
        /// </summary>
        IAuditService AuditService { get; }

        /// <summary>
        /// Преобразовать значение в SQL строку.
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <param name="convertValue">делегат для преобразования констант.</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов.</param>
        /// <returns></returns>
        string FunctionToSql(
            SQLWhereLanguageDef sqlLangDef,
            Function function,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        void LoadObject(ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">Имя представления объекта.</param>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">Представление объекта.</param>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище.</param>
        void LoadObject(
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">Наименование представления.</param>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище.</param>
        void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">Представление объекта.</param>
        /// <param name="dobject">Объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">Проверять ли существование объекта в хранилище.</param>
        /// <param name="dataObjectCache">Кеш объектов данных.</param>
        void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject, DataObjectCache dataObjectCache);

        //-----------------------------------------------------

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять существование.</param>
        /// <param name="dataObjectCache">использовать кеш.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения View для типа.</param>
        // void LoadObject(
        //    ICSSoft.STORMNET.View dataObjectView,
        //    ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject,
        //    DataObjectCache dataObjectCache, ChangeViewForTypeDelegate changeViewForTypeDelegate);
        ////-----------------------------------------------------

        /// <summary>
        /// Загрузка объектов данных
        /// </summary>
        /// <param name="dataobjects">исходные объекты</param>
        /// <param name="dataObjectView">представлене</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        void LoadObjects(ICSSoft.STORMNET.DataObject[] dataobjects, ICSSoft.STORMNET.View dataObjectView, bool clearDataObject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>результат запроса.</returns>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="State">Состояние вычитки( для последующей дочитки ).</param>
        /// <returns></returns>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct,
            ref object State, DataObjectCache dataObjectCache);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="State">Состояние вычитки( для последующей дочитки).</param>
        /// <returns></returns>
        ICSSoft.STORMNET.DataObject[] LoadObjects(ref object State, DataObjectCache dataObjectCache);

        //-------LOAD separated string Objetcs ------------------------------------

        /// <summary>
        /// Загрузка без создания объектов.
        /// </summary>
        /// <param name="separator">разделитель в строках.</param>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>массив структур <see cref="ObjectStringDataView"/>.</returns>
        ObjectStringDataView[] LoadStringedObjectView(
            char separator,
            LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Загрузка без создания объектов.
        /// </summary>
        /// <param name="separator">разделитель в строках.</param>
        /// <param name="customizationStruct"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        ObjectStringDataView[] LoadStringedObjectView(
            char separator,
            LoadingCustomizationStruct customizationStruct,
            ref object State);

        /// <summary>
        ///
        /// </summary>
        ObjectStringDataView[] LoadStringedObjectView(ref object state);

        /// <summary>
        /// Корректное завершения операции порционного чтения LoadStringedObjectView.
        /// </summary>
        /// <param name="state">Параметр состояния загрузки (массив объектов).</param>
        void CompleteLoadStringedObjectView(ref object state);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ICSSoft.STORMNET.DataObject dobject);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, DataObjectCache dataObjectCache, bool alwaysThrowException);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ICSSoft.STORMNET.DataObject dobject, bool alwaysThrowException);

        void UpdateObjects(
            ref ICSSoft.STORMNET.DataObject[] objects, DataObjectCache dataObjectCache);

        void UpdateObjects(
            ref ICSSoft.STORMNET.DataObject[] objects, DataObjectCache dataObjectCache, bool alwaysThrowException);

        void UpdateObjects(
            ref ICSSoft.STORMNET.DataObject[] objects);

        void UpdateObjects(
            ref ICSSoft.STORMNET.DataObject[] objects, bool alwaysThrowException);

        /// <summary>
        /// возвращает количество объектов удовлетворяющих запросу.
        /// </summary>
        /// <param name="customizationStruct">что выбираем.</param>
        /// <returns></returns>
        int GetObjectsCount(LoadingCustomizationStruct customizationStruct);

        ObjectStringDataView[] LoadValues(
            char separator,
            LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        void LoadObject(ICSSoft.STORMNET.DataObject dobject);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">имя представления объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        void LoadObject(
            string dataObjectViewName, ICSSoft.STORMNET.DataObject dobject);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление объекта.</param>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        void LoadObject(
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectViewName">наименование представления.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        void LoadObject(
            string dataObjectViewName,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject);

        /// <summary>
        /// Загрузка одного объекта данных.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="dobject">бъект данных, который требуется загрузить.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        /// <param name="checkExistingObject">проверять ли существование объекта в хранилище.</param>
        void LoadObject(
            ICSSoft.STORMNET.View dataObjectView,
            ICSSoft.STORMNET.DataObject dobject, bool clearDataObject, bool checkExistingObject);

        //-----------------------------------------------------

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="dataobjects">исходные объекты.</param>
        /// <param name="dataObjectView">представлене.</param>
        /// <param name="clearDataObject">Флаг, указывающий на необходмость очистки объекта перед вычиткой (<see cref="DataObject.Clear"/>).</param>
        void LoadObjects(ICSSoft.STORMNET.DataObject[] dataobjects,
            ICSSoft.STORMNET.View dataObjectView, bool clearDataObject);

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View dataObjectView);

        /// <summary>
        /// Загрузка объектов данных по массиву представлений.
        /// </summary>
        /// <param name="dataObjectViews">массив представлений.</param>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View[] dataObjectViews);

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">массив структур.</param>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct[] customizationStructs);

        /// <summary>
        /// Загрузка объектов данных по представлению.
        /// </summary>
        /// <param name="dataObjectView">представление.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения.</param>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View dataObjectView, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных по массиву представлений.
        /// </summary>
        /// <param name="dataObjectViews">массив представлений.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения.</param>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            ICSSoft.STORMNET.View[] dataObjectViews, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных по массиву структур.
        /// </summary>
        /// <param name="customizationStructs">массив структур.</param>
        /// <param name="changeViewForTypeDelegate">делегат для изменения.</param>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct[] customizationStructs, ChangeViewForTypeDelegate changeViewForTypeDelegate);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns>результат запроса.</returns>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="customizationStruct">настроичная структура для выборки<see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="State">Состояние вычитки( для последующей дочитки ).</param>
        /// <returns></returns>
        ICSSoft.STORMNET.DataObject[] LoadObjects(
            LoadingCustomizationStruct customizationStruct,
            ref object State);

        /// <summary>
        /// Загрузка объектов данных.
        /// </summary>
        /// <param name="State">Состояние вычитки( для последующей дочитки).</param>
        /// <returns></returns>
        ICSSoft.STORMNET.DataObject[] LoadObjects(ref object State);

        //-------LOAD separated string Objetcs ------------------------------------

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject);

        /// <summary>
        /// Обновление объекта данных.
        /// </summary>
        /// <param name="dobject">объект данных, который требуется обновить.</param>
        void UpdateObject(ref ICSSoft.STORMNET.DataObject dobject, bool alwaysThrowException);
    }
}
