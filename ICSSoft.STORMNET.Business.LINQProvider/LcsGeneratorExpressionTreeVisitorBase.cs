namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using ICSSoft.STORMNET.Business.LINQProvider.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;
    using ICSSoft.STORMNET.Windows.Forms;

    using Remotion.Linq.Clauses.Expressions;
    using Remotion.Linq.Clauses.ExpressionTreeVisitors;
    using Remotion.Linq.Parsing;
#if NETFX_45
    using Microsoft.Spatial;
#endif

    /// <summary>
    /// Visitor, который обходит распарсенноое дерево
    /// </summary>
    public abstract class LcsGeneratorExpressionTreeVisitorBase : ThrowingExpressionTreeVisitor, IExpressionTreeVisitor
    {
        /// <summary>
        /// Описание языка для построения lcs.
        /// </summary>
        protected readonly global::ICSSoft.STORMNET.Windows.Forms.ExternalLangDef _ldef = global::ICSSoft.STORMNET.Windows.Forms.ExternalLangDef.LanguageDef;

        /// <summary>
        /// Элемент для организации стека.
        /// </summary>
        protected TreeVisitorStacksHolder _stacksHolder;

        /// <summary>
        /// Представления мастеров, нужные для получения их детейлов (в случае динамических представлений null)..
        /// </summary>
        private IEnumerable<View> _resolvingViews;

        /// <summary>
        /// Сохраняет предыдущий посещенный член при многоуровневом вызове
        /// </summary>
        private MemberExpression _previosVisitedMemberExpression; // ToDo: ликвидировать, использовать для хранения состояния только стеки

        private bool _dataobjectmember;

        private View _view;

        /// <summary>
        /// Является ли представление динамическим  (формируется во время разбора выражения).
        /// </summary>
        private bool _viewIsDynamic;

        /// <summary>
        /// Конструктор класса <see cref="LcsGeneratorExpressionTreeVisitorCommon"/>.
        /// </summary>
        /// <param name="stacksHolder"> Элемент, организующий стек. </param>
        public LcsGeneratorExpressionTreeVisitorBase(TreeVisitorStacksHolder stacksHolder)
        {
            _stacksHolder = stacksHolder;
        }

        /// <summary>
        /// Конструктор класса <see cref="LcsGeneratorExpressionTreeVisitorCommon"/>.
        /// </summary>
        /// <param name="viewIsDynamic"> Является ли представление динамическим (формируется во время разбора выражения). </param>
        /// <param name="view"> Представление (в случае динамического представления может быть передано пустое представление, у которого задан только тип). </param>
        /// <param name="resolvingViews"> Представления мастеров, нужные для получения их детейлов (в случае динамических представлений null). </param>
        protected LcsGeneratorExpressionTreeVisitorBase(
            bool viewIsDynamic,
            View view,
            IEnumerable<View> resolvingViews)
        {
            _view = view;
            _viewIsDynamic = viewIsDynamic;
            _resolvingViews = resolvingViews;
            _stacksHolder = new TreeVisitorStacksHolder();
        }

        /// <summary>
        /// Получение сформированного lcs (берётся из стека).
        /// </summary>
        /// <returns> Сформированное lcs. </returns>
        public Function GetLcsExpression()
        {
            try
            {
                return _stacksHolder.PopFunction();
            }
            catch (Exception e)
            {
                throw new Exception("Неправильно сформированы стеки функций и параметров при разборе linq", e);
            }
        }

        /// <summary>
        /// Получить функцию ограничения для <see cref="LoadingCustomizationStruct"/>.
        /// </summary>
        /// <param name="linqExpression">Linq-выражение, для которого необходимо получить функцию ограничения.</param>
        /// <returns>Функция ограничения для <see cref="LoadingCustomizationStruct"/>.</returns>
        public abstract Function GetLcsExpression(Expression linqExpression);

        /// <summary>
        /// Создать экземпляр visitor'а для обработки запроса linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.
        /// </summary>
        /// <param name="viewIsDynamic">Динамически создавать представление.</param>
        /// <param name="view">Представление, если было указано.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns>Экземпляр visitor'а для обработки запроса linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.</returns>
        public abstract IQueryModelVisitor GetQueryModelVisitor(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews);

        /// <summary>
        /// Обход унарной операции в дереве выражения.
        /// </summary>
        /// <param name="expression"> Элемент, соответствующий унарной операции. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры. </returns>
        protected override Expression VisitUnaryExpression(UnaryExpression expression)
        {
            VisitExpression(expression.Operand);

            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    var func = _stacksHolder.PopFunction();
                    string ldefFunc;

                    // Если была проверка IsNull, то подменим ее на NotIsNull и наоборот.
                    if (func.FunctionDef.StringedView == _ldef.funcIsNull || func.FunctionDef.StringedView == _ldef.funcNotIsNull)
                    {
                        ldefFunc = func.FunctionDef.StringedView == _ldef.funcIsNull
                                       ? _ldef.funcNotIsNull
                                       : _ldef.funcIsNull;
                        func = _ldef.GetFunction(ldefFunc, func.Parameters[0]);
                    }
                    else
                    {
                        ldefFunc = UtilsLcs.GetFuncNameByExpressionType(expression.NodeType);
                        func = _ldef.GetFunction(ldefFunc, func);
                    }

                    _stacksHolder.PushFunction(func);
                    break;
            }

            return expression;
        }

        /// <summary>
        /// Обход подзапроса в дереве выражения.
        /// </summary>
        /// <param name="expression"> Выражение-подзапрос. </param>
        /// <returns> Данный метод реализован только у потомков. </returns>
        protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
        {
            throw new NotImplementedException("Метод VisitSubQueryExpression доступен только в потомках.");
        }

        /// <summary>
        /// Вспомогательный метод, содержащий общую обработку VisitSubQueryExpression, используемую в потомках.
        /// </summary>
        /// <param name="expression"> Выражение, содержащее подзапрос. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры. </returns>
        protected Expression VisitSubQueryExpressionHelper(SubQueryExpression expression)
        {
            var fromExpr = expression.QueryModel.MainFromClause.FromExpression;

            var subQueryExpression = fromExpr as SubQueryExpression;
            if (subQueryExpression != null)
            {
                fromExpr = subQueryExpression.QueryModel.MainFromClause.FromExpression;
            }

            if (fromExpr.NodeType == ExpressionType.MemberAccess)
            {
                var member = UtilsLcs.GetObjectPropertyValue(fromExpr, "Member");

                var ownerConnectProp = UtilsDetail.GetOwnerConnectProp(fromExpr);

                View parentView = _view;

                if (ownerConnectProp[0] != SQLWhereLanguageDef.StormMainObjectKey)
                {
                    // Подразумевается, что это возможно только в случае нединамического представления,
                    // так как при динамическом не задаются представления мастеров (resolvingViews),
                    // и обращение к детейлам других мастеров невозможно.
                    // Поэтому можно не проверять наличие мастера в представлении.
                    string masterName = parentView.GetMaster(ownerConnectProp[0]).MasterName;
                    var type = Information.GetPropertyType(parentView.DefineClassType, masterName);
                    parentView = UtilsDetail.GetResolvedView(type, _resolvingViews);
                }

                View detailView = UtilsLcs.AddDetailViewToView(parentView, member.Name, _viewIsDynamic);

                var innerLcs = GetQueryModelVisitor(_viewIsDynamic, detailView, _resolvingViews).GenerateLcs(expression.QueryModel);
                var lf = innerLcs.LimitFunction as object ?? true;

                string masterProp = Information.GetAgregatePropertyName(detailView.DefineClassType);
                var dvd = new DetailVariableDef
                {
                    ConnectMasterPorp = masterProp,
                    OwnerConnectProp = ownerConnectProp,
                    View = detailView,
                    Type = _ldef.GetObjectType("Details")
                };

                switch (innerLcs.ReturnType)
                {
                    case LcsReturnType.Any:
                        _stacksHolder.PushFunction(_ldef.GetFunction(_ldef.funcExist, dvd, lf));
                        break;
                    case LcsReturnType.All:
                        _stacksHolder.PushFunction(_ldef.GetFunction(_ldef.funcExistExact, dvd, lf));
                        break;
                }

                return expression;
            }

            // Подзапрос к коллекции
            if (expression.QueryModel.ResultOperators.Count > 1)
            {
                throw new NotSupportedException("Поддерживается только подзапрос с одним оператором");
            }

            return null;
        }

        /// <summary>
        /// Обработка выражения вида IIF(Condition, IfTrue, IfFalse).
        /// </summary>
        /// <param name="expression">Текущее рассматриваемое выражение.</param>
        /// <returns>Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры.</returns>
        protected override Expression VisitConditionalExpression(ConditionalExpression expression)
        {
            Expression iftrue = expression.IfTrue;
            Expression iffalse = expression.IfFalse;

            if (iftrue.NodeType == ExpressionType.Constant
                && ((ConstantExpression)iftrue).Value == null)
            {
                /*
                 * Из OData выражения Contains(Поле, Значение) приходят как "IIF(Поле == null || Значение == null, null, Convert(Поле.Contains(Значение))) == true".
                 * Поэтому опознаём такую ситуацию и оставляем только часть "Convert(Поле.Contains(Значение))".
                 */

                VisitExpression(iffalse);
                return expression;
            }

            return base.VisitConditionalExpression(expression);
        }

        /// <summary>
        /// Обход бинарной операции в дереве выражения.
        /// </summary>
        /// <param name="expression"> Элемент, соответствующий бинарной операции. </param>
        /// <returns>Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры.</returns>
        protected override Expression VisitBinaryExpression(BinaryExpression expression)
        {
            Expression boolRightExpression;
            ConstantExpression boolRightConstantExpression;
            Expression convertOperand;
            if (expression.NodeType == ExpressionType.Equal
                && ((expression.Right.NodeType == ExpressionType.Constant
                        && expression.Right.Type == typeof(bool)
                        && (bool)((ConstantExpression)expression.Right).Value)
                     || ((boolRightExpression = expression.Right).Type == typeof(bool?)
                        && ((boolRightExpression.NodeType == ExpressionType.Convert
                                && (convertOperand = ((UnaryExpression)boolRightExpression).Operand) != null
                                && convertOperand.NodeType == ExpressionType.Constant
                                && convertOperand.Type == typeof(bool)
                                && (bool)((ConstantExpression)convertOperand).Value)
                            || (boolRightExpression.NodeType == ExpressionType.Constant
                                && (boolRightConstantExpression = (ConstantExpression)boolRightExpression).Type == typeof(bool?)
                                && ((bool?)boolRightConstantExpression.Value).HasValue
                                && ((bool?)boolRightConstantExpression.Value).Value)))))
            {
                // Заменяем ситуацию вида: "o.BoolField == true" на простую проверку "o.BoolField".
                VisitExpression(expression.Left);
                return expression;
            }

            // Если только один из операндов == Convert
            if (expression.Left.NodeType == ExpressionType.Convert ^ expression.Right.NodeType == ExpressionType.Convert)
            {
                // Достанем тип операнда
                var operandType = expression.Left is UnaryExpression
                                      ? ((UnaryExpression)expression.Left).Operand.Type
                                      : expression.Right is UnaryExpression
                                            ? ((UnaryExpression)expression.Right).Operand.Type
                                            : null;

                // Справедливо только для несистемных enum
                if (operandType != null && operandType.Namespace != null
                    && !operandType.Namespace.Equals("System", StringComparison.OrdinalIgnoreCase)
                    && operandType.IsSubclassOf(typeof(Enum)))
                {
                    throw new NotSupportedException(
                        "Метод Convert() не поддерживается для enum. Используйте в expression переменную, в которой хранится необходимый enum.");
                }
            }

            Expression rightExpression = CheckOnFuncDayOfWeekZeroBased(expression.Left, expression.Right);
            Expression leftExpression = CheckOnFuncDayOfWeekZeroBased(expression.Right, expression.Left);

            VisitExpression(leftExpression);
            _dataobjectmember = false;
            VisitExpression(rightExpression);
            _dataobjectmember = false;

            // In production code, handle this via lookup tables.
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    // Если имеем дело со сравнением строк через CompareTo
                    if (leftExpression.NodeType == ExpressionType.Call)
                    {
                        var callExpression = (MethodCallExpression)leftExpression;
                        if (callExpression.Method.Name.Equals("CompareTo")
                            || callExpression.Method.Name.Equals("Compare"))
                        {
                            // Убирается лишний ноль из параметров (с которым сравнивался результат CompareTo)
                            _stacksHolder.PopParam();
                            _stacksHolder.PushFunction(
                                UtilsLcs.GetParamBinaryFunc(
                                    expression.NodeType, _stacksHolder.PopParam(), _stacksHolder.PopParam()));
                            break;
                        }
                    }

                    CompareWithObject(leftExpression, rightExpression, true);
                    CompareWithObject(rightExpression, leftExpression, false);
                    _stacksHolder.PushFunction(
                        UtilsLcs.GetParamBinaryFunc(
                            expression.NodeType, _stacksHolder.PopParam(), _stacksHolder.PopParam()));
                    break;
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                    var p1 = _stacksHolder.PopParam();
                    var p2 = _stacksHolder.PopParam();
                    var compiledExpression = UtilsLcs.TryExecuteBinaryOberation(expression.NodeType, p2, p1);
                    if (compiledExpression != null)
                    {
                        // Если выражение удалось скомпилировать и получить значение, то записываем уже его.
                        _stacksHolder.PushParam(compiledExpression);
                    }
                    else
                    {
                        _stacksHolder.PushFunction(UtilsLcs.GetParamBinaryFunc(expression.NodeType, p2, p1));
                    }

                    break;
                default:
                    throw new Exception("Неизвестный тип Expression");
            }

            return expression;
        }

        protected override Expression VisitQuerySourceReferenceExpression(QuerySourceReferenceExpression expression)
        {
            return expression;
        }

        /// <summary>
        /// Обход элемента в дереве выражения, соответствующему свойству элемента.
        /// </summary>
        /// <param name="expression"> Выражение, соответствующему свойству элемента. </param>
        /// <returns> Реализовано только в потомках, будет проброшено исключение. </returns>
        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            throw new NotImplementedException("Метод VisitMemberExpression доступен только в потомках.");
        }

        /// <summary>
        /// Вспомогательный метод, содержащий общую обработку VisitMemberExpressionHelper, используемую в потомках.
        /// </summary>
        /// <param name="expression"> Выражение, содержащее обращение к свойству. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры. </returns>
        protected Expression VisitMemberExpressionHelper(MemberExpression expression)
        {
            var declaringType = expression.Member.DeclaringType;
            var memberType = Information.GetPropertyType(declaringType, expression.Member.Name);

            // Обработка .net Nullable
            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                memberType = Nullable.GetUnderlyingType(memberType);
            }

            if (declaringType.IsGenericType && declaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                declaringType = Nullable.GetUnderlyingType(declaringType);
            }

            // Ещё могут взять "Value" от Nullable-типа.
            if (expression.NodeType == ExpressionType.MemberAccess
                    && expression.Member.Name == "Value"
                    && expression.Member.MemberType == MemberTypes.Property
                    && expression.Member.ReflectedType != null
                    && ((expression.Member.ReflectedType.IsGenericType
                    && expression.Member.ReflectedType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    || expression.Member.DeclaringType == typeof(NullableDateTime)
                    || expression.Member.DeclaringType == typeof(NullableInt)
                    || expression.Member.DeclaringType == typeof(NullableDecimal)))
            { // Value от Nullable-типа при переводе в lcs нам ничего не даст, поэтому просто опускаем его.
                expression = (MemberExpression)expression.Expression;
                return expression;
            }

            object master;
            string varname = PopMasterMemberName(expression, out master);
            if (memberType == typeof(KeyGuid))
            {
                VisitKeyGuidProperty(expression.Member.Name, master, varname);
            }
            else if (memberType == typeof(short) || memberType == typeof(ushort)
                || memberType == typeof(int) || memberType == typeof(uint)
                || memberType == typeof(long) || memberType == typeof(ulong)
                || memberType == typeof(float)
                || memberType == typeof(double)
                || memberType == typeof(decimal)
                || memberType == typeof(NullableInt)
                || memberType == typeof(NullableDecimal))
            {
                string memberName = expression.Member.Name;
                if (!string.IsNullOrEmpty(UtilsLcs.GetFunctionByName(memberName)))
                {
                    if (expression.Expression is ConstantExpression)
                    {
                        var param = _stacksHolder.PopParam();
                        _stacksHolder.PushParam(_ldef.GetFunction(UtilsLcs.GetFunctionByName(varname), param));
                    }
                    else
                    {
                        PushFunctionOfParent(expression, UtilsLcs.GetFunctionByName(varname));
                    }
                }
                else
                {
                    UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                    _stacksHolder.PushParam(new VariableDef(_ldef.NumericType, varname));
                }
            }
            else if (memberType == typeof(string) || memberType == typeof(char))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.StringType, varname));
            }
            else if (memberType == typeof(Guid))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.GuidType, varname));
            }
            else if (memberType == typeof(bool))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                var param = new VariableDef(_ldef.BoolType, varname);
                _stacksHolder.PushFunction(_ldef.GetFunction(_ldef.funcEQ, param));
            }
            else if (memberType == typeof(DateTime) || memberType == typeof(DayOfWeek)
                     || memberType == typeof(TimeSpan) || memberType == typeof(NullableDateTime))
            {
                string memberName = varname;
                if (expression.Expression is ConstantExpression)
                {
                    var param = _stacksHolder.PopParam();
                    _stacksHolder.PushParam(_ldef.GetFunction(UtilsLcs.GetFunctionByName(memberName), param));
                }
                else if (declaringType == typeof(DateTime) || declaringType == typeof(TimeSpan) || declaringType == typeof(NullableDateTime))
                {
                    switch (memberName)
                    {
                        case "Date":
                        case "DayOfWeek":
                        case "TimeOfDay":
                            PushFunctionOfParent(expression, UtilsLcs.GetFunctionByName(memberName));
                            break;

                        case "Now":
                            _stacksHolder.PushParam(_ldef.GetFunction(_ldef.paramTODAY));
                            break;
                        case "Today":
                            _stacksHolder.PushParam(
                                _ldef.GetFunction(_ldef.funcOnlyDate, _ldef.GetFunction(_ldef.paramTODAY)));
                            break;
                        default:
                            UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                            _stacksHolder.PushParam(new VariableDef(_ldef.DateTimeType, memberName));
                            break;
                    }
                }
                else
                {
                    UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                    _stacksHolder.PushParam(new VariableDef(_ldef.DateTimeType, memberName));
                }

                _previosVisitedMemberExpression = expression;
            }
            else if (memberType.IsSubclassOf(typeof(DataObject)))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _dataobjectmember = true;
                _stacksHolder.PushParam(new VariableDef(GetObjectTypeByType(memberType), varname));
                return expression;
            }
            else if (memberType.IsSubclassOf(typeof(Enum)))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.StringType, varname));
            }
            else if (memberType == typeof(object))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.StringType, varname));
            }
#if NETFX_45
            else if (memberType == typeof(Geography))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.GeographyType, varname));
            }
            else if (memberType == typeof(Geometry))
            {
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.GeometryType, varname));
            }
#endif
            else
            {
                throw new UnknownTypeException("Неизвестный тип операнда " + memberType.Name);
            }

            _dataobjectmember = false;
            return expression;
        }

        /// <summary>
        /// Обход константы в дереве выражения.
        /// </summary>
        /// <param name="expression"> Элемент, соответствующий константе. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры. </returns>
        protected override Expression VisitConstantExpression(ConstantExpression expression)
        {
            if (true.Equals(expression.Value))
            {
                _stacksHolder.PushFunction(UtilsLcs.GetTrueFunc());
            }
            else if (false.Equals(expression.Value))
            {
                _stacksHolder.PushFunction(UtilsLcs.GetFalseFunc());
            }
            else if (expression.Type.IsSubclassOf(typeof(Enum)))
            {
                // В БД хранятся caption
                string caption = EnumCaption.GetCaptionFor(expression.Value);
                _stacksHolder.PushParam(caption);
            }
            else
            {
                _stacksHolder.PushParam(expression.Value);
            }

            return expression;
        }

        /// <summary>
        /// Обход вызова метода в дереве выражения.
        /// </summary>
        /// <param name="expression"> Элемент, соответствующий вызову метода. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры. </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed. Suppression is OK here.")]
        protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
        {
            // ToDo: In production code, handle this via method lookup tables.
            string methodName = expression.Method.Name;
            switch (methodName)
            {
#if NETFX_45
                case "GeoIntersects":
                case "Intersects":
                    UtilsLcs.CheckMethodArguments(expression, new[] { typeof(Geography), typeof(Geography) });

                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Arguments[1]);
                    var arg2 = _stacksHolder.PopParam();
                    var arg1 = _stacksHolder.PopParam();

                    _stacksHolder.PushFunction(_ldef.GetFunction(_ldef.funcGeoIntersects, arg1, arg2));

                    return expression;

                case "GeomIntersects":
                    UtilsLcs.CheckMethodArguments(expression, new[] { typeof(Geometry), typeof(Geometry) });

                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Arguments[1]);
                    var arg_2 = _stacksHolder.PopParam();
                    var arg_1 = _stacksHolder.PopParam();

                    _stacksHolder.PushFunction(_ldef.GetFunction(_ldef.funcGeomIntersects, arg_1, arg_2));

                    return expression;
#endif

                case "Get":
                    // Обработка параметров
                    if (expression.Object.Type == typeof(ParamSet))
                    {
                        Type paramType = expression.Method.GetGenericArguments().First();

                        var paramDef = new ParameterDef
                        {
                            ParamName =
                                (string)((ConstantExpression)expression.Arguments[0]).Value,
                            Type =
                                new ObjectType(
                                paramType.AssemblyQualifiedName,
                                "@" + paramType.Name,
                                paramType)
                        };

                        _stacksHolder.PushParam(paramDef);
                    }

                    return expression;
                case "ToString":
                    // Для Nullable-типов проверка CheckMethodArguments не срабатывает, поскольку используется чуть другой ToString.
                    if (!(expression.Object != null
                        && expression.Object.Type.IsGenericType
                        && expression.Object.Type.GetGenericTypeDefinition() == typeof(Nullable<>)
                        && expression.Arguments.Count == 0))
                    {
                        UtilsLcs.CheckMethodArguments(expression, new Type[] { });
                    }

                    return VisitExpression(expression.Object);

                case "ToUpper":
                case "ToLower":
                    UtilsLcs.CheckMethodArguments(expression, new Type[] { });
                    VisitExpression(expression.Object);
                    _stacksHolder.PushFunction(_ldef.GetFunction(expression.Method.Name, _stacksHolder.PopParam()));
                    return expression;

                case "IsLike":
                    UtilsLcs.CheckMethodArguments(expression, new[] { typeof(string) });
                    if (expression.Arguments.Count != 2)
                    {
                        throw new Exception("Метод IsLike ожидает два параметра");
                    }

                    var exprPattern = expression.Arguments[1] as ConstantExpression;
                    if (exprPattern == null || exprPattern.Type != typeof(string))
                    {
                        throw new Exception("В качестве второго параметра метода IsLike поддеживается только строка");
                    }

                    var pattern = (string)exprPattern.Value;
                    return PushFunctionlike(expression, pattern);

                case "CompareTo":
                    UtilsLcs.CheckMethodArguments(expression, new[] { typeof(string) });
                    if (expression.Arguments.Count != 1)
                    {
                        throw new Exception("Функция CompareTo ожидает один параметр");
                    }

                    // Передача параметров в стек для дальнейшей обертки в стандартное сравнение lcs
                    VisitExpression(expression.Object);
                    VisitExpression(expression.Arguments[0]);
                    return expression;

                case "Compare":
                    UtilsLcs.CheckMethodArguments(expression, new[] { typeof(string), typeof(string) });
                    if (expression.Arguments.Count != 2 && (expression.Arguments.Count != 3 || expression.Arguments[2].Type != typeof(StringComparison)))
                    {
                        throw new Exception("Метод Compare ожидает два параметра");
                    }

                    // Передача параметров в стек для дальнейшей обертки в стандартное сравнение lcs.
                    // Если параметрова три и он типа StringComparison, то он игнорируется, потому что не даёт lcs никакой новой информации.
                    VisitExpression(expression.Arguments[0]);
                    VisitExpression(expression.Arguments[1]);
                    return expression;

                case "Contains":
                case "StartsWith":
                case "EndsWith":
                    UtilsLcs.CheckMethodArguments(expression, new[] { typeof(string) });
                    if (expression.Arguments[0].NodeType != ExpressionType.Constant)
                    {
                        throw new NotSupportedException(
                            string.Format(
                                "Методу {0} в качестве аргумента может быть передана только константа", methodName));
                    }

                    return PushFunctionlike(expression, UtilsLcs.GetLikePatternByFunctionName(methodName));

                case "Substring":
                    VisitExpression(expression.Object);
                    VisitExpression(expression.Arguments[0]);
                    var substringCount = -1;
                    if (expression.Arguments.Count > 1)
                    {
                        VisitExpression(expression.Arguments[1]);
                        substringCount = (int)_stacksHolder.PopParam();
                    }

                    var substringIndex = (int)_stacksHolder.PopParam();
                    var substringParam = _stacksHolder.PopParam();
                    _stacksHolder.PushFunction(
                        _ldef.GetFunction(_ldef.funcLike, substringParam, UtilsLcs.GetSqlWherePattern(substringIndex, substringCount)));
                    return expression;

                case "AddYears":
                    VisitExpression(expression.Object);
                    VisitExpression(expression.Arguments[0]);
                    var yearArg = _stacksHolder.PopParam();
                    var yearParam = _stacksHolder.PopParam();

                    _stacksHolder.PushFunction(
                        _ldef.GetFunction(_ldef.funcDateAdd, _ldef.GetFunction(_ldef.paramYearDIFF), yearArg, yearParam));
                    return expression;

                case "Equals":
                    VisitExpression(expression.Object);
                    foreach (Expression argument in expression.Arguments)
                    {
                        if (argument.Type == typeof(StringComparison))
                        {
                            throw new NotSupportedException(
                                "StringComparison не поддерживается, используйте другой перегруженный метод string.Equals(string, string)");
                        }

                        VisitExpression(argument);
                    }

                    var param1 = _stacksHolder.PopParam();
                    var param2 = _stacksHolder.PopParam();
                    _stacksHolder.PushFunction(
                        _ldef.GetFunction(_ldef.funcEQ, param2, param1));
                    return expression;

                case "Parse":
                    object param;
                    try
                    {
                        param = Expression.Lambda(expression).Compile().DynamicInvoke();
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Невозможно выполнить Parse для '{0}'", expression.Arguments[0]));
                    }

                    _stacksHolder.PushParam(param);
                    return expression;

                case "IsMatch": // Данная функция поддерживается для регулярных выражений
                    Expression resultExpression = VisitMethodIsMatch(expression);
                    if (resultExpression != null)
                    {
                        return resultExpression;
                    }

                    break;

                case "Any": // Данная функция вызывается для псевдодетейлов
                case "All": // Данная функция вызывается для псевдодетейлов
                    Expression resultExpression2 = this.VisitMethodAnyOrAllForPseudoDetail(expression, methodName);
                    if (resultExpression2 != null)
                    {
                        return resultExpression2;
                    }

                    break;

                case "IsNullOrEmpty":
                    VisitExpression(expression.Arguments[0]);
                    _stacksHolder.PushFunction(UtilsLcs.GetCompareWithNullFunction(ExpressionType.Equal, _stacksHolder.PopParam()));
                    return expression;
            }

            #region Неподдерживаемые функции

            if (UtilsLcs.ExpressionMethodEquals(expression, "AddDays", new[] { typeof(double) }))
            {
                throw new NotImplementedException();
            }

            #endregion Неподдерживаемые функции

            return base.VisitMethodCallExpression(expression);
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            string itemText = FormatUnhandledItem(unhandledItem);
            var message = string.Format("The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof(T));
            return new NotSupportedException(message);
        }

        /// <summary>
        /// Обработка метода Regex.IsMatch в Linq-выражении.
        /// </summary>
        /// <param name="expression"> Выражение, содержащее IsMatch. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые параметры. </returns>
        private Expression VisitMethodIsMatch(MethodCallExpression expression)
        {
            if (expression.Method.DeclaringType != typeof(System.Text.RegularExpressions.Regex))
            {
                return null;
            }

            if (expression.Arguments.Count != 2)
            {
                throw new NotSupportedException("Поддерживается выражение Regex.IsMatch только с двумя аргументами.");
            }

            bool hasParameter;

            if (expression.Arguments[1].NodeType == ExpressionType.Constant && expression.Arguments[1].Type == typeof(string))
            { // Это просто значение-константа.
                hasParameter = false;
            }
            else if (ParamsUtils.IsItParameter(expression.Arguments[1]))
            { // Это константа.
                hasParameter = true;
            }
            else
            {
                throw new NotSupportedException("Метод Regex.IsMatch ожидает вторым параметром шаблон в виде строки или параметр.");
            }

            Expression arg1 = expression.Arguments[0];
            Expression arg2 = expression.Arguments[1];

            VisitExpression(arg1);
            VisitExpression(arg2);

            var param1 = _stacksHolder.PopParam();
            if (!hasParameter)
            {
                param1 = UtilsLcs.ConvertRegexToSql((string)param1);
            }

            var param2 = _stacksHolder.PopParam();
            if (param2 is VariableDef)
            {
                param2 = new VariableDef(_ldef.StringType, ((VariableDef)param2).Caption);
            }
            else if (!(param2 is string))
            {
                throw new NotSupportedException("Метод Regex.IsMatch ожидает первым параметром константу-строку или обращение к свойству строкового типа.");
            }

            _stacksHolder.PushFunction(
                _ldef.GetFunction(
                _ldef.funcLike,
                param2,
                param1));

            return expression;
        }

        /// <summary>
        /// Данная функция работает в ситуации обработки выражения вида:
        /// new PseudoDetail_Порода, Кошка_(Кошка.Views.КошкаE,"Порода").Any(x => x.Кличка != "Барсик").
        /// Выражение будет обработано и построено корректное DetailVariableDef
        /// </summary>
        /// <param name="expression"> Выражение типа new PseudoDetail(...).Any(...) </param>
        /// <param name="methodName"> Какое выражение передано: All или Any. </param>
        /// <returns>То же выражение, но это будет значить, что оно корректно обработано и в стек помещены нужные данные (либо null вернётся, если данный метод не может по какой-то причине обработать вызов метода). </returns>
        private Expression VisitMethodAnyOrAllForPseudoDetail(MethodCallExpression expression, string methodName)
        {
            Type baseGenericType = typeof(PseudoDetail<,>);

            if (expression.Object == null
                || !expression.Object.Type.IsGenericType
                || expression.Object.Type.GetGenericTypeDefinition() != baseGenericType)
            {
                return null;
            }

            object pseudoDetailObject = null;

            switch (expression.Object.NodeType)
            {
                case ExpressionType.Constant: // Если константа, то из объекта сразу можно получать данные.
                    pseudoDetailObject = ((ConstantExpression)expression.Object).Value;
                    break;
                case ExpressionType.New: // Если стоит выражение типа new PseudoDetail(...), то выражение нужно скомпилировать и получить объект.
                    pseudoDetailObject = Expression.Lambda(expression.Object).Compile().DynamicInvoke();
                    break;
                default:
                    throw new NotSupportedException();
            }

            // Извлекаем значение свойств объекта типа PseudoDetail.
            Type realGenericType = baseGenericType.MakeGenericType(expression.Object.Type.GetGenericArguments());
            var pseudoDetailView = (ICSSoft.STORMNET.View)realGenericType.GetProperty(PseudoDetailConsts.PseudoDetailViewPropertyName).GetValue(pseudoDetailObject, null);
            var masterLinkName = (string)realGenericType.GetProperty(PseudoDetailConsts.MasterLinkNamePropertyName).GetValue(pseudoDetailObject, null);
            var masterToDetailPseudoProperty = (string)realGenericType.GetProperty(PseudoDetailConsts.MasterToDetailPseudoPropertyPropertyName).GetValue(pseudoDetailObject, null);
            var masterConnectProperties = (string[])realGenericType.GetProperty(PseudoDetailConsts.MasterConnectPropertiesPropertyName).GetValue(pseudoDetailObject, null);

            var detailVariableDef = new DetailVariableDef(
                        _ldef.GetObjectType("Details"),
                        masterToDetailPseudoProperty,
                        pseudoDetailView,
                        masterLinkName,
                        masterConnectProperties);

            // Получаем метод CreatePseudoDetailQuery для нужного нам типа.
            MethodInfo genericQueryMethod = this.GetType().GetMethod("CreatePseudoDetailQuery").MakeGenericMethod(
                new Type[] { expression.Object.Type.GetGenericArguments()[1] });

            Expression subQuery = null;

            object lf = null;
            switch (expression.Arguments.Count())
            {
                case 0: // Поддерживаем без параметров только Any.
                    if (methodName != "Any")
                    {
                        throw new NotSupportedException();
                    }

                    lf = true;
                    break;
                case 1: // All и Any аргументом имеют функцию
                    if (expression.Arguments[0] is UnaryExpression)
                    {
                        subQuery = ((UnaryExpression)expression.Arguments[0]).Operand;
                    }
                    else if (expression.Arguments[0] is ConstantExpression)
                    {
                        var possibleLambda = ((ConstantExpression)expression.Arguments[0]).Value;
                        if (!(possibleLambda is LambdaExpression))
                        {
                            throw new NotSupportedException();
                        }

                        subQuery = (LambdaExpression)possibleLambda;
                    }

                    if (!(subQuery is LambdaExpression))
                    {
                        throw new NotSupportedException();
                    }

                    var resultQuery = genericQueryMethod.Invoke(this, new object[] { subQuery });
                    var queryModel = UtilsLcs.CreateQueryParser().GetParsedQuery((Expression)resultQuery);
                    var innerLimitFunction = GetQueryModelVisitor(_viewIsDynamic, pseudoDetailView, _resolvingViews).GenerateLcs(queryModel).LimitFunction;
                    lf = innerLimitFunction as object ?? true;
                    break;
                default:
                    throw new NotSupportedException();
            }

            string lcsFunctionName = string.Empty;
            switch (methodName)
            {
                case "Any":
                    lcsFunctionName = _ldef.funcExist;
                    break;
                case "All":
                    lcsFunctionName = _ldef.funcExistExact;
                    break;
                default:
                    throw new ArgumentException();
            }

            _stacksHolder.PushFunction(_ldef.GetFunction(lcsFunctionName, detailVariableDef, lf));

            return expression;
        }

        /// <summary>
        /// Создаём lcs для ограничения на псевдодетейл.
        /// Делается это так: создаётся запрос к объекту типа псевдодетейл и получается lcs, откуда берётся правильная функция ограничения.
        /// </summary>
        /// <typeparam name="T"> Тип псевдодетейла. </typeparam>
        /// <param name="lambdaExpression"> Ограничение на псевдодетейл, из которого нужно получить LimitFunction. </param>
        /// <returns> LimitFunction для псевдодетейла. </returns>
        public Expression CreatePseudoDetailQuery<T>(LambdaExpression lambdaExpression) where T : DataObject
        {
            IQueryable<T> queryList = new List<T>().AsQueryable();
            IQueryable<T> query = queryList.Where((Expression<Func<T, bool>>)lambdaExpression);
            Expression queryExpression = query.Expression;
            return queryExpression;
        }

        /// <summary>
        /// Обрабатывает стеки если имеет место сравнения с объектом иначе ничего не делает.
        /// </summary>
        /// <param name="one"> Первый операнд сравнения. </param>
        /// <param name="two"> Второй операнд сравнения. </param>
        /// <param name="course"> Очередность операндов. </param>
        private void CompareWithObject(Expression one, Expression two, bool course)
        {
            if (one is QuerySourceReferenceExpression)
            {
                if (two is ConstantExpression)
                {
                    var param = _stacksHolder.PopParam();
                    AddTwoParamInCourse(
                        new VariableDef(_ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey),
                        ((DataObject)param).__PrimaryKey,
                        course);
                }
                else if (two is MemberExpression)
                {
                    var param = _stacksHolder.PopParam();
                    AddTwoParamInCourse(
                       new VariableDef(_ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey),
                       new VariableDef(_ldef.GuidType, string.Format(((VariableDef)param).Caption)),
                       course);
                }
            }
        }

        /// <summary>
        /// Кладет в стек два элемента с заданной очередностью.
        /// </summary>
        /// <param name="one"> Первый элемент. </param>
        /// <param name="two"> Второй элемент. </param>
        /// <param name="cource"> Сначала первый потом второй или наоборот. </param>
        private void AddTwoParamInCourse(object one, object two, bool cource)
        {
            if (cource)
            {
                _stacksHolder.PushParam(one);
                _stacksHolder.PushParam(two);
            }
            else
            {
                _stacksHolder.PushParam(two);
                _stacksHolder.PushParam(one);
            }
        }

        /// <summary>
        /// Возвращает lcs тип по типу system.Type.
        /// </summary>
        /// <param name="memberType"> Обычный тип. </param>
        /// <returns> Тип lcs. </returns>
        private ObjectType GetObjectTypeByType(Type memberType)
        {
            if (memberType == typeof(string))
            {
                return _ldef.StringType;
            }

            if (memberType == typeof(int) || memberType == typeof(long) || memberType == typeof(DayOfWeek) || memberType == typeof(NullableInt) || memberType == typeof(NullableDecimal))
            {
                return _ldef.NumericType;
            }

            if (memberType == typeof(DateTime) || memberType == typeof(NullableDateTime))
            {
                return _ldef.DateTimeType;
            }

            if (memberType == typeof(bool))
            {
                return _ldef.BoolType;
            }

            if (memberType == typeof(KeyGuid))
            {
                return _ldef.GuidType;
            }

            if (memberType.IsSubclassOf(typeof(DataObject)))
            {
                return _ldef.DataObjectType;
            }

            throw new UnknownTypeException("Неизвестный тип операнда " + memberType.Name);
        }

        private string PopMasterMemberName(MemberExpression expression, out object master)
        {
            string varName;
            master = null;
            if (!_dataobjectmember)
            {
                varName = expression.Member.Name;
            }
            else
            {
                var par = (VariableDef)_stacksHolder.PopParam();
                master = par;
                varName = string.Format(
                    "{0}.{1}", par.StringedView, expression.Member.Name);
                _dataobjectmember = false;
            }

            return varName;
        }

        /// <summary>
        /// Возвращает функцию с учетом родителя данного expression.
        /// </summary>
        /// <param name="expression"> Выражение. </param>
        /// <param name="func"> Имя функции. </param>
        private void PushFunctionOfParent(MemberExpression expression, string func)
        {
            string memberName = expression.Member.Name;
            object param = new VariableDef(_ldef.DateTimeType, memberName);

            MemberExpression value = null;
            if (expression.Expression is MemberExpression)
            {
                value = expression.Expression as MemberExpression;
            }

            // Если родитель этого Member предыдущий Member
            if (ReferenceEquals(_previosVisitedMemberExpression, expression.Expression) ||
                value != null && value.Type == typeof(DateTime) && value.Member.Name == "Value" && // Добавлена поддержка для Nullable<DateTime> и NullableDateTime
                ReferenceEquals(_previosVisitedMemberExpression, value.Expression)) // при обработке свойств DateTime (Day, Month, Year и т.д.).
            {
                param = _stacksHolder.PopParam();

                if (!(param is VariableDef))
                {
                    // Если параметр не VariableDef, то предыдущий Member был Now
                    param = _ldef.GetFunction(_ldef.paramTODAY);
                }
            }

            _stacksHolder.PushParam(_ldef.GetFunction(func, param));
        }

        /// <summary>
        /// Функция предназначена для добавления в стек функций функции like с параметром в формате заданом строкой
        /// </summary>
        /// <param name="expression"> Выражение. </param>
        /// <param name="format"> Формат строки. символ $ заменяется на значение взятое из стека. </param>
        private MethodCallExpression PushFunctionlike(MethodCallExpression expression, string format)
        {
            bool expressionHasObject = expression.Object != null;

            Expression arg1 = expressionHasObject ? expression.Object : expression.Arguments[0];
            Expression arg2 = expressionHasObject ? expression.Arguments[0] : expression.Arguments[1];

            VisitExpression(arg1);
            VisitExpression(arg2);

            var param1 = (string)_stacksHolder.PopParam();
            var param2 = (VariableDef)_stacksHolder.PopParam();

            _stacksHolder.PushFunction(
                _ldef.GetFunction(
                    _ldef.funcLike, new VariableDef(_ldef.StringType, param2.Caption), format.Replace("$", param1)));
            return expression;
        }

        private string FormatUnhandledItem<T>(T unhandledItem)
        {
            var itemAsExpression = unhandledItem as Expression;
            return itemAsExpression != null ? FormattingExpressionTreeVisitor.Format(itemAsExpression) : unhandledItem.ToString();
        }

        /// <summary>
        /// Проводим правильную обработку поля типа KeyGuid (это может быть первичный ключ, а может и просто свойство).
        /// </summary>
        /// <param name="expressionMemberName">Имя поля (короткое, если это цепочка из мастеров, то выводится только последнее почле точки).</param>
        /// <param name="master">Мастеровой объект, извлечённый из объекта по этому свойству (если null, то это не ссылка на мастера).</param>
        /// <param name="varname">Полное имя поля (выдаётся полная цепочка ссылок на мастеров).</param>
        private void VisitKeyGuidProperty(string expressionMemberName, object master, string varname)
        {
            if (expressionMemberName == "__PrimaryKey")
            { // Первичный ключ.
                if (master is VariableDef)
                {
                    UtilsLcs.AddMasterToView(_view, varname, _viewIsDynamic);
                    var varDefMaster = (VariableDef)master;
                    _stacksHolder.PushParam(new VariableDef(_ldef.GuidType, varDefMaster.StringedView));
                }
                else
                {
                    _stacksHolder.PushParam(new VariableDef(_ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey));
                }
            }
            else
            { // Это не первичный ключ, а просто собственное свойство.
                UtilsLcs.AddPropertyToView(_view, varname, _viewIsDynamic);
                _stacksHolder.PushParam(new VariableDef(_ldef.GuidType, varname));
            }
        }

        /// <summary>
        /// Конвертация функции в необходимую форму, если это выражение вида "o => o.Дата.DayOfWeek == DayOfWeek.Sunday".
        /// </summary>
        /// <param name="leftExpression">Левая часть выражения.</param>
        /// <param name="rightExpression">Правая часть выражения.</param>
        /// <returns>
        /// Если это не функция вида "o => o.Дата.DayOfWeek == DayOfWeek.Sunday", то вернётся просто значение <paramref name="rightExpression"/>.
        /// Если выявлено, что это функция подходящего вида, то вернётся отконвертированная в число DayOfWeek. Например, "DayOfWeek.Sunday" будет отконвертировано в "0".
        /// </returns>
        private Expression CheckOnFuncDayOfWeekZeroBased(Expression leftExpression, Expression rightExpression)
        {
            /* В lcs есть хитрая функция "funcDayOfWeekZeroBased", которая берёт текущий день недели.
            При её использовании при равенстве вторым параметром должно быть число, соответствующее дню недели.
            var specialFunction = ldef.GetFunction(ldef.funcEQ, ldef.GetFunction(ldef.funcDayOfWeekZeroBased, new VariableDef(ldef.DateTimeType, "Дата")), 0)

            В компиляторе VS2013, если в Linq-выражение передавалась константа enum-типа, то она преобразовывалась в число
            (а если значение переменной enum-типа, то Convert(Enum-значение) в int32).
            В компиляторе VS2015 это приведено к единообразию и теперь не срабатывает расположенное в методе VisitBinaryExpression исключение "NotSupportedException"
            (только формирование функции типа "specialFunction" проходило это условие и шло дальше).
            Теперь, чтобы "funcDayOfWeekZeroBased" могло корректно формироваться, нужно навести дополнительное условие с сохранением обратной совместимости.
            */

            if (leftExpression.NodeType == ExpressionType.Convert
                && rightExpression.NodeType == ExpressionType.Convert
                && leftExpression.Type == typeof(int)
                && rightExpression.Type == typeof(int))
            {
                var leftUnary = (UnaryExpression)leftExpression;
                var rightUnary = (UnaryExpression)rightExpression;

                if (rightUnary.Operand.NodeType == ExpressionType.Constant
                    && rightUnary.Operand.Type == typeof(DayOfWeek)
                    && leftUnary.Operand.NodeType == ExpressionType.MemberAccess
                    && leftUnary.Operand.Type == typeof(DayOfWeek)
                    && ((MemberExpression)leftUnary.Operand).Member.Name == "DayOfWeek"
                    && ((MemberExpression)leftUnary.Operand).Member.DeclaringType == typeof(DateTime))
                {
                    return Expression.Constant((int)Expression.Lambda(rightUnary.Operand).Compile().DynamicInvoke());
                }
            }

            return rightExpression;
        }
    }
}
