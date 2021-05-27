namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildGreaterTests : BaseFunctionTest
    {
        #region Greater

        [Fact]
        public void BuildGreaterTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(NullString, NullObject));
        }

        [Fact]
        public void BuildGreaterTest12()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(PropertyName, NullObject));
        }

        [Fact]
        public void BuildGreaterTest13()
        {
            FunctionBuilder.BuildGreater(PropertyName, 1);
        }

        [Fact]
        public void BuildGreaterTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(NullString, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildGreaterTest22()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(PropertyName, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildGreaterTest23()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(PropertyName, LangDef.NumericType, NullObject));
        }

        [Fact]
        public void BuildGreaterTest24()
        {
            FunctionBuilder.BuildGreater(PropertyName, LangDef.NumericType, 1);
        }

        [Fact]
        public void BuildGreaterTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildGreaterTest32()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(NumericVarDef, NullObject));
        }

        [Fact]
        public void BuildGreaterTest33()
        {
            FunctionBuilder.BuildGreater(NumericVarDef, 1);
        }

        [Fact]
        public void BuildGreaterTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, NullObject));
        }

        [Fact]
        public void BuildGreaterTest42()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, "asd"));
        }

        [Fact]
        public void BuildGreaterTest43()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, IntGenVarDef, Int1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, Int1));
        }

        [Fact]
        public void BuildGreaterTest44()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildGreaterTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, String1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildGreaterTest52()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(NullVarDef, StringGenVarDef));
        }

        [Fact]
        public void BuildGreaterTest53()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(StringGenVarDef, NullVarDef));
        }

        [Fact]
        public void BuildGreaterTest54()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreater(IntGenVarDef, DateGenVarDef));
        }

        [Fact]
        public void BuildGreaterTest55()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreater(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildGreaterTest56()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreater(StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildGreaterTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreater(StringGenVarDef, StringGenVarDef1));
        }

        [Fact]
        public void BuildGreaterTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(NullLambda, x => x.Hierarchy));
        }

        [Fact]
        public void BuildGreaterTest62()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildGreater(x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildGreaterTest63()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildGreaterTest64()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildGreaterTest71()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildGreaterTest72()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildGreaterTest73()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildGreaterTest74()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildGreaterTest75()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildGreater<TestDataObject>(x => x.Name, x => x.Name));
        }

        #endregion
    }
}
