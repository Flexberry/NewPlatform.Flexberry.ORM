namespace IIS.University.Tools
{
    using System;
    using System.Reflection.Emit;

    /// <summary>
    /// Static generic fast allocator.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the allocation method.</typeparam>
    public static class FastAllocator<T> where T : new()
    {
        public static Func<T> Creator;

        static FastAllocator()
        {
            var objectType = typeof(T);
            var defaultCtor = objectType.GetConstructor(new Type[0]);

            var dynMethod = new DynamicMethod(
                $"{nameof(FastAllocator<T>)}_{objectType.FullName}_{Guid.NewGuid()}",
                objectType,
                null);

            var il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, defaultCtor);
            il.Emit(OpCodes.Ret);

            Creator = dynMethod.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }

        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter, using the parameterless constructor.
        /// </summary>
        /// <returns>A reference to the newly created object.</returns>
        public static T New()
        {
            return Creator();
        }
    }
}
