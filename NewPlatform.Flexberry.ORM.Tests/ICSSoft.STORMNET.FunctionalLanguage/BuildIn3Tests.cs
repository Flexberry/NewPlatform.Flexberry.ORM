namespace IIS.University.Tools.Tests
{
    using ICSSoft.STORMNET.FunctionalLanguage;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIn3Tests : BaseFunctionTest
    {
        private static readonly ObjectType DataObjectType = LangDef.DataObjectType;

        [TestMethod]
        public void BuildInTest300()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(PropertyName, DataObjectType));
        }
    }
}
