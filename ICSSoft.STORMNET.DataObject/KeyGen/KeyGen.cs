namespace ICSSoft.STORMNET.KeyGen
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Статический класс генерации ключей, через который генерируются все первичные ключи
    /// Он использует для генерации ключа генераторы, наследуемые от BaseKeyGenerator.
    /// Этот конкретный генератор прописывается непосредственно объекту данных специальным атрибутом <see cref="KeyGeneratorAttribute"/>.
    /// </summary>
    public static class KeyGenerator
    {
        private static readonly ConcurrentDictionary<Type, BaseKeyGenerator> cacheGenerators = new ConcurrentDictionary<Type, BaseKeyGenerator>();

        private static readonly ConcurrentDictionary<Type, Type> cacheKeys = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Возвращает непосредственно генератор, производный от <see cref="BaseKeyGenerator"/>.
        /// Удобно, если требуется использовать у этого генератора методы,
        /// отличные от имеющихся в BaseKeyGenerator.
        /// </summary>
        /// <param name="dataobject">Объект данных.</param>
        public static BaseKeyGenerator Generator(DataObject dataobject)
        {
            if (dataobject == null)
            {
                throw new ArgumentNullException(nameof(dataobject));
            }

            return Generator(dataobject.GetType());
        }

        /// <summary>
        /// Возвращает непосредственно генератор, производный от <see cref="BaseKeyGenerator"/>.
        /// Удобно, если требуется использовать у этого генератора методы,
        /// отличные от имеющихся в BaseKeyGenerator.
        /// </summary>
        /// <param name="dataobjecttype">Тип объекта данных.</param>
        public static BaseKeyGenerator Generator(Type dataobjecttype)
        {
            BaseKeyGenerator genrtor = cacheGenerators.GetOrAdd(dataobjecttype, k => CreateKeyGenerator(dataobjecttype));
            return genrtor;
        }

        /// <summary>
        /// Сгенерировать ключ и установить его в объект данных.
        /// </summary>
        /// <param name="dataobject">Объект данных.</param>
        /// <param name="sds">Сервис данных.</param>
        public static object GenerateUnique(DataObject dataobject, object sds)
        {
            if (dataobject == null)
            {
                throw new ArgumentNullException(nameof(dataobject));
            }

            if (!dataobject.PrimaryKeyIsUnique)
            {
                dataobject.__PrimaryKey = GenerateUnique(dataobject.GetType(), sds);
                dataobject.PrimaryKeyIsUnique = true;
            }

            return dataobject.__PrimaryKey;
        }

        /// <summary>
        /// Сгенерировать ключ и установить его в объект данных.
        /// </summary>
        /// <param name="dataobject">Объект данных.</param>
        /// <param name="sds">Сервис данных.</param>
        public static object Generate(DataObject dataobject, object sds)
        {
            if (dataobject == null)
            {
                throw new ArgumentNullException(nameof(dataobject));
            }

            Type type = dataobject.GetType();
            dataobject.__PrimaryKey = Generate(type, sds);
            dataobject.PrimaryKeyIsUnique = Generator(type).Unique;
            return dataobject.__PrimaryKey;
        }

        /// <summary>
        /// Сгенерировать ключ.
        /// </summary>
        /// <param name="dataobjecttype">Тип объекта данных.</param>
        /// <param name="sds">Сервис данных.</param>
        public static object Generate(Type dataobjecttype, object sds)
        {
            BaseKeyGenerator keyGen = Generator(dataobjecttype);
            return keyGen.Generate(dataobjecttype, sds);
        }

        /// <summary>
        /// Сгенерировать ключ.
        /// </summary>
        /// <param name="dataobjecttype">Тип объекта данных.</param>
        /// <param name="sds">Сервис данных.</param>
        public static object GenerateUnique(Type dataobjecttype, object sds)
        {
            BaseKeyGenerator keyGen = Generator(dataobjecttype);
            return keyGen.GenerateUniqe(dataobjecttype, sds);
        }

        /// <summary>
        /// Возвращает тип ключа (например, для GUIDGenerator это typeof(Guid)).
        /// </summary>
        /// <param name="dataobject">Объект данных.</param>
        public static Type KeyType(DataObject dataobject)
        {
            if (dataobject == null)
            {
                throw new ArgumentNullException(nameof(dataobject));
            }

            return KeyType(dataobject.GetType());
        }

        /// <summary>
        /// Возвращает тип ключа (например, для GUIDGenerator это typeof(Guid)).
        /// </summary>
        /// <param name="dataobjecttype">Тип объекта данных.</param>
        public static Type KeyType(Type dataobjecttype)
        {
            if (dataobjecttype == null)
            {
                throw new ArgumentNullException(nameof(dataobjecttype));
            }

            Type keyType = cacheKeys.GetOrAdd(dataobjecttype, k => GetKeyType(dataobjecttype));
            return keyType;
        }

        private static BaseKeyGenerator CreateKeyGenerator(Type dataobjecttype)
        {
            Type keygentype = Information.GetKeyGeneratorType(dataobjecttype);
            var constructorInfo = keygentype.GetConstructor(new Type[] { });
            return (BaseKeyGenerator)constructorInfo.Invoke(new object[] { });
        }

        private static Type GetKeyType(Type dataobjecttype)
        {
            BaseKeyGenerator keyGen = Generator(dataobjecttype);
            return keyGen.KeyType;
        }
    }
}
