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
    /// Тесты LINQ-провайдера для типа DateOnly.
    /// </summary>
    public class LinqToLcsDateOnlyTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

#if NET6_0_OR_GREATER
        /// <summary>
        /// DateOnly равно константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyEqualsConstant()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var dateOnlyVal = new DateOnly(2026, 5, 20);
            var predicate1 = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp == dateOnlyVal);
            new Query<DateOnlyField>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        new VariableDef(ldef.DateTimeType, "DateOnlyProp"),
                        dateOnlyVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly меньше константы.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyLessThan()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var dateOnlyVal = new DateOnly(2026, 5, 20);
            var predicate1 = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp < dateOnlyVal);
            new Query<DateOnlyField>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcL,
                        new VariableDef(ldef.DateTimeType, "DateOnlyProp"),
                        dateOnlyVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly больше или равно константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyGreaterThanOrEqual()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var dateOnlyVal = new DateOnly(2026, 5, 20);
            var predicate1 = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp >= dateOnlyVal);
            new Query<DateOnlyField>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcGEQ,
                        new VariableDef(ldef.DateTimeType, "DateOnlyProp"),
                        dateOnlyVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly меньше или равно переменной.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyVariableCompare()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            DateOnly moment = new DateOnly(2026, 5, 20);
            var predicate1 = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp <= moment);
            new Query<DateOnlyField>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        new VariableDef(ldef.DateTimeType, "DateOnlyProp"),
                        moment),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly.DayNumber больше константы.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyDayNumber()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp.DayNumber > 100000);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcG,
                        ldef.GetFunction(ldef.funcDayNumber, new VariableDef(ldef.DateTimeType, "DateOnlyProp")),
                        100000),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly.DayOfYear равен константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyDayOfYear()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp.DayOfYear == 150);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcDayOfYear, new VariableDef(ldef.DateTimeType, "DateOnlyProp")),
                        150),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly.DayOfWeek равен понедельнику.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyDayOfWeek()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp.DayOfWeek == DayOfWeek.Monday);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcDayOfWeekZeroBased, new VariableDef(ldef.DateTimeType, "DateOnlyProp")),
                        1),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly.Year равен константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyYearPart()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp.Year == 2026);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcYearPart, new VariableDef(ldef.DateTimeType, "DateOnlyProp")),
                        2026),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly.Month равен константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyMonthPart()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp.Month == 5);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcMonthPart, new VariableDef(ldef.DateTimeType, "DateOnlyProp")),
                        5),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// DateOnly.Day равен константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyDayPart()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyProp.Day == 20);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcDayPart, new VariableDef(ldef.DateTimeType, "DateOnlyProp")),
                        20),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Nullable DateOnly? равно константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyNullableEqualsConstant()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var dateOnlyVal = new DateOnly(2026, 5, 20);
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyNullableProp == dateOnlyVal);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        new VariableDef(ldef.DateTimeType, "DateOnlyNullableProp"),
                        dateOnlyVal),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Nullable DateOnly?.Year равен константе.
        /// </summary>
        [Fact]
        public void GetLcsTestDateOnlyNullableYearPart()
        {
            var testProvider = new TestLcsQueryProvider<DateOnlyField>();
            var predicate = (Expression<Func<DateOnlyField, bool>>)(o => o.DateOnlyNullableProp.Value.Year == 2026);
            new Query<DateOnlyField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcYearPart, new VariableDef(ldef.DateTimeType, "DateOnlyNullableProp")),
                        2026),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, DateOnlyField.Views.DateOnlyFieldE);
            Assert.True(Equals(expected, actual));
        }
#endif
    }
}
