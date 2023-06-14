namespace ICSSoft.Services
{
    using System;

    using Unity;

    /// <summary>
    /// Расширения для внедрения зависимостей.
    /// </summary>
    public static class DIExtensions
    {
        /// <summary>
        /// Получить именованную зависимость с помощью IUnityContainer (IUnityContainer должен быть зарегистрирован в IServiceProvider).
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="serviceProvider">IServiceProvider.</param>
        /// <param name="name">Имя зависимости.</param>
        /// <returns>Разрешенная зависимость.</returns>
        /// <exception cref="InvalidOperationException">Ошибка получения именованной зависимости.</exception>
        public static object GetService<T>(this IServiceProvider serviceProvider, string name)
        {
            var unityContainer = serviceProvider.GetService(typeof(IUnityContainer));

            if (unityContainer == null)
            {
                throw new InvalidOperationException($"Невозможно получить именнованную зависимость - IUnityContainer не зарегистрирован в этом ServiceProvider.");
            }
            else
            {
                return (unityContainer as IUnityContainer).Resolve<T>(name);
            }
        }
    }
}
