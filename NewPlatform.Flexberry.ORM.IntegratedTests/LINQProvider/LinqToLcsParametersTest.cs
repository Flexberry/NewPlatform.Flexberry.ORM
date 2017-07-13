namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System.Linq;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    
    public class LinqToLcsParametersTest
    {
        [Fact]
        public void GetLcsWithParameterDef()
        {
            var provider = new TestLcsQueryProvider<Кошка>();

            var ParamSet = new ParamSet();
            var query = new Query<Кошка>(provider).Where(c => c.Кличка == ParamSet.Get<string>("NameParam"));
            LoadingCustomizationStruct lcs =
                LinqToLcs.GetLcs(query.Expression, Utils.GetDefaultView(typeof(Кошка)));

            bool correctParameter = false;

            var parameterDef = lcs.LimitFunction.Parameters[1] as ParameterDef;
            if (parameterDef != null)
            {
                if (parameterDef.Type.NetCompatibilityType == typeof(string) &&
                    parameterDef.ParamName == "NameParam")
                {
                    correctParameter = true;
                }
            }

            Assert.True(correctParameter);
        }
    }
}
