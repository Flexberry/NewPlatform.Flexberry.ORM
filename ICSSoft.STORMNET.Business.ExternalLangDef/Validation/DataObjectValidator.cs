namespace NewPlatform.Flexberry.ORM.Validation
{
    using System;
    using System.Collections.Generic;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Validation.Exceptions;

    /// <summary>
    /// Класс для проверки полномочий на объект.
    /// </summary>
    public static class DataObjectValidator
    {
        /// <summary>
        /// Язык задания ограничений.
        /// </summary>
        private static readonly ExternalLangDef _languageDef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Проверка, удовлетворяет ли объект данных заданному ограничению.
        /// </summary>
        /// <param name="objectToCheck">Объект данных, для которого применяется ограничение.</param>
        /// <param name="limitFunction">Функция ограничения, на удовлетворение которой проверяется объект данных.</param>
        /// <returns><c>true</c>, если объект удовлетворяет переданному ограничению. Иначе <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="objectToCheck"/>.</exception>
        /// <exception cref="ArgumentNullException">Исключение генерируется при передаче <c>null</c> в качестве значения для <paramref name="limitFunction"/>.</exception>
        public static bool CheckObject(DataObject objectToCheck, Function limitFunction)
        {
            if (objectToCheck == null)
            {
                throw new ArgumentNullException("objectToCheck");
            }

            if (limitFunction == null)
            {
                throw new ArgumentNullException("limitFunction");
            }

            return ProcessFunction(limitFunction, objectToCheck, new Dictionary<string, object>());
        }

        /// <summary>
        /// Основная функция проверки, удовлетворяет ли объект выражению.
        /// Отсюда идёт переход на другие методы.
        /// </summary>
        /// <param name="value">Текущее выражение для обработки, может быть либо функцией ограничения, либо полем, либо значением.</param>
        /// <param name="target">Объект, который проверяем на ограничение.</param>
        /// <param name="involved">Список полей со значениями из проверяемого объекта (записи добавляются при первом извлечении значения поля из объекта).</param>
        /// <returns>Результат проверки.</returns>
        internal static object Expression(object value, DataObject target, Dictionary<string, object> involved)
        {
            if (value is Function)
            {
                bool processedResult = ProcessFunction((Function)value, target, involved);
                return processedResult;
            }

            if (value is VariableDef)
            {
                object ret = ProcessVariableDef((VariableDef)value, target, ref involved);
                return ret;
            }

            return value;
        }

        /// <summary>
        /// Проверка, удовлетворяет ли объект заданной функции ограничения.
        /// </summary>
        /// <param name="function">Проверяемая функция ограничения.</param>
        /// <param name="target">Проверяемый объект.</param>
        /// <param name="involved">Список полей со значениями из проверяемого объекта (записи добавляются при первом извлечении значения поля из объекта).</param>
        /// <returns><c>True</c>, если объект удовлетворяет заданной функции ограничения. Иначе <c>false</c>.</returns>
        private static bool ProcessFunction(Function function, DataObject target, Dictionary<string, object> involved)
        {
            if (function.FunctionDef == null)
            {
                throw new NotSupportedException();
            }

            string functionStringedView = function.FunctionDef.StringedView;

            // И/ИЛИ.
            if (functionStringedView == _languageDef.funcAND
                || functionStringedView == _languageDef.funcOR)
            {
                return ProcessOrAnd(function, target, involved, functionStringedView);
            }

            // Операции сравнения.
            if (functionStringedView == _languageDef.funcEQ
                || functionStringedView == _languageDef.funcL
                || functionStringedView == _languageDef.funcLEQ
                || functionStringedView == _languageDef.funcG
                || functionStringedView == _languageDef.funcGEQ
                || functionStringedView == _languageDef.funcNEQ)
            {
                return ProcessCompare(function, target, involved, functionStringedView);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Обработка используемого в ограничении описания поля.
        /// Получение значения поля.
        /// </summary>
        /// <param name="variableDef">Используемое в ограничении описание поля.</param>
        /// <param name="target">Объект, из которого будет получено значение поля по описанию.</param>
        /// <param name="involved">Список полей со значениями из проверяемого объекта (записи добавляются при первом извлечении значения поля из объекта).</param>
        /// <returns>Значение поля.</returns>
        private static object ProcessVariableDef(VariableDef variableDef, DataObject target, ref Dictionary<string, object> involved)
        {
            if (variableDef.Type.StringedView == "Details")
            {
                throw new NotSupportedException();
            }

            string propertyPath = variableDef.StringedView;

            if (involved.ContainsKey(propertyPath))
            {
                return involved[propertyPath];
            }

            object currentPropertyValue = Information.GetPropValueByName(target, propertyPath);
            CheckLoadedProperty(target, propertyPath);
            involved[propertyPath] = Information.GetPropValueByName(target, propertyPath);
            return currentPropertyValue;
        }

        /// <summary>
        /// Проверка загруженности свойства, указанного в ограничении.
        /// </summary>
        /// <param name="target">Объект, в котором проверяется загруженность свойств.</param>
        /// <param name="propertyName">Свойство, которое проверяется на загруженность.</param>
        /// <exception cref="UsedNotLoadedPropertyValidationException">Исключение генерируется, когда оказывается, что переданное свойство не загружено.</exception>
        private static void CheckLoadedProperty(DataObject target, string propertyName)
        {
            if (target.GetStatus() == ObjectStatus.Created)
            {
                return;
            }

            string[] propertyNames = propertyName.Split(new char[] { '.' });
            if (!target.GetLoadedPropertiesList().Contains(propertyNames[0]))
            {
                throw new UsedNotLoadedPropertyValidationException(propertyNames[0]);
            }

            if (propertyNames.Length > 1)
            {
                object masterValue = Information.GetPropValueByName(target, propertyNames[0]);
                if (masterValue != null)
                {
                    string masterProperty = propertyName.Substring(propertyNames[0].Length + 1);
                    CheckLoadedProperty((DataObject)masterValue, masterProperty);
                }
            }
        }

        /// <summary>
        /// Обработчик функций OR и AND.
        /// </summary>
        /// <param name="limitFunction">Проверяемая функция ограничения.</param>
        /// <param name="target">Проверяемый объект.</param>
        /// <param name="involved">Список полей со значениями из проверяемого объекта (записи добавляются при первом извлечении значения поля из объекта).</param>
        /// <param name="operationOrAnd">Строковое представление функции ("OR" или "AND").</param>
        /// <exception cref="InvalidParameterCountValidationException">Возникает, если число параметров меньше двух.</exception>
        /// <exception cref="InvalidParameterTypeValidationException">Возникает, если хотя бы один операнд не логического типа.</exception>
        /// <returns><c>True</c>, если объект удовлетворяет заданной функции ограничения. Иначе <c>false</c>.</returns>
        private static bool ProcessOrAnd(Function limitFunction, DataObject target, Dictionary<string, object> involved, string operationOrAnd)
        {
            if (limitFunction.Parameters.Count < 1)
            {
                throw new InvalidParameterCountValidationException(operationOrAnd);
            }

            var result = operationOrAnd == _languageDef.funcAND;
            for (int i = 0; i < limitFunction.Parameters.Count; i++)
            {
                object work = Expression(limitFunction.Parameters[i], target, involved);
                if (!(work is bool))
                {
                    throw new InvalidParameterTypeValidationException(operationOrAnd, typeof(bool).ToString());
                }

                var logicResult = (bool)work;
                if (operationOrAnd == _languageDef.funcOR)
                {
                    result = result || logicResult;
                }
                else if (operationOrAnd == _languageDef.funcAND)
                {
                    result = result && logicResult;
                }

                if (result && operationOrAnd == _languageDef.funcOR)
                {
                    return true;
                }

                if (!result && operationOrAnd == _languageDef.funcAND)
                {
                    return false;
                }
            }

            return result;
        }

        /// <summary>
        /// Обработка функций сравнения.
        /// </summary>
        /// <param name="limitFunction">Проверяемая функция ограничения.</param>
        /// <param name="target">Проверяемый объект.</param>
        /// <param name="involved">Список полей со значениями из проверяемого объекта (записи добавляются при первом извлечении значения поля из объекта).</param>
        /// <param name="operationCompare">Строковое представление функции сравнения.</param>
        /// <returns><c>True</c>, если объект удовлетворяет заданной функции ограничения. Иначе <c>false</c>.</returns>
        private static bool ProcessCompare(Function limitFunction, DataObject target, Dictionary<string, object> involved, string operationCompare)
        {
            if (limitFunction.Parameters.Count != 2)
            {
                throw new InvalidParameterCountValidationException(operationCompare);
            }

            object leftResult = Expression(limitFunction.Parameters[0], target, involved);
            object rightResult = Expression(limitFunction.Parameters[1], target, involved);

            if (leftResult == null || rightResult == null)
            {
                // TODO: подозрительная логика.
                return operationCompare == "<>";
            }

            if (IsNumeric(leftResult) && IsNumeric(rightResult))
            {
                var leftNumber = ToDecimal(leftResult);
                var rightNumber = ToDecimal(rightResult);
                if (operationCompare == _languageDef.funcEQ)
                {
                    return leftNumber == rightNumber;
                }

                if (operationCompare == _languageDef.funcNEQ)
                {
                    return leftNumber != rightNumber;
                }

                if (operationCompare == _languageDef.funcLEQ)
                {
                    return leftNumber <= rightNumber;
                }

                if (operationCompare == _languageDef.funcL)
                {
                    return leftNumber < rightNumber;
                }

                if (operationCompare == _languageDef.funcGEQ)
                {
                    return leftNumber >= rightNumber;
                }

                if (operationCompare == _languageDef.funcG)
                {
                    return leftNumber > rightNumber;
                }

                throw new NotSupportedException();
            }

            if (IsDate(leftResult) && IsDate(rightResult))
            {
                var leftDate = ToDate(leftResult);
                var rightDate = ToDate(rightResult);
                if (operationCompare == _languageDef.funcEQ)
                {
                    return leftDate == rightDate;
                }

                if (operationCompare == _languageDef.funcNEQ)
                {
                    return leftDate != rightDate;
                }

                if (operationCompare == _languageDef.funcLEQ)
                {
                    return leftDate <= rightDate;
                }

                if (operationCompare == _languageDef.funcL)
                {
                    return leftDate < rightDate;
                }

                if (operationCompare == _languageDef.funcGEQ)
                {
                    return leftDate >= rightDate;
                }

                if (operationCompare == _languageDef.funcG)
                {
                    return leftDate > rightDate;
                }

                throw new NotSupportedException();
            }

            if (IsDataObject(leftResult) && IsDataObject(rightResult))
            {
                var leftDataObject = (DataObject)leftResult;
                var rightDataObject = (DataObject)rightResult;
                if (operationCompare == _languageDef.funcEQ)
                {
                    return leftDataObject.__PrimaryKey.Equals(rightDataObject.__PrimaryKey);
                }

                if (operationCompare == _languageDef.funcNEQ)
                {
                    return !leftDataObject.__PrimaryKey.Equals(rightDataObject.__PrimaryKey);
                }

                throw new NotSupportedException();
            }

            Guid leftGuid;
            Guid rightGuid;
            if (IsGuid(leftResult, out leftGuid) && IsGuid(rightResult, out rightGuid))
            {
                if (operationCompare == _languageDef.funcEQ)
                {
                    return leftGuid.Equals(rightGuid);
                }

                if (operationCompare == _languageDef.funcNEQ)
                {
                    return !leftGuid.Equals(rightGuid);
                }

                throw new NotSupportedException();
            }

            string leftString = leftResult.ToString();
            string rightString = rightResult.ToString();

            if (operationCompare == _languageDef.funcEQ)
            {
                return leftString == rightString;
            }

            if (operationCompare == _languageDef.funcNEQ)
            {
                return leftString != rightString;
            }

            int compareResult = string.Compare(leftString, rightString, StringComparison.InvariantCulture);

            if (operationCompare == _languageDef.funcLEQ)
            {
                return compareResult <= 0;
            }

            if (operationCompare == _languageDef.funcL)
            {
                return compareResult < 0;
            }

            if (operationCompare == _languageDef.funcGEQ)
            {
                return compareResult >= 0;
            }

            if (operationCompare == _languageDef.funcG)
            {
                return compareResult > 0;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Проверка, является ли переданный параметр числом.
        /// </summary>
        /// <param name="o">Объект, который будет проверяться, является ли он числом.</param>
        /// <returns><c>True</c>, если переданный параметр является числом. Иначе <c>false</c>.</returns>
        private static bool IsNumeric(object o)
        {
            decimal tmp;
            return decimal.TryParse(o.ToString(), out tmp);
        }

        /// <summary>
        /// Проверка, является ли переданный параметр датой.
        /// </summary>
        /// <param name="o">Объект, который будет проверяться, является ли он датой.</param>
        /// <returns><c>True</c>, если переданный параметр является датой. Иначе <c>false</c>.</returns>
        private static bool IsDate(object o)
        {
            DateTime work;
            return DateTime.TryParse(o.ToString(), out work);
        }

        /// <summary>
        /// Проверка, является ли переданный параметр <see cref="DataObject"/>.
        /// </summary>
        /// <param name="o">Объект, который будет проверяться, является ли он <see cref="DataObject"/>.</param>
        /// <returns><c>True</c>, если переданный параметр является <see cref="DataObject"/>. Иначе <c>false</c>.</returns>
        private static bool IsDataObject(object o)
        {
            return o.GetType().IsSubclassOf(typeof(DataObject));
        }

        /// <summary>
        /// Проверка, является ли переданный параметр гуидом.
        /// </summary>
        /// <param name="o">Объект, который будет проверяться, является ли он гуидом.</param>
        /// <param name="result">Гуид, получившийся из объекта.</param>
        /// <returns><c>True</c>, если переданный параметр является гуидом. Иначе <c>false</c>.</returns>
        private static bool IsGuid(object o, out Guid result)
        {
            result = Guid.Empty;
            try
            {
                result = new Guid(o.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Преобразует строковое представление объекта в <see cref="decimal"/>.
        /// </summary>
        /// <param name="o">Преобразуемое значение.</param>
        /// <returns>Преобразованное значение.</returns>
        private static decimal ToDecimal(object o)
        {
            return decimal.Parse(o.ToString());
        }

        /// <summary>
        /// Преобразует строковое представление объекта в <see cref="DateTime"/>.
        /// </summary>
        /// <param name="o">Преобразуемое значение.</param>
        /// <returns>Преобразованное значение.</returns>
        private static DateTime ToDate(object o)
        {
            return DateTime.Parse(o.ToString());
        }
    }
}
