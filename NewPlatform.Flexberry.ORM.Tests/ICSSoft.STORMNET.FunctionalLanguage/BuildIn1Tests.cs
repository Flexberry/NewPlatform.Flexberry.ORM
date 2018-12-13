namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;

    using Xunit;

    public class BuildIn1Tests : BaseFunctionTest
    {
        [Fact]
        public void BuildInTest100()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(NullVarDef));
        }

        [Fact]
        public void BuildInTest101()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, NullObject));
        }

        [Fact]
        public void BuildInTest102()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(GuidVarDef, NullObjects));
        }

        [Fact]
        public void BuildInTest103()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef));
        }

        [Fact]
        public void BuildInTest104()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, Guid1), FunctionBuilder.BuildIn(GuidVarDef, Guid1));
        }

        [Fact]
        public void BuildInTest105()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1), FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1));
        }

        [Fact]
        public void BuildInTest106()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1));
        }

        [Fact]
        public void BuildInTest107()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(NumericVarDef, 1), FunctionBuilder.BuildIn(NumericVarDef, 1));
        }

        [Fact]
        public void BuildInTest108()
        {
            Assert.Equal(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid2),
                FunctionBuilder.BuildIn(GuidVarDef, Guid1, Guid2));
        }

        [Fact]
        public void BuildInTest109()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, Guid1), FunctionBuilder.BuildIn(GuidVarDef, Guid1, Guid1));
        }

        [Fact]
        public void BuildInTest110()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, Guid1), FunctionBuilder.BuildIn(GuidVarDef, Guid1, NullObject));
        }

        [Fact]
        public void BuildInTest111()
        {
            Assert.Equal(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1, KeyGuid2));
        }

        [Fact]
        public void BuildInTest112()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1), FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1, KeyGuid1));
        }

        [Fact]
        public void BuildInTest113()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1), FunctionBuilder.BuildIn(GuidVarDef, KeyGuid1, NullObject));
        }

        [Fact]
        public void BuildInTest114()
        {
            Assert.Equal(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1, TestDataObject2));
        }

        [Fact]
        public void BuildInTest115()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1), FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1, TestDataObject1));
        }

        [Fact]
        public void BuildInTest116()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1), FunctionBuilder.BuildIn(GuidVarDef, TestDataObject1, NullObject));
        }

        [Fact]
        public void BuildInTest117()
        {
            Assert.Equal(FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, 1, 2), FunctionBuilder.BuildIn(NumericVarDef, 1, 2));
        }

        [Fact]
        public void BuildInTest118()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(NumericVarDef, 1), FunctionBuilder.BuildIn(NumericVarDef, 1, 1));
        }

        [Fact]
        public void BuildInTest119()
        {
            Assert.Equal(FunctionBuilder.BuildEquals(NumericVarDef, 1), FunctionBuilder.BuildIn(NumericVarDef, 1, NullObject));
        }

        [Fact]
        public void BuildInTest120()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<Guid>)null));
        }

        [Fact]
        public void BuildInTest121()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<KeyGuid>)null));
        }

        [Fact]
        public void BuildInTest122()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<TestDataObject>)null));
        }

        [Fact]
        public void BuildInTest123()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, (IEnumerable<int>)null));
        }

        [Fact]
        public void BuildInTest124()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<Guid>()));
        }

        [Fact]
        public void BuildInTest125()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<KeyGuid>()));
        }

        [Fact]
        public void BuildInTest126()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<TestDataObject>()));
        }

        [Fact]
        public void BuildInTest127()
        {
            Assert.Equal(FuncFalse, FunctionBuilder.BuildIn(GuidVarDef, Enumerable.Empty<int>()));
        }

        [Fact]
        public void BuildInTest128()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<Guid> { Guid1 }));
        }

        [Fact]
        public void BuildInTest129()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1 }));
        }

        [Fact]
        public void BuildInTest130()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1 }));
        }

        [Fact]
        public void BuildInTest131()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, 1),
                FunctionBuilder.BuildIn(NumericVarDef, new List<int> { 1 }));
        }

        [Fact]
        public void BuildInTest132()
        {
            Assert.Equal(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<Guid> { Guid1, Guid2 }));
        }

        [Fact]
        public void BuildInTest133()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<Guid> { Guid1, Guid1 }));
        }

        [Fact]
        public void BuildInTest134()
        {
            Assert.Equal(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1, KeyGuid2 }));
        }

        [Fact]
        public void BuildInTest135()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1, KeyGuid1 }));
        }

        [Fact]
        public void BuildInTest136()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<KeyGuid> { KeyGuid1, null }));
        }

        [Fact]
        public void BuildInTest137()
        {
            Assert.Equal(
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, TestDataObject1, TestDataObject2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1, TestDataObject2 }));
        }

        [Fact]
        public void BuildInTest138()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1, TestDataObject1 }));
        }

        [Fact]
        public void BuildInTest139()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1),
                FunctionBuilder.BuildIn(GuidVarDef, new List<TestDataObject> { TestDataObject1, null }));
        }

        [Fact]
        public void BuildInTest140()
        {
            Assert.Equal(
                FunctionBuilder.BuildIn(GuidVarDef, 1, 2),
                FunctionBuilder.BuildIn(GuidVarDef, new List<int> { 1, 2 }));
        }

        [Fact]
        public void BuildInTest141()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, 1),
                FunctionBuilder.BuildIn(NumericVarDef, new List<int> { 1, 1 }));
        }
    }
}
