namespace NewPlatform.Flexberry
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using System.Collections.Specialized;

    /// <summary>
    /// Данные для экспорта данных из ORM по частям.
    /// </summary>
    public abstract class BasePartedOdataExportServiceParam : BasePartedExportParam
    {
        public IDataService DataService;
        public IExportParams Parameters;
        public DataObject[] Objs;
        public NameValueCollection QueryParams;
    }
}
