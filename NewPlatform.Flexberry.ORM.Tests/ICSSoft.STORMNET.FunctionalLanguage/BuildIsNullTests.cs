namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildIsNullTests : BaseFunctionTest
    {
        #region IsNull

        [Fact]
        public void BuildIsNullTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIsNull(NullString));
        }

        [Fact]
        public void BuildIsNullTest12()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, StringVarDef),
                FunctionBuilder.BuildIsNull(PropertyName));
        }

        [Fact]
        public void BuildIsNullTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIsNull(NullVarDef));
        }

        [Fact]
        public void BuildIsNullTest22()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, StringVarDef),
                FunctionBuilder.BuildIsNull(StringVarDef));
        }

        [Fact]
        public void BuildIsNullTest31()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, IntGenVarDef),
                FunctionBuilder.BuildIsNull<TestDataObject>(x => x.Height));
        }

        [Fact]
        public void BuildIsNullTest32()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, DateGenVarDef),
                FunctionBuilder.BuildIsNull<TestDataObject>(x => x.BirthDate));
        }

        [Fact]
        public void BuildIsNullTest33()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, StringGenVarDef),
                FunctionBuilder.BuildIsNull<TestDataObject>(x => x.Name));
        }

        #endregion
    }
}
