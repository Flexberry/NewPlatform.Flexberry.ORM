namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.FunctionalLanguage;

    /// <summary>
    /// настройка загрузки группы объектов
    /// </summary>
    [Serializable]
    public class LoadingCustomizationStruct : ISerializable
    {
        private object creatorKey;
        private ColumnsSortDef[] fieldColumnsSort;
        private Function fieldLimitFunction;
        private System.Type[] fieldLoadingTypes;
        private ICSSoft.STORMNET.View fieldView;
        private string[] fieldColumnsOrder;
        private AdvansedColumn[] fieldAdvansedColumns;
        private bool fieldInitDataCopy = true;

        private int fieldReturnTop = 0;
        private int fieldLoadingBufferSize = 0;

        private RowNumberDef fieldRowNumber;

        public RowNumberDef RowNumber
        {
            get
            {
                return fieldRowNumber;
            }

            set
            {
                fieldRowNumber = value;
            }
        }

        private bool fieldDistinct = false;

        public bool Distinct
        {
            get { return fieldDistinct; } set { fieldDistinct = value; }
        }

        public int ReturnTop
        {
            get
            {
                return fieldReturnTop;
            }

            set
            {
                fieldReturnTop = value;
            }
        }

        public LcsReturnType ReturnType { get; set; }

        public int LoadingBufferSize
        {
            get { return fieldLoadingBufferSize; } set { fieldLoadingBufferSize = value; }
        }

        public bool InitDataCopy
        {
            get { return fieldInitDataCopy; }
            set { fieldInitDataCopy = value; }
        }

        public ColumnsSortDef[] GetOwnerOnlySortDef()
        {
            if (ColumnsSort == null || ColumnsSort.Length == 0)
            {
                return null;
            }
            else
            {
                System.Collections.ArrayList al = new System.Collections.ArrayList();
                foreach (ColumnsSortDef csd in ColumnsSort)
                {
                    if (View.CheckPropname(csd.Name) || csd.Name.IndexOf(".") == -1)
                    {
                        al.Add(csd);
                    }
                }

                return (ColumnsSortDef[])al.ToArray(typeof(ColumnsSortDef));
            }
        }

        public ColumnsSortDef[] GetColumnsSortDef(string ReferenceName)
        {
            if (ColumnsSort == null || ColumnsSort.Length == 0)
            {
                return null;
            }
            else
            {
                ReferenceName = ReferenceName + ".";
                System.Collections.ArrayList al = new System.Collections.ArrayList();
                foreach (ColumnsSortDef csd in ColumnsSort)
                {
                    if (csd.Name.StartsWith(ReferenceName))
                    {
                        ColumnsSortDef cs = csd;
                        cs.Name = csd.Name.Substring(ReferenceName.Length);
                        al.Add(cs);
                    }
                }

                return (ColumnsSortDef[])al.ToArray(typeof(ColumnsSortDef));
            }
        }

        /// <summary>
        /// Десереализация
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public LoadingCustomizationStruct(SerializationInfo info, StreamingContext context)
        {
            string s1 = info.GetString("ddd");

            // return;
            this.fieldColumnsSort = (ColumnsSortDef[])info.GetValue("fieldColumnsSort", typeof(ColumnsSortDef[]));

            // this.fieldLimitFunction = ( STORMFunction )info.GetValue( "fieldLimitFunction", typeof( STORMFunction ) );
            this.fieldLoadingTypes = (System.Type[])info.GetValue("fieldLoadingTypes", typeof(System.Type[]));
            this.fieldView = (ICSSoft.STORMNET.View)info.GetValue("fieldView", typeof(ICSSoft.STORMNET.View));
            this.fieldColumnsOrder = (string[])info.GetValue("fieldColumnsOrder", typeof(string[]));
            this.fieldAdvansedColumns =
                (AdvansedColumn[])info.GetValue("fieldAdvansedColumns", typeof(AdvansedColumn[]));
            this.fieldInitDataCopy = info.GetBoolean("fieldInitDataCopy");

            // [2012-05-21 Истомин] далее идут куча try\catch, нужны были для того, чтобы старые ограничения не упали
            try
            {
                // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException
                // Ловить будем только SerializationException
                ReturnType = (LcsReturnType)info.GetValue("ReturnType", typeof(LcsReturnType));
            }
            catch (SerializationException)
            {
                ReturnType = LcsReturnType.Objects;
            }

            try
            {
                // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException
                // Ловить будем только SerializationException
                ReturnTop = info.GetInt32("ReturnTop");
            }
            catch (SerializationException)
            {
                ReturnTop = 0;
            }

            try
            {
                // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException
                // Ловить будем только SerializationException
                LoadingBufferSize = info.GetInt32("LoadingBufferSize");
            }
            catch (SerializationException)
            {
                LoadingBufferSize = 0;
            }

            try
            {
                // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException
                // Ловить будем только SerializationException
                RowNumber = (RowNumberDef)info.GetValue("RowNumber", typeof(RowNumberDef));
            }
            catch (SerializationException)
            {
                RowNumber = null;
            }

            try
            {
                // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException
                // Ловить будем только SerializationException
                Distinct = info.GetBoolean("Distinct");
            }
            catch (SerializationException)
            {
                Distinct = false;
            }

            bool bNull = info.GetBoolean("fieldLimitFunctionNull");

            if (!bNull)
            {
                string sFunction = info.GetString("fieldLimitFunctionStr");

                FunctionalLanguage.FunctionForControls fcc = FunctionalLanguage.FunctionForControls.Parse(
                    sFunction, this.fieldView);

                this.fieldLimitFunction = fcc.Function;
            }
            else
            {
                this.fieldLimitFunction = null;
            }

            bool keynull = info.GetBoolean("keynull");

            if (!keynull)
            {
                var tp = (Type)info.GetValue("keytype", typeof(Type));

                string s = info.GetString("keystr");

                if (tp == typeof(int))
                {
                    creatorKey = int.Parse(s);
                }
                else
                {
                    MethodInfo mi = tp.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);

                    creatorKey = mi != null
                                     ? mi.Invoke(null, new object[] { s })
                                     : Activator.CreateInstance(tp, new object[] { s });
                }
            }
        }

        /// <summary>
        /// Cереализация
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ddd", "asdasd");

            info.AddValue("fieldColumnsSort", this.fieldColumnsSort);

            // info.AddValue( "fieldLimitFunction", this.fieldLimitFunction );
            info.AddValue("fieldLoadingTypes", this.fieldLoadingTypes);
            info.AddValue("fieldView", this.fieldView);
            info.AddValue("fieldColumnsOrder", this.fieldColumnsOrder);
            info.AddValue("fieldAdvansedColumns", this.fieldAdvansedColumns);
            info.AddValue("fieldInitDataCopy", this.fieldInitDataCopy);

            info.AddValue("ReturnType", ReturnType);
            info.AddValue("ReturnTop", ReturnTop);
            info.AddValue("LoadingBufferSize", LoadingBufferSize);
            info.AddValue("RowNumber", RowNumber);
            info.AddValue("Distinct", Distinct);

            info.AddValue("fieldLimitFunctionNull", this.fieldLimitFunction == null);
            if (this.fieldLimitFunction != null)
            {
                var f = new FunctionForControls(fieldView, fieldLimitFunction);

                info.AddValue("fieldLimitFunctionStr", f.ToString());
            }

            info.AddValue("keynull", this.creatorKey == null);
            if (this.creatorKey != null)
            {
                info.AddValue("keytype", this.creatorKey.GetType());
                info.AddValue("keystr", this.creatorKey.ToString());
            }
        }

        /// <summary>
        /// кто его создал
        /// </summary>
        public object CreatorKey
        {
            get
            {
                return creatorKey;
            }
        }

        /// <summary>
        /// Дополнительные колонки
        /// </summary>
        public AdvansedColumn[] AdvansedColumns
        {
            get
            {
                return fieldAdvansedColumns ?? (fieldAdvansedColumns = new AdvansedColumn[0]);
            }

            set
            {
                fieldAdvansedColumns = value;
            }
        }

        /// <summary>
        /// порядок колонок в выборке
        /// </summary>
        public string[] ColumnsOrder
        {
            get { return fieldColumnsOrder; } set { fieldColumnsOrder = value; }
        }

        /// <summary>
        /// сортировка колонок
        /// </summary>
        public ColumnsSortDef[] ColumnsSort
        {
            get { return fieldColumnsSort; } set { fieldColumnsSort = value; }
        }

        /// <summary>
        /// ограничение на объекты
        /// </summary>
        public Function LimitFunction
        {
            get { return fieldLimitFunction; } set { fieldLimitFunction = value; }
        }

        /// <summary>
        /// вычитываем эти типы
        /// </summary>
        public System.Type[] LoadingTypes
        {
            get { return fieldLoadingTypes; } set { fieldLoadingTypes = value; }
        }

        /// <summary>
        /// используемое представление
        /// </summary>
        public View View
        {
            get { return fieldView; } set { fieldView = value; }
        }

        private View getDefView(Type dataObjectType)
        {
            return new View(dataObjectType, View.ReadType.OnlyThatObject);
        }

        private View getDefView(Type[] dataObjectTypes)
        {
            return getDefView(GetBaseType(dataObjectTypes));
        }

        /// <summary>
        /// Добавить сортировку
        /// </summary>
        /// <param name="csd">Сортировка</param>
        public void AddColumnSort(ColumnsSortDef csd)
        {
            if (fieldColumnsSort == null)
            {
                fieldColumnsSort = new ColumnsSortDef[0];
            }

            int newsize = fieldColumnsSort.Length + 1;
            Array.Resize(ref fieldColumnsSort, newsize);
            fieldColumnsSort[newsize - 1] = csd;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="creatorKey">ключ для идентификации вызова</param>
        public LoadingCustomizationStruct(object creatorKey)
        {
            this.creatorKey = creatorKey;
        }

        private System.Type GetBaseType(System.Type[] types)
        {
            System.Type testType = types[0];
            bool testTypeIsAllParent = false;
            while (!testTypeIsAllParent)
            {
                testTypeIsAllParent = true;
                for (int i = 1; i < types.Length; i++)
                {
                    if (types[i] != testType && !types[i].IsSubclassOf(testType))
                    {
                        testTypeIsAllParent = false;
                        break;
                    }
                }

                if (!testTypeIsAllParent)
                {
                    testType = testType.BaseType;
                }
            }

            return testType;
        }

        /// <summary>
        /// Получить часто используемую функцию по ограничению
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Function Keys2Function(object[] keys)
        {
            var lg = FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
            var vd = new VariableDef(lg.GetObjectTypeForNetType(keys[0].GetType()), "STORMMainObjectKey");
            var pars = new object[keys.Length + 1];
            Array.Copy(keys, 0, pars, 1, keys.Length);
            pars[0] = vd;
            return lg.GetFunction(lg.funcIN, pars);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ldColumnsSort">сортировка</param>
        /// <param name="ldLimitFunction">ограничение</param>
        /// <param name="ldLoadingTypes">загружаемые типы</param>
        /// <param name="ldView">представление</param>
        /// <param name="ldColumnsOrder">порядочек колонок</param>
        public void Init(
            ColumnsSortDef[] ldColumnsSort,
            Function ldLimitFunction,
            System.Type[] ldLoadingTypes,
            ICSSoft.STORMNET.View ldView,
            string[] ldColumnsOrder)
        {
            Init(ldColumnsSort, ldLimitFunction, ldLoadingTypes, ldView, null, ldColumnsOrder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ldColumnsSort">сортировка</param>
        /// <param name="ldLimitFunction">ограничение</param>
        /// <param name="ldLoadingTypes">загружаемые типы</param>
        /// <param name="ldView">представление</param>
        /// <param name="ldAdvansedColumns">дополнительные колонки</param>
        /// <param name="ldColumnsOrder">порядочек колонок</param>
        public void Init(
            ColumnsSortDef[] ldColumnsSort,
            Function ldLimitFunction,
            System.Type[] ldLoadingTypes,
            ICSSoft.STORMNET.View ldView,
            AdvansedColumn[] ldAdvansedColumns,
            string[] ldColumnsOrder)
        {
            // ldLimitFunction.Cop

            // Перед инициализацией LCS скопируем настройки колонок и ограничения, чтобы внутрь lcs
            if (ldColumnsSort != null)
            {
                fieldColumnsSort = new ColumnsSortDef[ldColumnsSort.Length];
                ldColumnsSort.CopyTo(fieldColumnsSort, 0);
            }

            // fieldColumnsSort = ldColumnsSort;

            // ldLimitFunction.FunctionDef.CopyTo()
            // ldLimitFunction.Parameters.CopyTo

            if (ldLimitFunction != null)
            {
                fieldLimitFunction = ldLimitFunction.Clone();
            }

            if (ldLoadingTypes != null)
            {
                fieldLoadingTypes = new Type[ldLoadingTypes.Length];
                ldLoadingTypes.CopyTo(fieldLoadingTypes, 0);
            }

            // fieldLoadingTypes = ldLoadingTypes;
            if (ldView != null)
            {
                fieldView = ldView.Clone();
            }

            // fieldView = ldView;

            if (ldColumnsOrder != null)
            {
                fieldColumnsOrder = new string[ldColumnsOrder.Length];
                ldColumnsOrder.CopyTo(fieldColumnsOrder, 0);
            }

            // fieldColumnsOrder = ldColumnsOrder;
            if (ldAdvansedColumns != null)
            {
                fieldAdvansedColumns = new AdvansedColumn[ldAdvansedColumns.Length];
                ldAdvansedColumns.CopyTo(fieldAdvansedColumns, 0);
            }

            // fieldAdvansedColumns = ldAdvansedColumns;
        }

        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType, string View)
        {
            if (Information.GetView(View, DataObjectType) == null)
            {
                throw new Exception("Обратитесь к разработчику: в методе GetSimpleStruct указано неверное представление \"" + View + "\" для типа \"" + DataObjectType.AssemblyQualifiedName + "\"");
            }

            LoadingCustomizationStruct lcs = new LoadingCustomizationStruct(null);
            lcs.Init(null, null, new Type[] { DataObjectType }, Information.GetView(View, DataObjectType), null);
            return lcs;
        }

        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType, View View)
        {
            LoadingCustomizationStruct lcs = new LoadingCustomizationStruct(null);
            lcs.Init(null, null, new Type[] { DataObjectType }, View, null);
            return lcs;
        }

        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType, string View, string propertyName, object limitValue)
        {
            var lcs = new LoadingCustomizationStruct(null);
            FunctionalLanguage.SQLWhere.SQLWhereLanguageDef ldef =
                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
            Type proptype = Information.GetPropertyType(DataObjectType, propertyName);
            var var = new VariableDef(ldef.GetObjectTypeForNetType(proptype), propertyName);
            lcs.LoadingTypes = new[] { DataObjectType };
            lcs.View = Information.GetView(View, DataObjectType);
            lcs.LimitFunction = ldef.GetFunction(ldef.funcEQ, var, limitValue);
            return lcs;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(LoadingCustomizationStruct))
            {
                return false;
            }

            return Equals((LoadingCustomizationStruct)obj);
        }

        private bool ViewsEquals(View v1, View v2)
        {
            if (v1 == null)
            {
                return v2 == null;
            }

            if (v2 == null)
            {
                return false;
            }

            return v1.ToString(true) == v2.ToString(true);
        }

        private bool FunctionEquals(Function f1, Function f2)
        {
            if (f1 == null)
            {
                return f2 == null;
            }

            if (f2 == null)
            {
                return false;
            }

            return f1.ToString() == f2.ToString();
        }

        public bool Equals(LoadingCustomizationStruct other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.creatorKey, creatorKey) && Utils.ArraysEqual(other.fieldColumnsSort, fieldColumnsSort)
                   && FunctionEquals(other.fieldLimitFunction, fieldLimitFunction)
                   && Utils.ArraysEqual(other.fieldLoadingTypes, fieldLoadingTypes)
                   && ViewsEquals(other.fieldView, fieldView) && Utils.ArraysEqual(other.fieldColumnsOrder, fieldColumnsOrder)
                   && Utils.ArraysEqual(other.fieldAdvansedColumns, fieldAdvansedColumns)
                   && other.fieldInitDataCopy.Equals(fieldInitDataCopy) && other.fieldReturnTop == fieldReturnTop
                   && other.fieldLoadingBufferSize == fieldLoadingBufferSize
                   && Equals(other.fieldRowNumber, fieldRowNumber) && other.fieldDistinct.Equals(fieldDistinct)
                   && Equals(other.ReturnType, ReturnType);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = creatorKey != null ? creatorKey.GetHashCode() : 0;
                result = (result * 397) ^ (fieldColumnsSort != null ? fieldColumnsSort.GetHashCode() : 0);
                result = (result * 397) ^ (fieldLimitFunction != null ? fieldLimitFunction.GetHashCode() : 0);
                result = (result * 397) ^ (fieldLoadingTypes != null ? fieldLoadingTypes.GetHashCode() : 0);
                result = (result * 397) ^ (fieldView != null ? fieldView.GetHashCode() : 0);
                result = (result * 397) ^ (fieldColumnsOrder != null ? fieldColumnsOrder.GetHashCode() : 0);
                result = (result * 397) ^ (fieldAdvansedColumns != null ? fieldAdvansedColumns.GetHashCode() : 0);
                result = (result * 397) ^ fieldInitDataCopy.GetHashCode();
                result = (result * 397) ^ fieldReturnTop;
                result = (result * 397) ^ fieldLoadingBufferSize;
                result = (result * 397) ^ (fieldRowNumber != null ? fieldRowNumber.GetHashCode() : 0);
                result = (result * 397) ^ fieldDistinct.GetHashCode();
                result = (result * 397) ^ ReturnType.GetHashCode();
                return result;
            }
        }
    }
}