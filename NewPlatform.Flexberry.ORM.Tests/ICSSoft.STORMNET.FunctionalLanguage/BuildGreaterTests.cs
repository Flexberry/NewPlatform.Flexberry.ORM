namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildGreaterTests : BaseFunctionTest
    {
        #region Greater

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest11()
        {
            FunctionBuilder.BuildGreater(NullString, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest12()
        {
            FunctionBuilder.BuildGreater(PropertyName, NullObject);
        }

        [TestMethod]
        public void BuildGreaterTest13()
        {
            FunctionBuilder.BuildGreater(PropertyName, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest21()
        {
            FunctionBuilder.BuildGreater(NullString, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest22()
        {
            FunctionBuilder.BuildGreater(PropertyName, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest23()
        {
            FunctionBuilder.BuildGreater(PropertyName, LangDef.NumericType, NullObject);
        }

        [TestMethod]
        public void BuildGreaterTest24()
        {
            FunctionBuilder.BuildGreater(PropertyName, LangDef.NumericType, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest31()
        {
            FunctionBuilder.BuildGreater(NullVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest32()
        {
            FunctionBuilder.BuildGreater(NumericVarDef, NullObject);
        }

        [TestMethod]
        public void BuildGreaterTest33()
        {
            FunctionBuilder.BuildGreater(NumericVarDef, 1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest41()
        {
            FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildGreaterTest42()
        {
            FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildGreaterTest43()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, IntGenVarDef, Int1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, Int1));
        }

        [TestMethod]
        public void BuildGreaterTest44()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildGreaterTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, String1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest52()
        {
            FunctionBuilder.BuildGreater(NullVarDef, StringGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest53()
        {
            FunctionBuilder.BuildGreater(StringGenVarDef, NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterTest54()
        {
            FunctionBuilder.BuildGreater(IntGenVarDef, DateGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterTest55()
        {
            FunctionBuilder.BuildGreater(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterTest56()
        {
            FunctionBuilder.BuildGreater(StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        public void BuildGreaterTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreater(StringGenVarDef, StringGenVarDef1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest61()
        {
            FunctionBuilder.BuildGreater(NullLambda, x => x.Hierarchy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGreaterTest62()
        {
            FunctionBuilder.BuildGreater<TestDataObject>(x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterTest63()
        {
            FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildGreaterTest64()
        {
            FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildGreaterTest71()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildGreaterTest72()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildGreaterTest73()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildGreaterTest74()
        {

            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildGreaterTest75()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
