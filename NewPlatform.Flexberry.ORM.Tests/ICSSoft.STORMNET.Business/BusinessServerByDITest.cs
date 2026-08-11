namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using ICSSoft.Services;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;
    using ICSSoft.STORMNET.Business.Audit.Objects;
    using ICSSoft.STORMNET.Business.Interfaces;
    using ICSSoft.STORMNET.Security;
    using Moq;
    using NewPlatform.Flexberry.ORM.CurrentUserService;
    using Unity;
    using Unity.Injection;
    using Xunit;

    /// <summary>
    /// Test on resolving business servers with its registration by Unity.
    /// </summary>
    public class BusinessServerByDITest
    {
        /// <summary>
        /// Test with not named registration at unity.
        /// </summary>
        [Fact]
        public void TestResolvingWithUnity()
        {
            // Arrange.
            var container = new UnityContainer();
            container.RegisterType<ICurrentUser, TestCurrentUser>();
            IServiceProvider serviceProvider = new UnityServiceProvider(container);
            IBusinessServerProvider businessServerProvider = new BusinessServerProvider(serviceProvider);

            // Act.
            BusinessServer[] listOfBusinessServers = businessServerProvider.GetBusinessServer(typeof(FlexberryUserSettingForTest), ObjectStatus.Created, GetDataService());

            // Assert.
            Assert.Single(listOfBusinessServers);
            Assert.Equal(typeof(FlexberryUserSettingForTestBS), listOfBusinessServers[0].GetType());
            Assert.Equal(typeof(TestCurrentUser), ((FlexberryUserSettingForTestBS)listOfBusinessServers[0]).currentUser.GetType());
        }

        /// <summary>
        /// Test with named registration at unity.
        /// </summary>
        [Fact]
        public void TestResolvingWithUnityWithNamedEntity()
        {
            // Arrange.
            var container = new UnityContainer();
            container.RegisterType<ICurrentUser, TestCurrentUser>();
            container.RegisterType<ICurrentUser, TestCurrentUser2>("emptyUser");
            container.RegisterType<FlexberryUserSettingForTestBS>(new InjectionConstructor(container.Resolve<ICurrentUser>("emptyUser")));
            IServiceProvider serviceProvider = new UnityServiceProvider(container);
            IBusinessServerProvider businessServerProvider = new BusinessServerProvider(serviceProvider);

            // Act.
            BusinessServer[] listOfBusinessServers = businessServerProvider.GetBusinessServer(typeof(FlexberryUserSettingForTest), ObjectStatus.Created, GetDataService());

            // Assert.
            Assert.Single(listOfBusinessServers);
            Assert.Equal(typeof(FlexberryUserSettingForTestBS), listOfBusinessServers[0].GetType());
            Assert.Equal(typeof(TestCurrentUser2), ((FlexberryUserSettingForTestBS)listOfBusinessServers[0]).currentUser.GetType());
        }

        /// <summary>
        /// Test with registration of <see cref="BusinessServerProvider"/> as factory at Unity.
        /// </summary>
        [Fact]
        public void TestResolvingWithUnityWithFullResolving()
        {
            // Arrange.
            var container = new UnityContainer();
            IServiceProvider serviceProvider = new UnityServiceProvider(container);
            container.RegisterType<ICurrentUser, TestCurrentUser>();
            container.RegisterFactory<IBusinessServerProvider>(new Func<IUnityContainer, object>(o => new BusinessServerProvider(new UnityServiceProvider(o))), FactoryLifetime.Singleton);
            IBusinessServerProvider businessServerProvider = (IBusinessServerProvider)serviceProvider.GetService(typeof(IBusinessServerProvider));

            // Act.
            BusinessServer[] listOfBusinessServers = businessServerProvider.GetBusinessServer(typeof(FlexberryUserSettingForTest), ObjectStatus.Created, GetDataService());

            // Assert.
            Assert.Single(listOfBusinessServers);
            Assert.Equal(typeof(FlexberryUserSettingForTestBS), listOfBusinessServers[0].GetType());
            Assert.Equal(typeof(TestCurrentUser), ((FlexberryUserSettingForTestBS)listOfBusinessServers[0]).currentUser.GetType());
        }

        /// <summary>
        /// Test with registration of CurrentUser as property of <see cref="SQLDataService"/> at Unity.
        /// </summary>
        [Fact]
        public void TestRegistrationOfCurrentUserProperty()
        {
            // Arrange.
            var container = new UnityContainer();
            IServiceProvider serviceProvider = new UnityServiceProvider(container);
            container.RegisterType<ICurrentUser, TestCurrentUser>();
            container.RegisterFactory<IBusinessServerProvider>(new Func<IUnityContainer, object>(o => new BusinessServerProvider(new UnityServiceProvider(o))), FactoryLifetime.Singleton);
            container.RegisterType<IDataService, MSSQLDataService>(new InjectionProperty(nameof(SQLDataService.CurrentUser), container.Resolve<ICurrentUser>()));
            container.RegisterType<ISecurityManager, EmptySecurityManager>();
            container.RegisterType<IAuditService, TestAuditService>();
            IBusinessServerProvider businessServerProvider = (IBusinessServerProvider)serviceProvider.GetService(typeof(IBusinessServerProvider));

            // Act.
            IDataService ds = container.Resolve<IDataService>();

            // Assert.
            Assert.Equal(typeof(MSSQLDataService), ds.GetType());
            Assert.NotNull(((SQLDataService)ds).CurrentUser);
        }

        /// <summary>
        /// Getting of mocked dataservice (it is not used on tests).
        /// </summary>
        /// <returns></returns>
        private MSSQLDataService GetDataService()
        {
            Mock<ISecurityManager> mockSecurityManager = new Mock<ISecurityManager>();
            Mock<IAuditService> mockAuditService = new Mock<IAuditService>();
            Mock<IBusinessServerProvider> mockBusinessServerProvider = new Mock<IBusinessServerProvider>();
            return new MSSQLDataService(mockSecurityManager.Object, mockAuditService.Object, mockBusinessServerProvider.Object);
        }

        /// <summary>
        /// Test class for resolving interface <see cref="IAuditService"/>.
        /// </summary>
        private class TestAuditService : IAuditService
        {
            public bool IsAuditEnabled => throw new NotImplementedException();

            public bool IsAuditRemote => throw new NotImplementedException();

            public string AuditConnectionStringName => throw new NotImplementedException();

            public bool ShowPrimaryKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool PersistUtcDates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public AuditAppSetting AppSetting { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IAudit Audit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void AddCreateAuditInformation(DataObject operationedObject)
            {
                throw new NotImplementedException();
            }

            public void AddEditAuditInformation(DataObject operationedObject)
            {
                throw new NotImplementedException();
            }

            public void DisableAudit()
            {
                throw new NotImplementedException();
            }

            public bool EnableAudit(bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public View GetAuditViewByType(Type type, tTypeOfAuditOperation operationType)
            {
                throw new NotImplementedException();
            }

            public View GetViewByAuditRecord(IAuditRecord auditRecord)
            {
                throw new NotImplementedException();
            }

            public bool IsTypeAuditable(Type curType)
            {
                throw new NotImplementedException();
            }

            public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public bool RatifyAuditOperation(tExecutionVariant executionVariant, List<Guid> auditOperationIdList, IDataService dataService, bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public bool RatifyAuditOperationWithAutoFields(tExecutionVariant executionVariant, List<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public Guid? WriteCommonAuditOperation(DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
            {
                throw new NotImplementedException();
            }

            public void WriteCommonAuditOperationWithAutoFields(IEnumerable<DataObject> operationedObjects, ICollection<AuditAdditionalInfo> auditOperationInfoList, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
            {
                throw new NotImplementedException();
            }

            public AuditAdditionalInfo WriteCommonAuditOperationWithAutoFields(DataObject operationedObject, IDataService dataService, bool throwExceptions = true, IDbTransaction transaction = null)
            {
                throw new NotImplementedException();
            }

            public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, IDataService dataService, bool throwExceptions)
            {
                throw new NotImplementedException();
            }

            public Guid? WriteCustomAuditOperation(CustomAuditParameters customAuditParameters, string dataServiceConnectionString, Type dataServiceType, bool throwExceptions)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Test class for resolving interface <see cref="ICurrentUser"/>.
        /// </summary>
        private class TestCurrentUser : ICurrentUser
        {
            public string Login { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public string Domain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public string FriendlyName { get => "Some User"; set => throw new NotImplementedException(); }
        }

        /// <summary>
        /// Another test class for resolving interface <see cref="ICurrentUser"/>.
        /// </summary>
        private class TestCurrentUser2 : TestCurrentUser
        { }

        /// <summary>
        /// Test dataobject class with business server.
        /// </summary>
        [BusinessServer(typeof(FlexberryUserSettingForTestBS), DataServiceObjectEvents.OnAllEvents)]
        private class FlexberryUserSettingForTest : DataObject
        {
        }

        /// <summary>
        /// Business server that should be resolved just by unity.
        /// </summary>
        private class FlexberryUserSettingForTestBS : BusinessServer
        {
            public ICurrentUser currentUser;

            public FlexberryUserSettingForTestBS(ICurrentUser currentUser)
            {
                this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            }

            public DataObject[] OnUpdateFlexberryUserSetting(FlexberryUserSettingForTest updatedObject)
            {
                return new DataObject[0];
            }
        }
    }
}