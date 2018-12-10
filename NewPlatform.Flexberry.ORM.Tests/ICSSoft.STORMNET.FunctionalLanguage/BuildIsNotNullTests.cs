namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIsNotNullTests : BaseFunctionTest
    {
        #region NotIsNull

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotIsNullTest11()
        {
            FunctionBuilder.BuildIsNotNull(NullString);
        }

        [TestMethod]
        public void BuildNotIsNullTest12()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, StringVarDef),
                FunctionBuilder.BuildIsNotNull(PropertyName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotIsNullTest21()
        {
            FunctionBuilder.BuildIsNotNull(NullVarDef);
        }

        [TestMethod]
        public void BuildNotIsNullTest22()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, StringVarDef),
                FunctionBuilder.BuildIsNotNull(StringVarDef));
        }
        
        [TestMethod]
        public void BuildNotIsNullTest31()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, IntGenVarDef),
                FunctionBuilder.BuildIsNotNull<TestDataObject>(x => x.Height));
        }

        [TestMethod]
        public void BuilddNotIsNullTest32()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, DateGenVarDef),
                FunctionBuilder.BuildIsNotNull<TestDataObject>(x => x.BirthDate));
        }

        [TestMethod]
        public void BuilddNotIsNullTest33()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, StringGenVarDef),
                FunctionBuilder.BuildIsNotNull<TestDataObject>(x => x.Name));
        }
        #endregion
    }
}
