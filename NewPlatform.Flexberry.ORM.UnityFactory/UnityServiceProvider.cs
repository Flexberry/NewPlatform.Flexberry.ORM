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

        /// <summary>
        /// Init new instance of class <see cref="UnityServiceProvider"/>.
        /// </summary>
        /// <param name="container">Cantainer of type <see cref="IUnityContainer"/> for connection to <see cref="IServiceProvider"/>.</param>
        public UnityServiceProvider(IUnityContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (Unity.ResolutionFailedException)
            {
                return null;
            }
        }
    }
}
