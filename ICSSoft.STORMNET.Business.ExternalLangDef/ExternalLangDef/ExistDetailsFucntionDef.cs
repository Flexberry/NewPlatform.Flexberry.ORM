namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;
    using System.Text;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    public partial class ExternalLangDef
    {
        private string GetConditionForExistDetails(
            Function func,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier,
            IDataService dataService)
        {
            if (!(dataService is SQLDataService sqlDataService))
            {
                throw new Exception(string.Format("Кострукция ограничения {0} поддерживает только SQL сервис данных.",
                                                  funcExistDetails));
            }

            string wrongParametersMessage = string.Format(
                "Кострукция ограничения {0} поддерживает только операцию сравнения между двумя различными детейловыми свойствами одного уровня.",
                funcExistDetails);

            if (!CheckParametersFunctionForExistDetails(func))
            {
                throw new Exception(wrongParametersMessage);
            }

            var detail1 = (DetailVariableDef)func.Parameters[0];
            var detail2 = (DetailVariableDef)func.Parameters[1];
            var conditionFunc = (Function)func.Parameters[2];
            string detailAlias1 = convertIdentifier(detail1.StringedView);
            string detailAlias2 = convertIdentifier(detail2.StringedView);
            string agregatorPropName1 = convertIdentifier(detail1.ConnectMasterPorp);
            string agregatorPropName2 = convertIdentifier(detail2.ConnectMasterPorp);
            string[] agregatorKeys1 = detail1.OwnerConnectProp.Length == 0
                                          ? new[] { "STORMMainObjectKey" }
                                          : detail1.OwnerConnectProp;
            string[] agregatorKeys2 = detail2.OwnerConnectProp.Length == 0
                                          ? new[] { "STORMMainObjectKey" }
                                          : detail2.OwnerConnectProp;
            var conditionParameters = conditionFunc.Parameters;

            if (!CheckConditionFunctionForExistDetails(conditionFunc.FunctionDef) || conditionParameters.Count != 2
                || !(conditionParameters[0] is VariableDef) || !(conditionParameters[1] is VariableDef))
            {
                throw new Exception(wrongParametersMessage);
            }

            string selectForDetail1 = GetSelectForDetailVariableDef(detail1, null, sqlDataService);
            selectForDetail1 = selectForDetail1.Replace("STORMMainObjectKey", "STORMMainObjectKey1");
            string selectForDetail2 = GetSelectForDetailVariableDef(detail2, null, sqlDataService);
            selectForDetail2 = selectForDetail2.Replace("STORMMainObjectKey", "STORMMainObjectKey2");

            // формируем условие для функции
            string propName1 = ((VariableDef)conditionParameters[0]).StringedView;

            if (propName1.StartsWith(detail1.StringedView + "."))
            {
                propName1 = propName1.Remove(0, detail1.StringedView.Length + 1);
            }

            string propName2 = ((VariableDef)conditionParameters[1]).StringedView;

            if (propName2.StartsWith(detail2.StringedView + "."))
            {
                propName2 = propName2.Remove(0, detail2.StringedView.Length + 1);
            }

            string propIdetifier1 = convertIdentifier(propName1);
            string propIdetifier2 = convertIdentifier(propName2);
            string condition = string.Format("({0} {1} {2})", string.Format("{0}.{1}", detailAlias1, propIdetifier1),
                                             conditionFunc.FunctionDef.StringedView,
                                             string.Format("{0}.{1}", detailAlias2, propIdetifier2));

            var joinCondition = new StringBuilder();

            foreach (var keyName in agregatorKeys1)
            {
                if (joinCondition.Length != 0)
                {
                    joinCondition.Append(" and ");
                }

                joinCondition.AppendFormat("{0} = {1}.{2}", keyName, detailAlias1, agregatorPropName1);
            }

            foreach (var keyName in agregatorKeys2)
            {
                joinCondition.AppendFormat(" and {0} = {1}.{2}", keyName, detailAlias2, agregatorPropName2);
            }

            string result = string.Format(
                "exists(select * from ({0}) {3} join ({1}) {4} on {2} and ({5}))",
                selectForDetail1, selectForDetail2, joinCondition, detailAlias1, detailAlias2, condition);

            return result;
        }

        /// <summary>
        /// Проверка доступных операций сравнения для двух детейловых свойств.
        /// </summary>
        /// <param name="func">Функция сравнения, для проверки.</param>
        /// <returns>Результат проверки.</returns>
        public bool CheckConditionFunctionForExistDetails(FunctionDef func)
        {
            string operation = func.StringedView;
            return operation == funcEQ || operation == funcNEQ || operation == funcG || operation == funcL
                   || operation == funcGEQ || operation == funcLEQ;
        }

        /// <summary>
        /// Проверка доступных параметров при сравнении детейловых свойств.
        /// </summary>
        /// <param name="function">Функция ExistDetails.</param>
        /// <returns>Результат проверки.</returns>
        public bool CheckParametersFunctionForExistDetails(Function function)
        {
            var detail1 = (DetailVariableDef)function.Parameters[0];
            var detail2 = (DetailVariableDef)function.Parameters[1];

            bool result = detail1.StringedView != detail2.StringedView;
            return result;
        }
    }
}
