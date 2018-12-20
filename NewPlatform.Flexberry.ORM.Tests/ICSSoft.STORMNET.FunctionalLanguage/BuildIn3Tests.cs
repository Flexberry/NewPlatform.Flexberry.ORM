namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildIn3Tests : BaseFunctionTest
    {
        private static readonly ObjectType DataObjectType = LangDef.DataObjectType;

        [Fact]
        public void BuildInTest300()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(PropertyName, DataObjectType));
        }
    }
}
