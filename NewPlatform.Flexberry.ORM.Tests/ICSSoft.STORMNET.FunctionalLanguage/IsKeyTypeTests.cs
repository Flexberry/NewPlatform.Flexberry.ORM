namespace IIS.University.Tools.Tests
{
    using System;

    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;

    using IIS.University.Tools;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IsKeyTypeTests : BaseTest
    {
        private static readonly Type DataObjectType = LangDef.DataObjectType.NetCompatibilityType;

        private static readonly Type GuidType = LangDef.GuidType.NetCompatibilityType;

        private static readonly Type BoolType = LangDef.BoolType.NetCompatibilityType;

        private static readonly Type NumericType = LangDef.NumericType.NetCompatibilityType;

        private static readonly Type StringType = LangDef.StringType.NetCompatibilityType;

        private static readonly Type DateTimeType = LangDef.DateTimeType.NetCompatibilityType;

        private static readonly Type IntType = typeof(int);

        private static readonly Type NullableIntType = typeof(NullableInt);

        private static readonly Type NullableDecimalType = typeof(NullableDecimal);

        private static readonly Type NullableDateTimeType = typeof(NullableDateTime);

        private static readonly Type NGuidType = typeof(Guid?);

        private static readonly Type KeyGuidType = typeof(KeyGuid);

        private static readonly Type EnumType = typeof(tDayOfWeek);

        [TestMethod]
        public void IsKeyTypeTest00()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(KeyGuidType),
                true);
        }

        [TestMethod]
        public void IsKeyTypeTest01()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(DataObjectType),
                true);
        }

        [TestMethod]
        public void IsKeyTypeTest02()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(GuidType),
                true);
        }

        [TestMethod]
        public void IsKeyTypeTest03()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(NGuidType),
                true);
        }

        [TestMethod]
        public void IsKeyTypeTest04()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(BoolType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest05()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(NumericType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest06()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(StringType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest07()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(DateTimeType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest08()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(IntType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest09()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(NullableIntType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest10()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(NullableDecimalType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest11()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(NullableDateTimeType),
                false);
        }

        [TestMethod]
        public void IsKeyTypeTest12()
        {
            Assert.AreEqual(
                FunctionHelper.IsKeyType(EnumType),
                false);
        }
    }
}