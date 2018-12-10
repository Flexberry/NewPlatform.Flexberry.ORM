namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildLessOrEqualTests : BaseFunctionTest
    {
        #region LessOrEqual

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest11()
        {
            FunctionBuilder.BuildLessOrEqual(NullString, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest12()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, NullObject);
        }

        [TestMethod]
        public void BuildLessOrEqualTest13()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest21()
        {
            FunctionBuilder.BuildLessOrEqual(NullString, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest22()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest23()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, LangDef.NumericType, NullObject);
        }

        [TestMethod]
        public void BuildLessOrEqualTest24()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, LangDef.NumericType, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest31()
        {
            FunctionBuilder.BuildLessOrEqual(NullVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest32()
        {
            FunctionBuilder.BuildLessOrEqual(NumericVarDef, NullObject);
        }

        [TestMethod]
        public void BuildLessOrEqualTest33()
        {
            FunctionBuilder.BuildLessOrEqual(NumericVarDef, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest41()
        {
            FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildLessOrEqualTest42()
        {
            FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildLessOrEqualTest43()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, Int1));
        }

        [TestMethod]
        public void BuildLessOrEqualTest44()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildLessOrEqualTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest52()
        {
            FunctionBuilder.BuildLessOrEqual(NullVarDef, StringGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest53()
        {
            FunctionBuilder.BuildLessOrEqual(StringGenVarDef, NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessOrEqualTest54()
        {
            FunctionBuilder.BuildLessOrEqual(IntGenVarDef, DateGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessOrEqualTest55()
        {
            FunctionBuilder.BuildLessOrEqual(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessOrEqualTest56()
        {
            FunctionBuilder.BuildLessOrEqual(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        public void BuildLessOrEqualTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLessOrEqual(StringGenVarDef, StringGenVarDef1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest61()
        {
            FunctionBuilder.BuildLessOrEqual(NullLambda, x => x.Hierarchy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessOrEqualTest62()
        {
            FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessOrEqualTest63()
        {
            FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessOrEqualTest64()
        {
            FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildLessOrEqualTest71()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildLessOrEqualTest72()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildLessOrEqualTest73()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildLessOrEqualTest74()
        {

            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildLessOrEqualTest75()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
