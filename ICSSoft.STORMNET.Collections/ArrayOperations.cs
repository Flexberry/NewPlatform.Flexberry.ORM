namespace ICSSoft.STORMNET.Collections
{
    using System;

    /// <summary>
    /// Summary description for ArrayOperations.
    /// </summary>
    public class ArrayOperations
    {
        private ArrayOperations()
        {
        }

        /// <summary>
        /// соединить массивы.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static Array ConcatArrays(Type elementType, params Array[] arrays)
        {
            int Length = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                Length += arrays[i].Length;
            }

            Array res = Array.CreateInstance(elementType, Length);
            Length = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                arrays[i].CopyTo(res, Length);
                Length += arrays[i].Length;
            }

            return res;
        }
    }
}
