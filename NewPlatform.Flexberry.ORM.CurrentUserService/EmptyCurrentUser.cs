namespace ICSSoft.Services
{
    public class EmptyCurrentUser : CurrentUserService.IUser
    {
        /// <inheritdoc />
        public string Login { get; set; }

        /// <inheritdoc />
        public string Domain { get; set; }

        /// <inheritdoc />
        public string FriendlyName { get; set; }
    }
}
