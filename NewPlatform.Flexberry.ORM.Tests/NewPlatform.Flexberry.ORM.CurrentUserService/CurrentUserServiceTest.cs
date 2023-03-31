namespace CurrentUserService.Tests
{
    using System;

    using ICSSoft.Services;

    using Xunit;

    /// <summary>
    /// Class of unit test for <see cref="CurrentUserService"/>.
    /// </summary>
    public class CurrentUserServiceTest
    {
        /// <summary>
        /// Test implementation of <see cref="CurrentUserService.IUser"/>.
        /// </summary>
        private class CustomUser : CurrentUserService.IUser
        {
            public string FriendlyName
            {
                get { return "custom user name"; }
                set { throw new NotImplementedException(); }
            }

            public string Domain
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public string Login
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }

        /// <summary>
        /// Test method for <see cref="CurrentUserService.CurrentUser"/>
        /// Checks existence of the default user (Windows authorization is used in test runner).
        /// </summary>
        [Fact]
        public void TestDefaultUser()
        {
            Assert.False(string.IsNullOrEmpty(CurrentUserService.CurrentUser.FriendlyName));
        }

        /// <summary>
        /// Test method for <see cref="CurrentUserService.ResolveUser{T}"/>.
        /// Checks registering custom user for current.
        /// </summary>
        [Fact]
        public void TestCustomUser()
        {
            CurrentUserService.ResolveUser<CustomUser>();
            var myUser = new CustomUser();

            Assert.Equal(myUser.FriendlyName, CurrentUserService.CurrentUser.FriendlyName);

            CurrentUserService.ResolveUser<CurrentUser>();
        }

        /// <summary>
        /// Test method for <see cref="CurrentUserService.CurrentUser"/>.
        /// Checks getters and setter for data of current user.
        /// </summary>
        [Fact]
        public void TestCustomUserName()
        {
            const string fn = "friendly name";
            const string login = "login";

            CurrentUserService.CurrentUser.Login = login;
            Assert.Equal(login, CurrentUserService.CurrentUser.Login);

            CurrentUserService.CurrentUser.FriendlyName = fn;
            Assert.Equal(fn, CurrentUserService.CurrentUser.FriendlyName);
        }
    }
}
