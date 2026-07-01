namespace ICSSoft.STORMNET.Tests.DataObject
{
    using System;

    using ICSSoft.STORMNET;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты Information.SetPropValueByName и ParsePropertyValue для типа TimeOnly.
    /// </summary>
    public class InformationTimeOnlyTest
    {
#if NET6_0_OR_GREATER
        [Fact]
        public void SetPropValueByName_TimeSpan_To_TimeOnly()
        {
            var obj = new DateOnlyField();
            var ts = new TimeSpan(14, 30, 45);
            Information.SetPropValueByName(obj, "TimeOnlyProp", ts);
            Assert.Equal(new TimeOnly(14, 30, 45), obj.TimeOnlyProp);
        }

        [Fact]
        public void SetPropValueByName_TimeSpan_To_TimeOnlyNullable()
        {
            var obj = new DateOnlyField();
            var ts = new TimeSpan(14, 30, 45);
            Information.SetPropValueByName(obj, "TimeOnlyNullableProp", ts);
            Assert.Equal(new TimeOnly(14, 30, 45), obj.TimeOnlyNullableProp);
        }

        [Fact]
        public void SetPropValueByName_NullTimeSpan_To_TimeOnlyNullable()
        {
            var obj = new DateOnlyField();
            obj.TimeOnlyNullableProp = new TimeOnly(12, 0, 0);
            Information.SetPropValueByName(obj, "TimeOnlyNullableProp", (TimeSpan?)null);
            Assert.Null(obj.TimeOnlyNullableProp);
        }

        [Fact]
        public void SetPropValueByName_DateTime_To_TimeOnly()
        {
            var obj = new DateOnlyField();
            var dt = new DateTime(2026, 5, 20, 14, 30, 45);
            Information.SetPropValueByName(obj, "TimeOnlyProp", dt);
            Assert.Equal(new TimeOnly(14, 30, 45), obj.TimeOnlyProp);
        }

        [Fact]
        public void SetPropValueByName_DateTime_To_TimeOnlyNullable()
        {
            var obj = new DateOnlyField();
            var dt = new DateTime(2026, 5, 20, 14, 30, 45);
            Information.SetPropValueByName(obj, "TimeOnlyNullableProp", dt);
            Assert.Equal(new TimeOnly(14, 30, 45), obj.TimeOnlyNullableProp);
        }

        [Fact]
        public void SetPropValueByName_String_To_TimeOnly()
        {
            var obj = new DateOnlyField();
            Information.SetPropValueByName(obj, "TimeOnlyProp", "14:30:45");
            Assert.Equal(new TimeOnly(14, 30, 45), obj.TimeOnlyProp);
        }

        [Fact]
        public void ParsePropertyValue_TimeOnly()
        {
            var result = Information.ParsePropertyValue(typeof(DateOnlyField), "TimeOnlyProp", "14:30:45");
            Assert.Equal(new TimeOnly(14, 30, 45), result);
        }

        [Fact]
        public void SetPropValueByName_OutOfRangeTimeSpan_To_TimeOnly_Throws()
        {
            var obj = new DateOnlyField();
            Assert.ThrowsAny<Exception>(() =>
                Information.SetPropValueByName(obj, "TimeOnlyProp", TimeSpan.FromHours(25)));
        }

        [Fact]
        public void SetPropValueByName_NegativeTimeSpan_To_TimeOnly_Throws()
        {
            var obj = new DateOnlyField();
            Assert.ThrowsAny<Exception>(() =>
                Information.SetPropValueByName(obj, "TimeOnlyProp", TimeSpan.FromHours(-1)));
        }

        [Fact]
        public void SetPropValueByName_OutOfRangeTimeSpan_To_TimeOnlyNullable_Throws()
        {
            var obj = new DateOnlyField();
            Assert.ThrowsAny<Exception>(() =>
                Information.SetPropValueByName(obj, "TimeOnlyNullableProp", TimeSpan.FromDays(1).Add(TimeSpan.FromSeconds(1))));
        }

        [Fact]
        public void SetPropValueByName_OutOfRangeNullableTimeSpan_To_TimeOnlyNullable_Throws()
        {
            var obj = new DateOnlyField();
            Assert.ThrowsAny<Exception>(() =>
                Information.SetPropValueByName(obj, "TimeOnlyNullableProp", (TimeSpan?)TimeSpan.FromHours(25)));
        }

        [Fact]
        public void ParsePropertyValue_TimeOnly_InvalidString_Throws()
        {
            Assert.ThrowsAny<Exception>(() =>
                Information.ParsePropertyValue(typeof(DateOnlyField), "TimeOnlyProp", "not-a-time"));
        }
#endif
    }
}
