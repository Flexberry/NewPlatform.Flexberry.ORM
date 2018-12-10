namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildLessTests : BaseFunctionTest
    {
        #region Less

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest11()
        {
            FunctionBuilder.BuildLess(NullString, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest12()
        {
            FunctionBuilder.BuildLess(PropertyName, NullObject);
        }

        [TestMethod]
        public void BuildLessTest13()
        {
            FunctionBuilder.BuildLess(PropertyName, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest21()
        {
            FunctionBuilder.BuildLess(NullString, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest22()
        {
            FunctionBuilder.BuildLess(PropertyName, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest23()
        {
            FunctionBuilder.BuildLess(PropertyName, LangDef.NumericType, NullObject);
        }

        [TestMethod]
        public void BuildLessTest24()
        {
            FunctionBuilder.BuildLess(PropertyName, LangDef.NumericType, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest31()
        {
            FunctionBuilder.BuildLess(NullVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest32()
        {
            FunctionBuilder.BuildLess(NumericVarDef, NullObject);
        }

        [TestMethod]
        public void BuildLessTest33()
        {
            FunctionBuilder.BuildLess(NumericVarDef, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest41()
        {
            FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildLessTest42()
        {
            FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildLessTest43()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, Int1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, Int1));
        }

        [TestMethod]
        public void BuildLessTest44()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildLessTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, String1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest52()
        {
            FunctionBuilder.BuildLess(NullVarDef, StringGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest53()
        {
            FunctionBuilder.BuildLess(StringGenVarDef, NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessTest54()
        {
            FunctionBuilder.BuildLess(IntGenVarDef, DateGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessTest55()
        {
            FunctionBuilder.BuildLess(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessTest56()
        {
            FunctionBuilder.BuildLess(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        public void BuildLessTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLess(StringGenVarDef, StringGenVarDef1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest61()
        {
            FunctionBuilder.BuildLess(NullLambda, x => x.Hierarchy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLessTest62()
        {
            FunctionBuilder.BuildLess<TestDataObject>(x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessTest63()
        {
            FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildLessTest64()
        {
            FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildLessTest71()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildLessTest72()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildLessTest73()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildLessTest74()
        {

            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildLessTest75()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
