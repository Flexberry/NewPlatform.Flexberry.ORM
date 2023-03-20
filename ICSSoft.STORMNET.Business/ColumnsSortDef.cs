namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Структура для установления сортировки на колонку.
    /// </summary>
    [Serializable]
    public struct ColumnsSortDef : ISerializable
    {
        private string colName;
        private SortOrder colSort;

        /// <summary>
        /// Имя колонки в представлении.
        /// </summary>
        public string Name
        {
            get { return colName; }
            set { colName = value; }
        }

        /// <summary>
        /// как сортировать.
        /// </summary>
        public SortOrder Sort
        {
            get { return colSort; }
            set { colSort = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name">Имя колонки в педставлении.</param>
        /// <param name="sort">как сортировать.</param>
        public ColumnsSortDef(string name, SortOrder sort)
        {
            colName = name;
            colSort = sort;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ColumnsSortDef(SerializationInfo info, StreamingContext context)
        {
            this.colName = info.GetString("colName");
            this.colSort = (SortOrder)info.GetValue("colSort", typeof(SortOrder));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("colName", this.colName);
            info.AddValue("colSort", this.colSort);
        }
    }
}
