namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildGreaterOrEqualTests : BaseFunctionTest
    {
        #region GreaterOrEqual

        [Fact]
        public void BuildGreaterOrEqualTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(NullString, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest12()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(PropertyName, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest13()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, 1);
        }

        [Fact]
        public void BuildGreaterOrEqualTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(NullString, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest22()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(PropertyName, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest23()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(PropertyName, LangDef.NumericType, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest24()
        {
            FunctionBuilder.BuildGreaterOrEqual(PropertyName, LangDef.NumericType, 1);
        }

        [Fact]
        public void BuildGreaterOrEqualTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest32()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(NumericVarDef, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest33()
        {
            FunctionBuilder.BuildGreaterOrEqual(NumericVarDef, 1);
        }

        [Fact]
        public void BuildGreaterOrEqualTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, NullObject));
        }

        [Fact]
        public void BuildGreaterOrEqualTest42()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, "asd"));
        }

        [Fact]
        public void BuildGreaterOrEqualTest43()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, Int1));
        }

        [Fact]
        public void BuildGreaterOrEqualTest44()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildGreaterOrEqualTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildGreaterOrEqualTest52()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(NullVarDef, StringGenVarDef));
        }

        [Fact]
        public void BuildGreaterOrEqualTest53()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, NullVarDef));
        }

        [Fact]
        public void BuildGreaterOrEqualTest54()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreaterOrEqual(IntGenVarDef, DateGenVarDef));
        }

        [Fact]
        public void BuildGreaterOrEqualTest55()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildGreaterOrEqualTest56()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildGreaterOrEqualTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual(StringGenVarDef, StringGenVarDef1));
        }

        [Fact]
        public void BuildGreaterOrEqualTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(NullLambda, x => x.Hierarchy));
        }

        [Fact]
        public void BuildGreaterOrEqualTest62()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreaterOrEqual(x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildGreaterOrEqualTest63()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildGreaterOrEqualTest64()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildGreaterOrEqualTest71()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildGreaterOrEqualTest72()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildGreaterOrEqualTest73()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildGreaterOrEqualTest74()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildGreaterOrEqualTest75()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildGreaterOrEqual<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
