namespace IIS.University.Tools.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.UserDataTypes;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest01()
        {
            FunctionBuilder.BuildBetween(NullVarDef, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest02()
        {
            FunctionBuilder.BuildBetween(GuidVarDef, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildBetweenTest03()
        {
            FunctionBuilder.BuildBetween(GuidVarDef, Guid1, Bool1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildBetweenTest04()
        {
            FunctionBuilder.BuildBetween(BoolVarDef, Guid1, Bool1);
        }

        [TestMethod]
        public void BuildBetweenTest10()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween(GuidVarDef, Guid1, Guid2));
        }

        [TestMethod]
        public void BuildBetweenTest11()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildBetween(GuidVarDef, KeyGuid1, KeyGuid2));
        }

        [TestMethod]
        public void BuildBetweenTest12()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1), PKHelper.GetKeyByObject(TestDataObject2)),
                FunctionBuilder.BuildBetween(GuidVarDef, TestDataObject1, TestDataObject2));
        }

        [TestMethod]
        public void BuildBetweenTest13()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), KeyGuid1),
                FunctionBuilder.BuildBetween(GuidVarDef, Guid1, KeyGuid1));
        }

        [TestMethod]
        public void BuildBetweenTest14()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildBetween(GuidVarDef, Guid1, TestDataObject1));
        }

        [TestMethod]
        public void BuildBetweenTest20()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, BoolVarDef, Bool1, Bool2),
                FunctionBuilder.BuildBetween(BoolVarDef, Bool1, Bool2));
        }

        [TestMethod]
        public void BuildBetweenTest30()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, Int2));
        }

        [TestMethod]
        public void BuildBetweenTest31()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.BuildBetween(NumericVarDef, Decimal1, Decimal2));
        }

        [TestMethod]
        public void BuildBetweenTest32()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableInt2),
                FunctionBuilder.BuildBetween(NumericVarDef, NullableInt1, NullableInt2));
        }

        [TestMethod]
        public void BuildBetweenTest33()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableDecimal1, NullableDecimal2),
                FunctionBuilder.BuildBetween(NumericVarDef, NullableDecimal1, NullableDecimal2));
        }

        [TestMethod]
        public void BuildBetweenTest34()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, Decimal1));
        }

        [TestMethod]
        public void BuildBetweenTest35()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, NullableInt1));
        }

        [TestMethod]
        public void BuildBetweenTest36()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, Int1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest37()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef, Decimal1, NullableInt1));
        }

        [TestMethod]
        public void BuildBetweenTest38()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, Decimal1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest39()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef, NullableInt1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest40()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, String2),
                FunctionBuilder.BuildBetween(StringVarDef, String1, String2));
        }

        [TestMethod]
        public void BuildBetweenTest41()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, EnumCaption.GetCaptionFor(Enum1), EnumCaption.GetCaptionFor(Enum2)),
                FunctionBuilder.BuildBetween(StringVarDef, Enum1, Enum2));
        }

        [TestMethod]
        public void BuildBetweenTest42()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, EnumCaption.GetCaptionFor(Enum1)),
                FunctionBuilder.BuildBetween(StringVarDef, String1, Enum1));
        }

        [TestMethod]
        public void BuildBetweenTest50()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef, DateTime2, DateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest51()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, NullableDateTime1),
                FunctionBuilder.BuildBetween(DateVarDef, NullableDateTime2, NullableDateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest52()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef, NullableDateTime2, DateTime1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest101()
        {
            FunctionBuilder.BuildBetween(NullLambda, NullObject, NullObject);
        }

        [TestMethod]
        public void BuildBetweenTest102()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringGenVarDef, String1, DateTime1.ToString()),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, String1, DateTime1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildBetweenTest103()
        {
            FunctionBuilder.BuildBetween<TestDataObject>(x => x.Hierarchy, String1, DateTime1);
        }

        [TestMethod]
        public void BuildBetweenTest104()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringGenVarDef, String1, String2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, String1, String2));
        }

        [TestMethod]
        public void BuildBetweenTest105()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateGenVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.BirthDate, DateTime2, DateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest106()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, IntGenVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Height, Int1, Int2));
        }

        [TestMethod]
        public void BuildBetweenTest107()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidGenVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Hierarchy, Guid1, Guid2));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildBetweenTest110()
        {
            FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.Weight, String2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildBetweenTest111()
        {
            FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.Weight, 1.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildBetweenTest112()
        {
            FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.BirthDate, DateTime1.ToString());
        }

        [TestMethod]
        public void BuildBetweenTest113()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringGenVarDef, StringGenVarDef1, String2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Name, x => x.NickName, String2));
        }

        [TestMethod]
        public void BuildBetweenTest114()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateGenVarDef, DateGenVarDef1, DateTime1),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.BirthDate, x => x.DeathDate, DateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest115()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, IntGenVarDef, IntGenVarDef1, Int2),
                FunctionBuilder.BuildBetween<TestDataObject>(x => x.Height, x => x.Weight, Int2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest200()
        {
            FunctionBuilder.BuildBetween(NullString, NullObjectType, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest201()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, NullObjectType, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest202()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest203()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildBetweenTest204()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, Bool1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildBetweenTest205()
        {
            FunctionBuilder.BuildBetween(BoolVarDef.Caption, BoolVarDef.Type, Guid1, Bool1);
        }

        [TestMethod]
        public void BuildBetweenTest210()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, Guid2));
        }

        [TestMethod]
        public void BuildBetweenTest211()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, KeyGuid1, KeyGuid2));
        }

        [TestMethod]
        public void BuildBetweenTest212()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1), PKHelper.GetKeyByObject(TestDataObject2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, TestDataObject1, TestDataObject2));
        }

        [TestMethod]
        public void BuildBetweenTest213()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), KeyGuid1),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, KeyGuid1));
        }

        [TestMethod]
        public void BuildBetweenTest214()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, GuidVarDef.Type, Guid1, TestDataObject1));
        }

        [TestMethod]
        public void BuildBetweenTest220()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, BoolVarDef, Bool1, Bool2),
                FunctionBuilder.BuildBetween(BoolVarDef.Caption, BoolVarDef.Type, Bool1, Bool2));
        }

        [TestMethod]
        public void BuildBetweenTest230()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, Int2));
        }

        [TestMethod]
        public void BuildBetweenTest231()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Decimal1, Decimal2));
        }

        [TestMethod]
        public void BuildBetweenTest232()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableInt2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, NullableInt1, NullableInt2));
        }

        [TestMethod]
        public void BuildBetweenTest233()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableDecimal1, NullableDecimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, NullableDecimal1, NullableDecimal2));
        }

        [TestMethod]
        public void BuildBetweenTest234()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, Decimal1));
        }

        [TestMethod]
        public void BuildBetweenTest235()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, NullableInt1));
        }

        [TestMethod]
        public void BuildBetweenTest236()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Int1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest237()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Decimal1, NullableInt1));
        }

        [TestMethod]
        public void BuildBetweenTest238()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, Decimal1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest239()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NumericVarDef.Type, NullableInt1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest240()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, String2),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, StringVarDef.Type, String1, String2));
        }

        [TestMethod]
        public void BuildBetweenTest241()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, EnumCaption.GetCaptionFor(Enum1), EnumCaption.GetCaptionFor(Enum2)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, StringVarDef.Type, Enum1, Enum2));
        }

        [TestMethod]
        public void BuildBetweenTest242()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, EnumCaption.GetCaptionFor(Enum1)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, StringVarDef.Type, String1, Enum1));
        }

        [TestMethod]
        public void BuildBetweenTest250()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateVarDef.Type, DateTime2, DateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest251()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, NullableDateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateVarDef.Type, NullableDateTime2, NullableDateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest252()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateVarDef.Type, NullableDateTime2, DateTime1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest300()
        {
            FunctionBuilder.BuildBetween(NullString, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest301()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, NullObject, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildBetweenTest302()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildBetweenTest303()
        {
            FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, Bool1);
        }

        [TestMethod]
        public void BuildBetweenTest310()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, Guid2));
        }

        [TestMethod]
        public void BuildBetweenTest311()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, KeyGuid1, KeyGuid2));
        }

        [TestMethod]
        public void BuildBetweenTest312()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1), PKHelper.GetKeyByObject(TestDataObject2)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, TestDataObject1, TestDataObject2));
        }

        [TestMethod]
        public void BuildBetweenTest313()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), KeyGuid1),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, KeyGuid1));
        }

        [TestMethod]
        public void BuildBetweenTest314()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildBetween(GuidVarDef.Caption, Guid1, TestDataObject1));
        }

        [TestMethod]
        public void BuildBetweenTest320()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, BoolVarDef, Bool1, Bool2),
                FunctionBuilder.BuildBetween(BoolVarDef.Caption, Bool1, Bool2));
        }

        [TestMethod]
        public void BuildBetweenTest330()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Int2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, Int2));
        }

        [TestMethod]
        public void BuildBetweenTest331()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Decimal1, Decimal2));
        }

        [TestMethod]
        public void BuildBetweenTest332()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableInt2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NullableInt1, NullableInt2));
        }

        [TestMethod]
        public void BuildBetweenTest333()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableDecimal1, NullableDecimal2),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NullableDecimal1, NullableDecimal2));
        }

        [TestMethod]
        public void BuildBetweenTest334()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption,  Int1, Decimal1));
        }

        [TestMethod]
        public void BuildBetweenTest335()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, NullableInt1));
        }

        [TestMethod]
        public void BuildBetweenTest336()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Int1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Int1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest337()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableInt1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Decimal1, NullableInt1));
        }

        [TestMethod]
        public void BuildBetweenTest338()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, Decimal1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, Decimal1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest339()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, NumericVarDef, NullableInt1, NullableDecimal1),
                FunctionBuilder.BuildBetween(NumericVarDef.Caption, NullableInt1, NullableDecimal1));
        }

        [TestMethod]
        public void BuildBetweenTest340()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, String2),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, String1, String2));
        }

        [TestMethod]
        public void BuildBetweenTest341()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, EnumCaption.GetCaptionFor(Enum1), EnumCaption.GetCaptionFor(Enum2)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, Enum1, Enum2));
        }

        [TestMethod]
        public void BuildBetweenTest342()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, StringVarDef, String1, EnumCaption.GetCaptionFor(Enum1)),
                FunctionBuilder.BuildBetween(StringVarDef.Caption, String1, Enum1));
        }

        [TestMethod]
        public void BuildBetweenTest350()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, DateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, DateTime2, DateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest351()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, NullableDateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, NullableDateTime2, NullableDateTime1));
        }

        [TestMethod]
        public void BuildBetweenTest352()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcBETWEEN, DateVarDef, NullableDateTime2, DateTime1),
                FunctionBuilder.BuildBetween(DateVarDef.Caption, NullableDateTime2, DateTime1));
        }
    }
}