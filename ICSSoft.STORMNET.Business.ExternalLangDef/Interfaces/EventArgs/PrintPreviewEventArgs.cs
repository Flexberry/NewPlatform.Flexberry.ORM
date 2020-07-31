namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы события предварительного просмотра.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.PrintPreviewEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class PrintPreviewEventArgs : System.EventArgs
    {
        /// <summary>
        /// объекты данных для печати.
        /// </summary>
        private DataObject[] m_arrObjects;

        /// <summary>
        /// путь к контролу на форме, который инициировал печать.
        /// </summary>
        private string m_sContPath;

        /// <summary>
        /// имя свойства.
        /// </summary>
        private string m_sPropertyName;

        /// <summary>
        /// Аргументы события предварительного просмотра.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="contpath"></param>
        /// <param name="propertyname"></param>
        public PrintPreviewEventArgs(DataObject[] objects, string contpath, string propertyname)
        {
            m_arrObjects = objects;

            m_sContPath = contpath;

            m_sPropertyName = propertyname;
        }

        /// <summary>
        /// объекты данных для печати.
        /// </summary>
        public DataObject[] objects
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

        /// <summary>
        /// путь к контролу на форме, который инициировал печать.
        /// </summary>
        public string contpath
        {
            get
            {
                return m_sContPath;
            }

            set
            {
                m_sContPath = value;
            }
        }

        /// <summary>
        /// имя свойства.
        /// </summary>
        public string propertyname
        {
            get
            {
                return m_sPropertyName;
            }

            set
            {
                m_sPropertyName = value;
            }
        }
    }

    /// <summary>
    /// Делегат для событий предварительного просмотра.
    /// </summary>
    public delegate void PrintPreviewEventArgsHandler(object sender, ICSSoft.STORMNET.UI.PrintPreviewEventArgs e);
}
