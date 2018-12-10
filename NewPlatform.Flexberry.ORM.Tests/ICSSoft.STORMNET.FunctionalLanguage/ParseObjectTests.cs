namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.UserDataTypes;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParseObjectTests : BaseTest
    {
        private const bool Bool1 = true;

        private const bool Bool2 = false;

        private static readonly List<bool> BoolList = new List<bool> { Bool1, Bool2 };

        private static readonly Type BoolType = LangDef.BoolType.NetCompatibilityType;

        private const int Int1 = 1;

        private const int Int2 = 2;

        private static readonly List<int> IntList = new List<int> { Int1, Int2 };

        private static readonly Type IntType = typeof(int);

        private static readonly List<int?> nIntList = new List<int?> { Int1, Int2 };

        private static readonly Type nIntType = typeof(int?);

        private const decimal Decimal1 = decimal.One;

        private const decimal Decimal2 = 1.5m;

        private static readonly List<decimal> DecimalList = new List<decimal> { Decimal1, Decimal2 };

        private static readonly Type NumericType = LangDef.NumericType.NetCompatibilityType; // decimal

        private static readonly List<decimal?> nDecimalList = new List<decimal?> { Decimal1, Decimal2 };

        private static readonly Type nNumericType = LangDef.NumericType.NetCompatibilityType; // decimal

        private const string String1 = "asd";

        private const string String2 = "dsa";

        private static readonly List<string> StringList = new List<string> { String1, String2 };

        private static readonly Type StringType = LangDef.StringType.NetCompatibilityType;

        private static readonly DateTime DateTime1 = DateTime.Now;

        private static readonly DateTime DateTime2 = DateTime.Today;

        private static readonly List<DateTime> DateTimeList = new List<DateTime> { DateTime1, DateTime2 };

        private static readonly Type DateTimeType = LangDef.DateTimeType.NetCompatibilityType; // DateTime

        private static readonly NullableDateTime NullableDateTime1 = NullableDateTime.Now;

        private static readonly NullableDateTime NullableDateTime2 = NullableDateTime.Today;

        private static readonly List<NullableDateTime> NullableDateTimeList = new List<NullableDateTime> { NullableDateTime1, NullableDateTime2 };

        private static readonly Type NullableDateTimeType = typeof(NullableDateTime);

        private static readonly NullableInt NullableInt1 = Int1;

        private static readonly NullableInt NullableInt2 = Int2;

        private static readonly List<NullableInt> NullableIntList = new List<NullableInt> { NullableInt1, NullableInt2 };

        private static readonly Type NullableIntType = typeof(NullableInt);

        private static readonly NullableDecimal NullableDecimal1 = (NullableDecimal)Decimal1;

        private static readonly NullableDecimal NullableDecimal2 = (NullableDecimal)Decimal2;

        private static readonly List<NullableDecimal> NullableDecimalList = new List<NullableDecimal> { NullableDecimal1, NullableDecimal2 };

        private static readonly Type NullableDecimalType = typeof(NullableDecimal);

        private const tDayOfWeek Enum1 = tDayOfWeek.Day1;

        private const tDayOfWeek Enum2 = tDayOfWeek.Day2;

        private static readonly List<tDayOfWeek> EnumList = new List<tDayOfWeek> { Enum1, Enum2 };

        private static readonly Type EnumType = typeof(tDayOfWeek);

        [TestMethod]
        public void ParseObjectTest00()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(BoolType, BoolList));
        }

        [TestMethod]
        public void ParseObjectTest01()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(BoolType, BoolList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest02()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(BoolType, BoolList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest03()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(IntType, IntList));
        }

        [TestMethod]
        public void ParseObjectTest04()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(IntType, IntList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest05()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(IntType, IntList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest06()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NumericType, DecimalList));
        }

        [TestMethod]
        public void ParseObjectTest07()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NumericType, DecimalList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest08()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NumericType, DecimalList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest09()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(StringType, StringList));
        }

        [TestMethod]
        public void ParseObjectTest10()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(StringType, StringList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest11()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(StringType, StringList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest12()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(DateTimeType, DateTimeList));
        }

        [TestMethod]
        public void ParseObjectTest13()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(DateTimeType, DateTimeList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest14()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(DateTimeType, DateTimeList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest15()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableDateTimeType, NullableDateTimeList));
        }

        [TestMethod]
        public void ParseObjectTest16()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableDateTimeType, NullableDateTimeList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest17()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableDateTimeType, NullableDateTimeList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest18()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableIntType, NullableIntList));
        }

        [TestMethod]
        public void ParseObjectTest19()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableIntType, NullableIntList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest20()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableIntType, NullableIntList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest21()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableDecimalType, NullableDecimalList));
        }

        [TestMethod]
        public void ParseObjectTest22()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableDecimalType, NullableDecimalList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest23()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(NullableDecimalType, NullableDecimalList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest24()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(EnumType, EnumList));
        }

        [TestMethod]
        public void ParseObjectTest25()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(EnumType, EnumList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest26()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(EnumType, EnumList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest27()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(nIntType, nIntList));
        }

        [TestMethod]
        public void ParseObjectTest28()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(nIntType, nIntList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest29()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(nIntType, nIntList.Where(x => true)));
        }

        [TestMethod]
        public void ParseObjectTest30()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(nNumericType, nDecimalList));
        }

        [TestMethod]
        public void ParseObjectTest31()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(nNumericType, nDecimalList.ToArray()));
        }

        [TestMethod]
        public void ParseObjectTest32()
        {
            Assert.IsNotNull(FunctionHelper.ParseObject(nNumericType, nDecimalList.Where(x => true)));
        }
    }
}