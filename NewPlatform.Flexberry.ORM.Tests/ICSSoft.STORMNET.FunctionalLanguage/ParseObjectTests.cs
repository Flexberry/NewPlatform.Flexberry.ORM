namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.UserDataTypes;

    using Xunit;

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

        [Fact]
        public void ParseObjectTest00()
        {
            Assert.NotNull(FunctionHelper.ParseObject(BoolType, BoolList));
        }

        [Fact]
        public void ParseObjectTest01()
        {
            Assert.NotNull(FunctionHelper.ParseObject(BoolType, BoolList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest02()
        {
            Assert.NotNull(FunctionHelper.ParseObject(BoolType, BoolList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest03()
        {
            Assert.NotNull(FunctionHelper.ParseObject(IntType, IntList));
        }

        [Fact]
        public void ParseObjectTest04()
        {
            Assert.NotNull(FunctionHelper.ParseObject(IntType, IntList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest05()
        {
            Assert.NotNull(FunctionHelper.ParseObject(IntType, IntList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest06()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NumericType, DecimalList));
        }

        [Fact]
        public void ParseObjectTest07()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NumericType, DecimalList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest08()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NumericType, DecimalList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest09()
        {
            Assert.NotNull(FunctionHelper.ParseObject(StringType, StringList));
        }

        [Fact]
        public void ParseObjectTest10()
        {
            Assert.NotNull(FunctionHelper.ParseObject(StringType, StringList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest11()
        {
            Assert.NotNull(FunctionHelper.ParseObject(StringType, StringList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest12()
        {
            Assert.NotNull(FunctionHelper.ParseObject(DateTimeType, DateTimeList));
        }

        [Fact]
        public void ParseObjectTest13()
        {
            Assert.NotNull(FunctionHelper.ParseObject(DateTimeType, DateTimeList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest14()
        {
            Assert.NotNull(FunctionHelper.ParseObject(DateTimeType, DateTimeList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest15()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableDateTimeType, NullableDateTimeList));
        }

        [Fact]
        public void ParseObjectTest16()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableDateTimeType, NullableDateTimeList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest17()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableDateTimeType, NullableDateTimeList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest18()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableIntType, NullableIntList));
        }

        [Fact]
        public void ParseObjectTest19()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableIntType, NullableIntList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest20()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableIntType, NullableIntList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest21()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableDecimalType, NullableDecimalList));
        }

        [Fact]
        public void ParseObjectTest22()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableDecimalType, NullableDecimalList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest23()
        {
            Assert.NotNull(FunctionHelper.ParseObject(NullableDecimalType, NullableDecimalList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest24()
        {
            Assert.NotNull(FunctionHelper.ParseObject(EnumType, EnumList));
        }

        [Fact]
        public void ParseObjectTest25()
        {
            Assert.NotNull(FunctionHelper.ParseObject(EnumType, EnumList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest26()
        {
            Assert.NotNull(FunctionHelper.ParseObject(EnumType, EnumList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest27()
        {
            Assert.NotNull(FunctionHelper.ParseObject(nIntType, nIntList));
        }

        [Fact]
        public void ParseObjectTest28()
        {
            Assert.NotNull(FunctionHelper.ParseObject(nIntType, nIntList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest29()
        {
            Assert.NotNull(FunctionHelper.ParseObject(nIntType, nIntList.Where(x => true)));
        }

        [Fact]
        public void ParseObjectTest30()
        {
            Assert.NotNull(FunctionHelper.ParseObject(nNumericType, nDecimalList));
        }

        [Fact]
        public void ParseObjectTest31()
        {
            Assert.NotNull(FunctionHelper.ParseObject(nNumericType, nDecimalList.ToArray()));
        }

        [Fact]
        public void ParseObjectTest32()
        {
            Assert.NotNull(FunctionHelper.ParseObject(nNumericType, nDecimalList.Where(x => true)));
        }
    }
}