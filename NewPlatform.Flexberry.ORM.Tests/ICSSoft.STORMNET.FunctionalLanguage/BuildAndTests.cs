namespace IIS.University.Tools.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildAndTests : BaseFunctionTest
    {
        #region And

        [TestMethod]
        public void BuildAndTest10()
        {
            Function[] nullFunctions = null;
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildAnd(nullFunctions));
        }

        [TestMethod]
        public void BuildAndTest11()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildAnd());
        }

        [TestMethod]
        public void BuildAndTest12()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildAnd(NullFunction));
        }

        [TestMethod]
        public void BuildAndTest13()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildAnd(FuncTrue));
        }

        [TestMethod]
        public void BuildAndTest14()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildAnd(FuncTrue, NullFunction));
        }

        [TestMethod]
        public void BuildAndTest15()
        {
            Assert.AreEqual(LangDef.GetFunction(LangDef.funcAND, FuncTrue, FuncFalse), FunctionBuilder.BuildAnd(FuncTrue, FuncFalse));
        }

        [TestMethod]
        public void BuildAndTest21()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildAnd((IEnumerable<Function>)null));
        }

        [TestMethod]
        public void BuildAndTest22()
        {
            Assert.AreEqual(NullFunction, FunctionBuilder.BuildAnd(Enumerable.Empty<Function>()));
        }

        [TestMethod]
        public void BuildAndTest23()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildAnd(new List<Function> { FuncTrue }));
        }

        [TestMethod]
        public void BuildAndTest24()
        {
            Assert.AreEqual(FuncTrue, FunctionBuilder.BuildAnd(new List<Function> { FuncTrue, NullFunction }));
        }

        [TestMethod]
        public void BuildAndTest25()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcAND, FuncTrue, FuncFalse),
                FunctionBuilder.BuildAnd(new List<Function> { FuncTrue, FuncFalse }));
        }

        #endregion
    }
}
