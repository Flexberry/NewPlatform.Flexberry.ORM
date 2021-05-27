namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Unit tests for <see cref="AuditService.IsTypeAuditable"/>.
    /// </summary>
    public partial class AuditServiceTest
    {
        /// <summary>
        /// Tests the <see cref="AuditService.IsTypeAuditable"/> for audit service without settings.
        /// All types aren't auditable.
        /// </summary>
        [Fact]
        public void TestIsTypeAuditableWithoutAuditSettings()
        {
            // Assert.
            var auditService = new AuditService();

            // Act && Assert.
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithSettings)));
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithoutSettings)));
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithDisabledAudit)));
        }

        /// <summary>
        /// Tests the <see cref="AuditService.IsTypeAuditable"/> for audit service with disabled audit.
        /// All types aren't auditable.
        /// </summary>
        [Fact]
        public void TestIsTypeAuditableWithDisabledAudit()
        {
            // Assert.
            var auditService = new AuditService
            {
                AppSetting = new AuditAppSetting { AuditEnabled = false },
            };

            // Act && Assert.
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithSettings)));
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithoutSettings)));
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithDisabledAudit)));
        }

        /// <summary>
        /// Tests the <see cref="AuditService.IsTypeAuditable"/> for audit service with enabled audit.
        /// All types with settings and enabled audit are auditable.
        /// </summary>
        [Fact]
        public void TestIsTypeAuditableWithEnabledAudit()
        {
            // Assert.
            var auditService = new AuditService
            {
                AppSetting = new AuditAppSetting { AuditEnabled = true },
            };

            // Act && Assert.
            Assert.True(auditService.IsTypeAuditable(typeof(AuditClassWithSettings)));
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithoutSettings)));
            Assert.False(auditService.IsTypeAuditable(typeof(AuditClassWithDisabledAudit)));
        }
    }
}
