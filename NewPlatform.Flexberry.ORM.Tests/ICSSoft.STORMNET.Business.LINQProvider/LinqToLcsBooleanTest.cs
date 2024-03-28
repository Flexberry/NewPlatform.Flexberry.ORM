namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System.Linq;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="LinqToLcs"/> and <see cref="bool"/> type.
    /// </summary>
    public class LinqToLcsBooleanTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        [Fact]
        public void GetLcsTestBooleanTrueFunction()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // ReSharper disable NegativeEqualityExpression
            new Query<Кошка>(testProvider).Where(o => true).ToList();
            // ReSharper restore NegativeEqualityExpression
            Expression queryExpression = testProvider.InnerExpression;
            var lf = UtilsLcs.GetTrueFunc();
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestBooleanFalseFunction()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // ReSharper disable NegativeEqualityExpression
            new Query<Кошка>(testProvider).Where(o => false).ToList();
            // ReSharper restore NegativeEqualityExpression
            Expression queryExpression = testProvider.InnerExpression;
            var lf = UtilsLcs.GetFalseFunc();
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestBooleanNotFunction()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // ReSharper disable NegativeEqualityExpression
            new Query<Кошка>(testProvider).Where(o => !(o.Кличка == "кошка")).ToList();
            // ReSharper restore NegativeEqualityExpression
            Expression queryExpression = testProvider.InnerExpression;

            var lf = ldef.GetFunction(
                ldef.funcNOT, ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "Кличка"), "кошка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestBooleanVariableDef()
        {
            var testProvider = new TestLcsQueryProvider<ТипЛапы>();
            new Query<ТипЛапы>(testProvider).Where(o => o.Актуально).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.BoolType, "Актуально"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(ТипЛапы)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestBooleanNotVariableDef()
        {
            var testProvider = new TestLcsQueryProvider<ТипЛапы>();
            new Query<ТипЛапы>(testProvider).Where(o => !o.Актуально).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var lf = ldef.GetFunction(
                ldef.funcNOT, ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.BoolType, "Актуально")));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(ТипЛапы)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestBooleanVariableDefExplicitComparison()
        {
            var testProvider = new TestLcsQueryProvider<ТипЛапы>();
            new Query<ТипЛапы>(testProvider).Where(o => o.Актуально == true).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.BoolType, Information.ExtractPropertyPath<ТипЛапы>(x => x.Актуально)));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(ТипЛапы)));
            Assert.True(Equals(expected, actual));
        }
    }
}
