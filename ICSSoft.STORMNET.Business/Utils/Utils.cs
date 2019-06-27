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
        /// проверить - системный ли тип
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInternalBaseType(object value)
        {
            return value.GetType().Assembly == typeof(int).Assembly;
        }

        public static int CountJoins(STORMDO.Business.StorageStructForView.PropSource source, int index)
        {
            int res = 0;

            foreach (STORMDO.Business.StorageStructForView.PropSource subSource in source.LinckedStorages)
            {
                for (int j = 0; j < subSource.storage.Length; j++)
                {
                    if (subSource.storage[j].parentStorageindex == index)
                        res += 1 + CountJoins(subSource, j);
                }
            }
            return res;

        }

        /// <summary>
        /// Вычислить количество мастеровых ключей в запросе к хранилищу
        /// </summary>
        /// <param name="storStruct"><see cref="STORMDO.Business.StorageStructForView"/></param>
        /// <returns></returns>
        public static int CountMasterKeysInSelect(STORMDO.Business.StorageStructForView storStruct)
        {
            int res = 0;
            for (int i = 0; i < storStruct.props.Length; i++)
                res += storStruct.props[i].Stored ? storStruct.props[i].MastersTypesCount : 0;
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
            if (valType == typeof(Decimal))
            {
                return ((decimal)value).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            else if (valType == typeof(Guid))
                return ((Guid)value).ToString("B");
            else
                return value.ToString();
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
            ISecurityManager securityManager
            )
        {
            return ProcessingRowSet2StringedView(resValue, false, dataObjectType, PropCount, separator, customizationStruct, StorageStruct, dataService, TypesByKeys, ref prevState, new DataObjectCache(), securityManager);
        }

        /// <summary>
        /// Преобразование RowSet к <see cref="ObjectStringDataView"/>
        /// </summary>
        /// <param name="resValue">сами данные</param>
        /// <param name="forReadValues"></param>
        /// <param name="dataObjectType">типы объекта данных</param>
        /// <param name="propCount">количество свойств</param>
        /// <param name="separator">разделитель</param>
        /// <param name="customizationStruct">структура настройки выборки</param>
        /// <param name="storageStruct">структура хранилища</param>
        /// <param name="dataService">сервис данный, выполнивший запрос</param>
        /// <param name="typesByKeys"></param>
        /// <param name="prevState">Предыдущее состояние вычитки для порционного чтения</param>
        /// <param name="dataObjectCache">кэш объектов данных</param>
        /// <param name="securityManager">Менеджер полномочий.</param>
        /// <returns><see cref="ObjectStringDataView"/></returns>
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
                return new ObjectStringDataView[0];
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

            AdvancedColumn[] advancedColumns = customizationStruct.AdvancedColumns;
            int advancedColumnsLength = advancedColumns.Length;
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
                if (advancedColumnsLength > 0)
                {
                    indexes = customizationStruct.View.GetOrderedIndexes(columnsOrder,advancedColumns.Select(x=>x.Name).ToArray(),true);
                }
                else
                    indexes = customizationStruct.View.GetOrderedIndexes(columnsOrder,new string[0],true);
                //System.Collections.ArrayList NotInitiedMethodsIndex = new System.Collections.ArrayList();
                for (int i = 0; i < valueObjectInited.Length; i++)
                {
                    valueObjectInited[i] = false;
                    valueObjects[i] = null;
                }

                for (int i = 0; i < Math.Min(propCount,storageStruct[0].props.Length); i++)
                {
                    Type propType = storageStruct[0].props[i].propertyType;//  STORMDO.Information.GetPropertyType(customizationStruct.View.DefineClassType,StorageStruct[0].props[indexes[i]].Name);
                    if (propType.IsSubclassOf(typeof(DataObject)))
                        propType = KeyGen.KeyGenerator.KeyType(propType);
                    if (propType.Assembly == sysAsm)
                        valueObjectInited[i] = true;
                    else if (resValue[0][indexes[i]] != null && resValue[0][indexes[i]] != DBNull.Value)
                    {
                        valueObjects[i] = propType.GetMethod("op_Implicit", new[] { resValue[0][indexes[i]].GetType() });
                        valueObjectInited[i] = true;
                    }
                    else
                        valueObjectInited[i] = false;

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
                                    notstoredprops = new SortedList[dataObjectType.Length];
                                if (notstoredprops[j] == null)
                                    notstoredprops[j] = new SortedList();
                                notstoredprops[j].Add(prop.Name, i);
                            }
                        }
                    }
                    for (int i = 0; i < advancedColumnsLength; i++)
                    {
                        string expression = advancedColumns[i].Expression;
                        if (string.IsNullOrEmpty(expression))
                        {
                            if (notstoredprops == null)
                                notstoredprops = new SortedList[dataObjectType.Length];
                            if (notstoredprops[j] == null)
                                notstoredprops[j] = new SortedList();
                            notstoredprops[j].Add(advancedColumns[i].Name, i);
                        }
                    }
                }

                if (notstoredprops != null)
                {
                    testDos = new DataObject[dataObjectType.Length];
                    emptyObjcts = new Object[0];
                    //произведем упорядоченность

                    StringCollection props = new StringCollection();
                    for (int i = 0; i < advancedColumnsLength; i++)
                        props.Add(advancedColumns[i].Name);
                    PropertyInView[] propertiesInView = customizationStruct.View.Properties;
                    int propInViewLength = propertiesInView.Length;
                    for (int i = 0; i < propInViewLength; i++)
                        props.Add(propertiesInView[i].Name);

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
                                    k++;
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
                                if (nsps.ContainsKey(props[i]))
                                    nsps[props[i]] = i;
                        }
                    }
                }

                prevState = new object[] { valueObjects, valueObjectInited, testDos, notstoredprops, indexes, emptyObjcts };
                
            }

            int objectTypeIndexPOs = resValue[0].Length - 1;


 //       #if NETFX_35 
            for (int i = 0; i < resValueLength; i++) 
 //       #else
 //           System.Threading.Tasks.Parallel.For(0, resValueLength, i =>
 //       #endif
            {
                //object objectIndex = resValue[i][ObjectTypeIndexPOs];
                Int64 index = 0;
                object[] keysarr = resValue[i];
                if (!forReadValues)
                {

                    try
                    {
                        index = Convert.ToInt64(keysarr[objectTypeIndexPOs].ToString());
                    }
                    catch
                    {
                        //throw new Exception(resValue[i][ObjectTypeIndexPOs].GetType().Name);
                        index = Convert.ToInt64(keysarr[objectTypeIndexPOs].ToString());
                    }
                }
                //System.Int64 index = 0;
                //if (objectIndex.GetType()==typeof(System.Int32))			
                //	index = Convert.ToInt64((System.Int32)objectIndex);
                //else
                //   index= (System.Int64) objectIndex;

                //ObjectStringDataView objectStringDataView = res[i];
                Type dataobjecttype = dataObjectType[index];
                res[i].ObjectType = dataobjecttype;

                StorageStructForView storageStructForView = storageStruct[index];
                if (notstoredprops != null)
                {
                    SortedList nspsAtThisIndex = notstoredprops[index];
                    if (nspsAtThisIndex != null)
                    {
                        //DataObject testDo = testDos[index];
                        if (testDos[index] == null)
                            testDos[index] = (DataObject)dataobjecttype.GetConstructor(Type.EmptyTypes).Invoke(emptyObjcts);
                        else
                            testDos[index].Clear();
                        /*Utils.*/
                         FillRowSetToDataObject(testDos[index], keysarr, storageStructForView, customizationStruct, typesByKeys, advancedColumns, dataObjectCache, securityManager,true);
                        int count = nspsAtThisIndex.Count;
                        for (int j = 0; j < count; j++)
                        {
                            object val = Information.GetPropValueByName(testDos[index], (string)nspsAtThisIndex.GetKey(j));
                            if (val == null) val = string.Empty;
                            keysarr[(int)nspsAtThisIndex.GetByIndex(j)] = val;
                        }
                    }
                }


                if (!forReadValues)
                {
                    StorageStructForView.PropStorage[] props = storageStructForView.props;
                    int storageStructForViewPropsLength = props.Length;
                    int keyindex = storageStructForViewPropsLength - 1;
                    while (props[keyindex].MultipleProp || props[keyindex].AdditionalProp)
                        keyindex--;
                    keyindex = keyindex + 2 + advancedColumnsLength;
                    ArrayList al = new ArrayList();
                    GetMasterObjectStructs(keysarr, ref keyindex, 0, storageStructForView.sources, al, string.Empty);

                    for (int hryu = 0; hryu < storageStructForViewPropsLength; hryu++)
                    {
                        StorageStructForView.PropStorage ps = props[hryu];

                        if (ps.MastersTypesCount > 0)
                        {
                            Type mt = null;
                            object mv = null;
                            //changed by fat
                            //Братчиков 2010-10-29 (может встретиться несколько иерархий на пути, надо их тоже правильно обработать)
                            int колвоМастеровВСередине = ps.MastersTypes.Length; //предполагается, что тут будет число мастеров, получаемое при последовательных пристыковываниях нескольких иерархий наследования
                            int колвоМастеровВКонцеИерархии = ps.MastersTypes[0].Length;//это число типов мастеров с которым мы непосредственно сейчас работаем
                            //int masterTypesLength = колвоМастеровВСередине * колвоМастеровВКонцеИерархии;
                            for (int j = 0; j < колвоМастеровВСередине; j++)
                            {
                                for (int mc = 0; mc < колвоМастеровВКонцеИерархии /*ps.MasterTypesCount*/; mc++)
                                {
                                    if (keysarr[keyindex] != DBNull.Value)
                                    {
                                        //mt = ps.MastersTypes[0][mc];//тут не 0 и mc
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
                                    if (mos1.PropertyName == ps.Name) { found = true; break; }
                                }
                                if (!found)
                                    al.Add(mos);
                            }
                        }
                    }
                    res[i].Masters = (MasterObjStruct[])al.ToArray(typeof(MasterObjStruct));

                    res[i].Key = Information.TranslateValueToPrimaryKeyType(dataobjecttype, keysarr[propCount]);
                }
                //string resd = "";
                object[] resobjects = new object[propCount];
                for (int j = 0; j < propCount; j++)
                {
                    //string valstring ="";
                    //object keysarrAtJ = keysarr[j];
                    int nJ = indexes[j];
                    
                    if (keysarr[nJ] != DBNull.Value)
                    {
                        if (j < storageStruct[0].props.Length)
                        {
                            if (!valueObjectInited[j])
                            {
                                Type propType = (nJ > storageStruct[0].props.Length) ? typeof(object) : storageStruct[0].props[j].propertyType;
                                if (propType.IsSubclassOf(typeof(DataObject)))
                                    propType = KeyGen.KeyGenerator.KeyType(propType);
                                if (propType.Assembly != sysAsm)
                                    valueObjects[j] = propType.GetMethod("op_Implicit", new[] { keysarr[nJ].GetType() });
                                valueObjectInited[j] = true;
                            }
                            if (valueObjects[j] != null)
                            {
                                pars[0] = keysarr[nJ];
                                try
                                {
                                    keysarr[nJ] = valueObjects[j].Invoke(null, pars);
                                }
                                catch (Exception ex)
                                {
                                    //if (LogService.Log.IsWarnEnabled)
                                    //    LogService.Log.Warn("STORMNET ProcessingRowset2StringedView " + keysarr[nJ], ex);
                                    //System.Diagnostics.EventLog.WriteEntry("STORMNET", keysarrAtJ.ToString(), System.Diagnostics.EventLogEntryType.Error);
                                    throw ex;
                                }

                            }
                        }
                    }
                    else
                        keysarr[nJ] = null;
                    if (keysarr[nJ] is string)
                        keysarr[nJ] = ((string)keysarr[nJ]).TrimEnd();
                    resobjects[nJ] = keysarr[nJ];//valstring;
                }
                res[i].ObjectedData = resobjects;
                res[i].Separator = separator;
            }
            //#if (NETFX_35)
            //#else
            //    );
            //#endif
            return res;
        }

        /// <summary>
        /// Построить структуру мастеров
        /// </summary>
        /// <param name="keysarr">массив ключей</param>
        /// <param name="keyindex">текущий индекс</param>
        /// <param name="index">индекс вышестоящего источника</param>
        /// <param name="source">текущий источник</param>
        /// <param name="res">дин.массив, куда складываем пезультат</param>
        /// <param name="nameSpace">текущий NameSpace</param>
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
            System.Collections.SortedList TypesByKeys
            , DataObjectCache DataObjectCache)
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
                            if (subSource.LinckedStorages.Length > 0)
                                CreateMastersStruct(null, keysarr, ref keyindex, j, subSource, sourceToDataObjectList, TypesByKeys, DataObjectCache);
                        }
                        else 
                        //if (keysarr[keyindex] != null && keysarr[keyindex] != DBNull.Value)
                        {
                            int prevKeyIndex = keyindex;
                            DataObject masterobj = null;
                            if (!subSource.HierarchicalLink)
                            {
                                DataObject exmasterobj = (DataObject)Information.GetPropValueByName(dataObject, subSource.ObjectLink);
                                if (TypesByKeys == null)
                                {
                                    object masterkey = keysarr[keyindex++];
                                    if (masterkey == DBNull.Value) masterkey = null;
                                    if (masterkey != null)
                                    {
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
                                }
                                else
                                {
                                    object mastertype = keysarr[keyindex++];
                                    object masterkey = keysarr[keyindex++];
                                    if (mastertype != DBNull.Value && mastertype != null)
                                    {
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
                                }
                                if (masterobj != null)
                                {
                                    if (masterobj.GetStatus(false) == ObjectStatus.Created)
                                    {
                                        masterobj.SetLoadingState(LoadingState.LightLoaded);
                                        masterobj.SetStatus(ObjectStatus.UnAltered);
                                        masterobj.InitDataCopy(DataObjectCache);
                                        masterobj.AddLoadedProperties("__PrimaryKey");
                                    }
                                    if (!dataObject.CheckLoadedProperty(subSource.ObjectLink))
                                    {
                                        Information.SetPropValueByName(dataObject, subSource.ObjectLink, masterobj);
                                        DataObject copyDO = dataObject.GetDataCopy();
                                        if (copyDO != null ) // && !dataObject.CheckLoadedProperty(subSource.ObjectLink))
                                            Information.SetPropValueByName(copyDO, subSource.ObjectLink, masterobj);
                                    }
                                    sourceToDataObjectList.Add(subSource, new object[] { masterobj, j });
                                    dataObject.AddLoadedProperties(subSource.ObjectLink);
                                }
                                
                            }
                            else
                            {
                                keyindex = keyindex + 2;
                                masterobj = dataObject;
                                sourceToDataObjectList.Add(subSource, new object[] { masterobj, j });
                            }
                            if (subSource.LinckedStorages.Length > 0)
                                CreateMastersStruct(masterobj, keysarr, ref keyindex, j, subSource, sourceToDataObjectList, TypesByKeys, DataObjectCache);
                        }
                        //else
                        //{
                        //    keyindex++;
                        //    if (subSource.LinckedStorages.Length>0)
                        //         CreateMastersStruct(null, keysarr, ref keyindex, j, subSource, sourceToDataObjectList, TypesByKeys, DataObjectCache);
                        //}
                    }
                }
            }
        }




       

        /// <summary>
        /// Получить детейловые объекты
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="dobject"></param>
        /// <param name="DetailObjects"></param>
        /// <param name="DetailsViewsForReadingByDetailPath"></param>
        public static void getDetailsObjects(IDataService dataService, STORMDO.DataObject dobject, out STORMDO.DataObject[] DetailObjects, out STORMDO.DataObject[] mastersObjects, out STORMDO.View[] DetailsViewsForReadingByDetailPath)
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
                        resmasters.Add(val);
                }

                if (prtype.IsSubclassOf(darrtype))
                {
                    if (loadedprops.Contains(prop))
                    {
                        foreach (STORMDO.DataObject arrobj in (STORMDO.DetailArray)STORMDO.Information.GetPropValueByName(dobject, prop))
                        {

                            resarr.Add(arrobj);
                            STORMDO.DataObject[] subDetailObjects;
                            STORMDO.DataObject[] smastersObjs;
                            STORMDO.View[] subDetailsViewsForReadingByDetailPath;
                            getDetailsObjects(dataService, arrobj, out subDetailObjects, out smastersObjs, out subDetailsViewsForReadingByDetailPath);
                            resarr.AddRange(subDetailObjects);
                            resview.AddRange(subDetailsViewsForReadingByDetailPath);
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
                            //добавляем первичный ключ
                            curDetView.AddProperty("__PrimaryKey", string.Empty, false, string.Empty);
                            //добавляем ссылку на мастера
                            curDetView.AddProperty(agrname, string.Empty, false, string.Empty);

                            curDetView.MasterTypeFilters.Add(agrname, new Type[] { dotype }.ToList());

                            string[] detNames = Information.GetStorablePropertyNames(curdettype).Where(x => 
                            Information.IsStoredProperty(curdettype, x) && Information.GetPropertyType(curdettype,x).IsSubclassOf(typeof(DataObject))
                            ).ToArray();
                            //нужно добавить дочитывание мастеров
                            foreach (string s in detNames)
                            {
                                if (s != agrname)
                                        curDetView.AddProperty(s, string.Empty, false, string.Empty);
                            }

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
                DetailObjects = new STORMDO.DataObject[0];
            else
            {
                DetailObjects = new ICSSoft.STORMNET.DataObject[resarr.Count];
                resarr.CopyTo(DetailObjects);
            }

            mastersObjects = (ICSSoft.STORMNET.DataObject[])resmasters.ToArray(typeof(STORMDO.DataObject));
            if (resview.Count == 0)
                DetailsViewsForReadingByDetailPath = new STORMDO.View[0];
            else
            {
                DetailsViewsForReadingByDetailPath = new STORMDO.View[resview.Count];
                resview.CopyTo(DetailsViewsForReadingByDetailPath);
            }
        }

        /// <summary>
        /// Обновить внутренние данные объекта
        /// </summary>
        /// <param name="dobj">объект</param>
        /// <param name="UpLevel">верхнего ли уровня</param>
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
                            dobj.SetLoadingState(STORMDO.LoadingState.Loaded);
                        else
                            dobj.SetLoadingState(STORMDO.LoadingState.LightLoaded);
                        dobj.SetStatus(STORMDO.ObjectStatus.UnAltered);
                        if (UpLevel)
                        {
                            dobj.InitDataCopy(DataObjectCache);
                        }
                        else
                            dobj.clearDataCopy();
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
                        //добавляем первичный ключ
                        curDetView.AddProperty("__PrimaryKey", string.Empty, false, string.Empty);
                        //добавляем ссылку на мастера
                        curDetView.AddProperty(agrname + Upmaster, string.Empty, false, string.Empty);

                        //Collections.NameObjectCollection noc = curDetView.MasterTypeFilters;
                        //for (int i = 0; i < MasterTypes.Count; i++)
                        //    noc.Add(MasterTypes.Keys[i], MasterTypes.Get(i));
                        //noc.Add(agrname, dotype);

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
            STORMDO.View[] DetailsViewsForDetailPath = new STORMDO.View[res.Count];
            res.CopyTo(DetailsViewsForDetailPath);
            return DetailsViewsForDetailPath;
        }

        /// <summary>
        /// Сравнение двух массивов
        /// </summary>
        /// <typeparam name="T">Тип элементов массива</typeparam>
        /// <param name="a1">Массив 1</param>
        /// <param name="a2">Массив 2</param>
        /// <param name="strict">Строгое сравнение (при нестрогом null и пустой массив эквивалентны)</param>
        /// <returns>Равны или нет</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2, bool strict = true)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (!strict && (a1 == null && (a2?.Length ?? 0) == 0 || a2 == null && (a1?.Length ?? 0) == 0))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;

            return true;
        }
    }
}
