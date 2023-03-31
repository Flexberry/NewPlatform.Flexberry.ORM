namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildLessTests : BaseFunctionTest
    {
        #region Less

        [Fact]
        public void BuildLessTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(NullString, NullObject));
        }

        [Fact]
        public void BuildLessTest12()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(PropertyName, NullObject));
        }

        [Fact]
        public void BuildLessTest13()
        {
            FunctionBuilder.BuildLess(PropertyName, Int1);
        }

        [Fact]
        public void BuildLessTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(NullString, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildLessTest22()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(PropertyName, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildLessTest23()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(PropertyName, LangDef.NumericType, NullObject));
        }

        [Fact]
        public void BuildLessTest24()
        {
            FunctionBuilder.BuildLess(PropertyName, LangDef.NumericType, Int1);
        }

        [Fact]
        public void BuildLessTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildLessTest32()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(NumericVarDef, NullObject));
        }

        [Fact]
        public void BuildLessTest33()
        {
            FunctionBuilder.BuildLess(NumericVarDef, Int1);
        }

        [Fact]
        public void BuildLessTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, NullObject));
        }

        [Fact]
        public void BuildLessTest42()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, "asd"));
        }

        [Fact]
        public void BuildLessTest43()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, Int1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, Int1));
        }

        [Fact]
        public void BuildLessTest44()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildLessTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, String1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildLessTest52()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(NullVarDef, StringGenVarDef));
        }

        [Fact]
        public void BuildLessTest53()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(StringGenVarDef, NullVarDef));
        }

        [Fact]
        public void BuildLessTest54()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLess(IntGenVarDef, DateGenVarDef));
        }

        [Fact]
        public void BuildLessTest55()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLess(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildLessTest56()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLess(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildLessTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLess(StringGenVarDef, StringGenVarDef1));
        }

        [Fact]
        public void BuildLessTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(NullLambda, x => x.Hierarchy));
        }

        [Fact]
        public void BuildLessTest62()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildLess(x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildLessTest63()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildLessTest64()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildLessTest71()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildLessTest72()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildLessTest73()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildLessTest74()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildLessTest75()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildLess<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
