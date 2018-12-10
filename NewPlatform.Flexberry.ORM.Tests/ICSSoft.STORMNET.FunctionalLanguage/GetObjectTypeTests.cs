namespace IIS.University.Tools.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FileType;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetObjectTypeTests : BaseTest
    {
        [TestMethod]
        public void GetObjectTypeTest00()
        {
            Assert.AreEqual(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(DataObject)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest01()
        {
            Assert.AreEqual(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(TestDataObject)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest02()
        {
            Assert.AreEqual(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(Guid)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest03()
        {
            Assert.AreEqual(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(Guid?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest04()
        {
            Assert.AreEqual(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(KeyGuid)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest10()
        {
            Assert.AreEqual(LangDef.BoolType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(bool)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest11()
        {
            Assert.AreEqual(LangDef.BoolType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(bool?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest20()
        {
            Assert.AreEqual(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(string)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest21()
        {
            Assert.AreEqual(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(tDayOfWeek)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest23()
        {
            Assert.AreEqual(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(WebFile)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest24()
        {
            Assert.AreEqual(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(File)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest30()
        {
            Assert.AreEqual(LangDef.DateTimeType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(DateTime)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest31()
        {
            Assert.AreEqual(LangDef.DateTimeType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(DateTime?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest32()
        {
            Assert.AreEqual(LangDef.DateTimeType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(NullableDateTime)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest40()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(short)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest41()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(short?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest42()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(int)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest43()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(int?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest44()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(long)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest45()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(long?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest46()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ushort)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest47()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ushort?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest48()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(uint)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest49()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(uint?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest50()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ulong)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest51()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ulong?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest52()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(decimal)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest53()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(decimal?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest54()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(float)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest55()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(float?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest56()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(double)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest57()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(double?)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest58()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(NullableInt)).NetCompatibilityType);
        }

        [TestMethod]
        public void GetObjectTypeTest59()
        {
            Assert.AreEqual(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(NullableDecimal)).NetCompatibilityType);
        }
    }
}
