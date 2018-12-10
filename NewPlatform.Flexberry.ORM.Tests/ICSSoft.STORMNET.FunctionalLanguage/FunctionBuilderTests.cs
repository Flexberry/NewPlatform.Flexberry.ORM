namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FunctionBuilderTests : BaseFunctionTest
    {
        [TestMethod]
        public void BuildTrueTest()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, 1, 1),
                FunctionBuilder.BuildTrue());
        }

        [TestMethod]
        public void BuildFalseTest()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, 1, 0), 
                FunctionBuilder.BuildFalse());
        }

        [TestMethod]
        public void BuildSQLTest()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcSQL, "sql"), 
                FunctionBuilder.BuildSQL("sql"));
        }

        [TestMethod]
        public void BuildNotTest()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNOT, FuncTrue), 
                FunctionBuilder.BuildNot(FuncTrue));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotTest1()
        {
            FunctionBuilder.BuildNot(NullFunction);
        }
    }
}
