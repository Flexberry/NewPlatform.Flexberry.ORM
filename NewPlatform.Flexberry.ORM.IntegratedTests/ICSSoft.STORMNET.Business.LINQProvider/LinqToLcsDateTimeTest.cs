namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.Business.LINQProvider.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тесты поддержки типа DateTime.
    /// </summary>
    public class LinqToLcsTodayTest
    {
        /// <summary>
        /// LanguageDef.
        /// </summary>
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Только сегодняшняя дата.
        /// </summary>
        [Fact]
        public void GetLcsTestDateTimeNow()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата <= DateTime.Now);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(
                            ldef.funcLEQ,
                            new VariableDef(ldef.DateTimeType, "Дата"),
                            ldef.GetFunction("TODAY")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест функции только дата.
        /// </summary>
        [Fact]
        public void GetLcsTestDateTimeOnlyDate()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата.Date <= DateTime.Now.Date);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcOnlyDate, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcOnlyDate, ldef.GetFunction("TODAY"))),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест константы "воскресение".
        /// </summary>
        [Fact]
        public void GetLcsTestSunday()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата.DayOfWeek == DayOfWeek.Sunday);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcDayOfWeekZeroBased, new VariableDef(ldef.DateTimeType, "Дата")),
                        0),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест проверки на воскресение  в другу сторону.
        /// </summary>
        [Fact]
        public void GetLcsTestSundayReverse()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => DayOfWeek.Sunday == o.Дата.DayOfWeek);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        0,
                        ldef.GetFunction(ldef.funcDayOfWeekZeroBased, new VariableDef(ldef.DateTimeType, "Дата"))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// день недели.
        /// </summary>
        [Fact]
        public void GetLcsTestDayOfWeek()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата.DayOfWeek == DateTime.Now.DayOfWeek);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcDayOfWeekZeroBased, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcDayOfWeekZeroBased, ldef.GetFunction(ldef.paramTODAY))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// AddDays - исключение.
        /// </summary>
        [Fact]
        public void GetLcsTestDaysAdd()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Перелом>();
                var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата.AddDays(1) < DateTime.Now);
                new Query<Перелом>(testProvider).Where(predicate1).ToArray();

                Expression queryExpression = testProvider.InnerExpression;
                var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        ldef.GetFunction(
                            ldef.funcEQ,
                            ldef.GetFunction(ldef.funcDayOfWeekZeroBased, new VariableDef(ldef.DateTimeType, "Дата")),
                            ldef.GetFunction(ldef.funcDayOfWeekZeroBased, ldef.GetFunction("TODAY"))),
                };
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Перелом)));
                Assert.True(Equals(expected, actual));
            });
            Assert.IsType(typeof(NotImplementedException), exception);
        }

        /// <summary>
        /// сегодня.
        /// </summary>
        [Fact]
        public void GetLcsTestDateToday()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата.Date == DateTime.Today);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        ldef.GetFunction(ldef.funcOnlyDate, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcOnlyDate, ldef.GetFunction("TODAY"))),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// только время.
        /// </summary>
        [Fact]
        public void GetLcsTestOnlyTime()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата.TimeOfDay <= DateTime.Now.TimeOfDay);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcOnlyTime, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcOnlyTime, ldef.GetFunction("TODAY"))),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// сравнение с перееменной.
        /// </summary>
        [Fact]
        public void GetLcsTestVarDateTime()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            DateTime moment = DateTime.Now;
            var predicate1 = (Expression<Func<Перелом, bool>>)(o => o.Дата <= moment);
            new Query<Перелом>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        new VariableDef(ldef.DateTimeType, "Дата"),
                        moment),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// сравнение с константой с помощью Parse.
        /// </summary>
        [Fact]
        public void GetLcsTestParseDateTime()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата <= DateTime.Parse("18:00:00"));
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        new VariableDef(ldef.DateTimeType, "Дата"),
                        DateTime.Parse("18:00:00")),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// номер месяца.
        /// </summary>
        [Fact]
        public void GetLcsTestMonthPart()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Month <= DateTime.Now.Month);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcMonthPart, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcMonthPart, ldef.GetFunction(ldef.paramTODAY))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// номер дня в месяце.
        /// </summary>
        [Fact]
        public void GetLcsTestDayPart()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Day <= DateTime.Now.Day);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcDayPart, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcDayPart, ldef.GetFunction(ldef.paramTODAY))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// номер года.
        /// </summary>
        [Fact]
        public void GetLcsTestYearPart()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Year <= DateTime.Now.Year);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcYearPart, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcYearPart, ldef.GetFunction(ldef.paramTODAY))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// часы.
        /// </summary>
        [Fact]
        public void GetLcsTestHourPart()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Hour <= DateTime.Now.Hour);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcHHPart, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcHHPart, ldef.GetFunction(ldef.paramTODAY))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// минуты.
        /// </summary>
        [Fact]
        public void GetLcsTestMinPart()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Minute <= DateTime.Now.Minute);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcMIPart, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcMIPart, ldef.GetFunction(ldef.paramTODAY))),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// свойства переменной.
        /// </summary>
        [Fact]
        public void GetLcsTestMinPartVar()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            DateTime moment = DateTime.Now.AddDays(-7);
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Minute <= moment.Minute);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLEQ,
                        ldef.GetFunction(ldef.funcMIPart, new VariableDef(ldef.DateTimeType, "Дата")),
                        ldef.GetFunction(ldef.funcMIPart, moment)),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Другие свойства переменной.
        /// </summary>
        [Fact]
        public void GetLcsTestOnlyDateVar()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            DateTime moment = DateTime.Now.AddDays(-7);
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата == moment.Date);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        new VariableDef(ldef.DateTimeType, "Дата"),
                        ldef.GetFunction(ldef.funcOnlyDate, moment)),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Ticks, Second, Millisecond, DayOfYear приводят к исключению.
        /// </summary>
        [Fact]
        public void GetLcsTestTicksException()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Перелом>();
                DateTime moment = DateTime.Now.AddDays(-7);
                var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.Ticks == 34);
                new Query<Перелом>(testProvider).Where(predicate).ToArray();
                Expression queryExpression = testProvider.InnerExpression;

                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                     queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            });
            Assert.IsType(typeof(MethodSignatureException), exception);
        }

        /// <summary>
        /// Тест на AddYear.
        /// </summary>
        [Fact]
        public void GetLcsTestAddYear()
        {
            DateTime now = DateTime.Now;
            var testProvider = new TestLcsQueryProvider<Перелом>();
            var predicate = (Expression<Func<Перелом, bool>>)(o => o.Дата.AddYears(1) < now.Date);
            new Query<Перелом>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;

            var expectedLf = ldef.GetFunction(
                ldef.funcL,
                ldef.GetFunction(
                    ldef.funcDateAdd,
                    ldef.GetFunction(ldef.paramYearDIFF),
                    1,
                    new VariableDef(ldef.DateTimeType, "Дата")),
                ldef.GetFunction(ldef.funcOnlyDate, now));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = expectedLf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Перелом)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест на построение lcs, когда ограничение по полю с названием как у свойства DateTime типа.
        /// </summary>
        [Fact]
        public void GetLcsTestDateField()
        {
            DateTime now = DateTime.Now;
            var testProvider = new TestLcsQueryProvider<DateField>();
            var predicate = (Expression<Func<DateField, bool>>)(o => o.Date.Date < now.Date);
            new Query<DateField>(testProvider).Where(predicate).ToArray();

            Expression queryExpression = testProvider.InnerExpression;

            var expectedLf = ldef.GetFunction(
                ldef.funcL,
                ldef.GetFunction(
                    ldef.funcOnlyDate,
                    new VariableDef(ldef.DateTimeType, "Date")),
                ldef.GetFunction(ldef.funcOnlyDate, now));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = expectedLf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, DateField.Views.DateFieldE);
            Assert.True(Equals(expected, actual));
        }
    }
}
