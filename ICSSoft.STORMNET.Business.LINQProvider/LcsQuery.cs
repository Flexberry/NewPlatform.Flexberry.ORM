namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;

    /// <summary>
    /// The query.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class LcsQuery<T, Q> : Query<T> where T : DataObject where Q : IQueryModelVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{T}"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public LcsQuery(LcsQueryProvider<T, Q> provider) : base(provider)
        {
        }
    }
}