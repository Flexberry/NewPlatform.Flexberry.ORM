namespace ICSSoft.Services
{
    using System.Configuration;

    using Microsoft.Practices.Unity.Configuration;

    using Unity;

    /// <summary>
    /// Helper class for creating and using Unity containers.
    /// </summary>
    /// <remarks>
    /// You should avoid using this class when you can. Based on IoC / DI philosophy, you should create and configure
    /// you DI container at "composition root" of your application. Using singleton-based static approach is the best
    /// possible compromise for situations when you haven't got easy access to it (e.g. ASP.NET WebForms).
    /// </remarks>
    public static class UnityFactory
    {
        /// <summary>
        /// The singleton instance of Unity container for <see cref="GetContainer"/>.
        /// </summary>
        private static IUnityContainer _instance;

        /// <summary>
        /// The object for implementing double-check locking.
        /// </summary>
        private static readonly object _lock = new object();
        private static UnityConfigurationSection _section = null;

        /// <summary>
        /// Creates new instance of Unity container and configures it using configuration file (from 'unity' section).
        /// </summary>
        /// <remarks>
        /// <para>
        /// Using this method multiple times you'll get different instances of the container each time: <code>CreateContainer() != CreateContainer()</code>
        /// </para>
        /// <para>
        /// This behavior could be wrong when you want to use singleton (ContainerControlledLifetimeManager) because all lifetime managers work
        /// at separate containers (<a href="https://msdn.microsoft.com/en-us/library/ff660872(v=pandp.20).aspx">see documentation</a>) and created
        /// dependency will live as long as your container.
        /// </para>
        /// <para>
        /// For getting the same container use <see cref="GetContainer"/>.
        /// </para>
        /// </remarks>
        /// <returns>New instance of Unity container.</returns>
        public static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            if (_section == null)
            {
                _section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            }

            _section?.Configure(container);
            return container;
        }

        /// <summary>
        /// Gets the singleton instance of Unity container.
        /// </summary>
        /// <remarks>The first instance will be created using <see cref="CreateContainer"/>.</remarks>
        /// <returns>The singleton instance of Unity container.</returns>
        public static IUnityContainer GetContainer()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = CreateContainer();
                    }
                }
            }

            return _instance;
        }
    }
}
