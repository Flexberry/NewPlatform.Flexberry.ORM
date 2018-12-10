namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildOrTests : BaseFunctionTest
    {
        #region Or

        [Fact]
        public void BuildOrTest11()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildOr());
        }

        [Fact]
        public void BuildOrTest12()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildOr(NullFunction));
        }

        [Fact]
        public void BuildOrTest13()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildOr(FuncTrue));
        }

        [Fact]
        public void BuildOrTest14()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildOr(FuncTrue, NullFunction));
        }

        [Fact]
        public void BuildOrTest15()
        {
            Assert.Equal(LangDef.GetFunction(LangDef.funcOR, FuncTrue, FuncFalse), FunctionBuilder.BuildOr(FuncTrue, FuncFalse));
        }

        [Fact]
        public void BuildOrTest21()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildOr((IEnumerable<Function>)null));
        }

        [Fact]
        public void BuildOrTest22()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildOr(Enumerable.Empty<Function>()));
        }

        [Fact]
        public void BuildOrTest23()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildOr(new List<Function> { FuncTrue }));
        }

        [Fact]
        public void BuildOrTest24()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildOr(new List<Function> { FuncTrue, NullFunction }));
        }

        [Fact]
        public void BuildOrTest25()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcOR, FuncTrue, FuncFalse),
                FunctionBuilder.BuildOr(new List<Function> { FuncTrue, FuncFalse }));
        }

        #endregion
    }
}
