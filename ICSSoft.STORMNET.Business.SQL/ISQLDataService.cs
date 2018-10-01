using System;
using System.Collections.Generic;
using System.Text;
using ICSSoft.STORMNET.Business.Audit;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
using ICSSoft.STORMNET.FunctionalLanguage;

namespace ICSSoft.STORMNET.Business
{
    public class OnDeleteConflictReferenceException : Exception
    {
        public Type DataObjectType { get; set; }
        public string Property { get; set; }

        public OnDeleteConflictReferenceException(string message, Type dataobjecttype, string property) : base(message)
        {
            DataObjectType = dataobjecttype;
            Property = property;
        }
        public OnDeleteConflictReferenceException(string message, Type dataobjecttype, string property,Exception innerexception) : base(message,innerexception)
        {
            DataObjectType = dataobjecttype;
            Property = property;
        }
    }

    public interface ISQLDataService:IDataService
    {
        /// <summary>
        /// Текущий сервис аудита.
        /// </summary>
        IAuditService AuditService { get; }

        /// <summary>
        /// Преобразовать значение в SQL строку
        /// </summary>
        /// <param name="function">Функция</param>
        /// <param name="convertValue">делегат для преобразования констант</param>
        /// <param name="convertIdentifier">делегат для преобразования идентификаторов</param>
        /// <returns></returns>
        string FunctionToSql(
            SQLWhereLanguageDef sqlLangDef,
            Function function,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier,
            ref List<string> OTBSubquery,
            StorageStructForView[] StorageStruct);
    }
}
