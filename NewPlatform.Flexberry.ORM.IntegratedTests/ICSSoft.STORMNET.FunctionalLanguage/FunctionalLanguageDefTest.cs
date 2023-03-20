namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using System;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    using Xunit;

    /// <summary>
    /// Класс для тестирования FunctionalLanguageDef.
    /// </summary>
    public class FunctionalLanguageDefTest
    {
        /// <summary>
        /// Тест вызова исключения при получении определения функции по id.
        /// </summary>
        [Fact]
        public void FunctionalLanguageDefGetFunctionDefExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                FunctionalLanguageDef langDef = new SQLWhereLanguageDef();

                // Вызов функции с несуществующим идентификатором.
                langDef.GetFunctionDef(0);
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Тест вызова исключения при получении определения функции по его строковому представлению.
        /// </summary>
        [Fact]
        public void FunctionalLanguageDefGetFunctionDefByStringedViewExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                FunctionalLanguageDef langDef = new SQLWhereLanguageDef();

                // Вызов функции с несуществующим строковым представлением.
                langDef.GetFunctionDefByStringedView(String.Empty);
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Тест для метода GetObjectTypeForNetType.
        /// </summary>
        [Fact]

        public void FunctionalLanguageDefGetObjectTypeForNetTypeTest()
        {
            FunctionalLanguageDef langDef = new SQLWhereLanguageDef();

            var result = langDef.GetObjectTypeForNetType(typeof(System.Decimal));
            Assert.Equal(result.NetCompatibilityType.Name, "Decimal");
        }

        /// <summary>
        /// Тест для метода GetObjectTypeForNetType.
        /// </summary>
        [Fact]

        public void FunctionalLanguageDefGetObjectTypeForNetType1Test()
        {
            FunctionalLanguageDef langDef = new SQLWhereLanguageDef();

            var result = langDef.GetObjectTypeForNetType(typeof(DetailArray));
            Assert.Null(result);
        }

        /// <summary>
        /// Тест для вызова исключения в методе GetFunction.
        /// </summary>
        [Fact]
        public void FunctionalLanguageDefGetFunctionExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                FunctionalLanguageDef langDef = new SQLWhereLanguageDef();

                var result = langDef.GetFunction("functionString");
            });
            Assert.IsType(typeof(FunctionalLanguageDef.NotFoundFunctionBySignatureException), exception);
        }

        /// <summary>
        /// Тест для вызова исключения в методе GetFunction.
        /// </summary>
        [Fact]
        public void FunctionalLanguageDefGetFunctionException1Test()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;

                var result = langDef.GetFunction(langDef.funcEQ, new object[] { null });
            });
            Assert.IsType(typeof(NullReferenceException), exception);
        }
    }
}
