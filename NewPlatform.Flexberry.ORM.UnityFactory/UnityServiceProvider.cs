namespace ICSSoft.Services
{
    using System;

    using Unity;

    /// <summary>
    /// Help structure for connection of <see cref="IServiceProvider"/> and <see cref="IUnityContainer"/>.
    /// </summary>
    public class UnityServiceProvider : IServiceProvider
    {
        private readonly IUnityContainer container;

        public UnityServiceProvider(IUnityContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }
    }
}
