namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Interfaces;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Security;
    using ICSSoft.STORMNET.Windows.Forms;

    /// <summary>
    /// Сервис данных для работы с Microsoft SQL Server.
    /// </summary>
    public class MSSQLDataService : SQLDataService
    {
        /// <summary>
        /// Создание сервиса данных для Microsoft SQL Server с указанием настроек проверки полномочий.
        /// </summary>
        /// <param name="securityManager">Менеджер полномочий.</param>
        /// <param name="auditService">Сервис аудита.</param>
        /// <param name="businessServerProvider">The provider for <see cref="BusinessServer"/> creation.</param>
        public MSSQLDataService(ISecurityManager securityManager, IAuditService auditService, IBusinessServerProvider businessServerProvider)
            : base(securityManager, auditService, businessServerProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSSQLDataService"/> class with specified security manager, audit service and converter.
        /// </summary>
        /// <param name="securityManager">The security manager instance.</param>
        /// <param name="auditService">The audit service instance.</param>
        /// <param name="businessServerProvider">The provider for <see cref="BusinessServer"/> creation.</param>
        /// <param name="converterToQueryValueString">The converter instance.</param>
        /// <param name="notifierUpdateObjects">An instance of the class for custom process updated objects.</param>
        public MSSQLDataService(ISecurityManager securityManager, IAuditService auditService, IBusinessServerProvider businessServerProvider, IConverterToQueryValueString converterToQueryValueString, INotifyUpdateObjects notifierUpdateObjects = null)
            : base(securityManager, auditService, businessServerProvider, converterToQueryValueString, notifierUpdateObjects)
        {
        }

        /// <summary>
        /// Вернуть объект <see cref="System.Data.IDbConnection"/>, предназначенный для работы с MSSQLServer и настроенный на строку соединения <see cref="SQLDataService.CustomizationString"/>.
        /// </summary>
        /// <returns>Соединение с БД.</returns>
        public override System.Data.IDbConnection GetConnection()
        {
            return new System.Data.SqlClient.SqlConnection(CustomizationString);
        }

        /// <inheritdoc />
        public override System.Data.Common.DbProviderFactory ProviderFactory => System.Data.SqlClient.SqlClientFactory.Instance;

        /// <summary>
        /// Вернуть объект <see cref="System.Data.Common.DbConnection"/>, предназначенный для работы с MSSQLServer и настроенный на строку соединения <see cref="SQLDataService.CustomizationString"/>.
        /// </summary>
        /// <returns>Соединение с БД.</returns>
        public override System.Data.Common.DbConnection GetDbConnection()
        {
            return new System.Data.SqlClient.SqlConnection(CustomizationString);
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
                return "getdate()";
            }

            if (
                value.FunctionDef.StringedView == "YearPart" ||
                value.FunctionDef.StringedView == "MonthPart" ||
                value.FunctionDef.StringedView == "DayPart")
            {
                return string.Format("{0}({1})", value.FunctionDef.StringedView.Substring(0, value.FunctionDef.StringedView.Length - 4),
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (
                value.FunctionDef.StringedView == "hhPart" ||
                value.FunctionDef.StringedView == "miPart")
            {
                return string.Format("datepart({0},{1})", value.FunctionDef.StringedView.Substring(0, value.FunctionDef.StringedView.Length - 4),
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "DayOfWeek")
            {
                // здесь требуется преобразование из DATASERVICE
                return string.Format("(datepart({0}, {1})+@@DATEFIRST-2)%7 + 1", "DW",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcDayOfWeekZeroBased)
            {
                // здесь требуется преобразование из DATASERVICE
                return string.Format(
                    "(datepart({0}, {1})+@@DATEFIRST-1)%7",
                    "DW",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcDaysInMonth)
            {
                // здесь требуется преобразование из DATASERVICE
                string monthStr = string.Format("LTRIM(RTRIM(STR({0})))", langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
                string yearStr = string.Format("LTRIM(RTRIM(STR({0})))", langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this));
                monthStr = string.Format("CASE WHEN LEN({0})=1 THEN '0'+{0} ELSE {0} END", monthStr);
                return string.Format("DAY(DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,CAST({0}+{1}+'01' AS DATETIME))+1,0)))", yearStr, monthStr);
            }

            if (value.FunctionDef.StringedView == "OnlyDate")
            {
                return string.Format("cast(CONVERT(varchar(8), {1}, {0}) as datetime)", "112",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "CurrentUser")
            {
                if (CurrentUser != null)
                {
                    return string.Format("'{0}'", CurrentUser.FriendlyName);
                }
                else
                {
                    throw new InvalidOperationException("Property CurrentUser is not defined for this data service. Add initialization for this property.");
                }
            }

            if (value.FunctionDef.StringedView == "OnlyTime")
            {
                return string.Format("cast(CONVERT(varchar(8), {1}, {0}) as datetime)", "114",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == "DATEDIFF")
            {
                return string.Format("DATEDIFF ( {0} , {1} , {2})",
                langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
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
                    "isnull(" + sumExpression + ",0)", value.FunctionDef.StringedView);
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
                    "( Isnull(  ( SELECT {0} From ( " +
                    "SELECT Count(*) {0},{1} from ( {4} )pip group by {1} ) " +
                    " ahh where {1} in ({3}", // ),0))",
                    CountIdentifier,
                    convertIdentifier(dvd.ConnectMasterPorp),
                    convertIdentifier(Information.GetClassStorageName(dvd.View.DefineClassType)),
                    convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[0]),

                    // convertIdentifier(dvd.OwnerConnectProp),
                    Slct);
                for (int k = 1; k < dvd.OwnerConnectProp.Length; k++)
                {
                    res += "," + convertIdentifier("STORMGENERATEDQUERY") + "." + convertIdentifier(dvd.OwnerConnectProp[k]);
                }

                res += ")),0))";

                langDef.retVars = prevRetVars;
                return res;
            }

            if (value.FunctionDef.StringedView == langDef.funcToUpper)
            {
                return string.Format(
                    "Upper({0})",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcToLower)
            {
                return string.Format(
                    "Lower({0})",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcDateAdd)
            {
                return string.Format(
                    "dateadd({0}, {1}, {2})",
                    langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                    langDef.SQLTranslSwitch(value.Parameters[1], convertValue, convertIdentifier, this),
                    langDef.SQLTranslSwitch(value.Parameters[2], convertValue, convertIdentifier, this));
            }

            if (value.FunctionDef.StringedView == langDef.funcToChar)
            {
                // Общее преобразование в строку, задается значение и длина строки
                if (value.Parameters.Count == 2)
                {
                    return string.Format(
                        "CONVERT(VARCHAR({1}), {0})",
                        langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                        value.Parameters[1]);
                }

                // Преобразование даты и времени в строку; кроме значения, числом задается стиль
                // даты-времени, например, 104 (dd.mm.yyyy).
                // Стили перечислены здесь: http://msdn.microsoft.com/ru-ru/library/ms187928.aspx
                if (value.Parameters.Count == 3)
                {
                    return string.Format(
                        "CONVERT(VARCHAR({1}), {0}, {2})",
                        langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier, this),
                        value.Parameters[1],
                        value.Parameters[2]);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Вернуть ISNULL выражение (IfNull - на псевдокоде).
        /// </summary>
        /// <param name="identifiers">Идентификаторы выражения.</param>
        /// <returns>Строка для вставки в запрос.</returns>
        public override string GetIfNullExpression(params string[] identifiers)
        {
            if (identifiers == null)
            {
                throw new ArgumentNullException("identifiers");
            }

            int identifiersLength = identifiers.Length;
            if (identifiersLength == 0)
            {
                throw new ArgumentException("Identifiers array is empty.", "identifiers");
            }

            /*
             * Создаём строку вида:
             *
             * ds.GetIfNullExpression("identifier1");
             * //"identifier1"
             *
             * ds.GetIfNullExpression("identifier1", "identifier2");
             * //"ISNULL(identifier1, identifier2)"
             *
             * ds.GetIfNullExpression("identifier1", "identifier2", "identifier3");
             * //"ISNULL(identifier1, ISNULL(identifier2, identifier3))"
             *
             */
            string result = identifiers[identifiersLength - 1];
            for (int i = identifiers.Length - 2; i >= 0; i--)
            {
                result = string.Concat("ISNULL(", identifiers[i], ", ", result, ")");
            }

            return result;
        }

        /// <summary>
        /// Конвертация константных значений в строки запроса в соответствиями с требованиями MSSQLServer.
        /// </summary>
        /// <param name="value">Константное значение.</param>
        /// <returns>Строка запроса.</returns>
        public override string ConvertSimpleValueToQueryValueString(object value)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if (valueType == typeof(DateTime) || valueType == typeof(UserDataTypes.NullableDateTime))
                {
                    DateTime dateTime;
                    if (valueType == typeof(UserDataTypes.NullableDateTime))
                    {
                        var nullableDateTime = (UserDataTypes.NullableDateTime)value;

                        dateTime = nullableDateTime.Value;
                    }
                    else
                    {
                        dateTime = (DateTime)value;
                    }

                    return "'" + dateTime.ToString("yyyyMMdd HH:mm:ss.fff") + "'";
                }

                if (valueType.FullName == "Microsoft.OData.Edm.Library.Date" || valueType.FullName == "Microsoft.OData.Edm.Date")
                {
                    return $"'{value.ToString()}'";
                }

                if (valueType == typeof(string))
                {
                    if ((string)value == string.Empty)
                    {
                        return "NULL";
                    }

                    return "N'" + value.ToString().Replace("'", "''") + "'";
                }
            }

            return base.ConvertSimpleValueToQueryValueString(value);
        }
    }
}
