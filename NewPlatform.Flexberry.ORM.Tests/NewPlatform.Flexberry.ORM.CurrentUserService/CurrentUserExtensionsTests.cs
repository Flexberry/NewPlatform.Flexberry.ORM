namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using NewPlatform.Flexberry.ORM.CurrentUserService;

    using Xunit;

    /// <summary>
    /// Class of unit test for extension methods (<see cref="CurrentUserExtensions" />).
    /// </summary>
    public class CurrentUserExtensionsTests
    {
        /// <summary>
        /// Test method for <see cref="CurrentUserExtensions.GetDownLevelLogonName" />.
        /// Checks main logic.
        /// </summary>
        [Fact]
        public void TestGetLoginWithDomain()
        {
            Assert.Equal("TSTDMN\\tstlogin", new EmptyCurrentUser { Domain = "TSTDMN", Login = "tstlogin" }.GetDownLevelLogonName());
        }

        /// <summary>
        /// Test method for <see cref="CurrentUserExtensions.GetDownLevelLogonName" />.
        /// Checks exception on null user.
        /// </summary>
        [Fact]
        public void TestGetLoginWithDomainWithNullUser()
        {
            Assert.Throws<ArgumentNullException>(() => (null as ICurrentUser).GetDownLevelLogonName());
        }
    }
}
