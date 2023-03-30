namespace ICSSoft.STORMNET
{
    /// <summary>
    /// The interface allows you to define the conversion of an object to a string for use in a SQL query.
    /// </summary>
    public interface IConvertibleToQueryValueString
    {
        /// <summary>
        /// Converts an object to a string for use in a SQL query.
        /// </summary>
        /// <returns>A string suitable for use in a SQL query.</returns>
        string ConvertToQueryValueString();
    }
}
