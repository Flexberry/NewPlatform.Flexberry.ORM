namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Провайдер нужен для получения Expression в Unit-тестах.
    /// </summary>
    public class TestLcsQueryProvider<T> : IQueryProvider
    {
        public Expression InnerExpression { get; set; }

        private LcsReturnType _lcsReturnType;

        public TestLcsQueryProvider()
        {
            _lcsReturnType = LcsReturnType.Objects;
        }

        public TestLcsQueryProvider(LcsReturnType t)
        {
            _lcsReturnType = t;
        }

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
        {
            return new Query<S>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = expression.Type;
            try
            {
                return
                    (IQueryable)Activator.CreateInstance(
                        typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression)
        {
            InnerExpression = expression;
            return default(S);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        public object Execute(Expression expression)
        {
            InnerExpression = expression;
            switch (this._lcsReturnType)
            {
                case LcsReturnType.All:
                case LcsReturnType.Any:
                    return new List<bool>();
                case LcsReturnType.Count:
                    return new List<int>();
                default:
                    return new List<T>();
            }
        }
    }
}
