namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Дополнительная колоночка
    /// </summary>
    [Serializable]
    public struct AdvansedColumn : ISerializable
    {
        #region Constants and Fields

        /// <summary>
        /// The expression.
        /// </summary>
        private string expression;

        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        /// <summary>
        /// The storage source modification.
        /// </summary>
        private string storageSourceModification;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvansedColumn"/> struct.
        /// </summary>
        /// <param name="Name">
        /// The name.
        /// </param>
        /// <param name="Expression">
        /// The expression.
        /// </param>
        /// <param name="StorageSourceModification">
        /// The storage source modification.
        /// </param>
        public AdvansedColumn(string Name, string Expression, string StorageSourceModification)
        {
            name = Name;
            expression = Expression;
            storageSourceModification = StorageSourceModification;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvansedColumn"/> struct.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public AdvansedColumn(SerializationInfo info, StreamingContext context)
        {
            name = info.GetString("name");
            expression = info.GetString("expression");
            storageSourceModification = info.GetString("storageSourceModification");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Выражение
        /// </summary>
        public string Expression
        {
            get
            {
                return expression;
            }

            set
            {
                expression = value;
            }
        }

        /// <summary>
        /// Наименование колонки
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// ????????????????????????????
        /// </summary>
        public string StorageSourceModification
        {
            get
            {
                return storageSourceModification;
            }

            set
            {
                storageSourceModification = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get object data.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("expression", expression);
            info.AddValue("storageSourceModification", storageSourceModification);
        }

        #endregion
    }
}