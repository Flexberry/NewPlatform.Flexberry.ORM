namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildIn2Tests : BaseFunctionTest
    {
        [TestMethod]
        public void BuildInTest200()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda));
        }

        [TestMethod]
        public void BuildInTest201()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, NullObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildInTest202()
        {
            FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, NullObjects);
        }

        [TestMethod]
        public void BuildInTest206()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, 1));
        }

        [TestMethod]
        public void BuildInTest213()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, 1, 2));
        }

        [TestMethod]
        public void BuildInTest214()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, 1, 1));
        }

        [TestMethod]
        public void BuildInTest215()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<Guid>)null));
        }

        [TestMethod]
        public void BuildInTest216()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<KeyGuid>)null));
        }

        [TestMethod]
        public void BuildInTest217()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<TestDataObject>)null));
        }

        [TestMethod]
        public void BuildInTest218()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<int>)null));
        }

        [TestMethod]
        public void BuildInTest219()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<Guid>()));
        }

        [TestMethod]
        public void BuildInTest220()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<KeyGuid>()));
        }

        [TestMethod]
        public void BuildInTest221()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<TestDataObject>()));
        }

        [TestMethod]
        public void BuildInTest222()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<int>()));
        }

        [TestMethod]
        public void BuildInTest226()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, new List<int> { 1 }));
        }

        [TestMethod]
        public void BuildInTest233()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, new List<int> { 1, 2 }));
        }

        [TestMethod]
        public void BuildInTest234()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, new List<int> { 1, 1 }));
        }

        [TestMethod]
        public void BuildInTest203()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(NullLambda, Guid1)); // eq
        }

        [TestMethod]
        public void BuildInTest204()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(NullLambda, KeyGuid1)); // eq
        }

        [TestMethod]
        public void BuildInTest205()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, TestDataObject1)); // eq
        }

        [TestMethod]
        public void BuildInTest207()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(NullLambda, Guid1, Guid2)); // in
        }

        [TestMethod]
        public void BuildInTest208()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(NullLambda, Guid1, Guid1)); // eq
        }

        [TestMethod]
        public void BuildInTest209()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(NullLambda, KeyGuid1, KeyGuid2)); // in
        }

        [TestMethod]
        public void BuildInTest210()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(NullLambda, KeyGuid1, KeyGuid1)); // eq
        }

        [TestMethod]
        public void BuildInTest211()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(NullLambda, TestDataObject1, TestDataObject2)); // in
        }

        [TestMethod]
        public void BuildInTest212()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, TestDataObject1, TestDataObject1)); // eq
        }

        [TestMethod]
        public void BuildInTest223()
        {
            Assert.AreEqual(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(NullLambda, new List<Guid> { Guid1 }));
            // eq
        }

        [TestMethod]
        public void BuildInTest224()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(NullLambda, new List<KeyGuid> { KeyGuid1 })); // eq
        }

        [TestMethod]
        public void BuildInTest225()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, new List<TestDataObject> { TestDataObject1 })); // eq
        }

        [TestMethod]
        public void BuildInTest227()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(NullLambda, new List<Guid> { Guid1, Guid2 })); // in
        }

        [TestMethod]
        public void BuildInTest228()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(Guid1),
                FunctionBuilder.BuildIn(NullLambda, new List<Guid> { Guid1, Guid1 })); // eq
        }

        [TestMethod]
        public void BuildInTest229()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(NullLambda, new List<KeyGuid> { KeyGuid1, KeyGuid2 })); // in
        }

        [TestMethod]
        public void BuildInTest230()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(NullLambda, new List<KeyGuid> { KeyGuid1, KeyGuid1 })); // eq
        }

        [TestMethod]
        public void BuildInTest231()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(NullLambda, new List<TestDataObject> { TestDataObject1, TestDataObject2 })); // in
        }

        [TestMethod]
        public void BuildInTest232()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, new List<TestDataObject> { TestDataObject1, TestDataObject1 })); // eq
        }

        [TestMethod]
        public void BuildInTest235()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 1),
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, new List<int> { 1 }));
        }

        [TestMethod]
        public void BuildInTest236()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 1),
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, new List<int> { 1, 1 }));
        }
    }
}
