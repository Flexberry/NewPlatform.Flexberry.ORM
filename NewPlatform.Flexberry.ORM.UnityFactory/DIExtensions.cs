namespace ICSSoft.Services
{
    using System;

    using Unity;

    /// <summary>
    /// Extensions for resolving dependencies.
    /// </summary>
    public static class DIExtensions
    {
        /// <summary>
        /// Get a named dependency using IUnityContainer (IUnityContainer should be already registered at IServiceProvider).
        /// </summary>
        /// <typeparam name="T">Type to resolve.</typeparam>
        /// <param name="serviceProvider">A service provider. IUnityContainer container should be already registered.</param>
        /// <param name="name">Name of the dependency.</param>
        /// <returns>Resolved dependency.</returns>
        /// <exception cref="InvalidOperationException">Ошибка получения именованной зависимости.</exception>
        public static T GetService<T>(this IServiceProvider serviceProvider, string name)
        {
            var unityContainer = serviceProvider.GetService(typeof(IUnityContainer));

            if (unityContainer == null)
            {
                throw new InvalidOperationException($"Unable to resolve - IUnityContainer not registered in this IServiceProvider.");
            }
            else
            {
                return (unityContainer as IUnityContainer).Resolve<T>(name);
            }
        }
    }
}
