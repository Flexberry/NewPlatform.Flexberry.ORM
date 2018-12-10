namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildNotEqualsTests : BaseFunctionTest
    {
        private const string GuidString = "EFB82498-1935-41F0-B8CB-129372B4430E";

        #region Equals

        [Fact]
        public void BuildNotEqualsTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest12()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(GuidString));
        }

        [Fact]
        public void BuildNotEqualsTest13()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(Guid1));
        }

        [Fact]
        public void BuildNotEqualsTest14()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(KeyGuid1));
        }

        [Fact]
        public void BuildNotEqualsTest15()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(TestDataObject1));
        }

        [Fact]
        public void BuildNotEqualsTest16()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildNotEquals(1));
        }

        [Fact]
        public void BuildNotEqualsTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullString, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest22()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidVarDef),
                FunctionBuilder.BuildNotEquals(PropertyName, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest23()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(PropertyName, GuidString));
        }

        [Fact]
        public void BuildNotEqualsTest24()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, Guid1));
        }

        [Fact]
        public void BuildNotEqualsTest25()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, KeyGuid1));
        }

        [Fact]
        public void BuildNotEqualsTest26()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(PropertyName, TestDataObject1));
        }

        [Fact]
        public void BuildNotEqualsTest27()
        {
            // TODO: Enum
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, StringVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, 1));
        }

        [Fact]
        public void BuildNotEqualsTest28()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, 1));
        }

        [Fact]
        public void BuildNotEqualsTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullString, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest32()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(PropertyName, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest33()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidVarDef),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest34()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, GuidString));
        }

        [Fact]
        public void BuildNotEqualsTest35()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, Guid1));
        }

        [Fact]
        public void BuildNotEqualsTest36()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, KeyGuid1));
        }

        [Fact]
        public void BuildNotEqualsTest37()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, TestDataObject1));
        }

        [Fact]
        public void BuildNotEqualsTest38()
        {
            // TODO: Enum
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.NumericType, 1));
        }

        [Fact]
        public void BuildNotEqualsTest39()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.NumericType, 1));
        }

        [Fact]
        public void BuildNotEqualsTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest42()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest43()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidVarDef),
                FunctionBuilder.BuildNotEquals(GuidVarDef, NullObject));
        }

        [Fact]
        public void BuildNotEqualsTest44()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, GuidString));
        }

        [Fact]
        public void BuildNotEqualsTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, Guid1));
        }

        [Fact]
        public void BuildNotEqualsTest46()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, KeyGuid1));
        }

        [Fact]
        public void BuildNotEqualsTest47()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, TestDataObject1));
        }

        [Fact]
        public void BuildNotEqualsTest48()
        {
            FunctionBuilder.BuildNotEquals(StringVarDef, "");
        }

        [Fact]
        public void BuildNotEqualsTest49()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(NumericVarDef, 1));
        }

        [Fact]
        public void BuildNotEqualsTest51()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, "asd"));
        }

        [Fact]
        public void BuildNotEqualsTest52()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, "asd"));
        }

        [Fact]
        public void BuildNotEqualsTest53()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidGenVarDef),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, NullTestDataObject));
        }

        [Fact]
        public void BuildNotEqualsTest54()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidGenVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, KeyGuid1));
        }

        [Fact]
        public void BuildNotEqualsTest55()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildNotEqualsTest56()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildNotEqualsTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, Int1));
        }

        [Fact]
        public void BuildNotEqualsTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildNotEqualsTest62()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildNotEqualsTest63()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildNotEqualsTest64()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildNotEqualsTest65()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildNotEqualsTest66()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildNotEqualsTest67()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildNotEqualsTest68()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.Name));
        }

        [Fact]
        public void BuildNotEqualsTest69()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildNotEquals<IName>(x => x.Name, String1));
        }

        [Fact]
        public void BuildNotEqualsTest70()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullFunction));
        }

        [Fact]
        public void BuildNotEqualsTest71()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullVarDef, NullFunction));
        }

        [Fact]
        public void BuildNotEqualsTest72()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(GuidVarDef, NullFunction));
        }

        [Fact]
        public void BuildNotEqualsTest73()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals(NullLambda, NullFunction));
        }

        [Fact]
        public void BuildNotEqualsTest74()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, NullFunction));
        }

        [Fact]
        public void BuildNotEqualsTest80()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, PrimaryKeyVarDef, FuncSQL),
                FunctionBuilder.BuildNotEquals(FuncSQL));
        }

        [Fact]
        public void BuildNotEqualsTest81()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, FuncSQL),
                FunctionBuilder.BuildNotEquals(GuidVarDef, FuncSQL));
        }

        [Fact]
        public void BuildNotEqualsTest82()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcNEQ, IntGenVarDef, FuncSQL),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, FuncSQL));
        }

        #endregion
    }
}