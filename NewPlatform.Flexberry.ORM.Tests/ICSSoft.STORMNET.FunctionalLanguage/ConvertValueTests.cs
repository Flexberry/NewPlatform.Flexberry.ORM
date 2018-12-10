namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;

    using Xunit;

    public class ConvertValueTests : BaseTest
    {
        private static readonly ObjectType DataObjectType = LangDef.DataObjectType;

        private static readonly ObjectType GuidType = LangDef.GuidType;

        private static readonly ObjectType BoolType = LangDef.BoolType;

        private static readonly ObjectType NumericType = LangDef.NumericType;

        private static readonly ObjectType StringType = LangDef.StringType;

        private static readonly ObjectType DateTimeType = LangDef.DateTimeType;

        private const string TestGuidString = "EFB82498-1935-41F0-B8CB-129372B4430E";

        private static readonly Guid TestGuid = Guid.NewGuid();

        private static readonly KeyGuid TestKeyGuid = KeyGuid.NewGuid();

        private static readonly TestDataObject TestTestDataObject = new TestDataObject();

        private const bool TestBool = true;

        private const int TestInt = 1;

        private const decimal TestDecimal = decimal.MinusOne;

        private const string TestString = "asd";

        private static readonly DateTime TestDateTime = DateTime.Now;

        private static readonly NullableDateTime TestNullableDateTime = NullableDateTime.Now;

        private static readonly NullableInt TestNullableInt = TestInt;

        private static readonly NullableDecimal TestNullableDecimal = (NullableDecimal)TestDecimal;

        private const tDayOfWeek TestDayOfWeek = tDayOfWeek.Day1;

        private const object NullObject = null;

        [Fact]
        public void ConvertValueTest01()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestGuidString),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestGuidString));
        }

        [Fact]
        public void ConvertValueTest02()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestGuid),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestGuid));
        }

        [Fact]
        public void ConvertValueTest03()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestKeyGuid),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestKeyGuid));
        }

        [Fact]
        public void ConvertValueTest04()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestTestDataObject),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestTestDataObject));
        }

        [Fact]
        public void ConvertValueTest05()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestBool));
        }

        [Fact]
        public void ConvertValueTest06()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestInt));
        }

        [Fact]
        public void ConvertValueTest07()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestDecimal));
        }

        [Fact]
        public void ConvertValueTest08()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestString));
        }

        [Fact]
        public void ConvertValueTest09()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestDateTime));
        }

        [Fact]
        public void ConvertValueTest11()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestGuidString),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestGuidString));
        }

        [Fact]
        public void ConvertValueTest12()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestGuid),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestGuid));
        }

        [Fact]
        public void ConvertValueTest13()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestKeyGuid),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestKeyGuid));
        }

        [Fact]
        public void ConvertValueTest14()
        {
            Assert.Equal(
                PKHelper.GetKeyByObject(TestTestDataObject),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestTestDataObject));
        }

        [Fact]
        public void ConvertValueTest15()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestBool));
        }

        [Fact]
        public void ConvertValueTest16()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestInt));
        }

        [Fact]
        public void ConvertValueTest17()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestString));
        }

        [Fact]
        public void ConvertValueTest18()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestDateTime));
        }

        [Fact]
        public void ConvertValueTest19()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestDayOfWeek));
        }

        [Fact]
        public void ConvertValueTest21()
        {
            Assert.Throws<FormatException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestGuidString));
        }

        [Fact]
        public void ConvertValueTest22()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestGuid));
        }

        [Fact]
        public void ConvertValueTest23()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestKeyGuid));
        }

        [Fact]
        public void ConvertValueTest24()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestTestDataObject));
        }

        [Fact]
        public void ConvertValueTest25()
        {
            Assert.Equal(
                TestBool,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestBool));
        }

        [Fact]
        public void ConvertValueTest26()
        {
            Assert.Equal(
                true,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestInt));
        }

        [Fact]
        public void ConvertValueTest27()
        {
            Assert.Equal(
                true,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestDecimal));
        }

        [Fact]
        public void ConvertValueTest28()
        {
            Assert.Throws<FormatException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestString));
        }

        [Fact]
        public void ConvertValueTest29()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestDateTime));
        }

        [Fact]
        public void ConvertValueTest31()
        {
            Assert.Throws<FormatException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestGuidString));
        }

        [Fact]
        public void ConvertValueTest32()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestGuid));
        }

        [Fact]
        public void ConvertValueTest33()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestKeyGuid));
        }

        [Fact]
        public void ConvertValueTest34()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestTestDataObject));
        }

        [Fact]
        public void ConvertValueTest35()
        {
            Assert.Equal(
                1m,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestBool));
        }

        [Fact]
        public void ConvertValueTest36()
        {
            Assert.Equal(
                TestInt,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestInt));
        }

        [Fact]
        public void ConvertValueTest37()
        {
            Assert.Equal(
                TestDecimal,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestDecimal));
        }

        [Fact]
        public void ConvertValueTest38()
        {
            Assert.Throws<FormatException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestString));
        }

        [Fact]
        public void ConvertValueTest39()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestDateTime));
        }

        [Fact]
        public void ConvertValueTest41()
        {
            Assert.Equal(
                TestGuidString,
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestGuidString));
        }

        [Fact]
        public void ConvertValueTest42()
        {
            Assert.Equal(
                TestGuid.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestGuid));
        }

        [Fact]
        public void ConvertValueTest43()
        {
            Assert.Equal(
                TestKeyGuid.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestKeyGuid));
        }

        [Fact]
        public void ConvertValueTest44()
        {
            Assert.Equal(
                TestTestDataObject.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestTestDataObject));
        }

        [Fact]
        public void ConvertValueTest45()
        {
            Assert.Equal(
                TestBool.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestBool));
        }

        [Fact]
        public void ConvertValueTest46()
        {
            Assert.Equal(
                TestInt.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestInt));
        }

        [Fact]
        public void ConvertValueTest47()
        {
            Assert.Equal(
                TestDecimal.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestDecimal));
        }

        [Fact]
        public void ConvertValueTest48()
        {
            Assert.Equal(
                TestString,
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestString));
        }

        [Fact]
        public void ConvertValueTest49()
        {
            Assert.Equal(
                TestDateTime.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestDateTime));
        }

        [Fact]
        public void ConvertValueTest51()
        {
            Assert.Throws<FormatException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestGuidString));
        }

        [Fact]
        public void ConvertValueTest52()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestGuid));
        }

        [Fact]
        public void ConvertValueTest53()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestKeyGuid));
        }

        [Fact]
        public void ConvertValueTest54()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestTestDataObject));
        }

        [Fact]
        public void ConvertValueTest55()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestBool));
        }

        [Fact]
        public void ConvertValueTest56()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestInt));
        }

        [Fact]
        public void ConvertValueTest57()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestDecimal));
        }

        [Fact]
        public void ConvertValueTest58()
        {
            Assert.Throws<FormatException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestString));
        }

        [Fact]
        public void ConvertValueTest59()
        {
            Assert.Equal(
                TestDateTime,
                FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestDateTime));
        }

        [Fact]
        public void ConvertValueTest71()
        {
            Assert.Equal(
                TestNullableInt,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestNullableInt));
        }

        [Fact]
        public void ConvertValueTest72()
        {
            Assert.Equal(
                TestNullableDecimal,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestNullableDecimal));
        }

        [Fact]
        public void ConvertValueTest73()
        {
            Assert.Equal(
                TestNullableDateTime,
                FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestNullableDateTime));
        }

        [Fact]
        public void ConvertValueTest81()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, new List<DataObject>()));
        }

        [Fact]
        public void ConvertValueTest82()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, new List<Guid>()));
        }

        [Fact]
        public void ConvertValueTest83()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, new List<bool>()));
        }

        [Fact]
        public void ConvertValueTest84()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, new List<int>()));
        }

        [Fact]
        public void ConvertValueTest85()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(StringType.NetCompatibilityType, new List<string>()));
        }

        [Fact]
        public void ConvertValueTest86()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, new List<DateTime>()));
        }

        [Fact]
        public void ConvertValueTest91()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, NullObject));
        }

        [Fact]
        public void ConvertValueTest92()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, NullObject));
        }

        [Fact]
        public void ConvertValueTest93()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, NullObject));
        }

        [Fact]
        public void ConvertValueTest94()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, NullObject));
        }

        [Fact]
        public void ConvertValueTest95()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ConvertValue(StringType.NetCompatibilityType, NullObject));
        }

        [Fact]
        public void ConvertValueTest96()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, NullObject));
        }

        [Fact]
        public void ConvertValueTest101()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestDayOfWeek));
        }

        [Fact]
        public void ConvertValueTest102()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestDayOfWeek));
        }

        [Fact]
        public void ConvertValueTest103()
        {
            Assert.Equal(
                true,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestDayOfWeek));
        }

        [Fact]
        public void ConvertValueTest104()
        {
            Assert.Equal(
                1m,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestDayOfWeek));
        }

        [Fact]
        public void ConvertValueTest105()
        {
            FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestDayOfWeek);
        }

        [Fact]
        public void ConvertValueTest106()
        {
            Assert.Throws<InvalidCastException>(() => FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestDayOfWeek));
        }
    }
}