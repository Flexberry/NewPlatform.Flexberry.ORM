namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.KeyGen;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIn1Tests : BaseFunctionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest100()
        {
            FunctionBuilder.BuildIn(NullVarDef);
        }

        [TestMethod]
        public void BuildInTest101()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, NullObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest102()
        {
            FunctionBuilder.BuildIn(GuidVarDef, NullObjects);
        }

        [TestMethod]
        public void BuildInTest103()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef));
        }

        [TestMethod]
        public void BuildInTest104()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, Guid1), FunctionBuilder.BuildIn(GuidVarDef, Guid1));
        }

        [TestMethod]
        public void BuildInTest105()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1), FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1));
        }

        [TestMethod]
        public void BuildInTest106()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1));
        }

        [TestMethod]
        public void BuildInTest107()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(NumericVarDef, 1), FunctionBuilder.BuildIn(NumericVarDef, 1));
        }

        [TestMethod]
        public void BuildInTest108()
        {
            Assert.AreEqual(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid2),
                FunctionBuilder.BuildIn(GuidVarDef, Guid1, Guid2));
        }

        [TestMethod]
        public void BuildInTest109()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, Guid1), FunctionBuilder.BuildIn(GuidVarDef, Guid1, Guid1));
        }

        [TestMethod]
        public void BuildInTest110()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, Guid1), FunctionBuilder.BuildIn(GuidVarDef, Guid1, NullObject));
        }

        [TestMethod]
        public void BuildInTest111()
        {
            Assert.AreEqual(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1, KeyGuid2));
        }

        [TestMethod]
        public void BuildInTest112()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1), FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1, KeyGuid1));
        }

        [TestMethod]
        public void BuildInTest113()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1), FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1, NullObject));
        }

        [TestMethod]
        public void BuildInTest114()
        {
            Assert.AreEqual(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1, TestDataObject2));
        }

        [TestMethod]
        public void BuildInTest115()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1), FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1, TestDataObject1));
        }

        [TestMethod]
        public void BuildInTest116()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1), FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1, NullObject));
        }

        [TestMethod]
        public void BuildInTest117()
        {
            Assert.AreEqual(FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, 1, 2), FunctionBuilder.BuildIn(NumericVarDef, 1, 2));
        }

        [TestMethod]
        public void BuildInTest118()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(NumericVarDef, 1), FunctionBuilder.BuildIn(NumericVarDef, 1, 1));
        }

        [TestMethod]
        public void BuildInTest119()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(NumericVarDef, 1), FunctionBuilder.BuildIn(NumericVarDef, 1, NullObject));
        }

        [TestMethod]
        public void BuildInTest120()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<Guid>)null));
        }

        [TestMethod]
        public void BuildInTest121()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<KeyGuid>)null));
        }

        [TestMethod]
        public void BuildInTest122()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<TestDataObject>)null));
        }

        [TestMethod]
        public void BuildInTest123()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<int>)null));
        }

        [TestMethod]
        public void BuildInTest124()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<Guid>()));
        }

        [TestMethod]
        public void BuildInTest125()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<KeyGuid>()));
        }

        [TestMethod]
        public void BuildInTest126()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<TestDataObject>()));
        }

        [TestMethod]
        public void BuildInTest127()
        {
            Assert.AreEqual(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<int>()));
        }

        [TestMethod]
        public void BuildInTest128()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<Guid> { Guid1 }));
        }

        [TestMethod]
        public void BuildInTest129()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1 }));
        }

        [TestMethod]
        public void BuildInTest130()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1 }));
        }

        [TestMethod]
        public void BuildInTest131()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, 1),
                FunctionBuilder.BuildIn(NumericVarDef, new List<int> { 1 }));
        }

        [TestMethod]
        public void BuildInTest132()
        {
            Assert.AreEqual(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<Guid> { Guid1, Guid2 }));
        }

        [TestMethod]
        public void BuildInTest133()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<Guid> { Guid1, Guid1 }));
        }

        [TestMethod]
        public void BuildInTest134()
        {
            Assert.AreEqual(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1, KeyGuid2 }));
        }

        [TestMethod]
        public void BuildInTest135()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1, KeyGuid1 }));
        }

        [TestMethod]
        public void BuildInTest136()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1, null }));
        }

        [TestMethod]
        public void BuildInTest137()
        {
            Assert.AreEqual(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1, TestDataObject2 }));
        }

        [TestMethod]
        public void BuildInTest138()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1, TestDataObject1 }));
        }

        [TestMethod]
        public void BuildInTest139()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1, null }));
        }

        [TestMethod]
        public void BuildInTest140()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(GuidVarDef, 1, 2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<int> { 1, 2 }));
        }

        [TestMethod]
        public void BuildInTest141()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, 1),
                FunctionBuilder.BuildIn(NumericVarDef, new List<int> { 1, 1 }));
        }
    }
}
