namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Runtime.Serialization.Formatters.Binary;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Tools;
    using Xunit;

    /// <summary>
    /// Тесты для <see cref="ToolBinarySerializer"/>.
    /// </summary>
    public class ToolBinarySerializerTest
    {
        /// <summary>
        /// Проверка, что <see cref="BinaryFormatter"/> корректно отрабатывает под разными дотнетами.
        /// Класс помечен obsolete, поэтому требуются разные ухищрения для запуска.
        /// </summary>
        [Fact]
        public void TestCorrectBinaryFormatterWork()
        {
            Guid testGuid = Guid.NewGuid();
            SQLWhereLanguageDef languageDef = SQLWhereLanguageDef.LanguageDef;
            Function limitFunction = languageDef.GetFunction(languageDef.funcEQ, new VariableDef(languageDef.GuidType, Information.ExtractPropertyPath<DataObject>(x => x.__PrimaryKey)), testGuid);
            object pseudoserializedFunction = languageDef.FunctionToSimpleStruct(limitFunction);
            string filterString = ToolBinarySerializer.ObjectToString(pseudoserializedFunction);

            Assert.NotNull(filterString);
        }
    }
}
