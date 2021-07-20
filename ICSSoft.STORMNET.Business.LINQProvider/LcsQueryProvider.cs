namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LcsQueryProvider<T, Q> : QueryProvider
        where T : DataObject where Q : IQueryModelVisitor
    {
        private IDataService _ds;

        protected View View;

        private IEnumerable<View> _resolvingViews;

        public LcsQueryProvider(IDataService ds, View view, IEnumerable<View> resolvingViews)
        {
            _ds = ds;
            View = view;
            _resolvingViews = resolvingViews;
        }

        public override object Execute(Expression expression)
        {
            LoadingCustomizationStruct lcs = GetLcs(expression);
            lcs.LoadingTypes = new[] { typeof(T) };

            switch (lcs.ReturnType)
            {
                case LcsReturnType.Object:
                    {
                        DataObject[] objects = _ds.LoadObjects(lcs);
                        return !objects.Any() ? null : this._ds.LoadObjects(lcs)[0];
                    }

                case LcsReturnType.ObjectRequired:
                    {
                        DataObject[] objects = _ds.LoadObjects(lcs);
                        if (!objects.Any())
                        {
                            throw new InvalidOperationException("Sequence contains no matching element");
                        }

                        return _ds.LoadObjects(lcs)[0];
                    }

                case LcsReturnType.Objects:
                    return _ds.LoadObjects(lcs).Cast<T>();
                case LcsReturnType.All:
                    // todo: выполняется два запроса из-за ограниченных возможностей сервиса данных
                    var conditionalCount = _ds.GetObjectsCount(lcs);
                    lcs.LimitFunction = null;
                    return conditionalCount == _ds.GetObjectsCount(lcs);
                case LcsReturnType.Any:
                    return _ds.GetObjectsCount(lcs) > 0;
                case LcsReturnType.Count:
                    return _ds.GetObjectsCount(lcs);
                default:
                    throw new Exception("Linq провайдер не поддерживает тип: " + lcs.ReturnType);
            }
        }

        public override string GetQueryText(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для linq-выражения.
        /// </summary>
        /// <param name="expression">Linq-выражение, по которому необходимо сформировать <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученное для linq-выражения.</returns>
        protected virtual LoadingCustomizationStruct GetLcs(Expression expression)
        {
            // Представление динамическое.
            if (View == null)
            {
                return LinqToLcs.GetLcs<T, Q>(expression);
            }

            // Представление задано.
            var lcs = LinqToLcs.GetLcs<Q>(expression, View, _resolvingViews);
            lcs.View = View;

            return lcs;
        }
    }
}
