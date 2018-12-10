namespace IIS.University.Tools.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildLikeTests : BaseFunctionTest
    {
        #region common

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLikeTest1()
        {
            FunctionBuilder.BuildLike(NullString, NullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildLikeTest2()
        {
            FunctionBuilder.BuildLike(PropertyName, NullString);
        }

        [TestMethod]
        public void BuildLikeTest3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, String1),
                FunctionBuilder.BuildLike(PropertyName, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildStartsWith1()
        {
            FunctionBuilder.BuildStartsWith(NullString, NullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildStartsWith2()
        {
            FunctionBuilder.BuildStartsWith(PropertyName, NullString);
        }

        [TestMethod]
        public void BuildStartsWith3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, $"{String1}%"),
                FunctionBuilder.BuildStartsWith(PropertyName, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEndsWith1()
        {
            FunctionBuilder.BuildEndsWith(NullString, NullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEndsWith2()
        {
            FunctionBuilder.BuildEndsWith(PropertyName, NullString);
        }

        [TestMethod]
        public void BuildEndsWith3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, $"%{String1}"),
                FunctionBuilder.BuildEndsWith(PropertyName, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildContains1()
        {
            FunctionBuilder.BuildContains(NullString, NullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildContains2()
        {
            FunctionBuilder.BuildContains(PropertyName, NullString);
        }

        [TestMethod]
        public void BuildContains3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringVarDef, $"%{String1}%"),
                FunctionBuilder.BuildContains(PropertyName, String1));
        }

        #endregion

        #region generic

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGenLikeTest1()
        {
            FunctionBuilder.BuildLike<TestDataObject>(x => x.Name, NullString);
        }

        [TestMethod]
        public void BuildGenLikeTest2()
        {
            FunctionBuilder.BuildLike<TestDataObject>(x => x.BirthDate, String1);
        }

        [TestMethod]
        public void BuildGenLikeTest3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"{String1}"),
                FunctionBuilder.BuildLike<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGenStartsWithTest1()
        {
            FunctionBuilder.BuildStartsWith<TestDataObject>(x => x.Name, NullString);
        }

        [TestMethod]
        public void BuildGenStartsWithTest2()
        {
            FunctionBuilder.BuildStartsWith<TestDataObject>(x => x.BirthDate, String1);
        }

        [TestMethod]
        public void BuildGenStartsWithTest3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"{String1}%"),
                FunctionBuilder.BuildStartsWith<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGenEndsWithTest1()
        {
            FunctionBuilder.BuildEndsWith<TestDataObject>(x => x.Name, NullString);
        }

        [TestMethod]
        public void BuildGenEndsWithTest2()
        {
            FunctionBuilder.BuildEndsWith<TestDataObject>(x => x.BirthDate, String1);
        }

        [TestMethod]
        public void BuildGenEndsWithTest3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"%{String1}"),
                FunctionBuilder.BuildEndsWith<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildGenContainsTest1()
        {
            FunctionBuilder.BuildContains<TestDataObject>(x => x.Name, NullString);
        }

        [TestMethod]
        public void BuildGenContainsTest2()
        {
            FunctionBuilder.BuildContains<TestDataObject>(x => x.BirthDate, String1);
        }

        [TestMethod]
        public void BuildGenContainsTest3()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLike, StringGenVarDef, $"%{String1}%"),
                FunctionBuilder.BuildContains<TestDataObject>(x => x.Name, String1));
        }

        #endregion
    }
}
