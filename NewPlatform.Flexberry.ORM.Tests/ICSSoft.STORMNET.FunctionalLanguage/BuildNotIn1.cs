namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildNotIn1 : BaseFunctionTest
    {
        protected static readonly Function FuncNotFalse = FunctionBuilder.BuildNot(FunctionBuilder.BuildFalse());

        private static readonly ObjectType DataObjectType = LangDef.DataObjectType;

        [Fact]
        public void BuildNotInTest100()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(NullVarDef));
        }

        [Fact]
        public void BuildNotInTest102()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(GuidVarDef, NullObjects));
        }

        [Fact]
        public void BuildNotInTest103()
        {
            Assert.Equal(FuncNotFalse, FunctionBuilder.BuildNotIn(GuidVarDef));
        }

        [Fact]
        public void BuildNotInTest104()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals(GuidVarDef, Guid1)),
                FunctionBuilder.BuildNotIn(GuidVarDef, Guid1));
        }

        [Fact]
        public void BuildNotInTest201()
        {
            Assert.Equal(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn(NullLambda));
        }

        [Fact]
        public void BuildNotInTest202()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, NullObjects));
        }

        [Fact]
        public void BuildNotInTest203()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals(Guid1)),
                FunctionBuilder.BuildNotIn(NullLambda, Guid1)); // eq
        }

        [Fact]
        public void BuildNotInTest235()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, 1)),
                FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, new List<int> { 1 }));
        }

        [Fact]
        public void BuildNotInTest300()
        {
            Assert.Equal(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn(PropertyName, DataObjectType));
        }

        [Fact]
        public void BuildNotInTest400()
        {
            Assert.Equal(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn());
        }

        [Fact]
        public void BuildNotInTest401()
        {
            Assert.Equal(
                FuncNotFalse,
                FunctionBuilder.BuildNotIn(NullObject));
        }

        [Fact]
        public void BuildNotInTest402()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(NullObjects));
        }

        [Fact]
        public void BuildNotInTest403()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(FunctionBuilder.BuildEquals(Guid1)),
                FunctionBuilder.BuildNotIn(Guid1)); // eq
        }

        [Fact]
        public void BuildNotInTest500()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(NullFunction));
        }

        [Fact]
        public void BuildNotInTest501()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(NullVarDef, NullFunction));
        }

        [Fact]
        public void BuildNotInTest502()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(GuidVarDef, NullFunction));
        }

        [Fact]
        public void BuildNotInTest503()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn(NullLambda, NullFunction));
        }

        [Fact]
        public void BuildNotInTest504()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, NullFunction));
        }

        [Fact]
        public void BuildNotInTest510()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(LangDef.GetFunction(LangDef.funcIN, PrimaryKeyVarDef, FuncSQL)),
                FunctionBuilder.BuildNotIn(FuncSQL));
        }

        [Fact]
        public void BuildNotInTest511()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(LangDef.GetFunction(LangDef.funcIN, GuidVarDef, FuncSQL)),
                FunctionBuilder.BuildNotIn(GuidVarDef, FuncSQL));
        }

        [Fact]
        public void BuildNotInTest512()
        {
            Assert.Equal(
                FunctionBuilder.BuildNot(LangDef.GetFunction(LangDef.funcIN, IntGenVarDef, FuncSQL)),
                FunctionBuilder.BuildNotIn<TestDataObject>(x => x.Height, FuncSQL));
        }
    }
}