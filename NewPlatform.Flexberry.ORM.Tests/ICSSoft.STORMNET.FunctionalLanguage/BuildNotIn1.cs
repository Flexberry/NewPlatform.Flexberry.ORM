namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildNotIn1 : BaseFunctionTest
    {
        protected static readonly Function FuncNotFalse = FunctionBuilder.BuildNot(FunctionBuilder.BuildFalse());

        private static readonly ObjectType DataObjectType = LangDef.DataObjectType;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest100()
        {
            FunctionBuilder.BuildNotIn(NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest102()
        {
            FunctionBuilder.BuildNotIn(GuidVarDef, NullObjects);
        }

        [TestMethod]
        public void BuildNotInTest103()
        {
            Assert.AreEqual(FuncNotFalse, FunctionBuilder.BuildNotIn(GuidVarDef));
        }

        [TestMethod]
        public void BuildNotInTest104()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals(GuidVarDef, Guid1)), 
                FunctionBuilder.BuildNotIn(GuidVarDef, Guid1));
        }

        [TestMethod]
        public void BuildNotInTest201()
        {
            Assert.AreEqual(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn(NullLambda));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest202()
        {
            FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, NullObjects);
        }

        [TestMethod]
        public void BuildNotInTest203()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals(Guid1)), 
                FunctionBuilder.BuildNotIn(NullLambda, Guid1)); // eq
        }

        [TestMethod]
        public void BuildNotInTest235()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 1)),
                FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, new List<int> { 1 }));
        }

        [TestMethod]
        public void BuildNotInTest300()
        {
            Assert.AreEqual(FuncNotFalse, 
                FunctionBuilder.BuildNotIn(PropertyName, DataObjectType));
        }
        [TestMethod]
        public void BuildNotInTest400()
        {
            Assert.AreEqual(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn());
        }

        [TestMethod]
        public void BuildNotInTest401()
        {
            Assert.AreEqual(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn(NullObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest402()
        {
            FunctionBuilder.BuildNotIn(NullObjects);
        }

        [TestMethod]
        public void BuildNotInTest403()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals(Guid1)),
                FunctionBuilder.BuildNotIn(Guid1)); // eq
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest500()
        {
            FunctionBuilder.BuildNotIn(NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest501()
        {
            FunctionBuilder.BuildNotIn(NullVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest502()
        {
            FunctionBuilder.BuildNotIn(GuidVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest503()
        {
            FunctionBuilder.BuildNotIn(NullLambda, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotInTest504()
        {
            FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, NullFunction);
        }

        [TestMethod]
        public void BuildNotInTest510()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(LangDef.GetFunction(LangDef.funcIN, PrimaryKeyVarDef, FuncSQL)),
                FunctionBuilder.BuildNotIn(FuncSQL));
        }

        [TestMethod]
        public void BuildNotInTest511()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(LangDef.GetFunction(LangDef.funcIN, GuidVarDef, FuncSQL)),
                FunctionBuilder.BuildNotIn(GuidVarDef, FuncSQL));
        }

        [TestMethod]
        public void BuildNotInTest512()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildNot(LangDef.GetFunction(LangDef.funcIN, IntGenVarDef, FuncSQL)),
                FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, FuncSQL));
        }
    }
}