namespace IIS.University.Tools.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIn5Tests : BaseFunctionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest500()
        {
            FunctionBuilder.BuildIn(NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest501()
        {
            FunctionBuilder.BuildIn(NullVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest502()
        {
            FunctionBuilder.BuildIn(GuidVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest503()
        {
            FunctionBuilder.BuildIn(NullLambda, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest504()
        {
            FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, NullFunction);
        }

        [TestMethod]
        public void BuildInTest510()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, PrimaryKeyVarDef, FuncSQL),
                FunctionBuilder.BuildIn(FuncSQL));
        }

        [TestMethod]
        public void BuildInTest511()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, FuncSQL),
                FunctionBuilder.BuildIn(GuidVarDef, FuncSQL));
        }

        [TestMethod]
        public void BuildInTest512()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, IntGenVarDef, FuncSQL),
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, FuncSQL));
        }
    }
}