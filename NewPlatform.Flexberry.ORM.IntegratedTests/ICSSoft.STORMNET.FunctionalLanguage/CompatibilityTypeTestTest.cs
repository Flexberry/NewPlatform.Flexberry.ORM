namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using ICSSoft.STORMNET.FunctionalLanguage;
    using Xunit;

    /// <summary>
    ///  Класс для тестирования CompatibilityTypeTest.
    /// </summary>
    public class CompatibilityTypeTestTest
    {
        /// <summary>
        /// Тест для метода Check.
        /// </summary>
        [Fact]

        public void CompatibilityTypeTestCheckTest()
        {
            var result = CompatibilityTypeTest.Check(typeof(object), typeof(string));

            Assert.Equal(result, TypesCompatibilities.Convertable);
        }
    }
}
