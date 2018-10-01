namespace ICSSoft.STORMNET.FunctionalLanguage.SQLWhere
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// делегат для конвертации значений на FunctionalLanguage в значения на SQL
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate string delegateConvertValueToQueryValueString(object value);
    /// <summary>
    /// Делегат для помещения в скобки-кавычки идентификаторов
    /// </summary>
    /// <param name="identifier">идентификатор</param>
    /// <returns>идентификатор в скобках-кавычках</returns>
    public delegate string delegatePutIdentifierToBrackets(string identifier);

    /// <summary>
    /// Определение языка ограничений для конструирования ограничивающих функций
    /// </summary>
    [NotStored]
    public class SQLWhereLanguageDef : FunctionalLanguageDef
    {


        /// <summary>
        /// Конструктор по-умолчанию (CaseInsensitive берётся из конфига с флагом CaseInsensitive).
        /// </summary>
        public SQLWhereLanguageDef()
        {
            string settStr = System.Configuration.ConfigurationManager.AppSettings["CaseInsensitive"];
            bool settBool;
            if (bool.TryParse(settStr, out settBool))
            {
                _caseInsensitive = settBool;
            }
        }

        ///// <summary>
        ///// Сервис данных для построения подзапросов. Если не указан, используется DataServiceProvider.DataService
        ///// </summary>
        //public Business.IDataService DataService
        //{
        //    get
        //    {
        //        if (m_objDataService != null)
        //        {
        //            return m_objDataService;
        //        }
        //        else
        //        {
        //            m_objDataService = Business.DataServiceProvider.DataService;
        //            return m_objDataService;
        //        }
        //    }
        //    set
        //    {
        //        m_objDataService = value;
        //    }
        //}


        /// <summary>
        /// Получить funcIN для набора первичных ключей
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Function Keys2Function(object[] keys)
        {
            var lg = FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
            var vd = new VariableDef(lg.GetObjectTypeForNetType(keys[0].GetType()), SQLWhereLanguageDef.StormMainObjectKey);
            var pars = new object[keys.Length + 1];
            Array.Copy(keys, 0, pars, 1, keys.Length);
            pars[0] = vd;
            return lg.GetFunction(lg.funcIN, pars);
        }

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="caseInsensitive">Нечувствительный к регистру (если true, то для строк при сравнении делаем UPPER).</param>
        public SQLWhereLanguageDef(bool caseInsensitive)
        {
            _caseInsensitive = caseInsensitive;
        }

        /// <summary>
        /// Описане языка - одно на всех
        /// </summary>
        static private SQLWhereLanguageDef _lngDef = null;

        /// <summary>
        /// Получить описание языка
        /// </summary>
        static public SQLWhereLanguageDef LanguageDef
        {
            get
            {
                if (_lngDef != null)
                {
                    return _lngDef;
                }
                lock (_objNull)
                {
                    if (_lngDef == null)
                        _lngDef = new SQLWhereLanguageDef();
                    return /*new SQLWhereLanguageDef();*/_lngDef;
                }
            }
        }

        private bool _caseInsensitive = false;

        /// <summary>
        /// Чувствительность к регистру при построении ограничений (зависит от настроек БД. Если БД чувствительна к регистру, то нужно вправлять это свойство чтобы получить не чувтвительную к регистру систему)
        /// </summary>
        public bool CaseInsensitive
        {
            get { return _caseInsensitive; }
            set { _caseInsensitive = value;}
        }

        /// <summary>
        /// Получатель ObjectType по .NET-типу (для DataObject возвращается тип первичного ключа)
        /// </summary>
        /// <param name="type">.NET-тип</param>
        /// <returns>ObjectType-тип</returns>
        public override ObjectType GetObjectTypeForNetType(Type type)
        {
            if (type.IsSubclassOf(typeof(DataObject)))
            {
                // подменяем типом первичного ключа
                type = ((KeyGen.BaseKeyGenerator)Activator.CreateInstance(Information.GetKeyGeneratorType(type))).KeyType;
            }

            if (type.IsEnum)
                return StringType;
            if (type == typeof(KeyGen.KeyGuid))
                return GuidType;
            if (type == typeof(string))
                return StringType;
            if (type == typeof(int) || type == typeof(Int64))
                return NumericType;
            if (type == typeof(DateTime))
                return DateTimeType;
            if (type == typeof(bool))
                return BoolType;
            return base.GetObjectTypeForNetType(type);
        }


        private ObjectType fieldBoolType = new ObjectType("Boolean", "Логический", typeof(bool));
        private ObjectType fieldNumericType = new ObjectType("Numeric", "Число", typeof(decimal));
        private ObjectType fieldStringType = new ObjectType("String", "Текст", typeof(string));
        private ObjectType fieldDateTimeType = new ObjectType("DateTime", "Дата/Время", typeof(DateTime));
        private ObjectType fieldGuidType = new ObjectType("Guid", "Идентификатор", typeof(Guid));
        private ObjectType fieldQueryType = new ObjectType("Query", "SQL выражение", typeof(object));

        ///<summary>
        /// "Boolean","Логический"
        ///</summary>
        public ObjectType BoolType { get { return fieldBoolType; } }
        ///<summary>
        /// "Numeric","Число"
        ///</summary>
        public ObjectType NumericType { get { return fieldNumericType; } }
        ///<summary>
        /// "String","Текст"
        ///</summary>
        public ObjectType StringType { get { return fieldStringType; } }
        ///<summary>
        /// "DateTime","Дата/Время"
        ///</summary>
        public ObjectType DateTimeType { get { return fieldDateTimeType; } }
        ///<summary>
        /// "Guid","Идентификатор"
        ///</summary>
        public ObjectType GuidType { get { return fieldGuidType; } }
        ///<summary>
        /// "Query","SQL выражение"
        ///</summary>
        public ObjectType QueryType { get { return fieldQueryType; } }

        ///<summary>
        /// ISNULL
        ///</summary>
        public const string cfuncIsNull ="ISNULL";
        public readonly string funcIsNull=cfuncIsNull;
        ///<summary>
        /// NOT
        ///</summary>
        public const string cfuncNOT ="NOT";
        public readonly string funcNOT = cfuncNOT;
        ///<summary>
        /// OR
        ///</summary>
        public const string cfuncOR ="OR";
        public readonly string funcOR = cfuncOR;
        ///<summary>
        /// AND
        ///</summary>
        public const string cfuncAND ="AND";
        public readonly string funcAND = cfuncAND;
        ///<summary>
        /// +
        ///</summary>
        public const string cfuncPlus ="+";
        public readonly string funcPlus = cfuncPlus;
        ///<summary>
        /// *
        ///</summary>
        public const string  cfuncSub ="*";
        public readonly string funcSub = cfuncSub;
        ///<summary>
        /// -
        ///</summary>
        public const string cfuncMinus ="-";
        public readonly string funcMinus = cfuncMinus;
        ///<summary>
        /// /
        ///</summary>
        public const string cfuncDiv ="/";
        public readonly string funcDiv = cfuncDiv;
        ///<summary>
        /// LIKE
        ///</summary>
        public const string cfuncLike ="LIKE";
        public readonly string funcLike = cfuncLike;
        ///<summary>
        /// &lt;
        ///</summary>
        public const string cfuncL ="<";
        public readonly string funcL = cfuncL;
        ///<summary>
        /// &lt;=
        ///</summary>
        public const string cfuncLEQ ="<=";
        public readonly string funcLEQ = cfuncLEQ;
        ///<summary>
        /// =
        ///</summary>
        public const string cfuncEQ ="=";
        public readonly string funcEQ =cfuncEQ;
        ///<summary>
        /// &gt;=
        ///</summary>
        public const string cfuncGEQ =">=";
        public readonly string funcGEQ = cfuncGEQ;
        ///<summary>
        /// &gt;
        ///</summary>
        public const string cfuncG =">";
        public readonly string funcG = cfuncG;
        ///<summary>
        /// &lt;&gt;
        ///</summary>
        public const string cfuncNEQ ="<>";
        public readonly string funcNEQ = cfuncNEQ;
        ///<summary>
        /// IN
        ///</summary>
        public const string cfuncIN ="IN";
        public readonly string funcIN = cfuncIN;
        ///<summary>
        /// BETWEEN
        ///</summary>
        public const string cfuncBETWEEN ="BETWEEN";
        public readonly string funcBETWEEN = cfuncBETWEEN;
        ///<summary>
        /// SQL
        ///</summary>
        public const string cfuncSQL = "SQL";
        public readonly string funcSQL = cfuncSQL;


        ///<summary>
        /// Если в IN будет участвовать один объект, то IN заменится на =
        ///</summary>
        public static bool OptimizeINOperator = true;



        private string fQueryLikeAnyStringSymbol = "%";
        private string fQueryLikeAnyCharacterSymbol = "_";
        ///<summary>
        /// Получить символ, отвечающий за любую строку (по-умолчанию это "%")
        ///</summary>
        public virtual string QueryLikeAnyStringSymbol { get { return fQueryLikeAnyStringSymbol; } set { fQueryLikeAnyStringSymbol = value; } }
        /// <summary>
        /// Получить символ, отвечающий за любой символ в строке (по-умолчанию это "_")
        /// </summary>
        public virtual string QueryLikeAnyCharacterSymbol { get { return fQueryLikeAnyCharacterSymbol; } set { fQueryLikeAnyCharacterSymbol = value; } }

        private string fUserLikeAnyStringSymbol = "*";
        private string fUserLikeAnyCharacterSymbol = "_";
        ///<summary>
        /// Символ, который вводит пользователь, чтобы обозначить любую строку (по-умолчанию это "*")
        ///</summary>
        public virtual string UserLikeAnyStringSymbol { get { return fUserLikeAnyStringSymbol; } set { fUserLikeAnyStringSymbol = value; } }

        /// <summary>
        /// Символ, который вводит пользователь, чтобы обозначить любой символ (по-умолчанию это "_")
        /// </summary>
        public virtual string UserLikeAnyCharacterSymbol { get { return fUserLikeAnyCharacterSymbol; } set { fUserLikeAnyCharacterSymbol = value; } }

        /// <summary>
        /// Перенаправитель для обработки параметров: value is Function или value is VariableDef или это просто значение
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertValue"></param>
        /// <param name="convertIdentifier"></param>
        /// <returns></returns>
        public virtual string SQLTranslSwitch(object value, delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier, ref List<string> OTBSubqueries, Business.StorageStructForView[] storageStruct, Business.SQLDataService DataService)
        {
            if (value is Function)
            {
                return ((value as Function).FunctionDef.Language as SQLWhereLanguageDef).SQLTranslFunction((value as Function), convertValue, convertIdentifier,ref OTBSubqueries,storageStruct, DataService);
            }
            if (value is VariableDef)
            {
                
                if ((value as VariableDef).Language != null)
                {
                    //					if ((value as VariableDef).Type==SQLWhereLanguageDef.LanguageDef.BoolType)
                    //					{
                    //						return string.Format("{0}=1",((value as VariableDef).Language as SQLWhereLanguageDef).SQLTranslVariable((value as VariableDef),convertValue,convertIdentifier));
                    //					}
                    //					else
                    //					{
                    return ((value as VariableDef).Language as SQLWhereLanguageDef).SQLTranslVariable((value as VariableDef), convertValue, convertIdentifier);
                    //					}
                }
                return SQLTranslVariable((value as VariableDef), convertValue, convertIdentifier);
            }
            return convertValue(value);
        }

        /// <summary>
        /// Транслировать в SQL переменную
        /// </summary>
        /// <param name="value">переменная</param>
        /// <param name="convertValue">конвертилка выражений</param>
        /// <param name="convertIdentifier">помещатель в скобки-кавычки</param>
        /// <returns></returns>
        protected virtual string SQLTranslVariable(VariableDef value, delegateConvertValueToQueryValueString convertValue, delegatePutIdentifierToBrackets convertIdentifier)
        {
            return convertIdentifier(value.StringedView);
        }

        /// <summary>
        /// Транслировать в SQL функцию
        /// </summary>
        /// <param name="value">функция</param>
        /// <param name="convertValue">конвертилка выражений</param>
        /// <param name="convertIdentifier">помещатель в скобки-кавычки</param>
        /// <param name="OTBSubqueries"> подзапросы для выполнения функции </param>
        /// <returns></returns>
        protected virtual string SQLTranslFunction(
            Function value,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier,
            ref List<string> OTBSubqueries,
            Business.StorageStructForView[] StorageStruct,
            ICSSoft.STORMNET.Business.SQLDataService DataService)
        {
            if (value.Parameters.Count == 2)
            {
                if (value.FunctionDef.StringedView == "=" &&
                    ((value.Parameters[1] == null) || (value.Parameters[1].GetType() == typeof(string) && ((string)value.Parameters[1]) == string.Empty)))
                {
                    return "(" + SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + " IS NULL )";
                }
            }

            var valueType = value.Parameters[0].GetType();
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
                valueType = Nullable.GetUnderlyingType(valueType);

            switch (value.FunctionDef.StringedView)
            {
                case "SQL":
                    return value.Parameters[0].ToString();
                case "ISNULL":
                    return "(" + SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier,ref OTBSubqueries, StorageStruct,DataService) + " IS NULL )";
                case "NOT":
                    string result = string.Empty;
                    if (value.Parameters[0] is VariableDef)
                    {
                        VariableDef vdf = (VariableDef)value.Parameters[0];

                        var vdType = vdf.Type.NetCompatibilityType;
                        if (vdType.IsGenericType && vdType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            vdType = Nullable.GetUnderlyingType(vdType);

                        if (vdType == SQLWhereLanguageDef.LanguageDef.BoolType.NetCompatibilityType)
                        {
                            return string.Format("( NOT ({0}={1}))", convertIdentifier(vdf.StringedView), convertValue(true));
                        }
                    }
                    else if (valueType == typeof(bool))
                    {
                        return string.Format("( NOT ({0}={1}))", convertValue((bool)value.Parameters[0]), convertValue(true));
                    }

                    if (result == string.Empty)
                    {
                        result = "( NOT " + SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + " )";
                    }
                    return result;
                case "LIKE":
                    string par1 = string.Empty, par2 = string.Empty;

                    par1 = AddUpper(value.Parameters[0], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService);
                    par2 = AddUpper(value.Parameters[1], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService);

                    if (value.Parameters[1] != null && value.Parameters[1].GetType() == typeof(string))
                        par2 = par2.Replace(UserLikeAnyStringSymbol, QueryLikeAnyStringSymbol).Replace(UserLikeAnyCharacterSymbol, QueryLikeAnyCharacterSymbol);
                    return string.Format("( {0} like {1} )", par1, par2);
                case "OR":
                case "AND":
                case "+":
                case "*":
                case "-":
                case "/":
                case "<":
                case "<=":
                case "=":
                case ">=":
                case ">":
                case "<>":
                    string[] pars = null;
                    pars = new string[value.Parameters.Count];
                    for (int i = 0; i < pars.Length; i++)
                    {
                        pars[i] = string.Empty;
                        if (value.Parameters[i] is VariableDef)
                        {
                            var vdf = (VariableDef)value.Parameters[i];

                            Type vdType = vdf.Type.NetCompatibilityType;
                            if (vdType.IsGenericType && vdType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                vdType = Nullable.GetUnderlyingType(vdType);

                            if (vdType == LanguageDef.BoolType.NetCompatibilityType)
                            {
                                String funcStringed = value.FunctionDef.StringedView;

                                // Если не применена операция "=" или "<>" для двух булевых типов.
                                if (!(((funcStringed == "=") || (funcStringed == "<>")) && (pars.Length == 2)))
                                {
                                    pars[i] = string.Format("(({0}={1}))", convertIdentifier(vdf.StringedView), convertValue(true));
                                }
                            }
                        }

                        Type parType = null;
                        if (value.Parameters[i] != null)
                        {
                            parType = value.Parameters[i].GetType();
                            if (parType.IsGenericType && parType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                parType = Nullable.GetUnderlyingType(parType);
                        }

                        // Для И и ИЛИ.
                        if ((parType == typeof(bool)) && (value.FunctionDef.StringedView == LanguageDef.funcAND || value.FunctionDef.StringedView == LanguageDef.funcOR))
                            pars[i] = string.Format("(({0}={1}))", convertValue(value.Parameters[i]), convertValue(true));

                        if (pars[i] == string.Empty) 
                            pars[i] = AddUpper(value.Parameters[i], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService);

                        #region Обработка ситуации, когда операндом у функции "=" или "<>" идут функции.

                        if (((value.FunctionDef.StringedView == "=") || (value.FunctionDef.StringedView == "<>")) &&
                            (value.Parameters[i] is Function) &&
                            ((Function)value.Parameters[i]).FunctionDef.ReturnType.NetCompatibilityType ==
                            LanguageDef.BoolType.NetCompatibilityType)
                        {
                            var func = (Function)value.Parameters[i];
                            String funcStringed = func.FunctionDef.StringedView;
                            String trimmedAllPars = TrimAndUnbrake(pars[i]);

                            // Упрощение запроса: если true, то не нужен case.
                            if ((funcStringed.ToLower() == "true") || (trimmedAllPars == "0"))
                            {
                                pars[i] = "1";
                            }
                            else if (((funcStringed == funcNOT) && // если функция not
                                      (func.Parameters.Count == 1) && // у нее 1 параметр
                                      (func.Parameters[0] is Function) && // этот параметр функция
                                      (((Function)func.Parameters[0]).FunctionDef.StringedView.ToLower() == "true")) ||
                                     (trimmedAllPars == "1"))
                            {
                                pars[i] = "0";
                            }
                            else
                            {
                                pars[i] = String.Format("(case when {0} then 1 else 0 end)", pars[i]);
                            }
                        }

                        #endregion
                    }

                    result = "( " + string.Join(" " + value.FunctionDef.StringedView + " ", pars) + ")";
                    return result;
                case "IN":
                    if (OptimizeINOperator && value.Parameters.Count == 2 && (!
                        (value.Parameters[1] is Function && (value.Parameters[1] as Function).FunctionDef.StringedView == "SQL")
                        ))
                    {
                        pars = new string[value.Parameters.Count];
                        for (int i = 0; i < pars.Length; i++)
                        {
                            pars[i] = AddUpper(value.Parameters[i], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService);
                        }
                        return "( " + string.Join(" " + "=" + " ", pars) + ")";
                    }
                    else if (value.Parameters.Count == 1)
                    {
                        return SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + " is null";
                    }
                    pars = new string[value.Parameters.Count - 1];
                    for (int i = 1; i < value.Parameters.Count; i++)
                    {
                        pars[i - 1] = AddUpper(value.Parameters[i], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService);
                    }
                    return "( " + AddUpper(value.Parameters[0], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + " in (" + string.Join(",", pars) + "))";

                case "BETWEEN":
                    return "( " + AddUpper(value.Parameters[0], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + " between " + AddUpper(value.Parameters[1], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + " and " + AddUpper(value.Parameters[2], convertValue, convertIdentifier, ref OTBSubqueries, StorageStruct, DataService) + ")";
                default:
                    throw new Exception("Not found function :" + value.FunctionDef.StringedView);
            }

        }

        /// <summary>
        /// Убирает все пробелы и удаляет скобки с краев, например, "(( ( 0))" превратится в "(0"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string TrimAndUnbrake(string param)
        {
            if (string.IsNullOrEmpty(param)) return String.Empty;

            String res = param.Replace(" ", string.Empty);

            if (string.IsNullOrEmpty(res)) return String.Empty;

            while ((res[0] == '(') && (res[res.Length - 1] == ')'))
            {
                //здесь res.Length точно > 1
                res = res.Substring(1, res.Length - 2);
            }

            return res;
        }

        /// <summary>
        /// В зависимости от CaseInsensitive добавляет UPPER
        /// </summary>
        /// <param name="value">Function.Parameters[i]</param>
        /// <param name="convertValue"></param>
        /// <param name="convertIdentifier"></param>
        /// <returns></returns>
        protected string AddUpper(object value, delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier, ref List<string> OTBSubqueries, Business.StorageStructForView[] storageStruct, Business.SQLDataService DataService)
        {
            string retStr;
            if (CaseInsensitive && (value is VariableDef && ((VariableDef)value).Type.StringedView == "String"))
            {
                retStr = "UPPER( " + SQLTranslSwitch(value, convertValue, convertIdentifier, ref OTBSubqueries, storageStruct,  DataService) + " )";
                return retStr;
            }
            if (CaseInsensitive && value is String)
            {
                retStr = SQLTranslSwitch(value.ToString().ToUpper(), convertValue, convertIdentifier, ref OTBSubqueries, storageStruct, DataService);
                return retStr;
            }
            retStr = SQLTranslSwitch(value, convertValue, convertIdentifier,ref OTBSubqueries, storageStruct, DataService);
            return retStr;
        }

        /// <summary>
        /// Преобразовать значение в SQL строку
        /// </summary>
        /// <param name="function">Функция</param>
        /// <param name="convertValue">делегат для преобразования констант</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов</param>
        /// <returns></returns>
        public static string ToSQLString(Function function,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier,
            ref List<string> OTBSubqueries,
            Business.StorageStructForView[] StorageStruct,
            Business.SQLDataService DataService)
        {
            return (function.FunctionDef.Language as SQLWhereLanguageDef).SQLTranslFunction(function, convertValue, convertIdentifier,ref OTBSubqueries, StorageStruct, DataService);
        }

        /// <summary>
        /// return null;
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public virtual string[] GetExistingVariableNames(Function f)
        {
            return null;
        }

        /// <summary>
        /// Количество функций (return 100)
        /// </summary>
        public override int MaxFuncID
        {
            get
            {
                return 100;
            }
        }


        private static string _objNull = "CONST";


        private bool LikeUppered(string checkString, string mask, char singleCharMaskSymbol = '_', char zeroOrMoreCharMaskSymbol = '*')
        {
            if (mask == "" && checkString != "")
                return false;
            else if (checkString == "" && mask == "")
                return true;
            else if (mask == zeroOrMoreCharMaskSymbol.ToString())
                return true;
            else if (checkString.Length == 1 && mask == singleCharMaskSymbol.ToString())
                return true;
            else if (checkString == "" && mask != "")
                return false;
            else
            {
                if (mask[0] == singleCharMaskSymbol)
                    return LikeUppered(checkString.Substring(1), mask.Substring(1), singleCharMaskSymbol, zeroOrMoreCharMaskSymbol);
                else if (mask[0] == zeroOrMoreCharMaskSymbol)
                    return LikeUppered(checkString, mask.Substring(1), singleCharMaskSymbol, zeroOrMoreCharMaskSymbol) ||
                           LikeUppered(checkString.Substring(1), mask, singleCharMaskSymbol, zeroOrMoreCharMaskSymbol) ||
                           LikeUppered(checkString.Substring(1), mask.Substring(1), singleCharMaskSymbol, zeroOrMoreCharMaskSymbol);
                else if (checkString[0] == mask[0])
                    return LikeUppered(checkString.Substring(1), mask.Substring(1), singleCharMaskSymbol, zeroOrMoreCharMaskSymbol);
                else
                    return false;
            }
        }


        private  bool Like(string checkString, string mask, char singleCharMaskSymbol = '_', char zeroOrMoreCharMaskSymbol = '*')
        {
            return LikeUppered(checkString.ToUpper(), mask.ToUpper(), singleCharMaskSymbol, zeroOrMoreCharMaskSymbol);
        }



        /// <summary>
        /// Инициализация определений функций языка (для определения связки количества и типов параметров)
        /// </summary>
        protected override void InitializeDefs()
        {
            lock (_objNull)
            {
                fieldUpFunctionType = fieldBoolType;

                Types.AddRange(
                    fieldBoolType,
                    fieldNumericType,
                    fieldStringType,
                    fieldDateTimeType,
                    fieldGuidType);


                Functions.AddRange(
                    //ISNULL
                    new FunctionDef(1, fieldBoolType, "ISNULL", "НЕ ЗАПОЛНЕНО", "({0} не заполнено)", (x) => x[0] is null,  new FunctionParameterDef(fieldBoolType)),
                    new FunctionDef(2, fieldBoolType, "ISNULL", "НЕ ЗАПОЛНЕНО", "({0} не заполнено)", (x) => x[0] is null, new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(3, fieldBoolType, "ISNULL", "НЕ ЗАПОЛНЕНО", "({0} не заполнено)", (x) => x[0] is null, new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(4, fieldBoolType, "ISNULL", "НЕ ЗАПОЛНЕНО", "({0} не заполнено)", (x) => x[0] is null, new FunctionParameterDef(fieldDateTimeType)),
                    new FunctionDef(5, fieldBoolType, "ISNULL", "НЕ ЗАПОЛНЕНО", "({0} не заполнено)", (x) => x[0] is null, new FunctionParameterDef(fieldGuidType)),
                    //SQLQUERY
                    new FunctionDef(6, fieldQueryType, "SQL", "SQL", "SQL ('{0}')", true, new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(7, fieldBoolType, "SQL", "SQL", "SQL ('{0}')", true, new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(8, fieldNumericType, "SQL", "SQL", "SQL ('{0}')", true, new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(9, fieldStringType, "SQL", "SQL", "SQL ('{0}')", true, new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(10, fieldDateTimeType, "SQL", "SQL", "SQL ('{0}')", true, new FunctionParameterDef(fieldStringType)),
                    //NOT
                    new FunctionDef(11, fieldBoolType, "NOT", "НЕ", "НЕ {0}", (x) => !ObjectToBool(x[0]), new FunctionParameterDef(fieldBoolType)),
                    //OR
                    new FunctionDef(12, fieldBoolType, "OR", "ИЛИ", "({* ИЛИ})", (x) => x.Select(y=> ObjectToBool(y)).Cast<bool>().Aggregate((q, w) => q || w) , new FunctionParameterDef(fieldBoolType), new FunctionParameterDef(fieldBoolType, true)),
                    //AND
                    new FunctionDef(13, fieldBoolType, "AND", "И", "{* И}", (x) => x.Select(y => ObjectToBool(y)).Cast<bool>().Aggregate((q, w) => q && w), new FunctionParameterDef(fieldBoolType), new FunctionParameterDef(fieldBoolType, true)),
                    //+
                    new FunctionDef(14, fieldNumericType, "+", "+", "({* +})", (x) => x.Select(y => ObjectToDecimal(y)).Cast<decimal>().Aggregate((q, w) => q + w), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType, true)),
                    //*
                    new FunctionDef(15, fieldNumericType, "*", "*", "{* *}", (x) => x.Select(y => ObjectToDecimal(y)).Cast<decimal>().Aggregate((q, w) => q * w), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType, true)),
                    //-
                    new FunctionDef(16, fieldNumericType, "-", "-", "({0} - {1})", (x) => ObjectToDecimal(x[0]) - ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    // /
                    new FunctionDef(17, fieldNumericType, "/", "/", "({0} / {1})", (x) => ObjectToDecimal(x[0]) / ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    //LIKE
                    new FunctionDef(18, fieldBoolType, "LIKE", "ПО МАСКЕ", "({0} УДОВЛ.МАСКЕ {1})",(x)=>Like(ObjectToString(x[0]),ObjectToString(x[1])),  new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    //<
                    new FunctionDef(19, fieldBoolType, "<", "<", "({0} < {1})", (x) => ObjectToDecimal(x[0]) < ObjectToDecimal(x[1]),  new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(20, fieldBoolType, "<", "<", "({0} < {1})", (x) => ObjectToString(x[0]).CompareTo(ObjectToString(x[1]))<0, new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(21, fieldBoolType, "<", "<", "({0} < {1})", (x) => ObjectToDateTime(x[0]) < ObjectToDateTime(x[1]), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    //<=
                    new FunctionDef(22, fieldBoolType, "<=", "<=", "({0} <= {1})", (x) => ObjectToDecimal(x[0]) <= ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(23, fieldBoolType, "<=", "<=", "({0} <= {1})", (x) => ObjectToString(x[0]).CompareTo(ObjectToString(x[1])) <= 0,new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(24, fieldBoolType, "<=", "<=", "({0} <= {1})", (x) => ObjectToDateTime(x[0]) <= ObjectToDateTime(x[1]), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    //>
                    new FunctionDef(25, fieldBoolType, ">", ">", "({0} > {1})", (x) => ObjectToDecimal(x[0]) > ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(26, fieldBoolType, ">", ">", "({0} > {1})", (x) => ObjectToString(x[0]).CompareTo(ObjectToString(x[1])) > 0, new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(27, fieldBoolType, ">", ">", "({0} > {1})", (x) => ObjectToDateTime(x[0]) > ObjectToDateTime(x[1]), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    //>=
                    new FunctionDef(28, fieldBoolType, ">=", ">=", "({0} >= {1})", (x) => ObjectToDecimal(x[0]) >= ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(29, fieldBoolType, ">=", ">=", "({0} >= {1})", (x) => ObjectToString(x[0]).CompareTo(ObjectToString(x[1])) >= 0, new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(30, fieldBoolType, ">=", ">=", "({0} >= {1})", (x) => ObjectToDateTime(x[0]) > ObjectToDateTime(x[1]), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    //<>
                    new FunctionDef(31, fieldBoolType, "<>", "<>", "({0} <> {1})", (x) => ObjectToDecimal(x[0]) != ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(32, fieldBoolType, "<>", "<>", "({0} <> {1})", (x) => ObjectToString(x[0]).CompareTo(ObjectToString(x[1])) != 0, new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(33, fieldBoolType, "<>", "<>", "({0} <> {1})", (x) => ObjectToDateTime(x[0]) != ObjectToDateTime(x[1]), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    new FunctionDef(34, fieldBoolType, "<>", "<>", "({0} <> {1})", (x) => ObjectToGuid(x[0]) != ObjectToGuid(x[1]), new FunctionParameterDef(fieldGuidType), new FunctionParameterDef(fieldGuidType)),
                    //=
                    new FunctionDef(35, fieldBoolType, "=", "=", "{0} = {1}", (x) => ObjectToDecimal(x[0])== ObjectToDecimal(x[1]), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(36, fieldBoolType, "=", "=", "{0} = {1}", (x) => ObjectToString(x[0]).CompareTo(ObjectToString(x[1])) == 0, new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(37, fieldBoolType, "=", "=", "{0} = {1}", (x) => ObjectToDateTime(x[0]) == ObjectToDateTime(x[1]), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    new FunctionDef(38, fieldBoolType, "=", "=", "{0} = {1}", (x) => ObjectToGuid(x[0]) == ObjectToGuid(x[1]), new FunctionParameterDef(fieldGuidType), new FunctionParameterDef(fieldGuidType)),
                    //IN
                    new FunctionDef(39, fieldBoolType, "IN", "СРЕДИ ЗНАЧЕНИЙ", "({0} СРЕДИ {{{* ,}}})",(x)=> (x[0] == null) ? false : x.Skip(1).Select(y=>ObjectToDecimal(y)).Contains(ObjectToDecimal(x[0])),  new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType, true)),
                    new FunctionDef(40, fieldBoolType, "IN", "СРЕДИ ЗНАЧЕНИЙ", "({0} СРЕДИ {{{* ,}}})", (x) =>(x[0]==null)?false:x.Skip(1).Select(y => ObjectToString(y)).Contains(ObjectToString(x[0])), new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType, true)),
                    new FunctionDef(41, fieldBoolType, "IN", "СРЕДИ ЗНАЧЕНИЙ", "({0} СРЕДИ {{{* ,}}})", (x) => (x[0] == null) ? false : x.Skip(1).Select(y => ObjectToDateTime(y)).Contains(ObjectToDateTime(x[0])), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType, true)),
                    new FunctionDef(42, fieldBoolType, "IN", "СРЕДИ ЗНАЧЕНИЙ", "({0} СРЕДИ {{{* ,}}})", (x) => (x[0] == null) ? false : x.Skip(1).Select(y => ObjectToGuid(y)).Contains(ObjectToGuid(x[0])), new FunctionParameterDef(fieldGuidType), new FunctionParameterDef(fieldGuidType, true)),
                    //BETWEEN
                    new FunctionDef(43, fieldBoolType, "BETWEEN", "ДИАПАЗОН", "({0} в диапазоне от {1} до {2})",(x)=>(ObjectToDecimal(x[0])>=ObjectToDecimal(x[1]))&&(ObjectToDecimal(x[0])<=ObjectToDecimal(x[2])), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType), new FunctionParameterDef(fieldNumericType)),
                    new FunctionDef(44, fieldBoolType, "BETWEEN", "ДИАПАЗОН", "({0} в диапазоне от {1} до {2})", (x) => (ObjectToString(x[0]).CompareTo(ObjectToString(x[1]))>=0) && (ObjectToString(x[0]).CompareTo(ObjectToString(x[2]))<=0), new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType), new FunctionParameterDef(fieldStringType)),
                    new FunctionDef(45, fieldBoolType, "BETWEEN", "ДИАПАЗОН", "({0} в диапазоне от {1} до {2})", (x) => (ObjectToDateTime(x[0]) >= ObjectToDateTime(x[1])) && (ObjectToDateTime(x[0]) <= ObjectToDateTime(x[2])), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType), new FunctionParameterDef(fieldDateTimeType)),
                    //Равенство Boolean
                    new FunctionDef(46, fieldBoolType, "=", "=", "{0} = {1}",(x)=>ObjectToBool(x[0])==ObjectToBool(x[1]), new FunctionParameterDef(BoolType), new FunctionParameterDef(BoolType)),
                    //Неавенство Boolean
                    new FunctionDef(47, fieldBoolType, "<>", "<>", "{0} <> {1}", (x) => ObjectToBool(x[0]) != ObjectToBool(x[1]), new FunctionParameterDef(BoolType), new FunctionParameterDef(BoolType))
                    );
            }
        }
    }

}
