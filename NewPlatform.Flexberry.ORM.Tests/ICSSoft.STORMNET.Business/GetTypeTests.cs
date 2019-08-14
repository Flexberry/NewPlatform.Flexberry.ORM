namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using Xunit;

    public class GetTypeTests
    {
        [Fact]
        public void GetIDataServiceTypeTest()
        {
            string typeNameForIDataService = "ICSSoft.STORMNET.Business.IDataService, ICSSoft.STORMNET.Business, Version=1.0.0.1, Culture=neutral, PublicKeyToken=c17bb360f7843f45";
            Type iDataServiceType = Type.GetType(typeNameForIDataService);
            Assert.NotNull(iDataServiceType);
        }
    }
}
