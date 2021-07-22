namespace NewPlatform.Flexberry.ORM.Tests
{
    using Moq;

    using NewPlatform.Flexberry.ORM.CurrentUserService;

    using Unity;

    using Xunit;

    /// <summary>
    /// Тесты класса <see cref="UnityCurrentUserAccessor" />.
    /// </summary>
    public class UnityCurrentUserAccessorTests
    {
        /// <summary>
        /// Тест конструктора.
        /// </summary>
        [Fact]
        public void ConstructorTest()
        {
            // Arrange.
            var mockUnity = new Mock<IUnityContainer>();

            // Act.
            var accessor = new UnityCurrentUserAccessor(mockUnity.Object);

            // Assert.
            Assert.NotNull(accessor);
        }

        /// <summary>
        /// Тест получения текущего пользователя <see cref="UnityCurrentUserAccessor.CurrentUser" />,
        /// когда пользователь незарегистрирован в контейнере.
        /// </summary>
        [Fact]
        public void GetNotRegistredUserTest()
        {
            // Arrange.
            var mockUnity = new Mock<IUnityContainer>();
            var accessor = new UnityCurrentUserAccessor(mockUnity.Object);

            // Act.
            var user = accessor.CurrentUser;

            // Assert.
            Assert.Null(user);
        }
    }
}
