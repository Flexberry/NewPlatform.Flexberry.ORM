namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.Windows.Forms;

    using Oracle.ManagedDataAccess.Client;

    using static ICSSoft.STORMNET.Windows.Forms.ExternalLangDef;

    /// <summary>
    /// Сервис данных для доступа к данным Oracle.
    /// </summary>
    public class OracleDataService : ICSSoft.STORMNET.Business.SQLDataService
    {
        public const int MaxBytes = 30;

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
        /// Initializes a new instance of the <see cref="OracleDataService"/> class with specified security manager, audit service and converter.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service instance.</param>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        /// <param name="notifierUpdateObjects">An instance of the class for custom process updated objects.</param>
        public OracleDataService(ISecurityManager securityManager, IAuditService auditService, IConverterToQueryValueString converterToQueryValueString, INotifyUpdateObjects notifierUpdateObjects = null)
            : base(securityManager, auditService, converterToQueryValueString, notifierUpdateObjects)
        {
        }

        /// <summary>
        /// Преобразовать значение в SQL строку.
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <param name="convertValue">делегат для преобразования констант.</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов.</param>
        /// <returns></returns>
        public override string FunctionToSql(
            SQLWhereLanguageDef sqlLangDef,
            Function value,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier)
        {
            ExternalLangDef langDef = sqlLangDef as ExternalLangDef;

            if (value.FunctionDef.StringedView == "TODAY")
            {
                return "sysdate";
            }

            if (
                value.FunctionDef.StringedView == "YearPart" ||
                value.FunctionDef.StringedView == "MonthPart" ||
                value.FunctionDef.StringedView == "DayPart")
            {
                return string.Format("EXTRACT ({0} FROM {1})", value.FunctionDef.StringedView.Substring(0, value.FunctionDef.StringedView.Length - 4),
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (
                value.FunctionDef.StringedView == "hhPart")
            {
                return string.Format("TO_CHAR({1}, \'{0}\')", "HH24",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "miPart")
            {
                return string.Format("TO_CHAR({1}, \'{0}\')", "MI",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "DayOfWeek")
            {
                // здесь требуется преобразование из DATASERVICE
                return string.Format("TO_CHAR({1}, \'{0}\')", "D",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcDayOfWeekZeroBased)
            {
                throw new NotImplementedException(string.Format("Function {0} is not implemented for Oracle", langDef.funcDayOfWeekZeroBased));
            }

            if (value.FunctionDef.StringedView == langDef.funcDaysInMonth)
            {
                // здесь требуется преобразование из DATASERVICE
                string.Format("to_char(last_day(to_date('01.'||{0}||'.'||{1},'dd.mm.yyyy')),'dd')", langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this), langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this));
                return string.Empty;
            }

            if (value.FunctionDef.StringedView == "OnlyDate")
            {
                return string.Format("TRUNC({0})",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "CurrentUser")
            {
                // return string.Format("'{0}'", CurrentUserService.CurrentUser.FriendlyName);
                throw new NotImplementedException("Не реализована поддержка текущего пользователя");
            }

            if (value.FunctionDef.StringedView == "OnlyTime")
            {
                return string.Format("TRUNC({0})",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "DATEDIFF")
            {
                var ret = string.Empty;
                if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Year")
                {
                    ret = string.Format("EXTRACT (YEAR FROM {1}) - EXTRACT (YEAR FROM {0})",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Month")
                {
                    ret = string.Format("(EXTRACT (YEAR FROM {1}) - EXTRACT (YEAR FROM {0})) * 12 + (EXTRACT (MONTH FROM {1}) - EXTRACT (MONTH FROM {0}))",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Week")
                {
                    ret = string.Format("(TRUNC({1},'DAY') - TRUNC({0},'DAY'))/7",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Day")
                {
                    ret = string.Format("TRUNC({1}) - TRUNC({0})",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "quarter")
                {
                    ret = string.Format("(EXTRACT (YEAR FROM {1}) - EXTRACT (YEAR FROM {0})) * 4 + (TO_CHAR({1}, 'Q') - TO_CHAR({0}, 'Q'))",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }

                return ret;
            }

            if (value.FunctionDef.StringedView == "SUM" ||
                value.FunctionDef.StringedView == "AVG" ||
                value.FunctionDef.StringedView == "MAX" ||
                value.FunctionDef.StringedView == "MIN")
            {
                ICSSoft.STORMNET.Business.LoadingCustomizationStruct lcs = new ICSSoft.STORMNET.Business.LoadingCustomizationStruct(null);
                DetailVariableDef dvd = (DetailVariableDef)value.Parameters[0];
                lcs.LoadingTypes = new Type[] { dvd.View.DefineClassType };
                lcs.View = new View();
                lcs.View.DefineClassType = dvd.View.DefineClassType;
                lcs.View.AddProperty(dvd.ConnectMasterPorp);
                string[] prevRetVars = langDef.retVars;
                langDef.retVars = new string[] { dvd.ConnectMasterPorp };
                ArrayList al = new ArrayList();
                object par = langDef.TransformObject(value.Parameters[1], dvd.StringedView, al);
                foreach (string s in al)
                {
                    lcs.View.AddProperty(s);
                }

                string Slct = GenerateSQLSelect(lcs, false).Replace("STORMGENERATEDQUERY", "SGQ" + Guid.NewGuid().ToString().Replace("-", string.Empty));
                string CountIdentifier = convertIdentifier("g" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 29));

                // FunctionalLanguage.Function numFunc = (value.Parameters[1] as FunctionalLanguage.Function);

                string sumExpression = langDef.SQLTranslSwitch(par, convertValue, convertIdentifier, this);

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
                    "NVL(" + sumExpression + ",0)", value.FunctionDef.StringedView);
                for (int k = 0; k < dvd.OwnerConnectProp.Length; k++)
                {
                    res += "," + convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[k]);
                }

                res += "))";

                langDef.retVars = prevRetVars;
                return res;
            }

            if (value.FunctionDef.StringedView == langDef.funcCountWithLimit || value.FunctionDef.StringedView == "Count")
            {
                var lcs = new ICSSoft.STORMNET.Business.LoadingCustomizationStruct(null);
                var dvd = (DetailVariableDef)value.Parameters[0];
                lcs.LoadingTypes = new Type[] { dvd.View.DefineClassType };
                lcs.View = dvd.View.Clone();
                lcs.LimitFunction = value.FunctionDef.StringedView == langDef.funcCountWithLimit
                    ? langDef.TransformVariables((FunctionalLanguage.Function)value.Parameters[1], dvd.StringedView, null)
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

            if (value.FunctionDef.StringedView == langDef.funcToChar)
            {
                if (value.Parameters.Count == 2)
                {
                    return string.Format(
                        "SUBSTR(TO_CHAR({0}), 1, {1})",
                        langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                        value.Parameters[1]);
                }

                if (value.Parameters.Count == 3)
                {
                    return string.Format(
                        "SUBSTR(TO_CHAR({0}, {2}), 1, {1})",
                        langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                        value.Parameters[1],
                        ExternalLangDef.DateFormats.GetOracleDateFormat((int)value.Parameters[2]));
                }
            }
            else
            {
                throw new NotImplementedException(string.Format(
                    "Функция {0} не реализована для Oracle", value.FunctionDef.StringedView));
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
        /// <param name="identifier">идентификатор.</param>
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
        /// Получить идентификатор, обработанный с учётом требований ORACLE на длину в 30 байт.
        /// </summary>
        /// <param name="identifier">идентификатор.</param>
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
        /// Получить идентификатор, заключенный в кавычки, с учётом требований ORACLE на длину.
        /// </summary>
        /// <param name="identifier">идентификатор.</param>
        public static string PrepareIdentifier(string identifier)
        {
            identifier = GenerateShortName(identifier);
            return '"' + identifier + '"';
        }

        /// <summary>
        /// Получить идентификатор, заключенный в кавычки, с учётом требований ORACLE на длину.
        /// </summary>
        /// <param name="identifier">идентификатор.</param>
        /// <returns></returns>
        public override string PutIdentifierIntoBrackets(string identifier)
        {
            return PrepareIdentifier(identifier);
        }

        /// <inheritdoc />
        public override System.Data.IDbConnection GetConnection()
        {
            return new OracleConnection(this.CustomizationString);
        }

        /// <inheritdoc cref="SQLDataService.GetDbConnection"/>
        public override System.Data.Common.DbConnection GetDbConnection()
        {
            return new OracleConnection(this.CustomizationString);
        }

        /// <inheritdoc />
        public override DbProviderFactory ProviderFactory => OracleClientFactory.Instance;

        /// <summary>
        /// Вернуть ifnull выражение (для ORACLE используется ф-я NVL).
        /// </summary>
        /// <param name="identifiers">идентификаторы.</param>
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
            PrepareQuery(ref query);

            return base.ReadFirst(query, ref state, loadingBufferSize);
        }

        /// <summary>
        /// Reading data from database: read first part (by external connection).
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <param name="state">The reading state.</param>
        /// <param name="loadingBufferSize">The loading buffer size.</param>
        /// <param name="connection">Connection to use (you have to open and close it yourself).</param>
        /// <param name="transaction">Transaction to use.</param>
        /// <returns>The readed objects from database.</returns>
        public override object[][] ReadFirstByExtConn(string query, ref object state, int loadingBufferSize, IDbConnection connection, IDbTransaction transaction)
        {
            PrepareQuery(ref query);

            return base.ReadFirstByExtConn(query, ref state, loadingBufferSize, connection, transaction);
        }

        /// <summary>
        /// Асинхронная вычитка данных.
        /// </summary>
        /// <param name="query">Запрос для вычитки.</param>
        /// <param name="loadingBufferSize">Количество строк, которые нужно загрузить в рамках текущей вычитки (используется для повторной дочитки).</param>
        /// <param name="dbTransactionWrapperAsync">Содержит соединение и транзакцию, в рамках которых нужно выполнить запрос (если соединение закрыто - оно откроется).</param>
        /// <returns>Асинхронная операция (возвращает результат вычитки).</returns>
        protected override Task<object[][]> ReadByExtConnAsync(string query, int loadingBufferSize, DbTransactionWrapperAsync dbTransactionWrapperAsync)
        {
            PrepareQuery(ref query);

            return base.ReadByExtConnAsync(query, loadingBufferSize, dbTransactionWrapperAsync);
        }

        /// <summary>
        /// Предобработка запроса (функция конкретного датасервиса).
        /// </summary>
        /// <param name="query">Запрос который нужно подготовить.</param>
        private void PrepareQuery(ref string query)
        {
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
        }

        /// <summary>
        /// Перегрузка GenerateSQLSelect, связанная с необходимостью в ORACLE заменить TOP на ограничение rownum.
        /// </summary>
        /// <returns>Текст запроса на вычитку данных, модифицированный с учётом особенностей ORACLE.</returns>
        public override string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool ForReadValues, out StorageStructForView[] StorageStruct, bool Optimized)
        {
            // В предыдущей реализации вызывалась генерация с TOP, после чего данная подстрока вырезалась.
            // Сейчас TOP оказывается не только в основном запросе, но и в подзапросах, в связи с чем
            // генерируем отключив настройку в customizationStruct (подобно Postgre), делая одну обёртку с rownum.
            int top = customizationStruct.ReturnTop;

            if (top > 0)
            {
                customizationStruct.ReturnTop = 0;
            }

            string res = base.GenerateSQLSelect(customizationStruct, ForReadValues, out StorageStruct, Optimized);

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
            if (value != null)
            {
                if (value is DateTime dt)
                {
                    return string.Format("TO_DATE('{0}', 'YYYY-MM-DD HH24:MI:SS')", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                Type valueType = value.GetType();

                if (valueType.FullName == "Microsoft.OData.Edm.Library.Date" || valueType.FullName == "Microsoft.OData.Edm.Date")
                {
                    return $"TO_DATE('{value}', 'YYYY-MM-DD')";
                }

                if (value is ICSSoft.STORMNET.KeyGen.KeyGuid || value is System.Guid)
                {
                    // 382c74c3-721d-4f34-80e5-57657b6cbc27
                    //string res = value.ToString();
                    //res = res.Remove(23, 1);
                    //res = res.Remove(18, 1);
                    //res = res.Remove(13, 1);
                    //res = res.Remove(8, 1);
                    byte[] byteArrGuid = new Guid(value.ToString()).ToByteArray();
                    string hexGuidString = string.Empty;
                    foreach (byte b in byteArrGuid)
                    {
                        hexGuidString += b.ToString("x2"); // Получаем строку байтов.
                    }

                    return string.Format("HEXTORAW('{0}')", hexGuidString);
                }

                // Исключаем Error: ORA-01704: string literal too long
                if (value is string && value.ToString().Length > 4000)
                {
                    string paramName = "param_" + arParams.Count;
                    OracleParameter param = new OracleParameter(paramName, OracleDbType.Clob);
                    param.Value = value;
                    arParams.Add(param);
                    return ':' + paramName;
                }
            }

            return base.ConvertSimpleValueToQueryValueString(value);
        }

        protected override void CustomizeCommand(System.Data.IDbCommand cmd)
        {
            foreach (OracleParameter param in arParams)
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
