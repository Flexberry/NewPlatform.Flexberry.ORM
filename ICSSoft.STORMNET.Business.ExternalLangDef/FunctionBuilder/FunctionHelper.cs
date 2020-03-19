namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FileType;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;
    using ICSSoft.STORMNET.Windows.Forms;

    /// <summary>
    ///     Впомогательный класс для FunctionBuilder'a.
    /// </summary>
    public static class FunctionHelper
    {
        private static readonly ExternalLangDef LangDef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Статическая коллекция работает значительно быстрее, чем <see cref="ExternalLangDef.GetObjectTypeForNetType"/>.
        /// </summary>
        private static readonly Dictionary<ObjectType, Type[]> TypeDict = new Dictionary<ObjectType, Type[]>
        {
            { LangDef.BoolType, new[] { typeof(bool), typeof(bool?) } },
            { LangDef.StringType, new[] { typeof(string), typeof(Enum), typeof(WebFile), typeof(File) } },
            { LangDef.GuidType, new[] { typeof(Guid), typeof(Guid?), typeof(DataObject), typeof(KeyGuid) } },
            { LangDef.DateTimeType, new[] { typeof(DateTime), typeof(DateTime?), typeof(NullableDateTime) } },
            {
                LangDef.NumericType, new[]
                {
                    typeof(short),
                    typeof(short?),
                    typeof(int),
                    typeof(int?),
                    typeof(long),
                    typeof(long?),
                    typeof(ushort),
                    typeof(ushort?),
                    typeof(uint),
                    typeof(uint?),
                    typeof(ulong),
                    typeof(ulong?),
                    typeof(decimal),
                    typeof(decimal?),
                    typeof(double),
                    typeof(double?),
                    typeof(float),
                    typeof(float?),
                    typeof(NullableInt),
                    typeof(NullableDecimal)
                }
            }
        };

        private static readonly MethodInfo ParseIEnumerableMethod = typeof(FunctionHelper).GetMethod(
            nameof(ParseIEnumerable),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        ///     Разобрать объект как IEnumerable of T.
        ///     Вызывается через Reflection.
        /// </summary>
        /// <typeparam name="T">Generic-тип перечисления.</typeparam>
        /// <param name="value">Объект.</param>
        /// <returns>Массив разобранных объектов.</returns>
        private static object[] ParseIEnumerable<T>(object value)
        {
            object[] res = null;

            var ts = (value as IEnumerable)?.OfType<T>().ToArray();
            if (ts?.Any() == true)
            {
                res = new object[ts.Length];
                for (int i = 0; i < ts.Length; i++)
                {
                    res[i] = ts[i];
                }
            }

            return res;
        }

        private static ObjectType GetObjectTypeForNetType(Type type)
        {
            return TypeDict.Where(pair => pair.Value.Any(x => type.IsSubclassOf(x) || x == type)).Select(pair => pair.Key).FirstOrDefault()
                ?? LangDef.GetObjectTypeForNetType(type);
        }

        /// <summary>
        ///     Является ли GuidType или DataObjectType.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsKeyType(Type type)
        {
            return GetObjectType(type) == LangDef.GuidType;
        }

        #region Validate

        /// <summary>
        ///     Проверить VariableDef на валидность.
        /// </summary>
        /// <param name="vd">Переменная ограничения.</param>
        /// <exception cref="ArgumentNullException">Переменная ограничения пуста.</exception>
        /// <exception cref="ArgumentException">vd.Type - пусто.</exception>
        /// <exception cref="ArgumentException">vd.StringedView - пусто.</exception>
        internal static void ValidateVariableDef(VariableDef vd)
        {
            if (vd == null)
            {
                throw new ArgumentNullException(nameof(vd));
            }

            if (vd.Type == null)
            {
                throw new ArgumentException(nameof(vd.Type), $"{nameof(vd)}.{nameof(vd.Type)} - пусто.");
            }

            if (string.IsNullOrEmpty(vd.StringedView))
            {
                throw new ArgumentException(nameof(vd.StringedView), $"{nameof(vd)}.{nameof(vd.StringedView)} - пусто.");
            }
        }

        /// <summary>
        ///     Проверить пару VariableDef на совместимость.
        /// </summary>
        /// <param name="vd1">Переменная ограничения 1.</param>
        /// <param name="vd2">Переменная ограничения 2.</param>
        /// <exception cref="ArgumentException">Типы переменных ограничения несовместимы.</exception>
        internal static void ValidateCompatibleVariableDefs(VariableDef vd1, VariableDef vd2)
        {
            if (vd1.Type.NetCompatibilityType != vd2.Type.NetCompatibilityType)
            {
                throw new ArgumentException(nameof(vd2), $"{vd1.Type} несовместим с {vd2.Type}");
            }
        }

        /// <summary>
        ///     Проверить DetailVariableDef на валидность.
        /// </summary>
        /// <param name="dvd">Переменная ограничения.</param>
        /// <exception cref="ArgumentNullException">Переменная ограничения пуста.</exception>
        /// <exception cref="ArgumentException">dvd.Type - пусто.</exception>
        /// <exception cref="ArgumentException">dvd.View - пусто.</exception>
        internal static void ValidateDetailVariableDef(DetailVariableDef dvd)
        {
            if (dvd == null)
            {
                throw new ArgumentNullException(nameof(dvd));
            }

            if (dvd.Type == null)
            {
                throw new ArgumentException(nameof(dvd.Type), $"{nameof(dvd)}.{nameof(dvd.Type)} - пусто.");
            }

            if (dvd.View == null)
            {
                throw new ArgumentException(nameof(dvd.View), $"{nameof(dvd)}.{nameof(dvd.View)} - пусто.");
            }

            if (string.IsNullOrEmpty(dvd.ConnectMasterPorp))
            {
                throw new ArgumentException(nameof(dvd.ConnectMasterPorp), $"{nameof(dvd)}.{nameof(dvd.ConnectMasterPorp)} - пусто.");
            }

            if (dvd.OwnerConnectProp == null)
            {
                throw new ArgumentException(nameof(dvd.OwnerConnectProp), $"{nameof(dvd)}.{nameof(dvd.OwnerConnectProp)} - пусто.");
            }
        }

        /// <summary>
        ///     Проверить лямбда-выражение на валидность.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <exception cref="ArgumentNullException">Лямбда пуста.</exception>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        internal static void ValidateExpression<T>(Expression<Func<T, object>> propExpression)
        {
            if (propExpression == null)
            {
                throw new ArgumentNullException(nameof(propExpression));
            }
        }

        /// <summary>
        ///     Проверить propertyName на валидность.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <exception cref="ArgumentNullException">Имя свойства пусто.</exception>
        internal static void ValidatePropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
        }

        /// <summary>
        ///     Проверить functionString на валидность.
        /// </summary>
        /// <param name="functionString">Имя функции.</param>
        /// <param name="availableFunctions">Допустимые функции.</param>
        /// <exception cref="ArgumentNullException">Функция пуста.</exception>
        /// <exception cref="ArgumentException">Передана некорректная функция.</exception>
        internal static void ValidateFunctionString(string functionString, params string[] availableFunctions)
        {
            if (string.IsNullOrWhiteSpace(functionString))
            {
                throw new ArgumentNullException(nameof(functionString));
            }

            if (!availableFunctions.Contains(functionString))
            {
                throw new ArgumentException(nameof(functionString));
            }
        }

        /// <summary>
        ///     Проверить value на валидность.
        /// </summary>
        /// <param name="value">Значение свойства.</param>
        /// <exception cref="ArgumentNullException">Передан пустой параметр.</exception>
        internal static void ValidateValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        ///     Проверить objType на валидность.
        /// </summary>
        /// <param name="objType">Тип свойства.</param>
        /// <exception cref="ArgumentNullException">Тип свойства пуст.</exception>
        internal static void ValidateObjType(ObjectType objType)
        {
            if (objType == null)
            {
                throw new ArgumentNullException(nameof(objType));
            }
        }

        /// <summary>
        ///     Проверить function на валидность.
        /// </summary>
        /// <param name="function">Функция.</param>
        /// <exception cref="ArgumentNullException">Функция ограничения пуста.</exception>
        internal static void ValidateFunction(Function function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }
        }

        #endregion Validate

        /// <summary>
        ///     Получатить ObjectType по .NET-типу (для DataObject возвращается тип первичного ключа).
        /// </summary>
        /// <param name="type">.NET-тип.</param>
        /// <exception cref="ArgumentException">Невозможно привести к ObjectType-типу.</exception>
        /// <returns>ObjectType-тип.</returns>
        internal static ObjectType GetObjectType(Type type)
        {
            var objectType = GetObjectTypeForNetType(type);

            if (objectType == null)
            {
                throw new ArgumentException(nameof(type), $"Невозможно привести к {typeof(ObjectType).Name}-типу.");
            }

            return objectType;
        }

        /// <summary>
        ///     Получить VariableDef по лямбде.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="propExpression">Лямбда-имя свойства.</param>
        /// <returns>VariableDef.</returns>
        public static VariableDef GetVarDef<T>(Expression<Func<T, object>> propExpression)
        {
            string propName = Information.ExtractPropertyPath(propExpression);
            var type = Information.GetPropertyType(typeof(T), propName);
            var objectType = GetObjectType(type);
            return new VariableDef(objectType, propName);
        }

        /// <summary>
        ///     Получить простой DetailVariableDef.
        /// </summary>
        /// <param name="connectMasterProp">Путь от детейла.</param>
        /// <param name="view">Представление детейла.</param>
        /// <param name="ownerConnectProp">Путь от агрегатора.</param>
        /// <returns>DetailVariableDef.</returns>
        public static DetailVariableDef GetDetailVarDef(
            View view,
            string connectMasterProp = null,
            params string[] ownerConnectProp)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (ownerConnectProp.Length == 0)
            {
                ownerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey };
            }

            return new DetailVariableDef
            {
                Type = LangDef.DetailsType,
                View = view,
                OwnerConnectProp = ownerConnectProp,
                ConnectMasterPorp = connectMasterProp ?? Information.GetAgregatePropertyName(view.DefineClassType)
            };
        }

        /// <summary>
        ///     Разобрать объект как IEnumerable.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="value">Объект.</param>
        /// <returns>Массив разобранных объектов.</returns>
        internal static object[] ParseObject(Type type, object value)
        {
            object[] res = null;

            var innerObjectType = GetObjectType(type);

            var types = TypeDict[innerObjectType];
            foreach (var t in types)
            {
                var generic = ParseIEnumerableMethod.MakeGenericMethod(t);
                res = generic.Invoke(null, new[] { value }) as object[];
                if (res != null)
                {
                    if (t == typeof(Enum))
                    {
                        res = res.Select(EnumCaption.GetCaptionFor).Cast<object>().ToArray();
                    }

                    break;
                }
            }

            return res;
        }

        /// <summary>
        ///     Проверить и преобразовать аргумент в соответствии с типом.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <param name="value">Значение.</param>
        /// <exception cref="ArgumentException">Аргумент не содержит ключевой структуры.</exception>
        /// <exception cref="InvalidCastException">Не совпадают тип свойства и тип переданного параметра.</exception>
        /// <exception cref="FormatException"><seealso cref="M:Convert.ChangeType(object, Type)"/></exception>
        /// <returns>Преобразованный аргумент.</returns>
        internal static object ConvertValue(Type type, object value)
        {
            ValidateValue(value);

            var nctType = GetObjectType(type).NetCompatibilityType;
            var res = value;

            if (res.GetType().IsEnum)
            {
                if (nctType == typeof(string))
                {
                    res = EnumCaption.GetCaptionFor(res);
                }
                else if (nctType == typeof(decimal))
                {
                    res = Convert.ChangeType(res, nctType);
                }
            }

            if (IsKeyType(nctType))
            {
                res = PKHelper.GetKeyByObject(res);
                if (res == null)
                {
                    throw new ArgumentException(nameof(value));
                }
            }

            ValidateValue(res);

            // Когда свойство строкового типа, а значение - не строка.
            if (nctType == typeof(string) && GetObjectType(res.GetType()).NetCompatibilityType != typeof(string))
            {
                res = Convert.ToString(res);
            }

            if (GetObjectType(res.GetType()).NetCompatibilityType != nctType)
            {
                if (res.GetType().GetInterfaces().Any(x => x == typeof(IConvertible)))
                {
                    // Попробуем преобразовать в нужный тип.
                    res = Convert.ChangeType(res, type);
                }
                else
                {
                    throw new InvalidCastException(nameof(value));
                }
            }

            return res;
        }

        /// <summary>
        ///     Получить уникальные объекты типа type из перечисления values.
        /// </summary>
        /// <param name="type">Тип объектов.</param>
        /// <param name="values">Перечисление объектов.</param>
        /// <returns>Массив объектов.</returns>
        internal static object[] GetUniqueObjects(Type type, params object[] values)
        {
            ValidateValue(values);

            var res = new List<object>();
            foreach (var val in values.Where(x => x != null))
            {
                var objects = ParseObject(type, val);
                if (objects != null)
                {
                    res.AddRange(objects);
                }
                else
                {
                    var v = ConvertValue(type, val);
                    if (v != null)
                    {
                        res.Add(v);
                    }
                }
            }

            return res.Distinct().ToArray();
        }
    }
}
