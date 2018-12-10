namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FileType;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;

    using Xunit;

    public class GetObjectTypeTests : BaseTest
    {
        [Fact]
        public void GetObjectTypeTest00()
        {
            Assert.Equal(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(DataObject)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest01()
        {
            Assert.Equal(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(TestDataObject)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest02()
        {
            Assert.Equal(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(Guid)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest03()
        {
            Assert.Equal(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(Guid?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest04()
        {
            Assert.Equal(LangDef.GuidType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(KeyGuid)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest10()
        {
            Assert.Equal(LangDef.BoolType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(bool)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest11()
        {
            Assert.Equal(LangDef.BoolType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(bool?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest20()
        {
            Assert.Equal(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(string)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest21()
        {
            Assert.Equal(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(tDayOfWeek)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest23()
        {
            Assert.Equal(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(WebFile)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest24()
        {
            Assert.Equal(LangDef.StringType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(File)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest30()
        {
            Assert.Equal(LangDef.DateTimeType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(DateTime)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest31()
        {
            Assert.Equal(LangDef.DateTimeType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(DateTime?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest32()
        {
            Assert.Equal(LangDef.DateTimeType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(NullableDateTime)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest40()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(short)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest41()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(short?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest42()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(int)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest43()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(int?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest44()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(long)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest45()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(long?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest46()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ushort)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest47()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ushort?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest48()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(uint)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest49()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(uint?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest50()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ulong)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest51()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(ulong?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest52()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(decimal)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest53()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(decimal?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest54()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(float)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest55()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(float?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest56()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(double)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest57()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(double?)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest58()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(NullableInt)).NetCompatibilityType);
        }

        [Fact]
        public void GetObjectTypeTest59()
        {
            Assert.Equal(LangDef.NumericType.NetCompatibilityType, FunctionHelper.GetObjectType(typeof(NullableDecimal)).NetCompatibilityType);
        }
    }
}
