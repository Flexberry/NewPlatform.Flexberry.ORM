namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы события сохранения объекта данных.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.SaveEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class SaveEventArgs : ICSSoft.STORMNET.UI.DataObjectPropEventArg
    {
        /// <summary>
        /// Аргументы события сохранения объекта данных.
        /// </summary>
        /// <param name="propertyname">имя свойства объекта данных при частичном редактировании объекта.</param>
        /// <param name="dataobject">редактируемый объект данных.</param>
        /// <param name="contpath">путь на форме-инициаторе.</param>
        public SaveEventArgs(string propertyname, DataObject dataobject, string contpath)
            : base(propertyname, dataobject, contpath)
        {
        }
    }

    /// <summary>
    /// Делегат события сохранения объекта данных.
    /// </summary>
    public delegate void SaveEventArgsHandler(object sender, ICSSoft.STORMNET.UI.SaveEventArgs e);
}
