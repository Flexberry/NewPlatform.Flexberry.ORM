namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// параметры событий форм с передачей объекта данных и имени свойства.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.DataObjectPropEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class DataObjectPropEventArg : ICSSoft.STORMNET.UI.DataObjectEventArgs
    {
        private string fpropertyname;

        /// <summary>
        /// параметры событий форм с передачей объекта данных и имени свойства.
        /// </summary>
        /// <param name="propertyname"></param>
        /// <param name="dataobject"></param>
        /// <param name="contpath"></param>
        public DataObjectPropEventArg(string propertyname, DataObject dataobject, string contpath)
            : base(dataobject, contpath)
        {
            this.propertyname = propertyname;
        }

        /// <summary>
        /// параметры событий форм с передачей объекта данных и имени свойства.
        /// </summary>
        /// <param name="propertyname"></param>
        /// <param name="dataobject"></param>
        /// <param name="contpath"></param>
        /// <param name="tag"></param>
        public DataObjectPropEventArg(string propertyname, DataObject dataobject, string contpath, object tag)
            : base(dataobject, contpath, tag)
        {
            this.propertyname = propertyname;
        }

        /// <summary>
        /// имя свойства.
        /// </summary>
        public virtual string propertyname
        {
            get
            {
                string result = this.fpropertyname;
                return result;
            }

            set
            {
                this.fpropertyname = value;
            }
        }
    }

    public delegate void DataObjectPropEventArgHandler(object sender, ICSSoft.STORMNET.UI.DataObjectPropEventArg e);
}
