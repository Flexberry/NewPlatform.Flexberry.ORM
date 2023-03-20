namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using System;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Unit test class for <see cref="TypeAuditClassSettingsLoader"/>.
    /// </summary>
    public class AuditClassSettingsLoaderTest
    {
        /// <summary>
        /// The lock object for testing changes in static fields.
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.HasSettings"/>.
        /// Tests correctness of checking existence of audit settings class.
        /// </summary>
        [Fact]
        public void TestHasSettings()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.True(settingsLoader.HasSettings(typeof(AuditClassWithSettings)));
            Assert.False(settingsLoader.HasSettings(typeof(AuditClassWithoutSettings)));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.IsAuditEnabled(Type)"/>.
        /// Tests correctness of loading flag of enabled audit from audit settings class.
        /// </summary>
        [Fact]
        public void TestIsAuditEnabled()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.True(settingsLoader.IsAuditEnabled(typeof(AuditClassWithSettings)));

            TestWithExpectedException(() => settingsLoader.IsAuditEnabled(typeof(AuditClassWithoutSettings)));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.IsAuditEnabled(Type,tTypeOfAuditOperation)"/>.
        /// Tests correctness of loading flag of enabled audit from audit settings class.
        /// </summary>
        [Fact]
        public void TestIsAuditEnabledForOperation()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.True(settingsLoader.IsAuditEnabled(typeof(AuditClassWithSettings), tTypeOfAuditOperation.SELECT));
            Assert.True(settingsLoader.IsAuditEnabled(typeof(AuditClassWithSettings), tTypeOfAuditOperation.INSERT));
            Assert.True(settingsLoader.IsAuditEnabled(typeof(AuditClassWithSettings), tTypeOfAuditOperation.UPDATE));
            Assert.True(settingsLoader.IsAuditEnabled(typeof(AuditClassWithSettings), tTypeOfAuditOperation.DELETE));
            // Assert.False(settingsLoader.IsAuditEnabled(typeof(AuditClassWithSettings), tTypeOfAuditOperation.EXECUTE));

            TestWithExpectedException(() => settingsLoader.IsAuditEnabled(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.SELECT));
            TestWithExpectedException(() => settingsLoader.IsAuditEnabled(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.INSERT));
            TestWithExpectedException(() => settingsLoader.IsAuditEnabled(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.UPDATE));
            TestWithExpectedException(() => settingsLoader.IsAuditEnabled(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.DELETE));
            // TestWithExpectedException(() => settingsLoader.IsAuditEnabled(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.EXECUTE));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.UseDefaultAuditView"/>.
        /// Tests correctness of loading flag of using default audit view from audit settings class.
        /// </summary>
        [Fact]
        public void TestUseDefaultAuditView()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.False(settingsLoader.UseDefaultAuditView(typeof(AuditClassWithSettings)));

            TestWithExpectedException(() => settingsLoader.UseDefaultAuditView(typeof(AuditClassWithoutSettings)));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.GetAuditConnectionString"/>.
        /// Tests correctness of loading audit connection string from audit settings class.
        /// </summary>
        [Fact]
        public void TestGetAuditConnectionString()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.Null(settingsLoader.GetAuditConnectionString(typeof(AuditClassWithSettings)));

            TestWithExpectedException(() => settingsLoader.GetAuditConnectionString(typeof(AuditClassWithoutSettings)));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.GetAuditService"/>.
        /// Tests correctness of loading audit service from audit settings class.
        /// </summary>
        [Fact]
        public void TestGetAuditService()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.Null(settingsLoader.GetAuditService(typeof(AuditClassWithSettings)));

            TestWithExpectedException(() => settingsLoader.GetAuditService(typeof(AuditClassWithoutSettings)));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.GetAuditWriteMode"/>.
        /// Tests correctness of loading write mode from audit settings class.
        /// </summary>
        [Fact]
        public void TestGetAuditWriteMode()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.Equal(tWriteMode.Synchronous, settingsLoader.GetAuditWriteMode(typeof(AuditClassWithSettings)));

            TestWithExpectedException(() => settingsLoader.GetAuditWriteMode(typeof(AuditClassWithoutSettings)));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.GetAuditViewName"/>.
        /// Tests correctness of loading names of views from audit settings class.
        /// </summary>
        [Fact]
        public void TestGetAuditViewName()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.Equal("SelectAuditView", settingsLoader.GetAuditViewName(typeof(AuditClassWithSettings), tTypeOfAuditOperation.SELECT));
            Assert.Equal("InsertAuditView", settingsLoader.GetAuditViewName(typeof(AuditClassWithSettings), tTypeOfAuditOperation.INSERT));
            Assert.Equal("UpdateAuditView", settingsLoader.GetAuditViewName(typeof(AuditClassWithSettings), tTypeOfAuditOperation.UPDATE));
            Assert.Equal("DeleteAuditView", settingsLoader.GetAuditViewName(typeof(AuditClassWithSettings), tTypeOfAuditOperation.DELETE));
            // Assert.Equal("ExecuteAuditView", settingsLoader.GetAuditViewName(typeof(AuditClassWithSettings), tTypeOfAuditOperation.EXECUTE));

            TestWithExpectedException(() => settingsLoader.GetAuditViewName(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.SELECT));
            TestWithExpectedException(() => settingsLoader.GetAuditViewName(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.INSERT));
            TestWithExpectedException(() => settingsLoader.GetAuditViewName(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.UPDATE));
            TestWithExpectedException(() => settingsLoader.GetAuditViewName(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.DELETE));
            // TestWithExpectedException(() => settingsLoader.GetAuditViewName(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.EXECUTE));
        }

        /// <summary>
        /// Test method for <see cref="TypeAuditClassSettingsLoader.GetAuditView"/>.
        /// Tests correctness of loading views from audit settings class.
        /// </summary>
        [Fact]
        public void TestGetAuditView()
        {
            var settingsLoader = new TypeAuditClassSettingsLoader();

            Assert.Equal("SelectAuditView", settingsLoader.GetAuditView(typeof(AuditClassWithSettings), tTypeOfAuditOperation.SELECT).Name);
            Assert.Equal("InsertAuditView", settingsLoader.GetAuditView(typeof(AuditClassWithSettings), tTypeOfAuditOperation.INSERT).Name);
            Assert.Equal("UpdateAuditView", settingsLoader.GetAuditView(typeof(AuditClassWithSettings), tTypeOfAuditOperation.UPDATE).Name);
            Assert.Equal("DeleteAuditView", settingsLoader.GetAuditView(typeof(AuditClassWithSettings), tTypeOfAuditOperation.DELETE).Name);
            // Assert.Equal("ExecuteAuditView", settingsLoader.GetAuditView(typeof(AuditClassWithSettings), tTypeOfAuditOperation.EXECUTE).Name);

            // Test throwing exception whe specified view hasn't been found.
            lock (_lock)
            {
                string oldSelectAuditViewName = null;
                try
                {
                    oldSelectAuditViewName = AuditClassWithSettings.AuditSettings.SelectAuditViewName;
                    AuditClassWithSettings.AuditSettings.SelectAuditViewName = "unknownview";

                    settingsLoader.GetAuditView(typeof(AuditClassWithSettings), tTypeOfAuditOperation.SELECT);
                    Assert.True(false, "Assert.Fail");
                }
                catch (DataNotFoundAuditException)
                {
                }
                catch (Exception)
                {
                    Assert.True(false, "Assert.Fail");
                }
                finally
                {
                    if (oldSelectAuditViewName != null)
                    {
                        AuditClassWithSettings.AuditSettings.SelectAuditViewName = oldSelectAuditViewName;
                    }
                }
            }

            TestWithExpectedException(() => settingsLoader.GetAuditView(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.SELECT));
            TestWithExpectedException(() => settingsLoader.GetAuditView(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.INSERT));
            TestWithExpectedException(() => settingsLoader.GetAuditView(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.UPDATE));
            TestWithExpectedException(() => settingsLoader.GetAuditView(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.DELETE));
            // TestWithExpectedException(() => settingsLoader.GetAuditView(typeof(AuditClassWithoutSettings), tTypeOfAuditOperation.EXECUTE));
        }

        /// <summary>
        /// Tests the specified action with expected <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="action">The action for executing.</param>
        private static void TestWithExpectedException(Action action)
        {
            try
            {
                action();
                Assert.True(false, "Assert.Fail");
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception)
            {
                Assert.True(false, "Assert.Fail");
            }
        }
    }
}
