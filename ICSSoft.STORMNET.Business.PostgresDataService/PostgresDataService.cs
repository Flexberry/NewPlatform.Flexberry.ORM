namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.Windows.Forms;

    using Npgsql;

    using static Windows.Forms.ExternalLangDef;

    /// <summary>
    /// DataService for PostgreSQL.
    /// </summary>
    public class PostgresDataService : SQLDataService
    {
        /// <summary>
        /// Максимальная длина имени идентификатора Postgres. Без перекомпиляции Postgres составляет 64 - 1 байт.
        /// Для тестирования с использованием основного алгоритма можно указать 35 - 1.
        /// Для тестирования с использованием отладочного алгоритма можно указать 6 - 1.
        /// </summary>
        public const int MaxNameLength = 64 - 1;

        /// <summary>
        /// Псевдонимы типов в Postgres. Первый элемент в каждом массиве - это тип, остальные псевдонимы.
        /// </summary>
        private static string[][] typesAliases =
            new[]
            {
                new[] { "bigint", "int8" },
                new[] { "bigserial", "serial8" },
                new[] { "bit varying", "varbit" },
                new[] { "boolean", "bool" },
                new[] { "character", "char" },
                new[] { "character varying", "varchar" },
                new[] { "double precision", "float8" },
                new[] { "integer", "int", "int4" },
                new[] { "numeric", "decimal" },
                new[] { "real", "float4" },
                new[] { "smallint", "int2" },
                new[] { "smallserial", "serial2" },
                new[] { "serial", "serial4" },
            };

        /// <summary>
        /// The postgres reserved words.
        /// </summary>
        private static readonly HashSet<string> PostgresReservedWords = new HashSet<string>
            {
               "WINDOW",
               "ALL",
               "ANALYSE",
               "ANALYZE",
               "AND",
               "ANY",
               "ARRAY",
               "AS",
               "ASC",
               "ASYMMETRIC",
               "AUTHORIZATION",
               "BINARY",
               "BOTH",
               "CASE",
               "CAST",
               "CHECK",
               "COLLATE",
               "COLUMN",
               "CONCURRENTLY",
               "CONSTRAINT",
               "CREATE",
               "CROSS",
               "CURRENT_CATALOG",
               "CURRENT_DATE",
               "CURRENT_ROLE",
               "CURRENT_SCHEMA",
               "CURRENT_TIME",
               "CURRENT_TIMESTAMP",
               "CURRENT_USER",
               "DEFAULT",
               "DEFERRABLE",
               "DESC",
               "DISTINCT",
               "DO",
               "ELSE",
               "END",
               "EXCEPT",
               "FETCH",
               "FALSE",
               "FOR",
               "FOREIGN",
               "FROM",
               "FULL",
               "GRANT",
               "GROUP",
               "HAVING",
               "ILIKE",
               "IN",
               "INITIALLY",
               "INNER",
               "INTERSECT",
               "INTO",
               "IS",
               "ISNULL",
               "JOIN",
               "LATERAL",
               "LEADING",
               "LEFT",
               "LIKE",
               "LIMIT",
               "LOCALTIME",
               "LOCALTIMESTAMP",
               "NATURAL",
               "NOT",
               "NOTNULL",
               "NULL",
               "OFFSET",
               "ON",
               "ONLY",
               "OR",
               "ORDER",
               "OUTER",
               "OVERLAPS",
               "PLACING",
               "PRIMARY",
               "REFERENCES",
               "RETURNING",
               "RIGHT",
               "SELECT",
               "SESSION_USER",
               "SIMILAR",
               "SOME",
               "SYMMETRIC",
               "TABLE",
               "THEN",
               "TO",
               "TRAILING",
               "TRUE",
               "UNION",
               "UNIQUE",
               "USER",
               "USING",
               "VARIADIC",
               "VERBOSE",
               "WHEN",
               "WHERE",
               "WITH",
            };

        /// <summary>
        /// Словарь для маппинга длинных имён идентификаторов.
        /// </summary>
        private readonly Dictionary<string, string> dictionaryShortNames = new Dictionary<string, string>();

        /// <summary>
        /// The is generate sql select.
        /// </summary>
        private bool isGenerateSqlSelect;

        private int countGenerateSqlSelect = 0;

        /// <summary>
        /// Initializes static members of the <see cref="PostgresDataService"/> class.
        /// </summary>
        static PostgresDataService()
        {
        }

        /// <summary>
        /// Создание сервиса данных для PostgreSQL без параметров.
        /// </summary>
        public PostgresDataService()
        {
        }

        /// <summary>
        /// Создание сервиса данных для PostgreSQL с указанием настроек проверки полномочий.
        /// </summary>
        /// <param name="securityManager">Сконструированный менеджер полномочий.</param>
        public PostgresDataService(ISecurityManager securityManager)
            : base(securityManager)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresDataService"/> class with specified converter.
        /// </summary>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        public PostgresDataService(IConverterToQueryValueString converterToQueryValueString)
            : base(converterToQueryValueString)
        {
        }

        /// <summary>
        /// Создание сервиса данных для PostgreSQL с указанием настроек проверки полномочий.
        /// </summary>
        /// <param name="securityManager">Сенеджер полномочий.</param>
        /// <param name="auditService">Сервис аудита.</param>
        public PostgresDataService(ISecurityManager securityManager, IAuditService auditService)
            : base(securityManager, auditService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresDataService"/> class with specified security manager, audit service and converter.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service instance.</param>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        /// <param name="notifierUpdateObjects">An instance of the class for custom process updated objects.</param>
        public PostgresDataService(ISecurityManager securityManager, IAuditService auditService, IConverterToQueryValueString converterToQueryValueString, INotifyUpdateObjects notifierUpdateObjects)
            : base(securityManager, auditService, converterToQueryValueString, notifierUpdateObjects)
        {
        }

        /// <summary>
        /// Сравнивает имена типов.
        /// </summary>
        /// <param name="type1">Имя типа 1.</param>
        /// <param name="type2">Имя типа 2.</param>
        /// <returns>Если имена эквивалентны, то возвращает true.</returns>
        public static bool IsTypesEquals(string type1, string type2)
        {
            var types = new string[] { type1, type2 };
            for (int i = 0; i < types.Length; i++)
            {
                string type = Regex.Replace(types[i], @"\s+", " ").ToLower();
                if (type.Contains("without time zone"))
                {
                    type = type.Replace("without time zone", string.Empty);
                }

                if (type.Contains("with time zone"))
                {
                    type = type.Replace("with time zone", string.Empty);
                    if (type.Contains("timestamp"))
                    {
                        if (!type.Contains("timestamptz"))
                        {
                            type = type.Replace("timestamp", "timestamptz");
                        }
                    }
                    else
                    {
                        if (!type.Contains("timetz"))
                        {
                            type = type.Replace("time", "timetz");
                        }
                    }
                }

                string[] tokens = type.Split(new[] { '(', ')' }, StringSplitOptions.None);
                if (tokens.Length != 1 && tokens.Length != 3)
                {
                    return false;
                }

                for (int j = 0; j < tokens.Length; j++)
                {
                    tokens[j] = tokens[j].Trim();
                }

                string tail = string.Empty;
                if (tokens.Length == 3)
                {
                    tail = $"({tokens[1].Replace(" ", string.Empty)}){tokens[2]}";
                }

                types[i] = tokens[0];
                foreach (var t in typesAliases)
                {
                    if (t.Contains(tokens[0]))
                    {
                        types[i] = t[0];
                        break;
                    }
                }

                types[i] = types[i] + tail;
            }

            return types[0] == types[1];
        }

        /// <summary>
        /// Заключает идентификатор в кавычки, если он совпадает со словом из словаря зарезервированных слов Postgres.
        /// </summary>
        /// <param name="identifier">
        /// Имя идентификатора.
        /// </param>
        /// <returns>
        /// Return string.
        /// </returns>
        public static string PrepareIdentifier(string identifier)
        {
            if (PostgresReservedWords.Contains(identifier.ToUpper()))
            {
                identifier = "\"" + identifier + "\"";
            }

            return identifier;
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
                return "current_timestamp";
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
                value.FunctionDef.StringedView == "hhPart" ||
                value.FunctionDef.StringedView == "miPart")
            {
                string strView = value.FunctionDef.StringedView == "hhPart" ? "HOUR" : "MINUTE";

                return string.Format("EXTRACT ({0} FROM {1})", strView,
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "DayOfWeek")
            {
                return string.Format("EXTRACT ({0} FROM {1})", "ISODOW",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcDayOfWeekZeroBased)
            {
                return string.Format("EXTRACT ({0} FROM {1})", "DOW",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcDaysInMonth)
            {
                // здесь требуется преобразование из DATASERVICE

                return string.Format("DATE_PART('days', DATE_TRUNC('month', to_date('01.{0}.{1}','dd.mm.yyyy')) + '1 MONTH'::INTERVAL - DATE_TRUNC('month', to_date('01.{0}.{1}','dd.mm.yyyy')) )",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this), langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "OnlyDate")
            {
                return string.Format("date_trunc('day',{0})",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "CurrentUser")
            {
                return string.Format("'{0}'", CurrentUserService.CurrentUser.FriendlyName);
            }

            if (value.FunctionDef.StringedView == "OnlyTime")
            {
                return string.Format("(to_timestamp(0)+({0} - {0}::date))",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "DATEDIFF")
            {
                var ret = string.Empty;
                if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Year")
                {
                    ret = string.Format("DATE_PART('year', {1}) - DATE_PART('year', {0})",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Month")
                {
                    ret = string.Format("(DATE_PART('year', {1}) - DATE_PART('year', {0})) * 12 + (DATE_PART('month', {1}) - DATE_PART('month', {0}))",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Week")
                {
                    ret = string.Format("TRUNC(DATE_PART('day', {1} - {0})/7)",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "Day")
                {
                    ret = string.Format("DATE_PART('day', {1} - {0})",
                        langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                        langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
                }
                else if (langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this) == "quarter")
                {
                    ret = string.Format("EXTRACT(QUARTER FROM {1})-EXTRACT(QUARTER FROM {0})+4*(DATE_PART('year', {1}) - DATE_PART('year', {0}))",
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
                var lcs = new ICSSoft.STORMNET.Business.LoadingCustomizationStruct(null);
                var dvd = (DetailVariableDef)value.Parameters[0];
                lcs.LoadingTypes = new Type[] { dvd.View.DefineClassType };
                lcs.View = new View();
                lcs.View.DefineClassType = dvd.View.DefineClassType;
                lcs.View.AddProperty(dvd.ConnectMasterPorp);
                var prevRetVars = langDef.retVars;
                langDef.retVars = new string[] { dvd.ConnectMasterPorp };
                var al = new ArrayList();
                var par = langDef.TransformObject(value.Parameters[1], dvd.StringedView, al);
                foreach (string s in al)
                {
                    lcs.View.AddProperty(s);
                }

                var Slct = GenerateSQLSelect(lcs, false).Replace("STORMGENERATEDQUERY", "SGQ" + Guid.NewGuid().ToString().Replace("-", string.Empty));
                var CountIdentifier = convertIdentifier("g" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 29));

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
                    "COALESCE(" + sumExpression + ",0)", value.FunctionDef.StringedView);
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
                    "( COALESCE(  ( SELECT {0} From ( " +
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
                        "({0})::varchar({1})",
                        langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                        value.Parameters[1]);
                }

                if (value.Parameters.Count == 3)
                {
                    return string.Format(
                        "(to_char({0}, '{2}')::varchar({1}))",
                        langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                        value.Parameters[1],
                        DateFormats.GetPostgresDateFormat((int)value.Parameters[2]));
                }
            }

            throw new NotImplementedException(string.Format(
                    "Функция {0} не реализована для Postgres", value.FunctionDef.StringedView));
        }

        /// <summary>
        /// Get connection by Npgsql.
        /// </summary>
        /// <returns>Database connection.</returns>
        public override System.Data.IDbConnection GetConnection()
        {
            return new NpgsqlConnection(CustomizationString);
        }

        /// <inheritdoc />
        public override DbProviderFactory ProviderFactory => NpgsqlFactory.Instance;

        /// <summary>
        /// Get connection by Npgsql (DbConnection).
        /// </summary>
        /// <returns>Database connection.</returns>
        public override System.Data.Common.DbConnection GetDbConnection()
        {
            return new NpgsqlConnection(CustomizationString);
        }

        /// <summary>
        /// Put identifier into brackets.
        /// </summary>
        /// <param name="identifier">Identifier in query.</param>
        /// <returns>Identifier with brackets.</returns>
        public override string PutIdentifierIntoBrackets(string identifier)
        {
            return PutIdentifierIntoBrackets(identifier, isGenerateSqlSelect);
        }

        /// <summary>
        /// Conversation value to type.
        /// </summary>
        /// <param name="valType">Value type for conversation.</param>
        /// <param name="value">Value for conversation.</param>
        /// <returns>Converted value.</returns>
        public override string GetConvertToTypeExpression(Type valType, string value)
        {
            if (valType == typeof(Guid))
            {
                return "cast('" + value + "' as uuid)";
            }

            if (valType == typeof(decimal))
            {
                return "cast(" + value + " as numeric)";
            }

            if (valType == typeof(DateTime))
            {
                return "cast(" + value + " as timestamp)";
            }

            return string.Empty;
        }

        /// <summary>
        /// Convert simple value to query value string.
        /// </summary>
        /// <param name="value">Value for conversation.</param>
        /// <returns>Converted value.</returns>
        public override string ConvertSimpleValueToQueryValueString(object value)
        {
            if (value != null)
            {
                if (value is bool)
                {
                    return (bool)value ? "'1'" : "'0'";
                }

                if (value is DateTime)
                {
                    var dt = (DateTime)value;
                    return "timestamp'" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                }

                Type valueType = value.GetType();

                if (valueType.FullName == "Microsoft.OData.Edm.Library.Date" || valueType.FullName == "Microsoft.OData.Edm.Date")
                {
                    return $"date '{value.ToString()}'";
                }

                if (value is string)
                {
                    var res = base.ConvertSimpleValueToQueryValueString(value);
                    return res;
                }

                if (value is char)
                {
                    return Convert.ToInt32((char)value).ToString(CultureInfo.InvariantCulture);
                }

                if (value is byte[])
                {
                    return string.Format("decode('{0}', 'base64')", Convert.ToBase64String((byte[])value));
                }
            }

            return base.ConvertSimpleValueToQueryValueString(value);
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
            query = query.Replace("count(*)", "cast(count(*) as int)");

            if (query.IndexOf("TOP ", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"TOP (?<topcnt>[0-9]+)");
                Match matchTop = regex.Match(query);
                Match matchLimit = new Regex(@"LIMIT (?<topcnt>[0-9]+)").Match(query);
                if (matchTop.Success)
                {
                    string topcnt = matchTop.Groups["topcnt"].ToString();
                    query = query.Replace("TOP " + topcnt, string.Empty);
                    if (!matchLimit.Success)
                    {
                        query = $"{query}{Environment.NewLine}LIMIT {topcnt}";
                    }
                }
            }

            return base.ReadFirst(query, ref state, loadingBufferSize);
        }

        /// <summary>
        /// Метод переопределён, чтобы заменить длиные псевдонимы на короткие.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>If null expression.</returns>
        public override string GetIfNullExpression(params string[] identifiers)
        {
            string result = ReplaceLongAlias(identifiers[identifiers.Length - 1]);
            for (int i = identifiers.Length - 2; i >= 0; i--)
            {
                result = string.Concat("COALESCE(", ReplaceLongAlias(identifiers[i]), ", ", result, ")");
            }

            return result;
        }

        /// <summary>
        /// The generate sql select.
        /// </summary>
        /// <param name="customizationStruct">The customization struct.</param>
        /// <param name="optimized">Get optimized query (view only with properties from limitation).</param>
        /// <returns>The SQL SELECT Query for customization struct.</returns>
        public override string GenerateSQLSelect(LoadingCustomizationStruct customizationStruct, bool optimized)
        {
            int top = customizationStruct.ReturnTop;

            if (top > 0)
            {
                customizationStruct.ReturnTop = 0;
            }

            string sql = base.GenerateSQLSelect(customizationStruct, optimized);

            if (top > 0 && customizationStruct.RowNumber == null)
            {
                sql += " LIMIT " + top;
            }

            return sql;
        }

        /// <summary>
        /// Этот метод переопределён, чтобы создать словарь соответствия длинных и коротких имён для псевдонимов.
        /// </summary>
        /// <param name="storageStruct">
        /// The storage Struct.
        /// </param>
        /// <param name="addNotMainKeys">
        /// The add Not Main Keys.
        /// </param>
        /// <param name="addMasterFieldsCustomizer">
        /// The add Master Fields Customizer.
        /// </param>
        /// <param name="AddingAdvansedField">
        /// The Adding Advansed Field.
        /// </param>
        /// <param name="AddingKeysCount">
        /// The Adding Keys Count.
        /// </param>
        /// <param name="SelectTypesIds">
        /// The Select Types Ids.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string GenerateSQLSelectByStorageStruct(
            StorageStructForView storageStruct,
            bool addNotMainKeys,
            bool addMasterFieldsCustomizer,
            string AddingAdvansedField,
            int AddingKeysCount,
            bool SelectTypesIds)
        {
            StorageStructForView.PropSource source = storageStruct.sources;
            foreach (StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    string curAlias = subSource.Name + j;
                    GenerateShortName(curAlias);
                }
            }

            for (int i = 0; i < storageStruct.props.Length; i++)
            {
                StorageStructForView.PropStorage prop = storageStruct.props[i];
                if (prop.MultipleProp)
                {
                    continue;
                }

                bool propStored = prop.Stored;
                if (propStored && (string.IsNullOrEmpty(prop.Expression) || this.IsExpressionContainAttrubuteCheckOnly(prop.Expression)))
                {
                    if (prop.MastersTypesCount > 0)
                    {
                        string deniedAccessValue = string.Empty;
                        bool isAccessDenied = SecurityManager.UseRightsOnAttribute
                                              && !SecurityManager.CheckAccessToAttribute(
                                                  prop.Expression,
                                                  out deniedAccessValue);

                        for (int j = 0; j < prop.storage.Length; j++)
                        {
                            for (int k = 0; k < prop.MastersTypes[j].Length; k++)
                            {
                                string curName = isAccessDenied ? deniedAccessValue : prop.source.Name + j;
                                GenerateShortName(curName);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < prop.storage.Length; j++)
                        {
                            GenerateShortName(prop.source.Name + j);
                        }
                    }
                }
            }

            return base.GenerateSQLSelectByStorageStruct(storageStruct, addNotMainKeys, addMasterFieldsCustomizer, AddingAdvansedField, AddingKeysCount, SelectTypesIds);
        }

        /// <summary>
        /// Этот метод переопределён, чтобы обозначить начало создания словаря соответствия длинных и коротких имён для псевдонимов.
        /// </summary>
        /// <param name="customizationStruct">
        /// The customization struct.
        /// </param>
        /// <param name="ForReadValues">
        /// The for read values.
        /// </param>
        /// <param name="StorageStruct">
        /// The storage struct.
        /// </param>
        /// <param name="Optimized">
        /// The optimized.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string GenerateSQLSelect(
            LoadingCustomizationStruct customizationStruct,

            // ReSharper disable once InconsistentNaming
            bool ForReadValues,

            // ReSharper disable once InconsistentNaming
            out StorageStructForView[] StorageStruct,

            // ReSharper disable once InconsistentNaming
            bool Optimized)
        {
            lock (dictionaryShortNames)
            {
                string res;
                if (countGenerateSqlSelect == 0)
                {
                    isGenerateSqlSelect = true;
                }

                countGenerateSqlSelect++;
                //// Закомментировать следующую строку, если будет возникать NeedRestartGenerateSqlSelectExcepton и выяснить почему это происходит. По идее никогда не должен происходить.
                res = base.GenerateSQLSelect(customizationStruct, ForReadValues, out StorageStruct, Optimized);
                /*
                /// Раскомментировать, если будет возникать NeedRestartGenerateSqlSelectExcepton и выяснить почему это происходит.
                /// По идее никогда не должен происходить.
                StorageStruct = null;
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        res = base.GenerateSQLSelect(customizationStruct, ForReadValues, out StorageStruct, Optimized);
                    }
                    catch (NeedRestartGenerateSqlSelectExcepton)
                    {
                        continue;
                    }

                    break;
                }*/
                countGenerateSqlSelect--;
                if (countGenerateSqlSelect == 0)
                {
                    isGenerateSqlSelect = false;
                }

                return res;
            }
        }

        /// <inheritdoc cref="SQLDataService"/>
        public override void GenerateSQLRowNumber(LoadingCustomizationStruct customizationStruct, ref string resQuery, string orderByExpr)
        {
            string nl = Environment.NewLine;
            if (customizationStruct.RowNumber != null)
            {
                int fromInd = resQuery.IndexOf("FROM (");
                string селектСамогоВерхнегоУр = resQuery.Substring(0, fromInd);

                long offset = long.MaxValue;
                long limit = 0;
                if (customizationStruct.RowNumber.StartRow == 0)
                {
                    customizationStruct.RowNumber.StartRow = 1;
                }

                if (customizationStruct.RowNumber.StartRow > 0)
                {
                    offset = customizationStruct.RowNumber.StartRow - 1;
                    if (customizationStruct.RowNumber.EndRow >= customizationStruct.RowNumber.StartRow)
                    {
                        limit = customizationStruct.RowNumber.EndRow - customizationStruct.RowNumber.StartRow + 1;
                    }
                }

                string query = resQuery;
                string orderByExprForPaging = orderByExpr;

                // It is necessary to add primary key field for maintaining rows order while ordering field contains similar values.
                if (!string.IsNullOrEmpty(orderByExprForPaging) && !orderByExprForPaging.Contains(SQLWhereLanguageDef.StormMainObjectKey))
                {
                    orderByExprForPaging += $", {SQLWhereLanguageDef.StormMainObjectKey}";
                    query = query.Replace(orderByExpr, orderByExprForPaging);
                }
                else if (string.IsNullOrEmpty(orderByExprForPaging))
                {
                    orderByExprForPaging = $"{nl}ORDER BY {SQLWhereLanguageDef.StormMainObjectKey}";
                }

                resQuery =
                    $"{селектСамогоВерхнегоУр}FROM ({nl}{query}) rn{nl}{orderByExprForPaging}{nl} OFFSET {offset} LIMIT {limit}";
            }
        }

        /// <summary>
        /// Возвращает индексы и ключи объектов, встретившихся в массиве,
        /// при загрузке по указанному lcs. Объекты задаются через lf.
        /// </summary>
        /// <param name="lcs">Массив, в котором ищем.</param>
        /// <param name="limitFunction">Функция ограничения, определяющая искомые объекты.</param>
        /// <param name="maxResults">
        /// Максимальное число возвращаемых результатов.
        /// Этот параметр не соответствует. <code>lcs.ReturnTop</code>, а устанавливает максимальное число
        /// искомых объектов, тогда как. <code>lcs.ReturnTop</code> ограничивает число объектов, в которых
        /// проводится поиск.
        /// Если значение не определено (<c>null</c>), то возвращаются все найденные результаты.
        /// </param>
        /// <returns>
        /// Массив индексов найденных объектов начиная с 1. Не возвращает null.
        /// </returns>
        public override IDictionary<int, string> GetObjectIndexesWithPks(
            LoadingCustomizationStruct lcs,
            FunctionalLanguage.Function limitFunction,
            int? maxResults = null)
        {
            if (countGenerateSqlSelect == 0)
            {
                isGenerateSqlSelect = true;
            }

            countGenerateSqlSelect++;
            IDictionary<int, string> ret = GetObjectIndexesWithPksImplementation(lcs, limitFunction, maxResults);
            countGenerateSqlSelect--;
            if (countGenerateSqlSelect == 0)
            {
                isGenerateSqlSelect = false;
            }

            return ret;
        }

        /// <summary>
        /// Создать join соединения.
        /// </summary>
        /// <param name="source">Источник с которого формируется соединение.</param>
        /// <param name="parentAlias">Вышестоящий алиас.</param>
        /// <param name="index">Индекс источника.</param>
        /// <param name="keysandtypes">Ключи и типы.</param>
        /// <param name="baseOutline">Смещение в запросе.</param>
        /// <param name="joinscount">Количество соединений.</param>
        /// <param name="FromPart">FROM-часть SQL-запроса.</param>
        /// <param name="WherePart">WHERE-часть SQL-запроса.</param>
        public override void CreateJoins(
            StorageStructForView.PropSource source,
            string parentAlias,
            int index,
            ArrayList keysandtypes,
            string baseOutline,
            out int joinscount,
            out string FromPart,
            out string WherePart)
        {
            string newOutLine = baseOutline + "\t";
            joinscount = 0;
            var fromParts = new List<string>();
            WherePart = string.Empty;
            foreach (StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    StorageStructForView.ClassStorageDef classStorageDef = subSource.storage[j];
                    if (classStorageDef.parentStorageindex == index)
                    {
                        joinscount++;
                        string curAlias = subSource.Name + j;
                        keysandtypes.Add(
                            new string[]
                            {
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.PrimaryKeyStorageName),
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.TypeStorageName),
                                subSource.Name,
                            });
                        string link = PutIdentifierIntoBrackets(parentAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.objectLinkStorageName);
                        int subjoinscount;
                        string subjoin = string.Empty;
                        string temp;
                        CreateJoins(subSource, curAlias, j, keysandtypes, newOutLine, out subjoinscount, out subjoin, out temp);
                        string fromStr, whereStr;

                        // Проверка прав на мастера. Значение атрибута отображается пользователю,
                        // если у данного пользователя есть права на операцию, описанную в специальном формате в DataServiceExpression,
                        // иначе подставляем ссылку на фиктивного мастера - специальное значение из того же DataServiceExpression.
                        if (SecurityManager.UseRightsOnAttribute)
                        {
                            string expression = Information.GetPropertyExpression(
                                source.storage[index].ownerType,
                                subSource.ObjectLink,
                                this.GetType());

                            if (!string.IsNullOrEmpty(expression) && !SecurityManager.CheckAccessToAttribute(expression, out string deniedAccessValue))
                            {
                                link = deniedAccessValue;
                            }
                        }

                        string subTable = string.Concat(
                            GenString("(", subjoinscount),
                            " ",
                            GetTableStorageExpression(classStorageDef.Storage, true));

                        GetLeftJoinExpression(subTable, curAlias, link, classStorageDef.PrimaryKeyStorageName, subjoin, baseOutline, out fromStr, out whereStr);

                        fromParts.Add(fromStr + ")");
                    }
                }
            }

            FromPart = string.Join(string.Empty, fromParts);
        }

        /// <summary>
        /// создать join соединения.
        /// </summary>
        /// <param name="source">источник с которого формируется соединение.</param>
        /// <param name="parentAlias">вышестоящий алиас.</param>
        /// <param name="index">индекс источника.</param>
        /// <param name="keysandtypes">ключи и типы.</param>
        /// <param name="baseOutline">смещение в запросе.</param>
        /// <param name="joinscount">количество соединений.</param>
        /// <param name="FromPart">FROM-часть SQL-запроса.</param>
        /// <param name="WherePart">WHERE-часть SQL-запроса.</param>
        /// <param name="MustNewGenerate">Использовать генерацию SQL без вложенного подзапроса.</param>
        public override void CreateJoins(
            StorageStructForView.PropSource source,
            string parentAlias,
            int index,
            ArrayList keysandtypes,
            string baseOutline,
            out int joinscount,
            out string FromPart,
            out string WherePart,
            bool MustNewGenerate)
        {
            if (!MustNewGenerate)
            {
                CreateJoins(source, parentAlias, index, keysandtypes, baseOutline, out joinscount, out FromPart, out WherePart);
                return;
            }

            string newOutLine = baseOutline + "\t";
            joinscount = 0;
            var fromParts = new List<string>();
            WherePart = string.Empty;
            foreach (StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    StorageStructForView.ClassStorageDef classStorageDef = subSource.storage[j];
                    if (classStorageDef.parentStorageindex == index)
                    {
                        joinscount++;
                        string curAlias = subSource.Name + j;
                        keysandtypes.Add(
                            new string[]
                            {
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.PrimaryKeyStorageName),
                                PutIdentifierIntoBrackets(curAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.TypeStorageName),
                                subSource.Name,
                            });
                        string link = PutIdentifierIntoBrackets(parentAlias) + "." + PutIdentifierIntoBrackets(classStorageDef.objectLinkStorageName);
                        string subjoin = string.Empty;
                        string temp;
                        int subjoinscount = 0;
                        string fromStr, whereStr;

                        CreateJoins(subSource, curAlias, j, keysandtypes, newOutLine, out subjoinscount, out subjoin, out temp, MustNewGenerate);
                        string subTable = GetTableStorageExpression(classStorageDef.Storage, true);

                        GetLeftJoinExpression(subTable, curAlias, link, classStorageDef.PrimaryKeyStorageName, string.Empty, baseOutline, out fromStr, out whereStr);

                        fromParts.Add(fromStr);
                        if (!string.IsNullOrEmpty(subjoin))
                        {
                            fromParts.Add(subjoin);
                        }
                    }
                }
            }

            FromPart = string.Join(string.Empty, fromParts);
        }

        /// <summary>
        /// Put identifier into brackets.
        /// </summary>
        /// <param name="identifier">Identifier in query.</param>
        /// <param name="isGenerateSqlSelectMode">The flag, indicating that generate SQL select mode is on.</param>
        /// <returns>Identifier with brackets.</returns>
        protected string PutIdentifierIntoBrackets(string identifier, bool isGenerateSqlSelectMode)
        {
            string postgresIdentifier = PrepareIdentifier(identifier);

            // Multithreading app with single DS may be failed.
            if (!isGenerateSqlSelectMode)
            {
                return postgresIdentifier;
            }

            if (!dictionaryShortNames.ContainsKey(postgresIdentifier))
            {
                dictionaryShortNames[postgresIdentifier] = null;
            }

            if (postgresIdentifier.IndexOf(".", StringComparison.Ordinal) != -1)
            {
                postgresIdentifier = "\"" + GenerateShortName(postgresIdentifier) + "\"";
            }
            else
            {
                if (dictionaryShortNames[postgresIdentifier] == null)
                {
                    dictionaryShortNames[postgresIdentifier] = GenerateShortName(postgresIdentifier);
                }

                postgresIdentifier = dictionaryShortNames[postgresIdentifier];
            }

            return postgresIdentifier;
        }

        private IDictionary<int, string> GetObjectIndexesWithPksImplementation(
            LoadingCustomizationStruct lcs,
            FunctionalLanguage.Function limitFunction,
            int? maxResults = null)
        {
            string nl = Environment.NewLine;
            string keyName = PutIdentifierIntoBrackets("STORMMainObjectKey");

            var ret = new Dictionary<int, string>();
            if (lcs == null || limitFunction == null)
            {
                return ret;
            }

            if (maxResults.HasValue && maxResults < 0)
            {
                throw new ArgumentOutOfRangeException("maxResults", "Максимальное число возвращаемых результатов не может быть отрицательным.");
            }

            if (!DoNotChangeCustomizationString && ChangeCustomizationString != null)
            {
                string cs = ChangeCustomizationString(lcs.LoadingTypes);
                CustomizationString = string.IsNullOrEmpty(cs) ? CustomizationString : cs;
            }

            bool usedSorting = false;
            if (lcs.ColumnsSort == null || lcs.ColumnsSort.Length == 0)
            {
                lcs.ColumnsSort = new[] { new ColumnsSortDef("__PrimaryKey", SortOrder.Asc) };
            }
            else
            {
                usedSorting = true;
            }

            string innerQuery = GenerateSQLSelect(lcs, false);

            int orderByIndex = usedSorting ? innerQuery.ToLower().LastIndexOf("order by ") : -1;
            string orderByExpr = string.Empty;
            string orderByExprWithoutOffset = string.Empty;

            if (orderByIndex > -1)
            {
                orderByExpr = innerQuery.Substring(orderByIndex);
                orderByExprWithoutOffset = orderByExpr;

                if (lcs.RowNumber != null)
                {
                    int posOffset = orderByExpr.LastIndexOf("OFFSET");
                    orderByExprWithoutOffset = orderByExpr.Substring(0, posOffset - 1);
                }
            }

            string rowNumberExp = $"row_number() over (ORDER BY {keyName}) as \"RowNumber\"{nl}";

            if (!string.IsNullOrEmpty(orderByExpr))
            {
                rowNumberExp = $"row_number() over ({orderByExprWithoutOffset}) as \"RowNumber\"{nl}";
            }

            var source = $"select *, {rowNumberExp} from ({innerQuery}) NumberedRowsQuery";
            string top = maxResults.HasValue ? (" TOP " + maxResults) : string.Empty;
            string lf = LimitFunction2SQLWhere(limitFunction);

            string query =
                 $"SELECT{top} \"RowNumber\", {keyName} FROM {nl}({source}) QueryForGettingIndex {nl} WHERE ({lf}) {orderByExprWithoutOffset}";

            object state = null;
            object[][] res = ReadFirst(query, ref state, lcs.LoadingBufferSize);
            if (res != null)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    object pk = res[i][1];

                    pk = Information.TranslateValueToPrimaryKeyType(lcs.LoadingTypes[0], pk);

                    ret[(int)Convert.ChangeType(res[i][0], typeof(int))] = pk.ToString();
                }

                return ret;
            }

            return ret;
        }

        private string GenString(string stringBlock, int count)
        {
            if (count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(stringBlock.Length * count);
            for (int i = 0; i < count; i++)
            {
                sb.Append(stringBlock);
            }

            return sb.ToString();
        }

        /// <summary>
        /// The replace long alias.
        /// </summary>
        /// <param name="identifier">
        /// The identifier.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ReplaceLongAlias(string identifier)
        {
            if (identifier.IndexOf("COALESCE", StringComparison.Ordinal) != -1)
            {
                return identifier;
            }

            string postgresIdentifier = PrepareIdentifier(identifier);
            int p = postgresIdentifier.IndexOf(".", StringComparison.Ordinal);
            if (p != -1)
            {
                var alias = postgresIdentifier.Substring(0, p);
                string shortName;
                if (dictionaryShortNames.ContainsKey(alias) && dictionaryShortNames[alias] == null)
                {
                    shortName = GenerateShortName(alias);
                    if (alias != shortName)
                    {
                        ////По идее никогда не должен происходить.
                        throw new NeedRestartGenerateSqlSelectExcepton();
                    }
                }

                shortName = GenerateShortName(alias);
                if (alias != shortName)
                {
                    postgresIdentifier = shortName + "." + postgresIdentifier.Substring(p + 1);
                }
            }

            return postgresIdentifier;
        }

        /// <summary>
        /// Алгоритм генерации коротких имён. Не возникает коллизий с именами таблиц и столбцов, т.к. в коротких именах используется GUID.
        /// </summary>
        /// <param name="postgresIdentifier">
        /// The postgres identifier.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GenerateShortName(string postgresIdentifier)
        {
            if (Encoding.UTF8.GetByteCount(postgresIdentifier) > MaxNameLength)
            {
                if (!dictionaryShortNames.ContainsKey(postgresIdentifier))
                {
                    dictionaryShortNames[postgresIdentifier] = null;
                }

                if (dictionaryShortNames[postgresIdentifier] == null)
                {
                    string guid = Guid.NewGuid().ToString("N");
                    if (guid.Length >= MaxNameLength)
                    {
                        throw new ArgumentOutOfRangeException("guid.Length >= MaxNameLength");
                    }

                    var len = postgresIdentifier.Length - (guid.Length / 2);
                    int byteCount = Encoding.UTF8.GetByteCount(postgresIdentifier.Substring(0, len));
                    for (; len > 0; len--)
                    {
                        if (byteCount <= MaxNameLength - guid.Length)
                        {
                            break;
                        }

                        byteCount -= Encoding.UTF8.GetByteCount(postgresIdentifier[len - 1].ToString(CultureInfo.InvariantCulture));
                    }

                    string shortName = postgresIdentifier.Substring(0, len) + guid;
                    dictionaryShortNames[postgresIdentifier] = shortName;
                    postgresIdentifier = shortName;
                }
                else
                {
                    postgresIdentifier = dictionaryShortNames[postgresIdentifier];
                }
            }

            return postgresIdentifier;
        }

        /// <summary>
        /// Этот Exception необходим для прерывания обработки в методе GenerateSQLSelect в случае,
        /// если в процессе выполнения метода GetIfNullExpression было обнаружено,
        /// что обрабатываемые там идентификаторы превышают допустимую длину и не имеют короткого имени.
        /// Для исправления этой ситуации необходимо заново запустить обработку GenerateSQLSelect с
        /// сохранением коротких имён, созданных в предыдущей итерации GenerateSQLSelect.
        /// По идее никогда не должен происходить.
        /// </summary>
        private class NeedRestartGenerateSqlSelectExcepton : Exception
        {
        }
    }
}
