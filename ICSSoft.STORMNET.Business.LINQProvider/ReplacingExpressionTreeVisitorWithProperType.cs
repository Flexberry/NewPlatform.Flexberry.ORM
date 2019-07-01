namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System.Linq.Expressions;

    /// <summary>
    /// Данный класс представляет собой по сути ReplacingExpressionTreeVisitor с выправленным нужным образом методом VisitMemberExpression,
    /// который убирает привязку свойства псевдодетейла к объекту типа мастера.
    /// </summary>
    public class ReplacingExpressionTreeVisitorWithProperType : ExpressionVisitor
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
            return new ReplacingExpressionTreeVisitorWithProperType(replacedExpression, replacementExpression).Visit(sourceTree);
        }

        /// <summary>
        /// Перевод в модель полученного выражения.
        /// </summary>
        /// <param name="expression"> Выражение. </param>
        /// <returns> Преобразованное во внутреннее представление выражение. </returns>
        public override Expression Visit(Expression expression)
        {
            return Equals(expression, _replacedExpression)
                ? _replacementExpression
                : base.Visit(expression);
        }

        /// <summary>
        /// Перевод в модель аргумента.
        /// </summary>
        /// <param name="expression"> Аргумент выражения. </param>
        /// <returns> Вернётся аргумент, преобразование и привязка выполнена не будет, поскольку привязка будет пытаться выполниться для другого типа. </returns>
        protected override Expression VisitMember(MemberExpression expression)
        { // Данный класс затеян ради переопределения данного метода
            return expression;
        }
    }
}
