namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;
    using Security;
    using STORMDO = STORMNET;
    using STORMFunction = FunctionalLanguage.Function;

    /// <summary>
    /// Набор служебной логики для сервиса данных.
    /// </summary>
    public partial class Utils
    {
        private Utils()
        {
        }

        /// <summary>
        /// проверить - системный ли тип.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInternalBaseType(object value)
        {
            return value.GetType().Assembly == typeof(int).Assembly;
        }

        private static int CountJoins(STORMDO.Business.StorageStructForView.PropSource source, int index)
        {
            int res = 0;

            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    if (subSource.storage[j].parentStorageindex == index)
                    {
                        res += 1 + CountJoins(subSource, j);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Вычислить количество мастеровых ключей в запросе к хранилищу.
        /// </summary>
        /// <param name="storStruct"><see cref="STORMDO.Business.StorageStructForView"/>.</param>
        /// <returns></returns>
        public static int CountMasterKeysInSelect(STORMDO.Business.StorageStructForView storStruct)
        {
            int res = 0;
            for (int i = 0; i < storStruct.props.Length; i++)
            {
                res += storStruct.props[i].Stored ? storStruct.props[i].MastersTypesCount : 0;
            }

            res += CountJoins(storStruct.sources, 0);
            return res;
        }

        /// <summary>
        /// Привести к строке.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertSimpleValueString(object value)
        {
            System.Type valType = value.GetType();
            if (valType == typeof(decimal))
            {
                return ((decimal)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            else if (valType == typeof(Guid))
            {
                return ((Guid)value).ToString("B");
            }
            else
            {
                return value.ToString();
            }
        }

        public static ObjectStringDataView[] ProcessingRowSet2StringedView(
            object[][] resValue,

            System.Type[] dataObjectType,
            int PropCount,
            char separator,
            LoadingCustomizationStruct customizationStruct,
            STORMDO.Business.StorageStructForView[] StorageStruct,
            IDataService dataService,
            System.Collections.SortedList TypesByKeys,
            ref object[] prevState,
            ISecurityManager securityManager)
        {
            return ProcessingRowSet2StringedView(resValue, false, dataObjectType, PropCount, separator, customizationStruct, StorageStruct, dataService, TypesByKeys, ref prevState, new DataObjectCache(), securityManager);
        }

        /// <summary>
        /// Преобразование RowSet к <see cref="ObjectStringDataView"/>.
        /// </summary>
        /// <param name="resValue">сами данные.</param>
        /// <param name="forReadValues"></param>
        /// <param name="dataObjectType">типы объекта данных.</param>
        /// <param name="propCount">количество свойств.</param>
        /// <param name="separator">разделитель.</param>
        /// <param name="customizationStruct">структура настройки выборки.</param>
        /// <param name="storageStruct">структура хранилища.</param>
        /// <param name="dataService">сервис данный, выполнивший запрос.</param>
        /// <param name="typesByKeys"></param>
        /// <param name="prevState">Предыдущее состояние вычитки для порционного чтения.</param>
        /// <param name="dataObjectCache">кэш объектов данных.</param>
        /// <param name="securityManager">Менеджер полномочий.</param>
        /// <returns><see cref="ObjectStringDataView"/>.</returns>
        public static ObjectStringDataView[] ProcessingRowSet2StringedView(
            object[][] resValue,
            bool forReadValues,
            Type[] dataObjectType,
            int propCount,
            char separator,
            LoadingCustomizationStruct customizationStruct,
            StorageStructForView[] storageStruct,
            IDataService dataService,
            SortedList typesByKeys,
            ref object[] prevState,
            DataObjectCache dataObjectCache,
            ISecurityManager securityManager)
        {
            /*access changes*/
            foreach (Type tp in dataObjectType)
            {
                if (!securityManager.AccessObjectCheck(tp, tTypeAccess.Full, false))
                {
                    securityManager.AccessObjectCheck(tp, tTypeAccess.Read, true);
                }
            }

            /*access changes*/

            int resValueLength = resValue.Length;
            if (resValueLength == 0)
            {
                return new ObjectStringDataView[0];
            }

            System.Reflection.Assembly sysAsm = typeof(int).Assembly;
            ObjectStringDataView[] res = new ObjectStringDataView[resValueLength];
            object[] pars = new object[1];

            System.Reflection.MethodInfo[] valueObjects;
            DataObject[] testDos = null;
            bool[] valueObjectInited;

            // Нехранимые свойства.
            SortedList[] notstoredprops = null;
            int[] indexes;
            object[] emptyObjcts;

            AdvansedColumn[] advansedColumns = customizationStruct.AdvansedColumns;
            int advansedColumnsLength = advansedColumns.Length;
            if (prevState != null)
            {
                valueObjects = (System.Reflection.MethodInfo[])prevState[0];
                valueObjectInited = (bool[])prevState[1];
                testDos = (DataObject[])prevState[2];
                notstoredprops = (SortedList[])prevState[3];
                indexes = (int[])prevState[4];
                emptyObjcts = (object[])prevState[5];
            }
            else
            {
                valueObjects = new System.Reflection.MethodInfo[propCount];
                valueObjectInited = new bool[propCount];

                emptyObjcts = null;
                string[] columnsOrder = customizationStruct.ColumnsOrder;
                if (advansedColumnsLength > 0)
                {
                    ArrayList al = new ArrayList();
                    for (int i = 0; i < advansedColumnsLength; i++)
                    {
                        al.Add(advansedColumns[i].Name);
                    }

                    indexes = customizationStruct.View.GetOrderedIndexes(columnsOrder, (string[])al.ToArray(typeof(string)));
                }
                else
                {
                    indexes = customizationStruct.View.GetOrderedIndexes(columnsOrder);
                }

                // System.Collections.ArrayList NotInitiedMethodsIndex = new System.Collections.ArrayList();
                for (int i = 0; i < valueObjectInited.Length; i++)
                {
                    valueObjectInited[i] = true;
                    valueObjects[i] = null;
                }

                for (int i = 0; i < propCount; i++)
                {
                    Type propType = (indexes[i] >= storageStruct[0].props.Length) ? typeof(object) : storageStruct[0].props[indexes[i]].propertyType; // STORMDO.Information.GetPropertyType(customizationStruct.View.DefineClassType,StorageStruct[0].props[indexes[i]].Name);
                    if (propType.IsSubclassOf(typeof(DataObject)))
                    {
                        propType = KeyGen.KeyGenerator.KeyType(propType);
                    }

                    if (propType.Assembly == sysAsm)
                    {
                        valueObjectInited[i] = true;
                    }
                    else if (resValue[0][i] != null && resValue[0][i] != DBNull.Value)
                    {
                        valueObjects[i] = propType.GetMethod("op_Implicit", new[] { resValue[0][i].GetType() });
                        valueObjectInited[i] = true;
                    }
                    else
                    {
                        valueObjectInited[i] = false;
                    }
                }

                int storageStructLength = storageStruct.Length;
                for (int j = 0; j < storageStructLength; j++)
                {
                    StorageStructForView.PropStorage[] propStorages = storageStruct[j].props;
                    int propStoragesLength = propStorages.Length;
                    for (int i = 0; i < propStoragesLength; i++)
                    {
                        StorageStructForView.PropStorage prop = propStorages[i];
                        if (!prop.Stored)
                        {
                            string expression = prop.Expression;
                            if (string.IsNullOrEmpty(expression))
                            {
                                if (notstoredprops == null)
                                {
                                    notstoredprops = new SortedList[dataObjectType.Length];
                                }

                                if (notstoredprops[j] == null)
                                {
                                    notstoredprops[j] = new SortedList();
                                }

                                notstoredprops[j].Add(prop.Name, i);
                            }
                        }
                    }

                    for (int i = 0; i < advansedColumnsLength; i++)
                    {
                        string expression = advansedColumns[i].Expression;
                        if (string.IsNullOrEmpty(expression))
                        {
                            if (notstoredprops == null)
                            {
                                notstoredprops = new SortedList[dataObjectType.Length];
                            }

                            if (notstoredprops[j] == null)
                            {
                                notstoredprops[j] = new SortedList();
                            }

                            notstoredprops[j].Add(advansedColumns[i].Name, i);
                        }
                    }
                }

                if (notstoredprops != null)
                {
                    testDos = new DataObject[dataObjectType.Length];
                    emptyObjcts = new object[0];

                    // произведем упорядоченность
                    StringCollection props = new StringCollection();
                    for (int i = 0; i < advansedColumnsLength; i++)
                    {
                        props.Add(advansedColumns[i].Name);
                    }

                    PropertyInView[] propertiesInView = customizationStruct.View.Properties;
                    int propInViewLength = propertiesInView.Length;
                    for (int i = 0; i < propInViewLength; i++)
                    {
                        props.Add(propertiesInView[i].Name);
                    }

                    if (columnsOrder != null)
                    {
                        int k = 0;
                        int columnsOrderLength = columnsOrder.Length;
                        for (int i = 0; i < columnsOrderLength; i++)
                        {
                            string prop = columnsOrder[i];
                            int index = props.IndexOf(prop);
                            if (index >= 0)
                            {
                                if (index != k)
                                {
                                    props.RemoveAt(index);
                                    props.Insert(k++, prop);
                                }
                                else
                                {
                                    k++;
                                }
                            }
                        }
                    }

                    int notstoredpropsLength = notstoredprops.Length;
                    for (int j = 0; j < notstoredpropsLength; j++)
                    {
                        SortedList nsps = notstoredprops[j];
                        if (nsps != null)
                        {
                            int nspsCount = props.Count;
                            for (int i = 0; i < nspsCount; i++)
                            {
                                if (nsps.ContainsKey(props[i]))
                                {
                                    nsps[props[i]] = i;
                                }
                            }
                        }
                    }
                }

                prevState = new object[] { valueObjects, valueObjectInited, testDos, notstoredprops, indexes, emptyObjcts };
            }

            int objectTypeIndexPOs = resValue[0].Length - 1;

            // #if NETFX_35
            for (int i = 0; i < resValueLength; i++)

            // #else
            //           System.Threading.Tasks.Parallel.For(0, resValueLength, i =>
            //       #endif
            {
                // object objectIndex = resValue[i][ObjectTypeIndexPOs];
                long index = 0;
                object[] keysarr = resValue[i];
                if (!forReadValues)
                {
                    try
                    {
                        index = Convert.ToInt64(keysarr[objectTypeIndexPOs].ToString());
                    }
                    catch
                    {
                        // throw new Exception(resValue[i][ObjectTypeIndexPOs].GetType().Name);
                        index = Convert.ToInt64(keysarr[objectTypeIndexPOs].ToString());
                    }
                }

                // System.Int64 index = 0;
                // if (objectIndex.GetType()==typeof(System.Int32))
                //  index = Convert.ToInt64((System.Int32)objectIndex);
                // else
                //   index= (System.Int64) objectIndex;

                // ObjectStringDataView objectStringDataView = res[i];
                Type dataobjecttype = dataObjectType[index];
                res[i].ObjectType = dataobjecttype;

                StorageStructForView storageStructForView = storageStruct[index];
                if (notstoredprops != null)
                {
                    SortedList nspsAtThisIndex = notstoredprops[index];
                    if (nspsAtThisIndex != null)
                    {
                        // DataObject testDo = testDos[index];
                        if (testDos[index] == null)
                        {
                            testDos[index] = (DataObject)dataobjecttype.GetConstructor(Type.EmptyTypes).Invoke(emptyObjcts);
                        }
                        else
                        {
                            testDos[index].Clear();
                        }

                        /*Utils.*/
                        FillRowSetToDataObject(testDos[index], keysarr, storageStructForView, customizationStruct, typesByKeys, advansedColumns, dataObjectCache, securityManager);
                        int count = nspsAtThisIndex.Count;
                        for (int j = 0; j < count; j++)
                        {
                            object val = Information.GetPropValueByName(testDos[index], (string)nspsAtThisIndex.GetKey(j));
                            if (val == null)
                            {
                                val = string.Empty;
                            }

                            keysarr[(int)nspsAtThisIndex.GetByIndex(j)] = val;
                        }
                    }
                }

                if (!forReadValues)
                {
                    StorageStructForView.PropStorage[] props = storageStructForView.props;
                    int storageStructForViewPropsLength = props.Length;
                    int keyindex = storageStructForViewPropsLength - 1;
                    while (props[keyindex].MultipleProp)
                    {
                        keyindex--;
                    }

                    keyindex = keyindex + 2 + advansedColumnsLength;
                    ArrayList al = new ArrayList();
                    GetMasterObjectStructs(keysarr, ref keyindex, 0, storageStructForView.sources, al, string.Empty);

                    for (int hryu = 0; hryu < storageStructForViewPropsLength; hryu++)
                    {
                        StorageStructForView.PropStorage ps = props[hryu];

                        if (ps.MastersTypesCount > 0)
                        {
                            Type mt = null;
                            object mv = null;

                            // changed by fat
                            // Братчиков 2010-10-29 (может встретиться несколько иерархий на пути, надо их тоже правильно обработать)
                            int колвоМастеровВСередине = ps.MastersTypes.Length; // предполагается, что тут будет число мастеров, получаемое при последовательных пристыковываниях нескольких иерархий наследования
                            int колвоМастеровВКонцеИерархии = ps.MastersTypes[0].Length; // это число типов мастеров с которым мы непосредственно сейчас работаем
                            // int masterTypesLength = колвоМастеровВСередине * колвоМастеровВКонцеИерархии;
                            for (int j = 0; j < колвоМастеровВСередине; j++)
                            {
                                for (int mc = 0; mc < колвоМастеровВКонцеИерархии /*ps.MasterTypesCount*/; mc++)
                                {
                                    if (keysarr[keyindex] != DBNull.Value)
                                    {
                                        // mt = ps.MastersTypes[0][mc];//тут не 0 и mc
                                        mt = ps.MastersTypes[j][mc];
                                        mv = keysarr[keyindex];
                                    }

                                    keyindex++;
                                }
                            }

                            if (mv != null)
                            {
                                MasterObjStruct mos = new MasterObjStruct(Information.TranslateValueToPrimaryKeyType(mt, mv), mt, ps.Name);
                                bool found = false;
                                foreach (MasterObjStruct mos1 in al)
                                {
                                    if (mos1.PropertyName == ps.Name)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    al.Add(mos);
                                }
                            }
                        }
                    }

                    res[i].Masters = (MasterObjStruct[])al.ToArray(typeof(MasterObjStruct));

                    res[i].Key = Information.TranslateValueToPrimaryKeyType(dataobjecttype, keysarr[propCount]);
                }

                // string resd = "";
                object[] resobjects = new object[propCount];
                for (int j = 0; j < propCount; j++)
                {
                    // string valstring ="";
                    // object keysarrAtJ = keysarr[j];
                    if (keysarr[j] != DBNull.Value)
                    {
                        if (!valueObjectInited[j])
                        {
                            Type propType = (indexes[j] > storageStruct[0].props.Length) ? typeof(object) : storageStruct[0].props[indexes[j]].propertyType;
                            if (propType.IsSubclassOf(typeof(DataObject)))
                            {
                                propType = KeyGen.KeyGenerator.KeyType(propType);
                            }

                            if (propType.Assembly != sysAsm)
                            {
                                valueObjects[j] = propType.GetMethod("op_Implicit", new[] { keysarr[j].GetType() });
                            }

                            valueObjectInited[j] = true;
                        }

                        if (valueObjects[j] != null)
                        {
                            pars[0] = keysarr[j];
                            try
                            {
                                keysarr[j] = valueObjects[j].Invoke(null, pars);
                            }
                            catch (Exception ex)
                            {
                                if (LogService.Log.IsWarnEnabled)
                                {
                                    LogService.Log.Warn("STORMNET ProcessingRowset2StringedView " + keysarr[j], ex);
                                }

                                // System.Diagnostics.EventLog.WriteEntry("STORMNET", keysarrAtJ.ToString(), System.Diagnostics.EventLogEntryType.Error);
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        keysarr[j] = null;
                    }

                    if (keysarr[j] is string)
                    {
                        keysarr[j] = ((string)keysarr[j]).TrimEnd();
                    }

                    resobjects[j] = keysarr[j]; // valstring;
                }

                res[i].ObjectedData = resobjects;
                res[i].Separator = separator;
            }

            // #if (NETFX_35)
            // #else
            //    );
            // #endif
            return res;
        }

        /// <summary>
        /// Построить структуру мастеров.
        /// </summary>
        /// <param name="keysarr">массив ключей.</param>
        /// <param name="keyindex">текущий индекс.</param>
        /// <param name="index">индекс вышестоящего источника.</param>
        /// <param name="source">текущий источник.</param>
        /// <param name="res">дин.массив, куда складываем пезультат.</param>
        /// <param name="nameSpace">текущий NameSpace.</param>
        public static void GetMasterObjectStructs(object[] keysarr, ref int keyindex, int index,
            StorageStructForView.PropSource source, ArrayList res, string nameSpace)
        {
            foreach (StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                string masterName = (nameSpace == string.Empty) ? subSource.ObjectLink : nameSpace + "." + subSource.ObjectLink;
                int subSourceStorageLength = subSource.storage.Length;
                for (int j = 0; j < subSourceStorageLength; j++)
                {
                    StorageStructForView.ClassStorageDef classStorageDef = subSource.storage[j];
                    if (classStorageDef.parentStorageindex == index)
                    {
                        object info = keysarr[keyindex];
                        if (info != null && info != DBNull.Value)
                        {
                            MasterObjStruct mos = new MasterObjStruct(info, classStorageDef.ownerType, masterName);
                            keyindex++;
                            res.Add(mos);
                            GetMasterObjectStructs(keysarr, ref keyindex, j, subSource, res, masterName);
                        }
                        else
                        {
                            keyindex++;
                            GetMasterObjectStructs(keysarr, ref keyindex, j, subSource, res, masterName);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="keysarr"></param>
        /// <param name="keyindex"></param>
        /// <param name="index"></param>
        /// <param name="source"></param>
        /// <param name="sourceToDataObjectList"></param>
        /// <param name="TypesByKeys"></param>
        public static void CreateMastersStruct(
            ICSSoft.STORMNET.DataObject dataObject,
            object[] keysarr, ref int keyindex, int index,
            ICSSoft.STORMNET.Business.StorageStructForView.PropSource source,
            System.Collections.SortedList sourceToDataObjectList,
            System.Collections.SortedList TypesByKeys,
            DataObjectCache DataObjectCache)
        {
            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    if (subSource.storage[j].parentStorageindex == index)
                    {
                        if (dataObject == null)
                        {
                            keyindex++;
                            CreateMastersStruct(null, keysarr, ref keyindex, j, subSource, sourceToDataObjectList, TypesByKeys, DataObjectCache);
                        }
                        else if (keysarr[keyindex] != null && keysarr[keyindex] != DBNull.Value)
                        {
                            DataObject masterobj = null;
                            if (!subSource.HierarchicalLink)
                            {
                                DataObject exmasterobj = (DataObject)Information.GetPropValueByName(dataObject, subSource.ObjectLink);
                                if (TypesByKeys == null)
                                {
                                    object masterkey = keysarr[keyindex++];
                                    if (exmasterobj != null && exmasterobj.__PrimaryKey.Equals(masterkey)
                                        && exmasterobj.GetType() == subSource.storage[j].ownerType)
                                    {
                                        DataObjectCache.AddDataObject(exmasterobj);
                                        masterobj = exmasterobj;
                                    }
                                    else
                                    {
                                        masterobj = DataObjectCache.CreateDataObject(
                                            subSource.storage[j].ownerType, masterkey);

                                        // Братчиков: костыль для исправления ситуации когда мастер является ещё и детейлом. При обработке детейла, если мастер был проинициализирован тут, проинициализируем его ещё раз
                                        masterobj.DynamicProperties.Add("MasterInitDataCopy", true);
                                    }
                                }
                                else
                                {
                                    object mastertype = keysarr[keyindex++];
                                    object masterkey = keysarr[keyindex++];

                                    if (exmasterobj != null && exmasterobj.__PrimaryKey.Equals(masterkey)
                                        && exmasterobj.GetType() == (Type)TypesByKeys[mastertype])
                                    {
                                        DataObjectCache.AddDataObject(exmasterobj);
                                        masterobj = exmasterobj;
                                    }
                                    else
                                    {
                                        masterobj = DataObjectCache.CreateDataObject(
                                            (Type)TypesByKeys[mastertype], masterkey);

                                        // Братчиков: костыль для исправления ситуации когда мастер является ещё и детейлом. При обработке детейла, если мастер был проинициализирован тут, проинициализируем его ещё раз
                                        masterobj.DynamicProperties.Add("MasterInitDataCopy", true);
                                    }
                                }

                                if (masterobj.GetStatus(false) == ObjectStatus.Created)
                                {
                                    masterobj.SetLoadingState(LoadingState.LightLoaded);
                                    masterobj.SetStatus(ObjectStatus.UnAltered);
                                    masterobj.InitDataCopy(DataObjectCache);
                                    masterobj.AddLoadedProperties("__PrimaryKey");
                                }

                                Information.SetPropValueByName(dataObject, subSource.ObjectLink, masterobj);
                                sourceToDataObjectList.Add(subSource, new object[] { masterobj, j });
                                dataObject.AddLoadedProperties(subSource.ObjectLink);
                            }
                            else
                            {
                                keyindex = keyindex + 2;
                                masterobj = dataObject;
                                sourceToDataObjectList.Add(subSource, new object[] { masterobj, j });
                            }

                            CreateMastersStruct(masterobj, keysarr, ref keyindex, j, subSource, sourceToDataObjectList, TypesByKeys, DataObjectCache);
                        }
                        else
                        {
                            keyindex++;
                            CreateMastersStruct(null, keysarr, ref keyindex, j, subSource, sourceToDataObjectList, TypesByKeys, DataObjectCache);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Преобразовать рекордсета в объекты.
        /// </summary>
        /// <param name="value">Значения.</param>
        /// <param name="dataObjectType">Типы объектов.</param>
        /// <param name="storageStruct">Структуры выборки для типов.</param>
        /// <param name="customizationStruct">Настройка выборки.</param>
        /// <param name="res">Результат.</param>
        /// <param name="dataService">Сервис данных,которым читали.</param>
        /// <param name="typesByKeys"></param>
        /// <param name="сlearDataObjects">Очищать ли существующие объекты.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="securityManager">Менеджер полномочий.</param>
        /// <param name="connection">Коннекция, через которую желательно выполнять все зачитки объектов.</param>
        /// <param name="transaction">Транзакция, через которую желательно выполнять все зачитки объектов.</param>
        public static void ProcessingRowsetDataRef(
            object[][] value,
            Type[] dataObjectType,
            StorageStructForView[] storageStruct,
            LoadingCustomizationStruct customizationStruct,
            DataObject[] res,
            IDataService dataService,
            SortedList typesByKeys,
            bool сlearDataObjects,
            DataObjectCache dataObjectCache,
            ISecurityManager securityManager,
            IDbConnection connection = null,
            IDbTransaction transaction = null)
        {
            if (value.Length == 0)
            {
                return;
            }

            int ObjectTypeIndexPOs = value[0].Length - 1;
            object[] keys = new object[value.Length];
            int keyIndex = customizationStruct.View.Properties.Length + customizationStruct.AdvansedColumns.Length;
            bool[] readedTypes = new bool[dataObjectType.Length];
            Array.Clear(readedTypes, 0, readedTypes.Length);

            // #if NETFX_35
            for (int i = 0; i < value.Length; i++)

            // #else
            //    System.Threading.Tasks.Parallel.For(0, value.Length, i =>
            // #endif
            {
                long index = Convert.ToInt64(value[i][ObjectTypeIndexPOs].ToString());
                readedTypes[index] = true;
                if (res[i] == null)
                {
                    res[i] = dataObjectCache.CreateDataObject(dataObjectType[index], value[i][keyIndex]);
                }

                FillRowSetToDataObject(res[i], value[i], storageStruct[index], customizationStruct, typesByKeys, customizationStruct.AdvansedColumns, dataObjectCache, securityManager);
                keys[i] = res[i].__PrimaryKey;
            }

            // #if NETFX_35
            // #else
            //    );
            // #endif

            // обработка детейлов
            if (customizationStruct.View.Details != null && customizationStruct.View.Details.Length > 0)
            {
                var details = new SortedList();
                var detnames = new string[customizationStruct.View.Details.Length];
                for (int i = 0; i < customizationStruct.View.Details.Length; i++)
                {
                    DetailInView div = customizationStruct.View.Details[i];
                    detnames[i] = div.Name;
                    details.Add(detnames[i], div);
                }

                detnames = Information.SortByLoadingOrder(customizationStruct.View.DefineClassType, detnames);
                for (int i = 0; i < detnames.Length; i++)
                {
                    // Баг с зачиткой нехранимых детейлов в гроупэдите: проверю на хранимость детейла
                    if (!Information.IsStoredProperty(customizationStruct.View.DefineClassType, detnames[i]))
                    {
                        break; // если не хранимый, то пропустим
                    }

                    var div = (DetailInView)details[detnames[i]];
                    if (div.LoadOnLoadAgregator)
                    {
                        var detTypes = new ArrayList();
                        for (int h = 0; h < dataObjectType.Length; h++)
                        {
                            if (readedTypes[h])
                            {
                                Type[] tu = dataService.TypeUsage.GetUsageTypes(dataObjectType[h], div.Name);
                                for (int n = 0; n < tu.Length; n++)
                                {
                                    if (!detTypes.Contains(tu[n]))
                                    {
                                        detTypes.Add(tu[n]);
                                    }
                                }
                            }
                        }

                        var detailTypes = new Type[detTypes.Count];
                        detTypes.CopyTo(detailTypes);

                        // строим функцию ограничений
                        string agregatorName = Information.GetAgregatePropertyName(detailTypes[0]);

                        STORMFunction func;
                        FunctionalLanguage.SQLWhere.SQLWhereLanguageDef lg = FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
                        var vd =
                            new FunctionalLanguage.VariableDef(
                            lg.GetObjectTypeForNetType(keys[0].GetType()), agregatorName);

                        var alPars = new ArrayList();
                        alPars.Add(vd);
                        var alagrObjects = new ArrayList();
                        string divname = div.Name;
                        int divnameDotIndex = divname.IndexOf(".", StringComparison.Ordinal);
                        if (divnameDotIndex <= 0)
                        {
                            alPars.AddRange(keys);
                            alagrObjects.AddRange(res);
                        }
                        else
                        {
                            string mprop = div.Name.Substring(0, divnameDotIndex);
                            for (int k = 0; k < keys.Length; k++)
                            {
                                var dobj = (DataObject)Information.GetPropValueByName(res[k], mprop);
                                if (dobj != null)
                                {
                                    alPars.Add(dobj.__PrimaryKey);
                                    alagrObjects.Add(dobj);
                                }
                            }
                        }

                        func = lg.GetFunction(lg.funcIN, alPars.ToArray());
                        div.View.AddProperty(agregatorName, string.Empty, false, string.Empty);

                        var arsorts = new System.Collections.ArrayList();
                        ColumnsSortDef[] sorts;
                        string sortprop = ICSSoft.STORMNET.Information.GetOrderPropertyName(detailTypes[0]);
                        if (sortprop != string.Empty)
                        {
                            div.View.AddProperty(sortprop, string.Empty, false, string.Empty);
                            arsorts.AddRange(
                                new ColumnsSortDef[]
                                {
                                    new ColumnsSortDef(agregatorName, SortOrder.Asc),
                                    new ColumnsSortDef(sortprop, SortOrder.Asc),
                                });
                        }
                        else
                        {
                            arsorts.Add(new ColumnsSortDef(agregatorName, SortOrder.Asc));
                        }

                        ColumnsSortDef[] subsorts = customizationStruct.GetColumnsSortDef(div.Name);
                        if (subsorts != null && subsorts.Length > 0)
                        {
                            arsorts.AddRange(subsorts);
                        }

                        sorts = (ColumnsSortDef[])arsorts.ToArray(typeof(ColumnsSortDef));

                        // собираем типы данных для детейла
                        var dcs = new LoadingCustomizationStruct(dataService.GetInstanceId());
                        if (divnameDotIndex >= 0)
                        {
                            divname = divname.Substring(divnameDotIndex + 1);
                        }

                        for (int j = 0; j < alagrObjects.Count; j++)
                        {
                            ((DataObject)alagrObjects[j]).AddLoadedProperties(divname);
                        }

                        bool UseAdaptive = false;
                        DetailArray detArr = null;
                        int index = 0;
                        if (div.UseAdaptiveTypeLoading)
                        {
                            UseAdaptive = true;
                            bool ClearedArray = false;
                            for (int AdaptIndex = 0; AdaptIndex < detailTypes.Length; AdaptIndex++)
                            {
                                Type typeKey = detailTypes[AdaptIndex];
                                View typeView = div.AdaptiveTypeViews.Contains(typeKey) ? (ICSSoft.STORMNET.View)div.AdaptiveTypeViews[typeKey] : Information.GetView(div.View.Name, typeKey);
                                typeView = typeView == null ? div.View : div.View | typeView;

                                dcs.Init(sorts, func, new Type[] { typeKey }, typeView, null);
                                dcs.InitDataCopy = false;
                                DataObject[] detailObjects = LoadObjects(dcs, dataObjectCache, dataService, connection, transaction);

                                object curKey = null;
                                detArr = null;

                                for (int j = 0; j < detailObjects.Length; j++)
                                {
                                    object curObjectKey = ((STORMDO.DataObject)STORMDO.Information.GetPropValueByName(detailObjects[j], agregatorName)).__PrimaryKey;
                                    if (curKey != curObjectKey)
                                    {
                                        curKey = curObjectKey;
                                        index = 0;
                                        for (int ii = 0; ii < alagrObjects.Count; ii++)
                                        {
                                            if (((DataObject)alagrObjects[ii]).__PrimaryKey.Equals(curKey))
                                            {
                                                index = ii;
                                                break;
                                            }
                                        }

                                        if (detArr != null)
                                        {
                                            DataObject dobj = detArr.AgregatorObject;
                                            dobj.AddLoadedProperties(div.Name);
                                            if (customizationStruct.InitDataCopy)
                                            {
                                                dobj.InitDataCopy(dataObjectCache);
                                            }
                                        }

                                        detArr = (DetailArray)Information.GetPropValueByName((DataObject)alagrObjects[index], div.Name);
                                        if (detArr == null)
                                        {
                                            Type detArrType = Information.GetPropertyType(customizationStruct.View.DefineClassType, div.Name);
                                            detArr = (DetailArray)detArrType.GetConstructor(new Type[] { alagrObjects[index].GetType() }).Invoke(new object[] { (DataObject)alagrObjects[index] });
                                            Information.SetPropValueByName((DataObject)alagrObjects[index], divname, detArr);
                                        }

                                        if (!сlearDataObjects)
                                        {
                                            var al = new ArrayList();
                                            foreach (DataObject do1 in detArr.GetAllObjects())
                                            {
                                                bool founded = false;
                                                foreach (object loadedkey in keys)
                                                {
                                                    if (do1.__PrimaryKey.Equals(loadedkey))
                                                    {
                                                        founded = true;
                                                        break;
                                                    }
                                                }

                                                if (!founded)
                                                {
                                                    al.Add(do1);
                                                }
                                            }

                                            DataObject[] detailObjects1 = (DataObject[])al.ToArray(typeof(DataObject));

                                            // Братчиков 2009-07-08 - Баг при зачитке адаптивных детейлов. Представление не соответствует зачитываемым объектам. Антиглюк сделан по первому объекту в массиве (косяк выйдет если представление было всё-таки совместимым). Возможно всегда можно применять div.View.
                                            {
                                                View view4Load = dcs.View;
                                                if (detailObjects1.Length > 0 && !(detailObjects1[0].GetType().Equals(view4Load.DefineClassType) || detailObjects1[0].GetType().IsSubclassOf(view4Load.DefineClassType)))
                                                {
                                                    view4Load = div.View;
                                                }

                                                // TODO: разобраться, может ли это приводить к проблемам, если будет выполняться вне транзакции.
                                                dataService.LoadObjects(detailObjects1, view4Load, сlearDataObjects, dataObjectCache);
                                            }

                                            // Братчиков 2009-07-08
                                        }
                                        else
                                        {
                                            if (!ClearedArray)
                                            {
                                                ClearedArray = true;
                                                detArr.Clear();
                                            }
                                        }
                                    }

                                    if (!сlearDataObjects)
                                    {
                                        DataObject[] detailObjects1 = detArr.GetAllObjects();
                                        DataObject p = null;
                                        foreach (DataObject d in detailObjects1)
                                        {
                                            if (d.__PrimaryKey.ToString() == detailObjects[j].__PrimaryKey.ToString()
                                                || (d.Prototyped && d.__PrototypeKey.ToString() == detailObjects[j].__PrimaryKey.ToString()))
                                            {
                                                p = d;
                                            }
                                        }

                                        if (p != null)
                                        {
                                            detailObjects[j] = p;
                                        }
                                    }

                                    if (detArr.GetByKey(detailObjects[j].__PrimaryKey) != null)
                                    {
                                        detArr.RemoveByKey(detailObjects[j].__PrimaryKey);
                                    }

                                    detArr.AddObject(detailObjects[j]);
                                }
                            }
                        }
                        else
                        {
                            dcs.Init(sorts, func, detailTypes, div.View, null);
                            dcs.InitDataCopy = false;
                            DataObject[] detailObjects = LoadObjects(dcs, dataObjectCache, dataService, connection, transaction);

                            int allinserted = 0;

                            object curKey = null;
                            detArr = null;
                            for (int j = 0; j < detailObjects.Length; j++)
                            {
                                object curObjectKey = ((STORMDO.DataObject)STORMDO.Information.GetPropValueByName(detailObjects[j], agregatorName)).__PrimaryKey;
                                if ((curKey == null && curKey != curObjectKey) || (curKey != null && !curKey.Equals(curObjectKey)))
                                {
                                    curKey = curObjectKey;
                                    index = 0;
                                    for (; index < alagrObjects.Count; index++)
                                    {
                                        if (((DataObject)alagrObjects[index]).__PrimaryKey.Equals(curKey))
                                        {
                                            break;
                                        }
                                    }

                                    if (detArr != null)
                                    {
                                        DataObject dobj = detArr.AgregatorObject;
                                        dobj.AddLoadedProperties(divname);
                                    }

                                    detArr = (DetailArray)Information.GetPropValueByName((DataObject)alagrObjects[index], divname);
                                    if (detArr == null)
                                    {
                                        Type detArrType = Information.GetPropertyType(customizationStruct.View.DefineClassType, div.Name);
                                        detArr = (DetailArray)detArrType.GetConstructor(new Type[] { alagrObjects[index].GetType() }).Invoke(new object[] { ((DataObject)alagrObjects[index]) });
                                        Information.SetPropValueByName((DataObject)alagrObjects[index], divname, detArr);
                                    }

                                    if (!сlearDataObjects)
                                    {
                                        var al = new ArrayList();
                                        foreach (DataObject do1 in detArr.GetAllObjects())
                                        {
                                            bool founded = keys.Contains(do1.__PrimaryKey);
                                            if (!founded)
                                            {
                                                al.Add(do1);
                                            }
                                        }

                                        var detailObjects1 = (DataObject[])al.ToArray(typeof(DataObject));

                                        // TODO: разобраться, может ли это приводить к проблемам, если будет выполняться вне транзакции.
                                        dataService.LoadObjects(detailObjects1, dcs.View, сlearDataObjects, dataObjectCache);
                                    }
                                    else
                                    {
                                        detArr.Clear();
                                    }
                                }

                                if (!сlearDataObjects)
                                {
                                    DataObject[] detailObjects1 = detArr.GetAllObjects();
                                    DataObject p = null;
                                    foreach (DataObject d in detailObjects1)
                                    {
                                        if (d.__PrimaryKey.ToString() == detailObjects[j].__PrimaryKey.ToString()
                                            || (d.Prototyped && d.__PrototypeKey.ToString() == detailObjects[j].__PrimaryKey.ToString()))
                                        {
                                            p = d;
                                            break;
                                        }
                                    }

                                    if (p != null)
                                    {
                                        detailObjects[j] = p;
                                    }
                                }

                                if (detArr.GetByKey(detailObjects[j].__PrimaryKey) != null)
                                {
                                    detArr.RemoveByKey(detailObjects[j].__PrimaryKey);
                                }

                                detArr.AddObject(detailObjects[j]);
                                allinserted++;
                            }

                            index++;
                        }

                        for (index = 0; index < alagrObjects.Count; index++)
                        {
                            detArr = (DetailArray)Information.GetPropValueByName((DataObject)alagrObjects[index], divname);
                            if (detArr == null)
                            {
                                Type detArrType = Information.GetPropertyType(customizationStruct.View.DefineClassType, div.Name);
                                detArr = (DetailArray)detArrType.GetConstructor(new Type[] { alagrObjects[index].GetType() }).Invoke(new object[] { ((DataObject)alagrObjects[index]) });
                                Information.SetPropValueByName((DataObject)alagrObjects[index], divname, detArr);
                            }

                            if (UseAdaptive && sortprop != string.Empty)
                            {
                                detArr.Ordering();
                            }

                            DataObject dobj = detArr.AgregatorObject;
                            dobj.AddLoadedProperties(divname);
                        }
                    }
                }
            }

            if (customizationStruct.InitDataCopy)
            {
                // #if NETFX_35
                foreach (DataObject dobj in res)

                // #else
                //    System.Threading.Tasks.Parallel.ForEach(res, dobj =>
                // #endif
                {
                    dobj.SetStatus(ObjectStatus.UnAltered);
                    dobj.InitDataCopy(dataObjectCache);
                }

                // #if NETFX_35
                // #else
                //    );
                // #endif
            }
        }

        #region Вспомогательные методы для подгрузки данных (используется для определения того, как правильно грузить, с транзакцией или без).

        /// <summary>
        /// Загрузка объектов с возможностью указания транзакции, в рамках которой необходимо выполнять загрузку.
        /// </summary>
        /// <param name="dcs">Структура, определяющая, какие объекта и как загружать.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="dataService">Сервис данных, через который нужно выполнять загрузку данных.</param>
        /// <param name="connection">Коннекция, в рамках которой нужно выполнять загрузку данных.</param>
        /// <param name="transaction">Транзакция, в рамках которой нужно выполнять загрузку данных.</param>
        /// <returns>Загруженные объекты.</returns>
        private static DataObject[] LoadObjects(
            LoadingCustomizationStruct dcs,
            DataObjectCache dataObjectCache,
            IDataService dataService,
            IDbConnection connection,
            IDbTransaction transaction)
        {
            DataObject[] detailObjects;

            if (transaction == null || connection == null || !(dataService is SQLDataService))
            {
                detailObjects = dataService.LoadObjects(dcs, dataObjectCache);
            }
            else
            {
                var sqlDataService = (SQLDataService)dataService;
                object state = null;
                detailObjects = sqlDataService.LoadObjectsByExtConn(dcs, ref state, dataObjectCache, connection, transaction);
            }

            return detailObjects;
        }

        #endregion Вспомогательные методы для подгрузки данных (используется для определения того, как правильно грузить, с транзакцией или без).

        /// <summary>
        /// Преобразовать рекордсета в объекты.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataObjectType"></param>
        /// <param name="StorageStruct"></param>
        /// <param name="customizationStruct"></param>
        /// <param name="dataService"></param>
        /// <param name="TypesByKeys"></param>
        /// <param name="DataObjectCache">Кэш объектов данных.</param>
        /// <param name="securityManager">Менеджер полномочий.</param>
        /// <param name="connection">Коннекция, через которую желательно выполнять все зачитки объектов.</param>
        /// <param name="transaction">Транзакция, через которую желательно выполнять все зачитки объектов.</param>
        /// <returns></returns>
        public static ICSSoft.STORMNET.DataObject[] ProcessingRowsetData(
            object[][] value,
            System.Type[] dataObjectType,
            STORMDO.Business.StorageStructForView[] StorageStruct,
            LoadingCustomizationStruct customizationStruct,
            IDataService dataService,
            System.Collections.SortedList TypesByKeys,
            DataObjectCache DataObjectCache,
            ISecurityManager securityManager,
            IDbConnection connection = null,
            IDbTransaction transaction = null)
        {
            var res = new DataObject[value.Length];
            ProcessingRowsetDataRef(
                value, dataObjectType, StorageStruct, customizationStruct, res, dataService, TypesByKeys, true, DataObjectCache, securityManager, connection, transaction);
            return res;
        }

        /// <summary>
        /// Получить детейловые объекты.
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="dobject"></param>
        /// <param name="readedDetailObjects"></param>
        /// <param name="unreadedDetailsViewsForDetailPath"></param>
        public static void getDetailsObjects(IDataService dataService, STORMDO.DataObject dobject, out STORMDO.DataObject[] readedDetailObjects, out STORMDO.DataObject[] mastersObjects, out STORMDO.View[] unreadedDetailsViewsForDetailPath)
        {
            System.Type dotype = dobject.GetType();
            string[] props = STORMDO.Information.GetStorablePropertyNames(dotype);
            props = STORMDO.Information.SortByLoadingOrder(dotype, props);

            System.Type darrtype = typeof(STORMDO.DetailArray);
            System.Collections.ArrayList resarr = new System.Collections.ArrayList();
            System.Collections.ArrayList resview = new System.Collections.ArrayList();
            System.Collections.ArrayList resmasters = new System.Collections.ArrayList();

            StringCollection loadedprops = new StringCollection();
            loadedprops.AddRange(dobject.GetLoadedProperties());
            foreach (string prop in props)
            {
                System.Type prtype = STORMDO.Information.GetPropertyType(dotype, prop);
                if (prtype.IsSubclassOf(typeof(DataObject)))
                {
                    DataObject val = (STORMDO.DataObject)STORMDO.Information.GetPropValueByName(dobject, prop);
                    if (val != null && val.GetStatus(false) == ObjectStatus.Deleted)
                    {
                        resmasters.Add(val);
                    }
                }

                if (prtype.IsSubclassOf(darrtype))
                {
                    if (loadedprops.Contains(prop))
                    {
                        foreach (STORMDO.DataObject arrobj in (STORMDO.DetailArray)STORMDO.Information.GetPropValueByName(dobject, prop))
                        {
                            resarr.Add(arrobj);
                            STORMDO.DataObject[] subreadedDetailObjects;
                            STORMDO.DataObject[] smastersObjs;
                            STORMDO.View[] subunreadedDetailsViewsForDetailPath;
                            getDetailsObjects(dataService, arrobj, out subreadedDetailObjects, out smastersObjs, out subunreadedDetailsViewsForDetailPath);
                            resarr.AddRange(subreadedDetailObjects);
                            resview.AddRange(subunreadedDetailsViewsForDetailPath);
                            resmasters.AddRange(smastersObjs);
                        }
                    }
                    else
                    {
                        System.Type[] detPartType = dataService.TypeUsage.GetUsageTypes(dotype, prop);
                        foreach (System.Type curdettype in detPartType)
                        {
                            string agrname = Information.GetAgregatePropertyName(curdettype);

                            STORMDO.View curDetView = new ICSSoft.STORMNET.View();
                            curDetView.DefineClassType = curdettype;

                            // добавляем первичный ключ
                            curDetView.AddProperty("__PrimaryKey", string.Empty, false, string.Empty);

                            // добавляем ссылку на мастера
                            curDetView.AddProperty(agrname, string.Empty, false, string.Empty);

                            curDetView.MasterTypeFilters.Add(agrname, dotype);
                            resview.Add(curDetView);
                        }

                        foreach (System.Type curdettype in detPartType)
                        {
                            string agrname = Information.GetAgregatePropertyName(curdettype);
                            Collections.NameObjectCollection masterTypes = new ICSSoft.STORMNET.Collections.NameObjectCollection();
                            masterTypes.Add(agrname, dotype);
                            resview.AddRange(getUnreadedDetailsArrayReadView(dataService, curdettype, agrname, masterTypes));
                        }
                    }
                }
            }

            if (resarr.Count == 0)
            {
                readedDetailObjects = new STORMDO.DataObject[0];
            }
            else
            {
                readedDetailObjects = new ICSSoft.STORMNET.DataObject[resarr.Count];
                resarr.CopyTo(readedDetailObjects);
            }

            mastersObjects = (ICSSoft.STORMNET.DataObject[])resmasters.ToArray(typeof(STORMDO.DataObject));
            if (resview.Count == 0)
            {
                unreadedDetailsViewsForDetailPath = new STORMDO.View[0];
            }
            else
            {
                unreadedDetailsViewsForDetailPath = new STORMDO.View[resview.Count];
                resview.CopyTo(unreadedDetailsViewsForDetailPath);
            }
        }

        /// <summary>
        /// Обновить внутренние данные объекта.
        /// </summary>
        /// <param name="dobj">объект.</param>
        /// <param name="UpLevel">верхнего ли уровня.</param>
        public static void UpdateInternalDataInObjects(STORMDO.DataObject dobj, bool UpLevel, DataObjectCache DataObjectCache)
        {
            STORMDO.ObjectStatus objestat = dobj.GetStatus(false);
            System.Type dobjtype = dobj.GetType();
            System.Type detarrtype = typeof(STORMDO.DetailArray);
            switch (objestat)
            {
                case STORMDO.ObjectStatus.Altered:
                case STORMDO.ObjectStatus.Created:
                    {
                        string[] alteredProperties = dobj.GetAlteredPropertyNames(false);
                        foreach (string prop in alteredProperties)
                        {
                            if (STORMDO.Information.GetPropertyType(dobjtype, prop).IsSubclassOf(detarrtype))
                            {
                                STORMDO.DetailArray da = (STORMDO.DetailArray)Information.GetPropValueByName(dobj, prop);
                                for (int i = da.Count - 1; i >= 0; i--)
                                {
                                    STORMDO.DataObject detobj = da.ItemByIndex(i);
                                    STORMDO.ObjectStatus os = detobj.GetStatus(false);
                                    switch (os)
                                    {
                                        case STORMDO.ObjectStatus.Altered:
                                            UpdateInternalDataInObjects(detobj, false, DataObjectCache);
                                            break;
                                        case STORMDO.ObjectStatus.Created:
                                            UpdateInternalDataInObjects(detobj, false, DataObjectCache);
                                            break;
                                        case STORMDO.ObjectStatus.Deleted:
                                            da.RemoveByIndex(i);
                                            break;
                                    }
                                }
                            }
                        }

                        dobj.AddLoadedProperties(alteredProperties);
                        if (dobj.GetLoadedProperties().Length == dobj.GetAlteredPropertyNames(false).Length)
                        {
                            dobj.SetLoadingState(STORMDO.LoadingState.Loaded);
                        }
                        else
                        {
                            dobj.SetLoadingState(STORMDO.LoadingState.LightLoaded);
                        }

                        dobj.SetStatus(STORMDO.ObjectStatus.UnAltered);
                        if (UpLevel)
                        {
                            dobj.InitDataCopy(DataObjectCache);
                        }
                        else
                        {
                            dobj.clearDataCopy();
                        }

                        break;
                    }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="dotype"></param>
        /// <param name="path"></param>
        /// <param name="MasterTypes"></param>
        /// <returns></returns>
        public static STORMDO.View[] getUnreadedDetailsArrayReadView(IDataService dataService, System.Type dotype, string path, Collections.NameObjectCollection MasterTypes)
        {
            System.Collections.ArrayList res = new System.Collections.ArrayList();
            string[] storedprops = Information.GetStorablePropertyNames(dotype);
            System.Type detarr = typeof(STORMDO.DetailArray);
            string Upmaster = (path == string.Empty) ? string.Empty : "." + path;
            foreach (string prop in storedprops)
            {
                System.Type propType = Information.GetPropertyType(dotype, prop);
                if (propType.IsSubclassOf(detarr))
                {
                    System.Type[] detPartType = dataService.TypeUsage.GetUsageTypes(dotype, prop);
                    foreach (System.Type curdettype in detPartType)
                    {
                        string agrname = Information.GetAgregatePropertyName(curdettype);
                        STORMDO.View curDetView = new ICSSoft.STORMNET.View();
                        curDetView.DefineClassType = curdettype;

                        // добавляем первичный ключ
                        curDetView.AddProperty("__PrimaryKey", string.Empty, false, string.Empty);

                        // добавляем ссылку на мастера
                        curDetView.AddProperty(agrname + Upmaster, string.Empty, false, string.Empty);

                        Collections.NameObjectCollection noc = curDetView.MasterTypeFilters;
                        for (int i = 0; i < MasterTypes.Count; i++)
                        {
                            noc.Add(MasterTypes.Keys[i], MasterTypes.Get(i));
                        }

                        noc.Add(agrname, dotype);

                        res.Add(curDetView);
                    }

                    foreach (System.Type curdettype in detPartType)
                    {
                        string agrname = Information.GetAgregatePropertyName(curdettype);
                        MasterTypes.Add(agrname + Upmaster, dotype);
                        res.AddRange(getUnreadedDetailsArrayReadView(dataService, curdettype, agrname + Upmaster, MasterTypes));
                        MasterTypes.Remove(agrname + Upmaster);
                    }
                }
            }

            STORMDO.View[] unreadedDetailsViewsForDetailPath = new STORMDO.View[res.Count];
            res.CopyTo(unreadedDetailsViewsForDetailPath);
            return unreadedDetailsViewsForDetailPath;
        }

        /// <summary>
        /// Сравнение двух массивов.
        /// </summary>
        /// <typeparam name="T">Тип элементов массива.</typeparam>
        /// <param name="a1">Массив 1.</param>
        /// <param name="a2">Массив 2.</param>
        /// <returns>Равны или нет.</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
            {
                return true;
            }

            if (a1 == null || a2 == null)
            {
                return false;
            }

            if (a1.Length != a2.Length)
            {
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
