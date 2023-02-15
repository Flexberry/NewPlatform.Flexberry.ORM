namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="LinqToLcs"/> and <see cref="System.Nullable"/> type.
    /// </summary>
    public class LinqToLcsNullableTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Method for test Nullable attributes limit functions.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableCharIsNullFunction()
        {
            var testProvider = new TestLcsQueryProvider<FullTypesMaster1>();
            new Query<FullTypesMaster1>(testProvider).Where(o => o.PoleNullChar == null).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            Function function = ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.BoolType, nameof(FullTypesMaster1.PoleNullChar)));
            LoadingCustomizationStruct expected = new LoadingCustomizationStruct(null) { LimitFunction = function };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMaster1.Views.FullMasterView);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Method for test Nullable attributes limit functions.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableBoolIsNullFunction()
        {
            var testProvider = new TestLcsQueryProvider<ТипПороды>();
            new Query<ТипПороды>(testProvider).Where(o => o.Актуально == null).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            Function function = ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.BoolType, nameof(ТипПороды.Актуально)));
            LoadingCustomizationStruct expected = new LoadingCustomizationStruct(null) { LimitFunction = function };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, ТипПороды.Views.Актуальность);
            Assert.True(Equals(expected, actual));
        }
    }
}
