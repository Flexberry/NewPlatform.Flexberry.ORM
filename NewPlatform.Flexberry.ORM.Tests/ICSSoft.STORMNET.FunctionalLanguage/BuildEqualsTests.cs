namespace IIS.University.Tools.Tests
{
    using System;

    using ICSSoft.STORMNET;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildEqualsTests : BaseFunctionTest
    {
        private const string GuidString = "EFB82498-1935-41F0-B8CB-129372B4430E";

        #region Equals

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest11()
        {
            FunctionBuilder.BuildEquals(NullObject);
        }

        [TestMethod]
        public void BuildEqualsTest12()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(GuidString));
        }

        [TestMethod]
        public void BuildEqualsTest13()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(Guid1));
        }

        [TestMethod]
        public void BuildEqualsTest14()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(KeyGuid1));
        }

        [TestMethod]
        public void BuildEqualsTest15()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(TestDataObject1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildEqualsTest16()
        {
            FunctionBuilder.BuildEquals(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest21()
        {
            FunctionBuilder.BuildEquals(NullString, NullObject);
        }

        [TestMethod]
        public void BuildEqualsTest22()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(PropertyName, NullObject));
        }

        [TestMethod]
        public void BuildEqualsTest23()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(PropertyName, GuidString));
        }

        [TestMethod]
        public void BuildEqualsTest24()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(PropertyName, Guid1));
        }

        [TestMethod]
        public void BuildEqualsTest25()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(PropertyName, KeyGuid1));
        }

        [TestMethod]
        public void BuildEqualsTest26()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(PropertyName, TestDataObject1));
        }

        [TestMethod]
        public void BuildEqualsTest27()
        {
            // TODO: Enum
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, 1));
        }

        [TestMethod]
        public void BuildEqualsTest28()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest30()
        {
            FunctionBuilder.BuildEquals(NullString, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest31()
        {
            FunctionBuilder.BuildEquals(PropertyName, NullObjectType, NullObject);
        }

        [TestMethod]
        public void BuildEqualsTest32()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, NullObject));
        }

        [TestMethod]
        public void BuildEqualsTest33()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, ""));
        }

        [TestMethod]
        public void BuildEqualsTest34()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, GuidString));
        }

        [TestMethod]
        public void BuildEqualsTest35()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, Guid1));
        }

        [TestMethod]
        public void BuildEqualsTest36()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, KeyGuid1));
        }

        [TestMethod]
        public void BuildEqualsTest37()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.GuidType, TestDataObject1));
        }

        [TestMethod]
        public void BuildEqualsTest38()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.NumericType, 1));
        }

        [TestMethod]
        public void BuildEqualsTest39()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(PropertyName, LangDef.NumericType, 1));
        }

        [TestMethod]
        public void BuildEqualsTest40()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcEQ,
                    StringVarDef,
                    EnumCaption.GetCaptionFor(tDayOfWeek.Day0)),
                FunctionBuilder.BuildEquals(StringVarDef, tDayOfWeek.Day0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest41()
        {
            FunctionBuilder.BuildEquals(NullVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest42()
        {
            FunctionBuilder.BuildEquals(NullVarDef, NullObject);
        }

        [TestMethod]
        public void BuildEqualsTest43()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, GuidVarDef),
                FunctionBuilder.BuildEquals(GuidVarDef, NullObject));
        }

        [TestMethod]
        public void BuildEqualsTest44()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildEquals(GuidVarDef, GuidString));
        }

        [TestMethod]
        public void BuildEqualsTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildEquals(GuidVarDef, Guid1));
        }

        [TestMethod]
        public void BuildEqualsTest46()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals(GuidVarDef, KeyGuid1));
        }

        [TestMethod]
        public void BuildEqualsTest47()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildEquals(GuidVarDef, TestDataObject1));
        }

        [TestMethod]
        public void BuildEqualsTest48()
        {
            FunctionBuilder.BuildEquals(StringVarDef, "");
        }

        [TestMethod]
        public void BuildEqualsTest49()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, NumericVarDef, 1),
                FunctionBuilder.BuildEquals(NumericVarDef, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildEqualsTest51()
        {
            FunctionBuilder.BuildEquals<TestDataObject>(x => x.Hierarchy, "asd");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildEqualsTest52()
        {
            FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildEqualsTest53()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcIsNull, GuidGenVarDef),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Hierarchy, NullTestDataObject));
        }

        [TestMethod]
        public void BuildEqualsTest54()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidGenVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Hierarchy, KeyGuid1));
        }

        [TestMethod]
        public void BuildEqualsTest55()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        public void BuildEqualsTest56()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildEqualsTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, Int1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest61()
        {
            FunctionBuilder.BuildEquals(x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildEqualsTest62()
        {
            FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildEqualsTest63()
        {
            FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildEqualsTest64()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildEqualsTest65()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildEqualsTest66()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildEqualsTest67()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, StringGenVarDef2),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildEqualsTest68()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Name, x => x.Name));
        }

        [TestMethod]
        public void BuildEqualsTest69()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildEquals<IName>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest70()
        {
            FunctionBuilder.BuildEquals(NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest71()
        {
            FunctionBuilder.BuildEquals(NullVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest72()
        {
            FunctionBuilder.BuildEquals(GuidVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest73()
        {
            FunctionBuilder.BuildEquals(NullLambda, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildEqualsTest74()
        {
            FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, NullFunction);
        }

        [TestMethod]
        public void BuildEqualsTest80()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, PrimaryKeyVarDef, FuncSQL),
                FunctionBuilder.BuildEquals(FuncSQL));
        }

        [TestMethod]
        public void BuildEqualsTest81()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, GuidVarDef, FuncSQL),
                FunctionBuilder.BuildEquals(GuidVarDef, FuncSQL));
        }

        [TestMethod]
        public void BuildEqualsTest82()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcEQ, IntGenVarDef, FuncSQL),
                FunctionBuilder.BuildEquals<TestDataObject>(x => x.Height, FuncSQL));
        }

        #endregion
    }
}