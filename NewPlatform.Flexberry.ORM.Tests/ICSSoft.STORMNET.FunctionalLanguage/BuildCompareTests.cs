namespace IIS.University.Tools.Tests
{
    using System;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildCompareTests : BaseFunctionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest1()
        {
            FunctionBuilder.BuildCompare(NullString, NumericVarDef, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest2()
        {
            FunctionBuilder.BuildCompare(LangDef.funcIN, NumericVarDef, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest3()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NullVarDef, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest4()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildCompareTest5()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef, "asd");
        }

        [TestMethod]
        public void BuildCompareTest6()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, NumericVarDef, Int1),
                FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef, Int1));
        }

        [TestMethod]
        public void BuildCompareTest7()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateVarDef, DateTime1),
                FunctionBuilder.BuildCompare(LangDef.funcL, DateVarDef, DateTime1));
        }

        [TestMethod]
        public void BuildCompareTest8()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringVarDef, String1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringVarDef, String1));
        }

        [TestMethod]
        public void BuildCompareTest9()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest21()
        {
            FunctionBuilder.BuildCompare(NullString, NumericVarDef.Caption, NumericVarDef.Type, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest22()
        {
            FunctionBuilder.BuildCompare(LangDef.funcIN, NumericVarDef.Caption, NumericVarDef.Type, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest23()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NullString, NumericVarDef.Type, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest24()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NullObjectType, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest25()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NumericVarDef.Type, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildCompareTest26()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NumericVarDef.Type, "asd");
        }

        [TestMethod]
        public void BuildCompareTest27()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, NumericVarDef, Int1),
                FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NumericVarDef.Type, Int1));
        }

        [TestMethod]
        public void BuildCompareTest28()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateVarDef, DateTime1),
                FunctionBuilder.BuildCompare(LangDef.funcL, DateVarDef.Caption, DateVarDef.Type, DateTime1));
        }

        [TestMethod]
        public void BuildCompareTest29()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringVarDef, String1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringVarDef.Caption, StringVarDef.Type, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest31()
        {
            FunctionBuilder.BuildCompare(NullString, NumericVarDef.Caption, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest32()
        {
            FunctionBuilder.BuildCompare(LangDef.funcIN, NumericVarDef.Caption, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest33()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NullString, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest34()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, NullObject);
        }

        [TestMethod]
        public void BuildCompareTest35()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, NumericVarDef, Int1),
                FunctionBuilder.BuildCompare(LangDef.funcL, NumericVarDef.Caption, Int1));
        }

        [TestMethod]
        public void BuildCompareTest36()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateVarDef, DateTime1),
                FunctionBuilder.BuildCompare(LangDef.funcL, DateVarDef.Caption, DateTime1));
        }

        [TestMethod]
        public void BuildCompareTest37()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringVarDef, String1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringVarDef.Caption, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest41()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(NullString, x => x.Height, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest42()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcIN, x => x.Height, Int1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest43()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, NullObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BuildCompareTest44()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, "asd");
        }

        [TestMethod]
        public void BuildCompareTest45()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, Int1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, Int1));
        }

        [TestMethod]
        public void BuildCompareTest46()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateTime1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.BirthDate, DateTime1));
        }

        [TestMethod]
        public void BuildCompareTest47()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, String1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, String1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest51()
        {
            FunctionBuilder.BuildCompare(NullString, StringGenVarDef, StringGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest52()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, NullVarDef, StringGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest53()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, StringGenVarDef, NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest54()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, IntGenVarDef, DateGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest55()
        {
            FunctionBuilder.BuildCompare(LangDef.funcL, StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest56()
        {
            FunctionBuilder.BuildCompare(LangDef.funcIN, StringGenVarDef, IntGenVarDef);
        }

        [TestMethod]
        public void BuildCompareTest57()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildCompare(LangDef.funcL, StringGenVarDef, StringGenVarDef1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest61()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(NullString, x => x.Hierarchy, x => x.Hierarchy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest62()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, NullLambda, x => x.Hierarchy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildCompareTest63()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Hierarchy, NullLambda);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest64()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, x => x.BirthDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildCompareTest65()
        {
            FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.Height);
        }

        [TestMethod]
        public void BuildCompareTest71()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.NickName));
        }

        [TestMethod]
        public void BuildCompareTest72()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, IntGenVarDef, IntGenVarDef1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Height, x => x.Weight));
        }

        [TestMethod]
        public void BuildCompareTest73()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, DateGenVarDef, DateGenVarDef1),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.BirthDate, x => x.DeathDate));
        }

        [TestMethod]
        public void BuildCompareTest74()
        {

            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef2), 
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.Hierarchy.Name));
        }

        [TestMethod]
        public void BuildCompareTest75()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcLEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcLEQ, x => x.Name, x => x.Name));
        }

        [TestMethod]
        public void BuildCompareTest76()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcGEQ, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcGEQ, x => x.Name, x => x.Name));
        }

        [TestMethod]
        public void BuildCompareTest77()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcL, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcL, x => x.Name, x => x.Name));
        }

        [TestMethod]
        public void BuildCompareTest78()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcG, StringGenVarDef, StringGenVarDef),
                FunctionBuilder.BuildCompare<TestDataObject>(LangDef.funcG, x => x.Name, x => x.Name));
        }
    }
}