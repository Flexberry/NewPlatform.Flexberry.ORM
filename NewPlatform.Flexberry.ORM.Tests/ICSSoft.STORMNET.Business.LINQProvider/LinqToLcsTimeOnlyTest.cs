namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты LINQ-провайдера для типа TimeOnly.
    /// </summary>
    public class LinqToLcsTimeOnlyTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

#if NET6_0_OR_GREATER
        [Fact]
        public void GetLcsTestTimeOnlyEqualsConstant()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var timeVal = new TimeOnly(14, 30, 0);
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyProp == timeVal);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.DateTimeType, "TimeOnlyProp"), timeVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestTimeOnlyLessThan()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var timeVal = new TimeOnly(14, 30, 0);
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyProp < timeVal);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcL, new VariableDef(ldef.DateTimeType, "TimeOnlyProp"), timeVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestTimeOnlyHour()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyProp.Hour == 14);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, ldef.GetFunction(ldef.funcHHPart, new VariableDef(ldef.DateTimeType, "TimeOnlyProp")), 14),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestTimeOnlyMinute()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyProp.Minute == 30);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, ldef.GetFunction(ldef.funcMIPart, new VariableDef(ldef.DateTimeType, "TimeOnlyProp")), 30),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestTimeOnlySecond()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyProp.Second == 45);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, ldef.GetFunction(ldef.funcSSPart, new VariableDef(ldef.DateTimeType, "TimeOnlyProp")), 45),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestTimeOnlyNullableEqualsConstant()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var timeVal = new TimeOnly(14, 30, 0);
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyNullableProp == timeVal);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.DateTimeType, "TimeOnlyNullableProp"), timeVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestTimeOnlyNullableHour()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.TimeOnlyNullableProp.Value.Hour == 14);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, ldef.GetFunction(ldef.funcHHPart, new VariableDef(ldef.DateTimeType, "TimeOnlyNullableProp")), 14),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }
#endif
    }
}
