namespace IIS.University.Tools.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildExistsTests : BaseFunctionTest
    {
        private static readonly Function Func3 = FunctionBuilder.BuildEquals<TestDataObjectDetail>(x => x.Value, String1);

        private static readonly string ConnectMasterProp = Information.ExtractPropertyName<TestDataObjectDetail>(x => x.TestDataObject);

        private static readonly DetailVariableDef DetVarDef = FunctionHelper.GetDetailVarDef(TestDataObjectDetail.Views.D, ConnectMasterProp);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildExistsTest01()
        {
            FunctionBuilder.BuildExists(NullDetailVarDef);
        }

        [TestMethod]
        public void BuildExistsTest02()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncTrue),
                FunctionBuilder.BuildExists(DetVarDef));
        }

        [TestMethod]
        public void BuildExistsTest03()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncFalse),
                FunctionBuilder.BuildExists(DetVarDef, FuncFalse));
        }

        [TestMethod]
        public void BuildExistsTest04()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, Func3),
                FunctionBuilder.BuildExists(DetVarDef, Func3));
        }

        [TestMethod]
        public void BuildExistsTest05()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildExists(DetVarDef, Func3),
                FunctionBuilder.BuildExists(
                    FunctionHelper.GetDetailVarDef(TestDataObjectDetail.Views.D, ConnectMasterProp, SQLWhereLanguageDef.StormMainObjectKey),
                    Func3));
        }

        [TestMethod]
        public void BuildExistsTest06()
        {
            Assert.AreEqual(
                FunctionBuilder.BuildExists(DetVarDef, Func3),
                FunctionBuilder.BuildExists(
                    FunctionHelper.GetDetailVarDef(TestDataObjectDetail.Views.D, NullString, SQLWhereLanguageDef.StormMainObjectKey),
                    Func3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildExistsTest10()
        {
            FunctionBuilder.BuildExists(NullString, NullView, NullFunction);
        }

        [TestMethod]
        public void BuildExistsTest11()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncTrue),
                FunctionBuilder.BuildExists(ConnectMasterProp, TestDataObjectDetail.Views.D));
        }

        [TestMethod]
        public void BuildExistsTest12()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncFalse),
                FunctionBuilder.BuildExists(ConnectMasterProp, TestDataObjectDetail.Views.D, FuncFalse));
        }

        [TestMethod]
        public void BuildExistsTest13()
        {
            Assert.AreEqual(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, Func3),
                FunctionBuilder.BuildExists(ConnectMasterProp, TestDataObjectDetail.Views.D, Func3));
        }
    }
}