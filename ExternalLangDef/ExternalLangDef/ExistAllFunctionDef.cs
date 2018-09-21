using System;
using System.Collections;
using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

namespace ICSSoft.STORMNET.Windows.Forms
{
    public partial class ExternalLangDef
    {
        private string GetConditionForExistAll(Function func,
                                               delegateConvertValueToQueryValueString convertValue,
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
                if (func.Parameters.Count == 3)
                {
                    funcAdv = (Function) func.Parameters[2];
                }
                else
                {
                    var advParams = new ArrayList();

                    for (int i = 2; i < func.Parameters.Count; i++)
                        advParams.Add(func.Parameters[i]);

                    funcAdv = GetFunction(funcAND, advParams.ToArray());
                }
            }

            if (conditionFunc.FunctionDef.StringedView == funcEQ)
            {
                var f = new Function(GetFunctionDefByStringedView(funcExist),
                                     func.Parameters[0],
                                     funcAdv == null
                                         ? func.Parameters[1]
                                         : GetFunction(funcAND, funcAdv, func.Parameters[1]));

                return SQLTranslFunction(f, convertValue, convertIdentifier);
            }

            var vdefDet = func.Parameters[0] as VariableDef;
            var vdefIn = conditionFunc.Parameters[0] as VariableDef;
            var newpars = new object[conditionFunc.Parameters.Count - 1];

            for (int i = 1; i < conditionFunc.Parameters.Count; i++)
            {
                Function funcIn = GetFunction(funcEQ, vdefIn, conditionFunc.Parameters[i]);
                Function funcOperand = funcAdv == null
                                           ? funcIn
                                           : GetFunction(funcAND, funcIn, funcAdv);

                newpars[i - 1] = new Function(GetFunctionDefByStringedView(funcExist), vdefDet, funcOperand);
            }

            Function function = GetFunction(funcAND, newpars);
            return base.SQLTranslFunction(function, convertValue, convertIdentifier);
        }
    }
}
