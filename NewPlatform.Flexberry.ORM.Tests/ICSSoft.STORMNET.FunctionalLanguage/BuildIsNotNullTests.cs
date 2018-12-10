namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildIsNotNullTests : BaseFunctionTest
    {
        #region NotIsNull

        [Fact]
        public void BuildNotIsNullTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIsNotNull(NullString));
        }

        [Fact]
        public void BuildNotIsNullTest12()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, StringVarDef),
                FunctionBuilder.BuildIsNotNull(PropertyName));
        }

        [Fact]
        public void BuildNotIsNullTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildIsNotNull(NullVarDef));
        }

        [Fact]
        public void BuildNotIsNullTest22()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, StringVarDef),
                FunctionBuilder.BuildIsNotNull(StringVarDef));
        }

        [Fact]
        public void BuildNotIsNullTest31()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, IntGenVarDef),
                FunctionBuilder.BuildIsNotNull<TestDataObject>(x => x.Height));
        }

        [Fact]
        public void BuilddNotIsNullTest32()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, DateGenVarDef),
                FunctionBuilder.BuildIsNotNull<TestDataObject>(x => x.BirthDate));
        }

        [Fact]
        public void BuilddNotIsNullTest33()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, StringGenVarDef),
                FunctionBuilder.BuildIsNotNull<TestDataObject>(x => x.Name));
        }
        #endregion
    }
}
