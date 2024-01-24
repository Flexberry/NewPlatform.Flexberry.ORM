namespace NewPlatform.Flexberry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;

    public class ExportParams : IExportParams
    {
        /// <summary>
        /// Представление для загрузки экспортируемых данных.
        /// </summary>
        public View View { get; set; }

        /// <summary>
        /// Типы объектов для загрузки.
        /// </summary>
        public Type[] DataObjectTypes { get; set; }

        /// <summary>
        /// Ограничение для загрузки экспортируемых данных.
        /// </summary>
        public Function LimitFunction { get; set; }

        /// <summary>
        /// Сортировка колонок.
        /// </summary>
        public ColumnsSortDef[] Ics { get; set; }

        /// <summary>
        /// Выделенные на WOLV объекты - будут выгружены только они, если не null или Count == 0.
        /// </summary>
        public List<string> SelectedPrimaryKeys { get; set; }

        /// <summary>
        /// Свойство во ViewColumnProvider, которое отвечает за видимость свойства при экспорте.
        /// </summary>
        public string ExcelNameProp { get; set; }

        /// <summary>
        /// Имя настройки в web.config в которой храниться путь для хранения xls файлов.
        /// </summary>
        public string ConfigPathSetting { get; set; }

        /// <summary>
        /// Расширение файла.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Коллекция имен свойств и детейлов, задающая порядок отображения колонок.
        /// Применяется, когда в представлении есть детейлы, и их нужно поставить не в конец.
        /// </summary>
        public List<string> PropertiesOrder { get; set; }

        /// <summary>
        /// Заголовки столбцов.
        /// </summary>
        public List<IHeaderCaption> HeaderCaptions { get; set; }

        /// <summary>
        /// Экспортировать детейлы в отдельные строки.
        /// </summary>
        public bool? DetailsInSeparateRows { get; set; }

        /// <summary>
        /// Экспортировать детейлы в отдельные столбцы.
        /// </summary>
        public bool? DetailsInSeparateColumns { get; set; }

        /// <summary>
        /// Максимальное количество экспортируемых объектов.
        /// </summary>
        /// <remarks>Отрицательные значения и 0 отключают ограничение.</remarks>
        public int MaxObjectsCount { get; set; }
    }
}
