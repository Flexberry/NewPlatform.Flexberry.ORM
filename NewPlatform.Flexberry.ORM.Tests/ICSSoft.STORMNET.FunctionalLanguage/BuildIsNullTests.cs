namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIsNullTests : BaseFunctionTest
    {
        #region IsNull

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildIsNullTest11()
        {
            FunctionBuilder.BuildIsNull(NullString);
        }

        [TestMethod]
        public void BuildIsNullTest12()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, StringVarDef),
                FunctionBuilder.BuildIsNull(PropertyName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildIsNullTest21()
        {
            FunctionBuilder.BuildIsNull(NullVarDef);
        }

        [TestMethod]
        public void BuildIsNullTest22()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, StringVarDef),
                FunctionBuilder.BuildIsNull(StringVarDef));
        }

        [TestMethod]
        public void BuildIsNullTest31()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, IntGenVarDef),
                FunctionBuilder.BuildIsNull<TestDataObject>(x => x.Height));
        }

        [TestMethod]
        public void BuildIsNullTest32()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, DateGenVarDef),
                FunctionBuilder.BuildIsNull<TestDataObject>(x => x.BirthDate));
        }

        [TestMethod]
        public void BuildIsNullTest33()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, StringGenVarDef),
                FunctionBuilder.BuildIsNull<TestDataObject>(x => x.Name));
        }

        #endregion
    }
}
