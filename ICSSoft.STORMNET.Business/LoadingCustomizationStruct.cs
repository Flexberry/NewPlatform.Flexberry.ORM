namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    /// <summary>
    /// настройка загрузки группы объектов
    /// </summary>
    [ Serializable ]
    public class LoadingCustomizationStruct : ISerializable
    {

        public override string ToString()
        {
            return $"LCS({View} + {string.Join(",",AdvancedColumns.Select(x=>"#"+x.Expression+"#").ToArray())} [{LimitFunction}])";
        }
        private object creatorKey;
        private ColumnsSortDef[] fieldColumnsSort;
        private Function fieldLimitFunction;

        [NonSerialized]
        private System.Type[] fieldLoadingTypes;
        private ICSSoft.STORMNET.View fieldView;
        private string[] fieldColumnsOrder;
        private AdvancedColumn[] fieldAdvancedColumns;
        private bool fieldInitDataCopy  = true;
		
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

        public List<string> RankPropertyName { get; set; }

        public List<string> PartitionPropertyName { get; set; }


        public LoadingCustomizationStruct Clone()
        {
            LoadingCustomizationStruct res = new LoadingCustomizationStruct();

            if (this.AdvancedColumns != null)
                res.AdvancedColumns = this.AdvancedColumns.ToArray();
            if (this.AssemblyQualifiedTypeNames!=null)
                res.AssemblyQualifiedTypeNames = this.AssemblyQualifiedTypeNames.ToArray();
            if (this.ColumnsOrder!=null)
                res.ColumnsOrder = this.ColumnsOrder.ToArray();
            if (this.ColumnsSort!=null)
                res.ColumnsSort = this.ColumnsSort.ToArray();
            res.Distinct = this.Distinct;
            res.InitDataCopy = this.InitDataCopy;
            if (this.LimitFunction!=null)
                res.LimitFunction = this.LimitFunction.Clone();
            res.LoadingBufferSize = this.LoadingBufferSize;
            if (this.LoadingTypes!=null)
                res.LoadingTypes = this.LoadingTypes.ToArray();
            res.ReturnTop = this.ReturnTop;
            res.ReturnType = this.ReturnType;
            res.RowNumber = this.RowNumber;
            if (this.View!=null)
            res.View = this.View.Clone();

            return res;
        }



        private bool fieldDistinct = false;
        public bool Distinct {get {return fieldDistinct;}set {fieldDistinct = value;}}

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

        public int LoadingBufferSize {get {return fieldLoadingBufferSize;}set {fieldLoadingBufferSize = value;}}


        public bool InitDataCopy 
        {
            get {return fieldInitDataCopy;}
            set {fieldInitDataCopy = value;}
        }
		
        public ColumnsSortDef[] GetOwnerOnlySortDef()
        {
            if (ColumnsSort==null || ColumnsSort.Length==0)
                return null;
            else
            {
                System.Collections.ArrayList al = new System.Collections.ArrayList();
                foreach (ColumnsSortDef csd in ColumnsSort)
                {
                    if (View.CheckPropname(csd.Name) || csd.Name.IndexOf(".")==-1)
                        al.Add(csd);
                }
                return (ColumnsSortDef[])al.ToArray(typeof(ColumnsSortDef));
            }
        }

        public ColumnsSortDef[] GetColumnsSortDef(string ReferenceName)
        {
            if (ColumnsSort==null || ColumnsSort.Length==0)
                return null;
            else
            {
                ReferenceName= ReferenceName+".";
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


        [NonSerialized]
        private List<string> arrColNames;

        /// <summary>
        /// Возвращает индекс для колонки, соотв. атрибуту или дополнительной колонке в результирующем наборе данных с учетом 
        /// наличия этих самых дополнительных колонок в <see cref="AdvancedColumns"/> и настройки порядка следования колонок <see cref="ColumnsOrder"/>
        /// </summary>
        /// <param name="attributeOrAdvColumnName"></param>
        /// <returns></returns>
        public int GetColumnIndex(string attributeOrAdvColumnName)
        {
            if (arrColNames == null)
            {
                arrColNames = new List<string>();
                ColumnsOrder = ColumnsOrder == null ? new string[] { } : ColumnsOrder;
                arrColNames.AddRange(ColumnsOrder);
                arrColNames.AddRange(View.Properties.Select(x => x.Name).Except(ColumnsOrder));
                arrColNames.AddRange(AdvancedColumns.Select(x => x.Name).Except(ColumnsOrder));
            }
            return arrColNames.IndexOf(attributeOrAdvColumnName);
        }

        /// <summary>
        /// Десереализация
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public LoadingCustomizationStruct(SerializationInfo info, StreamingContext context)
        {
            //string s1 = info.GetString("ddd");
            ////return;
            //this.fieldColumnsSort = (ColumnsSortDef[])info.GetValue("fieldColumnsSort", typeof(ColumnsSortDef[]));
            ////this.fieldLimitFunction = ( STORMFunction )info.GetValue( "fieldLimitFunction", typeof( STORMFunction ) );
            //this.fieldLoadingTypes = (System.Type[])info.GetValue("fieldLoadingTypes", typeof(System.Type[]));
            //this.fieldView = (ICSSoft.STORMNET.View)info.GetValue("fieldView", typeof(ICSSoft.STORMNET.View));
            //this.fieldColumnsOrder = (string[])info.GetValue("fieldColumnsOrder", typeof(string[]));
            //this.fieldAdvansedColumns =
            //    (AdvansedColumn[])info.GetValue("fieldAdvansedColumns", typeof(AdvansedColumn[]));
            //this.fieldInitDataCopy = info.GetBoolean("fieldInitDataCopy");


            //// [2012-05-21 Истомин] далее идут куча try\catch, нужны были для того, чтобы старые ограничения не упали
            //try
            //{
            //    // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException 
            //    // Ловить будем только SerializationException
            //    ReturnType = (LcsReturnType)info.GetValue("ReturnType", typeof(LcsReturnType));
            //}
            //catch (SerializationException)
            //{
            //    ReturnType = LcsReturnType.Objects;
            //}

            //try
            //{
            //    // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException 
            //    // Ловить будем только SerializationException
            //    ReturnTop = info.GetInt32("ReturnTop");
            //}
            //catch (SerializationException)
            //{
            //    ReturnTop = 0;
            //}

            //try
            //{
            //    // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException 
            //    // Ловить будем только SerializationException
            //    LoadingBufferSize = info.GetInt32("LoadingBufferSize");
            //}
            //catch (SerializationException)
            //{
            //    LoadingBufferSize = 0;
            //}

            //try
            //{
            //    // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException 
            //    // Ловить будем только SerializationException
            //    RowNumber = (RowNumberDef)info.GetValue("RowNumber", typeof(RowNumberDef));
            //}
            //catch (SerializationException)
            //{
            //    RowNumber = null;
            //}

            //try
            //{
            //    // Может сгенерироваться 3 вида исключений: ArgumentNullException, InvalidCastException, SerializationException 
            //    // Ловить будем только SerializationException
            //    Distinct = info.GetBoolean("Distinct");
            //}
            //catch (SerializationException)
            //{
            //    Distinct = false;
            //}

            //bool bNull = info.GetBoolean("fieldLimitFunctionNull");

            //if (!bNull)
            //{
            //    string sFunction = info.GetString("fieldLimitFunctionStr");

            //    FunctionalLanguage.FunctionForControls fcc = FunctionalLanguage.FunctionForControls.Parse(
            //        sFunction, this.fieldView);

            //    this.fieldLimitFunction = fcc.Function;
            //}
            //else
            //{
            //    this.fieldLimitFunction = null;
            //}

            //bool keynull = info.GetBoolean("keynull");

            //if (!keynull)
            //{

            //    var tp = (Type)info.GetValue("keytype", typeof(Type));

            //    string s = info.GetString("keystr");

            //    if (tp == typeof(int))
            //    {
            //        creatorKey = int.Parse(s);
            //    }
            //    else
            //    {
            //        MethodInfo mi = tp.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);

            //        creatorKey = mi != null
            //                         ? mi.Invoke(null, new object[] { s })
            //                         : Activator.CreateInstance(tp, new object[] { s });
            //    }
            //}
        }

        /// <summary>
        /// Cереализация
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //info.AddValue("ddd", "asdasd");

            //info.AddValue("fieldColumnsSort", this.fieldColumnsSort);
            ////info.AddValue( "fieldLimitFunction", this.fieldLimitFunction );
            //info.AddValue("fieldLoadingTypes", this.fieldLoadingTypes);
            //info.AddValue("fieldView", this.fieldView);
            //info.AddValue("fieldColumnsOrder", this.fieldColumnsOrder);
            //info.AddValue("fieldAdvansedColumns", this.fieldAdvansedColumns);
            //info.AddValue("fieldInitDataCopy", this.fieldInitDataCopy);

            //info.AddValue("ReturnType", ReturnType);
            //info.AddValue("ReturnTop", ReturnTop);
            //info.AddValue("LoadingBufferSize", LoadingBufferSize);
            //info.AddValue("RowNumber", RowNumber);
            //info.AddValue("Distinct", Distinct);

            //info.AddValue("fieldLimitFunctionNull", this.fieldLimitFunction == null);
            //if (this.fieldLimitFunction != null)
            //{
            //    var f = new FunctionForControls(fieldView, fieldLimitFunction);

            //    info.AddValue("fieldLimitFunctionStr", f.ToString());
            //}

            //info.AddValue("keynull", this.creatorKey == null);
            //if (this.creatorKey != null)
            //{
            //    info.AddValue("keytype", this.creatorKey.GetType());
            //    info.AddValue("keystr", this.creatorKey.ToString());
            //}
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
        public AdvancedColumn[] AdvancedColumns
        {
            get
            {
                return fieldAdvancedColumns ?? (fieldAdvancedColumns = new AdvancedColumn[0]);
            }

            set
            {
                fieldAdvancedColumns = value;
            }
        }


        /// <summary>
        /// порядок колонок в выборке
        /// </summary>
        public string[] ColumnsOrder{get {return fieldColumnsOrder;}set{fieldColumnsOrder = value;}}


        /// <summary>
        /// сортировка колонок
        /// </summary>
        public ColumnsSortDef[] ColumnsSort {get {return fieldColumnsSort;} set {fieldColumnsSort = value;}}

        /// <summary>
        /// ограничение на объекты
        /// </summary>
        public Function LimitFunction { get {return fieldLimitFunction;} set { fieldLimitFunction = value;}}

        /// <summary>
        /// вычитываем эти типы
        /// </summary>
        [XmlIgnore]
        public System.Type[] LoadingTypes { get {return fieldLoadingTypes;} set {fieldLoadingTypes = value;}}

        [XmlElement("LoadingTypes")]
        public string[] AssemblyQualifiedTypeNames
        {
            get
            {
                if (LoadingTypes == null) return null;

                string[] assemblyQualifiedTypeNames = new string[LoadingTypes.Length];
                for (int i = 0; i < LoadingTypes.Length; i++)
                {
                    assemblyQualifiedTypeNames[i] = LoadingTypes[i].AssemblyQualifiedName;
                }
                return assemblyQualifiedTypeNames;
            }
            set
            {
                if (value == null) fieldLoadingTypes = null;
                fieldLoadingTypes = new Type[value.Length];
                for (int i = 0; i < value.Length; i++)
                {
                    try
                    {
                        fieldLoadingTypes[i] = Type.GetType(value[i], true);
                    }
                    catch (Exception ex)
                    {
                        string[] split = value[i].Split(',');
                        if (split.Length >= 2)
                            Assembly.LoadFrom(string.Format("{0}.dll", split[1].Trim()));

                        fieldLoadingTypes[i] = Type.GetType(value[i], true);
                    }
                }
            }
        }

        /// <summary>
        /// используемое представление
        /// </summary>
        public View View {get {return fieldView;} set {fieldView=value;}}

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


        public LoadingCustomizationStruct():this(null) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="creatorKey">ключ для идентификации вызова</param>
        public LoadingCustomizationStruct(object creatorKey){this.creatorKey = creatorKey;}


		
        private System.Type GetBaseType(System.Type[] types)
        {
            System.Type testType = types[0];
            bool testTypeIsAllParent = false;
            while (!testTypeIsAllParent)
            {
                testTypeIsAllParent=true;
                for (int i=1;i<types.Length;i++)
                {
                    if (types[i]!=testType && !types[i].IsSubclassOf(testType))
                    {
                        testTypeIsAllParent = false;
                        break;
                    }
                }
                if (!testTypeIsAllParent)
                    testType = testType.BaseType;
            }
            return testType;
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
            string[] ldColumnsOrder
            )
        {
            Init(ldColumnsSort,ldLimitFunction,ldLoadingTypes,ldView,null,ldColumnsOrder);
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
            AdvancedColumn[] ldAdvancedColumns,
            string[] ldColumnsOrder
            )
        {
            //ldLimitFunction.Cop

            //Перед инициализацией LCS скопируем настройки колонок и ограничения, чтобы внутрь lcs
            if (ldColumnsSort!=null)
            {
                fieldColumnsSort = new ColumnsSortDef[ldColumnsSort.Length];
                ldColumnsSort.CopyTo(fieldColumnsSort,0);
            }


            //fieldColumnsSort = ldColumnsSort;

            //ldLimitFunction.FunctionDef.CopyTo()
            //ldLimitFunction.Parameters.CopyTo 

            if (ldLimitFunction!=null)
            {
                fieldLimitFunction = ldLimitFunction.Clone();
            }
			

            if (ldLoadingTypes!=null)
            {
                fieldLoadingTypes = new Type[ldLoadingTypes.Length];
                ldLoadingTypes.CopyTo(fieldLoadingTypes,0);
            }
            //fieldLoadingTypes = ldLoadingTypes;
            if (ldView!=null)
            {
                fieldView = ldView.Clone();
            }
            //fieldView = ldView;
			
            if (ldColumnsOrder!=null)
            {
                fieldColumnsOrder = new string[ldColumnsOrder.Length];
                ldColumnsOrder.CopyTo(fieldColumnsOrder,0);
            }
            //fieldColumnsOrder = ldColumnsOrder;
            if (ldAdvancedColumns!=null)
            {
                fieldAdvancedColumns = new AdvancedColumn[ldAdvancedColumns.Length];
                ldAdvancedColumns.CopyTo(fieldAdvancedColumns,0);
            }
            //fieldAdvansedColumns = ldAdvansedColumns;
        }


        public void Init(LoadingCustomizationStruct other)
        {
            Init(other.ColumnsSort, other.LimitFunction, other.LoadingTypes, other.View, other.AdvancedColumns, other.ColumnsOrder);
            this.Distinct = other.Distinct;
            this.InitDataCopy = other.InitDataCopy;
            this.ReturnTop = other.ReturnTop;
            this.RankPropertyName = other.RankPropertyName;
            this.PartitionPropertyName = other.PartitionPropertyName;
            this.RowNumber =(other.RowNumber==null)?null:new RowNumberDef(other.RowNumber.StartRow, other.RowNumber.EndRow);
        }

        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType,string View)
        {
            if (Information.GetView(View, DataObjectType) == null)
            {
                throw new Exception("Обратитесь к разработчику: в методе GetSimpleStruct указано неверное представление \"" + View + "\" для типа \"" + DataObjectType.AssemblyQualifiedName + "\"");
            }
            LoadingCustomizationStruct lcs = new LoadingCustomizationStruct(null);
            lcs.Init(null,null,new Type[]{DataObjectType},Information.GetView(View,DataObjectType),null);
            return lcs;
        }

        static public LoadingCustomizationStruct GetSimpleStruct(View View)
        {
            return GetSimpleStruct(View.DefineClassType, View);
        }

        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType,View  View)
        {
            LoadingCustomizationStruct lcs = new LoadingCustomizationStruct(null);
            lcs.Init(null,null,new Type[]{DataObjectType},View,null);
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
                return v2 == null;
            if (v2 == null)
                return false;
            return v1.ToString(true) == v2.ToString(true);
        }

        private bool FunctionEquals(Function f1, Function f2)
        {
            if (f1 == null)
                return f2 == null;
            if (f2 == null)
                return false;
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

            return Equals(other.creatorKey, creatorKey) && Utils.ArraysEqual(other.fieldColumnsSort, fieldColumnsSort, false)
                   && FunctionEquals(other.fieldLimitFunction, fieldLimitFunction)
                   && Utils.ArraysEqual(other.fieldLoadingTypes, fieldLoadingTypes, false)
                   && ViewsEquals(other.fieldView, fieldView) && Utils.ArraysEqual(other.fieldColumnsOrder, fieldColumnsOrder, false)
                   && Utils.ArraysEqual(other.fieldAdvancedColumns, fieldAdvancedColumns, false)
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
                result = (result * 397) ^ (fieldAdvancedColumns != null ? fieldAdvancedColumns.GetHashCode() : 0);
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