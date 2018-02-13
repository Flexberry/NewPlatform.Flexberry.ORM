namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using System.Diagnostics;
    using System.Runtime.Serialization;

    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;

    using Xunit;

    
    public class FunctionSerializeTest
    {
        /// <summary>
        /// Тест десериализации функции из <see cref="ExternalLangDef"/>  через конструктор десериализации.
        /// </summary>
        [Fact]
        public void TestExternalLangDefFunctionDeserialization()
        {
            var externalLangDef = ExternalLangDef.LanguageDef;
            var function = externalLangDef.GetFunction(externalLangDef.funcCurrentUser);
            var info = new SerializationInfo(typeof(Function), new FormatterConverter());
            var context = new StreamingContext();
            function.GetObjectData(info, context);
            var deserialized = new Function(info, context);
            Debug.Assert(function.ToUserFriendlyString().Equals(deserialized.ToUserFriendlyString()));
        }
    }
}
