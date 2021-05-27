namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET;
    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="DataObject"/>.
    /// </summary>
    public class DataObjectTest
    {
        /// <summary>
        /// Тест метода <see cref="DataObject.ToStringForAudit"/>.
        /// Передаём представление, не содержащее свойств.
        /// </summary>
        [Fact]
        public void TestToStringForAuditEmptyView()
        {
            var client = new Клиент();
            var view = new View() { DefineClassType = typeof(Клиент) };

            string resultToString = client.ToStringForAudit(view);

            Assert.Equal("Клиент()", resultToString);
        }

        /// <summary>
        /// Тест метода <see cref="DataObject.ToStringForAudit"/>.
        /// Передаём представление, содержащее несколько свойств, все видимые.
        /// </summary>
        [Fact]
        public void TestToStringForAuditNormalView()
        {
            string propertyNameFio = Information.ExtractPropertyPath<Клиент>(x => x.ФИО);
            string propertyCaptionFio = "SomeFIOCaption";
            string propertyValueFio = "SomeFIOValue";

            string propertyNameAdress = Information.ExtractPropertyPath<Клиент>(x => x.Прописка);
            string propertyCaptionAdress = "SomeAdressCaption";
            string propertyValueAdress = "SomeAdressValue";

            var client = new Клиент { ФИО = propertyValueFio, Прописка = propertyValueAdress };
            var view = new View { DefineClassType = typeof(Клиент) };
            view.AddProperty(propertyNameFio, propertyCaptionFio, true, string.Empty);
            view.AddProperty(propertyNameAdress, propertyCaptionAdress, true, string.Empty);
            string expectedToString = string.Format("Клиент({0}={1}, {2}={3})", propertyCaptionFio, propertyValueFio, propertyCaptionAdress, propertyValueAdress);

            string resultToString = client.ToStringForAudit(view);

            Assert.Equal(expectedToString, resultToString);
        }

        /// <summary>
        /// Тест метода <see cref="DataObject.ToStringForAudit"/>.
        /// Передаём представление, содержащее свойства, в том числе невидимые.
        /// </summary>
        [Fact]
        public void TestToStringForAuditNormalViewWithInvisibleProperties()
        {
            string propertyCaption = "SomePropertyCaption";
            string propertyValue = "SomePropertyValue";
            var client = new Клиент { ФИО = propertyValue };
            var view = new View { DefineClassType = typeof(Клиент) };
            view.AddProperty(Information.ExtractPropertyPath<Клиент>(x => x.ФИО), propertyCaption, true, string.Empty);
            view.AddProperty(Information.ExtractPropertyPath<Клиент>(x => x.Прописка), "OtherCaption", false, string.Empty);
            string expectedToString = string.Format("Клиент({0}={1})", propertyCaption, propertyValue);

            string resultToString = client.ToStringForAudit(view);

            Assert.Equal(expectedToString, resultToString);
        }
    }
}
