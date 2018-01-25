namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Класс для сравнения объектов типа <see cref="TypeKeyPair"/>.
    /// </summary>
    public class TypeKeyPairEqualityComparer : EqualityComparer<TypeKeyPair>
    {
        /// <summary>
        /// Determines whether two objects of type <paramref name="x"/> are equal.
        /// </summary>
        /// <returns>
        /// True if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public override bool Equals(TypeKeyPair x, TypeKeyPair y)
        {
            bool typeEquals = x.Type.Equals(y.Type);
            bool keyEquals = x.Key.Equals(y.Key);
            return typeEquals && keyEquals;
        }

        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The object for which to get a hash code.</param>
        public override int GetHashCode(TypeKeyPair obj)
        {
            unchecked
            {
                return ((obj.Type != null ? obj.Type.GetHashCode() : 0) * 397) ^ (obj.Key != null ? obj.Key.GetHashCode() : 0);
            }
        }
    }
}
