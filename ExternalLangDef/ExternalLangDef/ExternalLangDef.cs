namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    public partial class ExternalLangDef : SQLWhereLanguageDef
    {
        /// <summary>
        /// Внимание, используйте конструктор только в исключительных ситуациях
        /// </summary>
        public ExternalLangDef()
        {
            fieldDataObjectType.SimplificationValue = DataObjectToSimpleValue;
            fieldDataObjectType.UnSimplificationValue = SimpleValueToDataObject;
        }

        static private ExternalLangDef _lngDef = null;
        private static string _objNull = "CONST";

        /// <summary>
        /// Статический ExternalLangDef, используется для получения функций.
        /// </summary>
        public static ExternalLangDef LanguageDef
        {
            get
            {
                if (_lngDef != null)
                {
                    return _lngDef;
                }

                lock (_objNull)
                {
                    return _lngDef ?? (_lngDef = new ExternalLangDef());
                }
            }
        }


        /// <summary>
        /// сервис данных для построения подзапросов
        /// </summary>
        private Business.IDataService m_objDataService;

        /// <summary>
        /// Сервис данных для построения подзапросов. Если не указан, используется DataServiceProvider.DataService
        /// </summary>
        public Business.IDataService DataService
        {
            get
            {
                if (m_objDataService != null)
                {
                    return m_objDataService;
                }
                else
                {
                    m_objDataService = Business.DataServiceProvider.DataService;
                    return m_objDataService;
                }
            }
            set
            {
                m_objDataService = value;
            }
        }


        public string paramTrue { get { return "True"; } }
        public string paramTODAY { get { return "TODAY"; } }

        public string paramYearDIFF { get { return "YearDIFF"; } }
        public string paramMonthDIFF { get { return "MonthDIFF"; } }
        public string paramWeekDIFF { get { return "WeekDIFF"; } }
        public string paramQuarterDIFF { get { return "quarterDIFF"; } }
        public string paramDayDIFF { get { return "DayDIFF"; } }

        ///<summary>
        /// Функция, возвращающая год от DateTime
        ///</summary>
        public string funcYearPart { get { return "YearPart"; } }

        ///<summary>
        /// Функция, возвращающая месяц(число) от DateTime
        ///</summary>
        public string funcMonthPart { get { return "MonthPart"; } }

        ///<summary>
        /// Функция, возвращающая день от DateTime
        ///</summary>
        public string funcDayPart { get { return "DayPart"; } }

        ///<summary>
        /// Функция, возвращающая часы от DateTime
        ///</summary>
        public string funcHHPart { get { return "hhPart"; } }

        ///<summary>
        /// Функция, возвращающая минуты от DateTime
        ///</summary>
        public string funcMIPart { get { return "miPart"; } }

        ///<summary>
        /// Функция, вычисляющая разность дат. Возвращает число, которое зависит от единицы измерения разности.
        ///</summary>
        public string funcDATEDIFF { get { return "DATEDIFF"; } }

        ///<summary>
        /// Функция, возвращающая только дату от DateTime
        ///</summary>
        public string funcOnlyDate { get { return "OnlyDate"; } }

        ///<summary>
        /// Функция, возвращающая день недели числом (1 = Понедельник, ..., 7 = Воскресенье)
        ///</summary>
        public string funcDayOfWeek { get { return "DayOfWeek"; } }

        ///<summary>
        /// Функция, возвращающая день недели числом (0 = Воскресенье, 1 = Понедельник, ...)
        ///</summary>
        public string funcDayOfWeekZeroBased { get { return "DayOfWeekZeroBased"; } }

        ///<summary>
        /// Функция, возвращающая имя текущего пользователя (FriendlyName)
        ///</summary>
        public string funcCurrentUser { get { return "CurrentUser"; } }

        ///<summary>
        /// Функция, возвращающая только время в формате чч:мм:сс от DateTime
        ///</summary>
        public string funcOnlyTime { get { return "OnlyTime"; } }

        /// <summary>
        /// Полный аналог SQL функции dateadd
        /// </summary>
        public string funcDateAdd
        {
            get
            {
                return "DateAdd";
            }
        }

        ///<summary>
        /// Импликация (Если-то)
        ///</summary>
        public string funcImplication { get { return "Implication"; } }

        /// <summary>
        /// Существуют только такие {}, что {}.	Вернет True, если все объекты удовлетворяют условию, в противном случае - False. Условие - только одна функция.
        /// </summary>
        public string funcExistExact { get { return "ExistExact"; } }

        /// <summary>
        /// Существуют все только такие {}, что {} И {} И {} ...	Вернет True, если все объекты удовлетворяют условию, в противном случае - False. В качестве условия могут выступать множество функций, которые автоматически соединятся конъюнкцией. Внимание! Допустимых видов функций только две: "=" (funcEQ) и "СРЕДИ ЗНАЧЕНИЙ()" (FuncIN).
        /// </summary>
        public string funcExistAllExact { get { return "ExistAllExact"; } }

        /// <summary>
        /// Существуют такие {}, что {}	Вернет True, если найдется хотя бы один объект, удовлетворяющий условию, в противном случае - False. Условие - только одна функция.
        /// </summary>
        public string funcExist { get { return "Exist"; } }

        /// <summary>
        /// Существуют такие {} и такие {}, что {}	Вернет True, если найдется хотя бы один объект, удовлетворяющий условию, в противном случае - False. Условие - только одна функция.
        /// Необходимо для сравнения свойств двух детейлов разных композиционных связей.
        /// </summary>
        public string funcExistDetails { get { return "ExistDetails"; } }

        /// <summary>
        /// Существуют все такие {}, что {} И {} И {} ...	Вернет True, если найдется хотя бы один объект, удовлетворяющий условию, в противном случае - False. В качестве условия могут выступать множество функций, которые автоматически соединятся конъюнкцией. Внимание! Допустимых видов функций только две: "=" (funcEQ) и "СРЕДИ ЗНАЧЕНИЙ()" (FuncIN).
        /// </summary>
        public string funcExistAll { get { return "ExistAll"; } }

        /// <summary>
        /// Максимальное значение в детейле с ограничением
        /// </summary>
        public string funcMaxWithLimit { get { return "MAXWithLimit"; } }

        /// <summary>
        /// Минимальное значение в детейле с ограничением
        /// </summary>
        public string funcMinWithLimit { get { return "MINWithLimit"; } }

        /// <summary>
        /// Среднее значение в детейле с ограничением
        /// </summary>
        public string funcAvgWithLimit { get { return "AVGWithLimit"; } }

        /// <summary>
        /// Сумма значений в детейле с ограничением
        /// </summary>
        public string funcSumWithLimit { get { return "SUMWithLimit"; } }

        /// <summary>
        /// Количество значений в детейле с ограничением
        /// </summary>
        public string funcCountWithLimit { get { return "CountWithLimit"; } }

        /// <summary>
        /// Количество
        /// </summary>
        public string funcCount { get { return "Count"; } }

        /// <summary>
        /// Не пусто
        /// </summary>
        public string funcNotIsNull { get { return "NOTISNULL"; } }

        /// <summary>
        /// Количество дней в месяце
        /// </summary>
        public string funcDaysInMonth { get { return "DaysInMonth"; } }

        /// <summary>
        /// Привести строку к верхнему регистру
        /// </summary>
        public string funcToUpper { get { return "ToUpper"; } }

        /// <summary>
        /// Привести строку к нижнему регистру
        /// </summary>
        public string funcToLower { get { return "ToLower"; } }

        /// <summary>
        /// Привести значение к строке
        /// </summary>
        public string funcToChar { get { return "ToChar"; } }

#region Расширение ограничения

        /// <summary>
        /// Метод для получения коллекции первичных ключей объектов в иерерхии, которые не были вычитаны вследствие наложения ограничения на родителей в иерархии.
        /// </summary>
        /// <remarks>
        /// При отображении списка объектов в иерархическом виде на OLV или WOLV, объекты с данными первичными ключами должны отображаться, но быть не доступными для выбора.
        /// </remarks>
        /// <param name="ds">Экземпляр используемого сервиса данных.</param>
        /// <param name="initialLcs">LCS с исходным ограничением на иерерхию объектов.</param>
        /// <param name="view">Представление, используемое для чтения иерархии объетов.</param>
        /// <param name="hierarchicalMasterName">Имя мастерового свойства объектов, которое используется для организации иерархии.</param>
        /// <param name="primaryKeyType">Тип используемых первичных ключей в иерархии объетов.</param>
        /// <returns>Коллекция первичных ключей (в виде строк) вычитанных родительских объектов в иерерхии.</returns>
        public static List<string> GetPrimaryKeysForParentsInHierarchy(IDataService ds, LoadingCustomizationStruct initialLcs, View view, string hierarchicalMasterName, ObjectType primaryKeyType)
        {
            var primaryKeys = new List<string>();
            var iterationPrimaryKeys = new List<string>();

            const int HierarchyStep = 3;
            var ld = SQLWhereLanguageDef.LanguageDef;
            var hierarchyProperties = new string[HierarchyStep]; // Массив имен свойств типа "Иерархия.Иерархия..."
            bool continueSearch;

            // Вычитываем объекты, удовлетворяющие начальному ограничению.
            var data = ds.LoadStringedObjectView('\t', initialLcs);

            int hierarchicalMasterNameIndex = -1;

            // Найдем индекс свойства иерархии.
            hierarchicalMasterNameIndex = initialLcs.ColumnsOrder != null ? Array.IndexOf(initialLcs.ColumnsOrder, hierarchicalMasterName) : view.GetPropertyIndex(hierarchicalMasterName);

            if (hierarchicalMasterNameIndex == -1)
            {
                return null;
            }

            var originalPrimaryKeys = data.Select(item => item.Key.ToString()).ToList();

            foreach (var item in data)
            {
                var value = item.ObjectedData[hierarchicalMasterNameIndex] == null || item.ObjectedData[hierarchicalMasterNameIndex] == DBNull.Value ? null : item.ObjectedData[hierarchicalMasterNameIndex].ToString();

                // Заполним массив первичных ключей.
                if (!string.IsNullOrEmpty(value) && !primaryKeys.Contains(value) && !originalPrimaryKeys.Contains(value))
                {
                    primaryKeys.Add(value);
                    iterationPrimaryKeys.Add(value);
                }
            }

            // Сформируем имена свойств.
            for (var i = 0; i < HierarchyStep; i++)
            {
                hierarchyProperties[i] = GetHierarchyPropertyName(hierarchicalMasterName, i);
            }

            // Подготовим представление.
            var viewForReadParents = new View { DefineClassType = view.DefineClassType };
            for (var i = 0; i < HierarchyStep; i++)
            {
                viewForReadParents.AddProperty(hierarchyProperties[i]);
            }

            var lcs = LoadingCustomizationStruct.GetSimpleStruct(view.DefineClassType, viewForReadParents);

            // Цикл продолжается то тех по пока вычитанная иерархия не будет пустой.
            do
            {
                continueSearch = false;

                var parameters = new ArrayList { new VariableDef(primaryKeyType, SQLWhereLanguageDef.StormMainObjectKey) };
                parameters.AddRange(iterationPrimaryKeys);
                lcs.LimitFunction = ld.GetFunction(ld.funcIN, parameters.ToArray());
                var dataobjects = ds.LoadStringedObjectView('\t', lcs);

                iterationPrimaryKeys.Clear();

                foreach (var dataObject in dataobjects)
                {
                    for (var i = 0; i < HierarchyStep; i++)
                    {
                        var propertyIndex = viewForReadParents.GetPropertyIndex(hierarchyProperties[i]);
                        var s = dataObject.Data.Split(dataObject.Separator);
                        var primaryKey = s[propertyIndex];
                        if (!string.IsNullOrEmpty(primaryKey))
                        {
                            if (!primaryKeys.Contains(primaryKey) && !originalPrimaryKeys.Contains(primaryKey))
                            {
                                primaryKeys.Add(primaryKey);
                                iterationPrimaryKeys.Add(primaryKey);
                            }

                            if (i == HierarchyStep - 1)
                            {
                                continueSearch = true; // Если заполнено самое глубокое свойство, продолжим цикл.
                            }
                        }
                    }
                }
            }
            while (continueSearch);

            return primaryKeys.Count == 0 ? null : primaryKeys;
        }

        /// <summary>
        /// Метод для построения ограничивающей функции для чтения дополнительных объектов по заданному множеству первичных ключей.
        /// </summary>
        /// <param name="primaryKeyType">Тип первичных ключей.</param>
        /// <param name="primaryKeys">Коллекция первичных ключей в виде строк.</param>
        /// <returns>Функция для чтения объектов по заданному множеству первичных ключей.</returns>
        public static Function ExtendLimitFunction(ObjectType primaryKeyType, List<string> primaryKeys)
        {
            if (primaryKeyType == null)
            {
                throw new ArgumentNullException("Type of primary key is not specified", (Exception)null);
            }

            if (primaryKeys == null)
            {
                return null;
            }

            var ld = SQLWhereLanguageDef.LanguageDef;
            var resParameters = new ArrayList { new VariableDef(primaryKeyType, SQLWhereLanguageDef.StormMainObjectKey) };
            resParameters.AddRange(primaryKeys);
            return ld.GetFunction(ld.funcIN, resParameters.ToArray());
        }

        private static string GetHierarchyPropertyName(string hierarchicalMasterName, int count)
        {
            var result = hierarchicalMasterName;
            for (int j = 1; j <= count; j++)
            {
                result += "." + hierarchicalMasterName;
            }
            return result;
        }
#endregion

        private object DataObjectToSimpleValue(object val)
        {
            var dobj = (DataObject)val;
            return new object[] { dobj.GetType().AssemblyQualifiedName, dobj.__PrimaryKey, dobj.GetLoadedProperties() };
        }

        /// <summary>
        /// Делегат для получения типа по его имени в методе SimpleValueToDataObject
        /// </summary>
        public static TypeResolveDelegate ExtraTypeResolver = null;

        private object SimpleValueToDataObject(object val)
        {
            var obj = (object[])val;
            var typeName = (string)obj[0];

            Type type = null;

            try
            {
                type = Type.GetType(typeName, true);
                if (type == null)
                {
                    throw new Exception(string.Format("Невозможно найти тип: {0}", typeName));
                }
            }
            catch (Exception)
            {
                if (ExtraTypeResolver != null)
                {
                    type = ExtraTypeResolver.Invoke(typeName);
                }

                if (type == null)
                {
                    throw;
                }
            }
            
            var dobj = (DataObject)Activator.CreateInstance(type);
            dobj.SetExistObjectPrimaryKey(obj[1]);
            var v = new View { DefineClassType = dobj.GetType() };
            var props = (string[])obj[2];
            foreach (var prop in props)
                v.AddProperty(prop);

            DataService.LoadObject(v, dobj);

            return dobj;
        }

        private ObjectType fieldDetails = new ObjectType("Details", "Зависимые объекты", typeof(DetailArray));
        public ObjectType DetailsType { get { return fieldDetails; } }

        private ObjectType fieldDataObjectType = new ObjectType("DataObject", "Сущность", typeof(DataObject), false);
        public ObjectType DataObjectType { get { return fieldDataObjectType; } }


        private ObjectType fieldDatePartType = new ObjectType("datepart", "Часть даты", typeof(DatePart), false);
        public ObjectType DatePartType { get { return fieldDatePartType; } }


        public override ObjectType GetObjectTypeForNetType(Type type)
        {
            if (type.IsSubclassOf(typeof(DetailArray)))
                return DetailsType;

            return type.IsSubclassOf(typeof(DataObject)) ? DataObjectType : base.GetObjectTypeForNetType(type);
        }

        protected override void InitializeDefs()
        {
            Types.AddObject(DetailsType);
            Types.AddObject(DataObjectType);
            Types.AddObject(DatePartType);
            Functions.AddRange(
                new FunctionDef(
                    base.MaxFuncID + 1,
                    NumericType,
                    "Count",
                    "Количество",
                    "(Количество({0}))",
                    new FunctionParameterDef(fieldDetails)),
                new FunctionDef(
                    base.MaxFuncID + 2,
                    BoolType,
                    "Exist",
                    "Существуют такие ...",
                    "(Существуют такие ({0}) , что {1})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 11,
                    BoolType,
                    "ExistExact",
                    "Существуют только такие ...",
                    "(Существуют только такие({0}) , что {1})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 3,
                    NumericType,
                    funcCountWithLimit,
                    "Количество с ограничением",
                    "(Количество из ({0}) таких, что {1})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 4,
                    BoolType,
                    "=",
                    "=",
                    "({0}={1})",
                    new FunctionParameterDef(fieldDataObjectType),
                    new FunctionParameterDef(fieldDataObjectType)),
                new FunctionDef(
                    base.MaxFuncID + 5,
                    BoolType,
                    "IN",
                    "СРЕДИ ЗНАЧЕНИЙ",
                    "({0} СРЕДИ {{{* ,}}})",
                    new FunctionParameterDef(fieldDataObjectType),
                    new FunctionParameterDef(fieldDataObjectType, true)),
                new FunctionDef(
                    base.MaxFuncID + 6,
                    NumericType,
                    "SUM",
                    "СУММА",
                    "( СУММА В {0} ПО {1} )",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType)),
                new FunctionDef(
                    base.MaxFuncID + 7,
                    NumericType,
                    "AVG",
                    "СРЕДНЕЕ ЗНАЧЕНИЕ",
                    "( СРЕДНЕЕ В {0} ПО {1} )",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType)),
                new FunctionDef(
                    base.MaxFuncID + 8,
                    NumericType,
                    "MAX",
                    "МАКСИМАЛЬНОЕ ЗНАЧЕНИЕ",
                    "( МАКСИМУМ В {0} ПО {1} )",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType)),
                new FunctionDef(
                    base.MaxFuncID + 9,
                    NumericType,
                    "MIN",
                    "МИНИМАЛЬНОЕ ЗНАЧЕНИЕ",
                    "( МИНИМУМ В {0} ПО {1} )",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType)),
                new FunctionDef(base.MaxFuncID + 10, DateTimeType, "TODAY", "СЕГОДНЯ", "( СЕГОДНЯ())"),
                new FunctionDef(
                    base.MaxFuncID + 12,
                    NumericType,
                    funcSumWithLimit,
                    "СУММА С ОГРАНИЧЕНИЕМ",
                    "(СУММА В ({0}) ПО {1} из таких, что {2})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 13,
                    NumericType,
                    funcAvgWithLimit,
                    "СРЕДНЕЕ ЗНАЧЕНИЕ С ОГРАНИЧЕНИЕМ",
                    "(СРЕДНЕЕ В ({0}) ПО {1} из таких, что {2})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 14,
                    NumericType,
                    funcMaxWithLimit,
                    "МАКСИМАЛЬНОЕ ЗНАЧЕНИЕ С ОГРАНИЧЕНИЕМ",
                    "(МАКСИМУМ В ({0}) ПО {1} из таких, что {2})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 15,
                    NumericType,
                    funcMinWithLimit,
                    "МИНИМАЛЬНОЕ ЗНАЧЕНИЕ С ОГРАНИЧЕНИЕМ",
                    "(МИНИМУМ В ({0}) ПО {1} из таких, что {2})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(NumericType),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 16,
                    NumericType,
                    "DATEDIFF",
                    "РАЗНОСТЬ ДАТ",
                    "(РАЗНОСТЬ ДАТ (ед измерения {0}) с даты {1} по дату{2})",
                    new FunctionParameterDef(DatePartType),
                    new FunctionParameterDef(DateTimeType),
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(base.MaxFuncID + 17, DatePartType, "YearDIFF", "ГОД", "ГОД"),
                new FunctionDef(base.MaxFuncID + 18, DatePartType, "quarterDIFF", "Квартал", "Квартал"),
                new FunctionDef(base.MaxFuncID + 19, DatePartType, "MonthDIFF", "МЕСЯЦ", "МЕСЯЦ"),
                new FunctionDef(base.MaxFuncID + 20, DatePartType, "WeekDIFF", "НЕДЕЛЯ", "НЕДЕЛЯ"),
                new FunctionDef(base.MaxFuncID + 21, DatePartType, "DayDIFF", "ДЕНЬ", "ДЕНЬ"),
                new FunctionDef(base.MaxFuncID + 36, DatePartType, "hhDIFF", "ЧАС", "ЧАС"),
                new FunctionDef(base.MaxFuncID + 38, DatePartType, "miDIFF", "Минута", "Минута"),
                new FunctionDef(
                    base.MaxFuncID + 33,
                    NumericType,
                    "YearPart",
                    "ГОД",
                    "ГОД ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 34,
                    NumericType,
                    "MonthPart",
                    "МЕСЯЦ",
                    "МЕСЯЦ ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 35,
                    NumericType,
                    "DayPart",
                    "ДЕНЬ",
                    "ДЕНЬ ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 37,
                    NumericType,
                    "hhPart",
                    "ЧАС",
                    "ЧАС ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 39,
                    NumericType,
                    "miPart",
                    "Минута",
                    "Минута ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 40,
                    NumericType,
                    "DayOfWeek",
                    "День недели",
                    "День недели ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 46,
                    NumericType,
                    funcDayOfWeekZeroBased,
                    "День недели с 0",
                    "День недели с 0({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 41,
                    DateTimeType,
                    "OnlyDate",
                    "Только дата",
                    "Только дата ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 42, StringType, "CurrentUser", "Текущий пользователь", "Текущий пользователь"),
                new FunctionDef(
                    base.MaxFuncID + 43,
                    DateTimeType,
                    "OnlyTime",
                    "Только время",
                    "Только время ({0})",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 44,
                    BoolType,
                    funcImplication,
                    "Если... то...",
                    "Если ({0}), то ({1})",
                    new FunctionParameterDef(BoolType),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 22,
                    BoolType,
                    "ExistAll",
                    "Существуют все такие ...",
                    "(Существуют все такие({0}) , что {* И})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(BoolType, true)),
                new FunctionDef(
                    base.MaxFuncID + 23,
                    BoolType,
                    "ExistAllExact",
                    "Существуют все только такие ...",
                    "(Существуют все только такие({0}) , что {* И})",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(BoolType, true)),
                new FunctionDef(
                    base.MaxFuncID + 30,
                    BoolType,
                    "<>",
                    "<>",
                    "({0}<>{1})",
                    new FunctionParameterDef(fieldDataObjectType),
                    new FunctionParameterDef(fieldDataObjectType)),
                new FunctionDef(
                    base.MaxFuncID + 25,
                    BoolType,
                    "NOTISNULL",
                    "НЕ ПУСТО",
                    "({0} не пусто)",
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    base.MaxFuncID + 26,
                    BoolType,
                    "NOTISNULL",
                    "НЕ ПУСТО",
                    "({0} не пусто)",
                    new FunctionParameterDef(NumericType)),
                new FunctionDef(
                    base.MaxFuncID + 27,
                    BoolType,
                    "NOTISNULL",
                    "НЕ ПУСТО",
                    "({0} не пусто)",
                    new FunctionParameterDef(StringType)),
                new FunctionDef(
                    base.MaxFuncID + 28,
                    BoolType,
                    "NOTISNULL",
                    "НЕ ПУСТО",
                    "({0} не пусто)",
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    base.MaxFuncID + 29,
                    BoolType,
                    "NOTISNULL",
                    "НЕ ПУСТО",
                    "({0} не пусто)",
                    new FunctionParameterDef(GuidType)),
                new FunctionDef(
                    base.MaxFuncID + 31,
                    BoolType,
                    "NOTISNULL",
                    "НЕ ПУСТО",
                    "({0} не пусто)",
                    new FunctionParameterDef(DataObjectType)),
                new FunctionDef(
                    base.MaxFuncID + 32,
                    BoolType,
                    "ISNULL",
                    "НЕ ЗАПОЛНЕНО",
                    "({0} не заполнено)",
                    new FunctionParameterDef(DataObjectType)),
                new FunctionDef(base.MaxFuncID + 24, BoolType, "True", "Истина", "(Истина)"),
                new FunctionDef(
                    base.MaxFuncID + 45,
                    NumericType,
                    funcDaysInMonth,
                    "Дней в месяце",
                    "Дней в {0} месяце {1} года",
                    new FunctionParameterDef(NumericType, "Month", "Месяц"),
                    new FunctionParameterDef(NumericType, "Year", "Год")),
                new FunctionDef(
                    MaxFuncID + 45,
                    BoolType,
                    funcExistDetails,
                    "Cуществуют такие и такие что...",
                    "Существуют такие {0} и такие {1}, что {2}",
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(fieldDetails),
                    new FunctionParameterDef(BoolType)),
                new FunctionDef(
                    MaxFuncID + 46,
                    StringType,
                    funcToUpper,
                    "Преобразовать в верхний регистр",
                    "Преобразовать '{0}' в верхний регистр",
                    new FunctionParameterDef(StringType)),
                new FunctionDef(
                    MaxFuncID + 47,
                    StringType,
                    funcToLower,
                    "Преобразовать в нижний регистр",
                    "Преобразовать '{0}' в нижний регистр",
                    new FunctionParameterDef(StringType)),
                new FunctionDef(
                    MaxFuncID + 48,
                    DateTimeType,
                    funcDateAdd,
                    "Добавить дату",
                    "Добавить (ед измерения '{0}') '{1}' к '{2}'",
                    new FunctionParameterDef(DatePartType),
                    new FunctionParameterDef(NumericType),
                    new FunctionParameterDef(DateTimeType)),
                new FunctionDef(
                    MaxFuncID + 49,
                    StringType,
                    funcToChar,
                    "Привести к строке",
                    "Привести {0} к строке длиной {1}",
                    new FunctionParameterDef(DateTimeType),
                    new FunctionParameterDef(NumericType)),
                    new FunctionDef(
                    MaxFuncID + 50,
                    StringType,
                    funcToChar,
                    "Привести к строке",
                    "Привести дату {0} к строке длиной {1} в формате {2}",
                    new FunctionParameterDef(DateTimeType),
                    new FunctionParameterDef(NumericType),
                    new FunctionParameterDef(NumericType)));

            base.InitializeDefs();
        }

        public override int MaxFuncID { get { return base.MaxFuncID + 35; } }
        private System.Collections.Specialized.StringCollection ChFuncNames = null;
        public override string[] GetExistingVariableNames(ICSSoft.STORMNET.FunctionalLanguage.Function f)
        {
            var al = new ArrayList();
            if (retVars != null) al.AddRange(retVars);
            if (ChFuncNames == null)
            {
                ChFuncNames = new System.Collections.Specialized.StringCollection();
                ChFuncNames.AddRange(new[] { "Count", "SUM", funcCountWithLimit, "ExistExact", "Exist", "AVG", "MAX", "MIN", funcSumWithLimit, funcAvgWithLimit, funcMaxWithLimit, funcMinWithLimit, "ExistAll", "ExistAllExact" });
            }

            if (ChFuncNames.Contains(f.FunctionDef.StringedView))
            {
                var dvd = (DetailVariableDef)f.Parameters[0];
                al.AddRange(dvd.OwnerConnectProp);
            }
            else
            {
                var r1 = base.GetExistingVariableNames(f);
                if (r1 != null)
                    al.AddRange(r1);
            }

            return (string[])al.ToArray(typeof(string));
        }

        /// <summary>
        /// Функция возвращает СУБД-зависимые выражения
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertValue"></param>
        /// <param name="convertIdentifier"></param>
        /// <returns></returns>
        private string DataServiceSwitch(ICSSoft.STORMNET.FunctionalLanguage.Function value, ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString convertValue, ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets convertIdentifier)
        {
            return DataService.FunctionToSql(this, value, convertValue, convertIdentifier);
        }

        public string[] retVars = null;

        public delegate string delegateUserSQLTranslFunction(ICSSoft.STORMNET.FunctionalLanguage.Function value, ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegateConvertValueToQueryValueString convertValue, ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.delegatePutIdentifierToBrackets convertIdentifier);
        public delegateUserSQLTranslFunction UserSQLTranslFunction;

        /// <summary>
        /// Метод для обертки в "case when" IS NULL и NOT IS NULL, добавлен для иключения дублирования кода
        /// </summary>
        /// <param name="param"></param>
        /// <param name="translSwitch"></param>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        private static string WrapNull(object param, string translSwitch, string wrapper)
        {
            String res = translSwitch;

            if ((param is Function) &&
                ((Function)param).FunctionDef.ReturnType.NetCompatibilityType ==
                LanguageDef.BoolType.NetCompatibilityType)
            {
                res = String.Format("(case when {0} then 1 else 0 end)", translSwitch);
            }

            return String.Format("({0} {1} )", res, wrapper);
        }

        protected override string SQLTranslFunction(Function value, delegateConvertValueToQueryValueString convertValue, delegatePutIdentifierToBrackets convertIdentifier)
        {
            if (value.FunctionDef.StringedView == "NOTISNULL")
            {
                string translSwitch = SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier);
                return WrapNull(value.Parameters[0], translSwitch, "IS NOT NULL");
            }
            
            if (value.FunctionDef.StringedView == "ISNULL")
            {
                string translSwitch = SQLTranslSwitch(value.Parameters[0], convertValue, convertIdentifier);
                return WrapNull(value.Parameters[0], translSwitch, "IS NULL");
            }
            
            if (value.FunctionDef.StringedView == "True")
            {
                return "(1=1)";
            }
            
            if (value.FunctionDef.StringedView == "TODAY")
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "YearDIFF" || value.FunctionDef.StringedView == "quarterDIFF"
                     || value.FunctionDef.StringedView == "MonthDIFF" || value.FunctionDef.StringedView == "WeekDIFF"
                     || value.FunctionDef.StringedView == "DayDIFF" || value.FunctionDef.StringedView == "hhDIFF"
                     || value.FunctionDef.StringedView == "miDIFF")
            {
                return value.FunctionDef.StringedView.Substring(0, value.FunctionDef.StringedView.Length - 4);
            }
            
            if (value.FunctionDef.StringedView == "YearPart" || value.FunctionDef.StringedView == "MonthPart"
                     || value.FunctionDef.StringedView == "DayPart")
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "hhPart" || value.FunctionDef.StringedView == "miPart")
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "DayOfWeek")
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcDayOfWeekZeroBased)
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "OnlyDate")
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcDaysInMonth)
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }

            if (value.FunctionDef.StringedView == "CurrentUser")
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "OnlyTime")
            {
                //здесь требуется преобразование из DATASERVICE
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcImplication)
            {
                // не А
                var f1 = GetFunction(funcNOT, value.Parameters[0]);
                // не А или В
                var fres = GetFunction(funcOR, f1, value.Parameters[1]);
                return base.SQLTranslFunction(fres, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "DATEDIFF")
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcExistExact)
            {
                return GetConditionForExistExact(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcExistDetails)
            {
                return GetConditionForExistDetails(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcExistAll)
            {
                return GetConditionForExistAll(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcExistAllExact)
            {
                return GetConditionForExistAllExact(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcExist)
            {
                return GetConditionForExist(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcSumWithLimit
                     || value.FunctionDef.StringedView == funcAvgWithLimit
                     || value.FunctionDef.StringedView == funcMaxWithLimit
                     || value.FunctionDef.StringedView == funcMinWithLimit)
            {
                var lcs = new Business.LoadingCustomizationStruct(null);
                var dvd = (DetailVariableDef)value.Parameters[0];
                lcs.LoadingTypes = new[] { dvd.View.DefineClassType };
                lcs.View = dvd.View.Clone();
                var prevRetVars = retVars;

                retVars = new[] { dvd.ConnectMasterPorp };
                var al = new ArrayList();
                var par = TransformObject(value.Parameters[1], dvd.StringedView, al);
                foreach (string s in al)
                {
                    lcs.View.AddProperty(s);
                }

#region Проставление ограничения

                var boolVariable = value.Parameters[2] as VariableDef;

                if ((boolVariable != null)
                    && (boolVariable.Type.NetCompatibilityType == BoolType.NetCompatibilityType))
                {
                    lcs.LimitFunction = TransformVariables(
                        GetFunction(funcEQ, boolVariable, 1), dvd.StringedView, al);
                }
                else
                {
                    lcs.LimitFunction = TransformVariables(
                        value.Parameters[2] as Function, dvd.StringedView, al);
                }

#endregion

                al.Add(dvd.ConnectMasterPorp);
                retVars = (string[])al.ToArray(typeof(string));

                string Slct =
                    (DataService as ICSSoft.STORMNET.Business.SQLDataService).GenerateSQLSelect(lcs, true)
                                                                             .Replace(
                                                                                 "STORMGENERATEDQUERY",
                                                                                 "SGQ"
                                                                                 + Guid.NewGuid()
                                                                                       .ToString()
                                                                                       .Replace(
                                                                                           "-", string.Empty));
                string CountIdentifier =
                    convertIdentifier(
                        "g" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 29));

                var sumExpression = SQLTranslSwitch(par, convertValue, convertIdentifier);

                var parBoolVariableDef = par as VariableDef;
                if ((parBoolVariableDef != null)
                    && (parBoolVariableDef.Type.NetCompatibilityType == BoolType.NetCompatibilityType))
                {
                    sumExpression = String.Format("CONVERT(INT,{0})", sumExpression);
                }

                string res = string.Empty;
                res =
                    string.Format(
                        "( SELECT {0} From ( " + "SELECT {6}({5}) {0},{1} from ( {4} )pip group by {1} ) "
                        + " ahh where {1} in ({3}",
                        CountIdentifier,
                        convertIdentifier(dvd.ConnectMasterPorp),
                        convertIdentifier(Information.GetClassStorageName(dvd.View.DefineClassType)),
                        convertIdentifier("STORMGENERATEDQUERY") + "."
                        + convertIdentifier(dvd.OwnerConnectProp[0]),
                        Slct,
                        sumExpression,
                        value.FunctionDef.StringedView.Substring(0, 3));
                for (int k = 1; k < dvd.OwnerConnectProp.Length; k++)
                {
                    res += "," + convertIdentifier("STORMGENERATEDQUERY") + "."
                           + convertIdentifier(dvd.OwnerConnectProp[k]);
                }
                res += "))";

                retVars = prevRetVars;
                return res;
            }
            
            if (value.FunctionDef.StringedView == funcCountWithLimit
                     || value.FunctionDef.StringedView == "Count")
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "SUM" || value.FunctionDef.StringedView == "AVG"
                     || value.FunctionDef.StringedView == "MAX"
                     || value.FunctionDef.StringedView == "MIN")
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == "OR")
            {
                bool DetFuncs = true;
                foreach (object obj in value.Parameters)
                {
                    if (obj is FunctionalLanguage.Function)
                    {
                        FunctionalLanguage.Function fnc = obj as FunctionalLanguage.Function;
                        DetFuncs = (fnc.FunctionDef.StringedView == "Exist"
                                    || fnc.FunctionDef.StringedView == "ExistExact"
                                    || fnc.FunctionDef.StringedView == "ExistAll"
                                    || fnc.FunctionDef.StringedView == "ExistAllExact");
                        if (!DetFuncs)
                        {
                            break;
                        }
                    }
                    else
                    {
                        DetFuncs = false;
                        break;
                    }
                }

                //Братчиков 24.10.2008
                DetFuncs = false;
                //Братчиков 24.10.2008

                if (!DetFuncs)
                {
                    return base.SQLTranslFunction(value, convertValue, convertIdentifier);
                }
                else
                {
                    //Удаляет первый и последний символ
                    string[] strs = new string[value.Parameters.Count];
                    string p = string.Empty;
                    for (int i = 0; i < strs.Length; i++)
                    {
                        string s =
                            SQLTranslFunction(
                                value.Parameters[i] as FunctionalLanguage.Function,
                                convertValue,
                                convertIdentifier);
                        if (s.IndexOf("in (") > 0)
                        {
                            p = s.Substring(0, s.IndexOf("in (") + 4);
                            s = s.Substring(s.IndexOf("in (") + 5 - 1);

                            //s = s.Substring(0,s.Length-1);
                        }
                        strs[i] = s;
                    }
                    p = "( " + p + string.Join(" OR ", strs) + " )";
                    return p;
                }
            } // Если начинается на "user_", то обрабатывается делегатом
            else if (value.FunctionDef.StringedView.Length >= 5
                     && value.FunctionDef.StringedView.Substring(0, 5).ToUpper() == "USER_"
                     && UserSQLTranslFunction != null)
            {
                var ret = string.Empty;
                try
                {
                    ret = UserSQLTranslFunction(value, convertValue, convertIdentifier);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Ошибка при обработке пользовательской функции "
                        + value.FunctionDef.StringedView);
                }

                return ret;
            }
            
            if (value.FunctionDef.StringedView == funcToUpper
                     || value.FunctionDef.StringedView == funcToLower)
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcDateAdd)
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }
            
            if (value.FunctionDef.StringedView == funcToChar)
            {
                return DataServiceSwitch(value, convertValue, convertIdentifier);
            }

            return base.SQLTranslFunction(value, convertValue, convertIdentifier);
        }

        //Заслуженный химик

        public FunctionalLanguage.Function TransformVariables(FunctionalLanguage.Function f, string killalias, ArrayList vars)
        {
            return TransformVariables(f, killalias, vars, new List<string>());
        }

        public Function TransformVariables(Function f, string killalias, ArrayList vars, List<string> otherDvds)
        {
            var objj = new object[f.Parameters.Count];
            for (int i = 0; i < objj.Length; i++)
            {
                if (f.Parameters[i] == null)
                    objj[i] = null;
                else if (f.Parameters[i].GetType() == typeof(VariableDef))
                {
                    var vd = (VariableDef)f.Parameters[i];

                    var alien = false;
                    if (vd.StringedView.StartsWith(killalias + "."))
                    {
                        foreach (string dvd in otherDvds)
                        {
                            if (dvd.StartsWith(killalias + ".") && vd.StringedView.StartsWith(dvd + "."))
                            {
                                alien = true;
                                break;
                            }
                        }
                    }

                    if (vd.StringedView.StartsWith(killalias + ".") && !alien)
                    {
                        if (vars != null)
                            vars.Add(vd.StringedView.Substring(killalias.Length + 1));
                        objj[i] = new VariableDef(vd.Type,
                                                                     vd.StringedView.Substring(killalias.Length + 1),
                                                                     vd.Caption);
                    }
                    else
                    {
                        if (vars != null)
                            vars.Add(vd.StringedView);
                        objj[i] = vd;
                    }
                }
                else if (f.Parameters[i] is FunctionalLanguage.Function)
                {
                    objj[i] = TransformVariables(f.Parameters[i] as FunctionalLanguage.Function, killalias, vars, otherDvds);
                }
                else
                {
                    if (f.Parameters[i] is DetailVariableDef)
                        otherDvds.Add((f.Parameters[i] as DetailVariableDef).StringedView);
                    objj[i] = f.Parameters[i];
                }
            }
            return new ICSSoft.STORMNET.FunctionalLanguage.Function(f.FunctionDef, objj);
        }

        public object TransformObject(object o, string killalias, ArrayList vars)
        {
            if (o is FunctionalLanguage.Function)
                return TransformVariables(o as FunctionalLanguage.Function, killalias, vars);
            else if (o.GetType() == typeof(FunctionalLanguage.VariableDef))
            {
                var vd = (FunctionalLanguage.VariableDef)o;
                if (vd.StringedView.StartsWith(killalias + "."))
                {
                    if (vars != null)
                        vars.Add(vd.StringedView.Substring(killalias.Length + 1));
                    return new FunctionalLanguage.VariableDef(vd.Type, vd.StringedView.Substring(killalias.Length + 1), vd.Caption);
                }

                if (vars != null)
                    vars.Add(vd.StringedView);
                return o;
            }

            return o; 
        }

    }
}

