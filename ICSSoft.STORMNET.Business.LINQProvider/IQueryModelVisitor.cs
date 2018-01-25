namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System.Collections.Generic;

    using Remotion.Linq;

    /// <summary>
    /// Интерфейс для основного visitor'а, который обрабатывает запрос linq-выражения для <see cref="LcsQueryProvider{T,Q}"/>.
    /// </summary>
    public interface IQueryModelVisitor
    {
        /// <summary>
        /// Создать экземпляр visitor'а для обработки дерева выражения для <see cref="LcsQueryProvider{T,Q}"/>.
        /// </summary>
        /// <param name="viewIsDynamic">Динамически создавать представление.</param>
        /// <param name="view">Представление, если было указано.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns>Экземпляр visitor'а для обработки дерева выражения для <see cref="LcsQueryProvider{T,Q}"/>.</returns>
        IExpressionTreeVisitor GetExpressionTreeVisitor(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews);

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для запроса linq-выражения.
        /// </summary>
        /// <param name="queryModel">Запроса linq-выражения.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/> полученный для запроса linq-выражения.</returns>
        LoadingCustomizationStruct GenerateLcs(QueryModel queryModel);
    }
}
