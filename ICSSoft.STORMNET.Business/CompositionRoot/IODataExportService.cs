namespace NewPlatform.Flexberry
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using System.Collections.Specialized;

    /// <summary>
    /// Data export service from ODataService.
    /// </summary>
    public interface IODataExportService
    {
        /// <summary>
        /// Creates a file for exporting data from ODataService.
        /// </summary>
        /// <param name="dataService">ORM data service.</param>
        /// <param name="parameters">Export options.</param>
        /// <param name="objs">Objects for export.</param>
        /// <param name="queryParams">Parameters in the query string to ODataService.</param>
        /// <returns>Returns the export file as MemoryStream.</returns>
        MemoryStream CreateExportStream(IDataService dataService, IExportParams parameters, DataObject[] objs, NameValueCollection queryParams);
    }
}
