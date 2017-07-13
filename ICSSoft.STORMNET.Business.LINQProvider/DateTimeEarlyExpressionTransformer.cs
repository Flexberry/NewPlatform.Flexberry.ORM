namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    using Remotion.Linq.Parsing.ExpressionTreeVisitors;
    using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DateTimeEarlyExpressionTransformer : IExpressionTransformer<Expression>
    {
        public Expression Transform(Expression expression)
        {
            if (expression is MemberExpression)
            {
                var memberexp = expression as MemberExpression;
                if (memberexp.Expression is ConstantExpression)
                {
                    switch (memberexp.Member.Name)
                    {
                        case "Minute":
                        case "Day":
                        case "Month":
                        case "Year":
                        case "Hour":
                        case "Date":
                            return expression;
                   }
                    return PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees(expression);
                }
             
                var member = UtilsLcs.GetObjectPropertyValue(expression, "Member");
                if (member.DeclaringType == typeof(DateTime))
                {
                    switch (member.Name)
                    {
                        case "Now":
                        case "Date":
                        case "Today":
                        case "DayOfWeek":
                        case "Day":
                        case "Month":
                        case "Year":
                        case "Hour":
                        case "Minute":
                        case "TimeOfDay":
                            return expression;
                    }
                }
            }

            // Может быть не к месту, но главное чтобы evaluate делался
            return PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees(expression);
        }

        public ExpressionType[] SupportedExpressionTypes
        {
            get
            {
                return new[] { ExpressionType.MemberAccess };
            }
        }
    }
}
