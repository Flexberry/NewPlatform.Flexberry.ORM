namespace ICSSoft.STORMNET.Convertors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Класс конвертации значений одного типа к другому типу посредством одного преобразования (Implicit или Explicit).
    /// Только статические методы работы.
    /// </summary>
    public sealed class InOperatorsConverter
    {
        /// <summary>
        /// Запретим создавать экземпляры этого класса.
        /// </summary>
        private InOperatorsConverter()
        {
        }

        /// <summary>
        /// Кеш разобранных ранее типов.
        /// </summary>
        private static readonly List<Type> ParsedTypes = new List<Type>();

        /// <summary>
        /// Кеш методов конвертации типов.
        /// </summary>
        private static readonly Dictionary<TypeTypePair, MethodInfo> TypeToTypeMethods = new Dictionary<TypeTypePair, MethodInfo>(new TypeTypePairEqualityComparer());


        /// <summary>
        /// Можно ли преобразовать один тип к другому.
        /// </summary>
        /// <param name="fromType">Откуда будем преобразовывать.</param>
        /// <param name="toType">Куда будем преобразовывать.</param>
        /// <returns>Если можно преобразовать, то <c>true</c>.</returns>
        public static bool CanConvert(Type fromType, Type toType)
        {
            lock (TypeToTypeMethods)
            {
                if ((fromType == typeof(string) && toType.IsEnum)
                    ||
                    (toType == typeof(string) && fromType.IsEnum))
                    return true;

                if ((fromType == typeof(int) && toType.IsEnum)
                    ||
                    (toType == typeof(int) && fromType.IsEnum))
                    return true;

                TypeTypePair key = new TypeTypePair(fromType, toType);
                if (TypeToTypeMethods.ContainsKey(key))
                    return true;

                if (!ParsedTypes.Contains(fromType)) 
                    AddTypeOperator(fromType);

                if (!ParsedTypes.Contains(toType)) 
                    AddTypeOperator(toType);

                return TypeToTypeMethods.ContainsKey(key);
            }
        }

        /// <summary>
        /// Преобразовать значение.
        /// </summary>
        /// <param name="value">Значение для преобразования.</param>
        /// <param name="toType">Тип, в который надо преобразовать.</param>
        /// <returns>Преобразованное значение.</returns>
        public static object Convert(object value, Type toType)
        {
            lock (TypeToTypeMethods)
            {
                Type fromType = value.GetType();

                if (fromType == toType) 
                    return value;

                if (fromType == typeof(string) && toType.IsEnum)
                    return EnumCaption.GetValueFor((string)value, toType);

                if (fromType.IsEnum && toType == typeof(string))
                    return EnumCaption.GetCaptionFor(value);

                if (fromType == typeof(int) && toType.IsEnum)
                    return Enum.Parse(toType, Enum.GetName(toType, value));

                if (fromType.IsEnum && toType == typeof(int))
                    return (int)value;

                TypeTypePair key = new TypeTypePair(fromType, toType);
                if (!ParsedTypes.Contains(fromType)) 
                    AddTypeOperator(fromType);

                if (!ParsedTypes.Contains(toType)) 
                    AddTypeOperator(toType);

                if (TypeToTypeMethods.ContainsKey(key))
                {
                    MethodInfo mi = TypeToTypeMethods[key];
                    return mi.Invoke(null, new[] { value });
                }

                if (toType == typeof(string))
                    return value.ToString();

                return System.Convert.ChangeType(value, toType);
            }
        }

        /// <summary>
        /// Добавить тип и все его преобразования в кеш.
        /// </summary>
        /// <param name="type">Тип для добавления.</param>
        private static void AddTypeOperator(Type type)
        {
            ParsedTypes.Add(type);
            MethodInfo[] mis = type.GetMethods();
            foreach (MethodInfo curmi in mis)
                if (((curmi.IsSpecialName && (curmi.Name == "op_Implicit" || curmi.Name == "op_Explicit")) || curmi.Name == "Parse") && curmi.GetParameters().Length == 1)
                {
                    TypeTypePair key = new TypeTypePair(curmi.GetParameters()[0].ParameterType, curmi.ReturnType);
                    if (!TypeToTypeMethods.ContainsKey(key))
                        TypeToTypeMethods.Add(key, curmi);
                }
        }

        /// <summary>
        /// Пара типов, применяется для организации ключей в коллекции.
        /// </summary>
        internal class TypeTypePair
        {
            /// <summary>
            /// Первый тип в паре.
            /// </summary>
            public Type FromType;

            /// <summary>
            /// Второй тип в паре.
            /// </summary>
            public Type ToType;

            /// <summary>
            /// Пара типов, применяется для организации ключей в коллекции.
            /// </summary>
            /// <param name="fromType">Первый тип в паре.</param>
            /// <param name="toType">Второй тип в паре.</param>
            public TypeTypePair(Type fromType, Type toType)
            {
                FromType = fromType;
                ToType = toType;
            }

            /// <summary>
            /// Serves as a hash function for a type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current object.
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((FromType != null ? FromType.GetHashCode() : 0) * 397) ^ (ToType != null ? ToType.GetHashCode() : 0);
                }
            }
        }

        /// <summary>
        /// Класс для сравнения объектов типа <see cref="TypeTypePair"/>.
        /// </summary>
        internal class TypeTypePairEqualityComparer : EqualityComparer<TypeTypePair>
        {
            /// <summary>
            /// Determines whether two objects of type <see cref="TypeTypePair"/> are equal.
            /// </summary>
            /// <returns>
            /// If the specified objects are equal then <c>true</c>; otherwise, <c>false</c>.
            /// </returns>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            public override bool Equals(TypeTypePair x, TypeTypePair y)
            {
                return x.FromType == y.FromType && x.ToType == y.ToType;
            }

            /// <summary>
            /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
            /// </summary>
            /// <returns>
            /// A hash code for the specified object.
            /// </returns>
            /// <param name="obj">The object for which to get a hash code.</param>
            public override int GetHashCode(TypeTypePair obj)
            {
                unchecked
                {
                    return ((obj.FromType != null ? obj.FromType.GetHashCode() : 0) * 397) ^ (obj.ToType != null ? obj.ToType.GetHashCode() : 0);
                }
            }
        }
    }
}
