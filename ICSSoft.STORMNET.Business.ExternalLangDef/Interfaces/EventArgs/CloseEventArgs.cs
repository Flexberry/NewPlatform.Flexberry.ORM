namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы к событию закрытия UI-зависимой Win-формы.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.CloseEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class CloseEventArgs : System.EventArgs
    {
        private bool fCancel;

        /// <summary>
        /// Аргументы к событию закрытия UI-зависимой Win-формы.
        /// </summary>
        /// <param name="Cancel">true вызывает отмену закрытия формы.</param>
        public CloseEventArgs(bool Cancel)
        {
            this.fCancel = Cancel;
        }

        /// <summary>
        /// Отмена закрытия формы.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return fCancel;
            }

            set
            {
                fCancel = value;
            }
        }
    }

    /// <summary>
    /// Делегат для события закрытия UI-зависимой Win-формы.
    /// </summary>
    public delegate void CloseEventArgsHandler(object sender, ICSSoft.STORMNET.UI.CloseEventArgs e);

    /// <summary>
    /// Делегат для события, происходящего перед закрытием UI-зависимой Win-формы.
    /// </summary>
    public delegate void BeforeCloseEventHandler(object sender, CloseEventArgs e);
}
