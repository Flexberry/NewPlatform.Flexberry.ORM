using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using System.Data;
using ICSSoft.STORMNET.Security;
using STORMFunction = ICSSoft.STORMNET.FunctionalLanguage.Function;


namespace ICSSoft.STORMNET.Business.SQL
{
    using STORMDO = STORMNET;

    public class Utils
    {


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
        /// Преобразовать рекордсета в объекты
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataObjectType"></param>
        /// <param name="StorageStruct"></param>
        /// <param name="customizationStruct"></param>
        /// <param name="dataService"></param>
        /// <param name="TypesByKeys"></param>
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
                value, dataObjectType, StorageStruct, customizationStruct, res, dataService, TypesByKeys, false, DataObjectCache, securityManager, connection, transaction);
            return res;
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
                return;

            int ObjectTypeIndexPOs = value[0].Length - 1;
            object[] keys = new object[value.Length];
            int keyIndex = customizationStruct.View.Properties.Length + customizationStruct.AdvancedColumns.Length;
            bool[] WasReadingTypes = new bool[dataObjectType.Length];
            Array.Clear(WasReadingTypes, 0, WasReadingTypes.Length);


            //#if NETFX_35
            for (int i = 0; i < value.Length; i++)
            //#else
            // System.Threading.Tasks.Parallel.For(0, value.Length, i =>
            //#endif
            {
                Int64 index = Convert.ToInt64(value[i][ObjectTypeIndexPOs].ToString());
                WasReadingTypes[index] = true;
                if (res[i] == null)
                    res[i] = dataObjectCache.CreateDataObject(dataObjectType[index], value[i][keyIndex]);
                ICSSoft.STORMNET.Business.Utils.FillRowSetToDataObject(res[i], value[i], storageStruct[index], customizationStruct, typesByKeys, customizationStruct.AdvancedColumns, dataObjectCache, securityManager, сlearDataObjects);
                keys[i] = res[i].__PrimaryKey;
            }
            //#if NETFX_35
            //#else
            //    );
            //#endif

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
                        break; // если не хранимый, то пропустим

                    var div = (DetailInView)details[detnames[i]];
                    if (div.LoadOnLoadAgregator)
                    {
                        var detTypes = new ArrayList();
                        for (int h = 0; h < dataObjectType.Length; h++)
                        {
                            if (WasReadingTypes[h])
                            {
                                Type[] tu = dataService.TypeUsage.GetUsageTypes(dataObjectType[h], div.Name);
                                for (int n = 0; n < tu.Length; n++)
                                    if (!detTypes.Contains(tu[n]))
                                        detTypes.Add(tu[n]);
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
                        if (div.Name.IndexOf(".") <= 0)
                        {
                            alPars.AddRange(keys);
                            alagrObjects.AddRange(res);
                        }
                        else
                        {
                            string mprop = div.Name.Substring(0, div.Name.IndexOf("."));
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
                                    new ColumnsSortDef(agregatorName,SortOrder.Asc),
                                    new ColumnsSortDef(sortprop,SortOrder.Asc)
                                });
                        }
                        else
                        {
                            arsorts.Add(new ColumnsSortDef(agregatorName, SortOrder.Asc));
                        }

                        ColumnsSortDef[] subsorts = customizationStruct.GetColumnsSortDef(div.Name);
                        if (subsorts != null && subsorts.Length > 0)
                            arsorts.AddRange(subsorts);
                        sorts = (ColumnsSortDef[])arsorts.ToArray(typeof(ColumnsSortDef));

                        // собираем типы данных для детейла
                        var dcs = new LoadingCustomizationStruct(dataService.GetInstanceId());
                        string divname = div.Name;
                        if (divname.IndexOf(".") >= 0) divname = divname.Substring(divname.IndexOf(".") + 1);
                        for (int j = 0; j < alagrObjects.Count; j++)
                            ((DataObject)alagrObjects[j]).AddLoadedProperties(divname);
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
                                View typeView = (div.AdaptiveTypeViews.Contains(typeKey)) ? (ICSSoft.STORMNET.View)div.AdaptiveTypeViews[typeKey] : Information.GetView(div.View.Name, typeKey);
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
                                                dobj.InitDataCopy(dataObjectCache,!сlearDataObjects);
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
                                                    al.Add(do1);
                                            }
                                            DataObject[] detailObjects1 = (DataObject[])al.ToArray(typeof(DataObject));
                                            //Братчиков 2009-07-08 - Баг при зачитке адаптивных детейлов. Представление не соответствует зачитываемым объектам. Антиглюк сделан по первому объекту в массиве (косяк выйдет если представление было всё-таки совместимым). Возможно всегда можно применять div.View.
                                            {
                                                View view4Load = dcs.View;
                                                if (detailObjects1.Length > 0 && !(detailObjects1[0].GetType().Equals(view4Load.DefineClassType) || detailObjects1[0].GetType().IsSubclassOf(view4Load.DefineClassType)))
                                                {
                                                    view4Load = div.View;
                                                }

                                                // TODO: разобраться, может ли это приводить к проблемам, если будет выполняться вне транзакции.
                                                dataService.LoadObjects(detailObjects1, view4Load, сlearDataObjects, dataObjectCache);
                                            }
                                            //Братчиков 2009-07-08
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
                                            if (d.__PrimaryKey.ToString() == detailObjects[j].__PrimaryKey.ToString()
                                                || (d.Prototyped && d.__PrototypeKey.ToString() == detailObjects[j].__PrimaryKey.ToString()))
                                            {
                                                p = d;
                                            }
                                        if (p != null)
                                            detailObjects[j] = p;
                                    }


                                    if (detArr.GetByKey(detailObjects[j].__PrimaryKey) != null) detArr.RemoveByKey(detailObjects[j].__PrimaryKey);
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
                                        if (((DataObject)alagrObjects[index]).__PrimaryKey.Equals(curKey)) break;

                                    if (detArr != null)
                                    {
                                        DataObject dobj = detArr.AgregatorObject;
                                        dobj.AddLoadedProperties(divname);
                                    }

                                    detArr = (DetailArray)Information.GetPropValueByName(((DataObject)alagrObjects[index]), divname);
                                    if (detArr == null)
                                    {
                                        Type detArrType = Information.GetPropertyType(customizationStruct.View.DefineClassType, div.Name);
                                        detArr = (DetailArray)detArrType.GetConstructor(new Type[] { alagrObjects[index].GetType() }).Invoke(new object[] { ((DataObject)alagrObjects[index]) });
                                        Information.SetPropValueByName(((DataObject)alagrObjects[index]), divname, detArr);
                                    }

                                    if (!сlearDataObjects)
                                    {
                                        var al = new ArrayList();
                                        foreach (DataObject do1 in detArr.GetAllObjects())
                                        {
                                            bool founded = keys.Contains(do1.__PrimaryKey);
                                            if (!founded)
                                                al.Add(do1);
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
                                        if (d.__PrimaryKey.ToString() == detailObjects[j].__PrimaryKey.ToString()
                                            || (d.Prototyped && d.__PrototypeKey.ToString() == detailObjects[j].__PrimaryKey.ToString()))
                                        {
                                            p = d;
                                            break;
                                        }
                                    if (p != null)
                                        detailObjects[j] = p;
                                }

                                if (detArr.GetByKey(detailObjects[j].__PrimaryKey) != null) detArr.RemoveByKey(detailObjects[j].__PrimaryKey);

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
                                detArr.Ordering();
                            DataObject dobj = detArr.AgregatorObject;
                            dobj.AddLoadedProperties(divname);
                        }
                    }
                }
                if (customizationStruct.InitDataCopy)
                {
                    //#if NETFX_35 
                    foreach (DataObject dobj in res)
                    //#else
                    //    System.Threading.Tasks.Parallel.ForEach(res, dobj =>
                    //#endif
                    {
                        ObjectStatus prevStatus = dobj.GetStatus(false);
                        dobj.InitDataCopy(dataObjectCache, !сlearDataObjects);
                        if (prevStatus == ObjectStatus.Deleted)
                            dobj.SetStatus(ObjectStatus.Deleted);
                        else
                            dobj.SetStatus(ObjectStatus.UnAltered);
                    }
                    //#if NETFX_35 
                    //#else
                    //    );
                    //#endif

                }
            }

            
        }
    }
}
