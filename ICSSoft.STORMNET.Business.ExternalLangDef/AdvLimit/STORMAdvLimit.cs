namespace ICSSoft.STORMNET.Windows.Forms
{
    /// <summary>
    /// Объектное представление сохраняемого в базе ограничения. Не содержит логики по десериализации. Сам класс знает только сериализованное Value. Десериализуется в своё нехранимое свойство AdvLimit при помощи AdvLimitComponent.
    /// </summary>
    [AutoAltered()]
    [View("V", new string[]
    {
        "User",
                                "Published",
                                "Module",
                                "Name",
                                "Value",
                                "HotKeyData",
    })]
    public partial class STORMAdvLimit : DataObject
    {
        private string fName;
        private string fUser;
        private string fModule;
        private string fValue;

        /// <summary>
        /// десериализованное AdvansedLimit.
        /// </summary>
        public AdvansedLimit fAdvLimit;
        private int fHotKeyData;

        /// <summary>
        /// Десериализованное AdvansedLimit. Десериализацией занимается AdvLimitComponent.
        /// </summary>
        [NotStored]
        public AdvansedLimit AdvLimit
        {
            get { return fAdvLimit; }
            set { fAdvLimit = value; }
        }

        /// <summary>
        /// горячая клавиша.
        /// </summary>
        public int HotKeyData
        {
            get
            {
                return fHotKeyData;
            }

            set
            {
                fHotKeyData = value;
            }
        }

        /// <summary>
        /// имя.
        /// </summary>
        public virtual string Name
        {
            get
            {
                string result = this.fName;
                return result;
            }

            set
            {
                this.fName = value;
            }
        }

        /// <summary>
        /// пользователь, чьё это ограничение.
        /// </summary>
        public virtual string User
        {
            get
            {
                string result = this.fUser;
                return result;
            }

            set
            {
                this.fUser = value;
            }
        }

        /// <summary>
        /// модуль (генерируется AdvLimitComponent-ом, содержит информацию о форме и о списковом контроле).
        /// </summary>
        public virtual string Module
        {
            get
            {
                string result = this.fModule;
                return result;
            }

            set
            {
                this.fModule = value;
            }
        }

        /// <summary>
        /// Сериализованное ограничение.
        /// </summary>
        public virtual string Value
        {
            get
            {
                string result = this.fValue;
                return result;
            }

            set
            {
                this.fValue = value;
            }
        }
    }
}
