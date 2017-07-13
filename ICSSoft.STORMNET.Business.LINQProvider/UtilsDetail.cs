namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class UtilsDetail
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get owner connect prop.
        /// </summary>
        /// <param name="fromExpr">
        /// The from expr.
        /// </param>
        /// <returns>
        /// The <see cref="string[]"/>.
        /// </returns>
        public static string[] GetOwnerConnectProp(Expression fromExpr)
        {
            string res = SQLWhereLanguageDef.StormMainObjectKey;

            var memberExpr = fromExpr as MemberExpression;
            if (memberExpr != null)
            {
                Expression innerExpression = memberExpr.Expression;
                MemberInfo parentMember = UtilsLcs.GetObjectPropertyValue(innerExpression, "Member");
                if (parentMember != null)
                {
                    res = parentMember.Name;
                }
            }

            return new[] { res };
        }

        /// <summary>
        /// Ищет View для типа
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="resolvingViews">
        /// The resolving Views.
        /// </param>
        /// <returns>
        /// The <see cref="View"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static View GetResolvedView(Type type, IEnumerable<View> resolvingViews)
        {
            // foreach по null падает
            if (resolvingViews != null)
            {
                foreach (View resolvingView in resolvingViews)
                {
                    if (resolvingView.DefineClassType == type)
                    {
                        return resolvingView;
                    }
                }
            }

            throw new Exception(string.Format("Укажите дополнительно представление для типа '{0}'", type.FullName));
        }

        #endregion
    }
}