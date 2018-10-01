 using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
using System.Collections.Generic;

namespace ICSSoft.STORMNET.Windows.Forms
{
    public partial class ExternalLangDef
    {
        private string GetConditionForExistExact(Function func, delegateConvertValueToQueryValueString convertValue,
                                                    delegatePutIdentifierToBrackets convertIdentifier, ref List<string>  OTBSubqueries,Business.StorageStructForView[] storageStruct,   Business.SQLDataService DataService)
        {
            lIDataService = DataService;
            var f1 = new Function(GetFunctionDefByStringedView(funcCountWithLimit), func.Parameters[0], func.Parameters[1]);
            var f2 = new Function(GetFunctionDefByStringedView(funcCount), func.Parameters[0]);
            Function fres = GetFunction(funcAND, GetFunction(funcEQ, f1, f2), GetFunction(funcG, f2, 0));
            return base.SQLTranslFunction(fres, convertValue, convertIdentifier, ref OTBSubqueries,storageStruct,  DataService);
        }
    }
}
