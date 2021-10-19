namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using ICSSoft.STORMNET.Security;

    /// <summary>
    /// Набор служебной логики для сервиса данных.
    /// </summary>
    public partial class Utils
    {
        /// <summary>
        /// Заполнить объект данных.
        /// </summary>
        /// <param name="dobject">Объект данных.</param>
        /// <param name="values">Значения для заполнения.</param>
        /// <param name="storageStruct">Метаданные структуры хранения.</param>
        /// <param name="customizationStruct">Настройка выборки данных.</param>
        /// <param name="typesByKeys">Служебная структура, увязывающая типы мастеров и их ключи.</param>
        /// <param name="advCols">Дополнительные колонки.</param>
        /// <param name="dataObjectCache">Кэш объектов данных.</param>
        /// <param name="securityManager">Менеджер полномочий.</param>
        public static void FillRowSetToDataObject(DataObject dobject, object[] values, StorageStructForView storageStruct, LoadingCustomizationStruct customizationStruct, System.Collections.SortedList typesByKeys, AdvansedColumn[] advCols, DataObjectCache dataObjectCache, ISecurityManager securityManager)
        {
            Type dobjectType = dobject.GetType();

            /* access type */

            if (!securityManager.AccessObjectCheck(dobjectType, tTypeAccess.Full, false))
            {
                securityManager.AccessObjectCheck(dobjectType, tTypeAccess.Read, true);
            }

            /* access type */

            // Заливаем данные в объект данных.
            int customizationStructViewPropertiesLength = customizationStruct.View.Properties.Length;
            int advColsLength = advCols.Length;
            Information.SetPropValueByName(dobject, nameof(DataObject.__PrimaryKey), values[customizationStructViewPropertiesLength + advColsLength]);

            // Смирнов: Прочие свойства будут добавлены в методе ProcessPropertyValues.
            // Необходимо для обработки ситуации, когда первичный ключ объекта не добавлен в представление.
            dobject.AddLoadedProperties(nameof(DataObject.__PrimaryKey));

            // 1. создаем структуру мастеров(свойств-объектов данных).
            SortedList assList = new SortedList();
            int index = customizationStructViewPropertiesLength + 1 + advColsLength;
            CreateMastersStruct(dobject, values, ref index, 0, storageStruct.sources, assList, typesByKeys, dataObjectCache);
            assList.Add(storageStruct.sources, new object[] { dobject, 0 });

            // 2. заливаем данные.
            ArrayList properiesValues = new ArrayList();
            StringCollection allAdvCols = new StringCollection();

            int masterPosition = index;

            for (int i = 0; i < advColsLength; i++)
            {
                object value = values[i + customizationStructViewPropertiesLength];
                if (value == DBNull.Value)
                {
                    value = null;
                }

                properiesValues.Add(new[] { advCols[i].Name, value, dobject });
                allAdvCols.Add(advCols[i].Name);
                dobject.DynamicProperties.Add(advCols[i].Name, null);
            }

            for (int i = 0; i < customizationStructViewPropertiesLength; i++)
            {
                StorageStructForView.PropStorage prop = storageStruct.props[i];

                // Information.IsStoredProperty for __PrimaryKey property always return true, but master may be NotStored then check it.
                string propertyName = prop.Name.Replace("." + nameof(DataObject.__PrimaryKey), string.Empty);

                if (Information.IsStoredProperty(dobjectType, propertyName) || prop.Expression != null)
                {
                    if (prop.MastersTypes == null)
                    {
                        object[] tmp = (object[])assList[prop.source];
                        object value;
                        if (customizationStruct.ColumnsOrder != null && customizationStruct.ColumnsOrder.Length >= customizationStructViewPropertiesLength)
                        {
                            value = values[Array.IndexOf(customizationStruct.ColumnsOrder, prop.Name)];
                        }
                        else
                        {
                            value = values[i];
                        }

                        if (value == DBNull.Value)
                        {
                            value = null;
                        }

                        if (tmp != null)
                        {
                            properiesValues.Add(
                                new[] { prop.simpleName, value, tmp[0] });
                        }
                    }
                    else
                    {
                        object[] tmp = (object[])assList[prop.source];
                        if (tmp != null)
                        {
                            // Ищем позицию.
                            int tmp1 = (int)tmp[1];
                            int curMasterPosition = masterPosition;
                            for (int j = 0; j < tmp1; j++)
                            {
                                curMasterPosition += prop.MastersTypes[j].Length;
                            }

                            int k = 0;
                            object value = values[curMasterPosition];
                            if (value == DBNull.Value)
                            {
                                value = null;
                            }

                            while (k < prop.MastersTypes[tmp1].Length - 1 && value == null)
                            {
                                k++;
                                value = values[curMasterPosition + k];
                                if (value == DBNull.Value)
                                {
                                    value = null;
                                }
                            }

                            object tmp0 = tmp[0];
                            if (value != null)
                            {
                                var master = Information.GetPropValueByName((DataObject)tmp0, prop.simpleName);
                                if (master == null)
                                {
                                    DataObject no = dataObjectCache.CreateDataObject(prop.MastersTypes[tmp1][k], value);
                                    if (no.GetStatus(false) == ObjectStatus.Created)
                                    {
                                        no.SetStatus(ObjectStatus.UnAltered);
                                        no.SetLoadingState(LoadingState.LightLoaded);
                                        no.InitDataCopy(dataObjectCache);
                                    }

                                    value = no;
                                    properiesValues.Add(new[] { prop.simpleName, value, tmp0 });
                                }
                                else
                                {
                                    // changed by fat
                                    properiesValues.Add(new[] { prop.simpleName, master, tmp0 });
                                }
                            }
                            else
                            {
                                properiesValues.Add(new[] { prop.simpleName, null, tmp0 });
                            }
                        }

                        masterPosition += prop.MastersTypesCount;
                    }
                }
            }

            // 2.2 Записываем в объекты.
            while (properiesValues.Count > 0)
            {
                ProcessPropertyValues(properiesValues);
            }
        }

        internal static void ProcessPropertyValues(ArrayList properiesValues)
        {
            // a. Выбираем для текущего объекта все свойства.
            object[] tmp = (object[])properiesValues[0];
            DataObject curobj = (DataObject)tmp[2];
            DataObject curobjCopy = curobj.GetDataCopy();
            Type dobjectType = curobj.GetType();
            SortedList curObjProperiesValues = new SortedList();

            List<string> loadedPropsColl = curobj.GetLoadedPropertiesList();

            for (int i = properiesValues.Count - 1; i >= 0; i--)
            {
                tmp = (object[])properiesValues[i];
                if (tmp[2] == curobj)
                {
                    object tmp0 = tmp[0];
                    if (!curObjProperiesValues.ContainsKey(tmp0))
                    {
                        curObjProperiesValues.Add(tmp0, tmp[1]);
                        string s = (string)tmp0;
                        if (!loadedPropsColl.Contains(s))
                        {
                            loadedPropsColl.Add(s);
                        }
                    }

                    properiesValues.RemoveAt(i);
                }
            }

            // b. Раскидываем согласно LoadOrder;
            string[] loadOrder = Information.GetLoadingOrder(dobjectType);
            int loadOrderLength = loadOrder.Length;
            for (int i = 0; i < loadOrderLength; i++)
            {
                string propName = loadOrder[i];
                if (curObjProperiesValues.ContainsKey(propName))
                {
                    Information.SetPropValueByName(curobj, propName, curObjProperiesValues[propName]);
                    curObjProperiesValues.Remove(propName);
                }
            }

            int curObjPropertiesValuesCount = curObjProperiesValues.Count;
            for (int i = 0; i < curObjPropertiesValuesCount; i++)
            {
                string propName = (string)curObjProperiesValues.GetKey(i);
                object propValue = curObjProperiesValues.GetByIndex(i);
                Information.SetPropValueByName(curobj, propName, propValue);

                // Смирнов: мастера инициализируются в методе CreateMastersStruct, придётся вручную записывать данные в копию.
                // Иначе свойство будет считаться измененным, т.к. в копии оно будет незаполнено.
                if (curobjCopy != null)
                {
                    if (propValue is DataObject dobjProp)
                    {
                        // При заполнении копии данных необходимо помещать копию мастера в копию объекта.
                        DataObject dobjPropCopy = dobjProp.GetDataCopy();
                        Information.SetPropValueByName(curobjCopy, propName, dobjPropCopy);
                    }
                    else
                    {
                        Information.SetPropValueByName(curobjCopy, propName, propValue);
                    }
                }
            }

            // Проверим, что все свойства объекта данных содержатся в перечне загруженных свойств.
            // Смирнов: для получения свойств объекта применяется метод, используемый в конструкторе `View(Type, ReadType)`.
            string[] doProperties = Information.GetStorablePropertyNames(dobjectType);
            if (loadedPropsColl.Count >= doProperties.Length
                && doProperties.All(p => loadedPropsColl.Contains(p)))
            {
                curobj.SetLoadingState(LoadingState.Loaded);
                curobjCopy?.SetLoadingState(LoadingState.Loaded);
            }
            else
            {
                curobj.SetLoadingState(LoadingState.LightLoaded);
            }

            // Смирнов: чтобы не сломать существующий код, загруженные свойства добавим в любом случае.
            curobj.AddLoadedProperties(loadedPropsColl);

            // Смирнов: мастера инициализируются в методе CreateMastersStruct, придётся вручную записывать данные в копию.
            curobjCopy?.AddLoadedProperties(loadedPropsColl);

            curobj.SetStatus(ObjectStatus.UnAltered);
        }
    }
}
