namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// Summary description for TypeUsageProvider.
    /// </summary>
    [Serializable]
    public class TypeUsage
    {
        private static string m_objNull = "CONST";

        /// <summary>
        ///
        /// </summary>
        public TypeUsage()
        {
        }

        private System.Collections.SortedList typeusagecollection = new System.Collections.SortedList();
        private ICSSoft.STORMNET.Collections.TypeBaseCollection usetypeascollection = new ICSSoft.STORMNET.Collections.TypeBaseCollection();

        /// <summary>
        /// вернуть используемые типы по шаблону.
        /// </summary>
        /// <param name="dataObjectTypeTemplate"></param>
        /// <returns></returns>
        public Type[] GetUseTypesAs(Type dataObjectTypeTemplate)
        {
            if (!usetypeascollection.Contains(dataObjectTypeTemplate))
            {
                AddUseTypesAs(dataObjectTypeTemplate, null);
            }

            return ((ICSSoft.STORMNET.Collections.TypesArrayList)usetypeascollection[dataObjectTypeTemplate]).ToArray();
        }

        /// <summary>
        /// добавить типы по шаблону.
        /// </summary>
        /// <param name="dataObjectTypeTmplate"></param>
        /// <param name="usageTypes"></param>
        public void AddUseTypesAs(Type dataObjectTypeTmplate, params Type[] usageTypes)
        {
            lock (m_objNull)
            {
                ICSSoft.STORMNET.Collections.TypesArrayList types;
                if (usetypeascollection.Contains(dataObjectTypeTmplate))
                {
                    types = (ICSSoft.STORMNET.Collections.TypesArrayList)usetypeascollection[dataObjectTypeTmplate];
                    types.Clear();
                }
                else if (usageTypes == null)
                {
                    types = new ICSSoft.STORMNET.Collections.TypesArrayList();
                    types.Add(dataObjectTypeTmplate);
                    usetypeascollection.Add(dataObjectTypeTmplate, types);
                    return;
                }
                else
                {
                    types = new ICSSoft.STORMNET.Collections.TypesArrayList();
                    typeusagecollection.Add(dataObjectTypeTmplate, types);
                }

                foreach (Type t in usageTypes)
                {
                    if (!types.Contains(t))
                    {
                        types.Add(t);
                    }
                }
            }
        }

        /// <summary>
        /// вернуть UsageTypes для заданного типа-свойства.
        /// </summary>
        /// <param name="DataObjectType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Type[] GetUsageTypes(Type DataObjectType, string propertyName)
        {
            string key = GetTypeUsageKey(DataObjectType, propertyName);
            lock (m_objNull)
            {
                if (!typeusagecollection.ContainsKey(key))
                {
                    System.Type propType = Information.GetPropertyType(DataObjectType, propertyName);
                    if (propType.IsSubclassOf(typeof(DetailArray)))
                    {
                        System.Type[] tps = Information.GetCompatibleTypesForDetailProperty(DataObjectType, propertyName);
                        ICSSoft.STORMNET.Collections.TypesArrayList types = new ICSSoft.STORMNET.Collections.TypesArrayList();
                        foreach (Type t in tps)
                        {
                            if (!types.Contains(t))
                            {
                                types.Add(t);
                            }
                        }

                        typeusagecollection.Add(key, types);
                    }
                    else
                    {
                        System.Type[] tps = Information.GetCompatibleTypesForProperty(DataObjectType, propertyName);
                        ICSSoft.STORMNET.Collections.TypesArrayList types = new ICSSoft.STORMNET.Collections.TypesArrayList();
                        foreach (Type t in tps)
                        {
                            if (!types.Contains(t))
                            {
                                types.Add(t);
                            }
                        }

                        typeusagecollection.Add(key, types);
                    }
                }
            }

            try
            {
                return ((ICSSoft.STORMNET.Collections.TypesArrayList)typeusagecollection[key]).ToArray();
            }
            catch
            {
                return Type.EmptyTypes;
            }
        }

        /// <summary>
        /// добавить UsageTypes для заданного типа-свойства.
        /// </summary>
        /// <param name="DataObjectType"></param>
        /// <param name="propertyName"></param>
        /// <param name="addtypes"></param>
        public void AddUsageTypes(Type DataObjectType, string propertyName, params Type[] addtypes)
        {
            string key = GetTypeUsageKey(DataObjectType, propertyName);
            if (!typeusagecollection.ContainsKey(key))
            {
                GetUsageTypes(DataObjectType, propertyName);
            }

            lock (m_objNull)
            {
                ICSSoft.STORMNET.Collections.TypesArrayList types = (ICSSoft.STORMNET.Collections.TypesArrayList)typeusagecollection[key];
                foreach (Type t in addtypes)
                {
                    if (!types.Contains(t))
                    {
                        types.Add(t);
                    }
                }
            }
        }

        /// <summary>
        /// установить UsageTypes для заданного типа-свойства.
        /// </summary>
        /// <param name="DataObjectType"></param>
        /// <param name="propertyName"></param>
        /// <param name="addtypes"></param>
        public void SetUsageTypes(Type DataObjectType, string propertyName, params Type[] addtypes)
        {
            string key = GetTypeUsageKey(DataObjectType, propertyName);
            if (!typeusagecollection.ContainsKey(key))
            {
                GetUsageTypes(DataObjectType, propertyName);
            }

            lock (m_objNull)
            {
                ICSSoft.STORMNET.Collections.TypesArrayList types = (ICSSoft.STORMNET.Collections.TypesArrayList)typeusagecollection[key];
                types.Clear();
                foreach (Type t in addtypes)
                {
                    if (!types.Contains(t))
                    {
                        types.Add(t);
                    }
                }
            }
        }

        private static string GetTypeUsageKey(Type DataObjectType, string propertyName)
        {
            return DataObjectType.AssemblyQualifiedName + "(" + propertyName + ")";
        }

        /// <summary>
        /// вернуть с учетом пути и UsageType на каждом участке.
        /// </summary>
        /// <param name="DataObjectType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Type[] GetCombinedTypeUsage(Type DataObjectType, string propertyName)
        {
            int PointIndex = propertyName.IndexOf(".");
            if (PointIndex < 0)
            {
                return GetUsageTypes(DataObjectType, propertyName);
            }
            else
            {
                string ownProp = propertyName.Substring(0, PointIndex);
                string newPropName = propertyName.Substring(PointIndex + 1);
                Type[] types = GetUsageTypes(DataObjectType, ownProp);
                Array[] arrays = new Array[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    arrays[i] = GetCombinedTypeUsage(types[i], newPropName);
                }

                if (types.Length == 1)
                {
                    return (Type[])arrays[0];
                }
                else
                {
                    return (Type[])ICSSoft.STORMNET.Collections.ArrayOperations.ConcatArrays(typeof(System.Type), arrays);
                }
            }
        }
    }

    /// <summary>
    /// класс для хранения статических TypeUsage.
    /// </summary>
    public class TypeUsageProvider
    {
        private TypeUsageProvider()
        {
        }

        private static TypeUsage fieldTypeUsages;

        /// <summary>
        ///
        /// </summary>
        public static TypeUsage TypeUsage
        {
            get
            {
                if (fieldTypeUsages == null)
                {
                    fieldTypeUsages = new TypeUsage();
                }

                return fieldTypeUsages;
            }
        }
    }
}
