namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;

    using Xunit;

    public class BuildIn4Tests : BaseFunctionTest
    {
        [Fact]
        public void BuildInTest400()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn());
        }

        [Fact]
        public void BuildInTest401()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(NullObject));
        }

        [Fact]
        public void BuildInTest402()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(NullObjects));
        }

        [Fact]
        public void BuildInTest406()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(1));
        }

        [Fact]
        public void BuildInTest413()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(1, 2));
        }

        [Fact]
        public void BuildInTest414()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(1, 1));
        }

        [Fact]
        public void BuildInTest415()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<Guid>)null));
        }

        [Fact]
        public void BuildInTest416()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<KeyGuid>)null));
        }

        [Fact]
        public void BuildInTest417()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<TestDataObject>)null));
        }

        [Fact]
        public void BuildInTest418()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn((IEnumerable<int>)null));
        }

        [Fact]
        public void BuildInTest419()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<Guid>()));
        }

        [Fact]
        public void BuildInTest420()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<KeyGuid>()));
        }

        [Fact]
        public void BuildInTest421()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<TestDataObject>()));
        }

        [Fact]
        public void BuildInTest422()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(Enumerable.Empty<int>()));
        }

        [Fact]
        public void BuildInTest426()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(new List<int> { 1 }));
        }

        [Fact]
        public void BuildInTest433()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(new List<int> { 1, 2 }));
        }

        [Fact]
        public void BuildInTest434()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(new List<int> { 1, 1 }));
        }

        [Fact]
        public void BuildInTest403()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(Guid1)); // eq
        }

        [Fact]
        public void BuildInTest404()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(KeyGuid1)); // eq
        }

        [Fact]
        public void BuildInTest405()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(TestDataObject1)); // eq
        }

        [Fact]
        public void BuildInTest407()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(Guid1, Guid2)); // in
        }

        [Fact]
        public void BuildInTest408()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(Guid1, Guid1)); // eq
        }

        [Fact]
        public void BuildInTest409()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(KeyGuid1, KeyGuid2)); // in
        }

        [Fact]
        public void BuildInTest410()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(KeyGuid1), FunctionBuilder.BuildIn(KeyGuid1, KeyGuid1)); // eq
        }

        [Fact]
        public void BuildInTest411()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(TestDataObject1, TestDataObject2)); // in
        }

        [Fact]
        public void BuildInTest412()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(TestDataObject1, TestDataObject1)); // eq
        }

        [Fact]
        public void BuildInTest423()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(Guid1), FunctionBuilder.BuildIn(new List<Guid> { Guid1 })); // eq
        }

        [Fact]
        public void BuildInTest424()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(new List<KeyGuid> { KeyGuid1 })); // eq
        }

        [Fact]
        public void BuildInTest425()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(new List<TestDataObject> { TestDataObject1 })); // eq
        }

        [Fact]
        public void BuildInTest427()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(new List<Guid> { Guid1, Guid2 })); // in
        }

        [Fact]
        public void BuildInTest428()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(Guid1),
                FunctionBuilder.BuildIn(new List<Guid> { Guid1, Guid1 })); // eq
        }

        [Fact]
        public void BuildInTest429()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(new List<KeyGuid> { KeyGuid1, KeyGuid2 })); // in
        }

        [Fact]
        public void BuildInTest430()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(KeyGuid1),
                FunctionBuilder.BuildIn(new List<KeyGuid> { KeyGuid1, KeyGuid1 })); // eq
        }

        [Fact]
        public void BuildInTest431()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(new List<TestDataObject> { TestDataObject1, TestDataObject2 })); // in
        }

        [Fact]
        public void BuildInTest432()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(TestDataObject1),
                FunctionBuilder.BuildIn(new List<TestDataObject> { TestDataObject1, TestDataObject1 })); // eq
        }

        [Fact]
        public void BuildInTest435()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(SQLWhereLanguageDef.StormMainObjectKey, Guid1, Guid2),
                FunctionBuilder.BuildIn(new List<Guid?> { Guid1, Guid2 })); // in
        }

        [Fact]
        public void BuildInTest436()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(Guid1),
                FunctionBuilder.BuildIn(new List<Guid?> { Guid1, Guid1 })); // eq
        }
    }
}
