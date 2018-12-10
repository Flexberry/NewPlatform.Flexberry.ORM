namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildIn5Tests : BaseFunctionTest
    {
        [Fact]
        public void BuildInTest500()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(NullFunction));
        }

        [Fact]
        public void BuildInTest501()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(NullVarDef, NullFunction));
        }

        [Fact]
        public void BuildInTest502()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(GuidVarDef, NullFunction));
        }

        [Fact]
        public void BuildInTest503()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn(NullLambda, NullFunction));
        }

        [Fact]
        public void BuildInTest504()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, NullFunction));
        }

        [Fact]
        public void BuildInTest510()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, PrimaryKeyVarDef, FuncSQL),
                FunctionBuilder.BuildIn(FuncSQL));
        }

        [Fact]
        public void BuildInTest511()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, FuncSQL),
                FunctionBuilder.BuildIn(GuidVarDef, FuncSQL));
        }

        [Fact]
        public void BuildInTest512()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, IntGenVarDef, FuncSQL),
                FunctionBuilder.BuildIn<TestDataObject>(x => x.Height, FuncSQL));
        }
    }
}