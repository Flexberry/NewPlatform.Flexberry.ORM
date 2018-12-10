namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;

    using Xunit;

    public class BuildEqualsTests : BaseFunctionTest
    {
        private const string GuidString = "EFB82498-1935-41F0-B8CB-129372B4430E";

        #region Equals

        [Fact]
        public void BuildEqualsTest11()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullObject));
        }

        [Fact]
        public void BuildEqualsTest12()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(GuidString));
        }

        [Fact]
        public void BuildEqualsTest13()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(Guid1));
        }

        [Fact]
        public void BuildEqualsTest14()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(KeyGuid1));
        }

        [Fact]
        public void BuildEqualsTest15()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(TestDataObject1));
        }

        [Fact]
        public void BuildEqualsTest16()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildEquals(1));
        }

        [Fact]
        public void BuildEqualsTest21()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullString, NullObject));
        }

        [Fact]
        public void BuildEqualsTest22()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(PropertyName, NullObject));
        }

        [Fact]
        public void BuildEqualsTest23()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(PropertyName, GuidString));
        }

        [Fact]
        public void BuildEqualsTest24()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(PropertyName, Guid1));
        }

        [Fact]
        public void BuildEqualsTest25()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(PropertyName, KeyGuid1));
        }

        [Fact]
        public void BuildEqualsTest26()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(PropertyName, TestDataObject1));
        }

        [Fact]
        public void BuildEqualsTest27()
        {
            // TODO: Enum
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, 1));
        }

        [Fact]
        public void BuildEqualsTest28()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, 1));
        }

        [Fact]
        public void BuildEqualsTest30()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullString, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildEqualsTest31()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(PropertyName, NullObjectType, NullObject));
        }

        [Fact]
        public void BuildEqualsTest32()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, NullObject));
        }

        [Fact]
        public void BuildEqualsTest33()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, ""));
        }

        [Fact]
        public void BuildEqualsTest34()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, GuidString));
        }

        [Fact]
        public void BuildEqualsTest35()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, Guid1));
        }

        [Fact]
        public void BuildEqualsTest36()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, KeyGuid1));
        }

        [Fact]
        public void BuildEqualsTest37()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, TestDataObject1));
        }

        [Fact]
        public void BuildEqualsTest38()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.NumericType, 1));
        }

        [Fact]
        public void BuildEqualsTest39()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.NumericType, 1));
        }

        [Fact]
        public void BuildEqualsTest40()
        {
            Assert.Equal(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    StringVarDef,
                    EnumCaption.GetCaptionFor(tDayOfWeek.Day0)),
                FunctionBuilder.BuildEquals(StringVarDef, tDayOfWeek.Day0));
        }

        [Fact]
        public void BuildEqualsTest41()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildEqualsTest42()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullVarDef, NullObject));
        }

        [Fact]
        public void BuildEqualsTest43()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(GuidVarDef, NullObject));
        }

        [Fact]
        public void BuildEqualsTest44()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(GuidVarDef, GuidString));
        }

        [Fact]
        public void BuildEqualsTest45()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1));
        }

        [Fact]
        public void BuildEqualsTest46()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1));
        }

        [Fact]
        public void BuildEqualsTest47()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1));
        }

        [Fact]
        public void BuildEqualsTest48()
        {
            FunctionBuilder.BuildEquals(StringVarDef, "");
        }

        [Fact]
        public void BuildEqualsTest49()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(NumericVarDef, 1));
        }

        [Fact]
        public void BuildEqualsTest51()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildEquals<TestDataObject>(x => x.Hierarchy, "asd"));
        }

        [Fact]
        public void BuildEqualsTest52()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, "asd"));
        }

        [Fact]
        public void BuildEqualsTest53()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIsNull, GuidGenVarDef),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Hierarchy, NullTestDataObject));
        }

        [Fact]
        public void BuildEqualsTest54()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidGenVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Hierarchy, KeyGuid1));
        }

        [Fact]
        public void BuildEqualsTest55()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, String1));
        }

        [Fact]
        public void BuildEqualsTest56()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [Fact]
        public void BuildEqualsTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, Int1));
        }

        [Fact]
        public void BuildEqualsTest61()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(x => x.Hierarchy, NullLambda));
        }

        [Fact]
        public void BuildEqualsTest62()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, x => x.BirthDate));
        }

        [Fact]
        public void BuildEqualsTest63()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.Height));
        }

        [Fact]
        public void BuildEqualsTest64()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [Fact]
        public void BuildEqualsTest65()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [Fact]
        public void BuildEqualsTest66()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [Fact]
        public void BuildEqualsTest67()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [Fact]
        public void BuildEqualsTest68()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.Name));
        }

        [Fact]
        public void BuildEqualsTest69()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildEquals<IName>(x => x.Name, String1));
        }

        [Fact]
        public void BuildEqualsTest70()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullFunction));
        }

        [Fact]
        public void BuildEqualsTest71()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullVarDef, NullFunction));
        }

        [Fact]
        public void BuildEqualsTest72()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(GuidVarDef, NullFunction));
        }

        [Fact]
        public void BuildEqualsTest73()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals(NullLambda, NullFunction));
        }

        [Fact]
        public void BuildEqualsTest74()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, NullFunction));
        }

        [Fact]
        public void BuildEqualsTest80()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, PrimaryKeyVarDef, FuncSQL),
                FunctionBuilder.BuildEquals(FuncSQL));
        }

        [Fact]
        public void BuildEqualsTest81()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, FuncSQL),
                FunctionBuilder.BuildEquals(GuidVarDef, FuncSQL));
        }

        [Fact]
        public void BuildEqualsTest82()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, FuncSQL),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, FuncSQL));
        }

        #endregion
    }
}