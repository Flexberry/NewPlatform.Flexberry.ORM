namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Расширения LINQ.
    /// </summary>
    public static class FlexLinqExtensions
    {
        /// <summary>
        /// Реализация функции Like.
        /// </summary>
        /// <param name="source">Исходная строка.</param>
        /// <param name="pattern">Паттерн для поиска.</param>
        /// <returns>Результат: подходит или нет.</returns>
        [Obsolete("Реализуется поддержка Regex в LinqProvider.")]
        public static bool IsLike(this string source, string pattern)
        {
            pattern = Regex.Escape(pattern);
            pattern = pattern.Replace("%", ".*?").Replace("_", ".");
            pattern = pattern.Replace(@"\[", "[").Replace(@"\]", "]").Replace(@"\^", "^");

            return Regex.IsMatch(source, pattern);
        }

        /// <summary>
        /// Диапазон значений.
        /// </summary>
        /// <typeparam name="T">Тип, для которого строим выражение.</typeparam>
        /// <param name="source">Что сравниваем.</param>
        /// <param name="left">Левый операнд.</param>
        /// <param name="right">Правый операнд.</param>
        /// <returns>Результат попадания в диапазон.</returns>
        public static bool Between<T>(this T source, T left, T right)
            where T : IComparable
        {
            return source.CompareTo(left) >= 0 && source.CompareTo(right) <= 0;
        }
    }
}
