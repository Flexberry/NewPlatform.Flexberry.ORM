namespace ICSSoft.STORMNET.Tests.DataObject
{
    using System;

    using ICSSoft.STORMNET;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты Information.SetPropValueByName и ParsePropertyValue для типа DateOnly.
    /// </summary>
    public class InformationDateOnlyTest
    {
#if NET6_0_OR_GREATER
        [Fact]
        public void SetPropValueByName_DateTime_To_DateOnly()
        {
            var obj = new DateOnlyField();
            var dt = new DateTime(2026, 5, 20, 14, 30, 0);
            Information.SetPropValueByName(obj, "DateOnlyProp", dt);
            Assert.Equal(new DateOnly(2026, 5, 20), obj.DateOnlyProp);
        }

        [Fact]
        public void SetPropValueByName_DateTime_To_DateOnlyNullable()
        {
            var obj = new DateOnlyField();
            var dt = new DateTime(2026, 5, 20, 14, 30, 0);
            Information.SetPropValueByName(obj, "DateOnlyNullableProp", dt);
            Assert.Equal(new DateOnly(2026, 5, 20), obj.DateOnlyNullableProp);
        }

        [Fact]
        public void SetPropValueByName_NullDateTime_To_DateOnlyNullable()
        {
            var obj = new DateOnlyField();
            obj.DateOnlyNullableProp = new DateOnly(2026, 1, 1);
            Information.SetPropValueByName(obj, "DateOnlyNullableProp", (DateTime?)null);
            Assert.Null(obj.DateOnlyNullableProp);
        }

        [Fact]
        public void SetPropValueByName_DateTimeNullableWithValue_To_DateOnlyNullable()
        {
            var obj = new DateOnlyField();
            DateTime? dt = new DateTime(2026, 7, 15, 10, 0, 0);
            Information.SetPropValueByName(obj, "DateOnlyNullableProp", dt);
            Assert.Equal(new DateOnly(2026, 7, 15), obj.DateOnlyNullableProp);
        }

        [Fact]
        public void SetPropValueByName_String_To_DateOnly()
        {
            var obj = new DateOnlyField();
            Information.SetPropValueByName(obj, "DateOnlyProp", "2026-05-20");
            Assert.Equal(new DateOnly(2026, 5, 20), obj.DateOnlyProp);
        }

        [Fact]
        public void ParsePropertyValue_DateOnly()
        {
            var result = Information.ParsePropertyValue(typeof(DateOnlyField), "DateOnlyProp", "2026-05-20");
            Assert.Equal(new DateOnly(2026, 5, 20), result);
        }

        [Fact]
        public void ParsePropertyValue_DateOnly_InvalidString_Throws()
        {
            Assert.ThrowsAny<Exception>(() =>
                Information.ParsePropertyValue(typeof(DateOnlyField), "DateOnlyProp", "not-a-date"));
        }
#endif
    }
}
