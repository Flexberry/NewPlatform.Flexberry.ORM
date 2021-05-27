namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using ICSSoft.STORMNET.Exceptions;

    /// <summary>
    /// The linq to lcs.
    /// </summary>
    public static class LinqToLcs
    {
        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для указанного linq-выражения по представлению.
        /// Будет использован <see cref="LcsGeneratorQueryModelVisitor"/> в качестве основного visitor'а для разбора запроса linq-выражения.
        /// </summary>
        /// <param name="queryExpression">Linq-выражение, по которому будет сформирован <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="view">Представление, по которому будут зачитаны данные.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученный для указанного linq-выражения по представлению.</returns>
        public static LoadingCustomizationStruct GetLcs(Expression queryExpression, View view, IEnumerable<View> resolvingViews = null)
        {
            return GetLcs<LcsGeneratorQueryModelVisitor>(queryExpression, view, resolvingViews);
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для указанного linq-выражения с динамически созданным представлением.
        /// Будет использован <see cref="LcsGeneratorQueryModelVisitor"/> в качестве основного visitor'а для разбора запроса linq-выражения.
        /// </summary>
        /// <param name="queryExpression">Linq-выражение, по которому будет сформирован <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученный для указанного linq-выражения с динамически созданным представлением.</returns>
        public static LoadingCustomizationStruct GetLcs<T>(Expression queryExpression)
            where T : DataObject
        {
            return GetLcs<LcsGeneratorQueryModelVisitor>(queryExpression, typeof(T));
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для указанного linq-выражения по представлению.
        /// </summary>
        /// <param name="queryExpression">Linq-выражение, по которому будет сформирован <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="view">Представление, по которому будут зачитаны данные.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученный для указанного linq-выражения по представлению.</returns>
        public static LoadingCustomizationStruct GetLcs<Q>(Expression queryExpression, View view, IEnumerable<View> resolvingViews)
            where Q : IQueryModelVisitor
        {
            var queryModel = UtilsLcs.CreateQueryParser().GetParsedQuery(queryExpression);
            return GetQueryModelVisitor<Q>(false, view, resolvingViews).GenerateLcs(queryModel);
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для указанного linq-выражения с динамически созданным представлением.
        /// </summary>
        /// <param name="queryExpression">Linq-выражение, по которому будет сформирован <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученный для указанного linq-выражения с динамически созданным представлением.</returns>
        public static LoadingCustomizationStruct GetLcs<T, Q>(Expression queryExpression)
            where T : DataObject where Q : IQueryModelVisitor
        {
            return GetLcs<Q>(queryExpression, typeof(T));
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для указанного linq-выражения с динамически созданным представлением.
        /// </summary>
        /// <param name="queryExpression">Linq-выражение, по которому будет сформирован <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="type">Тип объекта данных, для которого необходимо сформировать <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученный для указанного linq-выражения с динамически созданным представлением.</returns>
        public static LoadingCustomizationStruct GetLcs<Q>(Expression queryExpression, Type type)
            where Q : IQueryModelVisitor
        {
            var queryModel = UtilsLcs.CreateQueryParser().GetParsedQuery(queryExpression);

            if (!type.IsSubclassOf(typeof(DataObject)))
            {
                throw new ArgumentException(string.Format("Тип \"{0}\" должен наследовать DataObject", type));
            }

            var view = new View
            {
                DefineClassType = type,
                Name = string.Format("DynamicViewFor{0}", type.FullName),
            };

            return GetQueryModelVisitor<Q>(true, view, null).GenerateLcs(queryModel);
        }

        /// <summary>
        /// Получить <see cref="LoadingCustomizationStruct"/> для указанного linq-выражения с динамически созданным представлением.
        /// Будет использован <see cref="LcsGeneratorQueryModelVisitor"/> в качестве основного visitor'а для разбора запроса linq-выражения.
        /// </summary>
        /// <param name="queryExpression">Linq-выражение, по которому будет сформирован <see cref="LoadingCustomizationStruct"/>.</param>
        /// <param name="type">Тип объекта данных, для которого необходимо сформировать <see cref="LoadingCustomizationStruct"/>.</param>
        /// <returns><see cref="LoadingCustomizationStruct"/>, полученный для указанного linq-выражения с динамически созданным представлением.</returns>
        public static LoadingCustomizationStruct GetLcs(Expression queryExpression, Type type)
        {
            return GetLcs<LcsGeneratorQueryModelVisitor>(queryExpression, type);
        }

        /// <summary>
        /// The call count.
        /// </summary>
        /// <param name="expressionToQuery">
        /// The expression to query.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public static Expression CallCount(Expression expressionToQuery, Type t)
        {
            var ee = Expression.Call(
                typeof(Enumerable), "Count", new[] { t }, expressionToQuery);

            return ee;
        }

        /// <summary>
        /// Из linq-выражения получаем lcs.
        /// </summary>
        /// <param name="whereExpression"> Linq-выражениe c ограничением. </param>
        /// <param name="returnType"> Тип возвращаемого linq-выражением значения. </param>
        /// <returns> Сформированное lcs. </returns>
        public static Expression GetExpressionToQueryFromWhereExpression(Expression whereExpression, Type returnType)
        {
            Type genericList = typeof(List<>);

            Type constructed = genericList.MakeGenericType(new[] { returnType });
            Type[] typeArgs = { };
            ConstructorInfo ctor = constructed.GetConstructor(typeArgs);

            object[] ctorparams = { };

            Debug.Assert(ctor != null, "ctor != null");
            var list = ctor.Invoke(ctorparams);

            MethodInfo mi = typeof(Queryable).GetMethod(
                "AsQueryable", BindingFlags.Static | BindingFlags.Public, null, new[] { genericList }, null);
            var iquer = (IQueryable)mi.Invoke(list, new[] { list });

            Expression expressionToQuery = whereExpression != null
                                                   ? Expression.Call(
                                                       typeof(Queryable),
                                                       "Where",
                                                       new[] { returnType },
                                                       iquer.Expression,
                                                       Expression.Quote(whereExpression))
                                                   : iquer.Expression;
            return expressionToQuery;
        }

        /// <summary>
        /// Возвращает IQueryable, делающий запросы к SQLDataService.
        /// </summary>
        /// <typeparam name="T">
        /// Тип объектов для загрузки.
        /// </typeparam>
        /// <param name="ds">
        /// Сервис данных.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="resolvingViews">
        /// The resolving Views.
        /// </param>
        /// <returns>
        /// IQueryable.
        /// </returns>
        public static IQueryable<T> Query<T>(this IDataService ds, View view, IEnumerable<View> resolvingViews = null)
            where T : DataObject
        {
            return new LcsQuery<T, LcsGeneratorQueryModelVisitor>(new LcsQueryProvider<T, LcsGeneratorQueryModelVisitor>(ds, view, resolvingViews));
        }

        /// <summary>
        /// Возвращает IQueryable, делающий запросы к SQLDataService, динамически формируя представление.
        /// </summary>
        /// <typeparam name="T">Тип объектов для загрузки.</typeparam>
        /// <param name="ds">Сервис данных.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable<T> Query<T>(this IDataService ds) where T : DataObject
        {
            return new LcsQuery<T, LcsGeneratorQueryModelVisitor>(new LcsQueryProvider<T, LcsGeneratorQueryModelVisitor>(ds, null, null));
        }

        /// <summary>
        /// The query.
        /// </summary>
        /// <param name="ds">
        /// The ds.
        /// </param>
        /// <param name="viewName">
        /// The view name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        /// <exception cref="CantFindViewException">
        /// Представление не может быть найдено.
        /// </exception>
        public static IQueryable<T> Query<T>(this IDataService ds, string viewName) where T : DataObject
        {
            var v = Information.GetView(viewName, typeof(T));
            if (v == null)
            {
                throw new CantFindViewException(typeof(T), viewName);
            }

            return new LcsQuery<T, LcsGeneratorQueryModelVisitor>(new LcsQueryProvider<T, LcsGeneratorQueryModelVisitor>(ds, v, null));
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="ds">
        /// The ds.
        /// </param>
        /// <param name="viewName">
        /// The view name.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <param name="expr">
        /// The expr.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object Execute(this SQLDataService ds, string viewName, Type t, Expression expr)
        {
            return ds.Execute(viewName, t, expr, null);
        }

        /// <summary>
        /// Получить значения по сформированному ограничению.
        /// Является расширением функциональности.
        /// </summary>
        /// <param name="ds">
        /// Сервис данных, для которого будет вызван данный метод.
        /// </param>
        /// <param name="viewName">
        /// Имя представления, по которому будет производиться вычитка.
        /// </param>
        /// <param name="t">
        /// Тип объектов, на который мы накладываем ограничение.
        /// </param>
        /// <param name="expr">
        /// Выражение, задающее ограничение.
        /// </param>
        /// <param name="resolvingViews">
        /// Представления, которые в некоторых местах используются для восполнения недостающих свойств в представлении.
        /// </param>
        /// <returns>
        /// Найденные в соответствии с ограничением элемента.
        /// </returns>
        /// <exception cref="CantFindViewException">
        /// Представление не может быть найдено.
        /// </exception>
        public static object Execute(this SQLDataService ds, string viewName, Type t, Expression expr, IEnumerable<View> resolvingViews)
        {
            var v = Information.GetView(viewName, t);
            if (v == null)
            {
                throw new CantFindViewException(t, viewName);
            }

            Type generic = typeof(LcsQueryProvider<,>);

            Type constructed = generic.MakeGenericType(new[] { t, typeof(LcsGeneratorQueryModelVisitor) });
            Type[] typeArgs = { typeof(SQLDataService), typeof(View), typeof(IEnumerable<View>) };
            ConstructorInfo ctor = constructed.GetConstructor(typeArgs);

            object[] ctorparams = { ds, v, resolvingViews };

            Debug.Assert(ctor != null, "ctor != null");
            var queryProvider = (IQueryProvider)ctor.Invoke(ctorparams);

            return queryProvider.Execute(expr);
        }

        /// <summary>
        /// Получить экземпляр visitor'а для обработки linq-выражения.
        /// </summary>
        /// <typeparam name="Q">Тип visitor'а для обработки linq-выражения.</typeparam>
        /// <param name="viewIsDynamic">Динамически создавать представление.</param>
        /// <param name="view">Представление, если было указано.</param>
        /// <param name="resolvingViews">Представления мастеров, необходимые для получения их детейлов, в случае динамических представлений. </param>
        /// <returns>Созданный экземпляр visitor'а для обработки linq-выражения.</returns>
        private static IQueryModelVisitor GetQueryModelVisitor<Q>(bool viewIsDynamic, View view, IEnumerable<View> resolvingViews)
            where Q : IQueryModelVisitor
        {
            ConstructorInfo constructor = typeof(Q).GetConstructor(new[] { typeof(bool), typeof(View), typeof(IEnumerable<View>) });
            return (Q)constructor.Invoke(new object[] { viewIsDynamic, view, resolvingViews });
        }
    }
}
