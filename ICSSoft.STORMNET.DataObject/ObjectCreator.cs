namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Класс для создания объектов. Скажи кого и я его создам.
    /// </summary>
    public class ObjectCreator
    {
        /// <summary>
        /// Кеш для хендлеров создания объектов указанного типа.
        /// </summary>
        internal static readonly ConcurrentDictionary<int, InstantiateObjectHandler> CacheInstantiateObjectHandler = new ConcurrentDictionary<int, InstantiateObjectHandler>();

        /// <summary>
        /// Создать объект заданного типа.
        /// </summary>
        /// <param name="tp">Тип создаваемого объекта.</param>
        /// <returns>Созданный объект.</returns>
        public object CreateObject(Type tp)
        {
            if (tp == null)
            {
                throw new ArgumentNullException(nameof(tp));
            }

            int key = tp.GetHashCode();
            InstantiateObjectHandler instantiateObjectHandler = CacheInstantiateObjectHandler.GetOrAdd(key, k => DynamicMethodCompiler.CreateInstantiateObjectHandler(tp));

            return instantiateObjectHandler();
        }
    }
}