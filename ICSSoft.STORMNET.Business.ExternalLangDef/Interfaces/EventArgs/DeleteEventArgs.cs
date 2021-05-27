namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы для события удаления объекта.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.DeleteEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class DeleteEventArgs : ICSSoft.STORMNET.UI.ContActionEventArgs
    {
        private DataObject[] m_arrObjects;

        /// <summary>
        /// Аргументы для события удаления объектов данных.
        /// </summary>
        /// <param name="dataobjects">объекты данных.</param>
        /// <param name="contpath">некоторый путь на форме-инициаторе.</param>
        public DeleteEventArgs(DataObject[] dataobjects, string contpath)
            : base(contpath)
        {
            m_arrObjects = dataobjects;
        }

        /// <summary>
        /// Объекты данных.
        /// </summary>
        public DataObject[] DataObjects
        {
            get
            {
                return m_arrObjects;
            }

            set
            {
                m_arrObjects = value;
            }
        }
    }

    /// <summary>
    /// Делегат для событий удаления объектов данных.
    /// </summary>
    public delegate void DeleteEventArgsHandler(object sender, ICSSoft.STORMNET.UI.DeleteEventArgs e);
}
