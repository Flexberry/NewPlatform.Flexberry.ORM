namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Общий предок параметров событий форм с передаче объектов данных.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.DataObjectEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class DataObjectEventArgs : ICSSoft.STORMNET.UI.ContActionEventArgs
    {
        private DataObject fdataobject;

        /// <summary>
        /// Общий предок параметров событий форм с передаче объектов данных.
        /// </summary>
        /// <param name="dataobject"></param>
        /// <param name="contpath"></param>
        public DataObjectEventArgs(DataObject dataobject, string contpath)
            : base(contpath)
        {
            this.dataobject = dataobject;
        }

        /// <summary>
        /// Общий предок параметров событий форм с передаче объектов данных.
        /// </summary>
        /// <param name="dataobject"></param>
        /// <param name="contpath"></param>
        /// <param name="tag"></param>
        public DataObjectEventArgs(DataObject dataobject, string contpath, object tag)
            : base(contpath, tag)
        {
            this.dataobject = dataobject;
        }

        /// <summary>
        /// объект данных.
        /// </summary>
        public virtual DataObject dataobject
        {
            get
            {
                DataObject result = this.fdataobject;
                return result;
            }

            set
            {
                this.fdataobject = value;
            }
        }
    }

    public delegate void DataObjectEventArgsHandler(object sender, ICSSoft.STORMNET.UI.DataObjectEventArgs e);
}
