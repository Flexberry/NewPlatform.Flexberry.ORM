namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Общий предок агрументов событий форм.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.ContActionEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class ContActionEventArgs : System.EventArgs
    {
        private object ftag = null;

        private string fcontpath;

        /// <summary>
        /// Общий предок агрументов событий форм.
        /// </summary>
        /// <param name="contpath"></param>
        public ContActionEventArgs(string contpath)
        {
            this.fcontpath = contpath;
        }

        /// <summary>
        /// Общий предок агрументов событий форм.
        /// </summary>
        /// <param name="contpath"></param>
        /// <param name="tag"></param>
        public ContActionEventArgs(string contpath, object tag)
        {
            this.fcontpath = contpath;
            this.ftag = tag;
        }

        /// <summary>
        /// путь на форме к контролу, который инициировал событие.
        /// </summary>
        public virtual string contpath
        {
            get
            {
                string result = this.fcontpath;
                return result;
            }

            set
            {
                this.fcontpath = value;
            }
        }

        /// <summary>
        /// дополнительный параметр.
        /// </summary>
        public virtual object tag
        {
            get
            {
                return ftag;
            }

            set
            {
                this.ftag = value;
            }
        }
    }

    public delegate void ContActionEventArgsHandler(object sender, ICSSoft.STORMNET.UI.ContActionEventArgs e);
}
