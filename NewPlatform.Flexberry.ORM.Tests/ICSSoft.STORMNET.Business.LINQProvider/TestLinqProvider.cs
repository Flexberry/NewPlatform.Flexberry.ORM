namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using Xunit;

    /// <summary>
    /// Проверка методов преобразования LINQ-выражений в LCS-структуры.
    /// </summary>
    public class TestLinqProvider
    {
        /// <summary>
        /// Провайдер запросов для целей тестирования преобразователя LINQ-выражений.
        /// </summary>
        private class TestQueryProvider<T> : IQueryProvider
        {
            /// <summary>
            /// Выражение, которое продуцирует этот провайдер.
            /// </summary>
            public Expression InnerExpression { get; set; }

            /// <summary>
            /// Тип возвращаемого значения.
            /// </summary>
            private LcsReturnType _lcsReturnType;

            /// <summary>
            /// Конструктор по умолчанию, устанавливающий тип значения по умолчанию в <see cref="LcsReturnType.Objects"/>.
            /// </summary>
            public TestQueryProvider()
            {
                _lcsReturnType = LcsReturnType.Objects;
            }

            /// <summary>
            /// Создание выражения.
            /// </summary>
            /// <typeparam name="TS">Тип, для которого строится выражение.</typeparam>
            /// <param name="expression">Выражение, которое строится для данного провайдера.</param>
            /// <returns>Результат выполнения выражения.</returns>
            IQueryable<TS> IQueryProvider.CreateQuery<TS>(Expression expression)
            {
                return new Query<TS>(this, expression);
            }

            /// <summary>
            /// Создание выражения.
            /// </summary>
            /// <param name="expression">Выражение, которое строится для данного провайдера.</param>
            /// <returns>Результат выполнения выражения.</returns>
            IQueryable IQueryProvider.CreateQuery(Expression expression)
            {
                Type elementType = expression.Type;
                try
                {
                    return
                        (IQueryable)
                        Activator.CreateInstance(
                            typeof(Query<>).MakeGenericType(elementType), this, expression);
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }
            }

            /// <summary>
            /// Выполнение выражения.
            /// </summary>
            /// <typeparam name="TS">Тип, для которого строится выражение.</typeparam>
            /// <param name="expression">Выражение, которое строится для данного провайдера.</param>
            /// <returns>Результат выполнения выражения.</returns>
            TS IQueryProvider.Execute<TS>(Expression expression)
            {
                InnerExpression = expression;
                return default(TS);
            }

            /// <summary>
            /// Выполнение выражения.
            /// </summary>
            /// <param name="expression">Выражение, которое строится для данного провайдера.</param>
            /// <returns>Результат выполнения выражения.</returns>
            object IQueryProvider.Execute(Expression expression)
            {
                return Execute(expression);
            }

            /// <summary>
            /// Выполнение выражения.
            /// </summary>
            /// <param name="expression">Выражение, которое строится для данного провайдера.</param>
            /// <returns>Результат выполнения выражения.</returns>
            private object Execute(Expression expression)
            {
                InnerExpression = expression;
                switch (_lcsReturnType)
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

        /// <summary>
        /// Тип объекта данных для тестирования <see cref="LINQProvider"/>.
        /// </summary>
        private class LinqProviderTestDataObject : global::ICSSoft.STORMNET.DataObject
        {
            /// <summary>
            /// Свойство объекта данных типа <see cref="object"/>.
            /// </summary>
            public object ObjectProperty { get; set; }
        }

        /// <summary>
        /// Проверка преобразования выражения, в котором в качестве параметра передаётся объект типа object.
        /// </summary>
        [Fact]
        public void TestEqualsExpressionWithObjectParameter()
        {
            // Arrange.
            var testProvider = new TestQueryProvider<LinqProviderTestDataObject>();
            object value = "{72FCA622-A01E-494C-BE1C-0821178594FB}";
            new Query<LinqProviderTestDataObject>(testProvider).Where(o => o.ObjectProperty == value).ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            Function expected = langDef.GetFunction(langDef.funcEQ, new VariableDef(langDef.StringType, nameof(LinqProviderTestDataObject.ObjectProperty)), value);

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, typeof(LinqProviderTestDataObject));

            // Assert.
            Assert.Equal(expected, actual.LimitFunction);
        }

        /// <summary>
        /// Проверка преобразования выражения, в котором в качестве параметра передаётся объект типа object.
        /// </summary>
        [Fact]
        public void TestContainsExpressionWithObjectParameter()
        {
            // Arrange.
            var testProvider = new TestQueryProvider<LinqProviderTestDataObject>();
            string value = "{72FCA622-A01E-494C-BE1C-0821178594FB}";

            new Query<LinqProviderTestDataObject>(testProvider).Where(o => ((string)o.ObjectProperty).Contains(value)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            Function expected = langDef.GetFunction(langDef.funcLike, new VariableDef(langDef.StringType, nameof(LinqProviderTestDataObject.ObjectProperty)), "%" + value + "%");

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, typeof(LinqProviderTestDataObject));

            // Assert.
            Assert.Equal(expected, actual.LimitFunction);
        }
    }
}
