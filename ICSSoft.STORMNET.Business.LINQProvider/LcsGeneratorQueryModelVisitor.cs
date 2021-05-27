using System.Reflection;
using System.Text;

namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    using Remotion.Linq;
    using Remotion.Linq.Clauses;
    using Remotion.Linq.Clauses.ResultOperators;

    /// <summary>
    /// Генерация LCS по модели запроса.
    /// </summary>
    public class LcsGeneratorQueryModelVisitor : QueryModelVisitorBase, IQueryModelVisitor
    {
        /// <summary>
        /// Результирующий LCS.
        /// </summary>
        private readonly LoadingCustomizationStruct _resultLcs = new LoadingCustomizationStruct(null);

        /// <summary>
        /// Представление.
        /// </summary>
        protected View View;

        /// <summary>
        /// Является ли представление динамическим.
        /// </summary>
        private bool _viewIsDynamic;

        /// <summary>
        /// Дополнительные представления, необходимые для построения ограничений, например, на детейлы.
        /// </summary>
        private IEnumerable<View> _resolvingViews;

        /// <summary>
        /// Определение функций ограничения.
        /// </summary>
        protected SQLWhereLanguageDef langdef = SQLWhereLanguageDef.LanguageDef;

        /// <summary>
        /// Initializes a new instance of the <see cref="LcsGeneratorQueryModelVisitor"/> class.
        /// </summary>
        /// <param name="viewIsDynamic">
        /// The view is dynamic.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="resolvingViews">
        /// The resolving views.
        /// </param>
        public LcsGeneratorQueryModelVisitor(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews)
        {
            View = view;
            if (viewIsDynamic)
            {
                // ToDo: Представление записывается в LCS только в случае динамического
                // ToDo: представления, потому что иначе не проходятся существующие тесты
                // ToDo: (их нужно исправить, чтобы они записывали представление в LCS,
                // ToDo: с которой сравнивается результат)
                _resultLcs.View = View;
            }

            _viewIsDynamic = viewIsDynamic;
            _resolvingViews = resolvingViews;
        }

        /// <summary>
        /// Создать экземпляр visitor'а для обработки дерева выражения для <see cref="LcsQueryProvider{T,Q}"/>.
        /// </summary>
        /// <param name="viewIsDynamic">Динамически создавать представление.</param>
        /// <param name="view">Представление, если было указано.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns>Экземпляр visitor'а для обработки дерева выражения для <see cref="LcsQueryProvider{T,Q}"/>.</returns>
        public virtual IExpressionTreeVisitor GetExpressionTreeVisitor(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews)
        {
            return new LcsGeneratorExpressionTreeVisitorCommon(viewIsDynamic, view, resolvingViews);
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для запроса linq-выражения.
        /// </summary>
        /// <param name="queryModel">Запроса linq-выражения.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/> полученный для запроса linq-выражения.</returns>
        public LoadingCustomizationStruct GenerateLcs(QueryModel queryModel)
        {
            VisitQueryModel(queryModel);
            return GetLcs();
        }

        /// <summary>
        /// Получить внутреннюю пременную LCS. Перед вызовом убедитесь, что она готова.
        /// </summary>
        /// <returns>
        /// The <see cref="LoadingCustomizationStruct"/>.
        /// </returns>
        public LoadingCustomizationStruct GetLcs()
        {
            return _resultLcs;
        }

        /// <summary>
        /// The visit query model.
        /// </summary>
        /// <param name="queryModel">
        /// The query model.
        /// </param>
        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        /// <summary>
        /// Обработка фрагмента модели, соответствующей выражению с Where.
        /// </summary>
        /// <param name="whereClause"> Фрагмент модели, соответствующий выражению с Where. </param>
        /// <param name="queryModel"> Общая построенная модель запроса. </param>
        /// <param name="index"> Позиция фрагмента в общей модели. </param>
        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            FillLcsLimitFunction(whereClause.Predicate);
            base.VisitWhereClause(whereClause, queryModel, index);
        }

        /// <summary>
        /// The visit result operator.
        /// </summary>
        /// <param name="resultOperator">
        /// The result operator.
        /// </param>
        /// <param name="queryModel">
        /// The query model.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            if (resultOperator.GetType() == typeof(TakeResultOperator))
            {
                _resultLcs.ReturnTop = ((TakeResultOperator)resultOperator).GetConstantCount();
                if (_resultLcs.RowNumber != null)
                {
                    _resultLcs.RowNumber.EndRow = _resultLcs.RowNumber.StartRow + _resultLcs.ReturnTop - 1;
                    _resultLcs.ReturnTop = 0;
                }
            }
            else if (resultOperator.GetType() == typeof(FirstResultOperator))
            {
                var resOp = (FirstResultOperator)resultOperator;
                _resultLcs.ReturnType = resOp.ReturnDefaultWhenEmpty
                                            ? LcsReturnType.Object
                                            : LcsReturnType.ObjectRequired;
            }
            else if (resultOperator.GetType() == typeof(AnyResultOperator))
            {
                _resultLcs.ReturnType = LcsReturnType.Any;
            }
            else if (resultOperator.GetType() == typeof(AllResultOperator))
            {
                var a = (AllResultOperator)resultOperator;
                FillLcsLimitFunction(a.Predicate);
                _resultLcs.ReturnType = LcsReturnType.All;
            }
            else if (resultOperator.GetType() == typeof(CountResultOperator))
            {
                _resultLcs.ReturnType = LcsReturnType.Count;
            }
            else if (resultOperator is SkipResultOperator)
            {
                var minNumberExpression = ((SkipResultOperator)resultOperator).Count as ConstantExpression;

                if (minNumberExpression != null)
                {
                    int maxNumber = _resultLcs.ReturnTop == 0 ? int.MaxValue : _resultLcs.ReturnTop;
                    if (_resultLcs.RowNumber == null)
                    {
                        _resultLcs.RowNumber = new RowNumberDef((int)minNumberExpression.Value + 1, maxNumber);
                    }
                    else
                    {
                        _resultLcs.RowNumber.StartRow = _resultLcs.RowNumber.StartRow + (int)minNumberExpression.Value;
                    }

                    _resultLcs.ReturnTop = 0;
                }
            }

            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        /// <summary>
        /// The visit order by clause.
        /// </summary>
        /// <param name="orderByClause">
        /// The order by clause.
        /// </param>
        /// <param name="queryModel">
        /// The query model.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            foreach (Ordering ordering in orderByClause.Orderings)
            {
                MemberInfo member;
                Expression expression = null;
                StringBuilder path = new StringBuilder();
                if (ordering.Expression is MemberExpression)
                {
                    expression = ordering.Expression as MemberExpression;
                }
                else if (ordering.Expression is UnaryExpression)
                {
                    expression = ordering.Expression as UnaryExpression;
                }
                else if (ordering.Expression is ConditionalExpression)
                {
                    expression = (ordering.Expression as ConditionalExpression).IfFalse;
                    while (expression is ConditionalExpression)
                    {
                        expression = (expression as ConditionalExpression).IfFalse;
                    }
                }

                while (expression != null && expression.NodeType == ExpressionType.Convert)
                {
                    expression = (expression as UnaryExpression).Operand;
                }

                MemberExpression memberExpression = expression as MemberExpression;
                while (memberExpression != null)
                {
                    member = UtilsLcs.GetObjectPropertyValue(memberExpression, "Member");
                    memberExpression = memberExpression.Expression as MemberExpression;
                    if (member == null)
                    {
                        break;
                    }
                    else if (member.Name == "Value")
                    {
                        continue;
                    }

                    if (path.Length > 0)
                    {
                        path.Insert(0, ".");
                    }

                    path.Insert(0, member.Name);
                }

                if (path.Length > 0)
                {
                    AddColumnSort(path.ToString(), GetOrder(ordering.OrderingDirection));
                }
            }

            VisitOrderings(orderByClause.Orderings, queryModel, orderByClause);
        }

        /// <summary>
        /// Добавить сортировку на свойство объекта.
        /// Порядок сортировки определяется порядком вызова метода для различных свойств.
        /// </summary>
        /// <param name="propertyName">Наименование свойства, по которому необходимо сортировать.</param>
        /// <param name="sortOrder">Направление сортировки.</param>
        protected void AddColumnSort(string propertyName, SortOrder sortOrder)
        {
            _resultLcs.AddColumnSort(new ColumnsSortDef(propertyName, sortOrder));
        }

        /// <summary>
        /// Преобразовать направление сортировки в формате LINQ в направление сортировки в формате LCS.
        /// </summary>
        /// <param name="od">
        /// Направление сортировки в формате LINQ.
        /// </param>
        /// <returns>
        /// The <see cref="SortOrder"/>.
        /// </returns>
        protected SortOrder GetOrder(OrderingDirection od)
        {
            switch (od)
            {
                case OrderingDirection.Asc:
                    return SortOrder.Asc;
                case OrderingDirection.Desc:
                    return SortOrder.Desc;
                default:
                    return SortOrder.None;
            }
        }

        protected virtual void SetLimitFuncion(Function limitFunction, LoadingCustomizationStruct lcs)
        {
            if (lcs.LimitFunction != null)
            {
                // Для поддержки цепочного вызова Where необходимо объединить переданное ограничение с уже существующим.
                lcs.LimitFunction = langdef.GetFunction(langdef.funcAND, lcs.LimitFunction, limitFunction);
            }
            else
            {
                lcs.LimitFunction = limitFunction;
            }
        }

        /// <summary>
        /// The fill lcs limit function.
        /// </summary>
        /// <param name="expression">
        /// The linq expression.
        /// </param>
        private void FillLcsLimitFunction(Expression expression)
        {
            SetLimitFuncion(GetExpressionTreeVisitor(_viewIsDynamic, View, _resolvingViews).GetLcsExpression(expression), _resultLcs);
        }
    }
}
