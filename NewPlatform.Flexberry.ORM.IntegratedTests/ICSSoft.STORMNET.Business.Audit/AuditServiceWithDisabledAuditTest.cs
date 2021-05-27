namespace ICSSoft.STORMNET.Business.Audit.IntegratedTests
{
    using ICSSoft.STORMNET.Business.Audit.Objects;

    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// ORM-integrated unit test class for <see cref="AuditService"/> with disabled audit.
    /// </summary>
    public class AuditServiceWithDisabledAuditTest : BaseAuditServiceTest
    {
        /// <summary>
        /// Unit test for <see cref="AuditService.AddCreateAuditInformation"/> and <see cref="AuditService.AddEditAuditInformation"/>.
        /// Tests correctness of setting of audit fields from <see cref="IDataObjectWithAuditFields"/> for the type with enabled audit.
        /// </summary>
        [Fact]
        public void TestSettingAuditFieldsForTypeWithEnabledAudit()
        {
            foreach (var dataService in DataServices)
            {
                var d1 = new AuditClassWithSettings();

                // Audit fields are empty after creating object.
                Assert.Null(d1.CreateTime);
                Assert.Null(d1.Creator);
                Assert.Null(d1.Editor);
                Assert.Null(d1.EditTime);

                dataService.UpdateObject(d1);

                // Audit fields are empty after persisting.
                Assert.Null(d1.CreateTime);
                Assert.Null(d1.Creator);
                Assert.Null(d1.Editor);
                Assert.Null(d1.EditTime);

                var d2 = new AuditClassWithSettings();
                d2.SetExistObjectPrimaryKey(d1.__PrimaryKey);
                dataService.LoadObject(d2);

                // Audit fields are persisted.
                Assert.Null(d2.CreateTime);
                Assert.Null(d2.Creator);
                Assert.Null(d2.Editor);
                Assert.Null(d2.EditTime);

                d2.Name = "42";
                dataService.UpdateObject(d2);

                // Audit fields are empty after persisting.
                Assert.Null(d2.CreateTime);
                Assert.Null(d2.Creator);
                Assert.Null(d2.Editor);
                Assert.Null(d2.EditTime);

                var d3 = new AuditClassWithSettings();
                d3.SetExistObjectPrimaryKey(d2.__PrimaryKey);
                dataService.LoadObject(d3);

                // Audit fields are persisted.
                Assert.Null(d3.CreateTime);
                Assert.Null(d3.Creator);
                Assert.Null(d3.Editor);
                Assert.Null(d3.EditTime);
            }
        }

        /// <summary>
        /// Gets the disabled audit service for the test.
        /// </summary>
        /// <returns>Returns instance of the <see cref="AuditService" /> class that will be used for the test.</returns>
        protected override AuditService GetAuditServiceForTest()
        {
            return new AuditService
            {
                AppSetting = new AuditAppSetting { AuditEnabled = false },
                ApplicationMode = AppMode.Win,
                Audit = new EmptyAudit(),
            };
        }
    }
}
