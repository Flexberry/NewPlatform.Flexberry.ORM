namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Explicit interface implementation of IQueryable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Query<T> : IOrderedQueryable<T> // where T : new()
    {
        // QueryProvider provider;
        private IQueryProvider provider;

        private Expression expression;

        /// <summary>
        /// Выражение.
        /// </summary>
        public Expression Expression
        {
            get
            {
                return expression;
            }
        }

        // public Query(QueryProvider provider)
        public Query()
            : this(new T[1].AsQueryable().Provider)
        {
        }

        public Query(IQueryProvider provider)
        {
            this.provider = provider;
            this.expression = Expression.Constant(this); // this function implicitly calls the ToString method in Debug
        }

        public Query(IQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (
                !(typeof(IQueryable<T>).IsAssignableFrom(expression.Type)
                  || typeof(IEnumerable<T>).IsAssignableFrom(expression.Type)))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            this.provider = provider;
            this.expression = expression;
        }

        Expression IQueryable.Expression
        {
            get
            {
                return this.expression;
            }
        }

        Type IQueryable.ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                return this.provider;
            }
        }

        /// <summary>
        /// on the call to any of the System.Linq extension methods on IEnumerable{T}, this
        /// method will get called.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.provider.Execute(this.expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }
    }
}
