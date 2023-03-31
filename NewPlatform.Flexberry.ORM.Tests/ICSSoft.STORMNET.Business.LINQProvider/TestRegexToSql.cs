using NewPlatform.Flexberry.ORM.Tests;

namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.Business.LINQProvider.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;

    /// <summary>
    /// Класс для тестирования преобразований из шаблона типа Regex в шаблон типа sql-like и наоборот.
    /// </summary>
    public class TestRegexToSql
    {
        /// <summary>
        /// Проверяем, пройдёт ли проверку невалидное регулярное выражение.
        /// </summary>
        [Fact]
        public void TestCheckNotValidRegex()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("[[");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое можно перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegex()
        {
            UtilsLcs.MinimalRegexCheck("abc");
            UtilsLcs.MinimalRegexCheck("^abc$");
            UtilsLcs.MinimalRegexCheck("^ab.c$");
            UtilsLcs.MinimalRegexCheck("^ab.*c$");
            // UtilsLcs.MinimalRegexCheck(@"^ab\\.*c$"); // TODO: временно экранирование снимается полностью.
            // UtilsLcs.MinimalRegexCheck(@"^ab\\d.*c$");
            // UtilsLcs.MinimalRegexCheck(@"^ab\\d\$.*c$");
            // UtilsLcs.MinimalRegexCheck(@"^ab\\d\^.*c$");
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("ab{2}c");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("ab(d)c");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult3()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("abd|c");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult4()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("abd+c");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult5()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("abd.?c");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult6()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("ab$dc");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult7()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck(@"ab\.*dc");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult8()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck(@"ab\ddc");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult9()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("^ab.*[1-9]c$");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult10()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("^ab.*^c$");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем, пройдёт ли проверку выражение, которое нельзя пока перевести в sql-like.
        /// </summary>
        [Fact]
        public void TestCheckRegexTooDifficult11()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalRegexCheck("^ab.*$c$");
            });
            Assert.IsType(typeof(NotSupportedRegexException), exception);
        }

        /// <summary>
        /// Проверяем корректность перевода из regex в sql-like.
        /// </summary>
        [Fact]
        public void TestFromRegexToSql()
        {
            Assert.Equal(@"*abc*", UtilsLcs.ConvertRegexToSql(@"abc"));
            Assert.Equal(@"abc*", UtilsLcs.ConvertRegexToSql(@"^abc"));
            Assert.Equal(@"*abc", UtilsLcs.ConvertRegexToSql(@"abc$"));
            Assert.Equal(@"abc", UtilsLcs.ConvertRegexToSql(@"^abc$"));
            Assert.Equal(@"abc$*", UtilsLcs.ConvertRegexToSql(@"^abc\$"));
            // Assert.Equal(@"abc\$*", UtilsLcs.ConvertRegexToSql(@"^abc\\\$")); //TODO: временно экранирование снимается со всех символов
            Assert.Equal(@"*ab*c*", UtilsLcs.ConvertRegexToSql(@"ab.*c"));
            Assert.Equal(@"*ab_c*", UtilsLcs.ConvertRegexToSql(@"ab.c"));
            Assert.Equal(@"*ab.c*", UtilsLcs.ConvertRegexToSql(@"ab\.c"));
            // Assert.Equal(@"*ab_c*", UtilsLcs.ConvertRegexToSql(@"ab_c")); //TODO: временно экранирование снимается со всех символов
            // Assert.Equal(@"*ab*c*", UtilsLcs.ConvertRegexToSql(@"ab\*c")); //TODO: временно экранирование снимается со всех символов
            Assert.Equal(@"*ab[c*", UtilsLcs.ConvertRegexToSql(@"ab\[c")); // TODO: временно экранирование снимается со всех символов
            Assert.Equal(@"*ab]c*", UtilsLcs.ConvertRegexToSql(@"ab\]c")); // TODO: временно экранирование снимается со всех символов
        }

        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Проверка перевода Regex в sql-like.
        /// </summary>
        [Fact]
        public void GetLcsForRegex()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, название которых содержит строки "12", а потом "3".
            new Query<Порода>(testProvider).Where(x => Regex.IsMatch(x.Название, "12.*3")).ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = this.ldef.GetFunction(ldef.funcLike, new VariableDef(ldef.StringType, "Название"), "*12*3*"),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка перевода констранды под Regex в качестве первого параметра в sql-like.
        /// </summary>
        [Fact]
        public void GetLcsForRegex2()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, название которых содержит строки "12", а потом "3".
            new Query<Порода>(testProvider).Where(x => Regex.IsMatch("123", "12.*3")).ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = this.ldef.GetFunction(ldef.funcLike, "123", "*12*3*"),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка, что некорректные на настоящий момент шаблоны для funcLike вызовут исключение.
        /// </summary>
        [Fact]
        public void TestNotSupportedSqlLike1()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalSqlCheck(@"*ab{%%%}c*");
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Проверка, что некорректные на настоящий момент шаблоны для funcLike вызовут исключение.
        /// </summary>
        [Fact]
        public void TestNotSupportedSqlLike2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.MinimalSqlCheck(@"*ab\\c*");
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Проверка правильности перевода из шаблона для funcLike в шаблон для Regex.
        /// </summary>
        [Fact]
        public void TestSupportedSqlLike()
        {
            Assert.Equal(@"abc", UtilsLcs.ConvertSqlToRegex(@"*abc*"));
            Assert.Equal(@"abc$", UtilsLcs.ConvertSqlToRegex(@"*abc"));
            Assert.Equal(@"^abc", UtilsLcs.ConvertSqlToRegex(@"abc*"));
            Assert.Equal(@"^abc$", UtilsLcs.ConvertSqlToRegex(@"abc"));
            Assert.Equal(@"^abc\{\}\?\+\(\)\[\]\|\^\$$", UtilsLcs.ConvertSqlToRegex(@"abc{}?+()[]|^$"));
            Assert.Equal(@"a.b.*c", UtilsLcs.ConvertSqlToRegex(@"*a_b*c*"));
        }

        /// <summary>
        /// Проверка правильности перевода из шаблона для Regex в шаблон для funcLike и наоборот.
        /// </summary>
        [Fact]
        public void TestSupportedRegexToSqlAndBack()
        { // Взяты тесты, которые были при переводе Regex в funcLike.
            CheckFromRegexToSQLAndBack(@"abc");
            CheckFromRegexToSQLAndBack(@"^abc");
            CheckFromRegexToSQLAndBack(@"abc$");
            CheckFromRegexToSQLAndBack(@"^abc$");
            CheckFromRegexToSQLAndBack(@"^abc\$");
            CheckFromRegexToSQLAndBack(@"ab.*c");
            CheckFromRegexToSQLAndBack(@"ab.c");
            CheckFromRegexToSQLAndBack(@"ab\.c");
            CheckFromRegexToSQLAndBack(@"ab\[c");
            CheckFromRegexToSQLAndBack(@"ab\]c");
        }

        /// <summary>
        /// Проверка правильности перевода из шаблона для funcLike в шаблон для Regex и наоборот.
        /// </summary>
        [Fact]
        public void TestSupportedSqlLikeToRegexAndBack()
        { // Взяты тесты, которые были при переводе Regex в funcLike.
            CheckFromSQLToRegexAndBack(@"abc{...}?+()[]|^$");
            CheckFromSQLToRegexAndBack(@"*a_b*c*");
        }

        /// <summary>
        /// Проверяем правильность перевода из Regex в funcLike и наоборот (в результате должно получиться то же, что и ушло на вход).
        /// </summary>
        /// <param name="testString"> Исходная и в то же время результирующая строка. </param>
        private void CheckFromRegexToSQLAndBack(string testString)
        {
            Assert.Equal(testString, UtilsLcs.ConvertSqlToRegex(UtilsLcs.ConvertRegexToSql(testString)));
        }

        /// <summary>
        /// Проверяем правильность перевода из шаблона для funcLike в шаблон для Regex и наоборот (в результате должно получиться то же, что и ушло на вход).
        /// </summary>
        /// <param name="testString"> Исходная и в то же время результирующая строка. </param>
        private void CheckFromSQLToRegexAndBack(string testString)
        {
            Assert.Equal(testString, UtilsLcs.ConvertRegexToSql(UtilsLcs.ConvertSqlToRegex(testString)));
        }
    }
}
