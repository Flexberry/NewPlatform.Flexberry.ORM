namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;

    using Xunit;

    public class BuildIn2Tests : BaseFunctionTest
    {
        [Fact]
        public void BuildInTest200()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda));
        }

        [Fact]
        public void BuildInTest201()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, NullObject));
        }

        [Fact]
        public void BuildInTest202()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, NullObjects));
        }

        [Fact]
        public void BuildInTest206()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, 1));
        }

        [Fact]
        public void BuildInTest213()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, 1, 2));
        }

        [Fact]
        public void BuildInTest214()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, 1, 1));
        }

        [Fact]
        public void BuildInTest215()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<Guid>)null));
        }

        [Fact]
        public void BuildInTest216()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<KeyGuid>)null));
        }

        [Fact]
        public void BuildInTest217()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<TestDataObject>)null));
        }

        [Fact]
        public void BuildInTest218()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, (IEnumerable<int>)null));
        }

        [Fact]
        public void BuildInTest219()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<Guid>()));
        }

        [Fact]
        public void BuildInTest220()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<KeyGuid>()));
        }

        [Fact]
        public void BuildInTest221()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<TestDataObject>()));
        }

        [Fact]
        public void BuildInTest222()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, Enumerable.Empty<int>()));
        }

        [Fact]
        public void BuildInTest226()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, new List<int> { 1 }));
        }

        [Fact]
        public void BuildInTest233()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, new List<int> { 1, 2 }));
        }

        [Fact]
        public void BuildInTest234()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullLambda, new List<int> { 1, 1 }));
        }

        [Fact]
        public void BuildInTest203()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(NullLambda, Guid1)); // eq
        }

        [Fact]
        public void BuildInTest204()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(NullLambda, KeyGuid1)); // eq
        }

        [Fact]
        public void BuildInTest205()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, TestDataObject1)); // eq
        }

        [Fact]
        public void BuildInTest207()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(NullLambda, Guid1, Guid2)); // in
        }

        [Fact]
        public void BuildInTest208()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(NullLambda, Guid1, Guid1)); // eq
        }

        [Fact]
        public void BuildInTest209()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(NullLambda, KeyGuid1, KeyGuid2)); // in
        }

        [Fact]
        public void BuildInTest210()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(NullLambda, KeyGuid1, KeyGuid1)); // eq
        }

        [Fact]
        public void BuildInTest211()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(NullLambda, TestDataObject1, TestDataObject2)); // in
        }

        [Fact]
        public void BuildInTest212()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, TestDataObject1, TestDataObject1)); // eq
        }

        [Fact]
        public void BuildInTest223()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(NullLambda, new List<Guid> { Guid1 })); // eq
        }

        [Fact]
        public void BuildInTest224()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(NullLambda, new List<KeyGuid> { KeyGuid1 })); // eq
        }

        [Fact]
        public void BuildInTest225()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, new List<TestDataObject> { TestDataObject1 })); // eq
        }

        [Fact]
        public void BuildInTest227()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(NullLambda, new List<Guid> { Guid1, Guid2 })); // in
        }

        [Fact]
        public void BuildInTest228()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(Guid1),
                FunctionBuilder.BuildIn(NullLambda, new List<Guid> { Guid1, Guid1 })); // eq
        }

        [Fact]
        public void BuildInTest229()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(NullLambda, new List<KeyGuid> { KeyGuid1, KeyGuid2 })); // in
        }

        [Fact]
        public void BuildInTest230()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(NullLambda, new List<KeyGuid> { KeyGuid1, KeyGuid1 })); // eq
        }

        [Fact]
        public void BuildInTest231()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(NullLambda, new List<TestDataObject> { TestDataObject1, TestDataObject2 })); // in
        }

        [Fact]
        public void BuildInTest232()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(NullLambda, new List<TestDataObject> { TestDataObject1, TestDataObject1 })); // eq
        }

        [Fact]
        public void BuildInTest235()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 1),
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, new List<int> { 1 }));
        }

        [Fact]
        public void BuildInTest236()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 1),
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, new List<int> { 1, 1 }));
        }
    }
}
