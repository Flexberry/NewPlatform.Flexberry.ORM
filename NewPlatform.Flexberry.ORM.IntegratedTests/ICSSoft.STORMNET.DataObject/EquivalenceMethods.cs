namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Методы проверки эквивалентность различных структур данных.
    /// </summary>
    public class EquivalenceMethods
    {
        /// <summary>
        /// Сравнение списков.
        /// </summary>
        /// <param name="target">
        /// Target - первый список.
        /// </param>
        /// <param name="source">
        /// Source - второй список.
        /// </param>
        /// <returns>
        /// True, если списки одинаковые, False -в противном случае.
        /// </returns>
        public static bool ListEquals(List<object> target, List<object> source)
        {
            return ListEquals<object>(target, source);
        }

        /// <summary>
        /// Сравнение списков.
        /// </summary>
        /// <param name="target">
        /// Target - первый список.
        /// </param>
        /// <param name="source">
        /// Source - второй список.
        /// </param>
        /// <returns>
        /// True, если списки одинаковые, False -в противном случае.
        /// </returns>
        public static bool ListEquals<T>(IEnumerable<T> target, IEnumerable<T> source)
        // public static bool ListEquals<T>(List<T> target, List<T> source)
        {
            if (target == null || source == null)
            {
                return target == null && source == null;
            }

            var list = target.ToList();
            var list1 = source.ToList();
            return list.Count == list1.Count && source.All(target.Contains);
        }

        /// <summary>
        /// Сравнение массивов типов.
        /// </summary>
        /// <param name="a1">
        /// Первый массив.
        /// </param>
        /// <param name="a2">
        /// Второй массив.
        /// </param>
        /// <returns>
        /// True, если массивы эквивалентны, False -в противном случае.
        /// </returns>
        public static bool EqualTypeArrays(Type[] a1, Type[] a2)
        {
            var p = true;
            if (a1.Length != a2.Length)
            {
                p = false;
            }
            else
            {
                if (a1.Where((t, i) => t != a2[i]).Any())
                {
                    p = false;
                }
            }

            return p;
        }

        /// <summary>
        /// Сравнение массивов строк.
        /// </summary>
        /// <param name="a1">
        /// Первый массив.
        /// </param>
        /// <param name="a2">
        /// Второй массив.
        /// </param>
        /// <returns>
        /// True, если массивы эквивалентны, False -в противном случае.
        /// </returns>
        public static bool EqualStringArrays(IEnumerable<string> a1, IEnumerable<string> a2)
        {
            if (a1 == null || a2 == null)
            {
                return a1 == null && a2 == null;
            }

            return ListEquals(a1, a2);
        }
    }
}
