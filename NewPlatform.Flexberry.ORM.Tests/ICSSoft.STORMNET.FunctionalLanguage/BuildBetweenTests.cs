namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.UserDataTypes;

    using Xunit;

    public class BuildBetweenTests : BaseFunctionTest
    {
        private const bool Bool2 = false;

        private const int Int2 = 2;

        private const decimal Decimal2 = 1.5m;

        private static readonly NullableInt NullableInt1 = 3;

        private static readonly NullableInt NullableInt2 = 4;

        private static readonly NullableDecimal NullableDecimal1 = (NullableDecimal)2.1m;

        private static readonly NullableDecimal NullableDecimal2 = (NullableDecimal)2.3m;

        private const string String2 = "dsa";

        private const tDayOfWeek Enum1 = tDayOfWeek.Day1;

        private const tDayOfWeek Enum2 = tDayOfWeek.Day2;

        private static readonly NullableDateTime NullableDateTime1 = NullableDateTime.Now;

        private static readonly NullableDateTime NullableDateTime2 = NullableDateTime.Today;

        [Fact]
        public void BuildBetweenTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(NullVarDef, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest02()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(GuidVarDef, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest03()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildBetween(GuidVarDef, Guid1, Bool1));
        }

        [Fact]
        public void BuildBetweenTest04()
        {
            Assert.Throws<InvalidCastException>(() => FunctionBuilder.BuildBetween(BoolVarDef, Guid1, Bool1));
        }

        [Fact]
        public void BuildBetweenTest10()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween(GuidVarDef, Guid1, Guid2));
        }

        [Fact]
        public void BuildBetweenTest11()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildBetween(GuidVarDef, KeyGuid1, KeyGuid2));
        }

        [Fact]
        public void BuildBetweenTest12()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1), PKHelper.GetKeyByObject(TestDataObject2)),
                FunctionBuilder.BuildBetween(GuidVarDef, TestDataObject1, TestDataObject2));
        }

        [Fact]
        public void BuildBetweenTest13()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), KeyGuid1),
                FunctionBuilder.BuildBetween(GuidVarDef, Guid1, KeyGuid1));
        }

        [Fact]
        public void BuildBetweenTest14()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildBetween(GuidVarDef, Guid1, TestDataObject1));
        }

        [Fact]
        public void BuildBetweenTest20()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, BoolVarDef, Bool1, Bool2),
                FunctionBuilder.BuildBetween(BoolVarDef, Bool1, Bool2));
        }

        [Fact]
        public void BuildBetweenTest30()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, Int2));
        }

        [Fact]
        public void BuildBetweenTest31()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.BuildBetween(NumericVarDef, Decimal1, Decimal2));
        }

        [Fact]
        public void BuildBetweenTest32()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableInt2),
                FunctionBuilder.BuildBetween(NumericVarDef, NullableInt1, NullableInt2));
        }

        [Fact]
        public void BuildBetweenTest33()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableDecimal1, NullableDecimal2),
                FunctionBuilder.BuildBetween(NumericVarDef, NullableDecimal1, NullableDecimal2));
        }

        [Fact]
        public void BuildBetweenTest34()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, Decimal1));
        }

        [Fact]
        public void BuildBetweenTest35()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, NullableInt1));
        }

        [Fact]
        public void BuildBetweenTest36()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest37()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef, Decimal1, NullableInt1));
        }

        [Fact]
        public void BuildBetweenTest38()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, Decimal1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest39()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, NullableInt1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest40()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, String2),
                FunctionBuilder.BuildBetween(StringVarDef, String1, String2));
        }

        [Fact]
        public void BuildBetweenTest41()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, EnumCaption.GetCaptionFor(Enum1), EnumCaption.GetCaptionFor(Enum2)),
                FunctionBuilder.BuildBetween(StringVarDef, Enum1, Enum2));
        }

        [Fact]
        public void BuildBetweenTest42()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, EnumCaption.GetCaptionFor(Enum1)),
                FunctionBuilder.BuildBetween(StringVarDef, String1, Enum1));
        }

        [Fact]
        public void BuildBetweenTest50()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef, DateTime2, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest51()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, NullableDateTime1),
                FunctionBuilder.BuildBetween(DateVarDef, NullableDateTime2, NullableDateTime1));
        }

        [Fact]
        public void BuildBetweenTest52()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef, NullableDateTime2, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest101()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(NullLambda, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest102()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringGenVarDef, String1, DateTime1.ToString()),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, String1, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest103()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildBetween<TestDataObject>(x => x.Hierarchy, String1, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest104()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringGenVarDef, String1, String2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, String1, String2));
        }

        [Fact]
        public void BuildBetweenTest105()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateGenVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.BirthDate, DateTime2, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest106()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, IntGenVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Height, Int1, Int2));
        }

        [Fact]
        public void BuildBetweenTest107()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidGenVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Hierarchy, Guid1, Guid2));
        }

        [Fact]
        public void BuildBetweenTest110()
        {
            Assert.Throws<FormatException>(() => FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.Weight, String2));
        }

        [Fact]
        public void BuildBetweenTest111()
        {
            Assert.Throws<InvalidCastException>(() => FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.Weight, 1.ToString()));
        }

        [Fact]
        public void BuildBetweenTest112()
        {
            Assert.Throws<InvalidCastException>(() => FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.BirthDate, DateTime1.ToString()));
        }

        [Fact]
        public void BuildBetweenTest113()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringGenVarDef, StringGenVarDef1, String2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.NickName, String2));
        }

        [Fact]
        public void BuildBetweenTest114()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateGenVarDef, DateGenVarDef1, DateTime1),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.BirthDate, x => x.DeathDate, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest115()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, IntGenVarDef, IntGenVarDef1, Int2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Height, x => x.Weight, Int2));
        }

        [Fact]
        public void BuildBetweenTest200()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(NullString, NullObjectType, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest201()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, NullObjectType, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest202()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest203()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, NullObject));
        }

        [Fact]
        public void BuildBetweenTest204()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, Bool1));
        }

        [Fact]
        public void BuildBetweenTest205()
        {
            Assert.Throws<InvalidCastException>(() => FunctionBuilder.BuildBetween(BoolVarDef.Caption, BoolVarDef.Type, Guid1, Bool1));
        }

        [Fact]
        public void BuildBetweenTest210()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, Guid2));
        }

        [Fact]
        public void BuildBetweenTest211()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, KeyGuid1, KeyGuid2));
        }

        [Fact]
        public void BuildBetweenTest212()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1), PKHelper.GetKeyByObject(TestDataObject2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, TestDataObject1, TestDataObject2));
        }

        [Fact]
        public void BuildBetweenTest213()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), KeyGuid1),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, KeyGuid1));
        }

        [Fact]
        public void BuildBetweenTest214()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, TestDataObject1));
        }

        [Fact]
        public void BuildBetweenTest220()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, BoolVarDef, Bool1, Bool2),
                FunctionBuilder.BuildBetween(BoolVarDef.Caption, BoolVarDef.Type, Bool1, Bool2));
        }

        [Fact]
        public void BuildBetweenTest230()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, Int2));
        }

        [Fact]
        public void BuildBetweenTest231()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Decimal1, Decimal2));
        }

        [Fact]
        public void BuildBetweenTest232()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableInt2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, NullableInt1, NullableInt2));
        }

        [Fact]
        public void BuildBetweenTest233()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableDecimal1, NullableDecimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, NullableDecimal1, NullableDecimal2));
        }

        [Fact]
        public void BuildBetweenTest234()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, Decimal1));
        }

        [Fact]
        public void BuildBetweenTest235()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, NullableInt1));
        }

        [Fact]
        public void BuildBetweenTest236()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest237()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Decimal1, NullableInt1));
        }

        [Fact]
        public void BuildBetweenTest238()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Decimal1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest239()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, NullableInt1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest240()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, String2),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, StringVarDef.Type, String1, String2));
        }

        [Fact]
        public void BuildBetweenTest241()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, EnumCaption.GetCaptionFor(Enum1), EnumCaption.GetCaptionFor(Enum2)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, StringVarDef.Type, Enum1, Enum2));
        }

        [Fact]
        public void BuildBetweenTest242()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, EnumCaption.GetCaptionFor(Enum1)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, StringVarDef.Type, String1, Enum1));
        }

        [Fact]
        public void BuildBetweenTest250()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateVarDef.Type, DateTime2, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest251()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, NullableDateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateVarDef.Type, NullableDateTime2, NullableDateTime1));
        }

        [Fact]
        public void BuildBetweenTest252()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateVarDef.Type, NullableDateTime2, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest300()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(NullString, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest301()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, NullObject, NullObject));
        }

        [Fact]
        public void BuildBetweenTest302()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, NullObject));
        }

        [Fact]
        public void BuildBetweenTest303()
        {
            Assert.Throws<ArgumentException>(() => FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, Bool1));
        }

        [Fact]
        public void BuildBetweenTest310()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, Guid2));
        }

        [Fact]
        public void BuildBetweenTest311()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, KeyGuid1, KeyGuid2));
        }

        [Fact]
        public void BuildBetweenTest312()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1), PKHelper.GetKeyByObject(TestDataObject2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, TestDataObject1, TestDataObject2));
        }

        [Fact]
        public void BuildBetweenTest313()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), KeyGuid1),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, KeyGuid1));
        }

        [Fact]
        public void BuildBetweenTest314()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, TestDataObject1));
        }

        [Fact]
        public void BuildBetweenTest320()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, BoolVarDef, Bool1, Bool2),
                FunctionBuilder.BuildBetween(BoolVarDef.Caption, Bool1, Bool2));
        }

        [Fact]
        public void BuildBetweenTest330()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, Int2));
        }

        [Fact]
        public void BuildBetweenTest331()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Decimal1, Decimal2));
        }

        [Fact]
        public void BuildBetweenTest332()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableInt2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NullableInt1, NullableInt2));
        }

        [Fact]
        public void BuildBetweenTest333()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableDecimal1, NullableDecimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NullableDecimal1, NullableDecimal2));
        }

        [Fact]
        public void BuildBetweenTest334()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, Decimal1));
        }

        [Fact]
        public void BuildBetweenTest335()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, NullableInt1));
        }

        [Fact]
        public void BuildBetweenTest336()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest337()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Decimal1, NullableInt1));
        }

        [Fact]
        public void BuildBetweenTest338()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Decimal1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest339()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NullableInt1, NullableDecimal1));
        }

        [Fact]
        public void BuildBetweenTest340()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, String2),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, String1, String2));
        }

        [Fact]
        public void BuildBetweenTest341()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, EnumCaption.GetCaptionFor(Enum1), EnumCaption.GetCaptionFor(Enum2)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, Enum1, Enum2));
        }

        [Fact]
        public void BuildBetweenTest342()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, EnumCaption.GetCaptionFor(Enum1)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, String1, Enum1));
        }

        [Fact]
        public void BuildBetweenTest350()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateTime2, DateTime1));
        }

        [Fact]
        public void BuildBetweenTest351()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, NullableDateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, NullableDateTime2, NullableDateTime1));
        }

        [Fact]
        public void BuildBetweenTest352()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, NullableDateTime2, DateTime1));
        }
    }
}