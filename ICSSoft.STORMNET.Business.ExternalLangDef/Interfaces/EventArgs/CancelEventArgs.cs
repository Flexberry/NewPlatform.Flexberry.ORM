namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы к событию отмены действия инициатора.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.CancelEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class CancelEventArgs : ICSSoft.STORMNET.UI.DataObjectPropEventArg
    {
        /// <summary>
        /// Аргументы к событию отмены действия инициатора.
        /// </summary>
        /// <param name="propertyname">имя свойства, если передавать не требуется - "".</param>
        /// <param name="dataobject">объект данных.</param>
        /// <param name="contpath">некоторый путь на форме-инициаторе, для идентификации объекта, в случае, когда форма редактирует одновременно несколько объектов данных.</param>
        public CancelEventArgs(string propertyname, DataObject dataobject, string contpath)
            : base(propertyname, dataobject, contpath)
        {
        }
    }

    /// <summary>
    /// Делегат для события отмены действия инициатора.
    /// </summary>
    public delegate void CancelEventArgsHandler(object sender, ICSSoft.STORMNET.UI.CancelEventArgs e);
}
