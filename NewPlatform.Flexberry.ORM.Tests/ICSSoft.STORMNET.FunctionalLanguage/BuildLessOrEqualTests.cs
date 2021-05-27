namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildLessOrEqualTests : BaseFunctionTest
    {
        #region LessOrEqual

        [Fact]
        public void BuildLessOrEqualTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(NullString, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest12()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(PropertyName, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest13()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, 1);
        }

        [Fact]
        public void BuildLessOrEqualTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(NullString, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest22()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(PropertyName, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest23()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(PropertyName, LangDef.NumericType, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest24()
        {
            FunctionBuilder.BuildLessOrEqual(PropertyName, LangDef.NumericType, 1);
        }

        [Fact]
        public void BuildLessOrEqualTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest32()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(NumericVarDef, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest33()
        {
            FunctionBuilder.BuildLessOrEqual(NumericVarDef, 1);
        }

        [Fact]
        public void BuildLessOrEqualTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, NullObject));
        }

        [Fact]
        public void BuildLessOrEqualTest42()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, "asd"));
        }

        [Fact]
        public void BuildLessOrEqualTest43()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, Int1));
        }

        [Fact]
        public void BuildLessOrEqualTest44()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildLessOrEqualTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildLessOrEqualTest52()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(NullVarDef, StringGenVarDef));
        }

        [Fact]
        public void BuildLessOrEqualTest53()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(StringGenVarDef, NullVarDef));
        }

        [Fact]
        public void BuildLessOrEqualTest54()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLessOrEqual(IntGenVarDef, DateGenVarDef));
        }

        [Fact]
        public void BuildLessOrEqualTest55()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLessOrEqual(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildLessOrEqualTest56()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLessOrEqual(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildLessOrEqualTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLessOrEqual(StringGenVarDef, StringGenVarDef1));
        }

        [Fact]
        public void BuildLessOrEqualTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(NullLambda, x => x.Hierarchy));
        }

        [Fact]
        public void BuildLessOrEqualTest62()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLessOrEqual(x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildLessOrEqualTest63()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildLessOrEqualTest64()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildLessOrEqualTest71()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildLessOrEqualTest72()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildLessOrEqualTest73()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildLessOrEqualTest74()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildLessOrEqualTest75()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildLessOrEqual<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
