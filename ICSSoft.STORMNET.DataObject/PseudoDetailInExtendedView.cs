namespace ICSSoft.STORMNET
{
    using System;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.Exceptions;

    /// <summary>
    /// Псевдодетейл для добавления в упорядоченный список ExtendedView.
    /// </summary>
    [Serializable]
    public struct PseudoDetailInExtendedView : ISerializable
    {
        private string _pseudoDetailViewName;

        private Type _pseudoDetailType;

        private string _masterLinkName;

        private string _masterToDetailPseudoProperty;

        /// <summary>
        /// Имя представления, определяющего псевдодетейл.
        /// </summary>
        public string PseudoDetailViewName
        {
            get
            {
                return _pseudoDetailViewName;
            }
        }

        /// <summary>
        /// Тип псевдодетейла.
        /// </summary>
        public Type PseudoDetailType
        {
            get
            {
                return _pseudoDetailType;
            }
        }

        /// <summary>
        /// Свойство, по которому идёт связь от псевдодетейла к детейлу.
        /// </summary>
        public string MasterLinkName
        {
            get
            {
                return _masterLinkName;
            }
        }

        /// <summary>
        /// Имя псевдосвойства, по которому идёт связь от мастера к детейлу.
        /// </summary>
        public string MasterToDetailPseudoProperty
        {
            get
            {
                return _masterToDetailPseudoProperty;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pseudoDetailViewName">Имя представления, определяющего псевдодетейл.</param>
        /// <param name="pseudoDetailType">Тип псевдодетейла.</param>
        /// <param name="masterLinkName">Свойство, по которому идёт связь от псевдодетейла к детейлу.</param>
        /// <param name="masterToDetailPseudoProperty">Имя псевдосвойства, по которому идёт связь от мастера к детейлу.</param>
        public PseudoDetailInExtendedView(
            string pseudoDetailViewName, Type pseudoDetailType, string masterLinkName, string masterToDetailPseudoProperty)
        {
            _pseudoDetailViewName = pseudoDetailViewName;
            _masterLinkName = masterLinkName;
            _pseudoDetailType = pseudoDetailType;
            _masterToDetailPseudoProperty = masterToDetailPseudoProperty;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="pseudoDetailView">Представление, определяющее псевдодетейл.</param>
        /// <param name="masterLinkName">Свойство, по которому идёт связь от псевдодетейла к детейлу.</param>
        /// <param name="masterToDetailPseudoProperty">Имя псевдосвойства, по которому идёт связь от мастера к детейлу.</param>
        public PseudoDetailInExtendedView(View pseudoDetailView, string masterLinkName, string masterToDetailPseudoProperty)
        {
            _pseudoDetailViewName = pseudoDetailView.Name;
            _masterLinkName = masterLinkName;
            _pseudoDetailType = pseudoDetailView.DefineClassType;
            _masterToDetailPseudoProperty = masterToDetailPseudoProperty;
        }

        /// <summary>
        /// Конструктор класса при десериализации.
        /// </summary>
        /// <param name="info"> Сериализованные данные. </param>
        /// <param name="context"> Контекст сериализации. </param>
        public PseudoDetailInExtendedView(SerializationInfo info, StreamingContext context)
        {
            _pseudoDetailViewName = info.GetString("_pseudoDetailViewName");
            _masterLinkName = info.GetString("_masterLinkName");
            _pseudoDetailType = (Type)info.GetValue("_pseudoDetailType", typeof(Type));
            _masterToDetailPseudoProperty = info.GetString("_masterToDetailPseudoProperty");
        }

        /// <summary>
        /// Метод, сериализующий данный объект.
        /// </summary>
        /// <param name="info"> Сериализованные данные. </param>
        /// <param name="context"> Контекст сериализации. </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_pseudoDetailViewName", _pseudoDetailViewName);
            info.AddValue("_pseudoDetailType", _pseudoDetailType);
            info.AddValue("_masterLinkName", _masterLinkName);
            info.AddValue("_masterToDetailPseudoProperty", _masterToDetailPseudoProperty);
        }
    }
}
