namespace NewPlatform.Flexberry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Сервис экспорта данных из ORM.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Создаёт файл экспорта  данных из ORM.
        /// </summary>
        /// <param name="dataService">Сервис данных ORM.</param>
        /// <param name="parameters">Параметры экпорта.</param>
        /// <param name="objs">Объекты для экспорта.</param>
        /// <returns>Возвращает файл экспорта в виде MemoryStream.</returns>
        MemoryStream CreateExportStream(IDataService dataService, IExportParams parameters, DataObject[] objs);
    }
}
