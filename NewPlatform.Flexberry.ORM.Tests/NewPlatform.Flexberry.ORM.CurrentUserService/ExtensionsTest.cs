namespace CurrentUserService.Tests
{
    using System;

    using ICSSoft.Services;

    using Xunit;

    /// <summary>
    /// Class of unit test for extension methods (<see cref="CurrentUserServiceExtensions"/>).
    /// </summary>
    public class ExtensionsTest
    {
        /// <summary>
        /// Test method for <see cref="CurrentUserServiceExtensions.GetDownLevelLogonName"/>.
        /// Checks main logic.
        /// </summary>
        [Fact]
        public void TestGetLoginWithDomain()
        {
            Assert.Equal("TSTDMN\\tstlogin", new CurrentUser { Domain = "TSTDMN", Login = "tstlogin" }.GetDownLevelLogonName());
        }

        /// <summary>
        /// Test method for <see cref="CurrentUserServiceExtensions.GetDownLevelLogonName"/>.
        /// Checks exception on null user.
        /// </summary>
        [Fact]
        public void TestGetLoginWithDomainWithNullUser()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                (null as CurrentUser).GetDownLevelLogonName();
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }
    }
}
