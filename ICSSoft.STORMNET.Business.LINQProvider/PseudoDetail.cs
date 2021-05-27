namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    /// <summary>
    /// Вспомогательный класс, который в Linq-выражении обозначает псевдодетейл.
    /// </summary>
    /// <typeparam name="T"> Тип мастера. </typeparam>
    /// <typeparam name="TP"> Тип детейла. </typeparam>
    /// <remarks>Большое число конструкторов обусловлено тем, что в Linq-выражении сложности с использованием конструкторов со значением по умолчанию.</remarks>
    public class PseudoDetail<T, TP>
    {
        /// <summary>
        /// Представление псевдодетейла.
        /// </summary>
        private ICSSoft.STORMNET.View _pseudoDetailView;

        /// <summary>
        /// Свойства мастера, по которым можно произвести соединение.
        /// Фактически соединяем так: Агрегатор.MasterConnectProperties = Псевдодетейл.MasterLinkName.
        /// Аналог OwnerConnectProp для <see cref="DetailVariableDef"/> в lcs.
        /// </summary>
        private List<string> _masterConnectProperties;

        /// <summary>
        /// Представление псевдодетейла.
        /// </summary>
        public ICSSoft.STORMNET.View PseudoDetailView
        {
            get
            {
                return _pseudoDetailView;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (string.IsNullOrEmpty(value.Name))
                {
                    throw new ArgumentException("Представление должно иметь имя.");
                }

                _pseudoDetailView = value;
            }
        }

        /// <summary>
        /// Имя связи от псевдодетейла к мастеру.
        /// Аналог ConnectMasterPorp для <see cref="DetailVariableDef"/> в lcs.
        /// </summary>
        public string MasterLinkName { get; private set; }

        /// <summary>
        /// Имя связи от мастера к псевдодетейлу (псевдосвойство).
        /// </summary>
        public string MasterToDetailPseudoProperty { get; private set; }

        /// <summary>
        /// Свойства мастера, по которым можно произвести соединение.
        /// Фактически соединяем так: Агрегатор.MasterConnectProperties = Псевдодетейл.MasterLinkName.
        /// Аналог OwnerConnectProp для <see cref="DetailVariableDef"/> в lcs.
        /// Значение по умолчанию: "new string[] { SQLWhereLanguageDef.StormMainObjectKey }".
        /// </summary>
        public string[] MasterConnectProperties
        {
            get
            {
                return
                    (_masterConnectProperties == null || !_masterConnectProperties.Any())
                        ? new string[] { SQLWhereLanguageDef.StormMainObjectKey }
                        : _masterConnectProperties.ToArray();
            }

            private set
            {
                _masterConnectProperties = null;
                if (value != null)
                {
                    _masterConnectProperties =
                        value.Where(x => !string.IsNullOrEmpty(x) && x.Trim() != string.Empty)
                            .Select(x => x.Trim())
                            .ToList();
                }
            }
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении псевдодетейл.
        /// </summary>
        /// <param name="view"> Представление псевдодетейла. </param>
        /// <param name="masterLinkName"> Имя связи от псевдодетейла к мастеру. </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view,
            string masterLinkName)
            : this(view, masterLinkName, string.Empty, null)
        {
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении псевдодетейл.
        /// </summary>
        /// <param name="view"> Представление псевдодетейла. </param>
        /// <param name="masterLink"> Метод, определяющий имя связи от псевдодетейла к мастеру (определение идёт через "Information.ExtractPropertyPath(masterLink)"). </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view,
            Expression<Func<TP, object>> masterLink)
            : this(view, Information.ExtractPropertyPath(masterLink), string.Empty, null)
        {
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении настоящий детейл (для псевдодетейлов данный метод будет некорректен).
        /// </summary>
        /// <param name="view"> Представление детейла. </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view)
            : this(view, GetMasterLinkNameForRealDetail(), string.Empty, null)
        {
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении псевдодетейл.
        /// </summary>
        /// <param name="view"> Представление псевдодетейла. </param>
        /// <param name="masterLink"> Метод, определяющий имя связи от псевдодетейла к мастеру (определение идёт через "Information.ExtractPropertyPath(masterLink)"). </param>
        /// <param name="masterToDetailPseudoProperty"> Имя связи от мастера к псевдодетейлу (псевдосвойство). </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view,
            Expression<Func<TP, object>> masterLink,
            string masterToDetailPseudoProperty)
            : this(view, Information.ExtractPropertyPath(masterLink), masterToDetailPseudoProperty, null)
        {
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении псевдодетейл.
        /// </summary>
        /// <param name="view"> Представление псевдодетейла. </param>
        /// <param name="masterLink"> Метод, определяющий имя связи от псевдодетейла к мастеру (определение идёт через "Information.ExtractPropertyPath(masterLink)"). </param>
        /// <param name="masterToDetailPseudoProperty"> Имя связи от мастера к псевдодетейлу (псевдосвойство). </param>
        /// <param name="masterConnectProperties"> Свойства мастера, по которым можно произвести соединение. Аналог OwnerConnectProp для <see cref="DetailVariableDef"/> в lcs. </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view,
            Expression<Func<TP, object>> masterLink,
            string masterToDetailPseudoProperty,
            string[] masterConnectProperties)
            : this(view, Information.ExtractPropertyPath(masterLink), masterToDetailPseudoProperty, masterConnectProperties)
        {
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении псевдодетейл.
        /// </summary>
        /// <param name="view"> Представление псевдодетейла. </param>
        /// <param name="masterLinkName"> Имя связи от псевдодетейла к мастеру. </param>
        /// <param name="masterToDetailPseudoProperty"> Имя связи от мастера к псевдодетейлу (псевдосвойство). </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view,
            string masterLinkName,
            string masterToDetailPseudoProperty)
            : this(view, masterLinkName, masterToDetailPseudoProperty, null)
        {
        }

        /// <summary>
        /// Конструктор сущности, представляющей в Linq-выражении псевдодетейл.
        /// </summary>
        /// <param name="view"> Представление псевдодетейла. </param>
        /// <param name="masterLinkName"> Имя связи от псевдодетейла к мастеру. </param>
        /// <param name="masterToDetailPseudoProperty"> Имя связи от мастера к псевдодетейлу (псевдосвойство). </param>
        /// <param name="masterConnectProperties"> Свойства мастера, по которым можно произвести соединение. Аналог OwnerConnectProp для <see cref="DetailVariableDef"/> в lcs. </param>
        public PseudoDetail(
            ICSSoft.STORMNET.View view,
            string masterLinkName,
            string masterToDetailPseudoProperty,
            string[] masterConnectProperties)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            if (view.DefineClassType != typeof(TP))
            {
                throw new CantFindViewException(typeof(TP), view.Name);
            }

            if (masterLinkName == null || string.IsNullOrEmpty(masterLinkName.Trim()))
            {
                throw new ArgumentException("Заданное имя связи от псевдодетейла к мастеру некорректно.");
            }

            PseudoDetailView = view;
            MasterLinkName = masterLinkName;
            MasterToDetailPseudoProperty = string.IsNullOrEmpty(masterToDetailPseudoProperty)
                                            ? string.Format(
                                                "{0}__{1}",
                                                view.DefineClassType.Name.Replace(".", string.Empty),
                                                masterLinkName)
                                            : masterToDetailPseudoProperty;
            MasterConnectProperties = masterConnectProperties;
        }

        /// <summary>
        /// Вспомогательный метод, используемый для формирования ограничений на псевдодетейлы с условием произвольного типа.
        /// Метод используется для правильного формирования свойства, которое позднее возвращается в редактор ограничений.
        /// </summary>
        /// <param name="predicate"> Функция ограничения для псевдодетейла. </param>
        /// <returns> При компиляции вернёт <c>true</c>, при использовании в редакторе ограничений после преобразований данная функция затирается. </returns>
        public bool Any<TSomeType>(Expression<Func<TP, TSomeType>> predicate)
        {
            return true;
        }

        /// <summary>
        /// Вспомогательный метод, преобразуемый на этапе компиляции Linq-выражения в funcExist.
        /// </summary>
        /// <param name="predicate"> LimitFunction для псевдодетейла. </param>
        /// <returns> При компиляции вернёт true, при интерпретации в Linq формируется DetailVariableDef. </returns>
        public bool Any(Expression<Func<TP, bool>> predicate)
        {
            return true;
        }

        /// <summary>
        /// Вспомогательный метод, преобразуемый на этапе компиляции Linq-выражения в funcExist.
        /// </summary>
        /// <returns> При компиляции вернёт true, при интерпретации в Linq формируется DetailVariableDef. </returns>
        public bool Any()
        {
            return true;
        }

        /// <summary>
        /// Вспомогательный метод, преобразуемый на этапе компиляции Linq-выражения в funcExistExact.
        /// </summary>
        /// <param name="predicate"> LimitFunction для псевдодетейла. </param>
        /// <returns> При компиляции вернёт true, при интерпретации в Linq формируется DetailVariableDef. </returns>
        public bool All(Expression<Func<TP, bool>> predicate)
        {
            return true;
        }

        /// <summary>
        /// Вспомогательный метод, который для настоящего детейла определяет имя свойства, по которому он связывается с мастером.
        /// </summary>
        /// <returns> Имя свойства, по которому детейл связывается с мастером. </returns>
        private static string GetMasterLinkNameForRealDetail()
        {
            var masterLinkName = Information.GetAgregatePropertyName(typeof(TP));
            if (string.IsNullOrEmpty(masterLinkName))
            {
                throw new NotFoundAggregatorProperty();
            }

            return masterLinkName;
        }
    }
}
