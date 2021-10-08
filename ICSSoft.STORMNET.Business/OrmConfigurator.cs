namespace ICSSoft.STORMNET.Business
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Класс с настройками, используемыми в ORM.
    /// </summary>
    public static class OrmConfigurator
    {
        /// <summary>
        /// Список сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// </summary>
        private static IEnumerable<Assembly> assembliesForIReferencesNullDeleteSearch;

        /// <summary>
        /// Задание списка сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// </summary>
        /// <param name="searchAssemblies">Список сборок.</param>
        public static void SetAssembliesForIReferencesNullDeleteSearch(IEnumerable<Assembly> searchAssemblies)
        {
            assembliesForIReferencesNullDeleteSearch = searchAssemblies;
        }

        /// <summary>
        /// Получение списка сборок, где осуществляется поиск объектов, для которых переданный является мастером, при обработке интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete"/>.
        /// Если задано <c>null</c>, то поиск осуществляется только в сборке с удаляемым объектом.
        /// </summary>
        /// <returns>Список сборок.</returns>
        public static IEnumerable<Assembly> GetAssembliesForIReferencesNullDeleteSearch()
        {
            return assembliesForIReferencesNullDeleteSearch;
        }
    }
}
