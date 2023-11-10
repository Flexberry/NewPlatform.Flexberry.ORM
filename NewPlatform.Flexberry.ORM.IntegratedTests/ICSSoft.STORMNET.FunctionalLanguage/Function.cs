namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using System;
    using System.Collections.Generic;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using Unity.Injection;

    /// <summary>
    /// Класс для тестирования Function.cs.
    /// </summary>
    public class FunctionTest
    {
        private static ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Проверка преобразования в дружелюбную строку.
        /// </summary>
        [Fact]

        public void FunctionSerializeTst1()
        {
            ExternalLangDef eld = ExternalLangDef.LanguageDef;
            Guid userId = Guid.NewGuid();

            Function f = eld.GetFunction(
                eld.funcAND,
                eld.GetFunction(eld.funcEQ, new VariableDef(eld.BoolType, "Актуально")),
                eld.GetFunction(eld.funcEQ, new VariableDef(eld.GuidType, "Пользователь"), userId));
            string s = f.ToUserFriendlyString();

            Console.WriteLine(s);
            Assert.Equal("Актуально И Пользователь = " + userId.ToString(), s);
        }

        [Fact]

        public void FunctionSerializeTst2()
        {
            ExternalLangDef eld = ExternalLangDef.LanguageDef;
            Guid userId = Guid.NewGuid();

            Function f = eld.GetFunction(
                eld.funcAND,
                eld.GetFunction(eld.funcEQ, new VariableDef(eld.StringType, "Актуально"), "Да"),
                eld.GetFunction(eld.funcEQ, new VariableDef(eld.GuidType, "Пользователь"), userId));
            string s = f.ToUserFriendlyString();

            Console.WriteLine(s);
            Assert.Equal("Актуально = Да И Пользователь = " + userId.ToString(), s);
        }

        /// <summary>
        /// Проверяем переопределённый интерфейс IEquatable.
        /// </summary>
        [Fact]

        public void TestEqualFunctions()
        {
            Function emptyDetailNameFunc = GetSomeDetailFunction(string.Empty, ldef.funcG);
            Assert.NotEqual(emptyDetailNameFunc, null);

            Function emptyDetailNameFuncOther = GetSomeDetailFunction(string.Empty, ldef.funcG);
            Assert.Equal(emptyDetailNameFunc, emptyDetailNameFuncOther);
            Assert.True(Equals(emptyDetailNameFunc, emptyDetailNameFuncOther));

            Function someDetailNameFunc = GetSomeDetailFunction("SomeName", ldef.funcG);
            Assert.False(emptyDetailNameFunc == someDetailNameFunc);

            Function otherComparerElementFunc = GetSomeDetailFunction(string.Empty, ldef.funcGEQ);
            Assert.True(emptyDetailNameFunc != otherComparerElementFunc);

            var functionList = new List<Function>
                {
                    emptyDetailNameFunc,
                    someDetailNameFunc,
                };
            Assert.True(functionList.Contains(emptyDetailNameFuncOther));
            Assert.False(functionList.Contains(otherComparerElementFunc));
        }

        /// <summary>
        /// Тест метода GetLimitProperties.
        /// </summary>
        [Fact]

        public void FunctionGetLimitPropertiesTest()
        {
            var langdef = SQLWhereLanguageDef.LanguageDef;
            var func = new Function(
                langdef.Functions[0],
                new VariableDef(langdef.StringType, "Фамилия"),
                new Function(langdef.Functions[0]),
                    new VariableDef(langdef.StringType, "Имя"));
            var result = func.GetLimitProperties();

            Assert.True(result.Length == 2, "Длина массива.");
            Assert.Equal(result[0], "Фамилия");
            Assert.Equal(result[1], "Имя");
        }

        /// <summary>
        /// Тест метода Equals.
        /// </summary>
        [Fact]

        public void FunctionEqualsTest()
        {
            var langdef = SQLWhereLanguageDef.LanguageDef;
            var func = new Function(
                langdef.Functions[0],
                new VariableDef(langdef.StringType, "Фамилия"));
            Object obj = null;
            var result = func.Equals(obj);
            Assert.False(result);

            result = func.Equals(null);
            Assert.False(result);
        }

        /// <summary>
        /// Тест метода ToString.
        /// </summary>
        [Fact]

        public void FunctionToStringTest()
        {
            var langdef = SQLWhereLanguageDef.LanguageDef;
            var func = new Function(
                langdef.Functions[0],
                new VariableDef(langdef.StringType, "Фамилия"),
                null);

            var result = func.ToString();
            Assert.Equal(result, "ISNULL ( Фамилия NULL )");
        }

        /// <summary>
        /// Генерация exception в методе Check.
        /// </summary>
        [Fact]

        public void FunctionCheckExceptionTest()
        {
            var exception = Record.Exception(() =>
            {
                var langdef = SQLWhereLanguageDef.LanguageDef;
                var func = new Function(
                    langdef.Functions[0],
                    String.Empty);
                func.Check(true);
            });
            Assert.IsType(typeof(UncompatibleParameterTypeException), exception);
        }

        /// <summary>
        /// Генерация exception в методе Check.
        /// </summary>
        [Fact]

        public void FunctionCheckException1Test()
        {
            var exception = Record.Exception(() =>
            {
                var langdef = SQLWhereLanguageDef.LanguageDef;
                var func = new Function(
                    langdef.Functions[0],
                    new VariableDef(langdef.StringType, "Фамилия"));
                func.Check(true);
            });
            Assert.IsType(typeof(UncompatibleParameterTypeException), exception);
        }

        /// <summary>
        /// Генерация exception в методе Check.
        /// </summary>
        [Fact]

        public void FunctionCheckException2Test()
        {
            var exception = Record.Exception(() =>
            {
                var langdef = SQLWhereLanguageDef.LanguageDef;
                var func = new Function(
                    langdef.Functions[0],
                    new VariableDef(langdef.StringType, "Фамилия"),
                    null);
                func.Check(true);
            });
            Assert.IsType(typeof(ParameterCountException), exception);
        }

        /// <summary>
        /// Генерация exception в методе Check.
        /// </summary>
        [Fact]

        public void FunctionCheckException3Test()
        {
            var exception = Record.Exception(() =>
            {
                var langdef = SQLWhereLanguageDef.LanguageDef;
                var func = new Function(
                    langdef.Functions[42],
                    new Function(langdef.Functions[42]),
                        String.Empty);
                func.Check(true);
            });
            Assert.IsType(typeof(UncompatibleParameterTypeException), exception);
        }

        /// <summary>
        /// Тестирование метода Parse класса FunctionForControls.
        /// </summary>
        [Fact]

        public void FunctionForControlsParseTest()
        {
            var result = FunctionForControls.Parse("<test></test>", new View());
            Assert.Null(result.Function);
            Assert.Equal(result.Name, String.Empty);
        }

        /// <summary>
        /// Тестирование метода Parse класса FunctionForControls.
        /// </summary>
        [Fact]

        public void FunctionForControlsParse1Test()
        {
            var view = new View();
            var result = FunctionForControls.Parse("<test><Function></Function></test>", view);
            Assert.Null(result.Function);
            Assert.Equal(result.Name, String.Empty);
        }

        /// <summary>
        /// Тестирование метода Parse класса FunctionForControls.
        /// </summary>
        [Fact]

        public void FunctionForControlsParse2Test()
        {
            var view = new View();
            var result = FunctionForControls.Parse("<test><Value></Value></test>", view);
            Assert.Null(result.Function);
            Assert.Equal(result.Name, String.Empty);
        }

        /// <summary>
        /// Тестирование метода ToString класса FunctionForControls.
        /// </summary>
        [Fact]

        public void FunctionForControlsToStringTest()
        {
            var param = new Function(
                            new FunctionDef(
                                1,
                                new ObjectType("objStringedView", "objCaption", typeof(string)),
                                "stringView",
                                "objCaption",
                                "userViewFormat"));
            var functionForControls = new FunctionForControls(
                new View(),
                new Function(
                    new FunctionDef(
                        1,
                        new ObjectType("objStringedView", "objCaption", typeof(string)),
                        "stringView",
                        "objCaption",
                        "userViewFormat"),
                    new object[] { param }));
            var result = functionForControls.ToString();

            Assert.NotNull(result);
            Assert.Equal(
                result, "<Function Name=\"stringView\" ___Name=\"\"><Function Name=\"stringView\" /></Function>");
        }

        private Function GetSomeDetailFunction(string detailName, string comparer)
        {
            var dvd = new DetailVariableDef(
                ldef.GetObjectType("Details"),
                detailName,
                Выплаты.Views.ВыплатыViewE,
                Information.ExtractPropertyPath<Выплаты>(x => x.Кредит1),
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            Function lf = ldef.GetFunction(
                ldef.funcExistExact,
                dvd,
                ldef.GetFunction(
                    comparer,
                    new VariableDef(
                            ldef.NumericType,
                            Information.ExtractPropertyPath<Выплаты>(x => x.Кредит1.СрокКредита)),
                            20));
            return lf;
        }
    }
}
