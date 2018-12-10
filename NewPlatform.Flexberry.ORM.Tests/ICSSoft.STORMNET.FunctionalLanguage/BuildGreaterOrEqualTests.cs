namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildGreaterOrEqualTests : BaseFunctionTest
    {
        #region GreaterOrEqual

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest11()
        {
            FunctionBuilder.BuildGreaterOrEqual(NullString, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest12()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, NullObject);
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest13()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest21()
        {
            FunctionBuilder.BuildGreaterOrEqual(NullString, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest22()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest23()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, LangDef.NumericType, NullObject);
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest24()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, LangDef.NumericType, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest31()
        {
            FunctionBuilder.BuildGreaterOrEqual(NullVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest32()
        {
            FunctionBuilder.BuildGreaterOrEqual(NumericVarDef, NullObject);
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest33()
        {
            FunctionBuilder.BuildGreaterOrEqual(NumericVarDef, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest41()
        {
            FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildGreaterOrEqualTest42()
        {
            FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest43()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, Int1));
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest44()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest52()
        {
            FunctionBuilder.BuildGreaterOrEqual(NullVarDef, StringGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest53()
        {
            FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterOrEqualTest54()
        {
            FunctionBuilder.BuildGreaterOrEqual(IntGenVarDef, DateGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterOrEqualTest55()
        {
            FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterOrEqualTest56()
        {
            FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, StringGenVarDef1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest61()
        {
            FunctionBuilder.BuildGreaterOrEqual(NullLambda, x => x.Hierarchy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterOrEqualTest62()
        {
            FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterOrEqualTest63()
        {
            FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterOrEqualTest64()
        {
            FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest71()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest72()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest73()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest74()
        {

            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildGreaterOrEqualTest75()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
