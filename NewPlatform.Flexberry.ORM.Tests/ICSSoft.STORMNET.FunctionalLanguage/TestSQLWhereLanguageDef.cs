namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="SQLWhereLanguageDef"/>.
    /// </summary>
    public class TestSQLWhereLanguageDef
    {
        /// <summary>
        /// Тесты для метода <see cref="SQLWhereLanguageDef.SQLTranslSwitch"/>.
        /// </summary>
        [Fact]
        public void TestSQLTranslSwitch()
        {
            ExternalLangDef langdef = ExternalLangDef.LanguageDef;
            Assert.Throws<Exception>(() => langdef.SQLTranslSwitch(
                new ICSSoft.STORMNET.FunctionalLanguage.Function(),
                new delegateConvertValueToQueryValueString(TestMetodForDelegate),
                new delegatePutIdentifierToBrackets(TestMetodForDelegate),
                new object()));
        }

        /// <summary>
        /// Тесты для метода <see cref="SQLWhereLanguageDef.ToSQLString"/>.
        /// </summary>
        [Fact]
        public void TestToSQLString()
        {
            Assert.Throws<Exception>(() => SQLWhereLanguageDef.ToSQLString(
                    new ICSSoft.STORMNET.FunctionalLanguage.Function(),
                    new delegateConvertValueToQueryValueString(TestMetodForDelegate),
                    new delegatePutIdentifierToBrackets(TestMetodForDelegate),
                    new object()));
        }

        /// <summary>
        /// Mock метод для делегатов.
        /// </summary>
        /// <param name="value"></param>
        public virtual string TestMetodForDelegate(object value)
        {
            return "NULL";
        }
    }
}
