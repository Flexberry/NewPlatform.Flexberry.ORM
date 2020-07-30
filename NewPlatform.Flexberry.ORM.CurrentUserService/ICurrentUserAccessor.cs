namespace NewPlatform.Flexberry.ORM.CurrentUserService
{
    /// <summary>
    /// Provides access to the current <see cref="ICurrentUser"/>, if one is available.
    /// </summary>
    public interface ICurrentUserAccessor
    {
        /// <summary>
        /// Gets or sets the current <see cref="ICurrentUser"/>. Returns <see langword="null" /> if there is no active <see cref="ICurrentUser" />.
        /// </summary>
        ICurrentUser? CurrentUser { get; set; }
    }
}
