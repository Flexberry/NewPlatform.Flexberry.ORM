namespace ICSSoft.STORMNET.UI
{
    using System;

    /// <summary>
    /// Аргументы для события создания.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.NewEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class NewEventArgs : ICSSoft.STORMNET.UI.ContActionEventArgs
    {
        private Type ftype;

        private object ftag;

        /// <summary>
        /// Аргументы для события создания.
        /// </summary>
        /// <param name="type">тип создаваемого объекта данных.</param>
        /// <param name="contpath">путь на форме-инициаторе.</param>
        public NewEventArgs(Type type, string contpath)
            : base(contpath)
        {
            this.type = type;
        }

        /// <summary>
        /// Аргументы для события создания.
        /// </summary>
        /// <param name="type">тип создаваемого объекта данных.</param>
        /// <param name="contpath">путь на форме-инициаторе.</param>
        /// <param name="tag"> </param>
        public NewEventArgs(Type type, string contpath, object tag)
            : base(contpath)
        {
            this.type = type;
            this.tag = tag;
        }

        /// <summary>
        /// Тип создаваемого объекта данных.
        /// </summary>
        public virtual Type type
        {
            get
            {
                Type result = this.ftype;
                return result;
            }

            set
            {
                this.ftype = value;
            }
        }

        /// <summary>
        /// тег.
        /// </summary>
        public virtual object tag
        {
            get
            {
                return ftag;
            }

            set
            {
                ftag = value;
            }
        }
    }

    /// <summary>
    /// Делегат для событий создания объектов.
    /// </summary>
    public delegate void NewEventArgsHandler(object sender, ICSSoft.STORMNET.UI.NewEventArgs e);
}
