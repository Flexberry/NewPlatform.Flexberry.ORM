namespace ICSSoft.Services
{
    using Unity;
    using Unity.Lifetime;

    /// <summary>
    /// Сервис для получения текущего пользователя.
    /// </summary>
    public static partial class CurrentUserService
    {
        /// <summary>
        /// Контейнер Unity Framework.
        /// </summary>
        private static IUnityContainer _container;

        /// <summary>
        /// Пользователь по умолчанию, используемый в случае, если тип пользователя
        /// не зарегистрирован в конфигурации unity, либо его не удалось разрешить.
        /// </summary>
        private static IUser _defaultUser;

        /// <summary>
        /// Инициализация сервиса.
        /// </summary>
        static CurrentUserService()
        {
            Reset();
        }

        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public static IUser CurrentUser
        {
            get { return GetRegisteredUser() ?? _defaultUser; }
        }

        /// <summary>
        /// Изменить способ получения текущего пользователя.
        /// </summary>
        /// <typeparam name="T">Класс, ответственный за работу с текущим пользователем.</typeparam>
        public static void ResolveUser<T>() where T : IUser
        {
            _container.RegisterType<IUser, T>(new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Инициализирует настройки сервиса по умолчанию.
        /// </summary>
        public static void Reset()
        {
            _container = UnityFactory.CreateContainer();
            _defaultUser = new CurrentUser();
        }

        /// <summary>
        /// Получить пользователя, тип которого зарегистрирован в конфигурации unity.
        /// </summary>
        /// <returns>Данные о зарегистрированном пользователе, либо <c>null</c>, если его не удалось разрешить.</returns>
        private static IUser GetRegisteredUser()
        {
            // IsRegistered довольно долго выполняется к тому же он практически всегда возвращает true,
            // а резолв тут может упасть либо из-за корявого конфига юнити - тогда вообще ничего работать не будет
            // либо из-за того что IUser не прописан - то же что делает и IsRegistered.
            // return _container.IsRegistered<IUser>() ? _container.Resolve<IUser>() : null;
            try
            {
                return _container.Resolve<IUser>();
            }
            catch
            {
                return null;
            }
        }
    }
}
