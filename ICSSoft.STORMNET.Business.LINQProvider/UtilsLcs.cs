namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using ICSSoft.STORMNET.Business.LINQProvider.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;

    using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
    using Remotion.Linq.Parsing.Structure;
    using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;
    using Remotion.Linq.Parsing.Structure.NodeTypeProviders;

    /// <summary>
    ///     Вспомогательные методы.
    /// </summary>
    public static class UtilsLcs
    {
        /// <summary>
        /// Шаблон, по которому проверяется, поддерживается ли перевод из Regex в sql-like.
        /// </summary>
        private const string RegexCheckPattern = @"(([^\\](\\\\)*(([\+\?\{\}\(\)\|\[\]])|([^\.\\]\*)|(\\\.\*)|(\\\w)|(\$.+$)))|(^.*[^\\](\\\\)*\^))";

        /// <summary>
        /// Шаблон, по которому проверяются ограничения по переводу из Regex в sql-like, связанные с тем, что для разных диалектов sql пока что недоступно формирование разных шаблонов для like.
        /// </summary>
        private const string RegexDatabaseDependedPattern = @"(([^\\](\\\\)*\\\*)|_|%|(\\\\))";

        /// <summary>
        /// Regex, которым будет проверяться шаблон для регулярки для перевода из Regex в sql-like.
        /// </summary>
        private static Regex RegexChecker = new Regex(RegexCheckPattern);

        /// <summary>
        /// Regex, которым будет дополнительно проверяться шаблон для регулярки для перевода из Regex в sql-like на временные ограничениясвязанные с тем, что для разных диалектов sql пока что недоступно формирование разных шаблонов для like.
        /// </summary>
        private static Regex RegexDatabaseDependedChecker = new Regex(RegexDatabaseDependedPattern);

        /// <summary>
        /// Шаблон, по которому проверяются ограничения по переводу из sql-like в Regex, связанные с тем, что для разных диалектов sql пока что недоступно формирование разных шаблонов для like.
        /// </summary>
        private const string RegexDatabaseDependedPatternBack = @"[%\\]";

        /// <summary>
        /// Regex, которым будет дополнительно проверяться шаблон для регулярки для перевода из sql-like в Regex на временные ограничениясвязанные с тем, что для разных диалектов sql пока что недоступно формирование разных шаблонов для like.
        /// </summary>
        private static readonly Regex RegexDatabaseDependedCheckerBack = new Regex(RegexDatabaseDependedPatternBack);

        /// <summary>
        /// Символы, которые при переводе из sql-like в Regex нужно экранировать.
        /// </summary>
        private static HashSet<string> symbolsToEscape =
            new HashSet<string>() { "{", "}", "?", "+", "(", ")", "[", "]", "|", "^", "$", "." };

        #region Static Fields

        /// <summary>
        ///     The ldef.
        /// </summary>
        private static readonly global::ICSSoft.STORMNET.Windows.Forms.ExternalLangDef ldef = global::ICSSoft.STORMNET.Windows.Forms.ExternalLangDef.LanguageDef;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Проверка актуальных параметров метода на соответствие ожидаемым.
        /// </summary>
        /// <param name="expression">
        /// Выражение-вызов метода.
        /// </param>
        /// <param name="args">
        /// Ожидаемый массив типов аргументов метода.
        /// </param>
        public static void CheckMethodArguments(MethodCallExpression expression, Type[] args)
        {
            if (!ExpressionMethodEquals(expression, expression.Method.Name, args))
            {
                throw new MethodSignatureException(
                    string.Format("Метод {0} не поддерживается с данными аргументами", expression.Method.Name));
            }
        }

        /// <summary>
        /// The create default processor.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="CompoundExpressionTreeProcessor"/>.
        /// </returns>
        public static CompoundExpressionTreeProcessor CreateDefaultProcessor(IExpressionTranformationProvider provider)
        {
            return
                new CompoundExpressionTreeProcessor(
                    new IExpressionTreeProcessor[] { new TransformingExpressionTreeProcessor(provider) });
        }

        /// <summary>
        ///     The create query parser.
        /// </summary>
        /// <returns>
        ///     The <see cref="IQueryParser" />.
        /// </returns>
        public static IQueryParser CreateQueryParser()
        {
            CompoundNodeTypeProvider nodeTypeProvider = ExpressionTreeParser.CreateDefaultNodeTypeProvider();
            var earlyTransformerRegistry = new ExpressionTransformerRegistry();
            earlyTransformerRegistry.Register(new DateTimeEarlyExpressionTransformer());
            CompoundExpressionTreeProcessor processor = CreateDefaultProcessor(earlyTransformerRegistry);
            var expressionTreeParser = new ExpressionTreeParser(nodeTypeProvider, processor);
            var queryParser = new QueryParser(expressionTreeParser);
            return queryParser;
        }

        /// <summary>
        /// Проверка равен ли метод из expression заданному параметрами name и args.
        /// </summary>
        /// <param name="expression">
        /// Expression.
        /// </param>
        /// <param name="name">
        /// имя метода.
        /// </param>
        /// <param name="args">
        /// массив типов аргументов метода.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ExpressionMethodEquals(MethodCallExpression expression, string name, Type[] args)
        {
            if (expression.Object == null)
            {
                return expression.Method.Name.Equals(name);
            }

            MethodInfo supportedMethod = expression.Object.Type.GetMethod(name, args);
            return expression.Method.Equals(supportedMethod);
        }

        /// <summary>
        /// The get compare with null function.
        /// </summary>
        /// <param name="exprType">
        /// The expr type.
        /// </param>
        /// <param name="par">
        /// The par.
        /// </param>
        /// <returns>
        /// The <see cref="Function"/>.
        /// </returns>
        public static Function GetCompareWithNullFunction(ExpressionType exprType, object par)
        {
            if (exprType == ExpressionType.NotEqual)
            {
                return ldef.GetFunction(ldef.funcNotIsNull, par);
            }

            if (exprType == ExpressionType.Equal)
            {
                return ldef.GetFunction(ldef.funcIsNull, par);
            }

            // В оригинальном linq если сравнивать Поле1 > null, то ошибки не будет и вернётся false.
            var acceptableForNull = new List<ExpressionType>()
                                                     {
                                                         ExpressionType.GreaterThan,
                                                         ExpressionType.GreaterThanOrEqual,
                                                         ExpressionType.LessThan,
                                                         ExpressionType.LessThanOrEqual,
                                                     };
            if (acceptableForNull.Contains(exprType))
            {
                return GetFalseFunc();
            }

            throw new NotSupportedException("Неподдерживаемая операция с null в ограничении.");
        }

        /// <summary>
        ///     The get false func.
        /// </summary>
        /// <returns>
        ///     The <see cref="Function" />.
        /// </returns>
        public static Function GetFalseFunc()
        {
            return ldef.GetFunction(ldef.funcNOT, GetTrueFunc());
        }

        /// <summary>
        /// The get func name by expression type.
        /// </summary>
        /// <param name="exprType">
        /// The expr type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static string GetFuncNameByExpressionType(ExpressionType exprType)
        {
            switch (exprType)
            {
                case ExpressionType.Not:
                    return ldef.funcNOT;
                case ExpressionType.Equal:
                    return ldef.funcEQ;
                case ExpressionType.GreaterThan:
                    return ldef.funcG;
                case ExpressionType.GreaterThanOrEqual:
                    return ldef.funcGEQ;
                case ExpressionType.LessThan:
                    return ldef.funcL;
                case ExpressionType.LessThanOrEqual:
                    return ldef.funcLEQ;
                case ExpressionType.NotEqual:
                    return ldef.funcNEQ;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return ldef.funcAND;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return ldef.funcOR;
                case ExpressionType.Multiply:
                    return ldef.funcSub;
                case ExpressionType.Subtract:
                    return ldef.funcMinus;
                case ExpressionType.Add:
                    return ldef.funcPlus;
                case ExpressionType.Divide:
                    return ldef.funcDiv;

                // TODO: Разобраться с константами funcSub и funcMinus
                default:
                    throw new Exception("Нет соответствия типов");
            }
        }

        /// <summary>
        /// The get function and param.
        /// </summary>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="func">
        /// The func.
        /// </param>
        /// <param name="stringVal">
        /// The string val.
        /// </param>
        public static void GetFunctionAndParam(object p1, object p2, out Function func, out string stringVal)
        {
            if (p1 is Function)
            {
                func = p1 as Function;
                stringVal = p2 as string;
            }
            else
            {
                func = p2 as Function;
                stringVal = p1 as string;
            }
        }

        /// <summary>
        /// возвращает lcs функцию по имени функции C#.
        /// </summary>
        /// <param name="name">
        /// имя функции из C#.
        /// </param>
        /// <returns>
        /// функция lcs.
        /// </returns>
        /// <exception cref="MethodSignatureException">
        /// в lcs нет аналога этой функции.
        /// </exception>
        public static string GetFunctionByName(string name)
        {
            switch (name)
            {
                case "Day":
                    return ldef.funcDayPart;
                case "Hour":
                    return ldef.funcHHPart;
                case "Minute":
                    return ldef.funcMIPart;
                case "Year":
                    return ldef.funcYearPart;
                case "Month":
                    return ldef.funcMonthPart;
                case "Date":
                    return ldef.funcOnlyDate;
                case "TimeOfDay":
                    return ldef.funcOnlyTime;
                case "DayOfWeek":
                    return ldef.funcDayOfWeekZeroBased;
                case "Second":
                case "Millisecond":
                case "Ticks":
                case "DayOfYear":
                    throw new MethodSignatureException(string.Format("Функция {0} не поддерживается LCS", name));
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Получение шаблона для функции Like, куда будет вставлена строка поиска, по имени функции.
        /// </summary>
        /// <param name="name"> Имя функции. </param>
        /// <returns> Шаблон, соответствующий имени функции. </returns>
        /// <exception cref="Exception"> Если передано имя функции, для которой неизвестен шаблон, то пройдёт исключение. </exception>
        public static string GetLikePatternByFunctionName(string name)
        {
            switch (name)
            {
                case "Contains":
                    return "%$%";
                case "StartsWith":
                    return "$%";
                case "EndsWith":
                    return "%$";
                default:
                    throw new Exception(string.Format("Не найден шаблон для Like для функции {0}", name));
            }
        }

        /// <summary>
        /// Минимальная проверка шаблона sql-like: то, что в нём нет символов, которые пока нельзя перевести в Regex и обратно.
        /// </summary>
        /// <param name="sqlString"> Шаблон поиска в sql-like. </param>
        public static void MinimalSqlCheck(string sqlString)
        {
            // В конце проверяем временные ограничения (их появление связано с тем, что разные СУБД по-разному интерпретируют шаблоны, а это пока не поддерживается).
            if (RegexDatabaseDependedCheckerBack.IsMatch(sqlString))
            {
                throw new NotSupportedException(@"Временно не поддерживается использование в шаблоне funcLike: %, \.");
            }
        }

        /// <summary>
        /// Метод преобразует шаблон поиска в стиле sql-like в шаблон в стиле Regex.
        /// Как будет переводиться (*, а не %, поскольку так переводит SQLDataService):
        /// *abc* => abc
        /// abc* => ^abc
        /// *abc => abc$
        /// abc*d => abc.*d
        /// ab_c => ab.c.
        /// </summary>
        /// <param name="sqlString"> Строка, которая была в шаблоне поиска для sql-like. </param>
        /// <returns> Сформированный шаблон поиска для Regex. </returns>
        public static string ConvertSqlToRegex(string sqlString)
        {
            if (string.IsNullOrEmpty(sqlString))
            {
                return sqlString;
            }

            MinimalSqlCheck(sqlString);

            // Экранируем символы.
            sqlString = symbolsToEscape.Aggregate(sqlString, (current, escapeSymbol) => current.Replace(escapeSymbol, @"\" + escapeSymbol));

            // Обработка начала.
            sqlString = sqlString.StartsWith("*") ? sqlString.Remove(0, 1) : "^" + sqlString;

            // Обработка конца.
            sqlString = sqlString.EndsWith("*") ? sqlString.Remove(sqlString.Length - 1, 1) : sqlString + "$";

            // * => .*; _ => .
            sqlString = sqlString.Replace("*", ".*");
            sqlString = sqlString.Replace("_", ".");

            return sqlString;
        }

        /// <summary>
        /// Минимальная проверка шаблона регулярных выражений: то, что он вообще валиден, и то, что в нём нет символов, которые нельзя перевести в sql-like.
        /// </summary>
        /// <param name="regexString"> Шаблон регулярного выражения. </param>
        public static void MinimalRegexCheck(string regexString)
        {
            // Сначала проверяем валидность исходной строки шаблона.
            try
            {
                Regex.IsMatch(string.Empty, regexString);
            }
            catch
            {
                throw new NotSupportedRegexException("Регулярное выражение не прошло проверку на корректность.");
            }

            // Потом проверяем на наличие неподдерживаемых комбинаций.
            if (RegexChecker.IsMatch(regexString))
            {
                throw new NotSupportedRegexException(
                    "Из спецсимволов поддерживаются только следующие комбинации: ^, $, ., .*, [], [^].");
            }

            // В конце проверяем временные ограничения (их появление связано с тем, что разные СУБД по-разному интерпретируют шаблоны, а это пока не поддерживается).
            if (RegexDatabaseDependedChecker.IsMatch(regexString))
            {
                throw new NotSupportedRegexException(
                    @"Временно не поддерживается использование: *, %, _, \.");
            }
        }

        /// <summary>
        /// Преобразование из шаблона поиска Regex в шаблон поиска для sql-функции like.
        /// Как будет переводиться (*, а не %, поскольку так переводит SQLDataService):
        /// abc => *abc*
        /// ^abc => abc*
        /// abc$ => *abc
        /// abc.*d => abc*d
        /// ab.c => ab_c
        /// Порядок замены:
        /// 1. Экранируются _. //TODO: временно не делается
        /// 2. .* (точка не экранирована, это проверилось ранее)
        /// 3. . (если точка не экранирована)
        /// 4. ^, $ (если они не экранированы) и их отсутствие.
        /// 5. Снимается экранирование со всех символов, кроме *, [, ]. //TODO: временно экранирование снимается со всех символов, поскольку в разных версиях sql экранирование для шаблона like проходит по-разному.
        /// </summary>
        /// <param name="regexString"> Шаблон поиска в нотации для regex. </param>
        /// <returns> Шаблон поиска в нотации для sql-функции like. </returns>
        public static string ConvertRegexToSql(string regexString)
        {
            const string TempNotAccessibleString = "{%%%}";
            MinimalRegexCheck(regexString);

            // Экранируются _.
            // regexString = regexString.Replace("_", @"\_"); //TODO: временно не делается

            // .* (точка не экранирована, это проверилось ранее)
            regexString = regexString.Replace(".*", "*");

            // . (если точка не экранирована)
            regexString = regexString.Replace(@"\.", TempNotAccessibleString);
            regexString = regexString.Replace('.', '_');
            regexString = regexString.Replace(TempNotAccessibleString, ".");

            regexString = regexString.Replace(@"\\", TempNotAccessibleString);

            // ^, $ (если они не экранированы) и их отсутствие.
            regexString = "*" + regexString + "*";
            if (regexString.StartsWith("*^"))
            {
                regexString = regexString.Remove(0, 2);
            }

            if (regexString.EndsWith("$*") && !regexString.EndsWith(@"\$*"))
            {
                regexString = regexString.Remove(regexString.Length - 2, 2);
            }

            // Снимается экранирование со всех символов, кроме *, [, ], _.
            // regexString = regexString.Replace(@"\*", "{*}");
            // regexString = regexString.Replace(@"\[", "{[}");
            // regexString = regexString.Replace(@"\]", "{]}");
            // regexString = regexString.Replace(@"\_", "{_}");
            regexString = regexString.Replace(@"\", string.Empty); // TODO: временно экранирование снимается со всех символов
            // regexString = regexString.Replace("{_}", @"\_");
            // regexString = regexString.Replace("{]}", @"\]");
            // regexString = regexString.Replace("{[}", @"\[");
            // regexString = regexString.Replace("{*}", @"\*");

            regexString = regexString.Replace(TempNotAccessibleString, @"\"); // TODO: временно экранирование снимается со всех символов

            return regexString;
        }

        /// <summary>
        /// The get object property value.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/>.
        /// </returns>
        public static MemberInfo GetObjectPropertyValue(object obj, string propertyName)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propertyName);
            if (prop != null)
            {
                return (MemberInfo)prop.GetValue(obj, null);
            }

            return null;
        }

        /// <summary>
        /// The get param binary func.
        /// </summary>
        /// <param name="exprType">
        /// The expr type.
        /// </param>
        /// <param name="par1">
        /// The par 1.
        /// </param>
        /// <param name="par2">
        /// The par 2.
        /// </param>
        /// <returns>
        /// The <see cref="Function"/>.
        /// </returns>
        /// <exception cref="MethodSignatureException">
        /// </exception>
        public static Function GetParamBinaryFunc(ExpressionType exprType, object par2, object par1)
        {
            if (par1 == null && par2 == null)
            {
                switch (exprType)
                {
                    case ExpressionType.Equal:
                        return GetTrueFunc();
                    case ExpressionType.NotEqual:
                        return GetFalseFunc();
                    default:
                        throw new NotSupportedException("Попытка провести над null и null неподдерживаемую операцию.");
                }
            }

            // string.Empty приравниваем к null, т.к. DataService никогда не хранит пустые строки в БД.
            if (par1 == null || par1 as string == string.Empty)
            {
                return GetCompareWithNullFunction(exprType, par2);
            }

            // string.Empty приравниваем к null, т.к. DataService никогда не хранит пустые строки в БД.
            if (par2 == null || par2 as string == string.Empty)
            {
                return GetCompareWithNullFunction(exprType, par1);
            }

            // Если выражения вида substring() == "string", то на выходе будет like - одна функция.
            if ((IsExactFunction(par1, ldef.funcLike) && !(par2 is Function))
                || (IsExactFunction(par2, ldef.funcLike) && !(par1 is Function)))
            {
                Function likeFunc;
                string stringParam;
                GetFunctionAndParam(par1, par2, out likeFunc, out stringParam);
                if (stringParam == null)
                {
                    throw new MethodSignatureException("Сравнивайте результат Substring только с константой");
                }

                likeFunc.Parameters[1] = string.Format((string)likeFunc.Parameters[1], stringParam);
                return likeFunc;
            }

            return ldef.GetFunction(GetFuncNameByExpressionType(exprType), par1, par2);
        }

        /// <summary>
        /// Попытка скомпилировать бинарное выражение и вместо сложного выражения записать в lcs уже простую константу.
        /// </summary>
        /// <param name="exprType">Тип выражения (Add, Subtract, Multiply, Divide).</param>
        /// <param name="par2"></param>
        /// <param name="par1"></param>
        /// <returns></returns>
        public static object TryExecuteBinaryOberation(ExpressionType exprType, object par2, object par1)
        {
            try
            {
                if (exprType == ExpressionType.Add && par1 is string && par2 is string)
                {
                    // Linq не хочет конструировать "string1" + "string2", поэтому для него отдельная обработка.
                    return (string)par1 + (string)par2;
                }

                // Формируем выражение с константами.
                var binaryExpression = Expression.MakeBinary(exprType, Expression.Constant(par1), Expression.Constant(par2));

                // Компилируем выражение.
                object param = Expression.Lambda(binaryExpression).Compile().DynamicInvoke();
                return param;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The get sql where pattern.
        /// </summary>
        /// <param name="startIndex">
        /// The start index.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSqlWherePattern(int startIndex, int count)
        {
            const string str = "{0}";
            string returnstr = str.PadLeft(str.Length + startIndex, '_');
            if (count < 0)
            {
                return returnstr;
            }

            return returnstr + "%";
        }

        /// <summary>
        ///     The get true func.
        /// </summary>
        /// <returns>
        ///     The <see cref="Function" />.
        /// </returns>
        public static Function GetTrueFunc()
        {
            return ldef.GetFunction(ldef.paramTrue);
        }

        /// <summary>
        /// Проверяет, что объект - функция с определенным именем.
        /// </summary>
        /// <param name="f">
        /// </param>
        /// <param name="fName">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsExactFunction(object f, string fName)
        {
            var func = f as Function;
            if (func == null)
            {
                return false;
            }

            return func.FunctionDef.StringedView == fName;
        }

        /// <summary>
        /// Проверить, есть ли требуемое свойство в представлении.
        /// Если представление динамическое, то при отсутствии свойства оно добавляется.
        /// </summary>
        /// <param name="view">Текущее сформированное представление.</param>
        /// <param name="propertyName">Имя свойства, которое ищется в представлении.</param>
        /// <param name="viewIsDynamic">Является ли представление динамическим (если да, то в него можно добавлять недостающие свойства).</param>
        public static void AddPropertyToView(View view, string propertyName, bool viewIsDynamic)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            // Если свойство содержит точки, то нужно проверить по всем уровням.
            var propertyNames = propertyName.Split('.');
            var propertyNamesCount = propertyNames.Count();
            propertyName = propertyNames[0];
            var counter = 1;

            while (counter <= propertyNamesCount)
            {
                // Проверяем, есть ли в представлении такое свойство или любое из свойств мастера, если это мастер
                // (например: в случае свойства Порода нас устроит, если в представлении уже есть Порода.Название)
                if (!view.Properties.Any(p => p.Name == propertyName || p.Name.StartsWith(propertyName + ".")))
                {
                    if (viewIsDynamic)
                    {
                        view.AddProperty(propertyName);
                        string agregateProperty = Information.GetAgregatePropertyName(view.DefineClassType);
                        if (!string.IsNullOrEmpty(agregateProperty) && !view.Properties.Any(p => p.Name == agregateProperty || p.Name.StartsWith(agregateProperty + ".")))
                        {
                            view.AddProperty(agregateProperty);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format("Представление \"{0}\" не содержит свойства \"{1}\"", view.Name, propertyName));
                    }
                }

                if (counter < propertyNamesCount)
                {
                    propertyName += "." + propertyNames[counter];
                }

                counter++;
            }
        }

        /// <summary>
        /// Проверить, есть ли требуемый мастер в представлении.
        /// Если представление динамическое, то при отсутствии мастера он добавляется.
        /// </summary>
        /// <param name="view">Представление.</param>
        /// <param name="masterName">Имя мастера.</param>
        /// <param name="viewIsDynamic">Является ли представление динамическим.</param>
        public static void AddMasterToView(View view, string masterName, bool viewIsDynamic)
        {
            // Нужно проверить содержание в представлении только самого мастера.
            masterName = masterName.Split('.')[0];

            // "__PrimaryKey" - не мастер.
            if ("__PrimaryKey".Equals(masterName))
            {
                return;
            }

            if (!view.Properties.Any(x => x.Name.Equals(masterName)))
            {
                if (viewIsDynamic)
                {
                    view.AddProperty(masterName);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format("Представление \"{0}\" не содержит мастера \"{1}\"", view.Name, masterName));
                }
            }
        }

        /// <summary>
        /// Возвращает представление детейла из представления агрегатора.
        /// Если представление динамическое и в нем не оказалось нужного детейла, то
        /// добавляет его, задавая в качестве представления пустое представление (которое будет заполняться
        /// при разборе подзапросов к данному детейлу).
        /// </summary>
        /// <param name="view">Представление агрегатора.</param>
        /// <param name="detailName">Имя детейла.</param>
        /// <param name="viewIsDynamic">Является ли представление динамическим.</param>
        /// <returns>Представление детейла.</returns>
        public static View AddDetailViewToView(View view, string detailName, bool viewIsDynamic)
        {
            if (!view.Details.Any(x => x.Name.Equals(detailName)))
            {
                if (viewIsDynamic)
                {
                    var detailType = Information.GetItemType(view.DefineClassType, detailName);
                    var detailView = new View
                    {
                        DefineClassType = detailType,
                        Name = GetNameForDynamicView(detailType),
                    };
                    view.AddDetailInView(detailName, detailView, true, string.Empty, false, string.Empty, null);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format("Представление \"{0}\" не содержит детейла \"{1}\"", view.Name, detailName));
                }
            }

            return view.GetDetail(detailName).View;
        }

        /// <summary>
        /// Формируем динамическое имя для представления на основе типа.
        /// </summary>
        /// <param name="dataObjectType">Тип объекта данных, для представления которого формируется имя.</param>
        /// <returns>Сформированное имя.</returns>
        public static string GetNameForDynamicView(Type dataObjectType)
        {
            return string.Format("DynamicViewFor{0}", dataObjectType.FullName);
        }

        #endregion
    }
}