using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
using ICSSoft.STORMNET.Windows.Forms;
using System;
using STORMFunction = ICSSoft.STORMNET.FunctionalLanguage.Function;

namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Summary description for ODBCAccessDataService.
    /// </summary>
    public class ODBCAccessDataService : ODBCDataService
    {
        public ODBCAccessDataService()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Преобразовать значение в SQL строку
        /// </summary>
        /// <param name="function">Функция</param>
        /// <param name="convertValue">делегат для преобразования констант</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов</param>
        /// <returns></returns>
        public override string FunctionToSql(
            SQLWhereLanguageDef sqlLangDef,
            Function value,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier)
        {
            // реализована ЛИШЬ ЧАСТИЧНАЯ поддержка Access
            ExternalLangDef langDef = sqlLangDef as ExternalLangDef;
            if (value.FunctionDef.StringedView == "OnlyDate")
            {
                return string.Format("cdate (int( {0} ) )",
                                  langDef.SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier));
            }

            return base.FunctionToSql(sqlLangDef, value, convertValue, convertIdentifier);
        }

        /// <summary>
        /// Создание копии экземпляра сервиса данных.
        /// </summary>
        /// <returns>Копии экземпляра сервиса данных.</returns>
        public override object Clone()
        {
            var instance = (ODBCAccessDataService)base.Clone();
            instance.identDict = identDict;
            return instance;
        }

        public override string GetConvertToTypeExpression(Type valType, string value)
        {
            return value;
        }

        private System.Collections.Specialized.StringDictionary identDict = new System.Collections.Specialized.StringDictionary();

        public override string PutIdentifierIntoBrackets(string identifier)
        {
            if (identifier.IndexOf(".") >= 0 || identifier.Length > 32)
            {
                if (identDict.ContainsKey(identifier))
                {
                    return identDict[identifier];
                }
                else
                {
                    string ni = "[gi" + Guid.NewGuid().ToString("N").Substring(4) + "]";
                    identDict.Add(identifier, ni);
                    return ni;
                }
            }
            else
            {
                return "[" + identifier + "]";
            }
        }

        private void translateFunction(STORMFunction LimitFunction)
        {
            if (LimitFunction.FunctionDef.StringedView == "=")
            {
                LimitFunction.FunctionDef.StringedView = "IN";
            }

            for (int i = 0; i < LimitFunction.Parameters.Count; i++)
            {
                if (LimitFunction.Parameters[i] is STORMFunction)
                {
                    translateFunction((STORMFunction)LimitFunction.Parameters[i]);
                }
            }
        }

        public override string LimitFunction2SQLWhere(ICSSoft.STORMNET.FunctionalLanguage.Function LimitFunction, StorageStructForView[] StorageStruct, string[] asnameprop, bool MustNewGenerate)
        {
            translateFunction(LimitFunction);
            ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.OptimizeINOperator = false;
            return
                ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(LimitFunction,
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString(ConvertValueToQueryValueString),
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets(PutIdentifierIntoBrackets));
        }

        public override string LimitFunction2SQLWhere(STORMFunction LimitFunction)
        {
            translateFunction(LimitFunction);
            ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.OptimizeINOperator = false;
            return
                ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.ToSQLString(LimitFunction,
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString(ConvertValueToQueryValueString),
                new ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets(PutIdentifierIntoBrackets));
        }

        public override string GetIfNullExpression(params string[] identifiers)
        {
            string result = identifiers[identifiers.Length - 1];
            for (int i = identifiers.Length - 2; i >= 0; i--)
            {
                result = string.Concat("IIF ( IsNULL (", identifiers[i], "),", result, ", ", identifiers[i], ")");
            }

            return result;
        }

        public override string ConvertSimpleValueToQueryValueString(object value)
        {
            string res = string.Empty;
            if (value != null && value.GetType() == typeof(DateTime))
            {
                string frmt = System.Configuration.ConfigurationSettings.AppSettings["AccessDateFormat"];
                if (frmt != null && frmt != string.Empty)
                {
                    res = ((DateTime)value).ToString(frmt, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    res = base.ConvertSimpleValueToQueryValueString(value).Replace("'", "#");

                    // Access не поддерживает доли секунд, поэтому
                    // Шлыков
                    res = System.Text.RegularExpressions.Regex.Replace(res, @"\.\d*\#", "#");
                }

                return res;
            }
            else if (value != null && value.GetType() == typeof(Guid))
            {
                res = base.ConvertSimpleValueToQueryValueString(value);
                return res;
            }
            else
            {
                return base.ConvertSimpleValueToQueryValueString(value);
            }
        }
    }

    public enum State
    {
        Created,
        Deleted,
        MOdifyed
    }
}
