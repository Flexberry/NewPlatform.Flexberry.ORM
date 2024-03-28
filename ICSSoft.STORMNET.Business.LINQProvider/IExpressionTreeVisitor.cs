namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET.FunctionalLanguage;

    /// <summary>
    /// Интерфейс для visitor'а, который обрабатывает дерево выражения для <see cref="LcsQueryProvider{T,Q}"/>.
    /// </summary>
    public interface IExpressionTreeVisitor
    {
        /// <summary>
        /// Создать экземпляр visitor'а для обработки запроса linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.
        /// </summary>
        /// <param name="viewIsDynamic">Динамически создавать представление.</param>
        /// <param name="view">Представление, если было указано.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns>Экземпляр visitor'а для обработки запроса linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.</returns>
        IQueryModelVisitor GetQueryModelVisitor(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews);

        /// <summary>
        /// Получить функцию ограничения для <see cref="LoadingCustomizationStruct"/>.
        /// </summary>
        /// <param name="linqExpression">Linq-выражение, для которого необходимо получить функцию ограничения.</param>
        /// <returns>Функция ограничения для <see cref="LoadingCustomizationStruct"/>.</returns>
        Function GetLcsExpression(Expression linqExpression);
    }
}
