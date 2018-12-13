namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class FunctionBuilderTests : BaseFunctionTest
    {
        [Fact]
        public void BuildTrueTest()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, 1, 1),
                FunctionBuilder.BuildTrue());
        }

        [Fact]
        public void BuildFalseTest()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, 1, 0),
                FunctionBuilder.BuildFalse());
        }

        [Fact]
        public void BuildSQLTest()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcSQL, "sql"),
                FunctionBuilder.BuildSQL("sql"));
        }

        [Fact]
        public void BuildNotTest()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNOT, FuncTrue),
                FunctionBuilder.BuildNot(FuncTrue));
        }

        [Fact]
        public void BuildNotTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNot(NullFunction));
        }
    }
}
