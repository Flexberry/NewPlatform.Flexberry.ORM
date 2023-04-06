namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET.Business.Interfaces;

    /// <summary>
    /// Base test class for comfortable getting of <see cref="BusinessServerProvider"/>.
    /// </summary>
    public abstract class BaseBusinessServerTests
    {
        public static IBusinessServerProvider BSProvider
        {
            set
            {
                if (businessServerProvider != null)
                {
                    throw new Exception("BusinessServerProvider should not be initialized twice.");
                }

                businessServerProvider = value;
            }
        }

        protected static IBusinessServerProvider businessServerProvider;
    }
}
