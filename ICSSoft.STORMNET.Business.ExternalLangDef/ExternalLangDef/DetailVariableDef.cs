namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;
    using ICSSoft.Services;
    using ICSSoft.STORMNET.FunctionalLanguage;

    using Unity;
    using Unity.Exceptions;

    /// <summary>
    /// Определение переменной ограничения, предназначенное для описания детейлов.
    /// </summary>
    public class DetailVariableDef : VariableDef
    {
        private View fView;
        private string fConnectMasterPorp;
        private string[] fownerConnectProp;

        public DetailVariableDef()
        {
        }

        public View View
        {
            get { return fView; }
            set { fView = value; }
        }

        public string ConnectMasterPorp
        {
            get { return fConnectMasterPorp; }
            set { fConnectMasterPorp = value; }
        }

        public string[] OwnerConnectProp
        {
            get { return fownerConnectProp; }
            set { fownerConnectProp = value; }
        }

        public DetailVariableDef(ICSSoft.STORMNET.FunctionalLanguage.ObjectType type, string name, View v, string cmp, params string[] ocp)
            : base(type, name, name)
        {
            fView = v;
            ConnectMasterPorp = cmp ?? Information.GetAgregatePropertyName(v.DefineClassType);
            OwnerConnectProp = (ocp == null || (ocp.Length == 1 && ocp[0] == null)) ? new string[] { FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.StormMainObjectKey } : ocp;
            fView.AddProperty(ConnectMasterPorp);
        }

        /// <summary>
        /// Получение представления по его имени и типу.
        /// Если представление не найдено среди статических, то есть возможность сформировать его динамически (используется в редакторе ограничений).
        /// </summary>
        /// <param name="viewName">Имя представления, которое требуется получить.</param>
        /// <param name="dataObjectType">Тип объекта, для которого нужно представление.</param>
        /// <returns>Найденное/сформированное представление или <c>null</c>.</returns>
        public static View GetPossibleDynamicView(string viewName, Type dataObjectType)
        {
            View resultView = Information.GetView(viewName, dataObjectType);

            if (resultView == null)
            {
                // Если не удалось получить представление для детейла стандартным методом, то пробуем сделать это другим способом.
                IUnityContainer container = UnityFactory.GetContainer();
                IViewGenerator resolvedType;

                try
                {
                    resolvedType = container.Resolve<IViewGenerator>();
                }
                catch (ResolutionFailedException)
                {
                    resolvedType = null;
                }

                if (resolvedType != null)
                {
                    resultView = resolvedType.GenerateView(viewName, dataObjectType);
                }
            }

            return resultView;
        }

        public override object ToSimpleValue()
        {
            return new object[]
            {
                fView.Name,
                fView.DefineClassType.AssemblyQualifiedName,
                ConnectMasterPorp,
                OwnerConnectProp,
                base.ToSimpleValue(),
            };
        }

        /// <summary>
        /// Извлекаем описание ограничения на детейл из особой сериализованной структуры.
        /// </summary>
        /// <param name="value">Фрагмент сериализованной структуры, соответствующий описанию детейла.</param>
        /// <param name="ldef">Описание языка, посредством которого в том числе идёт расшифровка.</param>
        public override void FromSimpleValue(object value, FunctionalLanguageDef ldef)
        {
            object[] obj = (object[])value;
            base.FromSimpleValue(obj[4], ldef);
            Type tp = null;
            try
            {
                tp = System.Type.GetType((string)obj[1], false);
            }
            catch (Exception)
            {
                if (ExtraTypeResolver != null)
                {
                    tp = ExtraTypeResolver.Invoke((string)obj[1]);
                }

                if (tp == null)
                {
                    throw;
                }
            }

            fView = GetPossibleDynamicView((string)obj[0], tp);

            ConnectMasterPorp = (string)obj[2];
            if (obj[3] == null)
            {
                OwnerConnectProp = null;
            }
            else
            {
                OwnerConnectProp = (obj[3].GetType() == typeof(string)) ? new string[] { (string)obj[3] } : (string[])obj[3];
            }

            fView.AddProperty(ConnectMasterPorp);
        }
    }
}
