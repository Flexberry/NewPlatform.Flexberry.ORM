namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildAndTests : BaseFunctionTest
    {
        #region And

        [Fact]
        public void BuildAndTest10()
        {
            Function[] nullFunctions = null;
            Assert.Equal(NullFunction, FunctionBuilder.BuildAnd(nullFunctions));
        }

        [Fact]
        public void BuildAndTest11()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildAnd());
        }

        [Fact]
        public void BuildAndTest12()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildAnd(NullFunction));
        }

        [Fact]
        public void BuildAndTest13()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildAnd(FuncTrue));
        }

        [Fact]
        public void BuildAndTest14()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildAnd(FuncTrue, NullFunction));
        }

        [Fact]
        public void BuildAndTest15()
        {
            Assert.Equal(LangDef.GetFunction(LangDef.funcAND, FuncTrue, FuncFalse), FunctionBuilder.BuildAnd(FuncTrue, FuncFalse));
        }

        [Fact]
        public void BuildAndTest21()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildAnd((IEnumerable<Function>)null));
        }

        [Fact]
        public void BuildAndTest22()
        {
            Assert.Equal(NullFunction, FunctionBuilder.BuildAnd(Enumerable.Empty<Function>()));
        }

        [Fact]
        public void BuildAndTest23()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildAnd(new List<Function> { FuncTrue }));
        }

        [Fact]
        public void BuildAndTest24()
        {
            Assert.Equal(FuncTrue, FunctionBuilder.BuildAnd(new List<Function> { FuncTrue, NullFunction }));
        }

        [Fact]
        public void BuildAndTest25()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcAND, FuncTrue, FuncFalse),
                FunctionBuilder.BuildAnd(new List<Function> { FuncTrue, FuncFalse }));
        }

        #endregion
    }
}
