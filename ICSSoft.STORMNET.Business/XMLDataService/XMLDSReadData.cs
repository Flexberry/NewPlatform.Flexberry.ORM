namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;

    public partial class XMLFileDataService
    {
        private const string StormJoinedMasterKey = "STORMJoinedMasterKey";

        /// <summary>
        /// Константа для STORMMainObjectKey.
        /// </summary>
        private const string StormMainObjectKey = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.StormMainObjectKey;

        private const string StormDataObjectType = "STORMNETDATAOBJECTTYPE";

        /// <summary>
        /// Возвращает сформированную таблицу данных для указанного запроса в виде lcs.
        /// </summary>
        /// <param name="customizationStruct">lcs для формирования запроса данных.</param>
        /// <param name="storageStructs">Структуры хранения, которые будут получены из lcs.</param>
        /// <param name="maxCountKeys">Максимальное колличество ключей для структур хранения.</param>
        /// <returns></returns>
        private DataTable GetDataSet(LoadingCustomizationStruct customizationStruct, out StorageStructForView[] storageStructs,
                                     out int maxCountKeys)
        {
            var resData = new DataTable("ResultTable");
            maxCountKeys = 0;
            storageStructs = new StorageStructForView[customizationStruct.LoadingTypes.Length];

            if (customizationStruct.LoadingTypes.Length == 0)
            {
                return resData;
            }

            var countKeys = new int[customizationStruct.LoadingTypes.Length];

            for (int i = 0; i < customizationStruct.LoadingTypes.Length; i++)
            {
                storageStructs[i] = Information.GetStorageStructForView(customizationStruct.View, customizationStruct.LoadingTypes[i],
                                                                        StorageTypeEnum.SimpleStorage, null, GetType());
                countKeys[i] = Utils.CountMasterKeysInSelect(storageStructs[i]);

                if (countKeys[i] > maxCountKeys)
                {
                    maxCountKeys = countKeys[i];
                }
            }

            resData.TableName = "__" + customizationStruct.LoadingTypes[0].Name;
            List<ColumnInfo> columnsInfo = AddColumnsToDataTable(resData, storageStructs);

            for (int i = 0; i < storageStructs.Length; i++)
            {
                FillDataTable(resData, storageStructs[i], columnsInfo, i);
            }

            // TODO разобраться с for (int j = 0; j < addingKeysCount;  j++) AddColumn(STORMJoinedMasterKey)

            return resData;
        }

        /// <summary>
        /// Добавляет в таблицу новые строки с заполненными данными для указанной структуры.
        /// </summary>
        /// <param name="dt">Таблица, в которую добавляются строки.</param>
        /// <param name="storageStruct">Структура хранения на основе которой будут добавлять строки.</param>
        /// <param name="columnsInfo">Информация о колонках для определения, что и в какие колонки записывать.</param>
        /// <param name="structIndex">
        /// Номер структуры.
        /// Необходим для того, чтобы достать настройки из информации о колонках.
        /// Если необходимо заполнить таблицу одним массивом данных и структура была одна, то необходимо передать 0.
        /// </param>
        private void FillDataTable(DataTable dt, StorageStructForView storageStruct, List<ColumnInfo> columnsInfo,
                                   int structIndex)
        {
            DataTable mainDataTable = _dataSet.Tables[storageStruct.sources.storage[0].Storage];
            int startRowsIndex = dt.Rows.Count;

            // Добавляем столько строк, сколько их в таблице основного объекта данных
            for (int i = 0; i < mainDataTable.Rows.Count; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            // В этот список будут записаны мастеровые таблицы после соединения.
            var masterTables = new Dictionary<StorageStructForView.PropSource, DataTable>();
            SetAllMasterTables(storageStruct.sources, mainDataTable, masterTables);

            // Далее заполняем таблицу по ячейкам.
            foreach (var columnInfo in columnsInfo)
            {
                StorageStructForView.PropStorage propStorage = columnInfo.PropStorages == null
                                                                   ? null
                                                                   : columnInfo.PropStorages[structIndex];
                StorageStructForView.PropSource propSource = columnInfo.PropSources == null
                                                                 ? null
                                                                 : columnInfo.PropSources[structIndex];

                // Если это свойство хранимое
                if (propStorage != null && propStorage.Stored && !columnInfo.IsStormDataObjectType
                    && !columnInfo.IsStormJoinedMasterKey && !columnInfo.IsStormMainObjectKey)
                {
                    // Если это свойство является собственным для основного объекта данных, то ищем в основной таблице.
                    if (propSource == storageStruct.sources)
                    {
                        for (int i = 0; i < mainDataTable.Rows.Count; i++)
                        {
                            dt.Rows[i + startRowsIndex][columnInfo.ColumnName] = mainDataTable.Rows[i][propStorage.Name];
                        }
                    }
                    else if (masterTables.ContainsKey(propSource))
                    {
                        // иначе это свойство мастера и надо брать его из таблицы мастеров после соединения
                        for (int i = 0; i < mainDataTable.Rows.Count; i++)
                        {
                            dt.Rows[i + startRowsIndex][columnInfo.ColumnName] =
                                masterTables[propSource].Rows[i][propStorage.simpleName];
                        }
                    }
                }
                else
                {
                    if (columnInfo.IsStormMainObjectKey)
                    {
                        // в эту колонку записываются первичные ключи основного объекта данных
                        for (int i = 0; i < mainDataTable.Rows.Count; i++)
                        {
                            dt.Rows[i + startRowsIndex][columnInfo.ColumnName] =
                                mainDataTable.Rows[i][storageStruct.sources.storage[0].PrimaryKeyStorageName];
                        }
                    }
                    else if (columnInfo.IsStormDataObjectType)
                    {
                        // В эту колонку записывается номер типа для загрузки данных.
                        // Он необходим когда читаются наследники через базовый класс.
                        for (int i = 0; i < mainDataTable.Rows.Count; i++)
                        {
                            dt.Rows[i + startRowsIndex][columnInfo.ColumnName] = structIndex;
                        }
                    }
                    else if (columnInfo.IsStormJoinedMasterKey)
                    {
                        // если этот мастер был указан в представлении, то его можно вытянуть через PropStorage
                        string simpleName = propStorage != null
                                                ? propStorage.simpleName
                                                : propSource.storage[0].PrimaryKeyStorageName;

                        // в эти колонки записываются первичные ключи мастеров
                        for (int i = 0; i < mainDataTable.Rows.Count; i++)
                        {
                            // Ключ мастера храниться в таблице мастера
                            if (masterTables.ContainsKey(propSource))
                            {
                                dt.Rows[i + startRowsIndex][columnInfo.ColumnName] =
                                    masterTables[propSource].Rows[i][simpleName];
                            }
                            else
                            {
                                // или его можно взять сразу из таблицы основного объекта данных
                                dt.Rows[i + startRowsIndex][columnInfo.ColumnName] = mainDataTable.Rows[i][simpleName];
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает все таблицы мастеров после соединения.
        /// </summary>
        /// <param name="propSource">Базовый источник данных в котором будут искаться мастера.</param>
        /// <param name="mainTable">Таблица основного объекта данных.</param>
        /// <param name="result">Результат выполнения метода. Необходим для выполнения рекурсии.</param>
        /// <returns>Таблицы мастеров после соединения.</returns>
        private void SetAllMasterTables(StorageStructForView.PropSource propSource, DataTable mainTable,
                                        Dictionary<StorageStructForView.PropSource, DataTable> result)
        {
            if (propSource.LinckedStorages == null || propSource.LinckedStorages.Length == 0)
            {
                return;
            }

            foreach (var source in propSource.LinckedStorages)
            {
                string currentTableName = source.storage[0].Storage;

                DataTable dt1 = mainTable ?? result[propSource];
                DataTable dt2 = _dataSet.Tables[currentTableName];

                // соединяем переданный источник с текущим
                DataTable resultTable = GetResultJoinTables(dt1, dt2, source.ObjectLink,
                                                            source.storage[0].PrimaryKeyStorageName);
                result.Add(source, resultTable);

                SetAllMasterTables(source, null, result);
            }
        }

        /// <summary>
        /// Получить массив всех свойств мастера, после соединения таблиц по указанным полям.
        /// </summary>
        /// <param name="dt1">
        /// Таблица которую будут соединять.
        /// В этой таблице будет искаться свойство, в котором храниться ключ мастера.
        /// </param>
        /// <param name="dt2">
        /// Таблица с которой будут соединять (таблица мастера).
        /// Значения этой таблицы после соединения и будут результатом.
        /// </param>
        /// <param name="propMasterKey">
        /// Имя свойства мастера из первой таблицы.
        /// Имя поля по которому соединяют со стороны первой таблицы.
        /// </param>
        /// <param name="pkMasterName">
        /// Имя свойства мастера в котором храниться первичный ключ.
        /// Имя поля по которому соединяют со стороны второй таблицы.
        /// </param>
        /// <returns>
        /// Значения свойств мастера. Пара: имя свойства и массив значений.
        /// Все массивы размернойстью в количество строк основной таблицы (первой).
        /// Если ссылка на мастера была пустой или мастер не был найден, то весь массив будет заполнен null'ми.
        /// Возвращает null в случае, если не найдена таблицы или свойство мастера из первой таблицы.
        /// </returns>
        private DataTable GetResultJoinTables(DataTable dt1, DataTable dt2, string propMasterKey, string pkMasterName)
        {
            var propMasterNames = (from DataColumn column in dt2.Columns select column.ColumnName).ToList();

            return GetResultJoinTablesByPropNames(dt1, dt2, propMasterKey, pkMasterName, propMasterNames);
        }

        /// <summary>
        /// Получить массив свойств мастера, после соединения таблиц по указанным полям.
        /// </summary>
        /// <param name="dt1">
        /// Таблица которую будут соединять.
        /// В этой таблице будет искаться свойство, в котором храниться ключ мастера.
        /// </param>
        /// <param name="dt2">
        /// Таблица с которой будут соединять (таблица мастера).
        /// В этой таблице будут искаться свойства массивы значений которых возвращается.
        /// </param>
        /// <param name="propMasterKey">
        /// Имя свойства мастера из первой таблицы.
        /// Имя поля по которому соединяют со стороны первой таблицы.
        /// </param>
        /// <param name="pkMasterName">
        /// Имя свойства мастера в котором храниться первичный ключ.
        /// Имя поля по которому соединяют со стороны второй таблицы.
        /// </param>
        /// <param name="propMasterNames">
        /// Имена свойств мастера, которые необходимо вернуть.
        /// </param>
        /// <returns>
        /// Значения свойств мастера. Пара: имя свойства и массив значений.
        /// Все массивы размернойстью в количество строк основной таблицы (первой).
        /// Если ссылка на мастера была пустой или мастер не был найден, то весь массив будет заполнен null'ми.
        /// Возвращает null в случае, если не найдена таблицы или свойство мастера из первой таблицы.
        /// </returns>
        private DataTable GetResultJoinTablesByPropNames(DataTable dt1, DataTable dt2, string propMasterKey,
                                                        string pkMasterName, List<string> propMasterNames)
        {
            var result = new DataTable(dt2.TableName);

            foreach (var propMasterName in propMasterNames)
            {
                result.Columns.Add(new DataColumn(propMasterName, dt2.Columns[propMasterName].DataType));
            }

            propMasterNames.ToDictionary(propMasterName => propMasterName,
                                                      propMasterName => new object[dt1.Rows.Count]);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                result.Rows.Add(result.NewRow());
                object masterKey = dt1.Rows[i][propMasterKey];

                if (masterKey == null)
                {
                    // если ссылка на мастера пустая забиваем все null'ми
                    foreach (var propMasterName in propMasterNames)
                    {
                        result.Rows[i][propMasterName] = DBNull.Value;
                    }
                }
                else
                {
                    DataRow[] rows = dt2.Select(string.Format("{0}=\'{1}\'", pkMasterName, masterKey));

                    if (rows != null && rows.Length > 0)
                    {
                        // по умолчанию считаем, что ключ уникален, поэтому берем данные только из rows[0]
                        foreach (var propMasterName in propMasterNames)
                        {
                            result.Rows[i][propMasterName] = rows[0][propMasterName];
                        }
                    }
                    else
                    {
                        // если не нашли мастера с таким ключом забиваем все null'ми
                        foreach (var propMasterName in propMasterNames)
                        {
                            result.Rows[i][propMasterName] = DBNull.Value;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает все источники мастеров по иерархии для указанного источника данных.
        /// </summary>
        /// <param name="propSource">Источник данных от которого начнется проход по иерархии мастеров.</param>
        /// <returns>Список всех источников мастеров, полученных по иерархии.</returns>
        private List<StorageStructForView.PropSource> GetAllLinkedStoragesFroPropSource(StorageStructForView.PropSource propSource)
        {
            if (propSource.LinckedStorages == null || propSource.LinckedStorages.Length == 0)
            {
                return new List<StorageStructForView.PropSource>();
            }

            var result = new List<StorageStructForView.PropSource>();

            foreach (var source in propSource.LinckedStorages)
            {
                result.Add(source);

                var linkedStorages = GetAllLinkedStoragesFroPropSource(source);
                result.AddRange(linkedStorages);
            }

            return result;
        }

        /// <summary>
        /// Добавление колонок в таблицу.
        /// метод необходим при формировании DataSet'а при запросе данных.
        /// </summary>
        /// <param name="dt">Таблица в которой будут созданы колонки.</param>
        /// <param name="storageStructs">
        /// Структуры хранения, по которым будут сформированы колонки в таблице.
        /// </param>
        private List<ColumnInfo> AddColumnsToDataTable(DataTable dt, StorageStructForView[] storageStructs)
        {
            DataTable mainStructureTable = _altdataSet.Tables[storageStructs[0].sources.storage[0].Storage];
            DataTable mainDataTable = _dataSet.Tables[storageStructs[0].sources.storage[0].Storage];

            if (mainStructureTable == null || mainDataTable == null)
            {
                return new List<ColumnInfo>();
            }

            var columnsInfo = new List<ColumnInfo>();
            var listMasters = new List<List<StorageStructForView.PropStorage>>();

            // Сначало добавим колонки из представления для первой структуры,
            // так как для всех последующих их набор не изменится
            foreach (var prop in storageStructs[0].props)
            {
                if (prop.Name == "__PrimaryKey")
                {
                    continue;
                }

                // Если это мастер, то значит будет записан гуид, а гуиды записываются строкой
                Type colType = prop.MastersTypesCount == 0 ? prop.propertyType : typeof(string);

                // Если этот тип не находится, то пробуем взять его из таблицы
                if (Type.GetType(colType.FullName) == null || colType.Name.StartsWith("Nullable"))
                {
                    if (mainStructureTable.Columns.Contains(prop.Name))
                    {
                        colType = mainStructureTable.Columns[prop.Name].DataType;
                    }
                    else
                    {
                        DataTable masterTable = _altdataSet.Tables[prop.source.storage[0].Storage];

                        if (masterTable != null && masterTable.Columns.Contains(prop.simpleName))
                        {
                            colType = masterTable.Columns[prop.simpleName].DataType;
                        }
                    }
                }

                if (colType.IsEnum)
                {
                    colType = typeof(string);
                }

                dt.Columns.Add(prop.Name, colType);
                columnsInfo.Add(new ColumnInfo(prop.Name, prop));

                if (prop.MastersTypesCount > 0)
                {
                    listMasters.Add(new List<StorageStructForView.PropStorage> { prop });
                }
            }

            // затем добавим свойства для остальных структур, они понадобятся при заполнении таблицы данными
            for (int i = 1; i < storageStructs.Length; i++)
            {
                for (int j = 0; j < storageStructs[i].props.Length; j++)
                {
                    columnsInfo[j].PropStorages.Add(storageStructs[i].props[j]);

                    if (storageStructs[i].props[j].MastersTypesCount > 0)
                    {
                        listMasters[j].Add(storageStructs[i].props[j]);
                    }
                }
            }

            // добавим колонку для первичных ключей объектов
            dt.Columns.Add(StormMainObjectKey);
            columnsInfo.Add(new ColumnInfo(StormMainObjectKey));

            // добавим колонку для идентификации типа наследника объекта данных, для которрого было сформировано представление
            dt.Columns.Add(new DataColumn(StormDataObjectType) { DefaultValue = 0 });
            columnsInfo.Add(new ColumnInfo(StormDataObjectType));

            var linkedMasters = GetAllLinkedStoragesFroPropSource(storageStructs[0].sources);
            int startIndexJoinedMasterKeys = columnsInfo.Count;

            // добавим дополнительные колонки для всех мастеров,
            // которые необходимы для зачитки всех свойств представления
            for (int j = 0; j < linkedMasters.Count; j++)
            {
                string colName = StormJoinedMasterKey + j;
                dt.Columns.Add(colName);
                columnsInfo.Add(new ColumnInfo(colName, linkedMasters[j]));
            }

            // добавим источники для других структур, они пригодятся при заполнении таблицы данными
            for (int i = 1; i < storageStructs.Length; i++)
            {
                var linkedMasters2 = GetAllLinkedStoragesFroPropSource(storageStructs[i].sources);

                for (int j = startIndexJoinedMasterKeys; j < linkedMasters2.Count + startIndexJoinedMasterKeys; j++)
                {
                    columnsInfo[j].PropSources.Add(linkedMasters2[j - startIndexJoinedMasterKeys]);
                }
            }

            // затем добавим дополнительные колонки для всех мастеров из представления.
            // Отчасти они продублируют первый набор ключей, но ничего не поделать,
            // ICSSoft.STORMNET.Businessю.Utils работает именно с таким набором колонок.
            for (int j = 0; j < listMasters.Count; j++)
            {
                int index = linkedMasters.Count + j;
                string colName = StormJoinedMasterKey + index;
                dt.Columns.Add(colName);
                columnsInfo.Add(new ColumnInfo(colName, listMasters[j]));
            }

            return columnsInfo;
        }

        private object[][] ReadData(LoadingCustomizationStruct customizationStruct, out StorageStructForView[] storageStruct)
        {
            // строим запрос
            View dataObjectView = customizationStruct.View;
            Function limitFunction = customizationStruct.LimitFunction;
            int maxCountKeys;

            var resData = GetDataSet(customizationStruct, out storageStruct, out maxCountKeys);

            // строим Wrap-Select
            #region задаем порядок колонок в запросе - результат в props : StringCollection

            var props = new StringCollection();

            for (int i = 0; i < dataObjectView.Properties.Length; i++)
            {
                props.Add(dataObjectView.Properties[i].Name);
            }

            ColumnsSortDef[] sorts = customizationStruct.GetOwnerOnlySortDef();

            if (customizationStruct.ColumnsOrder != null)
            {
                int k = 0;

                foreach (string prop in customizationStruct.ColumnsOrder)
                {
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

            props.Add(StormMainObjectKey);

            for (int i = 0; i < maxCountKeys; i++)
            {
                props.Add(StormJoinedMasterKey + i);
            }

            props.Add(StormDataObjectType);
            #endregion

            string expression = string.Empty;
            string sort = string.Empty;

            if (limitFunction != null)
            {
                expression = LimitFunction2FilterExpression(limitFunction);
            }

            string[] ascDesc = { string.Empty, " ASC", " DESC" };

            if (sorts != null && sorts.Length > 0)
            {
                bool notfirst = false;

                foreach (ColumnsSortDef columnsSortDef in sorts)
                {
                    if (props.Contains(columnsSortDef.Name))
                    {
                        if (notfirst)
                        {
                            sort += ", ";
                        }

                        sort += PutIdentifierIntoBrackets(columnsSortDef.Name) + ascDesc[(int)columnsSortDef.Sort];
                        notfirst = true;
                    }
                }
            }

            DataRow[] resRows = resData.Select(expression, sort);

            int indexEnd = customizationStruct.ReturnTop > 0
                ? customizationStruct.ReturnTop
                    : (customizationStruct.RowNumber != null
                       ? customizationStruct.RowNumber.EndRow
                       : resRows.Length);
            int indexStart = customizationStruct.ReturnTop <= 0 && customizationStruct.RowNumber != null
                ? customizationStruct.RowNumber.StartRow
                      : 0;
            int count = indexEnd - indexStart;
            int indexCurrentRow = 0;

            var res = new object[count][];

            for (int i = indexStart; i < indexEnd; i++)
            {
                DataRow curRow = resRows[i];
                var curRes = new object[props.Count];

                for (int j = 0; j < props.Count; j++)
                {
                    curRes[j] = curRow[props[j]];
                }

                res[indexCurrentRow++] = curRes;
            }

            return res;
        }

        private class ColumnInfo
        {
            private List<StorageStructForView.PropSource> _propSources;

            public List<StorageStructForView.PropSource> PropSources
            {
                get
                {
                    return PropStorages == null || PropStorages.Count == 0
                        ? _propSources
                        : (from item in PropStorages select item.source).ToList();
                }
            }

            public bool IsStormDataObjectType
            {
                get { return ColumnName == StormDataObjectType; }
            }

            public bool IsStormJoinedMasterKey
            {
                get { return ColumnName.StartsWith(StormJoinedMasterKey); }
            }

            public bool IsStormMainObjectKey
            {
                get { return ColumnName == StormMainObjectKey; }
            }

            public string ColumnName { get; set; }

            public List<StorageStructForView.PropStorage> PropStorages { get; set; }

            /// <summary>
            /// Конструктор для вспомогательных колонок StormDataObjectType и StormMainObjectKey.
            /// </summary>
            public ColumnInfo(string columnName)
            {
                ColumnName = columnName;
            }

            public ColumnInfo(string columnName, StorageStructForView.PropStorage propStorage)
            {
                ColumnName = columnName;
                PropStorages = new List<StorageStructForView.PropStorage> { propStorage };
            }

            public ColumnInfo(string columnName, List<StorageStructForView.PropStorage> propStorage)
            {
                ColumnName = columnName;
                PropStorages = propStorage;
            }

            /// <summary>
            /// Конструктор для вспомогательной колонки StormJoinedMasterKey,
            /// котороя необходима для проставления ссылок на мастера
            /// (не только те мастера, которые указаны в представлении).
            /// </summary>
            public ColumnInfo(string columnName, StorageStructForView.PropSource propSource)
            {
                ColumnName = columnName;
                _propSources = new List<StorageStructForView.PropSource> { propSource };
            }
        }
    }
}
