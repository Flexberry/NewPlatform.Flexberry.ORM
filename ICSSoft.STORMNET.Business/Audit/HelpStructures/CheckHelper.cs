namespace ICSSoft.STORMNET.Business.Audit.HelpStructures
{
    /// <summary>
    /// Класс, содержащий обшие полезные методы проверки данных.
    /// </summary>
    public static class CheckHelper
    {
        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// Analogue of <c>string.isNullOrWhiteSpace</c> from .NET Framework >= 4.
        /// </summary>
        /// <param name="checkingString">The string to test.</param>
        /// <returns><c>true</c> if the value parameter is <c>null</c> or <c>String.Empty</c>, or if value consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(string checkingString)
        {
            return !string.IsNullOrEmpty(checkingString) && checkingString.Trim() != string.Empty;
        }
    }
}
