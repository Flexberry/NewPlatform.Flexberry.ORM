namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    public partial class ExternalLangDef
    {
        /// <summary>
        /// Имя представления для построения подзапроса в Exist.
        /// </summary>
        public const string ExistViewName = "__ExistView__";

        private string GetConditionForExist(
            Function func,
            delegateConvertValueToQueryValueString convertValue,
            delegatePutIdentifierToBrackets convertIdentifier,
            IDataService dataService)
        {
            if (!(dataService is SQLDataService sqlDataService))
            {
                throw new Exception(string.Format("Кострукция ограничения {0} поддерживает только SQL сервис данных.",
                                                  funcExist));
            }

            var dvd = (DetailVariableDef)func.Parameters[0];
            string[] agregatorKeys = dvd.OwnerConnectProp.Length == 0
                                          ? new[] { "STORMMainObjectKey" }
                                          : dvd.OwnerConnectProp;
            string detailAlias = "STORMGENERATEDQUERY_S";
            string agregatorAlias = "STORMGENERATEDQUERY";

            // генерируем подзапрос для exists
            string selectForCondition = GetSelectForDetailVariableDef(dvd, null, sqlDataService);
            selectForCondition = selectForCondition.Replace(convertIdentifier(agregatorAlias),
                                                            convertIdentifier(detailAlias));

            object conditionFunc = func.Parameters[1];

            if (conditionFunc != null)
            {
                if (!(conditionFunc is Function))
                {
                    // Если функция была задана переменной логического типа, то необходимо сформировать функцию
                    conditionFunc = GetFunction(funcAND, conditionFunc);
                }
                else
                {
                    // Для случаев, когда для обработки детейлового атрибута применяется И, ИЛИ, НЕ из базового языка
                    ((Function)conditionFunc).FunctionDef.Language = this;
                }

                conditionFunc = TransformVariables((Function)conditionFunc, dvd.StringedView, new ArrayList());
            }

            // генерируем where часть для подзапроса в exists
            string whereForConition = SQLTranslFunction(
                (Function)conditionFunc,
                convertValue,
                identifier => ConvertIdentifierForDetail(identifier, dvd.ConnectMasterPorp, agregatorAlias, detailAlias, convertIdentifier),
                dataService);

            string condition = string.Format("{0} WHERE {1}", selectForCondition, whereForConition);
            string result = string.Format("exists({4} and {2}.{0} = {3}.{1})",
                                          convertIdentifier(agregatorKeys[0]),
                                          convertIdentifier(dvd.ConnectMasterPorp),
                                          convertIdentifier(agregatorAlias),
                                          convertIdentifier(detailAlias),
                                          condition);

            for (int k = 1; k < agregatorKeys.Length; k++)
            {
                result += string.Format(" or {0} in (Select {1} from ({2}) ahh)",
                                        convertIdentifier(agregatorKeys[k]),
                                        convertIdentifier(dvd.ConnectMasterPorp),
                                        condition);
            }

            return result;
        }

        private string ConvertIdentifierForDetail(string identifier, string agregatorName, string agregatorAlias,
                                                  string detailAlias, delegatePutIdentifierToBrackets convertIdentifier)
        {
            string alias;

            if (identifier.StartsWith(agregatorName + "."))
            {
                identifier = identifier.Remove(0, agregatorName.Length + 1);
                alias = agregatorAlias;
            }
            else
            {
                alias = detailAlias;
            }

            return string.Format("{0}.{1}", convertIdentifier(alias), convertIdentifier(identifier));
        }

        /// <summary>
        /// Получить запрос по детейловому представлению с добавлением поля агригатора.
        /// </summary>
        /// <param name="dvd">Детейл.</param>
        /// <param name="additionalProperties">
        /// Свойства, которые необходимо добавить в представление при вычитки детейлов.
        /// </param>
        /// <param name="sqlDataService">Сервис данных.</param>
        /// <returns>Сформированный запрос по представлению детейла.</returns>
        private string GetSelectForDetailVariableDef(DetailVariableDef dvd, List<string> additionalProperties, SQLDataService sqlDataService)
        {
            var lcs = new LoadingCustomizationStruct(null)
            {
                LoadingTypes = new[] { dvd.View.DefineClassType },
                View = dvd.View.Clone(),
            };

            // Чтобы при построении запроса можно было понять, что это ограничение на детейлы, мы зададим специальное имя представления.
            lcs.View.Name = ExistViewName;

            // Добавляем свойство агрегатора
            lcs.View.AddProperty(dvd.ConnectMasterPorp);

            if (additionalProperties != null)
            {
                foreach (var propName in additionalProperties)
                {
                    lcs.View.AddProperty(propName);
                }
            }

            string query = sqlDataService.GenerateSQLSelect(lcs, false);
            return query;
        }
    }
}
