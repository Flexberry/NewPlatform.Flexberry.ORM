namespace ICSSoft.Services.UnityFactory.Tests
{
    using ICSSoft.Services;

    using Xunit;

    /// <summary>
    /// Unit test class for <see cref="UnityFactory"/> class.
    /// </summary>
    public class UnitFactoryTest
    {
        /// <summary>
        /// Tests the <see cref="UnityFactory.CreateContainer"/> method.
        /// Method has to return the different instances each time.
        /// </summary>
        [Fact]
        public void TestCreateContainer()
        {
            Assert.NotEqual(UnityFactory.CreateContainer(), UnityFactory.CreateContainer());
        }

        /// <summary>
        /// Tests the <see cref="UnityFactory.GetContainer"/> method.
        /// Method has to return the same instances each time.
        /// </summary>
        [Fact]
        public void TestGetContainer()
        {
            Assert.Equal(UnityFactory.GetContainer(), UnityFactory.GetContainer());
        }
    }
}
