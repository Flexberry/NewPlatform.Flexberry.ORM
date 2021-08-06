namespace ICSSoft.STORMNET.Tests.TestClasses.UserDataTypes
{
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    /// <summary>
    /// Класс для тестирования Contact.cs.
    /// </summary>
    public class ContactTest
    {
        /// <summary>
        /// Проверка метода Compare.
        /// </summary>
        [Fact]

        public void ContactCompareTest()
        {
            var contactPhone = new Contact { Name = "WorkPhone", Value = "1234567", ContactType = "Phone" };
            var contactEmail = new Contact { Name = "WorkEmail", Value = "email@mail.com", ContactType = "Email" };
            var contactPhone1 = new Contact { Name = "WorkPhone", Value = "7654321", ContactType = "Phone" };
            var contactPhone2 = new Contact { Name = "WorkPhone", Value = "1234567", ContactType = "Phone" };

            Assert.Equal(contactPhone.Compare(contactEmail), 1);
            Assert.Equal(contactPhone.Compare(contactPhone1), 1);
            Assert.Equal(contactPhone.Compare(contactPhone), 0);
            Assert.Equal(contactPhone.Compare(contactPhone2), 0);
        }

        /// <summary>
        /// Проверка явного преобразования Contact в string.
        /// </summary>
        [Fact]

        public void ContactExplicitContactToStringTest()
        {
            var contactPhone = new Contact();
            Assert.Equal((string)contactPhone, "<contact />");

            contactPhone.Name = "WorkPhone";
            Assert.Equal((string)contactPhone, "<contact name=\"WorkPhone\" />");

            contactPhone.Value = "1234567";
            Assert.Equal((string)contactPhone, "<contact name=\"WorkPhone\" value=\"1234567\" />");

            contactPhone.ContactType = "Phone";
            Assert.Equal((string)contactPhone, "<contact type=\"Phone\" name=\"WorkPhone\" value=\"1234567\" />");
        }

        /// <summary>
        /// Проверка явного преобразования string в Contact.
        /// </summary>
        [Fact]

        public void ContactExplicitStringToContactTest()
        {
            Assert.Null((Contact)string.Empty);

            var contactPhone = new Contact();
            var testXml = "<contact />";
            Assert.True(ExplicitStringToContactTest(testXml, contactPhone));

            contactPhone.Name = "WorkPhone";
            testXml = "<contact name=\"WorkPhone\" />";
            Assert.True(ExplicitStringToContactTest(testXml, contactPhone));

            contactPhone.Value = "1234567";
            testXml = "<contact name=\"WorkPhone\" value=\"1234567\" />";
            Assert.True(ExplicitStringToContactTest(testXml, contactPhone));

            contactPhone.ContactType = "Phone";
            testXml = "<contact type=\"Phone\" name=\"WorkPhone\" value=\"1234567\" />";
            Assert.True(ExplicitStringToContactTest(testXml, contactPhone));
        }

        /// <summary>
        /// Метод для проверки логики в тесте ContactExplicitStringToContactTest.
        /// </summary>
        /// <param name="testXml">
        /// XML для преобразования.
        /// </param>
        /// <param name="contact">
        /// Сравниваемый контакт.
        /// </param>
        /// <returns>
        /// Если все правильно, возвращается true <see cref="bool"/>.
        /// </returns>
        private bool ExplicitStringToContactTest(string testXml, Contact contact)
        {
            var xmlContact = (Contact)testXml;
            return xmlContact.Name == contact.Name && xmlContact.Value == contact.Value
                   && xmlContact.ContactType == contact.ContactType && xmlContact.ToString() == contact.ToString();
        }
    }
}
