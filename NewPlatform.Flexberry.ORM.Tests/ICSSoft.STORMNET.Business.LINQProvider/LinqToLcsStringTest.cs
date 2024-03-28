namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET.Business.LINQProvider.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    public class LinqToLcsStringTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        #region Дополнительные тесты на char и char?.

        /// <summary>
        /// Получение lcs для типа char.
        /// </summary>
        [Fact]
        public void GetLcsWithCharEq()
        {
            Assert.True(
                CheckChar(
                    x => x.РазмерChar == '1',
                    this.ldef.GetFunction(this.ldef.funcEQ, new VariableDef(this.ldef.StringType, "РазмерChar"), 49)));
        }

        /// <summary>
        /// Получение lcs для типа char.
        /// </summary>
        [Fact]
        public void GetLcsWithCharGeq()
        {
            Assert.True(
                CheckChar(
                    x => x.РазмерChar >= '1',
                    this.ldef.GetFunction(this.ldef.funcGEQ, new VariableDef(this.ldef.StringType, "РазмерChar"), 49)));
        }

        /// <summary>
        /// Получение lcs для типа char?.
        /// </summary>
        [Fact]
        public void GetLcsWithNullChar()
        {
            Assert.True(
                CheckChar(
                    x => x.РазмерNullableChar == '1',
                    this.ldef.GetFunction(this.ldef.funcEQ, new VariableDef(this.ldef.StringType, "РазмерNullableChar"), '1')));
        }

        /// <summary>
        /// Получение lcs для типа char?.
        /// </summary>
        [Fact]
        public void GetLcsWithNullChar2()
        {
            Assert.True(
                CheckChar(
                    x => x.РазмерNullableChar == null,
                    this.ldef.GetFunction(this.ldef.funcIsNull, new VariableDef(this.ldef.StringType, "РазмерNullableChar"))));
        }

        /// <summary>
        /// Общий метод сравнения предполагаемой lcs и результата конвертации linq-выражения.
        /// </summary>
        /// <param name="predicate">Условие linq-выражения.</param>
        /// <param name="limitFunction">Предполагаемая lcs.</param>
        private bool CheckChar(Expression<Func<Лапа, bool>> predicate, Function limitFunction)
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(predicate).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = limitFunction,
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            return Equals(expected, actual);
        }

        #endregion Дополнительные тесты на char и char?.

        [Fact]
        public void GetLcsTest6()
        {
            const string Pattern = "ош";
            const string ldefPattern = "%ош%";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(pn => pn.Кличка.Contains(Pattern));
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// равенствострок.
        /// </summary>
        [Fact]
        public void GetLcsTestStringEquals()
        {
            const string Pattern = "Кошка";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.Equals(Pattern) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ, new VariableDef(this.ldef.StringType, "Кличка"), Pattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// равенствострок наоборот.
        /// </summary>
        [Fact]
        public void GetLcsTestStringEqualsReverse()
        {
            const string Pattern = "Кошка";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where Pattern.Equals(pn.Кличка) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ, Pattern, new VariableDef(this.ldef.StringType, "Кличка")),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// тест фильтра StartsWith.
        /// </summary>
        [Fact]
        public void GetLcsTestStartWithString()
        {
            const string Pattern = "Ко";
            const string ldefPattern = "Ко%";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.StartsWith(Pattern) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест фильтра EndsWith.
        /// </summary>
        [Fact]
        public void GetLcsTestEndsWithString()
        {
            const string Pattern = "ка";
            const string ldefPattern = "%ка";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.EndsWith(Pattern) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест исключения при использовании перегруженного StartWith.
        /// </summary>
        [Fact]
        public void GetLcsTestStartsWithException()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                const string Pattern = "Ко";
                IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
                IQueryable<Кошка> query = from pn in queryList
                                          where pn.Кличка.StartsWith(Pattern, StringComparison.OrdinalIgnoreCase)
                                          select pn;
                Expression queryExpression = query.Expression;
                LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            });
            Assert.IsType(typeof(MethodSignatureException), exception);
        }

        /// <summary>
        /// Тест исключения при использовании перегруженного EndsWith.
        /// </summary>
        [Fact]
        public void GetLcsTestEndsWithException()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                const string Pattern = "ка";
                IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
                IQueryable<Кошка> query = from pn in queryList
                                          where pn.Кличка.EndsWith(Pattern, StringComparison.OrdinalIgnoreCase)
                                          select pn;
                Expression queryExpression = query.Expression;
                LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            });
            Assert.IsType(typeof(MethodSignatureException), exception);
        }

        /// <summary>
        /// Тест фильтра EndsWith.
        /// </summary>
        [Fact]
        public void GetLcsTestSubstring1()
        {
            const string ldefPattern = "__ри";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.Substring(2) == "ри" select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест фильтра Subtring.
        /// </summary>
        [Fact]
        public void GetLcsTestSubstring2()
        {
            const string ldefPattern = "__р%";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.Substring(2, 1) == "р" select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест фильтра Subtring.
        /// </summary>
        [Fact]
        public void GetLcsTestSubstringRevert()
        {
            const string ldefPattern = "__р%";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where "р" == pn.Кличка.Substring(2, 2) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест исключения у Substring.
        /// </summary>
        [Fact]
        public void GetLcsTestMemberSubstring()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                const string ldefPattern = "__р%";
                IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
                IQueryable<Кошка> query = from pn in queryList where pn.Кличка.Substring(2, 2) == pn.Порода.Название select pn;
                Expression queryExpression = query.Expression;
                var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        ldef.GetFunction(
                            this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Порода.Название"), ldefPattern),
                };
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
                Assert.True(Equals(expected, actual));
            });
            Assert.IsType(typeof(MethodSignatureException), exception);
        }

        /// <summary>
        /// Тест для выражения Like.
        /// </summary>
        [Fact]
        public void GetLcsTestLike()
        {
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.IsLike(pattern) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для операций меньше, больше, меньше/равно, больше/равно для строк.
        /// </summary>
        [Fact]
        public void GetLcsTestCompare()
        {
            #region string.CompareTo(string)

            var pattern = "sdf";

            // Больше или равно
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn.Кличка.CompareTo(pattern) >= 0 select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcGEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Меньше или равно
            query = from pn in queryList where pn.Кличка.CompareTo(pattern) <= 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Больше
            query = from pn in queryList where pn.Кличка.CompareTo(pattern) > 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcG, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Меньше
            query = from pn in queryList where pn.Кличка.CompareTo(pattern) < 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcL, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            #endregion string.CompareTo(string)

            #region Compare(string, string)

            // Больше или равно
            queryList = new List<Кошка>().AsQueryable();
            query = from pn in queryList where string.Compare(pn.Кличка, pattern) >= 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcGEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Меньше или равно
            query = from pn in queryList where string.Compare(pn.Кличка, pattern) <= 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcLEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Больше
            query = from pn in queryList where string.Compare(pn.Кличка, pattern) > 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcG, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Меньше
            query = from pn in queryList where string.Compare(pn.Кличка, pattern) < 0 select pn;
            queryExpression = query.Expression;
            expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        this.ldef.funcL, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };
            actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            #endregion Compare(string, string)
        }

        /// <summary>
        /// Тест для проверки перевода перевода конструкции "String.Compare(String, String, StringComparison)>0" в lcs.
        /// </summary>
        [Fact]
        public void GetLcsTestCompareWithThreeArgumentsG()
        {
            // Arrange.
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(x => String.Compare(x.Кличка, pattern, StringComparison.Ordinal) > 0);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(this.ldef.funcG, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };

            // Act.
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для проверки перевода перевода конструкции "String.Compare(String, String, StringComparison)>=0" в lcs.
        /// </summary>
        [Fact]
        public void GetLcsTestCompareWithThreeArgumentsGE()
        {
            // Arrange.
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(x => String.Compare(x.Кличка, pattern, StringComparison.Ordinal) >= 0);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(this.ldef.funcGEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };

            // Act.
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для проверки перевода перевода конструкции "String.Compare(String, String, StringComparison).<0" в lcs.
        /// </summary>
        [Fact]
        public void GetLcsTestCompareWithThreeArgumentsL()
        {
            // Arrange.
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(x => String.Compare(x.Кличка, pattern, StringComparison.Ordinal) < 0);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(this.ldef.funcL, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };

            // Act.
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для проверки перевода перевода конструкции "String.Compare(String, String, StringComparison).<=0" в lcs.
        /// </summary>
        [Fact]
        public void GetLcsTestCompareWithThreeArgumentsLE()
        {
            // Arrange.
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(x => String.Compare(x.Кличка, pattern, StringComparison.Ordinal) <= 0);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(this.ldef.funcLEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };

            // Act.
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для проверки перевода перевода конструкции "String.Compare(String, String, StringComparison)==0" в lcs.
        /// </summary>
        [Fact]
        public void GetLcsTestCompareWithThreeArgumentsEQ()
        {
            // Arrange.
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(x => String.Compare(x.Кличка, pattern, StringComparison.Ordinal) == 0);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(this.ldef.funcEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };

            // Act.
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для проверки перевода перевода конструкции "String.Compare(String, String, StringComparison)!=0" в lcs.
        /// </summary>
        [Fact]
        public void GetLcsTestCompareWithThreeArgumentsNEQ()
        {
            // Arrange.
            var pattern = "sdf";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(x => String.Compare(x.Кличка, pattern, StringComparison.Ordinal) != 0);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(this.ldef.funcNEQ, new VariableDef(this.ldef.StringType, "Кличка"), pattern),
            };

            // Act.
            var actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestToUpper()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Кличка.ToUpper() == "КОШКА").ToList();

            Expression queryExpression = testProvider.InnerExpression;
            var lf = ldef.GetFunction(
                ldef.funcEQ,
                ldef.GetFunction(
                    ldef.funcToUpper,
                    new VariableDef(ldef.StringType, "Кличка")),
                "КОШКА");
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestToLower()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Кличка.ToLower() == "кошка").ToList();

            Expression queryExpression = testProvider.InnerExpression;
            var lf = ldef.GetFunction(
                ldef.funcEQ,
                ldef.GetFunction(
                    ldef.funcToLower,
                    new VariableDef(ldef.StringType, "Кличка")),
                "кошка");
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestEqualsWithoutStringComparision()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => string.Equals(o.Кличка, "кошка")).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "Кличка"), "кошка");
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestEqualsWithStringComparision()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Кошка>();
                new Query<Кошка>(testProvider).Where(o => string.Equals(o.Кличка, "кошка", StringComparison.OrdinalIgnoreCase)).ToList();
                Expression queryExpression = testProvider.InnerExpression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Поддержка на равенство string.Empty.
        /// </summary>
        [Fact]
        public void GetLcsTestEqualStringEmpty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => o.Кличка == string.Empty).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка на неравенство string.Empty.
        /// </summary>
        [Fact]
        public void GetLcsTestNotEqualStringEmpty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => o.Кличка != string.Empty).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка на равенство Null.
        /// </summary>
        [Fact]
        public void GetLcsTestEqualNull()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => o.Кличка == null).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка на неравенство Null.
        /// </summary>
        [Fact]
        public void GetLcsTestNotEqualNull()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => o.Кличка != null).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка string.IsNullOrEmpty.
        /// </summary>
        [Fact]
        public void GetLcsTestIsNullOrEmpty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => string.IsNullOrEmpty(o.Кличка)).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка !string.IsNullOrEmpty.
        /// </summary>
        [Fact]
        public void GetLcsTestNotIsNullOrEmpty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => !string.IsNullOrEmpty(o.Кличка)).ToList();

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка !!string.IsNullOrEmpty.
        /// </summary>
        [Fact]
        public void GetLcsTestNotNotIsNullOrEmpty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // ReSharper disable DoubleNegationOperator
            new Query<Кошка>(testProvider).Where(o => !!string.IsNullOrEmpty(o.Кличка)).ToList();
            // ReSharper restore DoubleNegationOperator

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Поддержка !!!string.IsNullOrEmpty.
        /// </summary>
        [Fact]
        public void GetLcsTestNotNotNotIsNullOrEmpty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // ReSharper disable DoubleNegationOperator
            new Query<Кошка>(testProvider).Where(o => !!!string.IsNullOrEmpty(o.Кличка)).ToList();
            // ReSharper restore DoubleNegationOperator

            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var lf = ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.StringType, "Кличка"));

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        #region Тестирование ToString()

        /// <summary>
        /// Проверка, что корректно работает ToString в LinqProvider.
        /// </summary>
        [Fact]
        public void GetLcsTestToString()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(o => o.Номер.ToString() == "7").ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "Номер"), "7");
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = lf };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем, что для Nullable-типа в LinqProvider можно взять ToString.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntToString()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt.ToString() == "4").ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.StringType, "РазмерNullableInt"),
                        "4"),
            };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем, что для Nullable-типа в LinqProvider можно взять ToString от null.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntToStringNull()
        {
            int? size = null;
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt.ToString() == size.ToString()).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcIsNull,
                        new VariableDef(this.ldef.NumericType, "РазмерNullableInt")),
            };

            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем, что для Nullable-типа в LinqProvider можно взять ToString от Value.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithValueToString()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt.Value.ToString() == "4").ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.StringType, "РазмерNullableInt"),
                        "4"),
            };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем, что для Nullable-типа в LinqProvider можно взять ToString от Value в связке с другим выражением.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithValueToString2()
        {
            string size = "4";
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt != null && x.РазмерNullableInt.Value.ToString() == size).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcAND,
                        this.ldef.GetFunction(
                        this.ldef.funcNotIsNull,
                        new VariableDef(this.ldef.StringType, "РазмерNullableInt")),
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.StringType, "РазмерNullableInt"),
                        size)),
            };

            Assert.True(Equals(expected, actual));
        }
        #endregion Тестирование ToString()
    }
}
