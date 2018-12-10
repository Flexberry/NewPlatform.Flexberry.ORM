namespace IIS.University.Tools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.KeyGen;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildTests : BaseFunctionTest
    {
        private static readonly List<Guid> RepeatedGuidList = new List<Guid> { Guid1, Guid1 };

        private static readonly List<Guid> GuidList = new List<Guid> { Guid1, Guid2 };

        private static readonly List<KeyGuid> RepeatedKeyGuidList = new List<KeyGuid> { KeyGuid1, KeyGuid1 };

        private static readonly List<KeyGuid> KeyGuidList = new List<KeyGuid> { KeyGuid1, KeyGuid2 };

        private const int Int2 = 2;

        private static readonly List<int> RepeatedIntList = new List<int> { Int1, Int1 };

        private static readonly List<int> IntList = new List<int> { Int1, Int2 };

        private const decimal Decimal2 = 1.5m;

        private static readonly List<decimal> RepeatedDecimalList = new List<decimal> { Decimal1, Decimal1 };

        private static readonly List<decimal> DecimalList = new List<decimal> { Decimal1, Decimal2 };
        
        private const tDayOfWeek TestDayOfWeek1 = tDayOfWeek.Day1;

        private const tDayOfWeek TestDayOfWeek2 = tDayOfWeek.Day2;

        private static readonly List<tDayOfWeek> RepeatedEnumList = new List<tDayOfWeek> { TestDayOfWeek1, TestDayOfWeek1 };

        private static readonly List<tDayOfWeek> EnumList = new List<tDayOfWeek> { TestDayOfWeek1, TestDayOfWeek2 };

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildTest01()
        {
            FunctionBuilder.Build(LangDef.funcIN, NullVarDef);
        }

        [TestMethod]
        public void BuildTest02()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef));
        }

        [TestMethod]
        public void BuildTest03()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1));
        }

        [TestMethod]
        public void BuildTest04()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1));
        }

        [TestMethod]
        public void BuildTest05()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid1));
        }

        [TestMethod]
        public void BuildTest06()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid1));
        }

        [TestMethod]
        public void BuildTest07()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedGuidList));
        }

        [TestMethod]
        public void BuildTest08()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedKeyGuidList));
        }

        [TestMethod]
        public void BuildTest09()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedGuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest10()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedKeyGuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest11()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedGuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest12()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedKeyGuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest51()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2));
        }

        [TestMethod]
        public void BuildTest52()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid2));
        }

        [TestMethod]
        public void BuildTest53()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList));
        }

        [TestMethod]
        public void BuildTest54()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList));
        }

        [TestMethod]
        public void BuildTest55()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest56()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest58()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest71()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, KeyGuidList));
        }

        [TestMethod]
        public void BuildTest72()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList, GuidList));
        }

        [TestMethod]
        public void BuildTest73()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), KeyGuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest74()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.ToArray(), GuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest75()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), KeyGuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest76()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.Where(x => true), GuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest77()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, KeyGuidList.ToArray(), KeyGuidList.Where(x => true), KeyGuid1, KeyGuid2));
        }

        [TestMethod]
        public void BuildTest78()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList, GuidList.ToArray(), GuidList.Where(x => true), Guid1, Guid2));
        }

        [TestMethod]
        public void BuildTest79()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, GuidList));
        }

        [TestMethod]
        public void BuildTest80()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), GuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest81()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), GuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest82()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, GuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest83()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, GuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest84()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), GuidList));
        }

        [TestMethod]
        public void BuildTest85()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), GuidList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest86()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), GuidList));
        }

        [TestMethod]
        public void BuildTest87()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), GuidList.ToArray()));
        }

        [TestMethod]
        public void BuildTest100()
        {
            Assert.AreEqual(
                FuncFalse,
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef));
        }

        [TestMethod]
        public void BuildTest101()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Int1));
        }

        [TestMethod]
        public void BuildTest102()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Decimal1));
        }

        [TestMethod]
        public void BuildTest103()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Int1, Int1));
        }

        [TestMethod]
        public void BuildTest104()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Decimal1, Decimal1));
        }

        [TestMethod]
        public void BuildTest105()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedIntList));
        }

        [TestMethod]
        public void BuildTest106()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedDecimalList));
        }

        [TestMethod]
        public void BuildTest107()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedIntList.ToArray()));
        }

        [TestMethod]
        public void BuildTest108()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedDecimalList.ToArray()));
        }

        [TestMethod]
        public void BuildTest109()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedIntList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest110()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedDecimalList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest111()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcEQ, NumericVarDef, Decimal1));
        }

        [TestMethod]
        public void BuildTest121()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Int2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, IntList));
        }

        [TestMethod]
        public void BuildTest122()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, DecimalList));
        }

        [TestMethod]
        public void BuildTest123()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Int2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, IntList.ToArray()));
        }

        [TestMethod]
        public void BuildTest124()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, DecimalList.ToArray()));
        }

        [TestMethod]
        public void BuildTest125()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Int2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, IntList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest126()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, DecimalList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest127()
        {
            // R U CRAZY?!
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Int1, Decimal1));
        }

        [TestMethod]
        public void BuildTest201()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, TestDayOfWeek1));
        }

        [TestMethod]
        public void BuildTest202()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, TestDayOfWeek1, TestDayOfWeek1));
        }

        [TestMethod]
        public void BuildTest203()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, RepeatedEnumList));
        }

        [TestMethod]
        public void BuildTest204()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, RepeatedEnumList.ToArray()));
        }

        [TestMethod]
        public void BuildTest205()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, RepeatedEnumList.Where(x => true)));
        }

        [TestMethod]
        public void BuildTest206()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1), EnumCaption.GetCaptionFor(TestDayOfWeek2)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, EnumList));
        }

        [TestMethod]
        public void BuildTest207()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1), EnumCaption.GetCaptionFor(TestDayOfWeek2)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, EnumList.ToArray()));
        }

        [TestMethod]
        public void BuildTest209()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIN, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1), EnumCaption.GetCaptionFor(TestDayOfWeek2)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, EnumList.Where(x => true)));
        }
    }
}