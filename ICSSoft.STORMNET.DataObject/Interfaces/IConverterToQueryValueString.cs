namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    /// The interface allows you to define a class to convert values of different types to a string for use in a SQL query.
    /// </summary>
    public interface IConverterToQueryValueString
    {
        /// <summary>
        /// Checks if the type is supported.
        /// </summary>
        /// <param name="type">Type to be checked.</param>
        /// <returns><code>true</code> if the type is supported, otherwise. <code>false</code>.</returns>
        bool IsSupported(Type type);

        /// <summary>
        /// Converts a value to a string for use in an SQL query.
        /// </summary>
        /// <param name="value">The convertible value.</param>
        /// <returns>A string suitable for use in a SQL query.</returns>
        string ConvertToQueryValueString(object value);
    }
}
