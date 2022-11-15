namespace NewPlatform.Flexberry
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Данные для экспорта данных из ORM по частям.
    /// </summary>
    public abstract class BasePartedExportServiceParam : BasePartedExportParam
    {
        public IDataService DataService;
        public IExportParams Parameters;
        public DataObject[] Objs;
    }
}
