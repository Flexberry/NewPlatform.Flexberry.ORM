namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Класс для создания объектов. Скажи кого и я его создам.
    /// </summary>
    public class ObjectCreator
    {
        /// <summary>
        /// Кеш для хендлеров создания объектов указанного типа.
        /// </summary>
        internal static readonly Dictionary<int, InstantiateObjectHandler> CacheInstantiateObjectHandler = new Dictionary<int, InstantiateObjectHandler>();

        /// <summary>
        /// Создать объект заданного типа.
        /// </summary>
        /// <param name="tp">Тип создаваемого объекта.</param>
        /// <returns>Созданный объект.</returns>
        public object CreateObject(Type tp)
        {
            int key = tp.GetHashCode();
            InstantiateObjectHandler instantiateObjectHandler;

            if (!CacheInstantiateObjectHandler.TryGetValue(key, out instantiateObjectHandler))
            {
                lock (CacheInstantiateObjectHandler)
                {
                    if (!CacheInstantiateObjectHandler.ContainsKey(key))
                    {
                        instantiateObjectHandler = DynamicMethodCompiler.CreateInstantiateObjectHandler(tp);
                        CacheInstantiateObjectHandler.Add(key, instantiateObjectHandler);
                    }
                    else
                    {
                        CacheInstantiateObjectHandler.TryGetValue(key, out instantiateObjectHandler);
                    }
                }
            }

            return instantiateObjectHandler();
        }
    }
}