﻿namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Совместимость типов.
    /// </summary>
    public enum TypesCompatibilities
    {
        /// <summary>
        /// Не совместимы
        /// </summary>
        No,

        /// <summary>
        /// Конвертируемы
        /// </summary>
        Convertable,

        /// <summary>
        /// Равны
        /// </summary>
        Equal,
    }

    /// <summary>
    /// Класс для проверки совместимости типов.
    /// </summary>
    public static class CompatibilityTypeTest
    {
        private static readonly Dictionary<long, TypesCompatibilities> CacheCheck = new Dictionary<long, TypesCompatibilities>();

        private static readonly object LockConst = new object();

        private static List<string> _checkedTypes;

        private static Dictionary<string, List<string>> _canConvertTo;

        private static List<string> _knownTypes;

        private static bool _initialized;

        private static void ThisIsKnownType(Type type)
        {
            string typeName = type.FullName;

            if (!_knownTypes.Contains(typeName))
            {
                _knownTypes.Add(typeName);
            }
        }

        private static void AddPredifinedConvertion(Type systemtype, Type[] from, Type[] to)
        {
            if (!_checkedTypes.Contains(systemtype.FullName))
            {
                _checkedTypes.Add(systemtype.FullName);
            }

            ThisIsKnownType(systemtype);

            List<string> sl = GetConvertToList(systemtype);

            for (int i = 0; i < to.Length; i++)
            {
                ThisIsKnownType(to[i]);
                if (!sl.Contains(to[i].FullName))
                {
                    sl.Add(to[i].FullName);
                }
            }

            for (int i = 0; i < from.Length; i++)
            {
                ThisIsKnownType(from[i]);

                sl = GetConvertToList(from[i]);

                if (!sl.Contains(systemtype.FullName))
                {
                    sl.Add(systemtype.FullName);
                }
            }
        }

        private static void AddType(Type tp)
        {
            _checkedTypes.Add(tp.FullName);

            ThisIsKnownType(tp);

            System.Reflection.MethodInfo[] mis = tp.GetMethods();
            foreach (System.Reflection.MethodInfo mi in mis)
            {
                if (mi.IsSpecialName && (mi.Name == "op_Implicit" || mi.Name == "op_Explicit"))
                {
                    Type f = mi.GetParameters()[0].ParameterType;
                    Type t = mi.ReturnType;
                    List<string> sl = GetConvertToList(f);
                    if (!sl.Contains(t.FullName))
                    {
                        sl.Add(t.FullName);
                    }
                }
            }
        }

        private static List<string> GetConvertToList(Type from)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            return GetConvertToList(from.FullName);
        }

        private static List<string> GetConvertToList(string from)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (!_canConvertTo.ContainsKey(from))
            {
                _canConvertTo.Add(from, new List<string>());
            }

            return _canConvertTo[from];
        }

        /// <summary>
        /// Найти преобразование.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static bool FoundTransform(string from, string to, List<string> stack = null)
        {
            stack ??= new List<string>();

            var toList = GetConvertToList(from);
            if (!toList.Any())
            {
                return false;
            }

            bool res = false;
            if (toList.Contains(to))
            {
                res = true;
            }
            else
            {
                foreach (string k in toList)
                {
                    if (!stack.Contains(k))
                    {
                        stack.Add(k);
                        res = FoundTransform(k, to, stack);
                        stack.RemoveAt(stack.Count - 1);
                        if (res)
                        {
                            break;
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Проверка на совместимость типов.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static TypesCompatibilities Check(Type from, Type to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            long key = (((long)from.GetHashCode()) << 32) + to.GetHashCode();

            lock (LockConst)
            {
                if (CacheCheck.ContainsKey(key))
                {
                    return CacheCheck[key];
                }

                var res = CheckInternal(from, to);
                CacheCheck[key] = res;

                return res;
            }
        }

        internal static TypesCompatibilities CheckInternal(Type from, Type to)
        {
            if (from == to)
            {
                return TypesCompatibilities.Equal;
            }

            if (from.IsSubclassOf(to))
            {
                return TypesCompatibilities.Convertable;
            }

            if (from == typeof(object) || to == typeof(object))
            {
                return TypesCompatibilities.Convertable;
            }

            // стандартный Convert

            // System.Reflection.ConstructorInfo ci =from.GetConstructor(new System.Type[0]);
            //              try
            //              {
            //                  object obj = ci.Invoke(new object[0]);
            //                  if (obj!=null)
            //                  {
            //                      obj =  Convert.ChangeType(obj,to);
            //                      return TypesCompatibilities.Convertable;
            //                  }
            //                  else if (!to.IsValueType)
            //                      return TypesCompatibilities.Convertable;
            //              }
            //              catch
            //              {}

            // implicit преобразования
            Init();

            if (!_checkedTypes.Contains(from.FullName))
            {
                AddType(from);
            }

            if (!_checkedTypes.Contains(to.FullName))
            {
                AddType(to);
            }

            var toList = GetConvertToList(from);
            if (toList.Any())
            {
                if (toList.Contains(to.FullName))
                {
                    return TypesCompatibilities.Convertable;
                }

                if (_knownTypes.Contains(from.FullName) && _knownTypes.Contains(to.FullName) &&
                    FoundTransform(from.FullName, to.FullName))
                {
                    return TypesCompatibilities.Convertable;
                }

                return TypesCompatibilities.No;
            }

            if (_knownTypes.Contains(from.FullName) && _knownTypes.Contains(to.FullName) &&
                FoundTransform(from.FullName, to.FullName))
            {
                return TypesCompatibilities.Convertable;
            }

            return TypesCompatibilities.No;
        }

        internal static void Init()
        {
            if (_initialized)
            {
                return;
            }

            lock (LockConst)
            {
                if (_initialized)
                {
                    return;
                }

                _checkedTypes = new List<string>();
                _canConvertTo = new Dictionary<string, List<string>>();
                _knownTypes = new List<string>();

                // predefined implicit conversion
                // bool
                AddPredifinedConvertion(
                    typeof(bool),
                    new Type[] { },
                    new[] { typeof(int) });

                // typeof(byte)
                AddPredifinedConvertion(
                    typeof(byte),
                    new Type[] { },
                    new[]
                    {
                        typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
                        typeof(ulong),
                        typeof(float), typeof(double), typeof(decimal),
                    });

                // typeof(sbyte)
                AddPredifinedConvertion(
                    typeof(sbyte),
                    new Type[] { },
                    new[]
                    {
                        typeof(short), typeof(int), typeof(long), typeof(float), typeof(double),
                        typeof(decimal),
                    });

                // typeof(char)
                AddPredifinedConvertion(
                    typeof(char),
                    new Type[] { },
                    new[]
                    {
                        typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong),
                        typeof(float),
                        typeof(double), typeof(decimal),
                    });

                // typeof(int)
                AddPredifinedConvertion(
                    typeof(int),
                    new[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(char) },
                    new[] { typeof(long), typeof(float), typeof(double), typeof(decimal) });

                // typeof(uint)
                AddPredifinedConvertion(
                    typeof(uint),
                    new[] { typeof(byte), typeof(ushort), typeof(char) },
                    new[] { typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) });

                // typeof(long)
                AddPredifinedConvertion(
                    typeof(long),
                    new[]
                    {
                        typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int),
                        typeof(uint),
                        typeof(char),
                    },
                    new[] { typeof(float), typeof(double), typeof(decimal) });

                // typeof(ulong)
                AddPredifinedConvertion(
                    typeof(ulong),
                    new[] { typeof(byte), typeof(ushort), typeof(uint), typeof(char) },
                    new[] { typeof(float), typeof(double), typeof(decimal) });

                // typeof(short)
                AddPredifinedConvertion(
                    typeof(short),
                    new Type[] { },
                    new[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) });

                // typeof(ushort)
                AddPredifinedConvertion(
                    typeof(ushort),
                    new[] { typeof(byte), typeof(char) },
                    new[]
                    {
                        typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float),
                        typeof(double),
                        typeof(decimal),
                    });

                // typeof(string)
                AddPredifinedConvertion(
                    typeof(string),
                    new[]
                    {
                        typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float),
                        typeof(double), typeof(decimal),
                    },
                    new Type[] { });

                // typeof(Guid)
                AddPredifinedConvertion(
                    typeof(Guid),
                    new Type[] { },
                    new[] { typeof(string) });

                _initialized = true;
            }
        }
    }
}
