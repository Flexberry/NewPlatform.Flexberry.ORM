namespace NewPlatform.Flexberry
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using System.IO;

    /// <summary>
    /// Данные для экспорта данных из ORM по частям.
    /// </summary>
    public abstract class BasePartedExportParam
    {
        /// <summary>
        /// Поток данных с файлом экспорта из ORM.
        /// </summary>
        public MemoryStream ExportMemoryStream;

        public abstract int GetPortionSize();

        public abstract void SetNextPortionOfData(DataObject[] data);

        public abstract void SetNextPortionOfStringedData(ObjectStringDataView[] data);
    }
}
