namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildCompareTests : BaseFunctionTest
    {
        [Fact]
        public void BuildCompareTest1()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(NullString, NumericVarDef, Int1));
        }

        [Fact]
        public void BuildCompareTest2()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare(LangDef.funcIN, NumericVarDef, Int1));
        }

        [Fact]
        public void BuildCompareTest3()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NullVarDef, Int1));
        }

        [Fact]
        public void BuildCompareTest4()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef, NullObject));
        }

        [Fact]
        public void BuildCompareTest5()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef, "asd"));
        }

        [Fact]
        public void BuildCompareTest6()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, NumericVarDef, Int1),
                FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef, Int1));
        }

        [Fact]
        public void BuildCompareTest7()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateVarDef, DateTime1),
                FunctionBuilder.BuildCompare(LangDef.funcL, DateVarDef, DateTime1));
        }

        [Fact]
        public void BuildCompareTest8()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringVarDef, String1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringVarDef, String1));
        }

        [Fact]
        public void BuildCompareTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(NullString, NumericVarDef.Caption, NumericVarDef.Type, Int1));
        }

        [Fact]
        public void BuildCompareTest22()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare(LangDef.funcIN, NumericVarDef.Caption, NumericVarDef.Type, Int1));
        }

        [Fact]
        public void BuildCompareTest23()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NullString, NumericVarDef.Type, Int1));
        }

        [Fact]
        public void BuildCompareTest24()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NullObjectType, Int1));
        }

        [Fact]
        public void BuildCompareTest25()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NumericVarDef.Type, NullObject));
        }

        [Fact]
        public void BuildCompareTest26()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NumericVarDef.Type, "asd"));
        }

        [Fact]
        public void BuildCompareTest27()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, NumericVarDef, Int1),
                FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NumericVarDef.Type, Int1));
        }

        [Fact]
        public void BuildCompareTest28()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateVarDef, DateTime1),
                FunctionBuilder.BuildCompare(LangDef.funcL, DateVarDef.Caption, DateVarDef.Type, DateTime1));
        }

        [Fact]
        public void BuildCompareTest29()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringVarDef, String1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringVarDef.Caption, StringVarDef.Type, String1));
        }

        [Fact]
        public void BuildCompareTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(NullString, NumericVarDef.Caption, Int1));
        }

        [Fact]
        public void BuildCompareTest32()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare(LangDef.funcIN, NumericVarDef.Caption, Int1));
        }

        [Fact]
        public void BuildCompareTest33()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NullString, Int1));
        }

        [Fact]
        public void BuildCompareTest34()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NullObject));
        }

        [Fact]
        public void BuildCompareTest35()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, NumericVarDef, Int1),
                FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, Int1));
        }

        [Fact]
        public void BuildCompareTest36()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateVarDef, DateTime1),
                FunctionBuilder.BuildCompare(LangDef.funcL, DateVarDef.Caption, DateTime1));
        }

        [Fact]
        public void BuildCompareTest37()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringVarDef, String1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringVarDef.Caption, String1));
        }

        [Fact]
        public void BuildCompareTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare<TestDataObject>(NullString, x => x.Height, Int1));
        }

        [Fact]
        public void BuildCompareTest42()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcIN, x => x.Height, Int1));
        }

        [Fact]
        public void BuildCompareTest43()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, NullObject));
        }

        [Fact]
        public void BuildCompareTest44()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, "asd"));
        }

        [Fact]
        public void BuildCompareTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, Int1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, Int1));
        }

        [Fact]
        public void BuildCompareTest46()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildCompareTest47()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, String1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, String1));
        }

        [Fact]
        public void BuildCompareTest51()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(NullString, StringGenVarDef, StringGenVarDef));
        }

        [Fact]
        public void BuildCompareTest52()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NullVarDef, StringGenVarDef));
        }

        [Fact]
        public void BuildCompareTest53()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, StringGenVarDef, NullVarDef));
        }

        [Fact]
        public void BuildCompareTest54()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, IntGenVarDef, DateGenVarDef));
        }

        [Fact]
        public void BuildCompareTest55()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildCompareTest56()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare(LangDef.funcIN, StringGenVarDef, IntGenVarDef));
        }

        [Fact]
        public void BuildCompareTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringGenVarDef, StringGenVarDef1));
        }

        [Fact]
        public void BuildCompareTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare<TestDataObject>(NullString, x => x.Hierarchy, x => x.Hierarchy));
        }

        [Fact]
        public void BuildCompareTest62()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, NullLambda, x => x.Hierarchy));
        }

        [Fact]
        public void BuildCompareTest63()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildCompare(LangDef.funcL, x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildCompareTest64()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildCompareTest65()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildCompareTest71()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildCompareTest72()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildCompareTest73()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildCompareTest74()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildCompareTest75()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcLEQ, x => x.Name, x => x.Name));
        }

        [Fact]
        public void BuildCompareTest76()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcGEQ, x => x.Name, x => x.Name));
        }

        [Fact]
        public void BuildCompareTest77()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.Name));
        }

        [Fact]
        public void BuildCompareTest78()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcG, x => x.Name, x => x.Name));
        }
    }
}