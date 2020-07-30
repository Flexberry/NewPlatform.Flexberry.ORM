namespace NewPlatform.Flexberry.ORM.CurrentUserService
{
    using System;

    using Unity;

    /// <summary>
    /// Сервис для получения текущего пользователя через <see cref="IUnityContainer" />.
    /// </summary>
    public class UnityCurrentUserAccessor : ICurrentUserAccessor
    {
        /// <summary>
        /// Контейнер Unity Framework.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityCurrentUserAccessor" /> class.
        /// </summary>
        /// <param name="container">Unity контейнер.</param>
        public UnityCurrentUserAccessor(IUnityContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public ICurrentUser? CurrentUser
        {
            get { return GetRegisteredUser(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Получить пользователя, тип которого зарегистрирован в конфигурации unity.
        /// </summary>
        /// <returns>Данные о зарегистрированном пользователе, либо <c>null</c>, если его не удалось разрешить.</returns>
        private ICurrentUser? GetRegisteredUser()
        {
            // IsRegistered довольно долго выполняется к тому же он практически всегда возвращает true,
            // а резолв тут может упасть либо из-за корявого конфига юнити - тогда вообще ничего работать не будет
            // либо из-за того что ICurrentUser не прописан - то же что делает и IsRegistered.
            // return container.IsRegistered<ICurrentUser>() ? container.Resolve<ICurrentUser>() : null;
            try
            {
                return container.Resolve<ICurrentUser>();
            }
            catch
            {
                return null;
            }
        }
    }
}
