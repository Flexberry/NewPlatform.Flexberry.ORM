namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;

    using Xunit;

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

        [Fact]
        public void IsKeyTypeTest00()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(KeyGuidType),
                true);
        }

        [Fact]
        public void IsKeyTypeTest01()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(DataObjectType),
                true);
        }

        [Fact]
        public void IsKeyTypeTest02()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(GuidType),
                true);
        }

        [Fact]
        public void IsKeyTypeTest03()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(NGuidType),
                true);
        }

        [Fact]
        public void IsKeyTypeTest04()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(BoolType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest05()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(NumericType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest06()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(StringType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest07()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(DateTimeType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest08()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(IntType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest09()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(NullableIntType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest10()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(NullableDecimalType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest11()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(NullableDateTimeType),
                false);
        }

        [Fact]
        public void IsKeyTypeTest12()
        {
            Assert.Equal(
                FunctionHelper.IsKeyType(EnumType),
                false);
        }
    }
}