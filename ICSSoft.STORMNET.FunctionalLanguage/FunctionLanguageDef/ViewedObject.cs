namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// Нехранимая реализация DataObject с полями Caption и StringedView для различных классов языка задания ограничений.
    /// </summary>
    [NotStored]
    public abstract class ViewedObject : ICSSoft.STORMNET.DataObject
    {
        private string fieldStringedView;
        private string fieldCaption;

        // public Image ImagedView{get{return fieldImagedView;}set{fieldImagedView=value;}}

        /// <summary>
        /// Строковое представление.
        /// </summary>
        public virtual string StringedView
        {
            get { return fieldStringedView; }
            set { fieldStringedView = value; }
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public virtual string Caption
        {
            get { return fieldCaption; }
            set { fieldCaption = value; }
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        public ViewedObject()
        {
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="objStringedView"></param>
        /// <param name="objImagedView"></param>
        /// <param name="Caption"></param>
        public ViewedObject(string objStringedView, string Caption)
        {
            StringedView = objStringedView;
            this.Caption = Caption;
        }
    }
}
