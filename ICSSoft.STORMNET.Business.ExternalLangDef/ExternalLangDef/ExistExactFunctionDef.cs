namespace ICSSoft.STORMNET.Windows.Forms
{
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    public partial class ExternalLangDef
    {
        private string GetConditionForExistExact(
            Function func,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier,
            IDataService dataService)
        {
            var f1 = new Function(GetFunctionDefByStringedView(funcCountWithLimit), func.Parameters[0], func.Parameters[1]);
            var f2 = new Function(GetFunctionDefByStringedView(funcCount), func.Parameters[0]);
            Function fres = GetFunction(funcAND, GetFunction(funcEQ, f1, f2), GetFunction(funcG, f2, 0));
            return base.SQLTranslFunction(fres, convertValue, convertIdentifier, dataService);
        }
    }
}
