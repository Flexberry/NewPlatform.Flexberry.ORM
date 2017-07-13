namespace ICSSoft.STORMNET.Tests.TestClasses.UserDataTypes
{
    using System;
    using ICSSoft.STORMNET.UserDataTypes;
    using Xunit;

    /// <summary>
    /// Класс для тестирования Event.cs.
    /// </summary>
    
    public class EventTest
    {
        /// <summary>
        /// Проверка явного преобразования Event в string.
        /// </summary>
        [Fact]
        
        public void EventExplicitEventToStringTest()
        {
            var testEvent = new Event();
            Assert.Equal((string)testEvent, "<event />");

            testEvent.Name = "tName";
            Assert.Equal((string)testEvent, "<event type=\"tName\" />");

            testEvent.Description = "tDescr";
            Assert.Equal((string)testEvent, "<event type=\"tName\" description=\"tDescr\" />");

            testEvent.Author = "tAuthor";
            Assert.Equal((string)testEvent, "<event type=\"tName\" description=\"tDescr\" author=\"tAuthor\" />");

            testEvent.Place = "tPlace";
            Assert.Equal((string)testEvent, "<event type=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" />");

            testEvent.Category = "tCategory";
            Assert.Equal((string)testEvent, "<event type=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" category=\"tCategory\" />");

            testEvent.StartTime = new DateTime(2013, 09, 01);
            Assert.Equal((string)testEvent, "<event type=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" category=\"tCategory\" start=\"2013-09-01T00:00:00\" />");

            testEvent.FinishTime = new NullableDateTime { Value = new DateTime(2013, 09, 10) };
            Assert.Equal((string)testEvent, "<event type=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" category=\"tCategory\" start=\"2013-09-01T00:00:00\" finish=\"10.09.2013 0:00:00\" />");
        }

        /// <summary>
        /// Проверка явного преобразования string в Event.
        /// </summary>
        [Fact]
        
        public void EventExplicitStringToEventTest()
        {
            Assert.Null((Event)String.Empty);

            var testEvent = new Event();
            var testXml = "<event />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.Name = "tName";
            testXml = "<event name=\"tName\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.Description = "tDescr";
            testXml = "<event name=\"tName\" description=\"tDescr\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.Author = "tAuthor";
            testXml = "<event name=\"tName\" description=\"tDescr\" author=\"tAuthor\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.Place = "tPlace";
            testXml = "<event name=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.Category = "tCategory";
            testXml = "<event name=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" category=\"tCategory\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.StartTime = new DateTime(2013, 09, 01);
            testXml = "<event name=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" category=\"tCategory\" start=\"2013-09-01T00:00:00\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));

            testEvent.FinishTime = new NullableDateTime { Value = new DateTime(2013, 09, 10) };
            testXml = "<event name=\"tName\" description=\"tDescr\" author=\"tAuthor\" place=\"tPlace\" category=\"tCategory\" start=\"2013-09-01T00:00:00\" finish=\"10.09.2013 0:00:00\" />";
            Assert.True(ExplicitStringToEventTest(testXml, testEvent));
        }

        /// <summary>
        /// Метод для проверки логики в тесте EventExplicitStringToEventTest.
        /// </summary>
        /// <param name="testXml">
        /// XML для преобразования.
        /// </param>
        /// <param name="testEvent">
        /// Сравниваемый контакт.
        /// </param>
        /// <returns>
        /// Если все правильно, возвращается true <see cref="bool"/>.
        /// </returns>
        private bool ExplicitStringToEventTest(string testXml, Event testEvent)
        {
            var xmlEvent = (Event)testXml;
            return xmlEvent.Name == testEvent.Name && xmlEvent.Description == testEvent.Description
                   && xmlEvent.Author == testEvent.Author && xmlEvent.Place == testEvent.Place
                   && xmlEvent.Category == testEvent.Category && xmlEvent.StartTime == testEvent.StartTime
                   && xmlEvent.FinishTime == testEvent.FinishTime && xmlEvent.ToString() == testEvent.ToString();
        }
    }
}
