namespace NewPlatform.Flexberry
{
    using ICSSoft.STORMNET.Business;

    /// <summary>
    /// Данные для экспорта данных из ORM по частям.
    /// </summary>
    public abstract class BasePartedExportStringedObjectViewServiceParam : BasePartedExportParam
    {
        public IExportParams Parameters;
        public ObjectStringDataView[] Objs;
    }
}
