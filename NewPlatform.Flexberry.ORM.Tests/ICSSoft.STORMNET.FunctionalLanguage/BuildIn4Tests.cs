namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIn4Tests : BaseFunctionTest
    {
        [TestMethod]
        public void BuildInTest400()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn());
        }

        [TestMethod]
        public void BuildInTest401()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest402()
        {
            FunctionBuilder.BuildIn(NullObjects);
        }

        [TestMethod]
        public void BuildInTest406()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(1));
        }

        [TestMethod]
        public void BuildInTest413()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(1, 2));
        }

        [TestMethod]
        public void BuildInTest414()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(1, 1));
        }

        [TestMethod]
        public void BuildInTest415()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<Guid>)null));
        }

        [TestMethod]
        public void BuildInTest416()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<KeyGuid>)null));
        }

        [TestMethod]
        public void BuildInTest417()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<TestDataObject>)null));
        }

        [TestMethod]
        public void BuildInTest418()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<int>)null));
        }

        [TestMethod]
        public void BuildInTest419()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<Guid>()));
        }

        [TestMethod]
        public void BuildInTest420()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<KeyGuid>()));
        }

        [TestMethod]
        public void BuildInTest421()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<TestDataObject>()));
        }

        [TestMethod]
        public void BuildInTest422()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<int>()));
        }

        [TestMethod]
        public void BuildInTest426()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(new List<int> { 1 }));
        }

        [TestMethod]
        public void BuildInTest433()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(new List<int> { 1, 2 }));
        }

        [TestMethod]
        public void BuildInTest434()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(new List<int> { 1, 1 }));
        }

        [TestMethod]
        public void BuildInTest403()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(Guid1)); // eq
        }

        [TestMethod]
        public void BuildInTest404()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(KeyGuid1)); // eq
        }

        [TestMethod]
        public void BuildInTest405()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(TestDataObject1)); // eq
        }

        [TestMethod]
        public void BuildInTest407()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(Guid1, Guid2)); // in
        }

        [TestMethod]
        public void BuildInTest408()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(Guid1, Guid1)); // eq
        }

        [TestMethod]
        public void BuildInTest409()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(KeyGuid1, KeyGuid2)); // in
        }

        [TestMethod]
        public void BuildInTest410()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(KeyGuid1, KeyGuid1)); // eq
        }

        [TestMethod]
        public void BuildInTest411()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(TestDataObject1, TestDataObject2)); // in
        }

        [TestMethod]
        public void BuildInTest412()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(TestDataObject1, TestDataObject1)); // eq
        }

        [TestMethod]
        public void BuildInTest423()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(new List<Guid> { Guid1 }));
            // eq
        }

        [TestMethod]
        public void BuildInTest424()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(new List<KeyGuid> { KeyGuid1 })); // eq
        }

        [TestMethod]
        public void BuildInTest425()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(new List<TestDataObject> { TestDataObject1 })); // eq
        }

        [TestMethod]
        public void BuildInTest427()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(new List<Guid> { Guid1, Guid2 })); // in
        }

        [TestMethod]
        public void BuildInTest428()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(Guid1),
                FunctionBuilder.BuildIn(new List<Guid> { Guid1, Guid1 })); // eq
        }

        [TestMethod]
        public void BuildInTest429()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(new List<KeyGuid> { KeyGuid1, KeyGuid2 })); // in
        }

        [TestMethod]
        public void BuildInTest430()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(new List<KeyGuid> { KeyGuid1, KeyGuid1 })); // eq
        }

        [TestMethod]
        public void BuildInTest431()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(new List<TestDataObject> { TestDataObject1, TestDataObject2 })); // in
        }

        [TestMethod]
        public void BuildInTest432()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(new List<TestDataObject> { TestDataObject1, TestDataObject1 })); // eq
        }
    }
}
