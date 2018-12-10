namespace IIS.University.Tools.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValidateTests : BaseFunctionTest
    {
        private static readonly ObjectType ObjectType = LangDef.NumericType;

        private static readonly VariableDef VarDef0 = new VariableDef();

        private static readonly VariableDef VarDef1 = new VariableDef(ObjectType, PropertyName);

        private static readonly VariableDef VarDef2 = new VariableDef(VarDef1.Type, VarDef1.StringedView, VarDef1.Caption);

        #region ValidateVariableDef

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateVariableDefTest01()
        {
            FunctionHelper.ValidateVariableDef(NullVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateVariableDefTest02()
        {
            FunctionHelper.ValidateVariableDef(VarDef0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateVariableDefTest03()
        {
            FunctionHelper.ValidateVariableDef(new VariableDef { StringedView = "PropertyName" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateVariableDefTest04()
        {
            FunctionHelper.ValidateVariableDef(new VariableDef { Type = ObjectType });
        }

        [TestMethod]
        public void ValidateVariableDefTest05()
        {
            FunctionHelper.ValidateVariableDef(new VariableDef { StringedView = "PropertyName", Type = ObjectType });
        }

        [TestMethod]
        public void ValidateVariableDefTest06()
        {
            FunctionHelper.ValidateVariableDef(VarDef1);
        }

        [TestMethod]
        public void ValidateVariableDefTest07()
        {
            FunctionHelper.ValidateVariableDef(VarDef2);
        }

        #endregion

        #region ValidateDetailVariableDef

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateDetailVariableDefTest01()
        {
            FunctionHelper.ValidateDetailVariableDef(NullDetailVarDef);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDetailVariableDefTest02()
        {
            FunctionHelper.ValidateDetailVariableDef(new DetailVariableDef());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDetailVariableDefTest03()
        {
            FunctionHelper.ValidateDetailVariableDef(new DetailVariableDef { Type = LangDef.DetailsType });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDetailVariableDefTest04()
        {
            FunctionHelper.ValidateDetailVariableDef(new DetailVariableDef { Type = LangDef.DetailsType, View = TestDataObject.Views.E });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateDetailVariableDefTest05()
        {
            FunctionHelper.ValidateDetailVariableDef(
                new DetailVariableDef
                {
                    Type = LangDef.DetailsType,
                    View = TestDataObject.Views.E,
                    ConnectMasterPorp = Information.ExtractPropertyName<TestDataObject>(x => x.Hierarchy)
                });
        }

        [TestMethod]
        public void ValidateDetailVariableDefTest06()
        {
            FunctionHelper.ValidateDetailVariableDef(
                new DetailVariableDef
                {
                    Type = LangDef.DetailsType,
                    View = TestDataObject.Views.E,
                    ConnectMasterPorp = Information.ExtractPropertyName<TestDataObject>(x => x.Hierarchy),
                    OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey }
                });
        }

        #endregion

        #region ValidateExpression

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateExpressionTest01()
        {
            FunctionHelper.ValidateExpression(NullLambda);
        }

        [TestMethod]
        public void ValidateExpressionTest02()
        {
            FunctionHelper.ValidateExpression<TestDataObject>(x => x.Height);
        }

        #endregion

        #region ValidatePropertyName

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatePropertyNameTest01()
        {
            FunctionHelper.ValidatePropertyName(NullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatePropertyNameTest02()
        {
            FunctionHelper.ValidatePropertyName("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatePropertyNameTest03()
        {
            FunctionHelper.ValidatePropertyName("");
        }

        [TestMethod]
        public void ValidatePropertyNameTest04()
        {
            FunctionHelper.ValidatePropertyName(PropertyName);
        }

        #endregion

        #region ValidateFunctionString

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateFunctionStringTest01()
        {
            FunctionHelper.ValidateFunctionString(NullString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateFunctionStringTest02()
        {
            FunctionHelper.ValidateFunctionString("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateFunctionStringTest03()
        {
            FunctionHelper.ValidateFunctionString("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateFunctionStringTest04()
        {
            FunctionHelper.ValidateFunctionString(LangDef.funcL);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidateFunctionStringTest05()
        {
            FunctionHelper.ValidateFunctionString(LangDef.funcL, LangDef.funcLEQ);
        }

        [TestMethod]
        public void ValidateFunctionStringTest06()
        {
            FunctionHelper.ValidateFunctionString(LangDef.funcL, LangDef.funcL);
        }

        #endregion

        #region ValidateValue

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateValueTest01()
        {
            FunctionHelper.ValidateValue(NullObject);
        }

        [TestMethod]
        public void ValidateValueTest02()
        {
            FunctionHelper.ValidateValue(1);
        }

        #endregion

        #region ValidateObjectType

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateObjectTypeTest01()
        {
            FunctionHelper.ValidateObjType(NullObjectType);
        }

        [TestMethod]
        public void ValidateObjectTypeTest02()
        {
            FunctionHelper.ValidateObjType(ObjectType);
        }

        #endregion
    }
}