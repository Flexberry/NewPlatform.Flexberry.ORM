namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы для события редактирования объекта у инициатора.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.EditEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class EditEventArgs : ICSSoft.STORMNET.UI.DataObjectPropEventArg
    {
        /// <summary>
        /// Аргументы для события редактирования объекта у инициатора.
        /// </summary>
        /// <param name="propertyname">Имя свойства (в случае редактирования свойства объекта).</param>
        /// <param name="dataobject">объект данных, подлежащий редактированию.</param>
        /// <param name="contpath">некоторый путь на форме-инициаторе.</param>
        public EditEventArgs(string propertyname, DataObject dataobject, string contpath)
            : base(propertyname, dataobject, contpath)
        {
        }

        /// <summary>
        /// Аргументы для события редактирования объекта у инициатора.
        /// </summary>
        /// <param name="propertyname">Имя свойства (в случае редактирования свойства объекта).</param>
        /// <param name="dataobject">объект данных, подлежащий редактированию.</param>
        /// <param name="contpath">некоторый путь на форме-инициаторе.</param>
        public EditEventArgs(string propertyname, DataObject dataobject, string contpath, object tag)
            : base(propertyname, dataobject, contpath, tag)
        {
        }
    }

    /// <summary>
    /// Делегат для событий редактирования объекта.
    /// </summary>
    public delegate void EditEventArgsHandler(object sender, ICSSoft.STORMNET.UI.EditEventArgs e);
}
