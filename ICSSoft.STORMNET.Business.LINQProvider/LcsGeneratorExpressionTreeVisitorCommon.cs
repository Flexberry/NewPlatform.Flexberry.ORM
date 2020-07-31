namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Remotion.Linq.Clauses;
    using Remotion.Linq.Clauses.Expressions;
    using Remotion.Linq.Clauses.ResultOperators;

    /// <summary>
    /// Visitor, который обходит распарсенноое дерево (в большинстве случаев подходит эта реализация).
    /// </summary>
    public class LcsGeneratorExpressionTreeVisitorCommon : LcsGeneratorExpressionTreeVisitorBase
    {
        /// <summary>
        /// Конструктор класса <see cref="LcsGeneratorExpressionTreeVisitorCommon"/>.
        /// </summary>
        /// <param name="stacksHolder"> Элемент, организующий стек. </param>
        public LcsGeneratorExpressionTreeVisitorCommon(TreeVisitorStacksHolder stacksHolder)
            : base(stacksHolder)
        {
        }

        /// <summary>
        /// Конструктор класса <see cref="LcsGeneratorExpressionTreeVisitorCommon"/>.
        /// </summary>
        /// <param name="viewIsDynamic"> Является ли представление динамическим (формируется во время разбора выражения). </param>
        /// <param name="view"> Представление (в случае динамического представления может быть передано пустое представление, у которого задан только тип). </param>
        /// <param name="resolvingViews"> Представления мастеров, нужные для получения их детейлов (в случае динамических представлений null). </param>
        public LcsGeneratorExpressionTreeVisitorCommon(
            bool viewIsDynamic,
            View view,
            IEnumerable<View> resolvingViews)
            : base(viewIsDynamic, view, resolvingViews)
        {
        }

        /// <summary>
        /// Создать экземпляр visitor'а для обработки запроса linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.
        /// </summary>
        /// <param name="viewIsDynamic">Динамически создавать представление.</param>
        /// <param name="view">Представление, если было указано.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns>Экземпляр visitor'а для обработки запроса linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.</returns>
        public override IQueryModelVisitor GetQueryModelVisitor(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews)
        {
            return new LcsGeneratorQueryModelVisitor(viewIsDynamic, view, resolvingViews);
        }

        /// <summary>
        /// Получить функцию ограничения для <see cref="LoadingCustomizationStruct"/>.
        /// </summary>
        /// <param name="linqExpression">Linq-выражение, для которого необходимо получить функцию ограничения.</param>
        /// <returns>Функция ограничения для <see cref="LoadingCustomizationStruct"/>.</returns>
        public override Function GetLcsExpression(Expression linqExpression)
        {
            Visit(linqExpression);
            return GetLcsExpression();
        }

        /// <summary>
        /// Обход подзапроса в дереве выражения.
        /// </summary>
        /// <param name="expression"> Выражение-подзапрос. </param>
        /// <returns> Фактически возвращается то же выражение-подзапрос, но при этом в стеке появляется необходимая lcs. </returns>
        protected override Expression VisitSubQuery(SubQueryExpression expression)
        {
            var resultExpression = VisitSubQueryExpressionHelper(expression);

            if (resultExpression != null)
            {
                return resultExpression;
            }

            var resultOperator = expression.QueryModel.ResultOperators[0];
            var fromExpr = expression.QueryModel.MainFromClause.FromExpression;
            bool processed = ProcessContainsResultOperator(resultOperator, fromExpr);

            if (!processed)
            {
                throw new NotSupportedException("Функция подзапроса не поддерживается");
            }

            return expression;
        }

        /// <summary>
        /// Обход элемента в дереве выражения, соответствующему свойству элемента.
        /// </summary>
        /// <param name="expression"> Выражение, соответствующему свойству элемента. </param>
        /// <returns> Фактически возвращается то же выражение, но при этом в стеке появляются необходимые данные. </returns>
        protected override Expression VisitMember(MemberExpression expression)
        {
            Visit(expression.Expression);
            return VisitMemberExpressionHelper(expression);
        }

        /// <summary>
        /// Обработка подзапроса, содержащего операцию Contains (если запрос не содержит Contains, то вернётся false).
        /// </summary>
        /// <param name="resultOperator"> Оператор результата подзапроса, который может быть с Contains. </param>
        /// <param name="fromExpr"> From-часть выражения подзапроса. </param>
        /// <returns>False, если оператор результата подзапроса не содержит Contains (если содержит, но по какой-то причине обработка не прошла, то будет вызвано исключение). </returns>
        private bool ProcessContainsResultOperator(ResultOperatorBase resultOperator, Expression fromExpr)
        {
            var resOp = resultOperator as ContainsResultOperator;
            if (resOp == null)
            {
                return false;
            }

            var itemWithMemeber = resOp.Item;

            var methodCallExpression = resOp.Item as MethodCallExpression;
            if (methodCallExpression != null)
            {
                var itemMethod = methodCallExpression;
                if (!string.Equals(itemMethod.Method.Name, "tostring", StringComparison.OrdinalIgnoreCase))
                {
                    throw new NotSupportedException("Поддерживается только ToString внутри Contains");
                }

                itemWithMemeber = itemMethod.Object;
            }

            // Положим VariableDef в stack
            Visit(itemWithMemeber);
            var varDef = _stacksHolder.PopParam();

            var listToQuery = new List<object> { varDef };
            var expr = fromExpr as NewArrayExpression;
            var collectionValues = expr != null
                                       ? expr.Expressions.Select(x => ((ConstantExpression)x).Value)
                                       : (IEnumerable<object>)((ConstantExpression)fromExpr).Value;

            listToQuery.AddRange(collectionValues);
            _stacksHolder.PushFunction(_ldef.GetFunction(_ldef.funcIN, listToQuery.ToArray()));

            return true;
        }
    }
}
