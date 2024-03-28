namespace AdvLimit.ExternalLangDef
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    /// <summary>
    /// Класс для расширения представления свойствами, используемыми в ограничении, но отсутствующими в указанном представлении.
    /// </summary>
    public static class ViewPropertyAppender
    {
        /// <summary>
        /// Вспомогательный класс для хранения переменных, связанных с детейлами/псевдодетейлами.
        /// На настоящий момент данная структура не используется, поскольку не стоит задачи расширения представления детейлами.
        /// </summary>
        public class DetailVariableDefContainer
        {
            /// <summary>
            /// Описание связи с детейлом/псевдодетейлом.
            /// </summary>
            public DetailVariableDef DetailVariableDefValue;

            /// <summary>
            /// Используемые в ограничении на детейлы свойства детейлов.
            /// </summary>
            public List<string> DetailVariablesList = new List<string>();

            /// <summary>
            /// Используемые в ограничении на детейлы ограничения на детейлы более низкого порядка.
            /// </summary>
            public List<DetailVariableDefContainer> DetailDetailsList = new List<DetailVariableDefContainer>();

            /// <summary>
            /// На базе списка DetailVariableDef создать список DetailVariableDefContainer.
            /// </summary>
            /// <param name="detailVariableDefsList">Список DetailVariableDef.</param>
            /// <returns>Полученный список DetailVariableDefContainer.</returns>
            public static List<DetailVariableDefContainer> CreateDetailVariableDefContainersList(
                List<DetailVariableDef> detailVariableDefsList)
            {
                return detailVariableDefsList
                    .Select(
                        detailVariableDef =>
                        new DetailVariableDefContainer() { DetailVariableDefValue = detailVariableDef })
                    .ToList();
            }
        }

        /// <summary>
        /// В представления <see cref="DetailVariableDef"/> функции ограничения добавляются недостающие свойства, если это необходимо.
        /// </summary>
        /// <param name="function">Функция ограничения, которая может содержать <see cref="DetailVariableDef"/>.</param>
        /// <param name="dataService">Сервис данных, через который будет вестись обработка вычислимых свойств.</param>
        public static void EnrichDetailViewInLimitFunction(Function function, IDataService dataService)
        {
            var variableList = new List<string>();
            var detailList = new List<DetailVariableDefContainer>();
            FindPropertiesUsedInFunction(function, variableList, detailList);
            EnrichDetailViewList(detailList, dataService);
        }

        /// <summary>
        /// В представления переданных структур, соответствующих детейлам, добавляются недостающие свойства, если это необходимо.
        /// </summary>
        /// <param name="detailList">Список структур, соответствующих детейлам.</param>
        /// <param name="dataService">Сервис данных, через который будет вестись обработка вычислимых свойств.</param>
        private static void EnrichDetailViewList(List<DetailVariableDefContainer> detailList, IDataService dataService)
        {
            foreach (DetailVariableDefContainer detailVariableDefContainer in detailList)
            {
                bool foundChanges = false;
                View detailViewCopy = detailVariableDefContainer.DetailVariableDefValue.View.Clone();

                foreach (var property in detailVariableDefContainer.DetailVariablesList)
                {
                    if (!detailViewCopy.CheckPropname(property))
                    {
                        foundChanges = true;
                        detailViewCopy.AddProperty(property, property, false, string.Empty);
                    }

                    // Проверим нет ли у свойства выражения для вычисления.
                    List<string> propertiesUsedInExpression = GetPropertiesUsedInExpression(property, detailViewCopy.DefineClassType, dataService);
                    List<string> filteredProperties = propertiesUsedInExpression
                        .Where(propertyUsedInExpression => !detailViewCopy.CheckPropname(propertyUsedInExpression)
                                                           && !string.Equals(
                                                               propertyUsedInExpression,
                                                               SQLWhereLanguageDef.StormMainObjectKey,
                                                               StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (var propertyUsedInExpression in filteredProperties)
                    {
                        foundChanges = true;
                        detailViewCopy.AddProperty(propertyUsedInExpression, propertyUsedInExpression, false, string.Empty);
                    }
                }

                if (foundChanges)
                {
                    detailVariableDefContainer.DetailVariableDefValue.View = detailViewCopy;
                }

                if (detailVariableDefContainer.DetailDetailsList.Any())
                {
                    EnrichDetailViewList(detailVariableDefContainer.DetailDetailsList, dataService);
                }
            }
        }

        /// <summary>
        /// Метод поиска всех используемых в параметрах функции свойств (включая описание детейлов).
        /// </summary>
        /// <param name="function">Функция, в которой производится поиск свойств.</param>
        /// <param name="variableList">Список найденных в функции свойств.</param>
        /// <param name="detailList">Список найденных описаний детейлов.</param>
        public static void FindPropertiesUsedInFunction(Function function, List<string> variableList, List<DetailVariableDefContainer> detailList)
        {
            if (function == null)
            {
                return;
            }

            DetailVariableDefContainer currentDetailContainer = null;

            foreach (var parameter in function.Parameters)
            {
                if (parameter is DetailVariableDef)
                { // это детейл
                    var detailVariableDef = parameter as DetailVariableDef;
                    currentDetailContainer =
                        detailList.FirstOrDefault(
                            dvd => dvd.DetailVariableDefValue.StringedView == detailVariableDef.StringedView);
                    if (currentDetailContainer == null)
                    {
                        detailList.Add(currentDetailContainer = new DetailVariableDefContainer() { DetailVariableDefValue = detailVariableDef });
                    }
                }
                else if (parameter is VariableDef)
                { // это имя свойства
                    var variableDef = parameter as VariableDef;
                    if (!variableList.Contains(variableDef.StringedView)
                        && variableDef.StringedView != SQLWhereLanguageDef.StormMainObjectKey)
                    {
                        variableList.Add(variableDef.StringedView);
                    }
                }
                else if (parameter is Function)
                { // спускаемся вниз
                    if (currentDetailContainer == null)
                    {
                        FindPropertiesUsedInFunction(parameter as Function, variableList, detailList);
                    }
                    else
                    {
                        FindPropertiesUsedInFunction(
                            parameter as Function, currentDetailContainer.DetailVariablesList, currentDetailContainer.DetailDetailsList);
                    }
                }
            }
        }

        /// <summary>
        /// Получить список свойств, используемых DataServiceExpression.
        /// </summary>
        /// <param name="property"></param>
        /// <returns>Cписок уникальных свойств, найденных в DataServiceExpression.</returns>
        public static List<string> GetPropertiesUsedInExpression(string property, Type type, IDataService sqlDataService)
        {
            var result = new List<string>();
            var dataServiceType = sqlDataService.GetType();
            var propertyName = property;
            string prefix = string.Empty;
            string name = property;
            Type dataObjectType = type;

            if (propertyName.IndexOf(".") != -1)
            {
                prefix = propertyName.Substring(0, propertyName.LastIndexOf("."));
                name = propertyName.Substring(propertyName.LastIndexOf(".") + 1);

                dataObjectType = Information.GetPropertyType(dataObjectType, prefix);
            }

            var expressions = Information.GetExpressionForProperty(dataObjectType, name);
            if (expressions != null)
            {
                var expression = expressions.ToDictionary()
                    .FirstOrDefault(i => dataServiceType == i.Key || dataServiceType.IsSubclassOf(i.Key));

                if (!default(KeyValuePair<Type, object>).Equals(expression))
                {
                    var propertiesUsedInExpression = Information.GetPropertiesInExpression((string)expression.Value, string.Empty);
                    var filteredPropertiesUsedInExpression = propertiesUsedInExpression.Distinct().Select(p => string.IsNullOrEmpty(prefix) ? p : string.Format("{0}.{1}", prefix, p));
                    result.AddRange(filteredPropertiesUsedInExpression);
                }
            }

            return result;
        }

        /// <summary>
        /// Создание представление с добавление свойств из ограничения, которые используются в ограничении, но отсутствуют в указанном в качестве параметра ограничении. При поиске отсутствующих в представлении свойств учитываются также и выражения для вычислимых свойств.
        /// </summary>
        /// <param name="view">Представление, но основе которого создается новое представление, возвращаемое в качестве результата метода.</param>
        /// <param name="function">Функция, среди параметров которой происходит поиск неиспользуемых в представлении свойств.</param>
        /// <param name="dataService">Сервис данных необходим для правильного выбора выражения для вычислимиого свойства.</param>
        /// <returns>Представление, в которое добавлены необходимые свойства.</returns>
        public static View GetViewWithPropertiesUsedInFunction(View view, Function function, IDataService dataService)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            // Создадим копию представления.
            // ToDo: При текущей реализации клонирования происходят аномалии, т.к. вложенные объекты не копируются полностью, копируются только ссылки. В TFS создана ошибка №30700.
            var enrichedView = view.Clone();

            var variableList = enrichedView.Properties.Select(propertyInView => propertyInView.Name).ToList();

            var detailList = new List<DetailVariableDef>();

            // ToDo: Для расширения представлений детейлов необходимо раскомментировать следую строку.  На данный момент этот вызов приводит к появлению пустых свойств в представлении детейла. Явление, видимо, как-то связанное с клонированием представлений (см. №30700.).
            // var detailList = enrichedView.Details.Select(detailInView => new DetailVariableDef(langdef.GetObjectType(detailInView.Name), detailInView.Name, detailInView.View, string.Empty, null)).ToList();

            FindPropertiesUsedInFunction(
                function, variableList, DetailVariableDefContainer.CreateDetailVariableDefContainersList(detailList));

            // Просмотрим все найденные в органичении свойства
            foreach (var property in variableList)
            {
                if (!enrichedView.CheckPropname(property))
                {
                    enrichedView.AddProperty(property, property, false, string.Empty);
                }

                // Проверим нет ли у свойства выражения для вычисления.
                var propertiesUsedInExpression = GetPropertiesUsedInExpression(
                    property, enrichedView.DefineClassType, dataService);

                foreach (var propertyUsedInExpression in propertiesUsedInExpression)
                {
                    if (!enrichedView.CheckPropname(propertyUsedInExpression)
                        && !string.Equals(propertyUsedInExpression, SQLWhereLanguageDef.StormMainObjectKey, StringComparison.OrdinalIgnoreCase))
                    {
                        enrichedView.AddProperty(propertyUsedInExpression, propertyUsedInExpression, false, string.Empty);
                    }
                }
            }

            // Добавление описания детейлов: раскоментировать при ненеобходимости.
            // Пока в добавлении детейлов нет необходимости.
            // foreach (var detail in detailList)
            // {
            //    if (enrichedView.Details.Count(dvd => dvd.Name == detail.StringedView) == 0)
            //        enrichedView.AddDetailInView(detail.StringedView, detail.View, true);
            // }

            return enrichedView;
        }
    }
}
