namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
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

        [TestMethod]
        public void ValidateValueTest01()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestGuidString),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestGuidString));
        }

        [TestMethod]
        public void ValidateValueTest02()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestGuid),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestGuid));
        }

        [TestMethod]
        public void ValidateValueTest03()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestKeyGuid),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestKeyGuid));
        }

        [TestMethod]
        public void ValidateValueTest04()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestTestDataObject),
                FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestTestDataObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest05()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestBool);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest06()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestInt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest07()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestDecimal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest08()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest09()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestDateTime);
        }

        [TestMethod]
        public void ValidateValueTest11()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestGuidString),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestGuidString));
        }

        [TestMethod]
        public void ValidateValueTest12()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestGuid),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestGuid));
        }

        [TestMethod]
        public void ValidateValueTest13()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestKeyGuid),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestKeyGuid));
        }

        [TestMethod]
        public void ValidateValueTest14()
        {
            Assert.AreEqual(
                PKHelper.GetKeyByObject(TestTestDataObject),
                FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestTestDataObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest15()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestBool);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest16()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestInt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest17()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest18()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestDateTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest19()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestDayOfWeek);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateValueTest21()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestGuidString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest22()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest23()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestKeyGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest24()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestTestDataObject);
        }

        [TestMethod]
        public void ValidateValueTest25()
        {
            Assert.AreEqual(
                TestBool,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestBool));
        }

        [TestMethod]
        public void ValidateValueTest26()
        {
            Assert.AreEqual(
                true,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestInt));
        }

        [TestMethod]
        public void ValidateValueTest27()
        {
            Assert.AreEqual(
                true,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestDecimal));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateValueTest28()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest29()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestDateTime);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateValueTest31()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestGuidString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest32()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest33()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestKeyGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest34()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestTestDataObject);
        }

        [TestMethod]
        public void ValidateValueTest35()
        {
            Assert.AreEqual(
                1m,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestBool));
        }

        [TestMethod]
        public void ValidateValueTest36()
        {
            Assert.AreEqual(
                TestInt,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestInt));
        }

        [TestMethod]
        public void ValidateValueTest37()
        {
            Assert.AreEqual(
                TestDecimal,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestDecimal));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateValueTest38()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest39()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestDateTime);
        }

        [TestMethod]
        public void ValidateValueTest41()
        {
            Assert.AreEqual(
                TestGuidString,
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestGuidString));
        }

        [TestMethod]
        public void ValidateValueTest42()
        {
            Assert.AreEqual(
                TestGuid.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestGuid));
        }

        [TestMethod]
        public void ValidateValueTest43()
        {
            Assert.AreEqual(
                TestKeyGuid.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestKeyGuid));
        }

        [TestMethod]
        public void ValidateValueTest44()
        {
            Assert.AreEqual(
                TestTestDataObject.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestTestDataObject));
        }

        [TestMethod]
        public void ValidateValueTest45()
        {
            Assert.AreEqual(
                TestBool.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestBool));
        }

        [TestMethod]
        public void ValidateValueTest46()
        {
            Assert.AreEqual(
                TestInt.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestInt));
        }

        [TestMethod]
        public void ValidateValueTest47()
        {
            Assert.AreEqual(
                TestDecimal.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestDecimal));
        }

        [TestMethod]
        public void ValidateValueTest48()
        {
            Assert.AreEqual(
                TestString,
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestString));
        }

        [TestMethod]
        public void ValidateValueTest49()
        {
            Assert.AreEqual(
                TestDateTime.ToString(),
                FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestDateTime));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateValueTest51()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestGuidString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest52()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest53()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestKeyGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest54()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestTestDataObject);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest55()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestBool);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest56()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestInt);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest57()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestDecimal);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ValidateValueTest58()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestString);
        }

        [TestMethod]
        public void ValidateValueTest59()
        {
            Assert.AreEqual(
                TestDateTime,
                FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestDateTime));
        }

        [TestMethod]
        public void ValidateValueTest71()
        {
            Assert.AreEqual(
                TestNullableInt,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestNullableInt));
        }

        [TestMethod]
        public void ValidateValueTest72()
        {
            Assert.AreEqual(
                TestNullableDecimal,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestNullableDecimal));
        }

        [TestMethod]
        public void ValidateValueTest73()
        {
            Assert.AreEqual(
                TestNullableDateTime,
                FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestNullableDateTime));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest81()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, new List<DataObject>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest82()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, new List<Guid>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest83()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, new List<bool>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest84()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, new List<int>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest85()
        {
            FunctionHelper.ConvertValue(StringType.NetCompatibilityType, new List<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest86()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, new List<DateTime>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest91()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest92()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest93()
        {
            FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest94()
        {
            FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest95()
        {
            FunctionHelper.ConvertValue(StringType.NetCompatibilityType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest96()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest101()
        {
            FunctionHelper.ConvertValue(DataObjectType.NetCompatibilityType, TestDayOfWeek);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateValueTest102()
        {
            FunctionHelper.ConvertValue(GuidType.NetCompatibilityType, TestDayOfWeek);
        }

        [TestMethod]
        public void ValidateValueTest103()
        {
            Assert.AreEqual(
                true,
                FunctionHelper.ConvertValue(BoolType.NetCompatibilityType, TestDayOfWeek));
        }

        [TestMethod]
        public void ValidateValueTest104()
        {
            Assert.AreEqual(
                1m,
                FunctionHelper.ConvertValue(NumericType.NetCompatibilityType, TestDayOfWeek));
        }

        [TestMethod]
        public void ValidateValueTest105()
        {
            FunctionHelper.ConvertValue(StringType.NetCompatibilityType, TestDayOfWeek);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ValidateValueTest106()
        {
            FunctionHelper.ConvertValue(DateTimeType.NetCompatibilityType, TestDayOfWeek);
        }
    }
}