using System;
using System.Collections;
using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

namespace ICSSoft.STORMNET.Windows.Forms
{
    public partial class ExternalLangDef
    {
        private string GetConditionForExistAllExact(Function func, delegateConvertValueToQueryValueString convertValue,
                                                   delegatePutIdentifierToBrackets convertIdentifier)
        {
            if (!(func.Parameters[1] is Function) ||
                ((Function) func.Parameters[1]).FunctionDef.StringedView != funcEQ &&
                ((Function) func.Parameters[1]).FunctionDef.StringedView != funcIN)
                throw new Exception("<" + func.FunctionDef.StringedView + "> parameter1 must be <=> or <in>");

            var conditionFunc = (Function) func.Parameters[1];
            Function funcAdv = null;

            if (func.Parameters.Count > 2)
            {
                var advParams = new ArrayList();

                for (int i = 1; i < func.Parameters.Count; i++)
                    advParams.Add(func.Parameters[i]);

                funcAdv = GetFunction(funcAND, advParams.ToArray());
            }

            if (funcAdv == null)
                funcAdv = conditionFunc;

            var vdefDet = func.Parameters[0] as VariableDef;
            var funcOperand1 = new Function(GetFunctionDefByStringedView(funcExistAll), vdefDet, conditionFunc);
            Function funcOperand2 = GetFunction(funcEQ, 0,
                                                GetFunction(funcCountWithLimit, vdefDet, GetFunction(funcNOT, funcAdv)));
            Function function = GetFunction(funcAND, funcOperand1, funcOperand2);

            return base.SQLTranslFunction(function, convertValue, convertIdentifier);
        }
    }
}
