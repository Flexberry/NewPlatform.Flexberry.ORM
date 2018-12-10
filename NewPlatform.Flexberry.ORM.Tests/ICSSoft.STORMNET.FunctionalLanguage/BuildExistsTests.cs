namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    using Xunit;

    public class BuildExistsTests : BaseFunctionTest
    {
        private static readonly Function Func3 = FunctionBuilder.BuildEquals<TestDataObjectDetail>(x => x.Value, String1);

        private static readonly string ConnectMasterProp = Information.ExtractPropertyName<TestDataObjectDetail>(x => x.TestDataObject);

        private static readonly DetailVariableDef DetVarDef = FunctionHelper.GetDetailVarDef(TestDataObjectDetail.Views.D, ConnectMasterProp);

        [Fact]
        public void BuildExistsTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildExists(NullDetailVarDef));
        }

        [Fact]
        public void BuildExistsTest02()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncTrue),
                FunctionBuilder.BuildExists(DetVarDef));
        }

        [Fact]
        public void BuildExistsTest03()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncFalse),
                FunctionBuilder.BuildExists(DetVarDef, FuncFalse));
        }

        [Fact]
        public void BuildExistsTest04()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, Func3),
                FunctionBuilder.BuildExists(DetVarDef, Func3));
        }

        [Fact]
        public void BuildExistsTest05()
        {
            Assert.Equal(
                FunctionBuilder.BuildExists(DetVarDef, Func3),
                FunctionBuilder.BuildExists(
                    FunctionHelper.GetDetailVarDef(TestDataObjectDetail.Views.D, ConnectMasterProp, SQLWhereLanguageDef.StormMainObjectKey),
                    Func3));
        }

        [Fact]
        public void BuildExistsTest06()
        {
            Assert.Equal(
                FunctionBuilder.BuildExists(DetVarDef, Func3),
                FunctionBuilder.BuildExists(
                    FunctionHelper.GetDetailVarDef(TestDataObjectDetail.Views.D, NullString, SQLWhereLanguageDef.StormMainObjectKey),
                    Func3));
        }

        [Fact]
        public void BuildExistsTest10()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionBuilder.BuildExists(NullString, NullView, NullFunction));
        }

        [Fact]
        public void BuildExistsTest11()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncTrue),
                FunctionBuilder.BuildExists(ConnectMasterProp, TestDataObjectDetail.Views.D));
        }

        [Fact]
        public void BuildExistsTest12()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, FuncFalse),
                FunctionBuilder.BuildExists(ConnectMasterProp, TestDataObjectDetail.Views.D, FuncFalse));
        }

        [Fact]
        public void BuildExistsTest13()
        {
            Assert.Equal(
                LangDef.GetFunction(LangDef.funcExist, DetVarDef, Func3),
                FunctionBuilder.BuildExists(ConnectMasterProp, TestDataObjectDetail.Views.D, Func3));
        }
    }
}