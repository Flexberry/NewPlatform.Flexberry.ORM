namespace NewPlatform.Flexberry.ORM.IntegratedTests.LINQProvider
{
    using System.Linq;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.Business.LINQProvider.Tests;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;

    using Xunit;
    using Tests;

    /// <summary>
    /// Класс для проверки доработок в провайдер, сделанных для взаимодействия с ODataService.
    /// </summary>
    public class TestLinqProviderLogicForOData
    {
        /// <summary>
        /// Язык задания ограничений.
        /// </summary>
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Проверка, что выражение вида "Условие == true" заменится просто на "Условие".
        /// </summary>
        [Fact]
        public void TestODataRemoveEqualTrue()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            new Query<FullTypesMainAgregator>(testProvider)
                .Where(o => o.PoleString.Contains("test") == true)
                .ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcLike,
                        new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleString)),
                        "%test%"),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка, что выражение вида "IIF(Поле == null || Значение == null, null, Convert(Поле.Contains(Значение))) == true" заменится просто на "Convert(Поле.Contains(Значение))".
        /// При этом "true" передаётся как Convert(true) в тип <see cref="bool?"/>.
        /// </summary>
        [Fact]
        public void TestODataIifContainsNullable()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            new Query<FullTypesMainAgregator>(testProvider)
                .Where(o => (o.PoleString == null ? null : (bool?)o.PoleString.Contains("test")) == true)
                .ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcLike,
                        new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleString)),
                        "%test%"),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка, что выражение вида "IIF(Поле == null || Значение == null, null, Convert(Поле.Contains(Значение))) == true" заменится просто на "Convert(Поле.Contains(Значение))".
        /// При этом "true" передаётся как константа типа <see cref="bool?"/>.
        /// </summary>
        [Fact]
        public void TestODataIifContainsNullableWithoutRightConvert()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            var rightValue = (bool?)true;
            new Query<FullTypesMainAgregator>(testProvider)
                .Where(o => (o.PoleString == null ? null : (bool?)o.PoleString.Contains("test")) == rightValue)
                .ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcLike,
                        new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleString)),
                        "%test%"),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка обработки выражения с двумя iif, где в одном случае происходит обращение к свойству мастера.
        /// </summary>
        [Fact]
        public void TestODataIifWithOr()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            var rightValue = (bool?)true;
            new Query<FullTypesMainAgregator>(testProvider)
                .Where(o => ((bool)(o.PoleString == null ? null : (bool?)o.PoleString.Contains("test")) || ((o.FullTypesMaster1 == null ? null : o.FullTypesMaster1.PoleString) == "test")) == rightValue)
                .ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcOR,
                        this.ldef.GetFunction(
                            this.ldef.funcLike,
                            new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleString)),
                            "%test%"),
                        this.ldef.GetFunction(
                            this.ldef.funcEQ,
                            new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.FullTypesMaster1.PoleString)),
                            "test")),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }
    }
}
