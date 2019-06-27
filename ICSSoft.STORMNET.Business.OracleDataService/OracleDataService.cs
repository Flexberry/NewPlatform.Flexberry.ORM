namespace ICSSoft.STORMNET.Business
{
    using System;
    //using System.Data.OracleClient;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Security.Cryptography;
    //using FunctionalLanguage.SQLWhere;
    //using FunctionalLanguage;
    //using Windows.Forms;
    //using static Windows.Forms.ExternalLangDef;
    //using Services;
    using System.Collections;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    //using NewPlatform.Flexberry.ORM.ODataService.Functions;
    using ICSSoft.STORMNET.Windows.Forms;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using System.Collections.Generic;
    using ICSSoft.Services;
    using System.Collections.Specialized;
    using Oracle.ManagedDataAccess.Client;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Security;
    using static ICSSoft.STORMNET.Windows.Forms.ExternalLangDef;

    //using Security;
    //using Audit;

    /// <summary>
    /// Сервис данных для доступа к данным Oracle
    /// </summary>
    public class OracleDataService : ICSSoft.STORMNET.Business.SQLDataService
    {
        public const int MaxBytes = 30;

        /// <summary>
        /// Создание сервиса данных для Oracle без параметров.
        /// </summary>
        public OracleDataService()
        {
        }

        /// <summary>
        /// Создание сервиса данных для Oracle с указанием настроек проверки полномочий.
        /// </summary>
        /// <param name="securityManager">Сконструированный менеджер полномочий.</param>
        public OracleDataService(ISecurityManager securityManager)
            : base(securityManager)
        {
        }

        /// <summary>
        /// Создание сервиса данных для Oracle с указанием настроек проверки полномочий.
        /// </summary>
        /// <param name="securityManager">Сенеджер полномочий.</param>
        /// <param name="auditService">Сервис аудита.</param>
        public OracleDataService(ISecurityManager securityManager, IAuditService auditService)
            : base(securityManager, auditService)
        {
        }

        /// <summary>
        /// Преобразовать значение в SQL строку
        /// </summary>
        /// <param name="function">Функция</param>
        /// <param name="convertValue">делегат для преобразования констант</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов</param>
        /// <returns></returns>
        /// 
        public override string FunctionToSql(SQLWhereLanguageDef sqlLangDef, 
            Function function, 
            delegateConvertValueToQueryValueString convertValue, 
            delegatePutIdentifierToBrackets convertIdentifier, 
            ref List<string> OTBSubquery, 
            StorageStructForView[] StorageStruct)
        //public override string FunctionToSql(
        //    SQLWhereLanguageDef sqlLangDef,
        //    Function value,
        //    delegateConvertValueToQueryValueString convertValue,
        //    delegatePutIdentifierToBrackets convertIdentifier)
        {
            ExternalLangDef langDef = sqlLangDef as ExternalLangDef;

            if (function.FunctionDef.StringedView == "TODAY")
            {
                return "sysdate";
            }

            if (
                function.FunctionDef.StringedView == "YearPart" ||
                function.FunctionDef.StringedView == "MonthPart" ||
                function.FunctionDef.StringedView == "DayPart")
            {
                return string.Format("EXTRACT ({0} FROM {1})", function.FunctionDef.StringedView.Substring(0, function.FunctionDef.StringedView.Length - 4),
                    langDef.SQLTranslSwitch(function.Parameters[0],  convertValue, convertIdentifier,ref OTBSubquery, StorageStruct, this));
            }

            if (
                function.FunctionDef.StringedView == "hhPart")
            {
                return string.Format("TO_CHAR({1}, \'{0}\')", "HH24",
                    langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
            }

            if (function.FunctionDef.StringedView == "miPart")
            {
                return string.Format("TO_CHAR({1}, \'{0}\')", "MI",
                    langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
            }

            if (function.FunctionDef.StringedView == "DayOfWeek")
            {
                // здесь требуется преобразование из DATASERVICE
                return string.Format("TO_CHAR({1}, \'{0}\')", "D",
                    langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
            }

            if (function.FunctionDef.StringedView == langDef.funcDayOfWeekZeroBased)
            {
                throw new NotImplementedException(string.Format("Function {0} is not implemented for Oracle", langDef.funcDayOfWeekZeroBased));
            }

            if (function.FunctionDef.StringedView == langDef.funcDaysInMonth)
            {
                // здесь требуется преобразование из DATASERVICE
                string.Format("to_char(last_day(to_date('01.'||{0}||'.'||{1},'dd.mm.yyyy')),'dd')", 
                    langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this), langDef.SQLTranslSwitch(function.Parameters[1], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
                return string.Empty;
            }

            if (function.FunctionDef.StringedView == "OnlyDate")
            {
                return string.Format("TRUNC({0})",
                    langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
            }

            if (function.FunctionDef.StringedView == "CurrentUser")
            {
                return string.Format("'{0}'", CurrentUserService.CurrentUser.FriendlyName);

                // у нее нет параметров
                // langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier));
            }

            if (function.FunctionDef.StringedView == "OnlyTime")
            {
                return string.Format("TRUNC({0})",
                    langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
            }

            if (function.FunctionDef.StringedView == "DATEDIFF")
            {
                var ret = string.Empty;
                if (langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this) == "Year")
                {
                    ret = string.Format("EXTRACT (YEAR FROM {1}) - EXTRACT (YEAR FROM {0})",
                        langDef.SQLTranslSwitch(function.Parameters[1], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        langDef.SQLTranslSwitch(function.Parameters[2], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
                }
                else if (langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this) == "Month")
                {
                    ret = string.Format("(EXTRACT (YEAR FROM {1}) - EXTRACT (YEAR FROM {0})) * 12 + (EXTRACT (MONTH FROM {1}) - EXTRACT (MONTH FROM {0}))",
                        langDef.SQLTranslSwitch(function.Parameters[1], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        langDef.SQLTranslSwitch(function.Parameters[2], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
                }
                else if (langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this) == "Week")
                {
                    ret = string.Format("(TRUNC({1},'DAY') - TRUNC({0},'DAY'))/7",
                        langDef.SQLTranslSwitch(function.Parameters[1], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        langDef.SQLTranslSwitch(function.Parameters[2], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
                }
                else if (langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this) == "Day")
                {
                    ret = string.Format("TRUNC({1}) - TRUNC({0})",
                        langDef.SQLTranslSwitch(function.Parameters[1], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        langDef.SQLTranslSwitch(function.Parameters[2], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
                }
                else if (langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this) == "quarter")
                {
                    ret = string.Format("(EXTRACT (YEAR FROM {1}) - EXTRACT (YEAR FROM {0})) * 4 + (TO_CHAR({1}, 'Q') - TO_CHAR({0}, 'Q'))",
                        langDef.SQLTranslSwitch(function.Parameters[1], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        langDef.SQLTranslSwitch(function.Parameters[2], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this));
                }

                return ret;
            }

            if (function.FunctionDef.StringedView == "SUM" ||
                function.FunctionDef.StringedView == "AVG" ||
                function.FunctionDef.StringedView == "MAX" ||
                function.FunctionDef.StringedView == "MIN")
            {
                ICSSoft.STORMNET.Business.LoadingCustomizationStruct lcs = new ICSSoft.STORMNET.Business.LoadingCustomizationStruct(null);
                DetailVariableDef dvd = (DetailVariableDef)function.Parameters[0];
                lcs.LoadingTypes = new Type[] { dvd.View.DefineClassType };
                lcs.View = new View();
                lcs.View.DefineClassType = dvd.View.DefineClassType;
                lcs.View.AddProperty(dvd.ConnectMasterPorp);
                string[] prevRetVars = langDef.retVars;
                langDef.retVars = new string[] { dvd.ConnectMasterPorp };
                ArrayList al = new ArrayList();
                object par = langDef.TransformObject(function.Parameters[1], dvd.StringedView, al);
                foreach (string s in al)
                {
                    lcs.View.AddProperty(s);
                }

                string Slct = GenerateSQLSelect(lcs, false).Replace("STORMGENERATEDQUERY", "SGQ" + Guid.NewGuid().ToString().Replace("-", string.Empty));
                string CountIdentifier = convertIdentifier("g" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 29));

                // FunctionalLanguage.Function numFunc = (value.Parameters[1] as FunctionalLanguage.Function);

                string sumExpression = langDef.SQLTranslSwitch(par, convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this);

                string res = string.Empty;
                res = string.Format(
                    "( SELECT {0} From ( " +
                    "SELECT {6}({5}) {0},{1} from ( {4} )pip group by {1} ) " +
                    " ahh where {1} in ({3}",
                    CountIdentifier,
                    convertIdentifier(dvd.ConnectMasterPorp),
                    convertIdentifier(Information.GetClassStorageName(dvd.View.DefineClassType)),
                    convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[0]),

                    // convertIdentifier(dvd.OwnerConnectProp),
                    Slct,

                    // ВНИМАНИЕ ЗДЕСЬ ТРЕБУЕТСЯ ИЗМЕНИТь ISNULL на вычислитель в определенном DATASERVICE
                    "NVL(" + sumExpression + ",0)", function.FunctionDef.StringedView);
                for (int k = 0; k < dvd.OwnerConnectProp.Length; k++)
                {
                    res += "," + convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[k]);
                }

                res += "))";

                langDef.retVars = prevRetVars;
                return res;
            }

            if (function.FunctionDef.StringedView == langDef.funcCountWithLimit || function.FunctionDef.StringedView == "Count")
            {
                var lcs = new ICSSoft.STORMNET.Business.LoadingCustomizationStruct(null);
                var dvd = (DetailVariableDef)function.Parameters[0];
                lcs.LoadingTypes = new Type[] { dvd.View.DefineClassType };
                lcs.View = dvd.View.Clone();
                lcs.LimitFunction = function.FunctionDef.StringedView == langDef.funcCountWithLimit
                    ? langDef.TransformVariables((FunctionalLanguage.Function)function.Parameters[1], dvd.StringedView, null)
                    : langDef.GetFunction("True");
                var prevRetVars = langDef.retVars;
                langDef.retVars = new string[] { dvd.ConnectMasterPorp };
                var Slct = GenerateSQLSelect(lcs, true);
                var CountIdentifier = convertIdentifier("g" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 29));

                var res = string.Format(
                    "( NVL(  ( SELECT {0} From ( " +
                    "SELECT Count(*) {0},{1} from ( {4} )pip group by {1} ) " +
                    " ahh where {1} in ({3}",
                    CountIdentifier,
                    convertIdentifier(dvd.ConnectMasterPorp),
                    convertIdentifier(Information.GetClassStorageName(dvd.View.DefineClassType)),
                    convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[0]),
                    Slct);
                for (int k = 1; k < dvd.OwnerConnectProp.Length; k++)
                {
                    res += "," + convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[k]);
                }

                res += ")),0))";

                langDef.retVars = prevRetVars;
                return res;
            }

            if (function.FunctionDef.StringedView == langDef.funcToChar)
            {
                if (function.Parameters.Count == 2)
                {
                    return string.Format(
                        "SUBSTR(TO_CHAR({0}), 1, {1})",
                        langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        function.Parameters[1]);
                }

                if (function.Parameters.Count == 3)
                {
                    return string.Format(
                        "SUBSTR(TO_CHAR({0}, {2}), 1, {1})",
                        langDef.SQLTranslSwitch(function.Parameters[0], convertValue, convertIdentifier, ref OTBSubquery, StorageStruct, this),
                        function.Parameters[1],
                        DateFormats.GetOracleDateFormat((int)function.Parameters[2]));
                }
            }
            else
            {
                throw new NotImplementedException(string.Format(
                    "Функция {0} не реализована для Oracle", function.FunctionDef.StringedView));
            }

            return string.Empty;
        }

        /// <summary>
        /// Создание копии экземпляра сервиса данных.
        /// </summary>
        /// <returns>Копии экземпляра сервиса данных.</returns>
        public override object Clone()
        {
            var instance = (OracleDataService)base.Clone();
            instance.arParams = arParams;
            return instance;
        }

        /// <summary>
        /// Получить хэш-код идентификатора.
        /// </summary>
        /// <param name="identifier">идентификатор</param>
        public static int GetIdentifierHashCode(string identifier)
        {
            // В предыдущем варианте использовался  метод string.GetHashCode.
            // Однако, данный метод может срабатывать по-разному на 32 и 64-разрядных платформах, в разных доменах.
            // Официальная документация в связи с этим не рекомендует использовать полученный string.GetHashCode
            // хэш для хранения. Поэтому метод заменен данным. Его тестирование не проводилось, возможно, потребуется замена.
            int hashcode = 0;
            int MOD = 10007;
            int shift = 29;
            for (int i = 0; i < identifier.Length; i++)
            {
                hashcode = ((shift * hashcode) % MOD + identifier[i]) % MOD;
            }

            return hashcode;
        }

        /// <summary>
        /// Получить идентификатор, обработанный с учётом требований ORACLE на длину в 30 байт
        /// </summary>
        /// <param name="identifier">идентификатор</param>
        public static string GenerateShortName(string identifier)
        {
            var encoding = Encoding.UTF8;

            byte[] identifierBytes = encoding.GetBytes(identifier);

            if (identifierBytes.Length <= MaxBytes)
            {
                return identifier;
            }

            // Получаем хэш идентификатора (для формирования уникального имени).
            string identifierHash = GetIdentifierHashCode(identifier).ToString();

            // Урезаем основную часть идентификатора, чтобы дополнить хэшем.
            string shortIdentifier = encoding.GetString(identifierBytes, 0, 30 - 1 - identifierHash.Length);
            int lastIndex = shortIdentifier.Length - 1;
            if (identifier[lastIndex] != shortIdentifier[lastIndex])
            {
                // Убираем последний (неправильный) символ. Возможно для символов кирилицы, если остается один байт символа.
                shortIdentifier = shortIdentifier.Substring(0, lastIndex);
            }

            identifier = shortIdentifier + '_' + GetIdentifierHashCode(identifier);
            return identifier;
        }

        /// <summary>
        /// Получить идентификатор, заключенный в кавычки, с учётом требований ORACLE на длину
        /// </summary>
        /// <param name="identifier">идентификатор</param>
        public static string PrepareIdentifier(string identifier)
        {
            identifier = GenerateShortName(identifier);
            return '"' + identifier + '"';
        }

        /// <summary>
        /// Получить идентификатор, заключенный в кавычки, с учётом требований ORACLE на длину
        /// </summary>
        /// <param name="identifier">идентификатор</param>
        /// <returns></returns>
        public override string PutIdentifierIntoBrackets(string identifier)
        {
            return PrepareIdentifier(identifier);
        }

        public override System.Data.IDbConnection GetConnection()
        {
            return new Oracle.ManagedDataAccess.Client.OracleConnection(this.CustomizationString);
        }

        /// <summary>
        /// Вернуть ifnull выражение (для ORACLE используется ф-я NVL)
        /// </summary>
        /// <param name="identifiers">идентификаторы</param>
        /// <returns></returns>
        public override string GetIfNullExpression(params string[] identifiers)
        {
            string result = identifiers[identifiers.Length - 1];
            for (int i = identifiers.Length - 2; i >= 0; i--)
            {
                result = string.Concat("NVL(", identifiers[i], ", ", result, ")");
            }

            return result;
        }

        /// <summary>
        /// Reading data from database: read first part.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <param name="state">The reading state.</param>
        /// <param name="loadingBufferSize">The loading buffer size.</param>
        /// <returns>The readed objects from database.</returns>
        public override object[][] ReadFirst(string query, ref object state, int loadingBufferSize)
        {
            // Вообще, TOP заменяется в методе GenerateSQLSelect. Однако, базовый SQLDataService
            // может втсавлять TOP где угодно. Например, делает это в методе GetObjectIndexesWithPks.
            // Поскольку вместо TOP в оракле надо добавить ограничение в WHERE сделаем замену пока только в случае, когда TOP
            // в начале, т.е. ориентируясь только на конкретный код GetObjectIndexesWithPks.

            if (query.StartsWith("SELECT TOP "))
            {
                Regex regex = new Regex(@"TOP (?<topcnt>[0-9]+)");
                Match match = regex.Match(query);
                if (match.Success)
                {
                    string topcnt = match.Groups["topcnt"].ToString();
                    query = string.Format("SELECT * FROM ({0}) WHERE ROWNUM<={1}", query.Replace("TOP " + topcnt, string.Empty), topcnt);
                }
            }

            return base.ReadFirst(query, ref state, loadingBufferSize);
        }

        /// <summary>
        /// Перегрузка GenerateSQLSelect, связанная с необходимостью в ORACLE заменить TOP на ограничение rownum.
        /// </summary>
        /// <returns>Текст запроса на вычитку данных, модифицированный с учётом особенностей ORACLE</returns>

        public override string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool ForReadValues, out StorageStructForView[] StorageStruct, bool Optimized, out StringCollection props)
//        public override string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool ForReadValues, out StorageStructForView[] StorageStruct, bool Optimized)
        {
            // В предыдущей реализации вызывалась генерация с TOP, после чего данная подстрока вырезалась.
            // Сейчас TOP оказывается не только в основном запросе, но и в подзапросах, в связи с чем
            // генерируем отключив настройку в customizationStruct (подобно Postgre), делая одну обёртку с rownum.
            int top = customizationStruct.ReturnTop;

            if (top > 0)
            {
                customizationStruct.ReturnTop = 0;
            }

            string res = base.GenerateSQLSelect(customizationStruct, ForReadValues, out StorageStruct, Optimized, out props);

            if (top > 0)
            {
                res = string.Format("SELECT * FROM ({0}) WHERE ROWNUM<={1}", res, top);
                customizationStruct.ReturnTop = top;
            }

            return res;
        }

        public override string GetConvertToTypeExpression(Type valType, string value)
        {
            if (valType == typeof(decimal))
            {
                return value;
            }
            else if (valType == typeof(Guid))
            {
                // 382c74c3-721d-4f34-80e5-57657b6cbc27
//              string res=value.ToString();
//              res=res.Remove(23,1);
//              res=res.Remove(18,1);
//              res=res.Remove(13,1);
//              res=res.Remove(8,1);
//              return String.Format("HEXTORAW('{0}')",res);
                byte[] byteArrGuid = new Guid(value).ToByteArray();
                string hexGuidString = string.Empty;
                foreach (byte b in byteArrGuid)
                {
                    hexGuidString += b.ToString("x2"); // Получаем строку байтов.
                }

                return string.Format("HEXTORAW('{0}')", hexGuidString);
            }
            else
            {
                return string.Empty;
            }
        }

        private System.Collections.ArrayList arParams = new System.Collections.ArrayList();

        public override string ConvertSimpleValueToQueryValueString(object value)
        {
            if (value is DateTime)
            {
                return string.Format("TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS')", ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (value is ICSSoft.STORMNET.KeyGen.KeyGuid || value is System.Guid)
            {
                // 382c74c3-721d-4f34-80e5-57657b6cbc27
//              string res=value.ToString();
//              res=res.Remove(23,1);
//              res=res.Remove(18,1);
//              res=res.Remove(13,1);
//              res=res.Remove(8,1);
                byte[] byteArrGuid = new Guid(value.ToString()).ToByteArray();
                string hexGuidString = string.Empty;
                foreach (byte b in byteArrGuid)
                {
                    hexGuidString += b.ToString("x2"); // Получаем строку байтов.
                }

                return string.Format("HEXTORAW('{0}')", hexGuidString);
            }

            // Исключаем Error: ORA-01704: string literal too long
            else if (value is string && value.ToString().Length > 4000)
            {
                string paramName = "param_" + arParams.Count;
                OracleParameter param = new OracleParameter(paramName, OracleDbType.Clob);
                param.Value = value;
                arParams.Add(param);
                return ':' + paramName;
            }
            else
            {
                return base.ConvertSimpleValueToQueryValueString(value);
            }
        }

        protected override void CustomizeCommand(System.Data.IDbCommand cmd)
        {
            foreach(OracleParameter param in arParams)
            {
                (cmd as OracleCommand).Parameters.Add(param);
            }

            arParams.Clear();
            base.CustomizeCommand(cmd);
        }

////		/// <summary>
////		/// создать join соединения
////		/// </summary>
////		/// <param name="source">источник с которого формируется соединение</param>
////		/// <param name="parentAlias">вышестоящий алиас</param>
////		/// <param name="index">индекс источника</param>
////		/// <param name="keysandtypes">ключи и типы</param>
////		/// <param name="baseOutline">смещение в запросе</param>
////		/// <param name="joinscount">количество соединений</param>
////		/// <returns></returns>
////		public override void CreateJoins(STORMDO.Business.StorageStructForView.PropSource source,
////			string parentAlias,int index,
////			System.Collections.ArrayList  keysandtypes,
////			string baseOutline,out int joinscount,
////			out string FromPart, out string WherePart)
////		{
////			string nl = Environment.NewLine+baseOutline;
////			string newOutLine = baseOutline+"\t";
////			joinscount = 0;
////			FromPart = "";
////			WherePart = "";
////			foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
////			{
////				for (int j=0;j<subSource.storage.Length;j++)
////				{
////					if (subSource.storage[j].parentStorageindex == index)
////					{
////						joinscount++;
////						string curAlias = subSource.Name+j.ToString();
////						keysandtypes.Add(
////							new string[]{
////											PutIdentifierIntoBrackets(curAlias)+"."+PutIdentifierIntoBrackets(subSource.storage[j].PrimaryKeyStorageName),
////											PutIdentifierIntoBrackets(curAlias)+"."+PutIdentifierIntoBrackets(subSource.storage[j].TypeStorageName)
////										}
////
////							);
////						string Link = PutIdentifierIntoBrackets(parentAlias)+"."+PutIdentifierIntoBrackets(subSource.storage[j].objectLinkStorageName);//+"_M"+(locindex++).ToString());
////						int subjoinscount;
////						string subjoin = "";string temp;
////						CreateJoins(subSource,curAlias,j,keysandtypes,newOutLine,out subjoinscount,out subjoin,out temp);
////						string FromStr,WhereStr;
////
////						if (subSource.storage[j].nullableLink)
////							GetLeftJoinExpression(GenString("(",subjoinscount)+" "+ PutIdentifierIntoBrackets(subSource.storage[j].Storage),curAlias,Link,subSource.storage[j].PrimaryKeyStorageName,subjoin,baseOutline,out FromStr,out WhereStr);
////						else
////							GetInnerJoinExpression(GenString("(",subjoinscount)+" "+ PutIdentifierIntoBrackets(subSource.storage[j].Storage),curAlias,Link,subSource.storage[j].PrimaryKeyStorageName,subjoin,baseOutline,out FromStr,out WhereStr);
////						FromPart +=FromStr+")";
////					}
////				}
////			}
////		}
////
////		public override void GetLeftJoinExpression(string subTable, string subTableAlias, string parentAliasWithKey, string subTableKey, string subJoins, string baseOutline, out string FromPart, out string WherePart)
////		{
////
////			/*
////			string nl = Environment.NewLine+baseOutline;
////			FromPart = string.Concat(nl ," LEFT JOIN ",subTable, " " ,PutIdentifierIntoBrackets(subTableAlias),
////				subJoins,
////				nl , " ON " , parentAliasWithKey, " = ",PutIdentifierIntoBrackets(subTableAlias)+"."+PutIdentifierIntoBrackets(subTableKey));
////			WherePart = "";*/
////			FromPart = "";
////			WherePart = "";
////
////		}
////
////		public override void GetInnerJoinExpression(string subTable, string subTableAlias, string parentAliasWithKey, string subTableKey, string subJoins, string baseOutline, out string FromPart, out string WherePart)
////		{
////			/*
////			string nl = Environment.NewLine+baseOutline;
////			FromPart = String.Concat(nl ," INNER JOIN ",subTable, " " ,PutIdentifierIntoBrackets(subTableAlias),
////				subJoins,
////				nl , " ON " , parentAliasWithKey, " = ",PutIdentifierIntoBrackets(subTableAlias)+"."+PutIdentifierIntoBrackets(subTableKey));
////			WherePart = "";*/
////
////			FromPart = "";
////			WherePart = "";
////
////		}
    }
}
