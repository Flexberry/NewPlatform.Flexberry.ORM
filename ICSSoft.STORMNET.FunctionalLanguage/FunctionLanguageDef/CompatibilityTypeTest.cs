using System;
using System.Collections.Generic;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// Совместимость типов
    /// </summary>
    public enum TypesCompatibilities
    {

        ///<summary>
        /// Не совместимы
        ///</summary>
        No,
        /// <summary>
        /// Конвертируемы
        /// </summary>
        Convertable,
        /// <summary>
        /// Равны
        /// </summary>
        Equal
    }

    /// <summary>
    /// Класс для проверки совместимости типов
    /// </summary>
    public class CompatibilityTypeTest
    {


        private CompatibilityTypeTest()
        { }

        static private System.Collections.Specialized.StringCollection _checkedTypes;
        static private System.Collections.SortedList _canConvertTo;
        static private System.Collections.Specialized.StringCollection _knownTypes;
        static private Dictionary<long, TypesCompatibilities> _cacheCheck = new Dictionary<long, TypesCompatibilities>();
        private static string _lockConst = "CONST";


        static private void ThisIsKnownType(Type type)
        {
            string typeName = type.FullName;

            if (_knownTypes.Contains(typeName)) return;
            lock (_lockConst)
            {
                if (!_knownTypes.Contains(typeName))
                    _knownTypes.Add(typeName);
            }
        }

        static private void AddPredifinedConvertion(Type systemtype, Type[] from, Type[] to)
        {
            if (!_checkedTypes.Contains(systemtype.FullName))
            {
                lock (_lockConst)
                {
                    if (!_checkedTypes.Contains(systemtype.FullName))
                        _checkedTypes.Add(systemtype.FullName);
                }

            }

            ThisIsKnownType(systemtype);

            System.Collections.Specialized.StringCollection sl;
            if (!_canConvertTo.ContainsKey(systemtype.FullName))
            {
                lock (_lockConst)
                {
                    if (!_canConvertTo.ContainsKey(systemtype.FullName))
                    {
                        _canConvertTo.Add(systemtype.FullName, new System.Collections.Specialized.StringCollection());
                    }
                }
            }

            sl = (System.Collections.Specialized.StringCollection)_canConvertTo[systemtype.FullName];

            for (int i = 0; i < to.Length; i++)
            {
                ThisIsKnownType(to[i]);
                if (!sl.Contains(to[i].FullName))
                {
                    lock (_lockConst)
                    {
                        if (!sl.Contains(to[i].FullName))
                            sl.Add(to[i].FullName);
                    }
                }
            }
            for (int i = 0; i < from.Length; i++)
            {
                ThisIsKnownType(from[i]);
                if (!_canConvertTo.ContainsKey(from[i].FullName))
                {
                    lock (_lockConst)
                    {
                        if (!_canConvertTo.ContainsKey(from[i].FullName))
                            _canConvertTo.Add(from[i].FullName, new System.Collections.Specialized.StringCollection());
                    }
                }
                sl = (System.Collections.Specialized.StringCollection)_canConvertTo[from[i].FullName];

                if (!sl.Contains(systemtype.FullName))
                {
                    lock (_lockConst)
                    {
                        if (!sl.Contains(systemtype.FullName))
                            sl.Add(systemtype.FullName);
                    }
                }
            }
        }

        static private void AddType(Type tp)
        {
            lock (_lockConst)
            {
                _checkedTypes.Add(tp.FullName);
            }
            ThisIsKnownType(tp);

            System.Reflection.MethodInfo[] mis = tp.GetMethods();
            foreach (System.Reflection.MethodInfo mi in mis)
            {
                if (mi.IsSpecialName && (mi.Name == "op_Implicit" || mi.Name == "op_Explicit"))
                {
                    Type f = mi.GetParameters()[0].ParameterType;
                    Type t = mi.ReturnType;
                    System.Collections.Specialized.StringCollection sl;
                    if (!_canConvertTo.ContainsKey(f.FullName))
                    {
                        lock (_lockConst)
                        {
                            if (!_canConvertTo.ContainsKey(f.FullName))
                            {
                                _canConvertTo.Add(f.FullName, new System.Collections.Specialized.StringCollection());
                            }
                        }
                    }

                    sl = (System.Collections.Specialized.StringCollection)_canConvertTo[f.FullName];
                    if (!sl.Contains(t.FullName))
                        lock (_lockConst)
                        {
                            if (!sl.Contains(t.FullName))
                                sl.Add(t.FullName);
                        }
                }
            }
        }
        /// <summary>
        /// TODO: надо убедиться, что этот стек может быть разделён между потоками
        /// </summary>
        private static System.Collections.Specialized.StringCollection _stack;
        /// <summary>
        /// Найти преобразование
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        static private bool FoundTransform(string from, string to)
        {
            bool emptyStack;
            lock (_lockConst)
            {
                emptyStack = _stack == null;
                if (emptyStack)
                    _stack = new System.Collections.Specialized.StringCollection();
            }
            bool res = false;

            if (_canConvertTo.ContainsKey(from))
            {
                System.Collections.Specialized.StringCollection sl = (System.Collections.Specialized.StringCollection)_canConvertTo[from];
                if (sl.Contains(to))
                    res = true;
                else
                {
                    for (int i = 0; i < sl.Count; i++)
                    {
                        string k = sl[i];
                        if (!_stack.Contains(k))
                        {
                            lock (_lockConst)
                            {
                                if (!_stack.Contains(k))
                                {
                                    _stack.Add(k);
                                    res = FoundTransform(k, to);
                                    _stack.RemoveAt(_stack.Count - 1);
                                    if (res) break;
                                }
                            }
                        }
                    }
                }
            }
            //else
            //    res = false;
            lock (_lockConst)
            {
                if (emptyStack)
                    _stack = null;
            }
            return res;
        }

        /// <summary>
        /// Проверка на совместимость типов
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        static public TypesCompatibilities Check(Type from, Type to)
        {
            TypesCompatibilities retValue;
            long key = (((long)from.GetHashCode()) << 32) + to.GetHashCode();

            if (_cacheCheck.TryGetValue(key, out retValue))
            {
                return retValue;
            }

            if (from == to)
            {
                retValue = TypesCompatibilities.Equal;
                lock (_cacheCheck)
                {
                    if (!_cacheCheck.ContainsKey(key))
                    {
                        _cacheCheck.Add(key, retValue);
                    }
                }
                return retValue;
            }
            if (from.IsSubclassOf(to))
            {
                retValue = TypesCompatibilities.Convertable;
                lock (_cacheCheck)
                {
                    if (!_cacheCheck.ContainsKey(key))
                    {
                        _cacheCheck.Add(key, retValue);
                    }
                }
                return retValue;
            }
            if (from == typeof(object) || to == typeof(object))
            {
                retValue = TypesCompatibilities.Convertable;
                lock (_cacheCheck)
                {
                    if (!_cacheCheck.ContainsKey(key))
                    {
                        _cacheCheck.Add(key, retValue);
                    }
                }
                return retValue;
            }
            //стандартный Convert

            //				System.Reflection.ConstructorInfo ci =from.GetConstructor(new System.Type[0]); 
            //				try
            //				{
            //					object obj = ci.Invoke(new object[0]);
            //					if (obj!=null)
            //					{
            //						obj =  Convert.ChangeType(obj,to);
            //						return TypesCompatibilities.Convertable;
            //					}
            //					else if (!to.IsValueType)
            //						return TypesCompatibilities.Convertable;
            //				}
            //				catch
            //				{}
            //implicit преобразования
            if (_checkedTypes == null)
            {
                lock (_lockConst)
                {
                    if (_checkedTypes == null)
                    {
                        _checkedTypes = new System.Collections.Specialized.StringCollection();
                        _canConvertTo = new System.Collections.SortedList();
                        _knownTypes = new System.Collections.Specialized.StringCollection();
                        //predefined implicit conversion 
                        //bool
                        AddPredifinedConvertion(
                            typeof (bool),
                            new Type[] {},
                            new[] {typeof (int)});
                        //typeof(byte)
                        AddPredifinedConvertion(
                            typeof (byte),
                            new Type[] {},
                            new[]
                                {
                                    typeof (short), typeof (ushort), typeof (int), typeof (uint), typeof (long),
                                    typeof (ulong)
                                    , typeof (float), typeof (double), typeof (decimal)
                                });
                        //typeof(sbyte)
                        AddPredifinedConvertion(
                            typeof (sbyte),
                            new Type[] {},
                            new[]
                                {
                                    typeof (short), typeof (int), typeof (long), typeof (float), typeof (double),
                                    typeof (decimal)
                                });
                        //typeof(char)
                        AddPredifinedConvertion(
                            typeof (char),
                            new Type[] {},
                            new[]
                                {
                                    typeof (ushort), typeof (int), typeof (uint), typeof (long), typeof (ulong),
                                    typeof (float)
                                    , typeof (double), typeof (decimal)
                                });
                        //typeof(int)
                        AddPredifinedConvertion(
                            typeof (int),
                            new[] {typeof (sbyte), typeof (byte), typeof (short), typeof (ushort), typeof (char)},
                            new[] {typeof (long), typeof (float), typeof (double), typeof (decimal)});
                        //typeof(uint)
                        AddPredifinedConvertion(
                            typeof (uint),
                            new[] {typeof (byte), typeof (ushort), typeof (char)},
                            new[] {typeof (long), typeof (ulong), typeof (float), typeof (double), typeof (decimal)});
                        //typeof(long)
                        AddPredifinedConvertion(
                            typeof (long),
                            new[]
                                {
                                    typeof (sbyte), typeof (byte), typeof (short), typeof (ushort), typeof (int),
                                    typeof (uint)
                                    , typeof (char)
                                },
                            new[] {typeof (float), typeof (double), typeof (decimal)});
                        //typeof(ulong)
                        AddPredifinedConvertion(
                            typeof (ulong),
                            new[] {typeof (byte), typeof (ushort), typeof (uint), typeof (char)},
                            new[] {typeof (float), typeof (double), typeof (decimal)});
                        //typeof(short)
                        AddPredifinedConvertion(
                            typeof (short),
                            new Type[] {},
                            new[] {typeof (int), typeof (long), typeof (float), typeof (double), typeof (decimal)});
                        //typeof(ushort)
                        AddPredifinedConvertion(
                            typeof (ushort),
                            new[] {typeof (byte), typeof (char)},
                            new[]
                                {
                                    typeof (int), typeof (uint), typeof (long), typeof (ulong), typeof (float),
                                    typeof (double)
                                    , typeof (decimal)
                                });

                        //typeof(string)
                        AddPredifinedConvertion(
                            typeof (string),
                            new[]
                                {
                                    typeof (int), typeof (uint), typeof (long), typeof (ulong), typeof (float),
                                    typeof (double), typeof (decimal)
                                },
                            new Type[] {});

                        //typeof(Guid)
                        AddPredifinedConvertion(
                            typeof (Guid),
                            new Type[] {},
                            new[] {typeof (string)});
                    }
                }
            }

            if (!_checkedTypes.Contains(from.FullName))
                AddType(from);
            if (!_checkedTypes.Contains(to.FullName))
                AddType(to);

            if (_canConvertTo.ContainsKey(from.FullName))
            {
                System.Collections.Specialized.StringCollection sl = (System.Collections.Specialized.StringCollection)_canConvertTo[from.FullName];
                if (sl.Contains(to.FullName))
                {
                    retValue = TypesCompatibilities.Convertable;
                    lock (_cacheCheck)
                    {
                        if (!_cacheCheck.ContainsKey(key))
                        {
                            _cacheCheck.Add(key, retValue);
                        }
                    }
                    return retValue;
                }
                if (_knownTypes.Contains(from.FullName) && _knownTypes.Contains(to.FullName) &&
                    FoundTransform(from.FullName, to.FullName))
                {
                    retValue = TypesCompatibilities.Convertable;
                    lock (_cacheCheck)
                    {
                        if (!_cacheCheck.ContainsKey(key))
                        {
                            _cacheCheck.Add(key, retValue);
                        }
                    }
                    return retValue;
                }
                retValue = TypesCompatibilities.No;
                lock (_cacheCheck)
                {
                    if (!_cacheCheck.ContainsKey(key))
                    {
                        _cacheCheck.Add(key, retValue);
                    }
                }
                return retValue;
            }
            if (_knownTypes.Contains(from.FullName) && _knownTypes.Contains(to.FullName) &&
                FoundTransform(from.FullName, to.FullName))
            {
                retValue = TypesCompatibilities.Convertable;
                lock (_cacheCheck)
                {
                    if (!_cacheCheck.ContainsKey(key))
                    {
                        _cacheCheck.Add(key, retValue);
                    }
                }
                return retValue;
            }
            retValue = TypesCompatibilities.No;
            lock (_cacheCheck)
            {
                if (!_cacheCheck.ContainsKey(key))
                {
                    _cacheCheck.Add(key, retValue);
                }
            }
            return retValue;
        }
    }
    /*
    public class CompatibilityTypeTest1
    {
        public CompatibilityTypeTest1()
        { }

        private System.Collections.Specialized.StringCollection CheckedTypes;
        private System.Collections.SortedList CanConvertTo;
        private System.Collections.Specialized.StringCollection KnownTypes;

        private void ThisIsKnownType(System.Type type)
        {
            string TypeName = type.FullName;
            if (!KnownTypes.Contains(TypeName))
                KnownTypes.Add(TypeName);
        }

        private void AddPredifinedConvertion(System.Type systemtype, System.Type[] from, System.Type[] to)
        {
            if (!CheckedTypes.Contains(systemtype.FullName))
                CheckedTypes.Add(systemtype.FullName);
            ThisIsKnownType(systemtype);

            System.Collections.Specialized.StringCollection sl;
            if (CanConvertTo.ContainsKey(systemtype.FullName))
                sl = (System.Collections.Specialized.StringCollection)CanConvertTo[systemtype.FullName];
            else
            {
                sl = new System.Collections.Specialized.StringCollection();
                CanConvertTo.Add(systemtype.FullName, sl);
            }
            for (int i = 0; i < to.Length; i++)
            {
                ThisIsKnownType(to[i]);
                if (!sl.Contains(to[i].FullName))
                    sl.Add(to[i].FullName);
            }
            for (int i = 0; i < from.Length; i++)
            {
                ThisIsKnownType(from[i]);
                if (CanConvertTo.ContainsKey(from[i].FullName))
                    sl = (System.Collections.Specialized.StringCollection)CanConvertTo[from[i].FullName];
                else
                {
                    sl = new System.Collections.Specialized.StringCollection();
                    CanConvertTo.Add(from[i].FullName, sl);
                }
                if (!sl.Contains(systemtype.FullName))
                    sl.Add(systemtype.FullName);
            }
        }

        private void AddType(System.Type tp)
        {
            CheckedTypes.Add(tp.FullName);
            ThisIsKnownType(tp);

            System.Reflection.MethodInfo[] mis = tp.GetMethods();
            foreach (System.Reflection.MethodInfo mi in mis)
            {
                if (mi.IsSpecialName && (mi.Name == "op_Implicit" || mi.Name == "op_Explicit"))
                {
                    System.Type f = mi.GetParameters()[0].ParameterType;
                    System.Type t = mi.ReturnType;
                    System.Collections.Specialized.StringCollection sl;
                    if (CanConvertTo.ContainsKey(f.FullName))
                        sl = (System.Collections.Specialized.StringCollection)CanConvertTo[f.FullName];
                    else
                    {
                        sl = new System.Collections.Specialized.StringCollection();
                        CanConvertTo.Add(f.FullName, sl);
                    }
                    if (!sl.Contains(t.FullName))
                        sl.Add(t.FullName);
                }
            };
        }
        System.Collections.Specialized.StringCollection stack;
        private bool FoundTransform(string from, string to)
        {
            bool EmptyStack = stack == null;
            if (EmptyStack)
                stack = new System.Collections.Specialized.StringCollection();
            bool res = false;

            if (CanConvertTo.ContainsKey(from))
            {
                System.Collections.Specialized.StringCollection sl = (System.Collections.Specialized.StringCollection)CanConvertTo[from];
                if (sl.Contains(to))
                    res = true;
                else
                {
                    for (int i = 0; i < sl.Count; i++)
                    {
                        string k = sl[i];
                        if (!stack.Contains(k))
                        {
                            stack.Add(k);
                            res = FoundTransform(k, to);
                            stack.RemoveAt(stack.Count - 1);
                            if (res) break;
                        }
                    }
                }
            }
            else
                res = false;

            if (EmptyStack)
                stack = null;
            return res;
        }

        public TypesCompatibilities Check(System.Type from, System.Type to)
        {
            if (from == to)
                return TypesCompatibilities.Equal;
            if (from.IsSubclassOf(to))
                return TypesCompatibilities.Convertable;
            else if (from == typeof(object) || to == typeof(object))
            {
                return TypesCompatibilities.Convertable;
            }
            else
            {
                //стандартный Convert

                //				System.Reflection.ConstructorInfo ci =from.GetConstructor(new System.Type[0]); 
                //				try
                //				{
                //					object obj = ci.Invoke(new object[0]);
                //					if (obj!=null)
                //					{
                //						obj =  Convert.ChangeType(obj,to);
                //						return TypesCompatibilities.Convertable;
                //					}
                //					else if (!to.IsValueType)
                //						return TypesCompatibilities.Convertable;
                //				}
                //				catch
                //				{}
                //implicit преобразования
                if (CheckedTypes == null)
                {
                    CheckedTypes = new System.Collections.Specialized.StringCollection();
                    CanConvertTo = new System.Collections.SortedList();
                    KnownTypes = new System.Collections.Specialized.StringCollection();
                    //predefined implicit conversion 
                    //bool
                    AddPredifinedConvertion(
                        typeof(bool),
                        new Type[] { },
                        new Type[] { typeof(int) });
                    //typeof(byte)
                    AddPredifinedConvertion(
                        typeof(byte),
                        new Type[] { },
                        new Type[] { typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) });
                    //typeof(sbyte)
                    AddPredifinedConvertion(
                        typeof(sbyte),
                        new Type[] { },
                        new Type[] { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) });
                    //typeof(char)
                    AddPredifinedConvertion(
                        typeof(char),
                        new Type[] { },
                        new Type[] { typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) });
                    //typeof(int)
                    AddPredifinedConvertion(
                        typeof(int),
                        new Type[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(char) },
                        new Type[] { typeof(long), typeof(float), typeof(double), typeof(decimal) });
                    //typeof(uint)
                    AddPredifinedConvertion(
                        typeof(uint),
                        new Type[] { typeof(byte), typeof(ushort), typeof(char) },
                        new Type[] { typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) });
                    //typeof(long)
                    AddPredifinedConvertion(
                        typeof(long),
                        new Type[] { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(char) },
                        new Type[] { typeof(float), typeof(double), typeof(decimal) });
                    //typeof(ulong)
                    AddPredifinedConvertion(
                        typeof(ulong),
                        new Type[] { typeof(byte), typeof(ushort), typeof(uint), typeof(char) },
                        new Type[] { typeof(float), typeof(double), typeof(decimal) });
                    //typeof(short)
                    AddPredifinedConvertion(
                        typeof(short),
                        new Type[] { },
                        new Type[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) });
                    //typeof(ushort)
                    AddPredifinedConvertion(
                        typeof(ushort),
                        new Type[] { typeof(byte), typeof(char) },
                        new Type[] { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) });

                    //typeof(string)
                    AddPredifinedConvertion(
                        typeof(string),
                        new Type[] { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) },
                        new Type[] { });

                    //typeof(Guid)
                    AddPredifinedConvertion(
                        typeof(Guid),
                        new Type[] { },
                        new Type[] { typeof(string) });

                }
                if (!CheckedTypes.Contains(from.FullName))
                    AddType(from);
                if (!CheckedTypes.Contains(to.FullName))
                    AddType(to);
                if (CanConvertTo.ContainsKey(from.FullName))
                {
                    System.Collections.Specialized.StringCollection sl = (System.Collections.Specialized.StringCollection)CanConvertTo[from.FullName];
                    if (sl.Contains(to.FullName))
                        return TypesCompatibilities.Convertable;
                    else
                    {
                        if (KnownTypes.Contains(from.FullName) && KnownTypes.Contains(to.FullName) &&
                            FoundTransform(from.FullName, to.FullName))
                        {
                            return TypesCompatibilities.Convertable;
                        }
                        else
                            return TypesCompatibilities.No;
                    }
                }
                else
                {
                    if (KnownTypes.Contains(from.FullName) && KnownTypes.Contains(to.FullName) &&
                        FoundTransform(from.FullName, to.FullName))
                    {
                        return TypesCompatibilities.Convertable;
                    }
                    else
                        return TypesCompatibilities.No;
                }

            }
        }
     
    }* */
}
