namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildNotEqualsTests : BaseFunctionTest
    {
        private const string GuidString = "EFB82498-1935-41F0-B8CB-129372B4430E";

        #region Equals

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest11()
        {
            FunctionBuilder.BuildNotEquals(NullObject);
        }

        [TestMethod]
        public void BuildNotEqualsTest12()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(GuidString));
        }

        [TestMethod]
        public void BuildNotEqualsTest13()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(Guid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest14()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(KeyGuid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest15()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    PrimaryKeyVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(TestDataObject1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildNotEqualsTest16()
        {
            FunctionBuilder.BuildNotEquals(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest21()
        {
            FunctionBuilder.BuildNotEquals(NullString, NullObject);
        }

        [TestMethod]
        public void BuildNotEqualsTest22()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidVarDef),
                FunctionBuilder.BuildNotEquals(PropertyName, NullObject));
        }

        [TestMethod]
        public void BuildNotEqualsTest23()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(PropertyName, GuidString));
        }

        [TestMethod]
        public void BuildNotEqualsTest24()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, Guid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest25()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, KeyGuid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest26()
        {
            Assert.AreEqual(
                LangDef.GetFunction(
                    LangDef.funcNEQ,
                    GuidVarDef,
                    PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(PropertyName, TestDataObject1));
        }

        [TestMethod]
        public void BuildNotEqualsTest27()
        {
            // TODO: Enum
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, StringVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, 1));
        }

        [TestMethod]
        public void BuildNotEqualsTest28()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest31()
        {
            FunctionBuilder.BuildNotEquals(NullString, NullObjectType, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest32()
        {
            FunctionBuilder.BuildNotEquals(PropertyName, NullObjectType, NullObject);
        }

        [TestMethod]
        public void BuildNotEqualsTest33()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidVarDef),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, NullObject));
        }

        [TestMethod]
        public void BuildNotEqualsTest34()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, GuidString));
        }

        [TestMethod]
        public void BuildNotEqualsTest35()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, Guid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest36()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, KeyGuid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest37()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.GuidType, TestDataObject1));
        }

        [TestMethod]
        public void BuildNotEqualsTest38()
        {
            // TODO: Enum
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.NumericType, 1));
        }

        [TestMethod]
        public void BuildNotEqualsTest39()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(PropertyName, LangDef.NumericType, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest41()
        {
            FunctionBuilder.BuildNotEquals(NullVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest42()
        {
            FunctionBuilder.BuildNotEquals(NullVarDef, NullObject);
        }

        [TestMethod]
        public void BuildNotEqualsTest43()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidVarDef),
                FunctionBuilder.BuildNotEquals(GuidVarDef, NullObject));
        }

        [TestMethod]
        public void BuildNotEqualsTest44()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(GuidString)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, GuidString));
        }

        [TestMethod]
        public void BuildNotEqualsTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(Guid1)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, Guid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest46()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, KeyGuid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest47()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, PKHelper.GetKeyByObject(TestDataObject1)),
                FunctionBuilder.BuildNotEquals(GuidVarDef, TestDataObject1));
        }

        [TestMethod]
        public void BuildNotEqualsTest48()
        {
            FunctionBuilder.BuildNotEquals(StringVarDef, "");
        }

        [TestMethod]
        public void BuildNotEqualsTest49()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, NumericVarDef, 1),
                FunctionBuilder.BuildNotEquals(NumericVarDef, 1));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildNotEqualsTest51()
        {
            FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, "asd");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildNotEqualsTest52()
        {
            FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildNotEqualsTest53()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNotIsNull, GuidGenVarDef),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, NullTestDataObject));
        }

        [TestMethod]
        public void BuildNotEqualsTest54()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidGenVarDef, PKHelper.GetKeyByObject(KeyGuid1)),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, KeyGuid1));
        }

        [TestMethod]
        public void BuildNotEqualsTest55()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, String1));
        }

        [TestMethod]
        public void BuildNotEqualsTest56()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildNotEqualsTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, IntGenVarDef, Int1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, Int1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest61()
        {
            FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildNotEqualsTest62()
        {
            FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildNotEqualsTest63()
        {
            FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildNotEqualsTest64()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildNotEqualsTest65()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildNotEqualsTest66()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildNotEqualsTest67()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, StringGenVarDef2), 
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildNotEqualsTest68()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Name, x => x.Name));
        }

        [TestMethod]
        public void BuildNotEqualsTest69()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, StringGenVarDef, String1),
                FunctionBuilder.BuildNotEquals<IName>(x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest70()
        {
            FunctionBuilder.BuildNotEquals(NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest71()
        {
            FunctionBuilder.BuildNotEquals(NullVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest72()
        {
            FunctionBuilder.BuildNotEquals(GuidVarDef, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest73()
        {
            FunctionBuilder.BuildNotEquals(NullLambda, NullFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildNotEqualsTest74()
        {
            FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, NullFunction);
        }

        [TestMethod]
        public void BuildNotEqualsTest80()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, PrimaryKeyVarDef, FuncSQL),
                FunctionBuilder.BuildNotEquals(FuncSQL));
        }

        [TestMethod]
        public void BuildNotEqualsTest81()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, GuidVarDef, FuncSQL),
                FunctionBuilder.BuildNotEquals(GuidVarDef, FuncSQL));
        }

        [TestMethod]
        public void BuildNotEqualsTest82()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcNEQ, IntGenVarDef, FuncSQL),
                FunctionBuilder.BuildNotEquals<TestDataObject>(x => x.Height, FuncSQL));
        }

        #endregion
    }
}