namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    /// <summary>
    ///     Вспомогательный класс для работы с функциями ограничений.
    /// </summary>
    public static class FunctionBuilder
    {
        private static readonly ExternalLangDef LangDef = ExternalLangDef.LanguageDef;

        /// <summary>
        ///     Построить функцию ограничения "True".
        /// </summary>
        /// <returns>Функция.</returns>
        public static Function BuildTrue()
        {
            return LangDef.GetFunction(LangDef.funcEQ, 1, 1);
        }

        /// <summary>
        ///     Построить функцию ограничения "False".
        /// </summary>
        /// <returns>Функция.</returns>
        public static Function BuildFalse()
        {
            return LangDef.GetFunction(LangDef.funcEQ, 1, 0);
        }

        /// <summary>
        ///     Построить функцию ограничения по SQL-запросу.
        /// </summary>
        /// <param name="sql">SQL-запрос.</param>
        /// <returns>Функция.</returns>
        public static Function BuildSQL(string sql)
        {
            return LangDef.GetFunction(LangDef.funcSQL, sql);
        }

        /// <summary>
        ///     Построить отрицание функции.
        /// </summary>
        /// <param name="function">Функция для отрицания.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNot(Function function)
        {
            FunctionHelper.ValidateFunction(function);

            return LangDef.GetFunction(LangDef.funcNOT, function);
        }

        #region IsNull

        /// <summary>
        ///     Построить функцию "IS NULL".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIsNull(VariableDef vd)
        {
            FunctionHelper.ValidateVariableDef(vd);

            return LangDef.GetFunction(LangDef.funcIsNull, vd);
        }

        /// <summary>
        ///     Построить функцию "IS NULL".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIsNull(string propertyName)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            return BuildIsNull(new VariableDef(LangDef.StringType, propertyName));
        }

        /// <summary>
        ///     Построить функцию "IS NULL".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIsNull<T>(Expression<Func<T, object>> propExpression)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildIsNull(vd);
        }

        #endregion

        #region IsNotNull

        /// <summary>
        ///     Построить функцию "IS NOT NULL".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIsNotNull(VariableDef vd)
        {
            FunctionHelper.ValidateVariableDef(vd);

            return LangDef.GetFunction(LangDef.funcNotIsNull, vd);
        }

        /// <summary>
        ///     Построить функцию "IS NOT NULL".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIsNotNull(string propertyName)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            return BuildIsNotNull(new VariableDef(LangDef.StringType, propertyName));
        }

        /// <summary>
        ///     Построить функцию "IS NOT NULL".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIsNotNull<T>(Expression<Func<T, object>> propExpression)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildIsNotNull(vd);
        }

        #endregion

        #region And Or

        /// <summary>
        ///     Построить функцию OR или AND.
        /// </summary>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="values">Функции для объединения.</param>
        /// <returns></returns>
        internal static Function Build(string functionString, params Function[] values)
        {
            FunctionHelper.ValidateFunctionString(functionString, LangDef.funcAND, LangDef.funcOR);

            if (values == null)
            {
                return null;
            }

            Function result;
            var funcs = values.Where(a => a != null).Cast<object>().ToArray();
            switch (funcs.Length)
            {
                case 0:
                    result = null;
                    break;
                case 1:
                    result = funcs[0] as Function;
                    break;
                default:
                    result = LangDef.GetFunction(functionString, funcs);
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Построить функцию ограничения "AND".
        /// </summary>
        /// <param name="functions">Функции для объединения.</param>
        /// <returns>Функция.</returns>
        public static Function BuildAnd(params Function[] functions)
        {
            return Build(LangDef.funcAND, functions);
        }

        /// <summary>
        ///     Построить функцию ограничения "AND".
        /// </summary>
        /// <param name="functions">Функции для объединения.</param>
        /// <returns>Функция.</returns>
        public static Function BuildAnd(IEnumerable<Function> functions)
        {
            return functions == null ? null : BuildAnd(functions.ToArray());
        }

        /// <summary>
        ///     Построить функцию ограничения "OR".
        /// </summary>
        /// <param name="functions">Функции для объединения.</param>
        /// <returns>Функция.</returns>
        public static Function BuildOr(params Function[] functions)
        {
            return Build(LangDef.funcOR, functions);
        }

        /// <summary>
        ///     Построить функцию ограничения "OR".
        /// </summary>
        /// <param name="functions">Функции для объединения.</param>
        /// <returns>Функция.</returns>
        public static Function BuildOr(IEnumerable<Function> functions)
        {
            return functions == null ? null : BuildOr(functions.ToArray());
        }

        #endregion

        #region Equals

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals(VariableDef vd, object value)
        {
            FunctionHelper.ValidateVariableDef(vd);

            string stringed = value?.ToString();

            return value == null || stringed == string.Empty
                ? BuildIsNull(vd)
                : LangDef.GetFunction(LangDef.funcEQ, vd, FunctionHelper.ConvertValue(vd.Type.NetCompatibilityType, value));
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство двух свойств.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals(VariableDef vd1, VariableDef vd2)
        {
            FunctionHelper.ValidateVariableDef(vd1);
            FunctionHelper.ValidateVariableDef(vd2);
            FunctionHelper.ValidateCompatibleVariableDefs(vd1, vd2);

            return LangDef.GetFunction(LangDef.funcEQ, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals(VariableDef vd, Function function)
        {
            FunctionHelper.ValidateVariableDef(vd);
            FunctionHelper.ValidateFunction(function);

            return LangDef.GetFunction(LangDef.funcEQ, vd, function);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals(string propertyName, ObjectType objType, object value)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            FunctionHelper.ValidateObjType(objType);

            return BuildEquals(new VariableDef(objType, propertyName), value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildEquals(string propertyName, object value)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            Function result;
            if (value == null)
            {
                result = BuildIsNull(propertyName);
            }
            else
            {
                var keyGuid = PKHelper.GetKeyByObject(value);
                var objType = keyGuid != null
                    ? LangDef.GuidType
                    : FunctionHelper.GetObjectType(value.GetType());
                result = BuildEquals(propertyName, objType, value);
            }

            return result;
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство первичного ключа.
        /// </summary>
        /// <param name="value">Ключевая структура.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals(object value)
        {
            FunctionHelper.ValidateValue(value);

            return BuildEquals(new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство первичного ключа.
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals(Function function)
        {
            return BuildEquals(new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), function);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals<T>(Expression<Func<T, object>> propExpression, object value)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildEquals(vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство двух свойств.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            FunctionHelper.ValidateExpression(propExpression1);
            FunctionHelper.ValidateExpression(propExpression2);

            var vd1 = FunctionHelper.GetVarDef(propExpression1);
            var vd2 = FunctionHelper.GetVarDef(propExpression2);

            return BuildEquals(vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEquals<T>(Expression<Func<T, object>> propExpression, Function function)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildEquals(vd, function);
        }

        #endregion

        #region NotEquals

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство некоторого свойства.
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals(VariableDef vd, object value)
        {
            FunctionHelper.ValidateVariableDef(vd);

            return value == null
                ? BuildIsNotNull(vd)
                : LangDef.GetFunction(LangDef.funcNEQ, vd, FunctionHelper.ConvertValue(vd.Type.NetCompatibilityType, value));
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство двух свойств.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals(VariableDef vd1, VariableDef vd2)
        {
            FunctionHelper.ValidateVariableDef(vd1);
            FunctionHelper.ValidateVariableDef(vd2);
            FunctionHelper.ValidateCompatibleVariableDefs(vd1, vd2);

            return LangDef.GetFunction(LangDef.funcNEQ, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на равенство некоторого свойства.
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals(VariableDef vd, Function function)
        {
            FunctionHelper.ValidateVariableDef(vd);
            FunctionHelper.ValidateFunction(function);

            return LangDef.GetFunction(LangDef.funcNEQ, vd, function);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство некоторого свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals(string propertyName, ObjectType objType, object value)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            FunctionHelper.ValidateObjType(objType);

            return BuildNotEquals(new VariableDef(objType, propertyName), value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство некоторого свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildNotEquals(string propertyName, object value)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            Function result;
            if (value == null)
            {
                result = BuildIsNotNull(propertyName);
            }
            else
            {
                var keyGuid = PKHelper.GetKeyByObject(value);
                var objType = keyGuid != null
                    ? LangDef.GuidType
                    : FunctionHelper.GetObjectType(value.GetType());
                result = BuildNotEquals(propertyName, objType, value);
            }

            return result;
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство первичного ключа.
        /// </summary>
        /// <param name="value">Ключевая структура.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals(object value)
        {
            FunctionHelper.ValidateValue(value);

            return BuildNotEquals(new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство первичного ключа.
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals(Function function)
        {
            return BuildNotEquals(new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), function);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство некоторого свойства.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals<T>(Expression<Func<T, object>> propExpression, object value)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildNotEquals(vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство двух свойств.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            FunctionHelper.ValidateExpression(propExpression1);
            FunctionHelper.ValidateExpression(propExpression2);

            var vd1 = FunctionHelper.GetVarDef(propExpression1);
            var vd2 = FunctionHelper.GetVarDef(propExpression2);

            return BuildNotEquals(vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство некоторого свойства.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotEquals<T>(Expression<Func<T, object>> propExpression, Function function)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildNotEquals(vd, function);
        }

        #endregion

        #region Like

        /// <summary>
        ///     Проверяет строку по шаблону.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <exception cref="ArgumentNullException">Шаблон пуст.</exception>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildLike(string propertyName, string pattern)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return LangDef.GetFunction(LangDef.funcLike, new VariableDef(LangDef.StringType, propertyName), pattern);
        }

        /// <summary>
        ///     Проверяет строку по шаблону.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <exception cref="InvalidCastException">Тип свойства не является строкой.</exception>
        /// <exception cref="ArgumentNullException">Шаблон пуст.</exception>
        /// <returns>Функция.</returns>
        public static Function BuildLike<T>(Expression<Func<T, object>> propExpression, string pattern)
        {
            FunctionHelper.ValidateExpression(propExpression);

            string propName = Information.ExtractPropertyPath(propExpression);

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return LangDef.GetFunction(LangDef.funcLike, new VariableDef(LangDef.StringType, propName), pattern);
        }

        /// <summary>
        ///     Проверяет начало строки по шаблону.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildStartsWith(string propertyName, string pattern)
        {
            return BuildLike(propertyName, string.IsNullOrWhiteSpace(pattern) ? null : $"{pattern}%");
        }

        /// <summary>
        ///     Проверяет начало строки по шаблону.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <returns>Функция.</returns>
        public static Function BuildStartsWith<T>(Expression<Func<T, object>> propExpression, string pattern)
        {
            return BuildLike(propExpression, string.IsNullOrWhiteSpace(pattern) ? null : $"{pattern}%");
        }

        /// <summary>
        ///     Проверяет конец строки по шаблону.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildEndsWith(string propertyName, string pattern)
        {
            return BuildLike(propertyName, string.IsNullOrWhiteSpace(pattern) ? null : $"%{pattern}");
        }

        /// <summary>
        ///     Проверяет конец строки по шаблону.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <returns>Функция.</returns>
        public static Function BuildEndsWith<T>(Expression<Func<T, object>> propExpression, string pattern)
        {
            return BuildLike(propExpression, string.IsNullOrWhiteSpace(pattern) ? null : $"%{pattern}");
        }

        /// <summary>
        ///     Проверяет наличие подстроки по шаблону.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildContains(string propertyName, string pattern)
        {
            return BuildLike(propertyName, string.IsNullOrWhiteSpace(pattern) ? null : $"%{pattern}%");
        }

        /// <summary>
        ///     Проверяет наличие подстроки по шаблону.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="pattern">Шаблон строки.</param>
        /// <returns>Функция.</returns>
        public static Function BuildContains<T>(Expression<Func<T, object>> propExpression, string pattern)
        {
            return BuildLike(propExpression, string.IsNullOrWhiteSpace(pattern) ? null : $"%{pattern}%");
        }

        #endregion

        #region Compare

        /// <summary>
        ///     Построить функцию сравнения.
        /// </summary>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildCompare(string functionString, VariableDef vd, object value)
        {
            FunctionHelper.ValidateFunctionString(functionString, LangDef.funcL, LangDef.funcG, LangDef.funcLEQ, LangDef.funcGEQ);
            FunctionHelper.ValidateVariableDef(vd);
            FunctionHelper.ValidateValue(value);

            return LangDef.GetFunction(functionString, vd, FunctionHelper.ConvertValue(vd.Type.NetCompatibilityType, value));
        }

        /// <summary>
        ///     Построить функцию сравнения двух свойств.
        /// </summary>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildCompare(string functionString, VariableDef vd1, VariableDef vd2)
        {
            FunctionHelper.ValidateFunctionString(functionString, LangDef.funcL, LangDef.funcG, LangDef.funcLEQ, LangDef.funcGEQ);
            FunctionHelper.ValidateVariableDef(vd1);
            FunctionHelper.ValidateVariableDef(vd2);
            FunctionHelper.ValidateCompatibleVariableDefs(vd1, vd2);

            return LangDef.GetFunction(functionString, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию сравнения.
        /// </summary>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildCompare(string functionString, string propertyName, ObjectType objType, object value)
        {
            FunctionHelper.ValidatePropertyName(propertyName);
            FunctionHelper.ValidateObjType(objType);
            FunctionHelper.ValidateValue(value);

            return BuildCompare(functionString, new VariableDef(objType, propertyName), value);
        }

        /// <summary>
        ///     Построить функцию сравнения.
        /// </summary>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildCompare(string functionString, string propertyName, object value)
        {
            FunctionHelper.ValidateValue(value);

            return BuildCompare(functionString, propertyName, FunctionHelper.GetObjectType(value.GetType()), value);
        }

        /// <summary>
        ///     Построить функцию сравнения.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildCompare<T>(string functionString, Expression<Func<T, object>> propExpression, object value)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildCompare(functionString, vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения объекта на неравенство двух свойств.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildCompare<T>(string functionString, Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            FunctionHelper.ValidateExpression(propExpression1);
            FunctionHelper.ValidateExpression(propExpression2);

            var vd1 = FunctionHelper.GetVarDef(propExpression1);
            var vd2 = FunctionHelper.GetVarDef(propExpression2);

            return BuildCompare(functionString, vd1, vd2);
        }

        #endregion

        #region Less

        /// <summary>
        ///     Построить функцию "LESS".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLess(VariableDef vd, object value)
        {
            return BuildCompare(LangDef.funcL, vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "LESS" для двух свойств объекта.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLess(VariableDef vd1, VariableDef vd2)
        {
            return BuildCompare(LangDef.funcL, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию "LESS".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLess(string propertyName, ObjectType objType, object value)
        {
            return BuildCompare(LangDef.funcL, propertyName, objType, value);
        }

        /// <summary>
        ///     Построить функцию "LESS".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildLess(string propertyName, object value)
        {
            return BuildCompare(LangDef.funcL, propertyName, value);
        }

        /// <summary>
        ///     Построить функцию "LESS".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLess<T>(Expression<Func<T, object>> propExpression, object value)
        {
            return BuildCompare(LangDef.funcL, propExpression, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "LESS" для двух свойств объекта.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLess<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            return BuildCompare(LangDef.funcL, propExpression1, propExpression2);
        }

        #endregion

        #region LessOrEqual

        /// <summary>
        ///     Построить функцию "LessOrEqual".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLessOrEqual(VariableDef vd, object value)
        {
            return BuildCompare(LangDef.funcLEQ, vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "LessOrEqual" для двух свойств объекта.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLessOrEqual(VariableDef vd1, VariableDef vd2)
        {
            return BuildCompare(LangDef.funcLEQ, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию "LessOrEqual".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLessOrEqual(string propertyName, ObjectType objType, object value)
        {
            return BuildCompare(LangDef.funcLEQ, propertyName, objType, value);
        }

        /// <summary>
        ///     Построить функцию "LessOrEqual".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildLessOrEqual(string propertyName, object value)
        {
            return BuildCompare(LangDef.funcLEQ, propertyName, value);
        }

        /// <summary>
        ///     Построить функцию "LessOrEqual".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLessOrEqual<T>(Expression<Func<T, object>> propExpression, object value)
        {
            return BuildCompare(LangDef.funcLEQ, propExpression, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "LessOrEqual" для двух свойств объекта.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildLessOrEqual<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            return BuildCompare(LangDef.funcLEQ, propExpression1, propExpression2);
        }

        #endregion

        #region Greater

        /// <summary>
        ///     Построить функцию "Greater".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreater(VariableDef vd, object value)
        {
            return BuildCompare(LangDef.funcG, vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "Greater" для двух свойств объекта.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreater(VariableDef vd1, VariableDef vd2)
        {
            return BuildCompare(LangDef.funcG, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию "Greater".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreater(string propertyName, ObjectType objType, object value)
        {
            return BuildCompare(LangDef.funcG, propertyName, objType, value);
        }

        /// <summary>
        ///     Построить функцию "Greater".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildGreater(string propertyName, object value)
        {
            return BuildCompare(LangDef.funcG, propertyName, value);
        }

        /// <summary>
        ///     Построить функцию "Greater".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreater<T>(Expression<Func<T, object>> propExpression, object value)
        {
            return BuildCompare(LangDef.funcG, propExpression, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "Greater" для двух свойств объекта.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreater<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            return BuildCompare(LangDef.funcG, propExpression1, propExpression2);
        }

        #endregion

        #region GreaterOrEqual

        /// <summary>
        ///     Построить функцию "GreaterOrEqual".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreaterOrEqual(VariableDef vd, object value)
        {
            return BuildCompare(LangDef.funcGEQ, vd, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "GreaterOrEqual" для двух свойств объекта.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreaterOrEqual(VariableDef vd1, VariableDef vd2)
        {
            return BuildCompare(LangDef.funcGEQ, vd1, vd2);
        }

        /// <summary>
        ///     Построить функцию "GreaterOrEqual".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreaterOrEqual(string propertyName, ObjectType objType, object value)
        {
            return BuildCompare(LangDef.funcGEQ, propertyName, objType, value);
        }

        /// <summary>
        ///     Построить функцию "GreaterOrEqual".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildGreaterOrEqual(string propertyName, object value)
        {
            return BuildCompare(LangDef.funcGEQ, propertyName, value);
        }

        /// <summary>
        ///     Построить функцию "GreaterOrEqual".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreaterOrEqual<T>(Expression<Func<T, object>> propExpression, object value)
        {
            FunctionHelper.ValidateExpression(propExpression);

            return BuildCompare(LangDef.funcGEQ, propExpression, value);
        }

        /// <summary>
        ///     Построить функцию ограничения "GreaterOrEqual" для двух свойств объекта.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <returns>Функция.</returns>
        public static Function BuildGreaterOrEqual<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2)
        {
            return BuildCompare(LangDef.funcGEQ, propExpression1, propExpression2);
        }

        #endregion

        #region Between

        /// <summary>
        ///     Построить функцию "Between".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="value1">Первое значение свойства.</param>
        /// <param name="value2">Второе значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildBetween(VariableDef vd, object value1, object value2)
        {
            FunctionHelper.ValidateVariableDef(vd);
            FunctionHelper.ValidateValue(value1);
            FunctionHelper.ValidateValue(value2);

            var tValue1 = FunctionHelper.ConvertValue(vd.Type.NetCompatibilityType, value1);
            var tValue2 = FunctionHelper.ConvertValue(vd.Type.NetCompatibilityType, value2);

            return LangDef.GetFunction(LangDef.funcBETWEEN, vd, tValue1, tValue2);
        }

        /// <summary>
        ///     Построить функцию "Between".
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildBetween(VariableDef vd1, VariableDef vd2, object value)
        {
            FunctionHelper.ValidateVariableDef(vd1);
            FunctionHelper.ValidateVariableDef(vd2);
            FunctionHelper.ValidateValue(value);

            var tValue1 = FunctionHelper.ConvertValue(vd1.Type.NetCompatibilityType, value);
            var tValue2 = FunctionHelper.ConvertValue(vd2.Type.NetCompatibilityType, value);

            if (!tValue1.Equals(tValue2))
            {
                throw new InvalidCastException(nameof(value));
            }

            return LangDef.GetFunction(LangDef.funcBETWEEN, vd1, vd2, tValue2);
        }

        /// <summary>
        ///     Построить функцию "Between".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="value1">Первое значение свойства.</param>
        /// <param name="value2">Второе значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildBetween(string propertyName, ObjectType objType, object value1, object value2)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            FunctionHelper.ValidateObjType(objType);

            return BuildBetween(new VariableDef(objType, propertyName), value1, value2);
        }

        /// <summary>
        ///     Построить функцию "Between".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="value1">Первое значение свойства.</param>
        /// <param name="value2">Второе значение свойства.</param>
        /// <exception cref="ArgumentException">ObjectType-типы переданных значений различны.</exception>
        /// <returns>Функция.</returns>
        [Obsolete("Данный метод не является безопасным, используйте Generic-вариант.")]
        public static Function BuildBetween(string propertyName, object value1, object value2)
        {
            FunctionHelper.ValidatePropertyName(propertyName);
            FunctionHelper.ValidateValue(value1);
            FunctionHelper.ValidateValue(value2);

            var type1 = FunctionHelper.GetObjectType(value1.GetType());
            var type2 = FunctionHelper.GetObjectType(value2.GetType());
            if (type1 != type2)
            {
                throw new ArgumentException($"ObjectType-типы {nameof(value1)} и {nameof(value2)} не совпадают.");
            }

            return BuildBetween(propertyName, type1, value1, value2);
        }

        /// <summary>
        ///     Построить функцию "Between".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="value1">Первое значение свойства.</param>
        /// <param name="value2">Второе значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildBetween<T>(Expression<Func<T, object>> propExpression, object value1, object value2)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildBetween(vd, value1, value2);
        }

        /// <summary>
        ///     Построить функцию "Between".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression1">Лямбда-имя свойства 1.</param>
        /// <param name="propExpression2">Лямбда-имя свойства 2.</param>
        /// <param name="value">Значение свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildBetween<T>(Expression<Func<T, object>> propExpression1, Expression<Func<T, object>> propExpression2, object value)
        {
            FunctionHelper.ValidateExpression(propExpression1);
            FunctionHelper.ValidateExpression(propExpression2);

            var vd1 = FunctionHelper.GetVarDef(propExpression1);
            var vd2 = FunctionHelper.GetVarDef(propExpression2);

            return BuildBetween(vd1, vd2, value);
        }

        #endregion

        #region In

        internal static Function Build(string functionString, VariableDef vd, params object[] values)
        {
            FunctionHelper.ValidateVariableDef(vd);
            FunctionHelper.ValidateValue(values);

            var uniqValues = FunctionHelper.IsKeyType(vd.Type.NetCompatibilityType)
                ? PKHelper.GetKeys(values)
                : FunctionHelper.GetUniqueObjects(vd.Type.NetCompatibilityType, values);

            if (!uniqValues.Any())
            {
                return BuildFalse();
            }

            if (uniqValues.Length == 1 && functionString == LangDef.funcIN)
            {
                functionString = LangDef.funcEQ;
            }

            var funcParams = new object[uniqValues.Length + 1];
            funcParams[0] = vd;
            for (int i = 0; i < uniqValues.Length; i++)
            {
                funcParams[i + 1] = uniqValues[i];
            }

            return LangDef.GetFunction(functionString, funcParams);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="values">Значения свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIn(VariableDef vd, params object[] values)
        {
            FunctionHelper.ValidateVariableDef(vd);

            return Build(LangDef.funcIN, vd, values);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIn(VariableDef vd, Function function)
        {
            FunctionHelper.ValidateVariableDef(vd);
            FunctionHelper.ValidateFunction(function);

            return LangDef.GetFunction(LangDef.funcIN, vd, function);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="values">Значения свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIn(string propertyName, ObjectType objType, params object[] values)
        {
            FunctionHelper.ValidatePropertyName(propertyName);

            FunctionHelper.ValidateObjType(objType);

            return BuildIn(new VariableDef(objType, propertyName), values);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="values">Значения свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIn<T>(Expression<Func<T, object>> propExpression, params object[] values)
        {
            if (propExpression == null)
            {
                return BuildIn(values);
            }

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildIn(vd, values);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIn<T>(Expression<Func<T, object>> propExpression, Function function)
        {
            FunctionHelper.ValidateExpression(propExpression);

            var vd = FunctionHelper.GetVarDef(propExpression);

            return BuildIn(vd, function);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <param name="values">Значения свойства.</param>
        /// <exception cref="ArgumentException">Значения не содержат ключевые структуры.</exception>
        /// <returns>Функция.</returns>
        public static Function BuildIn(params object[] values)
        {
            // TODO: придумать валидацию совсем ужасных параметров.
            // PKHelper.GetKeys перерабатывает любые значения без исключений (откатить\измененить тесты).
            return BuildIn(new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), values);
        }

        /// <summary>
        ///     Построить функцию ограничения "среди значений".
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildIn(Function function)
        {
            return BuildIn(new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), function);
        }

        #endregion

        #region NotIn

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="values">Значения свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn(VariableDef vd, params object[] values)
        {
            return BuildNot(BuildIn(vd, values));
        }

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn(VariableDef vd, Function function)
        {
            return BuildNot(BuildIn(vd, function));
        }

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <param name="objType">Тип свойства.</param>
        /// <param name="values">Значения свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn(string propertyName, ObjectType objType, params object[] values)
        {
            return BuildNot(BuildIn(propertyName, objType, values));
        }

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="values">Значения свойства.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn<T>(Expression<Func<T, object>> propExpression, params object[] values)
        {
            return BuildNot(BuildIn(propExpression, values));
        }

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn<T>(Expression<Func<T, object>> propExpression, Function function)
        {
            return BuildNot(BuildIn(propExpression, function));
        }

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <param name="values">Значения свойства.</param>
        /// <exception cref="ArgumentException">Значения не содержат ключевые структуры.</exception>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn(params object[] values)
        {
            return BuildNot(BuildIn(values));
        }

        /// <summary>
        ///     Построить функцию ограничения "не среди значений".
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <returns>Функция.</returns>
        public static Function BuildNotIn(Function function)
        {
            return BuildNot(BuildIn(function));
        }

        #endregion

        #region Exists

        /// <summary>
        ///     Построить функцию органичения "Существуют такие".
        /// </summary>
        /// <param name="dvd">Переменная ограничения по детейлам.</param>
        /// <param name="function">Функция ограничения по детейлам.</param>
        /// <returns>Функция.</returns>
        public static Function BuildExists(DetailVariableDef dvd, Function function = null)
        {
            FunctionHelper.ValidateDetailVariableDef(dvd);

            var internalFunction = function ?? BuildTrue();

            return LangDef.GetFunction(LangDef.funcExist, dvd, internalFunction);
        }

        /// <summary>
        ///     Построить функцию органичения "Существуют такие".
        /// </summary>
        /// <param name="connectMasterPorp">Имя свойства от детейла к агрегатору.</param>
        /// <param name="view">Представление детейла.</param>
        /// <param name="function">Функция органичения по детейлу.</param>
        /// <returns>Функция.</returns>
        public static Function BuildExists(string connectMasterPorp, View view, Function function = null)
        {
            return BuildExists(FunctionHelper.GetDetailVarDef(view, connectMasterPorp), function);
        }

        #endregion
    }
}