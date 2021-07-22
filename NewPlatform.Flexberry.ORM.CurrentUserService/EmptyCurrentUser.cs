namespace NewPlatform.Flexberry.ORM.CurrentUserService
{
    /// <summary>
    /// Заглушка пустого пользователя.
    /// </summary>
    public class EmptyCurrentUser : ICurrentUser
    {
        /// <inheritdoc />
        public string? Login { get; set; }

        /// <inheritdoc />
        public string? Domain { get; set; }

        /// <inheritdoc />
        public string? FriendlyName { get; set; }
    }
}
