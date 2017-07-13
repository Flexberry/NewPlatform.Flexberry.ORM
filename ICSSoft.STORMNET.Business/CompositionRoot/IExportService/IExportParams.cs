using System;
using System.Collections.Generic;
using ICSSoft.STORMNET;
using ICSSoft.STORMNET.Business;
using ICSSoft.STORMNET.FunctionalLanguage;

namespace NewPlatform.Flexberry
{
    /// <summary>
    /// Интерфейс параметров для экспорта.
    /// </summary>
    public interface IExportParams
    {
        /// <summary>
        /// Представление для загрузки экспортируемых данных.
        /// </summary>
        View View { get; set; }

        /// <summary>
        /// Типы объектов для загрузки.
        /// </summary>
        Type[] DataObjectTypes { get; set; }

        /// <summary>
        /// Ограничение для загрузки экспортируемых данных.
        /// </summary>
        Function LimitFunction { get; set; }

        /// <summary>
        /// Сортировка колонок.
        /// </summary>
        ColumnsSortDef[] Ics { get; set; }

        /// <summary>
        /// Выделенные на WOLV объекты - будут выгружены только они, если не null или Count == 0.
        /// </summary>
        List<string> SelectedPrimaryKeys { get; set; }

        /// <summary>
        /// Свойство во ViewColumnProvider, которое отвечает за видимость свойства при экспорте.
        /// </summary>
        string ExcelNameProp { get; set; }

        /// <summary>
        /// Имя настройки в web.config в которой храниться путь для хранения xls файлов.
        /// </summary>
        string ConfigPathSetting { get; set; }

        /// <summary>
        /// Расширение файла.
        /// </summary>
        string FileExtension { get; set; }

        /// <summary>
        /// Коллекция имен свойств и детейлов, задающая порядок отображения колонок.
        /// Применяется, когда в представлении есть детейлы, и их нужно поставить не в конец.
        /// </summary>
        List<string> PropertiesOrder { get; set; }

        /// <summary>
        /// Заголовки столбцов (если параметр не указан, используются заголовки свойств).
        /// </summary>
        List<IHeaderCaption> HeaderCaptions { get; set; }

        /// <summary>
        /// Экспортировать детейлы в отдельные строки.
        /// </summary>
        bool? DetailsInSeparateRows { get; set; }

        /// <summary>
        /// Экспортировать детейлы в отдельные столбцы.
        /// </summary>
        bool? DetailsInSeparateColumns { get; set; }

        /// <summary>
        /// Максимальное количество экспортируемых объектов.
        /// </summary>
        /// <remarks>Отрицательные значения и 0 отключают ограничение.</remarks>
        int MaxObjectsCount { get; set; }
    }
}