namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System.Linq.Expressions;

    using Remotion.Linq.Clauses.Expressions;
    using Remotion.Linq.Parsing;

    /// <summary>
    /// Данный класс представляет собой по сути ReplacingExpressionTreeVisitor с выправленным нужным образом методом VisitMemberExpression,
    /// который убирает привязку свойства псевдодетейла к объекту типа мастера.
    /// </summary>
    public class ReplacingExpressionTreeVisitorWithProperType : ExpressionTreeVisitor
    {
        private readonly Expression _replacedExpression;
        private readonly Expression _replacementExpression;

        /// <summary>
        /// Конструктор класса <see cref="ReplacingExpressionTreeVisitorWithProperType"/>.
        /// </summary>
        /// <param name="replacedExpression"> Аргумент, который будет подменяться в выражении. </param>
        /// <param name="replacementExpression"> Текущая версия сформированного выражения. </param>
        private ReplacingExpressionTreeVisitorWithProperType(Expression replacedExpression, Expression replacementExpression)
        {
            _replacedExpression = replacedExpression;
            _replacementExpression = replacementExpression;
        }

        /// <summary>
        /// Выполнить замену и привязку аргумента в выражении.
        /// </summary>
        /// <param name="replacedExpression"> Аргумент, который будет подменяться в выражении. </param>
        /// <param name="replacementExpression"> Текущая версия сформированного выражения. </param>
        /// <param name="sourceTree"> Выражение, в котором будет производиться замена. </param>
        /// <returns> Сформированное в модель выражение (замена аргумента не произведена, поскольку необходима привязка к другому типу). </returns>
        public static Expression Replace(Expression replacedExpression, Expression replacementExpression, Expression sourceTree)
        {
            return new ReplacingExpressionTreeVisitorWithProperType(replacedExpression, replacementExpression).VisitExpression(sourceTree);
        }

        /// <summary>
        /// Перевод в модель полученного выражения.
        /// </summary>
        /// <param name="expression"> Выражение. </param>
        /// <returns> Преобразованное во внутреннее представление выражение. </returns>
        public override Expression VisitExpression(Expression expression)
        {
            return Equals(expression, _replacedExpression)
                ? _replacementExpression
                : base.VisitExpression(expression);
        }

        /// <summary>
        /// Перевод в модель полученного подзапроса.
        /// </summary>
        /// <param name="expression"> Подзапрос. </param>
        /// <returns> Преобразованный во внутреннее представление подзапрос. </returns>
        protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
        {
            expression.QueryModel.TransformExpressions(((ExpressionTreeVisitor)this).VisitExpression);
            return expression;
        }

        /// <summary>
        /// Перевод в модель полученной неизвестной структуры.
        /// </summary>
        /// <param name="expression"> Неизвестная структура. </param>
        /// <returns> Вместо исключения как в базовом типе будет просто возвращено выражение без изменений. </returns>
        protected override Expression VisitUnknownNonExtensionExpression(Expression expression)
        {
            return expression;
        }

        /// <summary>
        /// Перевод в модель аргумента.
        /// </summary>
        /// <param name="expression"> Аргумент выражения. </param>
        /// <returns> Вернётся аргумент, преобразование и привязка выполнена не будет, поскольку привязка будет пытаться выполниться для другого типа. </returns>
        protected override Expression VisitMemberExpression(MemberExpression expression)
        { // Данный класс затеян ради переопределения данного метода
            return expression;
        }
    }
}
