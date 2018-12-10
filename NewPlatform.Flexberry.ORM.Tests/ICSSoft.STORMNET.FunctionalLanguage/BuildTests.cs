namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;

    using Xunit;

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

        [Fact]
        public void BuildTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.Build(LangDef.funcIN, NullVarDef));
        }

        [Fact]
        public void BuildTest02()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef));
        }

        [Fact]
        public void BuildTest03()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1));
        }

        [Fact]
        public void BuildTest04()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1));
        }

        [Fact]
        public void BuildTest05()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid1));
        }

        [Fact]
        public void BuildTest06()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid1));
        }

        [Fact]
        public void BuildTest07()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedGuidList));
        }

        [Fact]
        public void BuildTest08()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedKeyGuidList));
        }

        [Fact]
        public void BuildTest09()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedGuidList.ToArray()));
        }

        [Fact]
        public void BuildTest10()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedKeyGuidList.ToArray()));
        }

        [Fact]
        public void BuildTest11()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedGuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest12()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, RepeatedKeyGuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest51()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2));
        }

        [Fact]
        public void BuildTest52()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, Guid1, Guid2));
        }

        [Fact]
        public void BuildTest53()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList));
        }

        [Fact]
        public void BuildTest54()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList));
        }

        [Fact]
        public void BuildTest55()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray()));
        }

        [Fact]
        public void BuildTest56()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.ToArray()));
        }

        [Fact]
        public void BuildTest57()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest58()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest71()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, KeyGuidList));
        }

        [Fact]
        public void BuildTest72()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList, GuidList));
        }

        [Fact]
        public void BuildTest73()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), KeyGuidList.ToArray()));
        }

        [Fact]
        public void BuildTest74()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.ToArray(), GuidList.ToArray()));
        }

        [Fact]
        public void BuildTest75()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), KeyGuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest76()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList.Where(x => true), GuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest77()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, KeyGuidList.ToArray(), KeyGuidList.Where(x => true), KeyGuid1, KeyGuid2));
        }

        [Fact]
        public void BuildTest78()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, GuidList, GuidList.ToArray(), GuidList.Where(x => true), Guid1, Guid2));
        }

        [Fact]
        public void BuildTest79()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, GuidList));
        }

        [Fact]
        public void BuildTest80()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), GuidList.ToArray()));
        }

        [Fact]
        public void BuildTest81()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), GuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest82()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, GuidList.ToArray()));
        }

        [Fact]
        public void BuildTest83()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList, GuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest84()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), GuidList));
        }

        [Fact]
        public void BuildTest85()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.ToArray(), GuidList.Where(x => true)));
        }

        [Fact]
        public void BuildTest86()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), GuidList));
        }

        [Fact]
        public void BuildTest87()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, GuidVarDef, KeyGuid1, KeyGuid2, PKHelper.GetKeyByObject(Guid1), PKHelper.GetKeyByObject(Guid2)),
                FunctionBuilder.Build(LangDef.funcIN, GuidVarDef, KeyGuidList.Where(x => true), GuidList.ToArray()));
        }

        [Fact]
        public void BuildTest100()
        {
            Assert.Equal(
                FuncFalse,
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef));
        }

        [Fact]
        public void BuildTest101()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Int1));
        }

        [Fact]
        public void BuildTest102()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Decimal1));
        }

        [Fact]
        public void BuildTest103()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Int1, Int1));
        }

        [Fact]
        public void BuildTest104()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Decimal1, Decimal1));
        }

        [Fact]
        public void BuildTest105()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedIntList));
        }

        [Fact]
        public void BuildTest106()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedDecimalList));
        }

        [Fact]
        public void BuildTest107()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedIntList.ToArray()));
        }

        [Fact]
        public void BuildTest108()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedDecimalList.ToArray()));
        }

        [Fact]
        public void BuildTest109()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Int1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedIntList.Where(x => true)));
        }

        [Fact]
        public void BuildTest110()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, RepeatedDecimalList.Where(x => true)));
        }

        [Fact]
        public void BuildTest111()
        {
            Assert.Equal(
                FunctionBuilder.BuildEquals(NumericVarDef, Decimal1),
                FunctionBuilder.Build(LangDef.funcEQ, NumericVarDef, Decimal1));
        }

        [Fact]
        public void BuildTest121()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Int2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, IntList));
        }

        [Fact]
        public void BuildTest122()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, DecimalList));
        }

        [Fact]
        public void BuildTest123()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Int2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, IntList.ToArray()));
        }

        [Fact]
        public void BuildTest124()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, DecimalList.ToArray()));
        }

        [Fact]
        public void BuildTest125()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Int2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, IntList.Where(x => true)));
        }

        [Fact]
        public void BuildTest126()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Decimal1, Decimal2),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, DecimalList.Where(x => true)));
        }

        [Fact]
        public void BuildTest127()
        {
            // R U CRAZY?!
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, NumericVarDef, Int1, Decimal1),
                FunctionBuilder.Build(LangDef.funcIN, NumericVarDef, Int1, Decimal1));
        }

        [Fact]
        public void BuildTest201()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, TestDayOfWeek1));
        }

        [Fact]
        public void BuildTest202()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, TestDayOfWeek1, TestDayOfWeek1));
        }

        [Fact]
        public void BuildTest203()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, RepeatedEnumList));
        }

        [Fact]
        public void BuildTest204()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, RepeatedEnumList.ToArray()));
        }

        [Fact]
        public void BuildTest205()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, RepeatedEnumList.Where(x => true)));
        }

        [Fact]
        public void BuildTest206()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1), EnumCaption.GetCaptionFor(TestDayOfWeek2)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, EnumList));
        }

        [Fact]
        public void BuildTest207()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1), EnumCaption.GetCaptionFor(TestDayOfWeek2)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, EnumList.ToArray()));
        }

        [Fact]
        public void BuildTest209()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcIN, StringVarDef, EnumCaption.GetCaptionFor(TestDayOfWeek1), EnumCaption.GetCaptionFor(TestDayOfWeek2)),
                FunctionBuilder.Build(LangDef.funcIN, StringVarDef, EnumList.Where(x => true)));
        }
    }
}