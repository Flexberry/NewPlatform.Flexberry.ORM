namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Вспомогательные методы для работы с параметрами.
    /// </summary>
    public static class ParamsUtils
    {
        /// <summary>
        /// Сгенерировать выражение для доступа к параметру (например, параметру ограничения).
        /// </summary>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="parameterName">Название параметра.</param>
        /// <returns>Выражение для доступа к параметру. Результирующее значение выражения
        /// будет иметь тип parameterType.</returns>
        public static Expression GenerateParameterAccess(Type parameterType, string parameterName)
        {
            var paramCall = Expression.Call(
                Expression.Parameter(typeof(ParamSet), "ParamSet"),
                "Get",
                new[] { parameterType },
                Expression.Constant(parameterName));
            return paramCall;
        }

        /// <summary>
        /// Является ли текущий рассматриваемый элемент вызовом параметра.
        /// </summary>
        /// <param name="checkExpression">Рассматриваемое выражение.</param>
        /// <returns><c>True</c>, если это вызов параметра.</returns>
        public static bool IsItParameter(Expression checkExpression)
        {
            MethodCallExpression paramMethodCall;
            return checkExpression.NodeType == ExpressionType.Call
                   && (paramMethodCall = (MethodCallExpression)checkExpression).Object != null
                   && paramMethodCall.Object.Type == typeof(ParamSet)
                   && paramMethodCall.Method.Name == "Get"
                   && paramMethodCall.Object.NodeType == ExpressionType.Parameter
                   && paramMethodCall.Arguments.Count == 1
                   && paramMethodCall.Arguments[0].NodeType == ExpressionType.Constant;
        }
    }
}
