namespace NewPlatform.Flexberry
{
    using System.IO;
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Data export service with ObjectStringDataView.
    /// </summary>
    public interface IExportStringedObjectViewService
    {
        /// <summary>
        /// Creates a file for exporting data from ODataService.
        /// </summary>
        /// <param name="parameters">Export options.</param>
        /// <param name="objs">Objects for export.</param>
        /// <returns>Returns the export file as MemoryStream.</returns>
        MemoryStream CreateExportStream(IExportParams parameters, ObjectStringDataView[] objs);
    }
}
