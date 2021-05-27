namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    using Xunit;

    public class ValidateTests : BaseFunctionTest
    {
        private static readonly ObjectType ObjectType = LangDef.NumericType;

        private static readonly VariableDef VarDef0 = new VariableDef();

        private static readonly VariableDef VarDef1 = new VariableDef(ObjectType, PropertyName);

        private static readonly VariableDef VarDef2 = new VariableDef(VarDef1.Type, VarDef1.StringedView, VarDef1.Caption);

        #region ValidateVariableDef

        [Fact]
        public void ValidateVariableDefTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateVariableDef(NullVarDef));
        }

        [Fact]
        public void ValidateVariableDefTest02()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateVariableDef(VarDef0));
        }

        [Fact]
        public void ValidateVariableDefTest03()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateVariableDef(new VariableDef { StringedView = "PropertyName" }));
        }

        [Fact]
        public void ValidateVariableDefTest04()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateVariableDef(new VariableDef { Type = ObjectType }));
        }

        [Fact]
        public void ValidateVariableDefTest05()
        {
            FunctionHelper.ValidateVariableDef(new VariableDef { StringedView = "PropertyName", Type = ObjectType });
        }

        [Fact]
        public void ValidateVariableDefTest06()
        {
            FunctionHelper.ValidateVariableDef(VarDef1);
        }

        [Fact]
        public void ValidateVariableDefTest07()
        {
            FunctionHelper.ValidateVariableDef(VarDef2);
        }

        #endregion

        #region ValidateDetailVariableDef

        [Fact]
        public void ValidateDetailVariableDefTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateDetailVariableDef(NullDetailVarDef));
        }

        [Fact]
        public void ValidateDetailVariableDefTest02()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateDetailVariableDef(new DetailVariableDef()));
        }

        [Fact]
        public void ValidateDetailVariableDefTest03()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateDetailVariableDef(new DetailVariableDef { Type = LangDef.DetailsType }));
        }

        [Fact]
        public void ValidateDetailVariableDefTest04()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateDetailVariableDef(new DetailVariableDef { Type = LangDef.DetailsType, View = TestDataObject.Views.E }));
        }

        [Fact]
        public void ValidateDetailVariableDefTest05()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateDetailVariableDef(
                new DetailVariableDef
                {
                    Type = LangDef.DetailsType,
                    View = TestDataObject.Views.E,
                    ConnectMasterPorp = Information.ExtractPropertyName<TestDataObject>(x => x.Hierarchy),
                }));
        }

        [Fact]
        public void ValidateDetailVariableDefTest06()
        {
            FunctionHelper.ValidateDetailVariableDef(
                new DetailVariableDef
                {
                    Type = LangDef.DetailsType,
                    View = TestDataObject.Views.E,
                    ConnectMasterPorp = Information.ExtractPropertyName<TestDataObject>(x => x.Hierarchy),
                    OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                });
        }

        #endregion

        #region ValidateExpression

        [Fact]
        public void ValidateExpressionTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateExpression(NullLambda));
        }

        [Fact]
        public void ValidateExpressionTest02()
        {
            FunctionHelper.ValidateExpression<TestDataObject>(x => x.Height);
        }

        #endregion

        #region ValidatePropertyName

        [Fact]
        public void ValidatePropertyNameTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidatePropertyName(NullString));
        }

        [Fact]
        public void ValidatePropertyNameTest02()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidatePropertyName("   "));
        }

        [Fact]
        public void ValidatePropertyNameTest03()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidatePropertyName(""));
        }

        [Fact]
        public void ValidatePropertyNameTest04()
        {
            FunctionHelper.ValidatePropertyName(PropertyName);
        }

        #endregion

        #region ValidateFunctionString

        [Fact]
        public void ValidateFunctionStringTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateFunctionString(NullString));
        }

        [Fact]
        public void ValidateFunctionStringTest02()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateFunctionString("   "));
        }

        [Fact]
        public void ValidateFunctionStringTest03()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateFunctionString(""));
        }

        [Fact]
        public void ValidateFunctionStringTest04()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateFunctionString(LangDef.funcL));
        }

        [Fact]
        public void ValidateFunctionStringTest05()
        {
            Assert.Throws<ArgumentException>(() => FunctionHelper.ValidateFunctionString(LangDef.funcL, LangDef.funcLEQ));
        }

        [Fact]
        public void ValidateFunctionStringTest06()
        {
            FunctionHelper.ValidateFunctionString(LangDef.funcL, LangDef.funcL);
        }

        #endregion

        #region ValidateValue

        [Fact]
        public void ValidateValueTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateValue(NullObject));
        }

        [Fact]
        public void ValidateValueTest02()
        {
            FunctionHelper.ValidateValue(1);
        }

        #endregion

        #region ValidateObjectType

        [Fact]
        public void ValidateObjectTypeTest01()
        {
            Assert.Throws<ArgumentNullException>(() => FunctionHelper.ValidateObjType(NullObjectType));
        }

        [Fact]
        public void ValidateObjectTypeTest02()
        {
            FunctionHelper.ValidateObjType(ObjectType);
        }

        #endregion
    }
}