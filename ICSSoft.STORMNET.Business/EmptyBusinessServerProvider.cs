namespace ICSSoft.STORMNET.Business
{
    using System;

    using ICSSoft.STORMNET.Business.Interfaces;

    /// <summary>
    /// Провайдер-пустышка для тестов.
    /// </summary>
    public class EmptyBusinessServerProvider : IBusinessServerProvider
    {
        /// <inheritdoc />
        public BusinessServer[] GetBusinessServer(Type dataObjectType, ObjectStatus objectStatus, IDataService ds)
        {
            return new BusinessServer[0];
        }
    }
}
