namespace IIS.University.Tools.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildOrTests : BaseFunctionTest
    {
        #region Or

        [TestMethod]
        public void BuildOrTest11()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildOr());
        }

        [TestMethod]
        public void BuildOrTest12()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildOr(NullFunction));
        }

        [TestMethod]
        public void BuildOrTest13()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildOr(FuncTrue));
        }

        [TestMethod]
        public void BuildOrTest14()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildOr(FuncTrue, NullFunction));
        }

        [TestMethod]
        public void BuildOrTest15()
        {
            Assert.AreEqual(LangDef.GetFunction(LangDef.funcOR, FuncTrue, FuncFalse), FunctionBuilder.BuildOr(FuncTrue, FuncFalse));
        }

        [TestMethod]
        public void BuildOrTest21()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildOr((IEnumerable<Function>)null));
        }

        [TestMethod]
        public void BuildOrTest22()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildOr(Enumerable.Empty<Function>()));
        }

        [TestMethod]
        public void BuildOrTest23()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildOr(new List<Function> { FuncTrue }));
        }

        [TestMethod]
        public void BuildOrTest24()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildOr(new List<Function> { FuncTrue, NullFunction }));
        }

        [TestMethod]
        public void BuildOrTest25()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcOR, FuncTrue, FuncFalse),
                FunctionBuilder.BuildOr(new List<Function> { FuncTrue, FuncFalse }));
        }

        #endregion
    }
}
